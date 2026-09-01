Namespace Login.Views
    Public Interface IChangePasswordView
        Property CurrentPassword As String
        Property NewPassword As String
        Property ConfirmNewPassword As String

        Sub ShowError(message As String)
        Sub CloseWithSuccess()
    End Interface
End Namespace