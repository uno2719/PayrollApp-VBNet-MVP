Imports System.Security.Cryptography

Namespace GlobalShared.Services

    ' Ginagamit natin ang PBKDF2 (industry-standard na password hashing algorithm)
    ' sa halip na i-store ang password nang plain text o gamit ang mas mahinang
    ' hashing tulad ng MD5/SHA1 lang.
    ' Shared na ito dahil ginagamit ng higit sa isang module (Login AT AppSettings).
    Public Class PasswordHasher

        Private Const SaltSize As Integer = 16
        Private Const KeySize As Integer = 32
        Private Const Iterations As Integer = 100000

        ' Gamitin ito PAG NAGSE-SET UP ng bagong user account (hal. seed ng unang Admin)
        Public Shared Function HashPassword(password As String) As (Hash As String, Salt As String)
            Dim saltBytes(SaltSize - 1) As Byte
            Using rng = RandomNumberGenerator.Create()
                rng.GetBytes(saltBytes)
            End Using

            Dim hashBytes = GenerateHash(password, saltBytes)

            Return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes))
        End Function

        ' Gamitin ito PAG LUMOLOGIN - kinukumpara ang typed password sa naka-store na hash
        Public Shared Function VerifyPassword(password As String, storedHash As String, storedSalt As String) As Boolean
            Dim saltBytes = Convert.FromBase64String(storedSalt)
            Dim hashBytes = GenerateHash(password, saltBytes)
            Dim computedHash = Convert.ToBase64String(hashBytes)

            Return computedHash = storedHash
        End Function

        Private Shared Function GenerateHash(password As String, saltBytes As Byte()) As Byte()
            Using pbkdf2 As New Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256)
                Return pbkdf2.GetBytes(KeySize)
            End Using
        End Function

    End Class

End Namespace