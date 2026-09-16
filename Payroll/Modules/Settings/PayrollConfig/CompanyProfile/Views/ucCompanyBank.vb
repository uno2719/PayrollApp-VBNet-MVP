' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyBank.vb
Imports System.Linq
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views
Imports Payroll.GlobalShared.Models

Public Class ucCompanyBank
    Implements ICompanyBankMaintenanceView, IAsyncLoadable

    Private _presenter As CompanyBankPresenter
    Private _isEditing As Boolean = False
    Private _employees As List(Of EmployeeContactLookupModel)

    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    Private Const BTN_REFRESH As Integer = 5

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

    Private Sub SetupGrid()
        With gridviewBankList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    Private Function ToNullableInt(value As Object) As Integer?
        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        End If
        Return Convert.ToInt32(value)
    End Function

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

    Public Property ContactPersonRecordId As Integer? Implements ICompanyBankMaintenanceView.ContactPersonRecordId
        Get
            Return ToNullableInt(lookupContactPerson.EditValue)
        End Get
        Set(value As Integer?)
            lookupContactPerson.EditValue = value
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

    Public Property PersonInCharge1RecordId As Integer? Implements ICompanyBankMaintenanceView.PersonInCharge1RecordId
        Get
            Return ToNullableInt(lookupPersonInCharge1.EditValue)
        End Get
        Set(value As Integer?)
            lookupPersonInCharge1.EditValue = value
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

    Public Property PersonInCharge2RecordId As Integer? Implements ICompanyBankMaintenanceView.PersonInCharge2RecordId
        Get
            Return ToNullableInt(lookupPersonInCharge2.EditValue)
        End Get
        Set(value As Integer?)
            lookupPersonInCharge2.EditValue = value
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

    ' === EMPLOYEE PICKER ===

    Public Sub SetEmployeeList(employees As List(Of EmployeeContactLookupModel)) _
        Implements ICompanyBankMaintenanceView.SetEmployeeList

        _employees = employees

        For Each lookup In New LookUpEdit() {lookupContactPerson, lookupPersonInCharge1, lookupPersonInCharge2}
            With lookup.Properties
                .DataSource = employees
                .DisplayMember = "FullName"
                .ValueMember = "RecordId"
                .NullText = "[Select employee]"
                .Columns.Clear()
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EmployeeNo", "Employee No.", 90))
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "Name", 200))
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PositionName", "Position", 150))
            End With
        Next

        txtContactPersonPosition.Properties.ReadOnly = True
        txtPersonInCharge1Position.Properties.ReadOnly = True
        txtPersonInCharge2Position.Properties.ReadOnly = True
    End Sub

    Private Sub lookupContactPerson_EditValueChanged(sender As Object, e As EventArgs) _
        Handles lookupContactPerson.EditValueChanged

        Dim selectedId = ToNullableInt(lookupContactPerson.EditValue)
        Dim matched = _employees?.FirstOrDefault(Function(emp) emp.RecordId = selectedId)
        txtContactPersonPosition.Text = If(matched?.PositionName, "")
    End Sub

    Private Sub lookupPersonInCharge1_EditValueChanged(sender As Object, e As EventArgs) _
        Handles lookupPersonInCharge1.EditValueChanged

        Dim selectedId = ToNullableInt(lookupPersonInCharge1.EditValue)
        Dim matched = _employees?.FirstOrDefault(Function(emp) emp.RecordId = selectedId)
        txtPersonInCharge1Position.Text = If(matched?.PositionName, "")
    End Sub

    Private Sub lookupPersonInCharge2_EditValueChanged(sender As Object, e As EventArgs) _
        Handles lookupPersonInCharge2.EditValueChanged

        Dim selectedId = ToNullableInt(lookupPersonInCharge2.EditValue)
        Dim matched = _employees?.FirstOrDefault(Function(emp) emp.RecordId = selectedId)
        txtPersonInCharge2Position.Text = If(matched?.PositionName, "")
    End Sub

    ' === GRID ===

    Public Sub BindList(items As List(Of CompanyBankModel)) Implements ICompanyBankMaintenanceView.BindList
        gridconBankList.DataSource = items
    End Sub

    ' === STATE / UX ===

    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ICompanyBankMaintenanceView.SetFormMode

        _isEditing = isEditable

        Dim textFields As TextEdit() = {
            txtBankName, txtBankCode, txtAccountName, txtAccountNo, txtBranch,
            txtAddress1, txtAddress2, txtAddress3, txtCountry, txtPostCode,
            txtTelephoneNo, txtFaxNo,
            txtContactPersonEmail, txtPersonInCharge1Email, txtPersonInCharge2Email,
            txtSwiftCode, txtBranchNo, txtCustomerID
        }

        For Each field In textFields
            field.Properties.ReadOnly = Not isEditable
        Next
        chkActive.Properties.ReadOnly = Not isEditable

        ' Ang 3 employee-picker - palaging naka-link sa isang tao lang,
        ' gawin ding ReadOnly kapag hindi editing (Position ay palaging
        ' ReadOnly, kahit sa editing mode - itinakda na sa SetEmployeeList)
        For Each lookup In New LookUpEdit() {lookupContactPerson, lookupPersonInCharge1, lookupPersonInCharge2}
            lookup.Properties.ReadOnly = Not isEditable
        Next

        gridconBankList.Enabled = Not isEditable

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

    Public Sub ClearFields() Implements ICompanyBankMaintenanceView.ClearFields
        Dim textFields As TextEdit() = {
            txtBankName, txtBankCode, txtAccountName, txtAccountNo, txtBranch,
            txtAddress1, txtAddress2, txtAddress3, txtCountry, txtPostCode,
            txtTelephoneNo, txtFaxNo,
            txtContactPersonEmail, txtContactPersonPosition,
            txtPersonInCharge1Email, txtPersonInCharge1Position,
            txtPersonInCharge2Email, txtPersonInCharge2Position,
            txtSwiftCode, txtBranchNo, txtCustomerID
        }

        For Each field In textFields
            field.Text = String.Empty
        Next

        lookupContactPerson.EditValue = Nothing
        lookupPersonInCharge1.EditValue = Nothing
        lookupPersonInCharge2.EditValue = Nothing

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