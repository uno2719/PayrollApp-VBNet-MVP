' File: Modules/Settings/SysConfig/GeneralSettings/Services/IGeneralSettingsService.vb
Imports Payroll.GlobalShared.Models

Namespace GeneralSettings.Services
    Public Interface IGeneralSettingsService
        Function GetAsync() As Task(Of GeneralSettingsModel)
        Function SaveAsync(item As GeneralSettingsModel, userName As String) As Task(Of GeneralSettingsSaveResult)
    End Interface

    Public Class GeneralSettingsSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace