Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblLeaveType. Halos kapareho ng LookupModel
    ''' (Id/Code/Name/IsActive/audit) PLUS ang Category (Annual/
    ''' Maternity/Paternity/Unpaid) - kaya hindi na natin ini-merge
    ''' sa LookupModel/ucLookupMaintenance mismo (para hindi na
    ''' kailangang lagyan ng special-case ang generic Lookup control
    ''' para lang sa isang table na ito).
    ''' </summary>
    Public Class LeaveTypeModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Name As String
        Public Property Category As String
        Public Property IsActive As Boolean = True

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
