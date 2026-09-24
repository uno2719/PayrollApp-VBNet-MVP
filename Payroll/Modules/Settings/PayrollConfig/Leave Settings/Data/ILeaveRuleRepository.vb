Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Data
    Public Interface ILeaveRuleRepository

        Function GetAllAsync() As Task(Of List(Of LeaveRuleModel))
        Function CombinationExistsAsync(leaveGroupId As Integer, leaveTypeId As Integer, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(item As LeaveRuleModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As LeaveRuleModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)

        ' Brackets (child rows) - laging binabasa/pinapalitan nang buo
        ' kasabay ng parent Rule, hindi hiwalay na CRUD per-row.
        Function GetBracketsAsync(leaveRuleId As Integer) As Task(Of List(Of LeaveRuleBracketModel))
        Function ReplaceBracketsAsync(leaveRuleId As Integer, brackets As List(Of LeaveRuleBracketModel), userName As String) As Task(Of Boolean)

        ' Insert-or-Update ng HEADER + buong palit ng brackets sa
        ' loob ng IISANG transaction - ito ang tinatawag ng Service
        ' sa normal na Save flow (hindi na hiwalay na InsertAsync/
        ' UpdateAsync + ReplaceBracketsAsync), para hindi maiwan ang
        ' isang bagong Rule na walang brackets kung biglang mag-crash
        ' sa pagitan ng dalawang hakbang. Nire-return ang LeaveRuleId
        ' (bago o dati na).
        Function SaveWithBracketsAsync(item As LeaveRuleModel, brackets As List(Of LeaveRuleBracketModel), userName As String) As Task(Of Integer)

    End Interface
End Namespace
