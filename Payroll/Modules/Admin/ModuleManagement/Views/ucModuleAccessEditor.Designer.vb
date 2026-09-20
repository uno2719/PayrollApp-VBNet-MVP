<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucModuleAccessEditor
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

        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        lblUnsavedBadge = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()

        grpUserList = New DevExpress.XtraEditors.GroupControl()
        gridconUserList = New DevExpress.XtraGrid.GridControl()
        gridviewUserList = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlUserFilter = New DevExpress.XtraEditors.PanelControl()
        rgUserFilter = New DevExpress.XtraEditors.RadioGroup()
        lblUserFilter = New DevExpress.XtraEditors.LabelControl()

        grpAccessGrid = New DevExpress.XtraEditors.GroupControl()
        lblSelectedUser = New DevExpress.XtraEditors.LabelControl()
        pnlBulkActions = New DevExpress.XtraEditors.PanelControl()
        btnSelectAllView = New DevExpress.XtraEditors.SimpleButton()
        btnSelectAllEdit = New DevExpress.XtraEditors.SimpleButton()
        btnClearAll = New DevExpress.XtraEditors.SimpleButton()
        btnCopyFrom = New DevExpress.XtraEditors.SimpleButton()
        gridconAccess = New DevExpress.XtraGrid.GridControl()
        gridviewAccess = New DevExpress.XtraGrid.Views.Grid.GridView()
        colModuleName = New DevExpress.XtraGrid.Columns.GridColumn()
        colGroupName = New DevExpress.XtraGrid.Columns.GridColumn()
        colCanView = New DevExpress.XtraGrid.Columns.GridColumn()
        colCanEdit = New DevExpress.XtraGrid.Columns.GridColumn()
        repoCheckView = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        repoCheckEdit = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()

        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpUserList, ComponentModel.ISupportInitialize).BeginInit()
        grpUserList.SuspendLayout()
        CType(gridconUserList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewUserList, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlUserFilter, ComponentModel.ISupportInitialize).BeginInit()
        pnlUserFilter.SuspendLayout()
        CType(rgUserFilter.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpAccessGrid, ComponentModel.ISupportInitialize).BeginInit()
        grpAccessGrid.SuspendLayout()
        CType(pnlBulkActions, ComponentModel.ISupportInitialize).BeginInit()
        pnlBulkActions.SuspendLayout()
        CType(gridconAccess, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewAccess, ComponentModel.ISupportInitialize).BeginInit()
        CType(repoCheckView, ComponentModel.ISupportInitialize).BeginInit()
        CType(repoCheckEdit, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblUnsavedBadge)
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(0, 0)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(960, 60)
        PanelControl1.TabIndex = 0
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 13.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 12)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(110, 25)
        lblTabPageTitle.TabIndex = 1
        lblTabPageTitle.Text = "ACCESS EDITOR"
        ' 
        lblUnsavedBadge.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblUnsavedBadge.Appearance.ForeColor = Color.DarkOrange
        lblUnsavedBadge.Appearance.Options.UseFont = True
        lblUnsavedBadge.Appearance.Options.UseForeColor = True
        lblUnsavedBadge.Location = New Point(8, 40)
        lblUnsavedBadge.Name = "lblUnsavedBadge"
        lblUnsavedBadge.Size = New Size(150, 13)
        lblUnsavedBadge.TabIndex = 2
        lblUnsavedBadge.Text = "* Unsaved changes"
        lblUnsavedBadge.Visible = False
        ' 
        ' wbpMainCommands
        ' 
        ' Index: 0=sep 1=Save 2=Copy From 3=sep 4=Refresh 5=sep
        wbpMainCommands.ButtonInterval = 15
        btnImg1.Image = My.Resources.icon_save_24
        btnImg2.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Save Access", True, btnImg1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Save Changes for This User", -1, True, Nothing, True, False, True, "Save", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, btnImg2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Location = New Point(600, 2)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(358, 56)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        ' 
        ' grpUserList
        ' 
        grpUserList.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpUserList.AppearanceCaption.Options.UseFont = True
        grpUserList.Controls.Add(gridconUserList)
        grpUserList.Controls.Add(pnlUserFilter)
        grpUserList.Dock = DockStyle.Right
        grpUserList.Location = New Point(690, 60)
        grpUserList.Name = "grpUserList"
        grpUserList.Size = New Size(270, 520)
        grpUserList.TabIndex = 1
        grpUserList.Text = " USERS"
        ' 
        gridconUserList.Dock = DockStyle.Fill
        gridconUserList.Location = New Point(2, 22)
        gridconUserList.MainView = gridviewUserList
        gridconUserList.Name = "gridconUserList"
        gridconUserList.Size = New Size(266, 468)
        gridconUserList.TabIndex = 0
        gridconUserList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewUserList})
        ' 
        gridviewUserList.GridControl = gridconUserList
        gridviewUserList.Name = "gridviewUserList"
        gridviewUserList.OptionsBehavior.Editable = False
        gridviewUserList.OptionsView.ShowGroupPanel = False
        ' 
        pnlUserFilter.Controls.Add(rgUserFilter)
        pnlUserFilter.Controls.Add(lblUserFilter)
        pnlUserFilter.Dock = DockStyle.Bottom
        pnlUserFilter.Location = New Point(2, 490)
        pnlUserFilter.Name = "pnlUserFilter"
        pnlUserFilter.Size = New Size(266, 28)
        pnlUserFilter.TabIndex = 1
        ' 
        rgUserFilter.Dock = DockStyle.Fill
        rgUserFilter.Location = New Point(49, 2)
        rgUserFilter.Name = "rgUserFilter"
        rgUserFilter.Properties.AllowMouseWheel = False
        rgUserFilter.Properties.Appearance.BackColor = Color.Transparent
        rgUserFilter.Properties.Appearance.Options.UseBackColor = True
        rgUserFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgUserFilter.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Far
        rgUserFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Active", "Active"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("All", "All")})
        rgUserFilter.Properties.Padding = New Padding(0)
        rgUserFilter.Size = New Size(215, 24)
        rgUserFilter.TabIndex = 0
        rgUserFilter.TabStop = False
        ' 
        lblUserFilter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblUserFilter.Dock = DockStyle.Left
        lblUserFilter.Location = New Point(2, 2)
        lblUserFilter.Name = "lblUserFilter"
        lblUserFilter.Size = New Size(47, 24)
        lblUserFilter.TabIndex = 1
        lblUserFilter.Text = "Show:"
        ' 
        ' grpAccessGrid
        ' 
        grpAccessGrid.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpAccessGrid.AppearanceCaption.Options.UseFont = True
        grpAccessGrid.Controls.Add(gridconAccess)
        grpAccessGrid.Controls.Add(pnlBulkActions)
        grpAccessGrid.Controls.Add(lblSelectedUser)
        grpAccessGrid.Dock = DockStyle.Fill
        grpAccessGrid.Location = New Point(0, 60)
        grpAccessGrid.Name = "grpAccessGrid"
        grpAccessGrid.Size = New Size(690, 520)
        grpAccessGrid.TabIndex = 2
        grpAccessGrid.Text = " MODULE ACCESS"
        ' 
        lblSelectedUser.Appearance.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSelectedUser.Appearance.Options.UseFont = True
        lblSelectedUser.Location = New Point(14, 30)
        lblSelectedUser.Name = "lblSelectedUser"
        lblSelectedUser.Size = New Size(200, 17)
        lblSelectedUser.TabIndex = 0
        lblSelectedUser.Text = "No user selected"
        ' 
        pnlBulkActions.Controls.Add(btnCopyFrom)
        pnlBulkActions.Controls.Add(btnClearAll)
        pnlBulkActions.Controls.Add(btnSelectAllEdit)
        pnlBulkActions.Controls.Add(btnSelectAllView)
        pnlBulkActions.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        pnlBulkActions.Location = New Point(14, 54)
        pnlBulkActions.Name = "pnlBulkActions"
        pnlBulkActions.Size = New Size(660, 32)
        pnlBulkActions.TabIndex = 1
        ' 
        btnSelectAllView.Location = New Point(0, 2)
        btnSelectAllView.Name = "btnSelectAllView"
        btnSelectAllView.Size = New Size(130, 26)
        btnSelectAllView.TabIndex = 0
        btnSelectAllView.Text = "Select All (View)"
        ' 
        btnSelectAllEdit.Location = New Point(136, 2)
        btnSelectAllEdit.Name = "btnSelectAllEdit"
        btnSelectAllEdit.Size = New Size(130, 26)
        btnSelectAllEdit.TabIndex = 1
        btnSelectAllEdit.Text = "Select All (Edit)"
        ' 
        btnClearAll.Location = New Point(272, 2)
        btnClearAll.Name = "btnClearAll"
        btnClearAll.Size = New Size(110, 26)
        btnClearAll.TabIndex = 2
        btnClearAll.Text = "Clear All"
        ' 
        btnCopyFrom.Location = New Point(400, 2)
        btnCopyFrom.Name = "btnCopyFrom"
        btnCopyFrom.Size = New Size(180, 26)
        btnCopyFrom.TabIndex = 3
        btnCopyFrom.Text = "Copy Access From..."
        ' 
        gridconAccess.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        gridconAccess.Location = New Point(14, 92)
        gridconAccess.MainView = gridviewAccess
        gridconAccess.Name = "gridconAccess"
        gridconAccess.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {repoCheckView, repoCheckEdit})
        gridconAccess.Size = New Size(660, 410)
        gridconAccess.TabIndex = 2
        gridconAccess.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewAccess})
        ' 
        gridviewAccess.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {
            colModuleName, colGroupName, colCanView, colCanEdit})
        gridviewAccess.GridControl = gridconAccess
        gridviewAccess.Name = "gridviewAccess"
        gridviewAccess.OptionsView.ShowGroupPanel = False
        gridviewAccess.OptionsBehavior.Editable = True
        ' 
        colModuleName.Caption = "Module"
        colModuleName.FieldName = "ModuleName"
        colModuleName.Name = "colModuleName"
        colModuleName.OptionsColumn.AllowEdit = False
        colModuleName.Visible = True
        colModuleName.VisibleIndex = 0
        colModuleName.Width = 260
        ' 
        colGroupName.Caption = "Group / Menu"
        colGroupName.FieldName = "GroupName"
        colGroupName.Name = "colGroupName"
        colGroupName.OptionsColumn.AllowEdit = False
        colGroupName.Visible = True
        colGroupName.VisibleIndex = 1
        colGroupName.Width = 160
        ' 
        colCanView.Caption = "View"
        colCanView.ColumnEdit = repoCheckView
        colCanView.FieldName = "CanView"
        colCanView.Name = "colCanView"
        colCanView.Visible = True
        colCanView.VisibleIndex = 2
        colCanView.Width = 90
        ' 
        colCanEdit.Caption = "Edit"
        colCanEdit.ColumnEdit = repoCheckEdit
        colCanEdit.FieldName = "CanEdit"
        colCanEdit.Name = "colCanEdit"
        colCanEdit.Visible = True
        colCanEdit.VisibleIndex = 3
        colCanEdit.Width = 90
        ' 
        repoCheckView.AutoHeight = False
        repoCheckView.Name = "repoCheckView"
        ' 
        repoCheckEdit.AutoHeight = False
        repoCheckEdit.Name = "repoCheckEdit"
        ' 
        ' ucModuleAccessEditor
        ' 
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpAccessGrid)
        Controls.Add(grpUserList)
        Controls.Add(PanelControl1)
        Name = "ucModuleAccessEditor"
        Size = New Size(960, 580)

        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpUserList, ComponentModel.ISupportInitialize).EndInit()
        grpUserList.ResumeLayout(False)
        CType(gridconUserList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewUserList, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlUserFilter, ComponentModel.ISupportInitialize).EndInit()
        pnlUserFilter.ResumeLayout(False)
        CType(rgUserFilter.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpAccessGrid, ComponentModel.ISupportInitialize).EndInit()
        grpAccessGrid.ResumeLayout(False)
        grpAccessGrid.PerformLayout()
        CType(pnlBulkActions, ComponentModel.ISupportInitialize).EndInit()
        pnlBulkActions.ResumeLayout(False)
        CType(gridconAccess, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewAccess, ComponentModel.ISupportInitialize).EndInit()
        CType(repoCheckView, ComponentModel.ISupportInitialize).EndInit()
        CType(repoCheckEdit, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblUnsavedBadge As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel

    Friend WithEvents grpUserList As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconUserList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewUserList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents pnlUserFilter As DevExpress.XtraEditors.PanelControl
    Friend WithEvents rgUserFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblUserFilter As DevExpress.XtraEditors.LabelControl

    Friend WithEvents grpAccessGrid As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblSelectedUser As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlBulkActions As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnSelectAllView As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSelectAllEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnClearAll As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCopyFrom As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridconAccess As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewAccess As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colModuleName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colGroupName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCanView As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCanEdit As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repoCheckView As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit
    Friend WithEvents repoCheckEdit As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit

End Class