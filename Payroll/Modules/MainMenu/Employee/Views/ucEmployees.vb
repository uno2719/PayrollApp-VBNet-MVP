Imports System.ComponentModel
Imports DevExpress.Mvvm.POCO
Imports DevExpress.Utils.Menu
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class ucEmployees
    Inherits GlobalShared.Base.ucBase
    Implements IAsyncLoadable

    ' =============================================
    ' STATE ENUM
    ' =============================================
    Private Enum FormState
        Idle
        New_Personal
        New_Employment
        New_Earnings
        New_Statutory
        Edit
    End Enum

    ' =============================================
    ' FIELDS
    ' =============================================
    Private _currentState As FormState = FormState.Idle
    Private _selectedRecordId As Integer = 0
    Private _currentTab As String = "Personal Info."

    Private _isSettingState As Boolean = False

    ' SubViews
    Private _ucPersonal As ucEmployeesPersonalInfo
    Private _ucEmployment As ucEmployeesEmployment
    Private _ucEarnings As ucEmployeesEarnings
    Private _ucStatutory As ucEmployeesStatutory

    ' Service
    Private ReadOnly _service As New Employee.Services.EmployeeService()

    ' Loans (read-only): para sa Loans badge sa header at sa quick view popup
    Private ReadOnly _loanService As Loans.Services.IEmployeeLoanService

    ' Header
    Private _headerModel As Employee.Models.EmployeeHeaderModel
    Private _chipStatus As GlobalShared.Controls.StatusChip
    Private _chipKind As GlobalShared.Controls.StatusChip
    Private _chipJoined As GlobalShared.Controls.StatusChip
    Private _chipCycle As GlobalShared.Controls.StatusChip
    Private _chipPayroll As GlobalShared.Controls.StatusChip
    Private _chipComplete As GlobalShared.Controls.StatusChip
    Private ReadOnly _chipTip As New System.Windows.Forms.ToolTip()

    ' Phase 3: 201 Files tab + Photo
    Private ReadOnly _fileService As New Employee.Services.EmployeeFileService()
    Private _ucDocuments As ucEmployeesDocuments
    Private _photoImage As Image
    Private _photoMenu As ContextMenuStrip
    Private _miPhotoChange As ToolStripMenuItem
    Private _miPhotoRemove As ToolStripMenuItem
    Private _actionsMenu As DXPopupMenu
    Private _miToggleActive As DXMenuItem
    Private _miRegularize As DXMenuItem
    Private _miPromote As DXMenuItem
    Private _miTransfer As DXMenuItem
    Private _miResign As DXMenuItem
    Private _miTerminate As DXMenuItem
    Private _miRetire As DXMenuItem

    ' Active loans ng napiling employee (galing sa UpdateHeaderAsync) - babala sa Actions dialog
    Private _activeLoanCount As Integer
    Private _activeLoanBalance As Decimal

    ' Button indexes
    Private Const BTN_NEW As Integer = 0
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3



    Public Sub New(personalInfoView As ucEmployeesPersonalInfo,
                   employmentView As ucEmployeesEmployment,
                   earningsView As ucEmployeesEarnings,
                   statutoryView As ucEmployeesStatutory,
                   loanService As Loans.Services.IEmployeeLoanService
                   )

        InitializeComponent()

        _loanService = loanService

        ' SAVE injected instances
        _ucPersonal = personalInfoView
        _ucEmployment = employmentView
        _ucEarnings = earningsView
        _ucStatutory = statutoryView

        ' Attach
        _ucPersonal.Dock = DockStyle.Fill
        tabpagePersonalInfo.Controls.Add(_ucPersonal)

        _ucEmployment.Dock = DockStyle.Fill
        tabpageEmployment.Controls.Add(_ucEmployment)

        _ucEarnings.Dock = DockStyle.Fill
        tabpageEarnings.Controls.Add(_ucEarnings)

        _ucStatutory.Dock = DockStyle.Fill
        tabpageStatutory.Controls.Add(_ucStatutory)

        AddHandler _ucPersonal.OnSaveCompleted,
        AddressOf HandleSaveCompleted

        AddHandler _ucEmployment.OnSaveCompleted,
        AddressOf HandleSaveCompleted

        AddHandler _ucEarnings.OnSaveCompleted,
        AddressOf HandleSaveCompleted

        AddHandler _ucStatutory.OnSaveCompleted,
        AddressOf HandleSaveCompleted

        ' Phase 3: 201 Files tab (hindi injected; ang service ay iisa lang at walang dependency)
        _ucDocuments = New ucEmployeesDocuments()
        _ucDocuments.Initialize(_fileService)
        _ucDocuments.Dock = DockStyle.Fill
        tabpage201Files.Controls.Add(_ucDocuments)
        AddHandler _ucDocuments.DocumentsChanged, AddressOf OnDocumentsChanged

        BuildHeaderUi()

    End Sub


    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Main > Employee > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Employee Profile"
        End Get
    End Property

    ' =============================================
    ' LOAD FORM
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        ' Setup button icons
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_personel_24
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_personel_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_32
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected"

        ' Setup grid
        SetupGrid()

        ' Load SubViews
        LoadSubView_Personal()
        LoadSubView_Employment()
        LoadSubView_Earnings()
        LoadSubView_Statutory()

        ' Load employee list
        Await LoadEmployeeListAsync()

        ' Set initial state
        SetFormState(FormState.Idle)

        ApplyReadOnlyMode(wbpMainCommands)

    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Main_Employees
        End Get
    End Property


    ' =============================================
    ' SUBVIEW LOADERS
    ' =============================================
    Private Sub LoadSubView_Personal()
        If _ucPersonal Is Nothing Then
            '_ucPersonal = New ucEmployeesPersonalInfo()
            'tabpagePersonalInfo.Controls.Add(_ucPersonal)
            '_ucPersonal.Dock = DockStyle.Fill
            'AddHandler _ucPersonal.OnSaveCompleted, AddressOf HandleSaveCompleted
            Throw New InvalidOperationException("Personal view was not injected.")
        End If
    End Sub

    Private Sub LoadSubView_Employment()
        If _ucEmployment Is Nothing Then
            '_ucEmployment = New ucEmployeesEmployment()
            'tabpageEmployment.Controls.Add(_ucEmployment)
            '_ucEmployment.Dock = DockStyle.Fill
            'AddHandler _ucEmployment.OnSaveCompleted, AddressOf HandleSaveCompleted
            Throw New InvalidOperationException("Employment view was not injected.")
        End If
    End Sub

    Private Sub LoadSubView_Earnings()
        If _ucEarnings Is Nothing Then
            _ucEarnings = New ucEmployeesEarnings()
            tabpageEarnings.Controls.Add(_ucEarnings)
            _ucEarnings.Dock = DockStyle.Fill
            AddHandler _ucEarnings.OnSaveCompleted, AddressOf HandleSaveCompleted
        End If
    End Sub

    Private Sub LoadSubView_Statutory()
        If _ucStatutory Is Nothing Then
            _ucStatutory = New ucEmployeesStatutory()
            tabpageStatutory.Controls.Add(_ucStatutory)
            _ucStatutory.Dock = DockStyle.Fill
            AddHandler _ucStatutory.OnSaveCompleted, AddressOf HandleSaveCompleted
        End If
    End Sub



    ' =============================================
    ' GRID SETUP
    ' =============================================
    Private Sub SetupGrid()
        With gridviewEmployeeList
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
            .OptionsView.ShowAutoFilterRow = True
        End With
    End Sub

    ' =============================================
    ' LOAD EMPLOYEE LIST
    ' =============================================
    Private Async Function LoadEmployeeListAsync() As Task
        Try
            Dim employees = Await _service.GetAllRecordsAsync(GetCurrentFilter())
            gridconEmployeeList.DataSource = employees
            SetupGridColumns()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Function

    Private Function GetCurrentFilter() As String
        Select Case rgFilter.SelectedIndex
            Case 0 : Return "Active"    ' IsActive=1, IsDeleted=0
            Case 1 : Return "Inactive"  ' IsActive=0, IsDeleted=0
            Case 2 : Return "All"       ' IsDeleted=0
            Case Else : Return "Active"
        End Select
    End Function

    Private Sub SetupGridColumns()
        For Each col As DevExpress.XtraGrid.Columns.GridColumn In gridviewEmployeeList.Columns
            col.Visible = False
        Next

        Dim colEmpNo = gridviewEmployeeList.Columns("EmployeeNo")
        If colEmpNo IsNot Nothing Then
            colEmpNo.Visible = True
            colEmpNo.Caption = "Emp. No."
            colEmpNo.VisibleIndex = 0
        End If

        Dim colFullName = gridviewEmployeeList.Columns("FullName")
        If colFullName IsNot Nothing Then
            colFullName.Visible = True
            colFullName.Caption = "Full Name"
            colFullName.VisibleIndex = 1
        End If

        gridviewEmployeeList.BestFitColumns()
    End Sub

    ' =============================================
    ' GRID — ROW SELECTED
    ' =============================================
    Private Sub gridviewEmployeeList_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewEmployeeList.FocusedRowChanged

        If _currentState <> FormState.Idle Then Return

        Dim recordId = gridviewEmployeeList.GetFocusedRowCellValue("RecordId")
        If recordId Is Nothing Then Return

        _selectedRecordId = CInt(recordId)
        LoadSelectedEmployee()

    End Sub
    Private Sub gridviewEmployeeList_Click(sender As Object, e As EventArgs) Handles gridviewEmployeeList.Click
        If _currentState <> FormState.Idle Then Return

        If gridviewEmployeeList.SelectedRowsCount > 0 Then
            Dim recordId = gridviewEmployeeList.GetFocusedRowCellValue("RecordId")
            If recordId Is Nothing Then Return

            _selectedRecordId = CInt(recordId)
            LoadSelectedEmployee()
        End If
    End Sub

    Private Sub LoadSelectedEmployee()
        If _selectedRecordId = 0 Then Return

        ' ✅ I-load lahat ng tabs agad
        _ucPersonal?.LoadEmployee(_selectedRecordId)

        If _ucEmployment IsNot Nothing Then
            _ucEmployment.LoadEmployee(_selectedRecordId)
        End If

        If _ucEarnings IsNot Nothing Then
            _ucEarnings.LoadEmployee(_selectedRecordId)
        End If

        If _ucStatutory IsNot Nothing Then
            _ucStatutory.LoadEmployee(_selectedRecordId)
        End If

        _ucDocuments?.SetRecord(_selectedRecordId)

        UpdateHeaderAsync()
    End Sub

    ' =============================================
    ' UPDATE HEADER
    ' =============================================
    Private Async Sub UpdateHeaderAsync()
        Dim recordId = _selectedRecordId
        Try
            Dim header = Await _service.GetHeaderAsync(recordId)
            If header Is Nothing OrElse recordId <> _selectedRecordId Then Return

            ' Loans ay pang-badge at pang-alert lang. Kapag pumalya ang query (hal. wala pang loan tables
            ' sa database na iyon), huwag masira ang buong header.
            Dim loans As List(Of GlobalShared.Models.EmployeeLoanModel)
            Try
                loans = Await _loanService.GetByEmployeeAsync(recordId, "Active")
            Catch ex As Exception
                Debug.WriteLine($"Header loans query failed: {ex.Message}")
                loans = New List(Of GlobalShared.Models.EmployeeLoanModel)()
            End Try

            ' Kung lumipat na ng ibang employee habang naghihintay, huwag nang i-apply.
            If recordId <> _selectedRecordId Then Return

            _activeLoanCount = loans.Count
            _activeLoanBalance = loans.Sum(Function(l) l.Balance)

            _headerModel = header
            ApplyHeaderState(Employee.Views.EmployeeHeaderPresentation.Build(header, loans, Date.Today))
            _ucDocuments?.SetEmployeeName(header.FullName)
            LoadPhotoAsync(recordId)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    ' =============================================
    ' HEADER UI (chips, alert, buttons)
    ' =============================================
    Private Sub BuildHeaderUi()
        _chipStatus = New GlobalShared.Controls.StatusChip() With {.Bold = True}
        _chipKind = New GlobalShared.Controls.StatusChip()
        _chipJoined = New GlobalShared.Controls.StatusChip()
        _chipCycle = New GlobalShared.Controls.StatusChip()
        _chipPayroll = New GlobalShared.Controls.StatusChip()
        _chipComplete = New GlobalShared.Controls.StatusChip()

        For Each chip In HeaderChips()
            chip.Visible = False
            pnlHdr_Chips.Controls.Add(chip)
        Next

        ' Actions menu: Regularize / Promote / Transfer | Resign / Terminate / Retire | Set inactive.
        ' (Salary adjustment ay hindi pa kasama - naghihintay sa HR.)
        _miRegularize = NewMovementItem("Regularize...", Employee.Models.MovementTypes.Regularize, False)
        _miPromote = NewMovementItem("Promote...", Employee.Models.MovementTypes.Promote, False)
        _miTransfer = NewMovementItem("Transfer...", Employee.Models.MovementTypes.Transfer, False)
        _miResign = NewMovementItem("Resign...", Employee.Models.MovementTypes.Resign, True)
        _miTerminate = NewMovementItem("Terminate...", Employee.Models.MovementTypes.Terminate, False)
        _miRetire = NewMovementItem("Retire...", Employee.Models.MovementTypes.Retire, False)

        _miToggleActive = New DXMenuItem("Set inactive", AddressOf OnToggleActiveClick)
        _miToggleActive.BeginGroup = True

        _actionsMenu = New DXPopupMenu()
        _actionsMenu.Items.Add(_miRegularize)
        _actionsMenu.Items.Add(_miPromote)
        _actionsMenu.Items.Add(_miTransfer)
        _actionsMenu.Items.Add(_miResign)
        _actionsMenu.Items.Add(_miTerminate)
        _actionsMenu.Items.Add(_miRetire)
        _actionsMenu.Items.Add(_miToggleActive)
        btnHdr_Actions.DropDownControl = _actionsMenu

        ' Photo: right-click sa larawan = Change / Remove (Can Edit sa "Employee Documents"). Ang built-in na menu
        ' ng PictureEdit (Load/Cut/Copy/Paste...) ay pinatay para hindi makapagpalit ng larawan na hindi nase-save.
        picEmployee.Properties.ShowMenu = False
        _miPhotoChange = New ToolStripMenuItem("Change photo...", Nothing, AddressOf OnChangePhotoClick)
        _miPhotoRemove = New ToolStripMenuItem("Remove photo", Nothing, AddressOf OnRemovePhotoClick)
        _photoMenu = New ContextMenuStrip()
        _photoMenu.Items.AddRange(New ToolStripItem() {_miPhotoChange, _miPhotoRemove})
        AddHandler _photoMenu.Opening, AddressOf OnPhotoMenuOpening
        picEmployee.ContextMenuStrip = _photoMenu

        UpdateHeaderButtons()
    End Sub

    Private Function NewMovementItem(caption As String, movementType As String, beginGroup As Boolean) As DXMenuItem
        Dim item As New DXMenuItem(caption, AddressOf OnMovementMenuClick)
        item.Tag = movementType
        item.BeginGroup = beginGroup
        Return item
    End Function

    Private Function HeaderChips() As GlobalShared.Controls.StatusChip()
        Return New GlobalShared.Controls.StatusChip() {_chipStatus, _chipKind, _chipJoined, _chipCycle, _chipPayroll, _chipComplete}
    End Function

    Private Shared Sub ApplyChip(chip As GlobalShared.Controls.StatusChip, state As Employee.Views.HeaderChipState)
        chip.Visible = state.Visible
        If Not state.Visible Then Return
        chip.SetColors(state.Back, state.Fore, state.Dot)
        chip.Text = state.Text
    End Sub

    Private Sub ApplyHeaderState(st As Employee.Views.EmployeeHeaderState)
        lblHdr_FullName.Text = st.FullName
        lblHdr_MainInfo.Text = st.SubLine

        pnlHdr_Chips.SuspendLayout()
        ApplyChip(_chipStatus, st.Status)
        ApplyChip(_chipKind, st.Kind)
        ApplyChip(_chipJoined, st.Joined)
        ApplyChip(_chipCycle, st.PayCycle)
        ApplyChip(_chipPayroll, st.PayrollChip)
        ApplyChip(_chipComplete, st.Completeness)
        _chipTip.SetToolTip(_chipComplete, st.CompletenessTip)
        pnlHdr_Chips.ResumeLayout(True)

        If String.IsNullOrEmpty(st.AlertText) Then
            lblHdr_Alert.Visible = False
        Else
            lblHdr_Alert.Text = st.AlertText
            lblHdr_Alert.ToolTip = st.AlertText
            lblHdr_Alert.Appearance.BackColor = st.AlertBack
            lblHdr_Alert.Appearance.ForeColor = st.AlertFore
            lblHdr_Alert.Visible = True
        End If

        btnHdr_Loans.Text = st.LoansCaption
        _miToggleActive.Caption = st.ToggleActiveCaption
        UpdateActionMenuState()

        UpdateHeaderButtons()
    End Sub

    ''' <summary>I-enable lang ang mga action na valid para sa kalagayan ng empleyado (EmployeeMovementRules).</summary>
    Private Sub UpdateActionMenuState()
        If _headerModel Is Nothing Then Return

        Dim ctx = Employee.Models.MovementContext.FromHeader(_headerModel)
        _miRegularize.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Regularize, ctx)
        _miPromote.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Promote, ctx)
        _miTransfer.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Transfer, ctx)
        _miResign.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Resign, ctx)
        _miTerminate.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Terminate, ctx)
        _miRetire.Enabled = Employee.Models.EmployeeMovementRules.IsAvailable(Employee.Models.MovementTypes.Retire, ctx)
    End Sub

    ''' <summary>
    ''' Header buttons: Loans = basta may napiling employee at Idle. Actions = dagdag pa ang
    ''' Edit access sa Employees AT ang "Employee Actions" access (Admin laging meron).
    ''' </summary>
    Private Sub UpdateHeaderButtons()
        Dim idle As Boolean = (_currentState = FormState.Idle)
        Dim hasRecord As Boolean = (_headerModel IsNot Nothing AndAlso
                                    _selectedRecordId > 0 AndAlso
                                    _headerModel.RecordId = _selectedRecordId)

        Dim canAct As Boolean = HasEditAccess AndAlso
            GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Actions)

        btnHdr_Actions.Enabled = idle AndAlso hasRecord AndAlso canAct
        btnHdr_Actions.ToolTip = If(canAct,
                                    "Employee actions",
                                    "Only Admin or users with Employee Actions access can use these actions.")
        btnHdr_Loans.Enabled = idle AndAlso hasRecord
        btnHdr_History.Enabled = idle AndAlso hasRecord
    End Sub

    Private Async Sub OnToggleActiveClick(sender As Object, e As EventArgs)
        If _headerModel Is Nothing OrElse _selectedRecordId = 0 OrElse
           _headerModel.RecordId <> _selectedRecordId Then Return

        Dim recordId = _selectedRecordId
        Dim who = If(String.IsNullOrWhiteSpace(_headerModel.FullName), "this employee", _headerModel.FullName)
        Dim makeActive = Not _headerModel.IsActive

        Dim title As String
        Dim prompt As String
        If makeActive Then
            title = "Reactivate Employee"
            prompt = $"Reactivate {who}?" & vbCrLf & vbCrLf &
                     "The employee will appear in the Active list again and can be included in payroll again."
        Else
            title = "Set Employee Inactive"
            prompt = $"Set {who} to Inactive?" & vbCrLf & vbCrLf &
                     "Inactive employees are hidden from the Active list. If there is no resignation, retirement, " &
                     "or termination date, they are also not included in payroll." & vbCrLf & vbCrLf &
                     "You can reverse this anytime using Actions > Reactivate."
        End If

        Dim answer = XtraMessageBox.Show(Me, prompt, title,
                                         MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button2)
        If answer <> DialogResult.Yes Then Return

        Try
            ShowLoading()
            Dim ok = Await _service.SetActiveStatusAsync(recordId, makeActive)
            Await RefreshAfterStatusChangeAsync()

            If ok Then
                ShowMessage(If(makeActive, "Employee reactivated.", "Employee set to Inactive."))
            Else
                ShowError("Nothing was changed - the record may no longer exist. The list has been refreshed.")
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            HideLoading()
        End Try
    End Sub

    Private Async Function RefreshAfterStatusChangeAsync() As Task
        If GetCurrentFilter() = "All" Then
            ' Nasa listahan pa rin ang employee (hindi ipinapakita ng grid ang IsActive) - header lang ang i-refresh.
            UpdateHeaderAsync()
            Return
        End If

        ' Aalis ang employee sa kasalukuyang listahan (Active/Inactive filter). Ang pag-rebind ng grid
        ' ang magpapa-select ng susunod na row (FocusedRowChanged) at magre-refresh ng header.
        Await LoadEmployeeListAsync()
        If gridviewEmployeeList.RowCount = 0 Then
            _selectedRecordId = 0
            ClearHeader()
        End If
    End Function

    ' Regularize / Promote / Transfer / Resign / Terminate / Retire: bubuksan ang dialog; ang dialog ang
    ' nagsusulat (isang transaction). Dito, refresh lang pagkatapos.
    Private Async Sub OnMovementMenuClick(sender As Object, e As EventArgs)
        Dim item = TryCast(sender, DXMenuItem)
        If item Is Nothing OrElse item.Tag Is Nothing Then Return
        If _headerModel Is Nothing OrElse _selectedRecordId = 0 OrElse
           _headerModel.RecordId <> _selectedRecordId Then Return

        Dim movementType = item.Tag.ToString()
        Dim saved As Boolean

        Using dlg As New frmEmployeeAction(_service, _headerModel, movementType,
                                           _activeLoanCount, _activeLoanBalance)
            saved = (dlg.ShowDialog(Me) = DialogResult.OK)
        End Using

        If Not saved Then Return

        Try
            ShowLoading()
            Await RefreshAfterMovementAsync(movementType)
            ShowMessage($"Employee {Employee.Models.MovementTypes.HistoryLabel(movementType).ToLowerInvariant()}. Recorded in History.")
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            HideLoading()
        End Try
    End Sub

    Private Async Function RefreshAfterMovementAsync(movementType As String) As Task
        ' Ang Resign/Terminate/Retire ay nagiging Inactive: sa Active list, aalis siya (parehong daan ng Set inactive).
        If Employee.Models.MovementTypes.IsSeparation(movementType) AndAlso GetCurrentFilter() = "Active" Then
            Await RefreshAfterStatusChangeAsync()
        Else
            ' Nagbago ang laman ng Employment tab (type/petsa/position...) kaya i-reload lahat ng tabs + header.
            LoadSelectedEmployee()
        End If
    End Function

    Private Sub btnHdr_History_Click(sender As Object, e As EventArgs) Handles btnHdr_History.Click
        If _headerModel Is Nothing OrElse _selectedRecordId = 0 Then Return

        Using dlg As New frmEmployeeHistory(_service, _selectedRecordId,
                                            _headerModel.EmployeeNo, _headerModel.FullName,
                                            _headerModel.DateJoined)
            dlg.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnHdr_Loans_Click(sender As Object, e As EventArgs) Handles btnHdr_Loans.Click
        If _headerModel Is Nothing OrElse _selectedRecordId = 0 Then Return

        Using dlg As New frmEmployeeLoansQuickView(_loanService, _selectedRecordId,
                                                   _headerModel.EmployeeNo, _headerModel.FullName)
            dlg.ShowDialog(Me)
        End Using
    End Sub

    ' =============================================
    ' TAB CHANGED
    ' =============================================
    Private Sub tabconMain_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconMain.SelectedPageChanged

        If _isSettingState Then Return

        _currentTab = tabconMain.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        ' Update Save button caption
        If _currentState = FormState.Edit Then
            UpdateSaveButtonCaption()
        End If

    End Sub

    ' =============================================
    ' FORM STATE MANAGEMENT
    ' =============================================
    Private Sub SetFormState(state As FormState)
        _isSettingState = True
        _currentState = state

        Select Case state

            Case FormState.Idle
                ' Grid
                gridconEmployeeList.Enabled = True

                ' wbp Buttons
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = True
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_personel_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_personel_24
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Delete"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_32
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected"

                ' Tabs — lahat enabled, controls disabled
                EnableTab(tabpagePersonalInfo, True)
                EnableTab(tabpageEmployment, True)
                EnableTab(tabpageEarnings, True)
                EnableTab(tabpageStatutory, True)
                EnableTab(tabpagePrevEmployer, True)
                EnableTab(tabpage201Files, CanView201Files())
                EnableTab(tabpageFixTransactions, True)

                ' Disable controls ng SubViews
                _ucPersonal?.SetEditMode(False)
                _ucEmployment?.SetEditMode(False)
                _ucEarnings?.SetEditMode(False)
                _ucStatutory?.SetEditMode(False)


            Case FormState.New_Personal
                gridconEmployeeList.Enabled = False

                ' wbp Buttons — hide New, Save = Edit btn, Cancel = Delete btn
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = False
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = "Save Personal Info"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Save"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Cancel"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Cancel"

                ' Tabs — Personal Info lang enabled
                EnableTab(tabpagePersonalInfo, True)
                EnableTab(tabpageEmployment, False)
                EnableTab(tabpageEarnings, False)
                EnableTab(tabpageStatutory, False)
                EnableTab(tabpagePrevEmployer, False)
                EnableTab(tabpage201Files, False)
                EnableTab(tabpageFixTransactions, False)

                ' Navigate to Personal Info tab
                tabconMain.SelectedTabPage = tabpagePersonalInfo

                ' ✅ I-ensure na naka-load ang SubView
                LoadSubView_Personal()

                ' Enable controls
                _ucPersonal?.SetEditMode(True)
                _ucPersonal?.RaiseNew()

            Case FormState.New_Employment
                gridconEmployeeList.Enabled = False

                ' wbp Buttons — Back = New btn
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = True
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Back"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_back_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Back"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = "Save Employment"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Save"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Cancel"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Cancel"

                ' Tabs — Employment lang enabled
                EnableTab(tabpagePersonalInfo, False)  ' pwede pa ring bumalik
                EnableTab(tabpageEmployment, True)
                EnableTab(tabpageEarnings, False)
                EnableTab(tabpageStatutory, False)
                EnableTab(tabpagePrevEmployer, False)
                EnableTab(tabpage201Files, False)
                EnableTab(tabpageFixTransactions, False)

                ' Navigate to Employment tab
                tabconMain.SelectedTabPage = tabpageEmployment

                ' Load SubView
                LoadSubView_Employment()
                _ucEmployment?.SetEditMode(True)
                _ucEmployment?.RaiseNew(_selectedRecordId)

            Case FormState.New_Earnings
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = True
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Back"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = "Save Earnings"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Cancel"

                EnableTab(tabpagePersonalInfo, False)
                EnableTab(tabpageEmployment, False)
                EnableTab(tabpageEarnings, True)
                EnableTab(tabpageStatutory, False)
                EnableTab(tabpagePrevEmployer, False)
                EnableTab(tabpage201Files, False)
                EnableTab(tabpageFixTransactions, False)

                tabconMain.SelectedTabPage = tabpageEarnings

                LoadSubView_Earnings()
                _ucEarnings?.SetEditMode(True)
                _ucEarnings?.RaiseNew(_selectedRecordId)

            Case FormState.New_Statutory
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = True
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Back"
                wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = "Save Statutory"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Cancel"

                EnableTab(tabpagePersonalInfo, False)
                EnableTab(tabpageEmployment, False)
                EnableTab(tabpageEarnings, False)
                EnableTab(tabpageStatutory, True)
                EnableTab(tabpagePrevEmployer, False)
                EnableTab(tabpage201Files, False)
                EnableTab(tabpageFixTransactions, False)

                tabconMain.SelectedTabPage = tabpageStatutory

                LoadSubView_Statutory()
                _ucStatutory?.SetEditMode(True)
                _ucStatutory?.RaiseNew(_selectedRecordId)

            Case FormState.Edit
                gridconEmployeeList.Enabled = False

                ' wbp Buttons
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = False
                UpdateSaveButtonCaption()
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Cancel"
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
                wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Cancel"

                ' Current tab lang enabled
                EnableOnlyCurrentTab()

                ' Enable controls ng current tab
                Select Case tabconMain.SelectedTabPageIndex
                    Case 0
                        _ucPersonal?.SetEditMode(True)
                    Case 1
                        LoadSubView_Employment()
                        _ucEmployment?.SetEditMode(True)
                    Case 2
                        LoadSubView_Earnings()
                        _ucEarnings?.SetEditMode(True)
                    Case 3
                        LoadSubView_Statutory()
                        _ucStatutory?.SetEditMode(True)

                End Select

        End Select

        ' Delete button: sa Idle, admin lang (at may Edit access). Sa ibang state ito ang
        ' Cancel button, kaya dapat laging enabled para makapag-Cancel ang non-admin na nag-e-edit.
        With wbpMainCommands.Buttons.Item(BTN_DELETE).Properties
            If state = FormState.Idle Then
                Dim canDelete = GlobalShared.Constants.AppSession.IsAdmin AndAlso HasEditAccess
                .Enabled = canDelete
                .ToolTip = If(GlobalShared.Constants.AppSession.IsAdmin,
                              "Delete Selected",
                              "Only administrators can delete records.")
            Else
                .Enabled = True
            End If
        End With

        UpdateHeaderButtons()

    End Sub

    ' =============================================
    ' HELPERS
    ' =============================================
    Private Sub EnableTab(page As DevExpress.XtraTab.XtraTabPage, enabled As Boolean)
        page.PageEnabled = enabled
    End Sub

    Private Sub EnableOnlyCurrentTab()
        EnableTab(tabpagePersonalInfo, tabconMain.SelectedTabPage Is tabpagePersonalInfo)
        EnableTab(tabpageEmployment, tabconMain.SelectedTabPage Is tabpageEmployment)
        EnableTab(tabpageEarnings, tabconMain.SelectedTabPage Is tabpageEarnings)
        EnableTab(tabpageStatutory, tabconMain.SelectedTabPage Is tabpageStatutory)
        EnableTab(tabpagePrevEmployer, tabconMain.SelectedTabPage Is tabpagePrevEmployer)
        EnableTab(tabpage201Files, tabconMain.SelectedTabPage Is tabpage201Files)
        EnableTab(tabpageFixTransactions, tabconMain.SelectedTabPage Is tabpageFixTransactions)
    End Sub

    Private Sub UpdateSaveButtonCaption()
        Dim caption = $"Update {tabconMain.SelectedTabPage.Text}"
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = caption
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image =
            My.Resources.icon_saveAs_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Amend"
    End Sub

    Private Sub SelectGridRow(recordId As Integer)
        ' Hanapin ang row na may matching RecordId
        Dim rowHandle = gridviewEmployeeList.LocateByValue("RecordId", recordId)

        ' Pag nahanap — i-select at i-focus
        If rowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle Then
            gridviewEmployeeList.ClearSelection()
            gridviewEmployeeList.FocusedRowHandle = rowHandle
            gridviewEmployeeList.SelectRow(rowHandle)

            ' ✅ I-scroll para makita yung selected row
            gridviewEmployeeList.MakeRowVisible(rowHandle)
        End If
    End Sub

    Private Sub ClearGridSelection()
        gridviewEmployeeList.ClearSelection()
        gridviewEmployeeList.FocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle
        ClearHeader()
    End Sub
    Private Sub ClearAllControls()
        ' I-clear lahat ng SubViews
        _ucPersonal?.RaiseNew()
        _ucEmployment?.RaiseNew(0)
        _ucEarnings?.RaiseNew(0)
        _ucStatutory?.RaiseNew(0)

        If _ucPersonal IsNot Nothing Then
            _ucPersonal.ClearFields()  ' ← gumawa tayo ng ClearFields
        End If                          '   instead of RaiseNew
        If _ucEmployment IsNot Nothing Then
            _ucEmployment.ClearFields()
        End If
        If _ucEarnings IsNot Nothing Then
            _ucEarnings?.ClearFields()
        End If
        If _ucStatutory IsNot Nothing Then
            _ucStatutory?.ClearFields()
        End If

    End Sub



    ' =============================================
    ' BUTTON EVENTS
    ' =============================================
    Private Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Select Case _currentState

            Case FormState.Idle
                Select Case tag
                    Case "New"
                        _selectedRecordId = 0  ' ✅ I-clear muna
                        ClearHeader()           ' ✅ I-clear ang header
                        SetFormState(FormState.New_Personal)
                    Case "Edit"
                        If _selectedRecordId = 0 Then
                            ShowMessage("Please select an employee first.")
                            Return
                        End If
                        SetFormState(FormState.Edit)
                    Case "Delete"
                        If _selectedRecordId = 0 Then
                            ShowMessage("Please select an employee first.")
                            Return
                        End If
                        HandleDelete()
                End Select

            Case FormState.New_Personal
                Select Case tag
                    Case "Edit" ' Save Personal Info
                        _ucPersonal?.RaiseSave()
                    Case "Delete" ' Cancel
                        HandleCancel()
                End Select

            Case FormState.New_Employment
                Select Case tag
                    Case "New" ' Back
                        SetFormState(FormState.New_Personal)
                        _ucPersonal?.LoadEmployee(_selectedRecordId)
                    Case "Edit" ' Save Employment
                        _ucEmployment?.RaiseSave()
                    Case "Delete" ' Cancel
                        HandleCancel()
                End Select

            Case FormState.New_Earnings
                Select Case tag
                    Case "New" ' Back
                        SetFormState(FormState.New_Employment)
                        _ucEmployment?.LoadEmployee(_selectedRecordId)
                    Case "Edit" ' Save Earnings
                        _ucEarnings?.RaiseSave()
                    Case "Delete" ' Cancel
                        HandleCancel()
                End Select

            Case FormState.New_Statutory
                Select Case tag
                    Case "New"
                        SetFormState(FormState.New_Earnings)
                        _ucEarnings?.LoadEmployee(_selectedRecordId)
                    Case "Edit"
                        _ucStatutory?.RaiseSave()  ' ✅ Hindi na commented out!
                    Case "Delete"
                        HandleCancel()
                End Select

            Case FormState.Edit
                Select Case tag
                    Case "Edit" ' Save current tab
                        Select Case tabconMain.SelectedTabPageIndex
                            Case 0
                                _ucPersonal?.RaiseSave()
                            Case 1
                                _ucEmployment?.RaiseSave()
                            Case 2
                                _ucEarnings?.RaiseSave()
                            Case 3
                                _ucStatutory?.RaiseSave()

                        End Select
                    Case "Delete" ' Cancel
                        HandleCancel()
                End Select

        End Select

    End Sub


    ' =============================================
    ' FILTER EVENT
    ' =============================================
    Private Async Sub rgFilter_SelectedIndexChanged(sender As Object, e As EventArgs) _
            Handles rgFilter.SelectedIndexChanged

        Await LoadEmployeeListAsync()
    End Sub


    ' =============================================
    ' SAVE COMPLETED — triggered by SubViews
    ' =============================================
    Private Async Sub HandleSaveCompleted(sender As Object, e As EventArgs)

        Select Case _currentState
            Case FormState.New_Personal
                ' Personal Info saved
                ' ✅ Kumuha ng bagong RecordId
                _selectedRecordId = _ucPersonal.RecordId

                ' ✅ I-reload ang grid agad para lumabas ang bagong employee
                Await LoadEmployeeListAsync()

                ' ✅ I-highlight yung bagong employee sa grid
                SelectGridRow(_selectedRecordId)

                ' ✅ Proceed to Employment
                SetFormState(FormState.New_Employment)

            Case FormState.New_Employment
                ' Employment saved — proceed to Earnings
                SetFormState(FormState.New_Earnings)

            Case FormState.New_Earnings
                ' Earnings saved — proceed to Statutory
                SetFormState(FormState.New_Statutory)

            Case FormState.New_Statutory
                ' Lahat saved! — balik sa Idle
                Await LoadEmployeeListAsync()
                ClearAllControls()      ' ✅
                ClearGridSelection()    ' ✅
                SetFormState(FormState.Idle)
                ShowMessage("New Employee saved successfully!")

            Case FormState.Edit
                Dim savedId = _selectedRecordId

                ' ✅ I-disable muna ang SubViews AGAD
                _ucPersonal?.SetEditMode(False)
                _ucEmployment?.SetEditMode(False)
                _ucEarnings?.SetEditMode(False)
                _ucStatutory?.SetEditMode(False)

                ' Per tab save — reload lang
                Await LoadEmployeeListAsync()
                SetFormState(FormState.Idle)

                ' ✅ I-restore ang selection
                If savedId > 0 Then
                    _selectedRecordId = savedId
                    SelectGridRow(savedId)
                    LoadSelectedEmployee()
                End If

                ShowMessage("Changes saved successfully!")
        End Select
    End Sub

    ' =============================================
    ' CANCEL
    ' =============================================
    Private Sub HandleCancel()
        ' ✅ I-clear muna ang validation warnings
        _ucPersonal?.ClearValidation()

        ClearAllControls()
        ClearGridSelection()
        SetFormState(FormState.Idle)
        _selectedRecordId = 0
        ClearHeader()
    End Sub

    ' =============================================
    ' DELETE
    ' =============================================
    Private Async Sub HandleDelete()
        ' Parehong security ng Settings Delete: admin lang, 2 confirm (default No) + password.
        ' Soft delete ito (IsDeleted = 1, hindi physical), kaya isSoftDelete:=True ang wording.
        Dim fullName = lblHdr_FullName.Text.Trim()
        Dim itemDescription = If(fullName.Length = 0, "this employee", $"employee '{fullName}'")

        If Not GlobalShared.Helpers.CommandBarHelper.ConfirmDelete(Me, itemDescription, isSoftDelete:=True) Then Return

        Try
            ShowLoading()
            Dim deleted = Await _service.DeleteAsync(_selectedRecordId)
            _selectedRecordId = 0
            Await LoadEmployeeListAsync()
            ClearHeader()
            SetFormState(FormState.Idle)
            If deleted Then
                ShowMessage("Employee deleted successfully!")
            Else
                ShowError("Nothing was deleted - the record may no longer exist. The list has been refreshed.")
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            HideLoading()
        End Try
    End Sub

    ' =============================================
    ' CLEAR HEADER
    ' =============================================
    Private Sub ClearHeader()
        _headerModel = Nothing

        lblHdr_FullName.Text = String.Empty
        lblHdr_MainInfo.Text = String.Empty

        If _chipStatus IsNot Nothing Then
            For Each chip In HeaderChips()
                chip.Visible = False
            Next
        End If

        lblHdr_Alert.Visible = False
        btnHdr_Loans.Text = "Loans"
        SetPhotoImage(Nothing)
        _ucDocuments?.SetRecord(0)

        UpdateHeaderButtons()
    End Sub

    ' =============================================
    ' PHOTO + 201 FILES (Phase 3)
    ' =============================================
    ''' <summary>Nothing = ibalik ang default na avatar. Dini-dispose ang naunang larawan (hindi ang resource).</summary>
    Private Sub SetPhotoImage(img As Image)
        Dim old As Image = _photoImage
        _photoImage = img
        If img IsNot Nothing Then
            picEmployee.EditValue = img
        Else
            picEmployee.EditValue = My.Resources.img_default_avatar1
        End If
        If old IsNot Nothing Then old.Dispose()
    End Sub

    Private Async Sub LoadPhotoAsync(recordId As Integer)
        Dim img As Image = Await _fileService.GetPhotoAsync(recordId)  ' hindi nagbabato; Nothing kung wala
        If recordId <> _selectedRecordId Then
            If img IsNot Nothing Then img.Dispose()
            Return
        End If
        SetPhotoImage(img)
    End Sub

    Private Function CanChangePhoto() As Boolean
        Return _currentState = FormState.Idle AndAlso
               _headerModel IsNot Nothing AndAlso _selectedRecordId > 0 AndAlso
               _headerModel.RecordId = _selectedRecordId AndAlso
               GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents)
    End Function

    Private Sub OnPhotoMenuOpening(sender As Object, e As System.ComponentModel.CancelEventArgs)
        Dim allowed As Boolean = CanChangePhoto()
        _miPhotoChange.Enabled = allowed
        _miPhotoRemove.Enabled = allowed AndAlso _photoImage IsNot Nothing
    End Sub

    Private Async Sub OnChangePhotoClick(sender As Object, e As EventArgs)
        If Not CanChangePhoto() Then Return
        Dim recordId As Integer = _selectedRecordId

        Dim path As String
        Using ofd As New OpenFileDialog()
            ofd.Title = "Choose a photo"
            ofd.CheckFileExists = True
            ofd.Filter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            If ofd.ShowDialog(Me) <> DialogResult.OK Then Return
            path = ofd.FileName
        End Using

        Try
            ShowLoading()
            Await _fileService.SetPhotoAsync(recordId, path)
            If recordId = _selectedRecordId Then
                SetPhotoImage(Await _fileService.GetPhotoAsync(recordId))
                UpdateHeaderAsync() ' para ma-update ang "Profile %"
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            HideLoading()
        End Try
    End Sub

    Private Async Sub OnRemovePhotoClick(sender As Object, e As EventArgs)
        If Not CanChangePhoto() Then Return
        Dim recordId As Integer = _selectedRecordId

        If XtraMessageBox.Show(Me, "Remove this employee's photo?", "Remove photo",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return

        Try
            ShowLoading()
            Await _fileService.RemovePhotoAsync(recordId)
            If recordId = _selectedRecordId Then
                SetPhotoImage(Nothing)
                UpdateHeaderAsync()
            End If
        Catch ex As Exception
            ShowError(ex.Message)
        Finally
            HideLoading()
        End Try
    End Sub

    ' Nagbago ang 201 Files (Add/Delete): i-refresh ang header para sa "Profile %".
    Private Sub OnDocumentsChanged(sender As Object, e As EventArgs)
        UpdateHeaderAsync()
    End Sub

    Private Shared Function CanView201Files() As Boolean
        Return GlobalShared.Security.PermissionService.CanView(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents)
    End Function


End Class
