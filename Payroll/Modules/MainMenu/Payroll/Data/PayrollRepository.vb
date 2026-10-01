Imports System.Data
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollProcessing.Data

    Public Class PayrollInputRepository
        Inherits BaseRepository(Of CutoffModel)
        Implements IPayrollInputRepository

        Private Const TxnSource As String = "PIE"   ' Payroll Input Entry

        ' Reused as-is from Payroll Settings — same catalogs, no duplicated SQL.
        Private ReadOnly _rateEntryRepo As PayrollRateEntryRepository
        Private ReadOnly _compensationRepo As CompensationRepository
        Private ReadOnly _flaggedEntryRepo As PayrollFlaggedEntryRepository

        Public Sub New(rateEntryRepo As PayrollRateEntryRepository,
                       compensationRepo As CompensationRepository,
                       flaggedEntryRepo As PayrollFlaggedEntryRepository)
            _rateEntryRepo = rateEntryRepo
            _compensationRepo = compensationRepo
            _flaggedEntryRepo = flaggedEntryRepo
        End Sub

        Public Async Function GetCutoffsAsync() As Task(Of List(Of CutoffModel)) Implements IPayrollInputRepository.GetCutoffsAsync
            Return Await MyBase.GetAllAsync("SELECT * FROM tblCutoff ORDER BY CutoffStart DESC")
        End Function

        Public Async Function CreateCutoffAsync(cutoff As CutoffModel) As Task(Of Integer) Implements IPayrollInputRepository.CreateCutoffAsync
            Dim sql = "
                INSERT INTO tblCutoff (CycleType, CutoffYear, CutoffStart, CutoffEnd, PayDate, CutoffLabel, Status, CreatedBy)
                OUTPUT INSERTED.CutoffID
                VALUES (@CycleType, @CutoffYear, @CutoffStart, @CutoffEnd, @PayDate, @CutoffLabel, @Status, @CreatedBy)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, cutoff)
            End Using
        End Function

        Public Async Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel)) Implements IPayrollInputRepository.GetColumnsAsync
            Dim columns As New List(Of PayrollInputColumnModel)

            ' Fixed core columns — always present, always "essential".
            For Each c In CoreTxnCode.Columns
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = c.Code, .Caption = c.Caption, .Category = PayrollInputCategory.Core,
                    .ValueMode = c.Mode, .IsEssential = True
                })
            Next

            ' Dynamic: one column per active Overtime/Holiday row.
            Dim overtimeRows = Await _rateEntryRepo.GetAllAsync("tblOvertime")
            For Each r In overtimeRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description, .Category = PayrollInputCategory.Overtime,
                    .ValueMode = PayrollTxnValueMode.Quantity, .IsEssential = False
                })
            Next

            Dim holidayRows = Await _rateEntryRepo.GetAllAsync("tblHoliday")
            For Each r In holidayRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description, .Category = PayrollInputCategory.Holiday,
                    .ValueMode = PayrollTxnValueMode.Quantity, .IsEssential = False
                })
            Next

            ' Dynamic: one column per active Compensation row (recurring allowances, manual override per cutoff).
            Dim compensationRows = Await _compensationRepo.GetAllAsync()
            For Each r In compensationRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description, .Category = PayrollInputCategory.Compensation,
                    .ValueMode = PayrollTxnValueMode.Amount, .IsEssential = False
                })
            Next

            ' Dynamic: one column per active Bonus row (the "Other Income" concept from the old C1Pay sheet).
            Dim bonusRows = Await _flaggedEntryRepo.GetAllAsync("tblBonus")
            For Each r In bonusRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description, .Category = PayrollInputCategory.Bonus,
                    .ValueMode = PayrollTxnValueMode.Amount, .IsEssential = False
                })
            Next

            Return columns
        End Function

        Public Async Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of DataTable) _
            Implements IPayrollInputRepository.GetInputDataAsync

            Dim table As New DataTable()
            table.Columns.Add("RecordId", GetType(Integer))
            table.Columns.Add("EmployeeNo", GetType(String))
            table.Columns.Add("EmployeeName", GetType(String))
            For Each col In columns
                table.Columns.Add(col.ColumnName, GetType(Decimal))
            Next

            Using conn = GetConnection()
                ' Employees eligible for this Cutoff = PayCycle matches the Cutoff's CycleType.
                Dim employees = Await conn.QueryAsync(Of (RecordId As Integer, EmployeeNo As String, EmployeeName As String))(
                    "SELECT e.RecordId, e.EmployeeNo, (e.LastName + ', ' + e.FirstName) AS EmployeeName
                     FROM tblEmployee e
                     INNER JOIN tblEmployeeEarnings ee ON ee.RecordId = e.RecordId
                     INNER JOIN tblCutoff c ON c.CycleType = ee.PayCycle
                     WHERE c.CutoffID = @CutoffID AND e.IsActive = 1 AND e.IsDeleted = 0
                     ORDER BY e.LastName, e.FirstName",
                    New With {cutoffId})

                Dim txnRows = (Await conn.QueryAsync(Of (RecordId As Integer, TxnCode As String, Qty As Decimal, Rate As Decimal))(
                    "SELECT RecordId, TxnCode, Qty, Rate
                     FROM tblPayrollInputTxn
                     WHERE CutoffID = @CutoffID AND TxnSrc = @TxnSrc",
                    New With {
                                cutoffId,
                                .TxnSrc = TxnSource
                            })).ToLookup(Function(t) t.RecordId)

                Dim modeByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.ValueMode)

                For Each emp In employees
                    Dim row = table.NewRow()
                    row("RecordId") = emp.RecordId
                    row("EmployeeNo") = emp.EmployeeNo
                    row("EmployeeName") = emp.EmployeeName
                    For Each col In columns
                        row(col.ColumnName) = 0D
                    Next
                    For Each t In txnRows(emp.RecordId)
                        If modeByCode.ContainsKey(t.TxnCode) Then
                            row(t.TxnCode) = If(modeByCode(t.TxnCode) = PayrollTxnValueMode.Amount, t.Rate, t.Qty)
                        End If
                    Next
                    table.Rows.Add(row)
                Next
            End Using

            Return table
        End Function

        Public Async Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task _
            Implements IPayrollInputRepository.SaveInputDataAsync

            Dim categoryByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.Category.ToString().ToUpperInvariant())
            Dim modeByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.ValueMode)

            Using conn = GetConnection()
                Using tx = conn.BeginTransaction()
                    For Each row As DataRow In table.Rows
                        Dim recordId = CInt(row("RecordId"))

                        ' Delete-then-reinsert, scoped to this Cutoff + our own source tag.
                        Await conn.ExecuteAsync(
                            "DELETE FROM tblPayrollInputTxn WHERE CutoffID = @CutoffID AND RecordId = @RecordId AND TxnSrc = @TxnSrc",
                            New With {
                                        cutoffId,
                                        .TxnSrc = TxnSource
                                    }, tx)

                        For Each col In columns
                            Dim value = Convert.ToDecimal(row(col.ColumnName))
                            If value = 0D Then Continue For   ' skip zero entries

                            Dim isAmount = modeByCode(col.ColumnName) = PayrollTxnValueMode.Amount
                            Dim qty As Decimal = If(isAmount, 1D, value)
                            Dim rate As Decimal = If(isAmount, value, 1D)

                            Await conn.ExecuteAsync(
                                "INSERT INTO tblPayrollInputTxn (CutoffID, RecordId, TxnCategory, TxnCode, Qty, Rate, TxnSrc, ModifiedBy, ModifiedDate)
                                 VALUES (@CutoffID, @RecordId, @TxnCategory, @TxnCode, @Qty, @Rate, @TxnSrc, @ModifiedBy, SYSDATETIME())",
                                New With {
                                    cutoffId, recordId,
                                    .TxnCategory = categoryByCode(col.ColumnName),
                                    .TxnCode = col.ColumnName,
                                    qty, rate,
                                    .TxnSrc = TxnSource,
                                    modifiedBy
                                }, tx)
                        Next
                    Next
                    tx.Commit()
                End Using
            End Using
        End Function

    End Class

End Namespace