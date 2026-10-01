Imports Payroll.GlobalShared.Security

Namespace DBConnection.Presenters

    Public Class DatabaseConnectionSettingsPresenter

        Private ReadOnly _view As Views.IDatabaseConnectionSettingsView
        Private ReadOnly _settingsService As Services.IDatabaseConnectionSettingsService
        Private ReadOnly _allowEdit As Boolean

        Public Sub New(
            view As Views.IDatabaseConnectionSettingsView,
            settingsService As Services.IDatabaseConnectionSettingsService,
            Optional allowEdit As Boolean = False)

            _view = view
            _settingsService = settingsService
            _allowEdit = allowEdit
        End Sub

        Public Sub LoadSettings()
            Dim settings = _settingsService.Load()

            _view.ServerAddress = settings.ServerAddress
            _view.DatabaseName = settings.DatabaseName
            _view.AuthenticationType = settings.AuthenticationType
            _view.SqlUsername = settings.SqlUsername

            ' Huwag ipakita ang naka-save na password.
            _view.SqlPassword = String.Empty
        End Sub

        Public Async Function TestConnectionAsync() As Task

            If Not ValidateRequiredFields() Then Return

            Dim settings = BuildSettingsFromView()
            Dim connString = _settingsService.BuildConnectionString(
                settings,
                ResolvePasswordForTest())

            Try
                Using conn As New System.Data.SqlClient.SqlConnection(connString)
                    Await conn.OpenAsync()
                End Using

                _view.ShowTestConnectionResult(
                    True,
                    "Successfully connected to the database!")

            Catch ex As Exception
                _view.ShowTestConnectionResult(
                    False,
                    $"Could not connect: {ex.Message}")
            End Try

        End Function

        Public Sub SaveSettings()

            ' Normal in-app access:
            ' kailangan pa rin ng regular AuthorizationService permission.
            '
            ' PIN-authorized access from frmLogin:
            ' _allowEdit = True, kaya puwedeng mag-save kahit wala pang
            ' logged-in employee/admin session.

            If Not _allowEdit AndAlso
               Not AuthorizationService.CanModify(ModuleCodes.Settings_DatabaseSettings) Then

                _view.ShowError(
                    "You do not have permission to modify database connection settings.")

                Return
            End If

            If Not ValidateRequiredFields() Then Return

            Dim settings = BuildSettingsFromView()

            _settingsService.Save(
                settings,
                _view.SqlPassword)

            _view.ShowMessage(
                "Database Connection Settings saved.")

        End Sub

        Private Function ValidateRequiredFields() As Boolean

            If String.IsNullOrWhiteSpace(_view.ServerAddress) Then
                _view.ShowError("Server Address is required.")
                Return False
            End If

            If String.IsNullOrWhiteSpace(_view.DatabaseName) Then
                _view.ShowError("Database Name is required.")
                Return False
            End If

            If _view.AuthenticationType =
                Models.DbAuthenticationType.WindowsAuthentication Then

                Return True
            End If

            If String.IsNullOrWhiteSpace(_view.SqlUsername) Then
                _view.ShowError("SQL Username is required.")
                Return False
            End If

            If String.IsNullOrEmpty(_view.SqlPassword) AndAlso
               String.IsNullOrEmpty(GetSavedPassword()) Then

                _view.ShowError("SQL Password is required.")
                Return False
            End If

            Return True

        End Function

        Private Function GetSavedPassword() As String
            Return _settingsService.GetDecryptedSqlPassword(
                _settingsService.Load())
        End Function

        Private Function ResolvePasswordForTest() As String

            If Not String.IsNullOrEmpty(_view.SqlPassword) Then
                Return _view.SqlPassword
            End If

            Return GetSavedPassword()

        End Function

        Private Function BuildSettingsFromView() As Models.DatabaseConnectionSettings

            Dim authType = _view.AuthenticationType

            Return New Models.DatabaseConnectionSettings With {
                .ServerAddress = _view.ServerAddress,
                .DatabaseName = _view.DatabaseName,
                .AuthenticationType = authType,
                .SqlUsername = If(
                    authType = Models.DbAuthenticationType.SqlServerAuthentication,
                    _view.SqlUsername,
                    String.Empty)
            }

        End Function

    End Class

End Namespace