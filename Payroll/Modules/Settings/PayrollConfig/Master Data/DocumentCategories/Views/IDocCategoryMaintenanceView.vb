Imports Payroll.DocumentCategories.Models

Namespace DocumentCategories.Views
    Public Interface IDocCategoryMaintenanceView

        ' Form fields
        Property CategoryName As String
        Property IsRequired As Boolean
        Property SortOrder As Integer
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of DocCategoryModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
