' File: Modules/Settings/PayrollConfig/Payroll Settings/Data/MinimumWageRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data

    Public Interface IMinimumWageRepository
        Function GetRatesAsync() As Task(Of List(Of MinimumWageModel))
        Function InsertRateAsync(item As MinimumWageModel, userName As String) As Task(Of Integer)
        Function UpdateRateAsync(item As MinimumWageModel, userName As String) As Task(Of Boolean)
        Function DeleteRateAsync(minimumWageId As Integer) As Task(Of Boolean)

        ''' <summary>Lahat ng branch/store at ang Wage Area nito.</summary>
        Function GetBranchesAsync() As Task(Of List(Of BranchWageAreaModel))
        Function SetBranchWageAreaAsync(branchId As Integer, wageArea As String, userName As String) As Task(Of Boolean)
    End Interface

    ''' <summary>Settings > Payroll > Minimum Wage: tblMinimumWage at tblBranch.WageArea.</summary>
    Public Class MinimumWageRepository
        Inherits BaseRepository(Of MinimumWageModel)
        Implements IMinimumWageRepository

        Public Async Function GetRatesAsync() As Task(Of List(Of MinimumWageModel)) Implements IMinimumWageRepository.GetRatesAsync
            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of MinimumWageModel)(
                    "SELECT MinimumWageId, WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks,
                            CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                     FROM tblMinimumWage
                     ORDER BY WageArea, EffectiveDate DESC")
                Return rows.ToList()
            End Using
        End Function

        Public Async Function InsertRateAsync(item As MinimumWageModel, userName As String) As Task(Of Integer) _
            Implements IMinimumWageRepository.InsertRateAsync
            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(
                    "INSERT INTO tblMinimumWage (WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks, CreatedAt, CreatedBy)
                     OUTPUT INSERTED.MinimumWageId
                     VALUES (@WageArea, @EffectiveDate, @DailyRate, @WageOrderNo, @Remarks, GETDATE(), @userName)",
                    New With {item.WageArea, .EffectiveDate = item.EffectiveDate.Date, item.DailyRate,
                              item.WageOrderNo, item.Remarks, userName})
            End Using
        End Function

        Public Async Function UpdateRateAsync(item As MinimumWageModel, userName As String) As Task(Of Boolean) _
            Implements IMinimumWageRepository.UpdateRateAsync
            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync(
                    "UPDATE tblMinimumWage
                     SET WageArea = @WageArea, EffectiveDate = @EffectiveDate, DailyRate = @DailyRate,
                         WageOrderNo = @WageOrderNo, Remarks = @Remarks, UpdatedAt = GETDATE(), UpdatedBy = @userName
                     WHERE MinimumWageId = @MinimumWageId",
                    New With {item.MinimumWageId, item.WageArea, .EffectiveDate = item.EffectiveDate.Date, item.DailyRate,
                              item.WageOrderNo, item.Remarks, userName})
                Return n > 0
            End Using
        End Function

        Public Async Function DeleteRateAsync(minimumWageId As Integer) As Task(Of Boolean) _
            Implements IMinimumWageRepository.DeleteRateAsync
            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync("DELETE FROM tblMinimumWage WHERE MinimumWageId = @minimumWageId", New With {minimumWageId})
                Return n > 0
            End Using
        End Function

        Public Async Function GetBranchesAsync() As Task(Of List(Of BranchWageAreaModel)) _
            Implements IMinimumWageRepository.GetBranchesAsync
            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of BranchWageAreaModel)(
                    "SELECT BranchId, BranchCode, BranchName, WageArea FROM tblBranch ORDER BY BranchName")
                Return rows.ToList()
            End Using
        End Function

        Public Async Function SetBranchWageAreaAsync(branchId As Integer, wageArea As String, userName As String) As Task(Of Boolean) _
            Implements IMinimumWageRepository.SetBranchWageAreaAsync
            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync(
                    "UPDATE tblBranch SET WageArea = @wageArea WHERE BranchId = @branchId",
                    New With {branchId, .wageArea = If(String.IsNullOrWhiteSpace(wageArea), Nothing, wageArea.Trim())})
                Return n > 0
            End Using
        End Function

    End Class

End Namespace
