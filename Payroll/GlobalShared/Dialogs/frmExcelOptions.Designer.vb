<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmExcelOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmExcelOptions))
        lblPrompt = New DevExpress.XtraEditors.LabelControl()
        btnImport = New DevExpress.XtraEditors.SimpleButton()
        btnExport = New DevExpress.XtraEditors.SimpleButton()
        SuspendLayout()
        ' 
        ' lblPrompt
        ' 
        lblPrompt.Location = New Point(24, 22)
        lblPrompt.Name = "lblPrompt"
        lblPrompt.Size = New Size(129, 13)
        lblPrompt.TabIndex = 0
        lblPrompt.Text = "What would you like to do?"
        ' 
        ' btnImport
        ' 
        btnImport.Location = New Point(24, 60)
        btnImport.Name = "btnImport"
        btnImport.Size = New Size(130, 34)
        btnImport.TabIndex = 1
        btnImport.Text = "Import from Excel"
        ' 
        ' btnExport
        ' 
        btnExport.Location = New Point(166, 60)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(130, 34)
        btnExport.TabIndex = 2
        btnExport.Text = "Export Template"
        ' 
        ' frmExcelOptions
        ' 
        AutoScaleMode = AutoScaleMode.None
        ClientSize = New Size(320, 116)
        Controls.Add(btnExport)
        Controls.Add(btnImport)
        Controls.Add(lblPrompt)
        FormBorderStyle = FormBorderStyle.FixedDialog
        IconOptions.SvgImage = CType(resources.GetObject("frmExcelOptions.IconOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmExcelOptions"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Excel Options"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblPrompt As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnImport As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExport As DevExpress.XtraEditors.SimpleButton

End Class