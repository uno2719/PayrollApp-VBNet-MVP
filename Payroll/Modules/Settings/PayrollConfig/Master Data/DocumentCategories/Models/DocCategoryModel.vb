Namespace DocumentCategories.Models

    ''' <summary>
    ''' Isang category ng 201 file (tblDocumentCategory) para sa Settings > Master Data > Document Categories.
    ''' (Hiwalay ito sa Employee.Models.DocumentCategoryModel na pang-picker lang ng 201 Files tab.)
    ''' </summary>
    Public Class DocCategoryModel
        Public Property Id As Integer
        Public Property Name As String

        ''' <summary>True = binibilang sa "Profile %" ng Employee header kapag wala pang file sa category na ito.</summary>
        Public Property IsRequired As Boolean
        Public Property SortOrder As Integer
        Public Property IsActive As Boolean = True

        ''' <summary>Ilang file (hindi pa binubura) ng lahat ng empleyado ang nasa category na ito. Display lang.</summary>
        Public Property FileCount As Integer

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class

End Namespace
