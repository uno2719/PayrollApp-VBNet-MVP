Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Services
    Public Interface ILeaveTypeService
        Function GetAllAsync() As Task(Of List(Of LeaveTypeModel))
        Function SaveAsync(item As LeaveTypeModel, userName As String) As Task(Of LeaveTypeSaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class LeaveTypeSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace
