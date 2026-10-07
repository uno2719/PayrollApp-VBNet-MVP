Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Class CompensationService
        Implements ICompensationService

        Private ReadOnly _repository As Data.ICompensationRepository
        Private ReadOnly _unitGuard As InputUnitGuard

        Public Sub New(repository As Data.ICompensationRepository, unitGuard As InputUnitGuard)
            _repository = repository
            _unitGuard = unitGuard
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of CompensationModel)) _
            Implements ICompensationService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As CompensationModel, userName As String, Optional confirmClearDrafts As Boolean = False) As Task(Of PayrollSettingsSaveResult) _
            Implements ICompensationService.SaveAsync

            If String.IsNullOrWhiteSpace(item.Code) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.Description) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Description is required."}
            End If

            Dim isDuplicate = Await _repository.CodeExistsAsync(item.Code.Trim(), item.Id)
            If isDuplicate Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = $"Code '{item.Code}' is already in use."}
            End If

            ' --- InputUnit: bantayan ang pagpalit ng unit (tingnan ang InputUnitGuard) ---
            If Not [Enum].IsDefined(GetType(PayrollInputUnit), item.InputUnit) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Please select a valid Input Unit."}
            End If

            Dim unitCategory = PayrollInputUnits.CategoryCompensation
            Dim codeToClear As String = Nothing

            If item.Id <> 0 Then
                Dim existing = (Await _repository.GetAllAsync()).FirstOrDefault(Function(x) x.Id = item.Id)
                If existing IsNot Nothing Then
                    Dim check = Await _unitGuard.CheckAsync(unitCategory, existing.Code, existing.InputUnit, item.InputUnit, confirmClearDrafts)
                    If Not check.Allowed Then
                        Return New PayrollSettingsSaveResult With {.Success = False, .NeedsConfirmation = check.NeedsConfirmation, .ErrorMessage = check.Message}
                    End If
                    If check.ClearDrafts Then codeToClear = existing.Code
                End If
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            If codeToClear IsNot Nothing Then
                Await _unitGuard.ClearDraftsAsync(unitCategory, codeToClear)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ICompensationService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function

    End Class

    ' Iisang SaveResult class na shared ng apat na Payroll Settings
    ' services (Compensation/FlaggedEntry/RateEntry/Loan) - pareho lang
    ' naman ang shape nila (Success + ErrorMessage), kaya hindi na
    ' kailangan mag-4 ng magkaparehong result class.
    Public Class PayrollSettingsSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        ''' <summary>True kung kailangan munang kumpirmahin ng user ang ErrorMessage (hal. i-clear ang draft entries) bago ituloy.</summary>
        Public Property NeedsConfirmation As Boolean
    End Class
End Namespace
