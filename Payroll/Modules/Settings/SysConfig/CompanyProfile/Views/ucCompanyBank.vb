' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyBank.vb
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views
Imports Payroll.GlobalShared.Models

Public Class ucCompanyBank
    Implements ICompanyBankMaintenanceView, IAsyncLoadable

    Private _presenter As CompanyBankPresenter
    Private _isEditing As Boolean = False

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    Public Sub SetPresenter(presenter As CompanyBankPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Company > Bank"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Bank"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupGrid()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Function

    ' Sadyang naka-OFF ang inline grid editing, parehong dahilan gaya ng
    ' ucLookupMaintenance - ang TOP FORM + buttons na lang ang single edit path.
    Private Sub SetupGrid()
        With gridviewBankList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' === ICompanyBankMaintenanceView - FORM FIELDS ===

    Public Property BankName As String Implements ICompanyBankMaintenanceView.BankName
        Get
            Return txtBankName.Text
        End Get
        Set(value As String)
            txtBankName.Text = value
        End Set
    End Property

    Public Property BankCode As String Implements ICompanyBankMaintenanceView.BankCode
        Get
            Return txtBankCode.Text
        End Get
        Set(value As String)
            txtBankCode.Text = value
        End Set
    End Property

    Public Property AccountName As String Implements ICompanyBankMaintenanceView.AccountName
        Get
            Return txtAccountName.Text
        End Get
        Set(value As String)
            txtAccountName.Text = value
        End Set
    End Property

    Public Property AccountNo As String Implements ICompanyBankMaintenanceView.AccountNo
        Get
            Return txtAccountNo.Text
        End Get
        Set(value As String)
            txtAccountNo.Text = value
        End Set
    End Property

    Public Property Branch As String Implements ICompanyBankMaintenanceView.Branch
        Get
            Return txtBranch.Text
        End Get
        Set(value As String)
            txtBranch.Text = value
        End Set
    End Property

    Public Property Address1 As String Implements ICompanyBankMaintenanceView.Address1
        Get
            Return txtAddress1.Text
        End Get
        Set(value As String)
            txtAddress1.Text = value
        End Set
    End Property

    Public Property Address2 As String Implements ICompanyBankMaintenanceView.Address2
        Get
            Return txtAddress2.Text
        End Get
        Set(value As String)
            txtAddress2.Text = value
        End Set
    End Property

    Public Property Address3 As String Implements ICompanyBankMaintenanceView.Address3
        Get
            Return txtAddress3.Text
        End Get
        Set(value As String)
            txtAddress3.Text = value
        End Set
    End Property

    Public Property Country As String Implements ICompanyBankMaintenanceView.Country
        Get
            Return txtCountry.Text
        End Get
        Set(value As String)
            txtCountry.Text = value
        End Set
    End Property

    Public Property PostCode As String Implements ICompanyBankMaintenanceView.PostCode
        Get
            Return txtPostCode.Text
        End Get
        Set(value As String)
            txtPostCode.Text = value
        End Set
    End Property

    Public Property TelephoneNo As String Implements ICompanyBankMaintenanceView.TelephoneNo
        Get
            Return txtTelephoneNo.Text
        End Get
        Set(value As String)
            txtTelephoneNo.Text = value
        End Set
    End Property

    Public Property FaxNo As String Implements ICompanyBankMaintenanceView.FaxNo
        Get
            Return txtFaxNo.Text
        End Get
        Set(value As String)
            txtFaxNo.Text = value
        End Set
    End Property

    Public Property ContactPerson As String Implements ICompanyBankMaintenanceView.ContactPerson
        Get
            Return txtContactPerson.Text
        End Get
        Set(value As String)
            txtContactPerson.Text = value
        End Set
    End Property

    Public Property ContactPersonPosition As String Implements ICompanyBankMaintenanceView.ContactPersonPosition
        Get
            Return txtContactPersonPosition.Text
        End Get
        Set(value As String)
            txtContactPersonPosition.Text = value
        End Set
    End Property

    Public Property ContactPersonEmail As String Implements ICompanyBankMaintenanceView.ContactPersonEmail
        Get
            Return txtContactPersonEmail.Text
        End Get
        Set(value As String)
            txtContactPersonEmail.Text = value
        End Set
    End Property

    Public Property PersonInCharge1 As String Implements ICompanyBankMaintenanceView.PersonInCharge1
        Get
            Return txtPersonInCharge1.Text
        End Get
        Set(value As String)
            txtPersonInCharge1.Text = value
        End Set
    End Property

    Public Property PersonInCharge1Position As String Implements ICompanyBankMaintenanceView.PersonInCharge1Position
        Get
            Return txtPersonInCharge1Position.Text
        End Get
        Set(value As String)
            txtPersonInCharge1Position.Text = value
        End Set
    End Property

    Public Property PersonInCharge1Email As String Implements ICompanyBankMaintenanceView.PersonInCharge1Email
        Get
            Return txtPersonInCharge1Email.Text
        End Get
        Set(value As String)
            txtPersonInCharge1Email.Text = value
        End Set
    End Property

    Public Property PersonInCharge2 As String Implements ICompanyBankMaintenanceView.PersonInCharge2
        Get
            Return txtPersonInCharge2.Text
        End Get
        Set(value As String)
            txtPersonInCharge2.Text = value
        End Set
    End Property

    Public Property PersonInCharge2Position As String Implements ICompanyBankMaintenanceView.PersonInCharge2Position
        Get
            Return txtPersonInCharge2Position.Text
        End Get
        Set(value As String)
            txtPersonInCharge2Position.Text = value
        End Set
    End Property

    Public Property PersonInCharge2Email As String Implements ICompanyBankMaintenanceView.PersonInCharge2Email
        Get
            Return txtPersonInCharge2Email.Text
        End Get
        Set(value As String)
            txtPersonInCharge2Email.Text = value
        End Set
    End Property

    Public Property SwiftCode As String Implements ICompanyBankMaintenanceView.SwiftCode
        Get
            Return txtSwiftCode.Text
        End Get
        Set(value As String)
            txtSwiftCode.Text = value
        End Set
    End Property

    Public Property BranchNo As String Implements ICompanyBankMaintenanceView.BranchNo
        Get
            Return txtBranchNo.Text
        End Get
        Set(value As String)
            txtBranchNo.Text = value
        End Set
    End Property

    Public Property CustomerID As String Implements ICompanyBankMaintenanceView.CustomerID
        Get
            Return txtCustomerID.Text
        End Get
        Set(value As String)
            txtCustomerID.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ICompanyBankMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' === GRID ===

    Public Sub BindList(items As List(Of CompanyBankModel)) Implements ICompanyBankMaintenanceView.BindList
        gridconBankList.DataSource = items
    End Sub

    ' === STATE / UX ===

    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ICompanyBankMaintenanceView.SetFormMode

        _isEditing = isEditable

        Dim allFields As TextEdit() = {
            txtBankName, txtBankCode, txtAccountName, txtAccountNo, txtBranch,
            txtAddress1, txtAddress2, txtAddress3, txtCountry, txtPostCode,
            txtTelephoneNo, txtFaxNo, txtContactPerson, txtContactPersonPosition, txtContactPersonEmail,
            txtPersonInCharge1, txtPersonInCharge1Position, txtPersonInCharge1Email,
            txtPersonInCharge2, txtPersonInCharge2Position, txtPersonInCharge2Email,
            txtSwiftCode, txtBranchNo, txtCustomerID
        }

        For Each field In allFields
            field.Properties.ReadOnly = Not isEditable
        Next
        chkActive.Properties.ReadOnly = Not isEditable

        gridconBankList.Enabled = Not isEditable

        ' NEW / SAVE / UPDATE BUTTON
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

        ' EDIT / CANCEL BUTTON
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

    Public Sub ClearFields() Implements ICompanyBankMaintenanceView.ClearFields
        Dim allFields As TextEdit() = {
            txtBankName, txtBankCode, txtAccountName, txtAccountNo, txtBranch,
            txtAddress1, txtAddress2, txtAddress3, txtCountry, txtPostCode,
            txtTelephoneNo, txtFaxNo, txtContactPerson, txtContactPersonPosition, txtContactPersonEmail,
            txtPersonInCharge1, txtPersonInCharge1Position, txtPersonInCharge1Email,
            txtPersonInCharge2, txtPersonInCharge2Position, txtPersonInCharge2Email,
            txtSwiftCode, txtBranchNo, txtCustomerID
        }

        For Each field In allFields
            field.Text = String.Empty
        Next
        chkActive.Checked = True
    End Sub

    Public Sub ShowMessage(message As String) Implements ICompanyBankMaintenanceView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements ICompanyBankMaintenanceView.ShowError
        MyBase.ShowError(message)
    End Sub

    ' === GRID SELECTION ===

    Private Sub gridviewBankList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewBankList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewBankList.GetFocusedRowCellValue("BankId")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewBankList_Click(sender As Object, e As EventArgs) _
        Handles gridviewBankList.Click

        If _isEditing Then Return
        If gridviewBankList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewBankList.GetFocusedRowCellValue("BankId")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    ' === BUTTON COMMANDS ===

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

                    Dim confirm = XtraMessageBox.Show($"Are you sure you want to {action} this bank entry?",
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