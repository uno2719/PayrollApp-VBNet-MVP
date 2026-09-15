' File: GlobalShared/Models/CompanyModel.vb
Namespace GlobalShared.Models

    Public Class CompanyModel
        Public Property CompanyId As Integer
        Public Property CompanyCode As String
        Public Property CompanyName As String
        Public Property Industry As String
        Public Property Country As String
        Public Property PostCode As String
        Public Property TelephoneNo As String
        Public Property FaxNo As String

        ' Contact Person - naka-link na sa Employee (FK), hindi na free text
        Public Property ContactPersonRecordId As Integer?
        Public Property ContactPersonName As String            ' display-only, mula sa JOIN
        Public Property ContactPersonPositionName As String    ' display-only, mula sa JOIN
        Public Property ContactPersonEmail As String

        Public Property Address1 As String
        Public Property Address2 As String
        Public Property Address3 As String
        Public Property SECRegistrationNo As String
        Public Property Website As String
        Public Property LogoPath As String
        Public Property IsActive As Boolean = True

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace