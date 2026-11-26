Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.Payslips.Presenters
Imports Payroll.Payslips.Views

''' <summary>
''' Reports > Payslip. Kaliwa: mga empleyado ng cutoff, o ng lahat ng cutoff ng isang pay date (Group by pay date).
''' Kanan: preview. Ang mga aksyon ay para sa mga naka-check; kapag walang naka-check, LAHAT ng empleyado sa napili.
''' </summary>
Public Class ucPayslip
    Implements IPayslipView
    Implements IAsyncLoadable

    Private _presenter As PayslipPresenter
    Private _cutoffs As New List(Of CutoffModel)()                ' mode: per cutoff
    Private _groups As New List(Of PayDateGroupModel)()          ' mode: Group by pay date
    Private _groupMode As Boolean = False
    Private _settingGroup As Boolean = False
    Private _rows As New List(Of PayslipEmployeeRow)()
    Private _columnsBuilt As Boolean = False
    Private _loadingCutoffs As Boolean = False
    Private _busy As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetPresenter(presenter As PayslipPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Reports > Payslip"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payslip"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Reports_Payslip
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        BuildGridColumns()
        Await _presenter.LoadAsync()
    End Function

    ''' <summary>
    ''' Tinatawag ng Shell mula sa Payroll > Output > Payslips: i-reload ang listahan (baka kaka-Process/Post lang) at piliin
    ''' ang cutoff. Kapag maraming cutoff (galing sa "Group by pay date" ng Output), lilipat din dito sa Group by pay date.
    ''' Ligtas kahit sabay sa unang load ng screen: pinapanatili ng combo ang napili kung nandoon pa.
    ''' </summary>
    Public Async Function FocusCutoffAsync(cutoffIds As List(Of Integer)) As Task
        BuildGridColumns()
        SetGroupByPayDate(cutoffIds IsNot Nothing AndAlso cutoffIds.Count > 1)
        Await _presenter.LoadAsync(cutoffIds)
    End Function

#Region "IPayslipView"

    Public Sub DisplayCutoffs(cutoffs As List(Of CutoffModel), selectCutoffIds As List(Of Integer)) Implements IPayslipView.DisplayCutoffs
        Dim previous As Object = If(_groupMode, Nothing, cboCutoff.EditValue)
        _groupMode = False
        _cutoffs = If(cutoffs, New List(Of CutoffModel)())
        _groups = New List(Of PayDateGroupModel)()

        Dim pick As Object = Nothing
        Dim wanted = If(selectCutoffIds, New List(Of Integer)())
        Dim hit = _cutoffs.FirstOrDefault(Function(c) wanted.Contains(c.CutoffID))
        If hit IsNot Nothing Then
            pick = hit.CutoffID
        ElseIf previous IsNot Nothing AndAlso previous IsNot DBNull.Value AndAlso _cutoffs.Any(Function(c) c.CutoffID = CInt(previous)) Then
            pick = previous
        ElseIf _cutoffs.Count > 0 Then
            pick = _cutoffs(0).CutoffID                                   ' pinakabago ang una
        End If

        lblCutoff.Text = "Cutoff:"
        BindCombo(_cutoffs, NameOf(CutoffModel.DisplayLabel), NameOf(CutoffModel.CutoffID), "Cutoff", pick, includeStatus:=True)
    End Sub

    Public Sub DisplayPayDates(groups As List(Of PayDateGroupModel), selectCutoffIds As List(Of Integer)) Implements IPayslipView.DisplayPayDates
        Dim previous As Object = If(_groupMode, cboCutoff.EditValue, Nothing)
        _groupMode = True
        _groups = If(groups, New List(Of PayDateGroupModel)())
        _cutoffs = New List(Of CutoffModel)()

        Dim pick As Object = Nothing
        Dim wanted = If(selectCutoffIds, New List(Of Integer)())
        Dim hit = _groups.FirstOrDefault(Function(g) g.Cutoffs.Any(Function(c) wanted.Contains(c.CutoffID)))
        If hit IsNot Nothing Then
            pick = hit.GroupKey
        ElseIf previous IsNot Nothing AndAlso previous IsNot DBNull.Value AndAlso _groups.Any(Function(g) g.GroupKey = CStr(previous)) Then
            pick = previous
        ElseIf _groups.Count > 0 Then
            pick = _groups(0).GroupKey
        End If

        lblCutoff.Text = "Pay date:"
        BindCombo(_groups, NameOf(PayDateGroupModel.DisplayLabel), NameOf(PayDateGroupModel.GroupKey), "Pay date", pick, includeStatus:=False)
    End Sub

    Private Sub BindCombo(source As Object, displayMember As String, valueMember As String, caption As String,
                          pick As Object, includeStatus As Boolean)
        _loadingCutoffs = True
        Try
            cboCutoff.EditValue = Nothing
            cboCutoff.Properties.DataSource = source
            cboCutoff.Properties.DisplayMember = displayMember
            cboCutoff.Properties.ValueMember = valueMember
            cboCutoff.Properties.Columns.Clear()
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(displayMember, caption))
            If includeStatus Then
                cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.Status), "Status"))
            End If
            cboCutoff.EditValue = pick
        Finally
            _loadingCutoffs = False
        End Try

        Dim colCutoff = gridView.Columns.ColumnByFieldName(NameOf(PayslipEmployeeRow.CutoffLabel))
        If colCutoff IsNot Nothing Then colCutoff.Visible = _groupMode

        Dim ignore = ReloadEmployeesAsync()
    End Sub

    Public Sub DisplayEmployees(rows As List(Of PayslipEmployeeRow)) Implements IPayslipView.DisplayEmployees
        _rows = If(rows, New List(Of PayslipEmployeeRow)())
        gridControl.DataSource = _rows
        gridView.BestFitColumns()
        UpdateState()
    End Sub

    Public Function IsGroupByPayDate() As Boolean Implements IPayslipView.IsGroupByPayDate
        Return chkGroupByPayDate.Checked
    End Function

    Public Sub SetGroupByPayDate(value As Boolean) Implements IPayslipView.SetGroupByPayDate
        _settingGroup = True
        Try
            chkGroupByPayDate.Checked = value
        Finally
            _settingGroup = False
        End Try
    End Sub

    Public Function GetSelectedCutoffs() As List(Of CutoffModel) Implements IPayslipView.GetSelectedCutoffs
        Dim value = cboCutoff.EditValue
        If value Is Nothing OrElse value Is DBNull.Value Then Return New List(Of CutoffModel)()

        If _groupMode Then
            Dim key = CStr(value)
            Dim grp = _groups.FirstOrDefault(Function(g) g.GroupKey = key)
            Return If(grp Is Nothing, New List(Of CutoffModel)(), grp.Cutoffs.ToList())
        End If

        Dim id = CInt(value)
        Return _cutoffs.Where(Function(c) c.CutoffID = id).ToList()
    End Function

    Public Function GetSelectionLabel() As String Implements IPayslipView.GetSelectionLabel
        Dim cutoffs = GetSelectedCutoffs()
        If cutoffs.Count = 0 Then Return String.Empty
        If cutoffs.Count = 1 Then Return cutoffs(0).DisplayLabel
        Return "pay date " & PayslipPresenter.EffectivePayDate(cutoffs(0)).ToString("MMM d, yyyy") & $" ({cutoffs.Count} cutoffs)"
    End Function

    Public Function GetTargetRows() As List(Of PayslipEmployeeRow) Implements IPayslipView.GetTargetRows
        gridView.CloseEditor()
        gridView.UpdateCurrentRow()
        Dim checkedRows = _rows.Where(Function(r) r.Selected).ToList()
        Return If(checkedRows.Count > 0, checkedRows, _rows.ToList())
    End Function

    Public Function GetFocusedRow() As PayslipEmployeeRow Implements IPayslipView.GetFocusedRow
        Return TryCast(gridView.GetFocusedRow(), PayslipEmployeeRow)
    End Function

    Public Function GetOptions() As PayslipOptions Implements IPayslipView.GetOptions
        Return New PayslipOptions With {
            .IncludeAllowance = chkIncludeAllowance.Checked,
            .ShowYtd = chkShowYtd.Checked,
            .ProtectPdf = chkProtectPdf.Checked}
    End Function

    Public Sub ShowPreview(report As rptPayslip) Implements IPayslipView.ShowPreview
        Dim old = TryCast(documentViewer.DocumentSource, rptPayslip)
        documentViewer.DocumentSource = report
        If old IsNot Nothing AndAlso old IsNot report Then old.Dispose()
    End Sub

    Public Sub SetBusy(isBusy As Boolean) Implements IPayslipView.SetBusy
        If isBusy = _busy Then Return
        _busy = isBusy
        If isBusy Then
            ShowLoading()
        Else
            HideLoading()
        End If
        UpdateState()
    End Sub

    Public Sub SetEmailProgress(visible As Boolean, done As Integer, total As Integer, text As String) Implements IPayslipView.SetEmailProgress
        pnlProgress.Visible = visible
        If visible Then
            progressEmail.Properties.Minimum = 0
            progressEmail.Properties.Maximum = Math.Max(1, total)
            progressEmail.Position = Math.Min(done, Math.Max(1, total))
            lblProgress.Text = text
            btnStopEmail.Enabled = True
        End If
        UpdateState()
    End Sub

    Public Function AskConfirm(message As String, title As String) As Boolean Implements IPayslipView.AskConfirm
        Return DevExpress.XtraEditors.XtraMessageBox.Show(message, title, MessageBoxButtons.OKCancel,
                                                          MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK
    End Function

    Public Function AskSaveFile(defaultFileName As String) As String Implements IPayslipView.AskSaveFile
        Using dlg As New SaveFileDialog With {
            .Title = "Save payslips (PDF)",
            .Filter = "PDF file (*.pdf)|*.pdf",
            .FileName = defaultFileName,
            .DefaultExt = "pdf",
            .AddExtension = True,
            .OverwritePrompt = True
        }
            Return If(dlg.ShowDialog() = DialogResult.OK, dlg.FileName, Nothing)
        End Using
    End Function

    Public Function AskFolder(description As String) As String Implements IPayslipView.AskFolder
        Using dlg As New FolderBrowserDialog With {.Description = description, .UseDescriptionForTitle = True, .ShowNewFolderButton = True}
            Return If(dlg.ShowDialog() = DialogResult.OK, dlg.SelectedPath, Nothing)
        End Using
    End Function

    ' Pinalitan ang pangalan para hindi sumalungat sa ShowMessage/ShowError ng ucBase (parehong pattern ng ibang view)
    Public Sub DisplayInfo(message As String) Implements IPayslipView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IPayslipView.ShowError
        ShowError(message)
    End Sub

#End Region

#Region "Grid"

    Private Sub BuildGridColumns()
        If _columnsBuilt Then Return
        _columnsBuilt = True

        gridView.Columns.Clear()

        Dim chk As New RepositoryItemCheckEdit()
        AddHandler chk.EditValueChanged, Sub(s, e) gridView.PostEditor()
        gridControl.RepositoryItems.Add(chk)

        Dim colSel = gridView.Columns.AddField(NameOf(PayslipEmployeeRow.Selected))
        colSel.Caption = " "
        colSel.ColumnEdit = chk
        colSel.Visible = True
        colSel.VisibleIndex = 0
        colSel.Width = 30
        colSel.OptionsColumn.AllowEdit = True
        colSel.OptionsColumn.FixedWidth = True
        colSel.OptionsColumn.ShowCaption = False

        AddColumn(NameOf(PayslipEmployeeRow.EmployeeNo), "Employee ID", 1, False)
        AddColumn(NameOf(PayslipEmployeeRow.EmployeeName), "Name", 2, False)
        AddColumn(NameOf(PayslipEmployeeRow.DepartmentName), "Department", 3, False)
        AddColumn(NameOf(PayslipEmployeeRow.PayBy), "Pay By", 4, False)
        AddColumn(NameOf(PayslipEmployeeRow.NetPay), "Net Pay", 5, True)
        AddColumn(NameOf(PayslipEmployeeRow.EmailAddress), "Email", 6, False)
        AddColumn(NameOf(PayslipEmployeeRow.EmailStatusText), "Email status", 7, False)
        AddColumn(NameOf(PayslipEmployeeRow.CutoffLabel), "Cutoff", 8, False)
        gridView.Columns.ColumnByFieldName(NameOf(PayslipEmployeeRow.CutoffLabel)).Visible = _groupMode

        gridView.OptionsBehavior.Editable = True
        gridView.OptionsView.ColumnAutoWidth = False
        gridView.OptionsView.ShowGroupPanel = False
        gridView.OptionsFind.AlwaysVisible = True
    End Sub

    Private Sub AddColumn(fieldName As String, caption As String, index As Integer, numeric As Boolean)
        Dim col = gridView.Columns.AddField(fieldName)
        col.Caption = caption
        col.Visible = True
        col.VisibleIndex = index
        col.OptionsColumn.AllowEdit = False
        col.OptionsColumn.AllowFocus = True
        If numeric Then
            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            col.DisplayFormat.FormatString = "N2"
        End If
    End Sub

    ' Pula ang walang email / pumalyang padala; berde ang naipadala na.
    Private Sub gridView_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles gridView.RowCellStyle
        If e.Column Is Nothing OrElse e.Column.FieldName <> NameOf(PayslipEmployeeRow.EmailStatusText) Then Return
        Dim row = TryCast(gridView.GetRow(e.RowHandle), PayslipEmployeeRow)
        If row Is Nothing Then Return
        If Not row.HasEmail OrElse (row.LastEmailDate.HasValue AndAlso Not row.LastEmailSuccess.GetValueOrDefault()) Then
            e.Appearance.ForeColor = Color.Firebrick
        ElseIf row.LastEmailSuccess.GetValueOrDefault() Then
            e.Appearance.ForeColor = Color.SeaGreen
        End If
    End Sub

    Private Sub gridView_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles gridView.CellValueChanged
        If e.Column IsNot Nothing AndAlso e.Column.FieldName = NameOf(PayslipEmployeeRow.Selected) Then UpdateState()
    End Sub

    ' Double-click sa row (hindi sa checkbox) = preview ng empleyadong iyon lang.
    Private Async Sub gridView_DoubleClick(sender As Object, e As EventArgs) Handles gridView.DoubleClick
        Dim pt = gridControl.PointToClient(Control.MousePosition)
        Dim hit = gridView.CalcHitInfo(pt)
        If Not hit.InRow OrElse (hit.Column IsNot Nothing AndAlso hit.Column.FieldName = NameOf(PayslipEmployeeRow.Selected)) Then Return
        Await RunAsync(Function() _presenter.PreviewAsync(singleFocused:=True))
    End Sub

#End Region

#Region "Events"

    Private Async Sub cboCutoff_EditValueChanged(sender As Object, e As EventArgs) Handles cboCutoff.EditValueChanged
        If _loadingCutoffs Then Return
        Await ReloadEmployeesAsync()
    End Sub

    ' Group by pay date: i-reload ang combo sa ibang mode (pinapanatili ang cutoff/pay date na napili kung kaya).
    Private Async Sub chkGroupByPayDate_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupByPayDate.CheckedChanged
        If _settingGroup OrElse _presenter Is Nothing Then Return
        Dim keep = GetSelectedCutoffs().Select(Function(c) c.CutoffID).ToList()
        Await RunAsync(Function() _presenter.LoadAsync(keep))
    End Sub

    Private Async Function ReloadEmployeesAsync() As Task
        Try
            Await _presenter.LoadEmployeesAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
        UpdateState()
    End Function

    ' Refresh: bagong Processed/Posted na cutoff at bagong email status.
    Private Async Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Await RunAsync(Function() _presenter.LoadAsync())
    End Sub

    Private Sub btnCheckAll_Click(sender As Object, e As EventArgs) Handles btnCheckAll.Click
        SetAllChecked(True)
    End Sub

    Private Sub btnUncheckAll_Click(sender As Object, e As EventArgs) Handles btnUncheckAll.Click
        SetAllChecked(False)
    End Sub

    ' "Check all" ay sumusunod sa search/filter ng grid: ang nakikitang rows lang ang chine-check.
    Private Sub SetAllChecked(value As Boolean)
        gridView.CloseEditor()
        If value Then
            For i = 0 To gridView.DataRowCount - 1
                Dim row = TryCast(gridView.GetRow(gridView.GetVisibleRowHandle(i)), PayslipEmployeeRow)
                If row IsNot Nothing Then row.Selected = True
            Next
        Else
            For Each r In _rows
                r.Selected = False
            Next
        End If
        gridView.RefreshData()
        UpdateState()
    End Sub

    Private Async Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        Await RunAsync(Function() _presenter.PreviewAsync(singleFocused:=False))
    End Sub

    Private Async Sub btnOpenViewer_Click(sender As Object, e As EventArgs) Handles btnOpenViewer.Click
        Await RunAsync(Function() _presenter.OpenViewerAsync())
    End Sub

    Private Async Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Await RunAsync(Function() _presenter.PrintAsync())
    End Sub

    Private Async Sub btnExportPdf_Click(sender As Object, e As EventArgs) Handles btnExportPdf.Click
        Await RunAsync(Function() _presenter.ExportCombinedAsync())
    End Sub

    Private Async Sub btnExportEach_Click(sender As Object, e As EventArgs) Handles btnExportEach.Click
        Await RunAsync(Function() _presenter.ExportEachAsync())
    End Sub

    Private Async Sub btnEmail_Click(sender As Object, e As EventArgs) Handles btnEmail.Click
        Await RunAsync(Function() _presenter.EmailAsync(singleFocused:=False))
    End Sub

    Private Async Sub btnEmailOne_Click(sender As Object, e As EventArgs) Handles btnEmailOne.Click
        Await RunAsync(Function() _presenter.EmailAsync(singleFocused:=True))
    End Sub

    Private Sub btnStopEmail_Click(sender As Object, e As EventArgs) Handles btnStopEmail.Click
        btnStopEmail.Enabled = False
        lblProgress.Text = "Stopping after the current email..."
        _presenter.CancelEmail()
    End Sub

    ' Huling sandigan: walang exception na makakalusot sa Async Sub (magko-crash ang app kapag nakalusot).
    Private Async Function RunAsync(action As Func(Of Task)) As Task
        Try
            Await action()
        Catch ex As Exception
            SetBusy(False)
            ShowError(ex.Message)
        End Try
        UpdateState()
    End Function

#End Region

    ' Mga button: naka-enable lang kapag may cutoff at may empleyado. Email: Posted lang, may Edit access, at walang padalang tumatakbo.
    Private Sub UpdateState()
        Dim cutoffs = GetSelectedCutoffs()
        Dim hasRows = cutoffs.Count > 0 AndAlso _rows.Count > 0
        Dim sending = _presenter IsNot Nothing AndAlso _presenter.IsSending
        Dim idle = hasRows AndAlso Not sending AndAlso Not _busy
        Dim allPosted = cutoffs.Count > 0 AndAlso cutoffs.All(Function(c) c.Status = CutoffStatus.Posted)

        btnPreview.Enabled = idle
        btnOpenViewer.Enabled = idle
        btnPrint.Enabled = idle
        btnExportPdf.Enabled = idle
        btnExportEach.Enabled = idle
        btnCheckAll.Enabled = hasRows AndAlso Not sending
        btnUncheckAll.Enabled = hasRows AndAlso Not sending
        cboCutoff.Enabled = Not sending
        chkGroupByPayDate.Enabled = Not sending AndAlso Not _busy
        btnRefresh.Enabled = Not sending AndAlso Not _busy

        Dim canEmail = idle AndAlso allPosted AndAlso HasEditAccess
        btnEmail.Enabled = canEmail
        btnEmailOne.Enabled = canEmail

        ' Status ng napili (isang cutoff, o buod ng mga cutoff ng pay date)
        If cutoffs.Count = 0 Then
            lblStatus.Text = String.Empty
        ElseIf allPosted Then
            lblStatus.Text = If(cutoffs.Count = 1, "POSTED", $"ALL {cutoffs.Count} CUTOFFS POSTED")
            lblStatus.Appearance.ForeColor = Color.SeaGreen
        ElseIf cutoffs.Count = 1 Then
            lblStatus.Text = cutoffs(0).Status.ToString().ToUpperInvariant() & " - payslips are DRAFT (email is for Posted cutoffs only)"
            lblStatus.Appearance.ForeColor = Color.DarkOrange
        Else
            Dim notPosted = cutoffs.Where(Function(c) c.Status <> CutoffStatus.Posted).Count()
            lblStatus.Text = $"{notPosted} of {cutoffs.Count} cutoffs not yet Posted - DRAFT payslips (email needs all Posted)"
            lblStatus.Appearance.ForeColor = Color.DarkOrange
        End If

        ' Saklaw ng mga aksyon
        Dim checkedCount = _rows.Where(Function(r) r.Selected).Count()
        Dim noEmail = _rows.Where(Function(r) Not r.HasEmail).Count()
        If _rows.Count = 0 Then
            lblSelection.Text = String.Empty
        Else
            lblSelection.Text = If(checkedCount > 0, $"Using {checkedCount} checked of {_rows.Count}", $"Using all {_rows.Count}") &
                                If(noEmail > 0, $"  ({noEmail} without email)", "")
        End If
    End Sub

End Class
