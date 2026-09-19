' ============================================================
' GlobalShared/Security/UserPermission.vb
' ============================================================
' Flat na resulta ng JOIN sa pagitan ng tblModules at
' tblUserModuleAccess para sa IISANG user.
'
' Pansinin: ModuleCode ang dala nito, HINDI ModuleId. Yung
' ModuleId (integer PK) ay panloob na detalye ng database -
' walang pakialam doon ang UI. Ang UI ay nakakakilala lang ng
' string code gaya ng "main_Employees".
' ============================================================
Namespace GlobalShared.Security

    Public Class UserPermission
        Public Property ModuleCode As String
        Public Property CanView As Boolean
        Public Property CanEdit As Boolean
    End Class

End Namespace