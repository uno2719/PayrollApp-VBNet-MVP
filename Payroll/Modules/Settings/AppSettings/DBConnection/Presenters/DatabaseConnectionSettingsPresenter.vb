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
            _view.SqlUsername = settings.SqlUsername

            ' Sinadyang HINDI natin ipapakita ulit ang dating password
            ' (kahit na-decrypt) - security best practice, blangko na lang
            ' ang password field hangga't hindi bagong tina-type ni user.
            _view.SqlPassword = String.Empty
        End Sub

        Public Async Function TestConnectionAsync() As Task

            If Not ValidateRequiredFields() Then Return

            Dim settings = BuildSettingsFromView()
            Dim connString = _settingsService.BuildConnectionString(settings, _view.SqlPassword)

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

            If String.IsNullOrWhiteSpace(_view.SqlUsername) Then
                _view.ShowError("SQL Username is required.")
                Return False
            End If

            Return True
        End Function

        Private Function BuildSettingsFromView() As Models.DatabaseConnectionSettings
            Return New Models.DatabaseConnectionSettings With {
                .ServerAddress = _view.ServerAddress,
                .DatabaseName = _view.DatabaseName,
                .SqlUsername = _view.SqlUsername,
                .AuthenticationType = Models.DbAuthenticationType.SqlServerAuthentication
            }
        End Function

    End Class

End Namespace