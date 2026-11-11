' File: Modules/MainMenu/Employee/Services/IEmployeeFileService.vb
Namespace Employee.Services

    Public Interface IEmployeeFileService

        Function GetCategoriesAsync() As Task(Of List(Of Models.DocumentCategoryModel))

        ''' <summary>Kailangan ang View sa "Employee Documents".</summary>
        Function GetDocumentsAsync(recordId As Integer) As Task(Of List(Of Models.EmployeeDocumentModel))

        ''' <summary>Kailangan ang Can Edit. Kokopyahin ang mga file sa Files Folder; ibinabalik kung ilan ang na-save at alin ang hindi.</summary>
        Function AddDocumentsAsync(recordId As Integer, categoryId As Integer, remarks As String,
                                   filePaths As IEnumerable(Of String)) As Task(Of Models.AddDocumentsResult)

        ''' <summary>Buong path ng file sa Files Folder (para buksan/kopyahin). Nagbabato ng mensahe kapag hindi makita.</summary>
        Function GetDocumentFullPathAsync(documentId As Integer) As Task(Of String)

        ''' <summary>Kailangan ang Can Edit. Soft delete; ang password ay tinitiyak ng UI bago tawagin ito.</summary>
        Function SoftDeleteDocumentAsync(documentId As Integer) As Task(Of Boolean)

        ''' <summary>Larawan ng empleyado; Nothing kung wala o hindi maabot ang folder (hindi nagbabato).</summary>
        Function GetPhotoAsync(recordId As Integer) As Task(Of System.Drawing.Image)

        ''' <summary>Kailangan ang Can Edit sa "Employee Documents".</summary>
        Function SetPhotoAsync(recordId As Integer, sourceImagePath As String) As Task
        Function RemovePhotoAsync(recordId As Integer) As Task

    End Interface

End Namespace
