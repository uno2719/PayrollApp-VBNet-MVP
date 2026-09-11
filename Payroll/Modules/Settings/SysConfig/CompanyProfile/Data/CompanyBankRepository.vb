' File: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyBankRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Class CompanyBankRepository
        Inherits BaseRepository(Of CompanyBankModel)
        Implements ICompanyBankRepository

        ' --- READ (list, active-first) ---
        Public Async Function GetAllAsync() As Task(Of List(Of CompanyBankModel)) _
            Implements ICompanyBankRepository.GetAllAsync

            Dim sql = "
                SELECT BankId, BankName, BankCode, AccountName, AccountNo, Branch, Address1, Address2,
                       Address3, Country, PostCode, TelephoneNo, FaxNo, ContactPerson, ContactPersonPosition,
                       ContactPersonEmail, PersonInCharge1, PersonInCharge1Position, PersonInCharge1Email,
                       PersonInCharge2, PersonInCharge2Position, PersonInCharge2Email, SwiftCode, BranchNo,
                       CustomerID, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM tblCompanyBank
                ORDER BY BankName"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(item As CompanyBankModel, userName As String) As Task(Of Integer) _
            Implements ICompanyBankRepository.InsertAsync

            Dim sql = "
                INSERT INTO tblCompanyBank
                    (BankName, BankCode, AccountName, AccountNo, Branch, Address1, Address2, Address3,
                     Country, PostCode, TelephoneNo, FaxNo, ContactPerson, ContactPersonPosition, ContactPersonEmail,
                     PersonInCharge1, PersonInCharge1Position, PersonInCharge1Email,
                     PersonInCharge2, PersonInCharge2Position, PersonInCharge2Email,
                     SwiftCode, BranchNo, CustomerID, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.BankId
                VALUES
                    (@BankName, @BankCode, @AccountName, @AccountNo, @Branch, @Address1, @Address2, @Address3,
                     @Country, @PostCode, @TelephoneNo, @FaxNo, @ContactPerson, @ContactPersonPosition, @ContactPersonEmail,
                     @PersonInCharge1, @PersonInCharge1Position, @PersonInCharge1Email,
                     @PersonInCharge2, @PersonInCharge2Position, @PersonInCharge2Email,
                     @SwiftCode, @BranchNo, @CustomerID, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.BankName, item.BankCode, item.AccountName, item.AccountNo, item.Branch,
                    item.Address1, item.Address2, item.Address3, item.Country, item.PostCode,
                    item.TelephoneNo, item.FaxNo, item.ContactPerson, item.ContactPersonPosition, item.ContactPersonEmail,
                    item.PersonInCharge1, item.PersonInCharge1Position, item.PersonInCharge1Email,
                    item.PersonInCharge2, item.PersonInCharge2Position, item.PersonInCharge2Email,
                    item.SwiftCode, item.BranchNo, item.CustomerID, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As CompanyBankModel, userName As String) As Task(Of Boolean) _
            Implements ICompanyBankRepository.UpdateAsync

            Dim sql = "
                UPDATE tblCompanyBank
                SET BankName = @BankName,
                    BankCode = @BankCode,
                    AccountName = @AccountName,
                    AccountNo = @AccountNo,
                    Branch = @Branch,
                    Address1 = @Address1,
                    Address2 = @Address2,
                    Address3 = @Address3,
                    Country = @Country,
                    PostCode = @PostCode,
                    TelephoneNo = @TelephoneNo,
                    FaxNo = @FaxNo,
                    ContactPerson = @ContactPerson,
                    ContactPersonPosition = @ContactPersonPosition,
                    ContactPersonEmail = @ContactPersonEmail,
                    PersonInCharge1 = @PersonInCharge1,
                    PersonInCharge1Position = @PersonInCharge1Position,
                    PersonInCharge1Email = @PersonInCharge1Email,
                    PersonInCharge2 = @PersonInCharge2,
                    PersonInCharge2Position = @PersonInCharge2Position,
                    PersonInCharge2Email = @PersonInCharge2Email,
                    SwiftCode = @SwiftCode,
                    BranchNo = @BranchNo,
                    CustomerID = @CustomerID,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE BankId = @BankId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.BankId, item.BankName, item.BankCode, item.AccountName, item.AccountNo, item.Branch,
                item.Address1, item.Address2, item.Address3, item.Country, item.PostCode,
                item.TelephoneNo, item.FaxNo, item.ContactPerson, item.ContactPersonPosition, item.ContactPersonEmail,
                item.PersonInCharge1, item.PersonInCharge1Position, item.PersonInCharge1Email,
                item.PersonInCharge2, item.PersonInCharge2Position, item.PersonInCharge2Email,
                item.SwiftCode, item.BranchNo, item.CustomerID, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ICompanyBankRepository.SetActiveStatusAsync

            Dim sql = "
                UPDATE tblCompanyBank
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE BankId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace