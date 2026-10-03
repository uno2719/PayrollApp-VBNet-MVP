Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollProcessing.Services

Public Class ucPayrollOutput
    Implements IAsyncLoadable

    ' Reuses the same service as Input Entry — Output just needs the Cutoff
    ' list for now. Once the computation engine (future thread) exists, this
    ' view will also pull the actual per-employee computed results.
    Private _service As IPayrollInputService

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetService(service As IPayrollInputService)
        _service = service
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Payroll > Output"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payroll Output"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Main_Payroll
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Dim cutoffs = Await _service.GetCutoffsAsync()
        cboCutoff.Properties.DataSource = cutoffs
        cboCutoff.Properties.DisplayMember = NameOf(CutoffModel.DisplayLabel)
        cboCutoff.Properties.ValueMember = NameOf(CutoffModel.CutoffID)
        cboCutoff.Properties.Columns.Clear()
        cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.DisplayLabel), "Cutoff"))

        ' TODO (computation-engine thread): load the computed per-employee output
        ' for the selected Cutoff (Basic/Bonus/Allowance/Overtime/Deduction/Leave/
        ' Gross Earning/Gross Deduction/Gross SSS/PhilHealth/Pag-IBIG/Net Pay),
        ' matching the C1Pay Process>Output reference screenshot.
    End Function

    Private Sub btnExportConverter_Click(sender As Object, e As EventArgs) Handles btnExportConverter.Click
        ShowMessage("Export format para sa BDO converter — pending pa ang format mula sa gagamit ng system.")
    End Sub

    Private Sub btnExportDirect_Click(sender As Object, e As EventArgs) Handles btnExportDirect.Click
        ShowMessage("Direct-to-BDO export format — pending pa ang format mula sa gagamit ng system.")
    End Sub

End Class