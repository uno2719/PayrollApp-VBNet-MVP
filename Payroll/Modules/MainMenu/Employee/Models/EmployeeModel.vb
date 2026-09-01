Namespace Employee.Models
    Public Class EmployeeModel ' <-- Personal Info Model
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property LastName As String
        Public Property Suffix As String
        Public Property BirthDate As Date?
        Public Property BirthPlace As String
        Public Property Gender As String
        Public Property CivilStatus As String
        Public Property Religion As String
        Public Property Citizenship As String
        Public Property EmailAddress As String
        Public Property ContactNo As String
        Public Property MailAddress1 As String
        Public Property MailAddress2 As String
        Public Property MailZipCode As String
        Public Property PermanentAddress1 As String
        Public Property PermanentAddress2 As String
        Public Property PermanentZipCode As String
        Public Property IsActive As Boolean
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        Public Property IsDeleted As Boolean
        Public Property DeletedAt As DateTime?
        Public Property DeletedBy As String

        Public Property PositionName As String

        ' Computed — para sa display sa header ng form
        Public ReadOnly Property FullName As String
            Get
                Return $"{FirstName} {MiddleName} {LastName}".Trim()
            End Get
        End Property
    End Class
End Namespace