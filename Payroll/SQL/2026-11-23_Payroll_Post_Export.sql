/* =====================================================================
   Payroll Output: POST + EXPORT LOG.

   Ginagawa nito (lahat IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblCutoff.PostedBy / PostedDate   - sino at kailan nag-Post (Status = 2 Posted, FINAL: walang Unpost)
     2) tblPayrollExportLog               - bawat export file (BDO / Other Bank / Cash / Cheque) kada cutoff.
        Ang ProcessedStamp ay kopya ng tblCutoff.LastProcessedDate noong nag-export: kapag na-re-process ang
        cutoff pagkatapos, hindi na tugma ang stamp -> hindi na "valid" ang export at kailangang ulitin bago mag-Post.

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

/* 1) Posted by / date */
IF COL_LENGTH('dbo.tblCutoff', 'PostedBy') IS NULL
    ALTER TABLE dbo.tblCutoff ADD PostedBy NVARCHAR(200) NULL;
GO
IF COL_LENGTH('dbo.tblCutoff', 'PostedDate') IS NULL
    ALTER TABLE dbo.tblCutoff ADD PostedDate DATETIME2(7) NULL;
GO

/* 2) Export log */
IF OBJECT_ID('dbo.tblPayrollExportLog', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblPayrollExportLog
    (
        ExportId       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblPayrollExportLog PRIMARY KEY,
        CutoffID       INT               NOT NULL,
        BatchId        UNIQUEIDENTIFIER  NOT NULL,                 -- pareho ang BatchId ng mga cutoff na nasa iisang file (combined)
        ListGroup      VARCHAR(20)       NOT NULL,                 -- BDO | OTHERBANK | CASH | CHEQUE
        Format         VARCHAR(20)       NOT NULL,                 -- BDO_CONVERTER | BDO_DIRECT | OTHER_LISTS
        FileName       NVARCHAR(260)     NULL,
        FilePath       NVARCHAR(500)     NULL,
        EmployeeCount  INT               NOT NULL,
        TotalAmount    DECIMAL(18,2)     NOT NULL,
        IsCombined     BIT               NOT NULL CONSTRAINT DF_tblPayrollExportLog_IsCombined DEFAULT (0),
        ProcessedStamp DATETIME2(0)      NULL,                     -- kopya ng tblCutoff.LastProcessedDate noong nag-export
        ExportedBy     NVARCHAR(200)     NULL,
        ExportedDate   DATETIME2(7)      NOT NULL CONSTRAINT DF_tblPayrollExportLog_ExportedDate DEFAULT (SYSDATETIME()),
        CONSTRAINT FK_tblPayrollExportLog_Cutoff FOREIGN KEY (CutoffID)
            REFERENCES dbo.tblCutoff (CutoffID) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE name = 'IX_tblPayrollExportLog_Cutoff' AND object_id = OBJECT_ID('dbo.tblPayrollExportLog'))
    CREATE INDEX IX_tblPayrollExportLog_Cutoff ON dbo.tblPayrollExportLog (CutoffID, ListGroup, ProcessedStamp);
GO
