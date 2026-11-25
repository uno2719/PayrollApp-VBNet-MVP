<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLoanOffset
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
        gridconOldLoans = New DevExpress.XtraGrid.GridControl()
        gridviewOldLoans = New DevExpress.XtraGrid.Views.Grid.GridView()
        colSelected = New DevExpress.XtraGrid.Columns.GridColumn()
        colLoanId = New DevExpress.XtraGrid.Columns.GridColumn()
        colLoanStartDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colTotalAmortization = New DevExpress.XtraGrid.Columns.GridColumn()
        colUnpaidCount = New DevExpress.XtraGrid.Columns.GridColumn()
        colUnpaidAmount = New DevExpress.XtraGrid.Columns.GridColumn()
        colBlockedReason = New DevExpress.XtraGrid.Columns.GridColumn()
        lblOffsetDate = New DevExpress.XtraEditors.LabelControl()
        dtOffsetDate = New DevExpress.XtraEditors.DateEdit()
        lblBalance = New DevExpress.XtraEditors.LabelControl()
        spnBalance = New DevExpress.XtraEditors.SpinEdit()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        lblCredit = New DevExpress.XtraEditors.LabelControl()
        rgCreditMode = New DevExpress.XtraEditors.RadioGroup()
        btnOffset = New DevExpress.XtraEditors.SimpleButton()
        btnKeepBoth = New DevExpress.XtraEditors.SimpleButton()
        btnCancel = New DevExpress.XtraEditors.SimpleButton()
        CType(gridconOldLoans, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewOldLoans, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtOffsetDate.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(dtOffsetDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).BeginInit()
        CType(spnBalance.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(rgCreditMode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' lblIntro
        '
        lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblIntro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblIntro.Location = New Point(16, 14)
        lblIntro.Name = "lblIntro"
        lblIntro.Size = New Size(688, 39)
        lblIntro.TabIndex = 0
        lblIntro.Text = "-"
        '
        ' gridconOldLoans
        '
        gridconOldLoans.Location = New Point(16, 62)
        gridconOldLoans.MainView = gridviewOldLoans
        gridconOldLoans.Name = "gridconOldLoans"
        gridconOldLoans.Size = New Size(688, 150)
        gridconOldLoans.TabIndex = 1
        gridconOldLoans.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewOldLoans})
        '
        ' gridviewOldLoans
        '
        gridviewOldLoans.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colSelected, colLoanId, colLoanStartDate, colTotalAmortization, colUnpaidCount, colUnpaidAmount, colBlockedReason})
        gridviewOldLoans.GridControl = gridconOldLoans
        gridviewOldLoans.Name = "gridviewOldLoans"
        gridviewOldLoans.OptionsCustomization.AllowGroup = False
        gridviewOldLoans.OptionsMenu.EnableColumnMenu = False
        gridviewOldLoans.OptionsView.ShowGroupPanel = False
        gridviewOldLoans.OptionsView.ShowIndicator = False
        '
        ' colSelected
        '
        colSelected.Caption = "Offset"
        colSelected.FieldName = "Selected"
        colSelected.Name = "colSelected"
        colSelected.Visible = True
        colSelected.VisibleIndex = 0
        colSelected.Width = 50
        '
        ' colLoanId
        '
        colLoanId.Caption = "Loan #"
        colLoanId.FieldName = "LoanId"
        colLoanId.Name = "colLoanId"
        colLoanId.OptionsColumn.AllowEdit = False
        colLoanId.Visible = True
        colLoanId.VisibleIndex = 1
        colLoanId.Width = 55
        '
        ' colLoanStartDate
        '
        colLoanStartDate.Caption = "Start Date"
        colLoanStartDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        colLoanStartDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        colLoanStartDate.FieldName = "LoanStartDate"
        colLoanStartDate.Name = "colLoanStartDate"
        colLoanStartDate.OptionsColumn.AllowEdit = False
        colLoanStartDate.Visible = True
        colLoanStartDate.VisibleIndex = 2
        colLoanStartDate.Width = 80
        '
        ' colTotalAmortization
        '
        colTotalAmortization.Caption = "Amortization"
        colTotalAmortization.DisplayFormat.FormatString = "n2"
        colTotalAmortization.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colTotalAmortization.FieldName = "TotalAmortization"
        colTotalAmortization.Name = "colTotalAmortization"
        colTotalAmortization.OptionsColumn.AllowEdit = False
        colTotalAmortization.Visible = True
        colTotalAmortization.VisibleIndex = 3
        colTotalAmortization.Width = 85
        '
        ' colUnpaidCount
        '
        colUnpaidCount.Caption = "Unpaid Inst."
        colUnpaidCount.FieldName = "UnpaidCount"
        colUnpaidCount.Name = "colUnpaidCount"
        colUnpaidCount.OptionsColumn.AllowEdit = False
        colUnpaidCount.Visible = True
        colUnpaidCount.VisibleIndex = 4
        colUnpaidCount.Width = 75
        '
        ' colUnpaidAmount
        '
        colUnpaidAmount.Caption = "Unpaid Amount"
        colUnpaidAmount.DisplayFormat.FormatString = "n2"
        colUnpaidAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colUnpaidAmount.FieldName = "UnpaidAmount"
        colUnpaidAmount.Name = "colUnpaidAmount"
        colUnpaidAmount.OptionsColumn.AllowEdit = False
        colUnpaidAmount.Visible = True
        colUnpaidAmount.VisibleIndex = 5
        colUnpaidAmount.Width = 95
        '
        ' colBlockedReason
        '
        colBlockedReason.Caption = "Note"
        colBlockedReason.FieldName = "BlockedReason"
        colBlockedReason.Name = "colBlockedReason"
        colBlockedReason.OptionsColumn.AllowEdit = False
        colBlockedReason.Visible = True
        colBlockedReason.VisibleIndex = 6
        colBlockedReason.Width = 248
        '
        ' lblOffsetDate
        '
        lblOffsetDate.Location = New Point(16, 224)
        lblOffsetDate.Name = "lblOffsetDate"
        lblOffsetDate.Size = New Size(60, 13)
        lblOffsetDate.TabIndex = 2
        lblOffsetDate.Text = "Offset Date"
        '
        ' dtOffsetDate
        '
        dtOffsetDate.EditValue = Nothing
        dtOffsetDate.Location = New Point(16, 242)
        dtOffsetDate.Name = "dtOffsetDate"
        dtOffsetDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtOffsetDate.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        dtOffsetDate.Properties.DisplayFormat.FormatString = "MM/dd/yyyy"
        dtOffsetDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        dtOffsetDate.Properties.EditFormat.FormatString = "MM/dd/yyyy"
        dtOffsetDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        dtOffsetDate.Properties.MaskSettings.Set("mask", "MM/dd/yyyy")
        dtOffsetDate.Size = New Size(150, 20)
        dtOffsetDate.TabIndex = 3
        dtOffsetDate.ToolTip = "The date the old loan is closed. Default is today (the day of encoding)."
        '
        ' lblBalance
        '
        lblBalance.Location = New Point(186, 224)
        lblBalance.Name = "lblBalance"
        lblBalance.Size = New Size(170, 13)
        lblBalance.TabIndex = 4
        lblBalance.Text = "Balance per statement (optional)"
        '
        ' spnBalance
        '
        spnBalance.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
        spnBalance.Location = New Point(186, 242)
        spnBalance.Name = "spnBalance"
        spnBalance.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        spnBalance.Properties.DisplayFormat.FormatString = "n2"
        spnBalance.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnBalance.Properties.EditFormat.FormatString = "n2"
        spnBalance.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        spnBalance.Properties.MaxValue = New Decimal(New Integer() {99999999, 0, 0, 0})
        spnBalance.Size = New Size(170, 20)
        spnBalance.TabIndex = 5
        spnBalance.ToolTip = "Optional. The old loan's balance on the SSS / Pag-IBIG statement (e.g. 19,380.43). Saved in the old loan's Remark and used to compute any credit for the employee. Leave 0 if not known."
        '
        ' lblSummary
        '
        lblSummary.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblSummary.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblSummary.Location = New Point(376, 224)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(328, 39)
        lblSummary.TabIndex = 6
        lblSummary.Text = "-"
        '
        ' lblCredit
        '
        lblCredit.Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCredit.Appearance.Options.UseFont = True
        lblCredit.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblCredit.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical
        lblCredit.Location = New Point(16, 278)
        lblCredit.Name = "lblCredit"
        lblCredit.Size = New Size(688, 26)
        lblCredit.TabIndex = 7
        lblCredit.Text = "-"
        '
        ' rgCreditMode
        '
        rgCreditMode.Location = New Point(16, 310)
        rgCreditMode.Name = "rgCreditMode"
        rgCreditMode.Properties.Columns = 1
        rgCreditMode.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("Refund", "Refund through payroll (added as income in the next Draft cutoff)"), New DevExpress.XtraEditors.Controls.RadioGroupItem("First", "Deduct from the first installment(s) of the new loan"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Last", "Deduct from the last installment(s) of the new loan")})
        rgCreditMode.Size = New Size(688, 78)
        rgCreditMode.TabIndex = 8
        '
        ' btnOffset
        '
        btnOffset.Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnOffset.Appearance.Options.UseFont = True
        btnOffset.Location = New Point(384, 404)
        btnOffset.Name = "btnOffset"
        btnOffset.Size = New Size(130, 28)
        btnOffset.TabIndex = 9
        btnOffset.Text = "Offset selected"
        '
        ' btnKeepBoth
        '
        btnKeepBoth.Location = New Point(520, 404)
        btnKeepBoth.Name = "btnKeepBoth"
        btnKeepBoth.Size = New Size(90, 28)
        btnKeepBoth.TabIndex = 10
        btnKeepBoth.Text = "Keep both"
        btnKeepBoth.ToolTip = "Save the new loan without touching the old one. Both loans will be deducted."
        '
        ' btnCancel
        '
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.Location = New Point(616, 404)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(88, 28)
        btnCancel.TabIndex = 11
        btnCancel.Text = "Cancel"
        btnCancel.ToolTip = "Go back to the loan form without saving."
        '
        ' frmLoanOffset
        '
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        CancelButton = btnCancel
        ClientSize = New Size(720, 446)
        Controls.Add(btnCancel)
        Controls.Add(btnKeepBoth)
        Controls.Add(btnOffset)
        Controls.Add(rgCreditMode)
        Controls.Add(lblCredit)
        Controls.Add(lblSummary)
        Controls.Add(spnBalance)
        Controls.Add(lblBalance)
        Controls.Add(dtOffsetDate)
        Controls.Add(lblOffsetDate)
        Controls.Add(gridconOldLoans)
        Controls.Add(lblIntro)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmLoanOffset"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Existing Loan Found - Offset?"
        CType(gridconOldLoans, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewOldLoans, ComponentModel.ISupportInitialize).EndInit()
        CType(dtOffsetDate.Properties.CalendarTimeProperties, ComponentModel.ISupportInitialize).EndInit()
        CType(dtOffsetDate.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(spnBalance.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(rgCreditMode.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents lblIntro As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridconOldLoans As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewOldLoans As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colSelected As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLoanId As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colLoanStartDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colTotalAmortization As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUnpaidCount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colUnpaidAmount As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colBlockedReason As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents lblOffsetDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents dtOffsetDate As DevExpress.XtraEditors.DateEdit
    Friend WithEvents lblBalance As DevExpress.XtraEditors.LabelControl
    Friend WithEvents spnBalance As DevExpress.XtraEditors.SpinEdit
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCredit As DevExpress.XtraEditors.LabelControl
    Friend WithEvents rgCreditMode As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents btnOffset As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnKeepBoth As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton

End Class
