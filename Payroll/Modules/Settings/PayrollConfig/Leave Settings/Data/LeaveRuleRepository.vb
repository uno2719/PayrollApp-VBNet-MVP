' ============================================================
' Modules/Settings/PayrollConfig/Leave Settings/Data/LeaveRuleRepository.vb
' ============================================================
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Data
    Public Class LeaveRuleRepository
        Inherits BaseRepository(Of LeaveRuleModel)
        Implements ILeaveRuleRepository

        Private Const RuleTable As String = "tblLeaveRule"
        Private Const BracketTable As String = "tblLeaveRuleBracket"

        ' ========================================================
        ' LIST - JOIN papunta sa Group/Type para lang sa display
        ' (LeaveGroupName, LeaveTypeName, LeaveCategory). Ang mismong
        ' bracket rows ay hindi kasama dito - hiwalay na tawag
        ' (GetBracketsAsync) pag na-select na ang isang rule.
        ' ========================================================
        Public Async Function GetAllAsync() As Task(Of List(Of LeaveRuleModel)) _
            Implements ILeaveRuleRepository.GetAllAsync

            Dim sql = $"
                SELECT  r.LeaveRuleId AS Id, r.LeaveGroupId, r.LeaveTypeId,
                        r.EntitlementMethod, r.ComputeBasedOn, r.PlotBasedOn, r.AnniversaryPlotOn,
                        r.UnitOfMeasure, r.HolidayIncluded, r.RequireAttachment, r.Monetize,
                        r.ShowEntitlement, r.IsActive,
                        r.CreatedAt, r.CreatedBy, r.UpdatedAt, r.UpdatedBy,
                        g.LeaveGroupCode, g.LeaveGroupName,
                        t.LeaveTypeCode, t.LeaveTypeName, t.LeaveCategory
                FROM    {RuleTable} r
                INNER JOIN tblLeaveGroup g ON g.LeaveGroupId = r.LeaveGroupId
                INNER JOIN tblLeaveType t ON t.LeaveTypeId = r.LeaveTypeId
                ORDER BY g.LeaveGroupName, t.LeaveTypeName"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of LeaveRuleModel)(sql)
                Return rows.ToList()
            End Using
        End Function

        ' ========================================================
        ' DUPLICATE COMBINATION CHECK (Group + Type)
        ' ========================================================
        Public Async Function CombinationExistsAsync(leaveGroupId As Integer, leaveTypeId As Integer, excludeId As Integer) As Task(Of Boolean) _
            Implements ILeaveRuleRepository.CombinationExistsAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM {RuleTable}
                WHERE LeaveGroupId = @leaveGroupId
                  AND LeaveTypeId = @leaveTypeId
                  AND LeaveRuleId <> @excludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {leaveGroupId, leaveTypeId, excludeId})
                Return count > 0
            End Using
        End Function

        ' ========================================================
        ' INSERT (header)
        ' ========================================================
        Public Async Function InsertAsync(item As LeaveRuleModel, userName As String) As Task(Of Integer) _
            Implements ILeaveRuleRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {RuleTable}
                    (LeaveGroupId, LeaveTypeId, EntitlementMethod, ComputeBasedOn, PlotBasedOn,
                     AnniversaryPlotOn, UnitOfMeasure, HolidayIncluded, RequireAttachment,
                     Monetize, ShowEntitlement, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.LeaveRuleId
                VALUES
                    (@LeaveGroupId, @LeaveTypeId, @EntitlementMethod, @ComputeBasedOn, @PlotBasedOn,
                     @AnniversaryPlotOn, @UnitOfMeasure, @HolidayIncluded, @RequireAttachment,
                     @Monetize, @ShowEntitlement, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.LeaveGroupId, item.LeaveTypeId, item.EntitlementMethod, item.ComputeBasedOn,
                    item.PlotBasedOn, item.AnniversaryPlotOn, item.UnitOfMeasure, item.HolidayIncluded,
                    item.RequireAttachment, item.Monetize, item.ShowEntitlement, item.IsActive, userName
                })
            End Using
        End Function

        ' ========================================================
        ' UPDATE (header)
        ' ========================================================
        Public Async Function UpdateAsync(item As LeaveRuleModel, userName As String) As Task(Of Boolean) _
            Implements ILeaveRuleRepository.UpdateAsync

            Dim sql = $"
                UPDATE {RuleTable}
                SET LeaveGroupId = @LeaveGroupId,
                    LeaveTypeId = @LeaveTypeId,
                    EntitlementMethod = @EntitlementMethod,
                    ComputeBasedOn = @ComputeBasedOn,
                    PlotBasedOn = @PlotBasedOn,
                    AnniversaryPlotOn = @AnniversaryPlotOn,
                    UnitOfMeasure = @UnitOfMeasure,
                    HolidayIncluded = @HolidayIncluded,
                    RequireAttachment = @RequireAttachment,
                    Monetize = @Monetize,
                    ShowEntitlement = @ShowEntitlement,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE LeaveRuleId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.LeaveGroupId, item.LeaveTypeId, item.EntitlementMethod, item.ComputeBasedOn,
                item.PlotBasedOn, item.AnniversaryPlotOn, item.UnitOfMeasure, item.HolidayIncluded,
                item.RequireAttachment, item.Monetize, item.ShowEntitlement, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' ========================================================
        ' SOFT DELETE / REACTIVATE
        ' ========================================================
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILeaveRuleRepository.SetActiveStatusAsync

            Dim sql = $"
                UPDATE {RuleTable}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE LeaveRuleId = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

        ' ========================================================
        ' BRACKETS - READ
        ' ========================================================
        Public Async Function GetBracketsAsync(leaveRuleId As Integer) As Task(Of List(Of LeaveRuleBracketModel)) _
            Implements ILeaveRuleRepository.GetBracketsAsync

            Dim sql = $"
                SELECT  LeaveRuleBracketId AS Id, LeaveRuleId,
                        YosFrom, YosTo, Entitlement, AnniversaryCredit,
                        Prorate, ForfeitOnMonth, BF, BFMax
                FROM    {BracketTable}
                WHERE   LeaveRuleId = @leaveRuleId
                ORDER BY YosFrom"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of LeaveRuleBracketModel)(sql, New With {leaveRuleId})
                Return rows.ToList()
            End Using
        End Function

        ' ========================================================
        ' BRACKETS - REPLACE (transactional)
        ' ========================================================
        ' Parehong dahilan at parehong ayos ng
        ' EmployeeLoanRepository.ReplaceScheduleAsync: delete-then-
        ' insert sa loob ng isang transaction, "lahat o wala", at mas
        ' simple kaysa sa row-by-row diffing kapag binago ng user ang
        ' bilang ng brackets (nagdagdag/nag-alis ng row sa grid).
        ' ========================================================
        Public Async Function ReplaceBracketsAsync(leaveRuleId As Integer,
                                                     brackets As List(Of LeaveRuleBracketModel),
                                                     userName As String) As Task(Of Boolean) _
            Implements ILeaveRuleRepository.ReplaceBracketsAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()

                    Try
                        Await conn.ExecuteAsync(
                            $"DELETE FROM {BracketTable} WHERE LeaveRuleId = @leaveRuleId",
                            New With {leaveRuleId}, tran)

                        If brackets IsNot Nothing AndAlso brackets.Count > 0 Then

                            Dim insertSql = $"
                                INSERT INTO {BracketTable}
                                    (LeaveRuleId, YosFrom, YosTo, Entitlement, AnniversaryCredit,
                                     Prorate, ForfeitOnMonth, BF, BFMax, CreatedAt, CreatedBy)
                                VALUES
                                    (@LeaveRuleId, @YosFrom, @YosTo, @Entitlement, @AnniversaryCredit,
                                     @Prorate, @ForfeitOnMonth, @BF, @BFMax, GETDATE(), @UserName)"

                            Dim payload = brackets.Select(Function(b) New With {
                                .LeaveRuleId = leaveRuleId,
                                b.YosFrom,
                                b.YosTo,
                                b.Entitlement,
                                b.AnniversaryCredit,
                                b.Prorate,
                                b.ForfeitOnMonth,
                                b.BF,
                                b.BFMax,
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

        ' ========================================================
        ' SAVE (HEADER + BRACKETS) - IIISANG TRANSACTION
        ' ========================================================
        Public Async Function SaveWithBracketsAsync(item As LeaveRuleModel,
                                                      brackets As List(Of LeaveRuleBracketModel),
                                                      userName As String) As Task(Of Integer) _
            Implements ILeaveRuleRepository.SaveWithBracketsAsync

            Using conn = GetConnection()
                Using tran = conn.BeginTransaction()

                    Try
                        Dim leaveRuleId As Integer

                        If item.Id = 0 Then
                            ' --- INSERT header ---
                            Dim insertSql = $"
                                INSERT INTO {RuleTable}
                                    (LeaveGroupId, LeaveTypeId, EntitlementMethod, ComputeBasedOn, PlotBasedOn,
                                     AnniversaryPlotOn, UnitOfMeasure, HolidayIncluded, RequireAttachment,
                                     Monetize, ShowEntitlement, IsActive, CreatedAt, CreatedBy)
                                OUTPUT INSERTED.LeaveRuleId
                                VALUES
                                    (@LeaveGroupId, @LeaveTypeId, @EntitlementMethod, @ComputeBasedOn, @PlotBasedOn,
                                     @AnniversaryPlotOn, @UnitOfMeasure, @HolidayIncluded, @RequireAttachment,
                                     @Monetize, @ShowEntitlement, @IsActive, GETDATE(), @UserName)"

                            leaveRuleId = Await conn.ExecuteScalarAsync(Of Integer)(insertSql, New With {
                                item.LeaveGroupId, item.LeaveTypeId, item.EntitlementMethod, item.ComputeBasedOn,
                                item.PlotBasedOn, item.AnniversaryPlotOn, item.UnitOfMeasure, item.HolidayIncluded,
                                item.RequireAttachment, item.Monetize, item.ShowEntitlement, item.IsActive, userName
                            }, tran)
                        Else
                            ' --- UPDATE header ---
                            leaveRuleId = item.Id

                            Dim updateSql = $"
                                UPDATE {RuleTable}
                                SET LeaveGroupId = @LeaveGroupId,
                                    LeaveTypeId = @LeaveTypeId,
                                    EntitlementMethod = @EntitlementMethod,
                                    ComputeBasedOn = @ComputeBasedOn,
                                    PlotBasedOn = @PlotBasedOn,
                                    AnniversaryPlotOn = @AnniversaryPlotOn,
                                    UnitOfMeasure = @UnitOfMeasure,
                                    HolidayIncluded = @HolidayIncluded,
                                    RequireAttachment = @RequireAttachment,
                                    Monetize = @Monetize,
                                    ShowEntitlement = @ShowEntitlement,
                                    IsActive = @IsActive,
                                    UpdatedAt = GETDATE(),
                                    UpdatedBy = @UserName
                                WHERE LeaveRuleId = @Id"

                            Await conn.ExecuteAsync(updateSql, New With {
                                item.Id, item.LeaveGroupId, item.LeaveTypeId, item.EntitlementMethod, item.ComputeBasedOn,
                                item.PlotBasedOn, item.AnniversaryPlotOn, item.UnitOfMeasure, item.HolidayIncluded,
                                item.RequireAttachment, item.Monetize, item.ShowEntitlement, item.IsActive, userName
                            }, tran)
                        End If

                        ' --- REPLACE brackets (parehong connection/transaction) ---
                        Await conn.ExecuteAsync(
                            $"DELETE FROM {BracketTable} WHERE LeaveRuleId = @leaveRuleId",
                            New With {leaveRuleId}, tran)

                        If brackets IsNot Nothing AndAlso brackets.Count > 0 Then

                            Dim insertBracketSql = $"
                                INSERT INTO {BracketTable}
                                    (LeaveRuleId, YosFrom, YosTo, Entitlement, AnniversaryCredit,
                                     Prorate, ForfeitOnMonth, BF, BFMax, CreatedAt, CreatedBy)
                                VALUES
                                    (@LeaveRuleId, @YosFrom, @YosTo, @Entitlement, @AnniversaryCredit,
                                     @Prorate, @ForfeitOnMonth, @BF, @BFMax, GETDATE(), @UserName)"

                            Dim payload = brackets.Select(Function(b) New With {
                                .LeaveRuleId = leaveRuleId,
                                b.YosFrom,
                                b.YosTo,
                                b.Entitlement,
                                b.AnniversaryCredit,
                                b.Prorate,
                                b.ForfeitOnMonth,
                                b.BF,
                                b.BFMax,
                                .UserName = userName
                            }).ToList()

                            Await conn.ExecuteAsync(insertBracketSql, payload, tran)

                        End If

                        tran.Commit()
                        Return leaveRuleId

                    Catch
                        tran.Rollback()
                        Throw
                    End Try

                End Using
            End Using

        End Function

    End Class
End Namespace
