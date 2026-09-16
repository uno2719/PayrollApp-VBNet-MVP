Imports DevExpress.Mvvm.Native
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

' Ginagamit ito ng 2 instances (Deduction, Bonus) - gaya ng
' ucLookupMaintenance na 8 instances.
Public Class ucPayrollFlaggedEntry
    Implements IPayrollFlaggedEntryMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As PayrollFlaggedEntryPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False
    Private _tabTitle As String = "Deduction"

    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    Private Const BTN_REFRESH As Integer = 5

    Public Sub SetPresenter(presenter As PayrollFlaggedEntryPresenter, tabTitle As String)
        _presenter = presenter
        _tabTitle = tabTitle
        lblTabPageTitle.Text = tabTitle.ToUpper
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Payroll > {_tabTitle}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return _tabTitle
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrid()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewFlaggedEntryList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    Private Sub SetupCommandImages()
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"

        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_24
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected"

        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ToolTip = "Reload from Database"
    End Sub

    ' =============================================
    ' IPayrollFlaggedEntryMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property Code As String Implements IPayrollFlaggedEntryMaintenanceView.Code
        Get
            Return txtCode.Text
        End Get
        Set(value As String)
            txtCode.Text = value
        End Set
    End Property

    Public Property Description As String Implements IPayrollFlaggedEntryMaintenanceView.Description
        Get
            Return txtDescription.Text
        End Get
        Set(value As String)
            txtDescription.Text = value
        End Set
    End Property

    Public Property TaxFlag As Boolean Implements IPayrollFlaggedEntryMaintenanceView.TaxFlag
        Get
            Return chkTaxFlag.Checked
        End Get
        Set(value As Boolean)
            chkTaxFlag.Checked = value
        End Set
    End Property

    Public Property SSSFlag As Boolean Implements IPayrollFlaggedEntryMaintenanceView.SSSFlag
        Get
            Return chkSSSFlag.Checked
        End Get
        Set(value As Boolean)
            chkSSSFlag.Checked = value
        End Set
    End Property

    Public Property PhilHealthFlag As Boolean Implements IPayrollFlaggedEntryMaintenanceView.PhilHealthFlag
        Get
            Return chkPhilHealthFlag.Checked
        End Get
        Set(value As Boolean)
            chkPhilHealthFlag.Checked = value
        End Set
    End Property

    Public Property PagIbigFlag As Boolean Implements IPayrollFlaggedEntryMaintenanceView.PagIbigFlag
        Get
            Return chkPagIbigFlag.Checked
        End Get
        Set(value As Boolean)
            chkPagIbigFlag.Checked = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IPayrollFlaggedEntryMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' IPayrollFlaggedEntryMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of PayrollFlaggedEntryModel)) Implements IPayrollFlaggedEntryMaintenanceView.BindList
        gridconFlaggedEntryList.DataSource = items
    End Sub

    ' =============================================
    ' IPayrollFlaggedEntryMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IPayrollFlaggedEntryMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        txtCode.Properties.ReadOnly = Not isEditable
        txtDescription.Properties.ReadOnly = Not isEditable
        chkTaxFlag.Properties.ReadOnly = Not isEditable
        chkSSSFlag.Properties.ReadOnly = Not isEditable
        chkPhilHealthFlag.Properties.ReadOnly = Not isEditable
        chkPagIbigFlag.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconFlaggedEntryList.Enabled = Not isEditable

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

    Public Sub ClearFields() Implements IPayrollFlaggedEntryMaintenanceView.ClearFields
        txtCode.Text = String.Empty
        txtDescription.Text = String.Empty
        chkTaxFlag.Checked = False
        chkSSSFlag.Checked = False
        chkPhilHealthFlag.Checked = False
        chkPagIbigFlag.Checked = False
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements IPayrollFlaggedEntryMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IPayrollFlaggedEntryMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewFlaggedEntryList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewFlaggedEntryList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewFlaggedEntryList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewFlaggedEntryList_Click(sender As Object, e As EventArgs) _
        Handles gridviewFlaggedEntryList.Click

        If _isEditing Then Return
        If gridviewFlaggedEntryList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewFlaggedEntryList.GetFocusedRowCellValue("Id")
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
