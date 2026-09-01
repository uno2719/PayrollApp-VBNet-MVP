Namespace Login.Services

    Public Interface IAuthenticationService
        Function AuthenticateAsync(username As String, password As String) As Task(Of AuthenticationResult)
        Function ChangePasswordAsync(username As String, currentPassword As String, newPassword As String) As Task(Of ChangePasswordResult)
    End Interface

    ' Hindi lang True/False ang gusto nating i-return - kailangan din natin ng
    ' error message at yung logged-in user info kapag successful.
    Public Class AuthenticationResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property User As Models.UserModel
    End Class

    Public Class ChangePasswordResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class

End Namespace