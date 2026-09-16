' File: GlobalShared/Models/GeneralSettingsModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row lang palagi mula sa tblGeneralSettings (parehong pattern
    ''' ng CompanyModel) - ang mga core payroll calculation parameters na
    ''' hindi kabilang sa Payroll Settings (Compensation/Deduction/Overtime/
    ''' Holiday/Bonus/Loan) at hindi rin kabilang sa Company Profile.
    '''
    ''' GeneralSettingsId = 0 ang ibig sabihin ay wala pang na-Insert
    ''' kailanman (bagong install) - ang Service ang bahalang magdesisyon
    ''' kung Insert o Update ang tatawagin sa Save.
    ''' </summary>
    Public Class GeneralSettingsModel
        Public Property GeneralSettingsId As Integer

        ' --- Payroll Parameters ---
        Public Property BonusCeiling As Decimal
        Public Property TotalDaysPerYear As Integer = 365
        Public Property WorkHourPerDay As Decimal = 8D
        Public Property AmountPrecision As Integer = 2
        Public Property PercentPrecision As Integer = 2

        ' --- Code Mapping (FK papunta sa tblCompensation / tblDeduction) ---
        ' Nullable dahil pwedeng hindi pa naka-set sa bagong install, o
        ' hindi talaga ginagamit ng company (hal. walang Early Out policy).
        Public Property BasicSalaryCodePlusId As Integer?
        Public Property BasicSalaryCodeMinusId As Integer?
        Public Property AbsentCodeId As Integer?
        Public Property LateInCodeId As Integer?
        Public Property EarlyOutCodeId As Integer?

        ' --- Statutory Basis ---
        Public Property SSSBasedOn As String = "Progressive Earnings"
        Public Property PhilHealthBasedOn As String = "Fixed Basic"

        ' --- Audit ---
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace