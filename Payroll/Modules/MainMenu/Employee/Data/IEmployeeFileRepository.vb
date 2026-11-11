' File: Modules/MainMenu/Employee/Data/IEmployeeFileRepository.vb
' Phase 3: database access para sa 201 Files, Photo path, at Files Folder setting.
Namespace Employee.Data

    Public Interface IEmployeeFileRepository

        ''' <summary>tblGeneralSettings.FileStorageRoot (Nothing/blank = hindi pa naka-set).</summary>
        Function GetStorageRootAsync() As Task(Of String)

        Function GetCategoriesAsync() As Task(Of List(Of Models.DocumentCategoryModel))

        Function GetDocumentsAsync(recordId As Integer) As Task(Of List(Of Models.EmployeeDocumentModel))
        Function GetDocumentAsync(documentId As Integer) As Task(Of Models.EmployeeDocumentModel)
        Function InsertDocumentAsync(model As Models.EmployeeDocumentModel) As Task(Of Integer)

        ''' <summary>Soft delete (IsDeleted = 1). Ang aktwal na file ay hindi binubura sa folder.</summary>
        Function SoftDeleteDocumentAsync(documentId As Integer, deletedBy As String) As Task(Of Boolean)

        Function GetPhotoPathAsync(recordId As Integer) As Task(Of String)
        Function SetPhotoPathAsync(recordId As Integer, photoPath As String, updatedBy As String) As Task(Of Boolean)

    End Interface

End Namespace
