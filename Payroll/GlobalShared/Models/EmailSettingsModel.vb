' File: GlobalShared/Models/EmailSettingsModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row lang palagi mula sa tblEmailSettings - ang SMTP account
    ''' na ginagamit sa pagpapadala ng payslip at iba pang email.
    '''
    ''' PANSININ: SmtpPasswordEncrypted ang hawak dito, hindi ang plain
    ''' password. Hindi kailanman dumadaan ang plain password sa model na
    ''' ito - hiwalay siyang ipinapasa bilang parameter sa Service
    ''' (SaveAsync / SendTestEmailAsync), at ang Service lang ang
    ''' marunong mag-encrypt at mag-decrypt.
    '''
    ''' EmailSettingsId = 0 = wala pang na-Insert kailanman.
    ''' </summary>
    Public Class EmailSettingsModel
        Public Property EmailSettingsId As Integer

        Public Property SmtpHost As String
        Public Property SmtpPort As Integer = 587
        Public Property SmtpUsername As String
        Public Property SmtpPasswordEncrypted As String
        Public Property UseSsl As Boolean = True

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
