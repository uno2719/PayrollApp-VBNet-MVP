<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucEmployees
    Inherits GlobalShared.Base.ucBase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucEmployees))
        Dim WindowsuiButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        TablePanel1 = New DevExpress.Utils.Layout.TablePanel()
        tabconMain = New DevExpress.XtraTab.XtraTabControl()
        tabpagePersonalInfo = New DevExpress.XtraTab.XtraTabPage()
        tabpageEmployment = New DevExpress.XtraTab.XtraTabPage()
        tabpageEarnings = New DevExpress.XtraTab.XtraTabPage()
        tabpageStatutory = New DevExpress.XtraTab.XtraTabPage()
        tabpagePrevEmployer = New DevExpress.XtraTab.XtraTabPage()
        tabpageFixTransactions = New DevExpress.XtraTab.XtraTabPage()
        grpMasterlist = New DevExpress.XtraEditors.GroupControl()
        gridconEmployeeList = New DevExpress.XtraGrid.GridControl()
        gridviewEmployeeList = New DevExpress.XtraGrid.Views.Grid.GridView()
        PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        rgFilter = New DevExpress.XtraEditors.RadioGroup()
        LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        grpHeader = New DevExpress.XtraEditors.GroupControl()
        StackPanel1 = New DevExpress.Utils.Layout.StackPanel()
        SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        SimpleButton3 = New DevExpress.XtraEditors.SimpleButton()
        SimpleButton4 = New DevExpress.XtraEditors.SimpleButton()
        lblHdr_MainInfo = New DevExpress.XtraEditors.LabelControl()
        lblHdr_ActiveIndicator = New DevExpress.XtraEditors.LabelControl()
        lblHdr_FullName = New DevExpress.XtraEditors.LabelControl()
        picEmployee = New DevExpress.XtraEditors.PictureEdit()
        PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        CType(TablePanel1, ComponentModel.ISupportInitialize).BeginInit()
        TablePanel1.SuspendLayout()
        CType(tabconMain, ComponentModel.ISupportInitialize).BeginInit()
        tabconMain.SuspendLayout()
        CType(grpMasterlist, ComponentModel.ISupportInitialize).BeginInit()
        grpMasterlist.SuspendLayout()
        CType(gridconEmployeeList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewEmployeeList, ComponentModel.ISupportInitialize).BeginInit()
        CType(PanelControl2, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl2.SuspendLayout()
        CType(rgFilter.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpHeader, ComponentModel.ISupportInitialize).BeginInit()
        grpHeader.SuspendLayout()
        CType(StackPanel1, ComponentModel.ISupportInitialize).BeginInit()
        StackPanel1.SuspendLayout()
        CType(picEmployee.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TablePanel1
        ' 
        TablePanel1.AutoScroll = True
        TablePanel1.Columns.AddRange(New DevExpress.Utils.Layout.TablePanelColumn() {New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 76F), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 5F), New DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 24F)})
        TablePanel1.Controls.Add(tabconMain)
        TablePanel1.Controls.Add(grpMasterlist)
        TablePanel1.Controls.Add(grpHeader)
        TablePanel1.Dock = DockStyle.Fill
        TablePanel1.Location = New Point(4, 4)
        TablePanel1.Margin = New Padding(3, 2, 3, 2)
        TablePanel1.Name = "TablePanel1"
        TablePanel1.Rows.AddRange(New DevExpress.Utils.Layout.TablePanelRow() {New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 160F), New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 5F), New DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F)})
        TablePanel1.Size = New Size(1099, 558)
        TablePanel1.TabIndex = 1
        TablePanel1.UseSkinIndents = True
        ' 
        ' tabconMain
        ' 
        tabconMain.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconMain.Appearance.Options.UseFont = True
        tabconMain.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconMain.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconMain.AppearancePage.Header.Options.UseFont = True
        tabconMain.AppearancePage.Header.Options.UseTextOptions = True
        tabconMain.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        tabconMain.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconMain.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconMain.AppearancePage.HeaderActive.Options.UseFont = True
        TablePanel1.SetColumn(tabconMain, 0)
        tabconMain.Dock = DockStyle.Fill
        tabconMain.Location = New Point(14, 177)
        tabconMain.Margin = New Padding(3, 2, 3, 2)
        tabconMain.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconMain.Name = "tabconMain"
        TablePanel1.SetRow(tabconMain, 2)
        tabconMain.SelectedTabPage = tabpagePersonalInfo
        tabconMain.Size = New Size(809, 368)
        tabconMain.TabIndex = 5
        tabconMain.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpagePersonalInfo, tabpageEmployment, tabpageEarnings, tabpageStatutory, tabpagePrevEmployer, tabpageFixTransactions})
        ' 
        ' tabpagePersonalInfo
        ' 
        tabpagePersonalInfo.Margin = New Padding(3, 2, 3, 2)
        tabpagePersonalInfo.Name = "tabpagePersonalInfo"
        tabpagePersonalInfo.Padding = New Padding(9, 8, 9, 8)
        tabpagePersonalInfo.Size = New Size(807, 339)
        tabpagePersonalInfo.Text = "Personal Info."
        ' 
        ' tabpageEmployment
        ' 
        tabpageEmployment.Margin = New Padding(3, 2, 3, 2)
        tabpageEmployment.Name = "tabpageEmployment"
        tabpageEmployment.Padding = New Padding(9, 8, 9, 8)
        tabpageEmployment.Size = New Size(807, 339)
        tabpageEmployment.Text = "Employment"
        ' 
        ' tabpageEarnings
        ' 
        tabpageEarnings.Margin = New Padding(3, 2, 3, 2)
        tabpageEarnings.Name = "tabpageEarnings"
        tabpageEarnings.Padding = New Padding(9, 8, 9, 8)
        tabpageEarnings.Size = New Size(807, 339)
        tabpageEarnings.Text = "Earnings"
        ' 
        ' tabpageStatutory
        ' 
        tabpageStatutory.Margin = New Padding(3, 2, 3, 2)
        tabpageStatutory.Name = "tabpageStatutory"
        tabpageStatutory.Padding = New Padding(9, 8, 9, 8)
        tabpageStatutory.Size = New Size(807, 339)
        tabpageStatutory.Text = "Statutory"
        ' 
        ' tabpagePrevEmployer
        ' 
        tabpagePrevEmployer.Margin = New Padding(3, 2, 3, 2)
        tabpagePrevEmployer.Name = "tabpagePrevEmployer"
        tabpagePrevEmployer.Padding = New Padding(9, 8, 9, 8)
        tabpagePrevEmployer.Size = New Size(807, 339)
        tabpagePrevEmployer.Text = "Previous Employer"
        ' 
        ' tabpageFixTransactions
        ' 
        tabpageFixTransactions.Margin = New Padding(3, 2, 3, 2)
        tabpageFixTransactions.Name = "tabpageFixTransactions"
        tabpageFixTransactions.Padding = New Padding(9, 8, 9, 8)
        tabpageFixTransactions.Size = New Size(807, 339)
        tabpageFixTransactions.Text = "Fix Transactions"
        ' 
        ' grpMasterlist
        ' 
        grpMasterlist.Appearance.Options.UseFont = True
        grpMasterlist.AppearanceCaption.Font = New Font("Segoe UI", 10.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpMasterlist.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpMasterlist.AppearanceCaption.Options.UseFont = True
        grpMasterlist.AppearanceCaption.Options.UseTextOptions = True
        grpMasterlist.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        grpMasterlist.CaptionImageOptions.Image = CType(resources.GetObject("grpMasterlist.CaptionImageOptions.Image"), Image)
        grpMasterlist.CaptionImageOptions.Location = DevExpress.Utils.GroupElementLocation.BeforeText
        TablePanel1.SetColumn(grpMasterlist, 2)
        grpMasterlist.Controls.Add(gridconEmployeeList)
        grpMasterlist.Controls.Add(PanelControl2)
        grpMasterlist.Controls.Add(PanelControl1)
        grpMasterlist.Dock = DockStyle.Fill
        grpMasterlist.Location = New Point(834, 12)
        grpMasterlist.Margin = New Padding(3, 2, 3, 2)
        grpMasterlist.Name = "grpMasterlist"
        TablePanel1.SetRow(grpMasterlist, 0)
        TablePanel1.SetRowSpan(grpMasterlist, 3)
        grpMasterlist.Size = New Size(251, 533)
        grpMasterlist.TabIndex = 4
        grpMasterlist.Text = " EMPLOYEE MASTERLIST"
        ' 
        ' gridconEmployeeList
        ' 
        gridconEmployeeList.Dock = DockStyle.Fill
        gridconEmployeeList.EmbeddedNavigator.Margin = New Padding(3, 2, 3, 2)
        gridconEmployeeList.Location = New Point(2, 87)
        gridconEmployeeList.MainView = gridviewEmployeeList
        gridconEmployeeList.Margin = New Padding(3, 2, 3, 2)
        gridconEmployeeList.Name = "gridconEmployeeList"
        gridconEmployeeList.Size = New Size(247, 420)
        gridconEmployeeList.TabIndex = 3
        gridconEmployeeList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewEmployeeList})
        ' 
        ' gridviewEmployeeList
        ' 
        gridviewEmployeeList.DetailHeight = 284
        gridviewEmployeeList.GridControl = gridconEmployeeList
        gridviewEmployeeList.Name = "gridviewEmployeeList"
        gridviewEmployeeList.OptionsEditForm.PopupEditFormWidth = 686
        gridviewEmployeeList.OptionsPrint.PrintFilterInfo = True
        ' 
        ' PanelControl2
        ' 
        PanelControl2.Controls.Add(rgFilter)
        PanelControl2.Controls.Add(LabelControl1)
        PanelControl2.Dock = DockStyle.Bottom
        PanelControl2.Location = New Point(2, 507)
        PanelControl2.Margin = New Padding(3, 2, 3, 2)
        PanelControl2.Name = "PanelControl2"
        PanelControl2.Size = New Size(247, 24)
        PanelControl2.TabIndex = 2
        ' 
        ' rgFilter
        ' 
        rgFilter.Dock = DockStyle.Fill
        rgFilter.Location = New Point(49, 2)
        rgFilter.Margin = New Padding(3, 2, 3, 2)
        rgFilter.Name = "rgFilter"
        rgFilter.Properties.AllowMouseWheel = False
        rgFilter.Properties.Appearance.BackColor = Color.Transparent
        rgFilter.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        rgFilter.Properties.Appearance.Options.UseBackColor = True
        rgFilter.Properties.Appearance.Options.UseFont = True
        rgFilter.Properties.AppearanceFocused.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        rgFilter.Properties.AppearanceFocused.FontStyleDelta = FontStyle.Bold
        rgFilter.Properties.AppearanceFocused.Options.UseFont = True
        rgFilter.Properties.AppearanceReadOnly.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        rgFilter.Properties.AppearanceReadOnly.Options.UseFont = True
        rgFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgFilter.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Far
        rgFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Active"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Inactive"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "All")})
        rgFilter.Properties.Padding = New Padding(0)
        rgFilter.Size = New Size(196, 20)
        rgFilter.TabIndex = 0
        rgFilter.TabStop = False
        ' 
        ' LabelControl1
        ' 
        LabelControl1.Appearance.Font = New Font("Segoe UI", 9F)
        LabelControl1.Appearance.Options.UseFont = True
        LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        LabelControl1.Dock = DockStyle.Left
        LabelControl1.Location = New Point(2, 2)
        LabelControl1.Margin = New Padding(3, 2, 3, 2)
        LabelControl1.Name = "LabelControl1"
        LabelControl1.Size = New Size(47, 20)
        LabelControl1.TabIndex = 1
        LabelControl1.Text = " FILTER:"
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(2, 23)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(247, 64)
        PanelControl1.TabIndex = 1
        ' 
        ' wbpMainCommands
        ' 
        wbpMainCommands.ButtonInterval = 5
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New", -1, True, Nothing, True, False, True, "New", -1, True), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(Nothing, True, -1, True), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Profile", -1, True, Nothing, True, False, True, "Edit  ", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Delete", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Delete Profile", -1, True, Nothing, True, False, True, "Delete", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Fill
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(5, 4)
        wbpMainCommands.Margin = New Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(237, 56)
        wbpMainCommands.TabIndex = 1
        wbpMainCommands.Text = "Commands"
        wbpMainCommands.WrapButtons = True
        ' 
        ' grpHeader
        ' 
        grpHeader.AppearanceCaption.Font = New Font("Segoe UI", 10.25F, FontStyle.Bold)
        grpHeader.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpHeader.AppearanceCaption.Options.UseFont = True
        grpHeader.AppearanceCaption.Options.UseTextOptions = True
        grpHeader.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near
        grpHeader.CaptionImageOptions.Image = CType(resources.GetObject("grpHeader.CaptionImageOptions.Image"), Image)
        TablePanel1.SetColumn(grpHeader, 0)
        grpHeader.Controls.Add(StackPanel1)
        grpHeader.Controls.Add(lblHdr_MainInfo)
        grpHeader.Controls.Add(lblHdr_ActiveIndicator)
        grpHeader.Controls.Add(lblHdr_FullName)
        grpHeader.Controls.Add(picEmployee)
        grpHeader.Dock = DockStyle.Fill
        grpHeader.Location = New Point(14, 12)
        grpHeader.Margin = New Padding(3, 2, 3, 2)
        grpHeader.Name = "grpHeader"
        grpHeader.Padding = New Padding(17, 4, 4, 4)
        TablePanel1.SetRow(grpHeader, 0)
        grpHeader.Size = New Size(809, 156)
        grpHeader.TabIndex = 3
        grpHeader.Text = "EMPLOYEE DETAILS"
        ' 
        ' StackPanel1
        ' 
        StackPanel1.AutoSize = True
        StackPanel1.Controls.Add(SimpleButton1)
        StackPanel1.Controls.Add(SimpleButton2)
        StackPanel1.Controls.Add(SimpleButton3)
        StackPanel1.Controls.Add(SimpleButton4)
        StackPanel1.Dock = DockStyle.Right
        StackPanel1.LayoutDirection = DevExpress.Utils.Layout.StackPanelLayoutDirection.TopDown
        StackPanel1.Location = New Point(686, 27)
        StackPanel1.Margin = New Padding(3, 2, 3, 2)
        StackPanel1.Name = "StackPanel1"
        StackPanel1.Size = New Size(117, 123)
        StackPanel1.TabIndex = 4
        StackPanel1.UseSkinIndents = True
        ' 
        ' SimpleButton1
        ' 
        SimpleButton1.AllowFocus = False
        SimpleButton1.Cursor = Cursors.Hand
        SimpleButton1.Location = New Point(2, 12)
        SimpleButton1.Margin = New Padding(2)
        SimpleButton1.Name = "SimpleButton1"
        SimpleButton1.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        SimpleButton1.Size = New Size(113, 21)
        SimpleButton1.TabIndex = 4
        SimpleButton1.Text = "SimpleButton1"
        SimpleButton1.Visible = False
        ' 
        ' SimpleButton2
        ' 
        SimpleButton2.AllowFocus = False
        SimpleButton2.Cursor = Cursors.Hand
        SimpleButton2.Location = New Point(2, 37)
        SimpleButton2.Margin = New Padding(2)
        SimpleButton2.Name = "SimpleButton2"
        SimpleButton2.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        SimpleButton2.Size = New Size(113, 21)
        SimpleButton2.TabIndex = 3
        SimpleButton2.Text = "SimpleButton2"
        SimpleButton2.Visible = False
        ' 
        ' SimpleButton3
        ' 
        SimpleButton3.AllowFocus = False
        SimpleButton3.Cursor = Cursors.Hand
        SimpleButton3.Location = New Point(2, 62)
        SimpleButton3.Margin = New Padding(2)
        SimpleButton3.Name = "SimpleButton3"
        SimpleButton3.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        SimpleButton3.Size = New Size(113, 21)
        SimpleButton3.TabIndex = 5
        SimpleButton3.Text = "SimpleButton3"
        SimpleButton3.Visible = False
        ' 
        ' SimpleButton4
        ' 
        SimpleButton4.AllowFocus = False
        SimpleButton4.Cursor = Cursors.Hand
        SimpleButton4.Location = New Point(2, 87)
        SimpleButton4.Margin = New Padding(2)
        SimpleButton4.Name = "SimpleButton4"
        SimpleButton4.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        SimpleButton4.Size = New Size(113, 21)
        SimpleButton4.TabIndex = 6
        SimpleButton4.Text = "SimpleButton4"
        SimpleButton4.Visible = False
        ' 
        ' lblHdr_MainInfo
        ' 
        lblHdr_MainInfo.Appearance.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold)
        lblHdr_MainInfo.Appearance.Options.UseFont = True
        lblHdr_MainInfo.Location = New Point(153, 70)
        lblHdr_MainInfo.Margin = New Padding(3, 2, 3, 2)
        lblHdr_MainInfo.Name = "lblHdr_MainInfo"
        lblHdr_MainInfo.Size = New Size(108, 20)
        lblHdr_MainInfo.TabIndex = 3
        lblHdr_MainInfo.Text = "..........................."
        ' 
        ' lblHdr_ActiveIndicator
        ' 
        lblHdr_ActiveIndicator.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblHdr_ActiveIndicator.Appearance.ForeColor = Color.SeaGreen
        lblHdr_ActiveIndicator.Appearance.Options.UseFont = True
        lblHdr_ActiveIndicator.Appearance.Options.UseForeColor = True
        lblHdr_ActiveIndicator.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        lblHdr_ActiveIndicator.ImageOptions.Alignment = ContentAlignment.MiddleLeft
        lblHdr_ActiveIndicator.ImageOptions.Image = CType(resources.GetObject("lblHdr_ActiveIndicator.ImageOptions.Image"), Image)
        lblHdr_ActiveIndicator.Location = New Point(153, 130)
        lblHdr_ActiveIndicator.Margin = New Padding(3, 2, 3, 2)
        lblHdr_ActiveIndicator.Name = "lblHdr_ActiveIndicator"
        lblHdr_ActiveIndicator.Size = New Size(33, 20)
        lblHdr_ActiveIndicator.TabIndex = 2
        lblHdr_ActiveIndicator.Text = "..."
        ' 
        ' lblHdr_FullName
        ' 
        lblHdr_FullName.Appearance.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold)
        lblHdr_FullName.Appearance.Options.UseFont = True
        lblHdr_FullName.Location = New Point(153, 29)
        lblHdr_FullName.Margin = New Padding(3, 2, 3, 2)
        lblHdr_FullName.Name = "lblHdr_FullName"
        lblHdr_FullName.Size = New Size(133, 37)
        lblHdr_FullName.TabIndex = 1
        lblHdr_FullName.Text = "..................."
        ' 
        ' picEmployee
        ' 
        picEmployee.Dock = DockStyle.Left
        picEmployee.EditValue = My.Resources.Resources.img_default_avatar1
        picEmployee.Location = New Point(19, 27)
        picEmployee.Margin = New Padding(3, 2, 3, 2)
        picEmployee.Name = "picEmployee"
        picEmployee.Properties.Padding = New Padding(5)
        picEmployee.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto
        picEmployee.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        picEmployee.Size = New Size(121, 123)
        picEmployee.TabIndex = 0
        ' 
        ' PictureEdit1
        ' 
        PictureEdit1.EditValue = resources.GetObject("PictureEdit1.EditValue")
        PictureEdit1.Location = New Point(12, 33)
        PictureEdit1.Name = "PictureEdit1"
        PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto
        PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        PictureEdit1.Size = New Size(138, 135)
        PictureEdit1.TabIndex = 0
        ' 
        ' ucEmployees
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(TablePanel1)
        Name = "ucEmployees"
        Padding = New Padding(4)
        Size = New Size(1107, 566)
        CType(TablePanel1, ComponentModel.ISupportInitialize).EndInit()
        TablePanel1.ResumeLayout(False)
        CType(tabconMain, ComponentModel.ISupportInitialize).EndInit()
        tabconMain.ResumeLayout(False)
        CType(grpMasterlist, ComponentModel.ISupportInitialize).EndInit()
        grpMasterlist.ResumeLayout(False)
        CType(gridconEmployeeList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewEmployeeList, ComponentModel.ISupportInitialize).EndInit()
        CType(PanelControl2, ComponentModel.ISupportInitialize).EndInit()
        PanelControl2.ResumeLayout(False)
        CType(rgFilter.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        CType(grpHeader, ComponentModel.ISupportInitialize).EndInit()
        grpHeader.ResumeLayout(False)
        grpHeader.PerformLayout()
        CType(StackPanel1, ComponentModel.ISupportInitialize).EndInit()
        StackPanel1.ResumeLayout(False)
        CType(picEmployee.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureEdit1.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents TablePanel1 As DevExpress.Utils.Layout.TablePanel
    Friend WithEvents grpMasterlist As DevExpress.XtraEditors.GroupControl
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents grpHeader As DevExpress.XtraEditors.GroupControl
    Friend WithEvents picEmployee As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents tabconMain As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpagePersonalInfo As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageEmployment As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageEarnings As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageStatutory As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePrevEmployer As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageFixTransactions As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents SimpleSeparator3 As DevExpress.XtraLayout.SimpleSeparator
    Friend WithEvents SimpleSeparator2 As DevExpress.XtraLayout.SimpleSeparator
    Friend WithEvents SimpleSeparator4 As DevExpress.XtraLayout.SimpleSeparator
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblHdr_FullName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblHdr_MainInfo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblHdr_ActiveIndicator As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridconEmployeeList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewEmployeeList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents StackPanel1 As DevExpress.Utils.Layout.StackPanel
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton3 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton4 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents rgFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl

End Class
