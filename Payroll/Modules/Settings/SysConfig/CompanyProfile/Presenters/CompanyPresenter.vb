' UPDATE: Modules/Settings/SysConfig/CompanyProfile/Presenters/CompanyPresenter.vb
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

            _view.ShowLogo(FileStorageHelper.GetFullPath(_current.LogoPath))
        End Function

        Public Sub UploadLogo()
            Dim pickedFile = _view.PromptForLogoFile()
            If String.IsNullOrWhiteSpace(pickedFile) Then Return

            If Not String.IsNullOrWhiteSpace(_current.LogoPath) Then
                FileStorageHelper.DeleteFile(_current.LogoPath)
            End If

            _current.LogoPath = FileStorageHelper.SaveFile(pickedFile, "CompanyLogo", "CompanyLogo")
            _view.ShowLogo(FileStorageHelper.GetFullPath(_current.LogoPath))
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

            Dim result = Await _service.SaveAsync(_current, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _current = Await _service.GetAsync()
            _view.ShowMessage("Company Profile saved.")
        End Function

    End Class
End Namespace