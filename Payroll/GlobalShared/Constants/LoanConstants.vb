' ============================================================
' GlobalShared/Constants/LoanConstants.vb
' ============================================================
' Mga string na pinagsasaluhan ng database, combo boxes, at
' business rules. Constants para pag nagkamali ka ng spelling,
' compile error - hindi tahimik na maling behavior.
' ============================================================
Namespace GlobalShared.Constants

    Public Module LoanStatus

        Public Const Active As String = "Active"
        Public Const Inactive As String = "Inactive"
        Public Const Completed As String = "Completed"
        Public Const Cancelled As String = "Cancelled"

        ''' <summary>
        ''' Napalitan ng bagong loan (renewal): ang natitirang hulog ay binayaran ng bagong loan (hal. SSS offset).
        ''' Hindi na kinakaltasan; ang tanging balik ay ang "Undo offset" (Admin lang).
        ''' </summary>
        Public Const Offset As String = "Offset"

        ''' <summary>
        ''' Kinakaltasan ba talaga ito sa payroll?
        ''' Ang Inactive (naka-hold), Completed at Cancelled ay hindi.
        ''' Ito ang tanging tanong na dapat itanong ng Payroll module
        ''' mamaya - kaya isang function lang, hindi kalat-kalat na
        ''' If Status = "Active" sa buong codebase.
        ''' </summary>
        Public Function IsDeductible(status As String) As Boolean
            Return String.Equals(status, Active, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Pwede pa bang i-toggle ng user ang Active/Inactive?
        ''' Hindi na kung tapos na o kinansela - final na ang mga iyon.
        ''' </summary>
        Public Function IsToggleable(status As String) As Boolean
            Return String.Equals(status, Active, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(status, Inactive, StringComparison.OrdinalIgnoreCase)
        End Function

    End Module


    Public Module LoanFrequency

        Public Const EveryCycle As String = "Every Cycle"
        Public Const FirstCycleOnly As String = "First Cycle Only"
        Public Const SecondCycleOnly As String = "Second Cycle Only"
        Public Const Monthly As String = "Monthly"

        Public ReadOnly All As String() = {
            EveryCycle, FirstCycleOnly, SecondCycleOnly, Monthly
        }

        ''' <summary>
        ''' Ilang kaltas kada buwan. Ginagamit ng schedule generator.
        ''' </summary>
        Public Function DeductionsPerMonth(frequency As String) As Integer
            Select Case frequency
                Case EveryCycle : Return 2
                Case Else : Return 1
            End Select
        End Function

    End Module


    Public Module LoanAmortizationMethod
        ' "Ilang hulog?"           -> ikaw ang magsasabi ng Terms
        Public Const BasedOnTerm As String = "Based on Term"
        ' "Magkano kada hulog?"    -> ikaw ang magsasabi ng Amortization
        Public Const BasedOnAmort As String = "Based on Amort"
    End Module


    Public Module LoanInterestMethod
        Public Const StraightLine As String = "Straight Line"
        Public Const Diminishing As String = "Diminishing"
    End Module


    ''' <summary>
    ''' Mga halaga ng PayrollRefNo sa tblEmployeeLoanSchedule na hindi galing sa Post ng cutoff (PR-{CutoffID}).
    ''' </summary>
    Public Module LoanPaymentRef

        ''' <summary>
        ''' Hulog na nabayaran na sa C1Pay bago ginamit ang app na ito ("opening payment").
        ''' </summary>
        Public Const OpeningBalance As String = "C1PAY"

        ''' <summary>Hulog ng LUMANG loan na binayaran ng bagong loan (offset). Format: OFFSET-{newLoanId}.</summary>
        Public Const OffsetPrefix As String = "OFFSET-"

        ''' <summary>Hulog ng BAGONG loan na binayaran ng credit mula sa offset. Format: CREDIT-{newLoanId}.</summary>
        Public Const CreditPrefix As String = "CREDIT-"

        Public Function IsOpening(payrollRefNo As String) As Boolean
            Return String.Equals(payrollRefNo, OpeningBalance, StringComparison.OrdinalIgnoreCase)
        End Function

        Public Function IsOffset(payrollRefNo As String) As Boolean
            Return Not String.IsNullOrEmpty(payrollRefNo) AndAlso
                   payrollRefNo.StartsWith(OffsetPrefix, StringComparison.OrdinalIgnoreCase)
        End Function

        Public Function IsCredit(payrollRefNo As String) As Boolean
            Return Not String.IsNullOrEmpty(payrollRefNo) AndAlso
                   payrollRefNo.StartsWith(CreditPrefix, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Bayad na HINDI galing sa Payroll Post (C1PAY, OFFSET-, CREDIT-). Hindi ito "posted deduction",
        ''' kaya hindi nito nilo-lock ang Mark paid / Undo ng loan.
        ''' </summary>
        Public Function IsSystemAdjustment(payrollRefNo As String) As Boolean
            Return IsOpening(payrollRefNo) OrElse IsOffset(payrollRefNo) OrElse IsCredit(payrollRefNo)
        End Function

    End Module


    ''' <summary>
    ''' Ang Interest Rate ba ay PER PERIOD (kada hulog, hal. 0.6667 kada buwan) o PER YEAR (hal. 8 = 8% kada taon,
    ''' hahatiin ng engine sa bilang ng hulog kada taon nang buong precision)? Diminishing lang ang gumagamit nito.
    ''' Ang string ay ito rin ang laman ng database at ng dropdown.
    ''' </summary>
    Public Module LoanRatePeriod
        Public Const PerPeriod As String = "Per Period"
        Public Const PerYear As String = "Per Year"
        Public ReadOnly All As String() = {PerPeriod, PerYear}
    End Module


    ''' <summary>
    ''' Para sa Frequency = Every Cycle lang: ang "Total Amortization" ba ay KADA KALTAS (dating behavior)
    ''' o KADA BUWAN (hahatiin sa dalawang kaltas ng buwan, hal. buwanang hulog ng SSS)?
    ''' Sa ibang Frequency buwanan na talaga ang hulog, kaya walang epekto ito.
    ''' </summary>
    Public Module LoanAmortBasis
        Public Const PerDeduction As String = "Per Deduction"
        Public Const PerMonth As String = "Per Month"
        Public ReadOnly All As String() = {PerDeduction, PerMonth}
    End Module


    ''' <summary>Ano ang gagawin sa sobrang bayad (credit) kapag nag-offset. Ito rin ang naka-save sa OffsetCreditMode.</summary>
    Public Module LoanCreditMode
        Public Const RefundThroughPayroll As String = "Refund"
        Public Const ApplyToFirst As String = "First"
        Public Const ApplyToLast As String = "Last"
    End Module


    ''' <summary>TxnSrc ng refund na ginawa ng offset sa tblPayrollInputTxn (hindi ito ginagalaw ng Input Entry Save).</summary>
    Public Module LoanTxnSource
        Public Const OffsetRefund As String = "LOANOFS"
    End Module

End Namespace
