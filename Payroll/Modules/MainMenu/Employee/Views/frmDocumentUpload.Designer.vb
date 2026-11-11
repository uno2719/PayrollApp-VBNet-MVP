<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDocumentUpload
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
        lblCategory = New DevExpress.XtraEditors.LabelControl()
        lookCategory = New DevExpress.XtraEditors.LookUpEdit()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        memoRemarks = New DevExpress.XtraEditors.MemoEdit()
        lblHint = New DevExpress.XtraEditors.LabelControl()
        btnChoose = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).BeginInit()
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
        ' lblCategory
        ' 
        lblCategory.Location = New Point(20, 61)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(58, 13)
        lblCategory.TabIndex = 1
        lblCategory.Text = "Category *"
        ' 
        ' lookCategory
        ' 
        lookCategory.Location = New Point(110, 57)
        lookCategory.Name = "lookCategory"
        lookCategory.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        lookCategory.Size = New Size(300, 20)
        lookCategory.TabIndex = 2
        ' 
        ' lblRemarks
        ' 
        lblRemarks.Location = New Point(20, 95)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(44, 13)
        lblRemarks.TabIndex = 3
        lblRemarks.Text = "Remarks"
        ' 
        ' memoRemarks
        ' 
        memoRemarks.Location = New Point(110, 91)
        memoRemarks.Name = "memoRemarks"
        memoRemarks.Size = New Size(300, 64)
        memoRemarks.TabIndex = 4
        ' 
        ' lblHint
        ' 
        lblHint.Appearance.ForeColor = Color.Gray
        lblHint.Appearance.Options.UseForeColor = True
        lblHint.Location = New Point(20, 170)
        lblHint.Name = "lblHint"
        lblHint.Size = New Size(390, 13)
        lblHint.TabIndex = 5
        lblHint.Text = "Next you'll choose the file(s). The same remarks apply to every file chosen."
        ' 
        ' btnChoose
        ' 
        btnChoose.Location = New Point(190, 200)
        btnChoose.Name = "btnChoose"
        btnChoose.Size = New Size(120, 28)
        btnChoose.TabIndex = 6
        btnChoose.Text = "Choose file(s)..."
        ' 
        ' btnCancel
        ' 
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(316, 200)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(94, 28)
        btnCancel.TabIndex = 7
        btnCancel.Text = "Cancel"
        ' 
        ' frmDocumentUpload
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(430, 244)
        Controls.Add(btnCancel)
        Controls.Add(btnChoose)
        Controls.Add(lblHint)
        Controls.Add(memoRemarks)
        Controls.Add(lblRemarks)
        Controls.Add(lookCategory)
        Controls.Add(lblCategory)
        Controls.Add(lblEmployee)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmDocumentUpload"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Add 201 File"
        CType(lookCategory.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCategory As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookCategory As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents memoRemarks As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblHint As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnChoose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
