' File: GlobalShared/Helpers/PayrollComputationRules.vb
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' PURE LOGIC LANG (walang database, walang UI): ang computation ng payroll ng isang cutoff.
    ''' Ang PayrollProcessService ang nagkakarga ng PayrollComputationContext; ang module na ito ang nag-compute
    ''' at nagbabalik ng PayrollEmployeeResult (header + mga linya + bracket snapshot + babala).
    '''
    ''' Pagkakasunod bawat employee:
    '''   1. Basic pay            = Basic Salary / bilang ng cutoff kada buwan
    '''   2. Input Entry          = Hours/Minutes/Days x rate ng employee (x Rate ng OT/Holiday), o ang peso mismo (Amount)
    '''   3. Bases                = flagged na kita MINUS flagged na manual deduction (Tax / SSS / PhilHealth / Pag-IBIG)
    '''   4. SSS, PhilHealth, Pag-IBIG (buwanang halaga mula sa bracket, hinati ayon sa Deduction Schedule)
    '''   5. Tax                  = (Tax gross - EE contributions na kinaltas sa cutoff na ito) sa income tax table
    '''   6. Loans                = mga hulog na due sa cutoff na ito
    '''   7. Net pay
    ''' Lahat ng linya ay 2 decimals (Round2).
    ''' </summary>
    Public Module PayrollComputationRules

        Public Const Decimals As Integer = 2

        Private Const SssTable As String = "tblStatutorySSS"
        Private Const PhilHealthTable As String = "tblStatutoryPhilHealth"
        Private Const PagIbigTable As String = "tblStatutoryPagIbig"

        ' =============================================
        ' MALILIIT NA HELPER
        ' =============================================

        Public Function Round2(value As Decimal) As Decimal
            Return Math.Round(value, Decimals, MidpointRounding.AwayFromZero)
        End Function

        ''' <summary>Susi ng catalog dictionary: "OVERTIME|OT1" (hindi sensitibo sa laki ng letra).</summary>
        Public Function CatalogKey(category As String, code As String) As String
            Return If(category, "").Trim().ToUpperInvariant() & "|" & If(code, "").Trim().ToUpperInvariant()
        End Function

        ''' <summary>
        ''' Aling buwan ng sahod ang cutoff: batay sa PayDate (kung wala, sa CutoffEnd). Ang numerong YYYYMM ay ginagamit
        ''' para hanapin ang mga naunang cutoff sa parehong pay month (para sa SSS base ng buong buwan).
        ''' </summary>
        Public Function PayMonthKey(c As CutoffModel) As Integer
            Dim d = If(c.PayDate.HasValue, c.PayDate.Value, c.CutoffEnd)
            Return d.Year * 100 + d.Month
        End Function

        ''' <summary>"SemiMonthly" -> "tblIncomeTaxSemiMonthly" (Nothing kung walang ganoong tax table).</summary>
        Public Function TaxTableNameFor(cycleType As String) As String
            Dim name = "tblIncomeTax" & PayCycleChoices.Normalize(cycleType)
            Return If(IncomeTaxTableRegistry.IsAllowed(name), name, Nothing)
        End Function

        Public Function UsesFixedBasic(basedOn As String) As Boolean
            Return Not String.IsNullOrWhiteSpace(basedOn) AndAlso
                   basedOn.IndexOf("Fixed", StringComparison.OrdinalIgnoreCase) >= 0
        End Function

        ''' <summary>
        ''' "Fixed Basic" na buwanang katumbas: Basic Salary; para sa Daily-rate employee ay Daily Rate x Daily Rate Monthly Factor.
        ''' </summary>
        Public Function MonthlyBasicEquivalent(ctx As PayrollComputationContext, emp As PayrollEmployeeInput) As Decimal
            If ctx.PayCycle.RateBasis = PayRateBasis.DailyRate Then
                Return Round2(emp.DailyRate.GetValueOrDefault() * ctx.Settings.DailyRateMonthlyFactor)
            End If
            Return emp.BasicSalary.GetValueOrDefault()
        End Function

        ''' <summary>
        ''' Rate kada unit mula sa employee record: Hours = Hourly Rate, Minutes = Hourly Rate / 60, Days = Daily Rate.
        ''' Nothing kung walang rate (o Amount ang unit - walang rate na kailangan).
        ''' </summary>
        Public Function PerUnitRate(unit As PayrollInputUnit, emp As PayrollEmployeeInput) As Decimal?
            Select Case unit
                Case PayrollInputUnit.Hours
                    If emp.HourlyRate.HasValue AndAlso emp.HourlyRate.Value > 0D Then Return emp.HourlyRate.Value
                Case PayrollInputUnit.Minutes
                    If emp.HourlyRate.HasValue AndAlso emp.HourlyRate.Value > 0D Then Return emp.HourlyRate.Value / 60D
                Case PayrollInputUnit.Days
                    If emp.DailyRate.HasValue AndAlso emp.DailyRate.Value > 0D Then Return emp.DailyRate.Value
            End Select
            Return Nothing
        End Function

        ''' <summary>
        ''' Halaga ng Hours/Minutes/Days entry: Qty x rate ng employee x multiplier (Rate ng OT/Holiday; 1 sa iba).
        ''' Minutes = Hourly Rate / 60 - hinahati sa DULO para walang naliligaw na sentimo sa rounding.
        ''' Nothing kung walang rate ang employee.
        ''' </summary>
        Public Function QuantityValue(unit As PayrollInputUnit, qty As Decimal, multiplier As Decimal, emp As PayrollEmployeeInput) As Decimal?
            If Not PerUnitRate(unit, emp).HasValue Then Return Nothing

            Select Case unit
                Case PayrollInputUnit.Hours
                    Return Round2(qty * emp.HourlyRate.Value * multiplier)
                Case PayrollInputUnit.Minutes
                    Return Round2(qty * emp.HourlyRate.Value * multiplier / 60D)
                Case Else
                    Return Round2(qty * emp.DailyRate.Value * multiplier)
            End Select
        End Function

        Public Function SeparationDateOf(emp As PayrollEmployeeInput) As Date?
            Dim result As Date? = Nothing
            For Each d In {emp.DateResigned, emp.DateRetired, emp.DateTerminated}
                If d.HasValue AndAlso (Not result.HasValue OrElse d.Value.Date < result.Value) Then
                    result = d.Value.Date
                End If
            Next
            Return result
        End Function

        ' =============================================
        ' BRACKET LOOKUP
        ' =============================================

        ''' <summary>
        ''' Bracket kung nasaan ang base (SalaryFrom - SalaryTo). Kung may puwang sa pagitan ng mga bracket (hal. 3249.99 at 3250.00),
        ''' kinukuha ang bracket na may pinakamataas na SalaryFrom na hindi lumalampas sa base. Nothing kung mas mababa sa lahat.
        ''' </summary>
        Public Function FindStatutoryBracket(rows As IEnumerable(Of StatutoryBracketModel), base As Decimal) As StatutoryBracketModel
            If rows Is Nothing Then Return Nothing

            Dim active = rows.Where(Function(b) b.IsActive).OrderBy(Function(b) b.SalaryFrom).ToList()

            For Each b In active
                If base >= b.SalaryFrom AndAlso base <= b.SalaryTo Then Return b
            Next

            Return active.Where(Function(b) b.SalaryFrom <= base).LastOrDefault()
        End Function

        Public Function FindTaxBracket(rows As IEnumerable(Of IncomeTaxBracketModel), taxable As Decimal) As IncomeTaxBracketModel
            If rows Is Nothing Then Return Nothing

            Dim active = rows.Where(Function(b) b.IsActive).OrderBy(Function(b) b.SalaryFrom).ToList()

            For Each b In active
                If taxable >= b.SalaryFrom AndAlso taxable <= b.SalaryTo Then Return b
            Next

            Return active.Where(Function(b) b.SalaryFrom <= taxable).LastOrDefault()
        End Function

        ''' <summary>"Percentage" = base x share / 100; ang iba ("Amount") = ang share mismo.</summary>
        Public Function ContributionAmount(contriType As String, share As Decimal, base As Decimal) As Decimal
            If String.Equals(If(contriType, "").Trim(), "Percentage", StringComparison.OrdinalIgnoreCase) Then
                Return Round2(base * share / 100D)
            End If
            Return Round2(share)
        End Function

        ''' <summary>Tax = Fix Tax Amount + Tax % x (taxable - Salary From), 2 decimals.</summary>
        Public Function TaxFor(bracket As IncomeTaxBracketModel, taxable As Decimal) As Decimal
            If bracket Is Nothing Then Return 0D
            Return Round2(bracket.FixTaxAmount + bracket.TaxPercentage / 100D * (taxable - bracket.SalaryFrom))
        End Function

        ''' <summary>Buwanang Pag-IBIG employee share: table amount, o ang voluntary amount ayon sa mode (default: ang voluntary ang PAPALIT).</summary>
        Public Function PagIbigEmployeeMonthly(tableEE As Decimal, voluntary As Decimal?, mode As PagIbigVoluntaryMode) As Decimal
            If Not voluntary.HasValue OrElse voluntary.Value <= 0D Then Return tableEE

            Select Case mode
                Case PagIbigVoluntaryMode.AddOnTop
                    Return Round2(tableEE + voluntary.Value)
                Case PagIbigVoluntaryMode.HigherOfTableOrVoluntary
                    Return Math.Max(tableEE, Round2(voluntary.Value))
                Case Else
                    Return Round2(voluntary.Value)
            End Select
        End Function

        ''' <summary>Share % ng bawat period ng pay cycle para sa isang agency. missing = True kung may period na walang naka-set na row.</summary>
        Public Function PeriodShares(schedule As IEnumerable(Of StatutoryScheduleModel), cycleType As String,
                                     agency As String, periods As Integer, ByRef missing As Boolean) As List(Of Decimal)
            Dim shares As New List(Of Decimal)()
            missing = False
            For p = 1 To periods
                Dim share = StatutoryScheduleRules.ShareFor(schedule, cycleType, agency, p)
                If Not share.HasValue Then missing = True
                shares.Add(share.GetValueOrDefault())
            Next
            Return shares
        End Function

        Public Function VoluntaryModeOf(schedule As IEnumerable(Of StatutoryScheduleModel), cycleType As String) As PagIbigVoluntaryMode
            Dim cycleKey = PayCycleChoices.Normalize(cycleType)
            For Each r In schedule
                If String.Equals(r.Agency, StatutoryAgency.PagIbig, StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(PayCycleChoices.Normalize(r.PayCycleType), cycleKey, StringComparison.OrdinalIgnoreCase) AndAlso
                   r.VoluntaryMode.HasValue Then
                    Return CType(r.VoluntaryMode.Value, PagIbigVoluntaryMode)
                End If
            Next
            Return PagIbigVoluntaryMode.ReplaceTable
        End Function

        ' =============================================
        ' COMPUTATION
        ' =============================================

        Public Function ComputeAll(ctx As PayrollComputationContext) As List(Of PayrollEmployeeResult)
            Dim results As New List(Of PayrollEmployeeResult)()
            For Each emp In ctx.Employees.OrderBy(Function(e) e.EmployeeName).ThenBy(Function(e) e.EmployeeNo)
                results.Add(ComputeEmployee(ctx, emp))
            Next
            Return results
        End Function

        Private Class MonthlyContribution
            Public Property Bracket As StatutoryBracketModel
            Public Property EE As Decimal
            Public Property ER As Decimal
            Public Property ECC As Decimal
            Public Property EEMPF As Decimal
            Public Property ERMPF As Decimal
        End Class

        Private Function MonthlyFor(brackets As IEnumerable(Of StatutoryBracketModel), lookupBase As Decimal) As MonthlyContribution
            Dim b = FindStatutoryBracket(brackets, lookupBase)
            If b Is Nothing Then Return Nothing

            Return New MonthlyContribution With {
                .Bracket = b,
                .EE = ContributionAmount(b.EEContriType, b.EEShare, lookupBase),
                .ER = ContributionAmount(b.ERContriType, b.ERShare, lookupBase),
                .ECC = Round2(b.ECCAmount),
                .EEMPF = Round2(b.EEMPF),
                .ERMPF = Round2(b.ERMPF)
            }
        End Function

        Private Function AddLine(res As PayrollEmployeeResult, lineType As String, category As String, code As String,
                                 description As String, qty As Decimal, unit As String, rateUsed As Decimal,
                                 amount As Decimal) As PayrollResultLineModel
            Dim ln As New PayrollResultLineModel With {
                .SeqNo = res.Lines.Count + 1,
                .RecordId = res.Header.RecordId,
                .CutoffID = res.Header.CutoffID,
                .LineType = lineType,
                .Category = category,
                .Code = code,
                .Description = description,
                .Qty = qty,
                .Unit = unit,
                .RateUsed = rateUsed,
                .Amount = amount
            }
            res.Lines.Add(ln)
            Return ln
        End Function

        Private Sub SetFlags(ln As PayrollResultLineModel, tax As Boolean, sss As Boolean, philHealth As Boolean, pagIbig As Boolean)
            ln.TaxFlag = tax
            ln.SSSFlag = sss
            ln.PhilHealthFlag = philHealth
            ln.PagIbigFlag = pagIbig
        End Sub

        Public Function ComputeEmployee(ctx As PayrollComputationContext, emp As PayrollEmployeeInput) As PayrollEmployeeResult
            Dim res As New PayrollEmployeeResult()
            Dim h = res.Header
            Dim cutoff = ctx.Cutoff
            Dim periods = Math.Max(1, ctx.PayCycle.PeriodsPerMonth)
            Dim periodNo = cutoff.PeriodInMonth.GetValueOrDefault(1)
            Dim cycleType = cutoff.CycleType

            ' ---- Snapshot ng employee at rates ----
            h.CutoffID = cutoff.CutoffID
            h.RecordId = emp.RecordId
            h.EmployeeNo = emp.EmployeeNo
            h.EmployeeName = emp.EmployeeName
            h.PayCycle = PayCycleChoices.Normalize(cycleType)
            h.RateBasis = CByte(ctx.PayCycle.RateBasis)
            h.PeriodInMonth = cutoff.PeriodInMonth
            h.BasicSalary = emp.BasicSalary
            h.DailyRate = emp.DailyRate
            h.HourlyRate = emp.HourlyRate
            h.DaysInYear = emp.DaysInYear
            h.WorkHourPer = emp.WorkHourPer
            h.DailyRateMonthlyFactor = ctx.Settings.DailyRateMonthlyFactor
            h.IsMinimumWage = emp.MinimumWage

            ' ---- Babala: bagong hire / umalis sa gitna ng cutoff (walang proration) ----
            If emp.DateJoined.HasValue AndAlso emp.DateJoined.Value.Date > cutoff.CutoffStart.Date Then
                res.Warnings.Add($"Joined {emp.DateJoined.Value:MMM d, yyyy} (mid-cutoff): full pay computed, no proration. Adjust with a deduction if needed.")
            End If
            Dim separated = SeparationDateOf(emp)
            If separated.HasValue AndAlso separated.Value.Date <= cutoff.CutoffEnd.Date Then
                res.Warnings.Add($"Separated {separated.Value:MMM d, yyyy}: full pay computed, no proration. Adjust with a deduction if needed.")
            End If

            ' ---- 1. Basic pay ----
            Dim basicSalary = emp.BasicSalary.GetValueOrDefault()
            Dim basicPay = Round2(basicSalary / CDec(periods))
            Dim basicLine = AddLine(res, ResultLineType.Earning, ResultCategory.Basic, ctx.BasicCode.Code, ctx.BasicCode.Description,
                                    1D, "Period", basicSalary, basicPay)
            SetFlags(basicLine, ctx.BasicCode.TaxFlag, ctx.BasicCode.SSSFlag, ctx.BasicCode.PhilHealthFlag, ctx.BasicCode.PagIbigFlag)
            h.BasicPay = basicPay

            ' ---- 2. Input Entry (Overtime/Holiday/Compensation/Bonus/Deduction) ----
            For Each t In emp.Txns
                Dim category = If(t.TxnCategory, "").Trim().ToUpperInvariant()
                If category = "CORE" Then Continue For          ' hal. DAYSWORKED - para sa Daily (susunod na delivery)

                Dim item As PayrollCatalogItem = Nothing
                If Not ctx.Catalog.TryGetValue(CatalogKey(category, t.TxnCode), item) Then
                    res.Warnings.Add($"Input '{t.TxnCode}' ({category}) is not in the catalog anymore - skipped.")
                    Continue For
                End If
                If Not item.IsActive Then
                    res.Warnings.Add($"Input '{item.Code}' is inactive in Payroll Settings but has an entry - still computed.")
                End If

                Dim amount As Decimal
                Dim qty As Decimal
                Dim rateUsed As Decimal
                Dim unitText = PayrollInputUnits.Text(item.InputUnit)

                If item.InputUnit = PayrollInputUnit.Amount Then
                    ' Amount: ang peso ang naka-save sa Rate (Qty = 1)
                    qty = t.Qty
                    rateUsed = t.Rate
                    amount = Round2(t.Qty * t.Rate)
                Else
                    Dim unitRate = PerUnitRate(item.InputUnit, emp)
                    If Not unitRate.HasValue Then
                        Dim needs = If(item.InputUnit = PayrollInputUnit.Days, "Daily Rate", "Hourly Rate")
                        res.Warnings.Add($"{item.Description}: employee has no {needs} (Employee > Earnings) - skipped.")
                        Continue For
                    End If
                    If item.Multiplier <= 0D AndAlso (category = ResultCategory.Overtime OrElse category = ResultCategory.Holiday) Then
                        res.Warnings.Add($"{item.Description}: Rate in Payroll Settings is 0 - computed as 0.")
                    End If
                    qty = t.Qty
                    rateUsed = unitRate.Value * item.Multiplier
                    amount = QuantityValue(item.InputUnit, qty, item.Multiplier, emp).Value
                End If

                If amount = 0D Then Continue For

                Dim isDeduction = (category = ResultCategory.Deduction)
                Dim ln = AddLine(res, If(isDeduction, ResultLineType.Deduction, ResultLineType.Earning), category,
                                 item.Code, item.Description, qty, unitText, rateUsed, amount)
                SetFlags(ln, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag)
            Next

            ' ---- 3. Totals at bases ----
            Dim gross As Decimal = 0D
            Dim manualDeduction As Decimal = 0D
            Dim taxBase As Decimal = 0D, sssBase As Decimal = 0D, phBase As Decimal = 0D, piBase As Decimal = 0D

            For Each ln In res.Lines
                Dim sign As Decimal = 0D
                If ln.LineType = ResultLineType.Earning Then
                    sign = 1D
                    gross += ln.Amount
                ElseIf ln.LineType = ResultLineType.Deduction AndAlso ln.Category = ResultCategory.Deduction Then
                    sign = -1D
                    manualDeduction += ln.Amount
                End If

                If sign <> 0D Then
                    If ln.TaxFlag Then taxBase += sign * ln.Amount
                    If ln.SSSFlag Then sssBase += sign * ln.Amount
                    If ln.PhilHealthFlag Then phBase += sign * ln.Amount
                    If ln.PagIbigFlag Then piBase += sign * ln.Amount
                End If
            Next

            h.GrossPay = gross
            h.OtherDeduction = manualDeduction
            h.TaxGross = Math.Max(0D, taxBase)
            h.SSSBase = Math.Max(0D, sssBase)
            h.PhilHealthBase = Math.Max(0D, phBase)
            h.PagIbigBase = Math.Max(0D, piBase)

            ' ---- 4. Statutory ----
            Dim fixedBasic = MonthlyBasicEquivalent(ctx, emp)

            ComputeSss(ctx, emp, res, periods, periodNo, fixedBasic)
            ComputePhilHealth(ctx, emp, res, periods, periodNo, fixedBasic)
            ComputePagIbig(ctx, emp, res, periods, periodNo, fixedBasic)

            Dim contributionEE = h.SSSEE + h.SSSMPFEE + h.PhilHealthEE + h.PagIbigEE

            ' ---- 5. Tax ----
            h.TaxableIncome = Math.Max(0D, h.TaxGross - contributionEE)

            If emp.MinimumWage Then
                h.WithholdingTax = 0D
            ElseIf ctx.TaxBrackets Is Nothing OrElse ctx.TaxBrackets.Count = 0 Then
                res.Warnings.Add($"No income tax table rows ({ctx.TaxTableName}) - tax not computed.")
            Else
                Dim tb = FindTaxBracket(ctx.TaxBrackets, h.TaxableIncome)
                If tb Is Nothing Then
                    res.Warnings.Add($"No tax bracket found for taxable income {h.TaxableIncome:N2} - tax not computed.")
                Else
                    h.WithholdingTax = TaxFor(tb, h.TaxableIncome)
                    res.Brackets.Add(New PayrollResultBracketModel With {
                        .Agency = ResultCategory.Tax, .SourceTable = ctx.TaxTableName, .SourceId = tb.Id,
                        .BasisAmount = h.TaxableIncome, .SalaryFrom = tb.SalaryFrom, .SalaryTo = tb.SalaryTo,
                        .TaxPercentage = tb.TaxPercentage, .FixTaxAmount = tb.FixTaxAmount})
                    If h.WithholdingTax <> 0D Then
                        AddLine(res, ResultLineType.Deduction, ResultCategory.Tax, "TAX", "Withholding Tax",
                                1D, "", h.TaxableIncome, h.WithholdingTax)
                    End If
                End If
            End If

            ' ---- 6. Loans ----
            Dim overdue = 0
            For Each l In emp.Loans
                If l.IsOverdue Then
                    overdue += 1
                    Continue For
                End If
                If l.BalanceDue <= 0D Then Continue For

                Dim amount = Round2(l.BalanceDue)
                Dim ln = AddLine(res, ResultLineType.Deduction, ResultCategory.Loan, l.LoanCode,
                                 If(String.IsNullOrWhiteSpace(l.LoanDescription), l.LoanCode, l.LoanDescription),
                                 1D, "Installment", amount, amount)
                ln.RefId = l.ScheduleId
                ln.Remark = $"Installment #{l.InstallmentNo}, due {l.ScheduledDate:MMM d, yyyy}"
                h.LoanDeduction += amount
            Next
            If overdue > 0 Then
                res.Warnings.Add($"{overdue} overdue loan installment(s) were NOT deducted automatically (Employee > Loans).")
            End If

            ' ---- 7. Net ----
            h.TotalDeduction = h.OtherDeduction + contributionEE + h.WithholdingTax + h.LoanDeduction
            h.NetPay = h.GrossPay - h.TotalDeduction
            If h.NetPay < 0D Then
                res.Warnings.Add($"Net pay is negative ({h.NetPay:N2}).")
            End If

            h.HasWarnings = res.Warnings.Count > 0
            If h.HasWarnings Then
                Dim text = String.Join(" | ", res.Warnings)
                h.WarningText = If(text.Length > 1000, text.Substring(0, 997) & "...", text)
            End If

            Return res
        End Function

        ' ---------------------------------------------
        ' SSS: base = SSS-flagged na kita ng BUONG pay month (mga naunang cutoff + ang cutoff na ito)
        ' ---------------------------------------------
        Private Sub ComputeSss(ctx As PayrollComputationContext, emp As PayrollEmployeeInput, res As PayrollEmployeeResult,
                               periods As Integer, periodNo As Integer, fixedBasic As Decimal)
            Dim h = res.Header
            Dim missing As Boolean
            Dim shares = PeriodShares(ctx.Schedule, ctx.Cutoff.CycleType, StatutoryAgency.SSS, periods, missing)
            If missing Then res.Warnings.Add("Statutory Deduction Schedule is incomplete for SSS.")

            Dim lookup = If(UsesFixedBasic(ctx.Settings.SSSBasedOn), fixedBasic, emp.Prior.SSSBase + h.SSSBase)
            h.SSSLookupBase = lookup

            Dim share = If(periodNo >= 1 AndAlso periodNo <= shares.Count, shares(periodNo - 1), 0D)
            If share <= 0D Then Return          ' hindi kinakaltas sa period na ito

            If periodNo > 1 AndAlso Not UsesFixedBasic(ctx.Settings.SSSBasedOn) AndAlso emp.Prior.SSSBase = 0D Then
                res.Warnings.Add("No SSS base from earlier cutoff(s) of this pay month (not processed here, Closed, or employee joined later) - SSS uses this cutoff only.")
            End If

            Dim m = MonthlyFor(ctx.SssBrackets, lookup)
            If m Is Nothing Then
                res.Warnings.Add($"No SSS bracket for base {lookup:N2} - SSS not deducted.")
                Return
            End If

            h.SSSEE = StatutoryScheduleRules.AmountForPeriod(m.EE, shares, periodNo, Decimals)
            h.SSSMPFEE = StatutoryScheduleRules.AmountForPeriod(m.EEMPF, shares, periodNo, Decimals)
            h.SSSER = StatutoryScheduleRules.AmountForPeriod(m.ER, shares, periodNo, Decimals)
            h.SSSMPFER = StatutoryScheduleRules.AmountForPeriod(m.ERMPF, shares, periodNo, Decimals)
            h.ECC = StatutoryScheduleRules.AmountForPeriod(m.ECC, shares, periodNo, Decimals)

            EmitContribution(res, ResultCategory.SSS, "SSS", "SSS Employee Share", h.SSSEE, ResultLineType.Deduction)
            EmitContribution(res, ResultCategory.SSS, "SSS-MPF", "SSS MPF (Employee)", h.SSSMPFEE, ResultLineType.Deduction)
            EmitContribution(res, ResultCategory.SSS, "SSS-ER", "SSS Employer Share", h.SSSER, ResultLineType.Employer)
            EmitContribution(res, ResultCategory.SSS, "SSS-MPF-ER", "SSS MPF (Employer)", h.SSSMPFER, ResultLineType.Employer)
            EmitContribution(res, ResultCategory.SSS, "ECC", "Employees' Compensation (ECC)", h.ECC, ResultLineType.Employer)

            res.Brackets.Add(SnapStatutory(StatutoryAgency.SSS, SssTable, m, lookup, m.EE, m.ER, share))
        End Sub

        ' ---------------------------------------------
        ' PhilHealth: Fixed Basic (default) o Progressive Earnings ayon sa General Settings
        ' ---------------------------------------------
        Private Sub ComputePhilHealth(ctx As PayrollComputationContext, emp As PayrollEmployeeInput, res As PayrollEmployeeResult,
                                      periods As Integer, periodNo As Integer, fixedBasic As Decimal)
            Dim h = res.Header
            Dim missing As Boolean
            Dim shares = PeriodShares(ctx.Schedule, ctx.Cutoff.CycleType, StatutoryAgency.PhilHealth, periods, missing)
            If missing Then res.Warnings.Add("Statutory Deduction Schedule is incomplete for PhilHealth.")

            Dim lookup = If(UsesFixedBasic(ctx.Settings.PhilHealthBasedOn), fixedBasic, emp.Prior.PhilHealthBase + h.PhilHealthBase)
            h.PhilHealthLookupBase = lookup

            Dim share = If(periodNo >= 1 AndAlso periodNo <= shares.Count, shares(periodNo - 1), 0D)
            If share <= 0D Then Return

            Dim m = MonthlyFor(ctx.PhilHealthBrackets, lookup)
            If m Is Nothing Then
                res.Warnings.Add($"No PhilHealth bracket for base {lookup:N2} - PhilHealth not deducted.")
                Return
            End If

            h.PhilHealthEE = StatutoryScheduleRules.AmountForPeriod(m.EE, shares, periodNo, Decimals)
            h.PhilHealthER = StatutoryScheduleRules.AmountForPeriod(m.ER, shares, periodNo, Decimals)

            EmitContribution(res, ResultCategory.PhilHealth, "PHIC", "PhilHealth Employee Share", h.PhilHealthEE, ResultLineType.Deduction)
            EmitContribution(res, ResultCategory.PhilHealth, "PHIC-ER", "PhilHealth Employer Share", h.PhilHealthER, ResultLineType.Employer)

            res.Brackets.Add(SnapStatutory(StatutoryAgency.PhilHealth, PhilHealthTable, m, lookup, m.EE, m.ER, share))
        End Sub

        ' ---------------------------------------------
        ' Pag-IBIG: buwanang halaga mula sa table; ang voluntary amount ang PAPALIT sa employee share (ayon sa mode)
        ' Ang lookup base ay ang buwanang Basic (hindi ang flagged na kita) - tingnan ang assumption sa delivery notes.
        ' ---------------------------------------------
        Private Sub ComputePagIbig(ctx As PayrollComputationContext, emp As PayrollEmployeeInput, res As PayrollEmployeeResult,
                                   periods As Integer, periodNo As Integer, fixedBasic As Decimal)
            Dim h = res.Header
            Dim missing As Boolean
            Dim shares = PeriodShares(ctx.Schedule, ctx.Cutoff.CycleType, StatutoryAgency.PagIbig, periods, missing)
            If missing Then res.Warnings.Add("Statutory Deduction Schedule is incomplete for Pag-IBIG.")

            h.PagIbigLookupBase = fixedBasic

            Dim share = If(periodNo >= 1 AndAlso periodNo <= shares.Count, shares(periodNo - 1), 0D)
            If share <= 0D Then Return

            Dim m = MonthlyFor(ctx.PagIbigBrackets, fixedBasic)
            If m Is Nothing Then
                res.Warnings.Add($"No Pag-IBIG bracket for base {fixedBasic:N2} - Pag-IBIG not deducted.")
                Return
            End If

            Dim mode = VoluntaryModeOf(ctx.Schedule, ctx.Cutoff.CycleType)
            Dim monthlyEE = PagIbigEmployeeMonthly(m.EE, emp.PagIBIGVoluntary, mode)

            h.PagIbigEE = StatutoryScheduleRules.AmountForPeriod(monthlyEE, shares, periodNo, Decimals)
            h.PagIbigER = StatutoryScheduleRules.AmountForPeriod(m.ER, shares, periodNo, Decimals)

            Dim eeLine = EmitContribution(res, ResultCategory.PagIbig, "PAGIBIG", "Pag-IBIG Employee Share", h.PagIbigEE, ResultLineType.Deduction)
            If eeLine IsNot Nothing AndAlso emp.PagIBIGVoluntary.HasValue AndAlso emp.PagIBIGVoluntary.Value > 0D Then
                eeLine.Remark = $"Voluntary {emp.PagIBIGVoluntary.Value:N2} (table {m.EE:N2})"
            End If
            EmitContribution(res, ResultCategory.PagIbig, "PAGIBIG-ER", "Pag-IBIG Employer Share", h.PagIbigER, ResultLineType.Employer)

            res.Brackets.Add(SnapStatutory(StatutoryAgency.PagIbig, PagIbigTable, m, fixedBasic, monthlyEE, m.ER, share))
        End Sub

        Private Function EmitContribution(res As PayrollEmployeeResult, category As String, code As String,
                                          description As String, amount As Decimal, lineType As String) As PayrollResultLineModel
            If amount = 0D Then Return Nothing
            Return AddLine(res, lineType, category, code, description, 1D, "", amount, amount)
        End Function

        Private Function SnapStatutory(agency As String, sourceTable As String, m As MonthlyContribution,
                                       basis As Decimal, monthlyEE As Decimal, monthlyER As Decimal,
                                       sharePercent As Decimal) As PayrollResultBracketModel
            Dim b = m.Bracket
            Return New PayrollResultBracketModel With {
                .Agency = agency, .SourceTable = sourceTable, .SourceId = b.Id,
                .BasisAmount = basis, .SalaryFrom = b.SalaryFrom, .SalaryTo = b.SalaryTo,
                .EEShare = b.EEShare, .EEContriType = b.EEContriType,
                .ERShare = b.ERShare, .ERContriType = b.ERContriType,
                .ECCAmount = b.ECCAmount, .EEMPF = b.EEMPF, .ERMPF = b.ERMPF,
                .MonthlyEE = monthlyEE, .MonthlyER = monthlyER, .SharePercent = sharePercent
            }
        End Function

    End Module

End Namespace
