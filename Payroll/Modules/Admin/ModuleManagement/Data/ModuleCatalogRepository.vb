' ============================================================
' Modules/Admin/ModuleManagement/Data/ModuleCatalogRepository.vb
' ============================================================
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Data

    Public Class ModuleCatalogRepository
        Inherits BaseRepository(Of ModuleCatalogModel)
        Implements IModuleCatalogRepository

        Private Const TableName As String = "tblModules"

        ' ========================================================
        ' LIST - kasama ang bilang ng user na may access (subquery)
        ' ========================================================
        ' OUTER APPLY (hindi INNER JOIN + GROUP BY) - sinasadya, para
        ' hindi mawala sa listahan ang mga module na WALANG assigned
        ' na user pa (magiging 0 na lang ang count, hindi mawawala
        ' ang buong row).
        ' ========================================================
        Public Async Function GetAllAsync() As Task(Of List(Of ModuleCatalogModel)) _
            Implements IModuleCatalogRepository.GetAllAsync

            Dim sql = $"
                SELECT  m.RecordId, m.ModuleCode, m.ModuleName, m.GroupName,
                        m.SortOrder, m.IsActive,
                        m.CreatedAt, m.CreatedBy, m.UpdatedAt, m.UpdatedBy,
                        ISNULL(cnt.UsersWithAccessCount, 0) AS UsersWithAccessCount
                FROM    {TableName} m
                OUTER APPLY (
                    SELECT COUNT(DISTINCT a.UserId) AS UsersWithAccessCount
                    FROM   tblUserModuleAccess a
                    WHERE  a.ModuleId = m.RecordId
                      AND  (a.CanView = 1 OR a.CanEdit = 1)
                ) cnt
                ORDER BY m.GroupName, m.SortOrder, m.ModuleName"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of ModuleCatalogModel)(sql)
                Return rows.ToList()
            End Using

        End Function

        Public Async Function GetByIdAsync(recordId As Integer) As Task(Of ModuleCatalogModel) _
            Implements IModuleCatalogRepository.GetByIdAsync

            Dim sql = $"
                SELECT  m.RecordId, m.ModuleCode, m.ModuleName, m.GroupName,
                        m.SortOrder, m.IsActive,
                        m.CreatedAt, m.CreatedBy, m.UpdatedAt, m.UpdatedBy,
                        ISNULL(cnt.UsersWithAccessCount, 0) AS UsersWithAccessCount
                FROM    {TableName} m
                OUTER APPLY (
                    SELECT COUNT(DISTINCT a.UserId) AS UsersWithAccessCount
                    FROM   tblUserModuleAccess a
                    WHERE  a.ModuleId = m.RecordId
                      AND  (a.CanView = 1 OR a.CanEdit = 1)
                ) cnt
                WHERE   m.RecordId = @recordId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of ModuleCatalogModel)(sql, New With {recordId})
            End Using

        End Function

        ' ========================================================
        ' DUPLICATE CHECK
        ' ========================================================
        ' excludeRecordId: para sa Edit scenario - pwedeng i-save
        ' ulit ang sarili niyang code nang hindi na-flag bilang
        ' duplicate. Sa New scenario, ipasa mo na lang 0.
        ' ========================================================
        Public Async Function IsModuleCodeTakenAsync(moduleCode As String, excludeRecordId As Integer) As Task(Of Boolean) _
            Implements IModuleCatalogRepository.IsModuleCodeTakenAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM   {TableName}
                WHERE  ModuleCode = @moduleCode
                  AND  RecordId <> @excludeRecordId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {moduleCode, excludeRecordId})
                Return count > 0
            End Using

        End Function

        Public Async Function InsertAsync(item As ModuleCatalogModel, userName As String) As Task(Of Integer) _
            Implements IModuleCatalogRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName}
                    (ModuleCode, ModuleName, GroupName, SortOrder, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.RecordId
                VALUES
                    (@ModuleCode, @ModuleName, @GroupName, @SortOrder, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.ModuleCode, item.ModuleName, item.GroupName, item.SortOrder,
                    item.IsActive, userName
                })
            End Using

        End Function

        ' ========================================================
        ' UPDATE - hindi kasama ang ModuleCode
        ' ========================================================
        ' SINASADYA ITO. Ang ModuleCode ay dapat literal na kapareho
        ' ng Tag sa frmMain.Designer.vb - kapag pinalitan mo ito
        ' pagkatapos na-save na, mapuputol ang koneksyon sa aktwal
        ' na accordion element (mananatiling naka-tag sa lumang code
        ' ang sidebar, pero maghahanap na ng bagong code ang DB row).
        ' Ang UI mismo ay gagawing read-only ang field na ito kapag
        ' Edit mode na - ito lang ang huling saklolo sa DB layer.
        ' ========================================================
        Public Async Function UpdateAsync(item As ModuleCatalogModel, userName As String) As Task(Of Boolean) _
            Implements IModuleCatalogRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET     ModuleName   = @ModuleName,
                        GroupName    = @GroupName,
                        SortOrder = @SortOrder,
                        IsActive     = @IsActive,
                        UpdatedAt    = GETDATE(),
                        UpdatedBy    = @UserName
                WHERE   RecordId = @RecordId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.RecordId, item.ModuleName, item.GroupName,
                item.SortOrder, item.IsActive, userName
            })

            Return rows > 0

        End Function

        Public Async Function DeleteAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IModuleCatalogRepository.DeleteAsync

            Dim sql = $"DELETE FROM {TableName} WHERE RecordId = @recordId"
            Dim rows = Await MyBase.ExecuteAsync(sql, New With {recordId})
            Return rows > 0

        End Function

        Public Async Function SetActiveAsync(recordId As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IModuleCatalogRepository.SetActiveAsync

            Dim sql = $"
                UPDATE {TableName}
                SET     IsActive  = @isActive,
                        UpdatedAt = GETDATE(),
                        UpdatedBy = @userName
                WHERE   RecordId = @recordId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {recordId, isActive, userName})
            Return rows > 0

        End Function

        Public Async Function GetUsersWithAccessCountAsync(recordId As Integer) As Task(Of Integer) _
            Implements IModuleCatalogRepository.GetUsersWithAccessCountAsync

            Dim sql = "
                SELECT COUNT(DISTINCT a.UserId)
                FROM   tblUserModuleAccess a
                WHERE  a.ModuleId = @recordId
                  AND  (a.CanView = 1 OR a.CanEdit = 1)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {recordId})
            End Using

        End Function

    End Class

End Namespace