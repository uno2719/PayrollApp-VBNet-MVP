<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucLookupMaintenance
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
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        chkActive = New DevExpress.XtraEditors.CheckEdit()
        txtName = New DevExpress.XtraEditors.TextEdit()
        lblName = New DevExpress.XtraEditors.LabelControl()
        txtCode = New DevExpress.XtraEditors.TextEdit()
        lblCode = New DevExpress.XtraEditors.LabelControl()
        gridconLookupList = New DevExpress.XtraGrid.GridControl()
        gridviewLookupList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colCode = New DevExpress.XtraGrid.Columns.GridColumn()
        colName = New DevExpress.XtraGrid.Columns.GridColumn()
        colActive = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtName.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridconLookupList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewLookupList, ComponentModel.ISupportInitialize).BeginInit()
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
        PanelControl1.Size = New Size(948, 70)
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
        lblTabPageTitle.Size = New Size(125, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "DASHBOARD"
        ' 
        ' wbpMainCommands
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
        ' grpDetails
        ' 
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(chkActive)
        grpDetails.Controls.Add(txtName)
        grpDetails.Controls.Add(lblName)
        grpDetails.Controls.Add(txtCode)
        grpDetails.Controls.Add(lblCode)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(948, 96)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        ' 
        ' chkActive
        ' 
        chkActive.Location = New Point(660, 50)
        chkActive.Margin = New Padding(3, 2, 3, 2)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active"
        chkActive.Size = New Size(90, 20)
        chkActive.TabIndex = 4
        ' 
        ' txtName
        ' 
        txtName.Location = New Point(200, 50)
        txtName.Margin = New Padding(3, 2, 3, 2)
        txtName.Name = "txtName"
        txtName.Properties.NullValuePrompt = "e.g. Main Office"
        txtName.Size = New Size(440, 20)
        txtName.TabIndex = 3
        ' 
        ' lblName
        ' 
        lblName.Location = New Point(200, 32)
        lblName.Margin = New Padding(3, 2, 3, 2)
        lblName.Name = "lblName"
        lblName.Size = New Size(27, 13)
        lblName.TabIndex = 2
        lblName.Text = "Name"
        ' 
        ' txtCode
        ' 
        txtCode.Location = New Point(24, 50)
        txtCode.Margin = New Padding(3, 2, 3, 2)
        txtCode.Name = "txtCode"
        txtCode.Properties.NullValuePrompt = "e.g. MAIN"
        txtCode.Size = New Size(150, 20)
        txtCode.TabIndex = 1
        ' 
        ' lblCode
        ' 
        lblCode.Location = New Point(24, 32)
        lblCode.Margin = New Padding(3, 2, 3, 2)
        lblCode.Name = "lblCode"
        lblCode.Size = New Size(25, 13)
        lblCode.TabIndex = 0
        lblCode.Text = "Code"
        ' 
        ' gridconLookupList
        ' 
        gridconLookupList.Dock = DockStyle.Fill
        gridconLookupList.Location = New Point(4, 170)
        gridconLookupList.MainView = gridviewLookupList
        gridconLookupList.Margin = New Padding(3, 2, 3, 2)
        gridconLookupList.Name = "gridconLookupList"
        gridconLookupList.Size = New Size(948, 360)
        gridconLookupList.TabIndex = 2
        gridconLookupList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewLookupList})
        ' 
        ' gridviewLookupList
        ' 
        gridviewLookupList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colCode, colName, colActive})
        gridviewLookupList.GridControl = gridconLookupList
        gridviewLookupList.Name = "gridviewLookupList"
        gridviewLookupList.OptionsPrint.PrintFilterInfo = True
        gridviewLookupList.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        gridviewLookupList.OptionsView.ShowGroupPanel = False
        ' 
        ' colCode
        ' 
        colCode.Caption = "Code"
        colCode.FieldName = "Code"
        colCode.Name = "colCode"
        colCode.Visible = True
        colCode.VisibleIndex = 0
        colCode.Width = 120
        ' 
        ' colName
        ' 
        colName.Caption = "Name"
        colName.FieldName = "Name"
        colName.Name = "colName"
        colName.Visible = True
        colName.VisibleIndex = 1
        colName.Width = 400
        ' 
        ' colActive
        ' 
        colActive.Caption = "Active"
        colActive.FieldName = "IsActive"
        colActive.Name = "colActive"
        colActive.Visible = True
        colActive.VisibleIndex = 2
        colActive.Width = 70
        ' 
        ' ucLookupMaintenance
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridconLookupList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucLookupMaintenance"
        Padding = New Padding(4)
        Size = New Size(956, 534)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtName.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridconLookupList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewLookupList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridconLookupList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewLookupList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl

End Class