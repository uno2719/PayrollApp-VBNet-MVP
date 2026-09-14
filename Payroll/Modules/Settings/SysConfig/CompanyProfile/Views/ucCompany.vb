' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompany.vb
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views

Public Class ucCompany
    Implements ICompanyView, IAsyncLoadable

    Private _presenter As CompanyPresenter

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
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    Private Sub btnUploadLogo_Click(sender As Object, e As EventArgs) Handles btnUploadLogo.Click
        _presenter.UploadLogo()
    End Sub

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

    Public Property ContactPerson As String Implements ICompanyView.ContactPerson
        Get
            Return txtContactPerson.Text
        End Get
        Set(value As String)
            txtContactPerson.Text = value
        End Set
    End Property

    Public Property ContactPersonPosition As String Implements ICompanyView.ContactPersonPosition
        Get
            Return txtContactPersonPosition.Text
        End Get
        Set(value As String)
            txtContactPersonPosition.Text = value
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

    Public Sub ShowLogo(fullPath As String) Implements ICompanyView.ShowLogo
        If String.IsNullOrWhiteSpace(fullPath) OrElse Not IO.File.Exists(fullPath) Then
            picLogo.EditValue = Nothing   ' pwede mo itong palitan ng default placeholder resource, e.g. My.Resources.Resources.img_default_logo, kung gagawa ka ng isa
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