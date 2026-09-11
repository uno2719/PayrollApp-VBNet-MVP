Imports System.Linq

Public Class ucIncomeTaxTableShell
    Implements IAsyncLoadable

    Private _currentTab As String = "Yearly"

    ' 5 injected instances - iisang class lang (ucIncomeTaxTable),
    ' pero magkakahiwalay na object, bawat isa may sariling grid/data/
    ' Presenter (naka-configure na sa ibang tableName - tingnan ang
    ' AppComposition.BuildIncomeTaxTableView).
    Private ReadOnly _ucYearly As ucIncomeTaxTable
    Private ReadOnly _ucMonthly As ucIncomeTaxTable
    Private ReadOnly _ucSemiMonthly As ucIncomeTaxTable
    Private ReadOnly _ucWeekly As ucIncomeTaxTable
    Private ReadOnly _ucDaily As ucIncomeTaxTable

    Public Sub New(
        yearlyView As ucIncomeTaxTable,
        monthlyView As ucIncomeTaxTable,
        semiMonthlyView As ucIncomeTaxTable,
        weeklyView As ucIncomeTaxTable,
        dailyView As ucIncomeTaxTable)

        InitializeComponent()

        _ucYearly = yearlyView
        _ucMonthly = monthlyView
        _ucSemiMonthly = semiMonthlyView
        _ucWeekly = weeklyView
        _ucDaily = dailyView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucYearly.Dock = DockStyle.Fill
        tabpageYearly.Controls.Add(_ucYearly)

        _ucMonthly.Dock = DockStyle.Fill
        tabpageMonthly.Controls.Add(_ucMonthly)

        _ucSemiMonthly.Dock = DockStyle.Fill
        tabpageSemiMonthly.Controls.Add(_ucSemiMonthly)

        _ucWeekly.Dock = DockStyle.Fill
        tabpageWeekly.Controls.Add(_ucWeekly)

        _ucDaily.Dock = DockStyle.Fill
        tabpageDaily.Controls.Add(_ucDaily)
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Income Tax Table > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Income Tax Table"
        End Get
    End Property

    ' =============================================
    ' LOAD - isang tab lang (Yearly, unang bukas) ang agad
    ' nilo-load dito. Ang ibang 4 ay lazy - tingnan
    ' tabconIncomeTax_SelectedPageChanged sa ibaba.
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucYearly.EnsureLoadedAsync()
    End Function

    ' =============================================
    ' TAB CHANGED - generic para sa lahat ng 5 (iisang class lang
    ' silang lahat), kaya walang kailangang 5-way Select Case.
    ' =============================================
    Private Async Sub tabconIncomeTax_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconIncomeTax.SelectedPageChanged

        _currentTab = tabconIncomeTax.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconIncomeTax.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class