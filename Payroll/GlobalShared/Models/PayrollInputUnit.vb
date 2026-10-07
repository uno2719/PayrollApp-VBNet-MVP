' File: GlobalShared/Models/PayrollInputUnit.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Anong klaseng value ang tinatanggap ng isang column sa Payroll Input Entry.
    ''' Itinatakda ito sa bawat catalog row (Compensation/Deduction/Overtime/Holiday/Bonus)
    ''' sa Payroll Settings, kaya malinaw kung ang "2" ay 2 oras, 2 araw, o 2 piso.
    '''   Hours / Minutes / Days = Quantity (ang halaga ay kinukuha sa rate ng employee)
    '''   Amount                 = direktang peso
    ''' </summary>
    Public Enum PayrollInputUnit As Byte
        Hours = 0
        Minutes = 1
        Days = 2
        Amount = 3
    End Enum

    Public Module PayrollInputUnits

        ' TxnCategory ng tblPayrollInputTxn (kapareho ng PayrollInputCategory.ToString().ToUpperInvariant())
        Public Const CategoryOvertime As String = "OVERTIME"
        Public Const CategoryHoliday As String = "HOLIDAY"
        Public Const CategoryCompensation As String = "COMPENSATION"
        Public Const CategoryBonus As String = "BONUS"
        Public Const CategoryDeduction As String = "DEDUCTION"

        Public Function Text(unit As PayrollInputUnit) As String
            Select Case unit
                Case PayrollInputUnit.Hours : Return "Hours"
                Case PayrollInputUnit.Minutes : Return "Minutes"
                Case PayrollInputUnit.Days : Return "Days"
                Case Else : Return "Amount"
            End Select
        End Function

        ''' <summary>Idinudugtong sa caption ng column sa Input Entry, hal. "Overtime Regular (hrs)".</summary>
        Public Function CaptionSuffix(unit As PayrollInputUnit) As String
            Select Case unit
                Case PayrollInputUnit.Hours : Return " (hrs)"
                Case PayrollInputUnit.Minutes : Return " (mins)"
                Case PayrollInputUnit.Days : Return " (days)"
                Case Else : Return " (amt)"
            End Select
        End Function

        Public Function IsAmount(unit As PayrollInputUnit) As Boolean
            Return unit = PayrollInputUnit.Amount
        End Function

        ''' <summary>Ang TxnCategory para sa isang catalog table (tblOvertime, tblHoliday, tblBonus, tblDeduction).</summary>
        Public Function CategoryForTable(tableName As String) As String
            Select Case If(tableName, "").Trim().ToLowerInvariant()
                Case "tblovertime" : Return CategoryOvertime
                Case "tblholiday" : Return CategoryHoliday
                Case "tblbonus" : Return CategoryBonus
                Case "tbldeduction" : Return CategoryDeduction
                Case Else : Throw New ArgumentException($"Unknown payroll catalog table '{tableName}'.", NameOf(tableName))
            End Select
        End Function

        ''' <summary>Default na unit ng BAGONG catalog row (pwede pa ring palitan ng user).</summary>
        Public Function DefaultForTable(tableName As String) As PayrollInputUnit
            Select Case CategoryForTable(tableName)
                Case CategoryOvertime : Return PayrollInputUnit.Hours
                Case CategoryHoliday : Return PayrollInputUnit.Days
                Case Else : Return PayrollInputUnit.Amount
            End Select
        End Function

    End Module

End Namespace
