Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Views

    Public Interface IPayrollOutputView

        ''' <summary>Mga cutoff na may naka-save na results (galing sa Process).</summary>
        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel))

        ''' <summary>Isang row kada empleyado + lahat ng linya (ang detail grid ay naka-filter sa napiling empleyado).</summary>
        Sub DisplayResults(headers As List(Of PayrollResultModel), lines As List(Of PayrollResultLineModel))

        Sub ClearResults()

        Function GetSelectedCutoffId() As Integer?

        ''' <summary>Naka-check ba ang "Combine same pay date"?</summary>
        Function GetCombineSamePayDate() As Boolean

        ''' <summary>OK / Cancel (default ay Cancel). True kapag OK.</summary>
        Function AskConfirm(message As String, title As String) As Boolean

        ''' <summary>Save dialog. Nothing kapag kinansela.</summary>
        Function AskSaveFile(defaultFileName As String, filter As String, defaultExt As String) As String

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace
