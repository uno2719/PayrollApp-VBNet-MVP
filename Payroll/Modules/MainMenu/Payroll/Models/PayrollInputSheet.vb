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
    End Class

End Namespace
