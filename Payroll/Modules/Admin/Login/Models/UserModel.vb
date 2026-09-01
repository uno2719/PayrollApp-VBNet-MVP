Namespace Login.Models
    Public Class UserModel
        Public Property RecordId As Integer
        Public Property Username As String
        Public Property PasswordHash As String
        Public Property PasswordSalt As String
        Public Property IsAdmin As Boolean
        Public Property EmployeeNo As String
        Public Property IsActive As Boolean
        Public Property IsDeleted As Boolean
        Public Property LastLoginDate As DateTime?
        Public Property FailedLoginCount As Integer
        Public Property LockedUntil As DateTime?
        Public Property MustChangePassword As Boolean

        ' NOTE: Hindi ito galing sa tblUsers - JOIN result ito mula sa
        ' tblEmployee, ginagamit lang para sa display purposes (hal. lblCurrentUserDisplayedName)
        Public Property FirstName As String
        Public Property LastName As String

        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
        Public Property DeletedAt As DateTime?
        Public Property DeletedBy As String
    End Class
End Namespace