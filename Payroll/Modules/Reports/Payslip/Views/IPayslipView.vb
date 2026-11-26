Imports Payroll.GlobalShared.Models

Namespace Payslips.Views

    Public Interface IPayslipView

        ''' <summary>Per-cutoff mode: isang cutoff kada item. selectCutoffIds: unahin ang may ganitong ID kung meron.</summary>
        Sub DisplayCutoffs(cutoffs As List(Of CutoffModel), selectCutoffIds As List(Of Integer))

        ''' <summary>Group by pay date mode: isang pay date kada item (lahat ng cutoff na may ganoong pay date).</summary>
        Sub DisplayPayDates(groups As List(Of PayDateGroupModel), selectCutoffIds As List(Of Integer))

        Sub DisplayEmployees(rows As List(Of PayslipEmployeeRow))

        Function IsGroupByPayDate() As Boolean
        ''' <summary>Binabago ang checkbox nang hindi nagre-reload (ang tumawag ang bahalang mag-LoadAsync).</summary>
        Sub SetGroupByPayDate(value As Boolean)

        ''' <summary>Ang mga cutoff ng napili: isa (per cutoff) o lahat ng cutoff ng pay date. Walang laman kung wala.</summary>
        Function GetSelectedCutoffs() As List(Of CutoffModel)

        ''' <summary>Pangalan ng napili para sa mga mensahe, hal. "Nov 11-25, 2026 (Semi-Monthly)" o "Pay date Nov 30, 2026".</summary>
        Function GetSelectionLabel() As String

        ''' <summary>Mga naka-check na empleyado; kapag walang naka-check, LAHAT ng empleyado sa napili.</summary>
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
