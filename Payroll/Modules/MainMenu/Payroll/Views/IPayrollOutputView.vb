Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Views

    Public Interface IPayrollOutputView

        ''' <summary>Mode na per-cutoff: mga cutoff na may naka-save na results (galing sa Process).</summary>
        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel))

        ''' <summary>Mode na "Group by pay date": mga pay date na may processed cutoff.</summary>
        Sub DisplayPayDates(groups As List(Of PayDateGroupModel))

        ''' <summary>Isang row kada empleyado + lahat ng linya (ang detail grid ay naka-filter sa napiling empleyado).</summary>
        Sub DisplayResults(headers As List(Of PayrollResultModel), lines As List(Of PayrollResultLineModel))

        Sub ClearResults()

        ''' <summary>Naka-check ba ang "Group by pay date"?</summary>
        Function IsGroupByPayDate() As Boolean

        ''' <summary>Mga cutoff ng napili: isa (per-cutoff) o lahat ng cutoff ng napiling pay date. Walang laman kung wala.</summary>
        Function GetSelectedCutoffIds() As List(Of Integer)

        ''' <summary>Ang napiling pay date sa "Group by pay date"; Nothing sa per-cutoff mode.</summary>
        Function GetSelectedPayDate() As Date?

        ''' <summary>OK / Cancel (default ay Cancel). True kapag OK.</summary>
        Function AskConfirm(message As String, title As String) As Boolean

        ''' <summary>Maliit na input box. Blanko/Nothing kapag kinansela.</summary>
        Function AskInput(prompt As String, title As String, defaultValue As String) As String

        ''' <summary>Save dialog. Nothing kapag kinansela.</summary>
        Function AskSaveFile(defaultFileName As String, filter As String, defaultExt As String) As String

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace
