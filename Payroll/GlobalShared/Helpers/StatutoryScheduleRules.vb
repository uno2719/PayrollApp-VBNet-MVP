Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' PURE LOGIC LANG (walang database, walang UI) para sa Statutory Deduction Schedule.
    ''' Ginagamit ng Deduction Schedule screen ngayon, at ng payroll computation sa susunod:
    '''   share = ShareFor(...)                    -> ilang % ng buwanang contribution ang kakaltasin sa cutoff na ito
    '''   amount = AmountForPeriod(...)            -> ang halagang kakaltasin (2 decimals, walang naliligaw na sentimo)
    ''' </summary>
    Public Module StatutoryScheduleRules

        ''' <summary>Pinapayagang pagkakaiba sa pagsusuma ng shares (decimal(5,2) ang naka-save).</summary>
        Public Const Tolerance As Decimal = 0.005D

        ' =============================================
        ' LOOKUP - para sa computation
        ' =============================================

        ''' <summary>
        ''' Share % ng agency sa period na ito ng pay cycle. Nothing = walang naka-set na row
        ''' (ang computation ang magdedesisyon kung ihihinto o magbababala, hindi tahimik na zero).
        ''' </summary>
        Public Function ShareFor(rows As IEnumerable(Of StatutoryScheduleModel),
                                 cycleType As String, agency As String, periodInMonth As Integer) As Decimal?

            If rows Is Nothing Then Return Nothing

            Dim cycleKey = PayCycleChoices.Normalize(cycleType)

            For Each r In rows
                If r.PeriodNo = periodInMonth AndAlso
                   String.Equals(r.Agency, agency, StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(PayCycleChoices.Normalize(r.PayCycleType), cycleKey, StringComparison.OrdinalIgnoreCase) Then
                    Return r.SharePercent
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Halagang kakaltasin sa isang period. Bawat period ay nire-round sa decimals, at ang HULING
        ''' period na may share ang kumukuha ng matitirang sentimo, kaya ang kabuuan ng lahat ng period
        ''' ay laging eksaktong monthlyAmount. Ipinapalagay na valid na ang shares (100% o 0% ang total).
        ''' </summary>
        Public Function AmountForPeriod(monthlyAmount As Decimal, shares As IList(Of Decimal),
                                        periodNo As Integer, Optional decimals As Integer = 2) As Decimal

            If shares Is Nothing OrElse periodNo < 1 OrElse periodNo > shares.Count Then Return 0D

            Dim index = periodNo - 1
            If shares(index) <= 0D Then Return 0D

            Dim lastDeducting = -1
            For i = 0 To shares.Count - 1
                If shares(i) > 0D Then lastDeducting = i
            Next

            If index <> lastDeducting Then
                Return Math.Round(monthlyAmount * shares(index) / 100D, decimals, MidpointRounding.AwayFromZero)
            End If

            Dim alreadyDeducted As Decimal = 0D
            For i = 0 To lastDeducting - 1
                If shares(i) > 0D Then
                    alreadyDeducted += Math.Round(monthlyAmount * shares(i) / 100D, decimals, MidpointRounding.AwayFromZero)
                End If
            Next

            Return Math.Round(monthlyAmount, decimals, MidpointRounding.AwayFromZero) - alreadyDeducted
        End Function

        ' =============================================
        ' PRESETS AT VALIDATION
        ' =============================================

        ''' <summary>Shares ng isang agency para sa bilang ng period ng pay cycle.</summary>
        Public Function ApplyPreset(preset As SchedulePreset, periodCount As Integer) As List(Of Decimal)
            Dim shares As New List(Of Decimal)()
            For i = 1 To periodCount
                shares.Add(0D)
            Next

            If periodCount <= 0 Then Return shares

            Select Case preset
                Case SchedulePreset.First
                    shares(0) = 100D

                Case SchedulePreset.Last
                    shares(periodCount - 1) = 100D

                Case SchedulePreset.Split
                    Dim part = Math.Round(100D / periodCount, 2, MidpointRounding.AwayFromZero)
                    For i = 0 To periodCount - 2
                        shares(i) = part
                    Next
                    shares(periodCount - 1) = 100D - part * (periodCount - 1)

                Case Else
                    ' NotDeducted: zero lahat
            End Select

            Return shares
        End Function

        ''' <summary>
        ''' Ang kasalukuyang gawi ng PACSPORTS: SSS sa huling period (katapusan), PhilHealth at Pag-IBIG
        ''' sa unang period (15th). Ito ang inilalagay ng app kapag wala pang naka-save na schedule.
        ''' </summary>
        Public Function DefaultPreset(agency As String) As SchedulePreset
            If String.Equals(agency, StatutoryAgency.SSS, StringComparison.OrdinalIgnoreCase) Then
                Return SchedulePreset.Last
            End If
            Return SchedulePreset.First
        End Function

        ''' <summary>Valid ang total ng isang agency kapag 100% (kinakaltas) o 0% (hindi kinakaltas).</summary>
        Public Function IsValidTotal(total As Decimal) As Boolean
            Return Math.Abs(total) <= Tolerance OrElse Math.Abs(total - 100D) <= Tolerance
        End Function

        ' =============================================
        ' TEXT - para sa header ng matrix at preview
        ' =============================================

        ''' <summary>"15th", "EOM", "1st", "22nd"... Ang 31 (o higit) sa pattern ay EOM.</summary>
        Public Function PayDayText(payDay As Integer) As String
            If payDay >= PayCycleChoices.EndOfMonthDay Then Return "EOM"

            Dim suffix = "th"
            Dim lastTwo = payDay Mod 100
            If lastTwo < 11 OrElse lastTwo > 13 Then
                Select Case payDay Mod 10
                    Case 1 : suffix = "st"
                    Case 2 : suffix = "nd"
                    Case 3 : suffix = "rd"
                End Select
            End If

            Return payDay.ToString() & suffix
        End Function

        ''' <summary>Saklaw ng period, hal. "1 - 15", "16 - EOM", "21 (prev) - 5".</summary>
        Public Function RangeText(period As PayCyclePeriodModel) As String
            Return $"{PayCycleChoices.DayText(period.FromDay)}{MonthTag(period.FromMonthOffset)} - " &
                   $"{PayCycleChoices.DayText(period.ToDay)}{MonthTag(period.ToMonthOffset)}"
        End Function

        Private Function MonthTag(offset As Integer) As String
            Select Case offset
                Case -1 : Return " (prev)"
                Case 1 : Return " (next)"
                Case Else : Return ""
            End Select
        End Function

    End Module

End Namespace
