' File: Modules/Settings/SysConfig/CompanyProfile/Presenters/CompanyBankPresenter.vb
Imports System.Linq
Imports Payroll.Employee.Data
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Presenters
    Public Class CompanyBankPresenter

        Private ReadOnly _view As Views.ICompanyBankMaintenanceView
        Private ReadOnly _service As Services.ICompanyBankService
        Private ReadOnly _employeeRepository As IEmployeeRepository
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of CompanyBankModel)

        Public Sub New(
            view As Views.ICompanyBankMaintenanceView,
            service As Services.ICompanyBankService,
            employeeRepository As IEmployeeRepository,
            userName As String)

            _view = view
            _service = service
            _employeeRepository = employeeRepository
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Dim employees = Await _employeeRepository.GetEmployeeContactLookupAsync()
            _view.SetEmployeeList(employees)

            Await LoadListAsync()

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)
        End Function

        Private Async Function LoadListAsync() As Task
            _currentList = Await _service.GetAllAsync()
            _view.BindList(_currentList)
        End Function

        Public Sub StartNew()
            _selectedId = 0
            _isNewMode = True

            _view.ClearFields()
            _view.IsActive = True
            _view.SetFormMode(True, True)
        End Sub

        Public Sub SelectItem(id As Integer)
            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.BankId = id)

            If selected IsNot Nothing Then
                _view.BankName = selected.BankName
                _view.BankCode = selected.BankCode
                _view.AccountName = selected.AccountName
                _view.AccountNo = selected.AccountNo
                _view.Branch = selected.Branch
                _view.Address1 = selected.Address1
                _view.Address2 = selected.Address2
                _view.Address3 = selected.Address3
                _view.Country = selected.Country
                _view.PostCode = selected.PostCode
                _view.TelephoneNo = selected.TelephoneNo
                _view.FaxNo = selected.FaxNo
                _view.ContactPersonRecordId = selected.ContactPersonRecordId
                _view.ContactPersonEmail = selected.ContactPersonEmail
                _view.PersonInCharge1RecordId = selected.PersonInCharge1RecordId
                _view.PersonInCharge1Email = selected.PersonInCharge1Email
                _view.PersonInCharge2RecordId = selected.PersonInCharge2RecordId
                _view.PersonInCharge2Email = selected.PersonInCharge2Email
                _view.SwiftCode = selected.SwiftCode
                _view.BranchNo = selected.BranchNo
                _view.CustomerID = selected.CustomerID
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(False, False)
        End Sub

        Public Sub StartEdit()
            If _selectedId = 0 Then
                _view.ShowError("Please select a bank entry first.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)
        End Sub

        Public Async Function SaveAsync() As Task
            Dim item As New CompanyBankModel With {
                .BankId = _selectedId,
                .BankName = If(_view.BankName, "").Trim(),
                .BankCode = _view.BankCode,
                .AccountName = _view.AccountName,
                .AccountNo = If(_view.AccountNo, "").Trim(),
                .Branch = _view.Branch,
                .Address1 = _view.Address1,
                .Address2 = _view.Address2,
                .Address3 = _view.Address3,
                .Country = _view.Country,
                .PostCode = _view.PostCode,
                .TelephoneNo = _view.TelephoneNo,
                .FaxNo = _view.FaxNo,
                .ContactPersonRecordId = _view.ContactPersonRecordId,
                .ContactPersonEmail = _view.ContactPersonEmail,
                .PersonInCharge1RecordId = _view.PersonInCharge1RecordId,
                .PersonInCharge1Email = _view.PersonInCharge1Email,
                .PersonInCharge2RecordId = _view.PersonInCharge2RecordId,
                .PersonInCharge2Email = _view.PersonInCharge2Email,
                .SwiftCode = _view.SwiftCode,
                .BranchNo = _view.BranchNo,
                .CustomerID = _view.CustomerID,
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage(If(_isNewMode, "Bank entry added.", "Bank entry updated."))

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)

            Await LoadListAsync()
        End Function

        Public Sub CancelEdit()
            If _selectedId > 0 AndAlso Not _isNewMode Then
                SelectItem(_selectedId)
            Else
                _selectedId = 0
                _isNewMode = False
                _view.ClearFields()
                _view.SetFormMode(False, False)
            End If
        End Sub

        Public Async Function ToggleActiveSelectedAsync() As Task
            If _selectedId = 0 Then
                _view.ShowError("Please select a bank entry first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive

            Await _service.SetActiveStatusAsync(_selectedId, newStatus, _userName)

            _view.IsActive = newStatus
            _view.ShowMessage(If(newStatus, "Bank entry reactivated.", "Bank entry deactivated."))

            Await LoadListAsync()
        End Function

    End Class
End Namespace