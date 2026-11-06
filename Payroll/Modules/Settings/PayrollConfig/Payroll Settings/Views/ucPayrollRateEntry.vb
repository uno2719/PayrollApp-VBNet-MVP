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
    Private Const BTN_DEACTIVATE As Integer = 3
    Private Const BTN_DELETE As Integer = 4
    Private Const BTN_REFRESH As Integer = 6
    Private _isActive As Boolean = True

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

        ApplyReadOnlyMode(wbpMainCommands)
    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Payroll
        End Get
    End Property

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

        CommandBarHelper.InitDeactivateDeleteButtons(wbpMainCommands, BTN_DEACTIVATE, BTN_DELETE)

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
            Return _isActive
        End Get
        Set(value As Boolean)
            _isActive = value
            CommandBarHelper.ShowStatus(lblStatus, value)
            CommandBarHelper.RefreshToggleCaption(wbpMainCommands, BTN_DEACTIVATE, value, HasEditAccess)
        End Set
    End Property

    Public Property InputUnit As PayrollInputUnit Implements IPayrollRateEntryMaintenanceView.InputUnit
        Get
            Return CType(Math.Max(cboInputUnit.SelectedIndex, 0), PayrollInputUnit)
        End Get
        Set(value As PayrollInputUnit)
            cboInputUnit.SelectedIndex = CInt(value)
        End Set
    End Property

    Public Function ConfirmUnitChange(message As String) As Boolean Implements IPayrollRateEntryMaintenanceView.ConfirmUnitChange
        Return DevExpress.XtraEditors.XtraMessageBox.Show(message, "Change Input Unit",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning) = DialogResult.Yes
    End Function

    Public Property IsEssential As Boolean Implements IPayrollRateEntryMaintenanceView.IsEssential
        Get
            Return chkIsEssential.Checked
        End Get
        Set(value As Boolean)
            chkIsEssential.Checked = value
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
        chkIsEssential.Properties.ReadOnly = Not isEditable
        cboInputUnit.Properties.ReadOnly = Not isEditable

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

        CommandBarHelper.ApplyModeState(wbpMainCommands, BTN_DEACTIVATE, BTN_DELETE, isEditable, IsActive, HasEditAccess)
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
        IsActive = True
        cboInputUnit.SelectedIndex = 0
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
                    If ConfirmSaveStatus() Then
                        Await _presenter.SaveAsync()
                    End If
                Else
                    _presenter.StartNew()
                End If

            Case "Edit"
                If _isEditing Then
                    _presenter.CancelEdit()
                Else
                    _presenter.StartEdit()
                End If

            Case "Deactivate"
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

            Case "Delete"
                If Not _isEditing Then
                    If Not _presenter.HasSelection() Then
                        XtraMessageBox.Show("Please select an entry first.", "Delete",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ElseIf CommandBarHelper.ConfirmDelete(Me, $"'{Code}'") Then
                        Await _presenter.DeleteSelectedAsync()
                    End If
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If

        End Select

    End Sub


    ' Kapag ine-edit ang INACTIVE na record at sine-save: itanong kung ibabalik sa Active
    ' o mananatiling Inactive. (Ang Status ay hindi na checkbox na pwedeng galawin ng user.)
    Private Function ConfirmSaveStatus() As Boolean
        If IsActive OrElse Not _presenter.IsEditingExisting() Then Return True

        Select Case CommandBarHelper.AskInactiveSaveChoice(Me)
            Case DialogResult.Yes
                IsActive = True
                Return True
            Case DialogResult.No
                Return True
            Case Else
                Return False
        End Select
    End Function

End Class
