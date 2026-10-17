Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' Mga patakaran kung aling cutoff ang "bukas" para sa payroll. Iisang pinagmulan para sa
    ''' Payroll Input Entry ngayon, at (susunod) sa indicator ng "next cutoff to process" sa itaas ng app / dashboard.
    ''' </summary>
    Public Module CutoffRules

        ''' <summary>
        ''' Lumalabas sa Payroll Input Entry: Draft o Processed pa lang (hindi Posted, hindi Closed), at
        ''' nagsimula na ang cutoff (hindi pa lumalabas ang mga susunod pang buwan).
        ''' </summary>
        Public Function IsOpenForInput(cutoff As CutoffModel, today As Date) As Boolean
            Return (cutoff.Status = CutoffStatus.Draft OrElse cutoff.Status = CutoffStatus.Processed) _
                   AndAlso cutoff.CutoffStart.Date <= today.Date
        End Function

        ''' <summary>
        ''' Ang SUSUNOD na i-process sa bawat pay cycle: ang pinakamaagang Draft na cutoff na nagsimula na.
        ''' </summary>
        Public Function NextToProcess(cutoffs As IEnumerable(Of CutoffModel), today As Date) As List(Of CutoffModel)
            Return cutoffs.
                Where(Function(c) c.Status = CutoffStatus.Draft AndAlso c.CutoffStart.Date <= today.Date).
                GroupBy(Function(c) c.CycleType).
                Select(Function(g) g.OrderBy(Function(c) c.CutoffStart).First()).
                OrderBy(Function(c) c.CycleType).
                ToList()
        End Function


        ' =============================================
        ' INDICATOR - "next cutoff to process" sa itaas ng app
        ' =============================================

        ''' <summary>Gaano na kalapit (o lampas na) ang pay date. Sa araw ang batayan, hindi sa oras.</summary>
        Public Function DeadlineState(payDate As Date?, today As Date) As CutoffDeadlineState
            If Not payDate.HasValue Then Return CutoffDeadlineState.NoPayDate

            Dim days = (payDate.Value.Date - today.Date).Days
            If days < 0 Then Return CutoffDeadlineState.Overdue
            If days = 0 Then Return CutoffDeadlineState.DueToday
            If days <= 3 Then Return CutoffDeadlineState.Soon
            Return CutoffDeadlineState.OnTrack
        End Function

        ''' <summary>Hal. "pay Oct 31 (in 14 days)", "pay Oct 15 (today)", "pay Oct 15 (2 days overdue)".</summary>
        Public Function DeadlineText(payDate As Date?, today As Date) As String
            If Not payDate.HasValue Then Return "no pay date"

            Dim days = (payDate.Value.Date - today.Date).Days
            Dim whenText As String
            If days < 0 Then
                whenText = If(days = -1, "1 day overdue", $"{-days} days overdue")
            ElseIf days = 0 Then
                whenText = "today"
            ElseIf days = 1 Then
                whenText = "tomorrow"
            Else
                whenText = $"in {days} days"
            End If

            Return $"pay {payDate.Value:MMM d} ({whenText})"
        End Function

        ''' <summary>Isang linya ng indicator bawat pay cycle: ang SUSUNOD na i-process (NextToProcess) at ang pay date nito.</summary>
        Public Function BuildIndicator(cutoffs As IEnumerable(Of CutoffModel), today As Date) As List(Of CutoffIndicatorItem)
            Return NextToProcess(cutoffs, today).
                Select(Function(c) New CutoffIndicatorItem With {
                    .CutoffId = c.CutoffID,
                    .CycleType = c.CycleType,
                    .Label = c.CutoffLabel,
                    .PayDate = c.PayDate,
                    .State = DeadlineState(c.PayDate, today),
                    .Text = DeadlineText(c.PayDate, today)
                }).ToList()
        End Function

    End Module

    Public Enum CutoffDeadlineState
        NoPayDate
        OnTrack         ' higit 3 araw pa
        Soon            ' 1 hanggang 3 araw
        DueToday
        Overdue
    End Enum

    Public Class CutoffIndicatorItem
        Public Property CutoffId As Integer
        Public Property CycleType As String
        Public Property Label As String
        Public Property PayDate As Date?
        Public Property State As CutoffDeadlineState
        Public Property Text As String
    End Class

End Namespace
