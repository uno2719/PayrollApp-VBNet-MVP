Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Class PayrollFlaggedEntryService
        Implements IPayrollFlaggedEntryService

        Private ReadOnly _repository As Data.IPayrollFlaggedEntryRepository

        Public Sub New(repository As Data.IPayrollFlaggedEntryRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollFlaggedEntryModel)) _
            Implements IPayrollFlaggedEntryService.GetAllAsync

            Return Await _repository.GetAllAsync(tableName)
        End Function

        Public Async Function SaveAsync(tableName As String, item As PayrollFlaggedEntryModel, userName As String) As Task(Of PayrollSettingsSaveResult) _
            Implements IPayrollFlaggedEntryService.SaveAsync

            If String.IsNullOrWhiteSpace(item.Code) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.Description) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Description is required."}
            End If

            Dim isDuplicate = Await _repository.CodeExistsAsync(tableName, item.Code.Trim(), item.Id)
            If isDuplicate Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = $"Code '{item.Code}' is already in use."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(tableName, item, userName)
            Else
                Await _repository.UpdateAsync(tableName, item, userName)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IPayrollFlaggedEntryService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(tableName, id, isActive, userName)
        End Function

    End Class
End Namespace
