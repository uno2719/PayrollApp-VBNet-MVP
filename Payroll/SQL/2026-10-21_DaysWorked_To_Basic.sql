/* =====================================================================
   Delivery 2: ang "Days Worked" ng Daily Rate cutoff ay ang BASIC compensation code na (Basic Salary Code +).
   Inililipat nito ang mga LUMANG naka-save na Days Worked (TxnCategory CORE / TxnCode DAYSWORKED)
   sa BASIC (TxnCategory COMPENSATION / TxnCode = code na naka-map sa General Settings).

   IDEMPOTENT: pwedeng ulitin; kung wala nang lumang DAYSWORKED, walang mababago.
   BAGO PATAKBUHIN: i-set muna ang "Basic Salary Code (+)" sa General Settings.
   ===================================================================== */
SET NOCOUNT ON;

DECLARE @BasicCode NVARCHAR(50);

SELECT TOP 1 @BasicCode = c.Code
FROM dbo.tblGeneralSettings g
     INNER JOIN dbo.tblCompensation c ON c.CompensationId = g.BasicSalaryCodePlusId;

IF @BasicCode IS NULL
BEGIN
    PRINT 'Hindi pa naka-set ang Basic Salary Code (+) sa General Settings - walang inilipat. I-set muna, tapos patakbuhin ulit.';
END
ELSE
BEGIN
    BEGIN TRANSACTION;

    -- 1. Ilipat ang DAYSWORKED sa BASIC (Qty = araw, Rate = 1 - pareho ang hugis)
    UPDATE t
    SET TxnCategory = 'COMPENSATION', TxnCode = @BasicCode
    FROM dbo.tblPayrollInputTxn t
    WHERE t.TxnCategory = 'CORE' AND t.TxnCode = 'DAYSWORKED'
      AND NOT EXISTS (SELECT 1 FROM dbo.tblPayrollInputTxn x
                      WHERE x.CutoffID = t.CutoffID AND x.RecordId = t.RecordId AND x.TxnSrc = t.TxnSrc
                        AND x.TxnCategory = 'COMPENSATION' AND x.TxnCode = @BasicCode);

    PRINT 'Nailipat na DAYSWORKED -> ' + @BasicCode + ': ' + CAST(@@ROWCOUNT AS VARCHAR(20)) + ' row(s).';

    -- 2. Kung may BASIC na sa parehong cutoff/empleyado (hindi dapat mangyari), ang natirang DAYSWORKED ay tinatanggal
    DELETE FROM dbo.tblPayrollInputTxn
    WHERE TxnCategory = 'CORE' AND TxnCode = 'DAYSWORKED';

    COMMIT TRANSACTION;
END
GO
