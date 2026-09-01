Imports Payroll.GlobalShared.Services

Namespace Login.Services

    ' Ginagamit lang ito para sa mga one-time/emergency na operations
    ' (unang setup ng Admin, o password recovery). Hindi ito permanenteng
    ' bahagi ng normal application flow - itatawag lang ito via command-line
    ' argument (tignan ang Application.vb).
    Public Module SeedTool

        Public Async Function SeedAdminUserAsync(username As String, plainPassword As String, employeeNo As String) As Task(Of String)

            Dim userRepo As New Data.UserRepository()

            Dim existing = Await userRepo.GetByUsernameAsync(username)
            If existing IsNot Nothing Then
                Return $"User '{username}' already exists. Seeding was not performed."
            End If

            Dim hashResult = PasswordHasher.HashPassword(plainPassword)

            Dim newUser As New Models.UserModel With {
                .Username = username,
                .PasswordHash = hashResult.Hash,
                .PasswordSalt = hashResult.Salt,
                .IsAdmin = True,
                .EmployeeNo = employeeNo,
                .CreatedBy = "SYSTEM"
            }

            Dim newId = Await userRepo.InsertAsync(newUser)

            Return $"Successfully created Admin user '{username}' (RecordId: {newId})."

        End Function

        ' Emergency recovery tool - ginagamit kung nawalan ng access ang
        ' isang account (nakalimutan ang password, o naka-lock) at walang
        ' ibang Admin na pwedeng mag-reset mula sa loob ng app mismo.
        Public Async Function ResetAdminPasswordAsync(username As String, newPlainPassword As String) As Task(Of String)

            Dim userRepo As New Data.UserRepository()

            Dim existing = Await userRepo.GetByUsernameAsync(username)
            If existing Is Nothing Then
                Return $"No user found with username '{username}'. Reset was not performed."
            End If

            Dim hashResult = PasswordHasher.HashPassword(newPlainPassword)

            Dim success = Await userRepo.ResetPasswordAsync(
                existing.RecordId, hashResult.Hash, hashResult.Salt)

            If success Then
                Return $"Successfully reset password for '{username}'. The account has also been unlocked if it was locked."
            Else
                Return $"There was a problem resetting the password for '{username}'."
            End If

        End Function

    End Module

End Namespace