Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Data
    Public Class LeaveTypeRepository
        Inherits BaseRepository(Of LeaveTypeModel)
        Implements ILeaveTypeRepository

        Private Const TableName As String = "tblLeaveType"

        ' --- READ ---
        Public Async Function GetAllAsync() As Task(Of List(Of LeaveTypeModel)) _
            Implements ILeaveTypeRepository.GetAllAsync

            Dim sql = $"
                SELECT LeaveTypeId AS Id, LeaveTypeCode AS Code, LeaveTypeName AS Name,
                       LeaveCategory AS Category, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {TableName}
                ORDER BY LeaveTypeName"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK ---
        Public Async Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements ILeaveTypeRepository.CodeExistsAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM {TableName}
                WHERE LeaveTypeCode = @Code
                  AND LeaveTypeId <> @ExcludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {code, excludeId})
                Return count > 0
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(item As LeaveTypeModel, userName As String) As Task(Of Integer) _
            Implements ILeaveTypeRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName} (LeaveTypeCode, LeaveTypeName, LeaveCategory, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.LeaveTypeId
                VALUES (@Code, @Name, @Category, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Name, item.Category, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As LeaveTypeModel, userName As String) As Task(Of Boolean) _
            Implements ILeaveTypeRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET LeaveTypeCode = @Code,
                    LeaveTypeName = @Name,
                    LeaveCategory = @Category,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE LeaveTypeId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Name, item.Category, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILeaveTypeRepository.SetActiveStatusAsync

            Dim sql = $"
                UPDATE {TableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE LeaveTypeId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace
