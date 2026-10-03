Namespace GlobalShared.Models

    Public Enum CutoffStatus As Byte
        Draft = 0
        Processed = 1
        Posted = 2     ' locked — future feature, not enforced yet
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
                Return If(String.IsNullOrWhiteSpace(CutoffLabel),
                           $"{CycleType} {CutoffStart:MMM d} - {CutoffEnd:MMM d, yyyy}",
                           CutoffLabel)
            End Get
        End Property
    End Class

End Namespace