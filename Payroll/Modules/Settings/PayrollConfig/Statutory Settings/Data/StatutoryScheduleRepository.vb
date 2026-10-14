Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Data

    Public Class StatutoryScheduleRepository
        Inherits BaseRepository(Of StatutoryScheduleModel)
        Implements IStatutoryScheduleRepository

        Private Const TableName As String = "tblStatutorySchedule"

        Private Const InsertSql As String =
            "INSERT INTO tblStatutorySchedule
                 (PayCycleType, Agency, PeriodNo, SharePercent, VoluntaryMode, CreatedAt, CreatedBy)
             VALUES
                 (@PayCycleType, @Agency, @PeriodNo, @SharePercent, @VoluntaryMode, SYSDATETIME(), @UserName)"

        ' --- READ ---
        Public Function GetAllAsync() As Task(Of List(Of StatutoryScheduleModel)) _
            Implements IStatutoryScheduleRepository.GetAllAsync

            Return MyBase.GetAllAsync(
                $"SELECT ScheduleId, PayCycleType, Agency, PeriodNo, SharePercent, VoluntaryMode,
                         CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                  FROM {TableName}
                  ORDER BY PayCycleType, Agency, PeriodNo")
        End Function

        ' --- INSERT (default seed) ---
        Public Async Function InsertManyAsync(items As List(Of StatutoryScheduleModel), userName As String) As Task _
            Implements IStatutoryScheduleRepository.InsertManyAsync

            If items Is Nothing OrElse items.Count = 0 Then Return

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        For Each item In items
                            Await conn.ExecuteAsync(InsertSql, New With {
                                item.PayCycleType, item.Agency, item.PeriodNo, item.SharePercent, item.VoluntaryMode, userName
                            }, tran)
                        Next
                        tran.Commit()
                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        End Function

        ' --- SAVE (update-or-insert bawat row, linisin ang sobrang period, itugma ang voluntary mode) ---
        Public Async Function SaveAllAsync(items As List(Of StatutoryScheduleModel),
                                           periodCounts As Dictionary(Of String, Integer),
                                           voluntaryMode As Byte,
                                           userName As String) As Task _
            Implements IStatutoryScheduleRepository.SaveAllAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()
                    Try
                        For Each item In items
                            Dim mode As Byte? = Nothing
                            If String.Equals(item.Agency, StatutoryAgency.PagIbig, StringComparison.OrdinalIgnoreCase) Then
                                mode = voluntaryMode
                            End If

                            Dim updated = Await conn.ExecuteAsync(
                                $"UPDATE {TableName}
                                  SET SharePercent = @SharePercent,
                                      UpdatedAt = SYSDATETIME(),
                                      UpdatedBy = @UserName
                                  WHERE PayCycleType = @PayCycleType AND Agency = @Agency AND PeriodNo = @PeriodNo",
                                New With {item.SharePercent, userName, item.PayCycleType, item.Agency, item.PeriodNo}, tran)

                            If updated = 0 Then
                                Await conn.ExecuteAsync(InsertSql, New With {
                                    item.PayCycleType, item.Agency, item.PeriodNo, item.SharePercent, .VoluntaryMode = mode, userName
                                }, tran)
                            End If
                        Next

                        ' Kung lumiit ang bilang ng period ng pay cycle, tanggalin ang mga row na lampas na.
                        For Each kv In periodCounts
                            Await conn.ExecuteAsync(
                                $"DELETE FROM {TableName} WHERE PayCycleType = @PayCycleType AND PeriodNo > @MaxPeriod",
                                New With {.PayCycleType = kv.Key, .MaxPeriod = kv.Value}, tran)
                        Next

                        ' Iisa ang Pag-IBIG voluntary mode para sa lahat ng pay cycle (kasama ang hindi active).
                        Await conn.ExecuteAsync(
                            $"UPDATE {TableName}
                              SET VoluntaryMode = @VoluntaryMode
                              WHERE Agency = @Agency AND (VoluntaryMode IS NULL OR VoluntaryMode <> @VoluntaryMode)",
                            New With {.VoluntaryMode = voluntaryMode, .Agency = StatutoryAgency.PagIbig}, tran)

                        tran.Commit()
                    Catch
                        tran.Rollback()
                        Throw
                    End Try
                End Using
            End Using
        End Function

    End Class

End Namespace
