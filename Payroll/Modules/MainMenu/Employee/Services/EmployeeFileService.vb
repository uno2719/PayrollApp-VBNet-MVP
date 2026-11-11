' File: Modules/MainMenu/Employee/Services/EmployeeFileService.vb
' ============================================================
' Phase 3: 201 Files + Photo. Ang Files Folder (shared path) ay galing sa General Settings; ang mga file ay
' nasa folder na iyon at ang RELATIVE path lang ang nasa database (tingnan ang SharedFileStorage).
' Permission: "Employee Documents" (main_Employees_Documents) - View para makita, Can Edit para mag-upload/delete.
' ============================================================
Imports System.IO
Imports Payroll.GlobalShared.Helpers

Namespace Employee.Services

    Public Class EmployeeFileService
        Implements IEmployeeFileService

        Private ReadOnly _repo As Data.IEmployeeFileRepository

        Public Sub New()
            _repo = New Data.EmployeeFileRepository()
        End Sub

        Public Sub New(repo As Data.IEmployeeFileRepository)
            _repo = repo
        End Sub

        ' ---------- PERMISSIONS (huling harang kahit lusutan ang UI) ----------
        Private Shared Sub RequireView()
            If Not GlobalShared.Security.PermissionService.CanView(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents) Then
                Throw New UnauthorizedAccessException("You don't have access to Employee Documents.")
            End If
        End Sub

        Private Shared Sub RequireEdit()
            If Not GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents) Then
                Throw New UnauthorizedAccessException("You are not allowed to change Employee Documents or photos.")
            End If
        End Sub

        ''' <summary>Ibinabalik ang root kapag magagamit mula sa PC na ito; kung hindi, nagbabato ng malinaw na mensahe.</summary>
        Private Async Function GetUsableRootAsync() As Task(Of String)
            Dim root As String = Await _repo.GetStorageRootAsync()
            Dim problem As String = Await Task.Run(Function() SharedFileStorage.ValidateRoot(root))
            If problem.Length > 0 Then Throw New InvalidOperationException(problem)
            Return root.Trim()
        End Function

        ' ---------- 201 FILES ----------

        Public Async Function GetCategoriesAsync() As Task(Of List(Of Models.DocumentCategoryModel)) _
            Implements IEmployeeFileService.GetCategoriesAsync
            RequireView()
            Return Await _repo.GetCategoriesAsync()
        End Function

        Public Async Function GetDocumentsAsync(recordId As Integer) As Task(Of List(Of Models.EmployeeDocumentModel)) _
            Implements IEmployeeFileService.GetDocumentsAsync
            RequireView()
            Return Await _repo.GetDocumentsAsync(recordId)
        End Function

        Public Async Function AddDocumentsAsync(recordId As Integer, categoryId As Integer, remarks As String,
                                                filePaths As IEnumerable(Of String)) As Task(Of Models.AddDocumentsResult) _
            Implements IEmployeeFileService.AddDocumentsAsync

            RequireEdit()
            If recordId <= 0 Then Throw New InvalidOperationException("No employee selected.")
            If categoryId <= 0 Then Throw New InvalidOperationException("Choose a category first.")

            Dim cleanRemarks As String = If(String.IsNullOrWhiteSpace(remarks), Nothing, remarks.Trim())
            If cleanRemarks IsNot Nothing AndAlso cleanRemarks.Length > 500 Then
                Throw New InvalidOperationException("Remarks is too long (maximum 500 characters).")
            End If

            Dim root As String = Await GetUsableRootAsync()
            Dim result As New Models.AddDocumentsResult()

            For Each filePath In filePaths
                Dim src As String = filePath
                Dim displayName As String = Path.GetFileName(src)

                Try
                    Dim fileProblem As String = SharedFileStorage.ValidateDocumentFile(src)
                    If fileProblem.Length > 0 Then
                        result.Failures.Add($"{displayName}: {fileProblem}")
                        Continue For
                    End If

                    Dim size As Long = New FileInfo(src).Length
                    Dim stored As String = Await Task.Run(Function() SharedFileStorage.SaveDocument(root, recordId, src))

                    Try
                        Await _repo.InsertDocumentAsync(New Models.EmployeeDocumentModel With {
                            .RecordId = recordId,
                            .CategoryId = categoryId,
                            .FileName = displayName,
                            .StoredPath = stored,
                            .FileSizeBytes = size,
                            .Remarks = cleanRemarks,
                            .CreatedBy = GlobalShared.Constants.AppSession.CurrentUser
                        })
                        result.Saved += 1
                    Catch
                        ' Walang row sa database = huwag mag-iwan ng "ulilang" file sa folder.
                        Try
                            SharedFileStorage.DeleteFile(root, stored)
                        Catch ex2 As Exception
                            Debug.WriteLine($"Cleanup failed: {ex2.Message}")
                        End Try
                        Throw
                    End Try

                Catch ex As Exception
                    result.Failures.Add($"{displayName}: {ex.Message}")
                End Try
            Next

            Return result
        End Function

        Public Async Function GetDocumentFullPathAsync(documentId As Integer) As Task(Of String) _
            Implements IEmployeeFileService.GetDocumentFullPathAsync

            RequireView()

            Dim doc = Await _repo.GetDocumentAsync(documentId)
            If doc Is Nothing Then Throw New InvalidOperationException("This file no longer exists in the list.")

            Dim root As String = Await GetUsableRootAsync()
            Dim fullPath As String = SharedFileStorage.GetFullPath(root, doc.StoredPath)

            Dim exists As Boolean = Await Task.Run(Function() File.Exists(fullPath))
            If Not exists Then
                Throw New FileNotFoundException($"The file can't be found in the Files Folder: {fullPath}")
            End If

            Return fullPath
        End Function

        Public Async Function SoftDeleteDocumentAsync(documentId As Integer) As Task(Of Boolean) _
            Implements IEmployeeFileService.SoftDeleteDocumentAsync

            RequireEdit()
            Return Await _repo.SoftDeleteDocumentAsync(documentId, GlobalShared.Constants.AppSession.CurrentUser)
        End Function

        ' ---------- PHOTO ----------

        Public Async Function GetPhotoAsync(recordId As Integer) As Task(Of System.Drawing.Image) _
            Implements IEmployeeFileService.GetPhotoAsync

            Try
                Dim relative As String = Await _repo.GetPhotoPathAsync(recordId)
                If String.IsNullOrWhiteSpace(relative) Then Return Nothing

                Dim root As String = Await _repo.GetStorageRootAsync()
                If String.IsNullOrWhiteSpace(root) Then Return Nothing

                Return Await Task.Run(Function() SharedFileStorage.LoadImage(root.Trim(), relative))
            Catch ex As Exception
                ' Hindi dapat masira ang Employee screen dahil lang sa photo (hal. hindi maabot ang folder).
                Debug.WriteLine($"Photo not available: {ex.Message}")
                Return Nothing
            End Try
        End Function

        Public Async Function SetPhotoAsync(recordId As Integer, sourceImagePath As String) As Task _
            Implements IEmployeeFileService.SetPhotoAsync

            RequireEdit()
            If recordId <= 0 Then Throw New InvalidOperationException("No employee selected.")

            Dim ext As String = Path.GetExtension(If(sourceImagePath, String.Empty)).ToLowerInvariant()
            If Not SharedFileStorage.AllowedPhotoExtensions.Contains(ext) Then
                Throw New InvalidOperationException($"Photo must be one of: {String.Join(", ", SharedFileStorage.AllowedPhotoExtensions)}.")
            End If

            Dim root As String = Await GetUsableRootAsync()
            Dim oldRelative As String = Await _repo.GetPhotoPathAsync(recordId)

            Dim newRelative As String = Await Task.Run(Function() SharedFileStorage.SavePhoto(root, recordId, sourceImagePath))
            Await _repo.SetPhotoPathAsync(recordId, newRelative, GlobalShared.Constants.AppSession.CurrentUser)

            ' Palitan: burahin ang lumang larawan (best-effort).
            If Not String.IsNullOrWhiteSpace(oldRelative) Then
                Try
                    SharedFileStorage.DeleteFile(root, oldRelative)
                Catch ex As Exception
                    Debug.WriteLine($"Old photo not deleted: {ex.Message}")
                End Try
            End If
        End Function

        Public Async Function RemovePhotoAsync(recordId As Integer) As Task _
            Implements IEmployeeFileService.RemovePhotoAsync

            RequireEdit()

            Dim oldRelative As String = Await _repo.GetPhotoPathAsync(recordId)
            Await _repo.SetPhotoPathAsync(recordId, Nothing, GlobalShared.Constants.AppSession.CurrentUser)

            If Not String.IsNullOrWhiteSpace(oldRelative) Then
                Try
                    Dim root As String = Await _repo.GetStorageRootAsync()
                    If Not String.IsNullOrWhiteSpace(root) Then SharedFileStorage.DeleteFile(root.Trim(), oldRelative)
                Catch ex As Exception
                    Debug.WriteLine($"Old photo not deleted: {ex.Message}")
                End Try
            End If
        End Function

    End Class

End Namespace
