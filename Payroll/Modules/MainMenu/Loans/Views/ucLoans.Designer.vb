<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucLoans
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
        Dim btnImg5 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()
        Dim btnImg6 As DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions = New DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions()

        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        wbpMainCommands = New DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel()

        grpEmployeeList = New DevExpress.XtraEditors.GroupControl()
        gridconEmployeeList = New DevExpress.XtraGrid.GridControl()
        gridviewEmployeeList = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlEmployeeFilter = New DevExpress.XtraEditors.PanelControl()
        rgEmployeeFilter = New DevExpress.XtraEditors.RadioGroup()
        lblEmployeeFilter = New DevExpress.XtraEditors.LabelControl()

        grpDetails = New DevExpress.XtraEditors.GroupControl()
        lblSelectedEmployee = New DevExpress.XtraEditors.LabelControl()
        lblLoanCode = New DevExpress.XtraEditors.LabelControl()
        lueLoanCode = New DevExpress.XtraEditors.LookUpEdit()
        lblLoanStartDate = New DevExpress.XtraEditors.LabelControl()
        dtLoanStartDate = New DevExpress.XtraEditors.DateEdit()
        lblInputDate = New DevExpress.XtraEditors.LabelControl()
        dtInputDate = New DevExpress.XtraEditors.DateEdit()
        lblFrequency = New DevExpress.XtraEditors.LabelControl()
        cboFrequency = New DevExpress.XtraEditors.ComboBoxEdit()

        lblAmortizationMethod = New DevExpress.XtraEditors.LabelControl()
        rgAmortizationMethod = New DevExpress.XtraEditors.RadioGroup()
        lblInterestMethod = New DevExpress.XtraEditors.LabelControl()
        rgInterestMethod = New DevExpress.XtraEditors.RadioGroup()
        lblInterestRate = New DevExpress.XtraEditors.LabelControl()
        spnInterestRate = New DevExpress.XtraEditors.SpinEdit()
        lblTerms = New DevExpress.XtraEditors.LabelControl()
        spnTerms = New DevExpress.XtraEditors.SpinEdit()

        lblPrincipalAmount = New DevExpress.XtraEditors.LabelControl()
        spnPrincipalAmount = New DevExpress.XtraEditors.SpinEdit()
        lblInterestAmount = New DevExpress.XtraEditors.LabelControl()
        spnInterestAmount = New DevExpress.XtraEditors.SpinEdit()
        lblTotalLoanAmount = New DevExpress.XtraEditors.LabelControl()
        spnTotalLoanAmount = New DevExpress.XtraEditors.SpinEdit()

        lblPrincipalAmortization = New DevExpress.XtraEditors.LabelControl()
        spnPrincipalAmortization = New DevExpress.XtraEditors.SpinEdit()
        lblInterestAmortization = New DevExpress.XtraEditors.LabelControl()
        spnInterestAmortization = New DevExpress.XtraEditors.SpinEdit()
        lblTotalAmortization = New DevExpress.XtraEditors.LabelControl()
        spnTotalAmortization = New DevExpress.XtraEditors.SpinEdit()

        lblRemark = New DevExpress.XtraEditors.LabelControl()
        txtRemark = New DevExpress.XtraEditors.TextEdit()
        lblStatusCaption = New DevExpress.XtraEditors.LabelControl()
        lblStatusValue = New DevExpress.XtraEditors.LabelControl()

        grpLoanList = New DevExpress.XtraEditors.GroupControl()
        gridconLoanList = New DevExpress.XtraGrid.GridControl()
        gridviewLoanList = New DevExpress.XtraGrid.Views.Grid.GridView()
        colLoanCode = New DevExpress.XtraGrid.Columns.GridColumn()
        colLoanDescription = New DevExpress.XtraGrid.Columns.GridColumn()
        colLoanStartDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colInterestRate = New DevExpress.XtraGrid.Columns.GridColumn()
        colTerms = New DevExpress.XtraGrid.Columns.GridColumn()
        colTotalLoanAmount = New DevExpress.XtraGrid.Columns.GridColumn()
        colTotalAmortization = New DevExpress.XtraGrid.Columns.GridColumn()
        colTotalPaid = New DevExpress.XtraGrid.Columns.GridColumn()
        colBalance = New DevExpress.XtraGrid.Columns.GridColumn()
        colNextDeductionDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colProgress = New DevExpress.XtraGrid.Columns.GridColumn()
        colStatus = New DevExpress.XtraGrid.Columns.GridColumn()
        pnlLoanFilter = New DevExpress.XtraEditors.PanelControl()
        rgLoanStatusFilter = New DevExpress.XtraEditors.RadioGroup()
        lblLoanFilter = New DevExpress.XtraEditors.LabelControl()

        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpEmployeeList, ComponentModel.ISupportInitialize).BeginInit()
        grpEmployeeList.SuspendLayout()
        CType(gridconEmployeeList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewEmployeeList, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlEmployeeFilter, ComponentModel.ISupportInitialize).BeginInit()
        pnlEmployeeFilter.SuspendLayout()
        CType(rgEmployeeFilter.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(lueLoanCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtLoanStartDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtLoanStartDate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtInputDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtInputDate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboFrequency.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(rgAmortizationMethod.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(rgInterestMethod.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnInterestRate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnTerms.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnPrincipalAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnInterestAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnTotalLoanAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnPrincipalAmortization.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnInterestAmortization.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnTotalAmortization.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtRemark.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpLoanList, ComponentModel.ISupportInitialize).BeginInit()
        grpLoanList.SuspendLayout()
        CType(gridconLoanList, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewLoanList, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlLoanFilter, ComponentModel.ISupportInitialize).BeginInit()
        pnlLoanFilter.SuspendLayout()
        CType(rgLoanStatusFilter.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(wbpMainCommands)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Padding = New Padding(3, 2, 3, 2)
        PanelControl1.Size = New Size(1192, 70)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabPageTitle.Appearance.ForeColor = Color.Black
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Appearance.Options.UseForeColor = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(100, 28)
        lblTabPageTitle.TabIndex = 1
        lblTabPageTitle.Text = "LOANS"
        ' 
        ' wbpMainCommands
        ' 
        ' Index: 0=sep 1=New 2=Edit 3=Delete 4=sep 5=Details 6=Toggle 7=sep 8=Refresh 9=sep
        ' Ang Details at Toggle ay walang image - idagdag mo na lang kung
        ' may bagay na icon ka sa Resources.
        ' 
        wbpMainCommands.ButtonInterval = 15
        btnImg1.Image = My.Resources.Resources.icon_add_property_24_png
        btnImg2.Image = My.Resources.Resources.icon_edit_property_24
        btnImg3.Image = My.Resources.Resources.icon_delete_24
        btnImg6.Image = My.Resources.Resources.icon_refresh_24
        wbpMainCommands.Buttons.AddRange(New DevExpress.XtraEditors.ButtonPanel.IBaseButton() {
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" New", True, btnImg1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Add New Loan", -1, True, Nothing, True, False, True, "New", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Edit", True, btnImg2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Edit Selected Loan", -1, True, Nothing, True, False, True, "Edit", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Delete", True, btnImg3, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Delete Selected Loan", -1, True, Nothing, True, False, True, "Delete", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Details", True, btnImg4, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "View Loan Details and Deduction History", -1, True, Nothing, True, False, True, "Details", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Set Inactive", True, btnImg5, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Activate / Deactivate Selected Loan", -1, True, Nothing, True, False, True, "Toggle", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator(),
            New DevExpress.XtraBars.Docking2010.WindowsUIButton(" Refresh", True, btnImg6, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Reload from Database", -1, True, Nothing, True, False, True, "Refresh", -1, False),
            New DevExpress.XtraBars.Docking2010.WindowsUISeparator()})
        wbpMainCommands.ContentAlignment = ContentAlignment.MiddleRight
        wbpMainCommands.Dock = DockStyle.Right
        wbpMainCommands.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        wbpMainCommands.Location = New Point(500, 4)
        wbpMainCommands.Margin = New Padding(1)
        wbpMainCommands.Name = "wbpMainCommands"
        wbpMainCommands.Size = New Size(688, 62)
        wbpMainCommands.TabIndex = 0
        wbpMainCommands.Text = "Commands"
        ' 
        ' grpEmployeeList
        ' 
        grpEmployeeList.AppearanceCaption.Font = New Font("Segoe UI", 10.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpEmployeeList.AppearanceCaption.Options.UseFont = True
        grpEmployeeList.Controls.Add(gridconEmployeeList)
        grpEmployeeList.Controls.Add(pnlEmployeeFilter)
        grpEmployeeList.Dock = DockStyle.Right
        grpEmployeeList.Location = New Point(926, 74)
        grpEmployeeList.Name = "grpEmployeeList"
        grpEmployeeList.Size = New Size(270, 622)
        grpEmployeeList.TabIndex = 3
        grpEmployeeList.Text = " EMPLOYEE MASTERLIST"
        ' 
        ' gridconEmployeeList
        ' 
        gridconEmployeeList.Dock = DockStyle.Fill
        gridconEmployeeList.Location = New Point(2, 31)
        gridconEmployeeList.MainView = gridviewEmployeeList
        gridconEmployeeList.Name = "gridconEmployeeList"
        gridconEmployeeList.Size = New Size(266, 561)
        gridconEmployeeList.TabIndex = 0
        gridconEmployeeList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewEmployeeList})
        ' 
        ' gridviewEmployeeList
        ' 
        gridviewEmployeeList.GridControl = gridconEmployeeList
        gridviewEmployeeList.Name = "gridviewEmployeeList"
        gridviewEmployeeList.OptionsView.ShowGroupPanel = False
        ' 
        ' pnlEmployeeFilter
        ' 
        pnlEmployeeFilter.Controls.Add(rgEmployeeFilter)
        pnlEmployeeFilter.Controls.Add(lblEmployeeFilter)
        pnlEmployeeFilter.Dock = DockStyle.Bottom
        pnlEmployeeFilter.Location = New Point(2, 592)
        pnlEmployeeFilter.Name = "pnlEmployeeFilter"
        pnlEmployeeFilter.Size = New Size(266, 28)
        pnlEmployeeFilter.TabIndex = 1
        ' 
        ' rgEmployeeFilter
        ' 
        rgEmployeeFilter.Dock = DockStyle.Fill
        rgEmployeeFilter.Location = New Point(49, 2)
        rgEmployeeFilter.Name = "rgEmployeeFilter"
        rgEmployeeFilter.Properties.AllowMouseWheel = False
        rgEmployeeFilter.Properties.Appearance.BackColor = Color.Transparent
        rgEmployeeFilter.Properties.Appearance.Options.UseBackColor = True
        rgEmployeeFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgEmployeeFilter.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Far
        rgEmployeeFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Active", "Active"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Inactive", "Inactive"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("All", "All")})
        rgEmployeeFilter.Properties.Padding = New Padding(0)
        rgEmployeeFilter.Size = New Size(215, 24)
        rgEmployeeFilter.TabIndex = 0
        rgEmployeeFilter.TabStop = False
        ' 
        ' lblEmployeeFilter
        ' 
        lblEmployeeFilter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblEmployeeFilter.Dock = DockStyle.Left
        lblEmployeeFilter.Location = New Point(2, 2)
        lblEmployeeFilter.Name = "lblEmployeeFilter"
        lblEmployeeFilter.Size = New Size(47, 24)
        lblEmployeeFilter.TabIndex = 1
        lblEmployeeFilter.Text = "Show:"
        ' 
        ' grpDetails
        ' 
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(lblSelectedEmployee)
        grpDetails.Controls.Add(lblLoanCode)
        grpDetails.Controls.Add(lueLoanCode)
        grpDetails.Controls.Add(lblLoanStartDate)
        grpDetails.Controls.Add(dtLoanStartDate)
        grpDetails.Controls.Add(lblInputDate)
        grpDetails.Controls.Add(dtInputDate)
        grpDetails.Controls.Add(lblFrequency)
        grpDetails.Controls.Add(cboFrequency)
        grpDetails.Controls.Add(lblAmortizationMethod)
        grpDetails.Controls.Add(rgAmortizationMethod)
        grpDetails.Controls.Add(lblInterestMethod)
        grpDetails.Controls.Add(rgInterestMethod)
        grpDetails.Controls.Add(lblInterestRate)
        grpDetails.Controls.Add(spnInterestRate)
        grpDetails.Controls.Add(lblTerms)
        grpDetails.Controls.Add(spnTerms)
        grpDetails.Controls.Add(lblPrincipalAmount)
        grpDetails.Controls.Add(spnPrincipalAmount)
        grpDetails.Controls.Add(lblInterestAmount)
        grpDetails.Controls.Add(spnInterestAmount)
        grpDetails.Controls.Add(lblTotalLoanAmount)
        grpDetails.Controls.Add(spnTotalLoanAmount)
        grpDetails.Controls.Add(lblPrincipalAmortization)
        grpDetails.Controls.Add(spnPrincipalAmortization)
        grpDetails.Controls.Add(lblInterestAmortization)
        grpDetails.Controls.Add(spnInterestAmortization)
        grpDetails.Controls.Add(lblTotalAmortization)
        grpDetails.Controls.Add(spnTotalAmortization)
        grpDetails.Controls.Add(lblRemark)
        grpDetails.Controls.Add(txtRemark)
        grpDetails.Controls.Add(lblStatusCaption)
        grpDetails.Controls.Add(lblStatusValue)
        grpDetails.Dock = DockStyle.Top
        grpDetails.Location = New Point(4, 74)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(922, 292)
        grpDetails.TabIndex = 1
        grpDetails.Text = " LOAN DETAILS"
        ' 
        ' lblSelectedEmployee
        ' 
        lblSelectedEmployee.Appearance.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSelectedEmployee.Appearance.Options.UseFont = True
        lblSelectedEmployee.Location = New Point(20, 30)
        lblSelectedEmployee.Name = "lblSelectedEmployee"
        lblSelectedEmployee.Size = New Size(200, 17)
        lblSelectedEmployee.TabIndex = 0
        lblSelectedEmployee.Text = "No employee selected"
        ' 
        ' ---------- COLUMN A ----------
        ' 
        lblLoanCode.Location = New Point(20, 58)
        lblLoanCode.Name = "lblLoanCode"
        lblLoanCode.Size = New Size(55, 13)
        lblLoanCode.TabIndex = 1
        lblLoanCode.Text = "Loan Code"
        ' 
        lueLoanCode.Location = New Point(20, 76)
        lueLoanCode.Name = "lueLoanCode"
        lueLoanCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lueLoanCode.Size = New Size(210, 20)
        lueLoanCode.TabIndex = 2
        ' 
        lblLoanStartDate.Location = New Point(20, 104)
        lblLoanStartDate.Name = "lblLoanStartDate"
        lblLoanStartDate.Size = New Size(80, 13)
        lblLoanStartDate.TabIndex = 3
        lblLoanStartDate.Text = "Loan Start Date"
        ' 
        dtLoanStartDate.EditValue = Nothing
        dtLoanStartDate.Location = New Point(20, 122)
        dtLoanStartDate.Name = "dtLoanStartDate"
        dtLoanStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtLoanStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtLoanStartDate.Size = New Size(210, 20)
        dtLoanStartDate.TabIndex = 4
        ' 
        lblInputDate.Location = New Point(20, 150)
        lblInputDate.Name = "lblInputDate"
        lblInputDate.Size = New Size(52, 13)
        lblInputDate.TabIndex = 5
        lblInputDate.Text = "Input Date"
        ' 
        dtInputDate.EditValue = Nothing
        dtInputDate.Location = New Point(20, 168)
        dtInputDate.Name = "dtInputDate"
        dtInputDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtInputDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtInputDate.Properties.ReadOnly = True
        dtInputDate.Size = New Size(210, 20)
        dtInputDate.TabIndex = 6
        ' 
        lblFrequency.Location = New Point(20, 196)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(53, 13)
        lblFrequency.TabIndex = 7
        lblFrequency.Text = "Frequency"
        ' 
        cboFrequency.Location = New Point(20, 214)
        cboFrequency.Name = "cboFrequency"
        cboFrequency.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboFrequency.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboFrequency.Size = New Size(210, 20)
        cboFrequency.TabIndex = 8
        ' 
        ' ---------- COLUMN B ----------
        ' 
        lblAmortizationMethod.Location = New Point(255, 58)
        lblAmortizationMethod.Name = "lblAmortizationMethod"
        lblAmortizationMethod.Size = New Size(107, 13)
        lblAmortizationMethod.TabIndex = 9
        lblAmortizationMethod.Text = "Amortization Method"
        ' 
        rgAmortizationMethod.Location = New Point(255, 76)
        rgAmortizationMethod.Name = "rgAmortizationMethod"
        rgAmortizationMethod.Properties.Appearance.BackColor = Color.Transparent
        rgAmortizationMethod.Properties.Appearance.Options.UseBackColor = True
        rgAmortizationMethod.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgAmortizationMethod.Properties.Columns = 2
        rgAmortizationMethod.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Based on Term", "Based on Term"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Based on Amort", "Based on Amort")})
        rgAmortizationMethod.Size = New Size(240, 22)
        rgAmortizationMethod.TabIndex = 10
        ' 
        lblInterestMethod.Location = New Point(255, 104)
        lblInterestMethod.Name = "lblInterestMethod"
        lblInterestMethod.Size = New Size(80, 13)
        lblInterestMethod.TabIndex = 11
        lblInterestMethod.Text = "Interest Method"
        ' 
        rgInterestMethod.Location = New Point(255, 122)
        rgInterestMethod.Name = "rgInterestMethod"
        rgInterestMethod.Properties.Appearance.BackColor = Color.Transparent
        rgInterestMethod.Properties.Appearance.Options.UseBackColor = True
        rgInterestMethod.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgInterestMethod.Properties.Columns = 2
        rgInterestMethod.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Straight Line", "Straight Line"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Diminishing", "Diminishing")})
        rgInterestMethod.Size = New Size(240, 22)
        rgInterestMethod.TabIndex = 12
        ' 
        lblInterestRate.Location = New Point(255, 150)
        lblInterestRate.Name = "lblInterestRate"
        lblInterestRate.Size = New Size(65, 13)
        lblInterestRate.TabIndex = 13
        lblInterestRate.Text = "Interest Rate"
        ' 
        spnInterestRate.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnInterestRate.Location = New Point(255, 168)
        spnInterestRate.Name = "spnInterestRate"
        spnInterestRate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnInterestRate.Properties.DisplayFormat.FormatString = "n4"
        spnInterestRate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestRate.Properties.EditFormat.FormatString = "n4"
        spnInterestRate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestRate.Properties.MaxValue = New Decimal(New Integer() {1000, 0, 0, 0})
        spnInterestRate.Size = New Size(110, 20)
        spnInterestRate.TabIndex = 14
        ' 
        lblTerms.Location = New Point(385, 150)
        lblTerms.Name = "lblTerms"
        lblTerms.Size = New Size(31, 13)
        lblTerms.TabIndex = 15
        lblTerms.Text = "Terms"
        ' 
        spnTerms.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnTerms.Location = New Point(385, 168)
        spnTerms.Name = "spnTerms"
        spnTerms.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnTerms.Properties.IsFloatValue = False
        spnTerms.Properties.DisplayFormat.FormatString = "n0"
        spnTerms.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTerms.Properties.EditFormat.FormatString = "n0"
        spnTerms.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTerms.Properties.MaxValue = New Decimal(New Integer() {600, 0, 0, 0})
        spnTerms.Size = New Size(110, 20)
        spnTerms.TabIndex = 16
        ' 
        ' ---------- COLUMN C ----------
        ' 
        lblPrincipalAmount.Location = New Point(520, 58)
        lblPrincipalAmount.Name = "lblPrincipalAmount"
        lblPrincipalAmount.Size = New Size(88, 13)
        lblPrincipalAmount.TabIndex = 17
        lblPrincipalAmount.Text = "Principal Amount"
        ' 
        spnPrincipalAmount.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnPrincipalAmount.Location = New Point(520, 76)
        spnPrincipalAmount.Name = "spnPrincipalAmount"
        spnPrincipalAmount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnPrincipalAmount.Properties.DisplayFormat.FormatString = "n2"
        spnPrincipalAmount.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnPrincipalAmount.Properties.EditFormat.FormatString = "n2"
        spnPrincipalAmount.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnPrincipalAmount.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnPrincipalAmount.Size = New Size(170, 20)
        spnPrincipalAmount.TabIndex = 18
        ' 
        lblInterestAmount.Location = New Point(520, 104)
        lblInterestAmount.Name = "lblInterestAmount"
        lblInterestAmount.Size = New Size(40, 13)
        lblInterestAmount.TabIndex = 19
        lblInterestAmount.Text = "Interest"
        ' 
        spnInterestAmount.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnInterestAmount.Location = New Point(520, 122)
        spnInterestAmount.Name = "spnInterestAmount"
        spnInterestAmount.Properties.DisplayFormat.FormatString = "n2"
        spnInterestAmount.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestAmount.Properties.EditFormat.FormatString = "n2"
        spnInterestAmount.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestAmount.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnInterestAmount.Properties.ReadOnly = True
        spnInterestAmount.Size = New Size(170, 20)
        spnInterestAmount.TabIndex = 20
        ' 
        lblTotalLoanAmount.Location = New Point(520, 150)
        lblTotalLoanAmount.Name = "lblTotalLoanAmount"
        lblTotalLoanAmount.Size = New Size(96, 13)
        lblTotalLoanAmount.TabIndex = 21
        lblTotalLoanAmount.Text = "Total Loan Amount"
        ' 
        spnTotalLoanAmount.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnTotalLoanAmount.Location = New Point(520, 168)
        spnTotalLoanAmount.Name = "spnTotalLoanAmount"
        spnTotalLoanAmount.Properties.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        spnTotalLoanAmount.Properties.Appearance.Options.UseFont = True
        spnTotalLoanAmount.Properties.DisplayFormat.FormatString = "n2"
        spnTotalLoanAmount.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTotalLoanAmount.Properties.EditFormat.FormatString = "n2"
        spnTotalLoanAmount.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTotalLoanAmount.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnTotalLoanAmount.Properties.ReadOnly = True
        spnTotalLoanAmount.Size = New Size(170, 20)
        spnTotalLoanAmount.TabIndex = 22
        ' 
        ' ---------- COLUMN D ----------
        ' 
        lblPrincipalAmortization.Location = New Point(715, 58)
        lblPrincipalAmortization.Name = "lblPrincipalAmortization"
        lblPrincipalAmortization.Size = New Size(117, 13)
        lblPrincipalAmortization.TabIndex = 23
        lblPrincipalAmortization.Text = "Principal Amortization"
        ' 
        spnPrincipalAmortization.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnPrincipalAmortization.Location = New Point(715, 76)
        spnPrincipalAmortization.Name = "spnPrincipalAmortization"
        spnPrincipalAmortization.Properties.DisplayFormat.FormatString = "n2"
        spnPrincipalAmortization.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnPrincipalAmortization.Properties.EditFormat.FormatString = "n2"
        spnPrincipalAmortization.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnPrincipalAmortization.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnPrincipalAmortization.Properties.ReadOnly = True
        spnPrincipalAmortization.Size = New Size(170, 20)
        spnPrincipalAmortization.TabIndex = 24
        ' 
        lblInterestAmortization.Location = New Point(715, 104)
        lblInterestAmortization.Name = "lblInterestAmortization"
        lblInterestAmortization.Size = New Size(110, 13)
        lblInterestAmortization.TabIndex = 25
        lblInterestAmortization.Text = "Interest Amortization"
        ' 
        spnInterestAmortization.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnInterestAmortization.Location = New Point(715, 122)
        spnInterestAmortization.Name = "spnInterestAmortization"
        spnInterestAmortization.Properties.DisplayFormat.FormatString = "n2"
        spnInterestAmortization.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestAmortization.Properties.EditFormat.FormatString = "n2"
        spnInterestAmortization.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnInterestAmortization.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnInterestAmortization.Properties.ReadOnly = True
        spnInterestAmortization.Size = New Size(170, 20)
        spnInterestAmortization.TabIndex = 26
        ' 
        lblTotalAmortization.Location = New Point(715, 150)
        lblTotalAmortization.Name = "lblTotalAmortization"
        lblTotalAmortization.Size = New Size(98, 13)
        lblTotalAmortization.TabIndex = 27
        lblTotalAmortization.Text = "Total Amortization"
        ' 
        spnTotalAmortization.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnTotalAmortization.Location = New Point(715, 168)
        spnTotalAmortization.Name = "spnTotalAmortization"
        spnTotalAmortization.Properties.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        spnTotalAmortization.Properties.Appearance.Options.UseFont = True
        spnTotalAmortization.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnTotalAmortization.Properties.DisplayFormat.FormatString = "n2"
        spnTotalAmortization.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTotalAmortization.Properties.EditFormat.FormatString = "n2"
        spnTotalAmortization.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnTotalAmortization.Properties.MaxValue = New Decimal(New Integer() {100000000, 0, 0, 0})
        spnTotalAmortization.Size = New Size(170, 20)
        spnTotalAmortization.TabIndex = 28
        ' 
        ' ---------- BOTTOM ROW ----------
        ' 
        lblRemark.Location = New Point(255, 196)
        lblRemark.Name = "lblRemark"
        lblRemark.Size = New Size(40, 13)
        lblRemark.TabIndex = 29
        lblRemark.Text = "Remark"
        ' 
        txtRemark.Location = New Point(255, 214)
        txtRemark.Name = "txtRemark"
        txtRemark.Properties.MaxLength = 500
        txtRemark.Size = New Size(435, 20)
        txtRemark.TabIndex = 30
        ' 
        lblStatusCaption.Location = New Point(715, 196)
        lblStatusCaption.Name = "lblStatusCaption"
        lblStatusCaption.Size = New Size(33, 13)
        lblStatusCaption.TabIndex = 31
        lblStatusCaption.Text = "Status"
        ' 
        lblStatusValue.Appearance.Font = New Font("Segoe UI", 11.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatusValue.Appearance.Options.UseFont = True
        lblStatusValue.Location = New Point(715, 214)
        lblStatusValue.Name = "lblStatusValue"
        lblStatusValue.Size = New Size(60, 18)
        lblStatusValue.TabIndex = 32
        lblStatusValue.Text = "-"
        ' 
        ' grpLoanList
        ' 
        grpLoanList.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpLoanList.AppearanceCaption.Options.UseFont = True
        grpLoanList.Controls.Add(gridconLoanList)
        grpLoanList.Controls.Add(pnlLoanFilter)
        grpLoanList.Dock = DockStyle.Fill
        grpLoanList.Location = New Point(4, 366)
        grpLoanList.Name = "grpLoanList"
        grpLoanList.Size = New Size(922, 330)
        grpLoanList.TabIndex = 2
        grpLoanList.Text = " LOAN LIST"
        ' 
        ' gridconLoanList
        ' 
        gridconLoanList.Dock = DockStyle.Fill
        gridconLoanList.Location = New Point(2, 31)
        gridconLoanList.MainView = gridviewLoanList
        gridconLoanList.Name = "gridconLoanList"
        gridconLoanList.Size = New Size(918, 269)
        gridconLoanList.TabIndex = 0
        gridconLoanList.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewLoanList})
        ' 
        ' gridviewLoanList
        ' 
        gridviewLoanList.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {
            colLoanCode, colLoanDescription, colLoanStartDate, colInterestRate, colTerms,
            colTotalLoanAmount, colTotalAmortization, colTotalPaid, colBalance,
            colNextDeductionDate, colProgress, colStatus})
        gridviewLoanList.GridControl = gridconLoanList
        gridviewLoanList.Name = "gridviewLoanList"
        gridviewLoanList.OptionsView.ShowGroupPanel = False
        ' 
        colLoanCode.Caption = "Loan Code"
        colLoanCode.FieldName = "LoanCode"
        colLoanCode.Name = "colLoanCode"
        colLoanCode.Visible = True
        colLoanCode.VisibleIndex = 0
        colLoanCode.Width = 80
        ' 
        colLoanDescription.Caption = "Loan Description"
        colLoanDescription.FieldName = "LoanDescription"
        colLoanDescription.Name = "colLoanDescription"
        colLoanDescription.Visible = True
        colLoanDescription.VisibleIndex = 1
        colLoanDescription.Width = 200
        ' 
        colLoanStartDate.Caption = "Start Date"
        colLoanStartDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        colLoanStartDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        colLoanStartDate.FieldName = "LoanStartDate"
        colLoanStartDate.Name = "colLoanStartDate"
        colLoanStartDate.Visible = True
        colLoanStartDate.VisibleIndex = 2
        colLoanStartDate.Width = 85
        ' 
        colInterestRate.Caption = "Int. Rate"
        colInterestRate.DisplayFormat.FormatString = "n2"
        colInterestRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colInterestRate.FieldName = "InterestRate"
        colInterestRate.Name = "colInterestRate"
        colInterestRate.Visible = True
        colInterestRate.VisibleIndex = 3
        colInterestRate.Width = 65
        ' 
        colTerms.Caption = "Terms"
        colTerms.FieldName = "Terms"
        colTerms.Name = "colTerms"
        colTerms.Visible = True
        colTerms.VisibleIndex = 4
        colTerms.Width = 55
        ' 
        colTotalLoanAmount.Caption = "Total Loan Amt."
        colTotalLoanAmount.DisplayFormat.FormatString = "n2"
        colTotalLoanAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colTotalLoanAmount.FieldName = "TotalLoanAmount"
        colTotalLoanAmount.Name = "colTotalLoanAmount"
        colTotalLoanAmount.Visible = True
        colTotalLoanAmount.VisibleIndex = 5
        colTotalLoanAmount.Width = 105
        ' 
        colTotalAmortization.Caption = "Amortization"
        colTotalAmortization.DisplayFormat.FormatString = "n2"
        colTotalAmortization.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colTotalAmortization.FieldName = "TotalAmortization"
        colTotalAmortization.Name = "colTotalAmortization"
        colTotalAmortization.Visible = True
        colTotalAmortization.VisibleIndex = 6
        colTotalAmortization.Width = 95
        ' 
        colTotalPaid.Caption = "Total Paid"
        colTotalPaid.DisplayFormat.FormatString = "n2"
        colTotalPaid.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colTotalPaid.FieldName = "TotalPaid"
        colTotalPaid.Name = "colTotalPaid"
        colTotalPaid.Visible = True
        colTotalPaid.VisibleIndex = 7
        colTotalPaid.Width = 95
        ' 
        colBalance.Caption = "Balance"
        colBalance.DisplayFormat.FormatString = "n2"
        colBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colBalance.FieldName = "Balance"
        colBalance.Name = "colBalance"
        colBalance.Visible = True
        colBalance.VisibleIndex = 8
        colBalance.Width = 95
        ' 
        colNextDeductionDate.Caption = "Next Deduction"
        colNextDeductionDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        colNextDeductionDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        colNextDeductionDate.FieldName = "NextDeductionDate"
        colNextDeductionDate.Name = "colNextDeductionDate"
        colNextDeductionDate.Visible = True
        colNextDeductionDate.VisibleIndex = 9
        colNextDeductionDate.Width = 100
        ' 
        colProgress.Caption = "Progress"
        colProgress.FieldName = "ProgressText"
        colProgress.Name = "colProgress"
        colProgress.Visible = True
        colProgress.VisibleIndex = 10
        colProgress.Width = 90
        ' 
        colStatus.Caption = "Status"
        colStatus.FieldName = "Status"
        colStatus.Name = "colStatus"
        colStatus.Visible = True
        colStatus.VisibleIndex = 11
        colStatus.Width = 80
        ' 
        ' pnlLoanFilter
        ' 
        pnlLoanFilter.Controls.Add(rgLoanStatusFilter)
        pnlLoanFilter.Controls.Add(lblLoanFilter)
        pnlLoanFilter.Dock = DockStyle.Bottom
        pnlLoanFilter.Location = New Point(2, 300)
        pnlLoanFilter.Name = "pnlLoanFilter"
        pnlLoanFilter.Size = New Size(918, 28)
        pnlLoanFilter.TabIndex = 1
        ' 
        ' rgLoanStatusFilter
        ' 
        rgLoanStatusFilter.Dock = DockStyle.Fill
        rgLoanStatusFilter.Location = New Point(84, 2)
        rgLoanStatusFilter.Name = "rgLoanStatusFilter"
        rgLoanStatusFilter.Properties.AllowMouseWheel = False
        rgLoanStatusFilter.Properties.Appearance.BackColor = Color.Transparent
        rgLoanStatusFilter.Properties.Appearance.Options.UseBackColor = True
        rgLoanStatusFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        rgLoanStatusFilter.Properties.ItemHorzAlignment = DevExpress.XtraEditors.RadioItemHorzAlignment.Near
        rgLoanStatusFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Active", "Active"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Inactive", "Inactive"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("Completed", "Completed"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem("All", "All")})
        rgLoanStatusFilter.Properties.Padding = New Padding(0)
        rgLoanStatusFilter.Size = New Size(832, 24)
        rgLoanStatusFilter.TabIndex = 0
        rgLoanStatusFilter.TabStop = False
        ' 
        ' lblLoanFilter
        ' 
        lblLoanFilter.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblLoanFilter.Dock = DockStyle.Left
        lblLoanFilter.Location = New Point(2, 2)
        lblLoanFilter.Name = "lblLoanFilter"
        lblLoanFilter.Size = New Size(82, 24)
        lblLoanFilter.TabIndex = 1
        lblLoanFilter.Text = "Loan Status:"
        ' 
        ' ucLoans
        ' 
        ' MAHALAGA ANG SUNOD-SUNOD NG Controls.Add DITO.
        ' Ang HULING idinagdag ang kumukuha ng pinakalabas na gilid.
        '   grpLoanList (Fill)      - idinagdag muna, kumukuha ng natira
        '   grpEmployeeList (Right) - kumakagat sa kanan
        '   grpDetails (Top)        - nasa ilalim ng command bar
        '   PanelControl1 (Top)     - huli, kaya siya ang pinakataas
        ' 
        Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Appearance.Options.UseFont = True
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpLoanList)
        Controls.Add(grpEmployeeList)
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucLoans"
        Padding = New Padding(4)
        Size = New Size(1200, 700)

        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpEmployeeList, ComponentModel.ISupportInitialize).EndInit()
        grpEmployeeList.ResumeLayout(False)
        CType(gridconEmployeeList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewEmployeeList, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlEmployeeFilter, ComponentModel.ISupportInitialize).EndInit()
        pnlEmployeeFilter.ResumeLayout(False)
        CType(rgEmployeeFilter.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(lueLoanCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtLoanStartDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtLoanStartDate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtInputDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtInputDate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboFrequency.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(rgAmortizationMethod.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(rgInterestMethod.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnInterestRate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnTerms.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnPrincipalAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnInterestAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnTotalLoanAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnPrincipalAmortization.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnInterestAmortization.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnTotalAmortization.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtRemark.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpLoanList, ComponentModel.ISupportInitialize).EndInit()
        grpLoanList.ResumeLayout(False)
        CType(gridconLoanList, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewLoanList, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlLoanFilter, ComponentModel.ISupportInitialize).EndInit()
        pnlLoanFilter.ResumeLayout(False)
        CType(rgLoanStatusFilter.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents wbpMainCommands As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel

    Friend WithEvents grpEmployeeList As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconEmployeeList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewEmployeeList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents pnlEmployeeFilter As DevExpress.XtraEditors.PanelControl
    Friend WithEvents rgEmployeeFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblEmployeeFilter As DevExpress.XtraEditors.LabelControl

    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblSelectedEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblLoanCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lueLoanCode As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblLoanStartDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtLoanStartDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblInputDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtInputDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblFrequency As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboFrequency As DevExpress.XtraEditors.ComboBoxEdit

    Friend WithEvents lblAmortizationMethod As DevExpress.XtraEditors.LabelControl
    Friend WithEvents rgAmortizationMethod As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblInterestMethod As DevExpress.XtraEditors.LabelControl
    Friend WithEvents rgInterestMethod As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblInterestRate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnInterestRate As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblTerms As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnTerms As DevExpress.XtraEditors.SpinEdit

    Friend WithEvents lblPrincipalAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnPrincipalAmount As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblInterestAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnInterestAmount As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblTotalLoanAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnTotalLoanAmount As DevExpress.XtraEditors.SpinEdit

    Friend WithEvents lblPrincipalAmortization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnPrincipalAmortization As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblInterestAmortization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnInterestAmortization As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblTotalAmortization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnTotalAmortization As DevExpress.XtraEditors.SpinEdit

    Friend WithEvents lblRemark As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtRemark As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblStatusCaption As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblStatusValue As DevExpress.XtraEditors.LabelControl

    Friend WithEvents grpLoanList As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconLoanList As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewLoanList As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colLoanCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLoanDescription As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLoanStartDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colInterestRate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTerms As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotalLoanAmount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotalAmortization As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotalPaid As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colBalance As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colNextDeductionDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colProgress As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colStatus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents pnlLoanFilter As DevExpress.XtraEditors.PanelControl
    Friend WithEvents rgLoanStatusFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents lblLoanFilter As DevExpress.XtraEditors.LabelControl

End Class