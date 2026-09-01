Imports Payroll.Login.Models
Imports Payroll.Users.Models

Namespace Users.Views
    Public Interface IUsersView

        ' Form fields
        Property Username As String
        Property SelectedEmployeeNo As String
        Property IsAdmin As Boolean
        Property IsActive As Boolean

        ' Masterlist grid
        Sub BindUsersList(users As List(Of UserModel))
        Sub BindEmployeeChoices(employees As List(Of KeyValuePair(Of String, String)))

        ' Module access (dynamic list, populated/read gamit ang mga selector)
        Sub BindModuleAccess(items As List(Of ModuleAccessItem))
        Function GetModuleAccessSelections() As List(Of ModuleAccessItem)

        ' State/UX
        ' isNewRecord: True = New (editable ang Username/Employee),
        '              False = Edit (readonly ang Username/Employee, dahil
        '              hindi na dapat baguhin ang mga ito pagkatapos ma-create)
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowTemporaryPassword(password As String)
        Sub HideTemporaryPassword()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace