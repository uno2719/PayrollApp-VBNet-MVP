Namespace DBConnection.Models

    Public Enum DbAuthenticationType
        SqlServerAuthentication
        WindowsAuthentication ' Reserved para sa hinaharap - hindi pa ginagamit
    End Enum

    Public Class DatabaseConnectionSettings
        Public Property ServerAddress As String = String.Empty
        Public Property DatabaseName As String = String.Empty
        Public Property SqlUsername As String = String.Empty
        Public Property SqlPasswordEncrypted As String = String.Empty ' DPAPI-encrypted, hindi plain text
        Public Property AuthenticationType As DbAuthenticationType = DbAuthenticationType.SqlServerAuthentication
    End Class
End Namespace