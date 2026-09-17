' File: Modules/Settings/AppSettings/EmailSettings/Views/ucEmailSettings.vb
Imports Payroll.EmailSettings.Presenters
Imports Payroll.EmailSettings.Views

Public Class ucEmailSettings
    Implements IEmailSettingsView, IAsyncLoadable

    Private _presenter As EmailSettingsPresenter

    Public Sub SetPresenter(presenter As EmailSettingsPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Email Settings"
        End Get
    End Property

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Application Settings > Email Settings"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _presenter.LoadAsync()
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    Private Async Sub btnSendTest_Click(sender As Object, e As EventArgs) Handles btnSendTest.Click
        ' Pwedeng ilang segundo bago sumagot ang SMTP server - i-disable
        ' muna ang button para hindi makapag-double click at makapagpadala
        ' ng dalawang test message.
        btnSendTest.Enabled = False
        lblTestResult.Text = "Sending..."
        lblTestResult.Appearance.ForeColor = Color.Gray

        Try
            Await _presenter.SendTestEmailAsync()
        Finally
            btnSendTest.Enabled = True
        End Try
    End Sub

    ' === IEmailSettingsView ===

    Public Property SmtpHost As String Implements IEmailSettingsView.SmtpHost
        Get
            Return txtSmtpHost.Text.Trim()
        End Get
        Set(value As String)
            txtSmtpHost.Text = value
        End Set
    End Property

    Public Property SmtpPort As Integer Implements IEmailSettingsView.SmtpPort
        Get
            Dim parsed As Integer
            Return If(Integer.TryParse(txtSmtpPort.Text, parsed), parsed, 0)
        End Get
        Set(value As Integer)
            txtSmtpPort.Text = value.ToString()
        End Set
    End Property

    Public Property SmtpUsername As String Implements IEmailSettingsView.SmtpUsername
        Get
            Return txtSmtpUsername.Text.Trim()
        End Get
        Set(value As String)
            txtSmtpUsername.Text = value
        End Set
    End Property

    Public Property SmtpPassword As String Implements IEmailSettingsView.SmtpPassword
        Get
            Return txtSmtpPassword.Text
        End Get
        Set(value As String)
            txtSmtpPassword.Text = value
        End Set
    End Property

    Public Property UseSsl As Boolean Implements IEmailSettingsView.UseSsl
        Get
            Return chkUseSsl.Checked
        End Get
        Set(value As Boolean)
            chkUseSsl.Checked = value
        End Set
    End Property

    Public Property TestRecipient As String Implements IEmailSettingsView.TestRecipient
        Get
            Return txtTestRecipient.Text.Trim()
        End Get
        Set(value As String)
            txtTestRecipient.Text = value
        End Set
    End Property

    Public Sub ShowPasswordStatus(hasSavedPassword As Boolean) Implements IEmailSettingsView.ShowPasswordStatus
        If hasSavedPassword Then
            lblPasswordHint.Text = "A password is saved. Leave blank to keep it."
            lblPasswordHint.Appearance.ForeColor = Color.Gray
        Else
            lblPasswordHint.Text = "No password saved yet."
            lblPasswordHint.Appearance.ForeColor = Color.Firebrick
        End If
    End Sub

    Public Sub ShowTestResult(success As Boolean, message As String) Implements IEmailSettingsView.ShowTestResult
        lblTestResult.Text = message
        lblTestResult.Appearance.ForeColor = If(success, Color.SeaGreen, Color.Firebrick)
    End Sub

    Public Sub ShowMessage(message As String) Implements IEmailSettingsView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements IEmailSettingsView.ShowError
        MyBase.ShowError(message)
    End Sub

End Class
