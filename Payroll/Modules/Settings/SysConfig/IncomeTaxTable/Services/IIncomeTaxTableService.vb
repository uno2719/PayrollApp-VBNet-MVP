Imports Payroll.GlobalShared.Models

Namespace IncomeTaxTable.Services
    Public Interface IIncomeTaxTableService
        Function GetAllAsync(tableName As String) As Task(Of List(Of IncomeTaxBracketModel))
        Function SaveAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of IncomeTaxTableSaveResult)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class IncomeTaxTableSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace