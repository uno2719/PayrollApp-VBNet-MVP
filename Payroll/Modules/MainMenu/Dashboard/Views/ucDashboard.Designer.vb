<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucDashboard
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
        LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        SuspendLayout()
        ' 
        ' LabelControl1
        ' 
        LabelControl1.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LabelControl1.Appearance.Options.UseFont = True
        LabelControl1.Location = New Point(16, 18)
        LabelControl1.Name = "LabelControl1"
        LabelControl1.Size = New Size(129, 25)
        LabelControl1.TabIndex = 0
        LabelControl1.Text = "DASHBOARD"
        ' 
        ' ucDashboard
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(7F, 16F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(LabelControl1)
        Margin = New Padding(4)
        Name = "ucDashboard"
        Size = New Size(1050, 574)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl

End Class
