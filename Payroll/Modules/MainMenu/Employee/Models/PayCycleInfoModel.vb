Namespace Employee.Models

    ''' <summary>
    ''' Isang pay cycle (tblPayCycle) na may Rate Basis at Active status. Ginagamit ng Earnings tab para malaman
    ''' kung Basic Salary (0) o Daily Rate (1) ang input ng employee, ayon sa napiling Pay Cycle.
    ''' </summary>
    Public Class PayCycleInfoModel
        Public Property PayCycleType As String
        Public Property RateBasis As Byte
        Public Property IsActive As Boolean
    End Class

End Namespace
