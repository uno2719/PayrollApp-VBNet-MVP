Imports System.Linq

Public Class ucLeaveSettingsShell
    Implements IAsyncLoadable

    Private _currentTab As String = "Group"

    ' 3 magkakaibang View class (hindi gaya ng Master Data na iisang
    ' ucLookupMaintenance lang ang paulit-ulit) - Group ay reused na
    ' ucLookupMaintenance (tblLeaveGroup), pero Type at Rule ay
    ' bagong control dahil may extra fields sila na wala sa generic
    ' Lookup shape. Naka-configure na ang bawat isa (Presenter na
    ' naka-inject) bago pa dumating dito - tingnan ang
    ' AppComposition.BuildLeaveSettingsView.
    Private ReadOnly _ucGroup As ucLookupMaintenance
    Private ReadOnly _ucType As ucLeaveType
    Private ReadOnly _ucRule As ucLeaveRule

    Public Sub New(
        groupView As ucLookupMaintenance,
        typeView As ucLeaveType,
        ruleView As ucLeaveRule)

        InitializeComponent()

        _ucGroup = groupView
        _ucType = typeView
        _ucRule = ruleView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucGroup.Dock = DockStyle.Fill
        tabpageGroup.Controls.Add(_ucGroup)

        _ucType.Dock = DockStyle.Fill
        tabpageType.Controls.Add(_ucType)

        _ucRule.Dock = DockStyle.Fill
        tabpageRule.Controls.Add(_ucRule)
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Leave Settings > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Leave Settings"
        End Get
    End Property

    ' =============================================
    ' LOAD - isang tab lang (Group, unang bukas) ang agad
    ' nilo-load. Ang Type at Rule ay lazy - tingnan
    ' tabconLeaveSettings_SelectedPageChanged sa ibaba.
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucGroup.EnsureLoadedAsync()
    End Function

    ' =============================================
    ' TAB CHANGED
    ' =============================================
    Private Async Sub tabconLeaveSettings_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconLeaveSettings.SelectedPageChanged

        _currentTab = tabconLeaveSettings.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconLeaveSettings.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class
