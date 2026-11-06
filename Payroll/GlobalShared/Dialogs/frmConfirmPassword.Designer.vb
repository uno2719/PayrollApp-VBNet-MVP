<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConfirmPassword
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
        lblMessage = New DevExpress.XtraEditors.LabelControl()
        txtPassword = New DevExpress.XtraEditors.TextEdit()
        lblError = New DevExpress.XtraEditors.LabelControl()
        btnConfirm = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(txtPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblMessage
        ' 
        lblMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblMessage.Location = New Point(24, 20)
        lblMessage.Name = "lblMessage"
        lblMessage.Size = New Size(352, 40)
        lblMessage.TabIndex = 0
        lblMessage.Text = "Enter your password to confirm."
        ' 
        ' txtPassword
        ' 
        txtPassword.Location = New Point(24, 68)
        txtPassword.Name = "txtPassword"
        txtPassword.Size = New Size(352, 20)
        txtPassword.TabIndex = 1
        ' 
        ' lblError
        ' 
        lblError.Appearance.ForeColor = Color.Firebrick
        lblError.Appearance.Options.UseForeColor = True
        lblError.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblError.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblError.Location = New Point(24, 96)
        lblError.Name = "lblError"
        lblError.Size = New Size(352, 32)
        lblError.TabIndex = 4
        ' 
        ' btnConfirm
        ' 
        btnConfirm.Location = New Point(206, 138)
        btnConfirm.Name = "btnConfirm"
        btnConfirm.Size = New Size(80, 30)
        btnConfirm.TabIndex = 2
        btnConfirm.Text = "Delete"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(296, 138)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 30)
        btnCancel.TabIndex = 3
        btnCancel.Text = "Cancel"
        ' 
        ' frmConfirmPassword
        ' 
        AcceptButton = btnConfirm
        AutoScaleMode = AutoScaleMode.None
        CancelButton = btnCancel
        ClientSize = New Size(400, 184)
        Controls.Add(lblError)
        Controls.Add(btnCancel)
        Controls.Add(btnConfirm)
        Controls.Add(txtPassword)
        Controls.Add(lblMessage)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmConfirmPassword"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Confirm Delete - Password Required"
        CType(txtPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblMessage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPassword As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblError As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnConfirm As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
