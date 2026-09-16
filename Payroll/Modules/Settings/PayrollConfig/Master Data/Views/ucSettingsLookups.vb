Imports System.Linq

Public Class ucSettingsLookups
    Implements IAsyncLoadable

    Private _currentTab As String = "Branch"

    ' 8 injected instances - IISANG class lang (ucLookupMaintenance),
    ' pero magkakahiwalay na object, bawat isa may sariling grid/data/
    ' Presenter (naka-configure na sa ibang tableName bago pa rito
    ' dumating - tingnan ang AppComposition.BuildSettingsLookupsView).
    Private ReadOnly _ucBranch As ucLookupMaintenance
    Private ReadOnly _ucDepartment As ucLookupMaintenance
    Private ReadOnly _ucPosition As ucLookupMaintenance
    Private ReadOnly _ucCategory As ucLookupMaintenance
    Private ReadOnly _ucJobClass As ucLookupMaintenance
    Private ReadOnly _ucHolidayGroup As ucLookupMaintenance
    Private ReadOnly _ucScheduleGroup As ucLookupMaintenance
    Private ReadOnly _ucBank As ucLookupMaintenance

    Public Sub New(
        branchView As ucLookupMaintenance,
        departmentView As ucLookupMaintenance,
        positionView As ucLookupMaintenance,
        categoryView As ucLookupMaintenance,
        jobClassView As ucLookupMaintenance,
        holidayGroupView As ucLookupMaintenance,
        scheduleGroupView As ucLookupMaintenance,
        bankView As ucLookupMaintenance)

        InitializeComponent()

        _ucBranch = branchView
        _ucDepartment = departmentView
        _ucPosition = positionView
        _ucCategory = categoryView
        _ucJobClass = jobClassView
        _ucHolidayGroup = holidayGroupView
        _ucScheduleGroup = scheduleGroupView
        _ucBank = bankView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucBranch.Dock = DockStyle.Fill
        tabpageBranch.Controls.Add(_ucBranch)

        _ucDepartment.Dock = DockStyle.Fill
        tabpageDepartment.Controls.Add(_ucDepartment)

        _ucPosition.Dock = DockStyle.Fill
        tabpagePosition.Controls.Add(_ucPosition)

        _ucCategory.Dock = DockStyle.Fill
        tabpageCategory.Controls.Add(_ucCategory)

        _ucJobClass.Dock = DockStyle.Fill
        tabpageJobClass.Controls.Add(_ucJobClass)

        _ucHolidayGroup.Dock = DockStyle.Fill
        tabpageHolidayGroup.Controls.Add(_ucHolidayGroup)

        _ucScheduleGroup.Dock = DockStyle.Fill
        tabpageScheduleGroup.Controls.Add(_ucScheduleGroup)

        _ucBank.Dock = DockStyle.Fill
        tabpageBank.Controls.Add(_ucBank)
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Master Data > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Master Data"
        End Get
    End Property

    ' =============================================
    ' LOAD - isang tab lang (Branch, unang bukas) ang agad
    ' nilo-load dito. Ang ibang 7 ay lazy - tingnan
    ' tabconMasterData_SelectedPageChanged sa ibaba.
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucBranch.EnsureLoadedAsync()
    End Function

    ' =============================================
    ' TAB CHANGED - generic para sa lahat ng 8 (iisang class lang
    ' silang lahat), kaya walang kailangang 8-way Select Case.
    ' =============================================
    Private Async Sub tabconMasterData_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconMasterData.SelectedPageChanged

        _currentTab = tabconMasterData.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconMasterData.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class