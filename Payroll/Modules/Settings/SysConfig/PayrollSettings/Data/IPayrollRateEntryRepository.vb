Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    Public Interface IPayrollRateEntryRepository
        Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollRateEntryModel))
        Function CodeExistsAsync(tableName As String, code As String, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(tableName As String, item As PayrollRateEntryModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(tableName As String, item As PayrollRateEntryModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
