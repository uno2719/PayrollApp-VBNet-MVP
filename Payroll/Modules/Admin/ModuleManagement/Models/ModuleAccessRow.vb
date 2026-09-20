' ============================================================
' Modules/Admin/ModuleManagement/Models/ModuleAccessRow.vb
' ============================================================
' Isang hilera sa checkbox grid ng Access Editor - isang module,
' para sa KASALUKUYANG piniling user.
'
' BAKIT HIWALAY ITO SA Users.Models.ModuleAccessItem?
'   Ang ModuleAccessItem ay may ModuleId + AccessLevel (enum) -
'   sapat na para sa SaveModuleAccessAsync, pero hindi maganda
'   i-bind nang direkta sa isang GridView na may dalawang HIWALAY
'   na checkbox column (View, Edit). Dagdag pa, gusto nating
'   makita rin ang ModuleName/GroupName sa grid - wala noon sa
'   ModuleAccessItem.
'
'   Kaya: itong klase ang nagbibigay-buhay sa checkbox grid,
'   at may dalawang converter na nag-uugnay pabalik sa
'   ModuleAccessItem para sa pag-save gamit ang EXISTING
'   SaveModuleAccessAsync na hindi na natin kailangang baguhin.
' ============================================================
Namespace ModuleManagement.Models

    Public Class ModuleAccessRow

        Public Property ModuleId As Integer
        Public Property ModuleName As String
        Public Property GroupName As String

        Public Property CanView As Boolean
        Public Property CanEdit As Boolean

        Public Shared Function FromAccessItem(item As Payroll.Users.Models.ModuleAccessItem, groupName As String) As ModuleAccessRow

            Return New ModuleAccessRow With {
                .ModuleId = item.ModuleId,
                .ModuleName = item.ModuleName,
                .GroupName = groupName,
                .CanView = item.AccessLevel <> Payroll.Users.Models.ModuleAccessLevel.NoAccess,
                .CanEdit = item.AccessLevel = Payroll.Users.Models.ModuleAccessLevel.CanEdit
            }

        End Function

        Public Function ToAccessItem() As Payroll.Users.Models.ModuleAccessItem

            Dim level = Payroll.Users.Models.ModuleAccessLevel.NoAccess

            If CanEdit Then
                level = Payroll.Users.Models.ModuleAccessLevel.CanEdit
            ElseIf CanView Then
                level = Payroll.Users.Models.ModuleAccessLevel.ViewOnly
            End If

            Return New Payroll.Users.Models.ModuleAccessItem With {
                .ModuleId = ModuleId,
                .ModuleName = ModuleName,
                .AccessLevel = level
            }

        End Function

    End Class

End Namespace