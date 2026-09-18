Namespace Payroll.GlobalShared.Security

    Public NotInheritable Class AuthorizationService

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property IsAdmin As Boolean
            Get
                Return AppSession.IsAdmin
            End Get
        End Property

        Public Shared Function CanAccessAdminArea() As Boolean
            Return AppSession.IsAdmin
        End Function

        Public Shared Function CanAccess(permission As String) As Boolean
            If String.IsNullOrWhiteSpace(permission) Then
                Return False
            End If

            ' Temporary implementation:
            ' currently Administration permissions are Admin-only.
            If permission.StartsWith("admin_", StringComparison.OrdinalIgnoreCase) Then
                Return AppSession.IsAdmin
            End If

            Return True
        End Function

    End Class

End Namespace