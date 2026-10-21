Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollProcessing.Presenters
Imports Payroll.PayrollProcessing.Views

Public Class ucPayrollOutput
    Implements IPayrollOutputView
    Implements IAsyncLoadable

    ' Itinakda ng AppComposition pagkatapos magawa ang View at Presenter (parehong pattern ng Input Entry).
    Private _presenter As PayrollOutputPresenter
    Private _headers As New List(Of PayrollResultModel)()
    Private _lines As New List(Of PayrollResultLineModel)()
    Private _columnsBuilt As Boolean = False
    Private _loadingCutoffs As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetPresenter(presenter As PayrollOutputPresenter)
        _presenter = presenter
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

    ' Tinatawag ng ucPayroll sa bawat pagbukas ng Output tab (ResetLoadState + EnsureLoadedAsync)
    ' para laging updated ang listahan pagkatapos mag-Process sa Input tab.
    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        BuildGridColumns()
        Await _presenter.LoadAsync()
    End Function

#Region "IPayrollOutputView"

    Public Sub DisplayCutoffs(cutoffs As List(Of CutoffModel)) Implements IPayrollOutputView.DisplayCutoffs
        Dim previous = GetSelectedCutoffId()

        _loadingCutoffs = True
        Try
            cboCutoff.Properties.DataSource = cutoffs
            cboCutoff.Properties.DisplayMember = NameOf(CutoffModel.DisplayLabel)
            cboCutoff.Properties.ValueMember = NameOf(CutoffModel.CutoffID)
            cboCutoff.Properties.Columns.Clear()
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.DisplayLabel), "Cutoff"))

            If previous.HasValue AndAlso cutoffs.Any(Function(c) c.CutoffID = previous.Value) Then
                cboCutoff.EditValue = previous.Value
            ElseIf cutoffs.Count > 0 Then
                cboCutoff.EditValue = cutoffs(0).CutoffID      ' pinakabago
            Else
                cboCutoff.EditValue = Nothing
            End If
        Finally
            _loadingCutoffs = False
        End Try

        ' Laging i-refresh ang results ng napiling cutoff (maaaring na-re-process sa Input tab).
        Dim selected = GetSelectedCutoffId()
        If selected.HasValue Then
            Dim ignore = LoadSelectedAsync(selected.Value)
        Else
            ClearResults()
        End If
    End Sub

    Public Sub DisplayResults(headers As List(Of PayrollResultModel), lines As List(Of PayrollResultLineModel)) _
        Implements IPayrollOutputView.DisplayResults

        _headers = If(headers, New List(Of PayrollResultModel)())
        _lines = If(lines, New List(Of PayrollResultLineModel)())

        gridControl.DataSource = _headers
        gridView.BestFitColumns()

        Dim gross = _headers.Sum(Function(h) h.GrossPay)
        Dim deductions = _headers.Sum(Function(h) h.TotalDeduction)
        Dim net = _headers.Sum(Function(h) h.NetPay)
        Dim withWarnings = _headers.Where(Function(h) h.HasWarnings).Count()

        lblSummary.Text = $"Employees: {_headers.Count}    Gross: {gross:N2}    Deductions: {deductions:N2}    Net: {net:N2}" &
                          If(withWarnings > 0, $"    With warnings: {withWarnings}", "")

        ShowLinesOfFocusedRow()
    End Sub

    Public Sub ClearResults() Implements IPayrollOutputView.ClearResults
        _headers = New List(Of PayrollResultModel)()
        _lines = New List(Of PayrollResultLineModel)()
        gridControl.DataSource = _headers
        gridControlLines.DataSource = New List(Of PayrollResultLineModel)()
        lblSummary.Text = "No processed results yet. Use Process in the Input tab."
    End Sub

    Public Function GetSelectedCutoffId() As Integer? Implements IPayrollOutputView.GetSelectedCutoffId
        If cboCutoff.EditValue Is Nothing OrElse cboCutoff.EditValue Is DBNull.Value Then Return Nothing
        Return CInt(cboCutoff.EditValue)
    End Function

    ' Pinalitan ang pangalan para hindi sumalungat sa ShowMessage/ShowError ng ucBase (parehong pattern ng Input Entry)
    Public Sub DisplayInfo(message As String) Implements IPayrollOutputView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IPayrollOutputView.ShowError
        ShowError(message)
    End Sub

#End Region

#Region "Grids"

    Private Sub BuildGridColumns()
        If _columnsBuilt Then Return
        _columnsBuilt = True

        ' ---- Isang row kada empleyado ----
        gridView.Columns.Clear()
        AddColumn(gridView, NameOf(PayrollResultModel.EmployeeNo), "Employee No", 0, False)
        AddColumn(gridView, NameOf(PayrollResultModel.EmployeeName), "Name", 1, False)
        AddColumn(gridView, NameOf(PayrollResultModel.BasicPay), "Basic Pay", 2, True)
        AddColumn(gridView, NameOf(PayrollResultModel.GrossPay), "Gross Pay", 3, True)
        AddColumn(gridView, NameOf(PayrollResultModel.OtherDeduction), "Other Deductions", 4, True)
        AddColumn(gridView, NameOf(PayrollResultModel.SSSEE), "SSS", 5, True)
        AddColumn(gridView, NameOf(PayrollResultModel.PhilHealthEE), "PhilHealth", 6, True)
        AddColumn(gridView, NameOf(PayrollResultModel.PagIbigEE), "Pag-IBIG", 7, True)
        AddColumn(gridView, NameOf(PayrollResultModel.TaxableIncome), "Taxable Income", 8, True)
        AddColumn(gridView, NameOf(PayrollResultModel.WithholdingTax), "Tax", 9, True)
        AddColumn(gridView, NameOf(PayrollResultModel.LoanDeduction), "Loans", 10, True)
        AddColumn(gridView, NameOf(PayrollResultModel.TotalDeduction), "Total Deductions", 11, True)
        AddColumn(gridView, NameOf(PayrollResultModel.NetPay), "Net Pay", 12, True)
        AddColumn(gridView, NameOf(PayrollResultModel.WarningText), "Warnings", 13, False)

        ' Mga nakatagong detalye (nasa Column Chooser): bases at employer share
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSMPFEE), "SSS MPF (EE)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.TaxGross), "Tax Gross")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSBase), "SSS Base (this cutoff)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSLookupBase), "SSS Base (month)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.PhilHealthBase), "PhilHealth Base")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.PagIbigBase), "Pag-IBIG Base")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSER), "SSS (ER)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSMPFER), "SSS MPF (ER)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.ECC), "ECC (ER)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.PhilHealthER), "PhilHealth (ER)")
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.PagIbigER), "Pag-IBIG (ER)")
        gridView.OptionsView.ColumnAutoWidth = False
        gridView.OptionsBehavior.Editable = False

        ' ---- Detalye kada linya ng napiling empleyado ----
        gridViewLines.Columns.Clear()
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.SeqNo), "#", 0, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.LineType), "Type", 1, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Category), "Category", 2, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Code), "Code", 3, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Description), "Description", 4, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Qty), "Qty", 5, True)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Unit), "Unit", 6, False)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.RateUsed), "Rate Used", 7, True)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Amount), "Amount", 8, True)
        AddColumn(gridViewLines, NameOf(PayrollResultLineModel.Remark), "Remark", 9, False)
        gridViewLines.OptionsBehavior.Editable = False
        gridViewLines.OptionsView.ShowGroupPanel = False
    End Sub

    Private Shared Sub AddColumn(view As DevExpress.XtraGrid.Views.Grid.GridView, fieldName As String, caption As String,
                                 index As Integer, numeric As Boolean)
        Dim col = view.Columns.AddField(fieldName)
        col.Caption = caption
        col.Visible = True
        col.VisibleIndex = index
        col.OptionsColumn.AllowEdit = False
        If numeric Then
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            col.DisplayFormat.FormatString = "N2"
        End If
    End Sub

    Private Shared Sub AddHiddenColumn(view As DevExpress.XtraGrid.Views.Grid.GridView, fieldName As String, caption As String)
        Dim col = view.Columns.AddField(fieldName)
        col.Caption = caption
        col.Visible = False
        col.OptionsColumn.AllowEdit = False
        col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        col.DisplayFormat.FormatString = "N2"
    End Sub

    Private Sub gridView_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gridView.FocusedRowChanged
        ShowLinesOfFocusedRow()
    End Sub

    Private Sub ShowLinesOfFocusedRow()
        Dim row = TryCast(gridView.GetFocusedRow(), PayrollResultModel)
        If row Is Nothing Then
            gridControlLines.DataSource = New List(Of PayrollResultLineModel)()
            Return
        End If

        gridControlLines.DataSource = _lines.Where(Function(l) l.ResultId = row.ResultId).OrderBy(Function(l) l.SeqNo).ToList()
        gridViewLines.BestFitColumns()
    End Sub

#End Region

    Private Async Sub cboCutoff_EditValueChanged(sender As Object, e As EventArgs) Handles cboCutoff.EditValueChanged
        If _loadingCutoffs Then Return
        Dim id = GetSelectedCutoffId()
        If id.HasValue Then
            Await LoadSelectedAsync(id.Value)
        Else
            ClearResults()
        End If
    End Sub

    Private Async Function LoadSelectedAsync(cutoffId As Integer) As Task
        Try
            Await _presenter.LoadCutoffAsync(cutoffId)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Function

    Private Sub btnExportConverter_Click(sender As Object, e As EventArgs) Handles btnExportConverter.Click
        ShowMessage("Export format para sa BDO converter - pending pa ang format mula sa gagamit ng system.")
    End Sub

    Private Sub btnExportDirect_Click(sender As Object, e As EventArgs) Handles btnExportDirect.Click
        ShowMessage("Direct-to-BDO export format - pending pa ang format mula sa gagamit ng system.")
    End Sub

End Class
