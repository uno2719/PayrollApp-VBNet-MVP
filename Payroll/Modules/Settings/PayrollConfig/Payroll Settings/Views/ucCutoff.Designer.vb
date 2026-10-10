<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucCutoff
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
        Dim WindowsuiButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions4 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        grpFilters = New DevExpress.XtraEditors.GroupControl()
        lblFilterCycle = New DevExpress.XtraEditors.LabelControl()
        cboFilterCycle = New DevExpress.XtraEditors.ComboBoxEdit()
        lblFilterYear = New DevExpress.XtraEditors.LabelControl()
        cboFilterYear = New DevExpress.XtraEditors.ComboBoxEdit()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        cboStatus = New DevExpress.XtraEditors.ComboBoxEdit()
        lblStatus = New DevExpress.XtraEditors.LabelControl()
        txtCutoffLabel = New DevExpress.XtraEditors.TextEdit()
        lblLabel = New DevExpress.XtraEditors.LabelControl()
        datePayDate = New DevExpress.XtraEditors.DateEdit()
        lblPayDate = New DevExpress.XtraEditors.LabelControl()
        dateCutoffEnd = New DevExpress.XtraEditors.DateEdit()
        lblCutoffEnd = New DevExpress.XtraEditors.LabelControl()
        dateCutoffStart = New DevExpress.XtraEditors.DateEdit()
        lblCutoffStart = New DevExpress.XtraEditors.LabelControl()
        numCutoffYear = New DevExpress.XtraEditors.SpinEdit()
        lblCutoffYear = New DevExpress.XtraEditors.LabelControl()
        cboCycleType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblCycleType = New DevExpress.XtraEditors.LabelControl()
        gridControl = New DevExpress.XtraGrid.GridControl()
        gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        colCycleType = New DevExpress.XtraGrid.Columns.GridColumn()
        colCutoffYear = New DevExpress.XtraGrid.Columns.GridColumn()
        colCutoffStart = New DevExpress.XtraGrid.Columns.GridColumn()
        colCutoffEnd = New DevExpress.XtraGrid.Columns.GridColumn()
        colPayDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colCutoffLabel = New DevExpress.XtraGrid.Columns.GridColumn()
        colStatus = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpFilters, System.ComponentModel.ISupportInitialize).BeginInit()
        grpFilters.SuspendLayout()
        CType(cboFilterCycle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(cboFilterYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(grpDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(cboStatus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(txtCutoffLabel.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(datePayDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(datePayDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffEnd.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffEnd.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffStart.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffStart.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(numCutoffYear.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(cboCycleType.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' PanelControl1 — same WindowsUIButtonPanel command-bar convention as
        ' ucPayrollRateEntry (Overtime/Holiday), copied from the real file.
        '
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
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
        lblTabPageTitle.Appearance.ForeColor = System.Drawing.Color.Black
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Appearance.Options.UseForeColor = True
        lblTabPageTitle.Location = New System.Drawing.Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New System.Drawing.Size(150, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "CUTOFF"
        '
        ' wbpMainCommands — [0]=sep, [1]=New (opens Generate dialog)/Update,
        ' [2]=Edit/Cancel, [3]=sep, [4]=Close Older, [5]=sep, [6]=Refresh, [7]=sep. Same index scheme as
        ' RateEntry's BTN_NEW/BTN_EDIT/BTN_REFRESH constants.
        '
        wbpMainCommands.ButtonInterval = 15
        WindowsuiButtonImageOptions1.Image = My.Resources.Resources.icon_add_property_24_png
        WindowsuiButtonImageOptions2.Image = My.Resources.Resources.icon_edit_property_24
        WindowsuiButtonImageOptions3.Image = My.Resources.Resources.icon_refresh_24
        WindowsuiButtonImageOptions4.Image = My.Resources.Resources.icon_cancel_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Generate Cutoffs for a Pay Cycle and Year", -1, True, Nothing, True, False, True, "New", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Close Older...", True, WindowsuiButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Close all Draft cutoffs that ended before a date (hal. mga bago nagsimulang gamitin ang app)", -1, True, Nothing, True, False, True, "CloseOlder", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight
        wbpMainCommands.Dock = System.Windows.Forms.DockStyle.Right
        wbpMainCommands.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New System.Drawing.Point(459, 4)
        wbpMainCommands.Margin = New System.Windows.Forms.Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New System.Drawing.Size(629, 62)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        '
        ' grpDetails
        '
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(cboStatus)
        grpDetails.Controls.Add(lblStatus)
        grpDetails.Controls.Add(txtCutoffLabel)
        grpDetails.Controls.Add(lblLabel)
        grpDetails.Controls.Add(datePayDate)
        grpDetails.Controls.Add(lblPayDate)
        grpDetails.Controls.Add(dateCutoffEnd)
        grpDetails.Controls.Add(lblCutoffEnd)
        grpDetails.Controls.Add(dateCutoffStart)
        grpDetails.Controls.Add(lblCutoffStart)
        grpDetails.Controls.Add(numCutoffYear)
        grpDetails.Controls.Add(lblCutoffYear)
        grpDetails.Controls.Add(cboCycleType)
        grpDetails.Controls.Add(lblCycleType)
        grpDetails.Dock = System.Windows.Forms.DockStyle.Top
        grpDetails.Location = New System.Drawing.Point(4, 74)
        grpDetails.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New System.Drawing.Size(1092, 110)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        '
        ' lblCycleType
        '
        lblCycleType.Location = New System.Drawing.Point(24, 34)
        lblCycleType.Name = "lblCycleType"
        lblCycleType.Size = New System.Drawing.Size(58, 13)
        lblCycleType.TabIndex = 0
        lblCycleType.Text = "Cycle Type"
        '
        ' cboCycleType
        '
        cboCycleType.Location = New System.Drawing.Point(24, 51)
        cboCycleType.Name = "cboCycleType"
        cboCycleType.Properties.Items.AddRange(New Object() {"Monthly", "SemiMonthly", "Weekly", "Daily"})
        cboCycleType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboCycleType.Size = New System.Drawing.Size(150, 20)
        cboCycleType.TabIndex = 1
        '
        ' lblCutoffYear
        '
        lblCutoffYear.Location = New System.Drawing.Point(190, 34)
        lblCutoffYear.Name = "lblCutoffYear"
        lblCutoffYear.Size = New System.Drawing.Size(21, 13)
        lblCutoffYear.TabIndex = 2
        lblCutoffYear.Text = "Year"
        '
        ' numCutoffYear
        '
        numCutoffYear.Location = New System.Drawing.Point(190, 51)
        numCutoffYear.Name = "numCutoffYear"
        numCutoffYear.Properties.Mask.EditMask = "N0"
        numCutoffYear.Properties.MaxValue = New Decimal(New Integer() {2100, 0, 0, 0})
        numCutoffYear.Properties.MinValue = New Decimal(New Integer() {2000, 0, 0, 0})
        numCutoffYear.Size = New System.Drawing.Size(80, 20)
        numCutoffYear.TabIndex = 3
        '
        ' lblCutoffStart
        '
        lblCutoffStart.Location = New System.Drawing.Point(290, 34)
        lblCutoffStart.Name = "lblCutoffStart"
        lblCutoffStart.Size = New System.Drawing.Size(22, 13)
        lblCutoffStart.TabIndex = 4
        lblCutoffStart.Text = "Start"
        '
        ' dateCutoffStart
        '
        dateCutoffStart.EditValue = Nothing
        dateCutoffStart.Location = New System.Drawing.Point(290, 51)
        dateCutoffStart.Name = "dateCutoffStart"
        dateCutoffStart.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dateCutoffStart.Size = New System.Drawing.Size(140, 20)
        dateCutoffStart.TabIndex = 5
        '
        ' lblCutoffEnd
        '
        lblCutoffEnd.Location = New System.Drawing.Point(440, 34)
        lblCutoffEnd.Name = "lblCutoffEnd"
        lblCutoffEnd.Size = New System.Drawing.Size(18, 13)
        lblCutoffEnd.TabIndex = 6
        lblCutoffEnd.Text = "End"
        '
        ' dateCutoffEnd
        '
        dateCutoffEnd.EditValue = Nothing
        dateCutoffEnd.Location = New System.Drawing.Point(440, 51)
        dateCutoffEnd.Name = "dateCutoffEnd"
        dateCutoffEnd.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dateCutoffEnd.Size = New System.Drawing.Size(140, 20)
        dateCutoffEnd.TabIndex = 7
        '
        ' lblPayDate
        '
        lblPayDate.Location = New System.Drawing.Point(590, 34)
        lblPayDate.Name = "lblPayDate"
        lblPayDate.Size = New System.Drawing.Size(46, 13)
        lblPayDate.TabIndex = 8
        lblPayDate.Text = "Pay Date"
        '
        ' datePayDate
        '
        datePayDate.EditValue = Nothing
        datePayDate.Location = New System.Drawing.Point(590, 51)
        datePayDate.Name = "datePayDate"
        datePayDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        datePayDate.Properties.NullText = "(optional)"
        datePayDate.Size = New System.Drawing.Size(140, 20)
        datePayDate.TabIndex = 9
        '
        ' lblLabel
        '
        lblLabel.Location = New System.Drawing.Point(740, 34)
        lblLabel.Name = "lblLabel"
        lblLabel.Size = New System.Drawing.Size(27, 13)
        lblLabel.TabIndex = 10
        lblLabel.Text = "Label"
        '
        ' txtCutoffLabel
        '
        txtCutoffLabel.Location = New System.Drawing.Point(740, 51)
        txtCutoffLabel.Name = "txtCutoffLabel"
        txtCutoffLabel.Properties.NullValuePrompt = "(auto if left blank)"
        txtCutoffLabel.Size = New System.Drawing.Size(250, 20)
        txtCutoffLabel.TabIndex = 11
        '
        ' lblStatus
        '
        lblStatus.Location = New System.Drawing.Point(24, 85)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New System.Drawing.Size(34, 13)
        lblStatus.TabIndex = 12
        lblStatus.Text = "Status:"
        '
        ' lblStatusValue
        '
        cboStatus.Location = New System.Drawing.Point(64, 82)
        cboStatus.Name = "cboStatus"
        cboStatus.Properties.Items.AddRange(New Object() {"Draft", "Processed", "Posted", "Closed"})
        cboStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboStatus.Size = New System.Drawing.Size(120, 20)
        cboStatus.TabIndex = 13
        cboStatus.Text = "Draft"
        cboStatus.ToolTip = "Draft / Closed lang ang pwedeng palitan dito. Ang Processed at Posted ay itinatakda ng payroll processing."
        '
        ' grpFilters — Pay Cycle + Year lang ang ipapakita sa grid (hindi lahat ng generated)
        '
        grpFilters.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpFilters.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpFilters.AppearanceCaption.Options.UseFont = True
        grpFilters.Controls.Add(cboFilterYear)
        grpFilters.Controls.Add(lblFilterYear)
        grpFilters.Controls.Add(cboFilterCycle)
        grpFilters.Controls.Add(lblFilterCycle)
        grpFilters.Dock = System.Windows.Forms.DockStyle.Top
        grpFilters.Location = New System.Drawing.Point(4, 184)
        grpFilters.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        grpFilters.Name = "grpFilters"
        grpFilters.Size = New System.Drawing.Size(1092, 62)
        grpFilters.TabIndex = 3
        grpFilters.Text = " FILTER"
        '
        ' lblFilterCycle
        '
        lblFilterCycle.Location = New System.Drawing.Point(24, 36)
        lblFilterCycle.Name = "lblFilterCycle"
        lblFilterCycle.Size = New System.Drawing.Size(47, 13)
        lblFilterCycle.TabIndex = 0
        lblFilterCycle.Text = "Pay cycle"
        '
        ' cboFilterCycle
        '
        cboFilterCycle.Location = New System.Drawing.Point(80, 33)
        cboFilterCycle.Name = "cboFilterCycle"
        cboFilterCycle.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboFilterCycle.Size = New System.Drawing.Size(150, 20)
        cboFilterCycle.TabIndex = 1
        '
        ' lblFilterYear
        '
        lblFilterYear.Location = New System.Drawing.Point(260, 36)
        lblFilterYear.Name = "lblFilterYear"
        lblFilterYear.Size = New System.Drawing.Size(21, 13)
        lblFilterYear.TabIndex = 2
        lblFilterYear.Text = "Year"
        '
        ' cboFilterYear
        '
        cboFilterYear.Location = New System.Drawing.Point(292, 33)
        cboFilterYear.Name = "cboFilterYear"
        cboFilterYear.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboFilterYear.Size = New System.Drawing.Size(100, 20)
        cboFilterYear.TabIndex = 3
        '
        ' gridControl
        '
        gridControl.Dock = System.Windows.Forms.DockStyle.Fill
        gridControl.Location = New System.Drawing.Point(4, 246)
        gridControl.MainView = gridView
        gridControl.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridControl.Name = "gridControl"
        gridControl.Size = New System.Drawing.Size(1092, 284)
        gridControl.TabIndex = 2
        gridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridView})
        '
        ' gridView
        '
        gridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colCycleType, colCutoffYear, colCutoffStart, colCutoffEnd, colPayDate, colCutoffLabel, colStatus})
        gridView.GridControl = gridControl
        gridView.Name = "gridView"
        gridView.OptionsBehavior.Editable = False
        gridView.OptionsSelection.EnableAppearanceFocusedCell = False
        gridView.OptionsView.ShowGroupPanel = False
        '
        ' colCycleType
        '
        colCycleType.Caption = "Cycle Type"
        colCycleType.FieldName = "CycleType"
        colCycleType.Name = "colCycleType"
        colCycleType.Visible = True
        colCycleType.VisibleIndex = 0
        '
        ' colCutoffYear
        '
        colCutoffYear.Caption = "Year"
        colCutoffYear.FieldName = "CutoffYear"
        colCutoffYear.Name = "colCutoffYear"
        colCutoffYear.Visible = True
        colCutoffYear.VisibleIndex = 1
        '
        ' colCutoffStart
        '
        colCutoffStart.Caption = "Start"
        colCutoffStart.FieldName = "CutoffStart"
        colCutoffStart.Name = "colCutoffStart"
        colCutoffStart.Visible = True
        colCutoffStart.VisibleIndex = 2
        '
        ' colCutoffEnd
        '
        colCutoffEnd.Caption = "End"
        colCutoffEnd.FieldName = "CutoffEnd"
        colCutoffEnd.Name = "colCutoffEnd"
        colCutoffEnd.Visible = True
        colCutoffEnd.VisibleIndex = 3
        '
        ' colPayDate
        '
        colPayDate.Caption = "Pay Date"
        colPayDate.FieldName = "PayDate"
        colPayDate.Name = "colPayDate"
        colPayDate.Visible = True
        colPayDate.VisibleIndex = 4
        '
        ' colCutoffLabel
        '
        colCutoffLabel.Caption = "Label"
        colCutoffLabel.FieldName = "CutoffLabel"
        colCutoffLabel.Name = "colCutoffLabel"
        colCutoffLabel.Visible = True
        colCutoffLabel.VisibleIndex = 5
        '
        ' colStatus
        '
        colStatus.Caption = "Status"
        colStatus.FieldName = "Status"
        colStatus.Name = "colStatus"
        colStatus.Visible = True
        colStatus.VisibleIndex = 6
        '
        ' ucCutoff
        '
        Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(gridControl)
        Me.Controls.Add(grpFilters)
        Me.Controls.Add(grpDetails)
        Me.Controls.Add(PanelControl1)
        Me.Name = "ucCutoff"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(1100, 534)
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpFilters, System.ComponentModel.ISupportInitialize).EndInit()
        grpFilters.ResumeLayout(False)
        grpFilters.PerformLayout()
        CType(cboFilterCycle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cboFilterYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(grpDetails, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cboStatus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(txtCutoffLabel.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(datePayDate.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(datePayDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffEnd.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffEnd.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffStart.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffStart.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(numCutoffYear.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cboCycleType.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents grpFilters As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblFilterCycle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboFilterCycle As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblFilterYear As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboFilterYear As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblCycleType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboCycleType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblCutoffYear As DevExpress.XtraEditors.LabelControl
    Friend WithEvents numCutoffYear As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblCutoffStart As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dateCutoffStart As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblCutoffEnd As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dateCutoffEnd As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblPayDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents datePayDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblLabel As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCutoffLabel As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboStatus As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents gridControl As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCycleType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCutoffYear As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCutoffStart As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCutoffEnd As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCutoffLabel As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStatus As DevExpress.XtraGrid.Columns.GridColumn

End Class