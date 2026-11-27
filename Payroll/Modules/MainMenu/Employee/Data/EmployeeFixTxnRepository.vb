' File: Modules/MainMenu/Employee/Data/EmployeeFixTxnRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace Employee.Data

    Public Class EmployeeFixTxnRepository
        Inherits GlobalShared.Base.BaseRepository(Of EmployeeFixTxnModel)
        Implements IEmployeeFixTxnRepository

        ' Result line na galing sa Fix Transaction: RefId = FixTxnId, Remark = 'Fix Transaction' (hindi LOAN).
        Private Const UsedSql As String =
            "EXISTS (SELECT 1 FROM tblPayrollResultLine rl
                     WHERE rl.RefId = f.FixTxnId AND rl.Category <> 'LOAN' AND rl.Remark = @fixRemark)"

        Private Const SelectSql As String =
            "SELECT f.FixTxnId, f.RecordId, f.TxnCategory, f.TxnCode, f.Amount,
                    CAST(f.StartYear AS INT) AS StartYear, CAST(f.StartMonth AS INT) AS StartMonth,
                    CAST(f.EndYear AS INT) AS EndYear, CAST(f.EndMonth AS INT) AS EndMonth,
                    f.Frequency, f.PayOn, f.IsActive, f.Remarks,
                    f.CreatedAt, f.CreatedBy, f.UpdatedAt, f.UpdatedBy,
                    COALESCE(c.Description, b.Description, d.Description, f.TxnCode) AS Description,
                    CAST(COALESCE(c.InputUnit, b.InputUnit, d.InputUnit, 3) AS TINYINT) AS InputUnit,
                    CAST(ISNULL(c.IsPayslipAllowance, 0) AS BIT) AS IsPayslipAllowance,
                    CAST(COALESCE(c.IsActive, b.IsActive, d.IsActive, 0) AS BIT) AS CodeIsActive,
                    CAST(CASE WHEN " & UsedSql & " THEN 1 ELSE 0 END AS BIT) AS IsUsed
             FROM tblEmployeeFixTxn f
                  LEFT JOIN tblCompensation c ON f.TxnCategory = 'COMPENSATION' AND c.Code = f.TxnCode
                  LEFT JOIN tblBonus b        ON f.TxnCategory = 'BONUS'        AND b.Code = f.TxnCode
                  LEFT JOIN tblDeduction d    ON f.TxnCategory = 'DEDUCTION'    AND d.Code = f.TxnCode"

        Public Async Function GetByEmployeeAsync(recordId As Integer) As Task(Of List(Of EmployeeFixTxnModel)) _
            Implements IEmployeeFixTxnRepository.GetByEmployeeAsync

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of EmployeeFixTxnModel)(
                    SelectSql & " WHERE f.RecordId = @recordId
                                  ORDER BY f.IsActive DESC, f.TxnCategory, f.TxnCode, f.StartYear, f.StartMonth",
                    New With {recordId, .fixRemark = FixTxnType.ResultRemark})
                Return rows.ToList()
            End Using
        End Function

        Public Async Function GetByIdAsync(fixTxnId As Integer) As Task(Of EmployeeFixTxnModel) _
            Implements IEmployeeFixTxnRepository.GetByIdAsync

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of EmployeeFixTxnModel)(
                    SelectSql & " WHERE f.FixTxnId = @fixTxnId",
                    New With {fixTxnId, .fixRemark = FixTxnType.ResultRemark})
            End Using
        End Function

        Public Async Function GetCodeOptionsAsync() As Task(Of List(Of FixTxnCodeOption)) _
            Implements IEmployeeFixTxnRepository.GetCodeOptionsAsync

            Const sql As String =
                "DECLARE @gs TABLE (BasicPlus INT, BasicMinus INT, Absent INT, LateIn INT, EarlyOut INT, LoanRefund INT);
                 INSERT INTO @gs
                 SELECT TOP 1 BasicSalaryCodePlusId, BasicSalaryCodeMinusId, AbsentCodeId, LateInCodeId, EarlyOutCodeId, LoanRefundCodeId
                 FROM tblGeneralSettings ORDER BY GeneralSettingsId;

                 SELECT 'COMPENSATION' AS TxnCategory, c.CompensationId AS CodeId, c.Code, c.Description,
                        CAST(c.InputUnit AS TINYINT) AS InputUnit, CAST(ISNULL(c.IsPayslipAllowance, 0) AS BIT) AS IsPayslipAllowance,
                        CAST(c.IsActive AS BIT) AS IsActive
                 FROM tblCompensation c
                 WHERE NOT EXISTS (SELECT 1 FROM @gs g WHERE c.CompensationId IN (ISNULL(g.BasicPlus, -1), ISNULL(g.LoanRefund, -1)))
                 UNION ALL
                 SELECT 'BONUS', b.BonusId, b.Code, b.Description,
                        CAST(b.InputUnit AS TINYINT), CAST(0 AS BIT), CAST(b.IsActive AS BIT)
                 FROM tblBonus b
                 UNION ALL
                 SELECT 'DEDUCTION', d.DeductionId, d.Code, d.Description,
                        CAST(d.InputUnit AS TINYINT), CAST(0 AS BIT), CAST(d.IsActive AS BIT)
                 FROM tblDeduction d
                 WHERE NOT EXISTS (SELECT 1 FROM @gs g
                                   WHERE d.DeductionId IN (ISNULL(g.BasicMinus, -1), ISNULL(g.Absent, -1),
                                                           ISNULL(g.LateIn, -1), ISNULL(g.EarlyOut, -1)))
                 ORDER BY 1, 3;"

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of FixTxnCodeOption)(sql)
                Return rows.ToList()
            End Using
        End Function

        Public Async Function InsertAsync(item As EmployeeFixTxnModel, userName As String) As Task(Of Integer) _
            Implements IEmployeeFixTxnRepository.InsertAsync

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(
                    "INSERT INTO tblEmployeeFixTxn
                        (RecordId, TxnCategory, TxnCode, Amount, StartYear, StartMonth, EndYear, EndMonth,
                         Frequency, PayOn, IsActive, Remarks, CreatedAt, CreatedBy)
                     OUTPUT INSERTED.FixTxnId
                     VALUES
                        (@RecordId, @TxnCategory, @TxnCode, @Amount, @StartYear, @StartMonth, @EndYear, @EndMonth,
                         @Frequency, @PayOn, @IsActive, @Remarks, GETDATE(), @userName)",
                    New With {item.RecordId, item.TxnCategory, item.TxnCode, item.Amount, item.StartYear, item.StartMonth,
                              item.EndYear, item.EndMonth, item.Frequency, item.PayOn, item.IsActive,
                              .Remarks = NullIfBlank(item.Remarks), userName})
            End Using
        End Function

        Public Async Function UpdateAsync(item As EmployeeFixTxnModel, userName As String) As Task(Of Boolean) _
            Implements IEmployeeFixTxnRepository.UpdateAsync

            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync(
                    "UPDATE tblEmployeeFixTxn
                     SET TxnCategory = @TxnCategory, TxnCode = @TxnCode, Amount = @Amount,
                         StartYear = @StartYear, StartMonth = @StartMonth, EndYear = @EndYear, EndMonth = @EndMonth,
                         Frequency = @Frequency, PayOn = @PayOn, Remarks = @Remarks,
                         UpdatedAt = GETDATE(), UpdatedBy = @userName
                     WHERE FixTxnId = @FixTxnId",
                    New With {item.FixTxnId, item.TxnCategory, item.TxnCode, item.Amount, item.StartYear, item.StartMonth,
                              item.EndYear, item.EndMonth, item.Frequency, item.PayOn,
                              .Remarks = NullIfBlank(item.Remarks), userName})
                Return n > 0
            End Using
        End Function

        Public Async Function SetActiveAsync(fixTxnId As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements IEmployeeFixTxnRepository.SetActiveAsync

            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync(
                    "UPDATE tblEmployeeFixTxn SET IsActive = @isActive, UpdatedAt = GETDATE(), UpdatedBy = @userName
                     WHERE FixTxnId = @fixTxnId",
                    New With {fixTxnId, isActive, userName})
                Return n > 0
            End Using
        End Function

        Public Async Function DeleteIfUnusedAsync(fixTxnId As Integer) As Task(Of Boolean) _
            Implements IEmployeeFixTxnRepository.DeleteIfUnusedAsync

            Using conn = GetConnection()
                Dim n = Await conn.ExecuteAsync(
                    "DELETE f FROM tblEmployeeFixTxn f
                     WHERE f.FixTxnId = @fixTxnId AND NOT " & UsedSql,
                    New With {fixTxnId, .fixRemark = FixTxnType.ResultRemark})
                Return n > 0
            End Using
        End Function

        Private Shared Function NullIfBlank(value As String) As String
            If String.IsNullOrWhiteSpace(value) Then Return Nothing
            Return value.Trim()
        End Function

    End Class

End Namespace
