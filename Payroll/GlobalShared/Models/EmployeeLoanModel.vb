' ============================================================
' GlobalShared/Models/EmployeeLoanModel.vb
' ============================================================
' Isang hilera mula sa tblEmployeeLoan, PLUS ilang computed na
' column na galing sa JOIN/subquery ng repository.
'
' BAKIT MAY COMPUTED PROPERTIES DITO?
'   Yung Balance, PaidCount, NextDeductionDate ay hindi naka-store
'   sa tblEmployeeLoan - kinukuwenta sila mula sa schedule table.
'   Pwede sana nating gawing hiwalay na "ListItem" class, pero
'   ibig sabihin noon ay dalawang model na halos magkapareho.
'
'   Mas simple: iisang model, ang mga derived na field ay
'   nilalagyan lang ng halaga ng SELECT na may subqueries. Kapag
'   ang tumawag ay isang plain na GetById (walang subqueries),
'   zero lang sila - at yun ay tama namang default.
' ============================================================
Namespace GlobalShared.Models

    Public Class EmployeeLoanModel

        ' --- Identity ---
        Public Property Id As Integer
        Public Property EmployeeRecordId As Integer
        Public Property LoanCode As String

        ' --- Petsa ---
        Public Property InputDate As Date = Date.Today
        Public Property LoanStartDate As Date = Date.Today

        ' --- Paraan ng kaltas ---
        Public Property Frequency As String
        Public Property AmortizationMethod As String
        Public Property InterestMethod As String
        Public Property InterestRate As Decimal
        Public Property Terms As Integer

        ''' <summary>Per Period (default, dating behavior) o Per Year. Diminishing lang. Tingnan ang LoanRatePeriod.</summary>
        Public Property RatePeriod As String = Constants.LoanRatePeriod.PerPeriod

        ''' <summary>Every Cycle lang: Per Deduction (default) o Per Month. Tingnan ang LoanAmortBasis.</summary>
        Public Property AmortBasis As String = Constants.LoanAmortBasis.PerDeduction

        ' --- Halaga ---
        Public Property PrincipalAmount As Decimal
        Public Property InterestAmount As Decimal
        Public Property TotalLoanAmount As Decimal

        Public Property PrincipalAmortization As Decimal
        Public Property InterestAmortization As Decimal
        Public Property TotalAmortization As Decimal

        ' --- Estado ---
        Public Property Status As String = Constants.LoanStatus.Active
        Public Property CompletedAt As DateTime?

        ' --- Offset (renewal) ---
        ''' <summary>Sa LUMANG loan (Status = Offset): ang bagong loan na pumalit dito, at kailan.</summary>
        Public Property OffsetByLoanId As Integer?
        Public Property OffsetDate As Date?

        ''' <summary>Sa BAGONG loan: ang credit (sobrang bayad) mula sa offset, paano ginamit, at ang refund row kung Refund.</summary>
        Public Property OffsetCredit As Decimal
        Public Property OffsetCreditMode As String
        Public Property OffsetRefundTxnId As Integer?

        ''' <summary>COMPUTED: ilang lumang loan ang na-offset ng loan na ito (0 kung wala).</summary>
        Public Property OffsetChildCount As Integer

        ''' <summary>
        ''' TRANSIENT (hindi naka-store): kung bagong loan na may offset, dala nito ang desisyon ng user mula sa
        ''' Offset dialog. Nothing = walang offset.
        ''' </summary>
        Public Property OffsetRequest As LoanOffsetRequest
        Public Property Remark As String

        ' --- Opening payments (C1Pay) ---
        ' TRANSIENT: hindi naka-store sa tblEmployeeLoan. Ginagamit lang sa pag-save ng BAGONG loan:
        ' ilan sa mga unang hulog ang nabayaran na sa C1Pay. Ang service ang magmamarka sa kanila.
        Public Property OpeningPaidInstallments As Integer

        ' --- Audit ---
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ' ========================================================
        ' COMPUTED - pinupuno lang ng list query
        ' ========================================================
        Public Property LoanDescription As String
        Public Property LoanType As String

        Public Property EmployeeNo As String
        Public Property EmployeeName As String

        Public Property TotalInstallments As Integer
        Public Property PaidInstallments As Integer
        Public Property TotalPaid As Decimal
        Public Property NextDeductionDate As Date?
        Public Property NextDeductionAmount As Decimal

        Public ReadOnly Property Balance As Decimal
            Get
                Dim bal = TotalLoanAmount - TotalPaid
                Return If(bal < 0D, 0D, bal)
            End Get
        End Property

        Public ReadOnly Property IsFullyPaid As Boolean
            Get
                If TotalInstallments <= 0 Then Return False
                Return PaidInstallments >= TotalInstallments
            End Get
        End Property

        Public ReadOnly Property RemainingInstallments As Integer
            Get
                Dim remaining = TotalInstallments - PaidInstallments
                Return If(remaining < 0, 0, remaining)
            End Get
        End Property

        ''' <summary>True kung ito ang LUMANG loan na na-offset na.</summary>
        Public ReadOnly Property IsOffsetOld As Boolean
            Get
                Return String.Equals(Status, Constants.LoanStatus.Offset, StringComparison.OrdinalIgnoreCase)
            End Get
        End Property

        ''' <summary>True kung ito ang BAGONG loan na nag-offset ng lumang loan.</summary>
        Public ReadOnly Property HasOffsetChildren As Boolean
            Get
                Return OffsetChildCount > 0
            End Get
        End Property

        ''' <summary>
        ''' Para sa display: "3 of 24 paid"
        ''' </summary>
        Public ReadOnly Property ProgressText As String
            Get
                Return $"{PaidInstallments} of {TotalInstallments} paid"
            End Get
        End Property

    End Class

    ''' <summary>Isang lumang loan na pwedeng i-offset (para sa Offset dialog).</summary>
    Public Class LoanOffsetCandidate
        Public Property LoanId As Integer
        Public Property LoanCode As String
        Public Property LoanStartDate As Date
        Public Property Status As String
        Public Property TotalLoanAmount As Decimal
        Public Property TotalAmortization As Decimal
        Public Property TotalInstallments As Integer
        Public Property UnpaidCount As Integer
        ''' <summary>Kabuuan ng (AmountDue - AmountPaid) ng mga hindi pa bayad na hulog.</summary>
        Public Property UnpaidAmount As Decimal
        ''' <summary>May laman kung bawal i-offset (hal. nasa Processed na cutoff ang hulog).</summary>
        Public Property BlockedReason As String

        ''' <summary>Para sa checklist ng dialog.</summary>
        Public Property Selected As Boolean = True

        Public ReadOnly Property IsBlocked As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(BlockedReason)
            End Get
        End Property
    End Class

    ''' <summary>Ang desisyon ng user sa Offset dialog.</summary>
    Public Class LoanOffsetRequest
        Public Property OldLoanIds As New List(Of Integer)
        Public Property OffsetDate As Date = Date.Today
        ''' <summary>Optional: balanse ng lumang loan ayon sa SSS/Pag-IBIG statement (reference + basehan ng credit).</summary>
        Public Property BalancePerStatement As Decimal?
        ''' <summary>LoanCreditMode.* kapag may credit (hindi pinapansin kung walang credit).</summary>
        Public Property CreditMode As String
    End Class

    ''' <summary>Ang pinili sa Offset dialog.</summary>
    Public Enum LoanOffsetChoice
        Cancel = 0
        KeepBoth = 1
        Offset = 2
    End Enum

End Namespace
