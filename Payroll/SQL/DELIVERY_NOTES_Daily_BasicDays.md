# Delivery 2 - Daily Rate + BASIC bilang Days Worked

## Paano i-apply (nakapatong sa Delivery 1)
1. General Settings > Code Mapping: dapat naka-set ang **Basic Salary Code (+)** = BASIC, at ang BASIC (Payroll Settings > Compensation) ay **Input unit = Days**.
2. Patakbuhin ang `SQL/2026-10-21_DaysWorked_To_Basic.sql` (idempotent): inililipat ang mga lumang DAYSWORKED (CORE) sa BASIC (COMPENSATION).
3. I-apply ang `process-daily-delta.patch` (o i-extract ang zip sa ibabaw ng repo). Compile.

## Ano ang nagbago
- Tinanggal ang hiwalay na "Days Worked" column. Sa Daily Rate cutoff, ang BASIC ang unang column (laging nakikita, Days, pwedeng decimal, hindi lalampas sa araw ng cutoff).
- Sa Basic Salary cutoff (hal. SemiMonthly), walang BASIC column; kung may natirang BASIC entry, hindi ito bibilangin at may babala sa Output.
- Process para sa Daily Rate basis:
  - Basic = Daily Rate x Days Worked (line: Code BASIC, Qty = days, Rate = Daily Rate, flags galing sa BASIC)
  - PhilHealth "Fixed Basic" = Daily Rate x Daily Rate Monthly Factor (General Settings, 30); Pag-IBIG lookup gamit din ito
  - SSS base = lahat ng SSS-flagged na kita ng buong pay month (D1 + D2), gaya ng SemiMonthly
  - Tax (pay cycle "Daily"): taxable per day = (Tax gross - EE contributions ng cutoff) / Days Worked -> bracket sa tblIncomeTaxDaily -> tax per day x Days Worked
  - Walang Days Worked = basic 0, tax 0, may babala sa row
- May hadlang na ngayon sa Process: Daily Rate pero hindi Days ang Input unit ng BASIC.

## Assumption para sa Accounting head (isa lang ang bago)
Daily tax sa 15-araw na cutoff: `(tax kada araw mula sa tblIncomeTaxDaily ng average na taxable kada araw) x Days Worked`.
Ang per-day na tax ay nire-round sa 2 decimals bago i-multiply sa Days Worked. Nasa isang bloke lang ng `PayrollComputationRules.ComputeEmployee` (hanapin ang "perDayTax") kaya madaling palitan.
Lahat ng assumption ng Delivery 1 (proration, EE contributions ng cutoff, Pag-IBIG base, OT Rate multiplier, atbp.) ay nananatili.

## Sample (na-test sa engine)
Daily 800/day, Hourly 100, 12 araw, OT 2 hrs x1.25, PhilHealth 2.5%, Pag-IBIG table 200, D1 (15th):
- Basic 9,600.00; OT 250.00; Gross 9,850.00
- PhilHealth base 24,000 -> EE 600.00; Pag-IBIG EE 200.00; Taxable 9,050.00
- Taxable/day = 754.1667 -> daily bracket (686-1,095.99, 20%): (754.1667 - 686) x 20% = 13.63/day -> x 12 = 163.56
- Net = 9,850 - 800 - 163.56 = 8,886.44
