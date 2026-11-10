<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEmployeeHistory
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
        lblEmployee = New DevExpress.XtraEditors.LabelControl()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        gridHistory = New DevExpress.XtraGrid.GridControl()
        gridviewHistory = New DevExpress.XtraGrid.Views.Grid.GridView()
        btnClose = New DevExpress.XtraEditors.SimpleButton()
        CType(gridHistory, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewHistory, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblEmployee
        ' 
        lblEmployee.Appearance.Font = New Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblEmployee.Appearance.Options.UseFont = True
        lblEmployee.Location = New Point(16, 14)
        lblEmployee.Name = "lblEmployee"
        lblEmployee.Size = New Size(13, 25)
        lblEmployee.TabIndex = 0
        lblEmployee.Text = "..."
        ' 
        ' lblSummary
        ' 
        lblSummary.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSummary.Appearance.Options.UseFont = True
        lblSummary.Location = New Point(16, 48)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(10, 17)
        lblSummary.TabIndex = 1
        lblSummary.Text = ""
        ' 
        ' gridHistory
        ' 
        gridHistory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        gridHistory.Location = New Point(16, 76)
        gridHistory.MainView = gridviewHistory
        gridHistory.Name = "gridHistory"
        gridHistory.Size = New Size(828, 296)
        gridHistory.TabIndex = 2
        gridHistory.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewHistory})
        ' 
        ' gridviewHistory
        ' 
        gridviewHistory.GridControl = gridHistory
        gridviewHistory.Name = "gridviewHistory"
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnClose.DialogResult = DialogResult.Cancel
        btnClose.Location = New Point(744, 384)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(100, 28)
        btnClose.TabIndex = 3
        btnClose.Text = "Close"
        ' 
        ' frmEmployeeHistory
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnClose
        ClientSize = New Size(860, 428)
        Controls.Add(btnClose)
        Controls.Add(gridHistory)
        Controls.Add(lblSummary)
        Controls.Add(lblEmployee)
        MinimizeBox = False
        Name = "frmEmployeeHistory"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Employment History"
        CType(gridHistory, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewHistory, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridHistory As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewHistory As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton

End Class
