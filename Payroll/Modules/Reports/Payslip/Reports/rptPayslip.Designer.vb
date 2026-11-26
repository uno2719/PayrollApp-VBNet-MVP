Imports DevExpress.XtraReports.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class rptPayslip
    Inherits DevExpress.XtraReports.UI.XtraReport

    'XtraReport overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    ' Units: 1/100 inch (ReportUnit default). Letter, 0.40" margins -> 770 ang lapad ng laman.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TopMargin = New TopMarginBand()
        BottomMargin = New BottomMarginBand()
        Detail = New DetailBand()
        picLogo = New XRPictureBox()
        lblCompanyName = New XRLabel()
        lblCompanyAddress = New XRLabel()
        lblTitle = New XRLabel()
        lblDraftTag = New XRLabel()
        pnlNet = New XRPanel()
        lblNetCaption = New XRLabel()
        lblNetAmount = New XRLabel()
        lblPayDate = New XRLabel()
        lineHeader = New XRLine()
        capEmployee = New XRLabel()
        valEmployee = New XRLabel()
        capEmployeeNo = New XRLabel()
        valEmployeeNo = New XRLabel()
        capPeriod = New XRLabel()
        valPeriod = New XRLabel()
        capDesignation = New XRLabel()
        valDesignation = New XRLabel()
        capDepartment = New XRLabel()
        valDepartment = New XRLabel()
        capPayCycle = New XRLabel()
        valPayCycle = New XRLabel()
        capSSS = New XRLabel()
        valSSS = New XRLabel()
        capPhilHealth = New XRLabel()
        valPhilHealth = New XRLabel()
        capPagIbig = New XRLabel()
        valPagIbig = New XRLabel()
        capTIN = New XRLabel()
        valTIN = New XRLabel()
        capBranch = New XRLabel()
        valBranch = New XRLabel()
        capRef = New XRLabel()
        valRef = New XRLabel()

        drRows = New DetailReportBand()
        hdrRows = New ReportHeaderBand()
        tblRowsHeader = New XRTable()
        rowRowsHeader = New XRTableRow()
        hdrEarnings = New XRTableCell()
        hdrEarningsAmt = New XRTableCell()
        hdrGap = New XRTableCell()
        hdrDeductions = New XRTableCell()
        hdrDeductionsAmt = New XRTableCell()
        DetailRows = New DetailBand()
        tblRows = New XRTable()
        rowRows = New XRTableRow()
        cellLeftText = New XRTableCell()
        cellLeftAmt = New XRTableCell()
        cellGap = New XRTableCell()
        cellRightText = New XRTableCell()
        cellRightAmt = New XRTableCell()

        drLoans = New DetailReportBand()
        hdrLoans = New ReportHeaderBand()
        tblLoansHeader = New XRTable()
        rowLoansHeader = New XRTableRow()
        hdrLoanName = New XRTableCell()
        hdrLoanProgress = New XRTableCell()
        hdrLoanBalance = New XRTableCell()
        DetailLoans = New DetailBand()
        tblLoans = New XRTable()
        rowLoans = New XRTableRow()
        cellLoanName = New XRTableCell()
        cellLoanProgress = New XRTableCell()
        cellLoanBalance = New XRTableCell()

        drFooter = New DetailReportBand()
        DetailFooter = New DetailBand()
        pnlYtd = New XRPanel()
        capYtdGross = New XRLabel()
        valYtdGross = New XRLabel()
        capYtdContrib = New XRLabel()
        valYtdContrib = New XRLabel()
        capYtdTax = New XRLabel()
        valYtdTax = New XRLabel()
        lblPrinted = New XRLabel()
        lblAllowanceNote = New XRLabel()
        lineReceivedBy = New XRLine()
        lblReceivedBy = New XRLabel()

        CType(tblRowsHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(tblRows, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(tblLoansHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(tblLoans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()

        Dim fontBase As New DevExpress.Drawing.DXFont("Segoe UI", 8.5F)
        Dim fontCaption As New DevExpress.Drawing.DXFont("Segoe UI", 8.0F)
        Dim fontBold As New DevExpress.Drawing.DXFont("Segoe UI", 8.5F, DevExpress.Drawing.DXFontStyle.Bold)
        Dim gray As System.Drawing.Color = System.Drawing.Color.FromArgb(110, 110, 110)
        Dim boxBorder As System.Drawing.Color = System.Drawing.Color.FromArgb(120, 120, 120)
        Dim headerBack As System.Drawing.Color = System.Drawing.Color.FromArgb(242, 242, 242)

        '
        ' TopMargin / BottomMargin
        '
        TopMargin.HeightF = 40.0F
        TopMargin.Name = "TopMargin"
        BottomMargin.HeightF = 40.0F
        BottomMargin.Name = "BottomMargin"

        '
        ' Detail (header ng payslip - isang PayslipDocument)
        '
        Detail.HeightF = 158.0F
        Detail.Name = "Detail"
        Detail.PageBreak = PageBreak.BeforeBandExceptFirstEntry
        Detail.Controls.AddRange(New XRControl() {picLogo, lblCompanyName, lblCompanyAddress, lblTitle, lblDraftTag, pnlNet, lineHeader,
            capEmployee, valEmployee, capEmployeeNo, valEmployeeNo, capPeriod, valPeriod,
            capDesignation, valDesignation, capDepartment, valDepartment, capPayCycle, valPayCycle,
            capSSS, valSSS, capPhilHealth, valPhilHealth, capPagIbig, valPagIbig,
            capTIN, valTIN, capBranch, valBranch, capRef, valRef})

        picLogo.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 0.0F)
        picLogo.SizeF = New System.Drawing.SizeF(62.0F, 62.0F)
        picLogo.Name = "picLogo"
        picLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage

        lblCompanyName.LocationFloat = New DevExpress.Utils.PointFloat(72.0F, 0.0F)
        lblCompanyName.SizeF = New System.Drawing.SizeF(450.0F, 24.0F)
        lblCompanyName.Name = "lblCompanyName"
        lblCompanyName.Font = New DevExpress.Drawing.DXFont("Segoe UI", 13.0F, DevExpress.Drawing.DXFontStyle.Bold)
        lblCompanyName.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblCompanyName.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[CompanyName]"))

        lblCompanyAddress.LocationFloat = New DevExpress.Utils.PointFloat(72.0F, 24.0F)
        lblCompanyAddress.SizeF = New System.Drawing.SizeF(450.0F, 16.0F)
        lblCompanyAddress.Name = "lblCompanyAddress"
        lblCompanyAddress.Font = fontCaption
        lblCompanyAddress.ForeColor = gray
        lblCompanyAddress.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblCompanyAddress.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[CompanyAddress]"))

        lblTitle.LocationFloat = New DevExpress.Utils.PointFloat(72.0F, 42.0F)
        lblTitle.SizeF = New System.Drawing.SizeF(110.0F, 18.0F)
        lblTitle.Name = "lblTitle"
        lblTitle.Font = New DevExpress.Drawing.DXFont("Segoe UI", 10.0F, DevExpress.Drawing.DXFontStyle.Bold)
        lblTitle.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblTitle.Text = "PAYSLIP"

        ' Pulang tag kada payslip na DRAFT (hindi pa Posted ang cutoff) - para sa Group by pay date na halo ang status.
        lblDraftTag.LocationFloat = New DevExpress.Utils.PointFloat(190.0F, 43.0F)
        lblDraftTag.SizeF = New System.Drawing.SizeF(330.0F, 16.0F)
        lblDraftTag.Name = "lblDraftTag"
        lblDraftTag.Font = New DevExpress.Drawing.DXFont("Segoe UI", 8.0F, DevExpress.Drawing.DXFontStyle.Bold)
        lblDraftTag.ForeColor = System.Drawing.Color.FromArgb(200, 30, 30)
        lblDraftTag.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblDraftTag.Text = "DRAFT - NOT YET POSTED"
        lblDraftTag.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Visible", "[IsDraft]"))

        ' NET PAY box (kanang itaas)
        pnlNet.LocationFloat = New DevExpress.Utils.PointFloat(540.0F, 0.0F)
        pnlNet.SizeF = New System.Drawing.SizeF(230.0F, 64.0F)
        pnlNet.Name = "pnlNet"
        pnlNet.BackColor = System.Drawing.Color.FromArgb(232, 245, 233)
        pnlNet.BorderColor = System.Drawing.Color.FromArgb(76, 140, 80)
        pnlNet.Borders = DevExpress.XtraPrinting.BorderSide.All
        pnlNet.BorderWidth = 1.0F
        pnlNet.Controls.AddRange(New XRControl() {lblNetCaption, lblNetAmount, lblPayDate})

        lblNetCaption.LocationFloat = New DevExpress.Utils.PointFloat(8.0F, 4.0F)
        lblNetCaption.SizeF = New System.Drawing.SizeF(214.0F, 14.0F)
        lblNetCaption.Name = "lblNetCaption"
        lblNetCaption.Font = New DevExpress.Drawing.DXFont("Segoe UI", 7.5F, DevExpress.Drawing.DXFontStyle.Bold)
        lblNetCaption.ForeColor = System.Drawing.Color.FromArgb(46, 105, 50)
        lblNetCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        lblNetCaption.Text = "NET PAY"

        lblNetAmount.LocationFloat = New DevExpress.Utils.PointFloat(8.0F, 18.0F)
        lblNetAmount.SizeF = New System.Drawing.SizeF(214.0F, 28.0F)
        lblNetAmount.Name = "lblNetAmount"
        lblNetAmount.Font = New DevExpress.Drawing.DXFont("Segoe UI", 16.0F, DevExpress.Drawing.DXFontStyle.Bold)
        lblNetAmount.ForeColor = System.Drawing.Color.FromArgb(46, 105, 50)
        lblNetAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        lblNetAmount.TextFormatString = "₱ {0:N2}"
        lblNetAmount.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[NetPay]"))

        lblPayDate.LocationFloat = New DevExpress.Utils.PointFloat(8.0F, 46.0F)
        lblPayDate.SizeF = New System.Drawing.SizeF(214.0F, 14.0F)
        lblPayDate.Name = "lblPayDate"
        lblPayDate.Font = New DevExpress.Drawing.DXFont("Segoe UI", 7.5F)
        lblPayDate.ForeColor = System.Drawing.Color.FromArgb(46, 105, 50)
        lblPayDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight
        lblPayDate.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "'Pay date: ' + [PayDateText]"))

        lineHeader.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 70.0F)
        lineHeader.SizeF = New System.Drawing.SizeF(770.0F, 4.0F)
        lineHeader.Name = "lineHeader"
        lineHeader.ForeColor = boxBorder

        ' Info grid: 3 column x 4 row. Caption (gray) + value.
        SetupInfo(capEmployee, valEmployee, 0, 0, "Employee", "[EmployeeName]", fontCaption, fontBold, gray)
        SetupInfo(capEmployeeNo, valEmployeeNo, 1, 0, "Employee ID", "[EmployeeNo]", fontCaption, fontBase, gray)
        SetupInfo(capPeriod, valPeriod, 2, 0, "Pay period", "[PayPeriod]", fontCaption, fontBold, gray)
        SetupInfo(capDesignation, valDesignation, 0, 1, "Designation", "[Designation]", fontCaption, fontBase, gray)
        SetupInfo(capDepartment, valDepartment, 1, 1, "Department", "[Department]", fontCaption, fontBase, gray)
        SetupInfo(capPayCycle, valPayCycle, 2, 1, "Pay cycle", "[PayCycle]", fontCaption, fontBase, gray)
        SetupInfo(capSSS, valSSS, 0, 2, "SSS #", "[SSSNo]", fontCaption, fontBase, gray)
        SetupInfo(capPhilHealth, valPhilHealth, 1, 2, "PhilHealth #", "[PhilHealthNo]", fontCaption, fontBase, gray)
        SetupInfo(capPagIbig, valPagIbig, 2, 2, "Pag-IBIG #", "[PagIBIGNo]", fontCaption, fontBase, gray)
        SetupInfo(capTIN, valTIN, 0, 3, "TIN", "[TIN]", fontCaption, fontBase, gray)
        SetupInfo(capBranch, valBranch, 1, 3, "Branch", "[Branch]", fontCaption, fontBase, gray)
        SetupInfo(capRef, valRef, 2, 3, "Ref no.", "[RefNo]", fontCaption, fontBase, gray)

        '
        ' drRows: magkatabing Earnings / Deductions (DataMember = Rows)
        '
        drRows.Name = "drRows"
        drRows.DataMember = "Rows"
        drRows.Level = 0
        drRows.Bands.AddRange(New Band() {hdrRows, DetailRows})

        hdrRows.HeightF = 22.0F
        hdrRows.Name = "hdrRows"
        hdrRows.Controls.Add(tblRowsHeader)

        tblRowsHeader.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 0.0F)
        tblRowsHeader.SizeF = New System.Drawing.SizeF(770.0F, 22.0F)
        tblRowsHeader.Name = "tblRowsHeader"
        tblRowsHeader.Rows.Add(rowRowsHeader)
        rowRowsHeader.Name = "rowRowsHeader"
        rowRowsHeader.Weight = 1.0R
        rowRowsHeader.Cells.AddRange(New XRTableCell() {hdrEarnings, hdrEarningsAmt, hdrGap, hdrDeductions, hdrDeductionsAmt})
        SetupHeaderCell(hdrEarnings, "Earnings", 285.0R, DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, False, fontBold, boxBorder, headerBack)
        SetupHeaderCell(hdrEarningsAmt, "Amount", 90.0R, DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, True, fontBold, boxBorder, headerBack)
        SetupHeaderCell(hdrGap, "", 20.0R, DevExpress.XtraPrinting.BorderSide.None, False, fontBold, boxBorder, System.Drawing.Color.Transparent)
        SetupHeaderCell(hdrDeductions, "Deductions", 285.0R, DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, False, fontBold, boxBorder, headerBack)
        SetupHeaderCell(hdrDeductionsAmt, "Amount", 90.0R, DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, True, fontBold, boxBorder, headerBack)

        DetailRows.HeightF = 17.0F
        DetailRows.Name = "DetailRows"
        DetailRows.Controls.Add(tblRows)

        tblRows.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 0.0F)
        tblRows.SizeF = New System.Drawing.SizeF(770.0F, 17.0F)
        tblRows.Name = "tblRows"
        tblRows.Rows.Add(rowRows)
        rowRows.Name = "rowRows"
        rowRows.Weight = 1.0R
        rowRows.Cells.AddRange(New XRTableCell() {cellLeftText, cellLeftAmt, cellGap, cellRightText, cellRightAmt})
        SetupBodyCell(cellLeftText, "[LeftText]", 285.0R, False, fontBase, boxBorder)
        SetupBodyCell(cellLeftAmt, "[LeftAmount]", 90.0R, True, fontBase, boxBorder)
        SetupBodyCell(cellGap, "", 20.0R, False, fontBase, boxBorder)
        SetupBodyCell(cellRightText, "[RightText]", 285.0R, False, fontBase, boxBorder)
        SetupBodyCell(cellRightAmt, "[RightAmount]", 90.0R, True, fontBase, boxBorder)

        '
        ' drLoans: listahan ng loans at balance (DataMember = Loans)
        '
        drLoans.Name = "drLoans"
        drLoans.DataMember = "Loans"
        drLoans.Level = 1
        drLoans.Bands.AddRange(New Band() {hdrLoans, DetailLoans})

        hdrLoans.HeightF = 34.0F
        hdrLoans.Name = "hdrLoans"
        hdrLoans.Controls.Add(tblLoansHeader)

        tblLoansHeader.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 12.0F)
        tblLoansHeader.SizeF = New System.Drawing.SizeF(770.0F, 22.0F)
        tblLoansHeader.Name = "tblLoansHeader"
        tblLoansHeader.Rows.Add(rowLoansHeader)
        rowLoansHeader.Name = "rowLoansHeader"
        rowLoansHeader.Weight = 1.0R
        rowLoansHeader.Cells.AddRange(New XRTableCell() {hdrLoanName, hdrLoanProgress, hdrLoanBalance})
        SetupHeaderCell(hdrLoanName, "Loans", 470.0R, DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, False, fontBold, boxBorder, headerBack)
        SetupHeaderCell(hdrLoanProgress, "Progress", 150.0R, DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, True, fontBold, boxBorder, headerBack)
        SetupHeaderCell(hdrLoanBalance, "Remaining balance", 150.0R, DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Top Or DevExpress.XtraPrinting.BorderSide.Bottom, True, fontBold, boxBorder, headerBack)

        DetailLoans.HeightF = 17.0F
        DetailLoans.Name = "DetailLoans"
        DetailLoans.Controls.Add(tblLoans)

        tblLoans.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 0.0F)
        tblLoans.SizeF = New System.Drawing.SizeF(770.0F, 17.0F)
        tblLoans.Name = "tblLoans"
        tblLoans.Rows.Add(rowLoans)
        rowLoans.Name = "rowLoans"
        rowLoans.Weight = 1.0R
        rowLoans.Cells.AddRange(New XRTableCell() {cellLoanName, cellLoanProgress, cellLoanBalance})
        SetupBodyCell(cellLoanName, "[LoanName]", 470.0R, False, fontBase, boxBorder)
        SetupBodyCell(cellLoanProgress, "[Progress]", 150.0R, True, fontBase, boxBorder)
        SetupBodyCell(cellLoanBalance, "[Balance]", 150.0R, True, fontBase, boxBorder)
        cellLoanName.Borders = DevExpress.XtraPrinting.BorderSide.Left Or DevExpress.XtraPrinting.BorderSide.Bottom
        cellLoanProgress.Borders = DevExpress.XtraPrinting.BorderSide.Bottom
        cellLoanBalance.Borders = DevExpress.XtraPrinting.BorderSide.Right Or DevExpress.XtraPrinting.BorderSide.Bottom

        '
        ' drFooter: YTD + printed by + allowance note + received by (DataMember = Footer, isang row)
        '
        drFooter.Name = "drFooter"
        drFooter.DataMember = "Footer"
        drFooter.Level = 2
        drFooter.Bands.Add(DetailFooter)

        DetailFooter.HeightF = 100.0F
        DetailFooter.Name = "DetailFooter"
        DetailFooter.Controls.AddRange(New XRControl() {pnlYtd, lblPrinted, lblAllowanceNote, lineReceivedBy, lblReceivedBy})

        pnlYtd.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 12.0F)
        pnlYtd.SizeF = New System.Drawing.SizeF(770.0F, 34.0F)
        pnlYtd.Name = "pnlYtd"
        pnlYtd.BackColor = headerBack
        pnlYtd.Controls.AddRange(New XRControl() {capYtdGross, valYtdGross, capYtdContrib, valYtdContrib, capYtdTax, valYtdTax})
        pnlYtd.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Visible", "[ShowYtd]"))
        SetupYtd(capYtdGross, valYtdGross, 0, "YTD gross", "[YtdGross]", fontCaption, fontBold, gray)
        SetupYtd(capYtdContrib, valYtdContrib, 1, "YTD contributions", "[YtdContributions]", fontCaption, fontBold, gray)
        SetupYtd(capYtdTax, valYtdTax, 2, "YTD tax withheld", "[YtdTax]", fontCaption, fontBold, gray)

        lblPrinted.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 66.0F)
        lblPrinted.SizeF = New System.Drawing.SizeF(500.0F, 14.0F)
        lblPrinted.Name = "lblPrinted"
        lblPrinted.Font = New DevExpress.Drawing.DXFont("Segoe UI", 7.0F)
        lblPrinted.ForeColor = gray
        lblPrinted.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblPrinted.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[PrintedText]"))

        ' Sadyang halos hindi makita (request ni Accounting Head): napakalabong kulay at maliit.
        lblAllowanceNote.LocationFloat = New DevExpress.Utils.PointFloat(0.0F, 80.0F)
        lblAllowanceNote.SizeF = New System.Drawing.SizeF(300.0F, 12.0F)
        lblAllowanceNote.Name = "lblAllowanceNote"
        lblAllowanceNote.Font = New DevExpress.Drawing.DXFont("Segoe UI", 6.0F)
        lblAllowanceNote.ForeColor = System.Drawing.Color.FromArgb(228, 228, 228)
        lblAllowanceNote.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        lblAllowanceNote.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", "[AllowanceNote]"))

        lineReceivedBy.LocationFloat = New DevExpress.Utils.PointFloat(570.0F, 62.0F)
        lineReceivedBy.SizeF = New System.Drawing.SizeF(200.0F, 4.0F)
        lineReceivedBy.Name = "lineReceivedBy"
        lineReceivedBy.ForeColor = boxBorder
        lineReceivedBy.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Visible", "[ShowReceivedBy]"))

        lblReceivedBy.LocationFloat = New DevExpress.Utils.PointFloat(570.0F, 66.0F)
        lblReceivedBy.SizeF = New System.Drawing.SizeF(200.0F, 14.0F)
        lblReceivedBy.Name = "lblReceivedBy"
        lblReceivedBy.Font = New DevExpress.Drawing.DXFont("Segoe UI", 7.0F)
        lblReceivedBy.ForeColor = gray
        lblReceivedBy.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter
        lblReceivedBy.Text = "Received by (signature over printed name)"
        lblReceivedBy.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Visible", "[ShowReceivedBy]"))

        '
        ' rptPayslip
        '
        Bands.AddRange(New Band() {TopMargin, Detail, BottomMargin, drRows, drLoans, drFooter})
        Font = fontBase
        Margins = New DevExpress.Drawing.DXMargins(40, 40, 40, 40)
        Version = "26.1"

        CType(tblRowsHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(tblRows, System.ComponentModel.ISupportInitialize).EndInit()
        CType(tblLoansHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(tblLoans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
    End Sub

    ' ---- Maliliit na helper para hindi paulit-ulit ang setup (hand-written designer) ----

    Private Shared Sub SetupInfo(cap As XRLabel, val As XRLabel, col As Integer, row As Integer, caption As String, expression As String,
                                 capFont As DevExpress.Drawing.DXFont, valFont As DevExpress.Drawing.DXFont, gray As System.Drawing.Color)
        Dim x = col * 262.0F
        Dim y = 80.0F + row * 18.0F
        Dim capWidth = 78.0F
        cap.LocationFloat = New DevExpress.Utils.PointFloat(x, y)
        cap.SizeF = New System.Drawing.SizeF(capWidth, 18.0F)
        cap.Font = capFont
        cap.ForeColor = gray
        cap.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100.0F)
        cap.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        cap.Text = caption

        val.LocationFloat = New DevExpress.Utils.PointFloat(x + capWidth, y)
        val.SizeF = New System.Drawing.SizeF(If(col = 2, 770.0F - (x + capWidth), 262.0F - capWidth - 6.0F), 18.0F)
        val.Font = valFont
        val.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100.0F)
        val.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft
        val.WordWrap = False
        val.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", expression))
    End Sub

    Private Shared Sub SetupYtd(cap As XRLabel, val As XRLabel, index As Integer, caption As String, expression As String,
                                capFont As DevExpress.Drawing.DXFont, valFont As DevExpress.Drawing.DXFont, gray As System.Drawing.Color)
        Dim x = 8.0F + index * 256.0F
        cap.LocationFloat = New DevExpress.Utils.PointFloat(x, 2.0F)
        cap.SizeF = New System.Drawing.SizeF(240.0F, 14.0F)
        cap.Font = capFont
        cap.ForeColor = gray
        cap.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        cap.Text = caption

        val.LocationFloat = New DevExpress.Utils.PointFloat(x, 16.0F)
        val.SizeF = New System.Drawing.SizeF(240.0F, 16.0F)
        val.Font = valFont
        val.Padding = New DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100.0F)
        val.TextFormatString = "{0:N2}"
        val.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", expression))
    End Sub

    Private Shared Sub SetupHeaderCell(cell As XRTableCell, text As String, weight As Double, borders As DevExpress.XtraPrinting.BorderSide,
                                       alignRight As Boolean, font As DevExpress.Drawing.DXFont, borderColor As System.Drawing.Color,
                                       backColor As System.Drawing.Color)
        cell.Text = text
        cell.Weight = weight
        cell.Borders = borders
        cell.BorderColor = borderColor
        cell.BorderWidth = 1.0F
        cell.BackColor = backColor
        cell.Font = font
        cell.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100.0F)
        cell.TextAlignment = If(alignRight, DevExpress.XtraPrinting.TextAlignment.MiddleRight, DevExpress.XtraPrinting.TextAlignment.MiddleLeft)
    End Sub

    Private Shared Sub SetupBodyCell(cell As XRTableCell, expression As String, weight As Double, alignRight As Boolean,
                                     font As DevExpress.Drawing.DXFont, borderColor As System.Drawing.Color)
        cell.Weight = weight
        cell.BorderColor = borderColor
        cell.BorderWidth = 1.0F
        cell.Font = font
        cell.Padding = New DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100.0F)
        cell.TextAlignment = If(alignRight, DevExpress.XtraPrinting.TextAlignment.MiddleRight, DevExpress.XtraPrinting.TextAlignment.MiddleLeft)
        cell.WordWrap = False
        If alignRight Then cell.TextFormatString = "{0:N2}"
        If Not String.IsNullOrEmpty(expression) Then
            cell.ExpressionBindings.Add(New ExpressionBinding("BeforePrint", "Text", expression))
        End If
    End Sub

    Friend WithEvents TopMargin As TopMarginBand
    Friend WithEvents BottomMargin As BottomMarginBand
    Friend WithEvents Detail As DetailBand
    Friend WithEvents picLogo As XRPictureBox
    Friend WithEvents lblCompanyName As XRLabel
    Friend WithEvents lblCompanyAddress As XRLabel
    Friend WithEvents lblTitle As XRLabel
    Friend WithEvents lblDraftTag As XRLabel
    Friend WithEvents pnlNet As XRPanel
    Friend WithEvents lblNetCaption As XRLabel
    Friend WithEvents lblNetAmount As XRLabel
    Friend WithEvents lblPayDate As XRLabel
    Friend WithEvents lineHeader As XRLine
    Friend WithEvents capEmployee As XRLabel
    Friend WithEvents valEmployee As XRLabel
    Friend WithEvents capEmployeeNo As XRLabel
    Friend WithEvents valEmployeeNo As XRLabel
    Friend WithEvents capPeriod As XRLabel
    Friend WithEvents valPeriod As XRLabel
    Friend WithEvents capDesignation As XRLabel
    Friend WithEvents valDesignation As XRLabel
    Friend WithEvents capDepartment As XRLabel
    Friend WithEvents valDepartment As XRLabel
    Friend WithEvents capPayCycle As XRLabel
    Friend WithEvents valPayCycle As XRLabel
    Friend WithEvents capSSS As XRLabel
    Friend WithEvents valSSS As XRLabel
    Friend WithEvents capPhilHealth As XRLabel
    Friend WithEvents valPhilHealth As XRLabel
    Friend WithEvents capPagIbig As XRLabel
    Friend WithEvents valPagIbig As XRLabel
    Friend WithEvents capTIN As XRLabel
    Friend WithEvents valTIN As XRLabel
    Friend WithEvents capBranch As XRLabel
    Friend WithEvents valBranch As XRLabel
    Friend WithEvents capRef As XRLabel
    Friend WithEvents valRef As XRLabel
    Friend WithEvents drRows As DetailReportBand
    Friend WithEvents hdrRows As ReportHeaderBand
    Friend WithEvents tblRowsHeader As XRTable
    Friend WithEvents rowRowsHeader As XRTableRow
    Friend WithEvents hdrEarnings As XRTableCell
    Friend WithEvents hdrEarningsAmt As XRTableCell
    Friend WithEvents hdrGap As XRTableCell
    Friend WithEvents hdrDeductions As XRTableCell
    Friend WithEvents hdrDeductionsAmt As XRTableCell
    Friend WithEvents DetailRows As DetailBand
    Friend WithEvents tblRows As XRTable
    Friend WithEvents rowRows As XRTableRow
    Friend WithEvents cellLeftText As XRTableCell
    Friend WithEvents cellLeftAmt As XRTableCell
    Friend WithEvents cellGap As XRTableCell
    Friend WithEvents cellRightText As XRTableCell
    Friend WithEvents cellRightAmt As XRTableCell
    Friend WithEvents drLoans As DetailReportBand
    Friend WithEvents hdrLoans As ReportHeaderBand
    Friend WithEvents tblLoansHeader As XRTable
    Friend WithEvents rowLoansHeader As XRTableRow
    Friend WithEvents hdrLoanName As XRTableCell
    Friend WithEvents hdrLoanProgress As XRTableCell
    Friend WithEvents hdrLoanBalance As XRTableCell
    Friend WithEvents DetailLoans As DetailBand
    Friend WithEvents tblLoans As XRTable
    Friend WithEvents rowLoans As XRTableRow
    Friend WithEvents cellLoanName As XRTableCell
    Friend WithEvents cellLoanProgress As XRTableCell
    Friend WithEvents cellLoanBalance As XRTableCell
    Friend WithEvents drFooter As DetailReportBand
    Friend WithEvents DetailFooter As DetailBand
    Friend WithEvents pnlYtd As XRPanel
    Friend WithEvents capYtdGross As XRLabel
    Friend WithEvents valYtdGross As XRLabel
    Friend WithEvents capYtdContrib As XRLabel
    Friend WithEvents valYtdContrib As XRLabel
    Friend WithEvents capYtdTax As XRLabel
    Friend WithEvents valYtdTax As XRLabel
    Friend WithEvents lblPrinted As XRLabel
    Friend WithEvents lblAllowanceNote As XRLabel
    Friend WithEvents lineReceivedBy As XRLine
    Friend WithEvents lblReceivedBy As XRLabel

End Class
