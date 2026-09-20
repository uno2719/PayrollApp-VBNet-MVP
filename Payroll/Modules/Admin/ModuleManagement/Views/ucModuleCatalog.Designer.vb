<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucModuleCatalog
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

        Dim btnImg1 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim btnImg2 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim btnImg3 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim btnImg4 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()

        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()

        grpDetails = New DevExpress.XtraEditors.GroupControl()
        lblModuleCode = New DevExpress.XtraEditors.LabelControl()
        txtModuleCode = New DevExpress.XtraEditors.TextEdit()
        lblModuleName = New DevExpress.XtraEditors.LabelControl()
        txtModuleName = New DevExpress.XtraEditors.TextEdit()
        lblGroupName = New DevExpress.XtraEditors.LabelControl()
        cboGroupName = New DevExpress.XtraEditors.ComboBoxEdit()
        lblSortOrder = New DevExpress.XtraEditors.LabelControl()
        spnSortOrder = New DevExpress.XtraEditors.SpinEdit()
        chkIsActive = New DevExpress.XtraEditors.CheckEdit()
        lblModuleCodeHint = New DevExpress.XtraEditors.LabelControl()

        grpModuleList = New DevExpress.XtraEditors.GroupControl()
        gridconModuleList = New DevExpress.XtraGrid.GridControl()
        gridviewModuleList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colModuleCode = New DevExpress.XtraGrid.Columns.GridColumn()
        colModuleName = New DevExpress.XtraGrid.Columns.GridColumn()
        colGroupName = New DevExpress.XtraGrid.Columns.GridColumn()
        colSortOrder = New DevExpress.XtraGrid.Columns.GridColumn()
        colUsersWithAccessCount = New DevExpress.XtraGrid.Columns.GridColumn()
        colStatusText = New DevExpress.XtraGrid.Columns.GridColumn()

        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(txtModuleCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtModuleName.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboGroupName.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnSortOrder.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkIsActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpModuleList, ComponentModel.ISupportInitialize).BeginInit()
        grpModuleList.SuspendLayout()
        CType(gridconModuleList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewModuleList, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(0, 0)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(960, 60)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 13.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 15)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(120, 25)
        lblTabPageTitle.TabIndex = 1
        lblTabPageTitle.Text = "MODULE CATALOG"
        ' 
        ' wbpMainCommands
        ' 
        ' Index: 0=sep 1=New 2=Edit 3=Delete 4=sep 5=Refresh 6=sep
        wbpMainCommands.ButtonInterval = 15
        btnImg1.Image = My.Resources.icon_add_property_24_png
        btnImg2.Image = My.Resources.icon_edit_property_24
        btnImg3.Image = My.Resources.icon_delete_24
        btnImg4.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, btnImg1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New Module Entry", -1, True, Nothing, True, False, True, "New", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, btnImg2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected Module", -1, True, Nothing, True, False, True, "Edit", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Delete", True, btnImg3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Delete Selected Module", -1, True, Nothing, True, False, True, "Delete", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, btnImg4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Location = New Point(272, 2)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(686, 56)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        ' 
        ' grpDetails
        ' 
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(lblModuleCode)
        grpDetails.Controls.Add(txtModuleCode)
        grpDetails.Controls.Add(lblModuleCodeHint)
        grpDetails.Controls.Add(lblModuleName)
        grpDetails.Controls.Add(txtModuleName)
        grpDetails.Controls.Add(lblGroupName)
        grpDetails.Controls.Add(cboGroupName)
        grpDetails.Controls.Add(lblSortOrder)
        grpDetails.Controls.Add(spnSortOrder)
        grpDetails.Controls.Add(chkIsActive)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(0, 60)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(960, 150)
        grpDetails.TabIndex = 1
        grpDetails.Text = " MODULE DETAILS"
        ' 
        lblModuleCode.Location = New Point(20, 34)
        lblModuleCode.Name = "lblModuleCode"
        lblModuleCode.Size = New Size(63, 13)
        lblModuleCode.TabIndex = 0
        lblModuleCode.Text = "Module Code"
        ' 
        txtModuleCode.Location = New Point(20, 52)
        txtModuleCode.Name = "txtModuleCode"
        txtModuleCode.Properties.MaxLength = 50
        txtModuleCode.Size = New Size(230, 20)
        txtModuleCode.TabIndex = 1
        ' 
        lblModuleCodeHint.Appearance.Font = New Font("Segoe UI", 7.5F, FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        lblModuleCodeHint.Appearance.ForeColor = Color.Gray
        lblModuleCodeHint.Appearance.Options.UseFont = True
        lblModuleCodeHint.Appearance.Options.UseForeColor = True
        lblModuleCodeHint.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblModuleCodeHint.Location = New Point(20, 74)
        lblModuleCodeHint.Name = "lblModuleCodeHint"
        lblModuleCodeHint.Size = New Size(230, 26)
        lblModuleCodeHint.TabIndex = 2
        lblModuleCodeHint.Text = "Must exactly match the Tag of the accordion element in frmMain.Designer.vb."
        ' 
        lblModuleName.Location = New Point(270, 34)
        lblModuleName.Name = "lblModuleName"
        lblModuleName.Size = New Size(68, 13)
        lblModuleName.TabIndex = 3
        lblModuleName.Text = "Module Name"
        ' 
        txtModuleName.Location = New Point(270, 52)
        txtModuleName.Name = "txtModuleName"
        txtModuleName.Properties.MaxLength = 100
        txtModuleName.Size = New Size(280, 20)
        txtModuleName.TabIndex = 4
        ' 
        lblGroupName.Location = New Point(570, 34)
        lblGroupName.Name = "lblGroupName"
        lblGroupName.Size = New Size(61, 13)
        lblGroupName.TabIndex = 5
        lblGroupName.Text = "Group / Menu"
        ' 
        cboGroupName.Location = New Point(570, 52)
        cboGroupName.Name = "cboGroupName"
        cboGroupName.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboGroupName.Size = New Size(180, 20)
        cboGroupName.TabIndex = 6
        ' 
        lblSortOrder.Location = New Point(770, 34)
        lblSortOrder.Name = "lblSortOrder"
        lblSortOrder.Size = New Size(70, 13)
        lblSortOrder.TabIndex = 7
        lblSortOrder.Text = "Sort Order"
        ' 
        spnSortOrder.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnSortOrder.Location = New Point(770, 52)
        spnSortOrder.Name = "spnSortOrder"
        spnSortOrder.Properties.DisplayFormat.FormatString = "n0"
        spnSortOrder.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnSortOrder.Properties.EditFormat.FormatString = "n0"
        spnSortOrder.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnSortOrder.Properties.IsFloatValue = False
        spnSortOrder.Properties.MaxValue = New Decimal(New Integer() {9999, 0, 0, 0})
        spnSortOrder.Size = New Size(120, 20)
        spnSortOrder.TabIndex = 8
        ' 
        chkIsActive.Location = New Point(770, 82)
        chkIsActive.Name = "chkIsActive"
        chkIsActive.Properties.Caption = "Active"
        chkIsActive.Size = New Size(120, 20)
        chkIsActive.TabIndex = 9
        ' 
        ' grpModuleList
        ' 
        grpModuleList.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpModuleList.AppearanceCaption.Options.UseFont = True
        grpModuleList.Controls.Add(gridconModuleList)
        grpModuleList.Dock = DockStyle.Fill
        grpModuleList.Location = New Point(0, 210)
        grpModuleList.Name = "grpModuleList"
        grpModuleList.Size = New Size(960, 370)
        grpModuleList.TabIndex = 2
        grpModuleList.Text = " REGISTERED MODULES"
        ' 
        gridconModuleList.Dock = DockStyle.Fill
        gridconModuleList.Location = New Point(2, 22)
        gridconModuleList.MainView = gridviewModuleList
        gridconModuleList.Name = "gridconModuleList"
        gridconModuleList.Size = New Size(956, 346)
        gridconModuleList.TabIndex = 0
        gridconModuleList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewModuleList})
        ' 
        gridviewModuleList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {
            colModuleCode, colModuleName, colGroupName, colSortOrder,
            colUsersWithAccessCount, colStatusText})
        gridviewModuleList.GridControl = gridconModuleList
        gridviewModuleList.Name = "gridviewModuleList"
        gridviewModuleList.OptionsView.ShowGroupPanel = False
        ' 
        colModuleCode.Caption = "Module Code"
        colModuleCode.FieldName = "ModuleCode"
        colModuleCode.Name = "colModuleCode"
        colModuleCode.Visible = True
        colModuleCode.VisibleIndex = 0
        colModuleCode.Width = 140
        ' 
        colModuleName.Caption = "Module Name"
        colModuleName.FieldName = "ModuleName"
        colModuleName.Name = "colModuleName"
        colModuleName.Visible = True
        colModuleName.VisibleIndex = 1
        colModuleName.Width = 220
        ' 
        colGroupName.Caption = "Group / Menu"
        colGroupName.FieldName = "GroupName"
        colGroupName.Name = "colGroupName"
        colGroupName.Visible = True
        colGroupName.VisibleIndex = 2
        colGroupName.Width = 140
        ' 
        colSortOrder.Caption = "Order"
        colSortOrder.FieldName = "SortOrder"
        colSortOrder.Name = "colSortOrder"
        colSortOrder.Visible = True
        colSortOrder.VisibleIndex = 3
        colSortOrder.Width = 60
        ' 
        colUsersWithAccessCount.Caption = "Users w/ Access"
        colUsersWithAccessCount.FieldName = "UsersWithAccessCount"
        colUsersWithAccessCount.Name = "colUsersWithAccessCount"
        colUsersWithAccessCount.Visible = True
        colUsersWithAccessCount.VisibleIndex = 4
        colUsersWithAccessCount.Width = 100
        ' 
        colStatusText.Caption = "Status"
        colStatusText.FieldName = "StatusText"
        colStatusText.Name = "colStatusText"
        colStatusText.Visible = True
        colStatusText.VisibleIndex = 5
        colStatusText.Width = 90
        ' 
        ' ucModuleCatalog
        ' 
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpModuleList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucModuleCatalog"
        Size = New Size(960, 580)

        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(txtModuleCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtModuleName.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboGroupName.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnSortOrder.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkIsActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpModuleList, ComponentModel.ISupportInitialize).EndInit()
        grpModuleList.ResumeLayout(False)
        CType(gridconModuleList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewModuleList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel

    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblModuleCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtModuleCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblModuleCodeHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblModuleName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtModuleName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblGroupName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboGroupName As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblSortOrder As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnSortOrder As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents chkIsActive As DevExpress.XtraEditors.CheckEdit

    Friend WithEvents grpModuleList As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconModuleList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewModuleList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colModuleCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colModuleName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colGroupName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSortOrder As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUsersWithAccessCount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStatusText As DevExpress.XtraGrid.Columns.GridColumn

End Class