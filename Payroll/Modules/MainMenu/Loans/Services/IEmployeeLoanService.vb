' ============================================================
' Modules/MainMenu/Loans/Services/IEmployeeLoanService.vb
' ============================================================
Imports Payroll.GlobalShared.Models

Namespace Loans.Services

    Public Interface IEmployeeLoanService

        Function GetByEmployeeAsync(employeeRecordId As Integer, statusFilter As String) _
            As Task(Of List(Of EmployeeLoanModel))

        Function GetDetailAsync(loanId As Integer) As Task(Of EmployeeLoanDetailModel)

        ''' <summary>
        ''' Purong kalkulasyon - WALANG database. Tinatawag ito ng View
        ''' habang nagta-type si user para live ang preview ng Interest,
        ''' Total Loan Amount, at Amortization.
        '''
        ''' Binabago nito ang mismong object na ipinasa mo.
        ''' </summary>
        Function Compute(item As EmployeeLoanModel) As LoanComputationResult

        ''' <summary>
        ''' Gumagawa ng listahan ng hulugan base sa nakuwentang loan.
        ''' Public ito (hindi Private) para magamit ng Details preview
        ''' bago pa man i-save.
        ''' </summary>
        Function GenerateSchedule(item As EmployeeLoanModel) As List(Of EmployeeLoanScheduleModel)

        Function SaveAsync(item As EmployeeLoanModel, userName As String) As Task(Of LoanSaveResult)
        Function DeleteAsync(loanId As Integer, userName As String) As Task(Of LoanSaveResult)

        Function ToggleActiveStatusAsync(loanId As Integer, userName As String) As Task(Of LoanSaveResult)

        Function RecordPaymentAsync(scheduleId As Integer, loanId As Integer,
                                    amountPaid As Decimal, paidDate As Date,
                                    payrollRefNo As String, userName As String) As Task(Of LoanSaveResult)

    End Interface


    Public Class LoanSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property NewStatus As String
    End Class

    ''' <summary>
    ''' Iba ito sa LoanSaveResult: ang Compute ay pwedeng "may mali sa
    ''' input" nang hindi naman ibig sabihing hindi na dapat ipakita ang
    ''' resulta. Halimbawa: 0 pa ang Terms habang nagta-type pa lang si
    ''' user - hindi iyon error na dapat may message box, pero hindi rin
    ''' pwedeng mag-divide by zero.
    ''' </summary>
    Public Class LoanComputationResult
        Public Property IsComplete As Boolean
        Public Property Warning As String
    End Class

End Namespace