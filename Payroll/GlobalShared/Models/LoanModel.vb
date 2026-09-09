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

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
