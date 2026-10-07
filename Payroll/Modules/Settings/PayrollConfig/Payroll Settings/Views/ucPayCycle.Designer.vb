<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayCycle
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
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        gridCycles = New DevExpress.XtraGrid.GridControl()
        gridViewCycles = New DevExpress.XtraGrid.Views.Grid.GridView()
        colCycleType = New DevExpress.XtraGrid.Columns.GridColumn()
        colCycleBasis = New DevExpress.XtraGrid.Columns.GridColumn()
        colCycleActive = New DevExpress.XtraGrid.Columns.GridColumn()
        pnlRight = New DevExpress.XtraEditors.PanelControl()
        grpPattern = New DevExpress.XtraEditors.GroupControl()
        gridControlPeriods = New DevExpress.XtraGrid.GridControl()
        gridViewPeriods = New DevExpress.XtraGrid.Views.Grid.GridView()
        colPeriodNo = New DevExpress.XtraGrid.Columns.GridColumn()
        colFromDay = New DevExpress.XtraGrid.Columns.GridColumn()
        colFromMonth = New DevExpress.XtraGrid.Columns.GridColumn()
        colToDay = New DevExpress.XtraGrid.Columns.GridColumn()
        colToMonth = New DevExpress.XtraGrid.Columns.GridColumn()
        colPayDay = New DevExpress.XtraGrid.Columns.GridColumn()
        repoDay = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        repoMonth = New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        lblPreview = New DevExpress.XtraEditors.LabelControl()
        pnlPatternTools = New DevExpress.XtraEditors.PanelControl()
        btnAddPeriod = New DevExpress.XtraEditors.SimpleButton()
        btnRemovePeriod = New DevExpress.XtraEditors.SimpleButton()
        lblPatternHint = New DevExpress.XtraEditors.LabelControl()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        lblPayCycle = New DevExpress.XtraEditors.LabelControl()
        txtPayCycle = New DevExpress.XtraEditors.TextEdit()
        lblRateBasis = New DevExpress.XtraEditors.LabelControl()
        cboRateBasis = New DevExpress.XtraEditors.ComboBoxEdit()
        chkActive = New DevExpress.XtraEditors.CheckEdit()
        lblRateNote = New DevExpress.XtraEditors.LabelControl()
        lblLockNote = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(gridCycles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewCycles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlRight, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlRight.SuspendLayout()
        CType(grpPattern, System.ComponentModel.ISupportInitialize).BeginInit()
        grpPattern.SuspendLayout()
        CType(gridControlPeriods, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewPeriods, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(repoDay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(repoMonth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlPatternTools, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlPatternTools.SuspendLayout()
        CType(grpDetails, System.ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(txtPayCycle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(cboRateBasis.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(chkActive.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' PanelControl1 - same WindowsUIButtonPanel command-bar convention as ucCutoff
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
        lblTabPageTitle.Size = New System.Drawing.Size(110, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "PAY CYCLE"
        '
        ' wbpMainCommands - [0]=sep, [1]=Edit/Update, [2]=Cancel, [3]=sep, [4]=Refresh, [5]=sep
        '
        wbpMainCommands.ButtonInterval = 15
        WindowsuiButtonImageOptions1.Image = My.Resources.Resources.icon_edit_property_24
        WindowsuiButtonImageOptions2.Image = My.Resources.Resources.icon_cancel_24
        WindowsuiButtonImageOptions3.Image = My.Resources.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Cancel", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Cancel", -1, True, Nothing, True, False, True, "Cancel", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight
        wbpMainCommands.Dock = System.Windows.Forms.DockStyle.Right
        wbpMainCommands.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New System.Drawing.Point(609, 4)
        wbpMainCommands.Margin = New System.Windows.Forms.Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New System.Drawing.Size(479, 62)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        '
        ' gridCycles - listahan ng 4 na pay cycle (kaliwa)
        '
        gridCycles.Dock = System.Windows.Forms.DockStyle.Left
        gridCycles.Location = New System.Drawing.Point(4, 74)
        gridCycles.MainView = gridViewCycles
        gridCycles.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridCycles.Name = "gridCycles"
        gridCycles.Size = New System.Drawing.Size(300, 456)
        gridCycles.TabIndex = 1
        gridCycles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewCycles})
        '
        ' gridViewCycles
        '
        gridViewCycles.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colCycleType, colCycleBasis, colCycleActive})
        gridViewCycles.GridControl = gridCycles
        gridViewCycles.Name = "gridViewCycles"
        gridViewCycles.OptionsBehavior.Editable = False
        gridViewCycles.OptionsSelection.EnableAppearanceFocusedCell = False
        gridViewCycles.OptionsView.ShowGroupPanel = False
        '
        ' colCycleType
        '
        colCycleType.Caption = "Pay cycle"
        colCycleType.FieldName = "PayCycleType"
        colCycleType.Name = "colCycleType"
        colCycleType.Visible = True
        colCycleType.VisibleIndex = 0
        colCycleType.Width = 100
        '
        ' colCycleBasis
        '
        colCycleBasis.Caption = "Rate basis"
        colCycleBasis.FieldName = "RateBasisText"
        colCycleBasis.Name = "colCycleBasis"
        colCycleBasis.Visible = True
        colCycleBasis.VisibleIndex = 1
        colCycleBasis.Width = 140
        '
        ' colCycleActive
        '
        colCycleActive.Caption = "Active"
        colCycleActive.FieldName = "ActiveText"
        colCycleActive.Name = "colCycleActive"
        colCycleActive.Visible = True
        colCycleActive.VisibleIndex = 2
        colCycleActive.Width = 50
        '
        ' pnlRight - detalye + cutoff pattern ng napiling pay cycle
        '
        pnlRight.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlRight.Controls.Add(grpPattern)
        pnlRight.Controls.Add(grpDetails)
        pnlRight.Dock = System.Windows.Forms.DockStyle.Fill
        pnlRight.Location = New System.Drawing.Point(304, 74)
        pnlRight.Name = "pnlRight"
        pnlRight.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        pnlRight.Size = New System.Drawing.Size(792, 456)
        pnlRight.TabIndex = 2
        '
        ' grpPattern
        '
        grpPattern.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpPattern.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpPattern.AppearanceCaption.Options.UseFont = True
        grpPattern.Controls.Add(gridControlPeriods)
        grpPattern.Controls.Add(lblPreview)
        grpPattern.Controls.Add(pnlPatternTools)
        grpPattern.Dock = System.Windows.Forms.DockStyle.Fill
        grpPattern.Location = New System.Drawing.Point(8, 150)
        grpPattern.Name = "grpPattern"
        grpPattern.Size = New System.Drawing.Size(784, 306)
        grpPattern.TabIndex = 1
        grpPattern.Text = " CUTOFF PATTERN (dates are relative to the pay month)"
        '
        ' gridControlPeriods
        '
        gridControlPeriods.Dock = System.Windows.Forms.DockStyle.Fill
        gridControlPeriods.Location = New System.Drawing.Point(2, 60)
        gridControlPeriods.MainView = gridViewPeriods
        gridControlPeriods.Name = "gridControlPeriods"
        gridControlPeriods.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {repoDay, repoMonth})
        gridControlPeriods.Size = New System.Drawing.Size(780, 200)
        gridControlPeriods.TabIndex = 0
        gridControlPeriods.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewPeriods})
        '
        ' gridViewPeriods
        '
        gridViewPeriods.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colPeriodNo, colFromDay, colFromMonth, colToDay, colToMonth, colPayDay})
        gridViewPeriods.GridControl = gridControlPeriods
        gridViewPeriods.Name = "gridViewPeriods"
        gridViewPeriods.OptionsView.ShowGroupPanel = False
        '
        ' colPeriodNo
        '
        colPeriodNo.Caption = "#"
        colPeriodNo.FieldName = "PeriodNo"
        colPeriodNo.Name = "colPeriodNo"
        colPeriodNo.OptionsColumn.AllowEdit = False
        colPeriodNo.Visible = True
        colPeriodNo.VisibleIndex = 0
        colPeriodNo.Width = 40
        '
        ' colFromDay
        '
        colFromDay.Caption = "From day"
        colFromDay.ColumnEdit = repoDay
        colFromDay.FieldName = "FromDay"
        colFromDay.Name = "colFromDay"
        colFromDay.Visible = True
        colFromDay.VisibleIndex = 1
        colFromDay.Width = 90
        '
        ' colFromMonth
        '
        colFromMonth.Caption = "From month"
        colFromMonth.ColumnEdit = repoMonth
        colFromMonth.FieldName = "FromMonthOffset"
        colFromMonth.Name = "colFromMonth"
        colFromMonth.Visible = True
        colFromMonth.VisibleIndex = 2
        colFromMonth.Width = 130
        '
        ' colToDay
        '
        colToDay.Caption = "To day"
        colToDay.ColumnEdit = repoDay
        colToDay.FieldName = "ToDay"
        colToDay.Name = "colToDay"
        colToDay.Visible = True
        colToDay.VisibleIndex = 3
        colToDay.Width = 90
        '
        ' colToMonth
        '
        colToMonth.Caption = "To month"
        colToMonth.ColumnEdit = repoMonth
        colToMonth.FieldName = "ToMonthOffset"
        colToMonth.Name = "colToMonth"
        colToMonth.Visible = True
        colToMonth.VisibleIndex = 4
        colToMonth.Width = 130
        '
        ' colPayDay
        '
        colPayDay.Caption = "Pay day"
        colPayDay.ColumnEdit = repoDay
        colPayDay.FieldName = "PayDay"
        colPayDay.Name = "colPayDay"
        colPayDay.Visible = True
        colPayDay.VisibleIndex = 5
        colPayDay.Width = 90
        '
        ' repoDay - 1..30 at EOM (naka-configure sa code-behind)
        '
        repoDay.AutoHeight = False
        repoDay.Name = "repoDay"
        '
        ' repoMonth - Previous / This / Next month (naka-configure sa code-behind)
        '
        repoMonth.AutoHeight = False
        repoMonth.Name = "repoMonth"
        '
        ' lblPreview
        '
        lblPreview.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblPreview.Appearance.Options.UseForeColor = True
        lblPreview.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblPreview.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblPreview.Dock = System.Windows.Forms.DockStyle.Bottom
        lblPreview.Location = New System.Drawing.Point(2, 260)
        lblPreview.Name = "lblPreview"
        lblPreview.Padding = New System.Windows.Forms.Padding(8, 6, 8, 6)
        lblPreview.Size = New System.Drawing.Size(780, 44)
        lblPreview.TabIndex = 2
        '
        ' pnlPatternTools
        '
        pnlPatternTools.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlPatternTools.Controls.Add(btnAddPeriod)
        pnlPatternTools.Controls.Add(btnRemovePeriod)
        pnlPatternTools.Controls.Add(lblPatternHint)
        pnlPatternTools.Dock = System.Windows.Forms.DockStyle.Top
        pnlPatternTools.Location = New System.Drawing.Point(2, 23)
        pnlPatternTools.Name = "pnlPatternTools"
        pnlPatternTools.Size = New System.Drawing.Size(780, 37)
        pnlPatternTools.TabIndex = 1
        '
        ' btnAddPeriod
        '
        btnAddPeriod.Location = New System.Drawing.Point(6, 5)
        btnAddPeriod.Name = "btnAddPeriod"
        btnAddPeriod.Size = New System.Drawing.Size(100, 26)
        btnAddPeriod.TabIndex = 0
        btnAddPeriod.Text = "Add period"
        '
        ' btnRemovePeriod
        '
        btnRemovePeriod.Location = New System.Drawing.Point(112, 5)
        btnRemovePeriod.Name = "btnRemovePeriod"
        btnRemovePeriod.Size = New System.Drawing.Size(110, 26)
        btnRemovePeriod.TabIndex = 1
        btnRemovePeriod.Text = "Remove period"
        '
        ' lblPatternHint
        '
        lblPatternHint.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblPatternHint.Appearance.Options.UseForeColor = True
        lblPatternHint.Location = New System.Drawing.Point(240, 11)
        lblPatternHint.Name = "lblPatternHint"
        lblPatternHint.Size = New System.Drawing.Size(330, 13)
        lblPatternHint.TabIndex = 2
        lblPatternHint.Text = "EOM = end of month. The pay day is always in the pay month."
        '
        ' grpDetails
        '
        grpDetails.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(lblLockNote)
        grpDetails.Controls.Add(lblRateNote)
        grpDetails.Controls.Add(chkActive)
        grpDetails.Controls.Add(cboRateBasis)
        grpDetails.Controls.Add(lblRateBasis)
        grpDetails.Controls.Add(txtPayCycle)
        grpDetails.Controls.Add(lblPayCycle)
        grpDetails.Dock = System.Windows.Forms.DockStyle.Top
        grpDetails.Location = New System.Drawing.Point(8, 0)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New System.Drawing.Size(784, 150)
        grpDetails.TabIndex = 0
        grpDetails.Text = " DETAILS"
        '
        ' lblPayCycle
        '
        lblPayCycle.Location = New System.Drawing.Point(16, 36)
        lblPayCycle.Name = "lblPayCycle"
        lblPayCycle.Size = New System.Drawing.Size(47, 13)
        lblPayCycle.TabIndex = 0
        lblPayCycle.Text = "Pay cycle"
        '
        ' txtPayCycle
        '
        txtPayCycle.Location = New System.Drawing.Point(16, 53)
        txtPayCycle.Name = "txtPayCycle"
        txtPayCycle.Properties.ReadOnly = True
        txtPayCycle.Size = New System.Drawing.Size(140, 20)
        txtPayCycle.TabIndex = 1
        '
        ' lblRateBasis
        '
        lblRateBasis.Location = New System.Drawing.Point(176, 36)
        lblRateBasis.Name = "lblRateBasis"
        lblRateBasis.Size = New System.Drawing.Size(51, 13)
        lblRateBasis.TabIndex = 2
        lblRateBasis.Text = "Rate basis"
        '
        ' cboRateBasis
        '
        cboRateBasis.Location = New System.Drawing.Point(176, 53)
        cboRateBasis.Name = "cboRateBasis"
        cboRateBasis.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboRateBasis.Size = New System.Drawing.Size(210, 20)
        cboRateBasis.TabIndex = 3
        '
        ' chkActive
        '
        chkActive.Location = New System.Drawing.Point(408, 52)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active (available for cutoff generation)"
        chkActive.Size = New System.Drawing.Size(260, 20)
        chkActive.TabIndex = 4
        '
        ' lblRateNote
        '
        lblRateNote.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblRateNote.Appearance.Options.UseForeColor = True
        lblRateNote.Location = New System.Drawing.Point(16, 90)
        lblRateNote.Name = "lblRateNote"
        lblRateNote.Size = New System.Drawing.Size(700, 13)
        lblRateNote.TabIndex = 5
        '
        ' lblLockNote
        '
        lblLockNote.Appearance.ForeColor = System.Drawing.Color.DarkOrange
        lblLockNote.Appearance.Options.UseForeColor = True
        lblLockNote.Location = New System.Drawing.Point(16, 112)
        lblLockNote.Name = "lblLockNote"
        lblLockNote.Size = New System.Drawing.Size(700, 13)
        lblLockNote.TabIndex = 6
        lblLockNote.Visible = False
        '
        ' ucPayCycle
        '
        Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(pnlRight)
        Me.Controls.Add(gridCycles)
        Me.Controls.Add(PanelControl1)
        Me.Name = "ucPayCycle"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(1100, 534)
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(gridCycles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewCycles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlRight, System.ComponentModel.ISupportInitialize).EndInit()
        pnlRight.ResumeLayout(False)
        CType(grpPattern, System.ComponentModel.ISupportInitialize).EndInit()
        grpPattern.ResumeLayout(False)
        grpPattern.PerformLayout()
        CType(gridControlPeriods, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewPeriods, System.ComponentModel.ISupportInitialize).EndInit()
        CType(repoDay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(repoMonth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlPatternTools, System.ComponentModel.ISupportInitialize).EndInit()
        pnlPatternTools.ResumeLayout(False)
        pnlPatternTools.PerformLayout()
        CType(grpDetails, System.ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(txtPayCycle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(cboRateBasis.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(chkActive.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents gridCycles As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewCycles As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCycleType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCycleBasis As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCycleActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents pnlRight As DevExpress.XtraEditors.PanelControl
    Friend WithEvents grpPattern As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridControlPeriods As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewPeriods As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPeriodNo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFromDay As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFromMonth As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colToDay As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colToMonth As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayDay As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repoDay As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents repoMonth As DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit
    Friend WithEvents lblPreview As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlPatternTools As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnAddPeriod As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnRemovePeriod As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblPatternHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblPayCycle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPayCycle As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblRateBasis As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboRateBasis As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblRateNote As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblLockNote As DevExpress.XtraEditors.LabelControl

End Class
