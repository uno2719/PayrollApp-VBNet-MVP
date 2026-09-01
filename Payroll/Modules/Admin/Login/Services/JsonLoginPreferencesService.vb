Imports System.IO
Imports System.Text.Json
Imports Payroll.Login.Models

Namespace Login.Services
    Public Class JsonLoginPreferencesService
        Implements ILoginPreferencesService

        Private ReadOnly _filePath As String

        Public Sub New()
            Dim folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PayrollApp")

            If Not Directory.Exists(folder) Then
                Directory.CreateDirectory(folder)
            End If

            _filePath = Path.Combine(folder, "loginpreferences.json")
        End Sub

        Public Function Load() As LoginPreferences Implements ILoginPreferencesService.Load
            Try
                If Not File.Exists(_filePath) Then
                    Return New LoginPreferences()
                End If

                Dim json = File.ReadAllText(_filePath)
                Return JsonSerializer.Deserialize(Of LoginPreferences)(json)
            Catch ex As Exception
                Return New LoginPreferences()
            End Try
        End Function

        Public Sub Save(preferences As LoginPreferences) Implements ILoginPreferencesService.Save
            Try
                Dim options = New JsonSerializerOptions With {.WriteIndented = True}
                Dim json = JsonSerializer.Serialize(preferences, options)
                File.WriteAllText(_filePath, json)
            Catch ex As Exception
                ' huwag i-crash kung walang write permission
            End Try
        End Sub

    End Class
End Namespace