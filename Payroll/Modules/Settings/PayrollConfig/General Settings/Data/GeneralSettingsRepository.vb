' File: Modules/Settings/SysConfig/GeneralSettings/Data/GeneralSettingsRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace GeneralSettings.Data

    ''' <summary>
    ''' Isang table lang (tblGeneralSettings) kaya walang TableRegistry
    ''' indirection dito - direktang Const, gaya ng CompensationRepository.
    ''' Walang CodeExists/SetActiveStatus dahil hindi ito CRUD list:
    ''' iisang settings row lang habambuhay.
    ''' </summary>
    Public Class GeneralSettingsRepository
        Inherits BaseRepository(Of GeneralSettingsModel)
        Implements IGeneralSettingsRepository

        Private Const TableName As String = "tblGeneralSettings"

        ' --- READ ---
        ' Isang row lang palagi - TOP 1, walang WHERE. Babalik ng Nothing
        ' kung bagong install pa (wala pang na-Insert); ang Service ang
        ' bahalang magdesisyon kung Insert o Update sa Save.
        Public Async Function GetAsync() As Task(Of GeneralSettingsModel) _
            Implements IGeneralSettingsRepository.GetAsync

            Dim sql = $"
                SELECT TOP 1
                       GeneralSettingsId,
                       BonusCeiling, TotalDaysPerYear, WorkHourPerDay,
                       AmountPrecision, PercentPrecision,
                       BasicSalaryCodePlusId, BasicSalaryCodeMinusId,
                       AbsentCodeId, LateInCodeId, EarlyOutCodeId,
                       SSSBasedOn, PhilHealthBasedOn,
                       CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {TableName}
                ORDER BY GeneralSettingsId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of GeneralSettingsModel)(sql)
            End Using
        End Function

        ' --- INSERT (unang Save lang, isang beses habambuhay) ---
        Public Async Function InsertAsync(item As GeneralSettingsModel, userName As String) As Task(Of Integer) _
            Implements IGeneralSettingsRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName}
                    (BonusCeiling, TotalDaysPerYear, WorkHourPerDay,
                     AmountPrecision, PercentPrecision,
                     BasicSalaryCodePlusId, BasicSalaryCodeMinusId,
                     AbsentCodeId, LateInCodeId, EarlyOutCodeId,
                     SSSBasedOn, PhilHealthBasedOn,
                     CreatedAt, CreatedBy)
                OUTPUT INSERTED.GeneralSettingsId
                VALUES
                    (@BonusCeiling, @TotalDaysPerYear, @WorkHourPerDay,
                     @AmountPrecision, @PercentPrecision,
                     @BasicSalaryCodePlusId, @BasicSalaryCodeMinusId,
                     @AbsentCodeId, @LateInCodeId, @EarlyOutCodeId,
                     @SSSBasedOn, @PhilHealthBasedOn,
                     GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.BonusCeiling, item.TotalDaysPerYear, item.WorkHourPerDay,
                    item.AmountPrecision, item.PercentPrecision,
                    item.BasicSalaryCodePlusId, item.BasicSalaryCodeMinusId,
                    item.AbsentCodeId, item.LateInCodeId, item.EarlyOutCodeId,
                    item.SSSBasedOn, item.PhilHealthBasedOn,
                    userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As GeneralSettingsModel, userName As String) As Task(Of Boolean) _
            Implements IGeneralSettingsRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET BonusCeiling           = @BonusCeiling,
                    TotalDaysPerYear       = @TotalDaysPerYear,
                    WorkHourPerDay         = @WorkHourPerDay,
                    AmountPrecision        = @AmountPrecision,
                    PercentPrecision       = @PercentPrecision,
                    BasicSalaryCodePlusId  = @BasicSalaryCodePlusId,
                    BasicSalaryCodeMinusId = @BasicSalaryCodeMinusId,
                    AbsentCodeId           = @AbsentCodeId,
                    LateInCodeId           = @LateInCodeId,
                    EarlyOutCodeId         = @EarlyOutCodeId,
                    SSSBasedOn             = @SSSBasedOn,
                    PhilHealthBasedOn      = @PhilHealthBasedOn,
                    UpdatedAt              = GETDATE(),
                    UpdatedBy              = @UserName
                WHERE GeneralSettingsId = @GeneralSettingsId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.GeneralSettingsId,
                item.BonusCeiling, item.TotalDaysPerYear, item.WorkHourPerDay,
                item.AmountPrecision, item.PercentPrecision,
                item.BasicSalaryCodePlusId, item.BasicSalaryCodeMinusId,
                item.AbsentCodeId, item.LateInCodeId, item.EarlyOutCodeId,
                item.SSSBasedOn, item.PhilHealthBasedOn,
                userName
            })
            Return rows > 0
        End Function

    End Class
End Namespace