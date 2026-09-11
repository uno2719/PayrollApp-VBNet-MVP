' File: GlobalShared/Models/CompanyBankModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblCompanyBank (Bank tab) - totoong CRUD list,
    ''' maaring maraming bank accounts ang company (BDO, BPI, atbp).
    ''' </summary>
    Public Class CompanyBankModel
        Public Property BankId As Integer
        Public Property BankName As String
        Public Property BankCode As String
        Public Property AccountName As String
        Public Property AccountNo As String
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
        Public Property SwiftCode As String
        Public Property BranchNo As String
        Public Property CustomerID As String
        Public Property IsActive As Boolean = True

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace