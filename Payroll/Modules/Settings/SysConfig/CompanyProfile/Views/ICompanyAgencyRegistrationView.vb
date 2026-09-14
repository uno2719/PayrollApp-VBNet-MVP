' File: Modules/Settings/SysConfig/CompanyProfile/Views/ICompanyAgencyRegistrationView.vb
Namespace CompanyProfile.Views
    Public Interface ICompanyAgencyRegistrationView
        Property RegistrationNo As String
        Property Branch As String
        Property Address1 As String
        Property Address2 As String
        Property Address3 As String
        Property Country As String
        Property PostCode As String
        Property TelephoneNo As String
        Property FaxNo As String
        Property ContactPerson As String
        Property ContactPersonPosition As String
        Property ContactPersonEmail As String
        Property PersonInCharge1 As String
        Property PersonInCharge1Position As String
        Property PersonInCharge1Email As String
        Property PersonInCharge2 As String
        Property PersonInCharge2Position As String
        Property PersonInCharge2Email As String
        Property Remarks As String

        ' Palitan ng "SSS No./Branch", "TIN/RDO", atbp. depende sa agency -
        ' tingnan ang CompanyAgencyTypeRegistry.
        Sub SetLabels(registrationNoLabel As String, branchLabel As String, displayName As String)
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace