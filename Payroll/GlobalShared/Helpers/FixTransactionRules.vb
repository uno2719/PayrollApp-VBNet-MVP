' File: GlobalShared/Helpers/FixTransactionRules.vb
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' PURE LOGIC LANG (walang database, walang UI) ng Employee > Fix Transactions.
    '''
    ''' Kailan papasok ang isang Fix Transaction sa isang cutoff:
    '''   1. Active ang entry.
    '''   2. Ang PAY MONTH ng cutoff (buwan ng Pay Date; kung wala, ng Cutoff End - kapareho ng PayrollComputationRules.PayMonthKey)
    '''      ay nasa loob ng Start ... End (walang End = tuloy-tuloy).
    '''   3. Frequency:
    '''        Every Cycle  = bawat cutoff
    '''        Monthly      = bawat buwan        } sa "Pay On" na cycle lang:
    '''        Quarterly    = bawat 3 buwan       }   1st Cycle  = PeriodInMonth 1
    '''        Semi-Annual  = bawat 6 buwan       }   Last Cycle = PeriodInMonth = bilang ng period ng pay cycle
    '''        Annual       = bawat 12 buwan      }
    '''      Ang bilang ng buwan ay mula sa Start Month (hal. Quarterly na Start Jan = Jan, Apr, Jul, Oct).
    '''
    ''' Laban sa doble: kapag may Input Entry (PIE) ang parehong Category + Code sa cutoff (kahit 0 = "skip"),
    ''' ang Input Entry ang masusunod at HINDI papasok ang Fix (MergeTxns).
    ''' </summary>
    Public Module FixTransactionRules

        Public Const MaxAmount As Decimal = 9999999.99D
        Public Const MaxRemarksLength As Integer = 500
        Public Const MinYear As Integer = 2000
        Public Const MaxYear As Integer = 2100

        Public Function MonthKey(year As Integer, month As Integer) As Integer
            Return year * 100 + month
        End Function

        Private Function MonthIndex(key As Integer) As Integer
            Return (key \ 100) * 12 + (key Mod 100) - 1
        End Function

        ''' <summary>Papasok ba ang Fix Transaction sa cutoff na ito?</summary>
        ''' <param name="payMonthKey">YYYYMM ng pay month ng cutoff (PayrollComputationRules.PayMonthKey).</param>
        ''' <param name="periodInMonth">PeriodInMonth ng cutoff (1 = unang cycle).</param>
        ''' <param name="periodsPerMonth">Bilang ng cycle sa isang buwan ng pay cycle (SemiMonthly = 2).</param>
        Public Function AppliesTo(fix As EmployeeFixTxnModel, payMonthKey As Integer,
                                  periodInMonth As Integer, periodsPerMonth As Integer) As Boolean
            If fix Is Nothing OrElse Not fix.IsActive Then Return False
            If payMonthKey < fix.StartKey Then Return False
            If fix.EndKey.HasValue AndAlso payMonthKey > fix.EndKey.Value Then Return False

            Dim interval = FixTxnFrequency.IntervalMonths(fix.Frequency)
            If interval = 0 Then Return True   ' Every Cycle

            Dim monthsSinceStart = MonthIndex(payMonthKey) - MonthIndex(fix.StartKey)
            If monthsSinceStart Mod interval <> 0 Then Return False

            Dim periods = Math.Max(1, periodsPerMonth)
            If String.Equals(fix.PayOn, FixTxnPayOn.LastCycle, StringComparison.OrdinalIgnoreCase) Then
                Return periodInMonth = periods
            End If
            Return periodInMonth = 1
        End Function

        ''' <summary>Parehong code ba (Category + Code, hindi sensitibo sa laki ng letra).</summary>
        Public Function SameCode(category1 As String, code1 As String, category2 As String, code2 As String) As Boolean
            Return String.Equals(If(category1, "").Trim(), If(category2, "").Trim(), StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(If(code1, "").Trim(), If(code2, "").Trim(), StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>Nagsasapawan ba ang period ng dalawang entry (Start-End, walang End = walang katapusan).</summary>
        Public Function PeriodsOverlap(a As EmployeeFixTxnModel, b As EmployeeFixTxnModel) As Boolean
            Dim aEnd = If(a.EndKey, Integer.MaxValue)
            Dim bEnd = If(b.EndKey, Integer.MaxValue)
            Return a.StartKey <= bEnd AndAlso b.StartKey <= aEnd
        End Function

        ''' <summary>
        ''' Ang mga Input ng isang empleyado para sa cutoff = lahat ng Input Entry (PIE) + ang mga Fix Transaction na tumatama
        ''' sa cutoff na WALANG Input Entry ang parehong code. Ang Fix row ay may FixTxnId (para sa RefId/Remark ng result line).
        ''' </summary>
        Public Function MergeTxns(pie As IEnumerable(Of PayrollTxnInput), fixes As IEnumerable(Of EmployeeFixTxnModel),
                                  recordId As Integer, payMonthKey As Integer,
                                  periodInMonth As Integer, periodsPerMonth As Integer) As List(Of PayrollTxnInput)

            Dim result As New List(Of PayrollTxnInput)()
            If pie IsNot Nothing Then result.AddRange(pie)
            If fixes Is Nothing Then Return result

            For Each fixRow In fixes
                If Not AppliesTo(fixRow, payMonthKey, periodInMonth, periodsPerMonth) Then Continue For

                Dim fx = fixRow
                Dim overridden = result.Any(Function(t) Not t.FixTxnId.HasValue AndAlso
                                                        SameCode(t.TxnCategory, t.TxnCode, fx.TxnCategory, fx.TxnCode))
                If overridden Then Continue For

                Dim isAmount = (fixRow.InputUnit = PayrollInputUnit.Amount)
                result.Add(New PayrollTxnInput With {
                    .RecordId = recordId,
                    .TxnCategory = fixRow.TxnCategory,
                    .TxnCode = fixRow.TxnCode,
                    .Qty = If(isAmount, 1D, fixRow.Amount),
                    .Rate = If(isAmount, fixRow.Amount, 1D),
                    .FixTxnId = fixRow.FixTxnId
                })
            Next

            Return result
        End Function

        ' =============================================
        ' DISPLAY
        ' =============================================

        Public Function MonthName(month As Integer) As String
            If month < 1 OrElse month > 12 Then Return "?"
            Return Globalization.CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(month)
        End Function

        ''' <summary>Hal. "Jan 2026".</summary>
        Public Function PeriodText(year As Integer?, month As Integer?) As String
            If Not year.HasValue OrElse Not month.HasValue Then Return "Ongoing"
            Return $"{MonthName(month.Value)} {year.Value}"
        End Function

        ''' <summary>Hal. "Every Cycle", "Monthly, Last Cycle", "Quarterly (Jan, Apr, Jul, Oct), 1st Cycle".</summary>
        Public Function ScheduleText(fix As EmployeeFixTxnModel) As String
            If fix Is Nothing Then Return String.Empty
            Dim interval = FixTxnFrequency.IntervalMonths(fix.Frequency)
            If interval = 0 Then Return FixTxnFrequency.EveryCycle

            Dim text = fix.Frequency
            If interval > 1 Then
                Dim months As New List(Of String)()
                Dim m = fix.StartMonth
                For i = 1 To 12 \ interval
                    months.Add(MonthName(m))
                    m += interval
                    If m > 12 Then m -= 12
                Next
                text &= $" ({String.Join(", ", months)})"
            End If
            Return $"{text}, {If(fix.PayOn, FixTxnPayOn.FirstCycle)}"
        End Function

        ''' <summary>Hal. "750.00" o "2.00 days".</summary>
        Public Function AmountText(amount As Decimal, unit As PayrollInputUnit) As String
            Select Case unit
                Case PayrollInputUnit.Amount : Return amount.ToString("N2")
                Case PayrollInputUnit.Days : Return $"{amount:N2} days"
                Case PayrollInputUnit.Hours : Return $"{amount:N2} hrs"
                Case Else : Return $"{amount:N2} mins"
            End Select
        End Function

        ' =============================================
        ' VALIDATION (sa Save; ang overlap ay laban sa iba pang entry ng parehong empleyado)
        ' =============================================

        ''' <summary>Empty string = valid.</summary>
        Public Function Validate(fix As EmployeeFixTxnModel, others As IEnumerable(Of EmployeeFixTxnModel)) As String
            If fix Is Nothing Then Return "Nothing to save."
            If fix.RecordId <= 0 Then Return "Select an employee first."
            If String.IsNullOrWhiteSpace(fix.TxnCategory) OrElse String.IsNullOrWhiteSpace(fix.TxnCode) Then
                Return "Select the Transaction Type and Code."
            End If
            If fix.Amount <= 0D Then Return "Amount must be more than 0."
            If fix.Amount > MaxAmount Then Return $"Amount can't be more than {MaxAmount:N2}."

            If fix.StartMonth < 1 OrElse fix.StartMonth > 12 Then Return "Select the Start Month."
            If fix.StartYear < MinYear OrElse fix.StartYear > MaxYear Then Return $"Start Year must be between {MinYear} and {MaxYear}."

            If fix.EndYear.HasValue <> fix.EndMonth.HasValue Then Return "Fill in both End Month and End Year, or leave both blank (ongoing)."
            If fix.EndKey.HasValue Then
                If fix.EndMonth.Value < 1 OrElse fix.EndMonth.Value > 12 Then Return "Select the End Month."
                If fix.EndYear.Value < MinYear OrElse fix.EndYear.Value > MaxYear Then Return $"End Year must be between {MinYear} and {MaxYear}."
                If fix.EndKey.Value < fix.StartKey Then
                    Return $"End ({PeriodText(fix.EndYear, fix.EndMonth)}) can't be before Start ({PeriodText(fix.StartYear, fix.StartMonth)})."
                End If
            End If

            If Not FixTxnFrequency.IsKnown(fix.Frequency) Then Return "Select the Frequency."
            If FixTxnFrequency.IntervalMonths(fix.Frequency) = 0 Then
                fix.PayOn = Nothing
            ElseIf Not FixTxnPayOn.All.Contains(fix.PayOn) Then
                Return "Select Pay On (1st Cycle or Last Cycle)."
            End If

            If If(fix.Remarks, "").Trim().Length > MaxRemarksLength Then Return $"Remarks is too long (maximum {MaxRemarksLength} characters)."

            ' Iisang active na entry lang bawat code sa iisang panahon (kung hindi, doble ang papasok).
            If fix.IsActive AndAlso others IsNot Nothing Then
                Dim clash = others.FirstOrDefault(Function(o) o.FixTxnId <> fix.FixTxnId AndAlso o.IsActive AndAlso
                                                              SameCode(o.TxnCategory, o.TxnCode, fix.TxnCategory, fix.TxnCode) AndAlso
                                                              PeriodsOverlap(o, fix))
                If clash IsNot Nothing Then
                    Return $"This employee already has an active {fix.TxnCode} entry for " &
                           $"{PeriodText(clash.StartYear, clash.StartMonth)} - {PeriodText(clash.EndYear, clash.EndMonth)}. " &
                           "Set an End on that entry (or set it Inactive) first, so the amount is not given twice."
                End If
            End If

            Return String.Empty
        End Function

    End Module

End Namespace
