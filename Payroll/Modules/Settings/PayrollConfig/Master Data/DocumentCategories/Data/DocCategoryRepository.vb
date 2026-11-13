Imports Dapper
Imports Payroll.DocumentCategories.Models
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace DocumentCategories.Data
    Public Class DocCategoryRepository
        Inherits BaseRepository(Of DocCategoryModel)
        Implements IDocCategoryRepository

        Private Const TableName As String = "tblDocumentCategory"

        ' --- READ ---
        Public Async Function GetAllAsync() As Task(Of List(Of DocCategoryModel)) _
            Implements IDocCategoryRepository.GetAllAsync

            Dim sql = $"
                SELECT c.CategoryId AS Id, c.CategoryName AS Name, c.IsRequired, c.SortOrder, c.IsActive,
                       (SELECT COUNT(1) FROM tblEmployeeDocument d
                        WHERE d.CategoryId = c.CategoryId AND d.IsDeleted = 0) AS FileCount,
                       c.CreatedAt, c.CreatedBy, c.UpdatedAt, c.UpdatedBy
                FROM {TableName} c
                ORDER BY c.SortOrder, c.CategoryName"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE NAME CHECK ---
        Public Async Function NameExistsAsync(name As String, excludeId As Integer) As Task(Of Boolean) _
            Implements IDocCategoryRepository.NameExistsAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM {TableName}
                WHERE LOWER(LTRIM(RTRIM(CategoryName))) = LOWER(LTRIM(RTRIM(@Name)))
                  AND CategoryId <> @ExcludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {name, excludeId})
                Return count > 0
            End Using
        End Function

        ' Bagong category na walang tinukoy na order: ilalagay sa dulo (hakbang na 10, gaya ng seed).
        Public Async Function NextSortOrderAsync() As Task(Of Integer) _
            Implements IDocCategoryRepository.NextSortOrderAsync

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(
                    $"SELECT ISNULL(MAX(SortOrder), 0) + 10 FROM {TableName}")
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(item As DocCategoryModel, userName As String) As Task(Of Integer) _
            Implements IDocCategoryRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName} (CategoryName, IsRequired, SortOrder, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.CategoryId
                VALUES (@Name, @IsRequired, @SortOrder, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Name, item.IsRequired, item.SortOrder, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As DocCategoryModel, userName As String) As Task(Of Boolean) _
            Implements IDocCategoryRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET CategoryName = @Name,
                    IsRequired = @IsRequired,
                    SortOrder = @SortOrder,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE CategoryId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Name, item.IsRequired, item.SortOrder, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- DEACTIVATE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IDocCategoryRepository.SetActiveStatusAsync

            Dim sql = $"
                UPDATE {TableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE CategoryId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

        ' --- HARD DELETE (admin lang; may in-use check). Kahit ang mga file na "binura" (soft delete) ay nakaturo pa
        ' sa category (may foreign key), kaya kasama sila sa bilang: ang daan ay Deactivate. ---
        Public Async Function DeleteAsync(id As Integer) As Task(Of DeleteResult) _
            Implements IDocCategoryRepository.DeleteAsync

            Dim refs As New List(Of UsageRef) From {
                New UsageRef("employee file(s)", "tblEmployeeDocument", "CategoryId = @Id")
            }

            Return Await RecordDeleter.DeleteAsync(
                "this document category",
                $"DELETE FROM {TableName} WHERE CategoryId = @Id",
                id,
                $"SELECT CategoryName FROM {TableName} WHERE CategoryId = @Id",
                refs)
        End Function

    End Class
End Namespace
