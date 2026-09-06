Imports System.Linq

Public Class ucStatutorySettingsShell
    Implements IAsyncLoadable

    Private _currentTab As String = "SSS"

    ' 3 injected instances - iisang class lang (ucStatutorySettings),
    ' pero magkakahiwalay na object, bawat isa may sariling grid/data/
    ' Presenter (naka-configure na sa ibang tableName - tingnan ang
    ' AppComposition.BuildStatutorySettingsView).
    Private ReadOnly _ucSSS As ucStatutorySettings
    Private ReadOnly _ucPhilHealth As ucStatutorySettings
    Private ReadOnly _ucPagIbig As ucStatutorySettings

    Public Sub New(
        sssView As ucStatutorySettings,
        philHealthView As ucStatutorySettings,
        pagIbigView As ucStatutorySettings)

        InitializeComponent()

        _ucSSS = sssView
        _ucPhilHealth = philHealthView
        _ucPagIbig = pagIbigView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucSSS.Dock = DockStyle.Fill
        tabpageSSS.Controls.Add(_ucSSS)

        _ucPhilHealth.Dock = DockStyle.Fill
        tabpagePhilHealth.Controls.Add(_ucPhilHealth)

        _ucPagIbig.Dock = DockStyle.Fill
        tabpagePagIbig.Controls.Add(_ucPagIbig)
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Statutory > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Statutory"
        End Get
    End Property

    ' =============================================
    ' LOAD - isang tab lang (SSS, unang bukas) ang agad
    ' nilo-load dito. Ang ibang 2 ay lazy - tingnan
    ' tabconStatutory_SelectedPageChanged sa ibaba.
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucSSS.EnsureLoadedAsync()
    End Function

    ' =============================================
    ' TAB CHANGED - generic para sa lahat ng 3 (iisang class lang
    ' silang lahat), kaya walang kailangang 3-way Select Case.
    ' =============================================
    Private Async Sub tabconStatutory_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconStatutory.SelectedPageChanged

        _currentTab = tabconStatutory.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconStatutory.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class