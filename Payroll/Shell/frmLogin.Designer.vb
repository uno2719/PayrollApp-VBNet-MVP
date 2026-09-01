<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
        Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
        Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        panelBranding = New DevExpress.XtraEditors.PanelControl()
        panelForm = New DevExpress.XtraEditors.PanelControl()
        lblErrorMessage = New DevExpress.XtraEditors.LabelControl()
        lblVersion = New DevExpress.XtraEditors.LabelControl()
        btnLogin = New DevExpress.XtraEditors.SimpleButton()
        lnkForgotPassword = New DevExpress.XtraEditors.HyperlinkLabelControl()
        chkRememberMe = New DevExpress.XtraEditors.CheckEdit()
        txtPassword = New DevExpress.XtraEditors.ButtonEdit()
        lblPassword = New DevExpress.XtraEditors.LabelControl()
        txtUsername = New DevExpress.XtraEditors.TextEdit()
        lblUsername = New DevExpress.XtraEditors.LabelControl()
        lblSubtitle = New DevExpress.XtraEditors.LabelControl()
        lblSignIn = New DevExpress.XtraEditors.LabelControl()
        CType(panelBranding, ComponentModel.ISupportInitialize).BeginInit()
        CType(panelForm, ComponentModel.ISupportInitialize).BeginInit()
        panelForm.SuspendLayout()
        CType(chkRememberMe.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtUsername.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' panelBranding
        ' 
        panelBranding.Appearance.BackColor = Color.Gray
        panelBranding.Appearance.Options.UseBackColor = True
        panelBranding.ContentImage = My.Resources.Resources.LoginBanner1
        panelBranding.Dock = DockStyle.Left
        panelBranding.Location = New Point(0, 0)
        panelBranding.Margin = New Padding(0)
        panelBranding.Name = "panelBranding"
        panelBranding.Size = New Size(270, 440)
        panelBranding.TabIndex = 0
        ' 
        ' panelForm
        ' 
        panelForm.Controls.Add(lblErrorMessage)
        panelForm.Controls.Add(lblVersion)
        panelForm.Controls.Add(btnLogin)
        panelForm.Controls.Add(lnkForgotPassword)
        panelForm.Controls.Add(chkRememberMe)
        panelForm.Controls.Add(txtPassword)
        panelForm.Controls.Add(lblPassword)
        panelForm.Controls.Add(txtUsername)
        panelForm.Controls.Add(lblUsername)
        panelForm.Controls.Add(lblSubtitle)
        panelForm.Controls.Add(lblSignIn)
        panelForm.Dock = DockStyle.Fill
        panelForm.Location = New Point(270, 0)
        panelForm.Name = "panelForm"
        panelForm.Size = New Size(370, 440)
        panelForm.TabIndex = 1
        ' 
        ' lblErrorMessage
        ' 
        lblErrorMessage.Appearance.ForeColor = Color.Firebrick
        lblErrorMessage.Appearance.Options.UseForeColor = True
        lblErrorMessage.Location = New Point(40, 305)
        lblErrorMessage.Name = "lblErrorMessage"
        lblErrorMessage.Size = New Size(0, 13)
        lblErrorMessage.TabIndex = 9
        lblErrorMessage.Visible = False
        ' 
        ' lblVersion
        ' 
        lblVersion.Appearance.ForeColor = Color.Silver
        lblVersion.Appearance.Options.UseForeColor = True
        lblVersion.Appearance.Options.UseTextOptions = True
        lblVersion.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        lblVersion.Location = New Point(40, 350)
        lblVersion.Name = "lblVersion"
        lblVersion.Size = New Size(129, 13)
        lblVersion.TabIndex = 10
        lblVersion.Text = "v1.0.0 — Internal use only"
        ' 
        ' btnLogin
        ' 
        btnLogin.Location = New Point(40, 263)
        btnLogin.Name = "btnLogin"
        btnLogin.Size = New Size(290, 36)
        btnLogin.TabIndex = 8
        btnLogin.Text = "Log in"
        ' 
        ' lnkForgotPassword
        ' 
        lnkForgotPassword.Location = New Point(230, 229)
        lnkForgotPassword.Name = "lnkForgotPassword"
        lnkForgotPassword.Size = New Size(86, 13)
        lnkForgotPassword.TabIndex = 7
        lnkForgotPassword.Text = "Forgot password?"
        ' 
        ' chkRememberMe
        ' 
        chkRememberMe.Location = New Point(40, 227)
        chkRememberMe.Name = "chkRememberMe"
        chkRememberMe.Properties.Caption = "Remember me"
        chkRememberMe.Size = New Size(120, 20)
        chkRememberMe.TabIndex = 6
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(40, 195)
        txtPassword.Name = "txtPassword"
        EditorButtonImageOptions1.Image = My.Resources.Resources.icon_hidePW
        txtPassword.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.Default)})
        txtPassword.Properties.PasswordChar = "●"c
        txtPassword.Size = New Size(290, 20)
        txtPassword.TabIndex = 5
        ' 
        ' lblPassword
        ' 
        lblPassword.Location = New Point(40, 177)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(46, 13)
        lblPassword.TabIndex = 4
        lblPassword.Text = "Password"
        ' 
        ' txtUsername
        ' 
        txtUsername.Location = New Point(40, 143)
        txtUsername.Name = "txtUsername"
        txtUsername.Properties.NullValuePrompt = "e.g. jdelacruz"
        txtUsername.Size = New Size(290, 20)
        txtUsername.TabIndex = 3
        ' 
        ' lblUsername
        ' 
        lblUsername.Location = New Point(40, 125)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New Size(48, 13)
        lblUsername.TabIndex = 2
        lblUsername.Text = "Username"
        ' 
        ' lblSubtitle
        ' 
        lblSubtitle.Appearance.ForeColor = Color.Gray
        lblSubtitle.Appearance.Options.UseForeColor = True
        lblSubtitle.Location = New Point(40, 85)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(163, 13)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Enter your credentials to continue"
        ' 
        ' lblSignIn
        ' 
        lblSignIn.Appearance.Font = New Font("Segoe UI", 15F)
        lblSignIn.Appearance.Options.UseFont = True
        lblSignIn.Location = New Point(40, 55)
        lblSignIn.Name = "lblSignIn"
        lblSignIn.Size = New Size(60, 28)
        lblSignIn.TabIndex = 0
        lblSignIn.Text = "Sign in"
        ' 
        ' frmLogin
        ' 
        AcceptButton = btnLogin
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.None
        ClientSize = New Size(640, 440)
        Controls.Add(panelForm)
        Controls.Add(panelBranding)
        FormBorderStyle = FormBorderStyle.FixedSingle
        IconOptions.Icon = CType(resources.GetObject("frmLogin.IconOptions.Icon"), Icon)
        IconOptions.Image = My.Resources.Resources.App_Logo
        MaximizeBox = False
        Name = "frmLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Pacsports Payroll — Login"
        CType(panelBranding, ComponentModel.ISupportInitialize).EndInit()
        CType(panelForm, ComponentModel.ISupportInitialize).EndInit()
        panelForm.ResumeLayout(False)
        panelForm.PerformLayout()
        CType(chkRememberMe.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtUsername.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents panelBranding As DevExpress.XtraEditors.PanelControl
    Friend WithEvents panelForm As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblSignIn As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSubtitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblUsername As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtUsername As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPassword As DevExpress.XtraEditors.ButtonEdit
    Friend WithEvents chkRememberMe As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lnkForgotPassword As DevExpress.XtraEditors.HyperlinkLabelControl
    Friend WithEvents btnLogin As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblVersion As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblErrorMessage As DevExpress.XtraEditors.LabelControl

End Class