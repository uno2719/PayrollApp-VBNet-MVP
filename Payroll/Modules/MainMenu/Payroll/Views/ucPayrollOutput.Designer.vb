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
        cboCutoff = New DevExpress.XtraEditors.LookUpEdit()
        lblCutoff = New DevExpress.XtraEditors.LabelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' PanelControl1 — same convention as the Input tab / ucLookupMaintenance
        '
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
        PanelControl1.Size = New System.Drawing.Size(1092, 70)
        PanelControl1.TabIndex = 0
        '
        ' lblTabPageTitle
        '
        lblTabPageTitle.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New System.Drawing.Font("Segoe UI", 15.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New System.Drawing.Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New System.Drawing.Size(200, 28)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "PAYROLL OUTPUT"
        '
        ' lblCutoff
        '
        lblCutoff.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        lblCutoff.Location = New System.Drawing.Point(260, 28)
        lblCutoff.Name = "lblCutoff"
        lblCutoff.Size = New System.Drawing.Size(34, 13)
        lblCutoff.TabIndex = 1
        lblCutoff.Text = "Cutoff:"
        '
        ' cboCutoff
        '
        cboCutoff.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left
        cboCutoff.Location = New System.Drawing.Point(308, 25)
        cboCutoff.Name = "cboCutoff"
        cboCutoff.Properties.NullText = "Select a Cutoff..."
        cboCutoff.Size = New System.Drawing.Size(280, 20)
        cboCutoff.TabIndex = 2
        '
        ' btnExportConverter
        '
        btnExportConverter.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnExportConverter.Location = New System.Drawing.Point(792, 20)
        btnExportConverter.Name = "btnExportConverter"
        btnExportConverter.Size = New System.Drawing.Size(140, 25)
        btnExportConverter.TabIndex = 3
        btnExportConverter.Text = "Export (Converter)"
        '
        ' btnExportDirect
        '
        btnExportDirect.Anchor = System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right
        btnExportDirect.Location = New System.Drawing.Point(940, 20)
        btnExportDirect.Name = "btnExportDirect"
        btnExportDirect.Size = New System.Drawing.Size(144, 25)
        btnExportDirect.TabIndex = 4
        btnExportDirect.Text = "Export (Direct to BDO)"
        '
        ' gridControl
        '
        gridControl.Dock = System.Windows.Forms.DockStyle.Fill
        gridControl.Location = New System.Drawing.Point(4, 74)
        gridControl.MainView = gridView
        gridControl.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridControl.Name = "gridControl"
        gridControl.Size = New System.Drawing.Size(1092, 456)
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
        ' ucPayrollOutput
        '
        AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(gridControl)
        Me.Controls.Add(PanelControl1)
        Me.Name = "ucPayrollOutput"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(1100, 534)
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCutoff As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCutoff As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents btnExportConverter As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportDirect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView

End Class