Namespace PayrollProcessing.Models

    Public Enum PayrollTxnValueMode
        Quantity    ' hours/days/minutes — Rate resolved elsewhere (catalog Rate x Employee rate)
        Amount      ' direct peso value — Qty stored as 1
    End Enum

    ''' <summary>
    ''' The handful of codes that are intrinsic to timekeeping itself —
    ''' NOT maintained catalogs like Overtime/Holiday/Compensation/Bonus,
    ''' so they stay as fixed constants rather than driven by a table.
    ''' </summary>
    Public NotInheritable Class CoreTxnCode
        Public Const Basic As String = "BASIC"
        Public Const RegularHours As String = "OREG"
        Public Const Late As String = "LATE"
        Public Const LeaveWithoutPay As String = "LWOP"         ' covers absence/undertime — no separate UNDT/ABSN codes
        Public Const NightDifferential As String = "NDIF"       ' Amount mode — see design notes
        Public Const ServiceIncentiveLeave As String = "SIL"

        ''' <summary>Fixed columns, in display order, with caption + value mode.</summary>
        Public Shared ReadOnly Columns As (Code As String, Caption As String, Mode As PayrollTxnValueMode)() = {
            (Basic, "Basic Salary", PayrollTxnValueMode.Quantity),
            (RegularHours, "Reg. Hours", PayrollTxnValueMode.Quantity),
            (Late, "Late (mins)", PayrollTxnValueMode.Quantity),
            (LeaveWithoutPay, "VL W/O Pay (days)", PayrollTxnValueMode.Quantity),
            (NightDifferential, "Night Diff. (Amount)", PayrollTxnValueMode.Amount),
            (ServiceIncentiveLeave, "SIL", PayrollTxnValueMode.Quantity)
        }
    End Class

    Public Enum PayrollInputCategory
        Core
        Overtime
        Holiday
        Compensation
        Bonus
        Deduction
    End Enum

    ''' <summary>
    ''' One grid column's metadata — whether it's a fixed Core code or came
    ''' from a catalog row (Overtime/Holiday/Compensation/Bonus), and what
    ''' DataTable column name it's bound to.
    ''' </summary>
    Public Class PayrollInputColumnModel
        Public Property ColumnName As String        ' DataTable column name (= TxnCode, must be unique)
        Public Property Caption As String
        Public Property Category As PayrollInputCategory
        Public Property ValueMode As PayrollTxnValueMode
        Public Property IsEssential As Boolean       ' shown by default; others start hidden via Column Chooser
    End Class

End Namespace