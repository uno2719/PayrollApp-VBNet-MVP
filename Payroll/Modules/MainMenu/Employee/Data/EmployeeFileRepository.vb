' File: Modules/MainMenu/Employee/Data/EmployeeFileRepository.vb
Imports Dapper

Namespace Employee.Data

    Public Class EmployeeFileRepository
        Inherits GlobalShared.Base.BaseRepository(Of Models.EmployeeDocumentModel)
        Implements IEmployeeFileRepository

        Public Async Function GetStorageRootAsync() As Task(Of String) _
            Implements IEmployeeFileRepository.GetStorageRootAsync

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of String)(
                    "SELECT TOP 1 FileStorageRoot FROM tblGeneralSettings ORDER BY GeneralSettingsId")
            End Using
        End Function

        Public Async Function GetCategoriesAsync() As Task(Of List(Of Models.DocumentCategoryModel)) _
            Implements IEmployeeFileRepository.GetCategoriesAsync

            Dim sql = "
                SELECT CategoryId, CategoryName, IsRequired, SortOrder, IsActive
                FROM tblDocumentCategory
                WHERE IsActive = 1
                ORDER BY SortOrder, CategoryName"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of Models.DocumentCategoryModel)(sql)
                Return rows.ToList()
            End Using
        End Function

        Private Const DocumentSelect As String = "
                SELECT d.DocumentId, d.RecordId, d.CategoryId, c.CategoryName,
                       d.FileName, d.StoredPath, d.FileSizeBytes, d.Remarks, d.CreatedAt, d.CreatedBy
                FROM tblEmployeeDocument d
                INNER JOIN tblDocumentCategory c ON c.CategoryId = d.CategoryId"

        Public Async Function GetDocumentsAsync(recordId As Integer) As Task(Of List(Of Models.EmployeeDocumentModel)) _
            Implements IEmployeeFileRepository.GetDocumentsAsync

            Dim sql = DocumentSelect & "
                WHERE d.RecordId = @RecordId AND d.IsDeleted = 0
                ORDER BY c.SortOrder, c.CategoryName, d.CreatedAt DESC"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of Models.EmployeeDocumentModel)(sql, New With {.RecordId = recordId})
                Return rows.ToList()
            End Using
        End Function

        Public Async Function GetDocumentAsync(documentId As Integer) As Task(Of Models.EmployeeDocumentModel) _
            Implements IEmployeeFileRepository.GetDocumentAsync

            Dim sql = DocumentSelect & "
                WHERE d.DocumentId = @DocumentId AND d.IsDeleted = 0"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of Models.EmployeeDocumentModel)(
                    sql, New With {.DocumentId = documentId})
            End Using
        End Function

        Public Async Function InsertDocumentAsync(model As Models.EmployeeDocumentModel) As Task(Of Integer) _
            Implements IEmployeeFileRepository.InsertDocumentAsync

            Dim sql = "
                INSERT INTO tblEmployeeDocument
                    (RecordId, CategoryId, FileName, StoredPath, FileSizeBytes, Remarks, CreatedAt, CreatedBy)
                OUTPUT INSERTED.DocumentId
                VALUES
                    (@RecordId, @CategoryId, @FileName, @StoredPath, @FileSizeBytes, @Remarks, GETDATE(), @CreatedBy)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    .RecordId = model.RecordId,
                    .CategoryId = model.CategoryId,
                    .FileName = model.FileName,
                    .StoredPath = model.StoredPath,
                    .FileSizeBytes = model.FileSizeBytes,
                    .Remarks = model.Remarks,
                    .CreatedBy = model.CreatedBy
                })
            End Using
        End Function

        Public Async Function SoftDeleteDocumentAsync(documentId As Integer, deletedBy As String) As Task(Of Boolean) _
            Implements IEmployeeFileRepository.SoftDeleteDocumentAsync

            Dim sql = "
                UPDATE tblEmployeeDocument SET
                    IsDeleted = 1,
                    DeletedAt = GETDATE(),
                    DeletedBy = @DeletedBy
                WHERE DocumentId = @DocumentId AND IsDeleted = 0"

            Using conn = GetConnection()
                Dim rows = Await conn.ExecuteAsync(sql, New With {.DocumentId = documentId, .DeletedBy = deletedBy})
                Return rows > 0
            End Using
        End Function

        Public Async Function GetPhotoPathAsync(recordId As Integer) As Task(Of String) _
            Implements IEmployeeFileRepository.GetPhotoPathAsync

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of String)(
                    "SELECT PhotoPath FROM tblEmployee WHERE RecordId = @RecordId AND IsDeleted = 0",
                    New With {.RecordId = recordId})
            End Using
        End Function

        Public Async Function SetPhotoPathAsync(recordId As Integer, photoPath As String, updatedBy As String) As Task(Of Boolean) _
            Implements IEmployeeFileRepository.SetPhotoPathAsync

            Dim sql = "
                UPDATE tblEmployee SET
                    PhotoPath = @PhotoPath,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UpdatedBy
                WHERE RecordId = @RecordId AND IsDeleted = 0"

            Using conn = GetConnection()
                Dim rows = Await conn.ExecuteAsync(sql, New With {
                    .RecordId = recordId,
                    .PhotoPath = photoPath,
                    .UpdatedBy = updatedBy
                })
                Return rows > 0
            End Using
        End Function

    End Class

End Namespace
