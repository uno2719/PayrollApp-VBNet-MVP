Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class InputUnitChangeCheck
        Public Property Allowed As Boolean = True
        Public Property NeedsConfirmation As Boolean
        Public Property ClearDrafts As Boolean
        Public Property Message As String
    End Class

    ''' <summary>
    ''' Binabantayan ang pagpalit ng InputUnit ng isang catalog code (Compensation/
    ''' Deduction/Overtime/Holiday/Bonus) para hindi magbago ang kahulugan ng mga
    ''' naka-save nang numero:
    '''   - May entry na sa Processed/Posted na cutoff  -> LOCKED, hindi pwedeng palitan.
    '''   - May entry lang sa Draft na cutoff           -> hihingan ng kumpirmasyon; kapag
    '''                                                    pumayag, buburahin ang draft entries.
    '''   - Walang entry                                -> malayang palitan.
    ''' Ang bagong unit ay para sa susunod na cutoff; ang mga luma ay hindi na magagalaw,
    ''' kaya hindi nagbabago ang mga lumang report.
    ''' </summary>
    Public Class InputUnitGuard

        Private ReadOnly _usage As IInputUnitUsageRepository

        Public Sub New(usage As IInputUnitUsageRepository)
            _usage = usage
        End Sub

        Public Async Function CheckAsync(category As String, code As String,
                                         currentUnit As PayrollInputUnit, newUnit As PayrollInputUnit,
                                         confirmClearDrafts As Boolean) As Task(Of InputUnitChangeCheck)

            If currentUnit = newUnit Then Return New InputUnitChangeCheck()

            Dim usage = Await _usage.GetUsageAsync(category, code)

            If usage.ProcessedEntries > 0 Then
                Return New InputUnitChangeCheck With {
                    .Allowed = False,
                    .Message = $"The input unit of '{code}' is locked: it is already used by {usage.ProcessedEntries} saved entr{If(usage.ProcessedEntries = 1, "y", "ies")} in processed cutoffs, " &
                               "so changing it would change the meaning of old payroll records." & vbCrLf & vbCrLf &
                               "Create a new code with the unit you want, then deactivate this one."
                }
            End If

            If usage.DraftEntries > 0 Then
                If Not confirmClearDrafts Then
                    Return New InputUnitChangeCheck With {
                        .Allowed = False,
                        .NeedsConfirmation = True,
                        .Message = $"'{code}' has {usage.DraftEntries} saved entr{If(usage.DraftEntries = 1, "y", "ies")} in cutoffs that are not processed yet." & vbCrLf & vbCrLf &
                                   $"Changing the unit from {PayrollInputUnits.Text(currentUnit)} to {PayrollInputUnits.Text(newUnit)} will CLEAR those entries, so the numbers do not change meaning. Continue?"
                    }
                End If

                Return New InputUnitChangeCheck With {.ClearDrafts = True}
            End If

            Return New InputUnitChangeCheck()
        End Function

        Public Function ClearDraftsAsync(category As String, code As String) As Task(Of Integer)
            Return _usage.ClearDraftEntriesAsync(category, code)
        End Function

    End Class

End Namespace
