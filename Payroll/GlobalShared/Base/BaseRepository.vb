Imports System.Data
Imports System.Data.SqlClient
Imports Dapper
Imports Payroll.GlobalShared.Database

Namespace GlobalShared.Base
    Public MustInherit Class BaseRepository(Of T As Class)

        Protected Function GetConnection() As SqlConnection
            Dim conn = DbConnectionFactory.CreateConnection()
            If conn.State <> ConnectionState.Open Then conn.Open()
            Return conn
        End Function

        Protected Function GetConnection(connection As SqlConnection) As SqlConnection
            Dim conn = connection
            If conn.State <> ConnectionState.Open Then conn.Open()
            Return conn
        End Function

        ' --- QUERY LIST ---
        ' Dinagdagan natin ng commandType parameter na default ay Text (SQL String)
        Public Async Function GetAllAsync(sql As String, Optional params As Object = Nothing, Optional commandType As CommandType = CommandType.Text) As Task(Of List(Of T))
            Using conn = GetConnection()
                Dim result = Await conn.QueryAsync(Of T)(sql, params, commandType:=commandType)
                Return result.ToList()
            End Using
        End Function

        ' --- QUERY SINGLE ---
        Public Async Function GetSingleAsync(sql As String, params As Object, Optional commandType As CommandType = CommandType.Text) As Task(Of T)
            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of T)(sql, params, commandType:=commandType)
            End Using
        End Function

        ' --- EXECUTE (Insert/Update/Delete) ---
        Public Async Function ExecuteAsync(sql As String, params As Object, Optional commandType As CommandType = CommandType.Text) As Task(Of Integer)
            Using conn = GetConnection()
                Return Await conn.ExecuteAsync(sql, params, commandType:=commandType)
            End Using
        End Function

    End Class
End Namespace
