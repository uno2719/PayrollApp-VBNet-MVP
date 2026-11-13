Imports Payroll.DocumentCategories.Models
Imports Payroll.GlobalShared.Models

Namespace DocumentCategories.Data
    Public Interface IDocCategoryRepository
        Function GetAllAsync() As Task(Of List(Of DocCategoryModel))
        Function NameExistsAsync(name As String, excludeId As Integer) As Task(Of Boolean)
        Function NextSortOrderAsync() As Task(Of Integer)
        Function InsertAsync(item As DocCategoryModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As DocCategoryModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
        Function DeleteAsync(id As Integer) As Task(Of DeleteResult)
    End Interface
End Namespace
