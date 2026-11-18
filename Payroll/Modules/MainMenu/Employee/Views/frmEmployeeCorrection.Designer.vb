<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEmployeeCorrection
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
        lblEmploymentType = New DevExpress.XtraEditors.LabelControl()
        cboEmploymentType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblJoined = New DevExpress.XtraEditors.LabelControl()
        dtpJoined = New DevExpress.XtraEditors.DateEdit()
        lblRegularization = New DevExpress.XtraEditors.LabelControl()
        dtpRegularization = New DevExpress.XtraEditors.DateEdit()
        lblLastPromoted = New DevExpress.XtraEditors.LabelControl()
        dtpLastPromoted = New DevExpress.XtraEditors.DateEdit()
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
        lblResigned = New DevExpress.XtraEditors.LabelControl()
        dtpResigned = New DevExpress.XtraEditors.DateEdit()
        lblRetired = New DevExpress.XtraEditors.LabelControl()
        dtpRetired = New DevExpress.XtraEditors.DateEdit()
        lblTerminated = New DevExpress.XtraEditors.LabelControl()
        dtpTerminated = New DevExpress.XtraEditors.DateEdit()
        lblEndOfContract = New DevExpress.XtraEditors.LabelControl()
        dtpEndOfContract = New DevExpress.XtraEditors.DateEdit()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        memoRemarks = New DevExpress.XtraEditors.MemoEdit()
        btnApply = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(cboEmploymentType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpJoined.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpJoined.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpRegularization.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpRegularization.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpLastPromoted.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpLastPromoted.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookPosition.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookJobClass.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookBranch.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookDepartment.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpResigned.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpResigned.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpRetired.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpRetired.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpTerminated.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpTerminated.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpEndOfContract.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpEndOfContract.Properties, ComponentModel.ISupportInitialize).BeginInit()
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
        lblInfo.Size = New Size(440, 48)
        lblInfo.TabIndex = 1
        lblInfo.Text = ""
        ' 
        ' lblEmploymentType
        ' 
        lblEmploymentType.Location = New Point(20, 112)
        lblEmploymentType.Name = "lblEmploymentType"
        lblEmploymentType.Size = New Size(80, 13)
        lblEmploymentType.TabIndex = 2
        lblEmploymentType.Text = "Employment Type *"
        ' 
        ' cboEmploymentType
        ' 
        cboEmploymentType.Location = New Point(170, 108)
        cboEmploymentType.Name = "cboEmploymentType"
        cboEmploymentType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboEmploymentType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboEmploymentType.Size = New Size(290, 20)
        cboEmploymentType.TabIndex = 3
        ' 
        ' lblJoined
        ' 
        lblJoined.Location = New Point(20, 142)
        lblJoined.Name = "lblJoined"
        lblJoined.Size = New Size(80, 13)
        lblJoined.TabIndex = 4
        lblJoined.Text = "Date Joined *"
        ' 
        ' dtpJoined
        ' 
        dtpJoined.EditValue = Nothing
        dtpJoined.Location = New Point(170, 138)
        dtpJoined.Name = "dtpJoined"
        dtpJoined.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpJoined.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpJoined.Size = New Size(290, 20)
        dtpJoined.TabIndex = 5
        ' 
        ' lblRegularization
        ' 
        lblRegularization.Location = New Point(20, 172)
        lblRegularization.Name = "lblRegularization"
        lblRegularization.Size = New Size(80, 13)
        lblRegularization.TabIndex = 6
        lblRegularization.Text = "Date Regularization"
        ' 
        ' dtpRegularization
        ' 
        dtpRegularization.EditValue = Nothing
        dtpRegularization.Location = New Point(170, 168)
        dtpRegularization.Name = "dtpRegularization"
        dtpRegularization.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpRegularization.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpRegularization.Size = New Size(290, 20)
        dtpRegularization.TabIndex = 7
        ' 
        ' lblLastPromoted
        ' 
        lblLastPromoted.Location = New Point(20, 202)
        lblLastPromoted.Name = "lblLastPromoted"
        lblLastPromoted.Size = New Size(80, 13)
        lblLastPromoted.TabIndex = 8
        lblLastPromoted.Text = "Date Last Promoted"
        ' 
        ' dtpLastPromoted
        ' 
        dtpLastPromoted.EditValue = Nothing
        dtpLastPromoted.Location = New Point(170, 198)
        dtpLastPromoted.Name = "dtpLastPromoted"
        dtpLastPromoted.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpLastPromoted.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpLastPromoted.Size = New Size(290, 20)
        dtpLastPromoted.TabIndex = 9
        ' 
        ' lblPosition
        ' 
        lblPosition.Location = New Point(20, 232)
        lblPosition.Name = "lblPosition"
        lblPosition.Size = New Size(80, 13)
        lblPosition.TabIndex = 10
        lblPosition.Text = "Position *"
        ' 
        ' lookPosition
        ' 
        lookPosition.Location = New Point(170, 228)
        lookPosition.Name = "lookPosition"
        lookPosition.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookPosition.Size = New Size(290, 20)
        lookPosition.TabIndex = 11
        ' 
        ' lblCategory
        ' 
        lblCategory.Location = New Point(20, 262)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(80, 13)
        lblCategory.TabIndex = 12
        lblCategory.Text = "Category Code *"
        ' 
        ' lookCategory
        ' 
        lookCategory.Location = New Point(170, 258)
        lookCategory.Name = "lookCategory"
        lookCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookCategory.Size = New Size(290, 20)
        lookCategory.TabIndex = 13
        ' 
        ' lblJobClass
        ' 
        lblJobClass.Location = New Point(20, 292)
        lblJobClass.Name = "lblJobClass"
        lblJobClass.Size = New Size(80, 13)
        lblJobClass.TabIndex = 14
        lblJobClass.Text = "Job Class *"
        ' 
        ' lookJobClass
        ' 
        lookJobClass.Location = New Point(170, 288)
        lookJobClass.Name = "lookJobClass"
        lookJobClass.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookJobClass.Size = New Size(290, 20)
        lookJobClass.TabIndex = 15
        ' 
        ' lblBranch
        ' 
        lblBranch.Location = New Point(20, 322)
        lblBranch.Name = "lblBranch"
        lblBranch.Size = New Size(80, 13)
        lblBranch.TabIndex = 16
        lblBranch.Text = "Branch *"
        ' 
        ' lookBranch
        ' 
        lookBranch.Location = New Point(170, 318)
        lookBranch.Name = "lookBranch"
        lookBranch.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookBranch.Size = New Size(290, 20)
        lookBranch.TabIndex = 17
        ' 
        ' lblDepartment
        ' 
        lblDepartment.Location = New Point(20, 352)
        lblDepartment.Name = "lblDepartment"
        lblDepartment.Size = New Size(80, 13)
        lblDepartment.TabIndex = 18
        lblDepartment.Text = "Department *"
        ' 
        ' lookDepartment
        ' 
        lookDepartment.Location = New Point(170, 348)
        lookDepartment.Name = "lookDepartment"
        lookDepartment.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookDepartment.Size = New Size(290, 20)
        lookDepartment.TabIndex = 19
        ' 
        ' lblResigned
        ' 
        lblResigned.Location = New Point(20, 382)
        lblResigned.Name = "lblResigned"
        lblResigned.Size = New Size(80, 13)
        lblResigned.TabIndex = 20
        lblResigned.Text = "Date Resigned"
        ' 
        ' dtpResigned
        ' 
        dtpResigned.EditValue = Nothing
        dtpResigned.Location = New Point(170, 378)
        dtpResigned.Name = "dtpResigned"
        dtpResigned.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpResigned.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpResigned.Size = New Size(290, 20)
        dtpResigned.TabIndex = 21
        ' 
        ' lblRetired
        ' 
        lblRetired.Location = New Point(20, 412)
        lblRetired.Name = "lblRetired"
        lblRetired.Size = New Size(80, 13)
        lblRetired.TabIndex = 22
        lblRetired.Text = "Date Retired"
        ' 
        ' dtpRetired
        ' 
        dtpRetired.EditValue = Nothing
        dtpRetired.Location = New Point(170, 408)
        dtpRetired.Name = "dtpRetired"
        dtpRetired.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpRetired.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpRetired.Size = New Size(290, 20)
        dtpRetired.TabIndex = 23
        ' 
        ' lblTerminated
        ' 
        lblTerminated.Location = New Point(20, 442)
        lblTerminated.Name = "lblTerminated"
        lblTerminated.Size = New Size(80, 13)
        lblTerminated.TabIndex = 24
        lblTerminated.Text = "Date Terminated"
        ' 
        ' dtpTerminated
        ' 
        dtpTerminated.EditValue = Nothing
        dtpTerminated.Location = New Point(170, 438)
        dtpTerminated.Name = "dtpTerminated"
        dtpTerminated.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpTerminated.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpTerminated.Size = New Size(290, 20)
        dtpTerminated.TabIndex = 25
        ' 
        ' lblEndOfContract
        ' 
        lblEndOfContract.Location = New Point(20, 472)
        lblEndOfContract.Name = "lblEndOfContract"
        lblEndOfContract.Size = New Size(80, 13)
        lblEndOfContract.TabIndex = 26
        lblEndOfContract.Text = "Date End of Contract"
        ' 
        ' dtpEndOfContract
        ' 
        dtpEndOfContract.EditValue = Nothing
        dtpEndOfContract.Location = New Point(170, 468)
        dtpEndOfContract.Name = "dtpEndOfContract"
        dtpEndOfContract.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEndOfContract.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEndOfContract.Size = New Size(290, 20)
        dtpEndOfContract.TabIndex = 27
        ' 
        ' lblRemarks
        ' 
        lblRemarks.Location = New Point(20, 506)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(55, 13)
        lblRemarks.TabIndex = 28
        lblRemarks.Text = "Remarks *"
        ' 
        ' memoRemarks
        ' 
        memoRemarks.Location = New Point(170, 502)
        memoRemarks.Name = "memoRemarks"
        memoRemarks.Size = New Size(290, 70)
        memoRemarks.TabIndex = 29
        ' 
        ' btnApply
        ' 
        btnApply.Enabled = False
        btnApply.Location = New Point(254, 588)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(100, 28)
        btnApply.TabIndex = 30
        btnApply.Text = "Apply correction"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(360, 588)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(100, 28)
        btnCancel.TabIndex = 31
        btnCancel.Text = "Cancel"
        ' 
        ' frmEmployeeCorrection
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(480, 632)
        Controls.Add(btnCancel)
        Controls.Add(btnApply)
        Controls.Add(memoRemarks)
        Controls.Add(lblRemarks)
        Controls.Add(dtpEndOfContract)
        Controls.Add(lblEndOfContract)
        Controls.Add(dtpTerminated)
        Controls.Add(lblTerminated)
        Controls.Add(dtpRetired)
        Controls.Add(lblRetired)
        Controls.Add(dtpResigned)
        Controls.Add(lblResigned)
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
        Controls.Add(dtpLastPromoted)
        Controls.Add(lblLastPromoted)
        Controls.Add(dtpRegularization)
        Controls.Add(lblRegularization)
        Controls.Add(dtpJoined)
        Controls.Add(lblJoined)
        Controls.Add(cboEmploymentType)
        Controls.Add(lblEmploymentType)
        Controls.Add(lblInfo)
        Controls.Add(lblEmployee)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmEmployeeCorrection"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Correct record"
        CType(cboEmploymentType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpJoined.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpJoined.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpRegularization.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpRegularization.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpLastPromoted.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpLastPromoted.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookPosition.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookJobClass.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookBranch.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookDepartment.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpResigned.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpResigned.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpRetired.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpRetired.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpTerminated.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpTerminated.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpEndOfContract.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpEndOfContract.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblInfo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblEmploymentType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboEmploymentType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblJoined As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpJoined As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblRegularization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpRegularization As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblLastPromoted As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpLastPromoted As DevExpress.XtraEditors.DateEdit
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
    Friend WithEvents lblResigned As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpResigned As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblRetired As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpRetired As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblTerminated As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpTerminated As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblEndOfContract As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpEndOfContract As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents memoRemarks As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents btnApply As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
