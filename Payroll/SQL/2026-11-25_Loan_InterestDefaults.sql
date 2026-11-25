/* ============================================================
   2026-11-25_Loan_InterestDefaults.sql
   ------------------------------------------------------------
   Payroll Settings > Loan: default na interes kada Loan Code.
     tblLoan.DefaultInterestMethod  'Straight Line' | 'Diminishing' | NULL (walang default)
     tblLoan.DefaultInterestRate    hal. 8.0000
     tblLoan.DefaultRatePeriod      'Per Year' | 'Per Period' (Diminishing lang) | NULL

   Kinokopya ito sa BAGONG employee loan pagpili ng Loan Code (editable pa rin doon).
   Hindi nito ginagalaw ang mga naka-save nang loan.

   Ligtas patakbuhin nang ilang beses. Patakbuhin sa LOCAL at sa SERVER bago i-build.
   ============================================================ */
SET NOCOUNT ON;

IF COL_LENGTH('dbo.tblLoan', 'DefaultInterestMethod') IS NULL
    ALTER TABLE dbo.tblLoan ADD DefaultInterestMethod VARCHAR(20) NULL;

IF COL_LENGTH('dbo.tblLoan', 'DefaultInterestRate') IS NULL
    ALTER TABLE dbo.tblLoan ADD DefaultInterestRate DECIMAL(9,4) NULL;

IF COL_LENGTH('dbo.tblLoan', 'DefaultRatePeriod') IS NULL
    ALTER TABLE dbo.tblLoan ADD DefaultRatePeriod VARCHAR(20) NULL;
GO

/* ---- OPTIONAL: SSS Salary Loan = Diminishing, 8% per year ----
   Galing sa SSS statement na ipinakita mo (40,000 / 24 x 1,809.09). I-check muna ang Code bago patakbuhin.
   Mas mabuting itakda na lang sa Payroll Settings > Loan; ang mga SSS/PAG-IBIG code na walang default ay
   hihingan ng value pagka-Edit doon.

UPDATE dbo.tblLoan
SET    DefaultInterestMethod = 'Diminishing', DefaultInterestRate = 8, DefaultRatePeriod = 'Per Year',
       UpdatedAt = GETDATE(), UpdatedBy = 'sql-script'
WHERE  Code = 'PALITAN'          -- Code ng SSS Salary Loan
  AND  DefaultInterestMethod IS NULL;
*/

/* ---- CHECK ---- */
SELECT LoanId, Code, Description, LoanType, DefaultInterestMethod, DefaultInterestRate, DefaultRatePeriod
FROM   dbo.tblLoan
ORDER BY Code;
