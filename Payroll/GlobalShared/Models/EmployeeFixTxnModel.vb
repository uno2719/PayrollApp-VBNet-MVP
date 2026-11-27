' File: GlobalShared/Models/EmployeeFixTxnModel.vb
' Isang row ng tblEmployeeFixTxn. Nasa GlobalShared dahil ginagamit ng Employee module (maintenance)
' at ng Payroll (Input Entry hints at Process).
Imports Payroll.GlobalShared.Constants

Namespace GlobalShared.Models

    Public Class EmployeeFixTxnModel
        Public Property FixTxnId As Integer
        Public Property RecordId As Integer
        Public Property TxnCategory As String          ' COMPENSATION / BONUS / DEDUCTION
        Public Property TxnCode As String
        Public Property Amount As Decimal              ' peso (Amount unit) o dami (Days/Hours/Minutes)
        Public Property StartYear As Integer
        Public Property StartMonth As Integer
        Public Property EndYear As Integer?
        Public Property EndMonth As Integer?
        Public Property Frequency As String = FixTxnFrequency.EveryCycle
        Public Property PayOn As String                ' Nothing kapag Every Cycle
        Public Property IsActive As Boolean = True
        Public Property Remarks As String
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ' ---- Galing sa catalog (JOIN), para sa display lang ----
        Public Property Description As String
        Public Property InputUnit As PayrollInputUnit = PayrollInputUnit.Amount
        Public Property IsPayslipAllowance As Boolean
        Public Property CodeIsActive As Boolean = True

        ''' <summary>True kapag may result line na sa payroll (hindi na pwedeng i-Delete; Set Inactive na lang).</summary>
        Public Property IsUsed As Boolean

        Public ReadOnly Property StartKey As Integer
            Get
                Return StartYear * 100 + StartMonth
            End Get
        End Property

        ''' <summary>YYYYMM ng End; Nothing = tuloy-tuloy.</summary>
        Public ReadOnly Property EndKey As Integer?
            Get
                If EndYear.HasValue AndAlso EndMonth.HasValue Then Return EndYear.Value * 100 + EndMonth.Value
                Return Nothing
            End Get
        End Property

        ''' <summary>"Allowance" / "Compensation" / "Bonus" / "Deduction".</summary>
        Public ReadOnly Property TypeText As String
            Get
                Select Case If(TxnCategory, "").Trim().ToUpperInvariant()
                    Case PayrollInputUnits.CategoryCompensation
                        Return If(IsPayslipAllowance, FixTxnType.Allowance, FixTxnType.Compensation)
                    Case PayrollInputUnits.CategoryBonus : Return FixTxnType.Bonus
                    Case PayrollInputUnits.CategoryDeduction : Return FixTxnType.Deduction
                    Case Else : Return TxnCategory
                End Select
            End Get
        End Property
    End Class

    ''' <summary>Isang code na pwedeng piliin sa Fix Transaction (galing sa tblCompensation / tblBonus / tblDeduction).</summary>
    Public Class FixTxnCodeOption
        Public Property TxnCategory As String
        Public Property CodeId As Integer
        Public Property Code As String
        Public Property Description As String
        Public Property InputUnit As PayrollInputUnit = PayrollInputUnit.Amount
        Public Property IsPayslipAllowance As Boolean
        Public Property IsActive As Boolean = True

        Public ReadOnly Property TypeText As String
            Get
                Select Case If(TxnCategory, "").Trim().ToUpperInvariant()
                    Case PayrollInputUnits.CategoryCompensation
                        Return If(IsPayslipAllowance, FixTxnType.Allowance, FixTxnType.Compensation)
                    Case PayrollInputUnits.CategoryBonus : Return FixTxnType.Bonus
                    Case Else : Return FixTxnType.Deduction
                End Select
            End Get
        End Property

        ''' <summary>Hal. "MEAL - Meal Allowance".</summary>
        Public ReadOnly Property DisplayText As String
            Get
                Return If(String.IsNullOrWhiteSpace(Description), Code, $"{Code} - {Description}")
            End Get
        End Property
    End Class

End Namespace
