' ============================================================
' GlobalShared/Models/EmployeeLoanScheduleModel.vb
' ============================================================
' Isang hulog. Ito ang lalabas sa gridview ng Details window -
' "kelan kinaltas, magkano, bayad na ba".
' ============================================================
Namespace GlobalShared.Models

    Public Class EmployeeLoanScheduleModel

        Public Property Id As Integer
        Public Property EmployeeLoanId As Integer

        Public Property InstallmentNo As Integer
        Public Property ScheduledDate As Date

        Public Property PrincipalDue As Decimal
        Public Property InterestDue As Decimal
        Public Property AmountDue As Decimal

        Public Property AmountPaid As Decimal
        Public Property PaidDate As Date?
        Public Property IsPaid As Boolean

        Public Property PayrollRefNo As String
        Public Property Remark As String

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ''' <summary>
        ''' Text na makikita sa grid. Tatlong estado, hindi dalawa:
        ''' bayad na, lumipas na ang petsa pero hindi pa nakaltas
        ''' (dapat mapansin ito ng HR), at hindi pa dumarating.
        ''' </summary>
        Public ReadOnly Property StatusText As String
            Get
                If IsPaid Then Return "Paid"
                If ScheduledDate < Date.Today Then Return "Overdue"
                Return "Scheduled"
            End Get
        End Property

        Public ReadOnly Property BalanceDue As Decimal
            Get
                Dim bal = AmountDue - AmountPaid
                Return If(bal < 0D, 0D, bal)
            End Get
        End Property

    End Class


    ''' <summary>
    ''' Ang buong laman ng Details window sa isang bagsak:
    ''' ang loan mismo + ang buong hulugan niya.
    '''
    ''' Isang service call lang para dito, hindi dalawa -
    ''' kaya walang panahon kung saan may loan ka nang nakikita
    ''' pero wala pang schedule sa grid.
    ''' </summary>
    Public Class EmployeeLoanDetailModel
        Public Property Loan As EmployeeLoanModel
        Public Property Schedule As List(Of EmployeeLoanScheduleModel)
    End Class

End Namespace