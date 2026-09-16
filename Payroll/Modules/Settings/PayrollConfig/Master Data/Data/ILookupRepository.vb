Imports Payroll.GlobalShared.Models

Namespace Lookups.Data
    Public Interface ILookupRepository
        Function GetAllAsync(tableName As String) As Task(Of List(Of LookupModel))
        Function CodeExistsAsync(tableName As String, code As String, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(tableName As String, item As LookupModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(tableName As String, item As LookupModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace