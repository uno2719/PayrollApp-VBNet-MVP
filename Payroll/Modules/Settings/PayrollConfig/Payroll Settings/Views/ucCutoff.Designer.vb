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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucCutoff))
        Dim WindowsuiButtonImageOptions4 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        lblStatusValue = New DevExpress.XtraEditors.LabelControl()
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
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(txtCutoffLabel.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(datePayDate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(datePayDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffEnd.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffEnd.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffStart.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dateCutoffStart.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(numCutoffYear.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboCycleType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridControl, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridView, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(1092, 70)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.ForeColor = Color.Black
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Appearance.Options.UseForeColor = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(73, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "CUTOFF"
        ' 
        ' wbpMainCommands
        ' 
        wbpMainCommands.ButtonInterval = 15
        WindowsuiButtonImageOptions1.Image = My.Resources.Resources.icon_add_property_24_png
        WindowsuiButtonImageOptions2.Image = My.Resources.Resources.icon_edit_property_24
        WindowsuiButtonImageOptions3.Image = CType(resources.GetObject("WindowsuiButtonImageOptions3.Image"), Image)
        WindowsuiButtonImageOptions4.Image = My.Resources.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New Cutoff", -1, True, Nothing, True, False, True, "New", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Generate for Year...", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Batch-create a full year of Cutoffs", -1, True, Nothing, True, False, True, "Generate", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(458, 4)
        wbpMainCommands.Margin = New Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(629, 62)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        ' 
        ' grpDetails
        ' 
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(lblStatusValue)
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
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(1092, 110)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        ' 
        ' lblStatusValue
        ' 
        lblStatusValue.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatusValue.Appearance.Options.UseFont = True
        lblStatusValue.Location = New Point(64, 85)
        lblStatusValue.Name = "lblStatusValue"
        lblStatusValue.Size = New Size(30, 15)
        lblStatusValue.TabIndex = 13
        lblStatusValue.Text = "Draft"
        ' 
        ' lblStatus
        ' 
        lblStatus.Location = New Point(24, 85)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(35, 13)
        lblStatus.TabIndex = 12
        lblStatus.Text = "Status:"
        ' 
        ' txtCutoffLabel
        ' 
        txtCutoffLabel.Location = New Point(740, 51)
        txtCutoffLabel.Name = "txtCutoffLabel"
        txtCutoffLabel.Properties.NullValuePrompt = "(auto if left blank)"
        txtCutoffLabel.Size = New Size(250, 20)
        txtCutoffLabel.TabIndex = 11
        ' 
        ' lblLabel
        ' 
        lblLabel.Location = New Point(740, 34)
        lblLabel.Name = "lblLabel"
        lblLabel.Size = New Size(25, 13)
        lblLabel.TabIndex = 10
        lblLabel.Text = "Label"
        ' 
        ' datePayDate
        ' 
        datePayDate.EditValue = Nothing
        datePayDate.Location = New Point(590, 51)
        datePayDate.Name = "datePayDate"
        datePayDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        datePayDate.Properties.NullText = "(optional)"
        datePayDate.Size = New Size(140, 20)
        datePayDate.TabIndex = 9
        ' 
        ' lblPayDate
        ' 
        lblPayDate.Location = New Point(590, 34)
        lblPayDate.Name = "lblPayDate"
        lblPayDate.Size = New Size(44, 13)
        lblPayDate.TabIndex = 8
        lblPayDate.Text = "Pay Date"
        ' 
        ' dateCutoffEnd
        ' 
        dateCutoffEnd.EditValue = Nothing
        dateCutoffEnd.Location = New Point(440, 51)
        dateCutoffEnd.Name = "dateCutoffEnd"
        dateCutoffEnd.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dateCutoffEnd.Size = New Size(140, 20)
        dateCutoffEnd.TabIndex = 7
        ' 
        ' lblCutoffEnd
        ' 
        lblCutoffEnd.Location = New Point(440, 34)
        lblCutoffEnd.Name = "lblCutoffEnd"
        lblCutoffEnd.Size = New Size(18, 13)
        lblCutoffEnd.TabIndex = 6
        lblCutoffEnd.Text = "End"
        ' 
        ' dateCutoffStart
        ' 
        dateCutoffStart.EditValue = Nothing
        dateCutoffStart.Location = New Point(290, 51)
        dateCutoffStart.Name = "dateCutoffStart"
        dateCutoffStart.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dateCutoffStart.Size = New Size(140, 20)
        dateCutoffStart.TabIndex = 5
        ' 
        ' lblCutoffStart
        ' 
        lblCutoffStart.Location = New Point(290, 34)
        lblCutoffStart.Name = "lblCutoffStart"
        lblCutoffStart.Size = New Size(24, 13)
        lblCutoffStart.TabIndex = 4
        lblCutoffStart.Text = "Start"
        ' 
        ' numCutoffYear
        ' 
        numCutoffYear.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        numCutoffYear.Location = New Point(190, 51)
        numCutoffYear.Name = "numCutoffYear"
        numCutoffYear.Properties.Mask.EditMask = "N0"
        numCutoffYear.Properties.MaxValue = New Decimal(New Integer() {2100, 0, 0, 0})
        numCutoffYear.Properties.MinValue = New Decimal(New Integer() {2000, 0, 0, 0})
        numCutoffYear.Size = New Size(80, 20)
        numCutoffYear.TabIndex = 3
        ' 
        ' lblCutoffYear
        ' 
        lblCutoffYear.Location = New Point(190, 34)
        lblCutoffYear.Name = "lblCutoffYear"
        lblCutoffYear.Size = New Size(22, 13)
        lblCutoffYear.TabIndex = 2
        lblCutoffYear.Text = "Year"
        ' 
        ' cboCycleType
        ' 
        cboCycleType.Location = New Point(24, 51)
        cboCycleType.Name = "cboCycleType"
        cboCycleType.Properties.Items.AddRange(New Object() {"Monthly", "SemiMonthly", "Weekly", "Daily"})
        cboCycleType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboCycleType.Size = New Size(150, 20)
        cboCycleType.TabIndex = 1
        ' 
        ' lblCycleType
        ' 
        lblCycleType.Location = New Point(24, 34)
        lblCycleType.Name = "lblCycleType"
        lblCycleType.Size = New Size(53, 13)
        lblCycleType.TabIndex = 0
        lblCycleType.Text = "Cycle Type"
        ' 
        ' gridControl
        ' 
        gridControl.Dock = DockStyle.Fill
        gridControl.Location = New Point(4, 184)
        gridControl.MainView = gridView
        gridControl.Margin = New Padding(3, 2, 3, 2)
        gridControl.Name = "gridControl"
        gridControl.Size = New Size(1092, 346)
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
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridControl)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucCutoff"
        Padding = New Padding(4)
        Size = New Size(1100, 534)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(txtCutoffLabel.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(datePayDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(datePayDate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffEnd.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffEnd.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffStart.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dateCutoffStart.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(numCutoffYear.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboCycleType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridControl, ComponentModel.ISupportInitialize).EndInit()
        CType(gridView, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
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
    Friend WithEvents lblStatusValue As DevExpress.XtraEditors.LabelControl
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