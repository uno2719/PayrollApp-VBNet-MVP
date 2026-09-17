' File: Modules/Settings/AppSettings/EmailSettings/Data/IEmailSettingsRepository.vb
Imports Payroll.GlobalShared.Models

Namespace EmailSettings.Data
    Public Interface IEmailSettingsRepository
        Function GetAsync() As Task(Of EmailSettingsModel)
        Function InsertAsync(item As EmailSettingsModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As EmailSettingsModel, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
