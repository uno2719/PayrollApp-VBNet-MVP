Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data

    Public Class PayCycleRepository
        Inherits BaseRepository(Of PayCycleModel)
        Implements IPayCycleRepository

        Private Const CycleTable As String = "tblPayCycle"
        Private Const PeriodTable As String = "tblPayCyclePeriod"

        Private Const CycleColumns As String =
            "PayCycleId, PayCycleType, RateBasis, IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy"

        Private Const PeriodColumns As String =
            "PayCyclePeriodId, PayCycleId, PeriodNo, FromDay, FromMonthOffset, ToDay, ToMonthOffset, PayDay"

        Public Async Function GetAllAsync() As Task(Of List(Of PayCycleModel)) _
            Implements IPayCycleRepository.GetAllAsync

            Using conn = GetConnection()
                Dim cycles = (Await conn.QueryAsync(Of PayCycleModel)(
                    $"SELECT {CycleColumns} FROM {CycleTable} ORDER BY PayCycleId")).ToList()

                Dim periods = (Await conn.QueryAsync(Of PayCyclePeriodModel)(
                    $"SELECT {PeriodColumns} FROM {PeriodTable} ORDER BY PayCycleId, PeriodNo")).ToList()

                For Each cycle In cycles
                    cycle.Periods = periods.Where(Function(p) p.PayCycleId = cycle.PayCycleId).ToList()
                Next

                Return cycles
            End Using
        End Function

        Public Async Function GetByTypeAsync(payCycleType As String) As Task(Of PayCycleModel) _
            Implements IPayCycleRepository.GetByTypeAsync

            Using conn = GetConnection()
                Dim cycle = Await conn.QuerySingleOrDefaultAsync(Of PayCycleModel)(
                    $"SELECT {CycleColumns} FROM {CycleTable} WHERE PayCycleType = @payCycleType",
                    New With {payCycleType})

                If cycle Is Nothing Then Return Nothing

                Dim periods = Await conn.QueryAsync(Of PayCyclePeriodModel)(
                    $"SELECT {PeriodColumns} FROM {PeriodTable} WHERE PayCycleId = @PayCycleId ORDER BY PeriodNo",
                    New With {cycle.PayCycleId})

                cycle.Periods = periods.ToList()
                Return cycle
            End Using
        End Function

        Public Async Function SaveAsync(item As PayCycleModel, userName As String) As Task(Of Boolean) _
            Implements IPayCycleRepository.SaveAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()

                    Try
                        Dim updateSql = $"
                            UPDATE {CycleTable}
                            SET RateBasis = @RateBasis,
                                IsActive = @IsActive,
                                UpdatedAt = SYSDATETIME(),
                                UpdatedBy = @UserName
                            WHERE PayCycleId = @PayCycleId"

                        Dim rows = Await conn.ExecuteAsync(updateSql, New With {
                            item.PayCycleId, .RateBasis = CByte(item.RateBasis), item.IsActive, userName
                        }, tran)

                        If rows = 0 Then
                            tran.Rollback()
                            Return False
                        End If

                        Await conn.ExecuteAsync(
                            $"DELETE FROM {PeriodTable} WHERE PayCycleId = @PayCycleId",
                            New With {item.PayCycleId}, tran)

                        If item.Periods IsNot Nothing AndAlso item.Periods.Count > 0 Then

                            Dim insertSql = $"
                                INSERT INTO {PeriodTable}
                                    (PayCycleId, PeriodNo, FromDay, FromMonthOffset, ToDay, ToMonthOffset, PayDay, CreatedAt, CreatedBy)
                                VALUES
                                    (@PayCycleId, @PeriodNo, @FromDay, @FromMonthOffset, @ToDay, @ToMonthOffset, @PayDay, SYSDATETIME(), @UserName)"

                            Dim payload = item.Periods.Select(Function(p) New With {
                                item.PayCycleId,
                                p.PeriodNo,
                                p.FromDay,
                                p.FromMonthOffset,
                                p.ToDay,
                                p.ToMonthOffset,
                                p.PayDay,
                                .UserName = userName
                            }).ToList()

                            Await conn.ExecuteAsync(insertSql, payload, tran)
                        End If

                        tran.Commit()
                        Return True

                    Catch
                        tran.Rollback()
                        Throw
                    End Try

                End Using
            End Using
        End Function

        Public Async Function CountEmployeesAsync(payCycleType As String) As Task(Of PayCycleEmployeeUsage) _
            Implements IPayCycleRepository.CountEmployeesAsync

            ' REPLACE: tugma rin ang lumang "Semi-Monthly" (may gitling) sa "SemiMonthly"
            Dim sql = "
                SELECT ISNULL(SUM(CASE WHEN REPLACE(REPLACE(ee.PayCycle, '-', ''), ' ', '') = @payCycleType THEN 1 ELSE 0 END), 0) AS AsPayCycle,
                       ISNULL(SUM(CASE WHEN REPLACE(REPLACE(ee.TaxFlag,  '-', ''), ' ', '') = @payCycleType THEN 1 ELSE 0 END), 0) AS AsTaxFlag
                FROM tblEmployeeEarnings ee
                INNER JOIN tblEmployee e ON e.RecordId = ee.RecordId
                WHERE e.IsActive = 1 AND e.IsDeleted = 0 AND ee.PayrollFlag = 1"

            Using conn = GetConnection()
                Return Await conn.QuerySingleAsync(Of PayCycleEmployeeUsage)(sql, New With {payCycleType})
            End Using
        End Function

        Public Async Function HasProcessedCutoffAsync(payCycleType As String) As Task(Of Boolean) _
            Implements IPayCycleRepository.HasProcessedCutoffAsync

            Dim sql = "
                SELECT COUNT(1) FROM tblCutoff
                WHERE CycleType = @payCycleType
                  AND Status IN (@processed, @posted)"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    payCycleType, .processed = CByte(CutoffStatus.Processed), .posted = CByte(CutoffStatus.Posted)
                })
                Return count > 0
            End Using
        End Function

    End Class

End Namespace
