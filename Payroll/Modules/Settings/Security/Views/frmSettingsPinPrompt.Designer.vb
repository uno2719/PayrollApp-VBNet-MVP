<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSettingsPinPrompt
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
        txtPin = New DevExpress.XtraEditors.TextEdit()
        btnOK = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(txtPin.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblPrompt
        ' 
        lblPrompt.Location = New Point(24, 22)
        lblPrompt.Name = "lblPrompt"
        lblPrompt.Size = New Size(92, 13)
        lblPrompt.TabIndex = 0
        lblPrompt.Text = "Enter Settings PIN:"
        ' 
        ' txtPin
        ' 
        txtPin.Location = New Point(24, 42)
        txtPin.Name = "txtPin"
        txtPin.Size = New Size(272, 20)
        txtPin.TabIndex = 1
        ' 
        ' btnOK
        ' 
        btnOK.Location = New Point(129, 78)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(80, 30)
        btnOK.TabIndex = 2
        btnOK.Text = "OK"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(216, 78)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 30)
        btnCancel.TabIndex = 3
        btnCancel.Text = "Cancel"
        ' 
        ' frmSettingsPinPrompt
        ' 
        AcceptButton = btnOK
        AutoScaleMode = AutoScaleMode.None
        CancelButton = btnCancel
        ClientSize = New Size(320, 130)
        Controls.Add(btnCancel)
        Controls.Add(btnOK)
        Controls.Add(txtPin)
        Controls.Add(lblPrompt)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSettingsPinPrompt"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Application Settings"
        CType(txtPin.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblPrompt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPin As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class