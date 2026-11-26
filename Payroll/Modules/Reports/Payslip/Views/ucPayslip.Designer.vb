<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayslip
    Inherits GlobalShared.Base.ucBase

    'UserControl overrides dispose to clean up the component list.
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
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        lblCutoff = New DevExpress.XtraEditors.LabelControl()
        cboCutoff = New DevExpress.XtraEditors.LookUpEdit()
        btnRefresh = New DevExpress.XtraEditors.SimpleButton()
        lblStatus = New DevExpress.XtraEditors.LabelControl()
        chkIncludeAllowance = New DevExpress.XtraEditors.CheckEdit()
        chkShowYtd = New DevExpress.XtraEditors.CheckEdit()
        chkProtectPdf = New DevExpress.XtraEditors.CheckEdit()
        btnCheckAll = New DevExpress.XtraEditors.SimpleButton()
        btnUncheckAll = New DevExpress.XtraEditors.SimpleButton()
        lblSelection = New DevExpress.XtraEditors.LabelControl()
        btnPreview = New DevExpress.XtraEditors.SimpleButton()
        btnOpenViewer = New DevExpress.XtraEditors.SimpleButton()
        btnPrint = New DevExpress.XtraEditors.SimpleButton()
        btnExportPdf = New DevExpress.XtraEditors.SimpleButton()
        btnExportEach = New DevExpress.XtraEditors.SimpleButton()
        btnEmail = New DevExpress.XtraEditors.SimpleButton()
        btnEmailOne = New DevExpress.XtraEditors.SimpleButton()
        pnlProgress = New DevExpress.XtraEditors.PanelControl()
        progressEmail = New DevExpress.XtraEditors.ProgressBarControl()
        lblProgress = New DevExpress.XtraEditors.LabelControl()
        btnStopEmail = New DevExpress.XtraEditors.SimpleButton()
        splitMain = New DevExpress.XtraEditors.SplitContainerControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        documentViewer = New DevExpress.XtraPrinting.Preview.DocumentViewer()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(cboCutoff.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkIncludeAllowance.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkShowYtd.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkProtectPdf.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlProgress, ComponentModel.ISupportInitialize).BeginInit()
        pnlProgress.SuspendLayout()
        CType(progressEmail.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(splitMain, ComponentModel.ISupportInitialize).BeginInit()
        CType(splitMain.Panel1, ComponentModel.ISupportInitialize).BeginInit()
        splitMain.Panel1.SuspendLayout()
        CType(splitMain.Panel2, ComponentModel.ISupportInitialize).BeginInit()
        splitMain.Panel2.SuspendLayout()
        splitMain.SuspendLayout()
        CType(gridControl, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(lblCutoff)
        PanelControl1.Controls.Add(cboCutoff)
        PanelControl1.Controls.Add(btnRefresh)
        PanelControl1.Controls.Add(lblStatus)
        PanelControl1.Controls.Add(chkIncludeAllowance)
        PanelControl1.Controls.Add(chkShowYtd)
        PanelControl1.Controls.Add(chkProtectPdf)
        PanelControl1.Controls.Add(btnCheckAll)
        PanelControl1.Controls.Add(btnUncheckAll)
        PanelControl1.Controls.Add(lblSelection)
        PanelControl1.Controls.Add(btnPreview)
        PanelControl1.Controls.Add(btnOpenViewer)
        PanelControl1.Controls.Add(btnPrint)
        PanelControl1.Controls.Add(btnExportPdf)
        PanelControl1.Controls.Add(btnExportEach)
        PanelControl1.Controls.Add(btnEmail)
        PanelControl1.Controls.Add(btnEmailOne)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(1192, 108)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 14)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(77, 28)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "PAYSLIP"
        ' 
        ' lblCutoff
        ' 
        lblCutoff.Location = New Point(190, 17)
        lblCutoff.Name = "lblCutoff"
        lblCutoff.Size = New Size(35, 13)
        lblCutoff.TabIndex = 1
        lblCutoff.Text = "Cutoff:"
        ' 
        ' cboCutoff
        ' 
        cboCutoff.Location = New Point(240, 14)
        cboCutoff.Name = "cboCutoff"
        cboCutoff.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboCutoff.Properties.NullText = "(no processed or posted cutoff)"
        cboCutoff.Properties.ShowHeader = False
        cboCutoff.Size = New Size(420, 20)
        cboCutoff.TabIndex = 2
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Location = New Point(666, 12)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(80, 25)
        btnRefresh.TabIndex = 17
        btnRefresh.Text = "Refresh"
        btnRefresh.ToolTip = "Reload the cutoff list and the email status."
        ' 
        ' lblStatus
        ' 
        lblStatus.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatus.Appearance.Options.UseFont = True
        lblStatus.Location = New Point(756, 16)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(0, 15)
        lblStatus.TabIndex = 3
        ' 
        ' chkIncludeAllowance
        ' 
        chkIncludeAllowance.EditValue = True
        chkIncludeAllowance.Location = New Point(240, 44)
        chkIncludeAllowance.Name = "chkIncludeAllowance"
        chkIncludeAllowance.Properties.Caption = "Include allowance"
        chkIncludeAllowance.Size = New Size(140, 20)
        chkIncludeAllowance.TabIndex = 4
        chkIncludeAllowance.ToolTip = "Uncheck to leave out the allowance codes (Payroll Settings > Compensation > Allowance (Payslip)). Gross and Net are recomputed."
        ' 
        ' chkShowYtd
        ' 
        chkShowYtd.EditValue = True
        chkShowYtd.Location = New Point(386, 44)
        chkShowYtd.Name = "chkShowYtd"
        chkShowYtd.Properties.Caption = "Show YTD"
        chkShowYtd.Size = New Size(90, 20)
        chkShowYtd.TabIndex = 5
        ' 
        ' chkProtectPdf
        ' 
        chkProtectPdf.EditValue = True
        chkProtectPdf.Location = New Point(482, 44)
        chkProtectPdf.Name = "chkProtectPdf"
        chkProtectPdf.Properties.Caption = "Password-protect emailed PDF"
        chkProtectPdf.Size = New Size(200, 20)
        chkProtectPdf.TabIndex = 6
        chkProtectPdf.ToolTip = "Password = employee's birthdate (MMDDYYYY). If there is no birthdate on file, the Employee ID."
        ' 
        ' btnCheckAll
        ' 
        btnCheckAll.Location = New Point(8, 74)
        btnCheckAll.Name = "btnCheckAll"
        btnCheckAll.Size = New Size(80, 25)
        btnCheckAll.TabIndex = 7
        btnCheckAll.Text = "Check all"
        ' 
        ' btnUncheckAll
        ' 
        btnUncheckAll.Location = New Point(92, 74)
        btnUncheckAll.Name = "btnUncheckAll"
        btnUncheckAll.Size = New Size(80, 25)
        btnUncheckAll.TabIndex = 8
        btnUncheckAll.Text = "Uncheck all"
        ' 
        ' lblSelection
        ' 
        lblSelection.Location = New Point(180, 80)
        lblSelection.Name = "lblSelection"
        lblSelection.Size = New Size(0, 13)
        lblSelection.TabIndex = 9
        ' 
        ' btnPreview
        ' 
        btnPreview.Location = New Point(400, 74)
        btnPreview.Name = "btnPreview"
        btnPreview.Size = New Size(90, 25)
        btnPreview.TabIndex = 10
        btnPreview.Text = "Preview"
        btnPreview.ToolTip = "Preview the checked employees (or all if none is checked). Double-click a row to preview one employee."
        ' 
        ' btnOpenViewer
        ' 
        btnOpenViewer.Location = New Point(496, 74)
        btnOpenViewer.Name = "btnOpenViewer"
        btnOpenViewer.Size = New Size(95, 25)
        btnOpenViewer.TabIndex = 11
        btnOpenViewer.Text = "Open Viewer"
        btnOpenViewer.ToolTip = "Open in a separate preview window (zoom, search, other export formats)."
        ' 
        ' btnPrint
        ' 
        btnPrint.Location = New Point(597, 74)
        btnPrint.Name = "btnPrint"
        btnPrint.Size = New Size(80, 25)
        btnPrint.TabIndex = 12
        btnPrint.Text = "Print"
        ' 
        ' btnExportPdf
        ' 
        btnExportPdf.Location = New Point(683, 74)
        btnExportPdf.Name = "btnExportPdf"
        btnExportPdf.Size = New Size(95, 25)
        btnExportPdf.TabIndex = 13
        btnExportPdf.Text = "Export PDF"
        btnExportPdf.ToolTip = "All payslips in one PDF file."
        ' 
        ' btnExportEach
        ' 
        btnExportEach.Location = New Point(784, 74)
        btnExportEach.Name = "btnExportEach"
        btnExportEach.Size = New Size(120, 25)
        btnExportEach.TabIndex = 14
        btnExportEach.Text = "PDF per Employee"
        btnExportEach.ToolTip = "One PDF file per employee in a folder (not password-protected)."
        ' 
        ' btnEmail
        ' 
        btnEmail.Appearance.Font = New Font("Tahoma", 8.25F, FontStyle.Bold)
        btnEmail.Appearance.Options.UseFont = True
        btnEmail.Location = New Point(940, 74)
        btnEmail.Name = "btnEmail"
        btnEmail.Size = New Size(120, 25)
        btnEmail.TabIndex = 15
        btnEmail.Text = "Email Payslips"
        btnEmail.ToolTip = "Email each checked employee (or all if none is checked) their own payslip PDF. Posted cutoffs only."
        ' 
        ' btnEmailOne
        ' 
        btnEmailOne.Location = New Point(1066, 74)
        btnEmailOne.Name = "btnEmailOne"
        btnEmailOne.Size = New Size(115, 25)
        btnEmailOne.TabIndex = 16
        btnEmailOne.Text = "Email this Employee"
        btnEmailOne.ToolTip = "Email only the highlighted (focused) employee."
        ' 
        ' pnlProgress
        ' 
        pnlProgress.Controls.Add(progressEmail)
        pnlProgress.Controls.Add(lblProgress)
        pnlProgress.Controls.Add(btnStopEmail)
        pnlProgress.Dock = DockStyle.Bottom
        pnlProgress.Location = New Point(4, 566)
        pnlProgress.Name = "pnlProgress"
        pnlProgress.Size = New Size(1192, 34)
        pnlProgress.TabIndex = 2
        pnlProgress.Visible = False
        ' 
        ' progressEmail
        ' 
        progressEmail.Location = New Point(8, 8)
        progressEmail.Name = "progressEmail"
        progressEmail.Properties.ShowTitle = True
        progressEmail.Size = New Size(300, 18)
        progressEmail.TabIndex = 0
        ' 
        ' lblProgress
        ' 
        lblProgress.Location = New Point(318, 10)
        lblProgress.Name = "lblProgress"
        lblProgress.Size = New Size(0, 13)
        lblProgress.TabIndex = 1
        ' 
        ' btnStopEmail
        ' 
        btnStopEmail.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnStopEmail.Location = New Point(1096, 5)
        btnStopEmail.Name = "btnStopEmail"
        btnStopEmail.Size = New Size(88, 24)
        btnStopEmail.TabIndex = 2
        btnStopEmail.Text = "Stop sending"
        ' 
        ' splitMain
        ' 
        splitMain.Dock = DockStyle.Fill
        splitMain.Location = New Point(4, 112)
        splitMain.Name = "splitMain"
        ' 
        ' splitMain.Panel1
        ' 
        splitMain.Panel1.Controls.Add(gridControl)
        splitMain.Panel1.Text = "Employees"
        ' 
        ' splitMain.Panel2
        ' 
        splitMain.Panel2.Controls.Add(documentViewer)
        splitMain.Panel2.Text = "Preview"
        splitMain.Size = New Size(1192, 454)
        splitMain.SplitterPosition = 560
        splitMain.TabIndex = 1
        ' 
        ' gridControl
        ' 
        gridControl.Dock = DockStyle.Fill
        gridControl.Location = New Point(0, 0)
        gridControl.MainView = gridView
        gridControl.Name = "gridControl"
        gridControl.Size = New Size(560, 454)
        gridControl.TabIndex = 0
        gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridView})
        ' 
        ' gridView
        ' 
        gridView.GridControl = gridControl
        gridView.Name = "gridView"
        gridView.OptionsFind.AlwaysVisible = True
        gridView.OptionsView.ShowGroupPanel = False
        ' 
        ' documentViewer
        ' 
        documentViewer.Dock = DockStyle.Fill
        documentViewer.Location = New Point(0, 0)
        documentViewer.Name = "documentViewer"
        documentViewer.Size = New Size(622, 454)
        documentViewer.TabIndex = 0
        ' 
        ' ucPayslip
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(splitMain)
        Controls.Add(pnlProgress)
        Controls.Add(PanelControl1)
        Name = "ucPayslip"
        Padding = New Padding(4)
        Size = New Size(1200, 604)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(cboCutoff.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkIncludeAllowance.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkShowYtd.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkProtectPdf.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlProgress, ComponentModel.ISupportInitialize).EndInit()
        pnlProgress.ResumeLayout(False)
        pnlProgress.PerformLayout()
        CType(progressEmail.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(splitMain.Panel1, ComponentModel.ISupportInitialize).EndInit()
        splitMain.Panel1.ResumeLayout(False)
        CType(splitMain.Panel2, ComponentModel.ISupportInitialize).EndInit()
        splitMain.Panel2.ResumeLayout(False)
        CType(splitMain, ComponentModel.ISupportInitialize).EndInit()
        splitMain.ResumeLayout(False)
        CType(gridControl, ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCutoff As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCutoff As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents btnRefresh As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkIncludeAllowance As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkShowYtd As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkProtectPdf As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents btnCheckAll As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnUncheckAll As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblSelection As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnPreview As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOpenViewer As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPrint As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportPdf As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportEach As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnEmail As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnEmailOne As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents pnlProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents progressEmail As DevExpress.XtraEditors.ProgressBarControl
    Friend WithEvents lblProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnStopEmail As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents splitMain As DevExpress.XtraEditors.SplitContainerControl
    Friend WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents documentViewer As DevExpress.XtraPrinting.Preview.DocumentViewer

End Class
