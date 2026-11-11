' UPDATE: Modules/Settings/SysConfig/CompanyProfile/Presenters/CompanyPresenter.vb
Imports System.IO
Imports Payroll.Employee.Data
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Presenters
    Public Class CompanyPresenter

        Private ReadOnly _view As Views.ICompanyView
        Private ReadOnly _service As Services.ICompanyService
        Private ReadOnly _employeeRepository As IEmployeeRepository
        Private ReadOnly _userName As String

        Private _current As CompanyModel

        ' Shared Files Folder (General Settings) para sa logo, at ang logo path na nasa database ngayon
        Private ReadOnly _fileRepository As IEmployeeFileRepository = New EmployeeFileRepository()
        Private _storageRoot As String
        Private _loadedLogoPath As String

        Public Sub New(
            view As Views.ICompanyView,
            service As Services.ICompanyService,
            employeeRepository As IEmployeeRepository,
            userName As String)

            _view = view
            _service = service
            _employeeRepository = employeeRepository
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            ' I-bind muna ang employee list bago i-set ang ContactPersonRecordId,
            ' kailangan ito ng GridLookUpEdit para maipakita ang tamang selection.
            Dim employees = Await _employeeRepository.GetEmployeeContactLookupAsync()
            _view.SetEmployeeList(employees)

            _current = Await _service.GetAsync()
            If _current Is Nothing Then
                _current = New CompanyModel()
            End If

            _view.CompanyCode = _current.CompanyCode
            _view.CompanyName = _current.CompanyName
            _view.Industry = _current.Industry
            _view.Country = _current.Country
            _view.PostCode = _current.PostCode
            _view.TelephoneNo = _current.TelephoneNo
            _view.FaxNo = _current.FaxNo
            _view.ContactPersonRecordId = _current.ContactPersonRecordId
            _view.ContactPersonEmail = _current.ContactPersonEmail
            _view.Address1 = _current.Address1
            _view.Address2 = _current.Address2
            _view.Address3 = _current.Address3
            _view.SECRegistrationNo = _current.SECRegistrationNo
            _view.Website = _current.Website
            _view.IsActive = _current.IsActive

            Try
                _storageRoot = Await _fileRepository.GetStorageRootAsync()
            Catch ex As Exception
                ' Wala pang column (hindi pa na-run ang Phase 3 SQL): gumamit ng lumang gawi.
                Debug.WriteLine($"Files Folder not available: {ex.Message}")
                _storageRoot = Nothing
            End Try

            _loadedLogoPath = _current.LogoPath
            _view.ShowLogo(ResolveLogoFullPath(_current.LogoPath))
        End Function

        ' Shared folder muna. Kung wala pa roon ang file pero nasa lokal na Uploads\ ng PC na ito (lumang logo na na-upload
        ' dito dati), kokopyahin ito sa shared folder (isang beses, walang binabago sa database dahil pareho ang relative path).
        Private Function ResolveLogoFullPath(relativePath As String) As String
            If String.IsNullOrWhiteSpace(relativePath) Then Return String.Empty

            Dim localFull As String = FileStorageHelper.GetFullPath(relativePath)
            If String.IsNullOrWhiteSpace(_storageRoot) Then Return localFull

            Dim sharedFull As String = SharedFileStorage.GetFullPath(_storageRoot, relativePath)
            Try
                If Not File.Exists(sharedFull) AndAlso File.Exists(localFull) Then
                    Directory.CreateDirectory(Path.GetDirectoryName(sharedFull))
                    File.Copy(localFull, sharedFull, overwrite:=False)
                End If
                Return If(File.Exists(sharedFull), sharedFull, localFull)
            Catch ex As Exception
                Debug.WriteLine($"Logo migration/read failed: {ex.Message}")
                Return localFull
            End Try
        End Function

        Private Sub TryDeleteLogo(relativePath As String)
            If String.IsNullOrWhiteSpace(relativePath) OrElse String.IsNullOrWhiteSpace(_storageRoot) Then Return
            Try
                SharedFileStorage.DeleteFile(_storageRoot, relativePath)
            Catch ex As Exception
                Debug.WriteLine($"Old logo not deleted: {ex.Message}")
            End Try
        End Sub

        Public Sub UploadLogo()
            If String.IsNullOrWhiteSpace(_storageRoot) Then
                _view.ShowError("The Files Folder is not set. An administrator must set it in Settings > General Settings " &
                                "before a logo can be uploaded.")
                Return
            End If

            Dim problem As String = SharedFileStorage.ValidateRoot(_storageRoot)
            If problem.Length > 0 Then
                _view.ShowError(problem)
                Return
            End If

            Dim pickedFile = _view.PromptForLogoFile()
            If String.IsNullOrWhiteSpace(pickedFile) Then Return

            Dim newPath As String
            Try
                newPath = SharedFileStorage.SaveFile(_storageRoot, pickedFile, "CompanyLogo", "CompanyLogo")
            Catch ex As Exception
                _view.ShowError($"The logo could not be uploaded: {ex.Message}")
                Return
            End Try

            ' Naunang upload sa session na ito na hindi pa nase-save: ligtas nang burahin. Ang logo na nasa database
            ' ay buburahin lang pagkatapos ng matagumpay na Save (para hindi masira ang logo kung hindi mase-save).
            If Not String.IsNullOrWhiteSpace(_current.LogoPath) AndAlso _current.LogoPath <> _loadedLogoPath Then
                TryDeleteLogo(_current.LogoPath)
            End If

            _current.LogoPath = newPath
            _view.ShowLogo(SharedFileStorage.GetFullPath(_storageRoot, newPath))
        End Sub

        Public Async Function SaveAsync() As Task
            _current.CompanyCode = If(_view.CompanyCode, "").Trim()
            _current.CompanyName = If(_view.CompanyName, "").Trim()
            _current.Industry = _view.Industry
            _current.Country = _view.Country
            _current.PostCode = _view.PostCode
            _current.TelephoneNo = _view.TelephoneNo
            _current.FaxNo = _view.FaxNo
            _current.ContactPersonRecordId = _view.ContactPersonRecordId
            _current.ContactPersonEmail = _view.ContactPersonEmail
            _current.Address1 = _view.Address1
            _current.Address2 = _view.Address2
            _current.Address3 = _view.Address3
            _current.SECRegistrationNo = _view.SECRegistrationNo
            _current.Website = _view.Website
            _current.IsActive = _view.IsActive

            Dim previousLogo As String = _loadedLogoPath
            Dim savedLogo As String = _current.LogoPath

            Dim result = Await _service.SaveAsync(_current, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            ' Napalitan ang logo at nase-save na: burahin ang luma sa shared folder (best-effort).
            If Not String.IsNullOrWhiteSpace(previousLogo) AndAlso previousLogo <> savedLogo Then
                TryDeleteLogo(previousLogo)
            End If
            _loadedLogoPath = savedLogo

            _current = Await _service.GetAsync()
            _view.ShowMessage("Company Profile saved.")
        End Function

    End Class
End Namespace