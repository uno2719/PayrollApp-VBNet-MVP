Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Data
    Public Interface ILeaveTypeRepository
        Function GetAllAsync() As Task(Of List(Of LeaveTypeModel))
        Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(item As LeaveTypeModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As LeaveTypeModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace
