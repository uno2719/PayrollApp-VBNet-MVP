Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    ' Ginagamit ito ng DALAWANG tables (Deduction, Bonus) - tableName mismo
    ' ang ipinapasa sa bawat call, gaya ng LookupRepository.
    Public Class PayrollFlaggedEntryRepository
        Inherits BaseRepository(Of PayrollFlaggedEntryModel)
        Implements IPayrollFlaggedEntryRepository

        ' --- READ ---
        Public Async Function GetAllAsync(tableName As String) As Task(Of List(Of PayrollFlaggedEntryModel)) _
            Implements IPayrollFlaggedEntryRepository.GetAllAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                SELECT {info.IdColumn} AS Id, Code, Description,
                       TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag,
                       IsActive, IsEssential, InputUnit, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {tableName}
                ORDER BY Code"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK (used before Insert/Update) ---
        Public Async Function CodeExistsAsync(tableName As String, code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements IPayrollFlaggedEntryRepository.CodeExistsAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)

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
        Public Async Function InsertAsync(tableName As String, item As PayrollFlaggedEntryModel, userName As String) As Task(Of Integer) _
            Implements IPayrollFlaggedEntryRepository.InsertAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                INSERT INTO {tableName}
                    (Code, Description, TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag, IsActive, IsEssential, InputUnit, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{info.IdColumn}
                VALUES
                    (@Code, @Description, @TaxFlag, @SSSFlag, @PhilHealthFlag, @PagIbigFlag, @IsActive, @IsEssential, @InputUnit, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                    item.IsActive, item.IsEssential, .InputUnit = CByte(item.InputUnit), userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(tableName As String, item As PayrollFlaggedEntryModel, userName As String) As Task(Of Boolean) _
            Implements IPayrollFlaggedEntryRepository.UpdateAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET Code = @Code,
                    Description = @Description,
                    TaxFlag = @TaxFlag,
                    SSSFlag = @SSSFlag,
                    PhilHealthFlag = @PhilHealthFlag,
                    PagIbigFlag = @PagIbigFlag,
                    IsActive = @IsActive,
                    IsEssential = @IsEssential,
                    InputUnit = @InputUnit,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                item.IsActive, item.IsEssential, .InputUnit = CByte(item.InputUnit), userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(tableName As String, id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IPayrollFlaggedEntryRepository.SetActiveStatusAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)

            Dim sql = $"
                UPDATE {tableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {info.IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function


        ' --- HARD DELETE (admin lang; may in-use check sa loob ng iisang transaction) ---
        Public Async Function DeleteAsync(tableName As String, id As Integer) As Task(Of DeleteResult) _
            Implements IPayrollFlaggedEntryRepository.DeleteAsync

            Dim info = PayrollFlaggedEntryTableRegistry.GetInfo(tableName)
            Dim category = PayrollInputUnits.CategoryForTable(info.TableName)

            ' Walang FK ang TxnCode/Code (string lang), kaya manual ang check dito.
            Dim refs As New List(Of UsageRef) From {
                New UsageRef("payroll input entry(ies)", "tblPayrollInputTxn", $"TxnCategory = '{category}' AND TxnCode = @Code"),
                New UsageRef("payroll result line(s)", "tblPayrollResultLine", $"Category = '{category}' AND Code = @Code")
            }

            ' Ang Deduction ay tinutukoy rin ng Code Mapping ng General Settings (may FK).
            If String.Equals(info.TableName, "tblDeduction", StringComparison.OrdinalIgnoreCase) Then
                refs.Add(New UsageRef("General Settings code mapping(s)", "tblGeneralSettings",
                    "BasicSalaryCodeMinusId = @Id OR AbsentCodeId = @Id OR LateInCodeId = @Id OR EarlyOutCodeId = @Id"))
            End If

            Return Await RecordDeleter.DeleteAsync(
                "this entry",
                $"DELETE FROM {info.TableName} WHERE {info.IdColumn} = @Id",
                id,
                $"SELECT Code FROM {info.TableName} WHERE {info.IdColumn} = @Id",
                refs)
        End Function

    End Class
End Namespace
