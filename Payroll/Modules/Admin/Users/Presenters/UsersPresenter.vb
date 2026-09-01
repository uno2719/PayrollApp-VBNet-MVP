Imports System.Linq

Namespace Users.Presenters

    Public Class UsersPresenter

        Private ReadOnly _view As Views.IUsersView
        Private ReadOnly _userService As Services.IUserManagementService
        Private ReadOnly _employeeService As Employee.Services.EmployeeService

        Private _selectedUserId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentFilter As String = "Active"
        Private _currentUsersList As List(Of Login.Models.UserModel)
        Private _allModules As List(Of Users.Models.ModuleInfo)

        Public Sub New(
            view As Views.IUsersView,
            userService As Services.IUserManagementService,
            employeeService As Employee.Services.EmployeeService)

            _view = view
            _userService = userService
            _employeeService = employeeService
        End Sub

        Public Async Function LoadAsync() As Task
            _allModules = Await _userService.GetAllModulesAsync()
            Await LoadUsersListAsync(_currentFilter)
            Await LoadEmployeeChoicesAsync()
            _view.SetFormMode(False, False)
        End Function

        Public Async Function LoadUsersListAsync(filter As String) As Task
            _currentFilter = filter
            _currentUsersList = Await _userService.GetAllUsersAsync(filter)
            _view.BindUsersList(_currentUsersList)
        End Function

        ' NOTE: kinukuha lahat ng ACTIVE employees bilang choices - hindi pa
        ' natin ini-exclude yung mga meron nang existing account.
        Private Async Function LoadEmployeeChoicesAsync() As Task
            Dim employees = Await _employeeService.GetAllRecordsAsync("Active")
            Dim choices As New List(Of KeyValuePair(Of String, String))

            For Each emp In employees
                choices.Add(New KeyValuePair(Of String, String)(emp.EmployeeNo, emp.FullName))
            Next

            _view.BindEmployeeChoices(choices)
        End Function

        Public Sub StartNew()
            _selectedUserId = 0
            _isNewMode = True
            _view.ClearFields()
            _view.HideTemporaryPassword()

            ' I-populate ang Module Access ng defaults (lahat NoAccess) -
            ' hindi na basta blangko. Ang View mismo ang bahala mag-toggle
            ' ng visibility nito base sa Administrator Access checkbox.
            Dim defaultAccess = _allModules.Select(
                Function(m) New Users.Models.ModuleAccessItem With {
                    .ModuleId = m.RecordId,
                    .ModuleName = m.ModuleName,
                    .AccessLevel = Users.Models.ModuleAccessLevel.NoAccess
                }).ToList()

            _view.BindModuleAccess(defaultAccess)
            _view.SetFormMode(True, True)
        End Sub

        Public Async Function SelectUserAsync(userId As Integer) As Task
            _selectedUserId = userId
            _isNewMode = False

            ' Kunin ang buong datos ng napiling user mula sa cache
            ' (nasa _currentUsersList na, hindi na kailangang mag-DB call ulit)
            Dim selectedUser = _currentUsersList?.FirstOrDefault(Function(u) u.RecordId = userId)
            If selectedUser IsNot Nothing Then
                _view.Username = selectedUser.Username
                _view.SelectedEmployeeNo = selectedUser.EmployeeNo
                _view.IsAdmin = selectedUser.IsAdmin
                _view.IsActive = selectedUser.IsActive
            End If

            Dim moduleAccess = Await _userService.GetModuleAccessAsync(userId)
            _view.BindModuleAccess(moduleAccess)
            _view.ShowMessage("")
        End Function

        Public Sub StartEdit()
            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            _isNewMode = False
            _view.HideTemporaryPassword()
            _view.SetFormMode(True, False)
        End Sub

        Public Async Function SaveAsync() As Task

            If String.IsNullOrWhiteSpace(_view.Username) Then
                _view.ShowError("Username is required.")
                Return
            End If

            Dim moduleAccess = _view.GetModuleAccessSelections()

            If _isNewMode Then
                Dim result = Await _userService.CreateUserAsync(
                    _view.Username, _view.SelectedEmployeeNo, _view.IsAdmin, moduleAccess)

                If Not result.Success Then
                    _view.ShowError(result.ErrorMessage)
                    Return
                End If

                _view.ShowTemporaryPassword(result.TemporaryPassword)
                _view.ShowMessage("User created successfully. Please share the temporary password securely.")
            Else
                Await _userService.UpdateUserAsync(_selectedUserId, _view.IsAdmin, moduleAccess)
                _view.ShowMessage("User updated successfully.")
            End If

            _view.SetFormMode(False, False)
            _isNewMode = False
            Await LoadUsersListAsync(_currentFilter)

        End Function

        Public Sub CancelEdit()
            _view.ClearFields()
            _view.HideTemporaryPassword()
            _view.SetFormMode(False, False)
            _isNewMode = False
        End Sub

        ' Ito na ngayon ang HARD (soft-delete) na Delete - IsDeleted=1.
        ' Ito yung nasa wbpMainCommands sa itaas (dating "Deactivate").
        Public Async Function DeleteSelectedAsync() As Task
            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            Await _userService.DeleteUserAsync(_selectedUserId)
            _view.ShowMessage("User deleted.")
            _view.ClearFields()
            _view.SetFormMode(False, False)
            _selectedUserId = 0
            Await LoadUsersListAsync(_currentFilter)
        End Function

        ' Bagong "Deactivate/Reactivate" toggle button - IsActive lang ang
        ' ginagalaw, hindi hard delete.
        Public Async Function ToggleActiveSelectedAsync() As Task
            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            If _view.IsActive Then
                Await _userService.DeactivateUserAsync(_selectedUserId)
                _view.ShowMessage("User deactivated.")
                _view.IsActive = False
            Else
                Await _userService.ActivateUserAsync(_selectedUserId)
                _view.ShowMessage("User reactivated.")
                _view.IsActive = True
            End If

            Await LoadUsersListAsync(_currentFilter)
        End Function

        ' Bagong "Reset Password" button - gagawa ng bagong temp password,
        ' ipapakita minsan lang (ShowTemporaryPassword), MustChangePassword=1.
        Public Async Function ResetPasswordSelectedAsync() As Task
            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            Dim newTempPassword = Await _userService.ResetUserPasswordAsync(_selectedUserId)
            _view.ShowTemporaryPassword(newTempPassword)
            _view.ShowMessage("Password has been reset. Please share the new temporary password securely.")
        End Function

    End Class

End Namespace