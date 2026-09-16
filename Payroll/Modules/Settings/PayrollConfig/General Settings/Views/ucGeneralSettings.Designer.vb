' File: Modules/Settings/SysConfig/GeneralSettings/Views/ucGeneralSettings.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucGeneralSettings
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
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        btnSave = New DevExpress.XtraEditors.SimpleButton()
        grpPayrollParameters = New DevExpress.XtraEditors.GroupControl()
        txtPercentPrecision = New DevExpress.XtraEditors.TextEdit()
        lblPercentPrecision = New DevExpress.XtraEditors.LabelControl()
        txtAmountPrecision = New DevExpress.XtraEditors.TextEdit()
        lblAmountPrecision = New DevExpress.XtraEditors.LabelControl()
        txtWorkHourPerDay = New DevExpress.XtraEditors.TextEdit()
        lblWorkHourPerDay = New DevExpress.XtraEditors.LabelControl()
        txtTotalDaysPerYear = New DevExpress.XtraEditors.TextEdit()
        lblTotalDaysPerYear = New DevExpress.XtraEditors.LabelControl()
        txtBonusCeiling = New DevExpress.XtraEditors.TextEdit()
        lblBonusCeiling = New DevExpress.XtraEditors.LabelControl()
        grpCodeMapping = New DevExpress.XtraEditors.GroupControl()
        lookupEarlyOutCode = New DevExpress.XtraEditors.LookUpEdit()
        lblEarlyOutCode = New DevExpress.XtraEditors.LabelControl()
        lookupLateInCode = New DevExpress.XtraEditors.LookUpEdit()
        lblLateInCode = New DevExpress.XtraEditors.LabelControl()
        lookupAbsentCode = New DevExpress.XtraEditors.LookUpEdit()
        lblAbsentCode = New DevExpress.XtraEditors.LabelControl()
        lookupBasicSalaryCodeMinus = New DevExpress.XtraEditors.LookUpEdit()
        lblBasicSalaryCodeMinus = New DevExpress.XtraEditors.LabelControl()
        lookupBasicSalaryCodePlus = New DevExpress.XtraEditors.LookUpEdit()
        lblBasicSalaryCodePlus = New DevExpress.XtraEditors.LabelControl()
        grpStatutoryBasis = New DevExpress.XtraEditors.GroupControl()
        cboPhilHealthBasedOn = New DevExpress.XtraEditors.ComboBoxEdit()
        lblPhilHealthBasedOn = New DevExpress.XtraEditors.LabelControl()
        cboSSSBasedOn = New DevExpress.XtraEditors.ComboBoxEdit()
        lblSSSBasedOn = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpPayrollParameters, ComponentModel.ISupportInitialize).BeginInit()
        grpPayrollParameters.SuspendLayout()
        CType(txtPercentPrecision.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAmountPrecision.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtWorkHourPerDay.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtTotalDaysPerYear.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtBonusCeiling.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpCodeMapping, ComponentModel.ISupportInitialize).BeginInit()
        grpCodeMapping.SuspendLayout()
        CType(lookupEarlyOutCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupLateInCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupAbsentCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupBasicSalaryCodeMinus.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupBasicSalaryCodePlus.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpStatutoryBasis, ComponentModel.ISupportInitialize).BeginInit()
        grpStatutoryBasis.SuspendLayout()
        CType(cboPhilHealthBasedOn.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboSSSBasedOn.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' PanelControl1
        ' 
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(btnSave)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Size = New Size(1069, 56)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(198, 30)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "GENERAL SETTINGS"
        ' 
        ' btnSave
        ' 
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.Location = New Point(969, 12)
        btnSave.Margin = New Padding(3, 2, 3, 2)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(90, 32)
        btnSave.TabIndex = 1
        btnSave.Text = "Save"
        ' 
        ' grpPayrollParameters
        ' 
        grpPayrollParameters.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpPayrollParameters.Appearance.Options.UseFont = True
        grpPayrollParameters.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpPayrollParameters.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpPayrollParameters.AppearanceCaption.Options.UseFont = True
        grpPayrollParameters.Controls.Add(txtPercentPrecision)
        grpPayrollParameters.Controls.Add(lblPercentPrecision)
        grpPayrollParameters.Controls.Add(txtAmountPrecision)
        grpPayrollParameters.Controls.Add(lblAmountPrecision)
        grpPayrollParameters.Controls.Add(txtWorkHourPerDay)
        grpPayrollParameters.Controls.Add(lblWorkHourPerDay)
        grpPayrollParameters.Controls.Add(txtTotalDaysPerYear)
        grpPayrollParameters.Controls.Add(lblTotalDaysPerYear)
        grpPayrollParameters.Controls.Add(txtBonusCeiling)
        grpPayrollParameters.Controls.Add(lblBonusCeiling)
        grpPayrollParameters.Location = New Point(4, 66)
        grpPayrollParameters.Margin = New Padding(3, 2, 3, 2)
        grpPayrollParameters.Name = "grpPayrollParameters"
        grpPayrollParameters.Size = New Size(1069, 140)
        grpPayrollParameters.TabIndex = 1
        grpPayrollParameters.Text = " PAYROLL PARAMETERS"
        ' 
        ' txtPercentPrecision
        ' 
        txtPercentPrecision.Location = New Point(280, 102)
        txtPercentPrecision.Margin = New Padding(3, 2, 3, 2)
        txtPercentPrecision.Name = "txtPercentPrecision"
        txtPercentPrecision.Properties.Mask.EditMask = "n0"
        txtPercentPrecision.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtPercentPrecision.Properties.Mask.UseMaskAsDisplayFormat = True
        txtPercentPrecision.Size = New Size(230, 20)
        txtPercentPrecision.TabIndex = 9
        ' 
        ' lblPercentPrecision
        ' 
        lblPercentPrecision.Location = New Point(280, 84)
        lblPercentPrecision.Name = "lblPercentPrecision"
        lblPercentPrecision.Size = New Size(82, 13)
        lblPercentPrecision.TabIndex = 8
        lblPercentPrecision.Text = "Percent Precision"
        ' 
        ' txtAmountPrecision
        ' 
        txtAmountPrecision.Location = New Point(24, 102)
        txtAmountPrecision.Margin = New Padding(3, 2, 3, 2)
        txtAmountPrecision.Name = "txtAmountPrecision"
        txtAmountPrecision.Properties.Mask.EditMask = "n0"
        txtAmountPrecision.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtAmountPrecision.Properties.Mask.UseMaskAsDisplayFormat = True
        txtAmountPrecision.Size = New Size(230, 20)
        txtAmountPrecision.TabIndex = 7
        ' 
        ' lblAmountPrecision
        ' 
        lblAmountPrecision.Location = New Point(24, 84)
        lblAmountPrecision.Name = "lblAmountPrecision"
        lblAmountPrecision.Size = New Size(82, 13)
        lblAmountPrecision.TabIndex = 6
        lblAmountPrecision.Text = "Amount Precision"
        ' 
        ' txtWorkHourPerDay
        ' 
        txtWorkHourPerDay.Location = New Point(536, 52)
        txtWorkHourPerDay.Margin = New Padding(3, 2, 3, 2)
        txtWorkHourPerDay.Name = "txtWorkHourPerDay"
        txtWorkHourPerDay.Properties.Mask.EditMask = "n2"
        txtWorkHourPerDay.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtWorkHourPerDay.Properties.Mask.UseMaskAsDisplayFormat = True
        txtWorkHourPerDay.Size = New Size(230, 20)
        txtWorkHourPerDay.TabIndex = 5
        ' 
        ' lblWorkHourPerDay
        ' 
        lblWorkHourPerDay.Location = New Point(536, 34)
        lblWorkHourPerDay.Name = "lblWorkHourPerDay"
        lblWorkHourPerDay.Size = New Size(92, 13)
        lblWorkHourPerDay.TabIndex = 4
        lblWorkHourPerDay.Text = "Work Hour Per Day"
        ' 
        ' txtTotalDaysPerYear
        ' 
        txtTotalDaysPerYear.Location = New Point(280, 52)
        txtTotalDaysPerYear.Margin = New Padding(3, 2, 3, 2)
        txtTotalDaysPerYear.Name = "txtTotalDaysPerYear"
        txtTotalDaysPerYear.Properties.Mask.EditMask = "n0"
        txtTotalDaysPerYear.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtTotalDaysPerYear.Properties.Mask.UseMaskAsDisplayFormat = True
        txtTotalDaysPerYear.Size = New Size(230, 20)
        txtTotalDaysPerYear.TabIndex = 3
        ' 
        ' lblTotalDaysPerYear
        ' 
        lblTotalDaysPerYear.Location = New Point(280, 34)
        lblTotalDaysPerYear.Name = "lblTotalDaysPerYear"
        lblTotalDaysPerYear.Size = New Size(95, 13)
        lblTotalDaysPerYear.TabIndex = 2
        lblTotalDaysPerYear.Text = "Total Days Per Year"
        ' 
        ' txtBonusCeiling
        ' 
        txtBonusCeiling.Location = New Point(24, 52)
        txtBonusCeiling.Margin = New Padding(3, 2, 3, 2)
        txtBonusCeiling.Name = "txtBonusCeiling"
        txtBonusCeiling.Properties.Mask.EditMask = "n2"
        txtBonusCeiling.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtBonusCeiling.Properties.Mask.UseMaskAsDisplayFormat = True
        txtBonusCeiling.Size = New Size(230, 20)
        txtBonusCeiling.TabIndex = 1
        ' 
        ' lblBonusCeiling
        ' 
        lblBonusCeiling.Location = New Point(24, 34)
        lblBonusCeiling.Name = "lblBonusCeiling"
        lblBonusCeiling.Size = New Size(63, 13)
        lblBonusCeiling.TabIndex = 0
        lblBonusCeiling.Text = "Bonus Ceiling"
        ' 
        ' grpCodeMapping
        ' 
        grpCodeMapping.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpCodeMapping.Appearance.Options.UseFont = True
        grpCodeMapping.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpCodeMapping.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpCodeMapping.AppearanceCaption.Options.UseFont = True
        grpCodeMapping.Controls.Add(lookupEarlyOutCode)
        grpCodeMapping.Controls.Add(lblEarlyOutCode)
        grpCodeMapping.Controls.Add(lookupLateInCode)
        grpCodeMapping.Controls.Add(lblLateInCode)
        grpCodeMapping.Controls.Add(lookupAbsentCode)
        grpCodeMapping.Controls.Add(lblAbsentCode)
        grpCodeMapping.Controls.Add(lookupBasicSalaryCodeMinus)
        grpCodeMapping.Controls.Add(lblBasicSalaryCodeMinus)
        grpCodeMapping.Controls.Add(lookupBasicSalaryCodePlus)
        grpCodeMapping.Controls.Add(lblBasicSalaryCodePlus)
        grpCodeMapping.Location = New Point(4, 212)
        grpCodeMapping.Margin = New Padding(3, 2, 3, 2)
        grpCodeMapping.Name = "grpCodeMapping"
        grpCodeMapping.Size = New Size(1069, 140)
        grpCodeMapping.TabIndex = 2
        grpCodeMapping.Text = " CODE MAPPING"
        ' 
        ' lookupEarlyOutCode
        ' 
        lookupEarlyOutCode.Location = New Point(280, 102)
        lookupEarlyOutCode.Margin = New Padding(3, 2, 3, 2)
        lookupEarlyOutCode.Name = "lookupEarlyOutCode"
        lookupEarlyOutCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookupEarlyOutCode.Size = New Size(230, 20)
        lookupEarlyOutCode.TabIndex = 9
        ' 
        ' lblEarlyOutCode
        ' 
        lblEarlyOutCode.Location = New Point(280, 84)
        lblEarlyOutCode.Name = "lblEarlyOutCode"
        lblEarlyOutCode.Size = New Size(73, 13)
        lblEarlyOutCode.TabIndex = 8
        lblEarlyOutCode.Text = "Early Out Code"
        ' 
        ' lookupLateInCode
        ' 
        lookupLateInCode.Location = New Point(24, 102)
        lookupLateInCode.Margin = New Padding(3, 2, 3, 2)
        lookupLateInCode.Name = "lookupLateInCode"
        lookupLateInCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookupLateInCode.Size = New Size(230, 20)
        lookupLateInCode.TabIndex = 7
        ' 
        ' lblLateInCode
        ' 
        lblLateInCode.Location = New Point(24, 84)
        lblLateInCode.Name = "lblLateInCode"
        lblLateInCode.Size = New Size(62, 13)
        lblLateInCode.TabIndex = 6
        lblLateInCode.Text = "Late In Code"
        ' 
        ' lookupAbsentCode
        ' 
        lookupAbsentCode.Location = New Point(536, 52)
        lookupAbsentCode.Margin = New Padding(3, 2, 3, 2)
        lookupAbsentCode.Name = "lookupAbsentCode"
        lookupAbsentCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookupAbsentCode.Size = New Size(230, 20)
        lookupAbsentCode.TabIndex = 5
        ' 
        ' lblAbsentCode
        ' 
        lblAbsentCode.Location = New Point(536, 34)
        lblAbsentCode.Name = "lblAbsentCode"
        lblAbsentCode.Size = New Size(62, 13)
        lblAbsentCode.TabIndex = 4
        lblAbsentCode.Text = "Absent Code"
        ' 
        ' lookupBasicSalaryCodeMinus
        ' 
        lookupBasicSalaryCodeMinus.Location = New Point(280, 52)
        lookupBasicSalaryCodeMinus.Margin = New Padding(3, 2, 3, 2)
        lookupBasicSalaryCodeMinus.Name = "lookupBasicSalaryCodeMinus"
        lookupBasicSalaryCodeMinus.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookupBasicSalaryCodeMinus.Size = New Size(230, 20)
        lookupBasicSalaryCodeMinus.TabIndex = 3
        ' 
        ' lblBasicSalaryCodeMinus
        ' 
        lblBasicSalaryCodeMinus.Location = New Point(280, 34)
        lblBasicSalaryCodeMinus.Name = "lblBasicSalaryCodeMinus"
        lblBasicSalaryCodeMinus.Size = New Size(100, 13)
        lblBasicSalaryCodeMinus.TabIndex = 2
        lblBasicSalaryCodeMinus.Text = "Basic Salary Code (-)"
        ' 
        ' lookupBasicSalaryCodePlus
        ' 
        lookupBasicSalaryCodePlus.Location = New Point(24, 52)
        lookupBasicSalaryCodePlus.Margin = New Padding(3, 2, 3, 2)
        lookupBasicSalaryCodePlus.Name = "lookupBasicSalaryCodePlus"
        lookupBasicSalaryCodePlus.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookupBasicSalaryCodePlus.Size = New Size(230, 20)
        lookupBasicSalaryCodePlus.TabIndex = 1
        ' 
        ' lblBasicSalaryCodePlus
        ' 
        lblBasicSalaryCodePlus.Location = New Point(24, 34)
        lblBasicSalaryCodePlus.Name = "lblBasicSalaryCodePlus"
        lblBasicSalaryCodePlus.Size = New Size(104, 13)
        lblBasicSalaryCodePlus.TabIndex = 0
        lblBasicSalaryCodePlus.Text = "Basic Salary Code (+)"
        ' 
        ' grpStatutoryBasis
        ' 
        grpStatutoryBasis.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        grpStatutoryBasis.Appearance.Options.UseFont = True
        grpStatutoryBasis.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpStatutoryBasis.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpStatutoryBasis.AppearanceCaption.Options.UseFont = True
        grpStatutoryBasis.Controls.Add(cboPhilHealthBasedOn)
        grpStatutoryBasis.Controls.Add(lblPhilHealthBasedOn)
        grpStatutoryBasis.Controls.Add(cboSSSBasedOn)
        grpStatutoryBasis.Controls.Add(lblSSSBasedOn)
        grpStatutoryBasis.Location = New Point(4, 358)
        grpStatutoryBasis.Margin = New Padding(3, 2, 3, 2)
        grpStatutoryBasis.Name = "grpStatutoryBasis"
        grpStatutoryBasis.Size = New Size(1069, 92)
        grpStatutoryBasis.TabIndex = 3
        grpStatutoryBasis.Text = " STATUTORY BASIS"
        ' 
        ' cboPhilHealthBasedOn
        ' 
        cboPhilHealthBasedOn.Location = New Point(280, 52)
        cboPhilHealthBasedOn.Margin = New Padding(3, 2, 3, 2)
        cboPhilHealthBasedOn.Name = "cboPhilHealthBasedOn"
        cboPhilHealthBasedOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboPhilHealthBasedOn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboPhilHealthBasedOn.Size = New Size(230, 20)
        cboPhilHealthBasedOn.TabIndex = 3
        ' 
        ' lblPhilHealthBasedOn
        ' 
        lblPhilHealthBasedOn.Location = New Point(280, 34)
        lblPhilHealthBasedOn.Name = "lblPhilHealthBasedOn"
        lblPhilHealthBasedOn.Size = New Size(94, 13)
        lblPhilHealthBasedOn.TabIndex = 2
        lblPhilHealthBasedOn.Text = "PhilHealth Based on"
        ' 
        ' cboSSSBasedOn
        ' 
        cboSSSBasedOn.Location = New Point(24, 52)
        cboSSSBasedOn.Margin = New Padding(3, 2, 3, 2)
        cboSSSBasedOn.Name = "cboSSSBasedOn"
        cboSSSBasedOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboSSSBasedOn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboSSSBasedOn.Size = New Size(230, 20)
        cboSSSBasedOn.TabIndex = 1
        ' 
        ' lblSSSBasedOn
        ' 
        lblSSSBasedOn.Location = New Point(24, 34)
        lblSSSBasedOn.Name = "lblSSSBasedOn"
        lblSSSBasedOn.Size = New Size(65, 13)
        lblSSSBasedOn.TabIndex = 0
        lblSSSBasedOn.Text = "SSS Based on"
        ' 
        ' ucGeneralSettings
        ' 
        Controls.Add(grpStatutoryBasis)
        Controls.Add(grpCodeMapping)
        Controls.Add(grpPayrollParameters)
        Controls.Add(PanelControl1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "ucGeneralSettings"
        Padding = New Padding(4)
        Size = New Size(1077, 600)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpPayrollParameters, ComponentModel.ISupportInitialize).EndInit()
        grpPayrollParameters.ResumeLayout(False)
        grpPayrollParameters.PerformLayout()
        CType(txtPercentPrecision.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAmountPrecision.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtWorkHourPerDay.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtTotalDaysPerYear.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtBonusCeiling.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpCodeMapping, ComponentModel.ISupportInitialize).EndInit()
        grpCodeMapping.ResumeLayout(False)
        grpCodeMapping.PerformLayout()
        CType(lookupEarlyOutCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupLateInCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupAbsentCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupBasicSalaryCodeMinus.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupBasicSalaryCodePlus.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpStatutoryBasis, ComponentModel.ISupportInitialize).EndInit()
        grpStatutoryBasis.ResumeLayout(False)
        grpStatutoryBasis.PerformLayout()
        CType(cboPhilHealthBasedOn.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboSSSBasedOn.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton

    Friend WithEvents grpPayrollParameters As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblBonusCeiling As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBonusCeiling As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblTotalDaysPerYear As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTotalDaysPerYear As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblWorkHourPerDay As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtWorkHourPerDay As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAmountPrecision As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAmountPrecision As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPercentPrecision As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPercentPrecision As DevExpress.XtraEditors.TextEdit

    Friend WithEvents grpCodeMapping As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblBasicSalaryCodePlus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupBasicSalaryCodePlus As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblBasicSalaryCodeMinus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupBasicSalaryCodeMinus As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblAbsentCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupAbsentCode As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblLateInCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupLateInCode As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblEarlyOutCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupEarlyOutCode As DevExpress.XtraEditors.LookUpEdit

    Friend WithEvents grpStatutoryBasis As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblSSSBasedOn As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboSSSBasedOn As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblPhilHealthBasedOn As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboPhilHealthBasedOn As DevExpress.XtraEditors.ComboBoxEdit

End Class