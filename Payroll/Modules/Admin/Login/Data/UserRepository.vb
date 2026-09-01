Imports Dapper
Imports System.Linq

Namespace Login.Data
    Public Class UserRepository
        Inherits GlobalShared.Base.BaseRepository(Of Models.UserModel)
        Implements IUserRepository

        ' Internal na klase lang para sa pag-map ng GetModuleAccessAsync query -
        ' hindi ito parte ng public Models, panloob na detalye lang ng Repository.
        Private Class ModuleAccessQueryRow
            Public Property ModuleId As Integer
            Public Property ModuleName As String
            Public Property CanView As Boolean
            Public Property CanEdit As Boolean
        End Class

        Public Async Function GetByUsernameAsync(username As String) As Task(Of Models.UserModel) _
            Implements IUserRepository.GetByUsernameAsync

            Dim sql = "
                SELECT u.RecordId, u.Username, u.PasswordHash, u.PasswordSalt,
                       u.IsAdmin, u.EmployeeNo, u.IsActive, u.IsDeleted,
                       u.LastLoginDate, u.FailedLoginCount, u.LockedUntil,
                       u.MustChangePassword,
                       u.CreatedAt, u.CreatedBy, u.UpdatedAt, u.UpdatedBy,
                       e.FirstName, e.LastName
                FROM tblUsers u
                INNER JOIN tblEmployee e ON u.EmployeeNo = e.EmployeeNo
                WHERE u.Username = @Username AND u.IsDeleted = 0"

            Return Await GetSingleAsync(sql, New With {username})

        End Function

        Public Async Function InsertAsync(model As Models.UserModel) As Task(Of Integer) _
            Implements IUserRepository.InsertAsync

            ' NOTE: kasama na si MustChangePassword mula sa model mismo -
            ' kaya kailangan mong i-set ang property na iyon bago tumawag dito
            ' (hal. True kapag auto-generated temp password ang gamit).
            Dim sql = "
                INSERT INTO tblUsers (
                    Username, PasswordHash, PasswordSalt, IsAdmin, EmployeeNo,
                    IsActive, MustChangePassword, CreatedAt, CreatedBy
                ) VALUES (
                    @Username, @PasswordHash, @PasswordSalt, @IsAdmin, @EmployeeNo,
                    1, @MustChangePassword, GETDATE(), @CreatedBy
                );
                SELECT CAST(SCOPE_IDENTITY() AS INT);"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, model)
            End Using

        End Function

        Public Async Function UpdateLastLoginAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserRepository.UpdateLastLoginAsync

            ' Pag successful ang login, i-reset natin yung failed count at lockout
            Dim sql = "
                UPDATE tblUsers SET
                    LastLoginDate = GETDATE(),
                    FailedLoginCount = 0,
                    LockedUntil = NULL
                WHERE RecordId = @RecordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        Public Async Function IncrementFailedLoginAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserRepository.IncrementFailedLoginAsync

            Dim sql = "
                UPDATE tblUsers SET
                    FailedLoginCount = FailedLoginCount + 1
                WHERE RecordId = @RecordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        Public Async Function LockAccountAsync(recordId As Integer, lockUntil As DateTime) As Task(Of Boolean) _
            Implements IUserRepository.LockAccountAsync

            Dim sql = "
                UPDATE tblUsers SET
                    LockedUntil = @lockUntil
                WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId, lockUntil})
            Return rows > 0

        End Function

        Public Async Function ResetPasswordAsync(recordId As Integer, newHash As String, newSalt As String) As Task(Of Boolean) _
            Implements IUserRepository.ResetPasswordAsync

            ' I-reset ang password AT i-unlock ang account sabay-sabay, AT
            ' i-clear ang MustChangePassword flag (kung ito ang unang beses
            ' na palitan ang temp password, tapos na ang "must change" state).
            Dim sql = "
                UPDATE tblUsers SET
                    PasswordHash = @newHash,
                    PasswordSalt = @newSalt,
                    FailedLoginCount = 0,
                    LockedUntil = NULL,
                    IsActive = 1,
                    MustChangePassword = 0
                WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId, newHash, newSalt})
            Return rows > 0

        End Function

        ' ============================================================
        ' MGA BAGONG METHOD - Users Management module
        ' ============================================================

        Public Async Function GetAllUsersAsync(filter As String) As Task(Of List(Of Models.UserModel)) _
            Implements IUserRepository.GetAllUsersAsync

            Dim whereClause As String
            Select Case filter
                Case "Active"
                    whereClause = "AND u.IsActive = 1"
                Case "Inactive"
                    whereClause = "AND u.IsActive = 0"
                Case Else
                    whereClause = ""
            End Select

            Dim sql = $"
                SELECT u.RecordId, u.Username, u.IsAdmin, u.EmployeeNo, u.IsActive,
                       u.LastLoginDate, u.MustChangePassword,
                       e.FirstName, e.LastName
                FROM tblUsers u
                INNER JOIN tblEmployee e ON u.EmployeeNo = e.EmployeeNo
                WHERE u.IsDeleted = 0 {whereClause}
                ORDER BY e.FirstName, e.LastName"

            Using conn = GetConnection()
                Dim results = Await conn.QueryAsync(Of Models.UserModel)(sql)
                Return results.ToList()
            End Using

        End Function

        Public Async Function IsUsernameTakenAsync(username As String) As Task(Of Boolean) _
            Implements IUserRepository.IsUsernameTakenAsync

            Dim sql = "SELECT COUNT(1) FROM tblUsers WHERE Username = @username AND IsDeleted = 0"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {username})
                Return count > 0
            End Using

        End Function

        Public Async Function UpdateIsAdminAsync(recordId As Integer, isAdmin As Boolean) As Task(Of Boolean) _
            Implements IUserRepository.UpdateIsAdminAsync

            Dim sql = "UPDATE tblUsers SET IsAdmin = @isAdmin WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId, isAdmin})
            Return rows > 0

        End Function

        Public Async Function DeactivateAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserRepository.DeactivateAsync

            Dim sql = "UPDATE tblUsers SET IsActive = 0 WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        Public Async Function ActivateAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserRepository.ActivateAsync

            Dim sql = "UPDATE tblUsers SET IsActive = 1 WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        Public Async Function GetAllModulesAsync() As Task(Of List(Of Users.Models.ModuleInfo)) _
            Implements IUserRepository.GetAllModulesAsync

            Dim sql = "
                SELECT RecordId, ModuleCode, ModuleName
                FROM tblModules
                WHERE IsActive = 1
                ORDER BY DisplayOrder"

            Using conn = GetConnection()
                Dim results = Await conn.QueryAsync(Of Users.Models.ModuleInfo)(sql)
                Return results.ToList()
            End Using

        End Function

        Public Async Function GetModuleAccessAsync(userId As Integer) As Task(Of List(Of Users.Models.ModuleAccessItem)) _
            Implements IUserRepository.GetModuleAccessAsync

            ' LEFT JOIN para makita LAHAT ng modules, kahit wala pang access
            ' record ang user dito (ipapakita bilang NoAccess by default).
            Dim sql = "
                SELECT m.RecordId AS ModuleId, m.ModuleName,
                       ISNULL(a.CanView, 0) AS CanView,
                       ISNULL(a.CanEdit, 0) AS CanEdit
                FROM tblModules m
                LEFT JOIN tblUserModuleAccess a
                    ON a.ModuleId = m.RecordId AND a.UserId = @userId
                WHERE m.IsActive = 1
                ORDER BY m.DisplayOrder"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of ModuleAccessQueryRow)(sql, New With {userId})

                Dim result As New List(Of Users.Models.ModuleAccessItem)
                For Each row In rows
                    Dim level = Users.Models.ModuleAccessLevel.NoAccess
                    If row.CanEdit Then
                        level = Users.Models.ModuleAccessLevel.CanEdit
                    ElseIf row.CanView Then
                        level = Users.Models.ModuleAccessLevel.ViewOnly
                    End If

                    result.Add(New Users.Models.ModuleAccessItem With {
                        .ModuleId = row.ModuleId,
                        .ModuleName = row.ModuleName,
                        .AccessLevel = level
                    })
                Next

                Return result
            End Using

        End Function

        Public Async Function SaveModuleAccessAsync(userId As Integer, accessList As List(Of Users.Models.ModuleAccessItem)) As Task(Of Boolean) _
            Implements IUserRepository.SaveModuleAccessAsync

            Using conn = GetConnection()

                ' Simple approach: burahin lahat ng dating access ng user,
                ' i-insert ulit base sa bagong listahan. Mas simple kaysa
                ' mag-diff, at hindi naman malaking table ito (ilang modules lang).
                Dim deleteSql = "DELETE FROM tblUserModuleAccess WHERE UserId = @userId"
                Await conn.ExecuteAsync(deleteSql, New With {userId})

                Dim insertSql = "
                    INSERT INTO tblUserModuleAccess (UserId, ModuleId, CanView, CanEdit, CreatedAt, CreatedBy)
                    VALUES (@UserId, @ModuleId, @CanView, @CanEdit, GETDATE(), @CreatedBy)"

                For Each item In accessList
                    If item.AccessLevel = Users.Models.ModuleAccessLevel.NoAccess Then Continue For

                    Dim canView = True
                    Dim canEdit = (item.AccessLevel = Users.Models.ModuleAccessLevel.CanEdit)

                    Await conn.ExecuteAsync(insertSql, New With {
                        .UserId = userId,
                        .ModuleId = item.ModuleId,
                        .CanView = canView,
                        .CanEdit = canEdit,
                        .CreatedBy = AppSession.CurrentUser
                    })
                Next

            End Using

            Return True

        End Function

        ' Hard soft-delete (IsDeleted=1) - iba ito sa Deactivate (IsActive=0).
        ' Mula rito, tuluyan nang hindi na makikita ang user sa GetAllUsersAsync
        ' at GetByUsernameAsync (parehong may "WHERE IsDeleted = 0" filter na).
        Public Async Function DeleteAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserRepository.DeleteAsync

            Dim sql = "UPDATE tblUsers SET IsDeleted = 1 WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        ' Admin-initiated password reset - kabaligtaran ng self-service
        ' ResetPasswordAsync: dito, MustChangePassword=1 (dapat palitan ng
        ' user ang temp password na ibinigay ng Admin sa unang login niya).
        Public Async Function AdminResetPasswordAsync(recordId As Integer, newHash As String, newSalt As String) As Task(Of Boolean) _
            Implements IUserRepository.AdminResetPasswordAsync

            Dim sql = "
                UPDATE tblUsers SET
                    PasswordHash = @newHash,
                    PasswordSalt = @newSalt,
                    FailedLoginCount = 0,
                    LockedUntil = NULL,
                    IsActive = 1,
                    MustChangePassword = 1
                WHERE RecordId = @recordId"

            Dim rows = Await ExecuteAsync(sql, New With {recordId, newHash, newSalt})
            Return rows > 0

        End Function

    End Class
End Namespace