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

    End Class

End Namespace
