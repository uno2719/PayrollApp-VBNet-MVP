Imports Payroll.GlobalShared.Security

Namespace DBConnection.Presenters

    Public Class DatabaseConnectionSettingsPresenter

        Private ReadOnly _view As Views.IDatabaseConnectionSettingsView
        Private ReadOnly _settingsService As Services.IDatabaseConnectionSettingsService
        Private ReadOnly _allowEdit As Boolean

        ' Ang Server + Database na naka-save nang buksan ang screen; ikinukumpara sa huling na-save
        ' para malaman kung nagbago ang database na gagamitin ng app.
        Private _openedServer As String = String.Empty
        Private _openedDatabase As String = String.Empty
        Private _savedServer As String = String.Empty
        Private _savedDatabase As String = String.Empty

        ''' <summary>True kapag ang Server o Database Name na na-save ay iba sa dati nang buksan ang screen.</summary>
        Public ReadOnly Property TargetChanged As Boolean
            Get
                Return Not String.Equals(_openedServer, _savedServer, StringComparison.OrdinalIgnoreCase) OrElse
                       Not String.Equals(_openedDatabase, _savedDatabase, StringComparison.OrdinalIgnoreCase)
            End Get
        End Property

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

            _openedServer = If(settings.ServerAddress, String.Empty).Trim()
            _openedDatabase = If(settings.DatabaseName, String.Empty).Trim()
            _savedServer = _openedServer
            _savedDatabase = _openedDatabase

            ' Huwag ipakita ang naka-save na password.
            _view.SqlPassword = String.Empty
        End Sub

        ' Sinusubukang kumonekta sa laman ng screen. Hindi nagpapakita ng message.
        Private Async Function TryConnectAsync() As Task(Of (Success As Boolean, Message As String))
            Dim settings = BuildSettingsFromView()
            Dim connString = _settingsService.BuildConnectionString(
                settings,
                ResolvePasswordForTest())

            ' Maikling timeout para hindi maghintay ng 15 segundo kapag mali ang server.
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder(connString) With {
                .ConnectTimeout = 8
            }

            Try
                Using conn As New System.Data.SqlClient.SqlConnection(builder.ConnectionString)
                    Await conn.OpenAsync()
                End Using

                Return (True, "Successfully connected to the database!")

            Catch ex As Exception
                Return (False, $"Could not connect: {ex.Message}")
            End Try
        End Function

        Public Async Function TestConnectionAsync() As Task

            If Not ValidateRequiredFields() Then Return

            Dim result = Await TryConnectAsync()
            _view.ShowTestConnectionResult(result.Success, result.Message)

        End Function

        ' Sinusubukan muna ang connection bago mag-save. Kung hindi makakonekta, may babala at tinatanong
        ' kung itutuloy pa rin ang pag-save (default: hindi).
        Public Async Function SaveSettingsAsync() As Task

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

            _view.SetBusy(True)
            Try
                Dim result = Await TryConnectAsync()

                If Not result.Success Then
                    _view.ShowTestConnectionResult(False, result.Message)

                    Dim saveAnyway = _view.ConfirmSaveAnyway(
                        "The app could not connect with these settings:" & Environment.NewLine & Environment.NewLine &
                        result.Message & Environment.NewLine & Environment.NewLine &
                        "If you save them, the app may not be able to log in until they are fixed." & Environment.NewLine &
                        "Save anyway?")

                    If Not saveAnyway Then
                        _view.ShowError("Not saved. Fix the settings, or use Test Connection to check them.")
                        Return
                    End If
                End If

                Dim settings = BuildSettingsFromView()

                _settingsService.Save(
                    settings,
                    _view.SqlPassword)

                _savedServer = If(settings.ServerAddress, String.Empty).Trim()
                _savedDatabase = If(settings.DatabaseName, String.Empty).Trim()

                If result.Success Then
                    _view.ShowMessage("Database Connection Settings saved.")
                Else
                    _view.ShowError("Saved, but the connection could not be verified.")
                End If

            Finally
                _view.SetBusy(False)
            End Try

        End Function

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