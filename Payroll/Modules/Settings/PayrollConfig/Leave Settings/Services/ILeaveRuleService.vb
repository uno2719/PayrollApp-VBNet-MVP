Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Services
    Public Interface ILeaveRuleService
        Function GetAllAsync() As Task(Of List(Of LeaveRuleModel))
        Function GetBracketsAsync(leaveRuleId As Integer) As Task(Of List(Of LeaveRuleBracketModel))
        Function SaveAsync(item As LeaveRuleModel, brackets As List(Of LeaveRuleBracketModel), userName As String) As Task(Of LeaveRuleSaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class LeaveRuleSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property SavedId As Integer
    End Class
End Namespace
