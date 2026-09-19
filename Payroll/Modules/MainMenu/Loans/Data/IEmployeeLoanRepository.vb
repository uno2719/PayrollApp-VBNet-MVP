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

        Function MarkInstallmentPaidAsync(scheduleId As Integer,
                                          amountPaid As Decimal,
                                          paidDate As Date,
                                          payrollRefNo As String,
                                          userName As String) As Task(Of Boolean)

    End Interface

End Namespace