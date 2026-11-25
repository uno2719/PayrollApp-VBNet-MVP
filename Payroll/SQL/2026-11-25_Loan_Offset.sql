/* ============================================================
   2026-11-25_Loan_Offset.sql
   ------------------------------------------------------------
   Loan engine fix + Loan Offset (renewal):
     tblEmployeeLoan
       RatePeriod        'Per Period' (dati) | 'Per Year'      (Diminishing lang)
       AmortBasis        'Per Deduction' (dati) | 'Per Month'  (Every Cycle lang)
       OffsetByLoanId    sa LUMANG loan: ang bagong loan na pumalit (Status = 'Offset')
       OffsetDate        sa LUMANG loan: kailan na-offset
       OffsetCredit      sa BAGONG loan: credit ng empleyado mula sa offset
       OffsetCreditMode  sa BAGONG loan: 'Refund' | 'First' | 'Last'
       OffsetRefundTxnId sa BAGONG loan: ang refund row sa tblPayrollInputTxn (kung Refund)
     tblGeneralSettings
       LoanRefundCodeId  Compensation na gagamitin sa refund (Code Mapping)

   Ligtas patakbuhin nang ilang beses (IF ... IS NULL). Patakbuhin sa LOCAL at sa SERVER bago i-build.
   Ang mga lumang loan ay nagiging 'Per Period' / 'Per Deduction' (dating behavior), kaya walang nagbabago sa kanila.
   ============================================================ */
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRANSACTION;

IF COL_LENGTH('dbo.tblEmployeeLoan', 'RatePeriod') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan
        ADD RatePeriod VARCHAR(20) NOT NULL
            CONSTRAINT DF_tblEmployeeLoan_RatePeriod DEFAULT ('Per Period');

IF COL_LENGTH('dbo.tblEmployeeLoan', 'AmortBasis') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan
        ADD AmortBasis VARCHAR(20) NOT NULL
            CONSTRAINT DF_tblEmployeeLoan_AmortBasis DEFAULT ('Per Deduction');

IF COL_LENGTH('dbo.tblEmployeeLoan', 'OffsetByLoanId') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan ADD OffsetByLoanId INT NULL;

IF COL_LENGTH('dbo.tblEmployeeLoan', 'OffsetDate') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan ADD OffsetDate DATE NULL;

IF COL_LENGTH('dbo.tblEmployeeLoan', 'OffsetCredit') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan
        ADD OffsetCredit DECIMAL(18,2) NOT NULL
            CONSTRAINT DF_tblEmployeeLoan_OffsetCredit DEFAULT (0);

IF COL_LENGTH('dbo.tblEmployeeLoan', 'OffsetCreditMode') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan ADD OffsetCreditMode VARCHAR(10) NULL;

IF COL_LENGTH('dbo.tblEmployeeLoan', 'OffsetRefundTxnId') IS NULL
    ALTER TABLE dbo.tblEmployeeLoan ADD OffsetRefundTxnId INT NULL;

IF COL_LENGTH('dbo.tblGeneralSettings', 'LoanRefundCodeId') IS NULL
    ALTER TABLE dbo.tblGeneralSettings ADD LoanRefundCodeId INT NULL;

COMMIT TRANSACTION;
GO

/* Self-FK: hindi mabubura ang bagong loan habang may lumang loan na naka-turo dito.
   (Ang app ay nag-a-Undo muna ng offset bago mag-Delete, kaya sa app ay hindi ito tatama.) */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_tblEmployeeLoan_OffsetByLoan')
    ALTER TABLE dbo.tblEmployeeLoan
        ADD CONSTRAINT FK_tblEmployeeLoan_OffsetByLoan
        FOREIGN KEY (OffsetByLoanId) REFERENCES dbo.tblEmployeeLoan (EmployeeLoanId);
GO

/* Para sa "ilang loan ang na-offset ng loan na ito" (OffsetChildCount) sa Loans list. */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblEmployeeLoan_OffsetByLoanId'
                                           AND object_id = OBJECT_ID('dbo.tblEmployeeLoan'))
    CREATE NONCLUSTERED INDEX IX_tblEmployeeLoan_OffsetByLoanId
        ON dbo.tblEmployeeLoan (OffsetByLoanId)
        WHERE OffsetByLoanId IS NOT NULL;
GO

/* ---- CHECK ---- */
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, COLUMN_DEFAULT
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  (TABLE_NAME = 'tblEmployeeLoan'
        AND COLUMN_NAME IN ('RatePeriod','AmortBasis','OffsetByLoanId','OffsetDate','OffsetCredit','OffsetCreditMode','OffsetRefundTxnId'))
   OR  (TABLE_NAME = 'tblGeneralSettings' AND COLUMN_NAME = 'LoanRefundCodeId')
ORDER BY TABLE_NAME, COLUMN_NAME;
-- Dapat 8 rows.
