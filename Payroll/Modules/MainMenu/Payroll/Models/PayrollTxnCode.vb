Namespace Modules.Payroll.Models

    ''' <summary>
    ''' Whether a user-entered grid value is a Quantity (hours/days/minutes — the
    ''' peso Rate for it is resolved elsewhere, e.g. Employee daily rate or an
    ''' Overtime/Holiday rate table) or a direct Amount (the peso value itself,
    ''' Qty stored as 1).
    ''' </summary>
    Public Enum PayrollTxnValueMode
        Quantity
        Amount
    End Enum

    ''' <summary>
    ''' Fixed TxnCode list for Payroll Input Entry, carried over from C1Pay's
    ''' actual transaction uploader (Latest_Transaction_Uploader_SAMPLE.xlsm),
    ''' plus UNDT/ABSN which that reference file did not have.
    ''' </summary>
    Public NotInheritable Class PayrollTxnCode

        Public Const Basic As String = "BASIC"
        Public Const RegularHours As String = "OREG"
        Public Const SpecialHoliday1 As String = "OSH1"
        Public Const SpecialHoliday2 As String = "OSH2"
        Public Const SpecialHoliday3 As String = "OSH3"
        Public Const LegalHoliday1 As String = "OLH1"
        Public Const LegalHoliday2 As String = "OLH2"
        Public Const LegalHoliday3 As String = "OLH3"
        Public Const RestdayOT1 As String = "ORD1"
        Public Const RestdayOT2 As String = "ORD2"
        Public Const RestdayOnLegal1 As String = "OLR1"
        Public Const RestdayOnLegal2 As String = "OLR2"
        Public Const RestdayOnSpecial1 As String = "OSR1"
        Public Const RestdayOnSpecial2 As String = "OSR2"
        Public Const NightDifferential As String = "NDIF"
        Public Const ServiceIncentiveLeave As String = "SIL"
        Public Const OtherIncomeOthers As String = "OIO"
        Public Const OtherIncomeCommission1 As String = "OIC1"
        Public Const OtherIncomeCommission2 As String = "OIC2"
        Public Const OtherIncomeCommission3 As String = "OIC3"
        Public Const Late As String = "LATE"
        Public Const Undertime As String = "UNDT"    ' new — not in the C1Pay reference file
        Public Const Absence As String = "ABSN"       ' new — not in the C1Pay reference file
        Public Const LeaveWithoutPay As String = "LWOP"

        ''' <summary>TxnType classification per code: A=Addition, O=Overtime/Holiday, D=Deduction.</summary>
        Public Shared ReadOnly TxnTypeMap As New Dictionary(Of String, Char) From {
            {Basic, "A"c},
            {RegularHours, "O"c},
            {SpecialHoliday1, "O"c}, {SpecialHoliday2, "O"c}, {SpecialHoliday3, "O"c},
            {LegalHoliday1, "O"c}, {LegalHoliday2, "O"c}, {LegalHoliday3, "O"c},
            {RestdayOT1, "O"c}, {RestdayOT2, "O"c},
            {RestdayOnLegal1, "O"c}, {RestdayOnLegal2, "O"c},
            {RestdayOnSpecial1, "O"c}, {RestdayOnSpecial2, "O"c},
            {NightDifferential, "A"c},
            {ServiceIncentiveLeave, "A"c},
            {OtherIncomeOthers, "A"c},
            {OtherIncomeCommission1, "A"c}, {OtherIncomeCommission2, "A"c}, {OtherIncomeCommission3, "A"c},
            {Late, "D"c}, {Undertime, "D"c}, {Absence, "D"c}, {LeaveWithoutPay, "D"c}
        }

        ''' <summary>
        ''' Value mode per code. NightDifferential is set to Amount, following the reference
        ''' sheet's written instruction ("Entry MUST be in Amount form") rather than its row-9
        ''' metadata marker, which showed "QTY" for every single column in the sample —
        ''' almost certainly a copy-paste artifact, not a deliberate setting. OtherIncome
        ''' Others/Commission are left as Quantity to match the sheet literally, since that
        ''' wasn't something Uno explicitly confirmed either way; flag for review if these
        ''' should also be Amount.
        ''' </summary>
        Public Shared ReadOnly ValueModeMap As New Dictionary(Of String, PayrollTxnValueMode) From {
            {Basic, PayrollTxnValueMode.Quantity},
            {RegularHours, PayrollTxnValueMode.Quantity},
            {SpecialHoliday1, PayrollTxnValueMode.Quantity}, {SpecialHoliday2, PayrollTxnValueMode.Quantity}, {SpecialHoliday3, PayrollTxnValueMode.Quantity},
            {LegalHoliday1, PayrollTxnValueMode.Quantity}, {LegalHoliday2, PayrollTxnValueMode.Quantity}, {LegalHoliday3, PayrollTxnValueMode.Quantity},
            {RestdayOT1, PayrollTxnValueMode.Quantity}, {RestdayOT2, PayrollTxnValueMode.Quantity},
            {RestdayOnLegal1, PayrollTxnValueMode.Quantity}, {RestdayOnLegal2, PayrollTxnValueMode.Quantity},
            {RestdayOnSpecial1, PayrollTxnValueMode.Quantity}, {RestdayOnSpecial2, PayrollTxnValueMode.Quantity},
            {NightDifferential, PayrollTxnValueMode.Amount},
            {ServiceIncentiveLeave, PayrollTxnValueMode.Quantity},
            {OtherIncomeOthers, PayrollTxnValueMode.Quantity},
            {OtherIncomeCommission1, PayrollTxnValueMode.Quantity},
            {OtherIncomeCommission2, PayrollTxnValueMode.Quantity},
            {OtherIncomeCommission3, PayrollTxnValueMode.Quantity},
            {Late, PayrollTxnValueMode.Quantity},
            {Undertime, PayrollTxnValueMode.Quantity},
            {Absence, PayrollTxnValueMode.Quantity},
            {LeaveWithoutPay, PayrollTxnValueMode.Quantity}
        }

        ''' <summary>All codes, in the fixed display/grid-column order.</summary>
        Public Shared ReadOnly AllCodesInOrder As String() = {
            Basic, RegularHours,
            SpecialHoliday1, SpecialHoliday2, SpecialHoliday3,
            LegalHoliday1, LegalHoliday2, LegalHoliday3,
            RestdayOT1, RestdayOT2, RestdayOnLegal1, RestdayOnLegal2, RestdayOnSpecial1, RestdayOnSpecial2,
            NightDifferential, ServiceIncentiveLeave,
            OtherIncomeOthers, OtherIncomeCommission1, OtherIncomeCommission2, OtherIncomeCommission3,
            Late, Undertime, Absence, LeaveWithoutPay
        }

    End Class

End Namespace
