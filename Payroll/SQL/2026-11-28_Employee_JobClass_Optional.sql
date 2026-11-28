/* =====================================================================
   Employee: Job Class ay OPSYONAL na (puwedeng blangko).

   Ginagawa nito (IDEMPOTENT - pwedeng ulit-ulitin, walang binubura):
     - Kung NOT NULL pa ang tblEmployeeEmployment.JobClassId, ginagawa itong NULL (parehong data type).
     - Kung NULL na, walang gagawin.
   Hindi ginagalaw ang tblJobClass at ang mga umiiral nang halaga ng empleyado.

   PATAKBUHIN SA PAREHONG DATABASE: local at main server.
   ===================================================================== */
SET NOCOUNT ON;
GO

IF COL_LENGTH('dbo.tblEmployeeEmployment', 'JobClassId') IS NULL
BEGIN
    RAISERROR('Walang tblEmployeeEmployment.JobClassId. Walang binago.', 16, 1);
END
ELSE IF EXISTS (SELECT 1 FROM sys.columns
                WHERE object_id = OBJECT_ID('dbo.tblEmployeeEmployment') AND name = 'JobClassId' AND is_nullable = 0)
BEGIN
    DECLARE @type sysname = (SELECT TYPE_NAME(user_type_id) FROM sys.columns
                             WHERE object_id = OBJECT_ID('dbo.tblEmployeeEmployment') AND name = 'JobClassId');
    IF @type NOT IN ('int', 'smallint', 'bigint')
        RAISERROR('Hindi inaasahang data type (%s). Walang binago; ipakita ito kay Claude.', 16, 1, @type);
    ELSE
    BEGIN
        DECLARE @sql nvarchar(400) = N'ALTER TABLE dbo.tblEmployeeEmployment ALTER COLUMN JobClassId ' + QUOTENAME(@type) + N' NULL;';
        EXEC (@sql);
        PRINT 'OK: ginawang NULL ang tblEmployeeEmployment.JobClassId.';
    END
END
ELSE
    PRINT 'Walang binago: NULL na ang tblEmployeeEmployment.JobClassId.';
GO

SELECT c.name AS ColumnName, TYPE_NAME(c.user_type_id) AS DataType, c.is_nullable AS IsNullable
FROM sys.columns c
WHERE c.object_id = OBJECT_ID('dbo.tblEmployeeEmployment') AND c.name = 'JobClassId';
GO
