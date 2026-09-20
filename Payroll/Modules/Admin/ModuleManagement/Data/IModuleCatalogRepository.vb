' ============================================================
' Modules/Admin/ModuleManagement/Data/IModuleCatalogRepository.vb
' ============================================================
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Data

    Public Interface IModuleCatalogRepository

        ' LAHAT - kasama ang mga Inactive. Ang mga query mo sa
        ' UserRepository (GetAllModulesAsync) ay Active lang - dito,
        ' kailangan mong makita LAHAT para ma-reactivate mo balik.
        Function GetAllAsync() As Task(Of List(Of ModuleCatalogModel))

        Function GetByIdAsync(recordId As Integer) As Task(Of ModuleCatalogModel)

        Function IsModuleCodeTakenAsync(moduleCode As String, excludeRecordId As Integer) As Task(Of Boolean)

        Function InsertAsync(item As ModuleCatalogModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As ModuleCatalogModel, userName As String) As Task(Of Boolean)
        Function DeleteAsync(recordId As Integer) As Task(Of Boolean)

        Function SetActiveAsync(recordId As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)

        Function GetUsersWithAccessCountAsync(recordId As Integer) As Task(Of Integer)

    End Interface

End Namespace