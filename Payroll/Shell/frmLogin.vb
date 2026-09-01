Imports System.Drawing
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports Payroll.DBConnection.Services
Imports Payroll.Login.Presenters
Imports Payroll.Login.Views

Public Class frmLogin
    Implements ILoginView

    Private _presenter As LoginPresenter
    Private _isPasswordVisible As Boolean = False
    Private _versionClickCount As Integer = 0
    Private _lastVersionClickTime As DateTime = DateTime.MinValue

    Public Sub SetPresenter(presenter As LoginPresenter)
        _presenter = presenter
    End Sub

    Public Property Username As String Implements ILoginView.Username
        Get
            Return txtUsername.Text
        End Get
        Set(value As String)
            txtUsername.Text = value
        End Set
    End Property

    Public Property Password As String Implements ILoginView.Password
        Get
            Return txtPassword.Text
        End Get
        Set(value As String)
            txtPassword.Text = value
        End Set
    End Property

    Public Property RememberMe As Boolean Implements ILoginView.RememberMe
        Get
            Return chkRememberMe.Checked
        End Get
        Set(value As Boolean)
            chkRememberMe.Checked = value
        End Set
    End Property

    Public Sub ShowError(message As String) Implements ILoginView.ShowError
        lblErrorMessage.Text = message
        lblErrorMessage.Visible = True
    End Sub

    Public Sub HideError() Implements ILoginView.HideError
        lblErrorMessage.Visible = False
        lblErrorMessage.Text = ""
    End Sub

    Public Sub CloseWithSuccess() Implements ILoginView.CloseWithSuccess
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Public Sub ForcePasswordChange() Implements ILoginView.ForcePasswordChange
        XtraMessageBox.Show(
            "This is your first login with a temporary password. Please set a new password to continue.",
            "Change Password Required",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)

        Dim changePasswordForm = AppComposition.BuildChangePasswordForm()

        If changePasswordForm.ShowDialog() = DialogResult.OK Then
            ' Successful ang pagpapalit ng password - saka lang tayo
            ' tuluyang papasok sa app.
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
        ' Kung Cancel, mananatili lang tayo sa LoginForm - hindi pa dapat
        ' makapasok ang user hangga't hindi napalitan ang temp password.
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyBrandingPanel()
        ApplyVersionLabel()

        ' Kailangan ito para gumana ang KeyDown kahit may nakafocus na
        ' child control (hal. txtUsername) - kung hindi ito naka-True,
        ' hindi maririnig ng form ang Ctrl+Shift+Alt+S habang nagta-type si user.
        Me.KeyPreview = True

        ' Dito na lang ilagay yung ToolTip ng show/hide button -
        ' ayaw kasi ng WinForms Designer parser ng "Buttons(0).ToolTip = ..."
        ' pattern sa loob mismo ng InitializeComponent.
        If txtPassword.Properties.Buttons.Count > 0 Then
            txtPassword.Properties.Buttons(0).ToolTip = "Show/Hide password"
            txtPassword.Properties.Buttons(0).ImageOptions.Image = My.Resources.icon_hidePW
        End If

        ' I-prefill yung username kung may naka-save na "Remember Me"
        _presenter.LoadSavedPreferences()
    End Sub

    ' Kinukuha sa assembly info ng project ang bersyon - hindi na kailangang
    ' i-update nang manual kada bagong release/build.
    Private Sub ApplyVersionLabel()
        Dim version = My.Application.Info.Version
        lblVersion.Text = $"v{version.Major}.{version.Minor}.{version.Build} — Internal use only"
    End Sub

    ' FIXED na brand color ng left panel - hindi na ito sumusunod sa skin/theme,
    ' dahil lalagyan na ito ng full design/branding image sa hinaharap, kaya
    ' kailangang stable/consistent ang kulay nito anuman ang skin na pinili ni user.
    Private Sub ApplyBrandingPanel()
        Dim brandColor As Color = Color.FromArgb(31, 111, 92) ' #1F6F5C - PACSPORTS sea-green

        panelBranding.Appearance.BackColor = brandColor
        panelBranding.Appearance.Options.UseBackColor = True

        'lblAppName.Appearance.ForeColor = Color.White
        'lblAppName.Appearance.Options.UseForeColor = True

        'lblAppTagline.Appearance.ForeColor = Color.FromArgb(210, 255, 255, 255)
        'lblAppTagline.Appearance.Options.UseForeColor = True
    End Sub

    ' Show/Hide password toggle gamit yung built-in glyph button ng ButtonEdit
    Private Sub txtPassword_ButtonClick(sender As Object, e As ButtonPressedEventArgs) _
        Handles txtPassword.ButtonClick

        _isPasswordVisible = Not _isPasswordVisible
        txtPassword.Properties.PasswordChar = If(_isPasswordVisible, Nothing, ChrW(9679))
        e.Button.ImageOptions.Image = If(_isPasswordVisible, My.Resources.icon_showPW, My.Resources.icon_hidePW)
        txtPassword.Refresh()
    End Sub

    Private Sub lnkForgotPassword_Click(sender As Object, e As EventArgs) Handles lnkForgotPassword.Click
        XtraMessageBox.Show(
            "Please contact your Administrator to reset your password.",
            "Forgot Password",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information)
    End Sub

    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' I-disable muna ang button habang nagve-verify sa DB, para hindi
        ' makapag-double-click si user habang nagpo-process pa.
        btnLogin.Enabled = False
        Try
            Await _presenter.LoginAsync()
        Catch ex As Exception
            ShowError("An unexpected error occurred. Please try again.")
        Finally
            btnLogin.Enabled = True
        End Try
    End Sub

    ' =============================================
    ' SECRET GESTURES - Application Settings Access
    ' =============================================

    ' Gesture 1: 5x click sa version label sa loob ng 2 segundo
    Private Sub lblVersion_Click(sender As Object, e As EventArgs) Handles lblVersion.Click
        Dim now = DateTime.Now

        If (now - _lastVersionClickTime).TotalSeconds > 2 Then
            _versionClickCount = 0
        End If

        _lastVersionClickTime = now
        _versionClickCount += 1

        If _versionClickCount >= 5 Then
            _versionClickCount = 0
            TryOpenSettingsPinPrompt()
        End If
    End Sub

    ' Gesture 2: Ctrl+Shift+Alt+S kahit saan sa form
    Private Sub frmLogin_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.Shift AndAlso e.Alt AndAlso e.KeyCode = Keys.S Then
            TryOpenSettingsPinPrompt()
        End If
    End Sub

    Private Sub TryOpenSettingsPinPrompt()
        Dim pinService As New SettingsPinService()

        If Not pinService.HasPin() Then
            XtraMessageBox.Show(
                "No Settings PIN has been set yet. Please use " &
                "--set-settings-pin from the command line first.",
                "Application Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Using pinPrompt As New frmSettingsPinPrompt()
            If pinPrompt.ShowDialog() = DialogResult.OK Then

                If pinService.VerifyPin(pinPrompt.EnteredPin) Then
                    Dim settingsView = AppComposition.BuildDatabaseConnectionSettingsView()
                    Using dlg As New frmDatabaseConnectionSettingsDialog(settingsView)
                        dlg.ShowDialog()
                    End Using
                Else
                    XtraMessageBox.Show(
                        "Incorrect PIN.",
                        "Application Settings",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
                End If

            End If
        End Using
    End Sub

End Class