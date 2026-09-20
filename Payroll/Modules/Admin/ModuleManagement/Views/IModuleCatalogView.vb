' ============================================================
' Modules/Admin/ModuleManagement/Views/IModuleCatalogView.vb
' ============================================================
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Views

    Public Interface IModuleCatalogView

        Sub BindModuleList(items As List(Of ModuleCatalogModel))
        Sub BindGroupChoices(groups As List(Of String))

        Property ModuleCode As String
        Property ModuleName As String
        Property GroupName As String
        Property SortOrder As Integer
        Property IsActive As Boolean

        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

        ''' <summary>
        ''' True kung sinagot ng user ng "Yes" ang isang babalang tanong
        ''' (hal. "may mga user na apektado, ituloy pa rin?"). Ipinasa
        ''' rin ang caption ng dialog para maiba ang message per sitwasyon.
        ''' </summary>
        Function Confirm(message As String, caption As String) As Boolean

    End Interface

End Namespace