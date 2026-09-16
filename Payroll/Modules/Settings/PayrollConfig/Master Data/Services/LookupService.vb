Imports Payroll.GlobalShared.Models

Namespace Lookups.Services
    Public Class LookupService
        Implements ILookupService

        Private ReadOnly _repository As Data.ILookupRepository

        Public Sub New(repository As Data.ILookupRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of LookupModel)) _
            Implements ILookupService.GetAllAsync

            Return Await _repository.GetAllAsync(tableName)
        End Function

        Public Async Function SaveAsync(tableName As String, item As LookupModel, userName As String) As Task(Of LookupSaveResult) _
            Implements ILookupService.SaveAsync

            If String.IsNullOrWhiteSpace(item.Code) Then
                Return New LookupSaveResult With {.Success = False, .ErrorMessage = "Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.Name) Then
                Return New LookupSaveResult With {.Success = False, .ErrorMessage = "Name is required."}
            End If

            Dim isDuplicate = Await _repository.CodeExistsAsync(tableName, item.Code.Trim(), item.Id)
            If isDuplicate Then
                Return New LookupSaveResult With {.Success = False, .ErrorMessage = $"Code '{item.Code}' is already in use."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(tableName, item, userName)
            Else
                Await _repository.UpdateAsync(tableName, item, userName)
            End If

            Return New LookupSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILookupService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(tableName, id, isActive, userName)
        End Function

    End Class
End Namespace