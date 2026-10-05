Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class CutoffService
        Implements ICutoffService

        Private ReadOnly _repository As ICutoffRepository

        Public Sub New(repository As ICutoffRepository)
            _repository = repository
        End Sub

        Public Function GetAllAsync() As Task(Of List(Of CutoffModel)) Implements ICutoffService.GetAllAsync
            Return _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As CutoffModel, userName As String) As Task(Of PayrollSettingsSaveResult) Implements ICutoffService.SaveAsync
            If item.CutoffEnd < item.CutoffStart Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Cutoff End date cannot be earlier than Cutoff Start date."}
            End If

            If String.IsNullOrWhiteSpace(item.CycleType) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Cycle Type is required."}
            End If

            Dim overlaps = Await _repository.OverlapExistsAsync(item.CycleType, item.CutoffStart, item.CutoffEnd, item.CutoffID)
            If overlaps Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = $"This date range overlaps an existing {item.CycleType} Cutoff."}
            End If

            If item.CutoffID = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        ''' <summary>
        ''' Derives the recurring pattern from an EXISTING Cutoff of the same
        ''' CycleType (the most recent one on file) instead of assuming a
        ''' calendar-standard split — PACSPORTS' actual SemiMonthly cycle is
        ''' 6th-20th / 21st-5th-of-next-month, not 1-15/16-31, and that pattern
        ''' isn't knowable without a real example to copy. Manually add ONE
        ''' Cutoff first (that defines the pattern), then Generate fills in
        ''' the rest of the year following it — including correctly crossing
        ''' month/year boundaries (e.g. Dec 21 - Jan 5).
        ''' </summary>
        Public Async Function GenerateForYearAsync(cycleType As String, year As Integer, userName As String) As Task(Of Integer) _
            Implements ICutoffService.GenerateForYearAsync

            If cycleType = "Daily" Then
                Throw New PayrollCutoffGenerateException(
                    "Batch generation isn't supported for 'Daily' — add those Cutoffs one at a time instead.")
            End If

            Dim allCutoffs = Await _repository.GetAllAsync()
            Dim reference = allCutoffs.
                Where(Function(c) c.CycleType = cycleType).
                OrderByDescending(Function(c) c.CutoffStart).
                FirstOrDefault()

            If reference Is Nothing Then
                Throw New PayrollCutoffGenerateException(
                    $"Wala pang existing na {cycleType} Cutoff na pwedeng gawing pattern — mag-add muna ng isa nang manually bago mag-Generate for Year.")
            End If

            Dim periods = BuildPeriodsForYear(cycleType, year, reference)
            Return Await _repository.BulkInsertAsync(periods, userName)
        End Function

        Private Function BuildPeriodsForYear(cycleType As String, year As Integer, reference As CutoffModel) As List(Of CutoffModel)
            Dim periods As New List(Of CutoffModel)

            Select Case cycleType
                Case "Monthly"
                    ' One fixed start-day-of-month, taken from the reference (usually the
                    ' 1st, but supports fiscal-month patterns like "26th to 25th" too).
                    Dim startDay = reference.CutoffStart.Day
                    Dim cursor = SafeDate(year, 1, startDay)

                    For i = 1 To 12
                        Dim periodEnd = cursor.AddMonths(1).AddDays(-1)
                        periods.Add(NewPeriod(cycleType, year, cursor, periodEnd))
                        cursor = cursor.AddMonths(1)
                    Next

                Case "SemiMonthly"
                    ' Infer BOTH half-start-days from the single reference Cutoff, regardless
                    ' of which half it happens to be — e.g. reference Oct 6-20 (first half,
                    ' doesn't cross a month) tells us firstStartDay=6, secondStartDay=20+1=21.
                    ' A reference like Dec 21 - Jan 5 (crosses into next month) works the same
                    ' way in reverse: secondStartDay=21, firstStartDay=5+1=6.
                    Dim firstStartDay As Integer
                    Dim secondStartDay As Integer

                    If reference.CutoffEnd.Month = reference.CutoffStart.Month Then
                        firstStartDay = reference.CutoffStart.Day
                        secondStartDay = reference.CutoffEnd.Day + 1
                    Else
                        secondStartDay = reference.CutoffStart.Day
                        firstStartDay = reference.CutoffEnd.Day + 1
                    End If

                    Dim cursor = SafeDate(year, 1, firstStartDay)
                    While cursor.Year <= year
                        Dim secondStart = SafeDate(cursor.Year, cursor.Month, secondStartDay)
                        If secondStart <= cursor Then secondStart = secondStart.AddMonths(1)
                        Dim firstEnd = secondStart.AddDays(-1)
                        periods.Add(NewPeriod(cycleType, year, cursor, firstEnd))

                        Dim nextFirstStart = cursor.AddMonths(1)
                        Dim secondEnd = nextFirstStart.AddDays(-1)
                        periods.Add(NewPeriod(cycleType, year, secondStart, secondEnd))

                        cursor = nextFirstStart
                        If cursor.Year > year Then Exit While
                    End While

                Case "Weekly"
                    ' Same day-of-week as the reference Cutoff's start (e.g. always Mondays).
                    Dim targetDow = reference.CutoffStart.DayOfWeek
                    Dim cursor = New Date(year, 1, 1)
                    While cursor.DayOfWeek <> targetDow
                        cursor = cursor.AddDays(1)
                    End While

                    While cursor.Year = year
                        Dim weekEnd = cursor.AddDays(6)
                        periods.Add(NewPeriod(cycleType, year, cursor, weekEnd))
                        cursor = cursor.AddDays(7)
                    End While

                Case Else
                    Throw New PayrollCutoffGenerateException(
                        $"Batch generation isn't supported for '{cycleType}' — add those Cutoffs one at a time instead.")
            End Select

            ' Safety net: only keep periods that actually belong to the requested year
            ' (a trailing Dec period legitimately ends in year+1 — that's expected and kept).
            Return periods.Where(Function(p) p.CutoffStart.Year = year OrElse p.CutoffEnd.Year = year).ToList()
        End Function

        ''' <summary>Clamps the day to whatever the target month actually has (e.g. day 31 in February) — this is what the old code was missing, causing the date overflow error.</summary>
        Private Function SafeDate(year As Integer, month As Integer, day As Integer) As Date
            Dim clampedDay = Math.Min(day, Date.DaysInMonth(year, month))
            Return New Date(year, month, clampedDay)
        End Function

        Private Function NewPeriod(cycleType As String, year As Integer, start As Date, [end] As Date) As CutoffModel
            Return New CutoffModel With {
                .CycleType = cycleType,
                .CutoffYear = year,
                .CutoffStart = start,
                .CutoffEnd = [end],
                .CutoffLabel = $"{cycleType} {start:MMM d} - {[end]:MMM d, yyyy}",
                .Status = CutoffStatus.Draft
            }
        End Function

    End Class

    Public Class PayrollCutoffGenerateException
        Inherits Exception
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
    End Class

End Namespace