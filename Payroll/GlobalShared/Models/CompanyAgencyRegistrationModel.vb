' File: GlobalShared/Models/CompanyAgencyRegistrationModel.vb
Namespace GlobalShared.Models

    Public Class CompanyAgencyRegistrationModel
        Public Property AgencyRegistrationId As Integer
        Public Property AgencyType As String
        Public Property RegistrationNo As String
        Public Property Branch As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property Country As String
        Public Property PostCode As String
        Public Property TelephoneNo As String
        Public Property FaxNo As String

        Public Property ContactPersonRecordId As Integer?
        Public Property ContactPersonName As String
        Public Property ContactPersonPositionName As String
        Public Property ContactPersonEmail As String

        Public Property PersonInCharge1RecordId As Integer?
        Public Property PersonInCharge1Name As String
        Public Property PersonInCharge1PositionName As String
        Public Property PersonInCharge1Email As String

        Public Property PersonInCharge2RecordId As Integer?
        Public Property PersonInCharge2Name As String
        Public Property PersonInCharge2PositionName As String
        Public Property PersonInCharge2Email As String

        Public Property Remarks As String

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace