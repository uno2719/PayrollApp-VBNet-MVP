Imports System.Linq

Public Class ucPayrollSettings
    Implements IAsyncLoadable

    Private _currentTab As String = "Compensation"

    Private ReadOnly _ucCompensation As ucCompensation
    Private ReadOnly _ucDeduction As ucPayrollFlaggedEntry
    Private ReadOnly _ucOvertime As ucPayrollRateEntry
    Private ReadOnly _ucHoliday As ucPayrollRateEntry
    Private ReadOnly _ucBonus As ucPayrollFlaggedEntry
    Private ReadOnly _ucLoan As ucLoan
    Private ReadOnly _ucCutoff As ucCutoff

    Public Sub New(
        compensationView As ucCompensation,
        deductionView As ucPayrollFlaggedEntry,
        overtimeView As ucPayrollRateEntry,
        holidayView As ucPayrollRateEntry,
        bonusView As ucPayrollFlaggedEntry,
        loanView As ucLoan,
        cutoffView As ucCutoff)

        InitializeComponent()

        _ucCompensation = compensationView
        _ucDeduction = deductionView
        _ucOvertime = overtimeView
        _ucHoliday = holidayView
        _ucBonus = bonusView
        _ucLoan = loanView
        _ucCutoff = cutoffView

        DockAllViews()
    End Sub

    Private Sub DockAllViews()
        _ucCompensation.Dock = DockStyle.Fill
        tabpageCompensation.Controls.Add(_ucCompensation)

        _ucDeduction.Dock = DockStyle.Fill
        tabpageDeduction.Controls.Add(_ucDeduction)

        _ucOvertime.Dock = DockStyle.Fill
        tabpageOvertime.Controls.Add(_ucOvertime)

        _ucHoliday.Dock = DockStyle.Fill
        tabpageHoliday.Controls.Add(_ucHoliday)

        _ucBonus.Dock = DockStyle.Fill
        tabpageBonus.Controls.Add(_ucBonus)

        _ucLoan.Dock = DockStyle.Fill
        tabpageLoan.Controls.Add(_ucLoan)

        _ucCutoff.Dock = DockStyle.Fill
        tabpageCutoff.Controls.Add(_ucCutoff)
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Payroll > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payroll Settings"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucCompensation.EnsureLoadedAsync()
    End Function

    Private Async Sub tabconPayrollSettings_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconPayrollSettings.SelectedPageChanged

        _currentTab = tabconPayrollSettings.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconPayrollSettings.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If
    End Sub

End Class