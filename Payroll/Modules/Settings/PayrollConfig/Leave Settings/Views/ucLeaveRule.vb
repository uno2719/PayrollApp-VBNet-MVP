Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.LeaveSettings.Presenters
Imports Payroll.LeaveSettings.Views

Public Class ucLeaveRule
    Implements ILeaveRuleMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As LeaveRulePresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False

    ' Working list ng bracket rows - ITO mismo ang DataSource ng
    ' bracket grid. Dahil List(Of T) ang direktang bound (hindi
    ' BindingList), kailangang tawagin ang RefreshDataSource() ng
    ' GridControl kada Add/Remove row - hindi awtomatikong nakikita
    ' ng grid ang pagbabago sa laki ng listahan.
    Private _workingBrackets As New List(Of LeaveRuleBracketModel)

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    Public Sub SetPresenter(presenter As LeaveRulePresenter)
        _presenter = presenter
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Leave Settings > Rule"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Rule"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Settings_Leave
        End Get
    End Property

    ' =============================================
    ' LOAD
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupRuleListGrid()
        SetupBracketGrid()
        SetupFixedCombos()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

        ApplyReadOnlyMode(wbpMainCommands)
    End Function

    ' Master list (kanan) - TOP FORM na lang ang single edit path,
    ' gaya ng ibang tabs - walang inline editing dito.
    Private Sub SetupRuleListGrid()
        With gridviewLeaveRuleList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowGroupPanel = False
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' Bracket grid - DITO naman SINASADYANG PINAGANA ang inline
    ' editing, kaiba sa lahat ng ibang grid sa app na ito. Dahilan:
    ' ang mga bracket row ay hindi mga independiyenteng record na
    ' may sariling buhay (gaya ng isang Branch o isang Employee) -
    ' laging anak lang sila ng isang Rule, laging sabay silang
    ' na-e-edit at na-se-save bilang isang buong set. Walang
    ' kalituhan kung ano ang "totoong" pinagmulan ng datos dahil
    ' iisa lang - itong grid mismo.
    Private Sub SetupBracketGrid()
        With gridviewBrackets
            .OptionsBehavior.Editable = True
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ShowAutoFilterRow = False
        End With
    End Sub

    Private Sub SetupFixedCombos()
        cboEntitlementMethod.Properties.Items.Clear()
        cboEntitlementMethod.Properties.Items.AddRange(LeaveEntitlementMethod.All)

        cboComputeBasedOn.Properties.Items.Clear()
        cboComputeBasedOn.Properties.Items.AddRange(LeaveDateBasis.All)

        cboPlotBasedOn.Properties.Items.Clear()
        cboPlotBasedOn.Properties.Items.AddRange(LeaveDateBasis.All)

        cboAnniversaryPlotOn.Properties.Items.Clear()
        cboAnniversaryPlotOn.Properties.Items.AddRange(LeaveDateBasis.All)

        cboUnitOfMeasure.Properties.Items.Clear()
        cboUnitOfMeasure.Properties.Items.AddRange(LeaveUnitOfMeasure.All)
    End Sub

    Private Sub SetupCommandImages()

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Rule"

        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected Rule"

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_32
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected Rule"

        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ToolTip = "Reload from Database"

    End Sub

    ' =============================================
    ' HELPERS
    ' =============================================
    Private Shared Function ToNullableInt(value As Object) As Integer?
        If value Is Nothing OrElse IsDBNull(value) Then Return Nothing
        Return Convert.ToInt32(value)
    End Function

    ' Iisang hitsura ang Leave Group / Leave Type dropdowns.
    Private Shared Sub BindIdLookup(Of T)(editor As LookUpEdit, source As List(Of T), codeMember As String, nameMember As String)
        With editor.Properties
            .DataSource = source
            .DisplayMember = nameMember
            .ValueMember = "Id"
            .NullText = "[Select]"
            .Columns.Clear()
            .Columns.Add(New LookUpColumnInfo(codeMember, "Code", 90))
            .Columns.Add(New LookUpColumnInfo(nameMember, "Name", 220))
        End With
    End Sub

    ' =============================================
    ' ILeaveRuleMaintenanceView - HEADER FIELDS
    ' =============================================
    Public Property LeaveGroupId As Integer Implements ILeaveRuleMaintenanceView.LeaveGroupId
        Get
            Return If(ToNullableInt(lueLeaveGroup.EditValue), 0)
        End Get
        Set(value As Integer)
            lueLeaveGroup.EditValue = If(value = 0, CObj(Nothing), value)
        End Set
    End Property

    Public Property LeaveTypeId As Integer Implements ILeaveRuleMaintenanceView.LeaveTypeId
        Get
            Return If(ToNullableInt(lueLeaveType.EditValue), 0)
        End Get
        Set(value As Integer)
            lueLeaveType.EditValue = If(value = 0, CObj(Nothing), value)
        End Set
    End Property

    Public Property EntitlementMethod As String Implements ILeaveRuleMaintenanceView.EntitlementMethod
        Get
            Return cboEntitlementMethod.Text
        End Get
        Set(value As String)
            cboEntitlementMethod.Text = value
        End Set
    End Property

    Public Property ComputeBasedOn As String Implements ILeaveRuleMaintenanceView.ComputeBasedOn
        Get
            Return cboComputeBasedOn.Text
        End Get
        Set(value As String)
            cboComputeBasedOn.Text = value
        End Set
    End Property

    Public Property PlotBasedOn As String Implements ILeaveRuleMaintenanceView.PlotBasedOn
        Get
            Return cboPlotBasedOn.Text
        End Get
        Set(value As String)
            cboPlotBasedOn.Text = value
        End Set
    End Property

    Public Property AnniversaryPlotOn As String Implements ILeaveRuleMaintenanceView.AnniversaryPlotOn
        Get
            Return cboAnniversaryPlotOn.Text
        End Get
        Set(value As String)
            cboAnniversaryPlotOn.Text = value
        End Set
    End Property

    Public Property UnitOfMeasure As String Implements ILeaveRuleMaintenanceView.UnitOfMeasure
        Get
            Return cboUnitOfMeasure.Text
        End Get
        Set(value As String)
            cboUnitOfMeasure.Text = value
        End Set
    End Property

    Public Property HolidayIncluded As Boolean Implements ILeaveRuleMaintenanceView.HolidayIncluded
        Get
            Return chkHolidayIncluded.Checked
        End Get
        Set(value As Boolean)
            chkHolidayIncluded.Checked = value
        End Set
    End Property

    Public Property RequireAttachment As Boolean Implements ILeaveRuleMaintenanceView.RequireAttachment
        Get
            Return chkRequireAttachment.Checked
        End Get
        Set(value As Boolean)
            chkRequireAttachment.Checked = value
        End Set
    End Property

    Public Property Monetize As Boolean Implements ILeaveRuleMaintenanceView.Monetize
        Get
            Return chkMonetize.Checked
        End Get
        Set(value As Boolean)
            chkMonetize.Checked = value
        End Set
    End Property

    Public Property ShowEntitlement As Boolean Implements ILeaveRuleMaintenanceView.ShowEntitlement
        Get
            Return chkShowEntitlement.Checked
        End Get
        Set(value As Boolean)
            chkShowEntitlement.Checked = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ILeaveRuleMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' ILeaveRuleMaintenanceView - COMBO DATA SOURCES
    ' =============================================
    Public Sub BindLeaveGroups(items As List(Of LookupModel)) Implements ILeaveRuleMaintenanceView.BindLeaveGroups
        BindIdLookup(lueLeaveGroup, items, "Code", "Name")
    End Sub

    Public Sub BindLeaveTypes(items As List(Of LeaveTypeModel)) Implements ILeaveRuleMaintenanceView.BindLeaveTypes
        BindIdLookup(lueLeaveType, items, "Code", "Name")
    End Sub

    ' =============================================
    ' ILeaveRuleMaintenanceView - MASTER LIST
    ' =============================================
    Public Sub BindList(items As List(Of LeaveRuleModel)) Implements ILeaveRuleMaintenanceView.BindList
        gridconLeaveRuleList.DataSource = items
    End Sub

    ' =============================================
    ' ILeaveRuleMaintenanceView - BRACKET SUB-GRID
    ' =============================================
    Public Sub BindBrackets(items As List(Of LeaveRuleBracketModel)) Implements ILeaveRuleMaintenanceView.BindBrackets
        _workingBrackets = If(items, New List(Of LeaveRuleBracketModel))
        gridconBrackets.DataSource = Nothing
        gridconBrackets.DataSource = _workingBrackets
    End Sub

    Public Function GetBrackets() As List(Of LeaveRuleBracketModel) Implements ILeaveRuleMaintenanceView.GetBrackets
        ' Kinukumpirma lang natin dito - ang mismong laman ay
        ' updated na sa bawat pag-type ni user sa grid (parehong
        ' bound object), maliban na lang sa mga row na kasalukuyang
        ' in-edit pa (hindi pa na-commit) - kaya nag-CloseEditor muna
        ' tayo bago kunin.
        gridviewBrackets.CloseEditor()
        gridviewBrackets.UpdateCurrentRow()
        Return _workingBrackets
    End Function

    Private Sub btnAddBracket_Click(sender As Object, e As EventArgs) Handles btnAddBracket.Click
        _workingBrackets.Add(New LeaveRuleBracketModel())
        gridconBrackets.RefreshDataSource()
    End Sub

    Private Sub btnRemoveBracket_Click(sender As Object, e As EventArgs) Handles btnRemoveBracket.Click
        Dim focused = TryCast(gridviewBrackets.GetFocusedRow(), LeaveRuleBracketModel)
        If focused Is Nothing Then Return

        _workingBrackets.Remove(focused)
        gridconBrackets.RefreshDataSource()
    End Sub

    ' =============================================
    ' ILeaveRuleMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ILeaveRuleMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        lueLeaveGroup.Properties.ReadOnly = Not isEditable
        lueLeaveType.Properties.ReadOnly = Not isEditable
        cboEntitlementMethod.Properties.ReadOnly = Not isEditable
        cboComputeBasedOn.Properties.ReadOnly = Not isEditable
        cboPlotBasedOn.Properties.ReadOnly = Not isEditable
        cboAnniversaryPlotOn.Properties.ReadOnly = Not isEditable
        cboUnitOfMeasure.Properties.ReadOnly = Not isEditable
        chkHolidayIncluded.Properties.ReadOnly = Not isEditable
        chkRequireAttachment.Properties.ReadOnly = Not isEditable
        chkMonetize.Properties.ReadOnly = Not isEditable
        chkShowEntitlement.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconLeaveRuleList.Enabled = Not isEditable
        gridviewBrackets.OptionsBehavior.Editable = isEditable
        btnAddBracket.Enabled = isEditable
        btnRemoveBracket.Enabled = isEditable

        If isEditable Then
            If isNewRecord Then
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Save"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Save New Rule"
            Else
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Rule"
            End If
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Rule"
        End If

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected Rule"
        End If

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Public Sub ClearFields() Implements ILeaveRuleMaintenanceView.ClearFields
        lueLeaveGroup.EditValue = Nothing
        lueLeaveType.EditValue = Nothing
        cboEntitlementMethod.SelectedIndex = -1
        cboComputeBasedOn.SelectedIndex = -1
        cboPlotBasedOn.SelectedIndex = -1
        cboAnniversaryPlotOn.SelectedIndex = -1
        cboUnitOfMeasure.SelectedIndex = -1
        chkHolidayIncluded.Checked = False
        chkRequireAttachment.Checked = False
        chkMonetize.Checked = False
        chkShowEntitlement.Checked = True
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements ILeaveRuleMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements ILeaveRuleMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' MASTER LIST SELECTION
    ' =============================================
    Private Sub gridviewLeaveRuleList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewLeaveRuleList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewLeaveRuleList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewLeaveRuleList_Click(sender As Object, e As EventArgs) _
        Handles gridviewLeaveRuleList.Click

        If _isEditing Then Return
        If gridviewLeaveRuleList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewLeaveRuleList.GetFocusedRowCellValue("Id")
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

                    Dim confirm = XtraMessageBox.Show($"Are you sure you want to {action} this Rule?",
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
