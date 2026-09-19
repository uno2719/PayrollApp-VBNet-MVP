<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLoanDetails
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
        pnlHeader = New DevExpress.XtraEditors.PanelControl()
        lblEmployee = New DevExpress.XtraEditors.LabelControl()
        lblLoanTitle = New DevExpress.XtraEditors.LabelControl()
        lblStatusBadge = New DevExpress.XtraEditors.LabelControl()
        grpSummary = New DevExpress.XtraEditors.GroupControl()
        lblCapTotalLoan = New DevExpress.XtraEditors.LabelControl()
        lblValTotalLoan = New DevExpress.XtraEditors.LabelControl()
        lblCapTotalPaid = New DevExpress.XtraEditors.LabelControl()
        lblValTotalPaid = New DevExpress.XtraEditors.LabelControl()
        lblCapBalance = New DevExpress.XtraEditors.LabelControl()
        lblValBalance = New DevExpress.XtraEditors.LabelControl()
        lblCapAmortization = New DevExpress.XtraEditors.LabelControl()
        lblValAmortization = New DevExpress.XtraEditors.LabelControl()
        lblCapStartDate = New DevExpress.XtraEditors.LabelControl()
        lblValStartDate = New DevExpress.XtraEditors.LabelControl()
        lblCapFrequency = New DevExpress.XtraEditors.LabelControl()
        lblValFrequency = New DevExpress.XtraEditors.LabelControl()
        lblCapProgress = New DevExpress.XtraEditors.LabelControl()
        lblValProgress = New DevExpress.XtraEditors.LabelControl()
        lblCapRemaining = New DevExpress.XtraEditors.LabelControl()
        lblValRemaining = New DevExpress.XtraEditors.LabelControl()
        lblCapNextDate = New DevExpress.XtraEditors.LabelControl()
        lblValNextDate = New DevExpress.XtraEditors.LabelControl()
        lblCapNextAmount = New DevExpress.XtraEditors.LabelControl()
        lblValNextAmount = New DevExpress.XtraEditors.LabelControl()
        lblCapMethod = New DevExpress.XtraEditors.LabelControl()
        lblValMethod = New DevExpress.XtraEditors.LabelControl()
        lblCapRemark = New DevExpress.XtraEditors.LabelControl()
        lblValRemark = New DevExpress.XtraEditors.LabelControl()
        progressPaid = New DevExpress.XtraEditors.ProgressBarControl()
        grpSchedule = New DevExpress.XtraEditors.GroupControl()
        gridconSchedule = New DevExpress.XtraGrid.GridControl()
        gridviewSchedule = New DevExpress.XtraGrid.Views.Grid.GridView()
        colInstallmentNo = New DevExpress.XtraGrid.Columns.GridColumn()
        colScheduledDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colPrincipalDue = New DevExpress.XtraGrid.Columns.GridColumn()
        colInterestDue = New DevExpress.XtraGrid.Columns.GridColumn()
        colAmountDue = New DevExpress.XtraGrid.Columns.GridColumn()
        colAmountPaid = New DevExpress.XtraGrid.Columns.GridColumn()
        colPaidDate = New DevExpress.XtraGrid.Columns.GridColumn()
        colSchedStatus = New DevExpress.XtraGrid.Columns.GridColumn()
        colPayrollRef = New DevExpress.XtraGrid.Columns.GridColumn()
        pnlFooter = New DevExpress.XtraEditors.PanelControl()
        btnClose = New DevExpress.XtraEditors.SimpleButton()
        CType(pnlHeader, ComponentModel.ISupportInitialize).BeginInit()
        pnlHeader.SuspendLayout()
        CType(grpSummary, ComponentModel.ISupportInitialize).BeginInit()
        grpSummary.SuspendLayout()
        CType(progressPaid.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpSchedule, ComponentModel.ISupportInitialize).BeginInit()
        grpSchedule.SuspendLayout()
        CType(gridconSchedule, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewSchedule, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlFooter, ComponentModel.ISupportInitialize).BeginInit()
        pnlFooter.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlHeader
        ' 
        pnlHeader.Controls.Add(lblEmployee)
        pnlHeader.Controls.Add(lblLoanTitle)
        pnlHeader.Controls.Add(lblStatusBadge)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New Point(0, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Size = New Size(960, 70)
        pnlHeader.TabIndex = 0
        ' 
        ' lblEmployee
        ' 
        lblEmployee.Appearance.Font = New Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblEmployee.Appearance.Options.UseFont = True
        lblEmployee.Location = New Point(18, 12)
        lblEmployee.Name = "lblEmployee"
        lblEmployee.Size = New Size(78, 23)
        lblEmployee.TabIndex = 0
        lblEmployee.Text = "Employee"
        ' 
        ' lblLoanTitle
        ' 
        lblLoanTitle.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblLoanTitle.Appearance.ForeColor = Color.DimGray
        lblLoanTitle.Appearance.Options.UseFont = True
        lblLoanTitle.Appearance.Options.UseForeColor = True
        lblLoanTitle.Location = New Point(20, 40)
        lblLoanTitle.Name = "lblLoanTitle"
        lblLoanTitle.Size = New Size(28, 17)
        lblLoanTitle.TabIndex = 1
        lblLoanTitle.Text = "Loan"
        ' 
        ' lblStatusBadge
        ' 
        lblStatusBadge.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblStatusBadge.Appearance.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatusBadge.Appearance.Options.UseFont = True
        lblStatusBadge.Appearance.Options.UseTextOptions = True
        lblStatusBadge.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        lblStatusBadge.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblStatusBadge.Location = New Point(740, 22)
        lblStatusBadge.Name = "lblStatusBadge"
        lblStatusBadge.Size = New Size(200, 26)
        lblStatusBadge.TabIndex = 2
        lblStatusBadge.Text = "ACTIVE"
        ' 
        ' grpSummary
        ' 
        grpSummary.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpSummary.AppearanceCaption.Options.UseFont = True
        grpSummary.Controls.Add(lblCapTotalLoan)
        grpSummary.Controls.Add(lblValTotalLoan)
        grpSummary.Controls.Add(lblCapTotalPaid)
        grpSummary.Controls.Add(lblValTotalPaid)
        grpSummary.Controls.Add(lblCapBalance)
        grpSummary.Controls.Add(lblValBalance)
        grpSummary.Controls.Add(lblCapAmortization)
        grpSummary.Controls.Add(lblValAmortization)
        grpSummary.Controls.Add(lblCapStartDate)
        grpSummary.Controls.Add(lblValStartDate)
        grpSummary.Controls.Add(lblCapFrequency)
        grpSummary.Controls.Add(lblValFrequency)
        grpSummary.Controls.Add(lblCapProgress)
        grpSummary.Controls.Add(lblValProgress)
        grpSummary.Controls.Add(lblCapRemaining)
        grpSummary.Controls.Add(lblValRemaining)
        grpSummary.Controls.Add(lblCapNextDate)
        grpSummary.Controls.Add(lblValNextDate)
        grpSummary.Controls.Add(lblCapNextAmount)
        grpSummary.Controls.Add(lblValNextAmount)
        grpSummary.Controls.Add(lblCapMethod)
        grpSummary.Controls.Add(lblValMethod)
        grpSummary.Controls.Add(lblCapRemark)
        grpSummary.Controls.Add(lblValRemark)
        grpSummary.Controls.Add(progressPaid)
        grpSummary.Dock = DockStyle.Top
        grpSummary.Location = New Point(0, 70)
        grpSummary.Name = "grpSummary"
        grpSummary.Size = New Size(960, 210)
        grpSummary.TabIndex = 1
        grpSummary.Text = " SUMMARY"
        ' 
        ' lblCapTotalLoan
        ' 
        lblCapTotalLoan.Appearance.ForeColor = Color.Gray
        lblCapTotalLoan.Appearance.Options.UseForeColor = True
        lblCapTotalLoan.Location = New Point(20, 32)
        lblCapTotalLoan.Name = "lblCapTotalLoan"
        lblCapTotalLoan.Size = New Size(90, 13)
        lblCapTotalLoan.TabIndex = 0
        lblCapTotalLoan.Text = "Total Loan Amount"
        ' 
        ' lblValTotalLoan
        ' 
        lblValTotalLoan.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValTotalLoan.Appearance.Options.UseFont = True
        lblValTotalLoan.Location = New Point(20, 50)
        lblValTotalLoan.Name = "lblValTotalLoan"
        lblValTotalLoan.Size = New Size(31, 20)
        lblValTotalLoan.TabIndex = 1
        lblValTotalLoan.Text = "0.00"
        ' 
        ' lblCapTotalPaid
        ' 
        lblCapTotalPaid.Appearance.ForeColor = Color.Gray
        lblCapTotalPaid.Appearance.Options.UseForeColor = True
        lblCapTotalPaid.Location = New Point(20, 82)
        lblCapTotalPaid.Name = "lblCapTotalPaid"
        lblCapTotalPaid.Size = New Size(47, 13)
        lblCapTotalPaid.TabIndex = 2
        lblCapTotalPaid.Text = "Total Paid"
        ' 
        ' lblValTotalPaid
        ' 
        lblValTotalPaid.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValTotalPaid.Appearance.ForeColor = Color.SeaGreen
        lblValTotalPaid.Appearance.Options.UseFont = True
        lblValTotalPaid.Appearance.Options.UseForeColor = True
        lblValTotalPaid.Location = New Point(20, 100)
        lblValTotalPaid.Name = "lblValTotalPaid"
        lblValTotalPaid.Size = New Size(31, 20)
        lblValTotalPaid.TabIndex = 3
        lblValTotalPaid.Text = "0.00"
        ' 
        ' lblCapBalance
        ' 
        lblCapBalance.Appearance.ForeColor = Color.Gray
        lblCapBalance.Appearance.Options.UseForeColor = True
        lblCapBalance.Location = New Point(20, 132)
        lblCapBalance.Name = "lblCapBalance"
        lblCapBalance.Size = New Size(99, 13)
        lblCapBalance.TabIndex = 4
        lblCapBalance.Text = "Outstanding Balance"
        ' 
        ' lblValBalance
        ' 
        lblValBalance.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValBalance.Appearance.ForeColor = Color.Firebrick
        lblValBalance.Appearance.Options.UseFont = True
        lblValBalance.Appearance.Options.UseForeColor = True
        lblValBalance.Location = New Point(20, 150)
        lblValBalance.Name = "lblValBalance"
        lblValBalance.Size = New Size(31, 20)
        lblValBalance.TabIndex = 5
        lblValBalance.Text = "0.00"
        ' 
        ' lblCapAmortization
        ' 
        lblCapAmortization.Appearance.ForeColor = Color.Gray
        lblCapAmortization.Appearance.Options.UseForeColor = True
        lblCapAmortization.Location = New Point(250, 32)
        lblCapAmortization.Name = "lblCapAmortization"
        lblCapAmortization.Size = New Size(114, 13)
        lblCapAmortization.TabIndex = 6
        lblCapAmortization.Text = "Amortization per Cutoff"
        ' 
        ' lblValAmortization
        ' 
        lblValAmortization.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValAmortization.Appearance.Options.UseFont = True
        lblValAmortization.Location = New Point(250, 50)
        lblValAmortization.Name = "lblValAmortization"
        lblValAmortization.Size = New Size(31, 20)
        lblValAmortization.TabIndex = 7
        lblValAmortization.Text = "0.00"
        ' 
        ' lblCapStartDate
        ' 
        lblCapStartDate.Appearance.ForeColor = Color.Gray
        lblCapStartDate.Appearance.Options.UseForeColor = True
        lblCapStartDate.Location = New Point(250, 82)
        lblCapStartDate.Name = "lblCapStartDate"
        lblCapStartDate.Size = New Size(76, 13)
        lblCapStartDate.TabIndex = 8
        lblCapStartDate.Text = "Loan Start Date"
        ' 
        ' lblValStartDate
        ' 
        lblValStartDate.Appearance.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblValStartDate.Appearance.Options.UseFont = True
        lblValStartDate.Location = New Point(250, 100)
        lblValStartDate.Name = "lblValStartDate"
        lblValStartDate.Size = New Size(5, 17)
        lblValStartDate.TabIndex = 9
        lblValStartDate.Text = "-"
        ' 
        ' lblCapFrequency
        ' 
        lblCapFrequency.Appearance.ForeColor = Color.Gray
        lblCapFrequency.Appearance.Options.UseForeColor = True
        lblCapFrequency.Location = New Point(250, 132)
        lblCapFrequency.Name = "lblCapFrequency"
        lblCapFrequency.Size = New Size(51, 13)
        lblCapFrequency.TabIndex = 10
        lblCapFrequency.Text = "Frequency"
        ' 
        ' lblValFrequency
        ' 
        lblValFrequency.Appearance.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblValFrequency.Appearance.Options.UseFont = True
        lblValFrequency.Location = New Point(250, 150)
        lblValFrequency.Name = "lblValFrequency"
        lblValFrequency.Size = New Size(5, 17)
        lblValFrequency.TabIndex = 11
        lblValFrequency.Text = "-"
        ' 
        ' lblCapProgress
        ' 
        lblCapProgress.Appearance.ForeColor = Color.Gray
        lblCapProgress.Appearance.Options.UseForeColor = True
        lblCapProgress.Location = New Point(710, 32)
        lblCapProgress.Name = "lblCapProgress"
        lblCapProgress.Size = New Size(42, 13)
        lblCapProgress.TabIndex = 18
        lblCapProgress.Text = "Progress"
        ' 
        ' lblValProgress
        ' 
        lblValProgress.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValProgress.Appearance.Options.UseFont = True
        lblValProgress.Location = New Point(710, 50)
        lblValProgress.Name = "lblValProgress"
        lblValProgress.Size = New Size(75, 20)
        lblValProgress.TabIndex = 19
        lblValProgress.Text = "0 of 0 paid"
        ' 
        ' lblCapRemaining
        ' 
        lblCapRemaining.Appearance.ForeColor = Color.Gray
        lblCapRemaining.Appearance.Options.UseForeColor = True
        lblCapRemaining.Location = New Point(710, 106)
        lblCapRemaining.Name = "lblCapRemaining"
        lblCapRemaining.Size = New Size(105, 13)
        lblCapRemaining.TabIndex = 21
        lblCapRemaining.Text = "Remaining Deductions"
        ' 
        ' lblValRemaining
        ' 
        lblValRemaining.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValRemaining.Appearance.Options.UseFont = True
        lblValRemaining.Location = New Point(710, 124)
        lblValRemaining.Name = "lblValRemaining"
        lblValRemaining.Size = New Size(9, 20)
        lblValRemaining.TabIndex = 22
        lblValRemaining.Text = "0"
        ' 
        ' lblCapNextDate
        ' 
        lblCapNextDate.Appearance.ForeColor = Color.Gray
        lblCapNextDate.Appearance.Options.UseForeColor = True
        lblCapNextDate.Location = New Point(480, 32)
        lblCapNextDate.Name = "lblCapNextDate"
        lblCapNextDate.Size = New Size(100, 13)
        lblCapNextDate.TabIndex = 12
        lblCapNextDate.Text = "Next Deduction Date"
        ' 
        ' lblValNextDate
        ' 
        lblValNextDate.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValNextDate.Appearance.ForeColor = Color.SteelBlue
        lblValNextDate.Appearance.Options.UseFont = True
        lblValNextDate.Appearance.Options.UseForeColor = True
        lblValNextDate.Location = New Point(480, 50)
        lblValNextDate.Name = "lblValNextDate"
        lblValNextDate.Size = New Size(6, 20)
        lblValNextDate.TabIndex = 13
        lblValNextDate.Text = "-"
        ' 
        ' lblCapNextAmount
        ' 
        lblCapNextAmount.Appearance.ForeColor = Color.Gray
        lblCapNextAmount.Appearance.Options.UseForeColor = True
        lblCapNextAmount.Location = New Point(480, 82)
        lblCapNextAmount.Name = "lblCapNextAmount"
        lblCapNextAmount.Size = New Size(114, 13)
        lblCapNextAmount.TabIndex = 14
        lblCapNextAmount.Text = "Next Deduction Amount"
        ' 
        ' lblValNextAmount
        ' 
        lblValNextAmount.Appearance.Font = New Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblValNextAmount.Appearance.Options.UseFont = True
        lblValNextAmount.Location = New Point(480, 100)
        lblValNextAmount.Name = "lblValNextAmount"
        lblValNextAmount.Size = New Size(31, 20)
        lblValNextAmount.TabIndex = 15
        lblValNextAmount.Text = "0.00"
        ' 
        ' lblCapMethod
        ' 
        lblCapMethod.Appearance.ForeColor = Color.Gray
        lblCapMethod.Appearance.Options.UseForeColor = True
        lblCapMethod.Location = New Point(480, 132)
        lblCapMethod.Name = "lblCapMethod"
        lblCapMethod.Size = New Size(88, 13)
        lblCapMethod.TabIndex = 16
        lblCapMethod.Text = "Computation Basis"
        ' 
        ' lblValMethod
        ' 
        lblValMethod.Appearance.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblValMethod.Appearance.Options.UseFont = True
        lblValMethod.Location = New Point(480, 150)
        lblValMethod.Name = "lblValMethod"
        lblValMethod.Size = New Size(5, 17)
        lblValMethod.TabIndex = 17
        lblValMethod.Text = "-"
        ' 
        ' lblCapRemark
        ' 
        lblCapRemark.Appearance.ForeColor = Color.Gray
        lblCapRemark.Appearance.Options.UseForeColor = True
        lblCapRemark.Location = New Point(20, 180)
        lblCapRemark.Name = "lblCapRemark"
        lblCapRemark.Size = New Size(36, 13)
        lblCapRemark.TabIndex = 23
        lblCapRemark.Text = "Remark"
        ' 
        ' lblValRemark
        ' 
        lblValRemark.Appearance.Font = New Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, CByte(0))
        lblValRemark.Appearance.Options.UseFont = True
        lblValRemark.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblValRemark.Location = New Point(75, 180)
        lblValRemark.Name = "lblValRemark"
        lblValRemark.Size = New Size(855, 14)
        lblValRemark.TabIndex = 24
        lblValRemark.Text = "-"
        ' 
        ' progressPaid
        ' 
        progressPaid.Location = New Point(710, 74)
        progressPaid.Name = "progressPaid"
        progressPaid.Properties.ShowTitle = True
        progressPaid.Size = New Size(220, 18)
        progressPaid.TabIndex = 20
        ' 
        ' grpSchedule
        ' 
        grpSchedule.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpSchedule.AppearanceCaption.Options.UseFont = True
        grpSchedule.Controls.Add(gridconSchedule)
        grpSchedule.Dock = DockStyle.Fill
        grpSchedule.Location = New Point(0, 280)
        grpSchedule.Name = "grpSchedule"
        grpSchedule.Size = New Size(960, 290)
        grpSchedule.TabIndex = 2
        grpSchedule.Text = " AMORTIZATION SCHEDULE / DEDUCTION HISTORY"
        ' 
        ' gridconSchedule
        ' 
        gridconSchedule.Dock = DockStyle.Fill
        gridconSchedule.Location = New Point(2, 23)
        gridconSchedule.MainView = gridviewSchedule
        gridconSchedule.Name = "gridconSchedule"
        gridconSchedule.Size = New Size(956, 265)
        gridconSchedule.TabIndex = 0
        gridconSchedule.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewSchedule})
        ' 
        ' gridviewSchedule
        ' 
        gridviewSchedule.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {colInstallmentNo, colScheduledDate, colPrincipalDue, colInterestDue, colAmountDue, colAmountPaid, colPaidDate, colSchedStatus, colPayrollRef})
        gridviewSchedule.GridControl = gridconSchedule
        gridviewSchedule.Name = "gridviewSchedule"
        gridviewSchedule.OptionsView.ShowFooter = True
        gridviewSchedule.OptionsView.ShowGroupPanel = False
        ' 
        ' colInstallmentNo
        ' 
        colInstallmentNo.Caption = "#"
        colInstallmentNo.FieldName = "InstallmentNo"
        colInstallmentNo.Name = "colInstallmentNo"
        colInstallmentNo.Visible = True
        colInstallmentNo.VisibleIndex = 0
        colInstallmentNo.Width = 45
        ' 
        ' colScheduledDate
        ' 
        colScheduledDate.Caption = "Scheduled Date"
        colScheduledDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        colScheduledDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        colScheduledDate.FieldName = "ScheduledDate"
        colScheduledDate.Name = "colScheduledDate"
        colScheduledDate.Visible = True
        colScheduledDate.VisibleIndex = 1
        colScheduledDate.Width = 110
        ' 
        ' colPrincipalDue
        ' 
        colPrincipalDue.Caption = "Principal"
        colPrincipalDue.DisplayFormat.FormatString = "n2"
        colPrincipalDue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colPrincipalDue.FieldName = "PrincipalDue"
        colPrincipalDue.Name = "colPrincipalDue"
        colPrincipalDue.Visible = True
        colPrincipalDue.VisibleIndex = 2
        colPrincipalDue.Width = 100
        ' 
        ' colInterestDue
        ' 
        colInterestDue.Caption = "Interest"
        colInterestDue.DisplayFormat.FormatString = "n2"
        colInterestDue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colInterestDue.FieldName = "InterestDue"
        colInterestDue.Name = "colInterestDue"
        colInterestDue.Visible = True
        colInterestDue.VisibleIndex = 3
        colInterestDue.Width = 100
        ' 
        ' colAmountDue
        ' 
        colAmountDue.Caption = "Amount Due"
        colAmountDue.DisplayFormat.FormatString = "n2"
        colAmountDue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colAmountDue.FieldName = "AmountDue"
        colAmountDue.Name = "colAmountDue"
        colAmountDue.Visible = True
        colAmountDue.VisibleIndex = 4
        colAmountDue.Width = 110
        ' 
        ' colAmountPaid
        ' 
        colAmountPaid.Caption = "Amount Paid"
        colAmountPaid.DisplayFormat.FormatString = "n2"
        colAmountPaid.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        colAmountPaid.FieldName = "AmountPaid"
        colAmountPaid.Name = "colAmountPaid"
        colAmountPaid.Visible = True
        colAmountPaid.VisibleIndex = 5
        colAmountPaid.Width = 110
        ' 
        ' colPaidDate
        ' 
        colPaidDate.Caption = "Date Deducted"
        colPaidDate.DisplayFormat.FormatString = "MM/dd/yyyy"
        colPaidDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        colPaidDate.FieldName = "PaidDate"
        colPaidDate.Name = "colPaidDate"
        colPaidDate.Visible = True
        colPaidDate.VisibleIndex = 6
        colPaidDate.Width = 110
        ' 
        ' colSchedStatus
        ' 
        colSchedStatus.Caption = "Status"
        colSchedStatus.FieldName = "StatusText"
        colSchedStatus.Name = "colSchedStatus"
        colSchedStatus.Visible = True
        colSchedStatus.VisibleIndex = 7
        colSchedStatus.Width = 90
        ' 
        ' colPayrollRef
        ' 
        colPayrollRef.Caption = "Payroll Ref."
        colPayrollRef.FieldName = "PayrollRefNo"
        colPayrollRef.Name = "colPayrollRef"
        colPayrollRef.Visible = True
        colPayrollRef.VisibleIndex = 8
        colPayrollRef.Width = 120
        ' 
        ' pnlFooter
        ' 
        pnlFooter.Controls.Add(btnClose)
        pnlFooter.Dock = DockStyle.Bottom
        pnlFooter.Location = New Point(0, 570)
        pnlFooter.Name = "pnlFooter"
        pnlFooter.Size = New Size(960, 50)
        pnlFooter.TabIndex = 3
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.Location = New Point(830, 11)
        btnClose.Name = "btnClose"
        btnClose.Size = New Size(110, 28)
        btnClose.TabIndex = 0
        btnClose.Text = "Close"
        ' 
        ' frmLoanDetails
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(960, 620)
        Controls.Add(grpSchedule)
        Controls.Add(grpSummary)
        Controls.Add(pnlHeader)
        Controls.Add(pnlFooter)
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmLoanDetails"
        StartPosition = FormStartPosition.CenterParent
        Text = "Loan Details"
        CType(pnlHeader, ComponentModel.ISupportInitialize).EndInit()
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        CType(grpSummary, ComponentModel.ISupportInitialize).EndInit()
        grpSummary.ResumeLayout(False)
        grpSummary.PerformLayout()
        CType(progressPaid.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpSchedule, ComponentModel.ISupportInitialize).EndInit()
        grpSchedule.ResumeLayout(False)
        CType(gridconSchedule, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewSchedule, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlFooter, ComponentModel.ISupportInitialize).EndInit()
        pnlFooter.ResumeLayout(False)
        ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblEmployee As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblLoanTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblStatusBadge As DevExpress.XtraEditors.LabelControl

    Friend WithEvents grpSummary As DevExpress.XtraEditors.GroupControl
    Friend WithEvents lblCapTotalLoan As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValTotalLoan As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapTotalPaid As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValTotalPaid As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapBalance As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValBalance As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapAmortization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValAmortization As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapStartDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValStartDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapFrequency As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValFrequency As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapRemaining As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValRemaining As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapNextDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValNextDate As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapNextAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValNextAmount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapMethod As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValMethod As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCapRemark As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblValRemark As DevExpress.XtraEditors.LabelControl
    Friend WithEvents progressPaid As DevExpress.XtraEditors.ProgressBarControl

    Friend WithEvents grpSchedule As DevExpress.XtraEditors.GroupControl
    Friend WithEvents gridconSchedule As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewSchedule As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colInstallmentNo As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colScheduledDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPrincipalDue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colInterestDue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAmountDue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAmountPaid As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPaidDate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSchedStatus As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPayrollRef As DevExpress.XtraGrid.Columns.GridColumn

    Friend WithEvents pnlFooter As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton

End Class