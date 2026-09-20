' ============================================================
' Modules/Admin/ModuleManagement/Services/ModuleCatalogService.vb
' ============================================================
Imports System.Text.RegularExpressions
Imports Payroll.ModuleManagement.Data
Imports Payroll.ModuleManagement.Models

Namespace ModuleManagement.Services

    Public Class ModuleCatalogService
        Implements IModuleCatalogService

        Private ReadOnly _repo As IModuleCatalogRepository

        ' Loosely-enforced na convention: prefix_PascalCase, gaya ng
        ' "main_Employees", "settings_General". Hindi ito hard rule
        ' sa DB, babala lang - si Uno pa rin ang huling desisyon kung
        ' talagang kailangan niyang lumihis dito.
        Private Shared ReadOnly ModuleCodePattern As New Regex("^[A-Za-z][A-Za-z0-9]*_[A-Za-z0-9]+$")

        Public Sub New(repo As IModuleCatalogRepository)
            _repo = repo
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of ModuleCatalogModel)) _
            Implements IModuleCatalogService.GetAllAsync

            Return Await _repo.GetAllAsync()

        End Function

        ' ========================================================
        ' SAVE
        ' ========================================================
        Public Async Function SaveAsync(item As ModuleCatalogModel, userName As String) _
            As Task(Of ModuleCatalogSaveResult) _
            Implements IModuleCatalogService.SaveAsync

            ' --- VALIDATION ---
            If String.IsNullOrWhiteSpace(item.ModuleCode) Then
                Return Fail("Module Code is required.")
            End If

            If item.ModuleCode.Contains(" ") Then
                Return Fail("Module Code cannot contain spaces.")
            End If

            If String.IsNullOrWhiteSpace(item.ModuleName) Then
                Return Fail("Module Name is required.")
            End If

            item.ModuleCode = item.ModuleCode.Trim()
            item.ModuleName = item.ModuleName.Trim()

            ' Babala lang ito (hindi Fail) - ipapakita sa View bilang
            ' isang confirm dialog, hindi bilang blocking error. Sadyang
            ' pinapayagan pa rin natin ito dahil baka may magandang
            ' dahilan si Uno para lumihis (halimbawa: "logout" na hindi
            ' naman module).
            Dim warning As String = Nothing
            If Not ModuleCodePattern.IsMatch(item.ModuleCode) Then
                warning = "The Module Code does not follow the usual 'prefix_Name' pattern " &
                          "(e.g. 'main_Employees'). Continuing anyway will save it as-is - " &
                          "make sure this matches the Tag you will set in frmMain.Designer.vb."
            End If

            ' --- DUPLICATE CHECK ---
            Dim isTaken = Await _repo.IsModuleCodeTakenAsync(item.ModuleCode, item.RecordId)
            If isTaken Then
                Return Fail($"Module Code '{item.ModuleCode}' is already used by another module.")
            End If

            ' --- PERSIST ---
            If item.RecordId = 0 Then
                item.RecordId = Await _repo.InsertAsync(item, userName)
            Else
                Await _repo.UpdateAsync(item, userName)
            End If

            Return New ModuleCatalogSaveResult With {.Success = True, .WarningMessage = warning}

        End Function

        ' ========================================================
        ' DELETE
        ' ========================================================
        Public Async Function DeleteAsync(recordId As Integer, userName As String) _
            As Task(Of ModuleCatalogSaveResult) _
            Implements IModuleCatalogService.DeleteAsync

            Dim usersWithAccess = Await _repo.GetUsersWithAccessCountAsync(recordId)

            If usersWithAccess > 0 Then
                ' HINDI natin pinapayagang mabura ito. Kung binura natin
                ' ang module row pero may naiwang tblUserModuleAccess rows
                ' na tumuturo dito (FK violation kung may FK; kung wala,
                ' mas malala - orphan rows na walang katuturan), o kaya
                ' naman kung may FK CASCADE, biglang mawawala ang access
                ' ng mga user na iyon nang hindi nila alam kung bakit.
                Return Fail(
                    $"This module currently has {usersWithAccess} user(s) with assigned access." &
                    Environment.NewLine &
                    "Please deactivate it instead of deleting, or remove their access first " &
                    "from the Access Editor tab.")
            End If

            Await _repo.DeleteAsync(recordId)

            Return New ModuleCatalogSaveResult With {.Success = True}

        End Function

        ' ========================================================
        ' TOGGLE ACTIVE
        ' ========================================================
        Public Async Function SetActiveAsync(recordId As Integer, isActive As Boolean, userName As String) _
            As Task(Of ModuleCatalogSaveResult) _
            Implements IModuleCatalogService.SetActiveAsync

            Dim warning As String = Nothing

            ' Babala lang (hindi pagbabawal) - kung may mga user na
            ' currently may access, dapat malaman ni Uno na maiimpluwensyahan
            ' sila nito bago siya mag-confirm sa dialog.
            If Not isActive Then
                Dim usersWithAccess = Await _repo.GetUsersWithAccessCountAsync(recordId)
                If usersWithAccess > 0 Then
                    warning = $"{usersWithAccess} user(s) currently have access to this module. " &
                              "Deactivating it will hide it from ALL of them (Admins are unaffected) " &
                              "until you reactivate it."
                End If
            End If

            Await _repo.SetActiveAsync(recordId, isActive, userName)

            Return New ModuleCatalogSaveResult With {.Success = True, .WarningMessage = warning}

        End Function

        Private Shared Function Fail(message As String) As ModuleCatalogSaveResult
            Return New ModuleCatalogSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

    End Class

End Namespace