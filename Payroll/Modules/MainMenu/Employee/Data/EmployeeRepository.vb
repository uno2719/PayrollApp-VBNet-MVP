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

            ' PAALALA: sinadyang HINDI isinama ang IsActive sa UPDATE na ito. Ang Personal Info save ay
            ' laging nagpapadala ng IsActive = True, kaya mabubuhay ulit ang Inactive/Resigned na employee
            ' tuwing may mage-edit ng pangalan. Ang IsActive ay binabago lang ng SetActiveAsync (at Delete).

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

        Public Async Function GetHeaderAsync(recordId As Integer) As Task(Of Models.EmployeeHeaderModel) _
            Implements IEmployeeRepository.GetHeaderAsync

            ' LEFT JOIN lahat: makita pa rin ang header kahit kulang ang Employment/Earnings setup.
            Dim sql = "
                SELECT e.RecordId, e.EmployeeNo, e.FirstName, e.MiddleName, e.LastName,
                       CAST(ISNULL(e.IsActive, 0) AS BIT) AS IsActive,
                       emp.EmployeeStatus, emp.DateJoined,
                       emp.DateResigned, emp.DateRetired, emp.DateTerminated,
                       p.PositionName, d.DepartmentName, b.BranchName,
                       CAST(CASE WHEN ern.RecordId IS NULL THEN 0 ELSE 1 END AS BIT) AS HasEarnings,
                       ern.PayCycle,
                       CAST(ISNULL(ern.PayrollFlag, 0) AS BIT) AS PayrollFlag,
                       CAST(CASE WHEN e.BirthDate IS NULL THEN 0 ELSE 1 END AS BIT) AS HasBirthDate,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(e.Gender, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasGender,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(e.CivilStatus, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasCivilStatus,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(e.ContactNo, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasContactNo,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(e.EmailAddress, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasEmail,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(e.PermanentAddress1, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasPermanentAddress,
                       CAST(CASE WHEN ISNULL(ern.BasicSalary, 0) > 0 OR ISNULL(ern.DailyRate, 0) > 0 THEN 1 ELSE 0 END AS BIT) AS HasPayRate,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(ern.PayBy, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasPayBy,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(st.TIN, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasTIN,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(st.SSSNo, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasSSSNo,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(st.PagIBIGNo, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasPagIBIGNo,
                       CAST(CASE WHEN LTRIM(RTRIM(ISNULL(st.PhilHealthNo, ''))) <> '' THEN 1 ELSE 0 END AS BIT) AS HasPhilHealthNo
                FROM tblEmployee e
                LEFT JOIN tblEmployeeEmployment emp ON emp.RecordId = e.RecordId
                LEFT JOIN tblPosition p ON p.PositionId = emp.PositionId
                LEFT JOIN tblDepartment d ON d.DepartmentId = emp.DepartmentId
                LEFT JOIN tblBranch b ON b.BranchId = emp.BranchId
                LEFT JOIN tblEmployeeEarnings ern ON ern.RecordId = e.RecordId
                LEFT JOIN tblEmployeeStatutory st ON st.RecordId = e.RecordId
                WHERE e.RecordId = @RecordId"

            Using conn = GetConnection()
                Dim header = Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeHeaderModel)(
                    sql, New With {recordId})

                If header IsNot Nothing Then
                    header.ProbationMonths = Await TryGetProbationMonthsAsync(conn)
                    Await TryFillPhotoAndDocumentsAsync(conn, header)
                    header.OriginalDateJoined = Await TryGetOriginalDateJoinedAsync(conn, recordId)
                End If

                Return header
            End Using

        End Function

        ' Phase 3: photo at required 201 files para sa "Profile %". Kapag wala pa ang mga bagong column/table
        ' (hindi pa na-run ang Phase 3 SQL), tahimik na lalaktawan - hindi dapat masira ang header dahil dito.
        Public Class RequiredDocumentRow
            Public Property CategoryName As String
            Public Property HasDocument As Boolean
        End Class

        Private Shared Async Function TryFillPhotoAndDocumentsAsync(conn As IDbConnection,
                                                                    header As Models.EmployeeHeaderModel) As Task
            Try
                header.HasPhoto = Await conn.ExecuteScalarAsync(Of Boolean)(
                    "SELECT CAST(CASE WHEN LTRIM(RTRIM(ISNULL(PhotoPath, ''))) <> '' THEN 1 ELSE 0 END AS BIT) " &
                    "FROM tblEmployee WHERE RecordId = @RecordId",
                    New With {.RecordId = header.RecordId})
            Catch ex As Exception
                Debug.WriteLine($"PhotoPath not available: {ex.Message}")
                header.HasPhoto = False
            End Try

            Try
                Dim rows = (Await conn.QueryAsync(Of RequiredDocumentRow)("
                    SELECT c.CategoryName,
                           CAST(CASE WHEN EXISTS (SELECT 1 FROM tblEmployeeDocument d
                                                  WHERE d.RecordId = @RecordId AND d.CategoryId = c.CategoryId AND d.IsDeleted = 0)
                                     THEN 1 ELSE 0 END AS BIT) AS HasDocument
                    FROM tblDocumentCategory c
                    WHERE c.IsActive = 1 AND c.IsRequired = 1
                    ORDER BY c.SortOrder, c.CategoryName",
                    New With {.RecordId = header.RecordId})).ToList()

                header.RequiredDocumentCount = rows.Count
                header.MissingRequiredDocuments = rows.Where(Function(r) Not r.HasDocument).
                                                       Select(Function(r) r.CategoryName).ToList()
                header.DocumentsKnown = True
            Catch ex As Exception
                Debug.WriteLine($"Required documents not available: {ex.Message}")
                header.DocumentsKnown = False
            End Try
        End Function

        ' Nothing kapag wala pang column (hindi pa na-run ang Rehire SQL) - hindi dapat masira ang header.
        Private Shared Async Function TryGetOriginalDateJoinedAsync(conn As IDbConnection, recordId As Integer) As Task(Of Date?)
            Try
                Return Await conn.QuerySingleOrDefaultAsync(Of Date?)(
                    "SELECT OriginalDateJoined FROM tblEmployeeEmployment WHERE RecordId = @RecordId",
                    New With {.RecordId = recordId})
            Catch ex As Exception
                Debug.WriteLine($"OriginalDateJoined not available: {ex.Message}")
                Return Nothing
            End Try
        End Function

        ' Galing sa General Settings (tblGeneralSettings.ProbationMonths). Kapag wala pa ang column
        ' (hindi pa na-run ang SQL) o wala pang settings row, Nothing ang ibabalik at ang default ng
        ' presentation ang gagamitin - hindi dapat masira ang buong header dahil dito.
        Private Shared Async Function TryGetProbationMonthsAsync(conn As IDbConnection) As Task(Of Integer?)
            Try
                Return Await conn.QuerySingleOrDefaultAsync(Of Integer?)(
                    "SELECT TOP 1 ProbationMonths FROM tblGeneralSettings ORDER BY GeneralSettingsId")
            Catch ex As Exception
                Debug.WriteLine($"ProbationMonths not available: {ex.Message}")
                Return Nothing
            End Try
        End Function

        Public Async Function SetActiveAsync(recordId As Integer, isActive As Boolean) As Task(Of Boolean) _
            Implements IEmployeeRepository.SetActiveAsync

            Dim sql = "
                UPDATE tblEmployee SET
                    IsActive  = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UpdatedBy
                WHERE RecordId = @RecordId
                  AND IsDeleted = 0"

            ' Phase 2: isang transaction ang pag-update at ang pagsulat sa History (tblEmployeeMovement),
            ' para walang status change na walang bakas.
            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        Dim rows = Await conn.ExecuteAsync(sql, New With {
                            recordId,
                            isActive,
                            .UpdatedBy = AppSession.CurrentUser
                        }, tran)

                        If rows > 0 Then
                            Await InsertMovementAsync(conn, tran, recordId,
                                If(isActive, Models.MovementTypes.Reactivate, Models.MovementTypes.SetInactive),
                                Date.Today,
                                If(isActive, "Inactive", "Active"),
                                If(isActive, "Active", "Inactive"),
                                Nothing, AppSession.CurrentUser)
                        End If

                        tran.Commit()
                        Return rows > 0
                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        End Function

        ' =============================================
        ' MOVEMENT (Phase 2)
        ' =============================================

        Private Shared Async Function InsertMovementAsync(conn As SqlConnection, tran As IDbTransaction,
                                                          recordId As Integer, movementType As String,
                                                          effectiveDate As Date, fromValue As String, toValue As String,
                                                          remarks As String, createdBy As String) As Task

            Dim sql = "
                INSERT INTO tblEmployeeMovement
                    (RecordId, MovementType, EffectiveDate, FromValue, ToValue, Remarks, CreatedAt, CreatedBy)
                VALUES
                    (@RecordId, @MovementType, @EffectiveDate, @FromValue, @ToValue, @Remarks, GETDATE(), @CreatedBy)"

            Await conn.ExecuteAsync(sql, New With {
                .RecordId = recordId,
                .MovementType = movementType,
                .EffectiveDate = effectiveDate.Date,
                .FromValue = fromValue,
                .ToValue = toValue,
                .Remarks = remarks,
                .CreatedBy = createdBy
            }, tran)
        End Function

        Private Shared Function FmtDate(value As Date?) As String
            If Not value.HasValue Then Return "(none)"
            Return value.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        End Function

        Private Shared Function OrNone(value As String) As String
            Return If(String.IsNullOrWhiteSpace(value), "(none)", value.Trim())
        End Function

        Public Async Function ApplyMovementAsync(request As Models.EmployeeMovementRequest) As Task(Of Boolean) _
            Implements IEmployeeRepository.ApplyMovementAsync

            Dim type = request.MovementType
            Dim userName = request.CreatedBy

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        ' 1) Basahin ang KASALUKUYANG kalagayan NANG MAY LOCK, para ang validation at ang update ay
                        '    sa iisang tanawin (walang ibang nag-e-edit sa pagitan).
                        Dim cur = Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeEmploymentModel)(
                            EmploymentSelectSql("WITH (UPDLOCK, ROWLOCK)"),
                            New With {.RecordId = request.RecordId}, tran)

                        If cur Is Nothing Then
                            Throw New InvalidOperationException(
                                "This employee has no Employment record yet. Complete and save the Employment tab first.")
                        End If

                        Dim activeFlag = Await conn.ExecuteScalarAsync(Of Boolean?)(
                            "SELECT CAST(IsActive AS BIT) FROM tblEmployee WITH (UPDLOCK, ROWLOCK) " &
                            "WHERE RecordId = @RecordId AND IsDeleted = 0",
                            New With {.RecordId = request.RecordId}, tran)

                        If Not activeFlag.HasValue Then
                            Throw New InvalidOperationException("The employee record no longer exists.")
                        End If

                        Dim ctx = Models.MovementContext.FromEmployment(cur, activeFlag.Value)
                        Dim problem = Models.EmployeeMovementRules.Validate(request, ctx, Date.Today)
                        If problem.Length > 0 Then Throw New InvalidOperationException(problem)

                        ' 2) Ilapat
                        Dim fromText As String = Nothing
                        Dim toText As String = Nothing
                        Dim effective = request.EffectiveDate.Date

                        Select Case type

                            Case Models.MovementTypes.Regularize
                                fromText = "Employment Type: " & OrNone(cur.EmployeeStatus)
                                toText = "Employment Type: Regular"

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployeeEmployment SET
                                        EmployeeStatus     = 'Regular',
                                        DateRegularization = @EffectiveDate,
                                        UpdatedAt          = GETDATE(),
                                        UpdatedBy          = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {.EffectiveDate = effective, .UserName = userName, .RecordId = request.RecordId}, tran)

                            Case Models.MovementTypes.Promote
                                Dim names = Await GetNewNamesAsync(conn, tran, request)
                                Dim fromParts As New List(Of String)
                                Dim toParts As New List(Of String)

                                If Not Models.EmployeeMovementRules.SameId(cur.PositionId, request.NewPositionId) Then
                                    fromParts.Add("Position: " & OrNone(cur.PositionName))
                                    toParts.Add("Position: " & OrNone(names.PositionName))
                                End If
                                If Not Models.EmployeeMovementRules.SameId(cur.CategoryId, request.NewCategoryId) Then
                                    fromParts.Add("Category: " & OrNone(cur.CategoryName))
                                    toParts.Add("Category: " & OrNone(names.CategoryName))
                                End If
                                If Not Models.EmployeeMovementRules.SameId(cur.JobClassId, request.NewJobClassId) Then
                                    fromParts.Add("Job Class: " & OrNone(cur.JobClassName))
                                    toParts.Add("Job Class: " & OrNone(names.JobClassName))
                                End If
                                fromText = String.Join("; ", fromParts)
                                toText = String.Join("; ", toParts)

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployeeEmployment SET
                                        PositionId       = @PositionId,
                                        CategoryId       = @CategoryId,
                                        JobClassId       = @JobClassId,
                                        DateLastPromoted = @EffectiveDate,
                                        UpdatedAt        = GETDATE(),
                                        UpdatedBy        = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {
                                        .PositionId = request.NewPositionId,
                                        .CategoryId = request.NewCategoryId,
                                        .JobClassId = request.NewJobClassId,
                                        .EffectiveDate = effective,
                                        .UserName = userName,
                                        .RecordId = request.RecordId
                                    }, tran)

                            Case Models.MovementTypes.Transfer
                                Dim names = Await GetNewNamesAsync(conn, tran, request)
                                Dim fromParts As New List(Of String)
                                Dim toParts As New List(Of String)

                                If Not Models.EmployeeMovementRules.SameId(cur.BranchId, request.NewBranchId) Then
                                    fromParts.Add("Branch: " & OrNone(cur.BranchName))
                                    toParts.Add("Branch: " & OrNone(names.BranchName))
                                End If
                                If Not Models.EmployeeMovementRules.SameId(cur.DepartmentId, request.NewDepartmentId) Then
                                    fromParts.Add("Department: " & OrNone(cur.DepartmentName))
                                    toParts.Add("Department: " & OrNone(names.DepartmentName))
                                End If
                                fromText = String.Join("; ", fromParts)
                                toText = String.Join("; ", toParts)

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployeeEmployment SET
                                        BranchId            = @BranchId,
                                        DepartmentId        = @DepartmentId,
                                        DateLastTransferred = @EffectiveDate,
                                        UpdatedAt           = GETDATE(),
                                        UpdatedBy           = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {
                                        .BranchId = request.NewBranchId,
                                        .DepartmentId = request.NewDepartmentId,
                                        .EffectiveDate = effective,
                                        .UserName = userName,
                                        .RecordId = request.RecordId
                                    }, tran)

                            Case Models.MovementTypes.Resign, Models.MovementTypes.Terminate, Models.MovementTypes.Retire
                                ' Ang lifecycle ay DERIVED (IsActive + mga petsa): petsa ng pag-alis + IsActive = 0.
                                ' HINDI ginagalaw ang Employment Type (Regular/Probationary...).
                                Dim dateColumn As String
                                Select Case type
                                    Case Models.MovementTypes.Resign : dateColumn = "DateResigned"
                                    Case Models.MovementTypes.Terminate : dateColumn = "DateTerminated"
                                    Case Else : dateColumn = "DateRetired"
                                End Select

                                fromText = If(activeFlag.Value, "Active", "Inactive")
                                toText = Models.MovementTypes.HistoryLabel(type)

                                Await conn.ExecuteAsync($"
                                    UPDATE tblEmployeeEmployment SET
                                        {dateColumn} = @EffectiveDate,
                                        UpdatedAt    = GETDATE(),
                                        UpdatedBy    = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {.EffectiveDate = effective, .UserName = userName, .RecordId = request.RecordId}, tran)

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployee SET
                                        IsActive  = 0,
                                        UpdatedAt = GETDATE(),
                                        UpdatedBy = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {.UserName = userName, .RecordId = request.RecordId}, tran)

                            Case Models.MovementTypes.Rehire
                                ' Bagong panahon ng serbisyo: ang rehire date ang bagong Date Joined (batayan ng retirement),
                                ' ang UNANG Date Joined ay itinatabi sa OriginalDateJoined (isang beses lang), nabubura ang
                                ' mga petsa ng pag-alis sa employment row (nananatili sa History), at Active na ulit.
                                Dim lastDate = Models.EmployeeMovementRules.LastSeparationDate(ctx)
                                Dim lastKind = Models.EmployeeMovementRules.LastSeparationKind(ctx)
                                Dim empType = Models.EmployeeMovementRules.CanonicalEmploymentType(request.NewEmploymentType)

                                fromText = $"Joined {FmtDate(cur.DateJoined)}; " &
                                           $"{If(lastKind.Length = 0, "Separated", lastKind)} {FmtDate(lastDate)}; " &
                                           $"Employment Type: {OrNone(cur.EmployeeStatus)}"
                                toText = $"Joined {FmtDate(effective)}; Active; Employment Type: {empType}"

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployeeEmployment SET
                                        OriginalDateJoined = ISNULL(OriginalDateJoined, DateJoined),
                                        DateJoined         = @EffectiveDate,
                                        DateResigned       = NULL,
                                        DateRetired        = NULL,
                                        DateTerminated     = NULL,
                                        EmployeeStatus     = @EmploymentType,
                                        DateRegularization = CASE WHEN @EmploymentType = 'Regular' THEN @EffectiveDate ELSE NULL END,
                                        UpdatedAt          = GETDATE(),
                                        UpdatedBy          = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {.EffectiveDate = effective, .EmploymentType = empType,
                                              .UserName = userName, .RecordId = request.RecordId}, tran)

                                Await conn.ExecuteAsync("
                                    UPDATE tblEmployee SET
                                        IsActive  = 1,
                                        UpdatedAt = GETDATE(),
                                        UpdatedBy = @UserName
                                    WHERE RecordId = @RecordId",
                                    New With {.UserName = userName, .RecordId = request.RecordId}, tran)

                            Case Else
                                Throw New InvalidOperationException("Unknown action.")
                        End Select

                        ' 3) Isulat sa History
                        Dim remarks As String = If(String.IsNullOrWhiteSpace(request.Remarks), Nothing, request.Remarks.Trim())
                        Await InsertMovementAsync(conn, tran, request.RecordId, type, effective,
                                                  fromText, toText, remarks, userName)

                        tran.Commit()
                        Return True

                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using

        End Function

        ' Mga pangalan ng BAGONG Position/Category/JobClass/Branch/Department para sa snapshot ng History.
        Private Shared Async Function GetNewNamesAsync(conn As SqlConnection, tran As IDbTransaction,
                                                       request As Models.EmployeeMovementRequest) As Task(Of Models.EmployeeEmploymentModel)
            Dim sql = "
                SELECT
                    (SELECT PositionName   FROM tblPosition     WHERE PositionId   = @PositionId)   AS PositionName,
                    (SELECT CategoryName   FROM tblCategoryCode WHERE CategoryId   = @CategoryId)   AS CategoryName,
                    (SELECT JobClassName   FROM tblJobClass     WHERE JobClassId   = @JobClassId)   AS JobClassName,
                    (SELECT BranchName     FROM tblBranch       WHERE BranchId     = @BranchId)     AS BranchName,
                    (SELECT DepartmentName FROM tblDepartment   WHERE DepartmentId = @DepartmentId) AS DepartmentName"

            Return Await conn.QuerySingleAsync(Of Models.EmployeeEmploymentModel)(sql, New With {
                .PositionId = request.NewPositionId,
                .CategoryId = request.NewCategoryId,
                .JobClassId = request.NewJobClassId,
                .BranchId = request.NewBranchId,
                .DepartmentId = request.NewDepartmentId
            }, tran)
        End Function

        Public Async Function GetMovementHistoryAsync(recordId As Integer) As Task(Of List(Of Models.EmployeeMovementModel)) _
            Implements IEmployeeRepository.GetMovementHistoryAsync

            Dim sql = "
                SELECT MovementId, RecordId, MovementType, EffectiveDate,
                       FromValue, ToValue, Remarks, CreatedAt, CreatedBy
                FROM tblEmployeeMovement
                WHERE RecordId = @RecordId
                ORDER BY EffectiveDate DESC, MovementId DESC"

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of Models.EmployeeMovementModel)(sql, New With {.RecordId = recordId})
                Return result.ToList()
            End Using
        End Function

        ' =============================================
        ' EMPLOYMENT
        ' =============================================

        Public Async Function GetEmploymentAsync(recordId As Integer) As Task(Of Models.EmployeeEmploymentModel) _
            Implements IEmployeeRepository.GetEmploymentAsync

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeEmploymentModel)(
                    EmploymentSelectSql(String.Empty), New With {recordId})
            End Using

        End Function

        ' Iisang SELECT para sa pagbasa ng Employment (may mga pangalan ng lookups). Ang lockHint
        ' (hal. "WITH (UPDLOCK, ROWLOCK)") ay para sa pagbasa sa loob ng transaction ng movement.
        Private Shared Function EmploymentSelectSql(lockHint As String) As String
            Return $"
                SELECT e.*,
                       b.BranchName, d.DepartmentName, p.PositionName,
                       c.CategoryName, j.JobClassName,
                       s.FirstName + ' ' + s.LastName AS SuperiorName,
                       lg.LeaveGroupName, hg.HolidayGroupName, sg.ScheduleGroupName
                FROM tblEmployeeEmployment e {lockHint}
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
                        PayCycle, PayBy, BankId, BankAccount,
                        CreatedAt, CreatedBy
                    ) VALUES (
                        @RecordId, @BasicSalary, @DailyRate, @HourlyRate,
                        @DaysInYear, @WorkHourPer, @PayrollFlag, @MinimumWage,
                        @PayCycle, @PayBy, @BankId, @BankAccount,
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

        ' Ang Pay Cycle options ng Earnings tab ay galing dito: ang mga pay cycle na
        ' Active sa Payroll Settings > Pay Cycle.
        Public Async Function GetActivePayCyclesAsync() As Task(Of List(Of String)) _
            Implements IEmployeeRepository.GetActivePayCyclesAsync

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of String)(
                    "SELECT PayCycleType FROM tblPayCycle WHERE IsActive = 1 ORDER BY PayCycleId")
                Return result.ToList()
            End Using
        End Function

        ' Lahat ng pay cycle (pati hindi na Active) para malaman pa rin ang Rate Basis ng employee
        ' na ang naka-save na cycle ay na-deactivate na.
        Public Async Function GetPayCycleInfoAsync() As Task(Of List(Of Models.PayCycleInfoModel)) _
            Implements IEmployeeRepository.GetPayCycleInfoAsync

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of Models.PayCycleInfoModel)(
                    "SELECT PayCycleType, RateBasis, IsActive FROM tblPayCycle ORDER BY PayCycleId")
                Return result.ToList()
            End Using
        End Function

        Public Async Function GetDailyRateMonthlyFactorAsync() As Task(Of Decimal) _
            Implements IEmployeeRepository.GetDailyRateMonthlyFactorAsync

            Using conn = GetConnection()
                Dim value = Await conn.ExecuteScalarAsync(Of Decimal?)(
                    "SELECT TOP 1 DailyRateMonthlyFactor FROM tblGeneralSettings ORDER BY GeneralSettingsId")
                Return If(value.HasValue AndAlso value.Value > 0D, value.Value, 30D)
            End Using
        End Function

        Public Async Function GetLookupsAsync(tableName As String) As Task(Of List(Of GlobalShared.Models.LookupModel)) _
          Implements IEmployeeRepository.GetLookupsAsync

            ' Whitelist + column names now live in one shared place:
            ' GlobalShared.Constants.LookupTableRegistry — also used by
            ' the Settings > Master Data CRUD module, so both readers
            ' always agree on the real column names.
            Dim info = GlobalShared.Constants.LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT {info.IdColumn} AS Id,
                       {info.CodeColumn} AS Code,
                       {info.NameColumn} AS Name
                FROM {tableName}
                WHERE IsActive = 1
                ORDER BY {info.NameColumn}"

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

        Public Async Function GetEmployeeContactLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeContactLookupModel)) _
            Implements IEmployeeRepository.GetEmployeeContactLookupAsync

            Dim sql = "
        SELECT e.RecordId, e.EmployeeNo, e.FirstName + ' ' + e.LastName AS FullName,
               p.PositionName
        FROM tblEmployee e
        LEFT JOIN tblEmployeeEmployment ee ON e.RecordId = ee.RecordId
        LEFT JOIN tblPosition p ON ee.PositionId = p.PositionId
        WHERE e.IsActive = 1
        ORDER BY e.LastName, e.FirstName"

            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of GlobalShared.Models.EmployeeContactLookupModel)(sql)
                Return result.ToList()
            End Using
        End Function

    End Class
End Namespace