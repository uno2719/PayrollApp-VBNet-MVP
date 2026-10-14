<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucStatutorySchedule
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
        pnlRight = New DevExpress.XtraEditors.PanelControl()
        grpSchedule = New DevExpress.XtraEditors.GroupControl()
        gridSchedule = New DevExpress.XtraGrid.GridControl()
        gridViewSchedule = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlQuickSet = New DevExpress.XtraEditors.PanelControl()
        lblQuickSet = New DevExpress.XtraEditors.LabelControl()
        btnFirst = New DevExpress.XtraEditors.SimpleButton()
        btnLast = New DevExpress.XtraEditors.SimpleButton()
        btnSplit = New DevExpress.XtraEditors.SimpleButton()
        btnNone = New DevExpress.XtraEditors.SimpleButton()
        lblPattern = New DevExpress.XtraEditors.LabelControl()
        grpVoluntary = New DevExpress.XtraEditors.GroupControl()
        lblVoluntary = New DevExpress.XtraEditors.LabelControl()
        cboVoluntaryMode = New DevExpress.XtraEditors.ComboBoxEdit()
        lblVoluntaryNote = New DevExpress.XtraEditors.LabelControl()
        grpPreview = New DevExpress.XtraEditors.GroupControl()
        lblPreview = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(gridCycles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewCycles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlRight, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlRight.SuspendLayout()
        CType(grpSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        grpSchedule.SuspendLayout()
        CType(gridSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(gridViewSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlQuickSet, System.ComponentModel.ISupportInitialize).BeginInit()
        pnlQuickSet.SuspendLayout()
        CType(grpVoluntary, System.ComponentModel.ISupportInitialize).BeginInit()
        grpVoluntary.SuspendLayout()
        CType(cboVoluntaryMode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(grpPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        grpPreview.SuspendLayout()
        Me.SuspendLayout()
        '
        ' PanelControl1 - same WindowsUIButtonPanel command-bar convention as ucPayCycle
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
        lblTabPageTitle.Size = New System.Drawing.Size(230, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "DEDUCTION SCHEDULE"
        '
        ' wbpMainCommands - [0]=sep, [1]=Edit/Update, [2]=Cancel, [3]=sep, [4]=Refresh, [5]=sep
        '
        wbpMainCommands.ButtonInterval = 15
        WindowsuiButtonImageOptions1.Image = My.Resources.Resources.icon_edit_property_24
        WindowsuiButtonImageOptions2.Image = My.Resources.Resources.icon_cancel_24
        WindowsuiButtonImageOptions3.Image = My.Resources.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit the deduction schedule", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Cancel", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Cancel", -1, True, Nothing, True, False, True, "Cancel", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
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
        ' gridCycles - mga active na pay cycle (kaliwa)
        '
        gridCycles.Dock = System.Windows.Forms.DockStyle.Left
        gridCycles.Location = New System.Drawing.Point(4, 74)
        gridCycles.MainView = gridViewCycles
        gridCycles.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        gridCycles.Name = "gridCycles"
        gridCycles.Size = New System.Drawing.Size(180, 456)
        gridCycles.TabIndex = 1
        gridCycles.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewCycles})
        '
        ' gridViewCycles
        '
        gridViewCycles.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colCycleType})
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
        '
        ' pnlRight - matrix + Pag-IBIG voluntary + preview
        '
        pnlRight.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlRight.Controls.Add(grpSchedule)
        pnlRight.Controls.Add(grpVoluntary)
        pnlRight.Controls.Add(grpPreview)
        pnlRight.Dock = System.Windows.Forms.DockStyle.Fill
        pnlRight.Location = New System.Drawing.Point(184, 74)
        pnlRight.Name = "pnlRight"
        pnlRight.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        pnlRight.Size = New System.Drawing.Size(912, 456)
        pnlRight.TabIndex = 2
        '
        ' grpSchedule
        '
        grpSchedule.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpSchedule.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpSchedule.AppearanceCaption.Options.UseFont = True
        grpSchedule.Controls.Add(gridSchedule)
        grpSchedule.Controls.Add(pnlQuickSet)
        grpSchedule.Controls.Add(lblPattern)
        grpSchedule.Dock = System.Windows.Forms.DockStyle.Fill
        grpSchedule.Location = New System.Drawing.Point(8, 0)
        grpSchedule.Name = "grpSchedule"
        grpSchedule.Size = New System.Drawing.Size(904, 206)
        grpSchedule.TabIndex = 0
        grpSchedule.Text = " SHARE OF THE MONTHLY CONTRIBUTION DEDUCTED PER CUTOFF PERIOD (%)"
        '
        ' gridSchedule - matrix: hilera = agency, column = period
        '
        gridSchedule.Dock = System.Windows.Forms.DockStyle.Fill
        gridSchedule.Location = New System.Drawing.Point(2, 46)
        gridSchedule.MainView = gridViewSchedule
        gridSchedule.Name = "gridSchedule"
        gridSchedule.Size = New System.Drawing.Size(900, 114)
        gridSchedule.TabIndex = 0
        gridSchedule.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridViewSchedule})
        '
        ' gridViewSchedule - ang mga column ay binubuo sa code ayon sa bilang ng period ng pay cycle
        '
        gridViewSchedule.GridControl = gridSchedule
        gridViewSchedule.Name = "gridViewSchedule"
        gridViewSchedule.OptionsBehavior.Editable = False
        gridViewSchedule.OptionsCustomization.AllowColumnMoving = False
        gridViewSchedule.OptionsCustomization.AllowFilter = False
        gridViewSchedule.OptionsCustomization.AllowSort = False
        gridViewSchedule.OptionsView.ColumnAutoWidth = False
        gridViewSchedule.OptionsView.ShowGroupPanel = False
        '
        ' pnlQuickSet - mabilis na pag-set ng napiling agency
        '
        pnlQuickSet.Controls.Add(lblQuickSet)
        pnlQuickSet.Controls.Add(btnFirst)
        pnlQuickSet.Controls.Add(btnLast)
        pnlQuickSet.Controls.Add(btnSplit)
        pnlQuickSet.Controls.Add(btnNone)
        pnlQuickSet.Dock = System.Windows.Forms.DockStyle.Bottom
        pnlQuickSet.Location = New System.Drawing.Point(2, 160)
        pnlQuickSet.Name = "pnlQuickSet"
        pnlQuickSet.Size = New System.Drawing.Size(900, 44)
        pnlQuickSet.TabIndex = 1
        '
        ' lblQuickSet
        '
        lblQuickSet.Location = New System.Drawing.Point(10, 15)
        lblQuickSet.Name = "lblQuickSet"
        lblQuickSet.Size = New System.Drawing.Size(168, 13)
        lblQuickSet.TabIndex = 0
        lblQuickSet.Text = "Quick set for the selected agency:"
        '
        ' btnFirst
        '
        btnFirst.Location = New System.Drawing.Point(200, 10)
        btnFirst.Name = "btnFirst"
        btnFirst.Size = New System.Drawing.Size(110, 24)
        btnFirst.TabIndex = 1
        btnFirst.Text = "First period"
        '
        ' btnLast
        '
        btnLast.Location = New System.Drawing.Point(320, 10)
        btnLast.Name = "btnLast"
        btnLast.Size = New System.Drawing.Size(110, 24)
        btnLast.TabIndex = 2
        btnLast.Text = "Last period"
        '
        ' btnSplit
        '
        btnSplit.Location = New System.Drawing.Point(440, 10)
        btnSplit.Name = "btnSplit"
        btnSplit.Size = New System.Drawing.Size(110, 24)
        btnSplit.TabIndex = 3
        btnSplit.Text = "Split evenly"
        '
        ' btnNone
        '
        btnNone.Location = New System.Drawing.Point(560, 10)
        btnNone.Name = "btnNone"
        btnNone.Size = New System.Drawing.Size(110, 24)
        btnNone.TabIndex = 4
        btnNone.Text = "Not deducted"
        '
        ' lblPattern - cutoff pattern ng napiling pay cycle
        '
        lblPattern.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblPattern.Appearance.Options.UseForeColor = True
        lblPattern.Appearance.Options.UseTextOptions = True
        lblPattern.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        lblPattern.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblPattern.Dock = System.Windows.Forms.DockStyle.Top
        lblPattern.Location = New System.Drawing.Point(2, 23)
        lblPattern.Name = "lblPattern"
        lblPattern.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        lblPattern.Size = New System.Drawing.Size(900, 23)
        lblPattern.TabIndex = 2
        '
        ' grpVoluntary - Pag-IBIG voluntary amount
        '
        grpVoluntary.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpVoluntary.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpVoluntary.AppearanceCaption.Options.UseFont = True
        grpVoluntary.Controls.Add(lblVoluntary)
        grpVoluntary.Controls.Add(cboVoluntaryMode)
        grpVoluntary.Controls.Add(lblVoluntaryNote)
        grpVoluntary.Dock = System.Windows.Forms.DockStyle.Bottom
        grpVoluntary.Location = New System.Drawing.Point(8, 206)
        grpVoluntary.Name = "grpVoluntary"
        grpVoluntary.Size = New System.Drawing.Size(904, 110)
        grpVoluntary.TabIndex = 1
        grpVoluntary.Text = " PAG-IBIG VOLUNTARY AMOUNT"
        '
        ' lblVoluntary
        '
        lblVoluntary.Location = New System.Drawing.Point(16, 36)
        lblVoluntary.Name = "lblVoluntary"
        lblVoluntary.Size = New System.Drawing.Size(364, 13)
        lblVoluntary.TabIndex = 0
        lblVoluntary.Text = "For employees with a voluntary amount (set in Employee > Statutory):"
        '
        ' cboVoluntaryMode
        '
        cboVoluntaryMode.Location = New System.Drawing.Point(16, 54)
        cboVoluntaryMode.Name = "cboVoluntaryMode"
        cboVoluntaryMode.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboVoluntaryMode.Size = New System.Drawing.Size(420, 20)
        cboVoluntaryMode.TabIndex = 1
        '
        ' lblVoluntaryNote
        '
        lblVoluntaryNote.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblVoluntaryNote.Appearance.Options.UseForeColor = True
        lblVoluntaryNote.Location = New System.Drawing.Point(16, 82)
        lblVoluntaryNote.Name = "lblVoluntaryNote"
        lblVoluntaryNote.Size = New System.Drawing.Size(0, 13)
        lblVoluntaryNote.TabIndex = 2
        '
        ' grpPreview - ano ang kakaltasin sa bawat payday
        '
        grpPreview.AppearanceCaption.Font = New System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CByte(0))
        grpPreview.AppearanceCaption.FontStyleDelta = System.Drawing.FontStyle.Bold
        grpPreview.AppearanceCaption.Options.UseFont = True
        grpPreview.Controls.Add(lblPreview)
        grpPreview.Dock = System.Windows.Forms.DockStyle.Bottom
        grpPreview.Location = New System.Drawing.Point(8, 316)
        grpPreview.Name = "grpPreview"
        grpPreview.Size = New System.Drawing.Size(904, 140)
        grpPreview.TabIndex = 2
        grpPreview.Text = " WHAT EACH PAYDAY WILL DEDUCT"
        '
        ' lblPreview
        '
        lblPreview.Appearance.ForeColor = System.Drawing.Color.DimGray
        lblPreview.Appearance.Options.UseForeColor = True
        lblPreview.Appearance.Options.UseTextOptions = True
        lblPreview.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
        lblPreview.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblPreview.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblPreview.Dock = System.Windows.Forms.DockStyle.Fill
        lblPreview.Location = New System.Drawing.Point(2, 23)
        lblPreview.Name = "lblPreview"
        lblPreview.Padding = New System.Windows.Forms.Padding(14, 8, 8, 4)
        lblPreview.Size = New System.Drawing.Size(900, 115)
        lblPreview.TabIndex = 0
        '
        ' ucStatutorySchedule
        '
        Appearance.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New System.Drawing.SizeF(6.0F, 13.0F)
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(pnlRight)
        Me.Controls.Add(gridCycles)
        Me.Controls.Add(PanelControl1)
        Me.Name = "ucStatutorySchedule"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.Size = New System.Drawing.Size(1100, 534)
        CType(PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(gridCycles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewCycles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlRight, System.ComponentModel.ISupportInitialize).EndInit()
        pnlRight.ResumeLayout(False)
        CType(grpSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        grpSchedule.ResumeLayout(False)
        CType(gridSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        CType(gridViewSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        CType(pnlQuickSet, System.ComponentModel.ISupportInitialize).EndInit()
        pnlQuickSet.ResumeLayout(False)
        pnlQuickSet.PerformLayout()
        CType(grpVoluntary, System.ComponentModel.ISupportInitialize).EndInit()
        grpVoluntary.ResumeLayout(False)
        grpVoluntary.PerformLayout()
        CType(cboVoluntaryMode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(grpPreview, System.ComponentModel.ISupportInitialize).EndInit()
        grpPreview.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents gridCycles As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewCycles As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCycleType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents pnlRight As DevExpress.XtraEditors.PanelControl
    Friend WithEvents grpSchedule As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridSchedule As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridViewSchedule As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents pnlQuickSet As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblQuickSet As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnFirst As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnLast As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSplit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnNone As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblPattern As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpVoluntary As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblVoluntary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboVoluntaryMode As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblVoluntaryNote As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpPreview As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblPreview As DevExpress.XtraEditors.LabelControl

End Class
