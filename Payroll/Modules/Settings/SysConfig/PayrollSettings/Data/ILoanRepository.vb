Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    Public Interface ILoanRepository
        Function GetAllAsync() As Task(Of List(Of LoanModel))
        Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(item As LoanModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As LoanModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
