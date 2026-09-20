' ============================================================
' GlobalShared/Constants/ModuleCodes.vb
' ============================================================
' ANG SUSI NG BUONG MODULE ACCESS SYSTEM.
'
' Ang ModuleCode ay EXACTLY kapareho ng Tag ng AccordionElement
' sa frmMain.Designer.vb (hal. aceEmployees.Tag = "main_Employees").
'
' BAKIT GANITO?
'   May tatlong bagay na kailangang mag-usap:
'     1. tblModules.ModuleCode      (database)
'     2. AccordionElement.Tag       (sidebar navigation)
'     3. ucBase.ModuleCode          (yung view mismo)
'
'   Kung iisang string lang ang gamit ng tatlo, hindi na kailangan
'   ng mapping table, switch statement, o kahit anong translation
'   layer. Isang string comparison lang - "main_Employees" = "main_Employees".
'
'   Kaya dito, ang tblModules.ModuleCode ay dapat literal na
'   "main_Employees", HINDI "EMPLOYEE" o "EMP01". Tingnan mo yung
'   SQL seed script (01_tblModules_Seed.sql).
'
' BAKIT CONSTANTS AT HINDI PURO MAGIC STRINGS?
'   Para pag nagkamali ka ng spelling, compile error agad imbes na
'   tahimik na "No Access" sa runtime na mahirap i-debug.
' ============================================================
Namespace GlobalShared.Constants

    Public Module ModuleCodes

        ' --- MAIN MENU ---
        Public Const Main_Dashboard As String = "main_Dashboard"
        Public Const Main_Employees As String = "main_Employees"
        Public Const Main_Payroll As String = "main_Payroll"
        Public Const Main_Loan As String = "main_Loan"
        Public Const Main_Leave As String = "main_Leave"

        ' --- ADMINISTRATION ---
        Public Const Admin_UsersAccount As String = "admin_UsersAccount"
        Public Const Admin_Modules As String = "admin_Modules"   ' <-- IDAGDAG ITO

        ' --- SETTINGS > PAYROLL SETUP ---
        Public Const Settings_General As String = "settings_General"
        Public Const Settings_Company As String = "settings_Company"
        Public Const Settings_MasterData As String = "settings_MasterData"
        Public Const Settings_CutOff As String = "settings_CutOff"
        Public Const Settings_Leave As String = "settings_Leave"
        Public Const Settings_Holidays As String = "settings_Holidays"
        Public Const Settings_TaxTable As String = "settings_TaxTable"
        Public Const Settings_Statutory As String = "settings_Statutory"
        Public Const Settings_Payroll As String = "settings_Payroll"

        ' --- SETTINGS > APPLICATION SETTINGS ---
        Public Const Settings_DatabaseSettings As String = "settings_DatabaseSettings"
        Public Const Settings_Email As String = "settings_Email"
        Public Const Settings_Themes As String = "settings_Themes"

        ' --- REPORTS ---
        Public Const Reports_HR As String = "reports_HR"
        Public Const Reports_Payslip As String = "reports_Payslip"
        Public Const Reports_Payroll As String = "reports_Payroll"
        Public Const Reports_Statutory As String = "reports_Statutory"
        Public Const Reports_Loans As String = "reports_Loans"
        Public Const Reports_Alphalist As String = "reports_Alphalist"

        ' ========================================================
        ' ALWAYS-ALLOWED
        ' ========================================================
        ' Ang Dashboard ay default landing page (_nav.SetDefault sa
        ' frmMain_Load). Kung ito ay maha-hide/ma-block, mapipilitan
        ' tayong mag-fallback logic - mas magulo. Mas simple na
        ' i-declare na lang na LAGING bukas ito sa lahat ng user.
        '
        ' Ganoon din ang "logout" - hindi naman talaga module yun,
        ' isa lang siyang action na naka-tag sa accordion.
        ' ========================================================
        Public ReadOnly AlwaysAllowed As String() = {
            Main_Dashboard,
            "logout"
        }

        Public Function IsAlwaysAllowed(moduleCode As String) As Boolean
            If String.IsNullOrWhiteSpace(moduleCode) Then Return False

            Return AlwaysAllowed.Any(
                Function(c) String.Equals(c, moduleCode, StringComparison.OrdinalIgnoreCase))
        End Function

    End Module

End Namespace