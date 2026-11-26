Imports System.IO
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace Payslips.Data

    ''' <summary>
    ''' Read-only na pinagkukunan ng payslip. Ang mga AMOUNT ay galing sa snapshot (tblPayrollResult / Line) kaya hindi
    ''' nagbabago sa reprint. Ang detalye ng empleyado (Position, Department, gov't IDs, email) ay LIVE.
    ''' </summary>
    Public Class PayslipRepository
        Inherits BaseRepository(Of PayrollResultModel)
        Implements IPayslipRepository

        ' =============================================
        ' SQL
        ' =============================================

        Private Const CutoffsSql As String =
            "SELECT c.*
             FROM tblCutoff c
             WHERE c.Status IN @statuses
               AND EXISTS (SELECT 1 FROM tblPayrollResult r WHERE r.CutoffID = c.CutoffID)
             ORDER BY ISNULL(c.PayDate, c.CutoffEnd) DESC, c.CutoffStart DESC, c.CycleType"

        Private Const EmployeeRowsSql As String =
            "SELECT CAST(0 AS BIT) AS Selected,
                    r.ResultId, r.CutoffID, r.RecordId, r.EmployeeNo, r.EmployeeName,
                    d.DepartmentName, p.PositionName, ee.PayBy,
                    LTRIM(RTRIM(e.EmailAddress)) AS EmailAddress, r.NetPay,
                    lg.SentDate AS LastEmailDate, lg.IsSuccess AS LastEmailSuccess, lg.ErrorMessage AS LastEmailError
             FROM tblPayrollResult r
                  LEFT JOIN tblEmployee e ON e.RecordId = r.RecordId
                  LEFT JOIN tblEmployeeEmployment em ON em.RecordId = r.RecordId
                  LEFT JOIN tblDepartment d ON d.DepartmentId = em.DepartmentId
                  LEFT JOIN tblPosition p ON p.PositionId = em.PositionId
                  LEFT JOIN tblEmployeeEarnings ee ON ee.RecordId = r.RecordId
                  OUTER APPLY (SELECT TOP 1 x.SentDate, x.IsSuccess, x.ErrorMessage
                               FROM tblPayslipEmailLog x
                               WHERE x.CutoffID = r.CutoffID AND x.RecordId = r.RecordId
                               ORDER BY x.SentDate DESC, x.EmailLogId DESC) lg
             WHERE r.CutoffID IN @cutoffIds
             ORDER BY r.EmployeeName, r.EmployeeNo, r.CutoffID"

        Private Const EmployeeInfoSql As String =
            "SELECT e.RecordId, p.PositionName, d.DepartmentName, b.BranchName,
                    st.SSSNo, st.PhilHealthNo, st.PagIBIGNo, st.TIN,
                    ee.PayBy, LTRIM(RTRIM(e.EmailAddress)) AS EmailAddress, e.BirthDate
             FROM tblEmployee e
                  LEFT JOIN tblEmployeeEmployment em ON em.RecordId = e.RecordId
                  LEFT JOIN tblPosition p ON p.PositionId = em.PositionId
                  LEFT JOIN tblDepartment d ON d.DepartmentId = em.DepartmentId
                  LEFT JOIN tblBranch b ON b.BranchId = em.BranchId
                  LEFT JOIN tblEmployeeStatutory st ON st.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeEarnings ee ON ee.RecordId = e.RecordId
             WHERE e.RecordId IN @ids"

        ' Pang-ilang hulog ang bawat LOAN line ng cutoff (para sa "(5/24)").
        Private Const LoanLinesSql As String =
            "SELECT l.LineId, s.EmployeeLoanId, s.InstallmentNo,
                    (SELECT COUNT(*) FROM tblEmployeeLoanSchedule x WHERE x.EmployeeLoanId = s.EmployeeLoanId) AS TotalInstallments
             FROM tblPayrollResultLine l
                  INNER JOIN tblEmployeeLoanSchedule s ON s.ScheduleId = l.RefId
             WHERE l.CutoffID = @cutoffId AND l.Category = @loan AND l.RecordId IN @ids"

        ' Balance as of pay date: lahat ng AmountDue bawas ang mga bayad na ang PaidDate ay hanggang pay date.
        ' Hindi kasama: Cancelled; Offset na na-offset na noong pay date; loan na nagsimula pagkatapos ng pay date.
        Private Const LoanBalanceSql As String =
            "SELECT l.EmployeeRecordId AS RecordId, l.EmployeeLoanId, l.LoanCode,
                    ISNULL(lt.Description, l.LoanCode) AS LoanName, l.Status,
                    COUNT(s.ScheduleId) AS TotalInstallments,
                    SUM(CASE WHEN s.IsPaid = 1 AND s.PaidDate < @payDateNext THEN 1 ELSE 0 END) AS PaidInstallments,
                    SUM(s.AmountDue) - SUM(CASE WHEN s.PaidDate < @payDateNext THEN s.AmountPaid ELSE 0 END) AS Balance
             FROM tblEmployeeLoan l
                  INNER JOIN tblEmployeeLoanSchedule s ON s.EmployeeLoanId = l.EmployeeLoanId
                  LEFT JOIN tblLoan lt ON lt.Code = l.LoanCode
             WHERE l.EmployeeRecordId IN @ids
               AND l.Status <> @cancelled
               AND l.LoanStartDate < @payDateNext
               AND NOT (l.Status = @offset AND (l.OffsetDate IS NULL OR l.OffsetDate < @payDateNext))
             GROUP BY l.EmployeeRecordId, l.EmployeeLoanId, l.LoanCode, lt.Description, l.Status"

        ' YTD: ang cutoff na ito + mga Posted cutoff ng parehong taon na ang pay date ay hanggang sa pay date nito.
        Private Const YtdSql As String =
            "SELECT r.RecordId,
                    SUM(r.GrossPay) AS Gross,
                    SUM(r.SSSEE + r.SSSMPFEE + r.PhilHealthEE + r.PagIbigEE) AS Contributions,
                    SUM(r.WithholdingTax) AS Tax,
                    SUM(ISNULL(al.Amount, 0)) AS Allowance
             FROM tblPayrollResult r
                  INNER JOIN tblCutoff c ON c.CutoffID = r.CutoffID
                  OUTER APPLY (SELECT SUM(x.Amount) AS Amount
                               FROM tblPayrollResultLine x
                                    INNER JOIN tblCompensation cp ON cp.Code = x.Code AND cp.IsPayslipAllowance = 1
                               WHERE x.ResultId = r.ResultId AND x.LineType = @earning AND x.Category = @compensation) al
             WHERE r.RecordId IN @ids
               AND (r.CutoffID = @cutoffId
                    OR (c.Status = @posted
                        AND ISNULL(c.PayDate, c.CutoffEnd) >= @yearStart
                        AND ISNULL(c.PayDate, c.CutoffEnd) < @payDateNext))
             GROUP BY r.RecordId"

        Private Const CompanySql As String =
            "SELECT TOP 1 CompanyName, Address1, Address2, Address3, PostCode, TelephoneNo, Website, LogoPath
             FROM tblCompany
             ORDER BY CASE WHEN IsActive = 1 THEN 0 ELSE 1 END, CompanyId"

        Private Const InsertEmailLogSql As String =
            "INSERT INTO tblPayslipEmailLog (CutoffID, RecordId, EmailAddress, IsSuccess, ErrorMessage, IncludeAllowance, SentBy)
             VALUES (@CutoffID, @RecordId, @EmailAddress, @IsSuccess, @ErrorMessage, @IncludeAllowance, @SentBy)"

        Private Class CompanyRow
            Public Property CompanyName As String
            Public Property Address1 As String
            Public Property Address2 As String
            Public Property Address3 As String
            Public Property PostCode As String
            Public Property TelephoneNo As String
            Public Property Website As String
            Public Property LogoPath As String
        End Class

        ' =============================================
        ' READ
        ' =============================================

        Public Async Function GetCutoffsAsync() As Task(Of List(Of CutoffModel)) _
            Implements IPayslipRepository.GetCutoffsAsync

            Dim statuses = New Integer() {CInt(CutoffStatus.Processed), CInt(CutoffStatus.Posted)}   ' Integer[] (hindi Byte[]: varbinary ang Byte[] sa Dapper)
            Using conn = GetConnection()
                Return (Await conn.QueryAsync(Of CutoffModel)(CutoffsSql, New With {statuses})).ToList()
            End Using
        End Function

        Public Async Function GetEmployeeRowsAsync(cutoffIds As IList(Of Integer)) As Task(Of List(Of PayslipEmployeeRow)) _
            Implements IPayslipRepository.GetEmployeeRowsAsync

            Dim ids = If(cutoffIds, New List(Of Integer)()).Distinct().ToList()
            If ids.Count = 0 Then Return New List(Of PayslipEmployeeRow)()
            Using conn = GetConnection()
                Return (Await conn.QueryAsync(Of PayslipEmployeeRow)(EmployeeRowsSql, New With {.cutoffIds = ids})).ToList()
            End Using
        End Function

        Public Async Function LoadSourceAsync(cutoffId As Integer, recordIds As IList(Of Integer)) As Task(Of PayslipSource) _
            Implements IPayslipRepository.LoadSourceAsync

            Dim src As New PayslipSource()
            Dim ids = If(recordIds, New List(Of Integer)()).Distinct().ToList()

            Using conn = GetConnection()
                src.Cutoff = Await conn.QuerySingleOrDefaultAsync(Of CutoffModel)(
                    "SELECT * FROM tblCutoff WHERE CutoffID = @cutoffId", New With {cutoffId})
                If src.Cutoff Is Nothing Then Throw New InvalidOperationException("Selected Cutoff no longer exists.")

                src.Company = Await LoadCompanyAsync(conn)

                src.AllowanceCodes = New HashSet(Of String)(
                    Await conn.QueryAsync(Of String)("SELECT Code FROM tblCompensation WHERE IsPayslipAllowance = 1"),
                    StringComparer.OrdinalIgnoreCase)

                If ids.Count = 0 Then Return src

                src.Headers = (Await conn.QueryAsync(Of PayrollResultModel)(
                    "SELECT * FROM tblPayrollResult WHERE CutoffID = @cutoffId AND RecordId IN @ids ORDER BY EmployeeName, EmployeeNo",
                    New With {cutoffId, ids})).ToList()

                src.Lines = (Await conn.QueryAsync(Of PayrollResultLineModel)(
                    "SELECT * FROM tblPayrollResultLine WHERE CutoffID = @cutoffId AND RecordId IN @ids ORDER BY ResultId, SeqNo",
                    New With {cutoffId, ids})).ToList()

                src.Employees = (Await conn.QueryAsync(Of PayslipEmployeeInfo)(EmployeeInfoSql, New With {ids})).ToList()

                src.LoanLines = (Await conn.QueryAsync(Of PayslipLoanLineInfo)(LoanLinesSql,
                    New With {cutoffId, .loan = ResultCategory.Loan, ids})).ToList()

                Dim payDate = If(src.Cutoff.PayDate.HasValue, src.Cutoff.PayDate.Value, src.Cutoff.CutoffEnd).Date
                Dim payDateNext = payDate.AddDays(1)

                src.LoanBalances = (Await conn.QueryAsync(Of PayslipLoanBalance)(LoanBalanceSql, New With {
                    ids, payDateNext, .cancelled = LoanStatus.Cancelled, .offset = LoanStatus.Offset})).ToList()

                src.Ytd = (Await conn.QueryAsync(Of PayslipYtd)(YtdSql, New With {
                    ids, cutoffId, payDateNext,
                    .yearStart = New Date(payDate.Year, 1, 1),
                    .posted = CByte(CutoffStatus.Posted),
                    .earning = ResultLineType.Earning,
                    .compensation = ResultCategory.Compensation})).ToList()
            End Using

            Return src
        End Function

        Private Shared Async Function LoadCompanyAsync(conn As System.Data.IDbConnection) As Task(Of PayslipCompanyInfo)
            Dim info As New PayslipCompanyInfo()
            Dim row = Await conn.QuerySingleOrDefaultAsync(Of CompanyRow)(CompanySql)
            If row Is Nothing Then
                info.CompanyName = "(Set up the company in Settings > Company Profile)"
                Return info
            End If

            info.CompanyName = row.CompanyName
            Dim parts = {row.Address1, row.Address2, row.Address3, row.PostCode}.
                Where(Function(s) Not String.IsNullOrWhiteSpace(s)).Select(Function(s) s.Trim())
            info.AddressText = String.Join(", ", parts)
            info.ContactText = If(String.IsNullOrWhiteSpace(row.TelephoneNo), "", "Tel. " & row.TelephoneNo.Trim())

            If Not String.IsNullOrWhiteSpace(row.LogoPath) Then
                Dim root = Await conn.QuerySingleOrDefaultAsync(Of String)(
                    "SELECT TOP 1 FileStorageRoot FROM tblGeneralSettings ORDER BY GeneralSettingsId")
                info.LogoFullPath = ResolveLogo(root, row.LogoPath)
            End If

            Return info
        End Function

        ' Parehong pagkakasunod ng Company Profile: shared folder muna, saka ang lokal na Uploads\.
        Private Shared Function ResolveLogo(storageRoot As String, relativePath As String) As String
            Try
                If Not String.IsNullOrWhiteSpace(storageRoot) Then
                    Dim sharedFull = SharedFileStorage.GetFullPath(storageRoot, relativePath)
                    If File.Exists(sharedFull) Then Return sharedFull
                End If
                Dim localFull = FileStorageHelper.GetFullPath(relativePath)
                If File.Exists(localFull) Then Return localFull
            Catch ex As Exception
                Debug.WriteLine($"Payslip logo not readable: {ex.Message}")
            End Try
            Return String.Empty
        End Function

        ' =============================================
        ' EMAIL LOG
        ' =============================================

        Public Async Function InsertEmailLogAsync(entry As PayslipEmailLogEntry) As Task _
            Implements IPayslipRepository.InsertEmailLogAsync

            If entry Is Nothing Then Return
            If entry.ErrorMessage IsNot Nothing AndAlso entry.ErrorMessage.Length > 1000 Then
                entry.ErrorMessage = entry.ErrorMessage.Substring(0, 997) & "..."
            End If

            Using conn = GetConnection()
                Await conn.ExecuteAsync(InsertEmailLogSql, entry)
            End Using
        End Function

    End Class

End Namespace
