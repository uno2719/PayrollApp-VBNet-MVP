Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Views

    Public Interface IPayrollOutputView

        ''' <summary>Mga cutoff na may naka-save na results (galing sa Process).</summary>
        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel))

        ''' <summary>Isang row kada empleyado + lahat ng linya (ang detail grid ay naka-filter sa napiling empleyado).</summary>
        Sub DisplayResults(headers As List(Of PayrollResultModel), lines As List(Of PayrollResultLineModel))

        Sub ClearResults()

        Function GetSelectedCutoffId() As Integer?

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace
