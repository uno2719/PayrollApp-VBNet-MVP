' File: GlobalShared/Constants/FixTransactionConstants.vb
' Mga pangalan ng Frequency / Pay On / Transaction Type ng Employee > Fix Transactions.
' Naka-save bilang TEXT sa tblEmployeeFixTxn (may CHECK constraint ang SQL), kaya dito lang ang spelling.
Namespace GlobalShared.Constants

    Public Module FixTxnFrequency
        Public Const EveryCycle As String = "Every Cycle"
        Public Const Monthly As String = "Monthly"
        Public Const Quarterly As String = "Quarterly"
        Public Const SemiAnnual As String = "Semi-Annual"
        Public Const Annual As String = "Annual"

        Public ReadOnly All As String() = {EveryCycle, Monthly, Quarterly, SemiAnnual, Annual}

        ''' <summary>Agwat ng buwan: Monthly 1, Quarterly 3, Semi-Annual 6, Annual 12. 0 = Every Cycle (walang agwat).</summary>
        Public Function IntervalMonths(frequency As String) As Integer
            Select Case If(frequency, "").Trim()
                Case Monthly : Return 1
                Case Quarterly : Return 3
                Case SemiAnnual : Return 6
                Case Annual : Return 12
                Case Else : Return 0
            End Select
        End Function

        Public Function IsKnown(frequency As String) As Boolean
            Return All.Any(Function(f) String.Equals(f, If(frequency, "").Trim(), StringComparison.Ordinal))
        End Function
    End Module

    Public Module FixTxnPayOn
        Public Const FirstCycle As String = "1st Cycle"
        Public Const LastCycle As String = "Last Cycle"

        Public ReadOnly All As String() = {FirstCycle, LastCycle}
    End Module

    ''' <summary>Ang "Transaction Type" sa screen. Ang Allowance at Compensation ay parehong COMPENSATION sa database.</summary>
    Public Module FixTxnType
        Public Const Allowance As String = "Allowance"
        Public Const Compensation As String = "Compensation"
        Public Const Bonus As String = "Bonus"
        Public Const Deduction As String = "Deduction"

        Public ReadOnly All As String() = {Allowance, Compensation, Bonus, Deduction}

        ''' <summary>Remark ng result line na galing sa Fix Transaction (RefId = FixTxnId).</summary>
        Public Const ResultRemark As String = "Fix Transaction"
    End Module

End Namespace
