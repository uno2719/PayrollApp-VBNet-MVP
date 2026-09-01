Namespace Login.Presenters

    Public Class LoginPresenter

        Private ReadOnly _view As Views.ILoginView
        Private ReadOnly _authService As Services.IAuthenticationService
        Private ReadOnly _loginPreferencesService As Services.ILoginPreferencesService

        Public Sub New(
            view As Views.ILoginView,
            authService As Services.IAuthenticationService,
            loginPreferencesService As Services.ILoginPreferencesService)

            _view = view
            _authService = authService
            _loginPreferencesService = loginPreferencesService
        End Sub

        ' Tinatawag ito sa frmLogin_Load - kunin yung na-save na "Remember Me" username
        Public Sub LoadSavedPreferences()
            Dim prefs = _loginPreferencesService.Load()

            If prefs.RememberMe AndAlso Not String.IsNullOrEmpty(prefs.RememberedUsername) Then
                _view.Username = prefs.RememberedUsername
                _view.RememberMe = True
            End If
        End Sub

        Public Async Function LoginAsync() As Task

            _view.HideError()

            If String.IsNullOrWhiteSpace(_view.Username) Then
                _view.ShowError("Please enter your username.")
                Return
            End If

            If String.IsNullOrWhiteSpace(_view.Password) Then
                _view.ShowError("Please enter your password.")
                Return
            End If

            Dim result = Await _authService.AuthenticateAsync(_view.Username, _view.Password)

            If result.Success Then
                AppSession.CurrentUser = result.User.Username
                AppSession.CurrentUserRecordID = result.User.RecordId
                AppSession.IsAdmin = result.User.IsAdmin
                AppSession.EmployeeNo = result.User.EmployeeNo
                AppSession.DisplayName = $"{result.User.FirstName} {result.User.LastName}".Trim()

                ' I-save o i-clear yung Remember Me preference base sa checkbox
                Dim prefs As New Models.LoginPreferences With {
                    .RememberMe = _view.RememberMe,
                    .RememberedUsername = If(_view.RememberMe, _view.Username, String.Empty)
                }
                _loginPreferencesService.Save(prefs)

                If result.User.MustChangePassword Then
                    ' Temporary password pa rin ito - kailangang palitan muna
                    ' bago pumasok sa app.
                    _view.ForcePasswordChange()
                Else
                    _view.CloseWithSuccess()
                End If
            Else
                _view.ShowError(result.ErrorMessage)
            End If

        End Function

    End Class

End Namespace