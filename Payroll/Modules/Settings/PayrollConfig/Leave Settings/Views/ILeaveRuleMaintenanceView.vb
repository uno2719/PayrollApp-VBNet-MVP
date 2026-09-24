Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Views
    Public Interface ILeaveRuleMaintenanceView

        ' --- Header form fields ---
        Property LeaveGroupId As Integer
        Property LeaveTypeId As Integer
        Property EntitlementMethod As String
        Property ComputeBasedOn As String
        Property PlotBasedOn As String
        Property AnniversaryPlotOn As String
        Property UnitOfMeasure As String
        Property HolidayIncluded As Boolean
        Property RequireAttachment As Boolean
        Property Monetize As Boolean
        Property ShowEntitlement As Boolean
        Property IsActive As Boolean

        ' --- Combo data sources (binubuo isang beses sa LoadAsync) ---
        Sub BindLeaveGroups(items As List(Of LookupModel))
        Sub BindLeaveTypes(items As List(Of LeaveTypeModel))

        ' --- Master list (kanang panel - listahan ng saved Rules) ---
        Sub BindList(items As List(Of LeaveRuleModel))

        ' --- Bracket sub-grid (YOS rows) ---
        Sub BindBrackets(items As List(Of LeaveRuleBracketModel))
        Function GetBrackets() As List(Of LeaveRuleBracketModel)

        ' --- State/UX ---
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
