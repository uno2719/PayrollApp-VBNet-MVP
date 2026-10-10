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

    End Module

End Namespace
