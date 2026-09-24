Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.LeaveSettings.Presenters
Imports Payroll.LeaveSettings.Views

Public Class ucLeaveType
    Implements ILeaveTypeMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As LeaveTypePresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    Public Sub SetPresenter(presenter As LeaveTypePresenter)
        _presenter = presenter
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Leave Settings > Type"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Type"
        End Get
    End Property

    ' =============================================
    ' LOAD
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrid()
        SetupCategoryCombo()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

        ApplyReadOnlyMode(wbpMainCommands)
    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Leave
        End Get
    End Property

    ' Sadyang naka-OFF ang inline grid editing - TOP FORM na lang
    ' (Code/Name/Category/Active) + buttons, gaya ng ucLookupMaintenance.
    Private Sub SetupGrid()
        With gridviewLeaveTypeList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    Private Sub SetupCategoryCombo()
        cboCategory.Properties.Items.Clear()
        cboCategory.Properties.Items.AddRange(LeaveCategory.All)
    End Sub

    Private Sub SetupCommandImages()

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"

        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_32
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected"

        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ToolTip = "Reload from Database"

    End Sub

    ' =============================================
    ' ILeaveTypeMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property Code As String Implements ILeaveTypeMaintenanceView.Code
        Get
            Return txtCode.Text
        End Get
        Set(value As String)
            txtCode.Text = value
        End Set
    End Property

    Public Property Name As String Implements ILeaveTypeMaintenanceView.Name
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property

    Public Property Category As String Implements ILeaveTypeMaintenanceView.Category
        Get
            Return cboCategory.Text
        End Get
        Set(value As String)
            cboCategory.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ILeaveTypeMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' ILeaveTypeMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of LeaveTypeModel)) Implements ILeaveTypeMaintenanceView.BindList
        gridconLeaveTypeList.DataSource = items
    End Sub

    ' =============================================
    ' ILeaveTypeMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ILeaveTypeMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        txtCode.Properties.ReadOnly = Not isEditable
        txtName.Properties.ReadOnly = Not isEditable
        cboCategory.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconLeaveTypeList.Enabled = Not isEditable

        If isEditable Then
            If isNewRecord Then
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Save"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Save New Entry"
            Else
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Record"
            End If
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"
        End If

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Public Sub ClearFields() Implements ILeaveTypeMaintenanceView.ClearFields
        txtCode.Text = String.Empty
        txtName.Text = String.Empty
        cboCategory.SelectedIndex = -1
        chkActive.Checked = True
    End Sub

    ' NOTE: DisplayInfo/DisplayValidationError - parehong pattern ng
    ' ucLookupMaintenance (iba ang pangalan dito para hindi mag-conflict
    ' sa minanang ShowMessage/ShowError ng ucBase).
    Public Sub DisplayInfo(message As String) Implements ILeaveTypeMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements ILeaveTypeMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewLeaveTypeList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewLeaveTypeList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewLeaveTypeList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewLeaveTypeList_Click(sender As Object, e As EventArgs) _
        Handles gridviewLeaveTypeList.Click

        If _isEditing Then Return
        If gridviewLeaveTypeList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewLeaveTypeList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    ' =============================================
    ' BUTTON COMMANDS
    ' =============================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Select Case tag
            Case "New"
                If _isEditing Then
                    Await _presenter.SaveAsync()
                Else
                    _presenter.StartNew()
                End If

            Case "Edit"
                If _isEditing Then
                    _presenter.CancelEdit()
                Else
                    _presenter.StartEdit()
                End If

            Case "Delete"
                If Not _isEditing Then
                    Dim action = If(IsActive, "deactivate", "reactivate")

                    Dim confirm = XtraMessageBox.Show($"Are you sure you want to {action} this entry?",
                                                        "Confirm",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.ToggleActiveSelectedAsync()
                    End If
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If

        End Select

    End Sub

End Class
