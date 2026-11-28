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
    Private _cutoffs As New List(Of CutoffModel)()              ' mode: per-cutoff
    Private _groups As New List(Of PayDateGroupModel)()         ' mode: Group by pay date
    Private _groupMode As Boolean = False

    ''' <summary>
    ''' Para sa Reports button: itatakda ng Shell kapag nagawa na ang Reports module (parameter = mga napiling CutoffID).
    ''' Hangga't Nothing, magpapakita lang ng "not available yet".
    ''' </summary>
    Public Property OpenReportsAction As Action(Of List(Of Integer))

    ''' <summary>Pareho, para sa Payslips button (Payslip module).</summary>
    Public Property OpenPayslipAction As Action(Of List(Of Integer))

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
        Dim previous As Object = If(_groupMode, Nothing, cboCutoff.EditValue)
        _groupMode = False
        _cutoffs = If(cutoffs, New List(Of CutoffModel)())
        _groups = New List(Of PayDateGroupModel)()

        lblCutoff.Text = "Cutoff:"
        BindCombo(_cutoffs, NameOf(CutoffModel.DisplayLabel), NameOf(CutoffModel.CutoffID), "Cutoff", previous,
                  If(_cutoffs.Count > 0, CObj(_cutoffs(0).CutoffID), Nothing))      ' pinakabago ang una
        RefreshSelection()
    End Sub

    Public Sub DisplayPayDates(groups As List(Of PayDateGroupModel)) Implements IPayrollOutputView.DisplayPayDates
        Dim previous As Object = If(_groupMode, cboCutoff.EditValue, Nothing)
        _groupMode = True
        _groups = If(groups, New List(Of PayDateGroupModel)())
        _cutoffs = New List(Of CutoffModel)()

        lblCutoff.Text = "Pay date:"
        BindCombo(_groups, NameOf(PayDateGroupModel.DisplayLabel), NameOf(PayDateGroupModel.GroupKey), "Pay date", previous,
                  If(_groups.Count > 0, CObj(_groups(0).GroupKey), Nothing))
        RefreshSelection()
    End Sub

    Private Sub BindCombo(source As Object, displayMember As String, valueMember As String, caption As String,
                          previous As Object, firstValue As Object)
        _loadingCutoffs = True
        Try
            cboCutoff.Properties.DataSource = source
            cboCutoff.Properties.DisplayMember = displayMember
            cboCutoff.Properties.ValueMember = valueMember
            cboCutoff.Properties.Columns.Clear()
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(displayMember, caption))

            Dim keep As Boolean = False
            If previous IsNot Nothing AndAlso previous IsNot DBNull.Value Then
                If _groupMode Then
                    keep = _groups.Any(Function(g) g.GroupKey = CStr(previous))
                Else
                    keep = _cutoffs.Any(Function(c) c.CutoffID = CInt(previous))
                End If
            End If
            cboCutoff.EditValue = If(keep, previous, firstValue)
        Finally
            _loadingCutoffs = False
        End Try
    End Sub

    ' Laging i-refresh ang results ng napili (maaaring na-re-process sa Input tab).
    Private Sub RefreshSelection()
        Dim ids = GetSelectedCutoffIds()
        If ids.Count > 0 Then
            Dim ignore = LoadSelectionAsync(ids)
        Else
            ClearResults()
        End If
    End Sub

    Public Sub DisplayResults(headers As List(Of PayrollResultModel), lines As List(Of PayrollResultLineModel)) _
        Implements IPayrollOutputView.DisplayResults

        _headers = If(headers, New List(Of PayrollResultModel)())
        _lines = If(lines, New List(Of PayrollResultLineModel)())

        gridControl.DataSource = _headers

        Dim cutoffCol = gridView.Columns.ColumnByFieldName(NameOf(PayrollResultModel.CutoffLabel))
        If cutoffCol IsNot Nothing Then
            cutoffCol.Visible = _groupMode
            If _groupMode Then cutoffCol.VisibleIndex = 2
        End If
        gridView.BestFitColumns()

        Dim gross = _headers.Sum(Function(h) h.GrossPay)
        Dim deductions = _headers.Sum(Function(h) h.TotalDeduction)
        Dim net = _headers.Sum(Function(h) h.NetPay)
        Dim withWarnings = _headers.Where(Function(h) h.HasWarnings).Count()

        lblSummary.Text = $"Employees: {_headers.Count}    Gross: {gross:N2}    Deductions: {deductions:N2}    Net: {net:N2}" &
                          If(withWarnings > 0, $"    With warnings: {withWarnings}", "")

        ShowLinesOfFocusedRow()
        UpdateActionState()
    End Sub

    Public Sub ClearResults() Implements IPayrollOutputView.ClearResults
        _headers = New List(Of PayrollResultModel)()
        _lines = New List(Of PayrollResultLineModel)()
        gridControl.DataSource = _headers
        gridControlLines.DataSource = New List(Of PayrollResultLineModel)()
        lblSummary.Text = "No processed results yet. Use Process in the Input tab."
        UpdateActionState()
    End Sub

    Public Function IsGroupByPayDate() As Boolean Implements IPayrollOutputView.IsGroupByPayDate
        Return chkCombine.Checked
    End Function

    ' Mga cutoff ng napili: isa sa per-cutoff mode, o lahat ng cutoff ng napiling pay date.
    Private Function SelectedCutoffList() As List(Of CutoffModel)
        Dim value = cboCutoff.EditValue
        If value Is Nothing OrElse value Is DBNull.Value Then Return New List(Of CutoffModel)()

        If _groupMode Then
            Dim key = CStr(value)
            Dim grp = _groups.FirstOrDefault(Function(g) g.GroupKey = key)
            Return If(grp Is Nothing, New List(Of CutoffModel)(), grp.Cutoffs)
        End If

        Dim id = CInt(value)
        Return _cutoffs.Where(Function(c) c.CutoffID = id).ToList()
    End Function

    Public Function GetSelectedCutoffIds() As List(Of Integer) Implements IPayrollOutputView.GetSelectedCutoffIds
        Return SelectedCutoffList().Select(Function(c) c.CutoffID).ToList()
    End Function

    Public Function GetSelectedPayDate() As Date? Implements IPayrollOutputView.GetSelectedPayDate
        If Not _groupMode Then Return Nothing
        Dim value = cboCutoff.EditValue
        If value Is Nothing OrElse value Is DBNull.Value Then Return Nothing
        Dim key = CStr(value)
        Dim grp = _groups.FirstOrDefault(Function(g) g.GroupKey = key)
        If grp Is Nothing Then Return Nothing
        Return grp.PayDate
    End Function

    Public Function AskConfirm(message As String, title As String) As Boolean Implements IPayrollOutputView.AskConfirm
        Return DevExpress.XtraEditors.XtraMessageBox.Show(message, title, MessageBoxButtons.OKCancel,
                                                          MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK
    End Function

    Public Function AskInput(prompt As String, title As String, defaultValue As String) As String Implements IPayrollOutputView.AskInput
        Return DevExpress.XtraEditors.XtraInputBox.Show(prompt, title, defaultValue)
    End Function

    Public Function AskSaveFile(defaultFileName As String, filter As String, defaultExt As String) As String Implements IPayrollOutputView.AskSaveFile
        Using dlg As New SaveFileDialog With {
            .Title = "Save export file",
            .Filter = filter,
            .FileName = defaultFileName,
            .DefaultExt = defaultExt,
            .AddExtension = True,
            .OverwritePrompt = True
        }
            Return If(dlg.ShowDialog() = DialogResult.OK, dlg.FileName, Nothing)
        End Using
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
        AddColumn(gridView, NameOf(PayrollResultModel.OtherEarnings), "Other Earnings", 3, True)   ' OT, Holiday, Compensation, Bonus
        AddColumn(gridView, NameOf(PayrollResultModel.GrossPay), "Gross Pay", 4, True)
        AddColumn(gridView, NameOf(PayrollResultModel.OtherDeduction), "Other Deductions", 5, True)
        AddColumn(gridView, NameOf(PayrollResultModel.SSSTotalEE), "SSS", 6, True)       ' EE + MPF
        AddColumn(gridView, NameOf(PayrollResultModel.PhilHealthEE), "PhilHealth", 7, True)
        AddColumn(gridView, NameOf(PayrollResultModel.PagIbigEE), "Pag-IBIG", 8, True)
        AddColumn(gridView, NameOf(PayrollResultModel.TaxableIncome), "Taxable Income", 9, True)
        AddColumn(gridView, NameOf(PayrollResultModel.WithholdingTax), "Tax", 10, True)
        AddColumn(gridView, NameOf(PayrollResultModel.LoanDeduction), "Loans", 11, True)
        AddColumn(gridView, NameOf(PayrollResultModel.TotalDeduction), "Total Deductions", 12, True)
        AddColumn(gridView, NameOf(PayrollResultModel.NetPay), "Net Pay", 13, True)
        AddColumn(gridView, NameOf(PayrollResultModel.WarningText), "Warnings", 14, False)

        ' Mga nakatagong detalye (nasa Column Chooser): bases at employer share
        AddHiddenColumn(gridView, NameOf(PayrollResultModel.SSSEE), "SSS (EE only)")
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
        ' "Cutoff" column: lumalabas lang sa "Group by pay date" (kinokontrol sa DisplayResults)
        Dim cutoffCol = gridView.Columns.AddField(NameOf(PayrollResultModel.CutoffLabel))
        cutoffCol.Caption = "Cutoff"
        cutoffCol.Visible = False
        cutoffCol.OptionsColumn.AllowEdit = False

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
        Dim ids = GetSelectedCutoffIds()
        If ids.Count > 0 Then
            Await LoadSelectionAsync(ids)
        Else
            ClearResults()
        End If
    End Sub

    Private Async Function LoadSelectionAsync(cutoffIds As List(Of Integer)) As Task
        Try
            Await _presenter.LoadSelectionAsync(cutoffIds)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Function

    ' Pag-toggle ng "Group by pay date": ibang laman ang combo (mga pay date vs mga cutoff).
    Private Async Sub chkCombine_CheckedChanged(sender As Object, e As EventArgs) Handles chkCombine.CheckedChanged
        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    ' Naka-enable lang ang mga button kapag may napili na may results; ang Post ay Admin lang at kapag may Processed na cutoff.
    Private Sub UpdateActionState()
        Dim selected = SelectedCutoffList()
        Dim hasRows = selected.Count > 0 AndAlso _headers.Count > 0

        btnExportConverter.Enabled = hasRows
        btnExportDirect.Enabled = hasRows
        btnExportOther.Enabled = hasRows
        btnPost.Enabled = hasRows AndAlso HasEditAccess AndAlso AppSession.IsAdmin AndAlso
                          selected.Any(Function(c) c.Status = CutoffStatus.Processed)
        btnPost.Text = If(_groupMode, "Post Pay Date", "Post Cutoff")
        btnReports.Enabled = selected.Count > 0
        btnPayslip.Enabled = selected.Count > 0

        If selected.Count = 0 Then
            lblStatus.Text = String.Empty
            lblStatus.Appearance.ForeColor = System.Drawing.Color.Empty
        ElseIf Not _groupMode Then
            Dim c = selected(0)
            If c.Status = CutoffStatus.Posted Then
                lblStatus.Text = "Status: POSTED" &
                                 If(String.IsNullOrWhiteSpace(c.PostedBy), "", " by " & c.PostedBy) &
                                 If(c.PostedDate.HasValue, " on " & c.PostedDate.Value.ToString("MMM d, yyyy h:mm tt"), "")
                lblStatus.Appearance.ForeColor = System.Drawing.Color.SeaGreen
            Else
                lblStatus.Text = "Status: " & c.Status.ToString() & If(c.Status = CutoffStatus.Processed, " (not yet posted)", "")
                lblStatus.Appearance.ForeColor = System.Drawing.Color.Empty
            End If
        Else
            Dim parts = selected.GroupBy(Function(c) c.Status).Select(Function(g) g.Count() & " " & g.Key.ToString())
            lblStatus.Text = $"{selected.Count} cutoff(s): " & String.Join(", ", parts)
            lblStatus.Appearance.ForeColor = If(selected.All(Function(c) c.Status = CutoffStatus.Posted),
                                                System.Drawing.Color.SeaGreen, System.Drawing.Color.Empty)
        End If
    End Sub

    Private Async Sub btnExportConverter_Click(sender As Object, e As EventArgs) Handles btnExportConverter.Click
        Await _presenter.ExportAsync(ExportFormat.BdoConverter)
    End Sub

    Private Async Sub btnExportDirect_Click(sender As Object, e As EventArgs) Handles btnExportDirect.Click
        Await _presenter.ExportAsync(ExportFormat.BdoDirect)
    End Sub

    Private Async Sub btnExportOther_Click(sender As Object, e As EventArgs) Handles btnExportOther.Click
        Await _presenter.ExportAsync(ExportFormat.OtherLists)
    End Sub

    Private Async Sub btnPost_Click(sender As Object, e As EventArgs) Handles btnPost.Click
        Await _presenter.PostAsync()
    End Sub

    Private Sub btnReports_Click(sender As Object, e As EventArgs) Handles btnReports.Click
        If OpenReportsAction Is Nothing Then
            ShowMessage("The Reports module is not available yet. Reports will be generated from Posted cutoffs.")
        Else
            OpenReportsAction.Invoke(GetSelectedCutoffIds())
        End If
    End Sub

    Private Sub btnPayslip_Click(sender As Object, e As EventArgs) Handles btnPayslip.Click
        If OpenPayslipAction Is Nothing Then
            ShowMessage("The Payslip module is not available yet. Payslips will be generated from Posted cutoffs.")
        Else
            OpenPayslipAction.Invoke(GetSelectedCutoffIds())
        End If
    End Sub

End Class
