Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    ' Ginagamit ito ng DALAWANG tables (Overtime, Holiday).
    Public Class PayrollRateEntryRepository
        Inherits BaseRepository(Of PayrollRateEntryModel)
        Implements IPayrollRateEntryRepository

        ' --- READ ---
        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollRateEntryModel)) _
            Implements IPayrollRateEntryRepository.GetAllAsync

            Dim info = PayrollRateEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT {info.IdColumn} AS Id, Code, Description,
                       TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag,
                       Rate, MapCode,
                       IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {tableName}
                ORDER BY Code"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK (used before Insert/Update) ---
        Public Async Function CodeExistsAsync(tableName As String, code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements IPayrollRateEntryRepository.CodeExistsAsync

            Dim info = PayrollRateEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT COUNT(1)
                FROM {tableName}
                WHERE Code = @Code
                  AND {info.IdColumn} <> @ExcludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {code, excludeId})
                Return count > 0
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(tableName As String, item As PayrollRateEntryModel, userName As String) As Task(Of Integer) _
            Implements IPayrollRateEntryRepository.InsertAsync

            Dim info = PayrollRateEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                INSERT INTO {tableName}
                    (Code, Description, TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag, Rate, MapCode, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{info.IdColumn}
                VALUES
                    (@Code, @Description, @TaxFlag, @SSSFlag, @PhilHealthFlag, @PagIbigFlag, @Rate, @MapCode, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                    item.Rate, item.MapCode, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(tableName As String, item As PayrollRateEntryModel, userName As String) As Task(Of Boolean) _
            Implements IPayrollRateEntryRepository.UpdateAsync

            Dim info = PayrollRateEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET Code = @Code,
                    Description = @Description,
                    TaxFlag = @TaxFlag,
                    SSSFlag = @SSSFlag,
                    PhilHealthFlag = @PhilHealthFlag,
                    PagIbigFlag = @PagIbigFlag,
                    Rate = @Rate,
                    MapCode = @MapCode,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                item.Rate, item.MapCode, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IPayrollRateEntryRepository.SetActiveStatusAsync

            Dim info = PayrollRateEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace
