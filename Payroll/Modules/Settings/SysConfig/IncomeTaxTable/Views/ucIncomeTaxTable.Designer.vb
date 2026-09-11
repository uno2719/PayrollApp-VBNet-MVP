' ucIncomeTaxTable.Designer.vb

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucIncomeTaxTable
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
        txtSalaryFrom = New DevExpress.XtraEditors.TextEdit()
        lblSalaryFrom = New DevExpress.XtraEditors.LabelControl()
        txtFixTaxAmount = New DevExpress.XtraEditors.TextEdit()
        lblFixTaxAmount = New DevExpress.XtraEditors.LabelControl()
        txtSalaryTo = New DevExpress.XtraEditors.TextEdit()
        lblSalaryTo = New DevExpress.XtraEditors.LabelControl()
        txtTaxPercentage = New DevExpress.XtraEditors.TextEdit()
        lblTaxPercentage = New DevExpress.XtraEditors.LabelControl()
        btnExcel = New DevExpress.XtraEditors.SimpleButton()
        gridconIncomeTaxList = New DevExpress.XtraGrid.GridControl()
        gridviewIncomeTaxList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colSalaryFrom = New DevExpress.XtraGrid.Columns.GridColumn()
        colSalaryTo = New DevExpress.XtraGrid.Columns.GridColumn()
        colTaxPercentage = New DevExpress.XtraGrid.Columns.GridColumn()
        colFixTaxAmount = New DevExpress.XtraGrid.Columns.GridColumn()
        colActive = New DevExpress.XtraGrid.Columns.GridColumn()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSalaryFrom.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtFixTaxAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSalaryTo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtTaxPercentage.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridconIncomeTaxList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewIncomeTaxList, ComponentModel.ISupportInitialize).BeginInit()
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
        PanelControl1.Size = New Size(1069, 70)
        PanelControl1.TabIndex = 0
        '
        ' lblTabPageTitle
        '
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(131, 30)
        lblTabPageTitle.TabIndex = 2
        lblTabPageTitle.Text = "DASHBOARD"
        '
        ' wbpMainCommands
        '
        wbpMainCommands.ButtonInterval = 15
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, WindowsuiButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New Entry", -1, True, Nothing, True, False, True, "New", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, WindowsuiButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected", -1, True, Nothing, True, False, True, "Edit", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Delete", True, WindowsuiButtonImageOptions3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Delete Selected", -1, True, Nothing, True, False, True, "Delete", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator(), New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, WindowsuiButtonImageOptions4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False), New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(580, 4)
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
        grpDetails.Controls.Add(txtSalaryFrom)
        grpDetails.Controls.Add(lblSalaryFrom)
        grpDetails.Controls.Add(txtFixTaxAmount)
        grpDetails.Controls.Add(lblFixTaxAmount)
        grpDetails.Controls.Add(txtSalaryTo)
        grpDetails.Controls.Add(lblSalaryTo)
        grpDetails.Controls.Add(txtTaxPercentage)
        grpDetails.Controls.Add(lblTaxPercentage)
        grpDetails.Controls.Add(btnExcel)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(1069, 190)
        grpDetails.TabIndex = 1
        grpDetails.Text = " DETAILS"
        '
        ' chkActive
        '
        chkActive.Location = New Point(536, 54)
        chkActive.Margin = New Padding(3, 2, 3, 2)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active"
        chkActive.Size = New Size(90, 20)
        chkActive.TabIndex = 4
        '
        ' txtSalaryFrom
        '
        txtSalaryFrom.Location = New Point(24, 52)
        txtSalaryFrom.Margin = New Padding(3, 2, 3, 2)
        txtSalaryFrom.Name = "txtSalaryFrom"
        txtSalaryFrom.Size = New Size(140, 20)
        txtSalaryFrom.TabIndex = 1
        '
        ' lblSalaryFrom
        '
        lblSalaryFrom.Location = New Point(24, 34)
        lblSalaryFrom.Margin = New Padding(3, 2, 3, 2)
        lblSalaryFrom.Name = "lblSalaryFrom"
        lblSalaryFrom.Size = New Size(57, 13)
        lblSalaryFrom.TabIndex = 0
        lblSalaryFrom.Text = "Salary From"
        '
        ' txtFixTaxAmount
        '
        txtFixTaxAmount.Location = New Point(280, 52)
        txtFixTaxAmount.Margin = New Padding(3, 2, 3, 2)
        txtFixTaxAmount.Name = "txtFixTaxAmount"
        txtFixTaxAmount.Size = New Size(140, 20)
        txtFixTaxAmount.TabIndex = 3
        '
        ' lblFixTaxAmount
        '
        lblFixTaxAmount.Location = New Point(280, 34)
        lblFixTaxAmount.Margin = New Padding(3, 2, 3, 2)
        lblFixTaxAmount.Name = "lblFixTaxAmount"
        lblFixTaxAmount.Size = New Size(66, 13)
        lblFixTaxAmount.TabIndex = 2
        lblFixTaxAmount.Text = "Fix Tax Amount"
        '
        ' txtSalaryTo
        '
        txtSalaryTo.Location = New Point(24, 102)
        txtSalaryTo.Margin = New Padding(3, 2, 3, 2)
        txtSalaryTo.Name = "txtSalaryTo"
        txtSalaryTo.Size = New Size(140, 20)
        txtSalaryTo.TabIndex = 6
        '
        ' lblSalaryTo
        '
        lblSalaryTo.Location = New Point(24, 84)
        lblSalaryTo.Margin = New Padding(3, 2, 3, 2)
        lblSalaryTo.Name = "lblSalaryTo"
        lblSalaryTo.Size = New Size(45, 13)
        lblSalaryTo.TabIndex = 5
        lblSalaryTo.Text = "Salary To"
        '
        ' txtTaxPercentage
        '
        txtTaxPercentage.Location = New Point(24, 152)
        txtTaxPercentage.Margin = New Padding(3, 2, 3, 2)
        txtTaxPercentage.Name = "txtTaxPercentage"
        txtTaxPercentage.Size = New Size(140, 20)
        txtTaxPercentage.TabIndex = 8
        '
        ' lblTaxPercentage
        '
        lblTaxPercentage.Location = New Point(24, 134)
        lblTaxPercentage.Margin = New Padding(3, 2, 3, 2)
        lblTaxPercentage.Name = "lblTaxPercentage"
        lblTaxPercentage.Size = New Size(72, 13)
        lblTaxPercentage.TabIndex = 7
        lblTaxPercentage.Text = "Tax Percentage"
        '
        ' btnExcel
        '
        btnExcel.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnExcel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        btnExcel.ImageOptions.SvgImageSize = New Size(28, 28)
        btnExcel.Location = New Point(1015, 34)
        btnExcel.Margin = New Padding(3, 2, 3, 2)
        btnExcel.Name = "btnExcel"
        btnExcel.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False
        btnExcel.Size = New Size(40, 42)
        btnExcel.TabIndex = 9
        btnExcel.ToolTip = "Import Data Options"
        '
        ' gridconIncomeTaxList
        '
        gridconIncomeTaxList.Dock = DockStyle.Fill
        gridconIncomeTaxList.Location = New Point(4, 264)
        gridconIncomeTaxList.MainView = gridviewIncomeTaxList
        gridconIncomeTaxList.Margin = New Padding(3, 2, 3, 2)
        gridconIncomeTaxList.Name = "gridconIncomeTaxList"
        gridconIncomeTaxList.Size = New Size(1069, 291)
        gridconIncomeTaxList.TabIndex = 2
        gridconIncomeTaxList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewIncomeTaxList})
        '
        ' gridviewIncomeTaxList
        '
        gridviewIncomeTaxList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colSalaryFrom, colSalaryTo, colTaxPercentage, colFixTaxAmount, colActive})
        gridviewIncomeTaxList.GridControl = gridconIncomeTaxList
        gridviewIncomeTaxList.Name = "gridviewIncomeTaxList"
        gridviewIncomeTaxList.OptionsPrint.PrintFilterInfo = True
        gridviewIncomeTaxList.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top
        gridviewIncomeTaxList.OptionsView.ShowGroupPanel = False
        '
        ' colSalaryFrom
        '
        colSalaryFrom.Caption = "Salary (FR)"
        colSalaryFrom.FieldName = "SalaryFrom"
        colSalaryFrom.Name = "colSalaryFrom"
        colSalaryFrom.Visible = True
        colSalaryFrom.VisibleIndex = 0
        colSalaryFrom.Width = 100
        '
        ' colSalaryTo
        '
        colSalaryTo.Caption = "Salary (TO)"
        colSalaryTo.FieldName = "SalaryTo"
        colSalaryTo.Name = "colSalaryTo"
        colSalaryTo.Visible = True
        colSalaryTo.VisibleIndex = 1
        colSalaryTo.Width = 100
        '
        ' colTaxPercentage
        '
        colTaxPercentage.Caption = "Tax Percentage"
        colTaxPercentage.FieldName = "TaxPercentage"
        colTaxPercentage.Name = "colTaxPercentage"
        colTaxPercentage.Visible = True
        colTaxPercentage.VisibleIndex = 2
        colTaxPercentage.Width = 110
        '
        ' colFixTaxAmount
        '
        colFixTaxAmount.Caption = "Fix Tax Amount"
        colFixTaxAmount.FieldName = "FixTaxAmount"
        colFixTaxAmount.Name = "colFixTaxAmount"
        colFixTaxAmount.Visible = True
        colFixTaxAmount.VisibleIndex = 3
        colFixTaxAmount.Width = 110
        '
        ' colActive
        '
        colActive.Caption = "Active"
        colActive.FieldName = "IsActive"
        colActive.Name = "colActive"
        colActive.Visible = True
        colActive.VisibleIndex = 4
        colActive.Width = 70
        '
        ' ucIncomeTaxTable
        '
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridconIncomeTaxList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucIncomeTaxTable"
        Padding = New Padding(4)
        Size = New Size(1077, 559)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSalaryFrom.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtFixTaxAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSalaryTo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtTaxPercentage.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridconIncomeTaxList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewIncomeTaxList, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtSalaryFrom As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSalaryFrom As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtFixTaxAmount As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblFixTaxAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSalaryTo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSalaryTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTaxPercentage As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblTaxPercentage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnExcel As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridconIncomeTaxList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewIncomeTaxList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colSalaryFrom As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSalaryTo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTaxPercentage As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colFixTaxAmount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colActive As DevExpress.XtraGrid.Columns.GridColumn

End Class