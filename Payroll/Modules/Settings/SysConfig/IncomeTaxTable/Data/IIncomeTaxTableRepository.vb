Imports Payroll.GlobalShared.Models

Namespace IncomeTaxTable.Data
    Public Interface IIncomeTaxTableRepository
        Function GetAllAsync(tableName As String) As Task(Of List(Of IncomeTaxBracketModel))
        Function OverlapExistsAsync(tableName As String, salaryFrom As Decimal, salaryTo As Decimal, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace