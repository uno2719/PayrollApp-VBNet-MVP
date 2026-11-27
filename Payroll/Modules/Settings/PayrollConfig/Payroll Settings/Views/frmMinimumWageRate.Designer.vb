<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMinimumWageRate
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
        lblArea = New DevExpress.XtraEditors.LabelControl()
        cboArea = New DevExpress.XtraEditors.ComboBoxEdit()
        lblEffective = New DevExpress.XtraEditors.LabelControl()
        dtpEffective = New DevExpress.XtraEditors.DateEdit()
        lblDailyRate = New DevExpress.XtraEditors.LabelControl()
        spnDailyRate = New DevExpress.XtraEditors.SpinEdit()
        lblWageOrder = New DevExpress.XtraEditors.LabelControl()
        txtWageOrder = New DevExpress.XtraEditors.TextEdit()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        txtRemarks = New DevExpress.XtraEditors.TextEdit()
        lblHint = New DevExpress.XtraEditors.LabelControl()
        btnOk = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(cboArea.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpEffective.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtpEffective.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnDailyRate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtWageOrder.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtRemarks.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblArea
        ' 
        lblArea.Location = New Point(20, 24)
        lblArea.Name = "lblArea"
        lblArea.Size = New Size(60, 13)
        lblArea.TabIndex = 0
        lblArea.Text = "Wage Area *"
        ' 
        ' cboArea
        ' 
        cboArea.Location = New Point(150, 20)
        cboArea.Name = "cboArea"
        cboArea.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        cboArea.Properties.MaxLength = 60
        cboArea.Size = New Size(300, 20)
        cboArea.TabIndex = 1
        cboArea.ToolTip = "Type a new area or choose an existing one, e.g. NCR, CAR, CALABARZON - Component City"
        ' 
        ' lblEffective
        ' 
        lblEffective.Location = New Point(20, 54)
        lblEffective.Name = "lblEffective"
        lblEffective.Size = New Size(76, 13)
        lblEffective.TabIndex = 2
        lblEffective.Text = "Effective Date *"
        ' 
        ' dtpEffective
        ' 
        dtpEffective.EditValue = Nothing
        dtpEffective.Location = New Point(150, 50)
        dtpEffective.Name = "dtpEffective"
        dtpEffective.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEffective.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtpEffective.Size = New Size(140, 20)
        dtpEffective.TabIndex = 3
        ' 
        ' lblDailyRate
        ' 
        lblDailyRate.Location = New Point(20, 84)
        lblDailyRate.Name = "lblDailyRate"
        lblDailyRate.Size = New Size(110, 13)
        lblDailyRate.TabIndex = 4
        lblDailyRate.Text = "Daily Minimum Wage *"
        ' 
        ' spnDailyRate
        ' 
        spnDailyRate.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnDailyRate.Location = New Point(150, 80)
        spnDailyRate.Name = "spnDailyRate"
        spnDailyRate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnDailyRate.Properties.DisplayFormat.FormatString = "n2"
        spnDailyRate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnDailyRate.Properties.EditFormat.FormatString = "n2"
        spnDailyRate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnDailyRate.Properties.MaxValue = New Decimal(New Integer() {9999999, 0, 0, 131072})
        spnDailyRate.Size = New Size(140, 20)
        spnDailyRate.TabIndex = 5
        ' 
        ' lblWageOrder
        ' 
        lblWageOrder.Location = New Point(20, 114)
        lblWageOrder.Name = "lblWageOrder"
        lblWageOrder.Size = New Size(80, 13)
        lblWageOrder.TabIndex = 6
        lblWageOrder.Text = "Wage Order No."
        ' 
        ' txtWageOrder
        ' 
        txtWageOrder.Location = New Point(150, 110)
        txtWageOrder.Name = "txtWageOrder"
        txtWageOrder.Properties.MaxLength = 50
        txtWageOrder.Size = New Size(140, 20)
        txtWageOrder.TabIndex = 7
        ' 
        ' lblRemarks
        ' 
        lblRemarks.Location = New Point(20, 144)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(41, 13)
        lblRemarks.TabIndex = 8
        lblRemarks.Text = "Remarks"
        ' 
        ' txtRemarks
        ' 
        txtRemarks.Location = New Point(150, 140)
        txtRemarks.Name = "txtRemarks"
        txtRemarks.Properties.MaxLength = 300
        txtRemarks.Size = New Size(300, 20)
        txtRemarks.TabIndex = 9
        ' 
        ' lblHint
        ' 
        lblHint.Appearance.ForeColor = Color.Gray
        lblHint.Appearance.Options.UseForeColor = True
        lblHint.Appearance.Options.UseTextOptions = True
        lblHint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblHint.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblHint.Location = New Point(20, 172)
        lblHint.Name = "lblHint"
        lblHint.Size = New Size(430, 30)
        lblHint.TabIndex = 10
        lblHint.Text = "Add a NEW row for each new wage order (do not overwrite the old rate), so past dates still use the old minimum wage."
        ' 
        ' btnOk
        ' 
        btnOk.Location = New Point(264, 214)
        btnOk.Name = "btnOk"
        btnOk.Size = New Size(90, 28)
        btnOk.TabIndex = 11
        btnOk.Text = "Save"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(360, 214)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(90, 28)
        btnCancel.TabIndex = 12
        btnCancel.Text = "Cancel"
        ' 
        ' frmMinimumWageRate
        ' 
        AcceptButton = btnOk
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(470, 258)
        Controls.Add(btnCancel)
        Controls.Add(btnOk)
        Controls.Add(lblHint)
        Controls.Add(txtRemarks)
        Controls.Add(lblRemarks)
        Controls.Add(txtWageOrder)
        Controls.Add(lblWageOrder)
        Controls.Add(spnDailyRate)
        Controls.Add(lblDailyRate)
        Controls.Add(dtpEffective)
        Controls.Add(lblEffective)
        Controls.Add(cboArea)
        Controls.Add(lblArea)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmMinimumWageRate"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Minimum Wage"
        CType(cboArea.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpEffective.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtpEffective.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnDailyRate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtWageOrder.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblArea As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cboArea As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents lblEffective As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtpEffective As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblDailyRate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnDailyRate As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblWageOrder As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtWageOrder As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtRemarks As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
