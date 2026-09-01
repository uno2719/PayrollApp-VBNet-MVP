Namespace Employee.Views
    Public Interface IEmployeesPersonalInfoView
        Inherits IBaseView

        ' --- Identity ---
        Property RecordId As Integer
        Property EmployeeNo As String

        ' --- Name ---
        Property FirstName As String
        Property MiddleName As String
        Property LastName As String
        Property Suffix As String

        ' --- Contact ---
        Property EmailAddress As String
        Property ContactNo As String

        ' --- Personal ---
        Property BirthDate As DateTimeOffset?
        Property BirthPlace As String
        Property Gender As String
        Property CivilStatus As String
        Property Religion As String
        Property Citizenship As String

        ' --- Mail Address ---
        Property MailAddress1 As String
        Property MailAddress2 As String
        Property MailZipCode As String

        ' --- Permanent Address ---
        Property PermanentAddress1 As String
        Property PermanentAddress2 As String
        Property PermanentZipCode As String

        ' --- Clear Fields ---
        Sub ClearFields()

        ' --- Events ---
        Event OnSave As EventHandler
        Event OnDelete As EventHandler
        Event OnNew As EventHandler
        Event OnSaveCompleted As EventHandler

    End Interface
End Namespace