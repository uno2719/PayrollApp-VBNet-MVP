Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Class PayrollRateEntryService
        Implements IPayrollRateEntryService

        Private ReadOnly _repository As Data.IPayrollRateEntryRepository
        Private ReadOnly _unitGuard As InputUnitGuard

        Public Sub New(repository As Data.IPayrollRateEntryRepository, unitGuard As InputUnitGuard)
            _repository = repository
            _unitGuard = unitGuard
        End Sub

        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollRateEntryModel)) _
            Implements IPayrollRateEntryService.GetAllAsync

            Return Await _repository.GetAllAsync(tableName)
        End Function

        Public Async Function SaveAsync(tableName As String, item As PayrollRateEntryModel, userName As String, Optional confirmClearDrafts As Boolean = False) As Task(Of PayrollSettingsSaveResult) _
            Implements IPayrollRateEntryService.SaveAsync

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

            ' --- InputUnit: bantayan ang pagpalit ng unit (tingnan ang InputUnitGuard) ---
            If Not [Enum].IsDefined(GetType(PayrollInputUnit), item.InputUnit) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Please select a valid Input Unit."}
            End If

            Dim unitCategory = PayrollInputUnits.CategoryForTable(tableName)
            Dim codeToClear As String = Nothing

            If item.Id <> 0 Then
                Dim existing = (Await _repository.GetAllAsync(tableName)).FirstOrDefault(Function(x) x.Id = item.Id)
                If existing IsNot Nothing Then
                    Dim check = Await _unitGuard.CheckAsync(unitCategory, existing.Code, existing.InputUnit, item.InputUnit, confirmClearDrafts)
                    If Not check.Allowed Then
                        Return New PayrollSettingsSaveResult With {.Success = False, .NeedsConfirmation = check.NeedsConfirmation, .ErrorMessage = check.Message}
                    End If
                    If check.ClearDrafts Then codeToClear = existing.Code
                End If
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(tableName, item, userName)
            Else
                Await _repository.UpdateAsync(tableName, item, userName)
            End If

            If codeToClear IsNot Nothing Then
                Await _unitGuard.ClearDraftsAsync(unitCategory, codeToClear)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IPayrollRateEntryService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(tableName, id, isActive, userName)
        End Function

    End Class
End Namespace
