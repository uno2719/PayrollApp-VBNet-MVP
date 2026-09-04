<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        fluentMainContainer = New DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer()
        AccordionControl1 = New DevExpress.XtraBars.Navigation.AccordionControl()
        aceHeaderHome = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceDashboard = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceEmployees = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        acePayroll = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceHeaderSettings = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsPayrollSetup = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsGeneral = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsCompany = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsMasterData = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsCutOff = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        AccordionControlSeparator6 = New DevExpress.XtraBars.Navigation.AccordionControlSeparator()
        aceSettingsPersonal = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettings_Leave = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        AccordionControlSeparator7 = New DevExpress.XtraBars.Navigation.AccordionControlSeparator()
        aceSettingsTaxTable = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsStatutory = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceSettingsPayroll = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        AccordionControlSeparator9 = New DevExpress.XtraBars.Navigation.AccordionControlSeparator()
        aceSettingsAppConfig = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceAppConfigDBSettings = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        AccordionControlElement1 = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceLogout = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceHeaderAdministration = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        aceAdminUsers = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        FluentDesignFormControl1 = New DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl()
        SkinDropDownButtonItem1 = New DevExpress.XtraBars.SkinDropDownButtonItem()
        SkinPaletteDropDownButtonItem1 = New DevExpress.XtraBars.SkinPaletteDropDownButtonItem()
        FluentFormDefaultManager1 = New DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(components)
        pnlHeaderMain = New DevExpress.XtraEditors.PanelControl()
        btnForward = New DevExpress.XtraEditors.SimpleButton()
        btnBack = New DevExpress.XtraEditors.SimpleButton()
        lblCurrentUserDisplayedName = New DevExpress.XtraEditors.LabelControl()
        PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        lblPageTitle = New DevExpress.XtraEditors.LabelControl()
        lblBreadcrumb = New DevExpress.XtraEditors.LabelControl()
        pnlFooterMain = New DevExpress.XtraEditors.PanelControl()
        lblHost = New DevExpress.XtraEditors.LabelControl()
        lblVersion = New DevExpress.XtraEditors.LabelControl()
        tmMain = New DevExpress.Utils.Animation.TransitionManager(components)
        aceSettingsCompanyProfile = New DevExpress.XtraBars.Navigation.AccordionControlElement()
        CType(AccordionControl1, ComponentModel.ISupportInitialize).BeginInit()
        CType(FluentDesignFormControl1, ComponentModel.ISupportInitialize).BeginInit()
        CType(FluentFormDefaultManager1, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlHeaderMain, ComponentModel.ISupportInitialize).BeginInit()
        pnlHeaderMain.SuspendLayout()
        CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlFooterMain, ComponentModel.ISupportInitialize).BeginInit()
        pnlFooterMain.SuspendLayout()
        SuspendLayout()
        ' 
        ' fluentMainContainer
        ' 
        fluentMainContainer.Dock = DockStyle.Fill
        fluentMainContainer.Location = New Point(250, 90)
        fluentMainContainer.Margin = New Padding(2)
        fluentMainContainer.Name = "fluentMainContainer"
        fluentMainContainer.Size = New Size(1148, 647)
        fluentMainContainer.TabIndex = 0
        ' 
        ' AccordionControl1
        ' 
        AccordionControl1.Appearance.AccordionControl.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        AccordionControl1.Appearance.AccordionControl.Options.UseFont = True
        AccordionControl1.Appearance.Group.Default.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold)
        AccordionControl1.Appearance.Group.Default.Options.UseFont = True
        AccordionControl1.Appearance.Item.Default.Font = New Font("Segoe UI", 9.75F)
        AccordionControl1.Appearance.Item.Default.Options.UseFont = True
        AccordionControl1.Dock = DockStyle.Left
        AccordionControl1.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceHeaderHome, aceHeaderSettings, aceLogout, aceHeaderAdministration})
        AccordionControl1.Location = New Point(0, 31)
        AccordionControl1.Margin = New Padding(2)
        AccordionControl1.Name = "AccordionControl1"
        AccordionControl1.OptionsFooter.ActiveGroupDisplayMode = DevExpress.XtraBars.Navigation.ActiveGroupDisplayMode.GroupHeaderAndContent
        AccordionControl1.OptionsHamburgerMenu.DisplayMode = DevExpress.XtraBars.Navigation.AccordionControlDisplayMode.Overlay
        AccordionControl1.OptionsMinimizing.AllowFooterResizing = False
        AccordionControl1.RootDisplayMode = DevExpress.XtraBars.Navigation.AccordionControlRootDisplayMode.Footer
        AccordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch
        AccordionControl1.Size = New Size(250, 730)
        AccordionControl1.TabIndex = 1
        AccordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu
        ' 
        ' aceHeaderHome
        ' 
        aceHeaderHome.Appearance.Default.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        aceHeaderHome.Appearance.Default.Options.UseFont = True
        aceHeaderHome.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceDashboard, aceEmployees, acePayroll})
        aceHeaderHome.Expanded = True
        aceHeaderHome.Hint = "Main Menu"
        aceHeaderHome.ImageOptions.SvgImage = CType(resources.GetObject("aceHeaderHome.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceHeaderHome.ImageOptions.SvgImageSize = New Size(30, 30)
        aceHeaderHome.Name = "aceHeaderHome"
        aceHeaderHome.Tag = "Main"
        aceHeaderHome.Text = "Main Menu"
        ' 
        ' aceDashboard
        ' 
        aceDashboard.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceDashboard.Appearance.Default.Options.UseFont = True
        aceDashboard.ImageOptions.SvgImage = CType(resources.GetObject("aceDashboard.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceDashboard.ImageOptions.SvgImageSize = New Size(14, 14)
        aceDashboard.Name = "aceDashboard"
        aceDashboard.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceDashboard.Tag = "main_Dashboard"
        aceDashboard.Text = "Dashboard"
        ' 
        ' aceEmployees
        ' 
        aceEmployees.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceEmployees.Appearance.Default.Options.UseFont = True
        aceEmployees.ImageOptions.SvgImage = CType(resources.GetObject("aceEmployees.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceEmployees.ImageOptions.SvgImageSize = New Size(18, 18)
        aceEmployees.Name = "aceEmployees"
        aceEmployees.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceEmployees.Tag = "main_Employees"
        aceEmployees.Text = "Employees"
        ' 
        ' acePayroll
        ' 
        acePayroll.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        acePayroll.Appearance.Default.Options.UseFont = True
        acePayroll.ImageOptions.SvgImage = CType(resources.GetObject("acePayroll.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        acePayroll.ImageOptions.SvgImageSize = New Size(16, 16)
        acePayroll.Name = "acePayroll"
        acePayroll.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        acePayroll.Tag = "main_Payroll"
        acePayroll.Text = "Payroll"
        ' 
        ' aceHeaderSettings
        ' 
        aceHeaderSettings.Appearance.Default.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold)
        aceHeaderSettings.Appearance.Default.Options.UseFont = True
        aceHeaderSettings.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceSettingsPayrollSetup, AccordionControlSeparator9, aceSettingsAppConfig})
        aceHeaderSettings.Expanded = True
        aceHeaderSettings.Hint = "System Settings"
        aceHeaderSettings.ImageOptions.SvgImage = CType(resources.GetObject("aceHeaderSettings.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceHeaderSettings.ImageOptions.SvgImageSize = New Size(30, 30)
        aceHeaderSettings.Name = "aceHeaderSettings"
        aceHeaderSettings.Tag = "Settings"
        aceHeaderSettings.Text = "Settings"
        ' 
        ' aceSettingsPayrollSetup
        ' 
        aceSettingsPayrollSetup.Appearance.Default.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        aceSettingsPayrollSetup.Appearance.Default.Options.UseFont = True
        aceSettingsPayrollSetup.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceSettingsGeneral, aceSettingsCompany, aceSettingsMasterData, aceSettingsCutOff, AccordionControlSeparator6, aceSettingsPersonal, aceSettings_Leave, AccordionControlSeparator7, aceSettingsTaxTable, aceSettingsStatutory, aceSettingsPayroll})
        aceSettingsPayrollSetup.Expanded = True
        aceSettingsPayrollSetup.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsPayrollSetup.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsPayrollSetup.ImageOptions.SvgImageSize = New Size(20, 20)
        aceSettingsPayrollSetup.Name = "aceSettingsPayrollSetup"
        aceSettingsPayrollSetup.Text = "Payroll Setup"
        ' 
        ' aceSettingsGeneral
        ' 
        aceSettingsGeneral.Appearance.Default.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        aceSettingsGeneral.Appearance.Default.Options.UseFont = True
        aceSettingsGeneral.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsGeneral.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsGeneral.ImageOptions.SvgImageSize = New Size(18, 18)
        aceSettingsGeneral.Name = "aceSettingsGeneral"
        aceSettingsGeneral.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsGeneral.Tag = "settings_General"
        aceSettingsGeneral.Text = "General"
        ' 
        ' aceSettingsCompany
        ' 
        aceSettingsCompany.Appearance.Default.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        aceSettingsCompany.Appearance.Default.Options.UseFont = True
        aceSettingsCompany.Expanded = True
        aceSettingsCompany.HeaderTemplate.AddRange(New DevExpress.XtraBars.Navigation.HeaderElementInfo() {New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Image), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Text), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.ContextButtons), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.HeaderControl)})
        aceSettingsCompany.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsCompany.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsCompany.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsCompany.Name = "aceSettingsCompany"
        aceSettingsCompany.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsCompany.Tag = "settings_Company"
        aceSettingsCompany.Text = "Company"
        aceSettingsCompany.HeaderTemplate.AddRange(New DevExpress.XtraBars.Navigation.HeaderElementInfo() {New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Image), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.Text), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.ContextButtons), New DevExpress.XtraBars.Navigation.HeaderElementInfo(DevExpress.XtraBars.Navigation.HeaderElementType.HeaderControl)})
        ' 
        ' aceSettingsMasterData
        ' 
        aceSettingsMasterData.Appearance.Default.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        aceSettingsMasterData.Appearance.Default.Options.UseFont = True
        aceSettingsMasterData.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsMasterData.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsMasterData.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsMasterData.Name = "aceSettingsMasterData"
        aceSettingsMasterData.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsMasterData.Text = "Master Data"
        ' 
        ' aceSettingsCutOff
        ' 
        aceSettingsCutOff.Appearance.Default.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        aceSettingsCutOff.Appearance.Default.Options.UseFont = True
        aceSettingsCutOff.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsCutOff.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsCutOff.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsCutOff.Name = "aceSettingsCutOff"
        aceSettingsCutOff.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsCutOff.Tag = "settings_CutOff"
        aceSettingsCutOff.Text = "Cut-off"
        ' 
        ' AccordionControlSeparator6
        ' 
        AccordionControlSeparator6.Name = "AccordionControlSeparator6"
        ' 
        ' aceSettingsPersonal
        ' 
        aceSettingsPersonal.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceSettingsPersonal.Appearance.Default.Options.UseFont = True
        aceSettingsPersonal.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsPersonal.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsPersonal.ImageOptions.SvgImageSize = New Size(18, 18)
        aceSettingsPersonal.Name = "aceSettingsPersonal"
        aceSettingsPersonal.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsPersonal.Tag = "settings_Personal"
        aceSettingsPersonal.Text = "Personal"
        ' 
        ' aceSettings_Leave
        ' 
        aceSettings_Leave.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceSettings_Leave.Appearance.Default.Options.UseFont = True
        aceSettings_Leave.ImageOptions.SvgImage = CType(resources.GetObject("aceSettings_Leave.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettings_Leave.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettings_Leave.Name = "aceSettings_Leave"
        aceSettings_Leave.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettings_Leave.Tag = "settings_Leave"
        aceSettings_Leave.Text = "Leave"
        ' 
        ' AccordionControlSeparator7
        ' 
        AccordionControlSeparator7.Name = "AccordionControlSeparator7"
        ' 
        ' aceSettingsTaxTable
        ' 
        aceSettingsTaxTable.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceSettingsTaxTable.Appearance.Default.Options.UseFont = True
        aceSettingsTaxTable.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsTaxTable.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsTaxTable.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsTaxTable.Name = "aceSettingsTaxTable"
        aceSettingsTaxTable.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsTaxTable.Tag = "settings_TaxTable"
        aceSettingsTaxTable.Text = "Tax Table"
        ' 
        ' aceSettingsStatutory
        ' 
        aceSettingsStatutory.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceSettingsStatutory.Appearance.Default.Options.UseFont = True
        aceSettingsStatutory.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsStatutory.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsStatutory.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsStatutory.Name = "aceSettingsStatutory"
        aceSettingsStatutory.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsStatutory.Tag = "settings_Statutory"
        aceSettingsStatutory.Text = "Statutory"
        ' 
        ' aceSettingsPayroll
        ' 
        aceSettingsPayroll.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceSettingsPayroll.Appearance.Default.Options.UseFont = True
        aceSettingsPayroll.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsPayroll.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsPayroll.ImageOptions.SvgImageSize = New Size(16, 16)
        aceSettingsPayroll.Name = "aceSettingsPayroll"
        aceSettingsPayroll.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsPayroll.Tag = "setting_Payroll"
        aceSettingsPayroll.Text = "Payroll"
        ' 
        ' AccordionControlSeparator9
        ' 
        AccordionControlSeparator9.Name = "AccordionControlSeparator9"
        ' 
        ' aceSettingsAppConfig
        ' 
        aceSettingsAppConfig.Appearance.Default.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        aceSettingsAppConfig.Appearance.Default.Options.UseFont = True
        aceSettingsAppConfig.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceAppConfigDBSettings, AccordionControlElement1})
        aceSettingsAppConfig.Expanded = True
        aceSettingsAppConfig.Hint = "App Config"
        aceSettingsAppConfig.ImageOptions.SvgImage = CType(resources.GetObject("aceSettingsAppConfig.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceSettingsAppConfig.ImageOptions.SvgImageSize = New Size(19, 19)
        aceSettingsAppConfig.Name = "aceSettingsAppConfig"
        aceSettingsAppConfig.Text = "Application Settings"
        ' 
        ' aceAppConfigDBSettings
        ' 
        aceAppConfigDBSettings.Appearance.Default.Font = New Font("Segoe UI", 9.75F)
        aceAppConfigDBSettings.Appearance.Default.Options.UseFont = True
        aceAppConfigDBSettings.ImageOptions.Image = CType(resources.GetObject("aceAppConfigDBSettings.ImageOptions.Image"), Image)
        aceAppConfigDBSettings.ImageOptions.SvgImageSize = New Size(16, 16)
        aceAppConfigDBSettings.Name = "aceAppConfigDBSettings"
        aceAppConfigDBSettings.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceAppConfigDBSettings.Text = "Database Settings"
        ' 
        ' AccordionControlElement1
        ' 
        AccordionControlElement1.ImageOptions.SvgImage = CType(resources.GetObject("AccordionControlElement1.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        AccordionControlElement1.ImageOptions.SvgImageSize = New Size(16, 16)
        AccordionControlElement1.Name = "AccordionControlElement1"
        AccordionControlElement1.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        AccordionControlElement1.Text = "Theme / Skin"
        ' 
        ' aceLogout
        ' 
        aceLogout.ControlFooterAlignment = DevExpress.XtraBars.Navigation.AccordionItemFooterAlignment.Far
        aceLogout.Hint = "Logout"
        aceLogout.ImageOptions.ImageLayoutMode = DevExpress.XtraBars.Navigation.ImageLayoutMode.Squeeze
        aceLogout.ImageOptions.SvgImage = My.Resources.Resources.logout2_svg
        aceLogout.ImageOptions.SvgImageSize = New Size(30, 30)
        aceLogout.Name = "aceLogout"
        aceLogout.ShortcutKey = New DevExpress.XtraBars.BarShortcut(Keys.Control Or Keys.L)
        aceLogout.ShortcutKeyDisplayString = "Ctrl + L"
        aceLogout.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceLogout.Tag = "logout"
        aceLogout.Text = "Logout"
        ' 
        ' aceHeaderAdministration
        ' 
        aceHeaderAdministration.ControlFooterAlignment = DevExpress.XtraBars.Navigation.AccordionItemFooterAlignment.Far
        aceHeaderAdministration.Elements.AddRange(New DevExpress.XtraBars.Navigation.AccordionControlElement() {aceAdminUsers})
        aceHeaderAdministration.Expanded = True
        aceHeaderAdministration.ImageOptions.SvgImage = CType(resources.GetObject("aceHeaderAdministration.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceHeaderAdministration.ImageOptions.SvgImageSize = New Size(30, 30)
        aceHeaderAdministration.Name = "aceHeaderAdministration"
        aceHeaderAdministration.Text = "Administration"
        ' 
        ' aceAdminUsers
        ' 
        aceAdminUsers.ImageOptions.SvgImage = CType(resources.GetObject("aceAdminUsers.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        aceAdminUsers.ImageOptions.SvgImageSize = New Size(16, 16)
        aceAdminUsers.Name = "aceAdminUsers"
        aceAdminUsers.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceAdminUsers.Tag = "admin_UsersAccount"
        aceAdminUsers.Text = "Users"
        ' 
        ' FluentDesignFormControl1
        ' 
        FluentDesignFormControl1.FluentDesignForm = Me
        FluentDesignFormControl1.Items.AddRange(New DevExpress.XtraBars.BarItem() {SkinDropDownButtonItem1, SkinPaletteDropDownButtonItem1})
        FluentDesignFormControl1.Location = New Point(0, 0)
        FluentDesignFormControl1.Manager = FluentFormDefaultManager1
        FluentDesignFormControl1.Margin = New Padding(2)
        FluentDesignFormControl1.Name = "FluentDesignFormControl1"
        FluentDesignFormControl1.Size = New Size(1398, 31)
        FluentDesignFormControl1.TabIndex = 2
        FluentDesignFormControl1.TabStop = False
        FluentDesignFormControl1.TitleItemLinks.Add(SkinDropDownButtonItem1)
        FluentDesignFormControl1.TitleItemLinks.Add(SkinPaletteDropDownButtonItem1)
        ' 
        ' SkinDropDownButtonItem1
        ' 
        SkinDropDownButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        SkinDropDownButtonItem1.Id = 0
        SkinDropDownButtonItem1.Name = "SkinDropDownButtonItem1"
        ' 
        ' SkinPaletteDropDownButtonItem1
        ' 
        SkinPaletteDropDownButtonItem1.ActAsDropDown = True
        SkinPaletteDropDownButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right
        SkinPaletteDropDownButtonItem1.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
        SkinPaletteDropDownButtonItem1.Id = 1
        SkinPaletteDropDownButtonItem1.Name = "SkinPaletteDropDownButtonItem1"
        ' 
        ' FluentFormDefaultManager1
        ' 
        FluentFormDefaultManager1.DockWindowTabFont = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        FluentFormDefaultManager1.Form = Me
        FluentFormDefaultManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {SkinDropDownButtonItem1, SkinPaletteDropDownButtonItem1})
        FluentFormDefaultManager1.MaxItemId = 2
        ' 
        ' pnlHeaderMain
        ' 
        pnlHeaderMain.Appearance.BackColor = Color.LightGray
        pnlHeaderMain.Appearance.Options.UseBackColor = True
        pnlHeaderMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlHeaderMain.Controls.Add(btnForward)
        pnlHeaderMain.Controls.Add(btnBack)
        pnlHeaderMain.Controls.Add(lblCurrentUserDisplayedName)
        pnlHeaderMain.Controls.Add(PictureEdit1)
        pnlHeaderMain.Controls.Add(lblPageTitle)
        pnlHeaderMain.Controls.Add(lblBreadcrumb)
        pnlHeaderMain.Dock = DockStyle.Top
        pnlHeaderMain.Location = New Point(250, 31)
        pnlHeaderMain.Name = "pnlHeaderMain"
        pnlHeaderMain.Padding = New Padding(5, 6, 5, 6)
        pnlHeaderMain.Size = New Size(1148, 59)
        pnlHeaderMain.TabIndex = 3
        ' 
        ' btnForward
        ' 
        btnForward.AllowFocus = False
        btnForward.Appearance.BackColor = Color.Silver
        btnForward.Appearance.BorderColor = Color.Silver
        btnForward.Appearance.Options.UseBackColor = True
        btnForward.Appearance.Options.UseBorderColor = True
        btnForward.AutoSize = True
        btnForward.Cursor = Cursors.Hand
        btnForward.Dock = DockStyle.Left
        btnForward.Enabled = False
        btnForward.ImageOptions.Image = CType(resources.GetObject("btnForward.ImageOptions.Image"), Image)
        btnForward.Location = New Point(45, 6)
        btnForward.Margin = New Padding(3, 2, 3, 2)
        btnForward.Name = "btnForward"
        btnForward.Padding = New Padding(2, 0, 0, 0)
        btnForward.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        btnForward.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        btnForward.Size = New Size(40, 47)
        btnForward.TabIndex = 5
        btnForward.TabStop = False
        ' 
        ' btnBack
        ' 
        btnBack.AllowFocus = False
        btnBack.Appearance.BackColor = Color.Silver
        btnBack.Appearance.BorderColor = Color.Silver
        btnBack.Appearance.Options.UseBackColor = True
        btnBack.Appearance.Options.UseBorderColor = True
        btnBack.AutoSize = True
        btnBack.Cursor = Cursors.Hand
        btnBack.Dock = DockStyle.Left
        btnBack.Enabled = False
        btnBack.ImageOptions.Image = CType(resources.GetObject("btnBack.ImageOptions.Image"), Image)
        btnBack.Location = New Point(5, 6)
        btnBack.Margin = New Padding(3, 2, 3, 2)
        btnBack.Name = "btnBack"
        btnBack.Padding = New Padding(0, 0, 2, 0)
        btnBack.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light
        btnBack.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        btnBack.Size = New Size(40, 47)
        btnBack.TabIndex = 4
        btnBack.TabStop = False
        ' 
        ' lblCurrentUserDisplayedName
        ' 
        lblCurrentUserDisplayedName.Appearance.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCurrentUserDisplayedName.Appearance.ForeColor = Color.Black
        lblCurrentUserDisplayedName.Appearance.Options.UseFont = True
        lblCurrentUserDisplayedName.Appearance.Options.UseForeColor = True
        lblCurrentUserDisplayedName.Appearance.Options.UseTextOptions = True
        lblCurrentUserDisplayedName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        lblCurrentUserDisplayedName.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        lblCurrentUserDisplayedName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblCurrentUserDisplayedName.Dock = DockStyle.Right
        lblCurrentUserDisplayedName.Location = New Point(730, 6)
        lblCurrentUserDisplayedName.Margin = New Padding(10)
        lblCurrentUserDisplayedName.Name = "lblCurrentUserDisplayedName"
        lblCurrentUserDisplayedName.Padding = New Padding(5)
        lblCurrentUserDisplayedName.Size = New Size(372, 47)
        lblCurrentUserDisplayedName.TabIndex = 3
        lblCurrentUserDisplayedName.Text = "Uno Alinea (Admin)"
        ' 
        ' PictureEdit1
        ' 
        PictureEdit1.Dock = DockStyle.Right
        PictureEdit1.EditValue = resources.GetObject("PictureEdit1.EditValue")
        PictureEdit1.Location = New Point(1102, 6)
        PictureEdit1.Margin = New Padding(10)
        PictureEdit1.MenuManager = FluentFormDefaultManager1
        PictureEdit1.Name = "PictureEdit1"
        PictureEdit1.Properties.Appearance.BackColor = Color.Transparent
        PictureEdit1.Properties.Appearance.Options.UseBackColor = True
        PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto
        PictureEdit1.Properties.ShowMenu = False
        PictureEdit1.Properties.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None
        PictureEdit1.Size = New Size(41, 47)
        PictureEdit1.TabIndex = 2
        ' 
        ' lblPageTitle
        ' 
        lblPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblPageTitle.Appearance.ForeColor = Color.Black
        lblPageTitle.Appearance.Options.UseFont = True
        lblPageTitle.Appearance.Options.UseForeColor = True
        lblPageTitle.Location = New Point(94, 20)
        lblPageTitle.Name = "lblPageTitle"
        lblPageTitle.Size = New Size(131, 30)
        lblPageTitle.TabIndex = 1
        lblPageTitle.Text = "DASHBOARD"
        ' 
        ' lblBreadcrumb
        ' 
        lblBreadcrumb.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblBreadcrumb.Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblBreadcrumb.Appearance.ForeColor = Color.FromArgb(CByte(64), CByte(64), CByte(64))
        lblBreadcrumb.Appearance.Options.UseFont = True
        lblBreadcrumb.Appearance.Options.UseForeColor = True
        lblBreadcrumb.Location = New Point(94, 5)
        lblBreadcrumb.Name = "lblBreadcrumb"
        lblBreadcrumb.Size = New Size(86, 13)
        lblBreadcrumb.TabIndex = 0
        lblBreadcrumb.Text = "HR > Dashboard"
        ' 
        ' pnlFooterMain
        ' 
        pnlFooterMain.Appearance.BackColor = Color.Silver
        pnlFooterMain.Appearance.Options.UseBackColor = True
        pnlFooterMain.Controls.Add(lblHost)
        pnlFooterMain.Controls.Add(lblVersion)
        pnlFooterMain.Dock = DockStyle.Bottom
        pnlFooterMain.Location = New Point(250, 737)
        pnlFooterMain.Name = "pnlFooterMain"
        pnlFooterMain.Size = New Size(1148, 24)
        pnlFooterMain.TabIndex = 4
        ' 
        ' lblHost
        ' 
        lblHost.Appearance.ForeColor = Color.DarkGray
        lblHost.Appearance.Options.UseForeColor = True
        lblHost.Appearance.Options.UseTextOptions = True
        lblHost.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        lblHost.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblHost.Dock = DockStyle.Right
        lblHost.Location = New Point(763, 2)
        lblHost.Name = "lblHost"
        lblHost.Padding = New Padding(8)
        lblHost.Size = New Size(383, 20)
        lblHost.TabIndex = 12
        lblHost.Text = "v1.0.0 — Internal use only"
        ' 
        ' lblVersion
        ' 
        lblVersion.Appearance.ForeColor = Color.DarkGray
        lblVersion.Appearance.Options.UseForeColor = True
        lblVersion.Appearance.Options.UseTextOptions = True
        lblVersion.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        lblVersion.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblVersion.Dock = DockStyle.Left
        lblVersion.Location = New Point(2, 2)
        lblVersion.Name = "lblVersion"
        lblVersion.Padding = New Padding(8)
        lblVersion.Size = New Size(394, 20)
        lblVersion.TabIndex = 11
        lblVersion.Text = "v1.0.0 — Internal use only"
        ' 
        ' aceSettingsCompanyProfile
        ' 
        aceSettingsCompanyProfile.Appearance.Default.Font = New Font("Segoe UI", 9F)
        aceSettingsCompanyProfile.Appearance.Default.Options.UseFont = True
        aceSettingsCompanyProfile.Name = "aceSettingsCompanyProfile"
        aceSettingsCompanyProfile.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item
        aceSettingsCompanyProfile.Text = "Company Profile"
        ' 
        ' frmMain
        ' 
        Appearance.BackColor = SystemColors.Control
        Appearance.Options.UseBackColor = True
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1398, 761)
        ControlContainer = fluentMainContainer
        Controls.Add(fluentMainContainer)
        Controls.Add(pnlFooterMain)
        Controls.Add(pnlHeaderMain)
        Controls.Add(AccordionControl1)
        Controls.Add(FluentDesignFormControl1)
        FluentDesignFormControl = FluentDesignFormControl1
        Font = New Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        IconOptions.Icon = CType(resources.GetObject("frmMain.IconOptions.Icon"), Icon)
        Margin = New Padding(2)
        MinimumSize = New Size(1400, 762)
        Name = "frmMain"
        NavigationControl = AccordionControl1
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pacsports Payroll"
        WindowState = FormWindowState.Maximized
        CType(AccordionControl1, ComponentModel.ISupportInitialize).EndInit()
        CType(FluentDesignFormControl1, ComponentModel.ISupportInitialize).EndInit()
        CType(FluentFormDefaultManager1, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlHeaderMain, ComponentModel.ISupportInitialize).EndInit()
        pnlHeaderMain.ResumeLayout(False)
        pnlHeaderMain.PerformLayout()
        CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlFooterMain, ComponentModel.ISupportInitialize).EndInit()
        pnlFooterMain.ResumeLayout(False)
        ResumeLayout(False)

    End Sub
    Friend WithEvents fluentMainContainer As DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer
    Friend WithEvents AccordionControl1 As DevExpress.XtraBars.Navigation.AccordionControl
    Friend WithEvents aceHeaderHome As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents FluentDesignFormControl1 As DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl
    Friend WithEvents FluentFormDefaultManager1 As DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager
    Friend WithEvents SkinDropDownButtonItem1 As DevExpress.XtraBars.SkinDropDownButtonItem
    Friend WithEvents SkinPaletteDropDownButtonItem1 As DevExpress.XtraBars.SkinPaletteDropDownButtonItem
    Friend WithEvents aceEmployees As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents acePayroll As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceHeaderSettings As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceDashboard As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsGeneral As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsCompany As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsTaxTable As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsStatutory As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents pnlFooterMain As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pnlHeaderMain As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblBreadcrumb As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lblCurrentUserDisplayedName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents tmMain As DevExpress.Utils.Animation.TransitionManager
    Friend WithEvents btnBack As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnForward As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents aceSettingsMasterData As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents lblVersion As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblHost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents aceSettingsCutOff As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsPersonal As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettings_Leave As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsPayroll As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsAppConfig As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceAppConfigDBSettings As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceLogout As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsPayrollSetup As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceHeaderAdministration As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents AccordionControlSeparator6 As DevExpress.XtraBars.Navigation.AccordionControlSeparator
    Friend WithEvents AccordionControlSeparator7 As DevExpress.XtraBars.Navigation.AccordionControlSeparator
    Friend WithEvents AccordionControlSeparator9 As DevExpress.XtraBars.Navigation.AccordionControlSeparator
    Friend WithEvents aceAdminUsers As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents AccordionControlElement1 As DevExpress.XtraBars.Navigation.AccordionControlElement
    Friend WithEvents aceSettingsCompanyProfile As DevExpress.XtraBars.Navigation.AccordionControlElement
End Class
