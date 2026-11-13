<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEmployeeAction
    Inherits DevExpress.XtraEditors.XtraForm

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
        lblEmployee = New DevExpress.XtraEditors.LabelControl()
        lblInfo = New DevExpress.XtraEditors.LabelControl()
        lblEffective = New DevExpress.XtraEditors.LabelControl()
        dtpEffective = New DevExpress.XtraEditors.DateEdit()
        lblPosition = New DevExpress.XtraEditors.LabelControl()
        lookPosition = New DevExpress.XtraEditors.LookUpEdit()
        lblCategory = New DevExpress.XtraEditors.LabelControl()
        lookCategory = New DevExpress.XtraEditors.LookUpEdit()
        lblJobClass = New DevExpress.XtraEditors.LabelControl()
        lookJobClass = New DevExpress.XtraEditors.LookUpEdit()
        lblBranch = New DevExpress.XtraEditors.LabelControl()
        lookBranch = New DevExpress.XtraEditors.LookUpEdit()
        lblDepartment = New DevExpress.XtraEditors.LabelControl()
        lookDepartment = New DevExpress.XtraEditors.LookUpEdit()
        lblEmploymentType = New DevExpress.XtraEditors.LabelControl()
        cboEmploymentType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblCurrentRate = New DevExpress.XtraEditors.LabelControl()
        lblCurrentRateValue = New DevExpress.XtraEditors.LabelControl()
        lblNewRate = New DevExpress.XtraEditors.LabelControl()
        spnNewRate = New DevExpress.XtraEditors.SpinEdit()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        memoRemarks = New DevExpress.XtraEditors.MemoEdit()
        lblWarning = New DevExpress.XtraEditors.LabelControl()
        btnApply = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(dtpEffective.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpEffective.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookPosition.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookJobClass.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookBranch.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookDepartment.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboEmploymentType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnNewRate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblEmployee
        ' 
        lblEmployee.Appearance.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblEmployee.Appearance.Options.UseFont = True
        lblEmployee.Location = New Point(20, 16)
        lblEmployee.Name = "lblEmployee"
        lblEmployee.Size = New Size(13, 21)
        lblEmployee.TabIndex = 0
        lblEmployee.Text = "..."
        ' 
        ' lblInfo
        ' 
        lblInfo.Appearance.Options.UseTextOptions = True
        lblInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblInfo.Location = New Point(20, 48)
        lblInfo.Name = "lblInfo"
        lblInfo.Size = New Size(430, 34)
        lblInfo.TabIndex = 1
        lblInfo.Text = ""
        ' 
        ' lblEffective
        ' 
        lblEffective.Location = New Point(20, 100)
        lblEffective.Name = "lblEffective"
        lblEffective.Size = New Size(79, 13)
        lblEffective.TabIndex = 2
        lblEffective.Text = "Effective date *"
        lblEffective.Visible = False
        ' 
        ' dtpEffective
        ' 
        dtpEffective.EditValue = Nothing
        dtpEffective.Location = New Point(150, 96)
        dtpEffective.Name = "dtpEffective"
        dtpEffective.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEffective.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEffective.Size = New Size(300, 20)
        dtpEffective.TabIndex = 3
        dtpEffective.Visible = False
        ' 
        ' lblPosition
        ' 
        lblPosition.Location = New Point(20, 130)
        lblPosition.Name = "lblPosition"
        lblPosition.Size = New Size(54, 13)
        lblPosition.TabIndex = 4
        lblPosition.Text = "Position *"
        lblPosition.Visible = False
        ' 
        ' lookPosition
        ' 
        lookPosition.Location = New Point(150, 126)
        lookPosition.Name = "lookPosition"
        lookPosition.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookPosition.Size = New Size(300, 20)
        lookPosition.TabIndex = 5
        lookPosition.Visible = False
        ' 
        ' lblCategory
        ' 
        lblCategory.Location = New Point(20, 160)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(72, 13)
        lblCategory.TabIndex = 6
        lblCategory.Text = "Category Code"
        lblCategory.Visible = False
        ' 
        ' lookCategory
        ' 
        lookCategory.Location = New Point(150, 156)
        lookCategory.Name = "lookCategory"
        lookCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookCategory.Size = New Size(300, 20)
        lookCategory.TabIndex = 7
        lookCategory.Visible = False
        ' 
        ' lblJobClass
        ' 
        lblJobClass.Location = New Point(20, 190)
        lblJobClass.Name = "lblJobClass"
        lblJobClass.Size = New Size(47, 13)
        lblJobClass.TabIndex = 8
        lblJobClass.Text = "Job Class"
        lblJobClass.Visible = False
        ' 
        ' lookJobClass
        ' 
        lookJobClass.Location = New Point(150, 186)
        lookJobClass.Name = "lookJobClass"
        lookJobClass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookJobClass.Size = New Size(300, 20)
        lookJobClass.TabIndex = 9
        lookJobClass.Visible = False
        ' 
        ' lblBranch
        ' 
        lblBranch.Location = New Point(20, 220)
        lblBranch.Name = "lblBranch"
        lblBranch.Size = New Size(37, 13)
        lblBranch.TabIndex = 10
        lblBranch.Text = "Branch"
        lblBranch.Visible = False
        ' 
        ' lookBranch
        ' 
        lookBranch.Location = New Point(150, 216)
        lookBranch.Name = "lookBranch"
        lookBranch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookBranch.Size = New Size(300, 20)
        lookBranch.TabIndex = 11
        lookBranch.Visible = False
        ' 
        ' lblDepartment
        ' 
        lblDepartment.Location = New Point(20, 250)
        lblDepartment.Name = "lblDepartment"
        lblDepartment.Size = New Size(60, 13)
        lblDepartment.TabIndex = 12
        lblDepartment.Text = "Department"
        lblDepartment.Visible = False
        ' 
        ' lookDepartment
        ' 
        lookDepartment.Location = New Point(150, 246)
        lookDepartment.Name = "lookDepartment"
        lookDepartment.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookDepartment.Size = New Size(300, 20)
        lookDepartment.TabIndex = 13
        lookDepartment.Visible = False
        ' 
        ' lblEmploymentType
        ' 
        lblEmploymentType.Location = New Point(20, 280)
        lblEmploymentType.Name = "lblEmploymentType"
        lblEmploymentType.Size = New Size(86, 13)
        lblEmploymentType.TabIndex = 19
        lblEmploymentType.Text = "Employment Type *"
        lblEmploymentType.Visible = False
        ' 
        ' cboEmploymentType
        ' 
        cboEmploymentType.Location = New Point(150, 276)
        cboEmploymentType.Name = "cboEmploymentType"
        cboEmploymentType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboEmploymentType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboEmploymentType.Size = New Size(300, 20)
        cboEmploymentType.TabIndex = 20
        cboEmploymentType.Visible = False
        ' 
        ' lblCurrentRate
        ' 
        lblCurrentRate.Location = New Point(20, 280)
        lblCurrentRate.Name = "lblCurrentRate"
        lblCurrentRate.Size = New Size(100, 13)
        lblCurrentRate.TabIndex = 21
        lblCurrentRate.Text = "Current Basic Salary"
        lblCurrentRate.Visible = False
        ' 
        ' lblCurrentRateValue
        ' 
        lblCurrentRateValue.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        lblCurrentRateValue.Appearance.Options.UseFont = True
        lblCurrentRateValue.Location = New Point(150, 276)
        lblCurrentRateValue.Name = "lblCurrentRateValue"
        lblCurrentRateValue.Size = New Size(300, 15)
        lblCurrentRateValue.TabIndex = 22
        lblCurrentRateValue.Text = "-"
        lblCurrentRateValue.Visible = False
        ' 
        ' lblNewRate
        ' 
        lblNewRate.Location = New Point(20, 280)
        lblNewRate.Name = "lblNewRate"
        lblNewRate.Size = New Size(80, 13)
        lblNewRate.TabIndex = 23
        lblNewRate.Text = "New Basic Salary *"
        lblNewRate.Visible = False
        ' 
        ' spnNewRate
        ' 
        spnNewRate.EditValue = Nothing
        spnNewRate.Location = New Point(150, 276)
        spnNewRate.Name = "spnNewRate"
        spnNewRate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnNewRate.Properties.DisplayFormat.FormatString = "n2"
        spnNewRate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnNewRate.Properties.EditFormat.FormatString = "n2"
        spnNewRate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnNewRate.Properties.MaxValue = New Decimal(New Integer() {999999999, 0, 0, 131072})
        spnNewRate.Properties.MinValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnNewRate.Size = New Size(300, 20)
        spnNewRate.TabIndex = 24
        spnNewRate.Visible = False
        ' 
        ' lblRemarks
        ' 
        lblRemarks.Location = New Point(20, 280)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(44, 13)
        lblRemarks.TabIndex = 14
        lblRemarks.Text = "Remarks"
        lblRemarks.Visible = False
        ' 
        ' memoRemarks
        ' 
        memoRemarks.Location = New Point(150, 276)
        memoRemarks.Name = "memoRemarks"
        memoRemarks.Size = New Size(300, 70)
        memoRemarks.TabIndex = 15
        memoRemarks.Visible = False
        ' 
        ' lblWarning
        ' 
        lblWarning.Appearance.BackColor = Color.FromArgb(250, 238, 218)
        lblWarning.Appearance.ForeColor = Color.FromArgb(99, 56, 6)
        lblWarning.Appearance.Options.UseBackColor = True
        lblWarning.Appearance.Options.UseForeColor = True
        lblWarning.Appearance.Options.UseTextOptions = True
        lblWarning.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        lblWarning.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblWarning.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblWarning.Location = New Point(20, 360)
        lblWarning.Name = "lblWarning"
        lblWarning.Padding = New Padding(8, 4, 8, 4)
        lblWarning.Size = New Size(430, 40)
        lblWarning.TabIndex = 16
        lblWarning.Visible = False
        ' 
        ' btnApply
        ' 
        btnApply.Enabled = False
        btnApply.Location = New Point(244, 416)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 28)
        btnApply.TabIndex = 17
        btnApply.Text = "Apply"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(350, 416)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 28)
        btnCancel.TabIndex = 18
        btnCancel.Text = "Cancel"
        ' 
        ' frmEmployeeAction
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(470, 460)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Controls.Add(lblWarning)
        Controls.Add(memoRemarks)
        Controls.Add(spnNewRate)
        Controls.Add(lblNewRate)
        Controls.Add(lblCurrentRateValue)
        Controls.Add(lblCurrentRate)
        Controls.Add(cboEmploymentType)
        Controls.Add(lblEmploymentType)
        Controls.Add(lblRemarks)
        Controls.Add(lookDepartment)
        Controls.Add(lblDepartment)
        Controls.Add(lookBranch)
        Controls.Add(lblBranch)
        Controls.Add(lookJobClass)
        Controls.Add(lblJobClass)
        Controls.Add(lookCategory)
        Controls.Add(lblCategory)
        Controls.Add(lookPosition)
        Controls.Add(lblPosition)
        Controls.Add(dtpEffective)
        Controls.Add(lblEffective)
        Controls.Add(lblInfo)
        Controls.Add(lblEmployee)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmEmployeeAction"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Employee Action"
        CType(dtpEffective.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpEffective.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookPosition.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookJobClass.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookBranch.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookDepartment.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboEmploymentType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnNewRate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEffective As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpEffective As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblPosition As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookPosition As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblCategory As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookCategory As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblJobClass As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookJobClass As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblBranch As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookBranch As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblDepartment As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookDepartment As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblEmploymentType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboEmploymentType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblCurrentRate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCurrentRateValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblNewRate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnNewRate As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents memoRemarks As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblWarning As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnApply As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
