Imports System.ComponentModel
Imports DevExpress.Mvvm.POCO
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

    ' Button indexes
    Private Const BTN_NEW As Integer = 0
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3



    Public Sub New(personalInfoView As ucEmployeesPersonalInfo,
                   employmentView As ucEmployeesEmployment,
                   earningsView As ucEmployeesEarnings,
                   statutoryView As ucEmployeesStatutory
                   )

        InitializeComponent()

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

        ' Setup SimpleButtons — hide muna
        SimpleButton1.Visible = False
        SimpleButton2.Visible = False
        SimpleButton3.Visible = False
        SimpleButton4.Visible = False

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

    End Function

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

        UpdateHeaderAsync()
    End Sub

    ' =============================================
    ' UPDATE HEADER
    ' =============================================
    Private Async Sub UpdateHeaderAsync()
        Try
            Dim emp = Await _service.GetByIdAsync(_selectedRecordId)
            If emp Is Nothing Then Return

            lblHdr_FullName.Text = emp.FullName

            ' ✅ EmployeeNo + PositionName
            If String.IsNullOrEmpty(emp.PositionName) Then
                lblHdr_MainInfo.Text = emp.EmployeeNo
            Else
                lblHdr_MainInfo.Text = $"{emp.EmployeeNo} | {StrConv(emp.PositionName, VbStrConv.ProperCase)}"
            End If

            lblHdr_ActiveIndicator.Text = If(emp.IsActive, "Active", "Inactive")
            lblHdr_ActiveIndicator.Appearance.ForeColor = If(emp.IsActive,
                                                         Color.SeaGreen,
                                                         Color.Red)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
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

                ' SimpleButtons — hide lahat
                SimpleButton1.Visible = False
                SimpleButton2.Visible = False
                SimpleButton3.Visible = False
                SimpleButton4.Visible = False

                ' Tabs — lahat enabled, controls disabled
                EnableTab(tabpagePersonalInfo, True)
                EnableTab(tabpageEmployment, True)
                EnableTab(tabpageEarnings, True)
                EnableTab(tabpageStatutory, True)
                EnableTab(tabpagePrevEmployer, True)
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
        Dim confirm = DevExpress.XtraEditors.XtraMessageBox.Show(
            "Are you sure you want to delete this employee?",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

        If confirm <> DialogResult.Yes Then Return

        Try
            ShowLoading()
            Await _service.DeleteAsync(_selectedRecordId)
            _selectedRecordId = 0
            Await LoadEmployeeListAsync()
            ClearHeader()
            SetFormState(FormState.Idle)
            ShowMessage("Employee deleted successfully!")
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
        lblHdr_FullName.Text = String.Empty
        lblHdr_MainInfo.Text = String.Empty
        lblHdr_ActiveIndicator.Text = String.Empty
        picEmployee.EditValue = My.Resources.img_default_avatar1
    End Sub


End Class