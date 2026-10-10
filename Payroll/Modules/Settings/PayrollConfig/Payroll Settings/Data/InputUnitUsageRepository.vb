Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data

    Public Class InputUnitUsageRepository
        Inherits BaseRepository(Of InputUnitUsage)
        Implements IInputUnitUsageRepository

        Public Async Function GetUsageAsync(category As String, code As String) As Task(Of InputUnitUsage) _
            Implements IInputUnitUsageRepository.GetUsageAsync

            Dim sql = "
                SELECT ISNULL(SUM(CASE WHEN c.Status IN (@processed, @posted) THEN 1 ELSE 0 END), 0) AS ProcessedEntries,
                       ISNULL(SUM(CASE WHEN c.Status NOT IN (@processed, @posted) THEN 1 ELSE 0 END), 0) AS DraftEntries
                FROM tblPayrollInputTxn t
                INNER JOIN tblCutoff c ON c.CutoffID = t.CutoffID
                WHERE t.TxnCategory = @category
                  AND t.TxnCode = @code"

            Using conn = GetConnection()
                Return Await conn.QuerySingleAsync(Of InputUnitUsage)(sql, New With {
                    category, code, .processed = CByte(CutoffStatus.Processed), .posted = CByte(CutoffStatus.Posted)
                })
            End Using
        End Function

        Public Async Function ClearDraftEntriesAsync(category As String, code As String) As Task(Of Integer) _
            Implements IInputUnitUsageRepository.ClearDraftEntriesAsync

            Dim sql = "
                DELETE t
                FROM tblPayrollInputTxn t
                INNER JOIN tblCutoff c ON c.CutoffID = t.CutoffID
                WHERE t.TxnCategory = @category
                  AND t.TxnCode = @code
                  AND c.Status NOT IN (@processed, @posted)"

            Using conn = GetConnection()
                Return Await conn.ExecuteAsync(sql, New With {
                    category, code, .processed = CByte(CutoffStatus.Processed), .posted = CByte(CutoffStatus.Posted)
                })
            End Using
        End Function

    End Class

End Namespace
