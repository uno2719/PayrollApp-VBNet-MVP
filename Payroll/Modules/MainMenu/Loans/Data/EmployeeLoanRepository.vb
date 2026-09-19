' ============================================================
' Modules/MainMenu/Loans/Data/EmployeeLoanRepository.vb
' ============================================================
Imports System.Data.SqlClient
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace Loans.Data

    Public Class EmployeeLoanRepository
        Inherits BaseRepository(Of EmployeeLoanModel)
        Implements IEmployeeLoanRepository

        Private Const LoanTable As String = "tblEmployeeLoan"
        Private Const SchedTable As String = "tblEmployeeLoanSchedule"

        ' ========================================================
        ' LIST - kasama na ang derived na Balance/Next Deduction
        ' ========================================================
        ' OUTER APPLY ang gamit para sa "next unpaid installment".
        '
        ' BAKIT OUTER APPLY AT HINDI SUBQUERY SA SELECT?
        '   Kailangan natin ng DALAWANG column (petsa at halaga) mula
        '   sa PAREHONG isang hilera. Kung scalar subquery, dalawang
        '   beses mong hahanapin ang parehong row - dalawang seek
        '   imbes na isa, at may panganib pang hindi sila magkatugma
        '   kung may dalawang installment sa parehong petsa.
        '   Ang OUTER APPLY ay isang hanap lang, sabay ang dalawa.
        '
        '   "OUTER" (hindi CROSS) para hindi mawala ang loan na wala
        '   nang natitirang unpaid - bayad na kasi lahat.
        ' ========================================================
        Public Async Function GetByEmployeeAsync(employeeRecordId As Integer, statusFilter As String) _
            As Task(Of List(Of EmployeeLoanModel)) _
            Implements IEmployeeLoanRepository.GetByEmployeeAsync

            Dim whereStatus As String = ""

            Select Case If(statusFilter, "Active")
                Case "Active"
                    whereStatus = " AND l.Status = 'Active' "
                Case "Inactive"
                    whereStatus = " AND l.Status = 'Inactive' "
                Case "Completed"
                    whereStatus = " AND l.Status IN ('Completed','Cancelled') "
                Case Else
                    whereStatus = ""    ' All
            End Select

            Dim sql = $"
                SELECT  l.EmployeeLoanId        AS Id,
                        l.EmployeeRecordId,
                        l.LoanCode,
                        l.InputDate,
                        l.LoanStartDate,
                        l.Frequency,
                        l.AmortizationMethod,
                        l.InterestMethod,
                        l.InterestRate,
                        l.Terms,
                        l.PrincipalAmount,
                        l.InterestAmount,
                        l.TotalLoanAmount,
                        l.PrincipalAmortization,
                        l.InterestAmortization,
                        l.TotalAmortization,
                        l.Status,
                        l.CompletedAt,
                        l.Remark,
                        l.CreatedAt, l.CreatedBy, l.UpdatedAt, l.UpdatedBy,

                        lt.Description          AS LoanDescription,
                        lt.LoanType             AS LoanType,

                        e.EmployeeNo            AS EmployeeNo,
                        (e.FirstName + ' ' + e.LastName) AS EmployeeName,

                        ISNULL(agg.TotalInstallments, 0) AS TotalInstallments,
                        ISNULL(agg.PaidInstallments, 0)  AS PaidInstallments,
                        ISNULL(agg.TotalPaid, 0)         AS TotalPaid,

                        nxt.ScheduledDate       AS NextDeductionDate,
                        ISNULL(nxt.AmountDue,0) AS NextDeductionAmount

                FROM    {LoanTable} l
                LEFT JOIN tblLoan    lt ON lt.Code     = l.LoanCode
                LEFT JOIN tblEmployee e ON e.RecordId  = l.EmployeeRecordId

                OUTER APPLY (
                    SELECT  COUNT(*)                                    AS TotalInstallments,
                            SUM(CASE WHEN s.IsPaid = 1 THEN 1 ELSE 0 END) AS PaidInstallments,
                            SUM(s.AmountPaid)                           AS TotalPaid
                    FROM    {SchedTable} s
                    WHERE   s.EmployeeLoanId = l.EmployeeLoanId
                ) agg

                OUTER APPLY (
                    SELECT  TOP 1 s2.ScheduledDate, s2.AmountDue
                    FROM    {SchedTable} s2
                    WHERE   s2.EmployeeLoanId = l.EmployeeLoanId
                      AND   s2.IsPaid = 0
                    ORDER BY s2.ScheduledDate, s2.InstallmentNo
                ) nxt

                WHERE   l.EmployeeRecordId = @employeeRecordId
                        {whereStatus}
                ORDER BY l.LoanStartDate DESC, l.EmployeeLoanId DESC"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of EmployeeLoanModel)(sql, New With {employeeRecordId})
                Return rows.ToList()
            End Using

        End Function

        ' ========================================================
        ' GET BY ID - parehong shape, isang hilera lang
        ' ========================================================
        Public Async Function GetByIdAsync(loanId As Integer) As Task(Of EmployeeLoanModel) _
            Implements IEmployeeLoanRepository.GetByIdAsync

            Dim sql = $"
                SELECT  l.EmployeeLoanId        AS Id,
                        l.EmployeeRecordId, l.LoanCode, l.InputDate, l.LoanStartDate,
                        l.Frequency, l.AmortizationMethod, l.InterestMethod,
                        l.InterestRate, l.Terms,
                        l.PrincipalAmount, l.InterestAmount, l.TotalLoanAmount,
                        l.PrincipalAmortization, l.InterestAmortization, l.TotalAmortization,
                        l.Status, l.CompletedAt, l.Remark,
                        l.CreatedAt, l.CreatedBy, l.UpdatedAt, l.UpdatedBy,

                        lt.Description AS LoanDescription,
                        lt.LoanType    AS LoanType,
                        e.EmployeeNo   AS EmployeeNo,
                        (e.FirstName + ' ' + e.LastName) AS EmployeeName,

                        ISNULL(agg.TotalInstallments, 0) AS TotalInstallments,
                        ISNULL(agg.PaidInstallments, 0)  AS PaidInstallments,
                        ISNULL(agg.TotalPaid, 0)         AS TotalPaid,
                        nxt.ScheduledDate                AS NextDeductionDate,
                        ISNULL(nxt.AmountDue,0)          AS NextDeductionAmount

                FROM    {LoanTable} l
                LEFT JOIN tblLoan    lt ON lt.Code    = l.LoanCode
                LEFT JOIN tblEmployee e ON e.RecordId = l.EmployeeRecordId
                OUTER APPLY (
                    SELECT COUNT(*) AS TotalInstallments,
                           SUM(CASE WHEN s.IsPaid = 1 THEN 1 ELSE 0 END) AS PaidInstallments,
                           SUM(s.AmountPaid) AS TotalPaid
                    FROM   {SchedTable} s WHERE s.EmployeeLoanId = l.EmployeeLoanId
                ) agg
                OUTER APPLY (
                    SELECT TOP 1 s2.ScheduledDate, s2.AmountDue
                    FROM   {SchedTable} s2
                    WHERE  s2.EmployeeLoanId = l.EmployeeLoanId AND s2.IsPaid = 0
                    ORDER BY s2.ScheduledDate, s2.InstallmentNo
                ) nxt
                WHERE   l.EmployeeLoanId = @loanId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of EmployeeLoanModel)(sql, New With {loanId})
            End Using

        End Function

        ' ========================================================
        ' INSERT
        ' ========================================================
        Public Async Function InsertAsync(item As EmployeeLoanModel, userName As String) As Task(Of Integer) _
            Implements IEmployeeLoanRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {LoanTable}
                    (EmployeeRecordId, LoanCode, InputDate, LoanStartDate,
                     Frequency, AmortizationMethod, InterestMethod, InterestRate, Terms,
                     PrincipalAmount, InterestAmount, TotalLoanAmount,
                     PrincipalAmortization, InterestAmortization, TotalAmortization,
                     Status, Remark, CreatedAt, CreatedBy)
                OUTPUT INSERTED.EmployeeLoanId
                VALUES
                    (@EmployeeRecordId, @LoanCode, @InputDate, @LoanStartDate,
                     @Frequency, @AmortizationMethod, @InterestMethod, @InterestRate, @Terms,
                     @PrincipalAmount, @InterestAmount, @TotalLoanAmount,
                     @PrincipalAmortization, @InterestAmortization, @TotalAmortization,
                     @Status, @Remark, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.EmployeeRecordId, item.LoanCode, item.InputDate, item.LoanStartDate,
                    item.Frequency, item.AmortizationMethod, item.InterestMethod,
                    item.InterestRate, item.Terms,
                    item.PrincipalAmount, item.InterestAmount, item.TotalLoanAmount,
                    item.PrincipalAmortization, item.InterestAmortization, item.TotalAmortization,
                    item.Status, item.Remark, userName
                })
            End Using

        End Function

        ' ========================================================
        ' UPDATE - hindi kasama ang Status
        ' ========================================================
        ' Sinasadya kong hindi isama ang Status dito. Ang Status ay
        ' may sarili niyang pinto (SetStatusAsync) dahil may sariling
        ' rules siya - hindi siya basta-basta form field. Kung kasama
        ' siya sa UPDATE, pwedeng aksidenteng ma-revert ang isang
        ' Completed na loan pabalik sa Active dahil lang na-save ulit
        ' ang form.
        ' ========================================================
        Public Async Function UpdateAsync(item As EmployeeLoanModel, userName As String) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.UpdateAsync

            Dim sql = $"
                UPDATE {LoanTable}
                SET     LoanCode              = @LoanCode,
                        LoanStartDate         = @LoanStartDate,
                        Frequency             = @Frequency,
                        AmortizationMethod    = @AmortizationMethod,
                        InterestMethod        = @InterestMethod,
                        InterestRate          = @InterestRate,
                        Terms                 = @Terms,
                        PrincipalAmount       = @PrincipalAmount,
                        InterestAmount        = @InterestAmount,
                        TotalLoanAmount       = @TotalLoanAmount,
                        PrincipalAmortization = @PrincipalAmortization,
                        InterestAmortization  = @InterestAmortization,
                        TotalAmortization     = @TotalAmortization,
                        Remark                = @Remark,
                        UpdatedAt             = GETDATE(),
                        UpdatedBy             = @UserName
                WHERE   EmployeeLoanId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.LoanCode, item.LoanStartDate,
                item.Frequency, item.AmortizationMethod, item.InterestMethod,
                item.InterestRate, item.Terms,
                item.PrincipalAmount, item.InterestAmount, item.TotalLoanAmount,
                item.PrincipalAmortization, item.InterestAmortization, item.TotalAmortization,
                item.Remark, userName
            })

            Return rows > 0

        End Function

        ' ========================================================
        ' DELETE - hard delete
        ' ========================================================
        ' Hard delete ito (hindi soft), pero protektado ng service:
        ' hindi papayag ang EmployeeLoanService na burahin ang loan
        ' na may naitalang bayad. Ang schedule ay sumasama sa ON
        ' DELETE CASCADE.
        ' ========================================================
        Public Async Function DeleteAsync(loanId As Integer) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.DeleteAsync

            Dim sql = $"DELETE FROM {LoanTable} WHERE EmployeeLoanId = @loanId"
            Dim rows = Await MyBase.ExecuteAsync(sql, New With {loanId})
            Return rows > 0

        End Function

        ' ========================================================
        ' SET STATUS
        ' ========================================================
        Public Async Function SetStatusAsync(loanId As Integer, status As String, userName As String) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.SetStatusAsync

            Dim sql = $"
                UPDATE {LoanTable}
                SET     Status      = @status,
                        CompletedAt = CASE WHEN @status = 'Completed'
                                           THEN ISNULL(CompletedAt, GETDATE())
                                           ELSE NULL END,
                        UpdatedAt   = GETDATE(),
                        UpdatedBy   = @userName
                WHERE   EmployeeLoanId = @loanId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {loanId, status, userName})
            Return rows > 0

        End Function

        ' ========================================================
        ' SCHEDULE - READ
        ' ========================================================
        Public Async Function GetScheduleAsync(loanId As Integer) As Task(Of List(Of EmployeeLoanScheduleModel)) _
            Implements IEmployeeLoanRepository.GetScheduleAsync

            Dim sql = $"
                SELECT  ScheduleId AS Id, EmployeeLoanId, InstallmentNo, ScheduledDate,
                        PrincipalDue, InterestDue, AmountDue,
                        AmountPaid, PaidDate, IsPaid,
                        PayrollRefNo, Remark,
                        CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM    {SchedTable}
                WHERE   EmployeeLoanId = @loanId
                ORDER BY InstallmentNo"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of EmployeeLoanScheduleModel)(sql, New With {loanId})
                Return rows.ToList()
            End Using

        End Function

        ' ========================================================
        ' SCHEDULE - REPLACE (transactional)
        ' ========================================================
        ' Delete-then-insert sa loob ng isang transaction.
        '
        ' BAKIT TRANSACTION?
        '   Kung mag-crash sa gitna, mawawala ang lumang schedule pero
        '   wala pang bago - magkakaroon ka ng loan na walang hulugan,
        '   na tahimik na sisira sa Balance at Next Deduction. Ang
        '   transaction ang nagsisiguro na "lahat o wala".
        '
        ' BAKIT DELETE-THEN-INSERT AT HINDI MERGE?
        '   Kapag nagbago ang Terms mula 24 papuntang 12, hindi lang
        '   update ang kailangan - kailangang mawala ang 12 na hilera.
        '   Mas simple at mas madaling maintindihan ang buong palit.
        '   Protektado naman ito: hindi ka papayagan ng service na
        '   mag-edit ng loan na may bayad na.
        ' ========================================================
        Public Async Function ReplaceScheduleAsync(loanId As Integer,
                                                   schedule As List(Of EmployeeLoanScheduleModel),
                                                   userName As String) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.ReplaceScheduleAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()

                    Try
                        Await conn.ExecuteAsync(
                            $"DELETE FROM {SchedTable} WHERE EmployeeLoanId = @loanId",
                            New With {loanId}, tran)

                        If schedule IsNot Nothing AndAlso schedule.Count > 0 Then

                            Dim insertSql = $"
                                INSERT INTO {SchedTable}
                                    (EmployeeLoanId, InstallmentNo, ScheduledDate,
                                     PrincipalDue, InterestDue, AmountDue,
                                     AmountPaid, IsPaid, CreatedAt, CreatedBy)
                                VALUES
                                    (@EmployeeLoanId, @InstallmentNo, @ScheduledDate,
                                     @PrincipalDue, @InterestDue, @AmountDue,
                                     0, 0, GETDATE(), @UserName)"

                            ' Dapper ay tumatanggap ng listahan dito - isang
                            ' tawag, maramihang INSERT, iisang transaction.
                            Dim payload = schedule.Select(Function(s) New With {
                                .EmployeeLoanId = loanId,
                                s.InstallmentNo,
                                s.ScheduledDate,
                                s.PrincipalDue,
                                s.InterestDue,
                                s.AmountDue,
                                .UserName = userName
                            }).ToList()

                            Await conn.ExecuteAsync(insertSql, payload, tran)

                        End If

                        tran.Commit()
                        Return True

                    Catch
                        tran.Rollback()
                        Throw
                    End Try

                End Using
            End Using

        End Function

        ' ========================================================
        ' MAY BAYAD NA BA?
        ' ========================================================
        ' Ginagamit ng service bilang guard bago mag-edit o mag-delete.
        Public Async Function HasAnyPaymentAsync(loanId As Integer) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.HasAnyPaymentAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM   {SchedTable}
                WHERE  EmployeeLoanId = @loanId
                  AND  (IsPaid = 1 OR AmountPaid > 0)"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {loanId})
                Return count > 0
            End Using

        End Function

        ' ========================================================
        ' MARK PAID
        ' ========================================================
        ' Tatawagin ito ng Payroll module mamaya kapag na-post na ang
        ' cutoff. Nailagay ko na ngayon para may laman ang Details
        ' window at para hindi mo na ito babalikan.
        ' ========================================================
        Public Async Function MarkInstallmentPaidAsync(scheduleId As Integer,
                                                       amountPaid As Decimal,
                                                       paidDate As Date,
                                                       payrollRefNo As String,
                                                       userName As String) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.MarkInstallmentPaidAsync

            Dim sql = $"
                UPDATE {SchedTable}
                SET     AmountPaid   = @amountPaid,
                        PaidDate     = @paidDate,
                        IsPaid       = CASE WHEN @amountPaid >= AmountDue THEN 1 ELSE 0 END,
                        PayrollRefNo = @payrollRefNo,
                        UpdatedAt    = GETDATE(),
                        UpdatedBy    = @userName
                WHERE   ScheduleId = @scheduleId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                scheduleId, amountPaid, paidDate, payrollRefNo, userName})

            Return rows > 0

        End Function

    End Class

End Namespace