Imports System.Data
Imports Payroll.PayrollProcessing.Models

Namespace PayrollProcessing.Views

    Public Interface IPayrollInputEntryView

        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel))

        ''' <summary>Builds the grid's columns (core fixed + dynamic catalog-driven) and sets up the Column Chooser.</summary>
        Sub DisplayColumns(columns As List(Of PayrollInputColumnModel))

        Sub DisplayData(table As DataTable)

        ''' <summary>Reads whatever the user has typed/edited back out of the grid.</summary>
        Function GetEditedData() As DataTable

        Function GetSelectedCutoffId() As Integer?

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace