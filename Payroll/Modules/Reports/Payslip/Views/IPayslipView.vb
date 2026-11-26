Imports Payroll.GlobalShared.Models

Namespace Payslips.Views

    Public Interface IPayslipView

        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel), selectCutoffId As Integer?)
        Sub DisplayEmployees(rows As List(Of PayslipEmployeeRow))

        ''' <summary>Ang napiling cutoff; Nothing kung wala.</summary>
        Function GetSelectedCutoff() As CutoffModel

        ''' <summary>Mga naka-check na empleyado; kapag walang naka-check, LAHAT ng empleyado sa cutoff.</summary>
        Function GetTargetRows() As List(Of PayslipEmployeeRow)

        ''' <summary>Ang naka-focus na row lang (para sa single preview / email).</summary>
        Function GetFocusedRow() As PayslipEmployeeRow

        Function GetOptions() As PayslipOptions

        ''' <summary>Ipinapakita ang report sa naka-embed na viewer (Nothing = linisin ang viewer).</summary>
        Sub ShowPreview(report As rptPayslip)

        Sub SetBusy(isBusy As Boolean)
        Sub SetEmailProgress(visible As Boolean, done As Integer, total As Integer, text As String)

        Function AskConfirm(message As String, title As String) As Boolean
        Function AskSaveFile(defaultFileName As String) As String
        Function AskFolder(description As String) As String

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface

End Namespace
