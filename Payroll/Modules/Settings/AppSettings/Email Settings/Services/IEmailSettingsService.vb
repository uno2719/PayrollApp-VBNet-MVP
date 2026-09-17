' File: Modules/Settings/AppSettings/EmailSettings/Services/IEmailSettingsService.vb
Imports Payroll.GlobalShared.Models

Namespace EmailSettings.Services
    Public Interface IEmailSettingsService

        Function GetAsync() As Task(Of EmailSettingsModel)

        ''' <summary>
        ''' Hiwalay ang plainPassword sa model dahil hindi kailanman
        ''' dapat may plain password na naka-lagay sa EmailSettingsModel.
        ''' Kung blangko ang plainPassword, PINANATILI ang dating naka-save
        ''' na password - hindi ito binubura (tingnan ang komento sa
        ''' implementation kung bakit).
        ''' </summary>
        Function SaveAsync(item As EmailSettingsModel, plainPassword As String, userName As String) As Task(Of EmailSettingsSaveResult)

        Function GetDecryptedPassword(item As EmailSettingsModel) As String

        ''' <summary>
        ''' Sinusubukang magpadala ng test message gamit ang settings na
        ''' nasa screen - hindi yung naka-save. Ganito para masubukan mo
        ''' muna bago mo i-Save.
        ''' </summary>
        Function SendTestEmailAsync(item As EmailSettingsModel, plainPassword As String, recipient As String) As Task(Of EmailTestResult)
    End Interface

    Public Class EmailSettingsSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class

    Public Class EmailTestResult
        Public Property Success As Boolean
        Public Property Message As String
    End Class
End Namespace
