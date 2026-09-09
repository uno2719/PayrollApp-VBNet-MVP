<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayrollFlaggedEntry
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
        chkPagIbigFlag = New DevExpress.XtraEditors.CheckEdit()
        chkPhilHealthFlag = New DevExpress.XtraEditors.CheckEdit()
        chkSSSFlag = New DevExpress.XtraEditors.CheckEdit()
        chkTaxFlag = New DevExpress.XtraEditors.CheckEdit()
        txtDescription = New DevExpress.XtraEditors.TextEdit()
        lblDescription = New DevExpress.XtraEditors.LabelControl()
        txtCode = New DevExpress.XtraEditors.TextEdit()
        lblCode = New DevExpress.XtraEditors.LabelControl()
        gridconFlaggedEntryList = New DevExpress.XtraGrid.GridControl()
        gridviewFlaggedEntryList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colCode = New DevExpress.XtraGrid.Columns.GridColumn()
        colDescription = New DevExpress.XtraGrid.Columns.GridColumn()
        colTaxFlag = New DevExpress.XtraGrid.Columns.GridColumn()
        colSSSFlag = New DevExpress.XtraGrid.Columns.GridColumn()
        colPhilHealthFlag = New DevExpress.XtraGrid.Columns.GridColumn()
        colPagIbigFlag = New DevExpress.XtraGrid.Columns.GridColumn()
        colActive = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkPagIbigFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkPhilHealthFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkSSSFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkTaxFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtDescription.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridconFlaggedEntryList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewFlaggedEntryList, ComponentModel.ISupportInitialize).BeginInit()
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
        lblTabPageTitle.Size = New Size(150, 28)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "DEDUCTION"
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
        grpDetails.Controls.Add(chkPagIbigFlag)
        grpDetails.Controls.Add(chkPhilHealthFlag)
        grpDetails.Controls.Add(chkSSSFlag)
        grpDetails.Controls.Add(chkTaxFlag)
        grpDetails.Controls.Add(txtDescription)
        grpDetails.Controls.Add(lblDescription)
        grpDetails.Controls.Add(txtCode)
        grpDetails.Controls.Add(lblCode)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(948, 150)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        ' 
        ' chkActive
        ' 
        chkActive.Location = New Point(24, 121)
        chkActive.Margin = New Padding(3, 2, 3, 2)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active"
        chkActive.Size = New Size(90, 20)
        chkActive.TabIndex = 8
        ' 
        ' chkPagIbigFlag
        ' 
        chkPagIbigFlag.Location = New Point(430, 91)
        chkPagIbigFlag.Margin = New Padding(3, 2, 3, 2)
        chkPagIbigFlag.Name = "chkPagIbigFlag"
        chkPagIbigFlag.Properties.Caption = "Pag-IBIG Flag"
        chkPagIbigFlag.Size = New Size(120, 20)
        chkPagIbigFlag.TabIndex = 7
        ' 
        ' chkPhilHealthFlag
        ' 
        chkPhilHealthFlag.Location = New Point(280, 91)
        chkPhilHealthFlag.Margin = New Padding(3, 2, 3, 2)
        chkPhilHealthFlag.Name = "chkPhilHealthFlag"
        chkPhilHealthFlag.Properties.Caption = "Phil Health Flag"
        chkPhilHealthFlag.Size = New Size(130, 20)
        chkPhilHealthFlag.TabIndex = 6
        ' 
        ' chkSSSFlag
        ' 
        chkSSSFlag.Location = New Point(150, 91)
        chkSSSFlag.Margin = New Padding(3, 2, 3, 2)
        chkSSSFlag.Name = "chkSSSFlag"
        chkSSSFlag.Properties.Caption = "SSS Flag"
        chkSSSFlag.Size = New Size(110, 20)
        chkSSSFlag.TabIndex = 5
        ' 
        ' chkTaxFlag
        ' 
        chkTaxFlag.Location = New Point(24, 91)
        chkTaxFlag.Margin = New Padding(3, 2, 3, 2)
        chkTaxFlag.Name = "chkTaxFlag"
        chkTaxFlag.Properties.Caption = "Tax Flag"
        chkTaxFlag.Size = New Size(110, 20)
        chkTaxFlag.TabIndex = 4
        ' 
        ' txtDescription
        ' 
        txtDescription.Location = New Point(280, 49)
        txtDescription.Margin = New Padding(3, 2, 3, 2)
        txtDescription.Name = "txtDescription"
        txtDescription.Properties.NullValuePrompt = "e.g. UNDERTIME"
        txtDescription.Size = New Size(460, 20)
        txtDescription.TabIndex = 3
        ' 
        ' lblDescription
        ' 
        lblDescription.Location = New Point(280, 32)
        lblDescription.Margin = New Padding(3, 2, 3, 2)
        lblDescription.Name = "lblDescription"
        lblDescription.Size = New Size(58, 13)
        lblDescription.TabIndex = 2
        lblDescription.Text = "Description"
        ' 
        ' txtCode
        ' 
        txtCode.Location = New Point(24, 49)
        txtCode.Margin = New Padding(3, 2, 3, 2)
        txtCode.Name = "txtCode"
        txtCode.Properties.NullValuePrompt = "e.g. EOUT"
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
        ' gridconFlaggedEntryList
        ' 
        gridconFlaggedEntryList.Dock = DockStyle.Fill
        gridconFlaggedEntryList.Location = New Point(4, 224)
        gridconFlaggedEntryList.MainView = gridviewFlaggedEntryList
        gridconFlaggedEntryList.Margin = New Padding(3, 2, 3, 2)
        gridconFlaggedEntryList.Name = "gridconFlaggedEntryList"
        gridconFlaggedEntryList.Size = New Size(948, 306)
        gridconFlaggedEntryList.TabIndex = 2
        gridconFlaggedEntryList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewFlaggedEntryList})
        ' 
        ' gridviewFlaggedEntryList
        ' 
        gridviewFlaggedEntryList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colCode, colDescription, colTaxFlag, colSSSFlag, colPhilHealthFlag, colPagIbigFlag, colActive})
        gridviewFlaggedEntryList.GridControl = gridconFlaggedEntryList
        gridviewFlaggedEntryList.Name = "gridviewFlaggedEntryList"
        gridviewFlaggedEntryList.OptionsPrint.PrintFilterInfo = True
        gridviewFlaggedEntryList.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        gridviewFlaggedEntryList.OptionsView.ShowGroupPanel = False
        ' 
        ' colCode
        ' 
        colCode.Caption = "Code"
        colCode.FieldName = "Code"
        colCode.Name = "colCode"
        colCode.Visible = True
        colCode.VisibleIndex = 0
        colCode.Width = 90
        ' 
        ' colDescription
        ' 
        colDescription.Caption = "Code Description"
        colDescription.FieldName = "Description"
        colDescription.Name = "colDescription"
        colDescription.Visible = True
        colDescription.VisibleIndex = 1
        colDescription.Width = 260
        ' 
        ' colTaxFlag
        ' 
        colTaxFlag.Caption = "Tax Flag"
        colTaxFlag.FieldName = "TaxFlag"
        colTaxFlag.Name = "colTaxFlag"
        colTaxFlag.Visible = True
        colTaxFlag.VisibleIndex = 2
        colTaxFlag.Width = 80
        ' 
        ' colSSSFlag
        ' 
        colSSSFlag.Caption = "SSS Flag"
        colSSSFlag.FieldName = "SSSFlag"
        colSSSFlag.Name = "colSSSFlag"
        colSSSFlag.Visible = True
        colSSSFlag.VisibleIndex = 3
        colSSSFlag.Width = 80
        ' 
        ' colPhilHealthFlag
        ' 
        colPhilHealthFlag.Caption = "Phil Health Flag"
        colPhilHealthFlag.FieldName = "PhilHealthFlag"
        colPhilHealthFlag.Name = "colPhilHealthFlag"
        colPhilHealthFlag.Visible = True
        colPhilHealthFlag.VisibleIndex = 4
        colPhilHealthFlag.Width = 100
        ' 
        ' colPagIbigFlag
        ' 
        colPagIbigFlag.Caption = "Pag-IBIG Flag"
        colPagIbigFlag.FieldName = "PagIbigFlag"
        colPagIbigFlag.Name = "colPagIbigFlag"
        colPagIbigFlag.Visible = True
        colPagIbigFlag.VisibleIndex = 5
        colPagIbigFlag.Width = 100
        ' 
        ' colActive
        ' 
        colActive.Caption = "Active"
        colActive.FieldName = "IsActive"
        colActive.Name = "colActive"
        colActive.Visible = True
        colActive.VisibleIndex = 6
        colActive.Width = 70
        ' 
        ' ucPayrollFlaggedEntry
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridconFlaggedEntryList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucPayrollFlaggedEntry"
        Padding = New Padding(4)
        Size = New Size(956, 534)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkPagIbigFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkPhilHealthFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkSSSFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkTaxFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtDescription.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridconFlaggedEntryList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewFlaggedEntryList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkPagIbigFlag As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkPhilHealthFlag As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkSSSFlag As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkTaxFlag As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtDescription As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblDescription As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridconFlaggedEntryList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewFlaggedEntryList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colDescription As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTaxFlag As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSSSFlag As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPhilHealthFlag As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPagIbigFlag As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl

End Class
