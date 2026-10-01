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
        components = New System.ComponentModel.Container()
        pnlTop = New System.Windows.Forms.Panel()
        btnExportDirect = New DevExpress.XtraEditors.SimpleButton()
        btnExportConverter = New DevExpress.XtraEditors.SimpleButton()
        cboCutoff = New DevExpress.XtraEditors.LookUpEdit()
        lblCutoff = New DevExpress.XtraEditors.LabelControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlTop.SuspendLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        pnlTop.Controls.Add(btnExportDirect)
        pnlTop.Controls.Add(btnExportConverter)
        pnlTop.Controls.Add(cboCutoff)
        pnlTop.Controls.Add(lblCutoff)
        pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        pnlTop.Location = New System.Drawing.Point(0, 0)
        pnlTop.Name = "pnlTop"
        pnlTop.Size = New System.Drawing.Size(1100, 44)
        pnlTop.TabIndex = 0
        '
        'lblCutoff
        '
        lblCutoff.Location = New System.Drawing.Point(12, 15)
        lblCutoff.Name = "lblCutoff"
        lblCutoff.Size = New System.Drawing.Size(34, 13)
        lblCutoff.TabIndex = 0
        lblCutoff.Text = "Cutoff:"
        '
        'cboCutoff
        '
        cboCutoff.Location = New System.Drawing.Point(60, 12)
        cboCutoff.Name = "cboCutoff"
        cboCutoff.Properties.NullText = "Select a Cutoff..."
        cboCutoff.Size = New System.Drawing.Size(320, 20)
        cboCutoff.TabIndex = 1
        '
        'btnExportConverter
        '
        btnExportConverter.Location = New System.Drawing.Point(788, 10)
        btnExportConverter.Name = "btnExportConverter"
        btnExportConverter.Size = New System.Drawing.Size(150, 25)
        btnExportConverter.TabIndex = 2
        btnExportConverter.Text = "Export (BDO Converter)"
        '
        'btnExportDirect
        '
        btnExportDirect.Location = New System.Drawing.Point(944, 10)
        btnExportDirect.Name = "btnExportDirect"
        btnExportDirect.Size = New System.Drawing.Size(144, 25)
        btnExportDirect.TabIndex = 3
        btnExportDirect.Text = "Export (Direct to BDO)"
        '
        'gridControl
        '
        gridControl.Dock = System.Windows.Forms.DockStyle.Fill
        gridControl.Location = New System.Drawing.Point(0, 44)
        gridControl.MainView = gridView
        gridControl.Name = "gridControl"
        gridControl.Size = New System.Drawing.Size(1100, 556)
        gridControl.TabIndex = 1
        gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridView})
        '
        'gridView
        '
        gridView.GridControl = gridControl
        gridView.Name = "gridView"
        gridView.OptionsBehavior.Editable = False
        gridView.OptionsView.ShowGroupPanel = False
        '
        'ucPayrollOutput
        '
        Me.Controls.Add(gridControl)
        Me.Controls.Add(pnlTop)
        Me.Name = "ucPayrollOutput"
        Me.Size = New System.Drawing.Size(1100, 600)
        pnlTop.ResumeLayout(False)
        pnlTop.PerformLayout()
        CType(cboCutoff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lblCutoff As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCutoff As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents btnExportConverter As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnExportDirect As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView

End Class