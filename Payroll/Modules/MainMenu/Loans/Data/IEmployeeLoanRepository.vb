' ============================================================
' Modules/MainMenu/Loans/Data/IEmployeeLoanRepository.vb
' ============================================================
Imports Payroll.GlobalShared.Models

Namespace Loans.Data

    Public Interface IEmployeeLoanRepository

        ' --- LOANS ---
        Function GetByEmployeeAsync(employeeRecordId As Integer, statusFilter As String) _
            As Task(Of List(Of EmployeeLoanModel))

        Function GetByIdAsync(loanId As Integer) As Task(Of EmployeeLoanModel)

        Function InsertAsync(item As EmployeeLoanModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As EmployeeLoanModel, userName As String) As Task(Of Boolean)
        Function DeleteAsync(loanId As Integer) As Task(Of Boolean)

        Function SetStatusAsync(loanId As Integer, status As String, userName As String) As Task(Of Boolean)

        ' --- SCHEDULE ---
        Function GetScheduleAsync(loanId As Integer) As Task(Of List(Of EmployeeLoanScheduleModel))

        ' Buong palit ng schedule (delete lahat, insert ulit) sa loob ng
        ' isang transaction. Ginagamit sa Insert at sa Update ng loan.
        Function ReplaceScheduleAsync(loanId As Integer,
                                      schedule As List(Of EmployeeLoanScheduleModel),
                                      userName As String) As Task(Of Boolean)

        Function HasAnyPaymentAsync(loanId As Integer) As Task(Of Boolean)

        ' May bayad na GALING SA PAYROLL POST? Hindi kasama ang C1PAY (opening), OFFSET- at CREDIT- (offset).
        Function HasPostedPaymentAsync(loanId As Integer) As Task(Of Boolean)

        ' --- Offset (renewal) ---
        Function GetOffsetCandidatesAsync(employeeRecordId As Integer, loanCode As String,
                                          excludeLoanId As Integer) As Task(Of List(Of LoanOffsetCandidate))

        Function GetRefundTargetAsync(employeeRecordId As Integer, offsetDate As Date) As Task(Of LoanRefundTarget)

        Function GetRefundTxnStateAsync(txnId As Integer) As Task(Of LoanRefundTxnState)

        ' Insert ng bagong loan + schedule + offset ng luma + credit, sa iisang transaction. Ibinabalik ang bagong Id.
        Function SaveWithOffsetAsync(item As EmployeeLoanModel,
                                     schedule As List(Of EmployeeLoanScheduleModel),
                                     plan As LoanOffsetPlan,
                                     userName As String) As Task(Of Integer)

        Function UndoOffsetAsync(newLoanId As Integer, userName As String) As Task(Of LoanOffsetUndoCounts)

        ' Markahang bayad ang unbayad na hulog #1..#upTo bilang opening payment (C1PAY). Ibinabalik ang bilang na na-update.
        Function MarkOpeningPaidAsync(loanId As Integer, upToInstallmentNo As Integer, userName As String) As Task(Of Integer)

        ' Ibalik sa unpaid ang LAHAT ng C1PAY na hulog ng loan. Ibinabalik ang bilang na na-update.
        Function UndoOpeningPaidAsync(loanId As Integer, userName As String) As Task(Of Integer)

        Function MarkInstallmentPaidAsync(scheduleId As Integer,
                                          amountPaid As Decimal,
                                          paidDate As Date,
                                          payrollRefNo As String,
                                          userName As String) As Task(Of Boolean)

    End Interface

End Namespace