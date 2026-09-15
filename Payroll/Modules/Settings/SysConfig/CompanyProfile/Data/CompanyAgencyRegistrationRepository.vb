' File: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyAgencyRegistrationRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Class CompanyAgencyRegistrationRepository
        Inherits BaseRepository(Of CompanyAgencyRegistrationModel)
        Implements ICompanyAgencyRegistrationRepository

        ' UPDATE: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyAgencyRegistrationRepository.vb
        ' (palitan ang buong GetAsync, InsertAsync, UpdateAsync - 3x ang JOIN dahil 3 person-links)

        Public Async Function GetAsync(agencyType As String) As Task(Of CompanyAgencyRegistrationModel) _
                Implements ICompanyAgencyRegistrationRepository.GetAsync

            If Not CompanyAgencyTypeRegistry.IsAllowed(agencyType) Then
                Throw New ArgumentException($"Invalid company agency type: {agencyType}")
            End If

            Dim sql = "
                    SELECT a.AgencyRegistrationId, a.AgencyType, a.RegistrationNo, a.Branch, a.Address1, a.Address2,
                           a.Address3, a.Country, a.PostCode, a.TelephoneNo, a.FaxNo,
                           a.ContactPersonRecordId,
                           ce.FirstName + ' ' + ce.LastName AS ContactPersonName,
                           cp.PositionName AS ContactPersonPositionName,
                           a.ContactPersonEmail,
                           a.PersonInCharge1RecordId,
                           p1e.FirstName + ' ' + p1e.LastName AS PersonInCharge1Name,
                           p1p.PositionName AS PersonInCharge1PositionName,
                           a.PersonInCharge1Email,
                           a.PersonInCharge2RecordId,
                           p2e.FirstName + ' ' + p2e.LastName AS PersonInCharge2Name,
                           p2p.PositionName AS PersonInCharge2PositionName,
                           a.PersonInCharge2Email,
                           a.Remarks, a.CreatedAt, a.CreatedBy, a.UpdatedAt, a.UpdatedBy
                    FROM tblCompanyAgencyRegistration a
                    LEFT JOIN tblEmployee ce ON a.ContactPersonRecordId = ce.RecordId
                    LEFT JOIN tblEmployeeEmployment cee ON ce.RecordId = cee.RecordId
                    LEFT JOIN tblPosition cp ON cee.PositionId = cp.PositionId
                    LEFT JOIN tblEmployee p1e ON a.PersonInCharge1RecordId = p1e.RecordId
                    LEFT JOIN tblEmployeeEmployment p1ee ON p1e.RecordId = p1ee.RecordId
                    LEFT JOIN tblPosition p1p ON p1ee.PositionId = p1p.PositionId
                    LEFT JOIN tblEmployee p2e ON a.PersonInCharge2RecordId = p2e.RecordId
                    LEFT JOIN tblEmployeeEmployment p2ee ON p2e.RecordId = p2ee.RecordId
                    LEFT JOIN tblPosition p2p ON p2ee.PositionId = p2p.PositionId
                    WHERE a.AgencyType = @AgencyType"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of CompanyAgencyRegistrationModel)(sql, New With {agencyType})
            End Using
        End Function

        Public Async Function InsertAsync(agencyType As String, item As CompanyAgencyRegistrationModel, userName As String) As Task(Of Integer) _
                Implements ICompanyAgencyRegistrationRepository.InsertAsync

            If Not CompanyAgencyTypeRegistry.IsAllowed(agencyType) Then
                Throw New ArgumentException($"Invalid company agency type: {agencyType}")
            End If

            Dim sql = "
                    INSERT INTO tblCompanyAgencyRegistration
                        (AgencyType, RegistrationNo, Branch, Address1, Address2, Address3, Country, PostCode,
                         TelephoneNo, FaxNo, ContactPersonRecordId, ContactPersonEmail,
                         PersonInCharge1RecordId, PersonInCharge1Email,
                         PersonInCharge2RecordId, PersonInCharge2Email,
                         Remarks, CreatedAt, CreatedBy)
                    OUTPUT INSERTED.AgencyRegistrationId
                    VALUES
                        (@AgencyType, @RegistrationNo, @Branch, @Address1, @Address2, @Address3, @Country, @PostCode,
                         @TelephoneNo, @FaxNo, @ContactPersonRecordId, @ContactPersonEmail,
                         @PersonInCharge1RecordId, @PersonInCharge1Email,
                         @PersonInCharge2RecordId, @PersonInCharge2Email,
                         @Remarks, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                agencyType, item.RegistrationNo, item.Branch, item.Address1, item.Address2, item.Address3,
                item.Country, item.PostCode, item.TelephoneNo, item.FaxNo,
                item.ContactPersonRecordId, item.ContactPersonEmail,
                item.PersonInCharge1RecordId, item.PersonInCharge1Email,
                item.PersonInCharge2RecordId, item.PersonInCharge2Email, item.Remarks, userName
            })
            End Using
        End Function

        Public Async Function UpdateAsync(item As CompanyAgencyRegistrationModel, userName As String) As Task(Of Boolean) _
                Implements ICompanyAgencyRegistrationRepository.UpdateAsync

            Dim sql = "
                    UPDATE tblCompanyAgencyRegistration
                    SET RegistrationNo = @RegistrationNo,
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
                        Remarks = @Remarks,
                        UpdatedAt = GETDATE(),
                        UpdatedBy = @UserName
                    WHERE AgencyRegistrationId = @AgencyRegistrationId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                    item.AgencyRegistrationId, item.RegistrationNo, item.Branch, item.Address1, item.Address2,
                    item.Address3, item.Country, item.PostCode, item.TelephoneNo, item.FaxNo,
                    item.ContactPersonRecordId, item.ContactPersonEmail,
                    item.PersonInCharge1RecordId, item.PersonInCharge1Email,
                    item.PersonInCharge2RecordId, item.PersonInCharge2Email, item.Remarks, userName
                })
            Return rows > 0
        End Function

    End Class
End Namespace