' File: Modules/MainMenu/Employee/Models/EmployeeDocumentModel.vb
' Phase 3: 201 Files. Mga model para sa category, file row, at resulta ng upload.
Namespace Employee.Models

    ''' <summary>Isang row ng tblDocumentCategory (ang listahan sa category picker).</summary>
    Public Class DocumentCategoryModel
        Public Property CategoryId As Integer
        Public Property CategoryName As String

        ''' <summary>Binibilang sa "Profile %" ng header kapag wala pang file sa category na ito.</summary>
        Public Property IsRequired As Boolean
        Public Property SortOrder As Integer
        Public Property IsActive As Boolean
    End Class

    ''' <summary>Isang row ng tblEmployeeDocument (kasama ang pangalan ng category).</summary>
    Public Class EmployeeDocumentModel
        Public Property DocumentId As Integer
        Public Property RecordId As Integer
        Public Property CategoryId As Integer
        Public Property CategoryName As String

        ''' <summary>Orihinal na pangalan ng file.</summary>
        Public Property FileName As String

        ''' <summary>RELATIVE sa Files Folder (hindi buong path).</summary>
        Public Property StoredPath As String
        Public Property FileSizeBytes As Long
        Public Property Remarks As String
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String

        Public ReadOnly Property SizeText As String
            Get
                If FileSizeBytes >= 1048576L Then Return $"{FileSizeBytes / 1048576.0:N1} MB"
                If FileSizeBytes >= 1024L Then Return $"{FileSizeBytes / 1024.0:N0} KB"
                Return $"{FileSizeBytes} B"
            End Get
        End Property
    End Class

    ''' <summary>Resulta ng pag-upload ng isa o higit pang file: ilan ang na-save at alin ang hindi (at bakit).</summary>
    Public Class AddDocumentsResult
        Public Property Saved As Integer
        Public ReadOnly Property Failures As New List(Of String)
    End Class

End Namespace
