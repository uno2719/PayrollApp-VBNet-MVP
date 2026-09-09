Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblCompensation (dating "Allowance" sa C1Pay
    ''' reference). Sariling shape ito - hindi na-share sa ibang Payroll
    ''' Settings tabs kasi may extra fields (2316 Component, Deminimis,
    ''' Ceiling, Frequency) na wala sa Deduction/Bonus/Overtime/Holiday/Loan.
    ''' </summary>
    Public Class CompensationModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Description As String
        Public Property TaxFlag As Boolean
        Public Property SSSFlag As Boolean
        Public Property PhilHealthFlag As Boolean
        Public Property PagIbigFlag As Boolean
        Public Property Component2316 As String
        Public Property DeminimisFlag As Boolean
        Public Property CeilingAmount As Decimal
        Public Property Frequency As String
        Public Property IsActive As Boolean = True

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
