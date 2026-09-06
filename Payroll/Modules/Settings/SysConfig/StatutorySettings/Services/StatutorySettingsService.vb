Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Services
    Public Class StatutorySettingsService
        Implements IStatutorySettingsService

        Private ReadOnly _repository As Data.IStatutorySettingsRepository

        Public Sub New(repository As Data.IStatutorySettingsRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of StatutoryBracketModel)) _
            Implements IStatutorySettingsService.GetAllAsync

            Return Await _repository.GetAllAsync(tableName)
        End Function

        Public Async Function SaveAsync(tableName As String, item As StatutoryBracketModel, userName As String) As Task(Of StatutorySettingsSaveResult) _
            Implements IStatutorySettingsService.SaveAsync

            If item.SalaryTo <= item.SalaryFrom Then
                Return New StatutorySettingsSaveResult With {.Success = False, .ErrorMessage = "Salary To must be greater than Salary From."}
            End If

            Dim isOverlapping = Await _repository.OverlapExistsAsync(tableName, item.SalaryFrom, item.SalaryTo, item.Id)
            If isOverlapping Then
                Return New StatutorySettingsSaveResult With {.Success = False, .ErrorMessage = "This salary range overlaps with an existing bracket."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(tableName, item, userName)
            Else
                Await _repository.UpdateAsync(tableName, item, userName)
            End If

            Return New StatutorySettingsSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IStatutorySettingsService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(tableName, id, isActive, userName)
        End Function

    End Class
End Namespace