Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Class CompensationService
        Implements ICompensationService

        Private ReadOnly _repository As Data.ICompensationRepository

        Public Sub New(repository As Data.ICompensationRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of CompensationModel)) _
            Implements ICompensationService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As CompensationModel, userName As String) As Task(Of PayrollSettingsSaveResult) _
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

            If item.Id = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
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
    End Class
End Namespace
