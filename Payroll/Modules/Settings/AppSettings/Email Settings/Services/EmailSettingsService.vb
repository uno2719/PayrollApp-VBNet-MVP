' File: Modules/Settings/AppSettings/EmailSettings/Services/EmailSettingsService.vb
Imports System.Net
Imports System.Net.Mail
Imports System.Security.Cryptography
Imports System.Text
Imports Payroll.GlobalShared.Models

Namespace EmailSettings.Services
    Public Class EmailSettingsService
        Implements IEmailSettingsService

        Private ReadOnly _repository As Data.IEmailSettingsRepository

        Public Sub New(repository As Data.IEmailSettingsRepository)
            _repository = repository
        End Sub

        Public Async Function GetAsync() As Task(Of EmailSettingsModel) _
            Implements IEmailSettingsService.GetAsync

            Return Await _repository.GetAsync()
        End Function

        Public Async Function SaveAsync(item As EmailSettingsModel, plainPassword As String, userName As String) As Task(Of EmailSettingsSaveResult) _
            Implements IEmailSettingsService.SaveAsync

            If String.IsNullOrWhiteSpace(item.SmtpHost) Then
                Return Fail("SMTP Host is required.")
            End If

            If item.SmtpPort < 1 OrElse item.SmtpPort > 65535 Then
                Return Fail("Port must be between 1 and 65535.")
            End If

            If Not String.IsNullOrWhiteSpace(plainPassword) Then
                ' May bagong tina-type na password - i-encrypt at gamitin.
                item.SmtpPasswordEncrypted = EncryptString(plainPassword)
            Else
                ' Blangko ang password field - HINDI ibig sabihin nito na
                ' gustong burahin ni user ang password. Sadyang hindi natin
                ' ipinapakita ulit ang naka-save na password (tingnan ang
                ' LoadAsync sa Presenter), kaya blangko talaga siya sa
                ' tuwing bubuksan ang screen. Panatilihin ang dati.
                Dim existing = Await _repository.GetAsync()
                item.SmtpPasswordEncrypted = If(existing?.SmtpPasswordEncrypted, String.Empty)
            End If

            If item.EmailSettingsId = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New EmailSettingsSaveResult With {.Success = True}
        End Function

        Public Function GetDecryptedPassword(item As EmailSettingsModel) As String _
            Implements IEmailSettingsService.GetDecryptedPassword

            If item Is Nothing OrElse String.IsNullOrEmpty(item.SmtpPasswordEncrypted) Then
                Return String.Empty
            End If

            Return DecryptString(item.SmtpPasswordEncrypted)
        End Function

        Public Async Function SendTestEmailAsync(item As EmailSettingsModel, plainPassword As String, recipient As String) As Task(Of EmailTestResult) _
            Implements IEmailSettingsService.SendTestEmailAsync

            If String.IsNullOrWhiteSpace(item.SmtpHost) Then
                Return TestFail("SMTP Host is required.")
            End If

            If String.IsNullOrWhiteSpace(item.SmtpUsername) Then
                Return TestFail("Username is required to send a test email.")
            End If

            If Not IsValidEmail(item.SmtpUsername) Then
                Return TestFail("Username must be a valid email address to be used as the sender.")
            End If

            If Not IsValidEmail(recipient) Then
                Return TestFail("Please enter a valid recipient email address.")
            End If

            ' Kung walang bagong tina-type, gamitin ang naka-save na
            ' password - para matest mo ang existing settings nang hindi
            ' kailangang i-type ulit ang password.
            Dim password = plainPassword
            If String.IsNullOrWhiteSpace(password) Then
                password = GetDecryptedPassword(item)
            End If

            Try
                Using client As New SmtpClient(item.SmtpHost, item.SmtpPort)
                    client.EnableSsl = item.UseSsl
                    client.Credentials = New NetworkCredential(item.SmtpUsername, password)

                    Using message As New MailMessage(
                        item.SmtpUsername,
                        recipient,
                        "Payroll App - Test Email",
                        "This is a test message from the Payroll App email settings screen." &
                        vbCrLf & vbCrLf &
                        "If you received this, your SMTP settings are working.")

                        Await client.SendMailAsync(message)
                    End Using
                End Using

                Return New EmailTestResult With {
                    .Success = True,
                    .Message = $"Test email sent to {recipient}."}

            Catch ex As SmtpException
                ' Karaniwang sanhi: maling password, maling port, o naka-block
                ' ang SMTP sa network. Ipinapakita natin ang totoong mensahe
                ' galing sa server dahil doon lang makikita kung alin doon.
                Return TestFail($"SMTP error: {ex.Message}")
            Catch ex As Exception
                Return TestFail($"Could not send: {ex.Message}")
            End Try
        End Function

        ' ===================================================
        ' ENCRYPTION
        '
        ' AES ang gamit dito, HINDI DPAPI - kabaligtaran ng
        ' DatabaseConnectionSettingsService.
        '
        ' Bakit magkaiba: naka-tali ang DPAPI sa MISMONG computer na
        ' nag-encrypt. Okay 'yon doon dahil per-machine na JSON file
        ' naman ang DB settings. Pero nasa database ang email settings at
        ' binabasa ng LAHAT ng workstation - kung DPAPI ang gamit,
        ' hindi ma-de-decrypt ng workstation B ang na-save ng workstation A.
        '
        ' ⚠️ ALAM MONG SANA: naka-embed sa app ang key, kaya OBFUSCATION
        ' ito, hindi tunay na seguridad. Kung may makakuha ng kopya ng
        ' database AT ng app, mababasa niya ang password. Ang PROTEKSYON
        ' nito ay laban sa kaswal na pagtingin sa table (hal. may nagbukas
        ' ng SSMS) - hindi laban sa determinadong attacker.
        '
        ' Ang tunay na panangga: gumamit ng DEDICATED sending account
        ' (gaya ng payslip@pacsports.com) na send-only ang kaya, hindi ang
        ' personal na email account ng kahit sino, at higpitan kung sino
        ' ang may access sa database.
        ' ===================================================

        ' Palitan mo 'to ng sarili mong string bago mag-production, at
        ' HUWAG mo nang babaguhin pagkatapos - hindi na mababasa ang mga
        ' dating na-save na password kapag binago mo ito.
        Private Const KeyMaterial As String = "PayrollApp.EmailSettings.v1"
        Private Shared ReadOnly KeySalt As Byte() =
            {&H50, &H41, &H43, &H53, &H50, &H4F, &H52, &H54, &H53, &H32, &H30, &H32, &H36, &H21, &H40, &H23}

        Private Shared Function BuildKey() As Byte()
            Using derive As New Rfc2898DeriveBytes(KeyMaterial, KeySalt, 100000, HashAlgorithmName.SHA256)
                Return derive.GetBytes(32)   ' AES-256
            End Using
        End Function

        Private Shared Function EncryptString(plainText As String) As String
            Using aes As Aes = Aes.Create()
                aes.Key = BuildKey()
                aes.GenerateIV()

                Using encryptor = aes.CreateEncryptor()
                    Dim plainBytes = Encoding.UTF8.GetBytes(plainText)
                    Dim cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length)

                    ' Isinasama ang IV sa unahan ng ciphertext - kailangan
                    ' siya sa pag-decrypt at hindi naman siya sikreto.
                    Dim combined(aes.IV.Length + cipherBytes.Length - 1) As Byte
                    Buffer.BlockCopy(aes.IV, 0, combined, 0, aes.IV.Length)
                    Buffer.BlockCopy(cipherBytes, 0, combined, aes.IV.Length, cipherBytes.Length)

                    Return Convert.ToBase64String(combined)
                End Using
            End Using
        End Function



        Private Shared Function DecryptString(encryptedText As String) As String
            Try
                Dim combined = Convert.FromBase64String(encryptedText)

                Using aes As Aes = Aes.Create()
                    aes.Key = BuildKey()

                    Dim iv(aes.BlockSize \ 8 - 1) As Byte
                    Buffer.BlockCopy(combined, 0, iv, 0, iv.Length)
                    aes.IV = iv

                    Using decryptor = aes.CreateDecryptor()
                        Dim cipherLength = combined.Length - iv.Length
                        Dim plainBytes = decryptor.TransformFinalBlock(combined, iv.Length, cipherLength)
                        Return Encoding.UTF8.GetString(plainBytes)
                    End Using
                End Using

            Catch
                ' Sirang data o binago ang KeyMaterial - mas mabuting
                ' blangko kaysa mag-crash ang buong screen. Kailangan na
                ' lang i-type ulit ni user ang password at mag-Save.
                Return String.Empty
            End Try
        End Function

        Private Shared Function IsValidEmail(value As String) As Boolean
            If String.IsNullOrWhiteSpace(value) Then Return False

            Try
                Dim parsed As New MailAddress(value)
                Return parsed.Address = value.Trim()
            Catch
                Return False
            End Try
        End Function

        Private Shared Function Fail(message As String) As EmailSettingsSaveResult
            Return New EmailSettingsSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

        Private Shared Function TestFail(message As String) As EmailTestResult
            Return New EmailTestResult With {.Success = False, .Message = message}
        End Function

    End Class
End Namespace
