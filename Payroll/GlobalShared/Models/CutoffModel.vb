Imports Payroll.GlobalShared.Constants

Namespace GlobalShared.Models

    Public Enum CutoffStatus As Byte
        Draft = 0
        Processed = 1
        Posted = 2     ' locked — future feature, not enforced yet

        ''' <summary>
        ''' Hindi ipo-process sa app na ito, hal. ang mga cutoff bago nagsimulang gamitin ang Payroll App.
        ''' Hindi na lumalabas sa Payroll Input Entry. Hindi ito binibilang na "processed" (hindi nila nila-lock
        ''' ang Rate Basis ng pay cycle o ang Input Unit ng mga code). Pwedeng ibalik sa Draft.
        ''' </summary>
        Closed = 3
    End Enum

    ''' <summary>
    ''' A pay period instance. Replaces C1Pay's subscription-capped "Cutoff"
    ''' concept (and fulfills the Cutoff-module dependency deferred earlier in
    ''' General Settings' Pay Cycle Counter feature) with no artificial usage
    ''' cap, since this is an in-house app. Managed via Payroll Settings' new
    ''' Cutoff tab; consumed (read-only) by Payroll Input Entry.
    ''' </summary>
    Public Class CutoffModel
        Public Property CutoffID As Integer
        Public Property CycleType As String        ' matches tblEmployeeEarnings.PayCycle
        Public Property CutoffYear As Integer
        Public Property CutoffStart As Date
        Public Property CutoffEnd As Date
        Public Property PayDate As Date?
        Public Property CutoffLabel As String
        Public Property Status As CutoffStatus
        Public Property CreatedBy As String
        Public Property CreatedDate As DateTime
        Public Property ModifiedBy As String
        Public Property ModifiedDate As DateTime?

        Public ReadOnly Property DisplayLabel As String
            Get
                ' Maikli na ang CutoffLabel (hal. "S1 Feb 2026"), kaya sa mga dropdown ay isinasama ang saklaw ng petsa.
                Dim fmt = AppConstants.DisplayDateFormat
                Dim range = $"{CutoffStart.ToString(fmt)} - {CutoffEnd.ToString(fmt)}"

                Return If(String.IsNullOrWhiteSpace(CutoffLabel),
                           $"{CycleType} {range}",
                           $"{CutoffLabel}  ({range})")
            End Get
        End Property
    End Class

End Namespace