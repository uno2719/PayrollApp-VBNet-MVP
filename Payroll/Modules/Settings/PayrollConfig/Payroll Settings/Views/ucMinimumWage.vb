' ============================================================
' Settings > Payroll > Minimum Wage
' ============================================================
' Kaliwa:  mga minimum wage rate bawat Wage Area at Effective Date (bagong row bawat wage order - huwag palitan ang luma).
' Kanan:   ang Wage Area ng bawat store/branch (i-click ang cell; pumili o mag-type).
' Ginagamit bilang PAALALA para sa "Minimum Wage Earner" (tax-exempt): Salary adjustment, Transfer, Change tax status, at ang
' alert sa Employee header. Ang checkbox sa Earnings pa rin ang batayan ng tax (manual, gaya ng C1Pay).
' Walang Presenter (kapareho ng 201 Files tab): tumatawag lang sa MinimumWageService. Lazy load kapag nakikita.
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Models

Public Class ucMinimumWage
    Inherits DevExpress.XtraEditors.XtraUserControl

    Public Class RateRow
        Public Property MinimumWageId As Integer
        Public Property WageArea As String
        Public Property EffectiveDate As Date
        Public Property DailyRate As Decimal
        Public Property WageOrderNo As String
        Public Property Current As String
        Public Property Remarks As String
    End Class

    Private Const Title As String = "Minimum Wage"

    Private ReadOnly _service As New PayrollSettings.Services.MinimumWageService()
    Private _rates As New List(Of MinimumWageModel)()
    Private _branches As New List(Of BranchWageAreaModel)()
    Private ReadOnly _areaEditor As New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
    Private _loaded As Boolean
    Private _busy As Boolean
    Private _binding As Boolean

    Public Sub New()
        InitializeComponent()

        With gridviewRates
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

        With gridviewBranches
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
        End With

        _areaEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        _areaEditor.MaxLength = PayrollSettings.Services.MinimumWageService.MaxAreaLength
        _areaEditor.NullValuePrompt = "(not set)"
        gridBranches.RepositoryItems.Add(_areaEditor)
    End Sub

    Private Sub ucMinimumWage_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible AndAlso Not _loaded Then LoadInBackground()
    End Sub

    Private Async Sub LoadInBackground()
        Await RefreshAsync()
    End Sub

    Private Function CanEdit() As Boolean
        Return PayrollSettings.Services.MinimumWageService.CanEdit()
    End Function

    Private Sub UpdateButtons()
        Dim edit = CanEdit() AndAlso Not _busy AndAlso _loaded
        Dim sel = SelectedRate()
        btnAddRate.Enabled = edit
        btnEditRate.Enabled = edit AndAlso sel IsNot Nothing
        btnDeleteRate.Enabled = edit AndAlso sel IsNot Nothing
        gridviewBranches.OptionsBehavior.Editable = edit
    End Sub

    Private Sub SetBusy(busy As Boolean)
        _busy = busy
        UseWaitCursor = busy
        UpdateButtons()
    End Sub

    ' =============================================
    ' LOAD
    ' =============================================
    Private Async Function RefreshAsync(Optional focusId As Integer = 0) As Task
        Try
            _rates = Await _service.GetRatesAsync()
            _branches = Await _service.GetBranchesAsync()
            _loaded = True
            BindRates(focusId)
            BindBranches()
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message & vbCrLf & vbCrLf &
                                "(Run SQL/2026-11-27_Employee_Contract_MinimumWage.sql if this is the first time.)",
                                Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        UpdateButtons()
    End Function

    Private Function AreaList() As List(Of String)
        Return _rates.Select(Function(r) r.WageArea).
                      Concat(_branches.Select(Function(b) b.WageArea)).
                      Where(Function(a) Not String.IsNullOrWhiteSpace(a)).
                      Select(Function(a) a.Trim()).
                      Distinct(StringComparer.OrdinalIgnoreCase).
                      OrderBy(Function(a) a).ToList()
    End Function

    Private Sub BindRates(Optional focusId As Integer = 0)
        Dim today = Date.Today
        Dim currentIds As New HashSet(Of Integer)()
        For Each area In _rates.Select(Function(r) r.WageArea).Distinct(StringComparer.OrdinalIgnoreCase)
            Dim cur = GlobalShared.Helpers.MinimumWageRules.RateOn(_rates, area, today)
            If cur IsNot Nothing Then currentIds.Add(cur.MinimumWageId)
        Next

        Dim rows = _rates.Select(Function(r) New RateRow With {
            .MinimumWageId = r.MinimumWageId,
            .WageArea = r.WageArea,
            .EffectiveDate = r.EffectiveDate,
            .DailyRate = r.DailyRate,
            .WageOrderNo = r.WageOrderNo,
            .Remarks = r.Remarks,
            .Current = If(currentIds.Contains(r.MinimumWageId), "Yes", "")
        }).ToList()

        gridRates.DataSource = rows

        SetCaption(gridviewRates, "WageArea", "Wage Area", 150)
        SetCaption(gridviewRates, "EffectiveDate", "Effective", 80)
        SetCaption(gridviewRates, "DailyRate", "Daily Rate", 80)
        SetCaption(gridviewRates, "WageOrderNo", "Wage Order", 80)
        SetCaption(gridviewRates, "Current", "Current", 50)
        SetCaption(gridviewRates, "Remarks", "Remarks", 150)
        Dim colId = gridviewRates.Columns("MinimumWageId")
        If colId IsNot Nothing Then colId.Visible = False
        Dim colRate = gridviewRates.Columns("DailyRate")
        If colRate IsNot Nothing Then
            colRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            colRate.DisplayFormat.FormatString = "N2"
        End If
        Dim colDate = gridviewRates.Columns("EffectiveDate")
        If colDate IsNot Nothing Then
            colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            colDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        End If

        If focusId > 0 Then
            Dim index = rows.FindIndex(Function(x) x.MinimumWageId = focusId)
            If index >= 0 Then gridviewRates.FocusedRowHandle = gridviewRates.GetRowHandle(index)
        End If
    End Sub

    Private Sub BindBranches()
        _binding = True
        Try
            _areaEditor.Items.Clear()
            _areaEditor.Items.AddRange(AreaList().ToArray())

            gridBranches.DataSource = _branches
            SetCaption(gridviewBranches, "BranchCode", "Code", 70)
            SetCaption(gridviewBranches, "BranchName", "Store / Branch", 170)
            SetCaption(gridviewBranches, "WageArea", "Wage Area", 170)
            Dim colId = gridviewBranches.Columns("BranchId")
            If colId IsNot Nothing Then colId.Visible = False

            For Each col As DevExpress.XtraGrid.Columns.GridColumn In gridviewBranches.Columns
                col.OptionsColumn.AllowEdit = (col.FieldName = "WageArea")
            Next
            Dim colArea = gridviewBranches.Columns("WageArea")
            If colArea IsNot Nothing Then colArea.ColumnEdit = _areaEditor
        Finally
            _binding = False
        End Try
    End Sub

    Private Shared Sub SetCaption(view As GridView, fieldName As String, caption As String, width As Integer)
        Dim col = view.Columns(fieldName)
        If col Is Nothing Then Return
        col.Caption = caption
        col.Width = width
    End Sub

    Private Function SelectedRate() As MinimumWageModel
        Dim row = TryCast(gridviewRates.GetFocusedRow(), RateRow)
        If row Is Nothing Then Return Nothing
        Return _rates.FirstOrDefault(Function(r) r.MinimumWageId = row.MinimumWageId)
    End Function

    Private Sub gridviewRates_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewRates.FocusedRowChanged
        UpdateButtons()
    End Sub

    ' =============================================
    ' RATES: ADD / EDIT / DELETE
    ' =============================================
    Private Async Sub btnAddRate_Click(sender As Object, e As EventArgs) Handles btnAddRate.Click
        Await AddOrEditAsync(Nothing)
    End Sub

    Private Async Sub btnEditRate_Click(sender As Object, e As EventArgs) Handles btnEditRate.Click
        Dim sel = SelectedRate()
        If sel IsNot Nothing Then Await AddOrEditAsync(sel)
    End Sub

    Private Async Sub gridviewRates_DoubleClick(sender As Object, e As EventArgs) Handles gridviewRates.DoubleClick
        Dim sel = SelectedRate()
        If sel IsNot Nothing AndAlso btnEditRate.Enabled Then Await AddOrEditAsync(sel)
    End Sub

    Private Async Function AddOrEditAsync(existing As MinimumWageModel) As Task
        If _busy OrElse Not CanEdit() Then Return

        Dim result As MinimumWageModel
        Using dlg As New frmMinimumWageRate(existing, AreaList(), _rates)
            If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
            result = dlg.Result
        End Using

        SetBusy(True)
        Dim failed As String = Nothing
        Dim savedId As Integer = 0
        Try
            savedId = Await _service.SaveRateAsync(result)
        Catch ex As Exception
            failed = ex.Message
        Finally
            SetBusy(False)
        End Try

        If failed IsNot Nothing Then
            XtraMessageBox.Show(FindForm(), failed, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Await RefreshAsync(savedId)
    End Function

    Private Async Sub btnDeleteRate_Click(sender As Object, e As EventArgs) Handles btnDeleteRate.Click
        Dim sel = SelectedRate()
        If sel Is Nothing OrElse _busy Then Return

        If XtraMessageBox.Show(FindForm(),
                $"Delete {sel.DisplayText}?" & vbCrLf & vbCrLf &
                "Only delete a row that was entered by mistake. For a new wage order, ADD a new row instead (the old rate stays for past dates).",
                "Delete Minimum Wage", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Return
        End If

        SetBusy(True)
        Dim failed As String = Nothing
        Try
            Await _service.DeleteRateAsync(sel.MinimumWageId)
        Catch ex As Exception
            failed = ex.Message
        Finally
            SetBusy(False)
        End Try

        If failed IsNot Nothing Then XtraMessageBox.Show(FindForm(), failed, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Await RefreshAsync()
    End Sub

    ' =============================================
    ' BRANCH -> WAGE AREA (sine-save agad pagkapalit ng cell)
    ' =============================================
    Private Async Sub gridviewBranches_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) _
        Handles gridviewBranches.CellValueChanged
        If _binding OrElse e.Column Is Nothing OrElse e.Column.FieldName <> "WageArea" Then Return
        Dim row = TryCast(gridviewBranches.GetRow(e.RowHandle), BranchWageAreaModel)
        If row Is Nothing Then Return

        Dim area = If(e.Value Is Nothing OrElse e.Value Is DBNull.Value, String.Empty, e.Value.ToString().Trim())
        Dim failed As String = Nothing
        Try
            Await _service.SetBranchWageAreaAsync(row.BranchId, area)
            row.WageArea = If(area.Length = 0, Nothing, area)
            If area.Length > 0 AndAlso GlobalShared.Helpers.MinimumWageRules.RateOn(_rates, area, Date.Today) Is Nothing Then
                failed = $"Saved. Note: there is no minimum wage rate for '{area}' yet - add it on the left."
            End If
        Catch ex As Exception
            failed = ex.Message
        End Try

        If failed IsNot Nothing Then XtraMessageBox.Show(FindForm(), failed, Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
