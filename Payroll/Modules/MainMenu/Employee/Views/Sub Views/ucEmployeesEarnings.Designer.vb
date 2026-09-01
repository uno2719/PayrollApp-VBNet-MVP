<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucEmployeesEarnings
    Inherits DevExpress.XtraEditors.XtraUserControl

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
        lcEarnings = New DevExpress.XtraLayout.LayoutControl()
        txtBasicSalary = New DevExpress.XtraEditors.TextEdit()
        txtDailyRate = New DevExpress.XtraEditors.TextEdit()
        txtHourlyRate = New DevExpress.XtraEditors.TextEdit()
        txtDaysInYear = New DevExpress.XtraEditors.TextEdit()
        txtWorkHourPer = New DevExpress.XtraEditors.TextEdit()
        chkPayrollFlag = New DevExpress.XtraEditors.CheckEdit()
        chkMinimumWage = New DevExpress.XtraEditors.CheckEdit()
        cboPayCycle = New DevExpress.XtraEditors.ComboBoxEdit()
        cboTaxFlag = New DevExpress.XtraEditors.ComboBoxEdit()
        cboPayBy = New DevExpress.XtraEditors.ComboBoxEdit()
        sleBank = New DevExpress.XtraEditors.SearchLookUpEdit()
        sleBankView = New DevExpress.XtraGrid.Views.Grid.GridView()
        txtBankAccount = New DevExpress.XtraEditors.TextEdit()
        SeparatorHeader = New DevExpress.XtraEditors.SeparatorControl()
        SeparatorPayment = New DevExpress.XtraEditors.SeparatorControl()
        Root_Earnings = New DevExpress.XtraLayout.LayoutControlGroup()
        lciHeader = New DevExpress.XtraLayout.LayoutControlItem()
        lciBasicSalary = New DevExpress.XtraLayout.LayoutControlItem()
        lciDailyRate = New DevExpress.XtraLayout.LayoutControlItem()
        lciHourlyRate = New DevExpress.XtraLayout.LayoutControlItem()
        lciDaysInYear = New DevExpress.XtraLayout.LayoutControlItem()
        lciWorkHourPer = New DevExpress.XtraLayout.LayoutControlItem()
        lciPayrollFlag = New DevExpress.XtraLayout.LayoutControlItem()
        lciMinimumWage = New DevExpress.XtraLayout.LayoutControlItem()
        lciSeparatorPayment = New DevExpress.XtraLayout.LayoutControlItem()
        lciPayCycle = New DevExpress.XtraLayout.LayoutControlItem()
        lciTaxFlag = New DevExpress.XtraLayout.LayoutControlItem()
        lciPayBy = New DevExpress.XtraLayout.LayoutControlItem()
        lciBank = New DevExpress.XtraLayout.LayoutControlItem()
        lciBankAccount = New DevExpress.XtraLayout.LayoutControlItem()
        emptyGap = New DevExpress.XtraLayout.EmptySpaceItem()
        emptyBottom = New DevExpress.XtraLayout.EmptySpaceItem()
        CType(lcEarnings, ComponentModel.ISupportInitialize).BeginInit()
        lcEarnings.SuspendLayout()
        CType(txtBasicSalary.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtDailyRate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtHourlyRate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtDaysInYear.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtWorkHourPer.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkPayrollFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkMinimumWage.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboPayCycle.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboTaxFlag.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboPayBy.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(sleBank.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(sleBankView, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtBankAccount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(SeparatorHeader, ComponentModel.ISupportInitialize).BeginInit()
        CType(SeparatorPayment, ComponentModel.ISupportInitialize).BeginInit()
        CType(Root_Earnings, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciHeader, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciBasicSalary, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciDailyRate, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciHourlyRate, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciDaysInYear, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciWorkHourPer, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPayrollFlag, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciMinimumWage, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciSeparatorPayment, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPayCycle, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciTaxFlag, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPayBy, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciBank, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciBankAccount, ComponentModel.ISupportInitialize).BeginInit()
        CType(emptyGap, ComponentModel.ISupportInitialize).BeginInit()
        CType(emptyBottom, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lcEarnings
        ' 
        lcEarnings.AllowCustomization = False
        lcEarnings.Appearance.Control.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lcEarnings.Appearance.Control.Options.UseFont = True
        lcEarnings.Controls.Add(txtBasicSalary)
        lcEarnings.Controls.Add(txtDailyRate)
        lcEarnings.Controls.Add(txtHourlyRate)
        lcEarnings.Controls.Add(txtDaysInYear)
        lcEarnings.Controls.Add(txtWorkHourPer)
        lcEarnings.Controls.Add(chkPayrollFlag)
        lcEarnings.Controls.Add(chkMinimumWage)
        lcEarnings.Controls.Add(cboPayCycle)
        lcEarnings.Controls.Add(cboTaxFlag)
        lcEarnings.Controls.Add(cboPayBy)
        lcEarnings.Controls.Add(sleBank)
        lcEarnings.Controls.Add(txtBankAccount)
        lcEarnings.Controls.Add(SeparatorHeader)
        lcEarnings.Controls.Add(SeparatorPayment)
        lcEarnings.Dock = DockStyle.Fill
        lcEarnings.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lcEarnings.Location = New Point(0, 0)
        lcEarnings.Name = "lcEarnings"
        lcEarnings.Root = Root_Earnings
        lcEarnings.Size = New Size(1067, 713)
        lcEarnings.TabIndex = 0
        ' 
        ' txtBasicSalary
        ' 
        txtBasicSalary.Location = New Point(128, 55)
        txtBasicSalary.Name = "txtBasicSalary"
        txtBasicSalary.Size = New Size(401, 24)
        txtBasicSalary.StyleController = lcEarnings
        txtBasicSalary.TabIndex = 4
        ' 
        ' txtDailyRate
        ' 
        txtDailyRate.Location = New Point(128, 83)
        txtDailyRate.Name = "txtDailyRate"
        txtDailyRate.Size = New Size(401, 24)
        txtDailyRate.StyleController = lcEarnings
        txtDailyRate.TabIndex = 5
        ' 
        ' txtHourlyRate
        ' 
        txtHourlyRate.Location = New Point(128, 111)
        txtHourlyRate.Name = "txtHourlyRate"
        txtHourlyRate.Size = New Size(401, 24)
        txtHourlyRate.StyleController = lcEarnings
        txtHourlyRate.TabIndex = 6
        ' 
        ' txtDaysInYear
        ' 
        txtDaysInYear.EnterMoveNextControl = True
        txtDaysInYear.Location = New Point(128, 139)
        txtDaysInYear.Name = "txtDaysInYear"
        txtDaysInYear.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        txtDaysInYear.Properties.Appearance.Options.UseFont = True
        txtDaysInYear.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        txtDaysInYear.Properties.MaskSettings.Set("mask", "d")
        txtDaysInYear.Properties.TextPadding = New Padding(5, 0, 5, 0)
        txtDaysInYear.Size = New Size(401, 22)
        txtDaysInYear.StyleController = lcEarnings
        txtDaysInYear.TabIndex = 7
        ' 
        ' txtWorkHourPer
        ' 
        txtWorkHourPer.EnterMoveNextControl = True
        txtWorkHourPer.Location = New Point(128, 165)
        txtWorkHourPer.Name = "txtWorkHourPer"
        txtWorkHourPer.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        txtWorkHourPer.Properties.Appearance.Options.UseFont = True
        txtWorkHourPer.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        txtWorkHourPer.Properties.MaskSettings.Set("mask", "f3")
        txtWorkHourPer.Properties.TextPadding = New Padding(5, 0, 5, 0)
        txtWorkHourPer.Size = New Size(401, 22)
        txtWorkHourPer.StyleController = lcEarnings
        txtWorkHourPer.TabIndex = 8
        ' 
        ' chkPayrollFlag
        ' 
        chkPayrollFlag.Location = New Point(12, 191)
        chkPayrollFlag.Name = "chkPayrollFlag"
        chkPayrollFlag.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        chkPayrollFlag.Properties.Appearance.Options.UseFont = True
        chkPayrollFlag.Properties.Caption = "Payroll Flag"
        chkPayrollFlag.Size = New Size(517, 20)
        chkPayrollFlag.StyleController = lcEarnings
        chkPayrollFlag.TabIndex = 9
        ' 
        ' chkMinimumWage
        ' 
        chkMinimumWage.Location = New Point(12, 215)
        chkMinimumWage.Name = "chkMinimumWage"
        chkMinimumWage.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        chkMinimumWage.Properties.Appearance.Options.UseFont = True
        chkMinimumWage.Properties.Caption = "Minimum Wage"
        chkMinimumWage.Size = New Size(517, 20)
        chkMinimumWage.StyleController = lcEarnings
        chkMinimumWage.TabIndex = 10
        ' 
        ' cboPayCycle
        ' 
        cboPayCycle.Location = New Point(661, 103)
        cboPayCycle.Name = "cboPayCycle"
        cboPayCycle.Properties.Items.AddRange(New Object() {"Weekly", "Bi-Weekly", "Semi-Monthly", "Monthly"})
        cboPayCycle.Size = New Size(394, 24)
        cboPayCycle.StyleController = lcEarnings
        cboPayCycle.TabIndex = 11
        ' 
        ' cboTaxFlag
        ' 
        cboTaxFlag.Location = New Point(661, 131)
        cboTaxFlag.Name = "cboTaxFlag"
        cboTaxFlag.Properties.Items.AddRange(New Object() {"Weekly", "Bi-Weekly", "Semi-Monthly", "Monthly"})
        cboTaxFlag.Size = New Size(394, 24)
        cboTaxFlag.StyleController = lcEarnings
        cboTaxFlag.TabIndex = 12
        ' 
        ' cboPayBy
        ' 
        cboPayBy.Location = New Point(661, 159)
        cboPayBy.Name = "cboPayBy"
        cboPayBy.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)})
        cboPayBy.Properties.Items.AddRange(New Object() {"Bank", "Cash", "Cheque"})
        cboPayBy.Size = New Size(394, 24)
        cboPayBy.StyleController = lcEarnings
        cboPayBy.TabIndex = 13
        ' 
        ' sleBank
        ' 
        sleBank.EnterMoveNextControl = True
        sleBank.Location = New Point(661, 187)
        sleBank.Name = "sleBank"
        sleBank.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        sleBank.Properties.Appearance.Options.UseFont = True
        sleBank.Properties.NullText = ""
        sleBank.Properties.PopupView = sleBankView
        sleBank.Properties.TextPadding = New Padding(5, 0, 5, 0)
        sleBank.Size = New Size(394, 22)
        sleBank.StyleController = lcEarnings
        sleBank.TabIndex = 14
        ' 
        ' sleBankView
        ' 
        sleBankView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
        sleBankView.Name = "sleBankView"
        sleBankView.OptionsBehavior.Editable = False
        sleBankView.OptionsSelection.EnableAppearanceFocusedCell = False
        sleBankView.OptionsView.ShowAutoFilterRow = True
        sleBankView.OptionsView.ShowGroupPanel = False
        ' 
        ' txtBankAccount
        ' 
        txtBankAccount.EnterMoveNextControl = True
        txtBankAccount.Location = New Point(661, 213)
        txtBankAccount.Name = "txtBankAccount"
        txtBankAccount.Properties.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        txtBankAccount.Properties.Appearance.Options.UseFont = True
        txtBankAccount.Properties.TextPadding = New Padding(5, 0, 5, 0)
        txtBankAccount.Size = New Size(394, 22)
        txtBankAccount.StyleController = lcEarnings
        txtBankAccount.TabIndex = 15
        ' 
        ' SeparatorHeader
        ' 
        SeparatorHeader.Location = New Point(12, 31)
        SeparatorHeader.Name = "SeparatorHeader"
        SeparatorHeader.Size = New Size(1043, 20)
        SeparatorHeader.TabIndex = 16
        ' 
        ' SeparatorPayment
        ' 
        SeparatorPayment.Location = New Point(545, 74)
        SeparatorPayment.Name = "SeparatorPayment"
        SeparatorPayment.Size = New Size(510, 25)
        SeparatorPayment.TabIndex = 17
        ' 
        ' Root_Earnings
        ' 
        Root_Earnings.AppearanceItemCaption.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Root_Earnings.AppearanceItemCaption.Options.UseFont = True
        Root_Earnings.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
        Root_Earnings.GroupBordersVisible = False
        Root_Earnings.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {lciHeader, lciBasicSalary, lciDailyRate, lciHourlyRate, lciDaysInYear, lciWorkHourPer, lciPayrollFlag, lciMinimumWage, lciSeparatorPayment, lciPayCycle, lciTaxFlag, lciPayBy, lciBank, lciBankAccount, emptyGap, emptyBottom})
        Root_Earnings.Name = "Root_Earnings"
        Root_Earnings.Size = New Size(1067, 713)
        Root_Earnings.TextVisible = False
        ' 
        ' lciHeader
        ' 
        lciHeader.AppearanceItemCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        lciHeader.AppearanceItemCaption.Options.UseFont = True
        lciHeader.Control = SeparatorHeader
        lciHeader.Location = New Point(0, 0)
        lciHeader.Name = "lciHeader"
        lciHeader.Size = New Size(1047, 43)
        lciHeader.Text = "Earnings Details"
        lciHeader.TextLocation = DevExpress.Utils.Locations.Top
        lciHeader.TextSize = New Size(104, 16)
        ' 
        ' lciBasicSalary
        ' 
        lciBasicSalary.Control = txtBasicSalary
        lciBasicSalary.Location = New Point(0, 43)
        lciBasicSalary.Name = "lciBasicSalary"
        lciBasicSalary.Size = New Size(521, 28)
        lciBasicSalary.Text = "Basic Salary"
        lciBasicSalary.TextSize = New Size(104, 17)
        ' 
        ' lciDailyRate
        ' 
        lciDailyRate.Control = txtDailyRate
        lciDailyRate.Location = New Point(0, 71)
        lciDailyRate.Name = "lciDailyRate"
        lciDailyRate.Size = New Size(521, 28)
        lciDailyRate.Text = "Daily Rate"
        lciDailyRate.TextSize = New Size(104, 17)
        ' 
        ' lciHourlyRate
        ' 
        lciHourlyRate.Control = txtHourlyRate
        lciHourlyRate.Location = New Point(0, 99)
        lciHourlyRate.Name = "lciHourlyRate"
        lciHourlyRate.Size = New Size(521, 28)
        lciHourlyRate.Text = "Hourly Rate"
        lciHourlyRate.TextSize = New Size(104, 17)
        ' 
        ' lciDaysInYear
        ' 
        lciDaysInYear.Control = txtDaysInYear
        lciDaysInYear.Location = New Point(0, 127)
        lciDaysInYear.Name = "lciDaysInYear"
        lciDaysInYear.Size = New Size(521, 26)
        lciDaysInYear.Text = "Days in Year"
        lciDaysInYear.TextSize = New Size(104, 17)
        ' 
        ' lciWorkHourPer
        ' 
        lciWorkHourPer.Control = txtWorkHourPer
        lciWorkHourPer.Location = New Point(0, 153)
        lciWorkHourPer.Name = "lciWorkHourPer"
        lciWorkHourPer.Size = New Size(521, 26)
        lciWorkHourPer.Text = "Work Hour Per"
        lciWorkHourPer.TextSize = New Size(104, 17)
        ' 
        ' lciPayrollFlag
        ' 
        lciPayrollFlag.Control = chkPayrollFlag
        lciPayrollFlag.Location = New Point(0, 179)
        lciPayrollFlag.Name = "lciPayrollFlag"
        lciPayrollFlag.Size = New Size(521, 24)
        lciPayrollFlag.TextVisible = False
        ' 
        ' lciMinimumWage
        ' 
        lciMinimumWage.Control = chkMinimumWage
        lciMinimumWage.Location = New Point(0, 203)
        lciMinimumWage.Name = "lciMinimumWage"
        lciMinimumWage.Size = New Size(521, 24)
        lciMinimumWage.TextVisible = False
        ' 
        ' lciSeparatorPayment
        ' 
        lciSeparatorPayment.AppearanceItemCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        lciSeparatorPayment.AppearanceItemCaption.Options.UseFont = True
        lciSeparatorPayment.Control = SeparatorPayment
        lciSeparatorPayment.Location = New Point(533, 43)
        lciSeparatorPayment.Name = "lciSeparatorPayment"
        lciSeparatorPayment.Size = New Size(514, 48)
        lciSeparatorPayment.Text = "Payment Details"
        lciSeparatorPayment.TextLocation = DevExpress.Utils.Locations.Top
        lciSeparatorPayment.TextSize = New Size(104, 16)
        ' 
        ' lciPayCycle
        ' 
        lciPayCycle.Control = cboPayCycle
        lciPayCycle.Location = New Point(533, 91)
        lciPayCycle.Name = "lciPayCycle"
        lciPayCycle.Size = New Size(514, 28)
        lciPayCycle.Text = "Pay Cycle"
        lciPayCycle.TextSize = New Size(104, 17)
        ' 
        ' lciTaxFlag
        ' 
        lciTaxFlag.Control = cboTaxFlag
        lciTaxFlag.Location = New Point(533, 119)
        lciTaxFlag.Name = "lciTaxFlag"
        lciTaxFlag.Size = New Size(514, 28)
        lciTaxFlag.Text = "Tax Flag"
        lciTaxFlag.TextSize = New Size(104, 17)
        ' 
        ' lciPayBy
        ' 
        lciPayBy.Control = cboPayBy
        lciPayBy.Location = New Point(533, 147)
        lciPayBy.Name = "lciPayBy"
        lciPayBy.Size = New Size(514, 28)
        lciPayBy.Text = "Pay By"
        lciPayBy.TextSize = New Size(104, 17)
        ' 
        ' lciBank
        ' 
        lciBank.Control = sleBank
        lciBank.Location = New Point(533, 175)
        lciBank.Name = "lciBank"
        lciBank.Size = New Size(514, 26)
        lciBank.Text = "Bank Name"
        lciBank.TextSize = New Size(104, 17)
        ' 
        ' lciBankAccount
        ' 
        lciBankAccount.Control = txtBankAccount
        lciBankAccount.Location = New Point(533, 201)
        lciBankAccount.Name = "lciBankAccount"
        lciBankAccount.Size = New Size(514, 26)
        lciBankAccount.Text = "Bank Account"
        lciBankAccount.TextSize = New Size(104, 17)
        ' 
        ' emptyGap
        ' 
        emptyGap.Location = New Point(521, 43)
        emptyGap.Name = "emptyGap"
        emptyGap.Size = New Size(12, 184)
        ' 
        ' emptyBottom
        ' 
        emptyBottom.Location = New Point(0, 227)
        emptyBottom.Name = "emptyBottom"
        emptyBottom.Size = New Size(1047, 466)
        ' 
        ' ucEmployeesEarnings
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(lcEarnings)
        Name = "ucEmployeesEarnings"
        Size = New Size(1067, 713)
        CType(lcEarnings, ComponentModel.ISupportInitialize).EndInit()
        lcEarnings.ResumeLayout(False)
        CType(txtBasicSalary.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtDailyRate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtHourlyRate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtDaysInYear.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtWorkHourPer.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkPayrollFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkMinimumWage.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboPayCycle.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboTaxFlag.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboPayBy.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(sleBank.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(sleBankView, ComponentModel.ISupportInitialize).EndInit()
        CType(txtBankAccount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(SeparatorHeader, ComponentModel.ISupportInitialize).EndInit()
        CType(SeparatorPayment, ComponentModel.ISupportInitialize).EndInit()
        CType(Root_Earnings, ComponentModel.ISupportInitialize).EndInit()
        CType(lciHeader, ComponentModel.ISupportInitialize).EndInit()
        CType(lciBasicSalary, ComponentModel.ISupportInitialize).EndInit()
        CType(lciDailyRate, ComponentModel.ISupportInitialize).EndInit()
        CType(lciHourlyRate, ComponentModel.ISupportInitialize).EndInit()
        CType(lciDaysInYear, ComponentModel.ISupportInitialize).EndInit()
        CType(lciWorkHourPer, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPayrollFlag, ComponentModel.ISupportInitialize).EndInit()
        CType(lciMinimumWage, ComponentModel.ISupportInitialize).EndInit()
        CType(lciSeparatorPayment, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPayCycle, ComponentModel.ISupportInitialize).EndInit()
        CType(lciTaxFlag, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPayBy, ComponentModel.ISupportInitialize).EndInit()
        CType(lciBank, ComponentModel.ISupportInitialize).EndInit()
        CType(lciBankAccount, ComponentModel.ISupportInitialize).EndInit()
        CType(emptyGap, ComponentModel.ISupportInitialize).EndInit()
        CType(emptyBottom, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    ' =============================================
    ' HELPER METHODS
    ' =============================================
    Private Sub SetupCurrencyEdit(txt As DevExpress.XtraEditors.TextEdit,
                                   name As String,
                                   _readOnly As Boolean)
        txt.EnterMoveNextControl = True
        txt.Name = name
        txt.Properties.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        txt.Properties.Appearance.Options.UseFont = True
        txt.Properties.TextPadding = New Padding(5, 0, 5, 0)
        txt.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        txt.Properties.MaskSettings.Set("mask", "f2")
        txt.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        txt.Properties.DisplayFormat.FormatString = "n2"
        If _readOnly Then
            txt.Properties.ReadOnly = True
        End If
    End Sub

    Private Sub SetupComboBox(cbo As DevExpress.XtraEditors.ComboBoxEdit,
                               name As String,
                               items As String())
        cbo.EnterMoveNextControl = True
        cbo.Name = name
        cbo.Properties.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        cbo.Properties.Appearance.Options.UseFont = True
        cbo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {
            New DevExpress.XtraEditors.Controls.EditorButton(
                DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cbo.Properties.Items.AddRange(items)
        cbo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cbo.Properties.TextPadding = New Padding(5, 0, 5, 0)
    End Sub

    ' =============================================
    ' FIELD DECLARATIONS
    ' =============================================
    Friend WithEvents lcEarnings As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root_Earnings As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SeparatorHeader As DevExpress.XtraEditors.SeparatorControl
    Friend WithEvents SeparatorPayment As DevExpress.XtraEditors.SeparatorControl
    Friend WithEvents txtBasicSalary As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDailyRate As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtHourlyRate As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtDaysInYear As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtWorkHourPer As DevExpress.XtraEditors.TextEdit
    Friend WithEvents chkPayrollFlag As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents chkMinimumWage As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cboPayCycle As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cboTaxFlag As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents cboPayBy As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents sleBank As DevExpress.XtraEditors.SearchLookUpEdit
    Friend WithEvents sleBankView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents txtBankAccount As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lciHeader As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciBasicSalary As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciDailyRate As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciHourlyRate As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciDaysInYear As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciWorkHourPer As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPayrollFlag As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciMinimumWage As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciSeparatorPayment As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPayCycle As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciTaxFlag As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPayBy As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciBank As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciBankAccount As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents emptyGap As DevExpress.XtraLayout.EmptySpaceItem
    Friend WithEvents emptyBottom As DevExpress.XtraLayout.EmptySpaceItem

End Class