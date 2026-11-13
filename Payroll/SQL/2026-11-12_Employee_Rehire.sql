/* =====================================================================
   Employee Rehire: bagong column para sa UNANG Date Joined.

   Kapag nag-Rehire (Actions > Rehire): ang rehire date ang magiging bagong DateJoined (batayan ng
   retirement), ang dating DateJoined ay itatabi dito (OriginalDateJoined) sa unang rehire lang, at
   mabubura ang mga petsa ng pag-alis sa employment row (nananatili ang mga ito sa History).

   IDEMPOTENT. PATAKBUHIN SA PAREHONG DATABASE (local at main server) BAGO ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

IF OBJECT_ID('dbo.tblEmployeeEmployment', 'U') IS NULL
    PRINT 'SKIPPED: walang tblEmployeeEmployment sa database na ito.'
ELSE IF COL_LENGTH('dbo.tblEmployeeEmployment', 'OriginalDateJoined') IS NULL
BEGIN
    ALTER TABLE dbo.tblEmployeeEmployment ADD OriginalDateJoined DATE NULL;
    PRINT 'OK: idinagdag ang tblEmployeeEmployment.OriginalDateJoined.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployeeEmployment.OriginalDateJoined.';
GO
