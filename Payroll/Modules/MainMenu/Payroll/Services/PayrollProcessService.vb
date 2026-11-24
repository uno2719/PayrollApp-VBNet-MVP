Imports System.IO
Imports Payroll.GeneralSettings.Data
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.IncomeTaxTable.Data
Imports Payroll.PayrollProcessing.Data
Imports Payroll.PayrollSettings.Data
Imports Payroll.StatutorySettings.Data

Namespace PayrollProcessing.Services

    ''' <summary>
    ''' Ang Process: sinusuri ang cutoff at mga setting, ikinakarga ang lahat ng kailangan, ipinapasa sa
    ''' PayrollComputationRules (pure logic), at ise-save ang resulta pagkatapos mag-Confirm ang user.
    ''' </summary>
    Public Class PayrollProcessService
        Inherits BaseService
        Implements IPayrollProcessService

        Private ReadOnly _processRepo As IPayrollProcessRepository
        Private ReadOnly _cutoffRepo As ICutoffRepository
        Private ReadOnly _payCycleRepo As IPayCycleRepository
        Private ReadOnly _settingsRepo As IGeneralSettingsRepository
        Private ReadOnly _compensationRepo As ICompensationRepository
        Private ReadOnly _rateEntryRepo As IPayrollRateEntryRepository
        Private ReadOnly _flaggedEntryRepo As IPayrollFlaggedEntryRepository
        Private ReadOnly _statutoryRepo As IStatutorySettingsRepository
        Private ReadOnly _taxRepo As IIncomeTaxTableRepository
        Private ReadOnly _scheduleRepo As IStatutoryScheduleRepository

        Public Sub New(processRepo As IPayrollProcessRepository,
                       cutoffRepo As ICutoffRepository,
                       payCycleRepo As IPayCycleRepository,
                       settingsRepo As IGeneralSettingsRepository,
                       compensationRepo As ICompensationRepository,
                       rateEntryRepo As IPayrollRateEntryRepository,
                       flaggedEntryRepo As IPayrollFlaggedEntryRepository,
                       statutoryRepo As IStatutorySettingsRepository,
                       taxRepo As IIncomeTaxTableRepository,
                       scheduleRepo As IStatutoryScheduleRepository)
            _processRepo = processRepo
            _cutoffRepo = cutoffRepo
            _payCycleRepo = payCycleRepo
            _settingsRepo = settingsRepo
            _compensationRepo = compensationRepo
            _rateEntryRepo = rateEntryRepo
            _flaggedEntryRepo = flaggedEntryRepo
            _statutoryRepo = statutoryRepo
            _taxRepo = taxRepo
            _scheduleRepo = scheduleRepo
        End Sub

        Private Shared Sub Fail(message As String)
            Throw New PayrollInputValidationException(message)
        End Sub

        Public Async Function BuildAsync(cutoffId As Integer) As Task(Of PayrollRunPreview) _
            Implements IPayrollProcessService.BuildAsync

            ' ---------- 1. Ang cutoff ----------
            Dim allCutoffs = Await _cutoffRepo.GetAllAsync()
            Dim cutoff = allCutoffs.FirstOrDefault(Function(c) c.CutoffID = cutoffId)

            If cutoff Is Nothing Then Fail("Selected Cutoff no longer exists.")
            If cutoff.Status = CutoffStatus.Closed Then
                Fail("This Cutoff is Closed. Set it back to Draft in Payroll Settings > Cut-Off before processing.")
            End If
            If cutoff.Status = CutoffStatus.Posted Then
                Fail("This Cutoff is already Posted and can no longer be re-processed.")
            End If
            If Not cutoff.PeriodInMonth.HasValue Then
                Fail("This Cutoff has no 'Period in month' (older cutoff). Open Payroll Settings > Cut-Off and regenerate or update it first.")
            End If

            ' ---------- 2. Pay cycle ----------
            Dim payCycle = Await _payCycleRepo.GetByTypeAsync(PayCycleChoices.Normalize(cutoff.CycleType))
            If payCycle Is Nothing Then payCycle = Await _payCycleRepo.GetByTypeAsync(cutoff.CycleType)
            If payCycle Is Nothing Then Fail($"Pay Cycle '{cutoff.CycleType}' was not found in Payroll Settings > Pay Cycle.")
            If payCycle.PeriodsPerMonth < 1 Then Fail($"Pay Cycle '{payCycle.PayCycleType}' has no cutoff pattern rows.")
            If cutoff.PeriodInMonth.Value > payCycle.PeriodsPerMonth Then
                Fail($"This Cutoff's period ({cutoff.PeriodInMonth.Value}) is more than the {payCycle.PeriodsPerMonth} period(s) of Pay Cycle '{payCycle.PayCycleType}'.")
            End If

            ' ---------- 3. General Settings at Basic Salary Code (+) ----------
            Dim settings = Await _settingsRepo.GetAsync()
            If settings Is Nothing Then Fail("General Settings are not set yet (Settings > General Settings).")
            If Not settings.BasicSalaryCodePlusId.HasValue Then
                Fail("'Basic Salary Code (+)' is not set in General Settings > Code Mapping. Set it to the BASIC compensation code first.")
            End If

            Dim compensation = Await _compensationRepo.GetAllAsync()
            Dim basicCode = compensation.FirstOrDefault(Function(c) c.Id = settings.BasicSalaryCodePlusId.Value)
            If basicCode Is Nothing Then Fail("'Basic Salary Code (+)' in General Settings points to a Compensation code that no longer exists.")
            If payCycle.RateBasis = PayRateBasis.DailyRate AndAlso basicCode.InputUnit <> PayrollInputUnit.Days Then
                Fail($"Pay Cycle '{payCycle.PayCycleType}' is Daily Rate: the Basic Salary code ({basicCode.Code}) must have Input unit = Days (Payroll Settings > Compensation), because its entry in Input Entry is the Days Worked.")
            End If

            ' ---------- 4. Statutory schedule at tables ----------
            Dim schedule = Await _scheduleRepo.GetAllAsync()
            For Each agency In StatutoryAgency.All
                Dim missing As Boolean
                PayrollComputationRules.PeriodShares(schedule, cutoff.CycleType, agency, payCycle.PeriodsPerMonth, missing)
                If missing Then
                    Fail($"Statutory Deduction Schedule is not set for {StatutoryAgency.DisplayName(agency)} ({payCycle.PayCycleType}). Set it in Settings > Statutory > Deduction Schedule.")
                End If
            Next

            Dim sss = Await _statutoryRepo.GetAllAsync("tblStatutorySSS")
            Dim philHealth = Await _statutoryRepo.GetAllAsync("tblStatutoryPhilHealth")
            Dim pagIbig = Await _statutoryRepo.GetAllAsync("tblStatutoryPagIbig")
            If Not sss.Any(Function(b) b.IsActive) Then Fail("The SSS table has no active rows (Settings > Statutory).")
            If Not philHealth.Any(Function(b) b.IsActive) Then Fail("The PhilHealth table has no active rows (Settings > Statutory).")
            If Not pagIbig.Any(Function(b) b.IsActive) Then Fail("The Pag-IBIG table has no active rows (Settings > Statutory).")

            Dim taxTable = PayrollComputationRules.TaxTableNameFor(cutoff.CycleType)
            If taxTable Is Nothing Then Fail($"There is no income tax table for Pay Cycle '{cutoff.CycleType}'.")
            Dim taxBrackets = Await _taxRepo.GetAllAsync(taxTable)
            If Not taxBrackets.Any(Function(b) b.IsActive) Then Fail($"The income tax table ({taxTable}) has no active rows (Settings > Income Tax Table).")

            ' ---------- 5. Mga naunang cutoff ng parehong pay month (para sa SSS base ng buong buwan) ----------
            Dim cycleKey = PayCycleChoices.Normalize(cutoff.CycleType)
            Dim monthKey = PayrollComputationRules.PayMonthKey(cutoff)

            Dim earlier = allCutoffs.Where(Function(c) c.CutoffID <> cutoff.CutoffID AndAlso
                                                       String.Equals(PayCycleChoices.Normalize(c.CycleType), cycleKey, StringComparison.OrdinalIgnoreCase) AndAlso
                                                       PayrollComputationRules.PayMonthKey(c) = monthKey AndAlso
                                                       c.PeriodInMonth.HasValue AndAlso
                                                       c.PeriodInMonth.Value < cutoff.PeriodInMonth.Value).ToList()

            Dim pending = earlier.Where(Function(c) c.Status = CutoffStatus.Draft).OrderBy(Function(c) c.CutoffStart).ToList()
            If pending.Count > 0 Then
                Fail("Process the earlier cutoff(s) of this pay month first (SSS needs the whole month's earnings): " &
                     String.Join(", ", pending.Select(Function(c) c.CutoffLabel)) &
                     ". If they are not processed in this app, set them to Closed in Payroll Settings > Cut-Off.")
            End If

            Dim priorIds = earlier.Where(Function(c) c.Status = CutoffStatus.Processed OrElse c.Status = CutoffStatus.Posted).
                                   Select(Function(c) c.CutoffID).ToList()

            ' Window ng loan: mula sa PayDate ng naunang cutoff ng parehong pay cycle (hindi kasama) hanggang sa PayDate nito.
            Dim payDate = If(cutoff.PayDate.HasValue, cutoff.PayDate.Value, cutoff.CutoffEnd).Date
            Dim previousPayDates = allCutoffs.Where(Function(c) String.Equals(PayCycleChoices.Normalize(c.CycleType), cycleKey, StringComparison.OrdinalIgnoreCase) AndAlso
                                                                c.PayDate.HasValue AndAlso c.PayDate.Value.Date < payDate).
                                              Select(Function(c) c.PayDate.Value.Date).ToList()
            Dim loanLowerBound = If(previousPayDates.Count > 0,
                                    previousPayDates.Max(),
                                    payDate.AddDays(-((cutoff.CutoffEnd.Date - cutoff.CutoffStart.Date).Days + 1)))

            ' ---------- 6. Mga empleyado ----------
            Dim load = Await _processRepo.LoadEmployeesAsync(cutoff, payCycle.RateBasis, priorIds, loanLowerBound)
            If load.Employees.Count = 0 Then
                Fail("There are no employees to process in this cutoff (check Employee > Earnings / Employment and the 'Include in payroll' flag).")
            End If

            ' ---------- 7. Catalog (bawat code sa Input Entry) ----------
            Dim catalog As New Dictionary(Of String, PayrollCatalogItem)(StringComparer.OrdinalIgnoreCase)

            For Each r In Await _rateEntryRepo.GetAllAsync("tblOvertime")
                catalog(PayrollComputationRules.CatalogKey(ResultCategory.Overtime, r.Code)) = New PayrollCatalogItem With {
                    .Category = ResultCategory.Overtime, .Code = r.Code, .Description = r.Description, .InputUnit = r.InputUnit,
                    .Multiplier = r.Rate, .TaxFlag = r.TaxFlag, .SSSFlag = r.SSSFlag, .PhilHealthFlag = r.PhilHealthFlag,
                    .PagIbigFlag = r.PagIbigFlag, .IsActive = r.IsActive}
            Next
            For Each r In Await _rateEntryRepo.GetAllAsync("tblHoliday")
                catalog(PayrollComputationRules.CatalogKey(ResultCategory.Holiday, r.Code)) = New PayrollCatalogItem With {
                    .Category = ResultCategory.Holiday, .Code = r.Code, .Description = r.Description, .InputUnit = r.InputUnit,
                    .Multiplier = r.Rate, .TaxFlag = r.TaxFlag, .SSSFlag = r.SSSFlag, .PhilHealthFlag = r.PhilHealthFlag,
                    .PagIbigFlag = r.PagIbigFlag, .IsActive = r.IsActive}
            Next
            For Each r In compensation
                ' Ang BASIC code (Basic Salary Code +) ay awtomatiko na - hindi pwedeng may Input Entry para dito (iwas doble).
                If r.Id = basicCode.Id Then Continue For
                catalog(PayrollComputationRules.CatalogKey(ResultCategory.Compensation, r.Code)) = New PayrollCatalogItem With {
                    .Category = ResultCategory.Compensation, .Code = r.Code, .Description = r.Description, .InputUnit = r.InputUnit,
                    .TaxFlag = r.TaxFlag, .SSSFlag = r.SSSFlag, .PhilHealthFlag = r.PhilHealthFlag,
                    .PagIbigFlag = r.PagIbigFlag, .IsActive = r.IsActive}
            Next
            For Each r In Await _flaggedEntryRepo.GetAllAsync("tblBonus")
                catalog(PayrollComputationRules.CatalogKey(ResultCategory.Bonus, r.Code)) = New PayrollCatalogItem With {
                    .Category = ResultCategory.Bonus, .Code = r.Code, .Description = r.Description, .InputUnit = r.InputUnit,
                    .TaxFlag = r.TaxFlag, .SSSFlag = r.SSSFlag, .PhilHealthFlag = r.PhilHealthFlag,
                    .PagIbigFlag = r.PagIbigFlag, .IsActive = r.IsActive}
            Next
            For Each r In Await _flaggedEntryRepo.GetAllAsync("tblDeduction")
                catalog(PayrollComputationRules.CatalogKey(ResultCategory.Deduction, r.Code)) = New PayrollCatalogItem With {
                    .Category = ResultCategory.Deduction, .Code = r.Code, .Description = r.Description, .InputUnit = r.InputUnit,
                    .TaxFlag = r.TaxFlag, .SSSFlag = r.SSSFlag, .PhilHealthFlag = r.PhilHealthFlag,
                    .PagIbigFlag = r.PagIbigFlag, .IsActive = r.IsActive}
            Next

            ' ---------- 8. Compute ----------
            Dim ctx As New PayrollComputationContext With {
                .Cutoff = cutoff,
                .PayCycle = payCycle,
                .Settings = settings,
                .BasicCode = basicCode,
                .Catalog = catalog,
                .Schedule = schedule,
                .SssBrackets = sss,
                .PhilHealthBrackets = philHealth,
                .PagIbigBrackets = pagIbig,
                .TaxTableName = taxTable,
                .TaxBrackets = taxBrackets,
                .Employees = load.Employees
            }

            Return New PayrollRunPreview With {
                .Cutoff = cutoff,
                .Results = PayrollComputationRules.ComputeAll(ctx),
                .Excluded = load.Excluded
            }
        End Function

        Public Function SaveAsync(preview As PayrollRunPreview, userName As String) As Task _
            Implements IPayrollProcessService.SaveAsync
            Return _processRepo.SaveResultsAsync(preview.Cutoff.CutoffID, preview.Results, userName)
        End Function

        Public Function GetResultCutoffsAsync() As Task(Of List(Of CutoffModel)) _
            Implements IPayrollProcessService.GetResultCutoffsAsync
            Return _processRepo.GetCutoffsWithResultsAsync()
        End Function

        Public Function GetResultsAsync(cutoffId As Integer) As Task(Of PayrollResultSheet) _
            Implements IPayrollProcessService.GetResultsAsync
            Return _processRepo.GetResultsAsync(cutoffId)
        End Function

        ' =============================================
        ' PAY-DATE MODE (Output tab)
        ' =============================================

        Public Async Function GetPayDateGroupsAsync() As Task(Of List(Of PayDateGroupModel)) _
            Implements IPayrollProcessService.GetPayDateGroupsAsync

            Dim withResults = Await _processRepo.GetCutoffsWithResultsAsync()
            Return withResults.
                GroupBy(Function(c) PayrollExportBuilder.EffectivePayDate(c)).
                OrderByDescending(Function(g) g.Key).
                Select(Function(g) New PayDateGroupModel With {
                    .PayDate = g.Key,
                    .Cutoffs = g.OrderBy(Function(c) c.CutoffStart).ThenBy(Function(c) c.CycleType).ToList()}).
                ToList()
        End Function

        Public Async Function GetResultsForCutoffsAsync(cutoffIds As IList(Of Integer)) As Task(Of PayrollResultSheet) _
            Implements IPayrollProcessService.GetResultsForCutoffsAsync

            Dim sheet = Await _processRepo.GetResultsForCutoffsAsync(cutoffIds)

            Dim withResults = Await _processRepo.GetCutoffsWithResultsAsync()
            Dim labels = withResults.ToDictionary(Function(c) c.CutoffID,
                                                  Function(c) If(String.IsNullOrWhiteSpace(c.CutoffLabel), c.DisplayLabel, c.CutoffLabel))
            For Each h In sheet.Headers
                Dim label As String = Nothing
                If labels.TryGetValue(h.CutoffID, label) Then h.CutoffLabel = label
            Next

            Return sheet
        End Function

        ' =============================================
        ' EXPORT (Output tab)
        ' =============================================

        Public Async Function BuildExportPlanAsync(cutoffIds As IList(Of Integer), kind As String) As Task(Of PayrollExportPlan) _
            Implements IPayrollProcessService.BuildExportPlanAsync

            Dim withResults = Await _processRepo.GetCutoffsWithResultsAsync()
            Dim cutoffs = withResults.Where(Function(c) cutoffIds.Contains(c.CutoffID)).
                                      OrderBy(Function(c) c.CutoffStart).ThenBy(Function(c) c.CycleType).ToList()
            If cutoffs.Count = 0 Then Throw New PayrollInputValidationException("The selected cutoff has no results yet.")

            Dim payouts = Await _processRepo.GetPayoutRowsAsync(cutoffs.Select(Function(c) c.CutoffID).ToList())
            Return PayrollExportBuilder.BuildPlan(kind, cutoffs, payouts)
        End Function

        ''' <summary>Company Code (Customer ID ng BDO sa Company Bank settings) at mga batch na nagamit na ngayong araw.</summary>
        Public Async Function GetBdoDirectNamingAsync() As Task(Of BdoDirectNaming) _
            Implements IPayrollProcessService.GetBdoDirectNamingAsync

            Dim code = Await _processRepo.GetBdoCompanyCodeAsync()
            If String.IsNullOrWhiteSpace(code) Then
                Throw New PayrollInputValidationException(
                    "The BDO Company Code is not set. Open the Company Bank settings, edit the active BDO bank and fill in its Customer ID " &
                    "(the Company Code used by the BDO Payroll Converter, e.g. IJ9).")
            End If

            Dim naming As New BdoDirectNaming With {.CompanyCode = code.Trim(), .FileDate = Date.Today}
            Dim prefix = naming.FilePrefix()

            Dim names = Await _processRepo.GetBdoDirectFileNamesAsync(prefix)
            For Each n In names
                Dim stem = Path.GetFileNameWithoutExtension(n)
                If stem.Length = prefix.Length + 2 AndAlso stem.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) Then
                    Dim batch As Integer
                    If Integer.TryParse(stem.Substring(prefix.Length), batch) AndAlso Not naming.UsedBatches.Contains(batch) Then
                        naming.UsedBatches.Add(batch)
                    End If
                End If
            Next
            naming.UsedBatches.Sort()

            Return naming
        End Function

        Public Async Function WriteExportAsync(plan As PayrollExportPlan, filePath As String, userName As String) As Task _
            Implements IPayrollProcessService.WriteExportAsync

            ' Ang pagsulat ng file ay sa background para hindi mag-freeze ang screen.
            Await Task.Run(Sub()
                               Select Case plan.Kind
                                   Case ExportFormat.BdoConverter
                                       PayrollExportBuilder.WriteBdoConverter(filePath, plan.Rows)
                                   Case ExportFormat.BdoDirect
                                       PayrollExportBuilder.WriteBdoDirect(filePath, plan.Rows)
                                   Case Else
                                       PayrollExportBuilder.WriteOtherLists(filePath, plan)
                               End Select
                           End Sub)

            ' Log: isang row kada cutoff kada listahan (pareho ang BatchId kapag combined).
            Dim batchId = Guid.NewGuid()
            Dim fileName = Path.GetFileName(filePath)
            Dim entries As New List(Of PayrollExportLogEntry)()

            For Each cutoff In plan.Cutoffs
                Dim cid = cutoff.CutoffID
                Dim groups = plan.Rows.Where(Function(r) r.CutoffID = cid).Select(Function(r) r.ListGroup).Distinct().ToList()
                For Each grp In groups
                    Dim g = grp
                    Dim part = plan.Rows.Where(Function(r) r.CutoffID = cid AndAlso r.ListGroup = g).ToList()
                    entries.Add(New PayrollExportLogEntry With {
                        .CutoffID = cid,
                        .BatchId = batchId,
                        .ListGroup = g,
                        .Format = plan.Kind,
                        .FileName = fileName,
                        .FilePath = filePath,
                        .EmployeeCount = part.Count,
                        .TotalAmount = part.Sum(Function(r) r.NetPay),
                        .IsCombined = plan.IsCombined,
                        .ExportedBy = userName})
                Next
            Next

            Await _processRepo.InsertExportLogAsync(entries)
        End Function

        ' =============================================
        ' POST (FINAL - walang Unpost)
        ' =============================================

        ''' <summary>
        ''' Pwede na bang i-Post? cutoffIds = isang cutoff, o lahat ng cutoff ng pay date (payDate ay may laman kapag "Group by pay date").
        ''' All-or-nothing. Ang mga Posted na ay lalaktawan; ang mga Processed lang ang ipo-post.
        ''' </summary>
        Public Async Function GetPostStateAsync(cutoffIds As IList(Of Integer), payDate As Date?) As Task(Of PayrollPostState) _
            Implements IPayrollProcessService.GetPostStateAsync

            Dim allCutoffs = Await _cutoffRepo.GetAllAsync()
            Dim selected = allCutoffs.Where(Function(c) cutoffIds.Contains(c.CutoffID)).
                                      OrderBy(Function(c) c.CutoffStart).ThenBy(Function(c) c.CycleType).ToList()
            If selected.Count = 0 Then Throw New PayrollInputValidationException("The selected cutoff no longer exists.")

            Dim state As New PayrollPostState()
            If Not AppSession.IsAdmin Then state.BlockReasons.Add("Only an Admin can post.")

            ' Group mode: may cutoff ba ng parehong Pay Date na hindi pa na-process (walang results)?
            If payDate.HasValue Then
                Dim notProcessed = allCutoffs.Where(Function(c) PayrollExportBuilder.EffectivePayDate(c) = payDate.Value.Date AndAlso
                                                                c.Status = CutoffStatus.Draft AndAlso
                                                                Not cutoffIds.Contains(c.CutoffID)).
                                              OrderBy(Function(c) c.CutoffStart).ToList()
                If notProcessed.Count > 0 Then
                    state.BlockReasons.Add("Not processed yet on this pay date: " &
                                           String.Join(", ", notProcessed.Select(Function(c) c.DisplayLabel)) &
                                           ". Process them in the Input tab, or use ""Close this cutoff"" there if they have no employees.")
                End If
            End If

            Dim toPost = selected.Where(Function(c) c.Status = CutoffStatus.Processed).ToList()
            Dim toPostIds = toPost.Select(Function(c) c.CutoffID).ToList()

            For Each c In selected
                If c.Status = CutoffStatus.Posted Then
                    state.AlreadyPosted.Add(c.DisplayLabel)
                ElseIf c.Status <> CutoffStatus.Processed Then
                    state.BlockReasons.Add($"{c.DisplayLabel}: only a Processed cutoff can be posted (this one is {c.Status}).")
                End If
            Next

            If toPost.Count = 0 Then
                state.IsPosted = (state.BlockReasons.Count = 0 AndAlso state.AlreadyPosted.Count > 0)
                state.CanPost = False
                Return state
            End If

            Dim payouts = Await _processRepo.GetPayoutRowsAsync(toPostIds)
            For Each p In payouts
                p.ListGroup = PayrollExportBuilder.Classify(p)
            Next

            For Each c In toPost
                Dim label = c.DisplayLabel
                Dim cid = c.CutoffID

                ' Dapat Posted (o Closed) na ang naunang cutoff(s) ng parehong pay cycle at pay month,
                ' maliban sa mga sabay na ipo-post ngayon.
                If c.PeriodInMonth.HasValue Then
                    Dim cycleKey = PayCycleChoices.Normalize(c.CycleType)
                    Dim monthKey = PayrollComputationRules.PayMonthKey(c)
                    Dim notPosted = allCutoffs.Where(Function(x) x.CutoffID <> cid AndAlso
                                                                 Not toPostIds.Contains(x.CutoffID) AndAlso
                                                                 String.Equals(PayCycleChoices.Normalize(x.CycleType), cycleKey, StringComparison.OrdinalIgnoreCase) AndAlso
                                                                 PayrollComputationRules.PayMonthKey(x) = monthKey AndAlso
                                                                 x.PeriodInMonth.HasValue AndAlso
                                                                 x.PeriodInMonth.Value < c.PeriodInMonth.Value AndAlso
                                                                 (x.Status = CutoffStatus.Draft OrElse x.Status = CutoffStatus.Processed)).
                                              OrderBy(Function(x) x.CutoffStart).ToList()
                    If notPosted.Count > 0 Then
                        state.BlockReasons.Add($"{label}: post the earlier cutoff(s) of this pay month first: " &
                                               String.Join(", ", notPosted.Select(Function(x) x.CutoffLabel)) &
                                               ". (If they are not handled in this app, set them to Closed.)")
                    End If
                End If

                Dim facts = Await _processRepo.GetPostFactsAsync(cid)
                Dim mine = payouts.Where(Function(p) p.CutoffID = cid).ToList()

                Dim item As New PayrollPostItem With {
                    .CutoffID = cid,
                    .CutoffLabel = label,
                    .EmployeeCount = mine.Count,
                    .TotalNet = mine.Sum(Function(p) p.NetPay),
                    .LoanInstallments = facts.LoanInstallments}

                If facts.WarningCount > 0 Then
                    state.BlockReasons.Add($"{label}: {facts.WarningCount} employee(s) still have warnings. Fix them and re-process the cutoff.")
                End If

                Dim unassigned = mine.Where(Function(p) p.ListGroup = PayoutGroup.Unassigned AndAlso p.NetPay > 0D).ToList()
                If unassigned.Count > 0 Then
                    state.BlockReasons.Add($"{label}: {unassigned.Count} employee(s) have no payout method or bank (Employee > Earnings): " &
                                           String.Join(", ", unassigned.Take(5).Select(Function(p) p.EmployeeName)) &
                                           If(unassigned.Count > 5, ", ...", ""))
                End If

                ' Dapat na-export na ang bawat listahang may babayaran (para sa kasalukuyang processing).
                item.RequiredGroups = PayoutGroup.Exportable.
                    Where(Function(g) mine.Any(Function(p) p.ListGroup = g AndAlso p.NetPay > 0D)).ToList()
                Dim missing = item.RequiredGroups.Where(Function(g) Not facts.ValidGroups.Contains(g)).ToList()
                If missing.Count > 0 Then
                    state.BlockReasons.Add($"{label}: not yet exported (or exported before the last Process): " &
                                           String.Join(", ", missing.Select(Function(g) PayoutGroup.DisplayName(g))))
                End If

                state.Items.Add(item)
            Next

            state.CanPost = (state.BlockReasons.Count = 0 AndAlso state.Items.Count > 0)
            Return state
        End Function

        Public Async Function PostAsync(cutoffIds As IList(Of Integer), payDate As Date?, userName As String) As Task(Of PayrollPostResult) _
            Implements IPayrollProcessService.PostAsync

            Dim state = Await GetPostStateAsync(cutoffIds, payDate)
            If state.IsPosted Then Throw New PayrollInputValidationException("Already Posted.")
            If Not state.CanPost Then Throw New PayrollInputValidationException(state.BlockedText())

            Dim requests = state.Items.Select(Function(i) New PayrollPostRequest With {
                .CutoffID = i.CutoffID, .CutoffLabel = i.CutoffLabel, .RequiredGroups = i.RequiredGroups}).ToList()

            Return Await _processRepo.PostCutoffsAsync(requests, userName)
        End Function

    End Class

End Namespace
