' File: Modules/Settings/SysConfig/CompanyProfile/Views/ICompanyView.vb
Namespace CompanyProfile.Views
    Public Interface ICompanyView
        Property CompanyCode As String
        Property CompanyName As String
        Property Industry As String
        Property Country As String
        Property PostCode As String
        Property TelephoneNo As String
        Property FaxNo As String
        Property ContactPerson As String
        Property ContactPersonPosition As String
        Property ContactPersonEmail As String
        Property Address1 As String
        Property Address2 As String
        Property Address3 As String
        Property SECRegistrationNo As String
        Property Website As String
        Property IsActive As Boolean

        Sub ShowLogo(fullPath As String)
        Function PromptForLogoFile() As String   ' Nothing kung Cancel ang pinindot sa OpenFileDialog
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace