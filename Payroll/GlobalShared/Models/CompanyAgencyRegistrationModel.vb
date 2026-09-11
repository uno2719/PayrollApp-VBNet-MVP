' File: GlobalShared/Models/CompanyAgencyRegistrationModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblCompanyAgencyRegistration (SSS/PhilHealth/
    ''' Pag-IBIG/BIR) - magkatulad ang shape ng 4, kaya isang model
    ''' class lang, gaya ng ginawa natin kay LookupModel at
    ''' StatutoryBracketModel.
    ''' </summary>
    Public Class CompanyAgencyRegistrationModel
        Public Property AgencyRegistrationId As Integer
        Public Property AgencyType As String   ' SSS | PHILHEALTH | PAGIBIG | BIR
        Public Property RegistrationNo As String
        Public Property Branch As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property Country As String
        Public Property PostCode As String
        Public Property TelephoneNo As String
        Public Property FaxNo As String
        Public Property ContactPerson As String
        Public Property ContactPersonPosition As String
        Public Property ContactPersonEmail As String
        Public Property PersonInCharge1 As String
        Public Property PersonInCharge1Position As String
        Public Property PersonInCharge1Email As String
        Public Property PersonInCharge2 As String
        Public Property PersonInCharge2Position As String
        Public Property PersonInCharge2Email As String
        Public Property Remarks As String

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace