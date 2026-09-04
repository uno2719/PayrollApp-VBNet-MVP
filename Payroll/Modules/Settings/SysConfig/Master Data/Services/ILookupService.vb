Imports Payroll.GlobalShared.Models

Namespace Lookups.Services
    Public Interface ILookupService
        Function GetAllAsync(tableName As String) As Task(Of List(Of LookupModel))
        Function SaveAsync(tableName As String, item As LookupModel, userName As String) As Task(Of LookupSaveResult)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class LookupSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace