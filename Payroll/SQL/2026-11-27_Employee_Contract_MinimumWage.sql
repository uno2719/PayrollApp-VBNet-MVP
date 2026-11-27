/* =====================================================================
   Employee Phase 2: Contractual (planned contract end, Convert to Probationary) at Minimum Wage / Tax-exempt.

   Ginagawa nito (IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblEmployeeEmployment.ContractEndDate    - PLANADONG katapusan ng kontrata (para sa paalala lang).
                                                   IBA ito sa DateEndOfContract (aktwal na paghiwalay, Actions > End of Contract).
        tblEmployeeEmployment.DateProbationStart - simula ng probation kapag galing sa Contractual (Convert to Probationary).
                                                   Walang laman = Date Joined ang simula (gaya ng dati).
     2) tblGeneralSettings.ContractMonths         - default na haba ng kontrata (3). Contract End = Date Joined + 3 buwan - 1 araw.
     3) Backfill: mga Contractual na walang ContractEndDate at hindi pa umaalis = Date Joined + ContractMonths - 1 araw.
     4) tblMinimumWage                            - minimum wage bawat "Wage Area" at petsa (NCR, CAR, CALABARZON - Component City...).
     5) tblBranch.WageArea                        - aling Wage Area ang sakop ng bawat store/branch.
     6) Seed (non-agriculture):
          NCR                          755.00  (WO NCR-28, 09/26/2026)
          CAR                          505.00  (WO CAR-24, 12/30/2025)  - Baguio
          CALABARZON - City            645.00  (WO IVA-23, 10/26/2026)  - Extended Metro Area + component cities
                                                                          (Antipolo, Binan, Calamba/Canlubang, Tagaytay)
          CALABARZON - 1st Class Town  590.00  (WO IVA-23, 10/26/2026)  - first-class municipalities (Silang, Angono)
        Ang Wage Area ng bawat store ay itatakda sa Settings > Payroll > Minimum Wage (kanang grid).

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

/* 1) Employment columns */
IF COL_LENGTH('dbo.tblEmployeeEmployment', 'ContractEndDate') IS NULL
BEGIN
    ALTER TABLE dbo.tblEmployeeEmployment ADD ContractEndDate DATE NULL;
    PRINT 'OK: idinagdag ang tblEmployeeEmployment.ContractEndDate.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployeeEmployment.ContractEndDate.';
GO

IF COL_LENGTH('dbo.tblEmployeeEmployment', 'DateProbationStart') IS NULL
BEGIN
    ALTER TABLE dbo.tblEmployeeEmployment ADD DateProbationStart DATE NULL;
    PRINT 'OK: idinagdag ang tblEmployeeEmployment.DateProbationStart.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployeeEmployment.DateProbationStart.';
GO

/* 2) General Settings: Contract Period (months) */
IF COL_LENGTH('dbo.tblGeneralSettings', 'ContractMonths') IS NULL
BEGIN
    ALTER TABLE dbo.tblGeneralSettings ADD ContractMonths INT NOT NULL
        CONSTRAINT DF_tblGeneralSettings_ContractMonths DEFAULT (3) WITH VALUES;
    PRINT 'OK: idinagdag ang tblGeneralSettings.ContractMonths (default 3).';
END
ELSE PRINT 'OK: nandoon na ang tblGeneralSettings.ContractMonths.';
GO

/* 3) Backfill ng planadong Contract End ng kasalukuyang Contractual */
DECLARE @months INT = ISNULL((SELECT TOP 1 ContractMonths FROM dbo.tblGeneralSettings ORDER BY GeneralSettingsId), 3);

UPDATE em
SET    em.ContractEndDate = DATEADD(DAY, -1, DATEADD(MONTH, @months, em.DateJoined)),
       em.UpdatedAt = GETDATE(),
       em.UpdatedBy = 'Contract_MWE'
FROM   dbo.tblEmployeeEmployment em
WHERE  LTRIM(RTRIM(ISNULL(em.EmployeeStatus, ''))) = 'Contractual'
  AND  em.ContractEndDate IS NULL
  AND  em.DateJoined IS NOT NULL
  AND  em.DateResigned IS NULL AND em.DateRetired IS NULL AND em.DateTerminated IS NULL AND em.DateEndOfContract IS NULL;

PRINT 'OK: Contract End na-set sa ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' Contractual na empleyado (Date Joined + ' + CAST(@months AS VARCHAR(5)) + ' buwan - 1 araw).';
GO

/* 4) Minimum wage table */
IF OBJECT_ID('dbo.tblMinimumWage', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblMinimumWage
    (
        MinimumWageId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblMinimumWage PRIMARY KEY CLUSTERED,
        WageArea      NVARCHAR(60)   NOT NULL,
        EffectiveDate DATE           NOT NULL,
        DailyRate     DECIMAL(12, 2) NOT NULL,
        WageOrderNo   NVARCHAR(50)   NULL,
        Remarks       NVARCHAR(300)  NULL,
        CreatedAt     DATETIME       NOT NULL CONSTRAINT DF_tblMinimumWage_CreatedAt DEFAULT (GETDATE()),
        CreatedBy     VARCHAR(50)    NULL,
        UpdatedAt     DATETIME       NULL,
        UpdatedBy     VARCHAR(50)    NULL,
        CONSTRAINT UQ_tblMinimumWage_Area_Date UNIQUE (WageArea, EffectiveDate),
        CONSTRAINT CK_tblMinimumWage_DailyRate CHECK (DailyRate > 0)
    );
    PRINT 'OK: ginawa ang tblMinimumWage.';
END
ELSE PRINT 'OK: nandoon na ang tblMinimumWage.';
GO

/* 5) Branch -> Wage Area */
IF OBJECT_ID('dbo.tblBranch', 'U') IS NULL
    PRINT 'SKIPPED: walang tblBranch sa database na ito.'
ELSE IF COL_LENGTH('dbo.tblBranch', 'WageArea') IS NULL
BEGIN
    ALTER TABLE dbo.tblBranch ADD WageArea NVARCHAR(60) NULL;
    PRINT 'OK: idinagdag ang tblBranch.WageArea (i-set sa Settings > Payroll > Minimum Wage).';
END
ELSE PRINT 'OK: nandoon na ang tblBranch.WageArea.';
GO

/* 6) Seed (non-agriculture) */
IF NOT EXISTS (SELECT 1 FROM dbo.tblMinimumWage WHERE WageArea = 'NCR' AND EffectiveDate = '2026-09-26')
    INSERT INTO dbo.tblMinimumWage (WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks, CreatedBy)
    VALUES ('NCR', '2026-09-26', 755.00, 'NCR-28', 'Non-agriculture', 'Contract_MWE');

IF NOT EXISTS (SELECT 1 FROM dbo.tblMinimumWage WHERE WageArea = 'CAR' AND EffectiveDate = '2025-12-30')
    INSERT INTO dbo.tblMinimumWage (WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks, CreatedBy)
    VALUES ('CAR', '2025-12-30', 505.00, 'CAR-24', 'Private sector, region-wide (Baguio included)', 'Contract_MWE');

IF NOT EXISTS (SELECT 1 FROM dbo.tblMinimumWage WHERE WageArea = 'CALABARZON - City' AND EffectiveDate = '2026-10-26')
    INSERT INTO dbo.tblMinimumWage (WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks, CreatedBy)
    VALUES ('CALABARZON - City', '2026-10-26', 645.00, 'IVA-23', 'Non-agriculture: Extended Metro Area, component cities, Rosario (Cavite)', 'Contract_MWE');

IF NOT EXISTS (SELECT 1 FROM dbo.tblMinimumWage WHERE WageArea = 'CALABARZON - 1st Class Town' AND EffectiveDate = '2026-10-26')
    INSERT INTO dbo.tblMinimumWage (WageArea, EffectiveDate, DailyRate, WageOrderNo, Remarks, CreatedBy)
    VALUES ('CALABARZON - 1st Class Town', '2026-10-26', 590.00, 'IVA-23', 'Non-agriculture: other first-class municipalities', 'Contract_MWE');

PRINT 'OK: seed ng NCR, CAR at CALABARZON (City 645 / 1st Class Town 590). I-set ang Wage Area ng bawat store sa Settings > Payroll > Minimum Wage.';
GO
