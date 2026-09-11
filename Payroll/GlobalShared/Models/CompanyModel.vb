' File: GlobalShared/Models/CompanyModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Singleton - iisang row lang ito palagi. Ito na rin ang bagong
    ''' "single source of truth" ng Company Code/Name (inalis natin sa
    ''' General, dito na lang mapupunta pag ginawa na natin yun module).
    ''' </summary>
    Public Class CompanyModel
        Public Property CompanyId As Integer
        Public Property CompanyCode As String
        Public Property CompanyName As String
        Public Property Industry As String
        Public Property Country As String
        Public Property PostCode As String
        Public Property TelephoneNo As String
        Public Property FaxNo As String
        Public Property ContactPerson As String
        Public Property ContactPersonPosition As String
        Public Property ContactPersonEmail As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property SECRegistrationNo As String
        Public Property Website As String
        Public Property LogoPath As String
        Public Property IsActive As Boolean = True

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace