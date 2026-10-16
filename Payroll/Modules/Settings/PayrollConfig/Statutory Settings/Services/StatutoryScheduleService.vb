Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data
Imports Payroll.StatutorySettings.Data

Namespace StatutorySettings.Services

    Public Class StatutoryScheduleService
        Implements IStatutoryScheduleService

        Private ReadOnly _repository As IStatutoryScheduleRepository
        Private ReadOnly _payCycleRepository As IPayCycleRepository

        Public Sub New(repository As IStatutoryScheduleRepository, payCycleRepository As IPayCycleRepository)
            _repository = repository
            _payCycleRepository = payCycleRepository
        End Sub

        ' =============================================
        ' ACTIVE PAY CYCLES - mga may cutoff pattern lang (kailangan ang bilang ng period)
        ' =============================================
        Private Async Function GetActiveCyclesAsync() As Task(Of List(Of PayCycleModel))
            Dim cycles = Await _payCycleRepository.GetAllAsync()
            Return cycles.
                Where(Function(c) c.IsActive AndAlso c.Periods IsNot Nothing AndAlso c.Periods.Count > 0).
                OrderBy(Function(c) c.PayCycleId).
                ToList()
        End Function

        Private Shared Function SameCycle(a As String, b As String) As Boolean
            Return String.Equals(PayCycleChoices.Normalize(a), PayCycleChoices.Normalize(b), StringComparison.OrdinalIgnoreCase)
        End Function

        Private Shared Function FindRow(existing As List(Of StatutoryScheduleModel),
                                        cycleType As String, agency As String, periodNo As Integer) As StatutoryScheduleModel
            For Each r In existing
                If r.PeriodNo = periodNo AndAlso
                   String.Equals(r.Agency, agency, StringComparison.OrdinalIgnoreCase) AndAlso
                   SameCycle(r.PayCycleType, cycleType) Then
                    Return r
                End If
            Next
            Return Nothing
        End Function

        Private Shared Function HasAnyRow(existing As List(Of StatutoryScheduleModel), cycleType As String, agency As String) As Boolean
            For Each r In existing
                If String.Equals(r.Agency, agency, StringComparison.OrdinalIgnoreCase) AndAlso SameCycle(r.PayCycleType, cycleType) Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>Ang naka-save na Pag-IBIG voluntary mode (galing sa kahit anong Pag-IBIG row); default kung wala pa.</summary>
        Private Shared Function StoredVoluntaryMode(existing As List(Of StatutoryScheduleModel)) As PagIbigVoluntaryMode
            For Each r In existing
                If String.Equals(r.Agency, StatutoryAgency.PagIbig, StringComparison.OrdinalIgnoreCase) AndAlso r.VoluntaryMode.HasValue Then
                    Return CType(r.VoluntaryMode.Value, PagIbigVoluntaryMode)
                End If
            Next
            Return PagIbigVoluntaryMode.ReplaceTable
        End Function

        ' =============================================
        ' DEFAULT SEED
        ' =============================================
        Public Async Function EnsureDefaultsAsync(userName As String) As Task _
            Implements IStatutoryScheduleService.EnsureDefaultsAsync

            Dim cycles = Await GetActiveCyclesAsync()
            If cycles.Count = 0 Then Return

            Dim existing = Await _repository.GetAllAsync()
            Dim mode = CByte(StoredVoluntaryMode(existing))
            Dim toInsert As New List(Of StatutoryScheduleModel)()

            For Each cycle In cycles
                Dim periods = cycle.Periods.OrderBy(Function(p) p.PeriodNo).ToList()

                For Each agency In StatutoryAgency.All
                    If HasAnyRow(existing, cycle.PayCycleType, agency) Then Continue For

                    Dim shares = StatutoryScheduleRules.ApplyPreset(StatutoryScheduleRules.DefaultPreset(agency), periods.Count)

                    For i = 0 To periods.Count - 1
                        Dim row As New StatutoryScheduleModel With {
                            .PayCycleType = cycle.PayCycleType,
                            .Agency = agency,
                            .PeriodNo = periods(i).PeriodNo,
                            .SharePercent = shares(i)
                        }
                        If agency = StatutoryAgency.PagIbig Then row.VoluntaryMode = mode
                        toInsert.Add(row)
                    Next
                Next
            Next

            If toInsert.Count > 0 Then
                Await _repository.InsertManyAsync(toInsert, userName)
            End If
        End Function

        ' =============================================
        ' LOAD
        ' =============================================
        Public Async Function GetSetupAsync() As Task(Of StatutoryScheduleSetup) _
            Implements IStatutoryScheduleService.GetSetupAsync

            Dim cycles = Await GetActiveCyclesAsync()
            Dim existing = Await _repository.GetAllAsync()
            Dim setup As New StatutoryScheduleSetup With {.VoluntaryMode = StoredVoluntaryMode(existing)}

            For Each cycle In cycles
                Dim periods = cycle.Periods.OrderBy(Function(p) p.PeriodNo).ToList()

                Dim info As New StatutoryScheduleCycleInfo With {
                    .PayCycleType = cycle.PayCycleType,
                    .PatternText = PayCyclePatternHelper.Describe(periods)
                }
                For Each p In periods
                    info.Periods.Add(New StatutorySchedulePeriodInfo With {
                        .PeriodNo = p.PeriodNo,
                        .RangeText = StatutoryScheduleRules.RangeText(p),
                        .PayDayText = StatutoryScheduleRules.PayDayText(p.PayDay)
                    })
                Next
                setup.Cycles.Add(info)

                ' Kapag nadagdagan ang period ng pattern pagkatapos mag-save, 0% ang bagong period -
                ' lalabas na hindi 100% ang total, kaya kailangang ayusin bago makapag-save ulit.
                Dim rows As New List(Of StatutoryScheduleRow)()
                For Each agency In StatutoryAgency.All
                    Dim row As New StatutoryScheduleRow With {.Agency = agency}
                    For Each p In periods
                        Dim saved = FindRow(existing, cycle.PayCycleType, agency, p.PeriodNo)
                        row.Shares.Add(If(saved Is Nothing, 0D, saved.SharePercent))
                    Next
                    rows.Add(row)
                Next
                setup.Rows(cycle.PayCycleType) = rows
            Next

            Return setup
        End Function

        ' =============================================
        ' VALIDATE
        ' =============================================
        Public Function Validate(setup As StatutoryScheduleSetup) As String _
            Implements IStatutoryScheduleService.Validate

            If setup Is Nothing OrElse setup.Cycles.Count = 0 Then
                Return "There is no active pay cycle with a cutoff pattern. Set one up in Payroll Settings > Pay Cycle first."
            End If

            For Each info In setup.Cycles
                If Not setup.Rows.ContainsKey(info.PayCycleType) Then
                    Return $"{info.PayCycleType}: the schedule rows are missing."
                End If

                Dim rows = setup.Rows(info.PayCycleType)
                Dim present = rows.Select(Function(r) r.Agency).ToList()

                For Each agency In StatutoryAgency.All
                    If Not present.Contains(agency) Then
                        Return $"{info.PayCycleType}: there is no row for {StatutoryAgency.DisplayName(agency)}."
                    End If
                Next

                For Each row In rows
                    Dim label = $"{info.PayCycleType} / {row.AgencyName}"

                    If row.Shares.Count <> info.Periods.Count Then
                        Return $"{label}: the schedule has {row.Shares.Count} period(s) but the cutoff pattern has {info.Periods.Count}. Reload the screen."
                    End If

                    For Each share In row.Shares
                        If share < 0D OrElse share > 100D Then
                            Return $"{label}: each share must be between 0 and 100."
                        End If
                    Next

                    If Not StatutoryScheduleRules.IsValidTotal(row.Total) Then
                        Return $"{label}: the shares total {row.Total:0.##}%. They must total 100%, or 0% if it is not deducted."
                    End If
                Next
            Next

            Return Nothing
        End Function

        ' =============================================
        ' SAVE
        ' =============================================
        Public Async Function SaveAsync(setup As StatutoryScheduleSetup, userName As String) As Task(Of StatutoryScheduleSaveResult) _
            Implements IStatutoryScheduleService.SaveAsync

            Dim problem = Validate(setup)
            If problem IsNot Nothing Then
                Return New StatutoryScheduleSaveResult With {.Success = False, .ErrorMessage = problem}
            End If

            Dim mode = CByte(setup.VoluntaryMode)
            Dim items As New List(Of StatutoryScheduleModel)()
            Dim periodCounts As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)

            For Each info In setup.Cycles
                periodCounts(info.PayCycleType) = info.Periods.Max(Function(p) p.PeriodNo)

                For Each row In setup.Rows(info.PayCycleType)
                    For i = 0 To info.Periods.Count - 1
                        items.Add(New StatutoryScheduleModel With {
                            .PayCycleType = info.PayCycleType,
                            .Agency = row.Agency,
                            .PeriodNo = info.Periods(i).PeriodNo,
                            .SharePercent = Math.Round(row.Shares(i), 2, MidpointRounding.AwayFromZero)
                        })
                    Next
                Next
            Next

            Await _repository.SaveAllAsync(items, periodCounts, mode, userName)
            Return New StatutoryScheduleSaveResult With {.Success = True}
        End Function

    End Class

End Namespace
