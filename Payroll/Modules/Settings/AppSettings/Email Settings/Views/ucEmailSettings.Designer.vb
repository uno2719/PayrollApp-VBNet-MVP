' File: Modules/Settings/AppSettings/EmailSettings/Views/ucEmailSettings.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucEmailSettings
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
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        btnSave = New DevExpress.XtraEditors.SimpleButton()
        grpSmtp = New DevExpress.XtraEditors.GroupControl()
        chkUseSsl = New DevExpress.XtraEditors.CheckEdit()
        lblPasswordHint = New DevExpress.XtraEditors.LabelControl()
        txtSmtpPassword = New DevExpress.XtraEditors.TextEdit()
        lblSmtpPassword = New DevExpress.XtraEditors.LabelControl()
        txtSmtpUsername = New DevExpress.XtraEditors.TextEdit()
        lblSmtpUsername = New DevExpress.XtraEditors.LabelControl()
        txtSmtpPort = New DevExpress.XtraEditors.TextEdit()
        lblSmtpPort = New DevExpress.XtraEditors.LabelControl()
        txtSmtpHost = New DevExpress.XtraEditors.TextEdit()
        lblSmtpHost = New DevExpress.XtraEditors.LabelControl()
        grpTest = New DevExpress.XtraEditors.GroupControl()
        lblTestResult = New DevExpress.XtraEditors.LabelControl()
        btnSendTest = New DevExpress.XtraEditors.SimpleButton()
        txtTestRecipient = New DevExpress.XtraEditors.TextEdit()
        lblTestRecipient = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpSmtp, ComponentModel.ISupportInitialize).BeginInit()
        grpSmtp.SuspendLayout()
        CType(chkUseSsl.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSmtpPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSmtpUsername.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSmtpPort.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSmtpHost.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpTest, ComponentModel.ISupportInitialize).BeginInit()
        grpTest.SuspendLayout()
        CType(txtTestRecipient.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(btnSave)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Size = New Size(1069, 56)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(133, 30)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "EMAIL SETUP"
        ' 
        ' btnSave
        ' 
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.Location = New Point(969, 12)
        btnSave.Margin = New Padding(3, 2, 3, 2)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(90, 32)
        btnSave.TabIndex = 1
        btnSave.Text = "Save"
        ' 
        ' grpSmtp
        ' 
        grpSmtp.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpSmtp.Appearance.Options.UseFont = True
        grpSmtp.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpSmtp.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpSmtp.AppearanceCaption.Options.UseFont = True
        grpSmtp.Controls.Add(chkUseSsl)
        grpSmtp.Controls.Add(lblPasswordHint)
        grpSmtp.Controls.Add(txtSmtpPassword)
        grpSmtp.Controls.Add(lblSmtpPassword)
        grpSmtp.Controls.Add(txtSmtpUsername)
        grpSmtp.Controls.Add(lblSmtpUsername)
        grpSmtp.Controls.Add(txtSmtpPort)
        grpSmtp.Controls.Add(lblSmtpPort)
        grpSmtp.Controls.Add(txtSmtpHost)
        grpSmtp.Controls.Add(lblSmtpHost)
        grpSmtp.Location = New Point(4, 66)
        grpSmtp.Margin = New Padding(3, 2, 3, 2)
        grpSmtp.Name = "grpSmtp"
        grpSmtp.Size = New Size(1069, 160)
        grpSmtp.TabIndex = 1
        grpSmtp.Text = " SMTP SERVER"
        ' 
        ' chkUseSsl
        ' 
        chkUseSsl.Location = New Point(22, 128)
        chkUseSsl.Margin = New Padding(3, 2, 3, 2)
        chkUseSsl.Name = "chkUseSsl"
        chkUseSsl.Properties.Caption = "Use SSL"
        chkUseSsl.Size = New Size(120, 20)
        chkUseSsl.TabIndex = 9
        ' 
        ' lblPasswordHint
        ' 
        lblPasswordHint.Appearance.Font = New Font("Segoe UI", 7.5F, FontStyle.Italic)
        lblPasswordHint.Appearance.ForeColor = Color.Gray
        lblPasswordHint.Appearance.Options.UseFont = True
        lblPasswordHint.Appearance.Options.UseForeColor = True
        lblPasswordHint.Location = New Point(280, 125)
        lblPasswordHint.Name = "lblPasswordHint"
        lblPasswordHint.Size = New Size(0, 12)
        lblPasswordHint.TabIndex = 8
        ' 
        ' txtSmtpPassword
        ' 
        txtSmtpPassword.Location = New Point(280, 102)
        txtSmtpPassword.Margin = New Padding(3, 2, 3, 2)
        txtSmtpPassword.Name = "txtSmtpPassword"
        txtSmtpPassword.Properties.PasswordChar = "●"c
        txtSmtpPassword.Properties.UseSystemPasswordChar = True
        txtSmtpPassword.Size = New Size(230, 20)
        txtSmtpPassword.TabIndex = 7
        ' 
        ' lblSmtpPassword
        ' 
        lblSmtpPassword.Location = New Point(280, 84)
        lblSmtpPassword.Name = "lblSmtpPassword"
        lblSmtpPassword.Size = New Size(46, 13)
        lblSmtpPassword.TabIndex = 6
        lblSmtpPassword.Text = "Password"
        ' 
        ' txtSmtpUsername
        ' 
        txtSmtpUsername.Location = New Point(24, 102)
        txtSmtpUsername.Margin = New Padding(3, 2, 3, 2)
        txtSmtpUsername.Name = "txtSmtpUsername"
        txtSmtpUsername.Size = New Size(230, 20)
        txtSmtpUsername.TabIndex = 5
        ' 
        ' lblSmtpUsername
        ' 
        lblSmtpUsername.Location = New Point(24, 84)
        lblSmtpUsername.Name = "lblSmtpUsername"
        lblSmtpUsername.Size = New Size(48, 13)
        lblSmtpUsername.TabIndex = 4
        lblSmtpUsername.Text = "Username"
        ' 
        ' txtSmtpPort
        ' 
        txtSmtpPort.Location = New Point(280, 52)
        txtSmtpPort.Margin = New Padding(3, 2, 3, 2)
        txtSmtpPort.Name = "txtSmtpPort"
        txtSmtpPort.Properties.Mask.EditMask = "n0"
        txtSmtpPort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtSmtpPort.Properties.Mask.UseMaskAsDisplayFormat = True
        txtSmtpPort.Size = New Size(120, 20)
        txtSmtpPort.TabIndex = 3
        ' 
        ' lblSmtpPort
        ' 
        lblSmtpPort.Location = New Point(280, 34)
        lblSmtpPort.Name = "lblSmtpPort"
        lblSmtpPort.Size = New Size(20, 13)
        lblSmtpPort.TabIndex = 2
        lblSmtpPort.Text = "Port"
        ' 
        ' txtSmtpHost
        ' 
        txtSmtpHost.Location = New Point(24, 52)
        txtSmtpHost.Margin = New Padding(3, 2, 3, 2)
        txtSmtpHost.Name = "txtSmtpHost"
        txtSmtpHost.Size = New Size(230, 20)
        txtSmtpHost.TabIndex = 1
        ' 
        ' lblSmtpHost
        ' 
        lblSmtpHost.Location = New Point(24, 34)
        lblSmtpHost.Name = "lblSmtpHost"
        lblSmtpHost.Size = New Size(22, 13)
        lblSmtpHost.TabIndex = 0
        lblSmtpHost.Text = "Host"
        ' 
        ' grpTest
        ' 
        grpTest.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpTest.Appearance.Options.UseFont = True
        grpTest.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpTest.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpTest.AppearanceCaption.Options.UseFont = True
        grpTest.Controls.Add(lblTestResult)
        grpTest.Controls.Add(btnSendTest)
        grpTest.Controls.Add(txtTestRecipient)
        grpTest.Controls.Add(lblTestRecipient)
        grpTest.Location = New Point(4, 232)
        grpTest.Margin = New Padding(3, 2, 3, 2)
        grpTest.Name = "grpTest"
        grpTest.Size = New Size(1069, 120)
        grpTest.TabIndex = 2
        grpTest.Text = " SEND TEST EMAIL"
        ' 
        ' lblTestResult
        ' 
        lblTestResult.Appearance.Font = New Font("Segoe UI", 8.25F)
        lblTestResult.Appearance.Options.UseFont = True
        lblTestResult.Appearance.Options.UseForeColor = True
        lblTestResult.Location = New Point(24, 84)
        lblTestResult.Name = "lblTestResult"
        lblTestResult.Size = New Size(0, 13)
        lblTestResult.TabIndex = 3
        ' 
        ' btnSendTest
        ' 
        btnSendTest.Location = New Point(280, 50)
        btnSendTest.Margin = New Padding(3, 2, 3, 2)
        btnSendTest.Name = "btnSendTest"
        btnSendTest.Size = New Size(110, 24)
        btnSendTest.TabIndex = 2
        btnSendTest.Text = "Send Test"
        ' 
        ' txtTestRecipient
        ' 
        txtTestRecipient.Location = New Point(24, 52)
        txtTestRecipient.Margin = New Padding(3, 2, 3, 2)
        txtTestRecipient.Name = "txtTestRecipient"
        txtTestRecipient.Size = New Size(230, 20)
        txtTestRecipient.TabIndex = 1
        ' 
        ' lblTestRecipient
        ' 
        lblTestRecipient.Location = New Point(24, 34)
        lblTestRecipient.Name = "lblTestRecipient"
        lblTestRecipient.Size = New Size(86, 13)
        lblTestRecipient.TabIndex = 0
        lblTestRecipient.Text = "Send test email to"
        ' 
        ' ucEmailSettings
        ' 
        Controls.Add(grpTest)
        Controls.Add(grpSmtp)
        Controls.Add(PanelControl1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "ucEmailSettings"
        Padding = New Padding(4)
        Size = New Size(1077, 600)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpSmtp, ComponentModel.ISupportInitialize).EndInit()
        grpSmtp.ResumeLayout(False)
        grpSmtp.PerformLayout()
        CType(chkUseSsl.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSmtpPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSmtpUsername.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSmtpPort.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSmtpHost.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpTest, ComponentModel.ISupportInitialize).EndInit()
        grpTest.ResumeLayout(False)
        grpTest.PerformLayout()
        CType(txtTestRecipient.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton

    Friend WithEvents grpSmtp As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblSmtpHost As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSmtpHost As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSmtpPort As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSmtpPort As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSmtpUsername As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSmtpUsername As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSmtpPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSmtpPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPasswordHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkUseSsl As DevExpress.XtraEditors.CheckEdit

    Friend WithEvents grpTest As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblTestRecipient As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTestRecipient As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnSendTest As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblTestResult As DevExpress.XtraEditors.LabelControl

End Class
