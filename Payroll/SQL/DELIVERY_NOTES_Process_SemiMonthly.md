# Process computation - Delivery 1 (SemiMonthly / Basic Salary)

## Paano i-apply
1. Patakbuhin ang `SQL/2026-10-20_PayrollResults.sql` sa Payroll DB (idempotent, pwedeng ulitin).
2. I-apply ang patch (`git apply process-semimonthly.patch`) o i-extract ang zip sa ibabaw ng repo (kasama ang .Designer.vb).
3. General Settings > Code Mapping: **Basic Salary Code (+)** = ang BASIC compensation code. (Required na ngayon.)
4. Compile, tapos Payroll > Input > Process.

## Bagong / binagong files
Bago: SQL/2026-10-20_PayrollResults.sql, GlobalShared/Models/PayrollResultModels.vb, GlobalShared/Models/PayrollComputationContext.vb,
GlobalShared/Helpers/PayrollComputationRules.vb (engine), Modules/MainMenu/Payroll/Data/IPayrollProcessRepository.vb + PayrollProcessRepository.vb,
Services/IPayrollProcessService.vb + PayrollProcessService.vb, Presenters/PayrollOutputPresenter.vb, Views/IPayrollOutputView.vb
Binago: PayrollInputRepository.vb (EligibilitySql = Friend; BASIC code hindi na Input column), PayrollInputEntryPresenter.vb, IPayrollInputEntryView.vb,
ucPayrollInputEntry.vb, ucPayrollOutput.vb + .Designer.vb, ucPayroll.vb, Shell/AppComposition.vb

## Flow ng Process button
Save inputs -> validations -> compute sa memory -> Confirm (buod + babala) -> isang transaction: burahin ang lumang results ng cutoff, ilagay ang bago, Status = Processed.
Re-process hanggang hindi Posted. Hindi hinahawakan ng Process ang loan schedule (IsPaid/AmountPaid ay sa future "Post").

## Mga hadlang (may malinaw na mensahe)
Closed/Posted na cutoff; walang PeriodInMonth; Daily Rate cycle (Delivery 2); walang General Settings o Basic Salary Code (+);
kulang ang Deduction Schedule; walang laman ang SSS/PhilHealth/Pag-IBIG/tax table; may naunang cutoff sa parehong pay month na Draft pa
(i-process muna, o gawing Closed kung hindi dito pino-process).

## Mga formula
- Basic = Basic Salary / PeriodsPerMonth
- Hours = Qty x HourlyRate; Minutes = Qty x HourlyRate / 60; Days = Qty x DailyRate; Amount = peso mismo. Overtime/Holiday x Rate (multiplier).
- Base (Tax/SSS/PH/PI) = flagged na kita - flagged na manual deduction (late/absent atbp.)
- SSS: lookup base = SSS base ng mga naunang cutoff ng pay month (galing sa naka-save na results) + kasalukuyang cutoff; kinakaltas ayon sa Deduction Schedule
- PhilHealth: lookup base = Fixed Basic (Basic Salary) kung "Fixed Basic" ang setting; kung hindi, Progressive Earnings gaya ng SSS
- Pag-IBIG: table amount; voluntary ang PAPALIT sa EE share (o ayon sa Voluntary Mode sa Deduction Schedule); ER = table
- Tax: TaxGross - EE contributions na kinaltas sa cutoff na ito -> tblIncomeTax{PayCycle}: Fix + % x (taxable - Salary From); 0 kung Minimum Wage
- Net = Gross - manual deductions - EE contributions - tax - loans

## Mga assumption (sabihin kung may babaguhin)
1. Walang proration ng hire/resign sa gitna ng cutoff: full Basic / 2, may babala sa row.
2. Tax = EE contributions ng CUTOFF NA ITO lang ang binabawas (hindi buong buwan).
3. Pag-IBIG bracket lookup base = buwanang Basic (gaya ng PhilHealth Fixed Basic).
4. OT/Holiday "Rate" = multiplier (1.25). Kung percent (125) ang nilalagay mo, sabihin mo.
5. Overdue loan installments (petsa bago ang window ng cutoff) hindi awtomatikong kinakaltas; babala lang. Window = pagkatapos ng PayDate ng naunang cutoff hanggang PayDate nito.
6. SSS MPF (EEMPF/ERMPF) at ECC ay hiwalay na linya; MPF ay kasama sa EE deductions at binabawas sa taxable; ECC employer lang.
7. Late/Absent atbp. ay ordinaryong Deduction codes: ang flags nila ang nagpapababa ng Tax/SSS/PH/PI base. Hindi ginagamit ang Basic Salary Code (-).
8. Walang auto-clamp ng negatibong net (babala lang). Employee FixTax at "Include in payroll" - hindi ginagamit sa Process (eligibility lang ang umiiral).

## Sample na kuwenta (na-test sa engine, para ma-check mo)
Basic 30,000 | Hourly 123.29 | OT1 (x1.25) 4 hrs | Late 30 min | Bonus 1,000 (Tax lang) | SemiMonthly, S2 (SSS 100% sa S2) | SSS bracket EE 1,400 | prior SSS base 15,000
- Basic 15,000.00; OT = 4 x 123.29 x 1.25 = 616.45; Gross = 16,616.45
- Late = 30 x 123.29 / 60 = 61.65 (round half away from zero)
- TaxGross = 15,000 + 616.45 + 1,000 - 61.65 = 16,554.80; SSS base (cutoff) = 15,554.80; month base = 30,554.80
- Taxable = 16,554.80 - 1,400 = 15,154.80; tax (15% over 10,417) = 710.67
- Loan 500.00 -> Total deductions 61.65 + 1,400 + 710.67 + 500 = 2,672.32; Net = 13,944.13
S1 (PhilHealth 100%, Pag-IBIG 100%): PhilHealth 2.5% x 30,000 = 750.00; Pag-IBIG voluntary 500 replaces table 200 (ER 200); taxable 13,750; tax 499.95; net 13,250.05
