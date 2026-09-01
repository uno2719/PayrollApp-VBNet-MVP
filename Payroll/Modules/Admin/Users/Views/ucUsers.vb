Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.Users.Presenters
Imports Payroll.Users.Views

Public Class ucUsers
    Implements IUsersView
    Implements IAsyncLoadable

    Private _temporaryPassword As String = String.Empty
    Private _presenter As UsersPresenter
    Private _isEditing As Boolean = False
    Private ReadOnly _moduleAccessControls As New Dictionary(Of Integer, ComboBoxEdit)

    Private Const BTN_NEW As Integer = 0
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3

    Public Sub SetPresenter(presenter As UsersPresenter)
        _presenter = presenter
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "System Administration > Users"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "User Accounts"
        End Get
    End Property

    ' =============================================
    ' LOAD
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image =
            My.Resources.icon_add_personel_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image =
            My.Resources.icon_edit_personel_24
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image =
            My.Resources.icon_delete_32

        SetupGrid()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewUsersList
            .OptionsBehavior.Editable = False
            .OptionsBehavior.AutoPopulateColumns = False
            .OptionsView.ShowGroupPanel = False
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .OptionsView.ShowAutoFilterRow = True
        End With
    End Sub

    ' Ginawa nang deterministic (AddVisible, hindi manual VisibleIndex
    ' assignment) - ito yung ayos sa bug kung saan nasa maling pwesto
    ' ang "Admin" column (dahil una itong na-declare kaysa FirstName/
    ' LastName sa UserModel, kaya nagkakaroon ng ordering conflict kapag
    ' VisibleIndex lang ang ginagalaw).
    Private Sub SetupGridColumns()
        gridviewUsersList.Columns.Clear()

        gridviewUsersList.Columns.AddVisible("Username", "Username")
        gridviewUsersList.Columns.AddVisible("FirstName", "First Name")
        gridviewUsersList.Columns.AddVisible("LastName", "Last Name")
        gridviewUsersList.Columns.AddVisible("IsAdmin", "Admin")

        gridviewUsersList.BestFitColumns()
    End Sub

    ' =============================================
    ' IUsersView - PROPERTIES
    ' =============================================
    Public Property Username As String Implements IUsersView.Username
        Get
            Return txtUsername.Text
        End Get
        Set(value As String)
            txtUsername.Text = value
        End Set
    End Property

    Public Property SelectedEmployeeNo As String Implements IUsersView.SelectedEmployeeNo
        Get
            Return If(lueEmployee.EditValue?.ToString(), String.Empty)
        End Get
        Set(value As String)
            lueEmployee.EditValue = value
        End Set
    End Property

    Public Property IsAdmin As Boolean Implements IUsersView.IsAdmin
        Get
            Return chkIsAdmin.Checked
        End Get
        Set(value As Boolean)
            chkIsAdmin.Checked = value
        End Set
    End Property

    ' Public Property IsActive As Boolean Implements IUsersView.IsActive
    Private _isActive As Boolean
    Public Property IsActive As Boolean Implements IUsersView.IsActive
        Get
            Return _isActive
        End Get
        Set(value As Boolean)
            _isActive = value
            btnToggleActive.Text = If(_isActive, "Deactivate", "Reactivate")
        End Set
    End Property

    ' =============================================
    ' IUsersView - DATA BINDING
    ' =============================================
    Public Sub BindUsersList(users As List(Of Payroll.Login.Models.UserModel)) _
        Implements IUsersView.BindUsersList

        gridconUsersList.DataSource = users
        SetupGridColumns()
    End Sub

    Public Sub BindEmployeeChoices(employees As List(Of KeyValuePair(Of String, String))) _
        Implements IUsersView.BindEmployeeChoices

        lueEmployee.Properties.DataSource = employees
        lueEmployee.Properties.ValueMember = "Key"
        lueEmployee.Properties.DisplayMember = "Value"
        lueEmployee.Properties.Columns.Clear()
        lueEmployee.Properties.Columns.Add(
            New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Employee Name"))
    End Sub

    ' =============================================
    ' IUsersView - MODULE ACCESS (dynamic controls)
    ' =============================================
    Public Sub BindModuleAccess(items As List(Of Payroll.Users.Models.ModuleAccessItem)) _
        Implements IUsersView.BindModuleAccess

        pnlModuleAccess.Controls.Clear()
        _moduleAccessControls.Clear()

        Dim y As Integer = 8
        For Each item In items
            Dim lbl As New LabelControl()
            lbl.Text = item.ModuleName
            lbl.Location = New Point(8, y + 3)
            lbl.Size = New Size(150, 13)
            pnlModuleAccess.Controls.Add(lbl)

            Dim cbo As New ComboBoxEdit()
            cbo.Properties.Items.AddRange(New String() {"No Access", "View Only", "Can Edit"})
            cbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            cbo.Location = New Point(170, y)
            cbo.Size = New Size(160, 20)
            cbo.Tag = item.ModuleId
            pnlModuleAccess.Controls.Add(cbo)
            cbo.SelectedIndex = CInt(item.AccessLevel)

            _moduleAccessControls.Add(item.ModuleId, cbo)

            y += 28
        Next

        ' Kung naka-check na ang Administrator Access, itago agad - walang
        ' saysay ipakita ang per-module access kung full access na siya.
        pnlModuleAccess.Visible = Not chkIsAdmin.Checked
        lblModuleAccessHeader.Visible = Not chkIsAdmin.Checked
    End Sub

    Public Function GetModuleAccessSelections() As List(Of Payroll.Users.Models.ModuleAccessItem) _
        Implements IUsersView.GetModuleAccessSelections

        Dim result As New List(Of Payroll.Users.Models.ModuleAccessItem)

        For Each kvp In _moduleAccessControls
            result.Add(New Payroll.Users.Models.ModuleAccessItem With {
                .ModuleId = kvp.Key,
                .AccessLevel = CType(kvp.Value.SelectedIndex, Payroll.Users.Models.ModuleAccessLevel)
            })
        Next

        Return result
    End Function

    ' Kapag na-toggle ang Administrator Access checkbox - itago/ipakita
    ' na lang ang Module Access section, HINDI burahin ang laman nito.
    ' Kaya kapag na-uncheck ulit, babalik ang mga dating selection.
    Private Sub chkIsAdmin_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chkIsAdmin.CheckedChanged

        pnlModuleAccess.Visible = Not chkIsAdmin.Checked
        lblModuleAccessHeader.Visible = Not chkIsAdmin.Checked
    End Sub

    ' =============================================
    ' IUsersView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IUsersView.SetFormMode

        _isEditing = isEditable

        ' Username/Employee - editable lang kapag "New." Kapag "Edit" ng
        ' existing user, ipapakita pa rin sila pero READONLY (hindi dapat
        ' baguhin ang mga ito pagkatapos ma-create ang account).
        txtUsername.Properties.ReadOnly = Not (isEditable AndAlso isNewRecord)
        lueEmployee.Properties.ReadOnly = Not (isEditable AndAlso isNewRecord)

        chkIsAdmin.Properties.ReadOnly = Not isEditable

        For Each ctrl As Control In pnlModuleAccess.Controls
            Dim cbo = TryCast(ctrl, ComboBoxEdit)
            If cbo IsNot Nothing Then
                cbo.Properties.ReadOnly = Not isEditable
            End If
        Next

        btnResetPassword.Enabled = Not isEditable
        btnToggleActive.Enabled = Not isEditable
        btnToggleActive.Text = If(IsActive, "Deactivate", "Reactivate")

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = False
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = "Save"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image =
                My.Resources.icon_save_24
            wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = "Cancel"
            wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image =
                My.Resources.icon_cancel_24
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = True
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image =
                My.Resources.icon_add_personel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image =
                My.Resources.icon_edit_personel_24
            wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption = " Delete"
            wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image =
                My.Resources.icon_delete_32
        End If
    End Sub

    Public Sub ClearFields() Implements IUsersView.ClearFields
        txtUsername.Text = String.Empty
        lueEmployee.EditValue = Nothing
        chkIsAdmin.Checked = False
        pnlModuleAccess.Controls.Clear()
        _moduleAccessControls.Clear()
        lblStatusMessage.Text = String.Empty
    End Sub

    Public Sub ShowTemporaryPassword(password As String) Implements IUsersView.ShowTemporaryPassword
        UpdateTemporaryPasswordDisplay(password, True)
    End Sub

    Public Sub HideTemporaryPassword() Implements IUsersView.HideTemporaryPassword
        UpdateTemporaryPasswordDisplay(String.Empty, False)
    End Sub

    Private Sub UpdateTemporaryPasswordDisplay(password As String, isVisible As Boolean)
        _temporaryPassword = password

        If isVisible Then
            lblTempPassword.Text =
            $"Temporary Password: {password}" & vbCrLf &
            "Please share this securely with the employee. This will not be shown again."
        Else
            lblTempPassword.Text = String.Empty
        End If

        lblTempPassword.Visible = isVisible
        btnCopyTempPassword.Visible = isVisible

        btnCopyTempPassword.Enabled = isVisible AndAlso Not String.IsNullOrWhiteSpace(password)
    End Sub

    ' NOTE: pinangalanan itong DisplayInfo/DisplayValidationError (hindi
    ' ShowMessage/ShowError) para hindi mag-conflict sa parehong pangalan
    ' na method na minana natin mula sa GlobalShared.Base.ucBase.
    Public Sub DisplayInfo(message As String) Implements IUsersView.ShowMessage
        lblStatusMessage.Appearance.ForeColor = Color.SeaGreen
        lblStatusMessage.Appearance.Options.UseForeColor = True
        lblStatusMessage.Text = message
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IUsersView.ShowError
        lblStatusMessage.Appearance.ForeColor = Color.Firebrick
        lblStatusMessage.Appearance.Options.UseForeColor = True
        lblStatusMessage.Text = message
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' May dalawang handler dito (FocusedRowChanged AT Click) - parehong
    ' pattern gaya ng ucEmployees, dahil minsan hindi maasahan mag-isa
    ' ang FocusedRowChanged (ito yung dahilan ng "Please select a user"
    ' bug kahit may selected na row).
    ' =============================================
    Private Async Sub gridviewUsersList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewUsersList.FocusedRowChanged

        If _isEditing Then Return

        Dim recordId = gridviewUsersList.GetFocusedRowCellValue("RecordId")
        If recordId Is Nothing Then Return

        Await _presenter.SelectUserAsync(CInt(recordId))
    End Sub

    Private Async Sub gridviewUsersList_Click(sender As Object, e As EventArgs) _
        Handles gridviewUsersList.Click

        If _isEditing Then Return
        If gridviewUsersList.SelectedRowsCount = 0 Then Return

        Dim recordId = gridviewUsersList.GetFocusedRowCellValue("RecordId")
        If recordId Is Nothing Then Return

        Await _presenter.SelectUserAsync(CInt(recordId))
    End Sub

    ' =============================================
    ' FILTER
    ' =============================================
    Private Async Sub rgFilter_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles rgFilter.SelectedIndexChanged

        Dim filter As String
        Select Case rgFilter.SelectedIndex
            Case 0 : filter = "Active"
            Case 1 : filter = "Inactive"
            Case 2 : filter = "All"
            Case Else : filter = "Active"
        End Select

        Await _presenter.LoadUsersListAsync(filter)
    End Sub

    ' =============================================
    ' BUTTON COMMANDS - itaas (New / Edit / Delete)
    ' =============================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        If Not _isEditing Then
            Select Case tag
                Case "New"
                    _presenter.StartNew()
                Case "Edit"
                    _presenter.StartEdit()
                Case "Delete"
                    Dim confirm = XtraMessageBox.Show(
                        "Are you sure you want to DELETE this user account? This cannot be undone.",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.DeleteSelectedAsync()
                    End If
            End Select
        Else
            Select Case tag
                Case "Edit" ' Save
                    Await _presenter.SaveAsync()
                Case "Delete" ' Cancel
                    _presenter.CancelEdit()
            End Select
        End If

    End Sub

    ' =============================================
    ' BUTTON COMMANDS - ibaba (Reset Password / Toggle Active)
    ' =============================================
    Private Async Sub btnResetPassword_Click(sender As Object, e As EventArgs) _
        Handles btnResetPassword.Click

        Dim confirm = XtraMessageBox.Show(
            "Generate a new temporary password for this user?",
            "Confirm Reset Password",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            Await _presenter.ResetPasswordSelectedAsync()
        End If
    End Sub

    Private Async Sub btnToggleActive_Click(sender As Object, e As EventArgs) _
        Handles btnToggleActive.Click

        Dim action = If(IsActive, "deactivate", "reactivate")
        Dim confirm = XtraMessageBox.Show(
            $"Are you sure you want to {action} this user?",
            "Confirm",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            Await _presenter.ToggleActiveSelectedAsync
            ' btnToggleActive.Text = If(IsActive, "Deactivate", "Reactivate")
        End If
    End Sub

    Private Async Sub btnCopyTempPassword_Click(sender As Object, e As EventArgs) Handles btnCopyTempPassword.Click
        If String.IsNullOrWhiteSpace(_temporaryPassword) Then Return

        Clipboard.SetText(_temporaryPassword)

        DisplayInfo("Temporary password copied to clipboard.")
        Dim originalText = btnCopyTempPassword.Text

        btnCopyTempPassword.Text = "Copied!"
        btnCopyTempPassword.Enabled = False

        Await Task.Delay(2000)

        btnCopyTempPassword.Text = originalText
        btnCopyTempPassword.Enabled = True
    End Sub
End Class