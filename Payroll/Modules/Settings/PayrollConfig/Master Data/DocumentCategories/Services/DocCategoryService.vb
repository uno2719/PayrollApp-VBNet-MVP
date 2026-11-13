Imports Payroll.DocumentCategories.Data
Imports Payroll.DocumentCategories.Models
Imports Payroll.GlobalShared.Models

Namespace DocumentCategories.Services
    Public Class DocCategoryService
        Implements IDocCategoryService

        Public Const MaxNameLength As Integer = 100
        Public Const MaxSortOrder As Integer = 9999

        Private ReadOnly _repository As IDocCategoryRepository

        Public Sub New(repository As IDocCategoryRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of DocCategoryModel)) _
            Implements IDocCategoryService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As DocCategoryModel, userName As String) As Task(Of DocCategorySaveResult) _
            Implements IDocCategoryService.SaveAsync

            Dim name As String = If(item.Name, String.Empty).Trim()

            If name.Length = 0 Then
                Return Fail("Category name is required.")
            End If
            If name.Length > MaxNameLength Then
                Return Fail($"Category name is too long (maximum {MaxNameLength} characters).")
            End If
            If item.SortOrder < 0 OrElse item.SortOrder > MaxSortOrder Then
                Return Fail($"Sort Order must be between 0 and {MaxSortOrder}.")
            End If

            If Await _repository.NameExistsAsync(name, item.Id) Then
                Return Fail($"A category named '{name}' already exists.")
            End If

            item.Name = name

            If item.Id = 0 Then
                ' 0 = ilagay sa dulo ng listahan
                If item.SortOrder = 0 Then item.SortOrder = Await _repository.NextSortOrderAsync()
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New DocCategorySaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IDocCategoryService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function

        ' Totoong pagbura - admin lang at may in-use check (nasa repository/RecordDeleter).
        Public Async Function DeleteAsync(id As Integer) As Task(Of DeleteResult) _
            Implements IDocCategoryService.DeleteAsync

            If Not AppSession.IsAdmin Then
                Return DeleteResult.Fail("Only administrators can delete records.")
            End If

            Return Await _repository.DeleteAsync(id)
        End Function

        Private Shared Function Fail(message As String) As DocCategorySaveResult
            Return New DocCategorySaveResult With {.Success = False, .ErrorMessage = message}
        End Function

    End Class
End Namespace
