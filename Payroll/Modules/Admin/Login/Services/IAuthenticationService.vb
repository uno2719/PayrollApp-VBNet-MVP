Namespace Login.Services

    Public Interface IAuthenticationService
        Function AuthenticateAsync(username As String, password As String) As Task(Of AuthenticationResult)
        Function ChangePasswordAsync(username As String, currentPassword As String, newPassword As String) As Task(Of ChangePasswordResult)
        ' Re-check ng password ng NAKA-LOGIN na user (hal. bago ang permanenteng Delete).
        ' Hindi ito nag-uupdate ng LastLogin; pero binibilang ang maling password
        ' sa parehong lockout rule ng login.
        Function VerifyCurrentUserPasswordAsync(username As String, password As String) As Task(Of PasswordCheckResult)
        Function GetUserPermissionsAsync(userId As Integer) As Task(Of List(Of Payroll.GlobalShared.Security.UserPermission))
    End Interface

    ' Hindi lang True/False ang gusto nating i-return - kailangan din natin ng
    ' error message at yung logged-in user info kapag successful.
    Public Class AuthenticationResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property User As Models.UserModel
    End Class

    Public Class PasswordCheckResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        ''' <summary>True kung wala nang saysay ang pag-retry (locked / inactive / not found).</summary>
        Public Property CloseDialog As Boolean
    End Class

    Public Class ChangePasswordResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class

End Namespace