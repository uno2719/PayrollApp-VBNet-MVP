Imports Payroll.DBConnection.Presenters
Imports Payroll.DBConnection.Services
Imports Payroll.DBConnection.Views
Imports Payroll.Employee.Data
Imports Payroll.Employee.Presenters
Imports Payroll.Employee.Services
Imports Payroll.Employee.Views
Imports Payroll.Login.Data
Imports Payroll.Login.Presenters
Imports Payroll.Login.Services
Imports Payroll.Lookups.Data
Imports Payroll.Lookups.Presenters
Imports Payroll.Lookups.Services
Imports Payroll.StatutorySettings.Data
Imports Payroll.StatutorySettings.Presenters
Imports Payroll.StatutorySettings.Services
Imports Payroll.Users.Presenters
Imports Payroll.Users.Services
Imports Payroll.Users.Views

Public Class AppComposition

    Public Shared Function BuildEmployeeView() As ucEmployees

        ' 1. Repository
        Dim empRepo As New EmployeeRepository()

        ' 2. Service
        Dim empService As New EmployeeService(empRepo)

        ' 3. Gawin MUNA ang Views — walang Presenter pa
        Dim personalInfoView As New ucEmployeesPersonalInfo()
        Dim employmentView As New ucEmployeesEmployment()
        Dim earningsView As New ucEmployeesEarnings()
        Dim statutoryView As New ucEmployeesStatutory()

        ' 4. Gawin ang Presenters — i-inject ang View + Service
        Dim personalInfoPresenter As New EmployeesPersonalInfoPresenter(personalInfoView, empService)
        Dim employmentPresenter As New EmployeesEmploymentPresenter(employmentView, empService)
        Dim earningsPresenter As New EmployeesEarningsPresenter(earningsView, empService)
        Dim statutoryPresenter As New EmployeesStatutoryPresenter(statutoryView, empService)

        ' 5. I-assign ang Presenter sa bawat View
        personalInfoView.SetPresenter(personalInfoPresenter)
        employmentView.SetPresenter(employmentPresenter)
        earningsView.SetPresenter(earningsPresenter)
        statutoryView.SetPresenter(statutoryPresenter)

        ' 6. Gawin ang Main View
        Return New ucEmployees(personalInfoView, employmentView, earningsView, statutoryView)

    End Function

    Public Shared Function BuildLoginForm() As frmLogin

        ' 1. Repository
        Dim userRepo As New UserRepository()

        ' 2. Services
        Dim authService As New AuthenticationService(userRepo)
        Dim loginPreferencesService As New JsonLoginPreferencesService()

        ' 3. View (yung frmLogin mismo ang View dito, hindi UserControl)
        Dim loginView As New frmLogin()

        ' 4. Presenter - i-inject ang View + Services
        Dim presenter As New LoginPresenter(loginView, authService, loginPreferencesService)

        ' 5. I-assign ang Presenter sa View
        loginView.SetPresenter(presenter)

        Return loginView

    End Function

    Public Shared Function BuildDatabaseConnectionSettingsView() As ucDatabaseConnectionSettings

        ' 1. Service (walang Repository - local JSON file lang ang storage)
        Dim settingsService As New DatabaseConnectionSettingsService()

        ' 2. View
        Dim view As New ucDatabaseConnectionSettings()

        ' 3. Presenter - i-inject ang View + Service
        Dim presenter As New DatabaseConnectionSettingsPresenter(view, settingsService)

        ' 4. I-assign ang Presenter sa View
        view.SetPresenter(presenter)

        Return view

    End Function

    Public Shared Function BuildChangePasswordForm() As frmChangePassword

        ' 1. Repository
        Dim userRepo As New UserRepository()

        ' 2. Service
        Dim authService As New AuthenticationService(userRepo)

        ' 3. View
        Dim view As New frmChangePassword()

        ' 4. Presenter - i-inject ang View + Service + kasalukuyang naka-login na username
        Dim presenter As New ChangePasswordPresenter(view, authService, AppSession.CurrentUser)

        ' 5. I-assign ang Presenter sa View
        view.SetPresenter(presenter)

        Return view

    End Function

    Public Shared Function BuildUsersView() As ucUsers

        ' 1. Repository - reused mula sa Login module (parehong tblUsers naman)
        Dim userRepo As New UserRepository()

        ' 2. Services
        Dim userMgmtService As New UserManagementService(userRepo)
        Dim empRepo As New EmployeeRepository()
        Dim empService As New EmployeeService(empRepo)

        ' 3. View
        Dim view As New ucUsers()

        ' 4. Presenter - i-inject ang View + Services
        Dim presenter As New UsersPresenter(view, userMgmtService, empService)

        ' 5. I-assign ang Presenter sa View
        view.SetPresenter(presenter)

        Return view

    End Function

    Public Shared Function BuildSettingsLookupsView() As ucSettingsLookups

        ' 1. Repository + Service - iisa lang, SHARED sa lahat ng 8 tabs.
        ' Stateless naman sila (tableName mismo ang ipinapasa sa bawat
        ' call), kaya walang dahilan gumawa ng 8 hiwalay na instances.
        Dim lookupRepo As New LookupRepository()
        Dim lookupService As New LookupService(lookupRepo)

        ' 2. Kasalukuyang naka-login na user - para sa CreatedBy/UpdatedBy
        Dim currentUser = AppSession.CurrentUser

        ' 3. Gawin MUNA ang 8 Views - iisang class lang (ucLookupMaintenance),
        ' walang Presenter pa (parehong hakbang gaya ng BuildEmployeeView)
        Dim branchView As New ucLookupMaintenance()
        Dim departmentView As New ucLookupMaintenance()
        Dim positionView As New ucLookupMaintenance()
        Dim categoryView As New ucLookupMaintenance()
        Dim jobClassView As New ucLookupMaintenance()
        Dim holidayGroupView As New ucLookupMaintenance()
        Dim scheduleGroupView As New ucLookupMaintenance()
        Dim bankView As New ucLookupMaintenance()

        ' 4. Gawin ang 8 Presenters - bawat isa naka-configure sa ibang
        ' tableName. Ang mismong pangalan ng table ay mula sa
        ' LookupTableRegistry.MaintainedTables (single source of truth) -
        ' kung magbabago man ang whitelist doon, dito lang ito babaguhin.
        Dim branchPresenter As New LookupPresenter(branchView, lookupService, "tblBranch", currentUser)
        Dim departmentPresenter As New LookupPresenter(departmentView, lookupService, "tblDepartment", currentUser)
        Dim positionPresenter As New LookupPresenter(positionView, lookupService, "tblPosition", currentUser)
        Dim categoryPresenter As New LookupPresenter(categoryView, lookupService, "tblCategoryCode", currentUser)
        Dim jobClassPresenter As New LookupPresenter(jobClassView, lookupService, "tblJobClass", currentUser)
        Dim holidayGroupPresenter As New LookupPresenter(holidayGroupView, lookupService, "tblHolidayGroup", currentUser)
        Dim scheduleGroupPresenter As New LookupPresenter(scheduleGroupView, lookupService, "tblScheduleGroup", currentUser)
        Dim bankPresenter As New LookupPresenter(bankView, lookupService, "tblBank", currentUser)

        ' 5. I-assign ang Presenter sa bawat View (kasama ang display
        ' title na makikita sa tab/breadcrumb)
        branchView.SetPresenter(branchPresenter, "Branch")
        departmentView.SetPresenter(departmentPresenter, "Department")
        positionView.SetPresenter(positionPresenter, "Position")
        categoryView.SetPresenter(categoryPresenter, "Category")
        jobClassView.SetPresenter(jobClassPresenter, "Job Class")
        holidayGroupView.SetPresenter(holidayGroupPresenter, "Holiday Group")
        scheduleGroupView.SetPresenter(scheduleGroupPresenter, "Schedule Group")
        bankView.SetPresenter(bankPresenter, "Bank")

        ' 6. Gawin ang Main View
        Return New ucSettingsLookups(branchView, departmentView, positionView,
            categoryView, jobClassView, holidayGroupView, scheduleGroupView, bankView)

    End Function

    Public Shared Function BuildStatutorySettingsView() As ucStatutorySettingsShell

        ' 1. Repository + Service - iisa lang, SHARED sa lahat ng 3 tabs,
        ' gaya ng ginawa sa Master Data (stateless, tableName ang variable).
        Dim statutoryRepo As New StatutorySettingsRepository()
        Dim statutoryService As New StatutorySettingsService(statutoryRepo)

        ' 2. Kasalukuyang naka-login na user - para sa CreatedBy/UpdatedBy
        Dim currentUser = AppSession.CurrentUser

        ' 3. Gawin MUNA ang 3 Views - iisang class lang (ucStatutorySettings)
        Dim sssView As New ucStatutorySettings()
        Dim philHealthView As New ucStatutorySettings()
        Dim pagIbigView As New ucStatutorySettings()

        ' 4. Gawin ang 3 Presenters - bawat isa naka-configure sa ibang
        ' tableName mula sa StatutorySettingsTableRegistry.
        Dim sssPresenter As New StatutorySettingsPresenter(sssView, statutoryService, "tblStatutorySSS", currentUser)
        Dim philHealthPresenter As New StatutorySettingsPresenter(philHealthView, statutoryService, "tblStatutoryPhilHealth", currentUser)
        Dim pagIbigPresenter As New StatutorySettingsPresenter(pagIbigView, statutoryService, "tblStatutoryPagIbig", currentUser)

        ' 5. I-assign ang Presenter sa bawat View (kasama ang display title)
        sssView.SetPresenter(sssPresenter, "SSS")
        philHealthView.SetPresenter(philHealthPresenter, "PhilHealth")
        pagIbigView.SetPresenter(pagIbigPresenter, "Pag-IBIG")

        ' 6. Gawin ang Main View
        Return New ucStatutorySettingsShell(sssView, philHealthView, pagIbigView)

    End Function

End Class