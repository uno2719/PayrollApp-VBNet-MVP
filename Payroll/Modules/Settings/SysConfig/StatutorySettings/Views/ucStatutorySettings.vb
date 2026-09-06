Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Extensions
Imports Payroll.GlobalShared.Models
Imports Payroll.StatutorySettings.Presenters
Imports Payroll.StatutorySettings.Views

Public Class ucStatutorySettings
    Implements IStatutorySettingsMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As StatutorySettingsPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False
    Private _tabTitle As String = "Statutory"

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    ' Tinatawag ito ng AppComposition kapag ginagawa yung 3 instances
    ' (SSS/PhilHealth/Pag-IBIG) - parehong pattern gaya ng Master Data.
    Public Sub SetPresenter(presenter As StatutorySettingsPresenter, tabTitle As String)
        _presenter = presenter
        _tabTitle = tabTitle
        lblTabPageTitle.Text = tabTitle
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE - dynamic per instance
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Statutory > {_tabTitle}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return _tabTitle
        End Get
    End Property

    ' =============================================
    ' LOAD (lazy - isang beses lang per tab, gaya ng Master Data)
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrid()
        SetupNumericFields()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewStatutoryList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' Currency mask sa lahat ng peso-value fields - isang beses lang
    ' i-set sa Load, hindi kailangan ulitin sa Designer.
    Private Sub SetupNumericFields()
        txtSalaryFrom.SetAsCurrency()
        txtSalaryTo.SetAsCurrency()
        txtEEShare.SetAsCurrency()
        txtERShare.SetAsCurrency()
        txtECCAmount.SetAsCurrency()
        txtEEMPF.SetAsCurrency()
        txtERMPF.SetAsCurrency()
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
    ' IStatutoryMaintenanceView - FORM FIELDS
    ' Peso-value fields: TextEdit na naka-Numeric mask (SetAsCurrency),
    ' kaya Decimal na agad ang EditValue - walang manual string parsing.
    ' =============================================
    Public Property SalaryFrom As Decimal Implements IStatutorySettingsMaintenanceView.SalaryFrom
        Get
            Return If(txtSalaryFrom.EditValue Is Nothing, 0D, Convert.ToDecimal(txtSalaryFrom.EditValue))
        End Get
        Set(value As Decimal)
            txtSalaryFrom.EditValue = value
        End Set
    End Property

    Public Property SalaryTo As Decimal Implements IStatutorySettingsMaintenanceView.SalaryTo
        Get
            Return If(txtSalaryTo.EditValue Is Nothing, 0D, Convert.ToDecimal(txtSalaryTo.EditValue))
        End Get
        Set(value As Decimal)
            txtSalaryTo.EditValue = value
        End Set
    End Property

    Public Property EEShare As Decimal Implements IStatutorySettingsMaintenanceView.EEShare
        Get
            Return If(txtEEShare.EditValue Is Nothing, 0D, Convert.ToDecimal(txtEEShare.EditValue))
        End Get
        Set(value As Decimal)
            txtEEShare.EditValue = value
        End Set
    End Property

    Public Property EEContriType As String Implements IStatutorySettingsMaintenanceView.EEContriType
        Get
            Return cboEEContriType.Text
        End Get
        Set(value As String)
            cboEEContriType.Text = value
        End Set
    End Property

    Public Property ERShare As Decimal Implements IStatutorySettingsMaintenanceView.ERShare
        Get
            Return If(txtERShare.EditValue Is Nothing, 0D, Convert.ToDecimal(txtERShare.EditValue))
        End Get
        Set(value As Decimal)
            txtERShare.EditValue = value
        End Set
    End Property

    Public Property ERContriType As String Implements IStatutorySettingsMaintenanceView.ERContriType
        Get
            Return cboERContriType.Text
        End Get
        Set(value As String)
            cboERContriType.Text = value
        End Set
    End Property

    Public Property ECCAmount As Decimal Implements IStatutorySettingsMaintenanceView.ECCAmount
        Get
            Return If(txtECCAmount.EditValue Is Nothing, 0D, Convert.ToDecimal(txtECCAmount.EditValue))
        End Get
        Set(value As Decimal)
            txtECCAmount.EditValue = value
        End Set
    End Property

    Public Property EEMPF As Decimal Implements IStatutorySettingsMaintenanceView.EEMPF
        Get
            Return If(txtEEMPF.EditValue Is Nothing, 0D, Convert.ToDecimal(txtEEMPF.EditValue))
        End Get
        Set(value As Decimal)
            txtEEMPF.EditValue = value
        End Set
    End Property

    Public Property ERMPF As Decimal Implements IStatutorySettingsMaintenanceView.ERMPF
        Get
            Return If(txtERMPF.EditValue Is Nothing, 0D, Convert.ToDecimal(txtERMPF.EditValue))
        End Get
        Set(value As Decimal)
            txtERMPF.EditValue = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IStatutorySettingsMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' IStatutoryMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of StatutoryBracketModel)) Implements IStatutorySettingsMaintenanceView.BindList
        gridconStatutoryList.DataSource = items
    End Sub

    ' =============================================
    ' IStatutoryMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IStatutorySettingsMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        '========================================
        ' Fields
        '========================================
        txtSalaryFrom.Properties.ReadOnly = Not isEditable
        txtSalaryTo.Properties.ReadOnly = Not isEditable
        txtEEShare.Properties.ReadOnly = Not isEditable
        cboEEContriType.Properties.ReadOnly = Not isEditable
        txtERShare.Properties.ReadOnly = Not isEditable
        cboERContriType.Properties.ReadOnly = Not isEditable
        txtECCAmount.Properties.ReadOnly = Not isEditable
        txtEEMPF.Properties.ReadOnly = Not isEditable
        txtERMPF.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconStatutoryList.Enabled = Not isEditable

        '========================================
        ' NEW / SAVE / UPDATE BUTTON
        '========================================
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
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_personel_24
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"
        End If

        '========================================
        ' EDIT / CANCEL BUTTON
        '========================================
        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

        '========================================
        ' DELETE / REFRESH
        '========================================
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Public Sub ClearFields() Implements IStatutorySettingsMaintenanceView.ClearFields
        txtSalaryFrom.EditValue = Nothing
        txtSalaryTo.EditValue = Nothing
        txtEEShare.EditValue = Nothing
        cboEEContriType.SelectedIndex = -1
        txtERShare.EditValue = Nothing
        cboERContriType.SelectedIndex = -1
        txtECCAmount.EditValue = Nothing
        txtEEMPF.EditValue = Nothing
        txtERMPF.EditValue = Nothing
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements IStatutorySettingsMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IStatutorySettingsMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewStatutoryList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewStatutoryList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewStatutoryList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewStatutoryList_Click(sender As Object, e As EventArgs) _
        Handles gridviewStatutoryList.Click

        If _isEditing Then Return
        If gridviewStatutoryList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewStatutoryList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    ' =============================================
    ' BUTTON COMMANDS (New / Save / Delete-Cancel / Refresh)
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

                    Dim confirm = XtraMessageBox.Show($"Are you sure you want to {action} this bracket?",
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