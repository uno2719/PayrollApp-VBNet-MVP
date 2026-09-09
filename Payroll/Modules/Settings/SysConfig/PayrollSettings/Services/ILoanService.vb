Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface ILoanService
        Function GetAllAsync() As Task(Of List(Of LoanModel))
        Function SaveAsync(item As LoanModel, userName As String) As Task(Of PayrollSettingsSaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
