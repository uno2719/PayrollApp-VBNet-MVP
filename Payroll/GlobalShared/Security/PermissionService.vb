' ============================================================
' GlobalShared/Security/PermissionService.vb
' ============================================================
' In-memory na permission cache ng kasalukuyang naka-login na user.
'
' BAKIT SHARED (static) AT NAKA-CACHE?
'   Ang tanong na "may access ba si user dito?" ay tatanungin ng
'   napakadaming lugar: sidebar navigation, bawat toolbar button,
'   bawat view. Kung tuwing tatanong ay may database roundtrip,
'   mababagal ang UI at wala namang mangyayaring pagbabago -
'   fixed na ang permissions mo sa buong session.
'
'   Kaya: ISANG query lang sa login, saka lahat ay memory lookup.
'
' PAANO ANG LIFECYCLE?
'   Login  -> Load(...)   (pinupuno ang cache)
'   Logout -> Clear()     (binubura - importante ito! kung hindi,
'                          madadala ng susunod na user ang
'                          permission ng nauna, dahil buhay pa rin
'                          ang process.)
' ============================================================
Imports Payroll.GlobalShared.Constants
Imports Payroll.Users.Models

Namespace GlobalShared.Security

    Public NotInheritable Class PermissionService

        Private Sub New()
        End Sub

        ' StringComparer.OrdinalIgnoreCase - para hindi ka maloko ng
        ' case sensitivity kung sakaling "Main_Employees" ang nai-encode
        ' sa database pero "main_Employees" ang nasa Tag.
        Private Shared _permissions As New Dictionary(Of String, UserPermission)(
            StringComparer.OrdinalIgnoreCase)

        Private Shared _isAdmin As Boolean = False
        Private Shared _isLoaded As Boolean = False

        Public Shared ReadOnly Property IsLoaded As Boolean
            Get
                Return _isLoaded
            End Get
        End Property

        ' ========================================================
        ' LOAD - tawagin ito PAGKATAPOS ng successful login
        ' ========================================================
        Public Shared Sub Load(permissions As List(Of UserPermission), isAdmin As Boolean)

            _permissions.Clear()
            _isAdmin = isAdmin

            If permissions IsNot Nothing Then
                For Each p In permissions
                    If String.IsNullOrWhiteSpace(p.ModuleCode) Then Continue For

                    ' Indexer (hindi .Add) para hindi mag-crash kung
                    ' sakaling may duplicate na ModuleCode sa tblModules.
                    _permissions(p.ModuleCode.Trim()) = p
                Next
            End If

            _isLoaded = True

        End Sub

        ' ========================================================
        ' CLEAR - tawagin ito sa logout
        ' ========================================================
        Public Shared Sub Clear()
            _permissions.Clear()
            _isAdmin = False
            _isLoaded = False
        End Sub

        ' ========================================================
        ' CAN VIEW - pwede bang buksan ang module na ito?
        ' ========================================================
        Public Shared Function CanView(moduleCode As String) As Boolean

            If String.IsNullOrWhiteSpace(moduleCode) Then Return False

            ' Bypass 1: Admin - lahat bukas.
            If _isAdmin Then Return True

            ' Bypass 2: Dashboard / logout - laging bukas.
            If ModuleCodes.IsAlwaysAllowed(moduleCode) Then Return True

            Dim perm As UserPermission = Nothing
            If Not _permissions.TryGetValue(moduleCode.Trim(), perm) Then
                ' WALANG row sa tblUserModuleAccess = NO ACCESS.
                ' Deny-by-default ito - mas ligtas kaysa allow-by-default,
                ' kasi kapag nakalimutan mong i-set ang access ng bagong
                ' module, naka-lock siya by default imbes na bukas sa lahat.
                Return False
            End If

            Return perm.CanView

        End Function

        ' ========================================================
        ' CAN EDIT - pwede ba siyang mag-save/delete dito?
        ' ========================================================
        ' Tandaan: ang CanEdit ay NAKASAKAY sa CanView. Walang sense
        ' ang "pwedeng mag-edit pero hindi pwedeng makita".
        ' ========================================================
        Public Shared Function CanEdit(moduleCode As String) As Boolean

            If String.IsNullOrWhiteSpace(moduleCode) Then Return False

            If _isAdmin Then Return True

            Dim perm As UserPermission = Nothing
            If Not _permissions.TryGetValue(moduleCode.Trim(), perm) Then Return False

            Return perm.CanView AndAlso perm.CanEdit

        End Function

        ' ========================================================
        ' GET LEVEL - kapag kailangan mo ng tatlong-antas na sagot
        ' ========================================================
        Public Shared Function GetLevel(moduleCode As String) As ModuleAccessLevel

            If CanEdit(moduleCode) Then Return ModuleAccessLevel.CanEdit
            If CanView(moduleCode) Then Return ModuleAccessLevel.ViewOnly

            Return ModuleAccessLevel.NoAccess

        End Function

        ' Para sa debugging - makikita mo kung ano talaga ang nabasa
        ' galing database para sa user na naka-login.
        Public Shared Function DebugDump() As String
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine($"IsAdmin: {_isAdmin} | Loaded: {_isLoaded} | Count: {_permissions.Count}")

            For Each kvp In _permissions
                sb.AppendLine($"  {kvp.Key} -> View:{kvp.Value.CanView} Edit:{kvp.Value.CanEdit}")
            Next

            Return sb.ToString()
        End Function

    End Class

End Namespace