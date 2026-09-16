Imports DevExpress.Mvvm.Native
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

Public Class ucCompensation
    Implements ICompensationMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As CompensationPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    ' Iisang instance lang si Compensation (hindi tulad ng
    ' FlaggedEntry/RateEntry na 2 instances) - naka-fix na ang title.
    Public Sub SetPresenter(presenter As CompensationPresenter)
        _presenter = presenter
        lblTabPageTitle.Text = "COMPENSATION"
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Payroll > Compensation"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Compensation"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrid()
        SetupCombos()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewCompensationList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' Base sa nakikita sa C1Pay reference screenshot lang muna (pwedeng
    ' dagdagan pa sa future kapag may kulang - sabi mo tanungin sa
    ' accounting officer niyo).
    Private Sub SetupCombos()
        cbo2316Component.Properties.Items.Clear()
        cbo2316Component.Properties.Items.AddRange({"COLA", "NDIF", "HOLI", "COMM", "REPR", "TRANS"})

        cboFrequency.Properties.Items.Clear()
        cboFrequency.Properties.Items.AddRange({"Monthly", "Semi-Monthly", "Yearly"})
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
    ' ICompensationMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property Code As String Implements ICompensationMaintenanceView.Code
        Get
            Return txtCode.Text
        End Get
        Set(value As String)
            txtCode.Text = value
        End Set
    End Property

    Public Property Description As String Implements ICompensationMaintenanceView.Description
        Get
            Return txtDescription.Text
        End Get
        Set(value As String)
            txtDescription.Text = value
        End Set
    End Property

    Public Property TaxFlag As Boolean Implements ICompensationMaintenanceView.TaxFlag
        Get
            Return chkTaxFlag.Checked
        End Get
        Set(value As Boolean)
            chkTaxFlag.Checked = value
        End Set
    End Property

    Public Property SSSFlag As Boolean Implements ICompensationMaintenanceView.SSSFlag
        Get
            Return chkSSSFlag.Checked
        End Get
        Set(value As Boolean)
            chkSSSFlag.Checked = value
        End Set
    End Property

    Public Property PhilHealthFlag As Boolean Implements ICompensationMaintenanceView.PhilHealthFlag
        Get
            Return chkPhilHealthFlag.Checked
        End Get
        Set(value As Boolean)
            chkPhilHealthFlag.Checked = value
        End Set
    End Property

    Public Property PagIbigFlag As Boolean Implements ICompensationMaintenanceView.PagIbigFlag
        Get
            Return chkPagIbigFlag.Checked
        End Get
        Set(value As Boolean)
            chkPagIbigFlag.Checked = value
        End Set
    End Property

    Public Property Component2316 As String Implements ICompensationMaintenanceView.Component2316
        Get
            Return cbo2316Component.Text
        End Get
        Set(value As String)
            cbo2316Component.Text = value
        End Set
    End Property

    Public Property DeminimisFlag As Boolean Implements ICompensationMaintenanceView.DeminimisFlag
        Get
            Return chkDeminimisFlag.Checked
        End Get
        Set(value As Boolean)
            chkDeminimisFlag.Checked = value
        End Set
    End Property

    Public Property CeilingAmount As Decimal Implements ICompensationMaintenanceView.CeilingAmount
        Get
            Return If(Decimal.TryParse(txtCeilingAmount.Text, Nothing), CDec(txtCeilingAmount.Text), 0D)
        End Get
        Set(value As Decimal)
            txtCeilingAmount.Text = value.ToString("0.00")
        End Set
    End Property

    Public Property Frequency As String Implements ICompensationMaintenanceView.Frequency
        Get
            Return cboFrequency.Text
        End Get
        Set(value As String)
            cboFrequency.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ICompensationMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' ICompensationMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of CompensationModel)) Implements ICompensationMaintenanceView.BindList
        gridconCompensationList.DataSource = items
    End Sub

    ' =============================================
    ' ICompensationMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ICompensationMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        txtCode.Properties.ReadOnly = Not isEditable
        txtDescription.Properties.ReadOnly = Not isEditable
        chkTaxFlag.Properties.ReadOnly = Not isEditable
        chkSSSFlag.Properties.ReadOnly = Not isEditable
        chkPhilHealthFlag.Properties.ReadOnly = Not isEditable
        chkPagIbigFlag.Properties.ReadOnly = Not isEditable
        cbo2316Component.Properties.ReadOnly = Not isEditable
        chkDeminimisFlag.Properties.ReadOnly = Not isEditable
        txtCeilingAmount.Properties.ReadOnly = Not isEditable
        cboFrequency.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconCompensationList.Enabled = Not isEditable

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

    Public Sub ClearFields() Implements ICompensationMaintenanceView.ClearFields
        txtCode.Text = String.Empty
        txtDescription.Text = String.Empty
        chkTaxFlag.Checked = False
        chkSSSFlag.Checked = False
        chkPhilHealthFlag.Checked = False
        chkPagIbigFlag.Checked = False
        cbo2316Component.Text = String.Empty
        chkDeminimisFlag.Checked = False
        txtCeilingAmount.Text = "0.00"
        cboFrequency.Text = String.Empty
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements ICompensationMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements ICompensationMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewCompensationList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewCompensationList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewCompensationList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewCompensationList_Click(sender As Object, e As EventArgs) _
        Handles gridviewCompensationList.Click

        If _isEditing Then Return
        If gridviewCompensationList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewCompensationList.GetFocusedRowCellValue("Id")
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
