<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLoanMarkPaid
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
        lblIntro = New DevExpress.XtraEditors.LabelControl()
        lblUpTo = New DevExpress.XtraEditors.LabelControl()
        spnUpTo = New DevExpress.XtraEditors.SpinEdit()
        lblHint = New DevExpress.XtraEditors.LabelControl()
        lblPreview = New DevExpress.XtraEditors.LabelControl()
        btnOk = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(spnUpTo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblIntro
        ' 
        lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblIntro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblIntro.Location = New Point(16, 14)
        lblIntro.Name = "lblIntro"
        lblIntro.Size = New Size(408, 52)
        lblIntro.TabIndex = 0
        lblIntro.Text = "Mark the installments that were already deducted in C1Pay before this loan was entered here. They are saved as paid with Payroll Ref 'C1PAY' and Paid Date = their Scheduled Date, so payroll will not deduct them again."
        ' 
        ' lblUpTo
        ' 
        lblUpTo.Location = New Point(16, 80)
        lblUpTo.Name = "lblUpTo"
        lblUpTo.Size = New Size(160, 13)
        lblUpTo.TabIndex = 1
        lblUpTo.Text = "Mark paid up to installment #"
        ' 
        ' spnUpTo
        ' 
        spnUpTo.EditValue = New Decimal(New Integer() {1, 0, 0, 0})
        spnUpTo.Location = New Point(190, 77)
        spnUpTo.Name = "spnUpTo"
        spnUpTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnUpTo.Properties.IsFloatValue = False
        spnUpTo.Properties.DisplayFormat.FormatString = "n0"
        spnUpTo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnUpTo.Properties.EditFormat.FormatString = "n0"
        spnUpTo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnUpTo.Properties.MaxValue = New Decimal(New Integer() {600, 0, 0, 0})
        spnUpTo.Properties.MinValue = New Decimal(New Integer() {1, 0, 0, 0})
        spnUpTo.Size = New Size(90, 20)
        spnUpTo.TabIndex = 2
        ' 
        ' lblHint
        ' 
        lblHint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblHint.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblHint.Location = New Point(16, 106)
        lblHint.Name = "lblHint"
        lblHint.Size = New Size(408, 30)
        lblHint.TabIndex = 3
        lblHint.Text = "-"
        ' 
        ' lblPreview
        ' 
        lblPreview.Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPreview.Appearance.Options.UseFont = True
        lblPreview.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblPreview.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblPreview.Location = New Point(16, 146)
        lblPreview.Name = "lblPreview"
        lblPreview.Size = New Size(408, 70)
        lblPreview.TabIndex = 4
        lblPreview.Text = "-"
        ' 
        ' btnOk
        ' 
        btnOk.DialogResult = DialogResult.OK
        btnOk.Location = New Point(214, 232)
        btnOk.Name = "btnOk"
        btnOk.Size = New Size(110, 28)
        btnOk.TabIndex = 5
        btnOk.Text = "Mark as paid"
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(330, 232)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(94, 28)
        btnCancel.TabIndex = 6
        btnCancel.Text = "Cancel"
        ' 
        ' frmLoanMarkPaid
        ' 
        AcceptButton = btnOk
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(440, 276)
        Controls.Add(btnCancel)
        Controls.Add(btnOk)
        Controls.Add(lblPreview)
        Controls.Add(lblHint)
        Controls.Add(spnUpTo)
        Controls.Add(lblUpTo)
        Controls.Add(lblIntro)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmLoanMarkPaid"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Mark Installments Paid (C1Pay)"
        CType(spnUpTo.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblIntro As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblUpTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnUpTo As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPreview As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
