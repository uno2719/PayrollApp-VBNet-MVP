<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayrollOutput
    Inherits Payroll.GlobalShared.Base.ucBase

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
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        lblStatus = New DevExpress.XtraEditors.LabelControl()
        chkCombine = New DevExpress.XtraEditors.CheckEdit()
        btnReports = New DevExpress.XtraEditors.SimpleButton()
        btnPayslip = New DevExpress.XtraEditors.SimpleButton()
        btnPost = New DevExpress.XtraEditors.SimpleButton()
        btnExportOther = New DevExpress.XtraEditors.SimpleButton()
        btnExportDirect = New DevExpress.XtraEditors.SimpleButton()
        btnExportConverter = New DevExpress.XtraEditors.SimpleButton()
        cboCutoff = New DevExpress.XtraEditors.LookUpEdit()
        lblCutoff = New DevExpress.XtraEditors.LabelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        splitterLines = New DevExpress.XtraEditors.SplitterControl()
        gridControlLines = New DevExpress.XtraGrid.GridControl()
        gridViewLines = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(chkCombine.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboCutoff.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControlLines, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewLines, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblSummary)
        PanelControl1.Controls.Add(lblStatus)
        PanelControl1.Controls.Add(chkCombine)
        PanelControl1.Controls.Add(btnReports)
        PanelControl1.Controls.Add(btnPayslip)
        PanelControl1.Controls.Add(btnPost)
        PanelControl1.Controls.Add(btnExportOther)
        PanelControl1.Controls.Add(btnExportDirect)
        PanelControl1.Controls.Add(btnExportConverter)
        PanelControl1.Controls.Add(cboCutoff)
        PanelControl1.Controls.Add(lblCutoff)
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(1092, 100)
        PanelControl1.TabIndex = 0
        ' 
        ' lblSummary
        ' 
        lblSummary.Location = New Point(8, 76)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(0, 13)
        lblSummary.TabIndex = 5
        ' 
        ' lblStatus
        ' 
        lblStatus.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatus.Appearance.Options.UseFont = True
        lblStatus.Location = New Point(700, 76)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(0, 15)
        lblStatus.TabIndex = 10
        ' 
        ' chkCombine
        ' 
        chkCombine.Location = New Point(316, 44)
        chkCombine.Name = "chkCombine"
        chkCombine.Properties.Caption = "Group by Pay Date"
        chkCombine.Size = New Size(200, 20)
        chkCombine.TabIndex = 9
        ' 
        ' btnReports
        ' 
        btnReports.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnReports.Location = New Point(974, 12)
        btnReports.Name = "btnReports"
        btnReports.Size = New Size(110, 25)
        btnReports.TabIndex = 8
        btnReports.Text = "Reports"
        ' 
        ' btnPayslip
        ' 
        btnPayslip.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnPayslip.Location = New Point(856, 12)
        btnPayslip.Name = "btnPayslip"
        btnPayslip.Size = New Size(110, 25)
        btnPayslip.TabIndex = 11
        btnPayslip.Text = "Payslips"
        ' 
        ' btnPost
        ' 
        btnPost.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnPost.Location = New Point(738, 12)
        btnPost.Name = "btnPost"
        btnPost.Size = New Size(110, 25)
        btnPost.TabIndex = 7
        btnPost.Text = "Post Cutoff"
        ' 
        ' btnExportOther
        ' 
        btnExportOther.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExportOther.Location = New Point(944, 44)
        btnExportOther.Name = "btnExportOther"
        btnExportOther.Size = New Size(140, 25)
        btnExportOther.TabIndex = 6
        btnExportOther.Text = "Export Other Lists"
        ' 
        ' btnExportDirect
        ' 
        btnExportDirect.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExportDirect.Location = New Point(792, 44)
        btnExportDirect.Name = "btnExportDirect"
        btnExportDirect.Size = New Size(144, 25)
        btnExportDirect.TabIndex = 4
        btnExportDirect.Text = "Export (Direct to BDO)"
        ' 
        ' btnExportConverter
        ' 
        btnExportConverter.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExportConverter.Location = New Point(644, 44)
        btnExportConverter.Name = "btnExportConverter"
        btnExportConverter.Size = New Size(140, 25)
        btnExportConverter.TabIndex = 3
        btnExportConverter.Text = "Export (Converter)"
        ' 
        ' cboCutoff
        ' 
        cboCutoff.Location = New Point(316, 14)
        cboCutoff.Name = "cboCutoff"
        cboCutoff.Properties.NullText = "Select a Cutoff..."
        cboCutoff.Size = New Size(400, 20)
        cboCutoff.TabIndex = 2
        ' 
        ' lblCutoff
        ' 
        lblCutoff.Location = New Point(260, 17)
        lblCutoff.Name = "lblCutoff"
        lblCutoff.Size = New Size(35, 13)
        lblCutoff.TabIndex = 1
        lblCutoff.Text = "Cutoff:"
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 14)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(171, 28)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "PAYROLL OUTPUT"
        ' 
        ' gridControl
        ' 
        gridControl.Dock = DockStyle.Fill
        gridControl.Location = New Point(4, 104)
        gridControl.MainView = gridView
        gridControl.Margin = New Padding(3, 2, 3, 2)
        gridControl.Name = "gridControl"
        gridControl.Size = New Size(1092, 216)
        gridControl.TabIndex = 1
        gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridView})
        ' 
        ' gridView
        ' 
        gridView.GridControl = gridControl
        gridView.Name = "gridView"
        gridView.OptionsBehavior.Editable = False
        gridView.OptionsView.ShowGroupPanel = False
        ' 
        ' splitterLines
        ' 
        splitterLines.Dock = DockStyle.Bottom
        splitterLines.Location = New Point(4, 320)
        splitterLines.Name = "splitterLines"
        splitterLines.Size = New Size(1092, 10)
        splitterLines.TabIndex = 2
        splitterLines.TabStop = False
        ' 
        ' gridControlLines
        ' 
        gridControlLines.Dock = DockStyle.Bottom
        gridControlLines.Location = New Point(4, 330)
        gridControlLines.MainView = gridViewLines
        gridControlLines.Margin = New Padding(3, 2, 3, 2)
        gridControlLines.Name = "gridControlLines"
        gridControlLines.Size = New Size(1092, 200)
        gridControlLines.TabIndex = 3
        gridControlLines.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewLines})
        ' 
        ' gridViewLines
        ' 
        gridViewLines.GridControl = gridControlLines
        gridViewLines.Name = "gridViewLines"
        gridViewLines.OptionsBehavior.Editable = False
        gridViewLines.OptionsView.ShowGroupPanel = False
        ' 
        ' ucPayrollOutput
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridControl)
        Controls.Add(splitterLines)
        Controls.Add(gridControlLines)
        Controls.Add(PanelControl1)
        Name = "ucPayrollOutput"
        Padding = New Padding(4)
        Size = New Size(1100, 534)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(chkCombine.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboCutoff.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, ComponentModel.ISupportInitialize).EndInit()
        CType(gridControlLines, ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewLines, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCutoff As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCutoff As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents btnExportConverter As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportDirect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportOther As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPost As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnReports As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPayslip As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents chkCombine As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents splitterLines As DevExpress.XtraEditors.SplitterControl
    Friend WithEvents gridControlLines As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewLines As DevExpress.XtraGrid.Views.Grid.GridView

End Class