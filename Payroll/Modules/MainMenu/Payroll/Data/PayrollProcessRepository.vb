Imports System.Data
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Data

    Public Class PayrollProcessRepository
        Inherits BaseRepository(Of PayrollResultModel)
        Implements IPayrollProcessRepository

        ' =============================================
        ' SQL
        ' =============================================

        Private Const EmployeeSql As String =
            "SELECT e.RecordId, e.EmployeeNo,
                    (e.LastName + ', ' + e.FirstName) AS EmployeeName,
                    ee.PayCycle, ee.BasicSalary, ee.DailyRate, ee.HourlyRate, ee.DaysInYear, ee.WorkHourPer,
                    CAST(ISNULL(ee.MinimumWage, 0) AS BIT) AS MinimumWage,
                    es.PagIBIGVoluntary, es.FixTax,
                    em.DateJoined, em.DateResigned, em.DateRetired, em.DateTerminated
             FROM tblEmployee e
                  INNER JOIN tblEmployeeEarnings ee ON ee.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeStatutory es ON es.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeEmployment em ON em.RecordId = e.RecordId
             WHERE e.RecordId IN @ids"

        Private Const TxnSql As String =
            "SELECT RecordId, TxnCategory, TxnCode, Qty, Rate
             FROM tblPayrollInputTxn
             WHERE CutoffID = @cutoffId AND RecordId IN @ids"

        ' Lahat ng hindi pa bayad na hulog hanggang sa PayDate; ang mga lampas na sa window ay minarkahang IsOverdue.
        Private Const LoanSql As String =
            "SELECT l.EmployeeRecordId AS RecordId, s.ScheduleId, l.EmployeeLoanId, l.LoanCode,
                    ISNULL(lt.Description, l.LoanCode) AS LoanDescription,
                    s.InstallmentNo, s.ScheduledDate,
                    (s.AmountDue - s.AmountPaid) AS BalanceDue,
                    CAST(CASE WHEN s.ScheduledDate <= @lowerBound THEN 1 ELSE 0 END AS BIT) AS IsOverdue
             FROM tblEmployeeLoanSchedule s
                  INNER JOIN tblEmployeeLoan l ON l.EmployeeLoanId = s.EmployeeLoanId
                  LEFT JOIN tblLoan lt ON lt.Code = l.LoanCode
             WHERE l.Status = @active
               AND s.IsPaid = 0
               AND s.ScheduledDate <= @payDate
               AND l.EmployeeRecordId IN @ids"

        Private Const PriorBaseSql As String =
            "SELECT RecordId,
                    SUM(SSSBase) AS SSSBase, SUM(PhilHealthBase) AS PhilHealthBase, SUM(PagIbigBase) AS PagIbigBase
             FROM tblPayrollResult
             WHERE CutoffID IN @priorIds AND RecordId IN @ids
             GROUP BY RecordId"

        Private Const InsertHeaderSql As String =
            "INSERT INTO tblPayrollResult
                (CutoffID, RecordId, EmployeeNo, EmployeeName, PayCycle, RateBasis, PeriodInMonth, DaysWorked,
                 BasicSalary, DailyRate, HourlyRate, DaysInYear, WorkHourPer, DailyRateMonthlyFactor, IsMinimumWage,
                 BasicPay, GrossPay,
                 TaxGross, SSSBase, PhilHealthBase, PagIbigBase, SSSLookupBase, PhilHealthLookupBase, PagIbigLookupBase,
                 SSSEE, SSSMPFEE, PhilHealthEE, PagIbigEE, SSSER, SSSMPFER, ECC, PhilHealthER, PagIbigER,
                 TaxableIncome, WithholdingTax, LoanDeduction, OtherDeduction, TotalDeduction, NetPay,
                 HasWarnings, WarningText, ProcessedBy, ProcessedDate)
             OUTPUT INSERTED.ResultId
             VALUES
                (@CutoffID, @RecordId, @EmployeeNo, @EmployeeName, @PayCycle, @RateBasis, @PeriodInMonth, @DaysWorked,
                 @BasicSalary, @DailyRate, @HourlyRate, @DaysInYear, @WorkHourPer, @DailyRateMonthlyFactor, @IsMinimumWage,
                 @BasicPay, @GrossPay,
                 @TaxGross, @SSSBase, @PhilHealthBase, @PagIbigBase, @SSSLookupBase, @PhilHealthLookupBase, @PagIbigLookupBase,
                 @SSSEE, @SSSMPFEE, @PhilHealthEE, @PagIbigEE, @SSSER, @SSSMPFER, @ECC, @PhilHealthER, @PagIbigER,
                 @TaxableIncome, @WithholdingTax, @LoanDeduction, @OtherDeduction, @TotalDeduction, @NetPay,
                 @HasWarnings, @WarningText, @ProcessedBy, SYSDATETIME())"

        Private Const InsertLineSql As String =
            "INSERT INTO tblPayrollResultLine
                (ResultId, CutoffID, RecordId, SeqNo, LineType, Category, Code, Description, Qty, Unit, RateUsed, Amount,
                 TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag, RefId, Remark)
             VALUES
                (@ResultId, @CutoffID, @RecordId, @SeqNo, @LineType, @Category, @Code, @Description, @Qty, @Unit, @RateUsed, @Amount,
                 @TaxFlag, @SSSFlag, @PhilHealthFlag, @PagIbigFlag, @RefId, @Remark)"

        Private Const InsertBracketSql As String =
            "INSERT INTO tblPayrollResultBracket
                (ResultId, Agency, SourceTable, SourceId, BasisAmount, SalaryFrom, SalaryTo,
                 EEShare, EEContriType, ERShare, ERContriType, ECCAmount, EEMPF, ERMPF,
                 TaxPercentage, FixTaxAmount, MonthlyEE, MonthlyER, SharePercent)
             VALUES
                (@ResultId, @Agency, @SourceTable, @SourceId, @BasisAmount, @SalaryFrom, @SalaryTo,
                 @EEShare, @EEContriType, @ERShare, @ERContriType, @ECCAmount, @EEMPF, @ERMPF,
                 @TaxPercentage, @FixTaxAmount, @MonthlyEE, @MonthlyER, @SharePercent)"

        ' =============================================
        ' LOAD
        ' =============================================

        Public Async Function LoadEmployeesAsync(cutoff As CutoffModel, basis As PayRateBasis,
                                                 priorCutoffIds As IList(Of Integer), loanLowerBound As Date) As Task(Of PayrollEmployeeLoad) _
            Implements IPayrollProcessRepository.LoadEmployeesAsync

            Dim result As New PayrollEmployeeLoad()

            Using conn = GetConnection()
                ' Iisang patakaran sa eligibility para sa Input Entry at sa Process (PayrollEligibilityRules)
                Dim candidates = (Await conn.QueryAsync(Of EligibilityCandidate)(PayrollInputRepository.EligibilitySql)).ToList()
                Dim eligibility = PayrollEligibilityRules.Build(candidates, cutoff.CycleType, cutoff.CutoffStart, cutoff.CutoffEnd, basis)
                result.Excluded = eligibility.Warnings

                Dim ids = eligibility.Eligible.Select(Function(e) e.RecordId).ToList()
                If ids.Count = 0 Then Return result

                result.Employees = (Await conn.QueryAsync(Of PayrollEmployeeInput)(EmployeeSql, New With {ids})).ToList()

                Dim txns = (Await conn.QueryAsync(Of PayrollTxnInput)(TxnSql, New With {.cutoffId = cutoff.CutoffID, ids})).ToLookup(Function(t) t.RecordId)

                Dim payDate = If(cutoff.PayDate.HasValue, cutoff.PayDate.Value, cutoff.CutoffEnd).Date
                Dim loans = (Await conn.QueryAsync(Of PayrollLoanDue)(LoanSql, New With {
                    .lowerBound = loanLowerBound.Date, .active = LoanStatus.Active, payDate, ids})).ToLookup(Function(l) l.RecordId)

                Dim priors As New Dictionary(Of Integer, PayrollPriorBases)()
                If priorCutoffIds IsNot Nothing AndAlso priorCutoffIds.Count > 0 Then
                    Dim priorRows = Await conn.QueryAsync(Of PayrollPriorBases)(PriorBaseSql, New With {.priorIds = priorCutoffIds, ids})
                    For Each p In priorRows
                        priors(p.RecordId) = p
                    Next
                End If

                For Each emp In result.Employees
                    emp.Txns = txns(emp.RecordId).ToList()
                    emp.Loans = loans(emp.RecordId).OrderBy(Function(l) l.ScheduledDate).ThenBy(Function(l) l.InstallmentNo).ToList()
                    Dim prior As PayrollPriorBases = Nothing
                    If priors.TryGetValue(emp.RecordId, prior) Then emp.Prior = prior
                Next
            End Using

            Return result
        End Function

        ' =============================================
        ' SAVE (isang transaction)
        ' =============================================

        Public Async Function SaveResultsAsync(cutoffId As Integer, results As List(Of PayrollEmployeeResult), userName As String) As Task _
            Implements IPayrollProcessRepository.SaveResultsAsync

            Using conn = GetConnection()
                Using tx = conn.BeginTransaction()

                    ' I-lock ang cutoff row habang nagse-save, at siguraduhing hindi pa Posted/Closed.
                    Dim status = Await conn.ExecuteScalarAsync(Of Byte?)(
                        "SELECT Status FROM tblCutoff WITH (UPDLOCK, ROWLOCK) WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

                    If Not status.HasValue Then
                        Throw New InvalidOperationException("Selected Cutoff no longer exists.")
                    End If
                    If status.Value = CByte(CutoffStatus.Posted) Then
                        Throw New InvalidOperationException("This Cutoff is already Posted and can no longer be re-processed.")
                    End If
                    If status.Value = CByte(CutoffStatus.Closed) Then
                        Throw New InvalidOperationException("This Cutoff is Closed. Set it back to Draft in Payroll Settings > Cut-Off first.")
                    End If

                    ' Re-process: burahin muna ang lahat ng lumang results ng cutoff (cascade sa lines at brackets).
                    Await conn.ExecuteAsync("DELETE FROM tblPayrollResult WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

                    For Each r In results
                        r.Header.CutoffID = cutoffId
                        r.Header.ProcessedBy = userName

                        Dim resultId = Await conn.ExecuteScalarAsync(Of Integer)(InsertHeaderSql, r.Header, tx)
                        r.Header.ResultId = resultId

                        For Each ln In r.Lines
                            ln.ResultId = resultId
                            ln.CutoffID = cutoffId
                            ln.RecordId = r.Header.RecordId
                        Next
                        If r.Lines.Count > 0 Then Await conn.ExecuteAsync(InsertLineSql, r.Lines, tx)

                        For Each b In r.Brackets
                            b.ResultId = resultId
                        Next
                        If r.Brackets.Count > 0 Then Await conn.ExecuteAsync(InsertBracketSql, r.Brackets, tx)
                    Next

                    Await conn.ExecuteAsync(
                        "UPDATE tblCutoff
                         SET Status = @processed, LastProcessedBy = @userName, LastProcessedDate = SYSDATETIME()
                         WHERE CutoffID = @cutoffId",
                        New With {.processed = CByte(CutoffStatus.Processed), userName, cutoffId}, tx)

                    tx.Commit()
                End Using
            End Using
        End Function

        ' =============================================
        ' OUTPUT TAB
        ' =============================================

        Public Async Function GetCutoffsWithResultsAsync() As Task(Of List(Of CutoffModel)) _
            Implements IPayrollProcessRepository.GetCutoffsWithResultsAsync

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of CutoffModel)(
                    "SELECT c.*
                     FROM tblCutoff c
                     WHERE EXISTS (SELECT 1 FROM tblPayrollResult r WHERE r.CutoffID = c.CutoffID)
                     ORDER BY c.CutoffStart DESC, c.CycleType")
                Return rows.ToList()
            End Using
        End Function

        Public Async Function GetResultsAsync(cutoffId As Integer) As Task(Of PayrollResultSheet) _
            Implements IPayrollProcessRepository.GetResultsAsync

            Dim sheet As New PayrollResultSheet()

            Using conn = GetConnection()
                sheet.Headers = (Await conn.QueryAsync(Of PayrollResultModel)(
                    "SELECT * FROM tblPayrollResult WHERE CutoffID = @cutoffId ORDER BY EmployeeName, EmployeeNo",
                    New With {cutoffId})).ToList()

                sheet.Lines = (Await conn.QueryAsync(Of PayrollResultLineModel)(
                    "SELECT * FROM tblPayrollResultLine WHERE CutoffID = @cutoffId ORDER BY ResultId, SeqNo",
                    New With {cutoffId})).ToList()
            End Using

            Return sheet
        End Function

    End Class

End Namespace
