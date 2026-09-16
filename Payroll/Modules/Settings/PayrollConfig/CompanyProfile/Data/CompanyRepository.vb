' File: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Class CompanyRepository
        Inherits BaseRepository(Of CompanyModel)
        Implements ICompanyRepository

        ' --- READ ---
        ' Isang row lang palagi ang tblCompany - TOP 1, walang WHERE
        ' kailangan. Babalik ng Nothing kung bagong install pa (wala
        ' pang na-Insert) - ang Service ang bahalang magdesisyon kung
        ' Insert o Update ang tatawagin sa Save.
        ' UPDATE: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyRepository.vb
        ' (palitan ang buong GetAsync, InsertAsync, UpdateAsync)
        Public Async Function GetAsync() As Task(Of CompanyModel) _
             Implements ICompanyRepository.GetAsync

            Dim sql = "
                    SELECT TOP 1 c.CompanyId, c.CompanyCode, c.CompanyName, c.Industry, c.Country,
                           c.PostCode, c.TelephoneNo, c.FaxNo,
                           c.ContactPersonRecordId,
                           ce.FirstName + ' ' + ce.LastName AS ContactPersonName,
                           cp.PositionName AS ContactPersonPositionName,
                           c.ContactPersonEmail,
                           c.Address1, c.Address2, c.Address3, c.SECRegistrationNo,
                           c.Website, c.LogoPath, c.IsActive, c.CreatedAt, c.CreatedBy, c.UpdatedAt, c.UpdatedBy
                    FROM tblCompany c
                    LEFT JOIN tblEmployee ce ON c.ContactPersonRecordId = ce.RecordId
                    LEFT JOIN tblEmployeeEmployment cee ON ce.RecordId = cee.RecordId
                    LEFT JOIN tblPosition cp ON cee.PositionId = cp.PositionId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of CompanyModel)(sql)
            End Using
        End Function

        Public Async Function InsertAsync(item As CompanyModel, userName As String) As Task(Of Integer) _
             Implements ICompanyRepository.InsertAsync

            Dim sql = "
                    INSERT INTO tblCompany
                        (CompanyCode, CompanyName, Industry, Country, PostCode, TelephoneNo, FaxNo,
                         ContactPersonRecordId, ContactPersonEmail, Address1, Address2,
                         Address3, SECRegistrationNo, Website, LogoPath, IsActive, CreatedAt, CreatedBy)
                    OUTPUT INSERTED.CompanyId
                    VALUES
                        (@CompanyCode, @CompanyName, @Industry, @Country, @PostCode, @TelephoneNo, @FaxNo,
                         @ContactPersonRecordId, @ContactPersonEmail, @Address1, @Address2,
                         @Address3, @SECRegistrationNo, @Website, @LogoPath, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
            item.CompanyCode, item.CompanyName, item.Industry, item.Country, item.PostCode,
            item.TelephoneNo, item.FaxNo, item.ContactPersonRecordId, item.ContactPersonEmail,
            item.Address1, item.Address2, item.Address3,
            item.SECRegistrationNo, item.Website, item.LogoPath, item.IsActive, userName
        })
            End Using
        End Function

        Public Async Function UpdateAsync(item As CompanyModel, userName As String) As Task(Of Boolean) _
             Implements ICompanyRepository.UpdateAsync

            Dim sql = "
                    UPDATE tblCompany
                    SET CompanyCode = @CompanyCode,
                        CompanyName = @CompanyName,
                        Industry = @Industry,
                        Country = @Country,
                        PostCode = @PostCode,
                        TelephoneNo = @TelephoneNo,
                        FaxNo = @FaxNo,
                        ContactPersonRecordId = @ContactPersonRecordId,
                        ContactPersonEmail = @ContactPersonEmail,
                        Address1 = @Address1,
                        Address2 = @Address2,
                        Address3 = @Address3,
                        SECRegistrationNo = @SECRegistrationNo,
                        Website = @Website,
                        LogoPath = @LogoPath,
                        IsActive = @IsActive,
                        UpdatedAt = GETDATE(),
                        UpdatedBy = @UserName
                    WHERE CompanyId = @CompanyId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
        item.CompanyId, item.CompanyCode, item.CompanyName, item.Industry, item.Country,
        item.PostCode, item.TelephoneNo, item.FaxNo, item.ContactPersonRecordId, item.ContactPersonEmail,
        item.Address1, item.Address2, item.Address3,
        item.SECRegistrationNo, item.Website, item.LogoPath, item.IsActive, userName
    })
            Return rows > 0
        End Function

    End Class
End Namespace