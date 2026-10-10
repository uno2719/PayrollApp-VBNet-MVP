Imports System.Data
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollProcessing.Data

    Public Class PayrollInputRepository
        Inherits BaseRepository(Of PayrollInputColumnModel)
        Implements IPayrollInputRepository

        Private Const TxnSource As String = "PIE"   ' Payroll Input Entry

        ' Reused as-is from Payroll Settings — same catalogs, no duplicated SQL.
        ' Cutoff CRUD itself now lives in CutoffRepository (Payroll Settings' new
        ' Cutoff tab) — this class only READS the list, via the same repository.
        Private ReadOnly _cutoffRepo As CutoffRepository
        Private ReadOnly _rateEntryRepo As PayrollRateEntryRepository
        Private ReadOnly _compensationRepo As CompensationRepository
        Private ReadOnly _flaggedEntryRepo As PayrollFlaggedEntryRepository

        Public Sub New(cutoffRepo As CutoffRepository,
                       rateEntryRepo As PayrollRateEntryRepository,
                       compensationRepo As CompensationRepository,
                       flaggedEntryRepo As PayrollFlaggedEntryRepository)
            _cutoffRepo = cutoffRepo
            _rateEntryRepo = rateEntryRepo
            _compensationRepo = compensationRepo
            _flaggedEntryRepo = flaggedEntryRepo
        End Sub

        ''' <summary>
        ''' Mga "bukas" na cutoff lang (CutoffRules.IsOpenForInput): Draft o Processed na nagsimula na.
        ''' Hindi lumalabas ang Closed (hal. mga bago nagsimulang gamitin ang app), Posted, at mga susunod pang buwan.
        ''' </summary>
        Public Async Function GetCutoffsAsync() As Task(Of List(Of CutoffModel)) Implements IPayrollInputRepository.GetCutoffsAsync
            Dim all = Await _cutoffRepo.GetAllAsync()
            Return all.
                Where(Function(c) CutoffRules.IsOpenForInput(c, Date.Today)).
                OrderBy(Function(c) c.CycleType).ThenBy(Function(c) c.CutoffStart).
                ToList()
        End Function

        ''' <summary>
        ''' Hours/Minutes/Days = Quantity (Qty ang naka-save, ang halaga ay kukuhanin sa rate ng employee);
        ''' Amount = direktang peso (naka-save sa Rate, Qty = 1).
        ''' </summary>
        Private Shared Function ModeOf(unit As PayrollInputUnit) As PayrollTxnValueMode
            Return If(unit = PayrollInputUnit.Amount, PayrollTxnValueMode.Amount, PayrollTxnValueMode.Quantity)
        End Function

        Public Async Function RequiresDaysWorkedAsync(cutoffId As Integer) As Task(Of Boolean) _
            Implements IPayrollInputRepository.RequiresDaysWorkedAsync

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(
                    "SELECT COUNT(1)
                     FROM tblCutoff c
                     INNER JOIN tblPayCycle p ON p.PayCycleType = c.CycleType
                     WHERE c.CutoffID = @cutoffId AND p.RateBasis = @dailyRate",
                    New With {cutoffId, .dailyRate = CByte(PayRateBasis.DailyRate)})
                Return count > 0
            End Using
        End Function

        Public Async Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel)) Implements IPayrollInputRepository.GetColumnsAsync
            Dim columns As New List(Of PayrollInputColumnModel)

            ' No more hardcoded Core columns (Basic/RegularHours/Late/LWOP/NightDiff/SIL) —
            ' confirmed out of scope for Payroll Input Entry entirely. Those will come from
            ' Employee's own stored rate and a future Fixed Transaction/Timekeeping feature,
            ' not from manual grid entry here. Every column now comes from Payroll Settings.

            ' Ang unit (Hours/Minutes/Days/Amount) ng bawat column ay galing sa InputUnit ng catalog row
            ' sa Payroll Settings, at ito rin ang lumalabas sa caption, hal. "Overtime Regular (hrs)".

            ' Dynamic: one column per active Overtime/Holiday row. IsEssential now
            ' comes straight from the catalog row — set it via the "show by default
            ' in Payroll Input Entry" checkbox in Payroll Settings, not hardcoded here.
            Dim overtimeRows = Await _rateEntryRepo.GetAllAsync("tblOvertime")
            For Each r In overtimeRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit), .Category = PayrollInputCategory.Overtime,
                    .InputUnit = r.InputUnit, .ValueMode = ModeOf(r.InputUnit), .IsEssential = r.IsEssential
                })
            Next

            Dim holidayRows = Await _rateEntryRepo.GetAllAsync("tblHoliday")
            For Each r In holidayRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit), .Category = PayrollInputCategory.Holiday,
                    .InputUnit = r.InputUnit, .ValueMode = ModeOf(r.InputUnit), .IsEssential = r.IsEssential
                })
            Next

            ' Dynamic: one column per active Compensation row (recurring allowances, manual override per cutoff).
            Dim compensationRows = Await _compensationRepo.GetAllAsync()
            For Each r In compensationRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit), .Category = PayrollInputCategory.Compensation,
                    .InputUnit = r.InputUnit, .ValueMode = ModeOf(r.InputUnit), .IsEssential = r.IsEssential
                })
            Next

            ' Dynamic: one column per active Bonus row (the "Other Income" concept from the old C1Pay sheet).
            Dim bonusRows = Await _flaggedEntryRepo.GetAllAsync("tblBonus")
            For Each r In bonusRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit), .Category = PayrollInputCategory.Bonus,
                    .InputUnit = r.InputUnit, .ValueMode = ModeOf(r.InputUnit), .IsEssential = r.IsEssential
                })
            Next

            ' Dynamic: one column per active Deduction row — confirmed separate from
            ' SSS/PhilHealth/Pag-IBIG/Loan (those stay automatic; tblLoan has its own
            ' table). tblDeduction covers ad-hoc deductions (uniform, cash advance, etc.)
            ' that DO belong here as manual, per-cutoff input.
            Dim deductionRows = Await _flaggedEntryRepo.GetAllAsync("tblDeduction")
            For Each r In deductionRows.Where(Function(x) x.IsActive)
                columns.Add(New PayrollInputColumnModel With {
                    .ColumnName = r.Code, .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit), .Category = PayrollInputCategory.Deduction,
                    .InputUnit = r.InputUnit, .ValueMode = ModeOf(r.InputUnit), .IsEssential = r.IsEssential
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
                    New With {cutoffId, .TxnSrc = TxnSource})).ToLookup(Function(t) t.RecordId)

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
                            New With {cutoffId, recordId, .TxnSrc = TxnSource}, tx)

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