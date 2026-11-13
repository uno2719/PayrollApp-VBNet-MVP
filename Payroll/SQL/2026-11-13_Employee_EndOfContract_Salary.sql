/* =====================================================================
   Employee: End of Contract, Salary adjustment, Document Categories (Settings).

   Ginagawa nito (lahat IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblEmployeeEmployment.DateEndOfContract   - petsa ng End of Contract (Actions > End of Contract)
     2) tblModules: 'main_Employees_Salary' ("Employee Salary") - permission ng Salary adjustment
        View     = makikita ang halaga ng sahod sa History
        Can Edit = makakapag-adjust ng sahod (Actions > Salary adjustment)
        + ibinibigay sa LAHAT ng user na may "Employee Actions" access na (Admin + HR), kapareho ng antas nila
     3) tblDocumentCategory.UpdatedAt / UpdatedBy - audit ng Settings > Master Data > Document Categories

   WALANG babaguhin sa tblEmployeeMovement: MovementType ay varchar(30) at walang CHECK constraint, kasya ang
   'End of Contract' at 'Salary adjustment'.

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   ===================================================================== */
SET NOCOUNT ON;
GO

/* 1) End of Contract date */
IF OBJECT_ID('dbo.tblEmployeeEmployment', 'U') IS NULL
    PRINT 'SKIPPED: walang tblEmployeeEmployment sa database na ito.'
ELSE IF COL_LENGTH('dbo.tblEmployeeEmployment', 'DateEndOfContract') IS NULL
BEGIN
    ALTER TABLE dbo.tblEmployeeEmployment ADD DateEndOfContract DATE NULL;
    PRINT 'OK: idinagdag ang tblEmployeeEmployment.DateEndOfContract.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployeeEmployment.DateEndOfContract.';
GO

/* 2) Permission: "Employee Salary" */
IF OBJECT_ID('dbo.tblModules', 'U') IS NULL
    PRINT 'SKIPPED: walang tblModules sa database na ito.'
ELSE IF NOT EXISTS (SELECT 1 FROM dbo.tblModules WHERE ModuleCode = 'main_Employees_Salary')
BEGIN
    DECLARE @GroupName NVARCHAR(200), @SortOrder INT;

    SELECT TOP 1 @GroupName = GroupName, @SortOrder = SortOrder
    FROM dbo.tblModules
    WHERE ModuleCode IN ('main_Employees_Actions', 'main_Employees')
    ORDER BY CASE ModuleCode WHEN 'main_Employees_Actions' THEN 0 ELSE 1 END;

    INSERT INTO dbo.tblModules (ModuleCode, ModuleName, GroupName, SortOrder, IsActive, CreatedAt, CreatedBy)
    VALUES ('main_Employees_Salary', 'Employee Salary', ISNULL(@GroupName, 'Main Menu'),
            ISNULL(@SortOrder, 0) + 2, 1, GETDATE(), 'EoC_Salary');

    PRINT 'OK: idinagdag ang module na main_Employees_Salary.';
END
ELSE PRINT 'OK: nandoon na ang module na main_Employees_Salary.';
GO

/* Ibigay sa lahat ng may "Employee Actions" na (Admin + HR) - kapareho ng CanView/CanEdit nila doon.
   Ang mga user na walang Actions access ay HINDI bibigyan. Ayusin sa Admin > Users Account kung kailangan. */
IF OBJECT_ID('dbo.tblUserModuleAccess', 'U') IS NOT NULL
   AND EXISTS (SELECT 1 FROM dbo.tblModules WHERE ModuleCode = 'main_Employees_Salary')
BEGIN
    INSERT INTO dbo.tblUserModuleAccess (UserId, ModuleId, CanView, CanEdit, CreatedAt, CreatedBy)
    SELECT a.UserId, s.RecordId, a.CanView, a.CanEdit, GETDATE(), 'EoC_Salary'
    FROM dbo.tblUserModuleAccess a
    INNER JOIN dbo.tblModules act ON act.RecordId = a.ModuleId AND act.ModuleCode = 'main_Employees_Actions'
    INNER JOIN dbo.tblModules s   ON s.ModuleCode = 'main_Employees_Salary'
    WHERE NOT EXISTS (SELECT 1 FROM dbo.tblUserModuleAccess x
                      WHERE x.UserId = a.UserId AND x.ModuleId = s.RecordId);

    PRINT 'OK: na-grant ang "Employee Salary" sa mga user na may Employee Actions access (' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' row).';
END
GO

/* 3) Document Categories: audit columns */
IF OBJECT_ID('dbo.tblDocumentCategory', 'U') IS NULL
    PRINT 'SKIPPED: walang tblDocumentCategory (patakbuhin muna ang 2026-11-11_Employee_Phase3_Files.sql).'
ELSE
BEGIN
    IF COL_LENGTH('dbo.tblDocumentCategory', 'UpdatedAt') IS NULL
    BEGIN
        ALTER TABLE dbo.tblDocumentCategory ADD UpdatedAt DATETIME NULL;
        PRINT 'OK: idinagdag ang tblDocumentCategory.UpdatedAt.';
    END
    ELSE PRINT 'OK: nandoon na ang tblDocumentCategory.UpdatedAt.';

    IF COL_LENGTH('dbo.tblDocumentCategory', 'UpdatedBy') IS NULL
    BEGIN
        ALTER TABLE dbo.tblDocumentCategory ADD UpdatedBy VARCHAR(50) NULL;
        PRINT 'OK: idinagdag ang tblDocumentCategory.UpdatedBy.';
    END
    ELSE PRINT 'OK: nandoon na ang tblDocumentCategory.UpdatedBy.';
END
GO
