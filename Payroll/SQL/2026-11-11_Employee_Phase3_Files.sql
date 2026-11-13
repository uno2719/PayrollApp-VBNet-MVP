/* =====================================================================
   Employee Phase 3: 201 Files, Employee Photo, Profile completeness.

   Ginagawa nito (lahat IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     1) tblGeneralSettings.FileStorageRoot   - shared folder path (ise-set sa Settings > General Settings)
     2) tblEmployee.PhotoPath                - relative path ng photo
     3) tblDocumentCategory                  - mga category ng 201 file (+ seed; IsRequired = para sa completeness)
     4) tblEmployeeDocument                  - isang row kada file (soft delete)
     5) tblModules: 'main_Employees_Documents' ("Employee Documents") - permission

   PATAKBUHIN SA PAREHONG DATABASE: local at main server, BAGO patakbuhin ang bagong build.
   Pagkatapos: i-grant ang "Employee Documents" (Can Edit) sa HR sa Admin > Users Account,
   at i-set ang Files Folder sa Settings > General Settings.
   ===================================================================== */
SET NOCOUNT ON;
GO

/* 1) Files Folder setting */
IF OBJECT_ID('dbo.tblGeneralSettings', 'U') IS NULL
    PRINT 'SKIPPED: walang tblGeneralSettings sa database na ito.'
ELSE IF COL_LENGTH('dbo.tblGeneralSettings', 'FileStorageRoot') IS NULL
BEGIN
    ALTER TABLE dbo.tblGeneralSettings ADD FileStorageRoot NVARCHAR(500) NULL;
    PRINT 'OK: idinagdag ang tblGeneralSettings.FileStorageRoot.';
END
ELSE PRINT 'OK: nandoon na ang tblGeneralSettings.FileStorageRoot.';
GO

/* 2) Photo */
IF COL_LENGTH('dbo.tblEmployee', 'PhotoPath') IS NULL
BEGIN
    ALTER TABLE dbo.tblEmployee ADD PhotoPath NVARCHAR(300) NULL;
    PRINT 'OK: idinagdag ang tblEmployee.PhotoPath.';
END
ELSE PRINT 'OK: nandoon na ang tblEmployee.PhotoPath.';
GO

/* 3) Mga category ng 201 file.
      IsRequired = 1 -> binibilang sa "Profile %" ng Employee header kapag wala pang file sa category.
      Ang mga default na required ay HULA (para sa HR na kumpirmahin); palitan ng:
          UPDATE dbo.tblDocumentCategory SET IsRequired = 0 WHERE CategoryName = 'Medical';
      Ang IsActive = 0 ay nagtatago ng category sa picker (ang mga lumang file ay nananatili). */
IF OBJECT_ID('dbo.tblDocumentCategory', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblDocumentCategory
    (
        CategoryId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblDocumentCategory PRIMARY KEY,
        CategoryName NVARCHAR(100) NOT NULL CONSTRAINT UQ_tblDocumentCategory_Name UNIQUE,
        IsRequired   BIT NOT NULL CONSTRAINT DF_tblDocumentCategory_IsRequired DEFAULT (0),
        SortOrder    INT NOT NULL CONSTRAINT DF_tblDocumentCategory_SortOrder DEFAULT (0),
        IsActive     BIT NOT NULL CONSTRAINT DF_tblDocumentCategory_IsActive DEFAULT (1),
        CreatedAt    DATETIME NOT NULL CONSTRAINT DF_tblDocumentCategory_CreatedAt DEFAULT (GETDATE()),
        CreatedBy    VARCHAR(50) NULL
    );
    PRINT 'OK: ginawa ang tblDocumentCategory.';
END
GO

IF OBJECT_ID('dbo.tblDocumentCategory', 'U') IS NOT NULL
BEGIN
    DECLARE @seed TABLE (CategoryName NVARCHAR(100), IsRequired BIT, SortOrder INT);
    INSERT INTO @seed VALUES
        (N'Resume',                  1, 10),
        (N'Application Form',        1, 20),
        (N'Government IDs',          1, 30),
        (N'NBI/Police Clearance',    1, 40),
        (N'Medical',                 1, 50),
        (N'Contract',                1, 60),
        (N'Certificates',            0, 70),
        (N'Memo/NTE',                0, 80),
        (N'Performance Evaluation',  0, 90),
        (N'Other',                   0, 100);

    INSERT INTO dbo.tblDocumentCategory (CategoryName, IsRequired, SortOrder, CreatedBy)
    SELECT s.CategoryName, s.IsRequired, s.SortOrder, 'Phase3'
    FROM @seed s
    WHERE NOT EXISTS (SELECT 1 FROM dbo.tblDocumentCategory c WHERE c.CategoryName = s.CategoryName);

    PRINT 'OK: na-seed ang mga default na category (mga wala pa lang).';
END
GO

/* 4) Mga file ng empleyado (soft delete: IsDeleted; ang aktwal na file ay nananatili sa folder) */
IF OBJECT_ID('dbo.tblEmployeeDocument', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblEmployeeDocument
    (
        DocumentId    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_tblEmployeeDocument PRIMARY KEY,
        RecordId      INT NOT NULL,                       -- tblEmployee.RecordId
        CategoryId    INT NOT NULL,                       -- tblDocumentCategory.CategoryId
        FileName      NVARCHAR(260) NOT NULL,             -- orihinal na pangalan ng file
        StoredPath    NVARCHAR(400) NOT NULL,             -- RELATIVE sa Files Folder
        FileSizeBytes BIGINT NOT NULL CONSTRAINT DF_tblEmployeeDocument_Size DEFAULT (0),
        Remarks       NVARCHAR(500) NULL,
        IsDeleted     BIT NOT NULL CONSTRAINT DF_tblEmployeeDocument_IsDeleted DEFAULT (0),
        DeletedAt     DATETIME NULL,
        DeletedBy     VARCHAR(50) NULL,
        CreatedAt     DATETIME NOT NULL CONSTRAINT DF_tblEmployeeDocument_CreatedAt DEFAULT (GETDATE()),
        CreatedBy     VARCHAR(50) NULL
    );
    PRINT 'OK: ginawa ang tblEmployeeDocument.';
END
GO

IF OBJECT_ID('dbo.tblEmployeeDocument', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes
                   WHERE name = 'IX_tblEmployeeDocument_RecordId'
                     AND object_id = OBJECT_ID('dbo.tblEmployeeDocument'))
BEGIN
    CREATE INDEX IX_tblEmployeeDocument_RecordId ON dbo.tblEmployeeDocument (RecordId, IsDeleted, CategoryId);
    PRINT 'OK: ginawa ang index na IX_tblEmployeeDocument_RecordId.';
END
GO

IF OBJECT_ID('dbo.tblEmployeeDocument', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_tblEmployeeDocument_tblEmployee')
BEGIN
    BEGIN TRY
        ALTER TABLE dbo.tblEmployeeDocument ADD CONSTRAINT FK_tblEmployeeDocument_tblEmployee
            FOREIGN KEY (RecordId) REFERENCES dbo.tblEmployee (RecordId);
    END TRY
    BEGIN CATCH
        PRINT 'SKIPPED (foreign key sa tblEmployee): ' + ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('dbo.tblEmployeeDocument', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_tblEmployeeDocument_tblDocumentCategory')
BEGIN
    BEGIN TRY
        ALTER TABLE dbo.tblEmployeeDocument ADD CONSTRAINT FK_tblEmployeeDocument_tblDocumentCategory
            FOREIGN KEY (CategoryId) REFERENCES dbo.tblDocumentCategory (CategoryId);
    END TRY
    BEGIN CATCH
        PRINT 'SKIPPED (foreign key sa tblDocumentCategory): ' + ERROR_MESSAGE();
    END CATCH
END
GO

/* 5) Permission: "Employee Documents" (Admin laging may access; ang HR ay bibigyan ng Can Edit sa Users Account).
      Kinokopya ang GroupName/SortOrder ng 'main_Employees_Actions' (o ng 'main_Employees' kung wala). */
IF OBJECT_ID('dbo.tblModules', 'U') IS NULL
    PRINT 'SKIPPED: walang tblModules sa database na ito.'
ELSE IF NOT EXISTS (SELECT 1 FROM dbo.tblModules WHERE ModuleCode = 'main_Employees_Documents')
BEGIN
    DECLARE @GroupName NVARCHAR(200), @SortOrder INT;

    SELECT TOP 1 @GroupName = GroupName, @SortOrder = SortOrder
    FROM dbo.tblModules
    WHERE ModuleCode IN ('main_Employees_Actions', 'main_Employees')
    ORDER BY CASE ModuleCode WHEN 'main_Employees_Actions' THEN 0 ELSE 1 END;

    INSERT INTO dbo.tblModules (ModuleCode, ModuleName, GroupName, SortOrder, IsActive, CreatedAt, CreatedBy)
    VALUES ('main_Employees_Documents', 'Employee Documents', ISNULL(@GroupName, 'Main Menu'),
            ISNULL(@SortOrder, 0) + 1, 1, GETDATE(), 'Phase3');

    PRINT 'OK: idinagdag ang module na main_Employees_Documents.';
END
ELSE PRINT 'OK: nandoon na ang module na main_Employees_Documents.';
GO

/* ---------------------------------------------------------------------
   OPSYONAL (naka-comment): ibigay ang "Employee Documents" sa LAHAT ng user na may
   "Employee Actions" access na (Admin + HR), na kapareho ng antas nila doon.
   Alisin ang comment kung ito ang gusto mo; kung hindi, itakda ito nang isa-isa
   sa Admin > Users Account.

   INSERT INTO dbo.tblUserModuleAccess (UserId, ModuleId, CanView, CanEdit, CreatedAt, CreatedBy)
   SELECT a.UserId, d.RecordId, a.CanView, a.CanEdit, GETDATE(), 'Phase3'
   FROM dbo.tblUserModuleAccess a
   INNER JOIN dbo.tblModules act ON act.RecordId = a.ModuleId AND act.ModuleCode = 'main_Employees_Actions'
   INNER JOIN dbo.tblModules d   ON d.ModuleCode = 'main_Employees_Documents'
   WHERE NOT EXISTS (SELECT 1 FROM dbo.tblUserModuleAccess x
                     WHERE x.UserId = a.UserId AND x.ModuleId = d.RecordId);
   --------------------------------------------------------------------- */
