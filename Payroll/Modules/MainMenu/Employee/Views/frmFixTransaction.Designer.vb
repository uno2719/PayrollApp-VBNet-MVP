<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFixTransaction
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
        lblType = New DevExpress.XtraEditors.LabelControl()
        cboType = New DevExpress.XtraEditors.ComboBoxEdit()
        lblCode = New DevExpress.XtraEditors.LabelControl()
        lookCode = New DevExpress.XtraEditors.LookUpEdit()
        lblAmount = New DevExpress.XtraEditors.LabelControl()
        spnAmount = New DevExpress.XtraEditors.SpinEdit()
        lblUnit = New DevExpress.XtraEditors.LabelControl()
        lblStart = New DevExpress.XtraEditors.LabelControl()
        cboStartMonth = New DevExpress.XtraEditors.ComboBoxEdit()
        spnStartYear = New DevExpress.XtraEditors.SpinEdit()
        chkHasEnd = New DevExpress.XtraEditors.CheckEdit()
        cboEndMonth = New DevExpress.XtraEditors.ComboBoxEdit()
        spnEndYear = New DevExpress.XtraEditors.SpinEdit()
        lblOngoing = New DevExpress.XtraEditors.LabelControl()
        lblFrequency = New DevExpress.XtraEditors.LabelControl()
        cboFrequency = New DevExpress.XtraEditors.ComboBoxEdit()
        lblPayOn = New DevExpress.XtraEditors.LabelControl()
        cboPayOn = New DevExpress.XtraEditors.ComboBoxEdit()
        lblSchedule = New DevExpress.XtraEditors.LabelControl()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        memoRemarks = New DevExpress.XtraEditors.MemoEdit()
        btnOk = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(cboType.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnAmount.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboStartMonth.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnStartYear.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(chkHasEnd.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboEndMonth.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnEndYear.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboFrequency.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(cboPayOn.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' lblEmployee
        '
        lblEmployee.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblEmployee.Appearance.Options.UseFont = True
        lblEmployee.Location = New Point(20, 16)
        lblEmployee.Name = "lblEmployee"
        lblEmployee.Size = New Size(13, 20)
        lblEmployee.TabIndex = 0
        lblEmployee.Text = "..."
        '
        ' lblType
        '
        lblType.Location = New Point(20, 60)
        lblType.Name = "lblType"
        lblType.Size = New Size(92, 13)
        lblType.TabIndex = 1
        lblType.Text = "Transaction Type *"
        '
        ' cboType
        '
        cboType.Location = New Point(150, 56)
        cboType.Name = "cboType"
        cboType.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboType.Size = New Size(180, 20)
        cboType.TabIndex = 2
        '
        ' lblCode
        '
        lblCode.Location = New Point(20, 90)
        lblCode.Name = "lblCode"
        lblCode.Size = New Size(37, 13)
        lblCode.TabIndex = 3
        lblCode.Text = "Code *"
        '
        ' lookCode
        '
        lookCode.Location = New Point(150, 86)
        lookCode.Name = "lookCode"
        lookCode.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookCode.Size = New Size(300, 20)
        lookCode.TabIndex = 4
        '
        ' lblAmount
        '
        lblAmount.Location = New Point(20, 120)
        lblAmount.Name = "lblAmount"
        lblAmount.Size = New Size(48, 13)
        lblAmount.TabIndex = 5
        lblAmount.Text = "Amount *"
        '
        ' spnAmount
        '
        spnAmount.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnAmount.Location = New Point(150, 116)
        spnAmount.Name = "spnAmount"
        spnAmount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnAmount.Properties.DisplayFormat.FormatString = "n2"
        spnAmount.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnAmount.Properties.EditFormat.FormatString = "n2"
        spnAmount.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnAmount.Properties.Increment = New Decimal(New Integer() {50, 0, 0, 0})
        spnAmount.Properties.MaxValue = New Decimal(New Integer() {999999999, 0, 0, 131072})
        spnAmount.Size = New Size(140, 20)
        spnAmount.TabIndex = 6
        '
        ' lblUnit
        '
        lblUnit.Appearance.ForeColor = Color.Gray
        lblUnit.Appearance.Options.UseForeColor = True
        lblUnit.Location = New Point(298, 120)
        lblUnit.Name = "lblUnit"
        lblUnit.Size = New Size(80, 13)
        lblUnit.TabIndex = 7
        lblUnit.Text = "per release"
        '
        ' lblStart
        '
        lblStart.Location = New Point(20, 150)
        lblStart.Name = "lblStart"
        lblStart.Size = New Size(34, 13)
        lblStart.TabIndex = 8
        lblStart.Text = "Start *"
        '
        ' cboStartMonth
        '
        cboStartMonth.Location = New Point(150, 146)
        cboStartMonth.Name = "cboStartMonth"
        cboStartMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboStartMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboStartMonth.Size = New Size(80, 20)
        cboStartMonth.TabIndex = 9
        '
        ' spnStartYear
        '
        spnStartYear.EditValue = New Decimal(New Integer() {2026, 0, 0, 0})
        spnStartYear.Location = New Point(236, 146)
        spnStartYear.Name = "spnStartYear"
        spnStartYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnStartYear.Properties.IsFloatValue = False
        spnStartYear.Properties.DisplayFormat.FormatString = "0"
        spnStartYear.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnStartYear.Properties.EditFormat.FormatString = "0"
        spnStartYear.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnStartYear.Properties.MaxValue = New Decimal(New Integer() {2100, 0, 0, 0})
        spnStartYear.Properties.MinValue = New Decimal(New Integer() {2000, 0, 0, 0})
        spnStartYear.Size = New Size(70, 20)
        spnStartYear.TabIndex = 10
        '
        ' chkHasEnd
        '
        chkHasEnd.Location = New Point(18, 177)
        chkHasEnd.Name = "chkHasEnd"
        chkHasEnd.Properties.Caption = "Has End"
        chkHasEnd.Size = New Size(120, 20)
        chkHasEnd.TabIndex = 11
        chkHasEnd.ToolTip = "Leave unticked if it has no end (ongoing)"
        '
        ' cboEndMonth
        '
        cboEndMonth.Enabled = False
        cboEndMonth.Location = New Point(150, 176)
        cboEndMonth.Name = "cboEndMonth"
        cboEndMonth.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboEndMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboEndMonth.Size = New Size(80, 20)
        cboEndMonth.TabIndex = 12
        '
        ' spnEndYear
        '
        spnEndYear.Enabled = False
        spnEndYear.EditValue = New Decimal(New Integer() {2026, 0, 0, 0})
        spnEndYear.Location = New Point(236, 176)
        spnEndYear.Name = "spnEndYear"
        spnEndYear.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnEndYear.Properties.IsFloatValue = False
        spnEndYear.Properties.DisplayFormat.FormatString = "0"
        spnEndYear.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnEndYear.Properties.EditFormat.FormatString = "0"
        spnEndYear.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnEndYear.Properties.MaxValue = New Decimal(New Integer() {2100, 0, 0, 0})
        spnEndYear.Properties.MinValue = New Decimal(New Integer() {2000, 0, 0, 0})
        spnEndYear.Size = New Size(70, 20)
        spnEndYear.TabIndex = 13
        '
        ' lblOngoing
        '
        lblOngoing.Appearance.ForeColor = Color.Gray
        lblOngoing.Appearance.Options.UseForeColor = True
        lblOngoing.Location = New Point(314, 180)
        lblOngoing.Name = "lblOngoing"
        lblOngoing.Size = New Size(120, 13)
        lblOngoing.TabIndex = 14
        lblOngoing.Text = "Ongoing (no end)"
        '
        ' lblFrequency
        '
        lblFrequency.Location = New Point(20, 210)
        lblFrequency.Name = "lblFrequency"
        lblFrequency.Size = New Size(59, 13)
        lblFrequency.TabIndex = 15
        lblFrequency.Text = "Frequency *"
        '
        ' cboFrequency
        '
        cboFrequency.Location = New Point(150, 206)
        cboFrequency.Name = "cboFrequency"
        cboFrequency.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboFrequency.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboFrequency.Size = New Size(156, 20)
        cboFrequency.TabIndex = 16
        '
        ' lblPayOn
        '
        lblPayOn.Location = New Point(20, 240)
        lblPayOn.Name = "lblPayOn"
        lblPayOn.Size = New Size(36, 13)
        lblPayOn.TabIndex = 17
        lblPayOn.Text = "Pay On"
        '
        ' cboPayOn
        '
        cboPayOn.Enabled = False
        cboPayOn.Location = New Point(150, 236)
        cboPayOn.Name = "cboPayOn"
        cboPayOn.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboPayOn.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        cboPayOn.Size = New Size(156, 20)
        cboPayOn.TabIndex = 18
        cboPayOn.ToolTip = "Which cutoff of the month gets it (Last Cycle = 2nd cutoff for Semi-Monthly)"
        '
        ' lblSchedule
        '
        lblSchedule.Appearance.ForeColor = Color.Gray
        lblSchedule.Appearance.Options.UseForeColor = True
        lblSchedule.Appearance.Options.UseTextOptions = True
        lblSchedule.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblSchedule.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblSchedule.Location = New Point(150, 264)
        lblSchedule.Name = "lblSchedule"
        lblSchedule.Size = New Size(300, 32)
        lblSchedule.TabIndex = 19
        lblSchedule.Text = ""
        '
        ' lblRemarks
        '
        lblRemarks.Location = New Point(20, 306)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(41, 13)
        lblRemarks.TabIndex = 20
        lblRemarks.Text = "Remarks"
        '
        ' memoRemarks
        '
        memoRemarks.Location = New Point(150, 302)
        memoRemarks.Name = "memoRemarks"
        memoRemarks.Properties.MaxLength = 500
        memoRemarks.Size = New Size(300, 56)
        memoRemarks.TabIndex = 21
        '
        ' btnOk
        '
        btnOk.Location = New Point(264, 376)
        btnOk.Name = "btnOk"
        btnOk.Size = New Size(90, 28)
        btnOk.TabIndex = 22
        btnOk.Text = "Save"
        '
        ' btnCancel
        '
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(360, 376)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(90, 28)
        btnCancel.TabIndex = 23
        btnCancel.Text = "Cancel"
        '
        ' frmFixTransaction
        '
        AcceptButton = btnOk
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(470, 420)
        Controls.Add(btnCancel)
        Controls.Add(btnOk)
        Controls.Add(memoRemarks)
        Controls.Add(lblRemarks)
        Controls.Add(lblSchedule)
        Controls.Add(cboPayOn)
        Controls.Add(lblPayOn)
        Controls.Add(cboFrequency)
        Controls.Add(lblFrequency)
        Controls.Add(lblOngoing)
        Controls.Add(spnEndYear)
        Controls.Add(cboEndMonth)
        Controls.Add(chkHasEnd)
        Controls.Add(spnStartYear)
        Controls.Add(cboStartMonth)
        Controls.Add(lblStart)
        Controls.Add(lblUnit)
        Controls.Add(spnAmount)
        Controls.Add(lblAmount)
        Controls.Add(lookCode)
        Controls.Add(lblCode)
        Controls.Add(cboType)
        Controls.Add(lblType)
        Controls.Add(lblEmployee)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmFixTransaction"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Fix Transaction"
        CType(cboType.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnAmount.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboStartMonth.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnStartYear.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(chkHasEnd.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboEndMonth.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnEndYear.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboFrequency.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(cboPayOn.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblType As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboType As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookCode As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnAmount As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblUnit As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblStart As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboStartMonth As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents spnStartYear As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents chkHasEnd As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cboEndMonth As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents spnEndYear As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblOngoing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblFrequency As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboFrequency As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblPayOn As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboPayOn As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblSchedule As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents memoRemarks As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
