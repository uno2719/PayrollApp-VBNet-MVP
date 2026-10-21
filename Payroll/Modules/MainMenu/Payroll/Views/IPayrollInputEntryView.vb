Imports Payroll.GlobalShared.Models
Imports System.Data
Imports Payroll.PayrollProcessing.Models

Namespace PayrollProcessing.Views

    Public Interface IPayrollInputEntryView

        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel))

        ''' <summary>Builds the grid's columns (core fixed + dynamic catalog-driven) and sets up the Column Chooser.</summary>
        Sub DisplayColumns(columns As List(Of PayrollInputColumnModel))

        Sub DisplayData(table As DataTable)

        ''' <summary>Mga empleyadong hindi naisama sa grid dahil kulang ang setup (walang laman = walang babala).</summary>
        Sub DisplayEligibilityWarnings(warnings As List(Of GlobalShared.Models.PayrollEligibilityWarning))

        ''' <summary>Reads whatever the user has typed/edited back out of the grid.</summary>
        Function GetEditedData() As DataTable

        Function GetSelectedCutoffId() As Integer?

        ''' <summary>Ipinapakita ang buod ng computation; True kung pumayag ang user na i-save.</summary>
        Function ConfirmProcess(summary As String) As Boolean

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace