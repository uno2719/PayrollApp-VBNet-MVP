' ============================================================
' GlobalShared/Security/AuthorizationService.vb  (PALITAN ANG LUMA)
' ============================================================
' Dati ay may "Temporary implementation" ka rito na basta
' nagre-return ng True para sa lahat maliban sa "admin_".
' Ngayon, idinu-dugtong na natin siya sa totoong PermissionService.
'
' Pinanatili ko ang lumang public surface (IsAdmin, CanAccessAdminArea,
' CanAccess) para walang masirang existing caller.
' ============================================================
Namespace GlobalShared.Security

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

        ' Dating "Return True" na lang sa dulo. Ngayon totoong
        ' tsinetsek na sa loaded permissions.
        Public Shared Function CanAccess(permission As String) As Boolean

            If String.IsNullOrWhiteSpace(permission) Then Return False

            ' Ang buong Administration area ay Admin-only pa rin -
            ' hindi ito nade-delegate sa module access grid.
            If permission.StartsWith("admin_", StringComparison.OrdinalIgnoreCase) Then
                Return AppSession.IsAdmin
            End If

            Return PermissionService.CanView(permission)

        End Function

        Public Shared Function CanModify(permission As String) As Boolean

            If String.IsNullOrWhiteSpace(permission) Then Return False

            If permission.StartsWith("admin_", StringComparison.OrdinalIgnoreCase) Then
                Return AppSession.IsAdmin
            End If

            Return PermissionService.CanEdit(permission)

        End Function

    End Class

End Namespace