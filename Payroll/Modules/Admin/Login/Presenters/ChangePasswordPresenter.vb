Namespace Login.Presenters

    Public Class ChangePasswordPresenter

        Private ReadOnly _view As Views.IChangePasswordView
        Private ReadOnly _authService As Services.IAuthenticationService
        Private ReadOnly _username As String

        Public Sub New(
            view As Views.IChangePasswordView,
            authService As Services.IAuthenticationService,
            username As String)

            _view = view
            _authService = authService
            _username = username
        End Sub

        Public Async Function ChangePasswordAsync() As Task

            If String.IsNullOrWhiteSpace(_view.CurrentPassword) Then
                _view.ShowError("Please enter your current password.")
                Return
            End If

            If String.IsNullOrWhiteSpace(_view.NewPassword) Then
                _view.ShowError("Please enter a new password.")
                Return
            End If

            If _view.NewPassword <> _view.ConfirmNewPassword Then
                _view.ShowError("New password and confirmation do not match.")
                Return
            End If

            Dim result = Await _authService.ChangePasswordAsync(
                _username, _view.CurrentPassword, _view.NewPassword)

            If result.Success Then
                _view.CloseWithSuccess()
            Else
                _view.ShowError(result.ErrorMessage)
            End If

        End Function

    End Class

End Namespace