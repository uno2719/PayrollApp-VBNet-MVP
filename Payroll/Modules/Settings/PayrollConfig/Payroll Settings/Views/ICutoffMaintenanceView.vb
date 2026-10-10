Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface ICutoffMaintenanceView

        ' Detalye ng napiling Cutoff (Edit lang ang pwede dito - ang paggawa ng
        ' bagong Cutoff ay sa Generate Cut-off dialog na nagbubukas sa [New]).
        Property CycleType As String
        Property CutoffYear As Integer
        Property CutoffStart As Date
        Property CutoffEnd As Date
        Property PayDate As Date?
        Property CutoffLabel As String
        Property Status As CutoffStatus

        ' Filter - Nothing/"" = (All)
        Property FilterCycleType As String
        Property FilterYear As Integer?
        Sub BindFilters(cycleTypes As List(Of String), years As List(Of Integer))

        ' Grid
        Sub BindList(items As List(Of CutoffModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

        ''' <summary>Yes/No bago gawing Closed ang mga lumang Draft na cutoff.</summary>
        Function ConfirmCloseOlder(message As String) As Boolean

    End Interface
End Namespace
