Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Services
    Public Class LeaveTypeService
        Implements ILeaveTypeService

        Private ReadOnly _repository As Data.ILeaveTypeRepository

        Public Sub New(repository As Data.ILeaveTypeRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of LeaveTypeModel)) _
            Implements ILeaveTypeService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As LeaveTypeModel, userName As String) As Task(Of LeaveTypeSaveResult) _
            Implements ILeaveTypeService.SaveAsync

            If String.IsNullOrWhiteSpace(item.Code) Then
                Return New LeaveTypeSaveResult With {.Success = False, .ErrorMessage = "Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.Name) Then
                Return New LeaveTypeSaveResult With {.Success = False, .ErrorMessage = "Name is required."}
            End If

            If Not LeaveCategory.All.Contains(item.Category) Then
                Return New LeaveTypeSaveResult With {.Success = False, .ErrorMessage = "Please select a valid Category."}
            End If

            Dim isDuplicate = Await _repository.CodeExistsAsync(item.Code.Trim(), item.Id)
            If isDuplicate Then
                Return New LeaveTypeSaveResult With {.Success = False, .ErrorMessage = $"Code '{item.Code}' is already in use."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New LeaveTypeSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILeaveTypeService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function

    End Class
End Namespace
