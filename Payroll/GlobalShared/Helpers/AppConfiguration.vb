Imports Microsoft.Extensions.Configuration
Imports Payroll.DBConnection.Services

Namespace GlobalShared.Database
    Public Module AppConfiguration
        Private _config As IConfiguration

        Public ReadOnly Property ConnectionString As String
            Get
                ' 1. Unahing tignan kung may LOCAL override na naka-set
                '    (via Application Settings, PIN-gated dialog sa Login screen).
                '    Dito papasok ang mga pagbabago kapag nagbago ang IP/server
                '    (hal. dahil sa FortiGate) - hindi na kailangang i-rebuild ang app.
                Dim settingsService As New DatabaseConnectionSettingsService()
                Dim localSettings = settingsService.Load()

                If Not String.IsNullOrWhiteSpace(localSettings.ServerAddress) Then
                    Dim plainPassword = settingsService.GetDecryptedSqlPassword(localSettings)
                    Return settingsService.BuildConnectionString(localSettings, plainPassword)
                End If

                ' 2. Kung wala pang local override (bagong install, walang
                '    na-configure pa), gamitin ang shipped default mula sa
                '    DBConnection.json.
                Return If(_config.GetConnectionString("PayrollDB"), String.Empty)
            End Get
        End Property

        Public Sub Initialize()
            _config = New ConfigurationBuilder() _
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) _
            .AddJsonFile("appsettings.json", optional:=False) _
            .Build()
        End Sub
    End Module

End Namespace