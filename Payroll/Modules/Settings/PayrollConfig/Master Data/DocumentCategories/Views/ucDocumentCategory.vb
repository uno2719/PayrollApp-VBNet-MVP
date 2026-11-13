Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.DocumentCategories.Models
Imports Payroll.DocumentCategories.Presenters
Imports Payroll.DocumentCategories.Views
Imports Payroll.GlobalShared.Constants

''' <summary>
''' Settings > Payroll Setup > Master Data > Document Categories: ang mga category ng 201 Files.
''' Required = binibilang sa "Profile %" ng Employee header. Inactive = nakatago sa picker at hindi na binibilang
''' (nananatili pa rin ang mga lumang file). Kapareho ng ucLeaveType ang itsura at daloy.
''' </summary>
Public Class ucDocumentCategory
    Implements IDocCategoryMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As DocCategoryPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DEACTIVATE As Integer = 3
    Private Const BTN_DELETE As Integer = 4
    ' index 5 = separator
    Private Const BTN_REFRESH As Integer = 6
    Private _isActive As Boolean = True
    ' index 7 = separator

    Public Sub SetPresenter(presenter As DocCategoryPresenter)
        _presenter = presenter
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Master Data > Document Categories"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Document Categories"
        End Get
    End Property

    ' Parehong permission ng ibang Master Data tabs.
    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Settings_MasterData
        End Get
    End Property

    ' =============================================
    ' LOAD
    ' =============================================
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

    ' Sadyang naka-OFF ang inline grid editing - TOP FORM na lang + buttons, gaya ng ibang Settings screens.
    Private Sub SetupGrid()
        With gridviewDocCategoryList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    Private Sub SetupCommandImages()

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Category"

        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"

        CommandBarHelper.InitDeactivateDeleteButtons(wbpMainCommands, BTN_DEACTIVATE, BTN_DELETE)

        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ToolTip = "Reload from Database"

    End Sub

    ' =============================================
    ' IDocCategoryMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property CategoryName As String Implements IDocCategoryMaintenanceView.CategoryName
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property

    Public Property IsRequired As Boolean Implements IDocCategoryMaintenanceView.IsRequired
        Get
            Return chkRequired.Checked
        End Get
        Set(value As Boolean)
            chkRequired.Checked = value
        End Set
    End Property

    Public Property SortOrder As Integer Implements IDocCategoryMaintenanceView.SortOrder
        Get
            Return Convert.ToInt32(spnSortOrder.Value)
        End Get
        Set(value As Integer)
            spnSortOrder.Value = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IDocCategoryMaintenanceView.IsActive
        Get
            Return _isActive
        End Get
        Set(value As Boolean)
            _isActive = value
            CommandBarHelper.ShowStatus(lblStatus, value)
            CommandBarHelper.RefreshToggleCaption(wbpMainCommands, BTN_DEACTIVATE, value, HasEditAccess)
        End Set
    End Property

    ' =============================================
    ' IDocCategoryMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of DocCategoryModel)) Implements IDocCategoryMaintenanceView.BindList
        gridconDocCategoryList.DataSource = items
    End Sub

    ' =============================================
    ' IDocCategoryMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IDocCategoryMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        txtName.Properties.ReadOnly = Not isEditable
        spnSortOrder.Properties.ReadOnly = Not isEditable
        chkRequired.Properties.ReadOnly = Not isEditable

        gridconDocCategoryList.Enabled = Not isEditable

        If isEditable Then
            If isNewRecord Then
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Save"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Save New Category"
            Else
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Record"
            End If
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Category"
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

    Public Sub ClearFields() Implements IDocCategoryMaintenanceView.ClearFields
        txtName.Text = String.Empty
        chkRequired.Checked = False
        spnSortOrder.Value = 0D     ' 0 = ilalagay sa dulo ng listahan (kapag bagong category)
        IsActive = True
    End Sub

    ' NOTE: DisplayInfo/DisplayValidationError - parehong pattern ng ucLeaveType (iba ang pangalan para hindi
    ' mag-conflict sa minanang ShowMessage/ShowError ng ucBase).
    Public Sub DisplayInfo(message As String) Implements IDocCategoryMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IDocCategoryMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewDocCategoryList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewDocCategoryList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewDocCategoryList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewDocCategoryList_Click(sender As Object, e As EventArgs) _
        Handles gridviewDocCategoryList.Click

        If _isEditing Then Return
        If gridviewDocCategoryList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewDocCategoryList.GetFocusedRowCellValue("Id")
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
                    If ConfirmSaveStatus() AndAlso ConfirmProfileImpact() Then
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
                    Dim message = $"Are you sure you want to {action} this category?"

                    ' Ang Required na category ay nakakaapekto sa Profile % ng lahat ng empleyado.
                    If IsRequired Then
                        message &= vbCrLf & vbCrLf & If(IsActive,
                            "It is marked Required: once inactive it is no longer counted in the employees' Profile %. Existing files are kept.",
                            "It is marked Required: once active again it is counted in the employees' Profile %.")
                    End If

                    Dim confirm = XtraMessageBox.Show(message, "Confirm",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.ToggleActiveSelectedAsync()
                    End If
                End If

            Case "Delete"
                If Not _isEditing Then
                    If Not _presenter.HasSelection() Then
                        XtraMessageBox.Show("Please select a category first.", "Delete",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ElseIf CommandBarHelper.ConfirmDelete(Me, $"'{CategoryName}'") Then
                        Await _presenter.DeleteSelectedAsync()
                    End If
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If

        End Select

    End Sub

    ' Kapag ine-edit ang INACTIVE na record at sine-save: itanong kung ibabalik sa Active o mananatiling Inactive.
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

    ' Ang bagong Required category, o ang pagpalit ng Required, ay babago sa Profile % ng lahat ng empleyado.
    Private Function ConfirmProfileImpact() As Boolean
        If Not _presenter.SaveChangesProfileCompleteness() Then Return True

        Dim message As String = If(IsRequired,
            "This category will be Required: employees without a file in it will show it as missing and their Profile % goes down.",
            "This category will no longer be Required: it is no longer counted in the employees' Profile %.")

        Return XtraMessageBox.Show(message & vbCrLf & vbCrLf & "Continue?", "Confirm",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
    End Function

End Class
