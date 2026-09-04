Imports DevExpress.Utils.Animation
Imports DevExpress.XtraBars.FluentDesignSystem
Imports DevExpress.XtraBars.Navigation
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Database

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




    ' Event Handler para sa AccordionControl Click
    Private Async Sub accordionControl1_ElementClick(sender As Object, e As ElementClickEventArgs) Handles AccordionControl1.ElementClick
        ' Siguraduhin na ang "Tag" property ng Accordion Element sa Designer ay may value
        If e.Element.Tag Is Nothing Then Return

        Await Task.Delay(50)
        Select Case e.Element.Tag.ToString()
            Case "main_Dashboard"

                _nav.NavigateTo(Of ucDashboard)()

            Case "main_Employees"
                _nav.NavigateTo(Of ucEmployees)(Function() AppComposition.BuildEmployeeView())

            Case "main_Payroll"
                _nav.NavigateTo(Of ucPayroll)()

            Case "admin_UsersAccount"
                _nav.NavigateTo(Of ucUsers)(Function() AppComposition.BuildUsersView())

            Case "settings_MasterData"
                _nav.NavigateTo(Of ucSettingsLookups)(Function() AppComposition.BuildSettingsLookupsView())

            Case "logout"
                PerformLogout()

        End Select

        btnBack.Enabled = _nav.IsNavigationChanged
        btnForward.Enabled = False
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