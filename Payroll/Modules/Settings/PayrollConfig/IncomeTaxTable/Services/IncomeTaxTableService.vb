Imports Payroll.GlobalShared.Models

Namespace IncomeTaxTable.Services
    Public Class IncomeTaxTableService
        Implements IIncomeTaxTableService

        Private ReadOnly _repository As Data.IIncomeTaxTableRepository

        Public Sub New(repository As Data.IIncomeTaxTableRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of IncomeTaxBracketModel)) _
            Implements IIncomeTaxTableService.GetAllAsync

            Return Await _repository.GetAllAsync(tableName)
        End Function

        Public Async Function SaveAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of IncomeTaxTableSaveResult) _
            Implements IIncomeTaxTableService.SaveAsync

            If item.SalaryFrom < 0 OrElse item.SalaryTo < 0 Then
                Return New IncomeTaxTableSaveResult With {.Success = False, .ErrorMessage = "Salary From/To cannot be negative."}
            End If

            If item.SalaryTo <= item.SalaryFrom Then
                Return New IncomeTaxTableSaveResult With {.Success = False, .ErrorMessage = "Salary To must be greater than Salary From."}
            End If

            If item.TaxPercentage < 0 OrElse item.TaxPercentage > 100 Then
                Return New IncomeTaxTableSaveResult With {.Success = False, .ErrorMessage = "Tax Percentage must be between 0 and 100."}
            End If

            If item.FixTaxAmount < 0 Then
                Return New IncomeTaxTableSaveResult With {.Success = False, .ErrorMessage = "Fix Tax Amount cannot be negative."}
            End If

            ' Enforced gaya ng Statutory Settings - iwas mangyari yung
            ' overlapping-range quirk na nakita natin sa C1Pay Daily tab.
            Dim isOverlapping = Await _repository.OverlapExistsAsync(tableName, item.SalaryFrom, item.SalaryTo, item.Id)
            If isOverlapping Then
                Return New IncomeTaxTableSaveResult With {.Success = False, .ErrorMessage = "This salary range overlaps with an existing bracket."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(tableName, item, userName)
            Else
                Await _repository.UpdateAsync(tableName, item, userName)
            End If

            Return New IncomeTaxTableSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IIncomeTaxTableService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(tableName, id, isActive, userName)
        End Function

    End Class
End Namespace