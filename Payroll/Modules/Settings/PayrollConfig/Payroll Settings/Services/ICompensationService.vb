Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface ICompensationService
        Function GetAllAsync() As Task(Of List(Of CompensationModel))
        Function SaveAsync(item As CompensationModel, userName As String) As Task(Of PayrollSettingsSaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
