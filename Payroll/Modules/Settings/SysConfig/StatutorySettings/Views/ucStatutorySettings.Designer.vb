' ucStatutorySettings.Designer.vb

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucStatutorySettings
    Inherits GlobalShared.Base.ucBase

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim WindowsuiButtonImageOptions1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim WindowsuiButtonImageOptions4 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        chkActive = New DevExpress.XtraEditors.CheckEdit()
        txtSalaryFrom = New DevExpress.XtraEditors.TextEdit()
        lblSalaryFrom = New DevExpress.XtraEditors.LabelControl()
        cboEEContriType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblEEContriType = New DevExpress.XtraEditors.LabelControl()
        txtECCAmount = New DevExpress.XtraEditors.TextEdit()
        lblECCAmount = New DevExpress.XtraEditors.LabelControl()
        txtSalaryTo = New DevExpress.XtraEditors.TextEdit()
        lblSalaryTo = New DevExpress.XtraEditors.LabelControl()
        txtERShare = New DevExpress.XtraEditors.TextEdit()
        lblERShare = New DevExpress.XtraEditors.LabelControl()
        txtEEMPF = New DevExpress.XtraEditors.TextEdit()
        lblEEMPF = New DevExpress.XtraEditors.LabelControl()
        txtEEShare = New DevExpress.XtraEditors.TextEdit()
        lblEEShare = New DevExpress.XtraEditors.LabelControl()
        cboERContriType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblERContriType = New DevExpress.XtraEditors.LabelControl()
        txtERMPF = New DevExpress.XtraEditors.TextEdit()
        lblERMPF = New DevExpress.XtraEditors.LabelControl()
        gridconStatutoryList = New DevExpress.XtraGrid.GridControl()
        gridviewStatutoryList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colSalaryFrom = New DevExpress.XtraGrid.Columns.GridColumn()
        colSalaryTo = New DevExpress.XtraGrid.Columns.GridColumn()
        colEEShare = New DevExpress.XtraGrid.Columns.GridColumn()
        colEEContriType = New DevExpress.XtraGrid.Columns.GridColumn()
        colERShare = New DevExpress.XtraGrid.Columns.GridColumn()
        colERContriType = New DevExpress.XtraGrid.Columns.GridColumn()
        colECCAmount = New DevExpress.XtraGrid.Columns.GridColumn()
        colEEMPF = New DevExpress.XtraGrid.Columns.GridColumn()
        colERMPF = New DevExpress.XtraGrid.Columns.GridColumn()
        colActive = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSalaryFrom.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboEEContriType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtECCAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSalaryTo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtERShare.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtEEMPF.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtEEShare.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboERContriType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtERMPF.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridconStatutoryList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewStatutoryList, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' PanelControl1 — SAME sa ucLookupMaintenance, walang binago
        '
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(948, 70)
        PanelControl1.TabIndex = 0
        '
        ' wbpMainCommands — SAME buttons (New/Edit/Delete/Refresh)
        '
        wbpMainCommands.ButtonInterval = 15
        WindowsuiButtonImageOptions1.Image = My.Resources.Resources.icon_add_property_24_png
        WindowsuiButtonImageOptions2.Image = My.Resources.Resources.icon_edit_property_24
        WindowsuiButtonImageOptions3.Image = My.Resources.Resources.icon_delete_24
        WindowsuiButtonImageOptions4.Image = My.Resources.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New Entry", -1, True, Nothing, True, False, True, "New", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Delete", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Delete Selected", -1, True, Nothing, True, False, True, "Delete", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(459, 4)
        wbpMainCommands.Margin = New Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(484, 62)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        '
        ' lblTabPageTitle — SAME (papalitan lang ni SetPresenter ang Text: SSS / PhilHealth / Pag-IBIG)
        '
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.ForeColor = Color.Black
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Appearance.Options.UseForeColor = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(131, 30)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "DASHBOARD"
        '
        ' grpDetails — PINALAKI: 9 fields (3 col x 3 row) + Active
        '
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(chkActive)
        grpDetails.Controls.Add(txtSalaryFrom)
        grpDetails.Controls.Add(lblSalaryFrom)
        grpDetails.Controls.Add(cboEEContriType)
        grpDetails.Controls.Add(lblEEContriType)
        grpDetails.Controls.Add(txtECCAmount)
        grpDetails.Controls.Add(lblECCAmount)
        grpDetails.Controls.Add(txtSalaryTo)
        grpDetails.Controls.Add(lblSalaryTo)
        grpDetails.Controls.Add(txtERShare)
        grpDetails.Controls.Add(lblERShare)
        grpDetails.Controls.Add(txtEEMPF)
        grpDetails.Controls.Add(lblEEMPF)
        grpDetails.Controls.Add(txtEEShare)
        grpDetails.Controls.Add(lblEEShare)
        grpDetails.Controls.Add(cboERContriType)
        grpDetails.Controls.Add(lblERContriType)
        grpDetails.Controls.Add(txtERMPF)
        grpDetails.Controls.Add(lblERMPF)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(948, 190)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        '
        ' Row 1 — Salary From | EE Contri Type | ECC Amount | Active
        '
        lblSalaryFrom.Location = New Point(24, 24)
        lblSalaryFrom.Margin = New Padding(3, 2, 3, 2)
        lblSalaryFrom.Name = "lblSalaryFrom"
        lblSalaryFrom.Size = New Size(60, 13)
        lblSalaryFrom.TabIndex = 0
        lblSalaryFrom.Text = "Salary From"
        '
        txtSalaryFrom.Location = New Point(24, 42)
        txtSalaryFrom.Margin = New Padding(3, 2, 3, 2)
        txtSalaryFrom.Name = "txtSalaryFrom"
        txtSalaryFrom.Size = New Size(140, 20)
        txtSalaryFrom.TabIndex = 1
        '
        lblEEContriType.Location = New Point(280, 24)
        lblEEContriType.Margin = New Padding(3, 2, 3, 2)
        lblEEContriType.Name = "lblEEContriType"
        lblEEContriType.Size = New Size(69, 13)
        lblEEContriType.TabIndex = 2
        lblEEContriType.Text = "EE Contri Type"
        '
        cboEEContriType.Location = New Point(280, 42)
        cboEEContriType.Margin = New Padding(3, 2, 3, 2)
        cboEEContriType.Name = "cboEEContriType"
        cboEEContriType.Properties.Items.AddRange(New Object() {"Amount", "Percentage"})
        cboEEContriType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboEEContriType.Size = New Size(160, 20)
        cboEEContriType.TabIndex = 3
        '
        lblECCAmount.Location = New Point(536, 24)
        lblECCAmount.Margin = New Padding(3, 2, 3, 2)
        lblECCAmount.Name = "lblECCAmount"
        lblECCAmount.Size = New Size(60, 13)
        lblECCAmount.TabIndex = 4
        lblECCAmount.Text = "ECC Amount"
        '
        txtECCAmount.Location = New Point(536, 42)
        txtECCAmount.Margin = New Padding(3, 2, 3, 2)
        txtECCAmount.Name = "txtECCAmount"
        txtECCAmount.Size = New Size(140, 20)
        txtECCAmount.TabIndex = 5
        '
        chkActive.Location = New Point(792, 44)
        chkActive.Margin = New Padding(3, 2, 3, 2)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active"
        chkActive.Size = New Size(90, 20)
        chkActive.TabIndex = 6
        '
        ' Row 2 — Salary To | ER Share | EE MPF
        '
        lblSalaryTo.Location = New Point(24, 74)
        lblSalaryTo.Margin = New Padding(3, 2, 3, 2)
        lblSalaryTo.Name = "lblSalaryTo"
        lblSalaryTo.Size = New Size(50, 13)
        lblSalaryTo.TabIndex = 7
        lblSalaryTo.Text = "Salary To"
        '
        txtSalaryTo.Location = New Point(24, 92)
        txtSalaryTo.Margin = New Padding(3, 2, 3, 2)
        txtSalaryTo.Name = "txtSalaryTo"
        txtSalaryTo.Size = New Size(140, 20)
        txtSalaryTo.TabIndex = 8
        '
        lblERShare.Location = New Point(280, 74)
        lblERShare.Margin = New Padding(3, 2, 3, 2)
        lblERShare.Name = "lblERShare"
        lblERShare.Size = New Size(47, 13)
        lblERShare.TabIndex = 9
        lblERShare.Text = "ER Share"
        '
        txtERShare.Location = New Point(280, 92)
        txtERShare.Margin = New Padding(3, 2, 3, 2)
        txtERShare.Name = "txtERShare"
        txtERShare.Size = New Size(160, 20)
        txtERShare.TabIndex = 10
        '
        lblEEMPF.Location = New Point(536, 74)
        lblEEMPF.Margin = New Padding(3, 2, 3, 2)
        lblEEMPF.Name = "lblEEMPF"
        lblEEMPF.Size = New Size(42, 13)
        lblEEMPF.TabIndex = 11
        lblEEMPF.Text = "EE MPF"
        '
        txtEEMPF.Location = New Point(536, 92)
        txtEEMPF.Margin = New Padding(3, 2, 3, 2)
        txtEEMPF.Name = "txtEEMPF"
        txtEEMPF.Size = New Size(140, 20)
        txtEEMPF.TabIndex = 12
        '
        ' Row 3 — EE Share | ER Contri Type | ER MPF
        '
        lblEEShare.Location = New Point(24, 124)
        lblEEShare.Margin = New Padding(3, 2, 3, 2)
        lblEEShare.Name = "lblEEShare"
        lblEEShare.Size = New Size(46, 13)
        lblEEShare.TabIndex = 13
        lblEEShare.Text = "EE Share"
        '
        txtEEShare.Location = New Point(24, 142)
        txtEEShare.Margin = New Padding(3, 2, 3, 2)
        txtEEShare.Name = "txtEEShare"
        txtEEShare.Size = New Size(140, 20)
        txtEEShare.TabIndex = 14
        '
        lblERContriType.Location = New Point(280, 124)
        lblERContriType.Margin = New Padding(3, 2, 3, 2)
        lblERContriType.Name = "lblERContriType"
        lblERContriType.Size = New Size(68, 13)
        lblERContriType.TabIndex = 15
        lblERContriType.Text = "ER Contri Type"
        '
        cboERContriType.Location = New Point(280, 142)
        cboERContriType.Margin = New Padding(3, 2, 3, 2)
        cboERContriType.Name = "cboERContriType"
        cboERContriType.Properties.Items.AddRange(New Object() {"Amount", "Percentage"})
        cboERContriType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboERContriType.Size = New Size(160, 20)
        cboERContriType.TabIndex = 16
        '
        lblERMPF.Location = New Point(536, 124)
        lblERMPF.Margin = New Padding(3, 2, 3, 2)
        lblERMPF.Name = "lblERMPF"
        lblERMPF.Size = New Size(41, 13)
        lblERMPF.TabIndex = 17
        lblERMPF.Text = "ER MPF"
        '
        txtERMPF.Location = New Point(536, 142)
        txtERMPF.Margin = New Padding(3, 2, 3, 2)
        txtERMPF.Name = "txtERMPF"
        txtERMPF.Size = New Size(140, 20)
        txtERMPF.TabIndex = 18
        '
        ' gridconStatutoryList
        '
        gridconStatutoryList.Dock = DockStyle.Fill
        gridconStatutoryList.Location = New Point(4, 264)
        gridconStatutoryList.MainView = gridviewStatutoryList
        gridconStatutoryList.Margin = New Padding(3, 2, 3, 2)
        gridconStatutoryList.Name = "gridconStatutoryList"
        gridconStatutoryList.Size = New Size(948, 266)
        gridconStatutoryList.TabIndex = 2
        gridconStatutoryList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewStatutoryList})
        '
        ' gridviewStatutoryList
        '
        gridviewStatutoryList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colSalaryFrom, colSalaryTo, colEEShare, colEEContriType, colERShare, colERContriType, colECCAmount, colEEMPF, colERMPF, colActive})
        gridviewStatutoryList.GridControl = gridconStatutoryList
        gridviewStatutoryList.Name = "gridviewStatutoryList"
        gridviewStatutoryList.OptionsPrint.PrintFilterInfo = True
        gridviewStatutoryList.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        gridviewStatutoryList.OptionsView.ShowGroupPanel = False
        '
        colSalaryFrom.Caption = "Salary (FR)"
        colSalaryFrom.FieldName = "SalaryFrom"
        colSalaryFrom.Name = "colSalaryFrom"
        colSalaryFrom.Visible = True
        colSalaryFrom.VisibleIndex = 0
        colSalaryFrom.Width = 100
        '
        colSalaryTo.Caption = "Salary (TO)"
        colSalaryTo.FieldName = "SalaryTo"
        colSalaryTo.Name = "colSalaryTo"
        colSalaryTo.Visible = True
        colSalaryTo.VisibleIndex = 1
        colSalaryTo.Width = 100
        '
        colEEShare.Caption = "Employee (EE) Share"
        colEEShare.FieldName = "EEShare"
        colEEShare.Name = "colEEShare"
        colEEShare.Visible = True
        colEEShare.VisibleIndex = 2
        colEEShare.Width = 90
        '
        colEEContriType.Caption = "EE Contri Type"
        colEEContriType.FieldName = "EEContriType"
        colEEContriType.Name = "colEEContriType"
        colEEContriType.Visible = True
        colEEContriType.VisibleIndex = 3
        colEEContriType.Width = 110
        '
        colERShare.Caption = "Employer (ER) Share"
        colERShare.FieldName = "ERShare"
        colERShare.Name = "colERShare"
        colERShare.Visible = True
        colERShare.VisibleIndex = 4
        colERShare.Width = 90
        '
        colERContriType.Caption = "ER Contri Type"
        colERContriType.FieldName = "ERContriType"
        colERContriType.Name = "colERContriType"
        colERContriType.Visible = True
        colERContriType.VisibleIndex = 5
        colERContriType.Width = 110
        '
        colECCAmount.Caption = "ECC Amount"
        colECCAmount.FieldName = "ECCAmount"
        colECCAmount.Name = "colECCAmount"
        colECCAmount.Visible = True
        colECCAmount.VisibleIndex = 6
        colECCAmount.Width = 90
        '
        colEEMPF.Caption = "EE MPF"
        colEEMPF.FieldName = "EEMPF"
        colEEMPF.Name = "colEEMPF"
        colEEMPF.Visible = True
        colEEMPF.VisibleIndex = 7
        colEEMPF.Width = 80
        '
        colERMPF.Caption = "ER MPF"
        colERMPF.FieldName = "ERMPF"
        colERMPF.Name = "colERMPF"
        colERMPF.Visible = True
        colERMPF.VisibleIndex = 8
        colERMPF.Width = 80
        '
        colActive.Caption = "Active"
        colActive.FieldName = "IsActive"
        colActive.Name = "colActive"
        colActive.Visible = True
        colActive.VisibleIndex = 9
        colActive.Width = 70
        '
        ' ucStatutory
        '
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridconStatutoryList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucStatutorySettings"
        Padding = New Padding(4)
        Size = New Size(956, 534)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSalaryFrom.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboEEContriType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtECCAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSalaryTo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtERShare.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtEEMPF.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtEEShare.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboERContriType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtERMPF.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridconStatutoryList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewStatutoryList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtSalaryFrom As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSalaryFrom As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboEEContriType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblEEContriType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtECCAmount As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblECCAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSalaryTo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSalaryTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtERShare As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblERShare As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtEEMPF As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblEEMPF As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtEEShare As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblEEShare As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboERContriType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblERContriType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtERMPF As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblERMPF As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridconStatutoryList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewStatutoryList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colSalaryFrom As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSalaryTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEEShare As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEEContriType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colERShare As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colERContriType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colECCAmount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colEEMPF As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colERMPF As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colActive As DevExpress.XtraGrid.Columns.GridColumn

End Class