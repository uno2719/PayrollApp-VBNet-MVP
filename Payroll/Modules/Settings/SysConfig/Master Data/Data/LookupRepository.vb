Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace Lookups.Data
    Public Class LookupRepository
        Inherits BaseRepository(Of LookupModel)
        Implements ILookupRepository

        ' --- READ ---
        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of LookupModel)) _
            Implements ILookupRepository.GetAllAsync

            Dim info = LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT {info.IdColumn}   AS Id,
                       {info.CodeColumn} AS Code,
                       {info.NameColumn} AS Name,
                       IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {tableName}
                ORDER BY {info.NameColumn}"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK (used before Insert/Update) ---
        Public Async Function CodeExistsAsync(tableName As String, code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements ILookupRepository.CodeExistsAsync

            Dim info = LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT COUNT(1)
                FROM {tableName}
                WHERE {info.CodeColumn} = @Code
                  AND {info.IdColumn} <> @ExcludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {code, excludeId})
                Return count > 0
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(tableName As String, item As LookupModel, userName As String) As Task(Of Integer) _
            Implements ILookupRepository.InsertAsync

            Dim info = LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                INSERT INTO {tableName} ({info.CodeColumn}, {info.NameColumn}, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{info.IdColumn}
                VALUES (@Code, @Name, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Name, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(tableName As String, item As LookupModel, userName As String) As Task(Of Boolean) _
            Implements ILookupRepository.UpdateAsync

            Dim info = LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET {info.CodeColumn} = @Code,
                    {info.NameColumn} = @Name,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Name, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        ' Intentionally never a hard DELETE — these tables are FK-referenced
        ' by existing Employee records. Deactivating removes them from the
        ' Employee dropdown (WHERE IsActive = 1) without breaking history.
        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILookupRepository.SetActiveStatusAsync

            Dim info = LookupTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace