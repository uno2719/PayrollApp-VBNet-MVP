Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    Public Interface ICompensationRepository
        Function GetAllAsync() As Task(Of List(Of CompensationModel))
        Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(item As CompensationModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As CompensationModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
