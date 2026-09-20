' ============================================================
' Modules/Admin/ModuleManagement/Services/IModuleCatalogService.vb
' ============================================================
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Services

    Public Interface IModuleCatalogService

        Function GetAllAsync() As Task(Of List(Of ModuleCatalogModel))

        Function SaveAsync(item As ModuleCatalogModel, userName As String) As Task(Of ModuleCatalogSaveResult)
        Function DeleteAsync(recordId As Integer, userName As String) As Task(Of ModuleCatalogSaveResult)
        Function SetActiveAsync(recordId As Integer, isActive As Boolean, userName As String) As Task(Of ModuleCatalogSaveResult)

    End Interface

    Public Class ModuleCatalogSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property WarningMessage As String    ' hindi nakakaharang, para lang sa confirmation dialog
    End Class

End Namespace