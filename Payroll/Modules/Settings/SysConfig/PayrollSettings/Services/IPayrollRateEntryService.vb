Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface IPayrollRateEntryService
        Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollRateEntryModel))
        Function SaveAsync(tableName As String, item As PayrollRateEntryModel, userName As String) As Task(Of PayrollSettingsSaveResult)
        Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
