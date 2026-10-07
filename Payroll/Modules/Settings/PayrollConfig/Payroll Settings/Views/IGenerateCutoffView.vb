Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface IGenerateCutoffView

        Property CycleType As String
        Property CutoffYear As Integer

        Sub BindCycles(cycleTypes As List(Of String))
        Sub BindPreview(rows As List(Of CutoffPreviewRow))
        Sub ShowPatternText(text As String)
        Sub ShowSummary(text As String)
        Sub SetGenerateEnabled(enabled As Boolean)

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
        Sub CloseWithSuccess()

    End Interface
End Namespace
