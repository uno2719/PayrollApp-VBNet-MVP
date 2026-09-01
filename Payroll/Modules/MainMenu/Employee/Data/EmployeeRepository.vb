Imports Dapper
Imports System.Data.SqlClient

Namespace Employee.Data
    Public Class EmployeeRepository
        Inherits GlobalShared.Base.BaseRepository(Of Models.EmployeeModel)
        Implements IEmployeeRepository

        ' =============================================
        ' PERSONAL INFO
        ' =============================================

        'Public Overloads Async Function GetAllAsync() As Task(Of List(Of Models.EmployeeModel)) _
        '    Implements IEmployeeRepository.GetAllAsync

        '    Dim sql = "
        '        SELECT RecordId, EmployeeNo,
        '               FirstName, MiddleName, LastName, Suffix,
        '               BirthDate, BirthPlace, Gender, CivilStatus,
        '               Religion, Citizenship, EmailAddress, ContactNo,
        '               MailAddress1, MailAddress2, MailZipCode,
        '               PermanentAddress1, PermanentAddress2, PermanentZipCode,
        '               IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
        '        FROM tblEmployee
        '        ORDER BY LastName, FirstName"

        '    Return Await MyBase.GetAllAsync(sql)

        'End Function

        Public Async Function GetAllRecordsAsync(Optional filter As String = "Active") As Task(Of List(Of Models.EmployeeModel)) _
             Implements IEmployeeRepository.GetAllRecordsAsync

            Dim whereClause As String
            Select Case filter
                Case "Active"
                    whereClause = "WHERE IsActive = 1 AND IsDeleted = 0 "
                Case "Inactive"
                    whereClause = "WHERE IsActive = 0 AND IsDeleted = 0 "
                Case "All"
                    whereClause = "WHERE IsDeleted = 0"
                Case Else
                    whereClause = "WHERE IsActive = 1 AND IsDeleted = 0 "
            End Select

            Dim sql = $"
                        SELECT RecordId, EmployeeNo,
                               FirstName, MiddleName, LastName, Suffix,
                               BirthDate, BirthPlace, Gender, CivilStatus,
                               Religion, Citizenship, EmailAddress, ContactNo,
                               MailAddress1, MailAddress2, MailZipCode,
                               PermanentAddress1, PermanentAddress2, PermanentZipCode,
                               IsActive, IsDeleted,
                               CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                        FROM tblEmployee 
                        {whereClause}
                        ORDER BY LastName, FirstName"

            Return Await GetAllAsync(sql)
        End Function

        Public Async Function GetByIdAsync(recordId As Integer) As Task(Of Models.EmployeeModel) _
            Implements IEmployeeRepository.GetByIdAsync


            Dim sql = "
                        SELECT e.*,
                               p.PositionName
                        FROM tblEmployee e
                        LEFT JOIN tblEmployeeEmployment emp ON e.RecordId = emp.RecordId
                        LEFT JOIN tblPosition p ON emp.PositionId = p.PositionId
                        WHERE e.RecordId = @RecordId"

            Return Await GetSingleAsync(sql, New With {recordId})

        End Function

        Public Async Function InsertAsync(model As Models.EmployeeModel) As Task(Of Integer) _
            Implements IEmployeeRepository.InsertAsync

            Dim sql = "
                INSERT INTO tblEmployee (
                    EmployeeNo, FirstName, MiddleName, LastName, Suffix,
                    BirthDate, BirthPlace, Gender, CivilStatus,
                    Religion, Citizenship, EmailAddress, ContactNo,
                    MailAddress1, MailAddress2, MailZipCode,
                    PermanentAddress1, PermanentAddress2, PermanentZipCode,
                    IsActive, CreatedAt, CreatedBy
                ) VALUES (
                    @EmployeeNo, @FirstName, @MiddleName, @LastName, @Suffix,
                    @BirthDate, @BirthPlace, @Gender, @CivilStatus,
                    @Religion, @Citizenship, @EmailAddress, @ContactNo,
                    @MailAddress1, @MailAddress2, @MailZipCode,
                    @PermanentAddress1, @PermanentAddress2, @PermanentZipCode,
                    @IsActive, GETDATE(), @CreatedBy
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, model)
            End Using

        End Function

        Public Async Function UpdateAsync(model As Models.EmployeeModel) As Task(Of Boolean) _
            Implements IEmployeeRepository.UpdateAsync

            Dim sql = "
                UPDATE tblEmployee SET
                    EmployeeNo          = @EmployeeNo,
                    FirstName           = @FirstName,
                    MiddleName          = @MiddleName,
                    LastName            = @LastName,
                    Suffix              = @Suffix,
                    BirthDate           = @BirthDate,
                    BirthPlace          = @BirthPlace,
                    Gender              = @Gender,
                    CivilStatus         = @CivilStatus,
                    Religion            = @Religion,
                    Citizenship         = @Citizenship,
                    EmailAddress        = @EmailAddress,
                    ContactNo           = @ContactNo,
                    MailAddress1        = @MailAddress1,
                    MailAddress2        = @MailAddress2,
                    MailZipCode         = @MailZipCode,
                    PermanentAddress1   = @PermanentAddress1,
                    PermanentAddress2   = @PermanentAddress2,
                    PermanentZipCode    = @PermanentZipCode,
                    IsActive            = @IsActive,
                    UpdatedAt           = GETDATE(),
                    UpdatedBy           = @UpdatedBy
                WHERE RecordId = @RecordId"

            Dim rows = Await ExecuteAsync(sql, model)
            Return rows > 0

        End Function

        Public Async Function DeleteAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IEmployeeRepository.DeleteAsync

            ' Soft delete lang — hindi physically buburahin
            Dim sql = "
                UPDATE tblEmployee SET
                    IsActive = 0,
                    IsDeleted = 1,
                    DeletedAt = GETDATE(),
                    DeletedBy = @UpdatedBy,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UpdatedBy
                WHERE RecordId = @RecordId"

            Dim rows = Await ExecuteAsync(sql, New With {
                recordId,
                .UpdatedBy = AppSession.CurrentUser
            })
            Return rows > 0

        End Function

        ' =============================================
        ' EMPLOYMENT
        ' =============================================

        Public Async Function GetEmploymentAsync(recordId As Integer) As Task(Of Models.EmployeeEmploymentModel) _
            Implements IEmployeeRepository.GetEmploymentAsync

            Dim sql = "
                SELECT e.*,
                       b.BranchName, d.DepartmentName, p.PositionName,
                       c.CategoryName, j.JobClassName,
                       s.FirstName + ' ' + s.LastName AS SuperiorName,
                       lg.LeaveGroupName, hg.HolidayGroupName, sg.ScheduleGroupName
                FROM tblEmployeeEmployment e
                LEFT JOIN tblBranch b ON e.BranchId = b.BranchId
                LEFT JOIN tblDepartment d ON e.DepartmentId = d.DepartmentId
                LEFT JOIN tblPosition p ON e.PositionId = p.PositionId
                LEFT JOIN tblCategoryCode c ON e.CategoryId = c.CategoryId
                LEFT JOIN tblJobClass j ON e.JobClassId = j.JobClassId
                LEFT JOIN tblEmployee s ON e.SuperiorRecordId = s.RecordId
                LEFT JOIN tblLeaveGroup lg ON e.LeaveGroupId = lg.LeaveGroupId
                LEFT JOIN tblHolidayGroup hg ON e.HolidayGroupId = hg.HolidayGroupId
                LEFT JOIN tblScheduleGroup sg ON e.ScheduleGroupId = sg.ScheduleGroupId
                WHERE e.RecordId = @RecordId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeEmploymentModel)(
                    sql, New With {recordId})
            End Using

        End Function

        Public Async Function SaveEmploymentAsync(model As Models.EmployeeEmploymentModel) As Task(Of Boolean) _
            Implements IEmployeeRepository.SaveEmploymentAsync

            ' UPSERT — Insert if not exists, Update if exists
            Dim sql = "
                IF EXISTS (SELECT 1 FROM tblEmployeeEmployment WHERE RecordId = @RecordId)
                    UPDATE tblEmployeeEmployment SET
                        EmployeeStatus          = @EmployeeStatus,
                        EmploymentType          = @EmploymentType,
                        BranchId                = @BranchId,
                        DepartmentId            = @DepartmentId,
                        PositionId              = @PositionId,
                        CategoryId              = @CategoryId,
                        JobClassId              = @JobClassId,
                        SuperiorRecordId        = @SuperiorRecordId,
                        LeaveGroupId            = @LeaveGroupId,
                        HolidayGroupId          = @HolidayGroupId,
                        ScheduleGroupId         = @ScheduleGroupId,
                        DateJoined              = @DateJoined,
                        DateRegularization      = @DateRegularization,
                        DateLastPromoted        = @DateLastPromoted,
                        DateLastIncremented     = @DateLastIncremented,
                        DateLastTransferred     = @DateLastTransferred,
                        DateResigned            = @DateResigned,
                        DateRetired             = @DateRetired,
                        DateTerminated          = @DateTerminated,
                        TimekeepingControlNo    = @TimekeepingControlNo,
                        UpdatedAt               = GETDATE(),
                        UpdatedBy               = @UpdatedBy
                    WHERE RecordId = @RecordId
                ELSE
                    INSERT INTO tblEmployeeEmployment (
                        RecordId, EmployeeStatus, EmploymentType,
                        BranchId, DepartmentId, PositionId,
                        CategoryId, JobClassId, SuperiorRecordId,
                        LeaveGroupId, HolidayGroupId, ScheduleGroupId,
                        DateJoined, DateRegularization, DateLastPromoted,
                        DateLastIncremented, DateLastTransferred,
                        DateResigned, DateRetired, DateTerminated,
                        TimekeepingControlNo, CreatedAt, CreatedBy
                    ) VALUES (
                        @RecordId, @EmployeeStatus, @EmploymentType,
                        @BranchId, @DepartmentId, @PositionId,
                        @CategoryId, @JobClassId, @SuperiorRecordId,
                        @LeaveGroupId, @HolidayGroupId, @ScheduleGroupId,
                        @DateJoined, @DateRegularization, @DateLastPromoted,
                        @DateLastIncremented, @DateLastTransferred,
                        @DateResigned, @DateRetired, @DateTerminated,
                        @TimekeepingControlNo, GETDATE(), @CreatedBy
                    )"

            Dim rows = Await ExecuteAsync(sql, model)
            Return rows > 0

        End Function

        ' =============================================
        ' EARNINGS
        ' =============================================

        Public Async Function GetEarningsAsync(recordId As Integer) As Task(Of Models.EmployeeEarningsModel) _
            Implements IEmployeeRepository.GetEarningsAsync

            Dim sql = "
                        SELECT e.*, b.BankName
                        FROM tblEmployeeEarnings e
                        LEFT JOIN tblBank b ON e.BankId = b.BankId
                        WHERE e.RecordId = @RecordId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeEarningsModel)(
                    sql, New With {recordId})
            End Using

        End Function

        Public Async Function SaveEarningsAsync(model As Models.EmployeeEarningsModel) As Task(Of Boolean) _
            Implements IEmployeeRepository.SaveEarningsAsync

            Dim sql = "
                IF EXISTS (SELECT 1 FROM tblEmployeeEarnings WHERE RecordId = @RecordId)
                    UPDATE tblEmployeeEarnings SET
                        BasicSalary     = @BasicSalary,
                        DailyRate       = @DailyRate,
                        HourlyRate      = @HourlyRate,
                        DaysInYear      = @DaysInYear,
                        WorkHourPer     = @WorkHourPer,
                        PayrollFlag     = @PayrollFlag,
                        MinimumWage     = @MinimumWage,
                        PayCycle        = @PayCycle,
                        TaxFlag         = @TaxFlag,
                        PayBy           = @PayBy,
                        BankId          = @BankId,
                        BankAccount     = @BankAccount,
                        UpdatedAt       = GETDATE(),
                        UpdatedBy       = @UpdatedBy
                    WHERE RecordId = @RecordId
                ELSE
                    INSERT INTO tblEmployeeEarnings (
                        RecordId, BasicSalary, DailyRate, HourlyRate,
                        DaysInYear, WorkHourPer, PayrollFlag, MinimumWage,
                        PayCycle, TaxFlag, PayBy, BankId, BankAccount,
                        CreatedAt, CreatedBy
                    ) VALUES (
                        @RecordId, @BasicSalary, @DailyRate, @HourlyRate,
                        @DaysInYear, @WorkHourPer, @PayrollFlag, @MinimumWage,
                        @PayCycle, @TaxFlag, @PayBy, @BankId, @BankAccount,
                        GETDATE(), @CreatedBy
                    )"

            Dim rows = Await ExecuteAsync(sql, model)
            Return rows > 0

        End Function

        ' =============================================
        ' STATUTORY
        ' =============================================

        Public Async Function GetStatutoryAsync(recordId As Integer) As Task(Of Models.EmployeeStatutoryModel) _
            Implements IEmployeeRepository.GetStatutoryAsync

            Dim sql = "
                SELECT * FROM tblEmployeeStatutory
                WHERE RecordId = @RecordId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeStatutoryModel)(
                    sql, New With {recordId})
            End Using

        End Function

        Public Async Function SaveStatutoryAsync(model As Models.EmployeeStatutoryModel) As Task(Of Boolean) _
            Implements IEmployeeRepository.SaveStatutoryAsync

            Dim sql = "
                IF EXISTS (SELECT 1 FROM tblEmployeeStatutory WHERE RecordId = @RecordId)
                    UPDATE tblEmployeeStatutory SET
                        TIN                 = @TIN,
                        SSSNo               = @SSSNo,
                        PagIBIGNo           = @PagIBIGNo,
                        PagIBIGVoluntary    = @PagIBIGVoluntary,
                        PhilHealthNo        = @PhilHealthNo,
                        FixTax              = @FixTax,
                        UpdatedAt           = GETDATE(),
                        UpdatedBy           = @UpdatedBy
                    WHERE RecordId = @RecordId
                ELSE
                    INSERT INTO tblEmployeeStatutory (
                        RecordId, TIN, SSSNo, PagIBIGNo,
                        PagIBIGVoluntary, PhilHealthNo, FixTax,
                        CreatedAt, CreatedBy
                    ) VALUES (
                        @RecordId, @TIN, @SSSNo, @PagIBIGNo,
                        @PagIBIGVoluntary, @PhilHealthNo, @FixTax,
                        GETDATE(), @CreatedBy
                    )"

            Dim rows = Await ExecuteAsync(sql, model)
            Return rows > 0

        End Function

        ' =============================================
        ' LOOKUPS
        ' =============================================

        Public Async Function GetLookupsAsync(tableName As String) As Task(Of List(Of GlobalShared.Models.LookupModel)) _
          Implements IEmployeeRepository.GetLookupsAsync

            ' Whitelist
            Dim allowed = New List(Of String) From {
                "tblBranch", "tblDepartment", "tblPosition",
                "tblCategoryCode", "tblJobClass", "tblLeaveGroup",
                "tblHolidayGroup", "tblScheduleGroup", "tblBank"
            }

            If Not allowed.Contains(tableName) Then
                Throw New ArgumentException($"Invalid table: {tableName}")
            End If

            ' ✅ Manual mapping para sa exact column names
            Dim columnMap As New Dictionary(Of String, String()) From {
                {"tblBranch", New String() {"BranchId", "BranchCode", "BranchName"}},
                {"tblDepartment", New String() {"DepartmentId", "DepartmentCode", "DepartmentName"}},
                {"tblPosition", New String() {"PositionId", "PositionCode", "PositionName"}},
                {"tblCategoryCode", New String() {"CategoryId", "CategoryCode", "CategoryName"}},
                {"tblJobClass", New String() {"JobClassId", "JobClassCode", "JobClassName"}},
                {"tblLeaveGroup", New String() {"LeaveGroupId", "LeaveGroupCode", "LeaveGroupName"}},
                {"tblHolidayGroup", New String() {"HolidayGroupId", "HolidayGroupCode", "HolidayGroupName"}},
                {"tblScheduleGroup", New String() {"ScheduleGroupId", "ScheduleGroupCode", "ScheduleGroupName"}},
                {"tblBank", New String() {"BankId", "BankCode", "BankName"}}
            }

            Dim cols = columnMap(tableName)
            Dim sql = $"
                SELECT {cols(0)} AS Id,
                       {cols(1)} AS Code,
                       {cols(2)} AS Name
                FROM {tableName}
                WHERE IsActive = 1
                ORDER BY {cols(2)}"

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of GlobalShared.Models.LookupModel)(sql)
                Return result.ToList()
            End Using

        End Function

        Public Async Function GetEmployeeLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeLookupModel)) _
            Implements IEmployeeRepository.GetEmployeeLookupAsync

            Dim sql = "
        SELECT RecordId,
               EmployeeNo,
               FirstName + ' ' + LastName AS FullName
        FROM tblEmployee
        WHERE IsActive = 1
        ORDER BY LastName, FirstName"

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of GlobalShared.Models.EmployeeLookupModel)(sql)
                Return result.ToList()
            End Using
        End Function

    End Class
End Namespace