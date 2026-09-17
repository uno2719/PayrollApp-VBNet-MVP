' File: Modules/Settings/AppSettings/EmailSettings/Presenters/EmailSettingsPresenter.vb
Imports Payroll.GlobalShared.Models

Namespace EmailSettings.Presenters
    Public Class EmailSettingsPresenter

        Private ReadOnly _view As Views.IEmailSettingsView
        Private ReadOnly _service As Services.IEmailSettingsService
        Private ReadOnly _userName As String

        Private _current As EmailSettingsModel

        Public Sub New(
            view As Views.IEmailSettingsView,
            service As Services.IEmailSettingsService,
            userName As String)

            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            _current = Await _service.GetAsync()

            If _current Is Nothing Then
                ' Bagong install - defaults na nasa model ang ipapakita
                ' (port 587, SSL naka-on), at Insert ang mangyayari sa
                ' unang Save.
                _current = New EmailSettingsModel()
            End If

            _view.SmtpHost = If(_current.SmtpHost, String.Empty)
            _view.SmtpPort = _current.SmtpPort
            _view.SmtpUsername = If(_current.SmtpUsername, String.Empty)
            _view.UseSsl = _current.UseSsl

            ' Sinadyang HINDI natin ipinapakita ulit ang naka-save na
            ' password kahit kaya naman itong i-decrypt - kaparehong
            ' patakaran ng DB Connection screen. Blangko ang field
            ' hangga't walang bagong tina-type; sa halip, sinasabi na
            ' lang natin kung may naka-save o wala.
            _view.SmtpPassword = String.Empty
            _view.ShowPasswordStatus(Not String.IsNullOrEmpty(_current.SmtpPasswordEncrypted))
        End Function

        Public Async Function SaveAsync() As Task
            ApplyViewToModel()

            Dim result = Await _service.SaveAsync(_current, _view.SmtpPassword, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            ' I-reload para makuha ang bagong EmailSettingsId pagkatapos
            ' ng unang Insert - kung hindi, mag-i-Insert ulit ang susunod
            ' na Save at magkakaroon ng pangalawang row.
            _current = Await _service.GetAsync()

            _view.SmtpPassword = String.Empty
            _view.ShowPasswordStatus(Not String.IsNullOrEmpty(_current?.SmtpPasswordEncrypted))
            _view.ShowMessage("Email Settings saved.")
        End Function

        Public Async Function SendTestEmailAsync() As Task
            ' Sinusubukan ang settings na NASA SCREEN, hindi ang naka-save -
            ' para matest mo muna bago ka mag-Save ng baguhin.
            ApplyViewToModel()

            Dim result = Await _service.SendTestEmailAsync(
                _current, _view.SmtpPassword, _view.TestRecipient)

            _view.ShowTestResult(result.Success, result.Message)
        End Function

        Private Sub ApplyViewToModel()
            _current.SmtpHost = _view.SmtpHost
            _current.SmtpPort = _view.SmtpPort
            _current.SmtpUsername = _view.SmtpUsername
            _current.UseSsl = _view.UseSsl
            ' Hindi kasama ang password dito - hiwalay siyang ipinapasa sa
            ' Service para hindi kailanman mahawakan ng model ang plain
            ' password.
        End Sub

    End Class
End Namespace
