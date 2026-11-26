Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.Payslips.Presenters
Imports Payroll.Payslips.Views

''' <summary>
''' Reports > Payslip. Kaliwa: mga empleyado ng cutoff (checkbox). Kanan: preview.
''' Ang mga aksyon ay para sa mga naka-check; kapag walang naka-check, LAHAT ng empleyado sa cutoff.
''' </summary>
Public Class ucPayslip
    Implements IPayslipView
    Implements IAsyncLoadable

    Private _presenter As PayslipPresenter
    Private _cutoffs As New List(Of CutoffModel)()
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
    ''' ang cutoff. Ligtas kahit sabay sa unang load ng screen: pinapanatili ng DisplayCutoffs ang napili kung nandoon pa.
    ''' </summary>
    Public Async Function FocusCutoffAsync(cutoffIds As List(Of Integer)) As Task
        Dim id As Integer? = Nothing
        If cutoffIds IsNot Nothing AndAlso cutoffIds.Count > 0 Then id = cutoffIds(0)

        BuildGridColumns()
        Await _presenter.LoadAsync(id)

        If cutoffIds IsNot Nothing AndAlso cutoffIds.Count > 1 Then
            ShowMessage("Payslips are generated per cutoff. Showing the first cutoff of the selected pay date; pick the others from the Cutoff list.")
        End If
    End Function

#Region "IPayslipView"

    Public Sub DisplayCutoffs(cutoffs As List(Of CutoffModel), selectCutoffId As Integer?) Implements IPayslipView.DisplayCutoffs
        Dim previous = cboCutoff.EditValue
        _cutoffs = If(cutoffs, New List(Of CutoffModel)())

        _loadingCutoffs = True
        Try
            cboCutoff.Properties.DataSource = _cutoffs
            cboCutoff.Properties.DisplayMember = NameOf(CutoffModel.DisplayLabel)
            cboCutoff.Properties.ValueMember = NameOf(CutoffModel.CutoffID)
            cboCutoff.Properties.Columns.Clear()
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.DisplayLabel), "Cutoff"))
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.Status), "Status"))

            Dim pick As Object = Nothing
            If selectCutoffId.HasValue AndAlso _cutoffs.Any(Function(c) c.CutoffID = selectCutoffId.Value) Then
                pick = selectCutoffId.Value
            ElseIf previous IsNot Nothing AndAlso previous IsNot DBNull.Value AndAlso _cutoffs.Any(Function(c) c.CutoffID = CInt(previous)) Then
                pick = previous
            ElseIf _cutoffs.Count > 0 Then
                pick = _cutoffs(0).CutoffID
            End If
            cboCutoff.EditValue = pick
        Finally
            _loadingCutoffs = False
        End Try

        Dim ignore = ReloadEmployeesAsync()
    End Sub

    Public Sub DisplayEmployees(rows As List(Of PayslipEmployeeRow)) Implements IPayslipView.DisplayEmployees
        _rows = If(rows, New List(Of PayslipEmployeeRow)())
        gridControl.DataSource = _rows
        gridView.BestFitColumns()
        UpdateState()
    End Sub

    Public Function GetSelectedCutoff() As CutoffModel Implements IPayslipView.GetSelectedCutoff
        Dim value = cboCutoff.EditValue
        If value Is Nothing OrElse value Is DBNull.Value Then Return Nothing
        Dim id = CInt(value)
        Return _cutoffs.FirstOrDefault(Function(c) c.CutoffID = id)
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
        Dim c = GetSelectedCutoff()
        Dim hasRows = c IsNot Nothing AndAlso _rows.Count > 0
        Dim sending = _presenter IsNot Nothing AndAlso _presenter.IsSending
        Dim idle = hasRows AndAlso Not sending AndAlso Not _busy

        btnPreview.Enabled = idle
        btnOpenViewer.Enabled = idle
        btnPrint.Enabled = idle
        btnExportPdf.Enabled = idle
        btnExportEach.Enabled = idle
        btnCheckAll.Enabled = hasRows AndAlso Not sending
        btnUncheckAll.Enabled = hasRows AndAlso Not sending
        cboCutoff.Enabled = Not sending
        btnRefresh.Enabled = Not sending AndAlso Not _busy

        Dim canEmail = idle AndAlso c.Status = CutoffStatus.Posted AndAlso HasEditAccess
        btnEmail.Enabled = canEmail
        btnEmailOne.Enabled = canEmail

        ' Status ng cutoff
        If c Is Nothing Then
            lblStatus.Text = String.Empty
        ElseIf c.Status = CutoffStatus.Posted Then
            lblStatus.Text = "POSTED"
            lblStatus.Appearance.ForeColor = Color.SeaGreen
        Else
            lblStatus.Text = c.Status.ToString().ToUpperInvariant() & " - payslips are DRAFT (email is for Posted cutoffs only)"
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
