Imports System.Data
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Data

    Public Class PayrollProcessRepository
        Inherits BaseRepository(Of PayrollResultModel)
        Implements IPayrollProcessRepository

        ' =============================================
        ' SQL
        ' =============================================

        Private Const EmployeeSql As String =
            "SELECT e.RecordId, e.EmployeeNo,
                    (e.LastName + ', ' + e.FirstName) AS EmployeeName,
                    ee.PayCycle, ee.BasicSalary, ee.DailyRate, ee.HourlyRate, ee.DaysInYear, ee.WorkHourPer,
                    CAST(ISNULL(ee.MinimumWage, 0) AS BIT) AS MinimumWage,
                    es.PagIBIGVoluntary, es.FixTax,
                    em.DateJoined, em.DateResigned, em.DateRetired, em.DateTerminated, em.DateEndOfContract
             FROM tblEmployee e
                  INNER JOIN tblEmployeeEarnings ee ON ee.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeStatutory es ON es.RecordId = e.RecordId
                  LEFT JOIN tblEmployeeEmployment em ON em.RecordId = e.RecordId
             WHERE e.RecordId IN @ids"

        Private Const TxnSql As String =
            "SELECT RecordId, TxnCategory, TxnCode, Qty, Rate
             FROM tblPayrollInputTxn
             WHERE CutoffID = @cutoffId AND RecordId IN @ids"

        ' Lahat ng hindi pa bayad na hulog hanggang sa PayDate; ang mga lampas na sa window ay minarkahang IsOverdue.
        Private Const LoanSql As String =
            "SELECT l.EmployeeRecordId AS RecordId, s.ScheduleId, l.EmployeeLoanId, l.LoanCode,
                    ISNULL(lt.Description, l.LoanCode) AS LoanDescription,
                    s.InstallmentNo, s.ScheduledDate,
                    (s.AmountDue - s.AmountPaid) AS BalanceDue,
                    CAST(CASE WHEN s.ScheduledDate <= @lowerBound THEN 1 ELSE 0 END AS BIT) AS IsOverdue
             FROM tblEmployeeLoanSchedule s
                  INNER JOIN tblEmployeeLoan l ON l.EmployeeLoanId = s.EmployeeLoanId
                  LEFT JOIN tblLoan lt ON lt.Code = l.LoanCode
             WHERE l.Status = @active
               AND s.IsPaid = 0
               AND s.ScheduledDate <= @payDate
               AND l.EmployeeRecordId IN @ids"

        Private Const PriorBaseSql As String =
            "SELECT RecordId,
                    SUM(SSSBase) AS SSSBase, SUM(PhilHealthBase) AS PhilHealthBase, SUM(PagIbigBase) AS PagIbigBase
             FROM tblPayrollResult
             WHERE CutoffID IN @priorIds AND RecordId IN @ids
             GROUP BY RecordId"

        Private Const InsertHeaderSql As String =
            "INSERT INTO tblPayrollResult
                (CutoffID, RecordId, EmployeeNo, EmployeeName, PayCycle, RateBasis, PeriodInMonth, DaysWorked,
                 BasicSalary, DailyRate, HourlyRate, DaysInYear, WorkHourPer, DailyRateMonthlyFactor, IsMinimumWage,
                 BasicPay, GrossPay,
                 TaxGross, SSSBase, PhilHealthBase, PagIbigBase, SSSLookupBase, PhilHealthLookupBase, PagIbigLookupBase,
                 SSSEE, SSSMPFEE, PhilHealthEE, PagIbigEE, SSSER, SSSMPFER, ECC, PhilHealthER, PagIbigER,
                 TaxableIncome, WithholdingTax, LoanDeduction, OtherDeduction, TotalDeduction, NetPay,
                 HasWarnings, WarningText, ProcessedBy, ProcessedDate)
             OUTPUT INSERTED.ResultId
             VALUES
                (@CutoffID, @RecordId, @EmployeeNo, @EmployeeName, @PayCycle, @RateBasis, @PeriodInMonth, @DaysWorked,
                 @BasicSalary, @DailyRate, @HourlyRate, @DaysInYear, @WorkHourPer, @DailyRateMonthlyFactor, @IsMinimumWage,
                 @BasicPay, @GrossPay,
                 @TaxGross, @SSSBase, @PhilHealthBase, @PagIbigBase, @SSSLookupBase, @PhilHealthLookupBase, @PagIbigLookupBase,
                 @SSSEE, @SSSMPFEE, @PhilHealthEE, @PagIbigEE, @SSSER, @SSSMPFER, @ECC, @PhilHealthER, @PagIbigER,
                 @TaxableIncome, @WithholdingTax, @LoanDeduction, @OtherDeduction, @TotalDeduction, @NetPay,
                 @HasWarnings, @WarningText, @ProcessedBy, SYSDATETIME())"

        Private Const InsertLineSql As String =
            "INSERT INTO tblPayrollResultLine
                (ResultId, CutoffID, RecordId, SeqNo, LineType, Category, Code, Description, Qty, Unit, RateUsed, Amount,
                 TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag, RefId, Remark)
             VALUES
                (@ResultId, @CutoffID, @RecordId, @SeqNo, @LineType, @Category, @Code, @Description, @Qty, @Unit, @RateUsed, @Amount,
                 @TaxFlag, @SSSFlag, @PhilHealthFlag, @PagIbigFlag, @RefId, @Remark)"

        Private Const InsertBracketSql As String =
            "INSERT INTO tblPayrollResultBracket
                (ResultId, Agency, SourceTable, SourceId, BasisAmount, SalaryFrom, SalaryTo,
                 EEShare, EEContriType, ERShare, ERContriType, ECCAmount, EEMPF, ERMPF,
                 TaxPercentage, FixTaxAmount, MonthlyEE, MonthlyER, SharePercent)
             VALUES
                (@ResultId, @Agency, @SourceTable, @SourceId, @BasisAmount, @SalaryFrom, @SalaryTo,
                 @EEShare, @EEContriType, @ERShare, @ERContriType, @ECCAmount, @EEMPF, @ERMPF,
                 @TaxPercentage, @FixTaxAmount, @MonthlyEE, @MonthlyER, @SharePercent)"

        ' ---- Export at Post ----
        Private Const PayoutSql As String =
            "SELECT r.ResultId, r.CutoffID, r.RecordId, r.EmployeeNo, r.EmployeeName, r.NetPay,
                    ee.PayBy, ee.BankId, b.BankCode, ISNULL(b.BankName, ee.BankName) AS BankName, ee.BankAccount
             FROM tblPayrollResult r
                  LEFT JOIN tblEmployeeEarnings ee ON ee.RecordId = r.RecordId
                  LEFT JOIN tblBank b ON b.BankId = ee.BankId
             WHERE r.CutoffID IN @ids
             ORDER BY r.EmployeeName, r.EmployeeNo"

        Private Const InsertExportLogSql As String =
            "INSERT INTO tblPayrollExportLog
                (CutoffID, BatchId, ListGroup, Format, FileName, FilePath, EmployeeCount, TotalAmount, IsCombined, ProcessedStamp, ExportedBy)
             SELECT @CutoffID, @BatchId, @ListGroup, @Format, @FileName, @FilePath, @EmployeeCount, @TotalAmount, @IsCombined,
                    c.LastProcessedDate, @ExportedBy
             FROM tblCutoff c
             WHERE c.CutoffID = @CutoffID"

        ' Mga listahang na-export para sa KASALUKUYANG processing (hindi na valid kapag na-re-process ang cutoff).
        Private Const ValidGroupsSql As String =
            "SELECT DISTINCT e.ListGroup
             FROM tblPayrollExportLog e
                  INNER JOIN tblCutoff c ON c.CutoffID = e.CutoffID
             WHERE e.CutoffID = @cutoffId AND e.ProcessedStamp = c.LastProcessedDate"

        Private Const LoanCheckSql As String =
            "SELECT r.EmployeeName, l.Description, l.Amount AS LineAmount,
                    s.ScheduleId AS FoundScheduleId, s.AmountDue, s.AmountPaid, s.IsPaid, ln.Status AS LoanStatus
             FROM tblPayrollResultLine l
                  INNER JOIN tblPayrollResult r ON r.ResultId = l.ResultId
                  LEFT JOIN tblEmployeeLoanSchedule s ON s.ScheduleId = l.RefId
                  LEFT JOIN tblEmployeeLoan ln ON ln.EmployeeLoanId = s.EmployeeLoanId
             WHERE l.CutoffID = @cutoffId AND l.Category = @loan"

        ' AmountPaid = dating bayad + ang kinaltas ngayon (laging buong BalanceDue ang LOAN line, kaya = AmountDue).
        Private Const MarkLoansPaidSql As String =
            "UPDATE s
             SET    s.AmountPaid   = s.AmountPaid + l.Amount,
                    s.PaidDate     = @paidDate,
                    s.IsPaid       = CASE WHEN s.AmountPaid + l.Amount >= s.AmountDue THEN 1 ELSE 0 END,
                    s.PayrollRefNo = @payrollRefNo,
                    s.UpdatedAt    = GETDATE(),
                    s.UpdatedBy    = @userName
             FROM   tblEmployeeLoanSchedule s
                    INNER JOIN tblPayrollResultLine l ON l.RefId = s.ScheduleId
             WHERE  l.CutoffID = @cutoffId AND l.Category = @loan"

        ' Kumpleto na ang loan kapag wala nang hindi bayad na installment (kapareho ng IsFullyPaid sa Loans module).
        Private Const CompleteLoansSql As String =
            "UPDATE ln
             SET    ln.Status      = @completed,
                    ln.CompletedAt = ISNULL(ln.CompletedAt, GETDATE()),
                    ln.UpdatedAt   = GETDATE(),
                    ln.UpdatedBy   = @userName
             FROM   tblEmployeeLoan ln
             WHERE  ln.Status = @active
               AND  ln.EmployeeLoanId IN (SELECT s.EmployeeLoanId
                                          FROM tblEmployeeLoanSchedule s
                                               INNER JOIN tblPayrollResultLine l ON l.RefId = s.ScheduleId
                                          WHERE l.CutoffID = @cutoffId AND l.Category = @loan)
               AND  NOT EXISTS (SELECT 1 FROM tblEmployeeLoanSchedule x
                                WHERE x.EmployeeLoanId = ln.EmployeeLoanId AND x.IsPaid = 0)"

        ' =============================================
        ' LOAD
        ' =============================================

        Public Async Function LoadEmployeesAsync(cutoff As CutoffModel, basis As PayRateBasis,
                                                 priorCutoffIds As IList(Of Integer), loanLowerBound As Date) As Task(Of PayrollEmployeeLoad) _
            Implements IPayrollProcessRepository.LoadEmployeesAsync

            Dim result As New PayrollEmployeeLoad()

            Using conn = GetConnection()
                ' Iisang patakaran sa eligibility para sa Input Entry at sa Process (PayrollEligibilityRules)
                Dim candidates = (Await conn.QueryAsync(Of EligibilityCandidate)(PayrollInputRepository.EligibilitySql)).ToList()
                Dim eligibility = PayrollEligibilityRules.Build(candidates, cutoff.CycleType, cutoff.CutoffStart, cutoff.CutoffEnd, basis)
                result.Excluded = eligibility.Warnings

                Dim ids = eligibility.Eligible.Select(Function(e) e.RecordId).ToList()
                If ids.Count = 0 Then Return result

                result.Employees = (Await conn.QueryAsync(Of PayrollEmployeeInput)(EmployeeSql, New With {ids})).ToList()

                Dim txns = (Await conn.QueryAsync(Of PayrollTxnInput)(TxnSql, New With {.cutoffId = cutoff.CutoffID, ids})).ToLookup(Function(t) t.RecordId)

                Dim payDate = If(cutoff.PayDate.HasValue, cutoff.PayDate.Value, cutoff.CutoffEnd).Date
                Dim loans = (Await conn.QueryAsync(Of PayrollLoanDue)(LoanSql, New With {
                    .lowerBound = loanLowerBound.Date, .active = LoanStatus.Active, payDate, ids})).ToLookup(Function(l) l.RecordId)

                Dim priors As New Dictionary(Of Integer, PayrollPriorBases)()
                If priorCutoffIds IsNot Nothing AndAlso priorCutoffIds.Count > 0 Then
                    Dim priorRows = Await conn.QueryAsync(Of PayrollPriorBases)(PriorBaseSql, New With {.priorIds = priorCutoffIds, ids})
                    For Each p In priorRows
                        priors(p.RecordId) = p
                    Next
                End If

                ' Employee > Fix Transactions (active lang; ang engine ang sumasala ayon sa cutoff at Input Entry override)
                Dim fixes = (Await conn.QueryAsync(Of EmployeeFixTxnModel)(PayrollInputRepository.FixTxnSql, New With {ids})).
                            ToLookup(Function(fx) fx.RecordId)

                For Each emp In result.Employees
                    emp.Txns = txns(emp.RecordId).ToList()
                    emp.FixTxns = fixes(emp.RecordId).ToList()
                    emp.Loans = loans(emp.RecordId).OrderBy(Function(l) l.ScheduledDate).ThenBy(Function(l) l.InstallmentNo).ToList()
                    Dim prior As PayrollPriorBases = Nothing
                    If priors.TryGetValue(emp.RecordId, prior) Then emp.Prior = prior
                Next
            End Using

            Return result
        End Function

        ' =============================================
        ' SAVE (isang transaction)
        ' =============================================

        Public Async Function SaveResultsAsync(cutoffId As Integer, results As List(Of PayrollEmployeeResult), userName As String) As Task _
            Implements IPayrollProcessRepository.SaveResultsAsync

            Using conn = GetConnection()
                Using tx = conn.BeginTransaction()

                    ' I-lock ang cutoff row habang nagse-save, at siguraduhing hindi pa Posted/Closed.
                    Dim status = Await conn.ExecuteScalarAsync(Of Byte?)(
                        "SELECT Status FROM tblCutoff WITH (UPDLOCK, ROWLOCK) WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

                    If Not status.HasValue Then
                        Throw New InvalidOperationException("Selected Cutoff no longer exists.")
                    End If
                    If status.Value = CByte(CutoffStatus.Posted) Then
                        Throw New InvalidOperationException("This Cutoff is already Posted and can no longer be re-processed.")
                    End If
                    If status.Value = CByte(CutoffStatus.Closed) Then
                        Throw New InvalidOperationException("This Cutoff is Closed. Set it back to Draft in Payroll Settings > Cut-Off first.")
                    End If

                    ' Re-process: burahin muna ang lahat ng lumang results ng cutoff (cascade sa lines at brackets).
                    Await conn.ExecuteAsync("DELETE FROM tblPayrollResult WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

                    For Each r In results
                        r.Header.CutoffID = cutoffId
                        r.Header.ProcessedBy = userName

                        Dim resultId = Await conn.ExecuteScalarAsync(Of Integer)(InsertHeaderSql, r.Header, tx)
                        r.Header.ResultId = resultId

                        For Each ln In r.Lines
                            ln.ResultId = resultId
                            ln.CutoffID = cutoffId
                            ln.RecordId = r.Header.RecordId
                        Next
                        If r.Lines.Count > 0 Then Await conn.ExecuteAsync(InsertLineSql, r.Lines, tx)

                        For Each b In r.Brackets
                            b.ResultId = resultId
                        Next
                        If r.Brackets.Count > 0 Then Await conn.ExecuteAsync(InsertBracketSql, r.Brackets, tx)
                    Next

                    Await conn.ExecuteAsync(
                        "UPDATE tblCutoff
                         SET Status = @processed, LastProcessedBy = @userName, LastProcessedDate = SYSDATETIME()
                         WHERE CutoffID = @cutoffId",
                        New With {.processed = CByte(CutoffStatus.Processed), userName, cutoffId}, tx)

                    tx.Commit()
                End Using
            End Using
        End Function

        ' =============================================
        ' OUTPUT TAB
        ' =============================================

        Public Async Function GetCutoffsWithResultsAsync() As Task(Of List(Of CutoffModel)) _
            Implements IPayrollProcessRepository.GetCutoffsWithResultsAsync

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of CutoffModel)(
                    "SELECT c.*
                     FROM tblCutoff c
                     WHERE EXISTS (SELECT 1 FROM tblPayrollResult r WHERE r.CutoffID = c.CutoffID)
                     ORDER BY c.CutoffStart DESC, c.CycleType")
                Return rows.ToList()
            End Using
        End Function

        Public Async Function GetResultsAsync(cutoffId As Integer) As Task(Of PayrollResultSheet) _
            Implements IPayrollProcessRepository.GetResultsAsync

            Dim sheet As New PayrollResultSheet()

            Using conn = GetConnection()
                sheet.Headers = (Await conn.QueryAsync(Of PayrollResultModel)(
                    "SELECT * FROM tblPayrollResult WHERE CutoffID = @cutoffId ORDER BY EmployeeName, EmployeeNo",
                    New With {cutoffId})).ToList()

                sheet.Lines = (Await conn.QueryAsync(Of PayrollResultLineModel)(
                    "SELECT * FROM tblPayrollResultLine WHERE CutoffID = @cutoffId ORDER BY ResultId, SeqNo",
                    New With {cutoffId})).ToList()
            End Using

            Return sheet
        End Function

        ' =============================================
        ' EXPORT LOG AT POST
        ' =============================================

        Public Async Function GetPayoutRowsAsync(cutoffIds As IList(Of Integer)) As Task(Of List(Of PayrollPayoutRow)) _
            Implements IPayrollProcessRepository.GetPayoutRowsAsync

            If cutoffIds Is Nothing OrElse cutoffIds.Count = 0 Then Return New List(Of PayrollPayoutRow)()

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of PayrollPayoutRow)(PayoutSql, New With {.ids = cutoffIds})
                Return rows.ToList()
            End Using
        End Function

        Public Async Function InsertExportLogAsync(entries As List(Of PayrollExportLogEntry)) As Task _
            Implements IPayrollProcessRepository.InsertExportLogAsync

            If entries Is Nothing OrElse entries.Count = 0 Then Return

            Using conn = GetConnection()
                Using tx = conn.BeginTransaction()
                    Await conn.ExecuteAsync(InsertExportLogSql, entries, tx)
                    tx.Commit()
                End Using
            End Using
        End Function

        Public Async Function GetPostFactsAsync(cutoffId As Integer) As Task(Of PayrollPostFacts) _
            Implements IPayrollProcessRepository.GetPostFactsAsync

            Dim facts As New PayrollPostFacts()

            Using conn = GetConnection()
                facts.WarningCount = Await conn.ExecuteScalarAsync(Of Integer)(
                    "SELECT COUNT(*) FROM tblPayrollResult WHERE CutoffID = @cutoffId AND HasWarnings = 1", New With {cutoffId})

                facts.LoanInstallments = Await conn.ExecuteScalarAsync(Of Integer)(
                    "SELECT COUNT(*) FROM tblPayrollResultLine WHERE CutoffID = @cutoffId AND Category = @loan AND RefId IS NOT NULL",
                    New With {cutoffId, .loan = ResultCategory.Loan})

                facts.ValidGroups = (Await conn.QueryAsync(Of String)(ValidGroupsSql, New With {cutoffId})).ToList()
            End Using

            Return facts
        End Function

        ''' <summary>Isang transaction para sa LAHAT ng hiniling na cutoff: kapag may isang pumalya, walang mapo-post (rollback lahat).</summary>
        Public Async Function PostCutoffsAsync(requests As IList(Of PayrollPostRequest), userName As String) As Task(Of PayrollPostResult) _
            Implements IPayrollProcessRepository.PostCutoffsAsync

            Dim total As New PayrollPostResult()
            Dim refs As New List(Of String)()

            Using conn = GetConnection()
                Using tx = conn.BeginTransaction()
                    For Each req In requests
                        Dim one As PayrollPostResult = Nothing
                        Try
                            one = Await PostOneAsync(conn, tx, req.CutoffID, req.RequiredGroups, userName)
                        Catch ex As InvalidOperationException
                            Throw New InvalidOperationException(req.CutoffLabel & ": " & ex.Message)
                        End Try

                        total.LoanInstallmentsPaid += one.LoanInstallmentsPaid
                        total.LoansCompleted += one.LoansCompleted
                        total.CutoffsPosted += 1
                        refs.Add(one.PayrollRefNo)
                    Next

                    tx.Commit()
                End Using
            End Using

            total.PayrollRefNo = String.Join(", ", refs)
            Return total
        End Function

        Private Async Function PostOneAsync(conn As IDbConnection, tx As IDbTransaction, cutoffId As Integer,
                                            requiredGroups As IList(Of String), userName As String) As Task(Of PayrollPostResult)

            Dim result As New PayrollPostResult With {.PayrollRefNo = "PR-" & cutoffId.ToString()}

            ' I-lock ang cutoff row habang nagpo-post.
            Dim status = Await conn.ExecuteScalarAsync(Of Byte?)(
                "SELECT Status FROM tblCutoff WITH (UPDLOCK, ROWLOCK) WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

            If Not status.HasValue Then Throw New InvalidOperationException("Selected Cutoff no longer exists.")
            If status.Value = CByte(CutoffStatus.Posted) Then Throw New InvalidOperationException("This Cutoff is already Posted.")
            If status.Value <> CByte(CutoffStatus.Processed) Then Throw New InvalidOperationException("Only a Processed Cutoff can be posted.")

            ' May babala pa?
            Dim warnings = Await conn.ExecuteScalarAsync(Of Integer)(
                "SELECT COUNT(*) FROM tblPayrollResult WHERE CutoffID = @cutoffId AND HasWarnings = 1", New With {cutoffId}, tx)
            If warnings > 0 Then
                Throw New InvalidOperationException($"{warnings} employee(s) still have warnings. Fix them and re-process the cutoff first.")
            End If

            ' Na-export na ba ang lahat ng kailangang listahan para sa KASALUKUYANG processing?
            Dim exported = (Await conn.QueryAsync(Of String)(ValidGroupsSql, New With {cutoffId}, tx)).ToList()
            Dim missing = If(requiredGroups, New List(Of String)()).Where(Function(g) Not exported.Contains(g)).ToList()
            If missing.Count > 0 Then
                Throw New InvalidOperationException("Export these lists first (the cutoff was re-processed or never exported): " &
                                                    String.Join(", ", missing.Select(Function(g) PayoutGroup.DisplayName(g))))
            End If

            ' Stale check: hindi dapat nagbago ang loan installments mula nang i-process.
            Dim loanRows = (Await conn.QueryAsync(Of PayrollLoanCheckRow)(LoanCheckSql, New With {cutoffId, .loan = ResultCategory.Loan}, tx)).ToList()
            Dim problems As New List(Of String)()
            For Each lr In loanRows
                Dim who = lr.EmployeeName & " - " & lr.Description
                If Not lr.FoundScheduleId.HasValue Then
                    problems.Add(who & ": installment no longer exists")
                ElseIf lr.IsPaid.GetValueOrDefault() Then
                    problems.Add(who & ": installment is already paid")
                ElseIf Not String.Equals(lr.LoanStatus, LoanStatus.Active, StringComparison.OrdinalIgnoreCase) Then
                    problems.Add(who & ": loan is no longer Active (" & lr.LoanStatus & ")")
                ElseIf Math.Abs((lr.AmountDue.GetValueOrDefault() - lr.AmountPaid.GetValueOrDefault()) - lr.LineAmount) > 0.005D Then
                    problems.Add(who & ": balance changed")
                End If
            Next
            If problems.Count > 0 Then
                Throw New InvalidOperationException("Loan installments changed after this cutoff was processed. Re-process the cutoff and export again:" &
                                                    Environment.NewLine & String.Join(Environment.NewLine, problems.Take(8).Select(Function(x) "- " & x)) &
                                                    If(problems.Count > 8, Environment.NewLine & "... and " & (problems.Count - 8) & " more", ""))
            End If

            ' Loans: markahang bayad
            Dim paidDate = Await conn.ExecuteScalarAsync(Of DateTime)(
                "SELECT ISNULL(PayDate, CutoffEnd) FROM tblCutoff WHERE CutoffID = @cutoffId", New With {cutoffId}, tx)

            result.LoanInstallmentsPaid = Await conn.ExecuteAsync(MarkLoansPaidSql, New With {
                cutoffId, .loan = ResultCategory.Loan, paidDate, .payrollRefNo = result.PayrollRefNo, userName}, tx)

            result.LoansCompleted = Await conn.ExecuteAsync(CompleteLoansSql, New With {
                cutoffId, .loan = ResultCategory.Loan, .completed = LoanStatus.Completed, .active = LoanStatus.Active, userName}, tx)

            ' I-lock ang cutoff
            Await conn.ExecuteAsync(
                "UPDATE tblCutoff
                 SET Status = @posted, PostedBy = @userName, PostedDate = SYSDATETIME(),
                     ModifiedBy = @userName, ModifiedDate = SYSDATETIME()
                 WHERE CutoffID = @cutoffId",
                New With {.posted = CByte(CutoffStatus.Posted), userName, cutoffId}, tx)

            Return result
        End Function

        ' =============================================
        ' PAY-DATE MODE / BDO NAMING
        ' =============================================

        Public Async Function GetResultsForCutoffsAsync(cutoffIds As IList(Of Integer)) As Task(Of PayrollResultSheet) _
            Implements IPayrollProcessRepository.GetResultsForCutoffsAsync

            Dim sheet As New PayrollResultSheet()
            If cutoffIds Is Nothing OrElse cutoffIds.Count = 0 Then Return sheet

            Using conn = GetConnection()
                sheet.Headers = (Await conn.QueryAsync(Of PayrollResultModel)(
                    "SELECT * FROM tblPayrollResult WHERE CutoffID IN @ids ORDER BY EmployeeName, EmployeeNo",
                    New With {.ids = cutoffIds})).ToList()

                sheet.Lines = (Await conn.QueryAsync(Of PayrollResultLineModel)(
                    "SELECT * FROM tblPayrollResultLine WHERE CutoffID IN @ids ORDER BY ResultId, SeqNo",
                    New With {.ids = cutoffIds})).ToList()
            End Using

            Return sheet
        End Function

        ''' <summary>Company Code ng BDO Payroll Converter = Customer ID ng aktibong BDO row sa Company Bank settings.</summary>
        Public Async Function GetBdoCompanyCodeAsync() As Task(Of String) _
            Implements IPayrollProcessRepository.GetBdoCompanyCodeAsync

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of String)(
                    "SELECT TOP 1 LTRIM(RTRIM(CustomerID))
                     FROM tblCompanyBank
                     WHERE BankCode = 'BDO' AND IsActive = 1 AND CustomerID IS NOT NULL AND LTRIM(RTRIM(CustomerID)) <> ''
                     ORDER BY BankId")
            End Using
        End Function

        Public Async Function GetBdoDirectFileNamesAsync(prefix As String) As Task(Of List(Of String)) _
            Implements IPayrollProcessRepository.GetBdoDirectFileNamesAsync

            Using conn = GetConnection()
                Dim rows = Await conn.QueryAsync(Of String)(
                    "SELECT DISTINCT FileName FROM tblPayrollExportLog WHERE Format = @format AND FileName LIKE @pattern",
                    New With {.format = ExportFormat.BdoDirect, .pattern = prefix & "%"})
                Return rows.ToList()
            End Using
        End Function

    End Class

End Namespace
