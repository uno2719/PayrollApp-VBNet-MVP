Namespace DBConnection.Services
    Public Interface IDatabaseConnectionSettingsService
        Function Load() As Models.DatabaseConnectionSettings

        ' Ang plainSqlPassword ay ine-encrypt sa loob mismo ng Save() -
        ' hindi kailangan ng caller na i-encrypt ito nang mano-mano.
        Sub Save(settings As Models.DatabaseConnectionSettings, plainSqlPassword As String)

        ' Kinukuha ang NA-DECRYPT na SQL password - gamitin lang ito kung
        ' talagang kailangan mong bumuo ng connection string GAMIT ang
        ' naka-SAVE na (dating) password.
        Function GetDecryptedSqlPassword(settings As Models.DatabaseConnectionSettings) As String

        ' Bumubuo ng aktwal na SQL Server connection string. Explicit na
        ' pinapasa ang plainPassword (hindi kinukuha diretso mula sa
        ' settings.SqlPasswordEncrypted) - dahil kailangan din nito
        ' gumana para sa "Test Connection" gamit ang BAGONG tina-type na
        ' password, bago pa man ito ma-save/ma-encrypt.
        Function BuildConnectionString(settings As Models.DatabaseConnectionSettings, plainPassword As String) As String
    End Interface
End Namespace