Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace IncomeTaxTable.Data
    Public Class IncomeTaxTableRepository
        Inherits BaseRepository(Of IncomeTaxBracketModel)
        Implements IIncomeTaxTableRepository

        ' --- READ ---
        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of IncomeTaxBracketModel)) _
            Implements IIncomeTaxTableRepository.GetAllAsync

            Dim info = IncomeTaxTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT {info.IdColumn} AS Id, SalaryFrom, SalaryTo,
                       TaxPercentage, FixTaxAmount, IsActive
                FROM {info.TableName}
                ORDER BY SalaryFrom"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- OVERLAP CHECK (used before Insert/Update) ---
        Public Async Function OverlapExistsAsync(tableName As String, salaryFrom As Decimal, salaryTo As Decimal, excludeId As Integer) As Task(Of Boolean) _
            Implements IIncomeTaxTableRepository.OverlapExistsAsync

            Dim info = IncomeTaxTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT COUNT(1)
                FROM {info.TableName}
                WHERE {info.IdColumn} <> @ExcludeId
                  AND SalaryFrom <= @SalaryTo
                  AND SalaryTo >= @SalaryFrom"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {salaryFrom, salaryTo, excludeId})
                Return count > 0
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of Integer) _
            Implements IIncomeTaxTableRepository.InsertAsync

            Dim info = IncomeTaxTableRegistry.GetInfo(tableName)

            Dim sql = $"
                INSERT INTO {info.TableName}
                    (SalaryFrom, SalaryTo, TaxPercentage, FixTaxAmount, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{info.IdColumn}
                VALUES
                    (@SalaryFrom, @SalaryTo, @TaxPercentage, @FixTaxAmount, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.SalaryFrom, item.SalaryTo, item.TaxPercentage, item.FixTaxAmount,
                    item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(tableName As String, item As IncomeTaxBracketModel, userName As String) As Task(Of Boolean) _
            Implements IIncomeTaxTableRepository.UpdateAsync

            Dim info = IncomeTaxTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {info.TableName}
                SET SalaryFrom = @SalaryFrom,
                    SalaryTo = @SalaryTo,
                    TaxPercentage = @TaxPercentage,
                    FixTaxAmount = @FixTaxAmount,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.SalaryFrom, item.SalaryTo, item.TaxPercentage, item.FixTaxAmount,
                item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IIncomeTaxTableRepository.SetActiveStatusAsync

            Dim info = IncomeTaxTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {info.TableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace