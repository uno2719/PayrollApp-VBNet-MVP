Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json

Namespace DBConnection.Services

    Public Class DatabaseConnectionSettingsService
        Implements IDatabaseConnectionSettingsService

        Private ReadOnly _filePath As String

        Public Sub New()
            Dim folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PayrollApp")

            If Not Directory.Exists(folder) Then
                Directory.CreateDirectory(folder)
            End If

            _filePath = Path.Combine(folder, "connectionsettings.json")
        End Sub

        Public Function Load() As Models.DatabaseConnectionSettings _
            Implements IDatabaseConnectionSettingsService.Load

            Try
                If Not File.Exists(_filePath) Then
                    Return New Models.DatabaseConnectionSettings()
                End If

                Dim json = File.ReadAllText(_filePath)
                Return JsonSerializer.Deserialize(Of Models.DatabaseConnectionSettings)(json)
            Catch ex As Exception
                Return New Models.DatabaseConnectionSettings()
            End Try
        End Function

        Public Sub Save(settings As Models.DatabaseConnectionSettings, plainSqlPassword As String) _
            Implements IDatabaseConnectionSettingsService.Save

            ' Sa ngayon, SqlServerAuthentication lang ang aktibong ginagamit
            ' (WindowsAuthentication ay reserved pa lang) - pero panatilihin
            ' natin ang check na ito para future-proof, kapag na-activate na.
            If settings.AuthenticationType = Models.DbAuthenticationType.SqlServerAuthentication _
               AndAlso Not String.IsNullOrEmpty(plainSqlPassword) Then

                settings.SqlPasswordEncrypted = EncryptString(plainSqlPassword)
            Else
                settings.SqlPasswordEncrypted = String.Empty
            End If

            Dim options = New JsonSerializerOptions With {.WriteIndented = True}
            Dim json = JsonSerializer.Serialize(settings, options)
            File.WriteAllText(_filePath, json)

        End Sub

        Public Function GetDecryptedSqlPassword(settings As Models.DatabaseConnectionSettings) As String _
            Implements IDatabaseConnectionSettingsService.GetDecryptedSqlPassword

            If String.IsNullOrEmpty(settings.SqlPasswordEncrypted) Then
                Return String.Empty
            End If

            Return DecryptString(settings.SqlPasswordEncrypted)
        End Function

        Public Function BuildConnectionString(settings As Models.DatabaseConnectionSettings, plainPassword As String) As String _
            Implements IDatabaseConnectionSettingsService.BuildConnectionString

            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder With {
                .DataSource = settings.ServerAddress,
                .InitialCatalog = settings.DatabaseName
            }

            If settings.AuthenticationType = Models.DbAuthenticationType.WindowsAuthentication Then
                ' Reserved path - hindi pa aktibong ginagamit/tine-test.
                builder.IntegratedSecurity = True
            Else
                builder.IntegratedSecurity = False
                builder.UserID = settings.SqlUsername
                builder.Password = plainPassword
            End If

            Return builder.ConnectionString

        End Function

        ' ===================================================
        ' DPAPI ENCRYPTION - naka-tali sa MISMONG COMPUTER na ito
        ' (LocalMachine scope, hindi CurrentUser) - kaya kahit sinong
        ' Windows user ang naka-login sa parehong computer, gagana pa
        ' rin ang pag-decrypt. Pero kung kokopyahin ang file papunta
        ' sa IBANG computer, HINDI na ito mabubuksan doon - dagdag
        ' proteksyon kung sakaling ma-leak/ma-copy ang config file.
        ' ===================================================

        Private Shared Function EncryptString(plainText As String) As String
            Dim plainBytes = Encoding.UTF8.GetBytes(plainText)
            Dim encryptedBytes = ProtectedData.Protect(
                plainBytes, Nothing, DataProtectionScope.LocalMachine)

            Return Convert.ToBase64String(encryptedBytes)
        End Function

        Private Shared Function DecryptString(encryptedText As String) As String
            Dim encryptedBytes = Convert.FromBase64String(encryptedText)
            Dim plainBytes = ProtectedData.Unprotect(
                encryptedBytes, Nothing, DataProtectionScope.LocalMachine)

            Return Encoding.UTF8.GetString(plainBytes)
        End Function

    End Class

End Namespace