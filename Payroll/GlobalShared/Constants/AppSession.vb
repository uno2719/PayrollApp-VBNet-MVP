' Shared/Constants/AppSession.vb
Namespace GlobalShared.Constants
    Public Module AppSession
        Public Property CurrentUser As String = String.Empty
        Public Property CurrentUserRecordID As Integer = 0
        Public Property IsAdmin As Boolean = False
        Public Property EmployeeNo As String = String.Empty
        Public Property DisplayName As String = String.Empty
    End Module
End Namespace