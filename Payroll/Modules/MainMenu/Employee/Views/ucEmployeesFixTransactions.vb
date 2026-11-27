' ============================================================
' Modules/MainMenu/Employee/Views/ucEmployeesFixTransactions.vb
' ============================================================
' Ang "Fix Transactions" tab ng Employee screen: paulit-ulit na Allowance / Compensation / Bonus / Deduction.
' Kapareho ng 201 Files: HIWALAY sa Edit/New flow ng ibang tabs (gumagana kapag Idle ang screen; ang ucEmployees ang
' nagdi-disable ng tab habang New/Edit). Walang Presenter - tumatawag lang sa IEmployeeFixTxnService.
'
' Add/Edit:  frmFixTransaction  ->  service.SaveAsync (validate ulit sa server)
' Active:    Set Active / Set Inactive (bago = Active)
' Delete:    para lang sa hindi pa nagamit sa payroll
' Lazy load: naglo-load lang kapag nakikita ang tab.
' Permission: "Employee Salary" (View = makita, Can Edit = magbago).
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Public Class ucEmployeesFixTransactions
    Inherits DevExpress.XtraEditors.XtraUserControl

    Public Class FixRow
        Public Property FixTxnId As Integer
        Public Property TxnType As String
        Public Property Code As String
        Public Property Description As String
        Public Property Amount As String
        Public Property Start As String
        Public Property [End] As String
        Public Property Schedule As String
        Public Property Active As Boolean
        Public Property InPayroll As String
        Public Property Remarks As String
        Public Property UpdatedBy As String
        Public Property UpdatedOn As DateTime?
    End Class

    Private Const Title As String = "Fix Transactions"

    Private _service As Employee.Services.IEmployeeFixTxnService
    Private _recordId As Integer
    Private _employeeName As String = String.Empty
    Private _loaded As Boolean
    Private _busy As Boolean
    Private _models As New List(Of EmployeeFixTxnModel)()

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub Initialize(service As Employee.Services.IEmployeeFixTxnService)
        _service = service
        ConfigureGrid()
        UpdateButtons()
    End Sub

    ' =============================================
    ' TINATAWAG NG ucEmployees
    ' =============================================
    Public Sub SetRecord(recordId As Integer)
        _recordId = recordId
        _loaded = False
        _models = New List(Of EmployeeFixTxnModel)()
        gridFix.DataSource = Nothing
        lblSummary.Text = String.Empty
        UpdateButtons()

        If recordId > 0 AndAlso Visible Then LoadInBackground()
    End Sub

    Public Sub SetEmployeeName(name As String)
        _employeeName = If(name, String.Empty)
    End Sub

    Private Sub ucEmployeesFixTransactions_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible AndAlso Not _loaded AndAlso _recordId > 0 Then LoadInBackground()
    End Sub

    Private Async Sub LoadInBackground()
        Await RefreshAsync()
    End Sub

    ' =============================================
    ' GRID
    ' =============================================
    Private Sub ConfigureGrid()
        With gridviewFix
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With
    End Sub

    Private Sub FormatColumns()
        SetCaption("TxnType", "Type", 85)
        SetCaption("Code", "Code", 80)
        SetCaption("Description", "Description", 150)
        SetCaption("Amount", "Amount", 80)
        SetCaption("Start", "Start", 70)
        SetCaption("End", "End", 70)
        SetCaption("Schedule", "Frequency", 190)
        SetCaption("Active", "Active", 50)
        SetCaption("InPayroll", "Used", 45)
        SetCaption("Remarks", "Remarks", 150)
        SetCaption("UpdatedBy", "Updated By", 80)
        SetCaption("UpdatedOn", "Updated On", 110)

        Dim colAmount = gridviewFix.Columns("Amount")
        If colAmount IsNot Nothing Then colAmount.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

        Dim colUsed = gridviewFix.Columns("InPayroll")
        If colUsed IsNot Nothing Then colUsed.ToolTip = "Yes = already given in a processed payroll (can't be deleted; set Inactive instead)"

        Dim colDate = gridviewFix.Columns("UpdatedOn")
        If colDate IsNot Nothing Then
            colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            colDate.DisplayFormat.FormatString = "MM/dd/yyyy hh:mm tt"
        End If

        Dim colId = gridviewFix.Columns("FixTxnId")
        If colId IsNot Nothing Then colId.Visible = False
    End Sub

    Private Sub SetCaption(fieldName As String, caption As String, width As Integer)
        Dim col = gridviewFix.Columns(fieldName)
        If col Is Nothing Then Return
        col.Caption = caption
        col.Width = width
    End Sub

    ' Inactive = gray
    Private Sub gridviewFix_RowStyle(sender As Object, e As RowStyleEventArgs) Handles gridviewFix.RowStyle
        If e.RowHandle < 0 Then Return
        Dim row = TryCast(gridviewFix.GetRow(e.RowHandle), FixRow)
        If row IsNot Nothing AndAlso Not row.Active Then e.Appearance.ForeColor = Color.Gray
    End Sub

    Private Function SelectedModel() As EmployeeFixTxnModel
        Dim row = TryCast(gridviewFix.GetFocusedRow(), FixRow)
        If row Is Nothing Then Return Nothing
        Return _models.FirstOrDefault(Function(m) m.FixTxnId = row.FixTxnId)
    End Function

    Private Sub gridviewFix_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewFix.FocusedRowChanged
        UpdateButtons()
    End Sub

    Private Sub UpdateButtons()
        Dim canEdit = Employee.Services.EmployeeFixTxnService.CanEdit()
        Dim hasRecord As Boolean = _recordId > 0 AndAlso _service IsNot Nothing AndAlso Not _busy AndAlso _loaded
        Dim sel = If(hasRecord, SelectedModel(), Nothing)

        btnAdd.Enabled = hasRecord AndAlso canEdit
        btnEdit.Enabled = sel IsNot Nothing AndAlso canEdit
        btnToggleActive.Enabled = sel IsNot Nothing AndAlso canEdit
        btnToggleActive.Text = If(sel IsNot Nothing AndAlso Not sel.IsActive, "Set Active", "Set Inactive")
        btnDelete.Enabled = sel IsNot Nothing AndAlso canEdit AndAlso Not sel.IsUsed
        chkShowInactive.Enabled = hasRecord
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
        If _service Is Nothing OrElse _recordId <= 0 Then Return

        If Not Employee.Services.EmployeeFixTxnService.CanView() Then
            gridFix.DataSource = Nothing
            lblSummary.Text = "You don't have access to Fix Transactions (Employee Salary)."
            _loaded = False
            UpdateButtons()
            Return
        End If

        Dim requested As Integer = _recordId
        Try
            Dim list = Await _service.GetByEmployeeAsync(requested)
            If requested <> _recordId Then Return ' nagbago na ang napiling empleyado

            _models = list
            _loaded = True
            BindRows(focusId)
        Catch ex As Exception
            lblSummary.Text = String.Empty
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        UpdateButtons()
    End Function

    Private Sub BindRows(Optional focusId As Integer = 0)
        Dim visibleModels = _models.Where(Function(m) m.IsActive OrElse chkShowInactive.Checked).ToList()

        Dim rows = visibleModels.Select(Function(m) New FixRow With {
            .FixTxnId = m.FixTxnId,
            .TxnType = m.TypeText,
            .Code = m.TxnCode,
            .Description = If(m.CodeIsActive, m.Description, m.Description & " (code inactive)"),
            .Amount = FixTransactionRules.AmountText(m.Amount, m.InputUnit),
            .Start = FixTransactionRules.PeriodText(m.StartYear, m.StartMonth),
            .[End] = FixTransactionRules.PeriodText(m.EndYear, m.EndMonth),
            .Schedule = FixTransactionRules.ScheduleText(m),
            .Active = m.IsActive,
            .InPayroll = If(m.IsUsed, "Yes", ""),
            .Remarks = m.Remarks,
            .UpdatedBy = If(m.UpdatedBy, m.CreatedBy),
            .UpdatedOn = If(m.UpdatedAt, m.CreatedAt)
        }).ToList()

        gridFix.DataSource = rows
        FormatColumns()

        If focusId > 0 Then
            Dim index = rows.FindIndex(Function(r) r.FixTxnId = focusId)
            If index >= 0 Then gridviewFix.FocusedRowHandle = gridviewFix.GetRowHandle(index)
        End If

        Dim activeCount = _models.Where(Function(m) m.IsActive).Count()
        Dim inactiveCount = _models.Count - activeCount
        lblSummary.Text = If(_models.Count = 0, "No fix transactions yet.",
                             $"{activeCount} active" & If(inactiveCount > 0, $", {inactiveCount} inactive", ""))
    End Sub

    Private Sub chkShowInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowInactive.CheckedChanged
        If Not _loaded Then Return
        Dim sel = SelectedModel()
        BindRows(If(sel IsNot Nothing, sel.FixTxnId, 0))
        UpdateButtons()
    End Sub

    ' =============================================
    ' ADD / EDIT
    ' =============================================
    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Await AddOrEditAsync(Nothing)
    End Sub

    Private Async Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim sel = SelectedModel()
        If sel IsNot Nothing Then Await AddOrEditAsync(sel)
    End Sub

    Private Async Sub gridviewFix_DoubleClick(sender As Object, e As EventArgs) Handles gridviewFix.DoubleClick
        Dim sel = SelectedModel()
        If sel IsNot Nothing AndAlso btnEdit.Enabled Then Await AddOrEditAsync(sel)
    End Sub

    Private Async Function AddOrEditAsync(existing As EmployeeFixTxnModel) As Task
        If _recordId <= 0 OrElse _service Is Nothing OrElse _busy Then Return

        Dim options As List(Of FixTxnCodeOption)
        Try
            options = Await _service.GetCodeOptionsAsync()
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        If existing Is Nothing AndAlso Not options.Any(Function(o) o.IsActive) Then
            XtraMessageBox.Show(FindForm(), "There are no active codes to use yet. Add them in Settings > Payroll Settings (Compensation, Bonus, Deduction).",
                                Title, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Paulit-ulit ang dialog hanggang ma-save o ma-Cancel (para hindi mawala ang na-type kapag may error sa server).
        Dim draft = existing
        Do
            Dim result As EmployeeFixTxnModel
            Using dlg As New frmFixTransaction(options, _models, _employeeName, _recordId, draft)
                If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
                result = dlg.Result
            End Using

            SetBusy(True)
            Try
                Dim id = Await _service.SaveAsync(result)
                Await RefreshAsync(id)
                Return
            Catch ex As InvalidOperationException
                XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' Bumalik sa dialog na dala ang na-type (pero existing pa rin ang Id/Used kung edit).
                result.IsUsed = existing IsNot Nothing AndAlso existing.IsUsed
                draft = result
                If existing Is Nothing Then draft.FixTxnId = 0
            Catch ex As Exception
                XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            Finally
                SetBusy(False)
            End Try
        Loop
    End Function

    ' =============================================
    ' SET ACTIVE / INACTIVE
    ' =============================================
    Private Async Sub btnToggleActive_Click(sender As Object, e As EventArgs) Handles btnToggleActive.Click
        Dim sel = SelectedModel()
        If sel Is Nothing OrElse _busy Then Return

        Dim makeActive = Not sel.IsActive
        Dim question = If(makeActive,
            $"Set {sel.TxnCode} ({FixTransactionRules.AmountText(sel.Amount, sel.InputUnit)}) back to Active?" & vbCrLf & vbCrLf &
            "It will be given again in payroll from its Start month.",
            $"Set {sel.TxnCode} ({FixTransactionRules.AmountText(sel.Amount, sel.InputUnit)}) Inactive?" & vbCrLf & vbCrLf &
            "It will no longer be given in payroll (including Processed cutoffs that are re-processed). " &
            "If it just has an end month, use Edit > Has End instead.")
        If XtraMessageBox.Show(FindForm(), question, Title, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return

        SetBusy(True)
        Try
            Await _service.SetActiveAsync(sel.FixTxnId, makeActive)
            Await RefreshAsync(sel.FixTxnId)
        Catch ex As InvalidOperationException
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    ' =============================================
    ' DELETE (hindi pa nagamit sa payroll lang)
    ' =============================================
    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim sel = SelectedModel()
        If sel Is Nothing OrElse _busy Then Return

        If XtraMessageBox.Show(FindForm(),
                $"Delete {sel.TxnCode} ({FixTransactionRules.AmountText(sel.Amount, sel.InputUnit)}, {FixTransactionRules.ScheduleText(sel)})?" &
                vbCrLf & vbCrLf & "It was never used in payroll, so it will be removed completely.",
                "Delete Fix Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then
            Return
        End If

        SetBusy(True)
        Dim refreshAfter As Boolean = False
        Try
            Await _service.DeleteAsync(sel.FixTxnId)
            Await RefreshAsync()
        Catch ex As InvalidOperationException
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            refreshAfter = True   ' (bawal ang Await sa loob ng Catch sa VB)
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, Title, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
        If refreshAfter Then Await RefreshAsync(sel.FixTxnId)
    End Sub

End Class
