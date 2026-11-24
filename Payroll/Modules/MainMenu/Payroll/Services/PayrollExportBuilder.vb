Imports System.Globalization
Imports System.IO
Imports System.Text
Imports ClosedXML.Excel
Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Services

    ''' <summary>
    ''' Gumagawa ng export plan (sino ang isasama / lalaktawan) at nagsusulat ng mga file:
    '''   - BDO Converter  : .xlsx na ipe-paste sa BDO ATM Payroll Converter (ACCOUNT #, AMOUNT, NAME)
    '''   - BDO Direct     : .txt, "account[TAB]amount" bawat linya, CRLF, walang header (kapareho ng output ng Converter)
    '''   - Other Lists    : .xlsx na may sheet para sa Other Bank, Cash (may pirma) at Cheque
    ''' Walang database dito - puro logic at file writing lang.
    ''' </summary>
    Public Module PayrollExportBuilder

        Private Const BdoAccountLength As Integer = 12   ' HULA: lahat ng sample na account ay 12 digits (may leading zeros)

        ' =============================================
        ' CLASSIFY / VALIDATE
        ' =============================================

        ''' <summary>Aling listahan mapupunta ang empleyado (PayBy + Bank).</summary>
        Public Function Classify(r As PayrollPayoutRow) As String
            Dim payBy = If(r.PayBy, "").Trim()

            If payBy.Equals("Bank", StringComparison.OrdinalIgnoreCase) Then
                If Not r.BankId.HasValue Then Return PayoutGroup.Unassigned
                If String.Equals(If(r.BankCode, "").Trim(), "BDO", StringComparison.OrdinalIgnoreCase) Then Return PayoutGroup.Bdo
                Return PayoutGroup.OtherBank
            End If
            If payBy.Equals("Cash", StringComparison.OrdinalIgnoreCase) Then Return PayoutGroup.Cash
            If payBy.Equals("Cheque", StringComparison.OrdinalIgnoreCase) Then Return PayoutGroup.Cheque
            Return PayoutGroup.Unassigned
        End Function

        Public Function EffectivePayDate(c As CutoffModel) As Date
            Return If(c.PayDate.HasValue, c.PayDate.Value, c.CutoffEnd).Date
        End Function

        ''' <summary>Round sa 2 decimals (BDO: "Round off to 2 decimal places").</summary>
        Public Function RoundAmount(value As Decimal) As Decimal
            Return Math.Round(value, 2, MidpointRounding.AwayFromZero)
        End Function

        Private Function NormalizeAccount(raw As String) As String
            Return If(raw, "").Replace(" ", "").Replace("-", "").Trim()
        End Function

        Private Function IsValidBdoAccount(account As String) As Boolean
            If account.Length <> BdoAccountLength Then Return False
            For Each ch In account
                If ch < "0"c OrElse ch > "9"c Then Return False
            Next
            Return True
        End Function

        Private Function Issue(p As PayrollPayoutRow, reason As String) As PayrollExportIssue
            Return New PayrollExportIssue With {.EmployeeNo = p.EmployeeNo, .EmployeeName = p.EmployeeName, .Reason = reason}
        End Function

        Private Function GroupsFor(kind As String) As String()
            If kind = ExportFormat.OtherLists Then
                Return New String() {PayoutGroup.OtherBank, PayoutGroup.Cash, PayoutGroup.Cheque}
            End If
            Return New String() {PayoutGroup.Bdo}
        End Function

        ' =============================================
        ' PLAN
        ' =============================================

        Public Function BuildPlan(kind As String, cutoffs As List(Of CutoffModel), payouts As List(Of PayrollPayoutRow)) As PayrollExportPlan
            Dim plan As New PayrollExportPlan With {
                .Kind = kind,
                .Cutoffs = cutoffs,
                .IsCombined = cutoffs.Count > 1
            }

            Dim byId = cutoffs.ToDictionary(Function(c) c.CutoffID)

            For Each p In payouts
                p.ListGroup = Classify(p)
                p.NetPay = RoundAmount(p.NetPay)
                Dim co As CutoffModel = Nothing
                If byId.TryGetValue(p.CutoffID, co) Then
                    p.CutoffLabel = If(String.IsNullOrWhiteSpace(co.CutoffLabel), co.DisplayLabel, co.CutoffLabel)
                End If
            Next

            Dim wanted = GroupsFor(kind)

            For Each p In payouts
                If p.ListGroup = PayoutGroup.Unassigned Then
                    If p.NetPay > 0D Then plan.Skipped.Add(Issue(p, "No payout method or bank set (Employee > Earnings)"))
                    Continue For
                End If
                If Not wanted.Contains(p.ListGroup) Then Continue For

                If p.NetPay <= 0D Then
                    plan.Skipped.Add(Issue(p, "Net pay is " & p.NetPay.ToString("N2")))
                    Continue For
                End If

                Select Case p.ListGroup
                    Case PayoutGroup.Bdo
                        Dim account = NormalizeAccount(p.BankAccount)
                        If account.Length = 0 Then
                            plan.Skipped.Add(Issue(p, "No BDO account number"))
                        ElseIf Not IsValidBdoAccount(account) Then
                            plan.Skipped.Add(Issue(p, "Invalid BDO account number '" & p.BankAccount & "' (expected " & BdoAccountLength & " digits)"))
                        Else
                            p.BankAccount = account
                            plan.Rows.Add(p)
                        End If

                    Case PayoutGroup.OtherBank
                        If String.IsNullOrWhiteSpace(p.BankAccount) Then
                            plan.Skipped.Add(Issue(p, "No account number for " & If(p.BankName, "the bank")))
                        Else
                            p.BankAccount = p.BankAccount.Trim()
                            plan.Rows.Add(p)
                        End If

                    Case Else
                        plan.Rows.Add(p)
                End Select
            Next

            ' Pangalan ng file at filter
            Dim label As String
            If cutoffs.Count = 1 Then
                label = If(String.IsNullOrWhiteSpace(cutoffs(0).CutoffLabel), cutoffs(0).CycleType, cutoffs(0).CutoffLabel)
            Else
                label = "Combined"
            End If
            Dim payDate = cutoffs.Select(Function(c) EffectivePayDate(c)).Min()
            Dim baseName = SafeFileName(label) & "_" & payDate.ToString("yyyyMMdd")

            Select Case kind
                Case ExportFormat.BdoConverter
                    plan.SuggestedFileName = "BDO_Converter_" & baseName
                    plan.FileFilter = "Excel Workbook (*.xlsx)|*.xlsx"
                    plan.DefaultExt = "xlsx"
                Case ExportFormat.BdoDirect
                    plan.SuggestedFileName = "BDO_Direct_" & baseName
                    plan.FileFilter = "Text file (*.txt)|*.txt"
                    plan.DefaultExt = "txt"
                Case Else
                    plan.SuggestedFileName = "Payroll_Lists_" & baseName
                    plan.FileFilter = "Excel Workbook (*.xlsx)|*.xlsx"
                    plan.DefaultExt = "xlsx"
            End Select

            Return plan
        End Function

        Private Function SafeFileName(value As String) As String
            Dim sb As New StringBuilder()
            Dim bad = Path.GetInvalidFileNameChars()
            For Each ch In If(value, "")
                If Char.IsWhiteSpace(ch) OrElse bad.Contains(ch) Then
                    sb.Append("_"c)
                Else
                    sb.Append(ch)
                End If
            Next
            Return sb.ToString()
        End Function

        Private Function AccountOf(p As PayrollPayoutRow) As String
            Return p.BankAccount
        End Function

        ''' <summary>"LAST, FIRST" na naka-uppercase (kapareho ng sample ng BDO).</summary>
        Private Function PayeeName(p As PayrollPayoutRow) As String
            Return If(p.EmployeeName, "").Trim().ToUpperInvariant()
        End Function

        ' =============================================
        ' WRITERS
        ' =============================================

        ''' <summary>Kapareho ng template: A = ACCOUNT # (text, may leading zeros), B = AMOUNT (numeric, 2 decimals), C = NAME.</summary>
        Public Sub WriteBdoConverter(filePath As String, rows As List(Of PayrollPayoutRow))
            Using workbook As New XLWorkbook()
                Dim ws = workbook.Worksheets.Add("Sheet1")

                ws.Cell(1, 1).Value = "ACCOUNT #"
                ws.Cell(1, 2).Value = "AMOUNT"
                ws.Cell(1, 3).Value = "NAME"
                ws.Range(1, 1, 1, 3).Style.Font.Bold = True

                Dim r = 2
                For Each p In rows
                    Dim acct = ws.Cell(r, 1)
                    acct.Style.NumberFormat.Format = "@"
                    acct.Value = AccountOf(p)

                    Dim amt = ws.Cell(r, 2)
                    amt.Value = CDbl(RoundAmount(p.NetPay))
                    amt.Style.NumberFormat.Format = "#,##0.00"

                    ws.Cell(r, 3).Value = PayeeName(p)
                    r += 1
                Next

                ws.Column(1).Width = 26.7
                ws.Column(2).Width = 15
                ws.Column(3).Width = 41

                workbook.SaveAs(filePath)
            End Using
        End Sub

        ''' <summary>.txt: "account[TAB]amount" bawat linya, CRLF pagkatapos ng bawat linya, walang header, walang BOM.</summary>
        Public Sub WriteBdoDirect(filePath As String, rows As List(Of PayrollPayoutRow))
            Dim sb As New StringBuilder()
            For Each p In rows
                sb.Append(AccountOf(p)).Append(vbTab).
                   Append(RoundAmount(p.NetPay).ToString("F2", CultureInfo.InvariantCulture)).Append(vbCrLf)
            Next
            File.WriteAllText(filePath, sb.ToString(), New UTF8Encoding(False))
        End Sub

        ''' <summary>Isang workbook: sheet para sa Other Bank, Cash (may signature column) at Cheque (may cheque no. column).</summary>
        Public Sub WriteOtherLists(filePath As String, plan As PayrollExportPlan)
            Using workbook As New XLWorkbook()
                For Each g In New String() {PayoutGroup.OtherBank, PayoutGroup.Cash, PayoutGroup.Cheque}
                    Dim grp = g
                    Dim rows = plan.Rows.Where(Function(x) x.ListGroup = grp).ToList()
                    If rows.Count > 0 Then AddListSheet(workbook, grp, rows, plan)
                Next
                workbook.SaveAs(filePath)
            End Using
        End Sub

        Private Sub AddListSheet(workbook As XLWorkbook, grp As String, rows As List(Of PayrollPayoutRow), plan As PayrollExportPlan)
            Dim ws = workbook.Worksheets.Add(PayoutGroup.DisplayName(grp))

            Dim labels = String.Join(", ", plan.Cutoffs.Select(Function(c) If(String.IsNullOrWhiteSpace(c.CutoffLabel), c.DisplayLabel, c.CutoffLabel)))
            Dim payDates = String.Join(", ", plan.Cutoffs.Select(Function(c) EffectivePayDate(c)).Distinct().OrderBy(Function(d) d).Select(Function(d) d.ToString("MMM d, yyyy")))

            ws.Cell(1, 1).Value = PayoutGroup.DisplayName(grp) & " list - " & labels
            ws.Cell(1, 1).Style.Font.Bold = True
            ws.Cell(2, 1).Value = "Pay date: " & payDates

            Dim headers As New List(Of String)()
            headers.Add("Employee No")
            headers.Add("Name")
            If plan.IsCombined Then headers.Add("Cutoff")
            If grp = PayoutGroup.OtherBank Then
                headers.Add("Bank")
                headers.Add("Account No")
            End If
            headers.Add("Net Pay")
            If grp = PayoutGroup.Cash Then headers.Add("Signature")
            If grp = PayoutGroup.Cheque Then headers.Add("Cheque No")

            Dim headerRow = 4
            For i = 0 To headers.Count - 1
                Dim hc = ws.Cell(headerRow, i + 1)
                hc.Value = headers(i)
                hc.Style.Font.Bold = True
                hc.Style.Fill.BackgroundColor = XLColor.LightGray
            Next

            Dim netCol = headers.IndexOf("Net Pay") + 1
            Dim r = headerRow + 1
            For Each p In rows
                Dim col = 1
                ws.Cell(r, col).Style.NumberFormat.Format = "@"
                ws.Cell(r, col).Value = If(p.EmployeeNo, "")
                col += 1
                ws.Cell(r, col).Value = PayeeName(p)
                col += 1
                If plan.IsCombined Then
                    ws.Cell(r, col).Value = If(p.CutoffLabel, "")
                    col += 1
                End If
                If grp = PayoutGroup.OtherBank Then
                    ws.Cell(r, col).Value = If(p.BankName, "")
                    col += 1
                    ws.Cell(r, col).Style.NumberFormat.Format = "@"
                    ws.Cell(r, col).Value = AccountOf(p)
                    col += 1
                End If
                ws.Cell(r, netCol).Value = CDbl(RoundAmount(p.NetPay))
                ws.Cell(r, netCol).Style.NumberFormat.Format = "#,##0.00"
                If grp = PayoutGroup.Cash Then ws.Row(r).Height = 26
                r += 1
            Next

            ' Total
            ws.Cell(r, netCol - 1).Value = "TOTAL"
            ws.Cell(r, netCol - 1).Style.Font.Bold = True
            ws.Cell(r, netCol).Value = CDbl(rows.Sum(Function(x) RoundAmount(x.NetPay)))
            ws.Cell(r, netCol).Style.NumberFormat.Format = "#,##0.00"
            ws.Cell(r, netCol).Style.Font.Bold = True

            Dim table = ws.Range(headerRow, 1, r - 1, headers.Count)
            table.Style.Border.OutsideBorder = XLBorderStyleValues.Thin
            table.Style.Border.InsideBorder = XLBorderStyleValues.Thin

            ws.SheetView.FreezeRows(headerRow)
            ws.Columns().AdjustToContents()
            If grp = PayoutGroup.Cash Then ws.Column(headers.Count).Width = 32
            If grp = PayoutGroup.Cheque Then ws.Column(headers.Count).Width = 20
            ws.PageSetup.FitToPages(1, 0)
        End Sub

    End Module

End Namespace
