<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmChangePassword
    Inherits DevExpress.XtraEditors.XtraForm

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        lblCurrentPassword = New DevExpress.XtraEditors.LabelControl()
        txtCurrentPassword = New DevExpress.XtraEditors.TextEdit()
        lblNewPassword = New DevExpress.XtraEditors.LabelControl()
        txtNewPassword = New DevExpress.XtraEditors.TextEdit()
        lblConfirmPassword = New DevExpress.XtraEditors.LabelControl()
        txtConfirmPassword = New DevExpress.XtraEditors.TextEdit()
        lblErrorMessage = New DevExpress.XtraEditors.LabelControl()
        btnChangePassword = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(txtCurrentPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtNewPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtConfirmPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblCurrentPassword
        ' 
        lblCurrentPassword.Location = New Point(24, 20)
        lblCurrentPassword.Name = "lblCurrentPassword"
        lblCurrentPassword.Size = New Size(86, 13)
        lblCurrentPassword.TabIndex = 0
        lblCurrentPassword.Text = "Current Password"
        ' 
        ' txtCurrentPassword
        ' 
        txtCurrentPassword.Location = New Point(24, 38)
        txtCurrentPassword.Name = "txtCurrentPassword"
        txtCurrentPassword.Properties.PasswordChar = "●"c
        txtCurrentPassword.Size = New Size(300, 20)
        txtCurrentPassword.TabIndex = 1
        ' 
        ' lblNewPassword
        ' 
        lblNewPassword.Location = New Point(24, 72)
        lblNewPassword.Name = "lblNewPassword"
        lblNewPassword.Size = New Size(70, 13)
        lblNewPassword.TabIndex = 2
        lblNewPassword.Text = "New Password"
        ' 
        ' txtNewPassword
        ' 
        txtNewPassword.Location = New Point(24, 90)
        txtNewPassword.Name = "txtNewPassword"
        txtNewPassword.Properties.PasswordChar = "●"c
        txtNewPassword.Size = New Size(300, 20)
        txtNewPassword.TabIndex = 3
        ' 
        ' lblConfirmPassword
        ' 
        lblConfirmPassword.Location = New Point(24, 124)
        lblConfirmPassword.Name = "lblConfirmPassword"
        lblConfirmPassword.Size = New Size(110, 13)
        lblConfirmPassword.TabIndex = 4
        lblConfirmPassword.Text = "Confirm New Password"
        ' 
        ' txtConfirmPassword
        ' 
        txtConfirmPassword.Location = New Point(24, 142)
        txtConfirmPassword.Name = "txtConfirmPassword"
        txtConfirmPassword.Properties.PasswordChar = "●"c
        txtConfirmPassword.Size = New Size(300, 20)
        txtConfirmPassword.TabIndex = 5
        ' 
        ' lblErrorMessage
        ' 
        lblErrorMessage.Appearance.ForeColor = Color.Firebrick
        lblErrorMessage.Appearance.Options.UseForeColor = True
        lblErrorMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblErrorMessage.Location = New Point(24, 176)
        lblErrorMessage.Name = "lblErrorMessage"
        lblErrorMessage.Size = New Size(300, 26)
        lblErrorMessage.TabIndex = 6
        ' 
        ' btnChangePassword
        ' 
        btnChangePassword.Location = New Point(24, 210)
        btnChangePassword.Name = "btnChangePassword"
        btnChangePassword.Size = New Size(145, 32)
        btnChangePassword.TabIndex = 7
        btnChangePassword.Text = "Change Password"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(179, 210)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(145, 32)
        btnCancel.TabIndex = 8
        btnCancel.Text = "Cancel"
        ' 
        ' frmChangePassword
        ' 
        AutoScaleMode = AutoScaleMode.None
        CancelButton = btnCancel
        ClientSize = New Size(350, 264)
        Controls.Add(btnCancel)
        Controls.Add(btnChangePassword)
        Controls.Add(lblErrorMessage)
        Controls.Add(txtConfirmPassword)
        Controls.Add(lblConfirmPassword)
        Controls.Add(txtNewPassword)
        Controls.Add(lblNewPassword)
        Controls.Add(txtCurrentPassword)
        Controls.Add(lblCurrentPassword)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmChangePassword"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Change Password"
        CType(txtCurrentPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtNewPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtConfirmPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblCurrentPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCurrentPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblNewPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtNewPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblConfirmPassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtConfirmPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblErrorMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnChangePassword As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class