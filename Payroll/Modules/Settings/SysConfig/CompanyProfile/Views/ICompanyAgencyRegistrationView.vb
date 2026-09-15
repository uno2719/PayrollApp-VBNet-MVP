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

        Property ContactPersonRecordId As Integer?
        Property ContactPersonEmail As String

        Property PersonInCharge1RecordId As Integer?
        Property PersonInCharge1Email As String

        Property PersonInCharge2RecordId As Integer?
        Property PersonInCharge2Email As String

        Property Remarks As String

        Sub SetLabels(registrationNoLabel As String, branchLabel As String, displayName As String)
        Sub SetEmployeeList(employees As List(Of GlobalShared.Models.EmployeeContactLookupModel))
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace