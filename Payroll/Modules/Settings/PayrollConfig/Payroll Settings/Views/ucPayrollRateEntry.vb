Imports DevExpress.Mvvm.Native
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

' Ginagamit ito ng 2 instances (Overtime, Holiday).
Public Class ucPayrollRateEntry
    Implements IPayrollRateEntryMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As PayrollRateEntryPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False
    Private _tabTitle As String = "Overtime"

    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    Private Const BTN_REFRESH As Integer = 5

    Public Sub SetPresenter(presenter As PayrollRateEntryPresenter, tabTitle As String)
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
        With gridviewRateEntryList
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
    ' IPayrollRateEntryMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property Code As String Implements IPayrollRateEntryMaintenanceView.Code
        Get
            Return txtCode.Text
        End Get
        Set(value As String)
            txtCode.Text = value
        End Set
    End Property

    Public Property Description As String Implements IPayrollRateEntryMaintenanceView.Description
        Get
            Return txtDescription.Text
        End Get
        Set(value As String)
            txtDescription.Text = value
        End Set
    End Property

    Public Property TaxFlag As Boolean Implements IPayrollRateEntryMaintenanceView.TaxFlag
        Get
            Return chkTaxFlag.Checked
        End Get
        Set(value As Boolean)
            chkTaxFlag.Checked = value
        End Set
    End Property

    Public Property SSSFlag As Boolean Implements IPayrollRateEntryMaintenanceView.SSSFlag
        Get
            Return chkSSSFlag.Checked
        End Get
        Set(value As Boolean)
            chkSSSFlag.Checked = value
        End Set
    End Property

    Public Property PhilHealthFlag As Boolean Implements IPayrollRateEntryMaintenanceView.PhilHealthFlag
        Get
            Return chkPhilHealthFlag.Checked
        End Get
        Set(value As Boolean)
            chkPhilHealthFlag.Checked = value
        End Set
    End Property

    Public Property PagIbigFlag As Boolean Implements IPayrollRateEntryMaintenanceView.PagIbigFlag
        Get
            Return chkPagIbigFlag.Checked
        End Get
        Set(value As Boolean)
            chkPagIbigFlag.Checked = value
        End Set
    End Property

    Public Property Rate As Decimal Implements IPayrollRateEntryMaintenanceView.Rate
        Get
            Return If(Decimal.TryParse(txtRate.Text, Nothing), CDec(txtRate.Text), 0D)
        End Get
        Set(value As Decimal)
            txtRate.Text = value.ToString("0.00")
        End Set
    End Property

    Public Property MapCode As String Implements IPayrollRateEntryMaintenanceView.MapCode
        Get
            Return txtMapCode.Text
        End Get
        Set(value As String)
            txtMapCode.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IPayrollRateEntryMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' IPayrollRateEntryMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of PayrollRateEntryModel)) Implements IPayrollRateEntryMaintenanceView.BindList
        gridconRateEntryList.DataSource = items
    End Sub

    ' =============================================
    ' IPayrollRateEntryMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IPayrollRateEntryMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        txtCode.Properties.ReadOnly = Not isEditable
        txtDescription.Properties.ReadOnly = Not isEditable
        chkTaxFlag.Properties.ReadOnly = Not isEditable
        chkSSSFlag.Properties.ReadOnly = Not isEditable
        chkPhilHealthFlag.Properties.ReadOnly = Not isEditable
        chkPagIbigFlag.Properties.ReadOnly = Not isEditable
        txtRate.Properties.ReadOnly = Not isEditable
        txtMapCode.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconRateEntryList.Enabled = Not isEditable

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

    Public Sub ClearFields() Implements IPayrollRateEntryMaintenanceView.ClearFields
        txtCode.Text = String.Empty
        txtDescription.Text = String.Empty
        chkTaxFlag.Checked = False
        chkSSSFlag.Checked = False
        chkPhilHealthFlag.Checked = False
        chkPagIbigFlag.Checked = False
        txtRate.Text = "0.00"
        txtMapCode.Text = String.Empty
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements IPayrollRateEntryMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IPayrollRateEntryMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewRateEntryList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewRateEntryList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewRateEntryList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewRateEntryList_Click(sender As Object, e As EventArgs) _
        Handles gridviewRateEntryList.Click

        If _isEditing Then Return
        If gridviewRateEntryList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewRateEntryList.GetFocusedRowCellValue("Id")
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
