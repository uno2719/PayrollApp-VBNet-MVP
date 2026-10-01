Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports Payroll.DBConnection.Presenters
Imports Payroll.GlobalShared.Security

Namespace DBConnection.Views
    Public Class ucDatabaseConnectionSettings
        Implements IDatabaseConnectionSettingsView

        Private _presenter As DatabaseConnectionSettingsPresenter
        Private _isPasswordVisible As Boolean = False

        Public Sub SetPresenter(presenter As DatabaseConnectionSettingsPresenter, Optional allowEdit As Boolean = False)

            _presenter = presenter
            _presenter.LoadSettings()

            btnSave.Enabled = allowEdit OrElse AuthorizationService.CanModify(ModuleCodes.Settings_DatabaseSettings)
        End Sub

        Public Property ServerAddress As String Implements IDatabaseConnectionSettingsView.ServerAddress
            Get
                Return txtServerAddress.Text
            End Get
            Set(value As String)
                txtServerAddress.Text = value
            End Set
        End Property

        Public Property DatabaseName As String Implements IDatabaseConnectionSettingsView.DatabaseName
            Get
                Return txtDatabaseName.Text
            End Get
            Set(value As String)
                txtDatabaseName.Text = value
            End Set
        End Property

        Public Property AuthenticationType As Models.DbAuthenticationType _
            Implements IDatabaseConnectionSettingsView.AuthenticationType
            Get
                Return If(cboAuthType.SelectedIndex = 1,
                          Models.DbAuthenticationType.WindowsAuthentication,
                          Models.DbAuthenticationType.SqlServerAuthentication)
            End Get
            Set(value As Models.DbAuthenticationType)
                cboAuthType.SelectedIndex = If(value = Models.DbAuthenticationType.WindowsAuthentication, 1, 0)
                UpdateAuthFields()
            End Set
        End Property

        Public Property SqlUsername As String Implements IDatabaseConnectionSettingsView.SqlUsername
            Get
                Return txtSqlUsername.Text
            End Get
            Set(value As String)
                txtSqlUsername.Text = value
            End Set
        End Property

        Public Property SqlPassword As String Implements IDatabaseConnectionSettingsView.SqlPassword
            Get
                Return txtSqlPassword.Text
            End Get
            Set(value As String)
                txtSqlPassword.Text = value
            End Set
        End Property

        Public Sub ShowMessage(message As String) Implements IDatabaseConnectionSettingsView.ShowMessage
            lblStatusMessage.Appearance.ForeColor = Color.SeaGreen
            lblStatusMessage.Appearance.Options.UseForeColor = True
            lblStatusMessage.Text = message
        End Sub

        Public Sub ShowError(message As String) Implements IDatabaseConnectionSettingsView.ShowError
            lblStatusMessage.Appearance.ForeColor = Color.Firebrick
            lblStatusMessage.Appearance.Options.UseForeColor = True
            lblStatusMessage.Text = message
        End Sub

        Public Sub ShowTestConnectionResult(success As Boolean, message As String) _
            Implements IDatabaseConnectionSettingsView.ShowTestConnectionResult

            If success Then
                ShowMessage(message)
            Else
                ShowError(message)
            End If
        End Sub

        Private Sub ucDatabaseConnectionSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            If txtSqlPassword.Properties.Buttons.Count > 0 Then
                txtSqlPassword.Properties.Buttons(0).ToolTip = "Show/Hide password"
                txtSqlPassword.Properties.Buttons(0).ImageOptions.Image = My.Resources.icon_hidePW
            End If
        End Sub

        Private Sub txtSqlPassword_ButtonClick(sender As Object, e As ButtonPressedEventArgs) _
            Handles txtSqlPassword.ButtonClick

            _isPasswordVisible = Not _isPasswordVisible
            txtSqlPassword.Properties.PasswordChar = If(_isPasswordVisible, Nothing, ChrW(9679))
            e.Button.ImageOptions.Image = If(_isPasswordVisible, My.Resources.icon_showPW, My.Resources.icon_hidePW)
            txtSqlPassword.Refresh()
        End Sub

        Private Sub cboAuthType_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles cboAuthType.SelectedIndexChanged

            UpdateAuthFields()
        End Sub

        ' Windows Authentication -> i-disable ang Username/Password (hindi
        ' ginagamit). Hindi natin binubura ang tina-type, para kung magbago
        ' ng isip si user at bumalik sa SQL Auth, nandoon pa rin.
        Private Sub UpdateAuthFields()
            Dim isSql = (cboAuthType.SelectedIndex <> 1)

            lblSqlUsername.Enabled = isSql
            txtSqlUsername.Enabled = isSql
            lblSqlPassword.Enabled = isSql
            txtSqlPassword.Enabled = isSql
        End Sub

        Private Async Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
            btnTestConnection.Enabled = False
            lblStatusMessage.Text = "Attempting to connect..."
            Try
                Await _presenter.TestConnectionAsync()
            Finally
                btnTestConnection.Enabled = True
            End Try
        End Sub

        Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
            _presenter.SaveSettings()
        End Sub

    End Class
End Namespace