' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyAgencyRegistration.vb
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Views

Public Class ucCompanyAgencyRegistration
    Implements ICompanyAgencyRegistrationView, IAsyncLoadable

    Private _presenter As CompanyAgencyRegistrationPresenter
    Private _pageTitle As String = "Agency Registration"

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
    End Function

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    ' === ICompanyAgencyRegistrationView ===

    ' SSS No./Branch, PhilHealth No./Branch, Pag-IBIG No./Branch, o TIN/RDO -
    ' depende sa kung sinong ucCompanyAgencyRegistration instance ito.
    Public Sub SetLabels(registrationNoLabel As String, branchLabel As String, displayName As String) _
        Implements ICompanyAgencyRegistrationView.SetLabels

        lblRegistrationNo.Text = registrationNoLabel
        lblBranch.Text = branchLabel
        lblTabPageTitle.Text = $"{displayName.ToUpper()} REGISTRATION"
        _pageTitle = $"{displayName} Registration"
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

    Public Property ContactPerson As String Implements ICompanyAgencyRegistrationView.ContactPerson
        Get
            Return txtContactPerson.Text
        End Get
        Set(value As String)
            txtContactPerson.Text = value
        End Set
    End Property

    Public Property ContactPersonPosition As String Implements ICompanyAgencyRegistrationView.ContactPersonPosition
        Get
            Return txtContactPersonPosition.Text
        End Get
        Set(value As String)
            txtContactPersonPosition.Text = value
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

    Public Property PersonInCharge1 As String Implements ICompanyAgencyRegistrationView.PersonInCharge1
        Get
            Return txtPersonInCharge1.Text
        End Get
        Set(value As String)
            txtPersonInCharge1.Text = value
        End Set
    End Property

    Public Property PersonInCharge1Position As String Implements ICompanyAgencyRegistrationView.PersonInCharge1Position
        Get
            Return txtPersonInCharge1Position.Text
        End Get
        Set(value As String)
            txtPersonInCharge1Position.Text = value
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

    Public Property PersonInCharge2 As String Implements ICompanyAgencyRegistrationView.PersonInCharge2
        Get
            Return txtPersonInCharge2.Text
        End Get
        Set(value As String)
            txtPersonInCharge2.Text = value
        End Set
    End Property

    Public Property PersonInCharge2Position As String Implements ICompanyAgencyRegistrationView.PersonInCharge2Position
        Get
            Return txtPersonInCharge2Position.Text
        End Get
        Set(value As String)
            txtPersonInCharge2Position.Text = value
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