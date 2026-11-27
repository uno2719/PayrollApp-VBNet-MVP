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

        ''' <summary>
        ''' Ang Compensation code na naka-map bilang "Basic Salary Code (+)" ay ang Days Worked ng Daily Rate cutoff
        ''' (Qty = araw, pwedeng may decimal). Laging nakikita (IsEssential) at una sa grid.
        ''' </summary>
        Public Async Function GetBasicDaysColumnAsync() As Task(Of PayrollInputColumnModel) _
            Implements IPayrollInputRepository.GetBasicDaysColumnAsync

            Dim basicCodeId As Integer? = Nothing
            Using conn = GetConnection()
                basicCodeId = Await conn.ExecuteScalarAsync(Of Integer?)(
                    "SELECT TOP 1 BasicSalaryCodePlusId FROM tblGeneralSettings ORDER BY 1")
            End Using
            If Not basicCodeId.HasValue Then Return Nothing

            Dim rows = Await _compensationRepo.GetAllAsync()
            Dim r = rows.FirstOrDefault(Function(x) x.Id = basicCodeId.Value)
            If r Is Nothing Then Return Nothing

            Return New PayrollInputColumnModel With {
                .ColumnName = r.Code,
                .Caption = r.Description & PayrollInputUnits.CaptionSuffix(r.InputUnit),
                .Category = PayrollInputCategory.Compensation,
                .InputUnit = r.InputUnit,
                .ValueMode = ModeOf(r.InputUnit),
                .IsEssential = True,
                .IsBasicDays = True
            }
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
            ' Ang code na naka-map bilang "Basic Salary Code (+)" sa General Settings ay AWTOMATIKO na ang basic pay
            ' (Basic / bilang ng cutoff). Hindi ito kasama sa listahang ito para walang doble; para sa Daily Rate
            ' cutoff ay hiwalay itong idinadagdag bilang Days Worked column (GetBasicDaysColumnAsync).
            Dim basicCodeId As Integer? = Nothing
            Using conn = GetConnection()
                basicCodeId = Await conn.ExecuteScalarAsync(Of Integer?)(
                    "SELECT TOP 1 BasicSalaryCodePlusId FROM tblGeneralSettings ORDER BY 1")
            End Using

            Dim compensationRows = Await _compensationRepo.GetAllAsync()
            For Each r In compensationRows.Where(Function(x) x.IsActive AndAlso (Not basicCodeId.HasValue OrElse x.Id <> basicCodeId.Value))
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

        ' Lahat ng hindi-deleted na empleyado kasama ang Earnings at Employment (LEFT JOIN, para makita rin ang
        ' mga kulang ang setup). Ang pagpapasya kung sino ang kasama ay nasa PayrollEligibilityRules (pure logic).
        Friend Const EligibilitySql As String =
            "SELECT e.RecordId, e.EmployeeNo,
                    (e.LastName + ', ' + e.FirstName) AS EmployeeName,
                    CAST(ISNULL(e.IsActive, 0) AS BIT) AS IsActive,
                    CAST(CASE WHEN ee.RecordId IS NULL THEN 0 ELSE 1 END AS BIT) AS HasEarnings,
                    ee.PayCycle,
                    CAST(ISNULL(ee.PayrollFlag, 0) AS BIT) AS PayrollFlag,
                    ee.BasicSalary, ee.DailyRate,
                    em.DateJoined, em.DateResigned, em.DateRetired, em.DateTerminated, em.DateEndOfContract
             FROM tblEmployee e
                  LEFT JOIN tblEmployeeEarnings ee ON ee.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeEmployment em ON em.RecordId = e.RecordId
             WHERE e.IsDeleted = 0
             ORDER BY e.LastName, e.FirstName"

        ' Employee > Fix Transactions: lahat ng ACTIVE na entry ng mga empleyado, kasama ang Input Unit ng code
        ' (para malaman kung peso o dami ang Amount). Ginagamit din ng PayrollProcessRepository.
        Friend Const FixTxnSql As String =
            "SELECT f.FixTxnId, f.RecordId, f.TxnCategory, f.TxnCode, f.Amount,
                    CAST(f.StartYear AS INT) AS StartYear, CAST(f.StartMonth AS INT) AS StartMonth,
                    CAST(f.EndYear AS INT) AS EndYear, CAST(f.EndMonth AS INT) AS EndMonth,
                    f.Frequency, f.PayOn, f.IsActive,
                    CAST(COALESCE(c.InputUnit, b.InputUnit, d.InputUnit, 3) AS TINYINT) AS InputUnit
             FROM tblEmployeeFixTxn f
                  LEFT JOIN tblCompensation c ON f.TxnCategory = 'COMPENSATION' AND c.Code = f.TxnCode
                  LEFT JOIN tblBonus b        ON f.TxnCategory = 'BONUS'        AND b.Code = f.TxnCode
                  LEFT JOIN tblDeduction d    ON f.TxnCategory = 'DEDUCTION'    AND d.Code = f.TxnCode
             WHERE f.IsActive = 1 AND f.RecordId IN @ids"

        ''' <summary>Bilang ng cycle sa isang buwan ng pay cycle ng cutoff (SemiMonthly = 2). 1 kung hindi mahanap.</summary>
        Private Shared Async Function PeriodsPerMonthAsync(conn As IDbConnection, cutoff As CutoffModel) As Task(Of Integer)
            Dim count = Await conn.ExecuteScalarAsync(Of Integer)(
                "SELECT COUNT(1)
                 FROM tblPayCyclePeriod p
                      INNER JOIN tblPayCycle c ON c.PayCycleId = p.PayCycleId
                 WHERE c.PayCycleId = (SELECT TOP 1 x.PayCycleId FROM tblPayCycle x
                                       WHERE x.PayCycleType IN (@raw, @normalized)
                                       ORDER BY CASE WHEN x.PayCycleType = @normalized THEN 0 ELSE 1 END)",
                New With {.raw = If(cutoff.CycleType, ""), .normalized = PayCycleChoices.Normalize(cutoff.CycleType)})
            Return Math.Max(1, count)
        End Function

        ''' <summary>
        ''' Halaga ng Fix Transaction na papasok sa cutoff bawat empleyado + column (PayrollInputSheet.HintKey).
        ''' Nasa unit ng column: peso para sa Amount, dami para sa Hours/Minutes/Days. Para lang sa mga code na nasa grid.
        ''' </summary>
        Private Shared Async Function LoadFixHintsAsync(conn As IDbConnection, cutoff As CutoffModel, ids As IList(Of Integer),
                                                        columns As List(Of PayrollInputColumnModel)) As Task(Of Dictionary(Of String, Decimal))
            Dim hints As New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase)
            If ids Is Nothing OrElse ids.Count = 0 OrElse Not cutoff.PeriodInMonth.HasValue Then Return hints

            Dim periods = Await PeriodsPerMonthAsync(conn, cutoff)
            Dim monthKey = PayrollComputationRules.PayMonthKey(cutoff)
            Dim fixes = Await conn.QueryAsync(Of EmployeeFixTxnModel)(FixTxnSql, New With {ids})

            For Each fixRow In fixes
                If Not FixTransactionRules.AppliesTo(fixRow, monthKey, cutoff.PeriodInMonth.Value, periods) Then Continue For
                Dim fx = fixRow
                Dim col = columns.FirstOrDefault(Function(c) Not c.IsBasicDays AndAlso
                                                             FixTransactionRules.SameCode(c.Category.ToString(), c.ColumnName, fx.TxnCategory, fx.TxnCode))
                If col Is Nothing Then Continue For   ' inactive na code / wala sa grid

                Dim key = PayrollInputSheet.HintKey(fixRow.RecordId, col.ColumnName)
                Dim current As Decimal = 0D
                hints.TryGetValue(key, current)
                hints(key) = current + fixRow.Amount
            Next

            Return hints
        End Function

        Public Async Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of PayrollInputSheet) _
            Implements IPayrollInputRepository.GetInputDataAsync

            Dim table As New DataTable()
            table.Columns.Add("RecordId", GetType(Integer))
            table.Columns.Add("EmployeeNo", GetType(String))
            table.Columns.Add("EmployeeName", GetType(String))
            For Each col In columns
                table.Columns.Add(col.ColumnName, GetType(Decimal))   ' nullable: blangko = walang Input (susundin ang Fix kung meron)
            Next

            Dim sheet As New PayrollInputSheet With {.Table = table}

            Dim allCutoffs = Await _cutoffRepo.GetAllAsync()
            Dim cutoff = allCutoffs.FirstOrDefault(Function(c) c.CutoffID = cutoffId)
            If cutoff Is Nothing Then Return sheet

            Using conn = GetConnection()
                ' Rate Basis ng pay cycle ng cutoff: dito nakabatay kung Basic Salary o Daily Rate ang kailangang may laman.
                Dim rateBasis = Await conn.ExecuteScalarAsync(Of Byte?)(
                    "SELECT RateBasis FROM tblPayCycle WHERE PayCycleType = @CycleType", New With {cutoff.CycleType})
                Dim basis = If(rateBasis.HasValue, CType(rateBasis.Value, PayRateBasis), PayRateBasis.BasicSalary)

                Dim candidates = (Await conn.QueryAsync(Of EligibilityCandidate)(EligibilitySql)).ToList()
                Dim eligibility = PayrollEligibilityRules.Build(candidates, cutoff.CycleType, cutoff.CutoffStart, cutoff.CutoffEnd, basis)
                sheet.Warnings = eligibility.Warnings

                Dim txnRows = (Await conn.QueryAsync(Of (RecordId As Integer, TxnCode As String, Qty As Decimal, Rate As Decimal))(
                    "SELECT RecordId, TxnCode, Qty, Rate
                     FROM tblPayrollInputTxn
                     WHERE CutoffID = @CutoffID AND TxnSrc = @TxnSrc",
                    New With {cutoffId, .TxnSrc = TxnSource})).ToLookup(Function(t) t.RecordId)

                Dim ids = eligibility.Eligible.Select(Function(e) e.RecordId).ToList()
                sheet.FixHints = Await LoadFixHintsAsync(conn, cutoff, ids, columns)

                Dim modeByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.ValueMode)

                For Each emp In eligibility.Eligible
                    Dim row = table.NewRow()
                    row("RecordId") = emp.RecordId
                    row("EmployeeNo") = emp.EmployeeNo
                    row("EmployeeName") = emp.EmployeeName
                    For Each col In columns
                        row(col.ColumnName) = DBNull.Value
                    Next
                    ' May naka-save na row = may Input (kasama ang 0 = "skip" ng Fix Transaction).
                    For Each t In txnRows(emp.RecordId)
                        If modeByCode.ContainsKey(t.TxnCode) Then
                            row(t.TxnCode) = If(modeByCode(t.TxnCode) = PayrollTxnValueMode.Amount, t.Rate, t.Qty)
                        End If
                    Next
                    table.Rows.Add(row)
                Next
            End Using

            Return sheet
        End Function

        ''' <summary>
        ''' Blangko = walang Input (hindi sine-save; susundin ang Fix Transaction kung meron).
        ''' May halaga = Input. 0 = sine-save LANG kapag may Fix Transaction ang code sa cutoff na ito ("skip ngayong cutoff");
        ''' kung walang Fix, ang 0 ay parang blangko (gaya ng dati).
        ''' </summary>
        Public Async Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task _
            Implements IPayrollInputRepository.SaveInputDataAsync

            Dim categoryByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.Category.ToString().ToUpperInvariant())
            Dim modeByCode = columns.ToDictionary(Function(c) c.ColumnName, Function(c) c.ValueMode)

            Dim allCutoffs = Await _cutoffRepo.GetAllAsync()
            Dim cutoff = allCutoffs.FirstOrDefault(Function(c) c.CutoffID = cutoffId)

            Using conn = GetConnection()
                Dim ids As New List(Of Integer)()
                For Each row As DataRow In table.Rows
                    ids.Add(CInt(row("RecordId")))
                Next
                Dim hints As New Dictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase)
                If cutoff IsNot Nothing Then hints = Await LoadFixHintsAsync(conn, cutoff, ids, columns)

                Using tx = conn.BeginTransaction()
                    For Each row As DataRow In table.Rows
                        Dim recordId = CInt(row("RecordId"))

                        ' Delete-then-reinsert, scoped to this Cutoff + our own source tag.
                        Await conn.ExecuteAsync(
                            "DELETE FROM tblPayrollInputTxn WHERE CutoffID = @CutoffID AND RecordId = @RecordId AND TxnSrc = @TxnSrc",
                            New With {cutoffId, recordId, .TxnSrc = TxnSource}, tx)

                        For Each col In columns
                            Dim cell = row(col.ColumnName)
                            If cell Is Nothing OrElse cell Is DBNull.Value Then Continue For   ' blangko = walang Input

                            Dim value = Convert.ToDecimal(cell)
                            If value = 0D AndAlso Not hints.ContainsKey(PayrollInputSheet.HintKey(recordId, col.ColumnName)) Then Continue For

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

        Public Async Function CloseEmptyDraftCutoffAsync(cutoffId As Integer, closedBy As String) As Task(Of Boolean) _
            Implements IPayrollInputRepository.CloseEmptyDraftCutoffAsync

            Using conn = GetConnection()
                Dim affected = Await conn.ExecuteAsync(
                    "UPDATE tblCutoff
                     SET Status = @closed, ModifiedBy = @closedBy, ModifiedDate = SYSDATETIME()
                     WHERE CutoffID = @cutoffId AND Status = @draft
                       AND NOT EXISTS (SELECT 1 FROM tblPayrollResult r WHERE r.CutoffID = tblCutoff.CutoffID)
                       AND NOT EXISTS (SELECT 1 FROM tblPayrollInputTxn t WHERE t.CutoffID = tblCutoff.CutoffID)",
                    New With {.closed = CByte(CutoffStatus.Closed), .draft = CByte(CutoffStatus.Draft), cutoffId, closedBy})
                Return affected > 0
            End Using
        End Function

    End Class

End Namespace