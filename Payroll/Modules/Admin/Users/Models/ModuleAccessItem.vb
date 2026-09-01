Namespace Users.Models

    Public Enum ModuleAccessLevel
        NoAccess
        ViewOnly
        CanEdit
    End Enum

    Public Class ModuleAccessItem
        Public Property ModuleId As Integer
        Public Property ModuleName As String
        Public Property AccessLevel As ModuleAccessLevel
    End Class

End Namespace