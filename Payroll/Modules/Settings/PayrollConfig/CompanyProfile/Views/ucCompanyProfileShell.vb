' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyProfileShell.vb
Imports System.Linq
Imports DocumentFormat.OpenXml.ExtendedProperties

Public Class ucCompanyProfileShell
    Implements IAsyncLoadable

    Private _currentTab As String = "Company Profile"

    ' 6 injected views - 3 magkaibang class (ucCompany, ucCompanyAgencyRegistration
    ' x4 na naka-configure sa magkaibang agencyType, ucCompanyBank) - tingnan ang
    ' AppComposition.BuildCompanyProfileView().
    Private ReadOnly _ucCompany As ucCompany
    Private ReadOnly _ucSSS As ucCompanyAgencyRegistration
    Private ReadOnly _ucPhilHealth As ucCompanyAgencyRegistration
    Private ReadOnly _ucPagIbig As ucCompanyAgencyRegistration
    Private ReadOnly _ucBIR As ucCompanyAgencyRegistration
    Private ReadOnly _ucBank As ucCompanyBank

    Public Sub New(
        companyView As ucCompany,
        sssView As ucCompanyAgencyRegistration,
        philHealthView As ucCompanyAgencyRegistration,
        pagIbigView As ucCompanyAgencyRegistration,
        birView As ucCompanyAgencyRegistration,
        bankView As ucCompanyBank)

        InitializeComponent()

        _ucCompany = companyView
        _ucSSS = sssView
        _ucPhilHealth = philHealthView
        _ucPagIbig = pagIbigView
        _ucBIR = birView
        _ucBank = bankView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucCompany.Dock = DockStyle.Fill
        tabpageCompanyProfile.Controls.Add(_ucCompany)

        _ucSSS.Dock = DockStyle.Fill
        tabpageSSS.Controls.Add(_ucSSS)

        _ucPhilHealth.Dock = DockStyle.Fill
        tabpagePhilHealth.Controls.Add(_ucPhilHealth)

        _ucPagIbig.Dock = DockStyle.Fill
        tabpagePagIbig.Controls.Add(_ucPagIbig)

        _ucBIR.Dock = DockStyle.Fill
        tabpageBIR.Controls.Add(_ucBIR)

        _ucBank.Dock = DockStyle.Fill
        tabpageBank.Controls.Add(_ucBank)
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Company > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Company Profile"
        End Get
    End Property

    ' Isang tab lang (Company Profile, unang bukas) ang agad nilo-load dito.
    ' Ang ibang 5 ay lazy - tingnan tabconCompany_SelectedPageChanged sa ibaba.
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucCompany.EnsureLoadedAsync()
    End Function

    ' Generic handler - gumagana kahit magkaiba ang 3 classes ng 6 tabs,
    ' basta't lahat sila ucBase (parehong ginawa mo sa Statutory Shell).
    Private Async Sub tabconCompany_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconCompany.SelectedPageChanged

        _currentTab = tabconCompany.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconCompany.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class