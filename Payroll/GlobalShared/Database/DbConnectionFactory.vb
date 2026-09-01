Imports System.Data.SqlClient

Namespace GlobalShared.Database
    Public Class DbConnectionFactory
        Public Shared Function CreateConnection() As SqlConnection
            Return New SqlConnection(AppConfiguration.ConnectionString)
        End Function
    End Class
End Namespace