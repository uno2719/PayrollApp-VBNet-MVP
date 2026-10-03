Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface ICutoffMaintenanceView

        ' Form fields — also doubles as the input for "Generate for Year"
        ' (CycleType + CutoffYear are reused for both single-add and batch-generate).
        Property CycleType As String
        Property CutoffYear As Integer
        Property CutoffStart As Date
        Property CutoffEnd As Date
        Property PayDate As Date?
        Property CutoffLabel As String
        Property Status As CutoffStatus

        ' Grid
        Sub BindList(items As List(Of CutoffModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
        Function ConfirmGenerate(cycleType As String, year As Integer) As Boolean

    End Interface
End Namespace
