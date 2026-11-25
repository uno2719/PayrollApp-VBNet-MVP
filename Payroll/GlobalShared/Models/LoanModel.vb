Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblLoan. Sariling shape ito - Loan Type combo
    ''' (Others/Company Loan/PAG-IBIG Loan/SSS Loan) imbes na yung
    ''' Tax/SSS/PhilHealth/Pag-IBIG flags na nasa ibang 5 tabs.
    ''' </summary>
    Public Class LoanModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Description As String
        Public Property LoanType As String
        Public Property IsActive As Boolean = True

        ' --- Default na interes para sa BAGONG employee loan na gagamit ng code na ito ---
        ' Snapshot lang: kinokopya sa loan pagpili ng Loan Code (New lang) at editable pa rin doon.
        ' Ang pagbabago dito ay HINDI gumagalaw sa mga naka-save nang loan.
        ''' <summary>LoanInterestMethod.* o Nothing (walang default).</summary>
        Public Property DefaultInterestMethod As String
        Public Property DefaultInterestRate As Decimal?
        ''' <summary>LoanRatePeriod.* (Diminishing lang ang gumagamit).</summary>
        Public Property DefaultRatePeriod As String

        Public ReadOnly Property HasInterestDefaults As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(DefaultInterestMethod)
            End Get
        End Property

        ''' <summary>Para sa grid: "Diminishing, 8.0000% per year".</summary>
        Public ReadOnly Property DefaultInterestText As String
            Get
                If Not HasInterestDefaults Then Return String.Empty
                Dim text = DefaultInterestMethod
                If DefaultInterestRate.HasValue Then
                    text &= $", {DefaultInterestRate.Value:0.####}%"
                    If String.Equals(DefaultInterestMethod, Constants.LoanInterestMethod.Diminishing, StringComparison.OrdinalIgnoreCase) Then
                        text &= If(String.Equals(DefaultRatePeriod, Constants.LoanRatePeriod.PerYear, StringComparison.OrdinalIgnoreCase),
                                   " per year", " per period")
                    End If
                End If
                Return text
            End Get
        End Property

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
