Imports System.IO
Imports System.Text.Json
Imports Payroll.GlobalShared.Contracts
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Services
    Public Class JsonThemeSettingsService
        Implements IThemeSettingsService

        Private ReadOnly _filePath As String

        Public Sub New()
            Dim folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PayrollApp")

            If Not Directory.Exists(folder) Then
                Directory.CreateDirectory(folder)
            End If

            _filePath = Path.Combine(folder, "themesettings.json")
        End Sub

        Public Function Load() As ThemeSettings Implements IThemeSettingsService.Load
            Try
                If Not File.Exists(_filePath) Then
                    Return New ThemeSettings() ' unang beses lang bubukas - default values
                End If

                Dim json = File.ReadAllText(_filePath)
                Return JsonSerializer.Deserialize(Of ThemeSettings)(json)
            Catch ex As Exception
                ' Kung corrupted o may access issue, huwag i-crash ang app -
                ' bumalik na lang sa default settings.
                Return New ThemeSettings()
            End Try
        End Function

        Public Sub Save(settings As ThemeSettings) Implements IThemeSettingsService.Save
            Try
                Dim options = New JsonSerializerOptions With {.WriteIndented = True}
                Dim json = JsonSerializer.Serialize(settings, options)
                File.WriteAllText(_filePath, json)
            Catch ex As Exception
                ' Huwag i-crash kung sakaling walang write permission -
                ' babalik na lang sa default sa susunod na buksan ang app.
            End Try
        End Sub

    End Class
End Namespace