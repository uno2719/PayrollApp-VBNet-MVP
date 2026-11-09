<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEmployeeLoansQuickView
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
        lblShow = New DevExpress.XtraEditors.LabelControl()
        cboShow = New DevExpress.XtraEditors.ComboBoxEdit()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        gridLoans = New DevExpress.XtraGrid.GridControl()
        gridviewLoans = New DevExpress.XtraGrid.Views.Grid.GridView()
        btnClose = New DevExpress.XtraEditors.SimpleButton()
        CType(cboShow.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridLoans, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewLoans, ComponentModel.ISupportInitialize).BeginInit()
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
        ' lblShow
        ' 
        lblShow.Location = New Point(16, 56)
        lblShow.Name = "lblShow"
        lblShow.Size = New Size(30, 13)
        lblShow.TabIndex = 1
        lblShow.Text = "Show:"
        ' 
        ' cboShow
        ' 
        cboShow.Location = New Point(56, 52)
        cboShow.Name = "cboShow"
        cboShow.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboShow.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboShow.Size = New Size(130, 20)
        cboShow.TabIndex = 2
        ' 
        ' lblSummary
        ' 
        lblSummary.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSummary.Appearance.Options.UseFont = True
        lblSummary.Location = New Point(206, 54)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(10, 17)
        lblSummary.TabIndex = 3
        lblSummary.Text = ""
        ' 
        ' gridLoans
        ' 
        gridLoans.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        gridLoans.Location = New Point(16, 84)
        gridLoans.MainView = gridviewLoans
        gridLoans.Name = "gridLoans"
        gridLoans.Size = New Size(728, 276)
        gridLoans.TabIndex = 4
        gridLoans.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewLoans})
        ' 
        ' gridviewLoans
        ' 
        gridviewLoans.GridControl = gridLoans
        gridviewLoans.Name = "gridviewLoans"
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnClose.DialogResult = DialogResult.Cancel
        btnClose.Location = New Point(644, 372)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(100, 28)
        btnClose.TabIndex = 5
        btnClose.Text = "Close"
        ' 
        ' frmEmployeeLoansQuickView
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnClose
        ClientSize = New Size(760, 416)
        Controls.Add(btnClose)
        Controls.Add(gridLoans)
        Controls.Add(lblSummary)
        Controls.Add(cboShow)
        Controls.Add(lblShow)
        Controls.Add(lblEmployee)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmEmployeeLoansQuickView"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Employee Loans"
        CType(cboShow.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridLoans, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewLoans, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblShow As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboShow As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridLoans As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewLoans As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton

End Class
