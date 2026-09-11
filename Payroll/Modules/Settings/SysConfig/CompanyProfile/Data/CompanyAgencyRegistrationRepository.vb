' File: Modules/Settings/SysConfig/CompanyProfile/Data/CompanyAgencyRegistrationRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Class CompanyAgencyRegistrationRepository
        Inherits BaseRepository(Of CompanyAgencyRegistrationModel)
        Implements ICompanyAgencyRegistrationRepository

        ' --- READ (by AgencyType - SSS/PHILHEALTH/PAGIBIG/BIR) ---
        Public Async Function GetAsync(agencyType As String) As Task(Of CompanyAgencyRegistrationModel) _
            Implements ICompanyAgencyRegistrationRepository.GetAsync

            If Not CompanyAgencyTypeRegistry.IsAllowed(agencyType) Then
                Throw New ArgumentException($"Invalid company agency type: {agencyType}")
            End If

            Dim sql = "
                SELECT AgencyRegistrationId, AgencyType, RegistrationNo, Branch, Address1, Address2,
                       Address3, Country, PostCode, TelephoneNo, FaxNo, ContactPerson,
                       ContactPersonPosition, ContactPersonEmail, PersonInCharge1, PersonInCharge1Position,
                       PersonInCharge1Email, PersonInCharge2, PersonInCharge2Position, PersonInCharge2Email,
                       Remarks, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM tblCompanyAgencyRegistration
                WHERE AgencyType = @AgencyType"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of CompanyAgencyRegistrationModel)(sql, New With {agencyType})
            End Using
        End Function

        ' --- INSERT --- (unang beses lang na na-configure yung agency na ito)
        Public Async Function InsertAsync(agencyType As String, item As CompanyAgencyRegistrationModel, userName As String) As Task(Of Integer) _
            Implements ICompanyAgencyRegistrationRepository.InsertAsync

            If Not CompanyAgencyTypeRegistry.IsAllowed(agencyType) Then
                Throw New ArgumentException($"Invalid company agency type: {agencyType}")
            End If

            Dim sql = "
                INSERT INTO tblCompanyAgencyRegistration
                    (AgencyType, RegistrationNo, Branch, Address1, Address2, Address3, Country, PostCode,
                     TelephoneNo, FaxNo, ContactPerson, ContactPersonPosition, ContactPersonEmail,
                     PersonInCharge1, PersonInCharge1Position, PersonInCharge1Email,
                     PersonInCharge2, PersonInCharge2Position, PersonInCharge2Email,
                     Remarks, CreatedAt, CreatedBy)
                OUTPUT INSERTED.AgencyRegistrationId
                VALUES
                    (@AgencyType, @RegistrationNo, @Branch, @Address1, @Address2, @Address3, @Country, @PostCode,
                     @TelephoneNo, @FaxNo, @ContactPerson, @ContactPersonPosition, @ContactPersonEmail,
                     @PersonInCharge1, @PersonInCharge1Position, @PersonInCharge1Email,
                     @PersonInCharge2, @PersonInCharge2Position, @PersonInCharge2Email,
                     @Remarks, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    agencyType, item.RegistrationNo, item.Branch, item.Address1, item.Address2, item.Address3,
                    item.Country, item.PostCode, item.TelephoneNo, item.FaxNo, item.ContactPerson,
                    item.ContactPersonPosition, item.ContactPersonEmail, item.PersonInCharge1,
                    item.PersonInCharge1Position, item.PersonInCharge1Email, item.PersonInCharge2,
                    item.PersonInCharge2Position, item.PersonInCharge2Email, item.Remarks, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
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
                    ContactPerson = @ContactPerson,
                    ContactPersonPosition = @ContactPersonPosition,
                    ContactPersonEmail = @ContactPersonEmail,
                    PersonInCharge1 = @PersonInCharge1,
                    PersonInCharge1Position = @PersonInCharge1Position,
                    PersonInCharge1Email = @PersonInCharge1Email,
                    PersonInCharge2 = @PersonInCharge2,
                    PersonInCharge2Position = @PersonInCharge2Position,
                    PersonInCharge2Email = @PersonInCharge2Email,
                    Remarks = @Remarks,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE AgencyRegistrationId = @AgencyRegistrationId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.AgencyRegistrationId, item.RegistrationNo, item.Branch, item.Address1, item.Address2,
                item.Address3, item.Country, item.PostCode, item.TelephoneNo, item.FaxNo, item.ContactPerson,
                item.ContactPersonPosition, item.ContactPersonEmail, item.PersonInCharge1,
                item.PersonInCharge1Position, item.PersonInCharge1Email, item.PersonInCharge2,
                item.PersonInCharge2Position, item.PersonInCharge2Email, item.Remarks, userName
            })
            Return rows > 0
        End Function

    End Class
End Namespace