Namespace PayrollSettings.Data

    ''' <summary>Ilang saved entries (sa Payroll Input Entry) ang gumagamit ng isang catalog code.</summary>
    Public Class InputUnitUsage
        ''' <summary>Sa mga cutoff na Processed/Posted na (hindi na dapat magbago ang kahulugan).</summary>
        Public Property ProcessedEntries As Integer
        ''' <summary>Sa mga cutoff na Draft pa (pwede pang i-clear).</summary>
        Public Property DraftEntries As Integer
    End Class

    Public Interface IInputUnitUsageRepository
        Function GetUsageAsync(category As String, code As String) As Task(Of InputUnitUsage)

        ''' <summary>Binubura ang entries ng code sa mga cutoff na Draft pa lang. Nagbabalik ng bilang na nabura.</summary>
        Function ClearDraftEntriesAsync(category As String, code As String) As Task(Of Integer)
    End Interface

End Namespace
