Imports Payroll.DocumentCategories.Models
Imports Payroll.GlobalShared.Models

Namespace DocumentCategories.Services
    Public Interface IDocCategoryService
        Function GetAllAsync() As Task(Of List(Of DocCategoryModel))
        Function SaveAsync(item As DocCategoryModel, userName As String) As Task(Of DocCategorySaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
        Function DeleteAsync(id As Integer) As Task(Of DeleteResult)
    End Interface

    Public Class DocCategorySaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace
