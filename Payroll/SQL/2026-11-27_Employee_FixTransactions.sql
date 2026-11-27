/* =====================================================================
   Employee > Fix Transactions (paulit-ulit na allowance / compensation / bonus / deduction ng bawat empleyado)

   Ginagawa nito (IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblEmployeeFixTxn  - ang mga Fix Transaction ng bawat empleyado
        TxnCategory  : COMPENSATION / BONUS / DEDUCTION (kapareho ng tblPayrollInputTxn.TxnCategory)
                       Ang "Allowance" sa screen = COMPENSATION na IsPayslipAllowance = 1 sa tblCompensation.
        Amount       : peso kung Amount ang Input Unit ng code; dami (days/hours/minutes) kung iba
                       (pareho ng ibig sabihin ng cell sa Payroll Input Entry).
        Start/End    : buwan at taon ng PAY MONTH (buwan ng Pay Date ng cutoff). Walang End = tuloy-tuloy.
        Frequency    : Every Cycle / Monthly / Quarterly / Semi-Annual / Annual
        PayOn        : 1st Cycle / Last Cycle (NULL kapag Every Cycle)
     2) Walang bagong permission: ginagamit ang "Employee Salary" (main_Employees_Salary)
        View = makikita ang tab, Can Edit = Add / Edit / Set Active / Delete.

   Ang result line ng payroll na galing sa Fix Transaction ay may RefId = FixTxnId at Remark = 'Fix Transaction'
   (ligtas: ang mga query ng Loan ay laging may Category = 'LOAN').

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

IF OBJECT_ID('dbo.tblEmployeeFixTxn', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblEmployeeFixTxn
    (
        FixTxnId     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblEmployeeFixTxn PRIMARY KEY CLUSTERED,
        RecordId     INT            NOT NULL,
        TxnCategory  NVARCHAR(30)   NOT NULL,
        TxnCode      NVARCHAR(40)   NOT NULL,
        Amount       DECIMAL(12, 2) NOT NULL,
        StartYear    SMALLINT       NOT NULL,
        StartMonth   TINYINT        NOT NULL,
        EndYear      SMALLINT       NULL,
        EndMonth     TINYINT        NULL,
        Frequency    VARCHAR(20)    NOT NULL CONSTRAINT DF_tblEmployeeFixTxn_Frequency DEFAULT ('Every Cycle'),
        PayOn        VARCHAR(20)    NULL,
        IsActive     BIT            NOT NULL CONSTRAINT DF_tblEmployeeFixTxn_IsActive DEFAULT (1),
        Remarks      NVARCHAR(500)  NULL,
        CreatedAt    DATETIME       NOT NULL CONSTRAINT DF_tblEmployeeFixTxn_CreatedAt DEFAULT (GETDATE()),
        CreatedBy    VARCHAR(50)    NULL,
        UpdatedAt    DATETIME       NULL,
        UpdatedBy    VARCHAR(50)    NULL,

        CONSTRAINT FK_tblEmployeeFixTxn_tblEmployee FOREIGN KEY (RecordId) REFERENCES dbo.tblEmployee (RecordId),
        CONSTRAINT CK_tblEmployeeFixTxn_Category   CHECK (TxnCategory IN ('COMPENSATION', 'BONUS', 'DEDUCTION')),
        CONSTRAINT CK_tblEmployeeFixTxn_Amount     CHECK (Amount > 0),
        CONSTRAINT CK_tblEmployeeFixTxn_StartMonth CHECK (StartMonth BETWEEN 1 AND 12),
        CONSTRAINT CK_tblEmployeeFixTxn_StartYear  CHECK (StartYear BETWEEN 2000 AND 2100),
        CONSTRAINT CK_tblEmployeeFixTxn_End        CHECK ((EndYear IS NULL AND EndMonth IS NULL) OR
                                                          (EndYear IS NOT NULL AND EndMonth BETWEEN 1 AND 12
                                                           AND EndYear * 100 + EndMonth >= StartYear * 100 + StartMonth)),
        CONSTRAINT CK_tblEmployeeFixTxn_Frequency  CHECK (Frequency IN ('Every Cycle', 'Monthly', 'Quarterly', 'Semi-Annual', 'Annual')),
        CONSTRAINT CK_tblEmployeeFixTxn_PayOn      CHECK ((Frequency = 'Every Cycle' AND PayOn IS NULL) OR
                                                          (Frequency <> 'Every Cycle' AND PayOn IN ('1st Cycle', 'Last Cycle')))
    );

    CREATE NONCLUSTERED INDEX IX_tblEmployeeFixTxn_RecordId
        ON dbo.tblEmployeeFixTxn (RecordId, IsActive)
        INCLUDE (TxnCategory, TxnCode, Amount, StartYear, StartMonth, EndYear, EndMonth, Frequency, PayOn);

    PRINT 'OK: ginawa ang tblEmployeeFixTxn.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployeeFixTxn.';
GO

/* Para sa "Used in payroll" check (Delete) at sa pag-trace: result lines na galing sa Fix Transaction. */
IF OBJECT_ID('dbo.tblPayrollResultLine', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tblPayrollResultLine_RefId' AND object_id = OBJECT_ID('dbo.tblPayrollResultLine'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tblPayrollResultLine_RefId ON dbo.tblPayrollResultLine (RefId) INCLUDE (Category, Remark);
    PRINT 'OK: idinagdag ang IX_tblPayrollResultLine_RefId.';
END
GO
