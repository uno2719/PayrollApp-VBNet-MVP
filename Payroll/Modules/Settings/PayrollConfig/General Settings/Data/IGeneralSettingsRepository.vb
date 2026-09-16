' File: Modules/Settings/SysConfig/GeneralSettings/Data/IGeneralSettingsRepository.vb
Imports Payroll.GlobalShared.Models

Namespace GeneralSettings.Data
    Public Interface IGeneralSettingsRepository
        Function GetAsync() As Task(Of GeneralSettingsModel)
        Function InsertAsync(item As GeneralSettingsModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As GeneralSettingsModel, userName As String) As Task(Of Boolean)
    End Interface
End Namespace