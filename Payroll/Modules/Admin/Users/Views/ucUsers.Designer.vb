<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucUsers
    Inherits GlobalShared.Base.ucBase

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim WindowsuiButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        grpMasterlist = New DevExpress.XtraEditors.GroupControl()
        gridconUsersList = New DevExpress.XtraGrid.GridControl()
        gridviewUsersList = New DevExpress.XtraGrid.Views.Grid.GridView()
        PanelControl2 = New DevExpress.XtraEditors.PanelControl()
        rgFilter = New DevExpress.XtraEditors.RadioGroup()
        LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        grpUserDetails = New DevExpress.XtraEditors.GroupControl()
        btnCopyTempPassword = New DevExpress.XtraEditors.SimpleButton()
        lblStatusMessage = New DevExpress.XtraEditors.LabelControl()
        lblTempPassword = New DevExpress.XtraEditors.LabelControl()
        btnToggleActive = New DevExpress.XtraEditors.SimpleButton()
        btnResetPassword = New DevExpress.XtraEditors.SimpleButton()
        pnlModuleAccess = New Panel()
        lblModuleAccessHeader = New DevExpress.XtraEditors.LabelControl()
        chkIsAdmin = New DevExpress.XtraEditors.CheckEdit()
        lueEmployee = New DevExpress.XtraEditors.LookUpEdit()
        lblEmployee = New DevExpress.XtraEditors.LabelControl()
        txtUsername = New DevExpress.XtraEditors.TextEdit()
        lblUsername = New DevExpress.XtraEditors.LabelControl()
        CType(grpMasterlist, ComponentModel.ISupportInitialize).BeginInit()
        grpMasterlist.SuspendLayout()
        CType(gridconUsersList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewUsersList, ComponentModel.ISupportInitialize).BeginInit()
        CType(PanelControl2, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl2.SuspendLayout()
        CType(rgFilter.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpUserDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpUserDetails.SuspendLayout()
        CType(chkIsAdmin.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lueEmployee.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtUsername.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' grpMasterlist
        ' 
        grpMasterlist.Appearance.Options.UseFont = True
        grpMasterlist.AppearanceCaption.Font = New Font("Segoe UI", 10.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpMasterlist.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpMasterlist.AppearanceCaption.Options.UseFont = True
        grpMasterlist.Controls.Add(gridconUsersList)
        grpMasterlist.Controls.Add(PanelControl2)
        grpMasterlist.Controls.Add(PanelControl1)
        grpMasterlist.Dock = DockStyle.Right
        grpMasterlist.Location = New Point(763, 4)
        grpMasterlist.Margin = New Padding(3, 2, 3, 2)
        grpMasterlist.Name = "grpMasterlist"
        grpMasterlist.Size = New Size(340, 558)
        grpMasterlist.TabIndex = 1
        grpMasterlist.Text = " USER ACCOUNTS"
        ' 
        ' gridconUsersList
        ' 
        gridconUsersList.Dock = DockStyle.Fill
        gridconUsersList.Location = New Point(2, 87)
        gridconUsersList.MainView = gridviewUsersList
        gridconUsersList.Margin = New Padding(3, 2, 3, 2)
        gridconUsersList.Name = "gridconUsersList"
        gridconUsersList.Size = New Size(336, 445)
        gridconUsersList.TabIndex = 3
        gridconUsersList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewUsersList})
        ' 
        ' gridviewUsersList
        ' 
        gridviewUsersList.GridControl = gridconUsersList
        gridviewUsersList.Name = "gridviewUsersList"
        gridviewUsersList.OptionsPrint.PrintFilterInfo = True
        ' 
        ' PanelControl2
        ' 
        PanelControl2.Controls.Add(rgFilter)
        PanelControl2.Controls.Add(LabelControl1)
        PanelControl2.Dock = DockStyle.Bottom
        PanelControl2.Location = New Point(2, 532)
        PanelControl2.Margin = New Padding(3, 2, 3, 2)
        PanelControl2.Name = "PanelControl2"
        PanelControl2.Size = New Size(336, 24)
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
        rgFilter.Properties.Appearance.Options.UseBackColor = True
        rgFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgFilter.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Far
        rgFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Active"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "Inactive"), New DevExpress.XtraEditors.Controls.RadioGroupItem(Nothing, "All")})
        rgFilter.Properties.Padding = New Padding(0)
        rgFilter.Size = New Size(285, 20)
        rgFilter.TabIndex = 0
        rgFilter.TabStop = False
        ' 
        ' LabelControl1
        ' 
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
        PanelControl1.Size = New Size(336, 64)
        PanelControl1.TabIndex = 1
        ' 
        ' wbpMainCommands
        ' 
        wbpMainCommands.ButtonInterval = 5
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New User", -1, True, Nothing, True, False, True, "New", -1, True), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(Nothing, True, -1, True), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit User", -1, True, Nothing, True, False, True, "Edit  ", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Deactivate", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Deactivate User", -1, True, Nothing, True, False, True, "Delete", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Fill
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(5, 4)
        wbpMainCommands.Margin = New Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(326, 56)
        wbpMainCommands.TabIndex = 1
        wbpMainCommands.Text = "Commands"
        wbpMainCommands.WrapButtons = True
        ' 
        ' grpUserDetails
        ' 
        grpUserDetails.Appearance.Options.UseFont = True
        grpUserDetails.AppearanceCaption.Font = New Font("Segoe UI", 10.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpUserDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpUserDetails.AppearanceCaption.Options.UseFont = True
        grpUserDetails.Controls.Add(btnCopyTempPassword)
        grpUserDetails.Controls.Add(lblStatusMessage)
        grpUserDetails.Controls.Add(lblTempPassword)
        grpUserDetails.Controls.Add(btnToggleActive)
        grpUserDetails.Controls.Add(btnResetPassword)
        grpUserDetails.Controls.Add(pnlModuleAccess)
        grpUserDetails.Controls.Add(lblModuleAccessHeader)
        grpUserDetails.Controls.Add(chkIsAdmin)
        grpUserDetails.Controls.Add(lueEmployee)
        grpUserDetails.Controls.Add(lblEmployee)
        grpUserDetails.Controls.Add(txtUsername)
        grpUserDetails.Controls.Add(lblUsername)
        grpUserDetails.Dock = DockStyle.Fill
        grpUserDetails.Location = New Point(4, 4)
        grpUserDetails.Margin = New Padding(3, 2, 3, 2)
        grpUserDetails.Name = "grpUserDetails"
        grpUserDetails.Padding = New Padding(17, 8, 8, 8)
        grpUserDetails.Size = New Size(759, 558)
        grpUserDetails.TabIndex = 0
        grpUserDetails.Text = " USER DETAILS"
        ' 
        ' btnCopyTempPassword
        ' 
        btnCopyTempPassword.Appearance.FontSizeDelta = -1
        btnCopyTempPassword.Appearance.ForeColor = Color.SeaGreen
        btnCopyTempPassword.Appearance.Options.UseFont = True
        btnCopyTempPassword.Appearance.Options.UseForeColor = True
        btnCopyTempPassword.Location = New Point(352, 426)
        btnCopyTempPassword.Name = "btnCopyTempPassword"
        btnCopyTempPassword.Size = New Size(90, 32)
        btnCopyTempPassword.TabIndex = 11
        btnCopyTempPassword.Text = "Copy to" & vbCrLf & "Clipboard"
        btnCopyTempPassword.Visible = False
        ' 
        ' lblStatusMessage
        ' 
        lblStatusMessage.Appearance.ForeColor = Color.Firebrick
        lblStatusMessage.Appearance.Options.UseForeColor = True
        lblStatusMessage.Location = New Point(22, 510)
        lblStatusMessage.Name = "lblStatusMessage"
        lblStatusMessage.Size = New Size(0, 13)
        lblStatusMessage.TabIndex = 10
        ' 
        ' lblTempPassword
        ' 
        lblTempPassword.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        lblTempPassword.Appearance.ForeColor = Color.SeaGreen
        lblTempPassword.Appearance.Options.UseFont = True
        lblTempPassword.Appearance.Options.UseForeColor = True
        lblTempPassword.Location = New Point(22, 466)
        lblTempPassword.Name = "lblTempPassword"
        lblTempPassword.Size = New Size(0, 17)
        lblTempPassword.TabIndex = 9
        lblTempPassword.Visible = False
        ' 
        ' btnToggleActive
        ' 
        btnToggleActive.Location = New Point(172, 426)
        btnToggleActive.Name = "btnToggleActive"
        btnToggleActive.Size = New Size(150, 32)
        btnToggleActive.TabIndex = 8
        btnToggleActive.Text = "Deactivate"
        ' 
        ' btnResetPassword
        ' 
        btnResetPassword.Location = New Point(22, 426)
        btnResetPassword.Name = "btnResetPassword"
        btnResetPassword.Size = New Size(140, 32)
        btnResetPassword.TabIndex = 7
        btnResetPassword.Text = "Reset Password"
        ' 
        ' pnlModuleAccess
        ' 
        pnlModuleAccess.AutoScroll = True
        pnlModuleAccess.BorderStyle = BorderStyle.FixedSingle
        pnlModuleAccess.Location = New Point(22, 206)
        pnlModuleAccess.Name = "pnlModuleAccess"
        pnlModuleAccess.Size = New Size(420, 210)
        pnlModuleAccess.TabIndex = 6
        ' 
        ' lblModuleAccessHeader
        ' 
        lblModuleAccessHeader.Appearance.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold)
        lblModuleAccessHeader.Appearance.Options.UseFont = True
        lblModuleAccessHeader.Location = New Point(22, 186)
        lblModuleAccessHeader.Name = "lblModuleAccessHeader"
        lblModuleAccessHeader.Size = New Size(90, 17)
        lblModuleAccessHeader.TabIndex = 5
        lblModuleAccessHeader.Text = "Module Access"
        ' 
        ' chkIsAdmin
        ' 
        chkIsAdmin.Location = New Point(22, 150)
        chkIsAdmin.Name = "chkIsAdmin"
        chkIsAdmin.Properties.Caption = "Administrator Access (full access sa lahat ng modules)"
        chkIsAdmin.Size = New Size(340, 20)
        chkIsAdmin.TabIndex = 4
        ' 
        ' lueEmployee
        ' 
        lueEmployee.Location = New Point(22, 116)
        lueEmployee.Name = "lueEmployee"
        lueEmployee.Properties.NullText = "Select an employee..."
        lueEmployee.Size = New Size(320, 20)
        lueEmployee.TabIndex = 3
        ' 
        ' lblEmployee
        ' 
        lblEmployee.Location = New Point(22, 98)
        lblEmployee.Name = "lblEmployee"
        lblEmployee.Size = New Size(46, 13)
        lblEmployee.TabIndex = 2
        lblEmployee.Text = "Employee"
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(22, 64)
        txtUsername.Name = "txtUsername"
        txtUsername.Properties.NullValuePrompt = "e.g. jdelacruz"
        txtUsername.Size = New Size(320, 20)
        txtUsername.TabIndex = 1
        ' 
        ' lblUsername
        ' 
        lblUsername.Location = New Point(22, 46)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(48, 13)
        lblUsername.TabIndex = 0
        lblUsername.Text = "Username"
        ' 
        ' ucUsers
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpUserDetails)
        Controls.Add(grpMasterlist)
        Name = "ucUsers"
        Padding = New Padding(4)
        Size = New Size(1107, 566)
        CType(grpMasterlist, ComponentModel.ISupportInitialize).EndInit()
        grpMasterlist.ResumeLayout(False)
        CType(gridconUsersList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewUsersList, ComponentModel.ISupportInitialize).EndInit()
        CType(PanelControl2, ComponentModel.ISupportInitialize).EndInit()
        PanelControl2.ResumeLayout(False)
        CType(rgFilter.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        CType(grpUserDetails, ComponentModel.ISupportInitialize).EndInit()
        grpUserDetails.ResumeLayout(False)
        grpUserDetails.PerformLayout()
        CType(chkIsAdmin.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lueEmployee.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtUsername.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents grpMasterlist As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconUsersList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewUsersList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents rgFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents grpUserDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblUsername As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtUsername As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueEmployee As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents chkIsAdmin As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblModuleAccessHeader As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlModuleAccess As System.Windows.Forms.Panel
    Friend WithEvents lblTempPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblStatusMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnResetPassword As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnToggleActive As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCopyTempPassword As DevExpress.XtraEditors.SimpleButton

End Class