Imports DevExpress.Utils.Animation
Imports DevExpress.XtraBars.FluentDesignSystem
Imports DevExpress.XtraBars.Navigation
Imports DevExpress.XtraEditors
Imports Payroll.DBConnection.Services
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Database
Imports Payroll.GlobalShared.Security

Public Class frmMain
    ' Dictionary para i-cache ang mga modules (para hindi na i-New ulit pag binalikan)
    Private ReadOnly _modules As New Dictionary(Of String, GlobalShared.Base.ucBase)
    Private _nav As NavigationService
    Public _logout As Boolean = False

    Public Sub New()
        InitializeComponent()

        Me.LookAndFeel.UseDefaultLookAndFeel = True
        Me.BackColor = Color.Empty

    End Sub


    ''' <summary>
    ''' Generic method to load and show UserControls in the container
    ''' </summary>
    Private Sub ShowModule(Of T As {GlobalShared.Base.ucBase, New})(moduleName As String)
        Dim targetModule As GlobalShared.Base.ucBase = Nothing

        If Not _modules.TryGetValue(moduleName, targetModule) Then
            targetModule = New T()

            Dim ctrl = DirectCast(targetModule, Control)
            ctrl.Dock = DockStyle.Fill

            fluentMainContainer.Controls.Add(ctrl)
            _modules.Add(moduleName, targetModule)
        End If

        ' 🔥 ANIMATION
        tmMain.StartTransition(fluentMainContainer)

        For Each ctrl As Control In fluentMainContainer.Controls
            ctrl.Visible = False
        Next

        Dim activeCtrl = DirectCast(targetModule, Control)
        activeCtrl.Visible = True
        activeCtrl.BringToFront()

        tmMain.EndTransition()

        Me.Text = $"PPI Payroll - {moduleName}"
    End Sub



    ' ========================================================
    ' NAVIGATION VISIBILITY
    ' ========================================================
    ' Nililibot ang BUONG accordion tree at itinatago ang mga
    ' elementong walang View access si user.
    '
    ' BAKIT RECURSIVE?
    '   Ang AccordionControl ay puno (tree), hindi listahan.
    '   May Elements sa ilalim ng Elements (Settings > Payroll
    '   Setup > General). Kung For Each lang sa top level, ang
    '   mga anak ay hindi mahahawakan.
    '
    ' BAKIT HIDE AT HINDI DISABLE?
    '   Ang naka-disable na menu ay nagsasabi pa rin kay user na
    '   "may ganito pala" - pinapakita mo ang mapa ng sistema sa
    '   taong hindi dapat makakita. Ang naka-hide ay parang wala
    '   talaga. (Exception: ang Administration header ay naka-
    '   .Enabled = AppSession.IsAdmin na dati pa - hinayaan ko
    '   na lang dahil tama naman siya.)
    '
    ' BAKIT KAILANGAN ITONG BOTTOM-UP?
    '   Kapag lahat ng anak ay naka-hide, dapat matago na rin ang
    '   magulang - kung hindi, may nakasabit na "Payroll Setup"
    '   na walang laman kapag pinindot.
    ' ========================================================
    Private Sub ApplyModuleAccessToNavigation()

        ' Ang Admin ay laging buo ang makikita - walang kailangang gawin.
        If AppSession.IsAdmin Then Return

        For Each element As AccordionControlElement In AccordionControl1.Elements
            ApplyAccessToElement(element)
        Next

    End Sub

    ' Ibinabalik: True kung dapat MANATILING VISIBLE ang element na ito.
    Private Function ApplyAccessToElement(element As AccordionControlElement) As Boolean

        If element Is Nothing Then Return False

        ' ---- MAY ANAK? (group / header) ----
        If element.Elements IsNot Nothing AndAlso element.Elements.Count > 0 Then

            Dim anyChildVisible As Boolean = False

            For Each child As AccordionControlElement In element.Elements
                ' Sinasadyang hindi ako gumamit ng OrElse dito.
                ' Kailangang MATAWAG ang ApplyAccessToElement sa
                ' LAHAT ng anak - kung OrElse, titigil siya sa unang
                ' True at hindi na matatago ang mga natitira.
                Dim childVisible = ApplyAccessToElement(child)
                anyChildVisible = anyChildVisible Or childVisible
            Next

            element.Visible = anyChildVisible
            Return anyChildVisible

        End If

        ' ---- WALANG ANAK (leaf / aktwal na module) ----
        Dim tag = element.Tag?.ToString()

        ' Walang Tag = hindi module, hindi natin hinuhusgahan.
        ' Hayaan mong nakikita para hindi ka mag-alala kung bakit
        ' may nawawalang item na hindi mo naman pala sinecure.
        If String.IsNullOrWhiteSpace(tag) Then
            element.Visible = True
            Return True
        End If

        Dim allowed = PermissionService.CanView(tag)
        element.Visible = allowed

        Return allowed

    End Function




    ' Event Handler para sa AccordionControl Click
    Private Async Sub accordionControl1_ElementClick(sender As Object, e As ElementClickEventArgs) Handles AccordionControl1.ElementClick
        ' Siguraduhin na ang "Tag" property ng Accordion Element sa Designer ay may value
        If e.Element.Tag Is Nothing Then Return

        ' ============ MODULE ACCESS GUARD ============

        ' Group header (may anak) - hindi ito module, expand/collapse
        ' lang ang ginagawa nito. Huwag guardahan, tumuloy na lang.
        If e.Element.Elements IsNot Nothing AndAlso e.Element.Elements.Count > 0 Then
            Return
        End If

        Dim moduleTag = e.Element.Tag.ToString()

        If Not PermissionService.CanView(moduleTag) Then
            XtraMessageBox.Show(
                "You do not have access to this module." & Environment.NewLine &
                "Please contact your system administrator.",
                "Access Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If
        ' =============================================

        Await Task.Delay(50)
        Select Case e.Element.Tag.ToString()
            Case "main_Dashboard"
                _nav.NavigateTo(Of ucDashboard)()

            Case "main_Employees"
                _nav.NavigateTo(Of ucEmployees)(Function() AppComposition.BuildEmployeeView())

            Case "main_Payroll"
                _nav.NavigateTo(Of ucPayroll)()

            Case "main_Loan"
                _nav.NavigateTo(Of ucLoans)(Function() AppComposition.BuildLoansView())

            Case "admin_UsersAccount"
                _nav.NavigateTo(Of ucUsers)(Function() AppComposition.BuildUsersView())

            Case "admin_Modules"
                _nav.NavigateTo(Of ucModuleManagement)(Function() AppComposition.BuildModuleManagementView())

            Case "settings_MasterData"
                _nav.NavigateTo(Of ucSettingsLookups)(Function() AppComposition.BuildSettingsLookupsView())

            Case "settings_Company"
                _nav.NavigateTo(Of ucCompanyProfileShell)(Function() AppComposition.BuildCompanyProfileView())

            Case "settings_Statutory"
                _nav.NavigateTo(Of ucStatutorySettingsShell)(Function() AppComposition.BuildStatutorySettingsView())

            Case "settings_Payroll"
                _nav.NavigateTo(Of ucPayrollSettings)(Function() AppComposition.BuildPayrollSettingsView())

            Case "settings_TaxTable"
                _nav.NavigateTo(Of ucIncomeTaxTableShell)(Function() AppComposition.BuildIncomeTaxTableView())

            Case "settings_DatabaseSettings"
                OpenDatabaseSettings()

            Case "settings_General"
                _nav.NavigateTo(Of ucGeneralSettings)(Function() AppComposition.BuildGeneralSettingsView())

            Case "settings_Email"
                _nav.NavigateTo(Of ucEmailSettings)(Function() AppComposition.BuildEmailSettingsView())

            Case "logout"
                PerformLogout()

        End Select

        btnBack.Enabled = _nav.IsNavigationChanged
        btnForward.Enabled = False
    End Sub

    ' =============================================
    ' DATABASE SETTINGS - Admin: diretso, walang PIN pa (naka-gate na
    ' sa Admin login mismo). Ordinary user: kailangan pa rin ng PIN -
    ' parehong PIN system (SettingsPinService) na ginagamit sa secret
    ' gesture sa login screen, kaya iisa lang ang PIN na pinapanatili.
    ' =============================================
    Private Sub OpenDatabaseSettings()

        If AppSession.IsAdmin Then
            ShowDatabaseSettingsDialog()
            Return
        End If

        Dim pinService As New SettingsPinService()

        If Not pinService.HasPin() Then
            XtraMessageBox.Show(
                "No Settings PIN has been set yet. Please use " &
                "--set-settings-pin from the command line first.",
                "Database Settings",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning)
            Return
        End If

        Using pinPrompt As New frmSettingsPinPrompt()
            If pinPrompt.ShowDialog() <> DialogResult.OK Then Return

            If pinService.VerifyPin(pinPrompt.EnteredPin) Then
                ShowDatabaseSettingsDialog()
            Else
                XtraMessageBox.Show(
                    "Incorrect PIN.",
                    "Database Settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
            End If
        End Using

    End Sub

    Private Sub ShowDatabaseSettingsDialog()
        Dim settingsView = AppComposition.BuildDatabaseConnectionSettingsView()
        Using dlg As New frmDatabaseConnectionSettingsDialog(settingsView)
            dlg.ShowDialog()
        End Using
    End Sub


    ' =============================================
    ' PROFILE DROPDOWN
    ' =============================================

    Private Sub lblCurrentUserDisplayedName_Click(sender As Object, e As EventArgs) _
        Handles lblCurrentUserDisplayedName.Click

        ShowProfileMenu()
    End Sub

    Private Sub PictureEdit1_Click(sender As Object, e As EventArgs) Handles PictureEdit1.Click
        ShowProfileMenu()
    End Sub

    Private Sub ShowProfileMenu()
        Dim menu As New frmProfileMenu()

        Dim screenPoint = pnlHeaderMain.PointToScreen(
            New Point(pnlHeaderMain.Width, pnlHeaderMain.Height))

        menu.Location = New Point(screenPoint.X - menu.Width - 10, screenPoint.Y)

        AddHandler menu.ChangePasswordRequested, AddressOf OnChangePasswordRequested
        AddHandler menu.LogoutRequested, AddressOf OnLogoutRequested

        menu.Show(Me)
    End Sub

    Private Sub OnChangePasswordRequested()
        Dim changePasswordForm = AppComposition.BuildChangePasswordForm()
        changePasswordForm.ShowDialog(Me)
    End Sub

    Private Sub OnLogoutRequested()
        PerformLogout()
    End Sub

    ' Iisa na lang ang lugar ng logout logic - tinatawag ito ng parehong
    ' Accordion sidebar AT ng bagong Profile dropdown, walang duplicate na code.
    Private Sub PerformLogout()
        Dim confirm = XtraMessageBox.Show(
                                        "Are you sure you want to logout?",
                                        "Logout",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question)

        If confirm <> DialogResult.Yes Then Return

        ' ✅ I-clear ang AppSession
        AppSession.CurrentUser = String.Empty
        AppSession.CurrentUserRecordID = 0
        AppSession.IsAdmin = False
        AppSession.EmployeeNo = String.Empty

        ' ✅ I-clear din ang permission cache - kung hindi, madadala
        ' ng susunod na mag-lolog-in ang permission ng nauna, dahil
        ' buhay pa rin ang process at Shared ang cache.
        PermissionService.Clear()

        _logout = True

        ' ✅ I-close ang current frmMain
        Me.Close()
    End Sub


    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load

        ApplyVersionLabel()
        ApplyCurrentUserLabel()
        ApplyHostLabel()

        ' I-set ang cursor para malinaw kay user na pwede itong i-click
        lblCurrentUserDisplayedName.Cursor = Cursors.Hand
        PictureEdit1.Cursor = Cursors.Hand

        _logout = False

        _nav = New NavigationService(
                                    fluentMainContainer,
                                    tmMain,
                                    Sub(text) lblBreadcrumb.Text = text,
                                    Sub(title) lblPageTitle.Text = title.ToUpper
                                    )
        ' 👉 default page
        _nav.SetDefault(Of ucDashboard)()
        AccordionControl1.OptionsMinimizing.State = DevExpress.XtraBars.Navigation.AccordionControlState.Normal

        ' =============================================
        ' ADMINISTRATION ACCESS
        ' =============================================
        aceHeaderAdministration.Visible = AppSession.IsAdmin
        aceLogout.Visible = Not AppSession.IsAdmin

        ' =============================================
        ' MODULE ACCESS - itago ang mga bawal
        ' =============================================
        ApplyModuleAccessToNavigation()

    End Sub

    ' Kinukuha sa assembly info ng project ang bersyon - hindi na kailangang
    ' i-update nang manual kada bagong release/build.
    Private Sub ApplyVersionLabel()
        Dim version = My.Application.Info.Version
        lblVersion.Text = $"v{version.Major}.{version.Minor}.{version.Build}"
    End Sub

    ' Ipinapakita ang pangalan ng currently logged-in user + role level,
    ' galing sa AppSession na na-populate ng LoginPresenter pagkatapos
    ' ng successful na login.
    Private Sub ApplyCurrentUserLabel()
        Dim roleLabel = If(AppSession.IsAdmin, "Admin", "Employee")
        lblCurrentUserDisplayedName.Text = $"{AppSession.DisplayName} ({roleLabel})"
    End Sub

    ' Kinukuha diretso mula sa connection string ang server name kung saan
    ' naka-connect ang system - hindi na kailangang mag-hardcode.
    Private Sub ApplyHostLabel()
        Using conn = DbConnectionFactory.CreateConnection()
            lblHost.Text = conn.DataSource
        End Using
    End Sub

    Private Async Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Await Task.Delay(50)
        _nav.GoBack()
        btnForward.Enabled = True
        btnBack.Enabled = _nav.CanGoBack()
    End Sub

    Private Async Sub btnForward_Click(sender As Object, e As EventArgs) Handles btnForward.Click
        Await Task.Delay(50)
        _nav.GoForward()
        btnBack.Enabled = True
        btnForward.Enabled = _nav.CanGoForward()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If Not _logout Then
            If XtraMessageBox.Show(
            "Are you sure you want to exit the Payroll System?" & Environment.NewLine & Environment.NewLine &
            "Any unsaved changes will be lost.",
            "Exit Confirmation",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) = DialogResult.No Then

                e.Cancel = True

            End If
        End If

    End Sub


End Class