Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Services
    Public Interface IStatutorySettingsService
        Function GetAllAsync(tableName As String) As Task(Of List(Of StatutoryBracketModel))
        Function SaveAsync(tableName As String, item As StatutoryBracketModel, userName As String) As Task(Of StatutorySettingsSaveResult)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class StatutorySettingsSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace