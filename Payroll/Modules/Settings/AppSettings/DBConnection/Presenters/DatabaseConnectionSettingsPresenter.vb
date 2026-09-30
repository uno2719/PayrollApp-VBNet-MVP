Imports Payroll.GlobalShared.Security

Namespace DBConnection.Presenters

    Public Class DatabaseConnectionSettingsPresenter

        Private ReadOnly _view As Views.IDatabaseConnectionSettingsView
        Private ReadOnly _settingsService As Services.IDatabaseConnectionSettingsService

        Public Sub New(
            view As Views.IDatabaseConnectionSettingsView,
            settingsService As Services.IDatabaseConnectionSettingsService)

            _view = view
            _settingsService = settingsService
        End Sub

        Public Sub LoadSettings()
            Dim settings = _settingsService.Load()

            _view.ServerAddress = settings.ServerAddress
            _view.DatabaseName = settings.DatabaseName
            _view.AuthenticationType = settings.AuthenticationType
            _view.SqlUsername = settings.SqlUsername

            ' Sinadyang HINDI natin ipapakita ulit ang dating password
            ' (kahit na-decrypt) - security best practice, blangko na lang
            ' ang password field hangga't hindi bagong tina-type ni user.
            _view.SqlPassword = String.Empty
        End Sub

        Public Async Function TestConnectionAsync() As Task

            If Not ValidateRequiredFields() Then Return

            Dim settings = BuildSettingsFromView()
            Dim connString = _settingsService.BuildConnectionString(settings, ResolvePasswordForTest())

            Try
                Using conn As New System.Data.SqlClient.SqlConnection(connString)
                    Await conn.OpenAsync()
                End Using

                _view.ShowTestConnectionResult(True, "Successfully connected to the database!")

            Catch ex As Exception
                _view.ShowTestConnectionResult(False, $"Could not connect: {ex.Message}")
            End Try

        End Function

        Public Sub SaveSettings()

            ' Hindi lang UI-level (naka-disable na button) ang gate dito -
            ' 'yon ay convenience lang, hindi enforcement. Ito ang totoong
            ' bantay: kahit paano pa matawag ang SaveSettings() balewala
            ' sa button state, hindi pa rin siya makakapag-save kung
            ' walang CanModify.
            If Not AuthorizationService.CanModify(ModuleCodes.Settings_DatabaseSettings) Then
                _view.ShowError("You do not have permission to modify database connection settings.")
                Return
            End If

            If Not ValidateRequiredFields() Then Return

            Dim settings = BuildSettingsFromView()
            _settingsService.Save(settings, _view.SqlPassword)

            _view.ShowMessage("Database Connection Settings saved.")

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

            ' Windows Authentication: walang username/password na kailangan -
            ' ang Windows account na nagpapatakbo ng app ang gagamitin.
            If _view.AuthenticationType = Models.DbAuthenticationType.WindowsAuthentication Then
                Return True
            End If

            If String.IsNullOrWhiteSpace(_view.SqlUsername) Then
                _view.ShowError("SQL Username is required.")
                Return False
            End If

            ' Blangko ang password field = "huwag palitan ang naka-save".
            ' Pero kung WALA namang naka-save (hal. bagong lipat mula sa
            ' Windows Authentication), kailangan talaga itong i-type.
            If String.IsNullOrEmpty(_view.SqlPassword) AndAlso
               String.IsNullOrEmpty(GetSavedPassword()) Then
                _view.ShowError("SQL Password is required.")
                Return False
            End If

            Return True
        End Function

        Private Function GetSavedPassword() As String
            Return _settingsService.GetDecryptedSqlPassword(_settingsService.Load())
        End Function

        ' Para sa Test Connection: kung blangko ang password field, gamitin
        ' ang naka-save na password (kapareho ng ginagawa ng Save).
        Private Function ResolvePasswordForTest() As String
            If Not String.IsNullOrEmpty(_view.SqlPassword) Then Return _view.SqlPassword
            Return GetSavedPassword()
        End Function

        Private Function BuildSettingsFromView() As Models.DatabaseConnectionSettings
            Dim authType = _view.AuthenticationType

            Return New Models.DatabaseConnectionSettings With {
                .ServerAddress = _view.ServerAddress,
                .DatabaseName = _view.DatabaseName,
                .AuthenticationType = authType,
                .SqlUsername = If(authType = Models.DbAuthenticationType.SqlServerAuthentication,
                                  _view.SqlUsername, String.Empty)
            }
        End Function

    End Class

End Namespace