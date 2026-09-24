Imports Payroll.CompanyProfile.Data
Imports Payroll.CompanyProfile.Presenters
Imports Payroll.CompanyProfile.Services
Imports Payroll.CompanyProfile.Views
Imports Payroll.DBConnection.Presenters
Imports Payroll.DBConnection.Services
Imports Payroll.DBConnection.Views
Imports Payroll.EmailSettings.Data
Imports Payroll.EmailSettings.Presenters
Imports Payroll.EmailSettings.Services
Imports Payroll.Employee.Data
Imports Payroll.Employee.Presenters
Imports Payroll.Employee.Services
Imports Payroll.Employee.Views
Imports Payroll.GeneralSettings.Data
Imports Payroll.GeneralSettings.Presenters
Imports Payroll.GeneralSettings.Services
Imports Payroll.IncomeTaxTable.Data
Imports Payroll.IncomeTaxTable.Presenters
Imports Payroll.IncomeTaxTable.Services
Imports Payroll.LeaveSettings.Data
Imports Payroll.LeaveSettings.Presenters
Imports Payroll.LeaveSettings.Services
Imports Payroll.Loans.Data
Imports Payroll.Loans.Presenters
Imports Payroll.Loans.Services
Imports Payroll.Login.Data
Imports Payroll.Login.Presenters
Imports Payroll.Login.Services
Imports Payroll.Lookups.Data
Imports Payroll.Lookups.Presenters
Imports Payroll.Lookups.Services
Imports Payroll.ModuleManagement.Data
Imports Payroll.ModuleManagement.Presenters
Imports Payroll.ModuleManagement.Services
Imports Payroll.PayrollSettings.Data
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Services
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

    Public Shared Function BuildLeaveSettingsView() As ucLeaveSettingsShell

        ' 1. Kasalukuyang naka-login na user - para sa CreatedBy/UpdatedBy
        Dim currentUser = AppSession.CurrentUser

        ' --- GROUP tab - reused na ucLookupMaintenance (tblLeaveGroup),
        ' parehong LookupRepository/LookupService gaya ng Master Data -
        ' walang bagong code para dito, iba lang ang nag-i-instantiate.
        Dim lookupRepo As New LookupRepository()
        Dim lookupService As New LookupService(lookupRepo)

        Dim groupView As New ucLookupMaintenance()
        Dim groupPresenter As New LookupPresenter(groupView, lookupService, "tblLeaveGroup", currentUser)
        groupView.SetPresenter(groupPresenter, "Group")

        ' --- TYPE tab - bagong dedikadong Repository/Service/Presenter
        ' (may extra Category field na wala sa generic Lookup shape).
        Dim leaveTypeRepo As New LeaveTypeRepository()
        Dim leaveTypeService As New LeaveTypeService(leaveTypeRepo)

        Dim typeView As New ucLeaveType()
        Dim typePresenter As New LeaveTypePresenter(typeView, leaveTypeService, currentUser)
        typeView.SetPresenter(typePresenter)

        ' --- RULE tab - kailangan ng LeaveRuleService (header+brackets)
        ' PLUS ang parehong lookupService (Leave Group combo) at
        ' leaveTypeService (Leave Type combo) mula sa itaas - hiram
        ' lang, hindi na kailangan pang gumawa ng bago.
        Dim leaveRuleRepo As New LeaveRuleRepository()
        Dim leaveRuleService As New LeaveRuleService(leaveRuleRepo)

        Dim ruleView As New ucLeaveRule()
        Dim rulePresenter As New LeaveRulePresenter(ruleView, leaveRuleService, lookupService, leaveTypeService, currentUser)
        ruleView.SetPresenter(rulePresenter)

        ' 2. Gawin ang Main View
        Return New ucLeaveSettingsShell(groupView, typeView, ruleView)

    End Function

    Public Shared Function BuildPayrollSettingsView() As ucPayrollSettings

        ' 1. Repositories + Services - 4 shapes, iisa lang bawat isa (SHARED
        ' sa mga magkaparehong-shape na tabs, gaya ng ginawa sa Master Data/
        ' Statutory - stateless naman sila, tableName mismo ang variable
        ' sa FlaggedEntry/RateEntry).
        Dim compensationRepo As New CompensationRepository()
        Dim compensationService As New CompensationService(compensationRepo)

        Dim flaggedEntryRepo As New PayrollFlaggedEntryRepository()
        Dim flaggedEntryService As New PayrollFlaggedEntryService(flaggedEntryRepo)

        Dim rateEntryRepo As New PayrollRateEntryRepository()
        Dim rateEntryService As New PayrollRateEntryService(rateEntryRepo)

        Dim loanRepo As New LoanRepository()
        Dim loanService As New LoanService(loanRepo)

        ' 2. Kasalukuyang naka-login na user - para sa CreatedBy/UpdatedBy
        Dim currentUser = AppSession.CurrentUser

        ' 3. Gawin MUNA ang 6 Views - 4 classes lang (Compensation/Loan may
        ' 1 instance bawat isa; Deduction+Bonus parehong ucPayrollFlaggedEntry;
        ' Overtime+Holiday parehong ucPayrollRateEntry) - walang Presenter pa.
        Dim compensationView As New ucCompensation()
        Dim deductionView As New ucPayrollFlaggedEntry()
        Dim overtimeView As New ucPayrollRateEntry()
        Dim holidayView As New ucPayrollRateEntry()
        Dim bonusView As New ucPayrollFlaggedEntry()
        Dim loanView As New ucLoan()

        ' 4. Gawin ang 6 Presenters - Deduction/Bonus naka-configure sa
        ' tblDeduction/tblBonus; Overtime/Holiday naka-configure sa
        ' tblOvertime/tblHoliday.
        Dim compensationPresenter As New CompensationPresenter(compensationView, compensationService, currentUser)
        Dim deductionPresenter As New PayrollFlaggedEntryPresenter(deductionView, flaggedEntryService, "tblDeduction", currentUser)
        Dim overtimePresenter As New PayrollRateEntryPresenter(overtimeView, rateEntryService, "tblOvertime", currentUser)
        Dim holidayPresenter As New PayrollRateEntryPresenter(holidayView, rateEntryService, "tblHoliday", currentUser)
        Dim bonusPresenter As New PayrollFlaggedEntryPresenter(bonusView, flaggedEntryService, "tblBonus", currentUser)
        Dim loanPresenter As New LoanPresenter(loanView, loanService, currentUser)

        ' 5. I-assign ang Presenter sa bawat View (Deduction/Bonus/Overtime/
        ' Holiday kasama ang display title na makikita sa tab/breadcrumb -
        ' Compensation/Loan iisa lang ang instance kaya naka-fix na ang title
        ' sa View mismo).
        compensationView.SetPresenter(compensationPresenter)
        deductionView.SetPresenter(deductionPresenter, "Deduction")
        overtimeView.SetPresenter(overtimePresenter, "Overtime")
        holidayView.SetPresenter(holidayPresenter, "Holiday")
        bonusView.SetPresenter(bonusPresenter, "Bonus")
        loanView.SetPresenter(loanPresenter)

        ' 6. Gawin ang Main View
        Return New ucPayrollSettings(compensationView, deductionView, overtimeView, holidayView, bonusView, loanView)

    End Function

    Public Shared Function BuildIncomeTaxTableView() As ucIncomeTaxTableShell

        Dim taxRepo As New IncomeTaxTableRepository()
        Dim taxService As New IncomeTaxTableService(taxRepo)

        Dim currentUser = AppSession.CurrentUser

        Dim yearlyView As New ucIncomeTaxTable()
        Dim monthlyView As New ucIncomeTaxTable()
        Dim semiMonthlyView As New ucIncomeTaxTable()
        Dim weeklyView As New ucIncomeTaxTable()
        Dim dailyView As New ucIncomeTaxTable()

        Dim yearlyPresenter As New IncomeTaxTablePresenter(yearlyView, taxService, "tblIncomeTaxYearly", currentUser)
        Dim monthlyPresenter As New IncomeTaxTablePresenter(monthlyView, taxService, "tblIncomeTaxMonthly", currentUser)
        Dim semiMonthlyPresenter As New IncomeTaxTablePresenter(semiMonthlyView, taxService, "tblIncomeTaxSemiMonthly", currentUser)
        Dim weeklyPresenter As New IncomeTaxTablePresenter(weeklyView, taxService, "tblIncomeTaxWeekly", currentUser)
        Dim dailyPresenter As New IncomeTaxTablePresenter(dailyView, taxService, "tblIncomeTaxDaily", currentUser)

        yearlyView.SetPresenter(yearlyPresenter, "Yearly")
        monthlyView.SetPresenter(monthlyPresenter, "Monthly")
        semiMonthlyView.SetPresenter(semiMonthlyPresenter, "Semi-Monthly")
        weeklyView.SetPresenter(weeklyPresenter, "Weekly")
        dailyView.SetPresenter(dailyPresenter, "Daily")

        Return New ucIncomeTaxTableShell(yearlyView, monthlyView, semiMonthlyView, weeklyView, dailyView)

    End Function

    Public Shared Function BuildCompanyProfileView() As ucCompanyProfileShell

        Dim companyRepo As New CompanyRepository()
        Dim companyService As New CompanyService(companyRepo)

        Dim agencyRepo As New CompanyAgencyRegistrationRepository()
        Dim agencyService As New CompanyAgencyRegistrationService(agencyRepo)

        Dim bankRepo As New CompanyBankRepository()
        Dim bankService As New CompanyBankService(bankRepo)

        ' Kailangan na ngayon ng Employee repository para sa Contact Person /
        ' Person-in-charge employee-picker (LookUpEdit) sa 3 magkaibang views.
        Dim employeeRepo As New EmployeeRepository()

        Dim currentUser = AppSession.CurrentUser

        Dim companyView As New ucCompany()
        Dim sssView As New ucCompanyAgencyRegistration()
        Dim philHealthView As New ucCompanyAgencyRegistration()
        Dim pagIbigView As New ucCompanyAgencyRegistration()
        Dim birView As New ucCompanyAgencyRegistration()
        Dim bankView As New ucCompanyBank()

        Dim companyPresenter As New CompanyPresenter(companyView, companyService, employeeRepo, currentUser)
        Dim sssPresenter As New CompanyAgencyRegistrationPresenter(sssView, agencyService, employeeRepo, "SSS", currentUser)
        Dim philHealthPresenter As New CompanyAgencyRegistrationPresenter(philHealthView, agencyService, employeeRepo, "PHILHEALTH", currentUser)
        Dim pagIbigPresenter As New CompanyAgencyRegistrationPresenter(pagIbigView, agencyService, employeeRepo, "PAGIBIG", currentUser)
        Dim birPresenter As New CompanyAgencyRegistrationPresenter(birView, agencyService, employeeRepo, "BIR", currentUser)
        Dim bankPresenter As New CompanyBankPresenter(bankView, bankService, employeeRepo, currentUser)

        companyView.SetPresenter(companyPresenter)
        sssView.SetPresenter(sssPresenter)
        philHealthView.SetPresenter(philHealthPresenter)
        pagIbigView.SetPresenter(pagIbigPresenter)
        birView.SetPresenter(birPresenter)
        bankView.SetPresenter(bankPresenter)

        Return New ucCompanyProfileShell(companyView, sssView, philHealthView, pagIbigView, birView, bankView)

    End Function

    Public Shared Function BuildGeneralSettingsView() As ucGeneralSettings

        Dim settingsRepo As New GeneralSettingsRepository()
        Dim settingsService As New GeneralSettingsService(settingsRepo)

        ' Read-only lang ang gamit natin sa dalawang ito — para sa code
        ' dropdowns (Basic Salary +/-, Absent, Late In, Early Out).
        ' Direktang repository, walang bagong Service layer, kapareho ng
        ' ginawa natin sa EmployeeRepository sa BuildCompanyProfileView.
        Dim compensationRepo As New CompensationRepository()
        Dim flaggedEntryRepo As New PayrollFlaggedEntryRepository()

        Dim currentUser = AppSession.CurrentUser

        Dim view As New ucGeneralSettings()
        Dim presenter As New GeneralSettingsPresenter(
            view, settingsService, compensationRepo, flaggedEntryRepo, currentUser)

        view.SetPresenter(presenter)

        Return view

    End Function


    Public Shared Function BuildEmailSettingsView() As ucEmailSettings

        Dim repo As New EmailSettingsRepository()
        Dim service As New EmailSettingsService(repo)

        Dim currentUser = AppSession.CurrentUser

        Dim view As New ucEmailSettings()
        Dim presenter As New EmailSettingsPresenter(view, service, currentUser)

        view.SetPresenter(presenter)

        Return view

    End Function

    Public Shared Function BuildLoansView() As ucLoans

        ' 1. Repository + Service ng mismong module
        Dim loanRepo As New EmployeeLoanRepository()
        Dim loanService As New EmployeeLoanService(loanRepo)

        ' 2. REUSED na dependencies - hindi tayo gumagawa ng bago:
        '
        '    - EmployeeService  : para sa employee masterlist sa gilid,
        '                         kapareho ng ginagamit ng ucEmployees.
        '
        '    - LoanRepository   : para sa Loan Code dropdown. Galing ito
        '      (PayrollSettings)  sa Payroll Settings > Loan tab. Direktang
        '                         repository, walang bagong service layer -
        '                         read-only lookup lang naman ang kailangan,
        '                         kapareho ng ginawa natin sa
        '                         BuildGeneralSettingsView at
        '                         BuildCompanyProfileView.
        Dim empRepo As New EmployeeRepository()
        Dim empService As New EmployeeService(empRepo)

        Dim loanCodeRepo As New PayrollSettings.Data.LoanRepository()

        ' 3. Kasalukuyang naka-login na user - para sa CreatedBy/UpdatedBy
        Dim currentUser = AppSession.CurrentUser

        ' 4. View
        Dim view As New ucLoans()

        ' 5. Presenter - i-inject ang View + lahat ng dependencies
        Dim presenter As New EmployeeLoanPresenter(
            view, loanService, empService, loanCodeRepo, currentUser)

        ' 6. I-assign ang Presenter sa View
        view.SetPresenter(presenter)

        Return view

    End Function

    Public Shared Function BuildModuleManagementView() As ucModuleManagement

        Dim currentUser = AppSession.CurrentUser

        ' --- TAB 1: Module Catalog - bagong stack ---
        Dim catalogRepo As New ModuleCatalogRepository()
        Dim catalogService As New ModuleCatalogService(catalogRepo)
        Dim catalogView As New ucModuleCatalog()
        Dim catalogPresenter As New ModuleCatalogPresenter(catalogView, catalogService, currentUser)
        catalogView.SetPresenter(catalogPresenter)

        ' --- TAB 2: Access Editor - MULING GINAMIT ang IUserManagementService
        ' na ginawa mo na para sa Users Account module. Iisa lang ang
        ' totoong pinagmumulan ng access data - dito man o doon, parehong
        ' UserRepository ang tinatawag sa likod. Kapareho ito ng syntax na
        ' ginamit mo na sa BuildUsersView (walang extra qualifier - naka-
        ' resolve na ito sa umiiral na Imports sa taas ng file). ---
        Dim userRepo As New UserRepository()
        Dim userMgmtService As New UserManagementService(userRepo)

        Dim accessView As New ucModuleAccessEditor()
        Dim accessPresenter As New ModuleAccessEditorPresenter(accessView, userMgmtService, currentUser)
        accessView.SetPresenter(accessPresenter)

        ' --- SHELL ---
        Dim shellView As New ucModuleManagement(catalogView, accessView)

        Return shellView

    End Function

End Class