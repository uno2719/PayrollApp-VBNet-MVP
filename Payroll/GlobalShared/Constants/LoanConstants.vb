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

End Namespace