' ============================================================
' Modules/MainMenu/Loans/Data/EmployeeLoanRepository.vb
' ============================================================
Imports System.Data.SqlClient
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
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
                    whereStatus = " AND l.Status IN ('Completed','Cancelled','Offset') "
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
                        l.RatePeriod,
                        l.AmortBasis,
                        l.PrincipalAmount,
                        l.InterestAmount,
                        l.TotalLoanAmount,
                        l.PrincipalAmortization,
                        l.InterestAmortization,
                        l.TotalAmortization,
                        l.Status,
                        l.CompletedAt,
                        l.Remark,
                        l.OffsetByLoanId, l.OffsetDate, l.OffsetCredit, l.OffsetCreditMode, l.OffsetRefundTxnId,
                        (SELECT COUNT(*) FROM {LoanTable} oc WHERE oc.OffsetByLoanId = l.EmployeeLoanId) AS OffsetChildCount,
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
                        l.InterestRate, l.Terms, l.RatePeriod, l.AmortBasis,
                        l.PrincipalAmount, l.InterestAmount, l.TotalLoanAmount,
                        l.PrincipalAmortization, l.InterestAmortization, l.TotalAmortization,
                        l.Status, l.CompletedAt, l.Remark,
                        l.OffsetByLoanId, l.OffsetDate, l.OffsetCredit, l.OffsetCreditMode, l.OffsetRefundTxnId,
                        (SELECT COUNT(*) FROM {LoanTable} oc WHERE oc.OffsetByLoanId = l.EmployeeLoanId) AS OffsetChildCount,
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
        ' Iisang INSERT para sa ordinaryong Save at sa Save na may Offset (SaveWithOffsetAsync), para hindi
        ' magkaiba ang dalawa kapag may bagong column.
        Private Shared ReadOnly InsertLoanSql As String = $"
                INSERT INTO {LoanTable}
                    (EmployeeRecordId, LoanCode, InputDate, LoanStartDate,
                     Frequency, AmortizationMethod, InterestMethod, InterestRate, Terms,
                     RatePeriod, AmortBasis,
                     PrincipalAmount, InterestAmount, TotalLoanAmount,
                     PrincipalAmortization, InterestAmortization, TotalAmortization,
                     Status, Remark, OffsetCredit, OffsetCreditMode, CreatedAt, CreatedBy)
                OUTPUT INSERTED.EmployeeLoanId
                VALUES
                    (@EmployeeRecordId, @LoanCode, @InputDate, @LoanStartDate,
                     @Frequency, @AmortizationMethod, @InterestMethod, @InterestRate, @Terms,
                     @RatePeriod, @AmortBasis,
                     @PrincipalAmount, @InterestAmount, @TotalLoanAmount,
                     @PrincipalAmortization, @InterestAmortization, @TotalAmortization,
                     @Status, @Remark, @OffsetCredit, @OffsetCreditMode, GETDATE(), @UserName)"

        ' PAALALA: may leading dot ang mga pinangalanang member (.UserName = ...) - kung wala, hindi sila
        ' nagiging parameter at "Must declare scalar variable" ang lalabas sa runtime.
        Private Shared Function InsertLoanParams(item As EmployeeLoanModel, userName As String) As Object
            Return New With {
                item.EmployeeRecordId, item.LoanCode, item.InputDate, item.LoanStartDate,
                item.Frequency, item.AmortizationMethod, item.InterestMethod,
                item.InterestRate, item.Terms,
                .RatePeriod = If(item.RatePeriod, LoanRatePeriod.PerPeriod),
                .AmortBasis = If(item.AmortBasis, LoanAmortBasis.PerDeduction),
                item.PrincipalAmount, item.InterestAmount, item.TotalLoanAmount,
                item.PrincipalAmortization, item.InterestAmortization, item.TotalAmortization,
                item.Status, item.Remark, item.OffsetCredit, item.OffsetCreditMode,
                .UserName = userName
            }
        End Function

        Public Async Function InsertAsync(item As EmployeeLoanModel, userName As String) As Task(Of Integer) _
            Implements IEmployeeLoanRepository.InsertAsync

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(InsertLoanSql, InsertLoanParams(item, userName))
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
                        RatePeriod            = @RatePeriod,
                        AmortBasis            = @AmortBasis,
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
                .RatePeriod = If(item.RatePeriod, LoanRatePeriod.PerPeriod),
                .AmortBasis = If(item.AmortBasis, LoanAmortBasis.PerDeduction),
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
        ' MAY BAYAD NA GALING SA PAYROLL? (hindi kasama ang C1PAY)
        ' ========================================================
        Public Async Function HasPostedPaymentAsync(loanId As Integer) As Task(Of Boolean) _
            Implements IEmployeeLoanRepository.HasPostedPaymentAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM   {SchedTable}
                WHERE  EmployeeLoanId = @loanId
                  AND  (IsPaid = 1 OR AmountPaid > 0)
                  AND  ISNULL(PayrollRefNo, '') <> @refNo
                  AND  ISNULL(PayrollRefNo, '') NOT LIKE @offsetLike
                  AND  ISNULL(PayrollRefNo, '') NOT LIKE @creditLike"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    loanId, .refNo = LoanPaymentRef.OpeningBalance,
                    .offsetLike = LoanPaymentRef.OffsetPrefix & "%",
                    .creditLike = LoanPaymentRef.CreditPrefix & "%"})
                Return count > 0
            End Using

        End Function

        ' ========================================================
        ' OPENING PAYMENTS (nabayaran na sa C1Pay)
        ' ========================================================
        ' Ang hulog #1..#upTo na hindi pa bayad ay minamarkahang bayad nang buo:
        '   AmountPaid = AmountDue, PaidDate = ScheduledDate, PayrollRefNo = 'C1PAY'.
        ' Isang UPDATE lang = atomic na.
        Public Async Function MarkOpeningPaidAsync(loanId As Integer, upToInstallmentNo As Integer, userName As String) _
            As Task(Of Integer) Implements IEmployeeLoanRepository.MarkOpeningPaidAsync

            Dim sql = $"
                UPDATE {SchedTable}
                SET     AmountPaid   = AmountDue,
                        PaidDate     = ScheduledDate,
                        IsPaid       = 1,
                        PayrollRefNo = @refNo,
                        UpdatedAt    = GETDATE(),
                        UpdatedBy    = @userName
                WHERE   EmployeeLoanId = @loanId
                  AND   InstallmentNo <= @upToInstallmentNo
                  AND   IsPaid = 0
                  AND   AmountPaid = 0"

            Return Await MyBase.ExecuteAsync(sql, New With {
                loanId, upToInstallmentNo, userName, .refNo = LoanPaymentRef.OpeningBalance})

        End Function

        Public Async Function UndoOpeningPaidAsync(loanId As Integer, userName As String) _
            As Task(Of Integer) Implements IEmployeeLoanRepository.UndoOpeningPaidAsync

            Dim sql = $"
                UPDATE {SchedTable}
                SET     AmountPaid   = 0,
                        PaidDate     = NULL,
                        IsPaid       = 0,
                        PayrollRefNo = NULL,
                        UpdatedAt    = GETDATE(),
                        UpdatedBy    = @userName
                WHERE   EmployeeLoanId = @loanId
                  AND   PayrollRefNo   = @refNo"

            Return Await MyBase.ExecuteAsync(sql, New With {
                loanId, userName, .refNo = LoanPaymentRef.OpeningBalance})

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

        ' ========================================================
        ' OFFSET (RENEWAL)
        ' ========================================================

        ' Mga Active na loan ng employee na may parehong LoanCode at may hindi pa bayad na hulog.
        ' Ang "blk" ay ang Processed-pero-hindi-pa-Posted na cutoff (kung meron) na may hawak ng hulog ng loan:
        ' bawal mag-offset doon, dahil magre-reklamo ang Post na "loan is no longer Active".
        Public Async Function GetOffsetCandidatesAsync(employeeRecordId As Integer, loanCode As String,
                                                       excludeLoanId As Integer) As Task(Of List(Of LoanOffsetCandidate)) _
            Implements IEmployeeLoanRepository.GetOffsetCandidatesAsync

            Dim sql = $"
                SELECT  l.EmployeeLoanId AS LoanId, l.LoanCode, l.LoanStartDate, l.Status,
                        l.TotalLoanAmount, l.TotalAmortization,
                        agg.TotalInstallments, agg.UnpaidCount, agg.UnpaidAmount,
                        blk.CutoffText AS BlockedCutoff
                FROM    {LoanTable} l
                OUTER APPLY (
                    SELECT COUNT(*) AS TotalInstallments,
                           SUM(CASE WHEN s.IsPaid = 0 THEN 1 ELSE 0 END) AS UnpaidCount,
                           SUM(CASE WHEN s.IsPaid = 0 THEN s.AmountDue - s.AmountPaid ELSE 0 END) AS UnpaidAmount
                    FROM   {SchedTable} s
                    WHERE  s.EmployeeLoanId = l.EmployeeLoanId
                ) agg
                OUTER APPLY (
                    SELECT TOP 1 ISNULL(NULLIF(c.CutoffLabel, ''), c.CycleType) AS CutoffText
                    FROM   tblPayrollResultLine rl
                           INNER JOIN {SchedTable} s2 ON s2.ScheduleId = rl.RefId
                           INNER JOIN tblCutoff c ON c.CutoffID = rl.CutoffID
                    WHERE  rl.Category = @loanCategory
                      AND  s2.EmployeeLoanId = l.EmployeeLoanId
                      AND  s2.IsPaid = 0
                      AND  c.Status = @processed
                ) blk
                WHERE   l.EmployeeRecordId = @employeeRecordId
                  AND   l.LoanCode = @loanCode
                  AND   l.Status = @active
                  AND   l.EmployeeLoanId <> @excludeLoanId
                  AND   ISNULL(agg.UnpaidCount, 0) > 0
                ORDER BY l.LoanStartDate, l.EmployeeLoanId"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of OffsetCandidateRow)(sql, New With {
                    employeeRecordId, loanCode, excludeLoanId,
                    .loanCategory = ResultCategory.Loan,
                    .processed = CByte(CutoffStatus.Processed),
                    .active = LoanStatus.Active})

                Return rows.Select(Function(r) New LoanOffsetCandidate With {
                    .LoanId = r.LoanId, .LoanCode = r.LoanCode, .LoanStartDate = r.LoanStartDate, .Status = r.Status,
                    .TotalLoanAmount = r.TotalLoanAmount, .TotalAmortization = r.TotalAmortization,
                    .TotalInstallments = r.TotalInstallments, .UnpaidCount = r.UnpaidCount,
                    .UnpaidAmount = If(r.UnpaidAmount, 0D),
                    .BlockedReason = If(String.IsNullOrWhiteSpace(r.BlockedCutoff), Nothing,
                        $"An installment of this loan is in cutoff {r.BlockedCutoff}, which is Processed but not yet Posted. " &
                        "Post that cutoff first, then offset."),
                    .Selected = String.IsNullOrWhiteSpace(r.BlockedCutoff)
                }).ToList()
            End Using

        End Function

        Private Class OffsetCandidateRow
            Public Property LoanId As Integer
            Public Property LoanCode As String
            Public Property LoanStartDate As Date
            Public Property Status As String
            Public Property TotalLoanAmount As Decimal
            Public Property TotalAmortization As Decimal
            Public Property TotalInstallments As Integer
            Public Property UnpaidCount As Integer
            Public Property UnpaidAmount As Decimal?
            Public Property BlockedCutoff As String
        End Class

        ' Ang Loan Refund Code (General Settings) at ang unang Draft cutoff ng pay cycle ng employee na
        ' nagtatapos sa o pagkatapos ng offset date.
        Public Async Function GetRefundTargetAsync(employeeRecordId As Integer, offsetDate As Date) As Task(Of LoanRefundTarget) _
            Implements IEmployeeLoanRepository.GetRefundTargetAsync

            Dim sql = "
                SELECT  g.LoanRefundCodeId AS CompensationId,
                        comp.Code          AS CompensationCode,
                        comp.Description   AS CompensationDescription,
                        CAST(comp.InputUnit AS TINYINT) AS InputUnit,
                        CAST(comp.IsActive AS BIT)      AS CompensationIsActive,
                        ee.PayCycle,
                        cut.CutoffID       AS CutoffId,
                        cut.CutoffText     AS CutoffLabel
                FROM    (SELECT 1 AS One) x
                OUTER APPLY (SELECT TOP 1 LoanRefundCodeId FROM tblGeneralSettings ORDER BY GeneralSettingsId) g
                LEFT JOIN tblCompensation comp ON comp.CompensationId = g.LoanRefundCodeId
                OUTER APPLY (SELECT TOP 1 PayCycle FROM tblEmployeeEarnings WHERE RecordId = @employeeRecordId) ee
                OUTER APPLY (
                    SELECT TOP 1 c.CutoffID, ISNULL(NULLIF(c.CutoffLabel, ''), c.CycleType) AS CutoffText
                    FROM   tblCutoff c
                    WHERE  c.CycleType = ee.PayCycle
                      AND  c.Status = @draft
                      AND  c.CutoffEnd >= @offsetDate
                    ORDER BY c.CutoffStart
                ) cut"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of LoanRefundTarget)(sql, New With {
                    employeeRecordId, .offsetDate = offsetDate.Date, .draft = CByte(CutoffStatus.Draft)})
            End Using

        End Function

        Public Async Function GetRefundTxnStateAsync(txnId As Integer) As Task(Of LoanRefundTxnState) _
            Implements IEmployeeLoanRepository.GetRefundTxnStateAsync

            Dim sql = "
                SELECT  CAST(1 AS BIT) AS Found,
                        CAST(c.Status AS TINYINT) AS CutoffStatus,
                        ISNULL(NULLIF(c.CutoffLabel, ''), c.CycleType) AS CutoffLabel
                FROM    tblPayrollInputTxn t
                        INNER JOIN tblCutoff c ON c.CutoffID = t.CutoffID
                WHERE   t.TxnID = @txnId"

            Using conn = GetConnection()
                Dim row = Await conn.QuerySingleOrDefaultAsync(Of LoanRefundTxnState)(sql, New With {txnId})
                Return If(row, New LoanRefundTxnState With {.Found = False})
            End Using

        End Function

        ' --------------------------------------------------------
        ' SAVE + OFFSET: iisang transaction. Kapag may pumalya kahit isa, walang mase-save.
        ' --------------------------------------------------------
        Public Async Function SaveWithOffsetAsync(item As EmployeeLoanModel,
                                                  schedule As List(Of EmployeeLoanScheduleModel),
                                                  plan As LoanOffsetPlan,
                                                  userName As String) As Task(Of Integer) _
            Implements IEmployeeLoanRepository.SaveWithOffsetAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        ' 1. Bagong loan
                        Dim newId = Await conn.ExecuteScalarAsync(Of Integer)(InsertLoanSql, InsertLoanParams(item, userName), tran)

                        ' 2. Schedule nito
                        Await InsertScheduleRowsAsync(conn, tran, newId, schedule, userName)

                        Dim offsetRef = LoanPaymentRef.OffsetPrefix & newId.ToString()

                        ' 3. Bawat lumang loan
                        For Each oldId In plan.OldLoanIds

                            ' Muling check sa loob ng transaction (baka may nag-Process habang nakabukas ang dialog).
                            Dim blocked = Await conn.ExecuteScalarAsync(Of Integer)(
                                $"SELECT COUNT(*)
                                  FROM   tblPayrollResultLine rl
                                         INNER JOIN {SchedTable} s ON s.ScheduleId = rl.RefId
                                         INNER JOIN tblCutoff c ON c.CutoffID = rl.CutoffID
                                  WHERE  rl.Category = @loanCategory AND s.EmployeeLoanId = @oldId
                                    AND  s.IsPaid = 0 AND c.Status = @processed",
                                New With {oldId, .loanCategory = ResultCategory.Loan, .processed = CByte(CutoffStatus.Processed)}, tran)

                            If blocked > 0 Then
                                Throw New InvalidOperationException(
                                    $"Loan #{oldId} has an installment in a Processed (not yet Posted) cutoff. Post that cutoff first, then offset.")
                            End If

                            Await conn.ExecuteAsync(
                                $"UPDATE {SchedTable}
                                  SET    AmountPaid   = AmountDue,
                                         PaidDate     = @offsetDate,
                                         IsPaid       = 1,
                                         PayrollRefNo = @offsetRef,
                                         UpdatedAt    = GETDATE(),
                                         UpdatedBy    = @userName
                                  WHERE  EmployeeLoanId = @oldId AND IsPaid = 0",
                                New With {oldId, .offsetDate = plan.OffsetDate, offsetRef, userName}, tran)

                            Dim remark = LoanOffsetRules.BuildOldLoanRemark(newId, plan.OffsetDate, plan.BalancePerStatement)

                            Dim changed = Await conn.ExecuteAsync(
                                $"UPDATE {LoanTable}
                                  SET    Status         = @offsetStatus,
                                         OffsetByLoanId = @newId,
                                         OffsetDate     = @offsetDate,
                                         Remark         = LEFT(CASE WHEN ISNULL(Remark, N'') = N'' THEN @remark
                                                                    ELSE Remark + N' | ' + @remark END, 1000),
                                         UpdatedAt      = GETDATE(),
                                         UpdatedBy      = @userName
                                  WHERE  EmployeeLoanId = @oldId AND Status = @active",
                                New With {oldId, newId, remark, userName, .offsetDate = plan.OffsetDate,
                                          .offsetStatus = LoanStatus.Offset, .active = LoanStatus.Active}, tran)

                            If changed <> 1 Then
                                Throw New InvalidOperationException($"Loan #{oldId} is no longer Active. Nothing was saved - please try again.")
                            End If
                        Next

                        ' 4. Credit
                        If plan.Credit > 0D Then

                            If plan.CreditMode = LoanCreditMode.RefundThroughPayroll Then

                                Dim exists = Await conn.ExecuteScalarAsync(Of Integer)(
                                    "SELECT COUNT(*) FROM tblPayrollInputTxn
                                     WHERE CutoffID = @cutoffId AND RecordId = @recordId AND TxnCategory = @category
                                       AND TxnCode = @code AND TxnSrc = @src",
                                    New With {.cutoffId = plan.RefundCutoffId.Value, .recordId = item.EmployeeRecordId,
                                              .category = PayrollInputUnits.CategoryCompensation, .code = plan.RefundCode,
                                              .src = LoanTxnSource.OffsetRefund}, tran)

                                If exists > 0 Then
                                    Throw New InvalidOperationException(
                                        "This employee already has a loan-offset refund in that cutoff. Undo that offset first, or use 'Deduct from the first/last installment(s)'.")
                                End If

                                Dim stillDraft = Await conn.ExecuteScalarAsync(Of Integer)(
                                    "SELECT COUNT(*) FROM tblCutoff WHERE CutoffID = @cutoffId AND Status = @draft",
                                    New With {.cutoffId = plan.RefundCutoffId.Value, .draft = CByte(CutoffStatus.Draft)}, tran)

                                If stillDraft = 0 Then
                                    Throw New InvalidOperationException("The refund cutoff is no longer Draft. Nothing was saved - please try again.")
                                End If

                                Dim txnId = Await conn.ExecuteScalarAsync(Of Integer)(
                                    "INSERT INTO tblPayrollInputTxn (CutoffID, RecordId, TxnCategory, TxnCode, Qty, Rate, TxnSrc, ModifiedBy, ModifiedDate)
                                     OUTPUT INSERTED.TxnID
                                     VALUES (@cutoffId, @recordId, @category, @code, 1, @amount, @src, @userName, SYSDATETIME())",
                                    New With {.cutoffId = plan.RefundCutoffId.Value, .recordId = item.EmployeeRecordId,
                                              .category = PayrollInputUnits.CategoryCompensation, .code = plan.RefundCode,
                                              .amount = plan.Credit, .src = LoanTxnSource.OffsetRefund, userName}, tran)

                                Await conn.ExecuteAsync(
                                    $"UPDATE {LoanTable} SET OffsetRefundTxnId = @txnId WHERE EmployeeLoanId = @newId",
                                    New With {txnId, newId}, tran)

                            Else

                                Dim creditRef = LoanPaymentRef.CreditPrefix & newId.ToString()

                                Dim payload = plan.CreditAllocations.Select(Function(a) New With {
                                    .newId = newId,
                                    .installmentNo = a.InstallmentNo,
                                    .amount = a.Amount,
                                    .offsetDate = plan.OffsetDate,
                                    .creditRef = creditRef,
                                    .userName = userName
                                }).ToList()

                                Await conn.ExecuteAsync(
                                    $"UPDATE {SchedTable}
                                      SET    AmountPaid   = @amount,
                                             IsPaid       = CASE WHEN @amount >= AmountDue THEN 1 ELSE 0 END,
                                             PaidDate     = @offsetDate,
                                             PayrollRefNo = @creditRef,
                                             UpdatedAt    = GETDATE(),
                                             UpdatedBy    = @userName
                                      WHERE  EmployeeLoanId = @newId AND InstallmentNo = @installmentNo",
                                    payload, tran)

                            End If
                        End If

                        tran.Commit()
                        Return newId

                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        End Function

        ' --------------------------------------------------------
        ' UNDO OFFSET: iisang transaction din.
        ' --------------------------------------------------------
        Public Async Function UndoOffsetAsync(newLoanId As Integer, userName As String) As Task(Of LoanOffsetUndoCounts) _
            Implements IEmployeeLoanRepository.UndoOffsetAsync

            Dim counts As New LoanOffsetUndoCounts
            Dim offsetRef = LoanPaymentRef.OffsetPrefix & newLoanId.ToString()
            Dim creditRef = LoanPaymentRef.CreditPrefix & newLoanId.ToString()

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        ' 1. Hulog ng lumang loan -> unpaid ulit
                        counts.OldInstallments = Await conn.ExecuteAsync(
                            $"UPDATE s
                              SET    s.AmountPaid = 0, s.PaidDate = NULL, s.IsPaid = 0, s.PayrollRefNo = NULL,
                                     s.UpdatedAt = GETDATE(), s.UpdatedBy = @userName
                              FROM   {SchedTable} s
                                     INNER JOIN {LoanTable} l ON l.EmployeeLoanId = s.EmployeeLoanId
                              WHERE  l.OffsetByLoanId = @newLoanId AND s.PayrollRefNo = @offsetRef",
                            New With {newLoanId, offsetRef, userName}, tran)

                        ' 2. Lumang loan -> Active ulit (naiiwan ang Remark para may bakas)
                        counts.OldLoans = Await conn.ExecuteAsync(
                            $"UPDATE {LoanTable}
                              SET    Status = @active, OffsetByLoanId = NULL, OffsetDate = NULL,
                                     Remark = LEFT(ISNULL(Remark, N'') + N' | Offset undone', 1000),
                                     UpdatedAt = GETDATE(), UpdatedBy = @userName
                              WHERE  OffsetByLoanId = @newLoanId",
                            New With {newLoanId, userName, .active = LoanStatus.Active}, tran)

                        ' 3. Credit sa bagong loan
                        counts.CreditInstallments = Await conn.ExecuteAsync(
                            $"UPDATE {SchedTable}
                              SET    AmountPaid = 0, PaidDate = NULL, IsPaid = 0, PayrollRefNo = NULL,
                                     UpdatedAt = GETDATE(), UpdatedBy = @userName
                              WHERE  EmployeeLoanId = @newLoanId AND PayrollRefNo = @creditRef",
                            New With {newLoanId, creditRef, userName}, tran)

                        ' 4. Refund row (Draft cutoff lang - ang Service na ang nag-check)
                        Dim refundId = Await conn.ExecuteScalarAsync(Of Integer?)(
                            $"SELECT OffsetRefundTxnId FROM {LoanTable} WHERE EmployeeLoanId = @newLoanId",
                            New With {newLoanId}, tran)

                        If refundId.HasValue Then
                            Dim removed = Await conn.ExecuteAsync(
                                "DELETE t FROM tblPayrollInputTxn t
                                        INNER JOIN tblCutoff c ON c.CutoffID = t.CutoffID
                                 WHERE  t.TxnID = @refundId AND t.TxnSrc = @src AND c.Status = @draft",
                                New With {.refundId = refundId.Value, .src = LoanTxnSource.OffsetRefund,
                                          .draft = CByte(CutoffStatus.Draft)}, tran)
                            counts.RefundRemoved = removed > 0
                        End If

                        ' 5. Linisin ang offset info ng bagong loan
                        Await conn.ExecuteAsync(
                            $"UPDATE {LoanTable}
                              SET    OffsetCredit = 0, OffsetCreditMode = NULL, OffsetRefundTxnId = NULL,
                                     UpdatedAt = GETDATE(), UpdatedBy = @userName
                              WHERE  EmployeeLoanId = @newLoanId",
                            New With {newLoanId, userName}, tran)

                        tran.Commit()
                        Return counts

                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        End Function

        ' Ginagamit ng SaveWithOffsetAsync (pareho ang INSERT ng ReplaceScheduleAsync).
        Private Shared Async Function InsertScheduleRowsAsync(conn As SqlConnection, tran As SqlTransaction, loanId As Integer,
                                                              schedule As List(Of EmployeeLoanScheduleModel),
                                                              userName As String) As Task

            If schedule Is Nothing OrElse schedule.Count = 0 Then Return

            Dim insertSql = $"
                INSERT INTO {SchedTable}
                    (EmployeeLoanId, InstallmentNo, ScheduledDate,
                     PrincipalDue, InterestDue, AmountDue,
                     AmountPaid, IsPaid, CreatedAt, CreatedBy)
                VALUES
                    (@EmployeeLoanId, @InstallmentNo, @ScheduledDate,
                     @PrincipalDue, @InterestDue, @AmountDue,
                     0, 0, GETDATE(), @UserName)"

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

        End Function

    End Class

End Namespace
