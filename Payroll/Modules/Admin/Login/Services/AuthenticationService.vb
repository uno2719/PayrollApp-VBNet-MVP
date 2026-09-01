Imports Payroll.GlobalShared.Services

Namespace Login.Services

    Public Class AuthenticationService
        Implements IAuthenticationService

        Private ReadOnly _userRepo As Data.IUserRepository
        Private Const MaxFailedAttempts As Integer = 5
        Private Const LockoutMinutes As Integer = 15

        Public Sub New(userRepo As Data.IUserRepository)
            _userRepo = userRepo
        End Sub

        Public Async Function AuthenticateAsync(username As String, password As String) As Task(Of AuthenticationResult) _
            Implements IAuthenticationService.AuthenticateAsync
            Dim user = Await _userRepo.GetByUsernameAsync(username)

            ' Generic na error message - HUWAG sasabihin kung username ba o password
            ' ang mali, para hindi malaman ng gustong mang-atake kung tama ba yung
            ' username na ginamit niya.
            Dim invalidCredentials = New AuthenticationResult With {
                .Success = False,
                .ErrorMessage = "Invalid username or password."
            }

            If user Is Nothing Then
                Return invalidCredentials
            End If

            If Not user.IsActive Then
                Return New AuthenticationResult With {
                    .Success = False,
                    .ErrorMessage = $"This account has been deactivated.{vbCrLf}Please contact your administrator."
                }
            End If

            If user.LockedUntil.HasValue AndAlso user.LockedUntil.Value > DateTime.Now Then
                Return New AuthenticationResult With {
                    .Success = False,
                    .ErrorMessage = $"Account is locked. Please try again after {user.LockedUntil.Value:hh:mm tt}."
                }
            End If

            Dim isPasswordValid = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt)

            If Not isPasswordValid Then
                Await _userRepo.IncrementFailedLoginAsync(user.RecordId)

                Dim updatedFailCount = user.FailedLoginCount + 1
                If updatedFailCount >= MaxFailedAttempts Then
                    Await _userRepo.LockAccountAsync(user.RecordId, DateTime.Now.AddMinutes(LockoutMinutes))
                    Return New AuthenticationResult With {
                        .Success = False,
                        .ErrorMessage = $"Too many failed attempts. Account locked for {LockoutMinutes} minutes."
                    }
                End If

                Return invalidCredentials
            End If

            Await _userRepo.UpdateLastLoginAsync(user.RecordId)

            Return New AuthenticationResult With {
                .Success = True,
                .User = user
            }

        End Function

        Public Async Function ChangePasswordAsync(username As String, currentPassword As String, newPassword As String) As Task(Of ChangePasswordResult) _
            Implements IAuthenticationService.ChangePasswordAsync
            Dim user = Await _userRepo.GetByUsernameAsync(username)

            If user Is Nothing Then
                Return New ChangePasswordResult With {
                    .success = False,
                    .ErrorMessage = "User not found."
                }
            End If

            Dim isCurrentPasswordValid = PasswordHasher.VerifyPassword(
                currentPassword, user.PasswordHash, user.PasswordSalt)

            If Not isCurrentPasswordValid Then
                Return New ChangePasswordResult With {
                    .success = False,
                    .ErrorMessage = "Current password is incorrect."
                }
            End If

            If String.IsNullOrWhiteSpace(newPassword) OrElse newPassword.Length < 8 Then
                Return New ChangePasswordResult With {
                    .success = False,
                    .ErrorMessage = "New password must be at least 8 characters."
                }
            End If

            Dim hashResult = PasswordHasher.HashPassword(newPassword)

            ' Muling gamit ang ResetPasswordAsync na ginawa na natin dati para
            ' sa recovery tool - parehong operasyon naman talaga ito
            ' (i-update ang hash+salt, i-unlock kung sakaling naka-lock).
            Dim success = Await _userRepo.ResetPasswordAsync(
                user.RecordId, hashResult.Hash, hashResult.Salt)

            If success Then
                Return New ChangePasswordResult With {.success = True}
            Else
                Return New ChangePasswordResult With {
                    .success = False,
                    .ErrorMessage = "Failed to update password. Please try again."
                }
            End If

        End Function

    End Class

End Namespace