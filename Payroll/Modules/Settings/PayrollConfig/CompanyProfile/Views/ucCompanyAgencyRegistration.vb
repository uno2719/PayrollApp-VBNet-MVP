' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyAgencyRegistration.vb
Imports System.Linq
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views
Imports Payroll.GlobalShared.Models

Public Class ucCompanyAgencyRegistration
    Implements ICompanyAgencyRegistrationView, IAsyncLoadable

    Private _presenter As CompanyAgencyRegistrationPresenter
    Private _pageTitle As String = "Agency Registration"
    Private _employees As List(Of EmployeeContactLookupModel)

    Public Sub SetPresenter(presenter As CompanyAgencyRegistrationPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return _pageTitle
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _presenter.LoadAsync()

        btnSave.Enabled = HasEditAccess
    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Company
        End Get
    End Property

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    Private Function ToNullableInt(value As Object) As Integer?
        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        End If
        Return Convert.ToInt32(value)
    End Function

    ' === ICompanyAgencyRegistrationView ===

    Public Sub SetLabels(registrationNoLabel As String, branchLabel As String, displayName As String) _
        Implements ICompanyAgencyRegistrationView.SetLabels

        lblRegistrationNo.Text = registrationNoLabel
        lblBranch.Text = branchLabel
        lblTabPageTitle.Text = $"{displayName.ToUpper()} REGISTRATION"
        _pageTitle = $"{displayName} Registration"
    End Sub

    Public Sub SetEmployeeList(employees As List(Of EmployeeContactLookupModel)) _
        Implements ICompanyAgencyRegistrationView.SetEmployeeList

        _employees = employees

        For Each lookup In New DevExpress.XtraEditors.LookUpEdit() {lookupContactPerson, lookupPersonInCharge1, lookupPersonInCharge2}
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

        ' Palaging read-only na ang Position - "matic" na, hindi na type-able.
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

    Public Property RegistrationNo As String Implements ICompanyAgencyRegistrationView.RegistrationNo
        Get
            Return txtRegistrationNo.Text
        End Get
        Set(value As String)
            txtRegistrationNo.Text = value
        End Set
    End Property

    Public Property Branch As String Implements ICompanyAgencyRegistrationView.Branch
        Get
            Return txtBranch.Text
        End Get
        Set(value As String)
            txtBranch.Text = value
        End Set
    End Property

    Public Property Address1 As String Implements ICompanyAgencyRegistrationView.Address1
        Get
            Return txtAddress1.Text
        End Get
        Set(value As String)
            txtAddress1.Text = value
        End Set
    End Property

    Public Property Address2 As String Implements ICompanyAgencyRegistrationView.Address2
        Get
            Return txtAddress2.Text
        End Get
        Set(value As String)
            txtAddress2.Text = value
        End Set
    End Property

    Public Property Address3 As String Implements ICompanyAgencyRegistrationView.Address3
        Get
            Return txtAddress3.Text
        End Get
        Set(value As String)
            txtAddress3.Text = value
        End Set
    End Property

    Public Property Country As String Implements ICompanyAgencyRegistrationView.Country
        Get
            Return txtCountry.Text
        End Get
        Set(value As String)
            txtCountry.Text = value
        End Set
    End Property

    Public Property PostCode As String Implements ICompanyAgencyRegistrationView.PostCode
        Get
            Return txtPostCode.Text
        End Get
        Set(value As String)
            txtPostCode.Text = value
        End Set
    End Property

    Public Property TelephoneNo As String Implements ICompanyAgencyRegistrationView.TelephoneNo
        Get
            Return txtTelephoneNo.Text
        End Get
        Set(value As String)
            txtTelephoneNo.Text = value
        End Set
    End Property

    Public Property FaxNo As String Implements ICompanyAgencyRegistrationView.FaxNo
        Get
            Return txtFaxNo.Text
        End Get
        Set(value As String)
            txtFaxNo.Text = value
        End Set
    End Property

    Public Property ContactPersonRecordId As Integer? Implements ICompanyAgencyRegistrationView.ContactPersonRecordId
        Get
            Return ToNullableInt(lookupContactPerson.EditValue)
        End Get
        Set(value As Integer?)
            lookupContactPerson.EditValue = value
        End Set
    End Property

    Public Property ContactPersonEmail As String Implements ICompanyAgencyRegistrationView.ContactPersonEmail
        Get
            Return txtContactPersonEmail.Text
        End Get
        Set(value As String)
            txtContactPersonEmail.Text = value
        End Set
    End Property

    Public Property PersonInCharge1RecordId As Integer? Implements ICompanyAgencyRegistrationView.PersonInCharge1RecordId
        Get
            Return ToNullableInt(lookupPersonInCharge1.EditValue)
        End Get
        Set(value As Integer?)
            lookupPersonInCharge1.EditValue = value
        End Set
    End Property

    Public Property PersonInCharge1Email As String Implements ICompanyAgencyRegistrationView.PersonInCharge1Email
        Get
            Return txtPersonInCharge1Email.Text
        End Get
        Set(value As String)
            txtPersonInCharge1Email.Text = value
        End Set
    End Property

    Public Property PersonInCharge2RecordId As Integer? Implements ICompanyAgencyRegistrationView.PersonInCharge2RecordId
        Get
            Return ToNullableInt(lookupPersonInCharge2.EditValue)
        End Get
        Set(value As Integer?)
            lookupPersonInCharge2.EditValue = value
        End Set
    End Property

    Public Property PersonInCharge2Email As String Implements ICompanyAgencyRegistrationView.PersonInCharge2Email
        Get
            Return txtPersonInCharge2Email.Text
        End Get
        Set(value As String)
            txtPersonInCharge2Email.Text = value
        End Set
    End Property

    Public Property Remarks As String Implements ICompanyAgencyRegistrationView.Remarks
        Get
            Return memoRemarks.Text
        End Get
        Set(value As String)
            memoRemarks.Text = value
        End Set
    End Property

    Public Sub ShowMessage(message As String) Implements ICompanyAgencyRegistrationView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements ICompanyAgencyRegistrationView.ShowError
        MyBase.ShowError(message)
    End Sub

End Class