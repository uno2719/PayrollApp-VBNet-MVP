Imports System.Linq

' Shell na host ng 6 tabs. 4 distinct View classes lang (Compensation
' at Loan - 1 instance bawat isa; FlaggedEntry at RateEntry - 2
' instances bawat isa), gaya ng ginawa sa ucSettingsLookups (8 instances
' ng iisang ucLookupMaintenance class).
Public Class ucPayrollSettings
    Implements IAsyncLoadable

    Private _currentTab As String = "Compensation"

    Private ReadOnly _ucCompensation As ucCompensation
    Private ReadOnly _ucDeduction As ucPayrollFlaggedEntry
    Private ReadOnly _ucOvertime As ucPayrollRateEntry
    Private ReadOnly _ucHoliday As ucPayrollRateEntry
    Private ReadOnly _ucBonus As ucPayrollFlaggedEntry
    Private ReadOnly _ucLoan As ucLoan

    Public Sub New(
        compensationView As ucCompensation,
        deductionView As ucPayrollFlaggedEntry,
        overtimeView As ucPayrollRateEntry,
        holidayView As ucPayrollRateEntry,
        bonusView As ucPayrollFlaggedEntry,
        loanView As ucLoan)

        InitializeComponent()

        _ucCompensation = compensationView
        _ucDeduction = deductionView
        _ucOvertime = overtimeView
        _ucHoliday = holidayView
        _ucBonus = bonusView
        _ucLoan = loanView

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
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE
    ' =============================================
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

    ' =============================================
    ' LOAD - Compensation lang (unang tab) ang agad nilo-load.
    ' Ang ibang 5 ay lazy - tingnan tabconPayrollSettings_SelectedPageChanged.
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucCompensation.EnsureLoadedAsync()
    End Function

    ' =============================================
    ' TAB CHANGED
    ' =============================================
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
