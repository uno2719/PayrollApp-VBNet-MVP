<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPickUser
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

        lblPrompt = New DevExpress.XtraEditors.LabelControl()
        cboUsers = New DevExpress.XtraEditors.ComboBoxEdit()
        btnOK = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()

        CType(cboUsers.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' lblPrompt
        '
        lblPrompt.Location = New Point(16, 16)
        lblPrompt.Name = "lblPrompt"
        lblPrompt.Size = New Size(130, 13)
        lblPrompt.TabIndex = 0
        lblPrompt.Text = "Copy module access from:"
        '
        ' cboUsers
        '
        cboUsers.Location = New Point(16, 36)
        cboUsers.Name = "cboUsers"
        cboUsers.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboUsers.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboUsers.Size = New Size(360, 20)
        cboUsers.TabIndex = 1
        '
        ' btnOK
        '
        btnOK.DialogResult = DialogResult.OK
        btnOK.Location = New Point(216, 68)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(75, 26)
        btnOK.TabIndex = 2
        btnOK.Text = "OK"
        '
        ' btnCancel
        '
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(301, 68)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(75, 26)
        btnCancel.TabIndex = 3
        btnCancel.Text = "Cancel"
        '
        ' frmPickUser
        '
        AcceptButton = btnOK
        CancelButton = btnCancel
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(392, 106)
        Controls.Add(btnCancel)
        Controls.Add(btnOK)
        Controls.Add(cboUsers)
        Controls.Add(lblPrompt)
        FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmPickUser"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Copy Access From"
        CType(cboUsers.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblPrompt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboUsers As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class