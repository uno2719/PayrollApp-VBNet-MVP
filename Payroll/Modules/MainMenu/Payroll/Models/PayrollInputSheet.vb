Imports System.Data
Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Models

    ''' <summary>
    ''' Laman ng Payroll Input Entry para sa isang cutoff: ang grid (mga kasamang empleyado lang) at ang mga
    ''' empleyadong hindi naisama dahil kulang ang setup (para sa banner ng babala).
    ''' </summary>
    Public Class PayrollInputSheet
        Public Property Table As DataTable
        Public Property Warnings As List(Of PayrollEligibilityWarning) = New List(Of PayrollEligibilityWarning)()

        ''' <summary>
        ''' Employee > Fix Transactions na papasok sa cutoff na ito kapag BLANGKO ang cell (key = HintKey).
        ''' Nasa unit ng column (peso o dami). Ipinapakita bilang gray na hint sa grid.
        ''' </summary>
        Public Property FixHints As Dictionary(Of String, Decimal) = New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase)

        Public Shared Function HintKey(recordId As Integer, columnName As String) As String
            Return recordId.ToString() & "|" & If(columnName, "").Trim().ToUpperInvariant()
        End Function
    End Class

End Namespace
