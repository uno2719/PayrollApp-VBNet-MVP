' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompany.vb
Imports System.Linq
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views
Imports Payroll.GlobalShared.Models

Public Class ucCompany
    Implements ICompanyView, IAsyncLoadable

    Private _presenter As CompanyPresenter
    Private _employees As List(Of EmployeeContactLookupModel)

    Public Sub SetPresenter(presenter As CompanyPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Company Profile"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _presenter.LoadAsync()

        btnSave.Enabled = HasEditAccess
        btnUploadLogo.Enabled = HasEditAccess    ' nagpapalit ito ng naka-store na logo - mutating din
    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Company
        End Get
    End Property

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    Private Sub btnUploadLogo_Click(sender As Object, e As EventArgs) Handles btnUploadLogo.Click
        _presenter.UploadLogo()
    End Sub

    ' Hindi pwede ang TryCast sa Nullable(Of Integer) dahil value type siya -
    ' ito ang ginagamit sa halip, dito at sa EditValueChanged sa ibaba.
    Private Function ToNullableInt(value As Object) As Integer?
        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        End If
        Return Convert.ToInt32(value)
    End Function


    ' === ICompanyView ===

    Public Property CompanyCode As String Implements ICompanyView.CompanyCode
        Get
            Return txtCompanyCode.Text
        End Get
        Set(value As String)
            txtCompanyCode.Text = value
        End Set
    End Property

    Public Property CompanyName As String Implements ICompanyView.CompanyName
        Get
            Return txtCompanyName.Text
        End Get
        Set(value As String)
            txtCompanyName.Text = value
        End Set
    End Property

    Public Property Industry As String Implements ICompanyView.Industry
        Get
            Return txtIndustry.Text
        End Get
        Set(value As String)
            txtIndustry.Text = value
        End Set
    End Property

    Public Property Country As String Implements ICompanyView.Country
        Get
            Return txtCountry.Text
        End Get
        Set(value As String)
            txtCountry.Text = value
        End Set
    End Property

    Public Property PostCode As String Implements ICompanyView.PostCode
        Get
            Return txtPostCode.Text
        End Get
        Set(value As String)
            txtPostCode.Text = value
        End Set
    End Property

    Public Property TelephoneNo As String Implements ICompanyView.TelephoneNo
        Get
            Return txtTelephoneNo.Text
        End Get
        Set(value As String)
            txtTelephoneNo.Text = value
        End Set
    End Property

    Public Property FaxNo As String Implements ICompanyView.FaxNo
        Get
            Return txtFaxNo.Text
        End Get
        Set(value As String)
            txtFaxNo.Text = value
        End Set
    End Property

    Public Property ContactPersonRecordId As Integer? Implements ICompanyView.ContactPersonRecordId
        Get
            Return ToNullableInt(lookupContactPerson.EditValue)
        End Get
        Set(value As Integer?)
            lookupContactPerson.EditValue = value
        End Set
    End Property

    Public Property ContactPersonEmail As String Implements ICompanyView.ContactPersonEmail
        Get
            Return txtContactPersonEmail.Text
        End Get
        Set(value As String)
            txtContactPersonEmail.Text = value
        End Set
    End Property

    Public Property Address1 As String Implements ICompanyView.Address1
        Get
            Return txtAddress1.Text
        End Get
        Set(value As String)
            txtAddress1.Text = value
        End Set
    End Property

    Public Property Address2 As String Implements ICompanyView.Address2
        Get
            Return txtAddress2.Text
        End Get
        Set(value As String)
            txtAddress2.Text = value
        End Set
    End Property

    Public Property Address3 As String Implements ICompanyView.Address3
        Get
            Return txtAddress3.Text
        End Get
        Set(value As String)
            txtAddress3.Text = value
        End Set
    End Property

    Public Property SECRegistrationNo As String Implements ICompanyView.SECRegistrationNo
        Get
            Return txtSECRegistrationNo.Text
        End Get
        Set(value As String)
            txtSECRegistrationNo.Text = value
        End Set
    End Property

    Public Property Website As String Implements ICompanyView.Website
        Get
            Return txtWebsite.Text
        End Get
        Set(value As String)
            txtWebsite.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ICompanyView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    Public Sub SetEmployeeList(employees As List(Of EmployeeContactLookupModel)) Implements ICompanyView.SetEmployeeList
        _employees = employees

        With lookupContactPerson.Properties
            .DataSource = employees
            .DisplayMember = "FullName"
            .ValueMember = "RecordId"
            .NullText = "[Select employee]"
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("EmployeeNo", "Employee No.", 90))
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("FullName", "Name", 200))
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("PositionName", "Position", 150))
        End With
    End Sub

    ' Kapag pumili ng ibang employee, i-refresh agad ang read-only Position
    ' textbox mula sa parehong naka-bind na listahan - hindi na kailangan
    ' bumalik pa sa database para dito.
    Private Sub lookupContactPerson_EditValueChanged(sender As Object, e As EventArgs) _
        Handles lookupContactPerson.EditValueChanged

        Dim selectedId = ToNullableInt(lookupContactPerson.EditValue)
        Dim matched = _employees?.FirstOrDefault(Function(emp) emp.RecordId = selectedId)

        txtContactPersonPosition.Text = If(matched?.PositionName, "")
    End Sub

    Public Sub ShowLogo(fullPath As String) Implements ICompanyView.ShowLogo
        If String.IsNullOrWhiteSpace(fullPath) OrElse Not IO.File.Exists(fullPath) Then
            picLogo.EditValue = Nothing
            Return
        End If

        picLogo.EditValue = Image.FromFile(fullPath)
    End Sub

    Public Function PromptForLogoFile() As String Implements ICompanyView.PromptForLogoFile
        Using dlg As New OpenFileDialog()
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            dlg.Title = "Select Company Logo"

            If dlg.ShowDialog() = DialogResult.OK Then
                Return dlg.FileName
            End If

            Return Nothing
        End Using
    End Function

    Public Sub ShowMessage(message As String) Implements ICompanyView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements ICompanyView.ShowError
        MyBase.ShowError(message)
    End Sub

End Class