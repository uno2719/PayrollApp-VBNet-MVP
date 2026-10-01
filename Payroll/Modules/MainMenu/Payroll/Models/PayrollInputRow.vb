Namespace Modules.Payroll.Models

    ''' <summary>
    ''' Flat, grid-friendly view of one employee's timekeeping input for a given Cutoff.
    ''' Each property below maps to a fixed TxnCode in tblPayrollInputTxn (see
    ''' PayrollTxnCode). PayrollRepository flattens/unflattens between this shape
    ''' and the underlying transaction rows — the DB itself stays a generic,
    ''' code-driven log, but the grid and the rest of the UI never deal with
    ''' magic strings.
    ''' </summary>
    Public Class PayrollInputRow

        ' -- Reference columns (AllowEdit = False in the grid) --
        Public Property EmployeeID As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String

        ' -- Additions --
        Public Property BasicSalary As Decimal                ' BASIC
        Public Property RegularHours As Decimal                ' OREG
        Public Property SpecialHoliday1 As Decimal             ' OSH1
        Public Property SpecialHoliday2 As Decimal             ' OSH2
        Public Property SpecialHoliday3 As Decimal             ' OSH3
        Public Property LegalHoliday1 As Decimal               ' OLH1
        Public Property LegalHoliday2 As Decimal               ' OLH2
        Public Property LegalHoliday3 As Decimal               ' OLH3
        Public Property RestdayOT1 As Decimal                  ' ORD1
        Public Property RestdayOT2 As Decimal                  ' ORD2
        Public Property RestdayOnLegal1 As Decimal             ' OLR1
        Public Property RestdayOnLegal2 As Decimal             ' OLR2
        Public Property RestdayOnSpecial1 As Decimal           ' OSR1
        Public Property RestdayOnSpecial2 As Decimal           ' OSR2
        Public Property NightDifferential As Decimal           ' NDIF — Amount mode
        Public Property ServiceIncentiveLeave As Decimal       ' SIL
        Public Property OtherIncomeOthers As Decimal           ' OIO
        Public Property OtherIncomeCommission1 As Decimal      ' OIC1
        Public Property OtherIncomeCommission2 As Decimal      ' OIC2
        Public Property OtherIncomeCommission3 As Decimal      ' OIC3

        ' -- Deductions --
        Public Property Late As Decimal                        ' LATE, minutes
        Public Property Undertime As Decimal                   ' UNDT, minutes
        Public Property Absence As Decimal                     ' ABSN, days
        Public Property LeaveWithoutPay As Decimal             ' LWOP, days

    End Class

End Namespace
