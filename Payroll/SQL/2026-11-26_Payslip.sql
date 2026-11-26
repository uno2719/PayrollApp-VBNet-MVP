/* =====================================================================
   PACSPORTS Payroll - PAYSLIP module.

   Ginagawa nito (IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblCompensation.IsPayslipAllowance  - naka-check = allowance. Ito ang mga linyang pwedeng
        i-include / i-exclude sa payslip (option na "Include allowance" sa Reports > Payslip).
     2) tblPayslipEmailLog                  - bawat pagpapadala ng payslip (PDF) sa email ng empleyado,
        pumalya man o hindi. Ito ang "Last emailed" sa grid ng Payslip screen.

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

/* 1) Allowance flag kada compensation code */
IF COL_LENGTH('dbo.tblCompensation', 'IsPayslipAllowance') IS NULL
    ALTER TABLE dbo.tblCompensation
        ADD IsPayslipAllowance BIT NOT NULL
            CONSTRAINT DF_tblCompensation_IsPayslipAllowance DEFAULT (0);
GO

/* 2) Email log */
IF OBJECT_ID('dbo.tblPayslipEmailLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblPayslipEmailLog
    (
        EmailLogId      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblPayslipEmailLog PRIMARY KEY,
        CutoffID        INT               NOT NULL,
        RecordId        INT               NOT NULL,
        EmailAddress    NVARCHAR(200)     NULL,
        IsSuccess       BIT               NOT NULL,
        ErrorMessage    NVARCHAR(1000)    NULL,
        IncludeAllowance BIT              NOT NULL CONSTRAINT DF_tblPayslipEmailLog_IncAllow DEFAULT (1),
        SentBy          NVARCHAR(200)     NULL,
        SentDate        DATETIME2(0)      NOT NULL CONSTRAINT DF_tblPayslipEmailLog_SentDate DEFAULT (SYSDATETIME()),
        CONSTRAINT FK_tblPayslipEmailLog_Cutoff FOREIGN KEY (CutoffID)
            REFERENCES dbo.tblCutoff (CutoffID) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE name = 'IX_tblPayslipEmailLog_Cutoff' AND object_id = OBJECT_ID('dbo.tblPayslipEmailLog'))
    CREATE INDEX IX_tblPayslipEmailLog_Cutoff ON dbo.tblPayslipEmailLog (CutoffID, RecordId, SentDate);
GO

/* Silipin: mga allowance code (si Accounting Head ang magche-check sa Payroll Settings > Compensation) */
SELECT Code, Description, TaxFlag, DeminimisFlag, IsPayslipAllowance, IsActive
FROM dbo.tblCompensation
ORDER BY Code;
GO
