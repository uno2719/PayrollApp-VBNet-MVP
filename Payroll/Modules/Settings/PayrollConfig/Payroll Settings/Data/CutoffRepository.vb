Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data

    Public Class CutoffRepository
        Inherits BaseRepository(Of CutoffModel)
        Implements ICutoffRepository

        Public Function GetAllAsync() As Task(Of List(Of CutoffModel)) Implements ICutoffRepository.GetAllAsync
            Return MyBase.GetAllAsync("SELECT * FROM tblCutoff ORDER BY CutoffStart DESC")
        End Function

        Public Async Function OverlapExistsAsync(cycleType As String, cutoffStart As Date, cutoffEnd As Date, excludeId As Integer) As Task(Of Boolean) _
            Implements ICutoffRepository.OverlapExistsAsync

            ' Two date ranges overlap when each one starts before the other ends.
            Dim sql = "
                SELECT COUNT(1) FROM tblCutoff
                WHERE CycleType = @CycleType
                  AND CutoffID <> @ExcludeId
                  AND CutoffStart <= @CutoffEnd
                  AND CutoffEnd >= @CutoffStart"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {cycleType, cutoffStart, cutoffEnd, excludeId})
                Return count > 0
            End Using
        End Function

        Public Async Function InsertAsync(item As CutoffModel, userName As String) As Task(Of Integer) Implements ICutoffRepository.InsertAsync
            Dim sql = "
                INSERT INTO tblCutoff (CycleType, CutoffYear, CutoffStart, CutoffEnd, PayDate, CutoffLabel, Status, CreatedBy)
                OUTPUT INSERTED.CutoffID
                VALUES (@CycleType, @CutoffYear, @CutoffStart, @CutoffEnd, @PayDate, @CutoffLabel, @Status, @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.CycleType, item.CutoffYear, item.CutoffStart, item.CutoffEnd,
                    item.PayDate, item.CutoffLabel, .Status = CByte(item.Status), userName
                })
            End Using
        End Function

        Public Async Function UpdateAsync(item As CutoffModel, userName As String) As Task(Of Boolean) Implements ICutoffRepository.UpdateAsync
            Dim sql = "
                UPDATE tblCutoff
                SET CycleType = @CycleType,
                    CutoffYear = @CutoffYear,
                    CutoffStart = @CutoffStart,
                    CutoffEnd = @CutoffEnd,
                    PayDate = @PayDate,
                    CutoffLabel = @CutoffLabel,
                    Status = @Status,
                    ModifiedBy = @UserName,
                    ModifiedDate = SYSDATETIME()
                WHERE CutoffID = @CutoffID"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.CutoffID, item.CycleType, item.CutoffYear, item.CutoffStart, item.CutoffEnd,
                item.PayDate, item.CutoffLabel, .Status = CByte(item.Status), userName
            })
            Return rows > 0
        End Function

        Public Async Function CountDraftEndingBeforeAsync(beforeDate As Date) As Task(Of Integer) _
            Implements ICutoffRepository.CountDraftEndingBeforeAsync

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(
                    "SELECT COUNT(1) FROM tblCutoff WHERE Status = @draft AND CutoffEnd < @beforeDate",
                    New With {.draft = CByte(CutoffStatus.Draft), beforeDate})
            End Using
        End Function

        Public Async Function CloseDraftEndingBeforeAsync(beforeDate As Date, userName As String) As Task(Of Integer) _
            Implements ICutoffRepository.CloseDraftEndingBeforeAsync

            Dim sql = "
                UPDATE tblCutoff
                SET Status = @closed,
                    ModifiedBy = @UserName,
                    ModifiedDate = SYSDATETIME()
                WHERE Status = @draft AND CutoffEnd < @beforeDate"

            Return Await MyBase.ExecuteAsync(sql, New With {
                .closed = CByte(CutoffStatus.Closed), .draft = CByte(CutoffStatus.Draft), beforeDate, userName
            })
        End Function

        Public Async Function BulkInsertAsync(items As List(Of CutoffModel), userName As String) As Task(Of Integer) Implements ICutoffRepository.BulkInsertAsync
            Dim inserted = 0
            For Each item In items
                Dim overlaps = Await OverlapExistsAsync(item.CycleType, item.CutoffStart, item.CutoffEnd, 0)
                If Not overlaps Then
                    Await InsertAsync(item, userName)
                    inserted += 1
                End If
            Next
            Return inserted
        End Function

    End Class

End Namespace