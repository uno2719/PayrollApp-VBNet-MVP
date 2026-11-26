Imports System.IO
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraReports.UI
Imports Payroll.GlobalShared.Models

''' <summary>
''' Payslip (Letter, isang payslip kada page). DataSource = List(Of PayslipDocument).
''' PAALALA: hand-written ang Designer.vb (may mga helper Sub). Kung bubuksan sa Report Designer ng Visual Studio at
''' ise-save, baka i-regenerate nito ang InitializeComponent - mas ligtas na sa code na lang baguhin ang layout.
''' </summary>
Public Class rptPayslip

    Private Shared ReadOnly BoxBorder As Color = Color.FromArgb(120, 120, 120)
    Private Shared ReadOnly NetBack As Color = Color.FromArgb(232, 245, 233)
    Private Shared ReadOnly NetFore As Color = Color.FromArgb(46, 105, 50)
    Private Shared ReadOnly GroupFore As Color = Color.FromArgb(110, 110, 110)

    Private Shared ReadOnly FontLine As New DevExpress.Drawing.DXFont("Segoe UI", 8.5F)
    Private Shared ReadOnly FontBold As New DevExpress.Drawing.DXFont("Segoe UI", 8.5F, DevExpress.Drawing.DXFontStyle.Bold)
    Private Shared ReadOnly FontGroup As New DevExpress.Drawing.DXFont("Segoe UI", 7.5F, DevExpress.Drawing.DXFontStyle.Italic)
    Private Shared ReadOnly FontNet As New DevExpress.Drawing.DXFont("Segoe UI", 9.5F, DevExpress.Drawing.DXFontStyle.Bold)

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>Gumawa ng report para sa mga payslip (lahat ng documents ng batch, o isang bahagi lang nito).</summary>
    Public Shared Function Create(batch As PayslipBatch, Optional documents As IEnumerable(Of PayslipDocument) = Nothing) As rptPayslip
        Dim rpt As New rptPayslip()
        Dim docs = If(documents, batch.Documents).ToList()
        rpt.DataSource = docs
        rpt.DisplayName = "Payslip " & batch.DisplayLabel

        ' Logo (iisa para sa lahat): file path mula sa Company Profile. Walang logo = itatago at aatras ang pangalan.
        Dim logo = batch.Company?.LogoFullPath
        If Not String.IsNullOrWhiteSpace(logo) AndAlso File.Exists(logo) Then
            rpt.picLogo.ImageUrl = logo
        Else
            rpt.picLogo.Visible = False
            For Each lbl In {rpt.lblCompanyName, rpt.lblCompanyAddress, rpt.lblTitle, rpt.lblDraftTag}
                lbl.LocationFloat = New DevExpress.Utils.PointFloat(lbl.LocationFloat.X - 72.0F, lbl.LocationFloat.Y)
            Next
        End If

        ' Watermark kapag LAHAT ay draft; kapag halo (Group by pay date), ang pulang tag na lang sa header ng bawat draft.
        If docs.Count > 0 AndAlso docs.All(Function(d) d.IsDraft) Then
            rpt.Watermark.Text = "DRAFT - NOT YET POSTED"
            rpt.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal
            rpt.Watermark.Font = New DevExpress.Drawing.DXFont("Segoe UI", 48.0F, DevExpress.Drawing.DXFontStyle.Bold)
            rpt.Watermark.ForeColor = Color.Red
            rpt.Watermark.TextTransparency = 215
            rpt.Watermark.ShowBehind = False
        End If

        Return rpt
    End Function

    ' =============================================
    ' Estilo ng bawat row ng Earnings / Deductions ayon sa Kind
    ' =============================================
    Private Sub DetailRows_BeforePrint(sender As Object, e As EventArgs) Handles DetailRows.BeforePrint
        Dim row = TryCast(drRows.GetCurrentRow(), PayslipPairRow)
        If row Is Nothing Then Return

        StyleSide(row.LeftKind, cellLeftText, cellLeftAmt)
        StyleSide(row.RightKind, cellRightText, cellRightAmt)
    End Sub

    Private Shared Sub StyleSide(kind As PayslipCellKind, textCell As XRTableCell, amtCell As XRTableCell)
        ' Default: linya sa loob ng kahon (border sa labas na gilid lang)
        textCell.Font = FontLine
        amtCell.Font = FontLine
        textCell.ForeColor = Color.Black
        amtCell.ForeColor = Color.Black
        textCell.BackColor = Color.Transparent
        amtCell.BackColor = Color.Transparent
        textCell.BorderColor = BoxBorder
        amtCell.BorderColor = BoxBorder
        textCell.Borders = BorderSide.Left
        amtCell.Borders = BorderSide.Right
        textCell.Padding = New PaddingInfo(5, 5, 0, 0, 100.0F)

        Select Case kind
            Case PayslipCellKind.GroupHeader
                textCell.Font = FontGroup
                textCell.ForeColor = GroupFore

            Case PayslipCellKind.GroupLine
                textCell.Padding = New PaddingInfo(16, 5, 0, 0, 100.0F)

            Case PayslipCellKind.Total
                textCell.Font = FontBold
                amtCell.Font = FontBold
                textCell.Borders = BorderSide.Left Or BorderSide.Top Or BorderSide.Bottom
                amtCell.Borders = BorderSide.Right Or BorderSide.Top Or BorderSide.Bottom

            Case PayslipCellKind.Net
                textCell.Font = FontNet
                amtCell.Font = FontNet
                textCell.ForeColor = NetFore
                amtCell.ForeColor = NetFore
                textCell.BackColor = NetBack
                amtCell.BackColor = NetBack
                textCell.BorderColor = NetFore
                amtCell.BorderColor = NetFore
                textCell.Borders = BorderSide.Left Or BorderSide.Bottom
                amtCell.Borders = BorderSide.Right Or BorderSide.Bottom

            Case PayslipCellKind.Outside
                textCell.Borders = BorderSide.None
                amtCell.Borders = BorderSide.None
        End Select
    End Sub

End Class
