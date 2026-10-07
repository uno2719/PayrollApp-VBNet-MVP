Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class CutoffService
        Implements ICutoffService

        Private ReadOnly _repository As ICutoffRepository
        Private ReadOnly _payCycleRepository As IPayCycleRepository

        Public Sub New(repository As ICutoffRepository, payCycleRepository As IPayCycleRepository)
            _repository = repository
            _payCycleRepository = payCycleRepository
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

        Public Async Function GetActiveCycleTypesAsync() As Task(Of List(Of String)) _
            Implements ICutoffService.GetActiveCycleTypesAsync

            Dim cycles = Await _payCycleRepository.GetAllAsync()
            Return cycles.Where(Function(c) c.IsActive).Select(Function(c) c.PayCycleType).ToList()
        End Function

        Public Async Function GetPatternDescriptionAsync(cycleType As String) As Task(Of String) _
            Implements ICutoffService.GetPatternDescriptionAsync

            Dim cycle = Await _payCycleRepository.GetByTypeAsync(cycleType)
            If cycle Is Nothing OrElse cycle.Periods.Count = 0 Then Return ""
            Return PayCyclePatternHelper.Describe(cycle.Periods)
        End Function

        Public Async Function PreviewForYearAsync(cycleType As String, year As Integer) As Task(Of List(Of CutoffPreviewRow)) _
            Implements ICutoffService.PreviewForYearAsync

            Dim periods = Await BuildPeriodsForYearAsync(cycleType, year)
            Dim rows As New List(Of CutoffPreviewRow)()

            For i = 0 To periods.Count - 1
                Dim p = periods(i)
                Dim exists = Await _repository.OverlapExistsAsync(p.CycleType, p.CutoffStart, p.CutoffEnd, 0)
                rows.Add(New CutoffPreviewRow With {
                    .PeriodNo = i + 1,
                    .CutoffStart = p.CutoffStart,
                    .CutoffEnd = p.CutoffEnd,
                    .PayDate = p.PayDate,
                    .AlreadyExists = exists
                })
            Next

            Return rows
        End Function

        Public Async Function GenerateForYearAsync(cycleType As String, year As Integer, userName As String) As Task(Of Integer) _
            Implements ICutoffService.GenerateForYearAsync

            Dim periods = Await BuildPeriodsForYearAsync(cycleType, year)
            Return Await _repository.BulkInsertAsync(periods, userName)
        End Function

        ' ========================================================
        ' Ang pattern ng pay cycle (Pay Cycle Settings) ang pinagmumulan ng
        ' lahat ng petsa: From/To/Pay day ng bawat period, naka-anchor sa
        ' pay month. Ang CutoffYear ay ayon sa PAY DATE ng period.
        ' ========================================================
        Private Async Function BuildPeriodsForYearAsync(cycleType As String, year As Integer) As Task(Of List(Of CutoffModel))
            If String.IsNullOrWhiteSpace(cycleType) Then
                Throw New PayrollCutoffGenerateException("Select a pay cycle first.")
            End If

            Dim cycle = Await _payCycleRepository.GetByTypeAsync(cycleType)
            If cycle Is Nothing Then
                Throw New PayrollCutoffGenerateException($"Pay cycle '{cycleType}' is not set up in Pay Cycle Settings.")
            End If

            If Not cycle.IsActive Then
                Throw New PayrollCutoffGenerateException($"Pay cycle '{cycleType}' is inactive. Activate it in Pay Cycle Settings first.")
            End If

            If cycle.Periods.Count > 0 Then
                Dim problem = PayCyclePatternHelper.Validate(cycle.Periods)
                If problem IsNot Nothing Then
                    Throw New PayrollCutoffGenerateException($"The {cycleType} cutoff pattern needs fixing: {problem}")
                End If

                Return PayCyclePatternHelper.BuildForYear(cycle.Periods, year) _
                    .Select(Function(p) NewPeriod(cycleType, year, p.CutoffStart, p.CutoffEnd, p.PayDate)) _
                    .ToList()
            End If

            ' Weekly: every-7-days (hindi day-of-month), kaya walang pattern - lumang calendar-week generator.
            If String.Equals(cycleType, PayCycleService.WeeklyCycle, StringComparison.OrdinalIgnoreCase) Then
                Return BuildWeeklyPeriods(cycleType, year)
            End If

            Throw New PayrollCutoffGenerateException(
                $"'{cycleType}' has no cutoff pattern yet. Add its periods in Pay Cycle Settings first.")
        End Function

        Private Function BuildWeeklyPeriods(cycleType As String, year As Integer) As List(Of CutoffModel)
            Dim periods As New List(Of CutoffModel)
            Dim cursor = New Date(year, 1, 1)

            While cursor.Year <= year
                Dim weekEnd = cursor.AddDays(6)
                If cursor.Year <> year AndAlso weekEnd.Year <> year Then Exit While
                periods.Add(NewPeriod(cycleType, year, cursor, weekEnd, Nothing))
                cursor = weekEnd.AddDays(1)
                If cursor.Year > year Then Exit While
            End While

            Return periods
        End Function

        Private Function NewPeriod(cycleType As String, year As Integer, start As Date, [end] As Date, payDate As Date?) As CutoffModel
            Return New CutoffModel With {
                .CycleType = cycleType,
                .CutoffYear = year,
                .CutoffStart = start,
                .CutoffEnd = [end],
                .PayDate = payDate,
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
