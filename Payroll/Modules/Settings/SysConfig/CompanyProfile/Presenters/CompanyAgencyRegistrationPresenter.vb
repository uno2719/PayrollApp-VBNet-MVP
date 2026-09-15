' File: Modules/Settings/SysConfig/CompanyProfile/Presenters/CompanyAgencyRegistrationPresenter.vb
Imports Payroll.Employee.Data
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Presenters
    Public Class CompanyAgencyRegistrationPresenter

        Private ReadOnly _view As Views.ICompanyAgencyRegistrationView
        Private ReadOnly _service As Services.ICompanyAgencyRegistrationService
        Private ReadOnly _employeeRepository As IEmployeeRepository
        Private ReadOnly _agencyType As String
        Private ReadOnly _userName As String

        Private _current As CompanyAgencyRegistrationModel

        Public Sub New(
            view As Views.ICompanyAgencyRegistrationView,
            service As Services.ICompanyAgencyRegistrationService,
            employeeRepository As IEmployeeRepository,
            agencyType As String,
            userName As String)

            _view = view
            _service = service
            _employeeRepository = employeeRepository
            _agencyType = agencyType
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Dim info = CompanyAgencyTypeRegistry.GetInfo(_agencyType)
            _view.SetLabels(info.RegistrationNoLabel, info.BranchLabel, info.DisplayName)

            Dim employees = Await _employeeRepository.GetEmployeeContactLookupAsync()
            _view.SetEmployeeList(employees)

            _current = Await _service.GetAsync(_agencyType)

            If _current Is Nothing Then
                _current = New CompanyAgencyRegistrationModel With {.AgencyType = _agencyType}
            End If

            _view.RegistrationNo = _current.RegistrationNo
            _view.Branch = _current.Branch
            _view.Address1 = _current.Address1
            _view.Address2 = _current.Address2
            _view.Address3 = _current.Address3
            _view.Country = _current.Country
            _view.PostCode = _current.PostCode
            _view.TelephoneNo = _current.TelephoneNo
            _view.FaxNo = _current.FaxNo
            _view.ContactPersonRecordId = _current.ContactPersonRecordId
            _view.ContactPersonEmail = _current.ContactPersonEmail
            _view.PersonInCharge1RecordId = _current.PersonInCharge1RecordId
            _view.PersonInCharge1Email = _current.PersonInCharge1Email
            _view.PersonInCharge2RecordId = _current.PersonInCharge2RecordId
            _view.PersonInCharge2Email = _current.PersonInCharge2Email
            _view.Remarks = _current.Remarks
        End Function

        Public Async Function SaveAsync() As Task
            _current.RegistrationNo = If(_view.RegistrationNo, "").Trim()
            _current.Branch = _view.Branch
            _current.Address1 = _view.Address1
            _current.Address2 = _view.Address2
            _current.Address3 = _view.Address3
            _current.Country = _view.Country
            _current.PostCode = _view.PostCode
            _current.TelephoneNo = _view.TelephoneNo
            _current.FaxNo = _view.FaxNo
            _current.ContactPersonRecordId = _view.ContactPersonRecordId
            _current.ContactPersonEmail = _view.ContactPersonEmail
            _current.PersonInCharge1RecordId = _view.PersonInCharge1RecordId
            _current.PersonInCharge1Email = _view.PersonInCharge1Email
            _current.PersonInCharge2RecordId = _view.PersonInCharge2RecordId
            _current.PersonInCharge2Email = _view.PersonInCharge2Email
            _current.Remarks = _view.Remarks

            Dim result = Await _service.SaveAsync(_agencyType, _current, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _current = Await _service.GetAsync(_agencyType)
            _view.ShowMessage($"{CompanyAgencyTypeRegistry.GetInfo(_agencyType).DisplayName} registration saved.")
        End Function

    End Class
End Namespace