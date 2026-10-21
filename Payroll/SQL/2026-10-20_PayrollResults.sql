/* =====================================================================
   PACSPORTS Payroll - Process computation results (Delivery 1)
   IDEMPOTENT: pwedeng patakbuhin nang paulit-ulit, walang mawawala.

   Mga table:
     tblPayrollResult         isang row kada employee kada cutoff (header + snapshot ng rates at bases)
     tblPayrollResultLine     detalye kada linya (para sa payslip / audit)
     tblPayrollResultBracket  snapshot ng bracket na ginamit (SSS, PhilHealth, Pag-IBIG, TAX)
     tblCutoff                +LastProcessedBy, +LastProcessedDate
   ===================================================================== */

/* ---------- tblPayrollResult ---------- */
IF OBJECT_ID(N'dbo.tblPayrollResult', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblPayrollResult
    (
        ResultId            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblPayrollResult PRIMARY KEY,
        CutoffID            INT            NOT NULL,
        RecordId            INT            NOT NULL,

        -- snapshot ng employee
        EmployeeNo          NVARCHAR(50)   NULL,
        EmployeeName        NVARCHAR(200)  NULL,
        PayCycle            NVARCHAR(50)   NULL,
        RateBasis           TINYINT        NOT NULL CONSTRAINT DF_tblPayrollResult_RateBasis DEFAULT (0),
        PeriodInMonth       INT            NULL,
        DaysWorked          DECIMAL(9,2)   NULL,

        -- snapshot ng rates na ginamit
        BasicSalary         DECIMAL(18,2)  NULL,
        DailyRate           DECIMAL(18,4)  NULL,
        HourlyRate          DECIMAL(18,4)  NULL,
        DaysInYear          INT            NULL,
        WorkHourPer         DECIMAL(9,2)   NULL,
        DailyRateMonthlyFactor DECIMAL(9,2) NULL,
        IsMinimumWage       BIT            NOT NULL CONSTRAINT DF_tblPayrollResult_MinWage DEFAULT (0),

        -- kita
        BasicPay            DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_BasicPay DEFAULT (0),
        GrossPay            DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_GrossPay DEFAULT (0),

        -- bases (flagged na kita MINUS flagged na deduction, sa cutoff na ito)
        TaxGross            DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_TaxGross DEFAULT (0),
        SSSBase             DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSBase DEFAULT (0),
        PhilHealthBase      DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PHBase DEFAULT (0),
        PagIbigBase         DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PIBase DEFAULT (0),
        -- ang aktwal na amount na hinanap sa bracket (buwanang basehan)
        SSSLookupBase       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSLookup DEFAULT (0),
        PhilHealthLookupBase DECIMAL(18,2) NOT NULL CONSTRAINT DF_tblPayrollResult_PHLookup DEFAULT (0),
        PagIbigLookupBase   DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PILookup DEFAULT (0),

        -- employee contributions na kinaltas SA CUTOFF NA ITO
        SSSEE               DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSEE DEFAULT (0),
        SSSMPFEE            DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSMPFEE DEFAULT (0),
        PhilHealthEE        DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PHEE DEFAULT (0),
        PagIbigEE           DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PIEE DEFAULT (0),

        -- employer share
        SSSER               DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSER DEFAULT (0),
        SSSMPFER            DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_SSSMPFER DEFAULT (0),
        ECC                 DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_ECC DEFAULT (0),
        PhilHealthER        DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PHER DEFAULT (0),
        PagIbigER           DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_PIER DEFAULT (0),

        -- tax at net
        TaxableIncome       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_Taxable DEFAULT (0),
        WithholdingTax      DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_Tax DEFAULT (0),
        LoanDeduction       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_Loan DEFAULT (0),
        OtherDeduction      DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_Other DEFAULT (0),
        TotalDeduction      DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_TotalDed DEFAULT (0),
        NetPay              DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResult_Net DEFAULT (0),

        HasWarnings         BIT            NOT NULL CONSTRAINT DF_tblPayrollResult_HasWarn DEFAULT (0),
        WarningText         NVARCHAR(1000) NULL,

        ProcessedBy         NVARCHAR(100)  NULL,
        ProcessedDate       DATETIME2(0)   NOT NULL CONSTRAINT DF_tblPayrollResult_ProcDate DEFAULT (SYSDATETIME())
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_tblPayrollResult_Cutoff_Record' AND object_id = OBJECT_ID(N'dbo.tblPayrollResult'))
    CREATE UNIQUE INDEX UX_tblPayrollResult_Cutoff_Record ON dbo.tblPayrollResult (CutoffID, RecordId);
GO

-- FK sa tblCutoff: kung hindi pwede (hal. walang PK/unique ang CutoffID), magpi-print lang ng babala at hindi titigil ang script.
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tblPayrollResult_tblCutoff')
BEGIN
    BEGIN TRY
        ALTER TABLE dbo.tblPayrollResult ADD CONSTRAINT FK_tblPayrollResult_tblCutoff
            FOREIGN KEY (CutoffID) REFERENCES dbo.tblCutoff (CutoffID);
    END TRY
    BEGIN CATCH
        PRINT 'WARNING: hindi nagawa ang FK_tblPayrollResult_tblCutoff - ' + ERROR_MESSAGE();
    END CATCH
END
GO

/* ---------- tblPayrollResultLine ---------- */
IF OBJECT_ID(N'dbo.tblPayrollResultLine', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblPayrollResultLine
    (
        LineId          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblPayrollResultLine PRIMARY KEY,
        ResultId        INT            NOT NULL,
        CutoffID        INT            NOT NULL,
        RecordId        INT            NOT NULL,
        SeqNo           INT            NOT NULL,
        LineType        VARCHAR(10)    NOT NULL,   -- EARNING / DEDUCTION / EMPLOYER
        Category        VARCHAR(20)    NOT NULL,   -- BASIC, OVERTIME, HOLIDAY, COMPENSATION, BONUS, DEDUCTION, SSS, PHILHEALTH, PAGIBIG, TAX, LOAN
        Code            NVARCHAR(50)   NULL,
        Description     NVARCHAR(200)  NULL,
        Qty             DECIMAL(18,4)  NOT NULL CONSTRAINT DF_tblPayrollResultLine_Qty DEFAULT (0),
        Unit            NVARCHAR(20)   NULL,
        RateUsed        DECIMAL(18,4)  NOT NULL CONSTRAINT DF_tblPayrollResultLine_Rate DEFAULT (0),
        Amount          DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResultLine_Amount DEFAULT (0),
        TaxFlag         BIT            NOT NULL CONSTRAINT DF_tblPayrollResultLine_Tax DEFAULT (0),
        SSSFlag         BIT            NOT NULL CONSTRAINT DF_tblPayrollResultLine_SSS DEFAULT (0),
        PhilHealthFlag  BIT            NOT NULL CONSTRAINT DF_tblPayrollResultLine_PH DEFAULT (0),
        PagIbigFlag     BIT            NOT NULL CONSTRAINT DF_tblPayrollResultLine_PI DEFAULT (0),
        RefId           INT            NULL,       -- hal. ScheduleId ng loan installment
        Remark          NVARCHAR(200)  NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tblPayrollResultLine_Result' AND object_id = OBJECT_ID(N'dbo.tblPayrollResultLine'))
    CREATE INDEX IX_tblPayrollResultLine_Result ON dbo.tblPayrollResultLine (ResultId, SeqNo);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tblPayrollResultLine_Cutoff' AND object_id = OBJECT_ID(N'dbo.tblPayrollResultLine'))
    CREATE INDEX IX_tblPayrollResultLine_Cutoff ON dbo.tblPayrollResultLine (CutoffID, RecordId);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tblPayrollResultLine_Result')
    ALTER TABLE dbo.tblPayrollResultLine ADD CONSTRAINT FK_tblPayrollResultLine_Result
        FOREIGN KEY (ResultId) REFERENCES dbo.tblPayrollResult (ResultId) ON DELETE CASCADE;
GO

/* ---------- tblPayrollResultBracket ---------- */
IF OBJECT_ID(N'dbo.tblPayrollResultBracket', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblPayrollResultBracket
    (
        BracketSnapId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblPayrollResultBracket PRIMARY KEY,
        ResultId        INT            NOT NULL,
        Agency          VARCHAR(20)    NOT NULL,   -- SSS / PhilHealth / PagIbig / TAX
        SourceTable     NVARCHAR(60)   NULL,       -- hal. tblStatutorySSS, tblIncomeTaxSemiMonthly
        SourceId        INT            NULL,
        BasisAmount     DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResultBracket_Basis DEFAULT (0),
        SalaryFrom      DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResultBracket_From DEFAULT (0),
        SalaryTo        DECIMAL(18,2)  NOT NULL CONSTRAINT DF_tblPayrollResultBracket_To DEFAULT (0),
        EEShare         DECIMAL(18,4)  NULL,
        EEContriType    NVARCHAR(20)   NULL,
        ERShare         DECIMAL(18,4)  NULL,
        ERContriType    NVARCHAR(20)   NULL,
        ECCAmount       DECIMAL(18,2)  NULL,
        EEMPF           DECIMAL(18,2)  NULL,
        ERMPF           DECIMAL(18,2)  NULL,
        TaxPercentage   DECIMAL(9,4)   NULL,
        FixTaxAmount    DECIMAL(18,2)  NULL,
        MonthlyEE       DECIMAL(18,2)  NULL,       -- buwanang halaga bago hatiin sa Deduction Schedule
        MonthlyER       DECIMAL(18,2)  NULL,
        SharePercent    DECIMAL(9,4)   NULL        -- share % ng period na ito sa Deduction Schedule
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tblPayrollResultBracket_Result' AND object_id = OBJECT_ID(N'dbo.tblPayrollResultBracket'))
    CREATE INDEX IX_tblPayrollResultBracket_Result ON dbo.tblPayrollResultBracket (ResultId);
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tblPayrollResultBracket_Result')
    ALTER TABLE dbo.tblPayrollResultBracket ADD CONSTRAINT FK_tblPayrollResultBracket_Result
        FOREIGN KEY (ResultId) REFERENCES dbo.tblPayrollResult (ResultId) ON DELETE CASCADE;
GO

/* ---------- tblCutoff: kailan at sino ang huling nag-process ---------- */
IF COL_LENGTH(N'dbo.tblCutoff', N'LastProcessedBy') IS NULL
    ALTER TABLE dbo.tblCutoff ADD LastProcessedBy NVARCHAR(100) NULL;
GO

IF COL_LENGTH(N'dbo.tblCutoff', N'LastProcessedDate') IS NULL
    ALTER TABLE dbo.tblCutoff ADD LastProcessedDate DATETIME2(0) NULL;
GO
