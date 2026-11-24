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
        btnExportDirect = New DevExpress.XtraEditors.SimpleButton()
        btnExportConverter = New DevExpress.XtraEditors.SimpleButton()
        btnExportOther = New DevExpress.XtraEditors.SimpleButton()
        btnPost = New DevExpress.XtraEditors.SimpleButton()
        btnReports = New DevExpress.XtraEditors.SimpleButton()
        btnPayslip = New DevExpress.XtraEditors.SimpleButton()
        chkCombine = New DevExpress.XtraEditors.CheckEdit()
        lblStatus = New DevExpress.XtraEditors.LabelControl()
        cboCutoff = New DevExpress.XtraEditors.LookUpEdit()
        lblCutoff = New DevExpress.XtraEditors.LabelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        splitterLines = New DevExpress.XtraEditors.SplitterControl()
        gridControlLines = New DevExpress.XtraGrid.GridControl()
        gridViewLines = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(chkCombine.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControlLines, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewLines, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' PanelControl1 — same convention as the Input tab / ucLookupMaintenance
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
        PanelControl1.Dock = System.Windows.Forms.DockStyle.Top
        PanelControl1.Location = New System.Drawing.Point(4, 4)
        PanelControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        PanelControl1.Size = New System.Drawing.Size(1092, 100)
        PanelControl1.TabIndex = 0
        '
        ' lblTabPageTitle
        '
        lblTabPageTitle.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New System.Drawing.Font("Segoe UI", 15.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New System.Drawing.Point(8, 14)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New System.Drawing.Size(200, 28)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "PAYROLL OUTPUT"
        '
        ' lblCutoff
        '
        lblCutoff.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        lblCutoff.Location = New System.Drawing.Point(260, 17)
        lblCutoff.Name = "lblCutoff"
        lblCutoff.Size = New System.Drawing.Size(34, 13)
        lblCutoff.TabIndex = 1
        lblCutoff.Text = "Cutoff:"
        '
        ' cboCutoff
        '
        cboCutoff.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        cboCutoff.Location = New System.Drawing.Point(316, 14)
        cboCutoff.Name = "cboCutoff"
        cboCutoff.Properties.NullText = "Select a Cutoff..."
        cboCutoff.Size = New System.Drawing.Size(400, 20)
        cboCutoff.TabIndex = 2
        '
        ' btnExportConverter
        '
        btnExportConverter.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnExportConverter.Location = New System.Drawing.Point(644, 44)
        btnExportConverter.Name = "btnExportConverter"
        btnExportConverter.Size = New System.Drawing.Size(140, 25)
        btnExportConverter.TabIndex = 3
        btnExportConverter.Text = "Export (Converter)"
        '
        ' btnExportDirect
        '
        btnExportDirect.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnExportDirect.Location = New System.Drawing.Point(792, 44)
        btnExportDirect.Name = "btnExportDirect"
        btnExportDirect.Size = New System.Drawing.Size(144, 25)
        btnExportDirect.TabIndex = 4
        btnExportDirect.Text = "Export (Direct to BDO)"
        '
        ' btnExportOther - Other Bank / Cash (may pirma) / Cheque, isang Excel file
        '
        btnExportOther.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnExportOther.Location = New System.Drawing.Point(944, 44)
        btnExportOther.Name = "btnExportOther"
        btnExportOther.Size = New System.Drawing.Size(140, 25)
        btnExportOther.TabIndex = 6
        btnExportOther.Text = "Export Other Lists"
        '
        ' btnPost - Admin lang, FINAL (walang Unpost)
        '
        btnPost.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnPost.Location = New System.Drawing.Point(738, 12)
        btnPost.Name = "btnPost"
        btnPost.Size = New System.Drawing.Size(110, 25)
        btnPost.TabIndex = 7
        btnPost.Text = "Post Cutoff"
        '
        ' btnReports - shortcut papunta sa Reports module (wala pa)
        '
        btnReports.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnReports.Location = New System.Drawing.Point(974, 12)
        btnReports.Name = "btnReports"
        btnReports.Size = New System.Drawing.Size(110, 25)
        btnReports.TabIndex = 8
        btnReports.Text = "Reports"
        '
        ' btnPayslip - shortcut papunta sa Payslip module (wala pa)
        '
        btnPayslip.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnPayslip.Location = New System.Drawing.Point(856, 12)
        btnPayslip.Name = "btnPayslip"
        btnPayslip.Size = New System.Drawing.Size(110, 25)
        btnPayslip.TabIndex = 11
        btnPayslip.Text = "Payslips"
        '
        ' chkCombine - isang file para sa lahat ng cutoff na pareho ang Pay Date (default: unchecked)
        '
        chkCombine.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        chkCombine.Location = New System.Drawing.Point(316, 44)
        chkCombine.Name = "chkCombine"
        chkCombine.Properties.Caption = "Group by pay date"
        chkCombine.Size = New System.Drawing.Size(200, 20)
        chkCombine.TabIndex = 9
        '
        ' lblStatus - Processed / POSTED (sino at kailan)
        '
        lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        lblStatus.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        lblStatus.Appearance.Options.UseFont = True
        lblStatus.Location = New System.Drawing.Point(700, 76)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New System.Drawing.Size(0, 15)
        lblStatus.TabIndex = 10
        '
        ' gridControl
        '
        gridControl.Dock = System.Windows.Forms.DockStyle.Fill
        gridControl.Location = New System.Drawing.Point(4, 104)
        gridControl.MainView = gridView
        gridControl.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridControl.Name = "gridControl"
        gridControl.Size = New System.Drawing.Size(1092, 426)
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
        ' lblSummary - buod ng napiling cutoff (employees, gross, deductions, net)
        '
        lblSummary.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        lblSummary.Location = New System.Drawing.Point(8, 76)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New System.Drawing.Size(0, 13)
        lblSummary.TabIndex = 5
        '
        ' splitterLines - hati sa pagitan ng listahan ng empleyado at ng detalye ng mga linya
        '
        splitterLines.Dock = System.Windows.Forms.DockStyle.Bottom
        splitterLines.Location = New System.Drawing.Point(4, 324)
        splitterLines.Name = "splitterLines"
        splitterLines.Size = New System.Drawing.Size(1092, 6)
        splitterLines.TabIndex = 2
        splitterLines.TabStop = False
        '
        ' gridControlLines - detalye kada linya (payslip / audit) ng napiling empleyado
        '
        gridControlLines.Dock = System.Windows.Forms.DockStyle.Bottom
        gridControlLines.Location = New System.Drawing.Point(4, 330)
        gridControlLines.MainView = gridViewLines
        gridControlLines.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridControlLines.Name = "gridControlLines"
        gridControlLines.Size = New System.Drawing.Size(1092, 200)
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
        AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(gridControl)
        Me.Controls.Add(splitterLines)
        Me.Controls.Add(gridControlLines)
        Me.Controls.Add(PanelControl1)
        Me.Name = "ucPayrollOutput"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(1100, 534)
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(chkCombine.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControlLines, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewLines, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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