' File: Modules/Settings/AppSettings/EmailSettings/Views/IEmailSettingsView.vb
Namespace EmailSettings.Views
    Public Interface IEmailSettingsView

        Property SmtpHost As String
        Property SmtpPort As Integer
        Property SmtpUsername As String

        ''' <summary>
        ''' Plain password galing sa textbox. Blangko ito sa tuwing bagong
        ''' bukas ang screen - sinadya, hindi bug (tingnan ang LoadAsync
        ''' sa Presenter).
        ''' </summary>
        Property SmtpPassword As String

        Property UseSsl As Boolean
        Property TestRecipient As String

        ''' <summary>
        ''' Ipinapakita kung may naka-save nang password o wala, nang hindi
        ''' ipinapakita ang mismong password.
        ''' </summary>
        Sub ShowPasswordStatus(hasSavedPassword As Boolean)

        Sub ShowTestResult(success As Boolean, message As String)

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace
