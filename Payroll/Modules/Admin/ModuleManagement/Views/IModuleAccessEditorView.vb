' ============================================================
' Modules/Admin/ModuleManagement/Views/IModuleAccessEditorView.vb
' ============================================================
Imports Payroll.Login.Models
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Views

    Public Interface IModuleAccessEditorView

        Sub BindUserList(users As List(Of UserModel))
        Sub SetSelectedUserHeader(displayName As String, isAdmin As Boolean)

        Sub BindAccessGrid(rows As List(Of ModuleAccessRow))
        Function GetAccessGridRows() As List(Of ModuleAccessRow)

        ''' <summary>
        ''' True kung may ibang user na currently piniling "kopyahin ang
        ''' access mula kay ___" - ipinapakita bilang isang lookup/dropdown
        ''' dialog ng ibang user (maliban sa kasalukuyang selected).
        ''' Ibinabalik ang piniling UserId, o Nothing kung kinansela.
        ''' </summary>
        Function PromptCopyFromUser(candidates As List(Of UserModel)) As Integer?

        Sub SetGridEnabled(enabled As Boolean)
        Sub SetHasUnsavedChanges(hasChanges As Boolean)

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
        Function Confirm(message As String, caption As String) As Boolean

    End Interface

End Namespace