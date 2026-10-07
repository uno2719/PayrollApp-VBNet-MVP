' File: GlobalShared/Helpers/PayCyclePatternHelper.vb
' ============================================================
' PURE LOGIC LANG ITO - walang database, walang UI. Dito nanggagaling
' ang lahat ng date computation ng cutoff pattern, para ISA LANG ang
' source of truth: ginagamit ng Cutoff generation, ng preview sa
' Pay Cycle Settings, at ng validation bago i-save ang pattern.
' ============================================================
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>Isang cutoff na lumabas mula sa pattern (wala pang CutoffID/CycleType).</summary>
    Public Class PayCyclePeriodInstance
        Public Property PeriodNo As Integer          ' 1..n sa loob ng taon (ayon sa petsa)
        Public Property PatternRowNo As Integer      ' alin sa pattern rows ang pinanggalingan
        Public Property CutoffStart As Date
        Public Property CutoffEnd As Date
        Public Property PayDate As Date
    End Class

    Public Module PayCyclePatternHelper

        ''' <summary>
        ''' Petsa para sa (pay month + offset, day). Kapag mas maikli ang buwan
        ''' kaysa sa day, kinukuha ang huling araw ng buwan (kaya gumagana ang
        ''' EOM = 31 sa Pebrero at sa 30-day months).
        ''' </summary>
        Public Function ResolveDate(year As Integer, month As Integer, monthOffset As Integer, day As Integer) As Date
            Dim anchor = New Date(year, month, 1).AddMonths(monthOffset)
            Dim d = Math.Min(Math.Max(day, 1), Date.DaysInMonth(anchor.Year, anchor.Month))
            Return New Date(anchor.Year, anchor.Month, d)
        End Function

        ''' <summary>Mga cutoff na ang PAYDAY ay nasa buwan na ito.</summary>
        Public Function BuildForMonth(rows As IEnumerable(Of PayCyclePeriodModel), year As Integer, month As Integer) As List(Of PayCyclePeriodInstance)
            Dim result As New List(Of PayCyclePeriodInstance)()
            If rows Is Nothing Then Return result

            For Each periodRow In rows.OrderBy(Function(r) r.PeriodNo)
                result.Add(New PayCyclePeriodInstance With {
                    .PatternRowNo = periodRow.PeriodNo,
                    .CutoffStart = ResolveDate(year, month, periodRow.FromMonthOffset, periodRow.FromDay),
                    .CutoffEnd = ResolveDate(year, month, periodRow.ToMonthOffset, periodRow.ToDay),
                    .PayDate = ResolveDate(year, month, 0, periodRow.PayDay)
                })
            Next

            Return result
        End Function

        ''' <summary>
        ''' Lahat ng cutoff ng isang TAON. Ang taon ay ayon sa PAY DATE:
        ''' ang Dec 21 - Jan 5 na binabayaran ng Jan 15 ay kasama sa taong
        ''' iyon ng Jan 15 (kahit Dec ang simula).
        ''' </summary>
        Public Function BuildForYear(rows As IEnumerable(Of PayCyclePeriodModel), year As Integer) As List(Of PayCyclePeriodInstance)
            Dim all As New List(Of PayCyclePeriodInstance)()
            For m = 1 To 12
                all.AddRange(BuildForMonth(rows, year, m))
            Next

            all = all.OrderBy(Function(p) p.CutoffStart).ThenBy(Function(p) p.CutoffEnd).ToList()
            For i = 0 To all.Count - 1
                all(i).PeriodNo = i + 1
            Next
            Return all
        End Function

        ''' <summary>
        ''' Nagbabalik ng error message kapag may mali sa pattern, o Nothing kung ayos.
        ''' Sinusuri: saklaw ng values, End bago Start, OVERLAP, at GAP (butas) sa
        ''' pagitan ng magkakasunod na cutoff - sa loob ng 5 taon (kasama ang leap
        ''' year at lahat ng haba ng buwan).
        ''' </summary>
        Public Function Validate(rows As IEnumerable(Of PayCyclePeriodModel)) As String
            Dim list = If(rows, Enumerable.Empty(Of PayCyclePeriodModel)()).OrderBy(Function(r) r.PeriodNo).ToList()

            If list.Count = 0 Then
                Return "Add at least one period to the cutoff pattern."
            End If

            For Each r In list
                If r.FromDay < 1 OrElse r.FromDay > 31 OrElse r.ToDay < 1 OrElse r.ToDay > 31 OrElse r.PayDay < 1 OrElse r.PayDay > 31 Then
                    Return $"Period {r.PeriodNo}: days must be between 1 and 31 (31 = EOM)."
                End If
                If r.FromMonthOffset < -1 OrElse r.FromMonthOffset > 1 OrElse r.ToMonthOffset < -1 OrElse r.ToMonthOffset > 1 Then
                    Return $"Period {r.PeriodNo}: month must be Previous, This or Next."
                End If
            Next

            Dim instances As New List(Of PayCyclePeriodInstance)()
            For y = 2023 To 2027
                For m = 1 To 12
                    instances.AddRange(BuildForMonth(list, y, m))
                Next
            Next

            For Each p In instances
                If p.CutoffEnd < p.CutoffStart Then
                    Return $"Period {p.PatternRowNo}: the End date falls before the Start date (e.g. {p.CutoffStart:MM/dd/yyyy} to {p.CutoffEnd:MM/dd/yyyy})."
                End If
            Next

            Dim ordered = instances.OrderBy(Function(p) p.CutoffStart).ThenBy(Function(p) p.CutoffEnd).ToList()
            For i = 1 To ordered.Count - 1
                Dim prev = ordered(i - 1)
                Dim cur = ordered(i)

                If cur.CutoffStart <= prev.CutoffEnd Then
                    Return $"Periods {prev.PatternRowNo} and {cur.PatternRowNo} overlap ({prev.CutoffStart:MM/dd/yyyy}-{prev.CutoffEnd:MM/dd/yyyy} and {cur.CutoffStart:MM/dd/yyyy}-{cur.CutoffEnd:MM/dd/yyyy})."
                End If
                If cur.CutoffStart > prev.CutoffEnd.AddDays(1) Then
                    Return $"There is a gap between Period {prev.PatternRowNo} and {cur.PatternRowNo}: no cutoff covers {prev.CutoffEnd.AddDays(1):MM/dd/yyyy} to {cur.CutoffStart.AddDays(-1):MM/dd/yyyy}."
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>Maikling paglalarawan, hal. "21 (prev month)-5, paid 15 | 6-20, paid EOM".</summary>
        Public Function Describe(rows As IEnumerable(Of PayCyclePeriodModel)) As String
            If rows Is Nothing Then Return ""
            Dim parts = rows.OrderBy(Function(r) r.PeriodNo).Select(
                Function(r) $"{DayText(r.FromDay)}{OffsetSuffix(r.FromMonthOffset)}-{DayText(r.ToDay)}{OffsetSuffix(r.ToMonthOffset)}, paid {DayText(r.PayDay)}").ToList()
            Return String.Join("  |  ", parts)
        End Function

        Private Function OffsetSuffix(offset As Integer) As String
            Select Case offset
                Case -1 : Return " (prev month)"
                Case 1 : Return " (next month)"
                Case Else : Return ""
            End Select
        End Function

    End Module

End Namespace
