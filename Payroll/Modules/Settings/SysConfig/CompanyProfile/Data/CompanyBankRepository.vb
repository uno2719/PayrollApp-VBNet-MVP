' File: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyBankRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Class CompanyBankRepository
        Inherits BaseRepository(Of CompanyBankModel)
        Implements ICompanyBankRepository

        ' UPDATE: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyBankRepository.vb
        ' (parehong pattern - 3x JOIN, palitan ang GetAllAsync, InsertAsync, UpdateAsync)

        Public Async Function GetAllAsync() As Task(Of List(Of CompanyBankModel)) _
                Implements ICompanyBankRepository.GetAllAsync

            Dim sql = "
                    SELECT b.BankId, b.BankName, b.BankCode, b.AccountName, b.AccountNo, b.Branch, b.Address1, b.Address2,
                           b.Address3, b.Country, b.PostCode, b.TelephoneNo, b.FaxNo,
                           b.ContactPersonRecordId,
                           ce.FirstName + ' ' + ce.LastName AS ContactPersonName,
                           cp.PositionName AS ContactPersonPositionName,
                           b.ContactPersonEmail,
                           b.PersonInCharge1RecordId,
                           p1e.FirstName + ' ' + p1e.LastName AS PersonInCharge1Name,
                           p1p.PositionName AS PersonInCharge1PositionName,
                           b.PersonInCharge1Email,
                           b.PersonInCharge2RecordId,
                           p2e.FirstName + ' ' + p2e.LastName AS PersonInCharge2Name,
                           p2p.PositionName AS PersonInCharge2PositionName,
                           b.PersonInCharge2Email,
                           b.SwiftCode, b.BranchNo, b.CustomerID, b.IsActive, b.CreatedAt, b.CreatedBy, b.UpdatedAt, b.UpdatedBy
                    FROM tblCompanyBank b
                    LEFT JOIN tblEmployee ce ON b.ContactPersonRecordId = ce.RecordId
                    LEFT JOIN tblEmployeeEmployment cee ON ce.RecordId = cee.RecordId
                    LEFT JOIN tblPosition cp ON cee.PositionId = cp.PositionId
                    LEFT JOIN tblEmployee p1e ON b.PersonInCharge1RecordId = p1e.RecordId
                    LEFT JOIN tblEmployeeEmployment p1ee ON p1e.RecordId = p1ee.RecordId
                    LEFT JOIN tblPosition p1p ON p1ee.PositionId = p1p.PositionId
                    LEFT JOIN tblEmployee p2e ON b.PersonInCharge2RecordId = p2e.RecordId
                    LEFT JOIN tblEmployeeEmployment p2ee ON p2e.RecordId = p2ee.RecordId
                    LEFT JOIN tblPosition p2p ON p2ee.PositionId = p2p.PositionId
                    ORDER BY b.BankName"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        Public Async Function InsertAsync(item As CompanyBankModel, userName As String) As Task(Of Integer) _
                Implements ICompanyBankRepository.InsertAsync

            Dim sql = "
                    INSERT INTO tblCompanyBank
                        (BankName, BankCode, AccountName, AccountNo, Branch, Address1, Address2, Address3,
                         Country, PostCode, TelephoneNo, FaxNo, ContactPersonRecordId, ContactPersonEmail,
                         PersonInCharge1RecordId, PersonInCharge1Email,
                         PersonInCharge2RecordId, PersonInCharge2Email,
                         SwiftCode, BranchNo, CustomerID, IsActive, CreatedAt, CreatedBy)
                    OUTPUT INSERTED.BankId
                    VALUES
                        (@BankName, @BankCode, @AccountName, @AccountNo, @Branch, @Address1, @Address2, @Address3,
                         @Country, @PostCode, @TelephoneNo, @FaxNo, @ContactPersonRecordId, @ContactPersonEmail,
                         @PersonInCharge1RecordId, @PersonInCharge1Email,
                         @PersonInCharge2RecordId, @PersonInCharge2Email,
                         @SwiftCode, @BranchNo, @CustomerID, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                        item.BankName, item.BankCode, item.AccountName, item.AccountNo, item.Branch,
                        item.Address1, item.Address2, item.Address3, item.Country, item.PostCode,
                        item.TelephoneNo, item.FaxNo, item.ContactPersonRecordId, item.ContactPersonEmail,
                        item.PersonInCharge1RecordId, item.PersonInCharge1Email,
                        item.PersonInCharge2RecordId, item.PersonInCharge2Email,
                        item.SwiftCode, item.BranchNo, item.CustomerID, item.IsActive, userName
                    })
            End Using
        End Function

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
                        ContactPersonRecordId = @ContactPersonRecordId,
                        ContactPersonEmail = @ContactPersonEmail,
                        PersonInCharge1RecordId = @PersonInCharge1RecordId,
                        PersonInCharge1Email = @PersonInCharge1Email,
                        PersonInCharge2RecordId = @PersonInCharge2RecordId,
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
                    item.TelephoneNo, item.FaxNo, item.ContactPersonRecordId, item.ContactPersonEmail,
                    item.PersonInCharge1RecordId, item.PersonInCharge1Email,
                    item.PersonInCharge2RecordId, item.PersonInCharge2Email,
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