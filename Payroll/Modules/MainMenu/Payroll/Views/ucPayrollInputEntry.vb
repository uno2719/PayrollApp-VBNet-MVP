Imports Payroll.GlobalShared.Models
Imports System.Data
Imports Payroll.GlobalShared.Constants
Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollProcessing.Presenters
Imports Payroll.PayrollProcessing.Views

Public Class ucPayrollInputEntry
    Implements IPayrollInputEntryView
    Implements IAsyncLoadable

    ' Assigned by AppComposition after both View and Presenter are constructed —
    ' same wiring pattern as ucLookupMaintenance/LookupPresenter.
    Private _presenter As PayrollInputEntryPresenter
    Private _table As DataTable
    Private _warnings As New List(Of GlobalShared.Models.PayrollEligibilityWarning)()
    Private _cutoffs As New List(Of CutoffModel)()
    Private _loadingCutoffs As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetPresenter(presenter As PayrollInputEntryPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Payroll > Input"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payroll Input Entry"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Main_Payroll
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _presenter.LoadAsync()
    End Function

    Private Async Sub cboCutoff_EditValueChanged(sender As Object, e As EventArgs) Handles cboCutoff.EditValueChanged
        If _loadingCutoffs Then Return
        Dim cutoffId = GetSelectedCutoffId()
        If cutoffId.HasValue Then
            ShowLoading()
            Try
                Await _presenter.LoadCutoffDataAsync(cutoffId.Value)
            Finally
                HideLoading()
            End Try
        End If
    End Sub

    Private Async Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        ' Save inputs -> compute (in memory) -> Confirm -> save results (Draft/Processed -> Processed).
        ShowLoading()
        Try
            Await _presenter.ProcessAsync()
        Finally
            HideLoading()
        End Try
    End Sub

    Private Sub btnColumns_Click(sender As Object, e As EventArgs) Handles btnColumns.Click
        gridView.ShowCustomization()
    End Sub

#Region "IPayrollInputEntryView"

    Public Sub DisplayCutoffs(cutoffs As List(Of CutoffModel)) Implements IPayrollInputEntryView.DisplayCutoffs
        Dim previous = GetSelectedCutoffId()
        _cutoffs = If(cutoffs, New List(Of CutoffModel)())

        _loadingCutoffs = True
        Try
            cboCutoff.Properties.DataSource = _cutoffs
            cboCutoff.Properties.DisplayMember = NameOf(CutoffModel.DisplayLabel)
            cboCutoff.Properties.ValueMember = NameOf(CutoffModel.CutoffID)
            cboCutoff.Properties.Columns.Clear()
            cboCutoff.Properties.Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(CutoffModel.DisplayLabel), "Cutoff"))

            ' Kapag wala na sa listahan ang napili (Closed o Posted na): i-clear ang pagpili at ang grid.
            ' Kapag nandoon pa, hindi ginagalaw ang grid para hindi mawala ang mga hindi pa na-save na input.
            If previous.HasValue AndAlso Not _cutoffs.Any(Function(c) c.CutoffID = previous.Value) Then
                cboCutoff.EditValue = Nothing
                _table = Nothing
                gridControl.DataSource = Nothing
                _warnings = New List(Of GlobalShared.Models.PayrollEligibilityWarning)()
                pnlWarnings.Visible = False
            End If
        Finally
            _loadingCutoffs = False
        End Try

        UpdateCloseButton()
    End Sub

    ' Lumalabas lang ang "Close this cutoff" kapag Draft ang napiling cutoff at WALANG empleyadong lumabas sa grid
    ' (at walang empleyadong hindi nakasama dahil kulang ang setup).
    Private Sub UpdateCloseButton()
        Dim id = GetSelectedCutoffId()
        Dim cutoff As CutoffModel = Nothing
        If id.HasValue Then cutoff = _cutoffs.FirstOrDefault(Function(c) c.CutoffID = id.Value)

        Dim isEmpty = _table IsNot Nothing AndAlso _table.Rows.Count = 0 AndAlso _warnings.Count = 0
        btnCloseCutoff.Visible = cutoff IsNot Nothing AndAlso cutoff.Status = CutoffStatus.Draft AndAlso isEmpty AndAlso HasEditAccess
    End Sub

    ''' <summary>Tinatawag ng ucPayroll kapag bumalik sa Input tab (hal. pagkatapos mag-Post sa Output).</summary>
    Public Async Function RefreshCutoffsAsync() As Task
        If _presenter Is Nothing Then Return
        Try
            Await _presenter.RefreshCutoffsAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Function

    Private Async Sub btnCloseCutoff_Click(sender As Object, e As EventArgs) Handles btnCloseCutoff.Click
        ShowLoading()
        Try
            Await _presenter.CloseEmptyCutoffAsync()
        Finally
            HideLoading()
        End Try
    End Sub

    Public Function AskConfirm(message As String, title As String) As Boolean Implements IPayrollInputEntryView.AskConfirm
        HideLoading()
        Dim answer = DevExpress.XtraEditors.XtraMessageBox.Show(message, title, MessageBoxButtons.OKCancel,
                                                                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        If answer = DialogResult.OK Then ShowLoading()
        Return answer = DialogResult.OK
    End Function

    Public Sub DisplayColumns(columns As List(Of PayrollInputColumnModel)) Implements IPayrollInputEntryView.DisplayColumns
        ' Tinatawag ulit ito kapag nagbago ang set ng columns (hal. may Days Worked na) - alisin muna ang
        ' lumang data para hindi mag-auto-create ng columns ang grid; susunod ang DisplayData().
        gridControl.DataSource = Nothing
        _table = Nothing

        gridView.Columns.Clear()

        AddReferenceColumn("EmployeeNo", "Employee No", 0)
        AddReferenceColumn("EmployeeName", "Name", 1)

        Dim index = 2
        For Each col In columns
            Dim gridCol = gridView.Columns.AddField(col.ColumnName)
            gridCol.Caption = col.Caption
            gridCol.Visible = col.IsEssential          ' essential columns shown by default; rest start hidden
            gridCol.VisibleIndex = If(col.IsEssential, index, -1)
            gridCol.OptionsColumn.AllowEdit = True
            gridCol.OptionsColumn.ShowInCustomizationForm = True  ' lets the user pull hidden ones back in via Columns...
            gridCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            gridCol.DisplayFormat.FormatString = "N2"
            If col.IsEssential Then index += 1
        Next

        ' RecordId stays in the DataTable for save-back, but never shown.
        Dim recordIdCol = gridView.Columns.AddField("RecordId")
        recordIdCol.Visible = False
        recordIdCol.OptionsColumn.ShowInCustomizationForm = False
    End Sub

    Private Sub AddReferenceColumn(fieldName As String, caption As String, visibleIndex As Integer)
        Dim col = gridView.Columns.AddField(fieldName)
        col.Caption = caption
        col.Visible = True
        col.VisibleIndex = visibleIndex
        col.OptionsColumn.AllowEdit = False
        col.OptionsColumn.ShowInCustomizationForm = False
        col.OptionsColumn.AllowShowHide = False     ' the actual fix — ShowInCustomizationForm alone still let the
        ' quick-hide "x" on the column header remove these two columns
    End Sub

    Public Sub DisplayData(table As DataTable) Implements IPayrollInputEntryView.DisplayData
        _table = table
        gridControl.DataSource = _table
        UpdateCloseButton()
    End Sub

    Public Sub DisplayEligibilityWarnings(warnings As List(Of GlobalShared.Models.PayrollEligibilityWarning)) _
        Implements IPayrollInputEntryView.DisplayEligibilityWarnings

        _warnings = If(warnings, New List(Of GlobalShared.Models.PayrollEligibilityWarning)())
        pnlWarnings.Visible = _warnings.Count > 0
        If _warnings.Count > 0 Then
            lnkWarnings.Text = $"{_warnings.Count} employee(s) can't be included because their setup is incomplete. Click to see who."
        End If
        UpdateCloseButton()
    End Sub

    Private Sub lnkWarnings_Click(sender As Object, e As EventArgs) Handles lnkWarnings.Click
        If _warnings.Count = 0 Then Return

        Const MaxLines As Integer = 15
        Dim lines = _warnings.Take(MaxLines).Select(Function(w) $"{w.EmployeeNo}  {w.EmployeeName}: {w.Reason}").ToList()
        If _warnings.Count > MaxLines Then lines.Add($"...and {_warnings.Count - MaxLines} more.")

        DevExpress.XtraEditors.XtraMessageBox.Show(
            "These employees are not in the list below until their setup is completed:" & Environment.NewLine & Environment.NewLine &
            String.Join(Environment.NewLine, lines),
            "Incomplete employee setup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Public Function GetEditedData() As DataTable Implements IPayrollInputEntryView.GetEditedData
        gridView.CloseEditor()
        gridView.UpdateCurrentRow()
        Return _table
    End Function

    Public Function GetSelectedCutoffId() As Integer? Implements IPayrollInputEntryView.GetSelectedCutoffId
        If cboCutoff.EditValue Is Nothing OrElse cboCutoff.EditValue Is DBNull.Value Then Return Nothing
        Return CInt(cboCutoff.EditValue)
    End Function

    ' Renamed locally (DisplayInfo/DisplayValidationError) to avoid clashing with
    ' ucBase's own inherited ShowMessage/ShowError — same pattern as ucLookupMaintenance.
    Public Function ConfirmProcess(summary As String) As Boolean Implements IPayrollInputEntryView.ConfirmProcess
        ' Naka-hide muna ang loading overlay habang naghihintay ng sagot ng user.
        HideLoading()
        Dim answer = DevExpress.XtraEditors.XtraMessageBox.Show(summary, "Process Payroll",
                                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If answer = DialogResult.Yes Then ShowLoading()
        Return answer = DialogResult.Yes
    End Function

    Public Sub DisplayInfo(message As String) Implements IPayrollInputEntryView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IPayrollInputEntryView.ShowError
        ShowError(message)
    End Sub

#End Region

End Class