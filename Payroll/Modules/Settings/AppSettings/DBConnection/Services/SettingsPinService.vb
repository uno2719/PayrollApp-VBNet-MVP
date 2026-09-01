Imports System.IO
Imports System.Text.Json
Imports Payroll.GlobalShared.Services

Namespace DBConnection.Services

    ' Naka-hash (hindi encrypted/reversible) ang PIN dito - kailangan lang
    ' natin i-VERIFY kung tugma, hindi natin kailangang "ibalik" ang plain
    ' PIN kahit kailan. Parehong PBKDF2 approach gaya ng password ng users.
    Public Class SettingsPinService
        Implements ISettingsPinService

        Private ReadOnly _filePath As String

        Public Sub New()
            Dim folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PayrollApp")

            If Not Directory.Exists(folder) Then
                Directory.CreateDirectory(folder)
            End If

            _filePath = Path.Combine(folder, "settingspin.json")
        End Sub

        Public Function HasPin() As Boolean Implements ISettingsPinService.HasPin
            Return LoadPin().IsSet
        End Function

        Public Function VerifyPin(enteredPin As String) As Boolean Implements ISettingsPinService.VerifyPin
            Dim stored = LoadPin()

            If Not stored.IsSet Then Return False

            Return PasswordHasher.VerifyPassword(enteredPin, stored.PinHash, stored.PinSalt)
        End Function

        Public Sub SetPin(newPin As String) Implements ISettingsPinService.SetPin
            Dim hashResult = PasswordHasher.HashPassword(newPin)

            Dim pin As New Models.SettingsPin With {
                .PinHash = hashResult.Hash,
                .PinSalt = hashResult.Salt,
                .IsSet = True
            }

            Dim options = New JsonSerializerOptions With {.WriteIndented = True}
            Dim json = JsonSerializer.Serialize(pin, options)
            File.WriteAllText(_filePath, json)
        End Sub

        Private Function LoadPin() As Models.SettingsPin
            Try
                If Not File.Exists(_filePath) Then
                    Return New Models.SettingsPin()
                End If

                Dim json = File.ReadAllText(_filePath)
                Return JsonSerializer.Deserialize(Of Models.SettingsPin)(json)
            Catch ex As Exception
                Return New Models.SettingsPin()
            End Try
        End Function

    End Class

End Namespace