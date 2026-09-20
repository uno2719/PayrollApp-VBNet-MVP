Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.ModuleManagement.Models
Imports Payroll.ModuleManagement.Presenters
Imports Payroll.ModuleManagement.Views

Public Class ucModuleCatalog
    Implements IModuleCatalogView
    Implements IAsyncLoadable

    Private _presenter As ModuleCatalogPresenter

    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    Private Const BTN_REFRESH As Integer = 5

    Public Sub SetPresenter(presenter As ModuleCatalogPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Administration > Module Management > Module Catalog"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Module Catalog"
        End Get
    End Property

    ' Ang buong Administration area ay Admin-only na sa AuthorizationService
    ' (Case "admin_Modules" sa frmMain ay hindi na dadaan sa PermissionService.
    ' CanView dahil AppSession.IsAdmin ang direktang tinitignan doon), kaya
    ' hindi na kailangan ng hiwalay na ModuleCode override dito. Ang shell
    ' (ucModuleManagement) ang may hawak niyan.

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupGrid()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewModuleList
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        End With
    End Sub

    ' ========================================================
    ' IModuleCatalogView
    ' ========================================================
    Public Sub BindModuleList(items As List(Of ModuleCatalogModel)) _
        Implements IModuleCatalogView.BindModuleList

        gridconModuleList.DataSource = items

    End Sub

    Public Sub BindGroupChoices(groups As List(Of String)) _
        Implements IModuleCatalogView.BindGroupChoices

        cboGroupName.Properties.Items.Clear()
        cboGroupName.Properties.Items.AddRange(groups)

    End Sub

    Public Property ModuleCode As String Implements IModuleCatalogView.ModuleCode
        Get
            Return txtModuleCode.Text
        End Get
        Set(value As String)
            txtModuleCode.Text = value
        End Set
    End Property

    Public Property ModuleName As String Implements IModuleCatalogView.ModuleName
        Get
            Return txtModuleName.Text
        End Get
        Set(value As String)
            txtModuleName.Text = value
        End Set
    End Property

    Public Property GroupName As String Implements IModuleCatalogView.GroupName
        Get
            Return cboGroupName.Text
        End Get
        Set(value As String)
            cboGroupName.Text = value
        End Set
    End Property

    Public Property SortOrder As Integer Implements IModuleCatalogView.SortOrder
        Get
            Return CInt(spnSortOrder.Value)
        End Get
        Set(value As Integer)
            spnSortOrder.Value = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IModuleCatalogView.IsActive
        Get
            Return chkIsActive.Checked
        End Get
        Set(value As Boolean)
            chkIsActive.Checked = value
        End Set
    End Property

    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IModuleCatalogView.SetFormMode

        ' Ang ModuleCode ay editable LAMANG kapag New. Kapag Edit na ng
        ' existing row, laging ReadOnly - protektado rin sa Presenter/
        ' Repository layer, pero dito pa lang, hindi na natin bibigyan
        ' ng pagkakataon si Uno na aksidenteng baguhin ito.
        txtModuleCode.Properties.ReadOnly = Not (isEditable AndAlso isNewRecord)

        txtModuleName.Properties.ReadOnly = Not isEditable
        cboGroupName.Properties.ReadOnly = Not isEditable
        spnSortOrder.Properties.ReadOnly = Not isEditable
        chkIsActive.Properties.ReadOnly = Not isEditable

        gridconModuleList.Enabled = Not isEditable

        If isEditable Then
            SetButton(BTN_NEW, If(isNewRecord, " Save", " Update"),
                     If(isNewRecord, My.Resources.icon_save_24, My.Resources.icon_saveAs_24))
            SetButton(BTN_EDIT, " Cancel", My.Resources.icon_cancel_24)
        Else
            SetButton(BTN_NEW, " New", My.Resources.icon_add_property_24_png)
            SetButton(BTN_EDIT, " Edit", My.Resources.icon_edit_property_24)
        End If

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Private Sub SetButton(index As Integer, caption As String, img As Image)
        With wbpMainCommands.Buttons.Item(index).Properties
            .Caption = caption
            .ImageOptions.Image = img
        End With
    End Sub

    Public Sub ClearFields() Implements IModuleCatalogView.ClearFields
        txtModuleCode.Text = String.Empty
        txtModuleName.Text = String.Empty
        cboGroupName.Text = String.Empty
        spnSortOrder.Value = 0
        chkIsActive.Checked = True
    End Sub

    Public Sub ShowMessageBox(message As String) Implements IModuleCatalogView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub ShowErrorBox(message As String) Implements IModuleCatalogView.ShowError
        ShowError(message)
    End Sub

    Public Function Confirm(message As String, caption As String) As Boolean _
        Implements IModuleCatalogView.Confirm

        Return XtraMessageBox.Show(message, caption,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) = DialogResult.Yes

    End Function

    ' ========================================================
    ' GRID SELECTION
    ' ========================================================
    Private Sub gridviewModuleList_FocusedRowChanged(
        sender As Object,
        e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewModuleList.FocusedRowChanged

        If _presenter Is Nothing OrElse _presenter.IsEditing Then Return
        If gridviewModuleList.FocusedRowHandle < 0 Then Return

        Dim id = gridviewModuleList.GetFocusedRowCellValue("RecordId")
        If id Is Nothing Then Return

        _presenter.SelectModule(CInt(id))

    End Sub

    ' ========================================================
    ' COMMANDS
    ' ========================================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Try
            Select Case tag

                Case "New"
                    If _presenter.IsEditing Then
                        Await _presenter.SaveAsync()
                    Else
                        _presenter.StartNew()
                    End If

                Case "Edit"
                    If _presenter.IsEditing Then
                        _presenter.CancelEdit()
                    Else
                        _presenter.StartEdit()
                    End If

                Case "Delete"
                    If _presenter.IsEditing Then Return
                    Await _presenter.DeleteAsync()

                Case "Refresh"
                    If _presenter.IsEditing Then Return
                    Await _presenter.RefreshListAsync()

            End Select

        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

End Class