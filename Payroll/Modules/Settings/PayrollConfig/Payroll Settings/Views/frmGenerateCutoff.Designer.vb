<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGenerateCutoff
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
        lblCycleType = New DevExpress.XtraEditors.LabelControl()
        cboCycleType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblYear = New DevExpress.XtraEditors.LabelControl()
        numYear = New DevExpress.XtraEditors.SpinEdit()
        lblPattern = New DevExpress.XtraEditors.LabelControl()
        gridControlPreview = New DevExpress.XtraGrid.GridControl()
        gridViewPreview = New DevExpress.XtraGrid.Views.Grid.GridView()
        colPeriod = New DevExpress.XtraGrid.Columns.GridColumn()
        colStart = New DevExpress.XtraGrid.Columns.GridColumn()
        colEnd = New DevExpress.XtraGrid.Columns.GridColumn()
        colPayDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colStatus = New DevExpress.XtraGrid.Columns.GridColumn()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        btnGenerate = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(cboCycleType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(numYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControlPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' lblCycleType
        '
        lblCycleType.Location = New System.Drawing.Point(20, 20)
        lblCycleType.Name = "lblCycleType"
        lblCycleType.Size = New System.Drawing.Size(47, 13)
        lblCycleType.TabIndex = 0
        lblCycleType.Text = "Pay cycle"
        '
        ' cboCycleType
        '
        cboCycleType.Location = New System.Drawing.Point(20, 38)
        cboCycleType.Name = "cboCycleType"
        cboCycleType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboCycleType.Size = New System.Drawing.Size(180, 20)
        cboCycleType.TabIndex = 1
        '
        ' lblYear
        '
        lblYear.Location = New System.Drawing.Point(220, 20)
        lblYear.Name = "lblYear"
        lblYear.Size = New System.Drawing.Size(21, 13)
        lblYear.TabIndex = 2
        lblYear.Text = "Year"
        '
        ' numYear
        '
        numYear.EditValue = New Decimal(New Integer() {2026, 0, 0, 0})
        numYear.Location = New System.Drawing.Point(220, 38)
        numYear.Name = "numYear"
        numYear.Properties.Mask.EditMask = "N0"
        numYear.Properties.MaxValue = New Decimal(New Integer() {2100, 0, 0, 0})
        numYear.Properties.MinValue = New Decimal(New Integer() {2000, 0, 0, 0})
        numYear.Size = New System.Drawing.Size(90, 20)
        numYear.TabIndex = 3
        '
        ' lblPattern
        '
        lblPattern.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblPattern.Appearance.Options.UseForeColor = True
        lblPattern.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblPattern.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblPattern.Location = New System.Drawing.Point(20, 70)
        lblPattern.Name = "lblPattern"
        lblPattern.Size = New System.Drawing.Size(560, 18)
        lblPattern.TabIndex = 4
        '
        ' gridControlPreview
        '
        gridControlPreview.Location = New System.Drawing.Point(20, 96)
        gridControlPreview.MainView = gridViewPreview
        gridControlPreview.Name = "gridControlPreview"
        gridControlPreview.Size = New System.Drawing.Size(560, 280)
        gridControlPreview.TabIndex = 5
        gridControlPreview.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewPreview})
        '
        ' gridViewPreview
        '
        gridViewPreview.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colPeriod, colStart, colEnd, colPayDate, colStatus})
        gridViewPreview.GridControl = gridControlPreview
        gridViewPreview.Name = "gridViewPreview"
        gridViewPreview.OptionsBehavior.Editable = False
        gridViewPreview.OptionsSelection.EnableAppearanceFocusedCell = False
        gridViewPreview.OptionsView.ShowGroupPanel = False
        '
        ' colPeriod
        '
        colPeriod.Caption = "Period"
        colPeriod.FieldName = "PeriodNo"
        colPeriod.Name = "colPeriod"
        colPeriod.Visible = True
        colPeriod.VisibleIndex = 0
        colPeriod.Width = 55
        '
        ' colStart
        '
        colStart.Caption = "From"
        colStart.FieldName = "CutoffStart"
        colStart.Name = "colStart"
        colStart.Visible = True
        colStart.VisibleIndex = 1
        colStart.Width = 100
        '
        ' colEnd
        '
        colEnd.Caption = "To"
        colEnd.FieldName = "CutoffEnd"
        colEnd.Name = "colEnd"
        colEnd.Visible = True
        colEnd.VisibleIndex = 2
        colEnd.Width = 100
        '
        ' colPayDate
        '
        colPayDate.Caption = "Pay date"
        colPayDate.FieldName = "PayDate"
        colPayDate.Name = "colPayDate"
        colPayDate.Visible = True
        colPayDate.VisibleIndex = 3
        colPayDate.Width = 100
        '
        ' colStatus
        '
        colStatus.Caption = "Status"
        colStatus.FieldName = "StatusText"
        colStatus.Name = "colStatus"
        colStatus.Visible = True
        colStatus.VisibleIndex = 4
        colStatus.Width = 120
        '
        ' lblSummary
        '
        lblSummary.Appearance.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        lblSummary.Appearance.Options.UseFont = True
        lblSummary.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblSummary.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblSummary.Location = New System.Drawing.Point(20, 386)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New System.Drawing.Size(340, 36)
        lblSummary.TabIndex = 6
        '
        ' btnGenerate
        '
        btnGenerate.Location = New System.Drawing.Point(380, 390)
        btnGenerate.Name = "btnGenerate"
        btnGenerate.Size = New System.Drawing.Size(95, 30)
        btnGenerate.TabIndex = 7
        btnGenerate.Text = "Generate"
        '
        ' btnCancel
        '
        btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        btnCancel.Location = New System.Drawing.Point(485, 390)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New System.Drawing.Size(95, 30)
        btnCancel.TabIndex = 8
        btnCancel.Text = "Cancel"
        '
        ' frmGenerateCutoff
        '
        AcceptButton = btnGenerate
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        CancelButton = btnCancel
        ClientSize = New System.Drawing.Size(600, 440)
        Controls.Add(btnCancel)
        Controls.Add(btnGenerate)
        Controls.Add(lblSummary)
        Controls.Add(gridControlPreview)
        Controls.Add(lblPattern)
        Controls.Add(numYear)
        Controls.Add(lblYear)
        Controls.Add(cboCycleType)
        Controls.Add(lblCycleType)
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmGenerateCutoff"
        ShowInTaskbar = False
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Text = "Generate Cut-off"
        CType(cboCycleType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(numYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControlPreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewPreview, System.ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblCycleType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCycleType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblYear As DevExpress.XtraEditors.LabelControl
    Friend WithEvents numYear As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblPattern As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridControlPreview As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewPreview As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPeriod As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStart As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEnd As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStatus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnGenerate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
