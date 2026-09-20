Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.Login.Models
Imports Payroll.ModuleManagement.Models
Imports Payroll.ModuleManagement.Presenters
Imports Payroll.ModuleManagement.Views

Public Class ucModuleAccessEditor
    Implements IModuleAccessEditorView
    Implements IAsyncLoadable

    Private _presenter As ModuleAccessEditorPresenter

    ' Parehong guard gaya ng ginamit sa Loans - habang PINUPUNAN ng
    ' Presenter ang grid (BindAccessGrid), ayaw nating ma-trigger ang
    ' CellValueChanged bilang kung ito ay galing sa aktwal na pag-tap
    ' ni user sa checkbox.
    Private _isBinding As Boolean = False

    Private Const BTN_SAVE As Integer = 1
    Private Const BTN_REFRESH As Integer = 3

    Public Sub SetPresenter(presenter As ModuleAccessEditorPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Administration > Module Management > Access Editor"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Module Access Editor"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupGrids()
        rgUserFilter.EditValue = "Active"

        SetGridEnabled(False)

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrids()

        With gridviewUserList
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        End With

        With gridviewAccess
            .OptionsView.ShowGroupPanel = True
            .OptionsView.ShowIndicator = False
        End With

        ' Grupo bilang default view - mas madaling ma-scan ang 24
        ' modules kapag naka-grupo sa Main Menu / Administration /
        ' Settings / Reports imbes na iisang mahabang listahan.
        colGroupName.OptionsColumn.ShowInCustomizationForm = True
        gridviewAccess.GroupCount = 0

    End Sub

    ' ========================================================
    ' IModuleAccessEditorView
    ' ========================================================
    Public Sub BindUserList(users As List(Of UserModel)) _
        Implements IModuleAccessEditorView.BindUserList

        gridconUserList.DataSource = users

        For Each col As DevExpress.XtraGrid.Columns.GridColumn In gridviewUserList.Columns
            col.Visible = False
        Next

        Dim colUsername = gridviewUserList.Columns("Username")
        If colUsername IsNot Nothing Then
            colUsername.Visible = True
            colUsername.Caption = "Username"
            colUsername.VisibleIndex = 0
        End If

        Dim colFirst = gridviewUserList.Columns("FirstName")
        If colFirst IsNot Nothing Then
            colFirst.Visible = True
            colFirst.Caption = "First Name"
            colFirst.VisibleIndex = 1
        End If

        Dim colLast = gridviewUserList.Columns("LastName")
        If colLast IsNot Nothing Then
            colLast.Visible = True
            colLast.Caption = "Last Name"
            colLast.VisibleIndex = 2
        End If

        gridviewUserList.BestFitColumns()

    End Sub

    Public Sub SetSelectedUserHeader(displayName As String, isAdmin As Boolean) _
        Implements IModuleAccessEditorView.SetSelectedUserHeader

        lblSelectedUser.Text = displayName

    End Sub

    Public Sub BindAccessGrid(rows As List(Of ModuleAccessRow)) _
        Implements IModuleAccessEditorView.BindAccessGrid

        _isBinding = True
        Try
            gridconAccess.DataSource = Nothing
            gridconAccess.DataSource = rows
        Finally
            _isBinding = False
        End Try

    End Sub

    Public Function GetAccessGridRows() As List(Of ModuleAccessRow) _
        Implements IModuleAccessEditorView.GetAccessGridRows

        Return TryCast(gridconAccess.DataSource, List(Of ModuleAccessRow))

    End Function

    Public Function PromptCopyFromUser(candidates As List(Of UserModel)) As Integer? _
        Implements IModuleAccessEditorView.PromptCopyFromUser

        Dim displayItems = candidates.
            Select(Function(u) $"{u.FirstName} {u.LastName} ({u.Username})").
            ToList()

        Using dlg As New frmPickUser("Copy module access from:", displayItems)

            If dlg.ShowDialog(Me) <> DialogResult.OK Then Return Nothing
            If Not dlg.SelectedIndex.HasValue Then Return Nothing

            Return candidates(dlg.SelectedIndex.Value).RecordId

        End Using

    End Function

    Public Sub SetGridEnabled(enabled As Boolean) Implements IModuleAccessEditorView.SetGridEnabled

        gridconAccess.Enabled = enabled
        pnlBulkActions.Enabled = enabled
        wbpMainCommands.Buttons.Item(BTN_SAVE).Properties.Enabled = enabled

    End Sub

    Public Sub SetHasUnsavedChanges(hasChanges As Boolean) _
        Implements IModuleAccessEditorView.SetHasUnsavedChanges

        lblUnsavedBadge.Visible = hasChanges

    End Sub

    Public Sub ShowMessageBox(message As String) Implements IModuleAccessEditorView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub ShowErrorBox(message As String) Implements IModuleAccessEditorView.ShowError
        ShowError(message)
    End Sub

    Public Function Confirm(message As String, caption As String) As Boolean _
        Implements IModuleAccessEditorView.Confirm

        Return XtraMessageBox.Show(message, caption,
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) = DialogResult.Yes

    End Function

    ' ========================================================
    ' USER LIST
    ' ========================================================
    Private Async Sub gridviewUserList_FocusedRowChanged(
        sender As Object,
        e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewUserList.FocusedRowChanged

        If gridviewUserList.FocusedRowHandle < 0 Then Return

        Dim recordId = gridviewUserList.GetFocusedRowCellValue("RecordId")
        If recordId Is Nothing Then Return

        Try
            Await _presenter.SelectUserAsync(CInt(recordId))
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    Private Async Sub rgUserFilter_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles rgUserFilter.SelectedIndexChanged

        Try
            Await _presenter.RefreshUserListAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    ' ========================================================
    ' ACCESS GRID - live checkbox editing
    ' ========================================================
    Private Sub gridviewAccess_CellValueChanged(
        sender As Object,
        e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) _
        Handles gridviewAccess.CellValueChanged

        If _isBinding Then Return

        ' Kung na-check ang Edit, dapat awtomatikong ma-check din ang
        ' View - walang saysay ang "puwedeng mag-edit pero hindi
        ' puwedeng makita" (parehong rule ito sa PermissionService.CanEdit
        ' sa GlobalShared - dito lang natin ito ini-enforce sa UI mismo
        ' para hindi na kailangang itama pa sa likod).
        If e.Column Is colCanEdit Then
            Dim isEditChecked = CBool(e.Value)
            If isEditChecked Then
                gridviewAccess.SetRowCellValue(e.RowHandle, colCanView, True)
            End If
        End If

        _presenter.MarkDirty()

    End Sub

    ' ========================================================
    ' BULK ACTION BUTTONS
    ' ========================================================
    Private Sub btnSelectAllView_Click(sender As Object, e As EventArgs) Handles btnSelectAllView.Click
        _presenter.SelectAll(grantEdit:=False)
    End Sub

    Private Sub btnSelectAllEdit_Click(sender As Object, e As EventArgs) Handles btnSelectAllEdit.Click
        _presenter.SelectAll(grantEdit:=True)
    End Sub

    Private Sub btnClearAll_Click(sender As Object, e As EventArgs) Handles btnClearAll.Click
        _presenter.ClearAll()
    End Sub

    Private Async Sub btnCopyFrom_Click(sender As Object, e As EventArgs) Handles btnCopyFrom.Click
        Try
            Await _presenter.CopyFromAnotherUserAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    ' ========================================================
    ' COMMANDS
    ' ========================================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Try
            Select Case tag

                Case "Save"
                    Await _presenter.SaveAsync()

                Case "Refresh"
                    Await _presenter.RefreshUserListAsync()

            End Select

        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

End Class