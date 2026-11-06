Imports System.Data
Imports System.Data.SqlClient
Imports Dapper
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Database
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Base

    ''' <summary>
    ''' Isang lugar na maaaring tumutukoy sa record na buburahin.
    ''' TableName at WhereClause lang ang ibinibigay (hindi buong SQL) para
    ''' mailagay ang OBJECT_ID guard - hindi mag-eerror ang check kung wala
    ''' pa ang table sa DB na ginagamit. Pwedeng gamitin ang @Id at @Code.
    ''' </summary>
    Public Class UsageRef
        Public ReadOnly Property Description As String
        Public ReadOnly Property TableName As String
        Public ReadOnly Property WhereClause As String

        Public Sub New(description As String, tableName As String, whereClause As String)
            Me.Description = description
            Me.TableName = tableName
            Me.WhereClause = whereClause
        End Sub
    End Class

    ''' <summary>
    ''' Iisang daanan ng lahat ng totoong Delete sa Settings modules:
    '''   1. Admin lang (AppSession.IsAdmin) - ito ang huling harang kahit lusutan ang UI.
    '''   2. In-use check (count ng mga tumuturo) sa loob ng parehong transaction ng DELETE.
    '''   3. Kapag may gumagamit -> hindi buburahin; sasabihin kung ilan at ituturo sa Deactivate.
    '''   4. Safety net: SQL error 547 (FK violation) ay gagawing malinaw na message.
    ''' </summary>
    Public NotInheritable Class RecordDeleter

        Private Sub New()
        End Sub

        ''' <param name="itemName">Pangalan na lalabas sa message kung walang Code (hal. "this bracket").</param>
        ''' <param name="deleteSql">DELETE statement na may @Id.</param>
        ''' <param name="codeSql">Optional: SELECT ng Code ng record (@Id). Ang resulta ay magagamit bilang @Code sa refs.</param>
        ''' <param name="refs">Mga lugar na maaaring tumuturo sa record.</param>
        ''' <param name="preDeleteSql">Optional: buburahin muna (hal. child brackets), @Id.</param>
        Public Shared Async Function DeleteAsync(
            itemName As String,
            deleteSql As String,
            id As Integer,
            Optional codeSql As String = Nothing,
            Optional refs As IEnumerable(Of UsageRef) = Nothing,
            Optional preDeleteSql As String = Nothing) As Task(Of DeleteResult)

            If Not AppSession.IsAdmin Then
                Return DeleteResult.Fail("Only administrators can delete records.")
            End If

            If id <= 0 Then
                Return DeleteResult.Fail("Please select an entry first.")
            End If

            Try
                Using conn = DbConnectionFactory.CreateConnection()
                    If conn.State <> ConnectionState.Open Then Await conn.OpenAsync()

                    Using tx = conn.BeginTransaction()

                        Dim code As String = Nothing
                        If Not String.IsNullOrWhiteSpace(codeSql) Then
                            code = Await conn.ExecuteScalarAsync(Of String)(codeSql, New With {id}, tx)
                        End If

                        Dim label = If(String.IsNullOrWhiteSpace(code), itemName, $"'{code}'")

                        ' --- IN-USE CHECK ---
                        Dim blockers As New List(Of String)
                        If refs IsNot Nothing Then
                            For Each r In refs
                                Dim countSql =
                                    $"IF OBJECT_ID(N'dbo.{r.TableName}', N'U') IS NULL SELECT 0 " &
                                    $"ELSE SELECT COUNT(1) FROM dbo.{r.TableName} WHERE {r.WhereClause}"

                                Dim n = Await conn.ExecuteScalarAsync(Of Integer)(countSql, New With {id, code}, tx)
                                If n > 0 Then blockers.Add($"   • {n} {r.Description}")
                            Next
                        End If

                        If blockers.Count > 0 Then
                            tx.Rollback()
                            Return DeleteResult.Fail(
                                $"Cannot delete {label} because it is still in use by:" & vbCrLf &
                                String.Join(vbCrLf, blockers) & vbCrLf & vbCrLf &
                                "Use Deactivate instead - it stops appearing in new entries but keeps old records intact.")
                        End If

                        ' --- DELETE ---
                        If Not String.IsNullOrWhiteSpace(preDeleteSql) Then
                            Await conn.ExecuteAsync(preDeleteSql, New With {id}, tx)
                        End If

                        Dim rows = Await conn.ExecuteAsync(deleteSql, New With {id}, tx)

                        If rows = 0 Then
                            tx.Rollback()
                            Return DeleteResult.Fail("This entry no longer exists. Please refresh the list.")
                        End If

                        tx.Commit()
                        Return DeleteResult.Ok()
                    End Using
                End Using

            Catch ex As SqlException When ex.Number = 547
                Return DeleteResult.Fail(
                    "Cannot delete this entry because other records in the database still refer to it." & vbCrLf & vbCrLf &
                    "Use Deactivate instead.")
            Catch ex As Exception
                Return DeleteResult.Fail($"Delete failed: {ex.Message}")
            End Try
        End Function

    End Class

End Namespace
