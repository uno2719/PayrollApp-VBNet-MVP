Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace Payslips.Services

    ''' <summary>
    ''' Pure logic: ginagawang PayslipDocument (ang hugis na kailangan ng rptPayslip) ang hilaw na data ng isang cutoff.
    ''' Walang database at walang UI dito.
    ''' </summary>
    Public Module PayslipDocumentBuilder

        ''' <summary>Pinakamaliit na bilang ng row sa Earnings/Deductions box (para hindi mukhang pipi ang payslip).</summary>
        Public Const MinBodyRows As Integer = 8

        Public Function Build(src As PayslipSource, options As PayslipOptions) As PayslipBatch
            Dim batch As New PayslipBatch With {
                .Cutoff = src.Cutoff,
                .Cutoffs = New List(Of CutoffModel) From {src.Cutoff},
                .Company = If(src.Company, New PayslipCompanyInfo()),
                .IsDraft = src.Cutoff.Status <> CutoffStatus.Posted
            }

            Dim infoById = src.Employees.ToDictionary(Function(e) e.RecordId)
            Dim linesByResult = src.Lines.ToLookup(Function(l) l.ResultId)
            Dim loanLineById = src.LoanLines.ToDictionary(Function(l) l.LineId)
            Dim balancesByEmp = src.LoanBalances.ToLookup(Function(b) b.RecordId)
            Dim ytdById = src.Ytd.ToDictionary(Function(y) y.RecordId)

            For Each h In src.Headers
                Dim info As PayslipEmployeeInfo = Nothing
                infoById.TryGetValue(h.RecordId, info)
                Dim ytd As PayslipYtd = Nothing
                ytdById.TryGetValue(h.RecordId, ytd)

                batch.Documents.Add(BuildOne(src, batch, h, info, linesByResult(h.ResultId).OrderBy(Function(l) l.SeqNo).ToList(),
                                             loanLineById, balancesByEmp(h.RecordId).ToList(), ytd, options))
            Next

            Return batch
        End Function

        Private Function BuildOne(src As PayslipSource, batch As PayslipBatch, h As PayrollResultModel, info As PayslipEmployeeInfo,
                                  lines As List(Of PayrollResultLineModel), loanLineById As Dictionary(Of Integer, PayslipLoanLineInfo),
                                  balances As List(Of PayslipLoanBalance), ytd As PayslipYtd, options As PayslipOptions) As PayslipDocument

            Dim c = src.Cutoff
            Dim payDate = If(c.PayDate.HasValue, c.PayDate.Value, c.CutoffEnd).Date

            Dim doc As New PayslipDocument With {
                .CutoffID = c.CutoffID,
                .RecordId = h.RecordId,
                .IsDraft = batch.IsDraft,
                .CompanyName = batch.Company.CompanyName,
                .CompanyAddress = JoinNonBlank("  |  ", batch.Company.AddressText, batch.Company.ContactText),
                .EmployeeNo = h.EmployeeNo,
                .EmployeeName = h.EmployeeName,
                .Designation = Dash(info?.PositionName),
                .Department = Dash(info?.DepartmentName),
                .Branch = Dash(info?.BranchName),
                .SSSNo = Dash(info?.SSSNo),
                .PhilHealthNo = Dash(info?.PhilHealthNo),
                .PagIBIGNo = Dash(info?.PagIBIGNo),
                .TIN = Dash(info?.TIN),
                .PayPeriod = PeriodText(c.CutoffStart, c.CutoffEnd),
                .PayDate = payDate,
                .PayDateText = payDate.ToString("MMM d, yyyy"),
                .PayCycle = Dash(h.PayCycle),
                .RefNo = "PR-" & c.CutoffID.ToString(),
                .EmailAddress = info?.EmailAddress,
                .BirthDate = info?.BirthDate
            }

            ' ---- Earnings (kaliwa) ----
            Dim left As New List(Of Cell)()
            Dim excluded As Decimal = 0D
            Dim hasAllowanceShown As Boolean = False

            For Each ln In lines.Where(Function(l) l.LineType = ResultLineType.Earning)
                Dim isAllowance = (ln.Category = ResultCategory.Compensation AndAlso
                                   Not String.IsNullOrWhiteSpace(ln.Code) AndAlso src.AllowanceCodes.Contains(ln.Code))
                If isAllowance AndAlso Not options.IncludeAllowance Then
                    excluded += ln.Amount
                    Continue For
                End If
                Dim text = LineText(ln)
                If isAllowance Then
                    text &= " (A)"
                    hasAllowanceShown = True
                End If
                left.Add(New Cell(text, ln.Amount, PayslipCellKind.Line))
            Next

            ' ---- Deductions (kanan): Statutory / Loans / Others ----
            ' Hindi isinasama ang linyang 0 ang halaga; kapag walang natira sa isang group, wala ring group header.
            Dim right As New List(Of Cell)()
            Dim statutory = {
                New Cell("SSS", h.SSSEE + h.SSSMPFEE, PayslipCellKind.GroupLine),
                New Cell("PhilHealth", h.PhilHealthEE, PayslipCellKind.GroupLine),
                New Cell("Pag-IBIG", h.PagIbigEE, PayslipCellKind.GroupLine),
                New Cell("Withholding tax", h.WithholdingTax, PayslipCellKind.GroupLine)
            }.Where(Function(x) x.Amount.GetValueOrDefault() <> 0D).ToList()
            If statutory.Count > 0 Then
                right.Add(New Cell("Statutory", Nothing, PayslipCellKind.GroupHeader))
                right.AddRange(statutory)
            End If

            Dim loanLines = lines.Where(Function(l) l.LineType = ResultLineType.Deduction AndAlso l.Category = ResultCategory.Loan AndAlso l.Amount <> 0D).ToList()
            If loanLines.Count > 0 Then
                right.Add(New Cell("Loans", Nothing, PayslipCellKind.GroupHeader))
                For Each ln In loanLines
                    Dim text = If(String.IsNullOrWhiteSpace(ln.Description), ln.Code, ln.Description)
                    Dim li As PayslipLoanLineInfo = Nothing
                    If loanLineById.TryGetValue(ln.LineId, li) AndAlso li.TotalInstallments > 0 Then
                        text &= $" ({li.InstallmentNo}/{li.TotalInstallments})"
                    End If
                    right.Add(New Cell(text, ln.Amount, PayslipCellKind.GroupLine))
                Next
            End If

            Dim others = lines.Where(Function(l) l.LineType = ResultLineType.Deduction AndAlso l.Category = ResultCategory.Deduction AndAlso l.Amount <> 0D).ToList()
            If others.Count > 0 Then
                right.Add(New Cell("Others", Nothing, PayslipCellKind.GroupHeader))
                For Each ln In others
                    right.Add(New Cell(LineText(ln), ln.Amount, PayslipCellKind.GroupLine))
                Next
            End If

            ' ---- Totals ----
            doc.ExcludedAllowance = excluded
            doc.GrossPay = h.GrossPay - excluded
            doc.TotalDeduction = h.TotalDeduction
            doc.NetPay = h.NetPay - excluded

            ' ---- Pagpapares ng kaliwa at kanan ----
            Dim bodyCount = Math.Max(MinBodyRows, Math.Max(left.Count, right.Count))
            For i = 0 To bodyCount - 1
                Dim l = If(i < left.Count, left(i), New Cell(Nothing, Nothing, PayslipCellKind.Blank))
                Dim r = If(i < right.Count, right(i), New Cell(Nothing, Nothing, PayslipCellKind.Blank))
                doc.Rows.Add(New PayslipPairRow With {
                    .LeftText = l.Text, .LeftAmount = l.Amount, .LeftKind = l.Kind,
                    .RightText = r.Text, .RightAmount = r.Amount, .RightKind = r.Kind})
            Next
            doc.Rows.Add(New PayslipPairRow With {
                .LeftText = "Gross pay", .LeftAmount = doc.GrossPay, .LeftKind = PayslipCellKind.Total,
                .RightText = "Total deductions", .RightAmount = doc.TotalDeduction, .RightKind = PayslipCellKind.Total})
            doc.Rows.Add(New PayslipPairRow With {
                .LeftKind = PayslipCellKind.Outside,
                .RightText = "Net pay", .RightAmount = doc.NetPay, .RightKind = PayslipCellKind.Net})

            ' ---- Loans list ----
            doc.Loans = BuildLoans(batch.IsDraft, balances, loanLines, loanLineById)

            ' ---- Footer ----
            Dim isCash = String.Equals(If(info?.PayBy, "").Trim(), "Cash", StringComparison.OrdinalIgnoreCase)
            Dim footer As New PayslipFooterRow With {
                .ShowYtd = options.ShowYtd,
                .PrintedText = "System-generated payslip. Printed " & DateTime.Now.ToString("MMM d, yyyy h:mm tt") &
                               If(String.IsNullOrWhiteSpace(options.PrintedBy), "", " by " & options.PrintedBy) & "." &
                               If(hasAllowanceShown, "   (A) = allowance", ""),
                .AllowanceNote = If(options.IncludeAllowance, "", "Allowance excluded"),
                .ShowReceivedBy = isCash
            }
            If ytd IsNot Nothing Then
                footer.YtdGross = ytd.Gross - If(options.IncludeAllowance, 0D, ytd.Allowance)
                footer.YtdContributions = ytd.Contributions
                footer.YtdTax = ytd.Tax
            Else
                footer.YtdGross = doc.GrossPay
                footer.YtdContributions = h.SSSEE + h.SSSMPFEE + h.PhilHealthEE + h.PagIbigEE
                footer.YtdTax = h.WithholdingTax
            End If
            doc.Footer.Add(footer)

            Return doc
        End Function

        ''' <summary>
        ''' Balance PAGKATAPOS ng payslip na ito. Kapag Posted na ang cutoff, kasama na sa balance mula sa DB ang bayad nito
        ''' (PaidDate = pay date). Kapag hindi pa (Draft), ibinabawas dito ang LOAN lines ng cutoff.
        ''' </summary>
        Private Function BuildLoans(isDraft As Boolean, balances As List(Of PayslipLoanBalance),
                                   loanLines As List(Of PayrollResultLineModel),
                                   loanLineById As Dictionary(Of Integer, PayslipLoanLineInfo)) As List(Of PayslipLoanRow)

            Dim deductedNow As New Dictionary(Of Integer, Decimal)()
            Dim countNow As New Dictionary(Of Integer, Integer)()
            If isDraft Then
                For Each ln In loanLines
                    Dim li As PayslipLoanLineInfo = Nothing
                    If Not loanLineById.TryGetValue(ln.LineId, li) Then Continue For
                    deductedNow(li.EmployeeLoanId) = If(deductedNow.ContainsKey(li.EmployeeLoanId), deductedNow(li.EmployeeLoanId), 0D) + ln.Amount
                    countNow(li.EmployeeLoanId) = If(countNow.ContainsKey(li.EmployeeLoanId), countNow(li.EmployeeLoanId), 0) + 1
                Next
            End If

            Dim rows As New List(Of PayslipLoanRow)()
            For Each b In balances.OrderBy(Function(x) x.LoanName).ThenBy(Function(x) x.EmployeeLoanId)
                Dim bal = b.Balance
                Dim paid = b.PaidInstallments
                If deductedNow.ContainsKey(b.EmployeeLoanId) Then
                    bal -= deductedNow(b.EmployeeLoanId)
                    paid += countNow(b.EmployeeLoanId)
                End If
                If bal <= 0.005D Then Continue For

                Dim name = b.LoanName
                If String.Equals(b.Status, LoanStatus.Inactive, StringComparison.OrdinalIgnoreCase) Then name &= " (on hold)"

                rows.Add(New PayslipLoanRow With {
                    .LoanName = name,
                    .Progress = $"{Math.Min(paid, b.TotalInstallments)} of {b.TotalInstallments}",
                    .Balance = bal})
            Next

            If rows.Count = 0 Then rows.Add(New PayslipLoanRow With {.LoanName = "N/A", .Progress = "", .Balance = Nothing})
            Return rows
        End Function

        ' ---------------------------------------------
        ' Helpers
        ' ---------------------------------------------

        Private Class Cell
            Public ReadOnly Text As String
            Public ReadOnly Amount As Decimal?
            Public ReadOnly Kind As PayslipCellKind
            Public Sub New(text As String, amount As Decimal?, kind As PayslipCellKind)
                Me.Text = text
                Me.Amount = amount
                Me.Kind = kind
            End Sub
        End Class

        ''' <summary>Description + dami kung oras/araw/minuto, hal. "Regular OT (5.00 hrs)".</summary>
        Public Function LineText(ln As PayrollResultLineModel) As String
            Dim desc = If(String.IsNullOrWhiteSpace(ln.Description), ln.Code, ln.Description.Trim())
            Dim unit = If(ln.Unit, "").Trim()
            Dim suffix As String = Nothing
            Select Case unit.ToUpperInvariant()
                Case "HOURS" : suffix = "hrs"
                Case "MINUTES" : suffix = "mins"
                Case "DAYS" : suffix = If(ln.Qty = 1D, "day", "days")
            End Select
            If suffix Is Nothing OrElse ln.Qty = 0D Then Return desc
            Return $"{desc} ({ln.Qty:#,##0.##} {suffix})"
        End Function

        Public Function PeriodText(startDate As Date, endDate As Date) As String
            If startDate.Year <> endDate.Year Then
                Return startDate.ToString("MMM d, yyyy") & " - " & endDate.ToString("MMM d, yyyy")
            End If
            If startDate.Month = endDate.Month Then
                Return startDate.ToString("MMM d") & "-" & endDate.ToString("d, yyyy")
            End If
            Return startDate.ToString("MMM d") & " - " & endDate.ToString("MMM d, yyyy")
        End Function

        Private Function Dash(value As String) As String
            Return If(String.IsNullOrWhiteSpace(value), "-", value.Trim())
        End Function

        Private Function JoinNonBlank(separator As String, ParamArray parts As String()) As String
            Return String.Join(separator, parts.Where(Function(p) Not String.IsNullOrWhiteSpace(p)))
        End Function

    End Module

End Namespace
