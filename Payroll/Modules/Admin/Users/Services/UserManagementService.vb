Imports Payroll.GlobalShared.Services
Imports Payroll.Login.Data
Imports Payroll.Login.Models

Namespace Users.Services

    Public Class UserManagementService
        Implements IUserManagementService

        Private ReadOnly _userRepo As IUserRepository

        Public Sub New(userRepo As IUserRepository)
            _userRepo = userRepo
        End Sub

        Public Async Function GetAllUsersAsync(filter As String) As Task(Of List(Of UserModel)) _
            Implements IUserManagementService.GetAllUsersAsync

            Return Await _userRepo.GetAllUsersAsync(filter)
        End Function

        Public Async Function GetAllModulesAsync() As Task(Of List(Of Models.ModuleInfo)) _
            Implements IUserManagementService.GetAllModulesAsync

            Return Await _userRepo.GetAllModulesAsync()
        End Function

        Public Async Function GetModuleAccessAsync(userId As Integer) As Task(Of List(Of Models.ModuleAccessItem)) _
            Implements IUserManagementService.GetModuleAccessAsync

            Return Await _userRepo.GetModuleAccessAsync(userId)
        End Function

        Public Async Function CreateUserAsync(
            username As String, employeeNo As String, isAdmin As Boolean,
            moduleAccess As List(Of Models.ModuleAccessItem)) As Task(Of CreateUserResult) _
            Implements IUserManagementService.CreateUserAsync

            If String.IsNullOrWhiteSpace(username) Then
                Return New CreateUserResult With {.Success = False, .ErrorMessage = "Username is required."}
            End If

            If String.IsNullOrWhiteSpace(employeeNo) Then
                Return New CreateUserResult With {.Success = False, .ErrorMessage = "Please select an employee."}
            End If

            Dim isTaken = Await _userRepo.IsUsernameTakenAsync(username)
            If isTaken Then
                Return New CreateUserResult With {.Success = False, .ErrorMessage = "This username is already taken."}
            End If

            Dim tempPassword = GenerateTemporaryPassword()
            Dim hashResult = PasswordHasher.HashPassword(tempPassword)

            Dim newUser As New UserModel With {
                .Username = username,
                .PasswordHash = hashResult.Hash,
                .PasswordSalt = hashResult.Salt,
                .IsAdmin = isAdmin,
                .EmployeeNo = employeeNo,
                .MustChangePassword = True,
                .CreatedBy = AppSession.CurrentUser
            }

            Dim newId = Await _userRepo.InsertAsync(newUser)

            ' Hindi na kailangang i-save ang module access kung Admin -
            ' full access na siya by default (bypass sa PermissionService mamaya).
            If Not isAdmin AndAlso moduleAccess IsNot Nothing Then
                Await _userRepo.SaveModuleAccessAsync(newId, moduleAccess)
            End If

            Return New CreateUserResult With {
                .Success = True,
                .TemporaryPassword = tempPassword
            }

        End Function

        Public Async Function UpdateUserAsync(
            recordId As Integer, isAdmin As Boolean,
            moduleAccess As List(Of Models.ModuleAccessItem)) As Task(Of Boolean) _
            Implements IUserManagementService.UpdateUserAsync

            Await _userRepo.UpdateIsAdminAsync(recordId, isAdmin)

            If Not isAdmin AndAlso moduleAccess IsNot Nothing Then
                Await _userRepo.SaveModuleAccessAsync(recordId, moduleAccess)
            End If

            Return True

        End Function

        Public Async Function DeactivateUserAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserManagementService.DeactivateUserAsync

            Return Await _userRepo.DeactivateAsync(recordId)
        End Function

        Public Async Function ActivateUserAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserManagementService.ActivateUserAsync

            Return Await _userRepo.ActivateAsync(recordId)
        End Function

        Public Async Function ResetUserPasswordAsync(recordId As Integer) As Task(Of String) _
            Implements IUserManagementService.ResetUserPasswordAsync

            Dim tempPassword = GenerateTemporaryPassword()
            Dim hashResult = PasswordHasher.HashPassword(tempPassword)

            Await _userRepo.AdminResetPasswordAsync(recordId, hashResult.Hash, hashResult.Salt)

            Return tempPassword
        End Function

        Public Async Function DeleteUserAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IUserManagementService.DeleteUserAsync

            Return Await _userRepo.DeleteAsync(recordId)
        End Function

        ' Simpleng random password generator - sapat na kasiguraduhan dahil
        ' TEMPORARY lang ito, palitan din naman agad sa unang login.
        Private Function GenerateTemporaryPassword() As String
            Const chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789"
            Dim rng = New Random()
            Dim sb As New System.Text.StringBuilder()

            For i As Integer = 1 To 10
                sb.Append(chars(rng.Next(chars.Length)))
            Next

            Return sb.ToString()
        End Function

    End Class

End Namespace