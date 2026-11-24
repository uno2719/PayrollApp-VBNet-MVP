/* =====================================================================
   TEST ONLY (v2) - HUWAG GAMITIN SA PRODUCTION / SA TOTOONG PAYROLL.
   Ibinabalik ang isa o higit pang Posted na cutoff sa Processed (kabaligtaran ng Post sa app).
   Walang Unpost sa app dahil sa desisyon ng accounting head; ito ay para sa pagte-test mo lang.

   PAANO GAMITIN
     1) Ilagay sa @Cutoffs ang CutoffID (isa o higit pa). Kapag "Post Pay Date" ang ginamit mo, ILAGAY LAHAT ng cutoff ng pay date.
     2) Patakbuhin nang buo. Default ay PREVIEW (@Commit = 0): ipapakita ang BAGO at PAGKATAPOS, tapos ROLLBACK.
     3) Kung ayos ang preview, gawing @Commit = 1 at patakbuhin ulit.
     4) Pagkatapos, isara at buksan ulit ang Loans screen sa app (naka-cache ang grid hangga't hindi nire-reload).

   ANO ANG IBINABALIK
     - tblCutoff: Status = 1 (Processed), PostedBy/PostedDate = NULL
     - Loan installments na may PayrollRefNo = 'PR-{CutoffID}' (galing sa Post): AmountPaid -= kinaltas, IsPaid/PaidDate/PayrollRefNo ayon doon
     - Loan na naging Completed dahil sa Post (may hindi na bayad na installment ulit) -> Active
   HINDI ginagalaw: tblPayrollExportLog.

   MAHALAGA: ang mga installment na MAY ibang PayrollRefNo (hal. ni-record mo mismo sa Loans screen) ay HINDI ginagalaw nito.
   Lalabas iyon sa "MatchesPostRef = 0" sa diagnostic sa baba.
   ===================================================================== */
SET NOCOUNT ON;

DECLARE @Commit BIT = 0;       -- 0 = preview lang (ROLLBACK), 1 = i-commit

DECLARE @Cutoffs TABLE (CutoffID INT PRIMARY KEY);
INSERT INTO @Cutoffs (CutoffID) VALUES (0);         -- <== ILAGAY DITO, hal. VALUES (27), (28), (29)

BEGIN TRY
    IF EXISTS (SELECT 1 FROM @Cutoffs WHERE CutoffID = 0)
        RAISERROR('Ilagay muna ang totoong CutoffID sa @Cutoffs (sa taas).', 16, 1);

    BEGIN TRAN;

    IF EXISTS (SELECT 1 FROM @Cutoffs x WHERE NOT EXISTS (SELECT 1 FROM dbo.tblCutoff c WHERE c.CutoffID = x.CutoffID))
        RAISERROR('May CutoffID sa listahan na wala sa tblCutoff.', 16, 1);

    IF EXISTS (SELECT 1 FROM @Cutoffs x INNER JOIN dbo.tblCutoff c ON c.CutoffID = x.CutoffID WHERE c.Status <> 2)
        RAISERROR('May cutoff sa listahan na hindi Posted (Status <> 2). Walang gagawin.', 16, 1);

    IF EXISTS (SELECT 1
               FROM @Cutoffs x
                    INNER JOIN dbo.tblCutoff c ON c.CutoffID = x.CutoffID
                    INNER JOIN dbo.tblCutoff n ON n.CycleType = c.CycleType AND n.Status = 2 AND n.CutoffStart > c.CutoffStart
               WHERE n.CutoffID NOT IN (SELECT CutoffID FROM @Cutoffs))
        RAISERROR('May mas bagong Posted na cutoff ng parehong pay cycle na wala sa listahan. Isama o i-unpost muna iyon.', 16, 1);

    /* ---------- BAGO: lahat ng LOAN line ng mga cutoff na ito at ang kalagayan ng installment ---------- */
    SELECT 'BAGO' AS Stage, l.CutoffID, r.EmployeeName, s.ScheduleId, l.Amount AS LineAmount, s.AmountDue, s.AmountPaid, s.IsPaid,
           s.PayrollRefNo,
           CASE WHEN s.PayrollRefNo = 'PR-' + CAST(l.CutoffID AS VARCHAR(20)) THEN 1 ELSE 0 END AS MatchesPostRef
    FROM dbo.tblPayrollResultLine l
         INNER JOIN @Cutoffs x ON x.CutoffID = l.CutoffID
         INNER JOIN dbo.tblPayrollResult r ON r.ResultId = l.ResultId
         LEFT JOIN dbo.tblEmployeeLoanSchedule s ON s.ScheduleId = l.RefId
    WHERE l.Category = 'LOAN'
    ORDER BY l.CutoffID, r.EmployeeName;

    /* ---------- 1) Loan installments ---------- */
    UPDATE s
    SET    s.AmountPaid   = CASE WHEN s.AmountPaid - l.Amount < 0 THEN 0 ELSE s.AmountPaid - l.Amount END,
           s.IsPaid       = CASE WHEN s.AmountPaid - l.Amount >= s.AmountDue THEN 1 ELSE 0 END,
           s.PaidDate     = CASE WHEN s.AmountPaid - l.Amount <= 0 THEN NULL ELSE s.PaidDate END,
           s.PayrollRefNo = CASE WHEN s.AmountPaid - l.Amount <= 0 THEN NULL ELSE s.PayrollRefNo END,
           s.UpdatedAt    = GETDATE(),
           s.UpdatedBy    = 'TEST-UNPOST'
    FROM   dbo.tblEmployeeLoanSchedule s
           INNER JOIN dbo.tblPayrollResultLine l ON l.RefId = s.ScheduleId
           INNER JOIN @Cutoffs x ON x.CutoffID = l.CutoffID
    WHERE  l.Category = 'LOAN'
      AND  s.PayrollRefNo = 'PR-' + CAST(l.CutoffID AS VARCHAR(20));
    PRINT 'Installments na naibalik: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

    /* ---------- 2) Loans na naging Completed dahil sa Post -> Active ulit ---------- */
    UPDATE ln
    SET    ln.Status      = 'Active',
           ln.CompletedAt = NULL,
           ln.UpdatedAt   = GETDATE(),
           ln.UpdatedBy   = 'TEST-UNPOST'
    FROM   dbo.tblEmployeeLoan ln
    WHERE  ln.Status = 'Completed'
      AND  ln.EmployeeLoanId IN (SELECT s.EmployeeLoanId
                                 FROM dbo.tblEmployeeLoanSchedule s
                                      INNER JOIN dbo.tblPayrollResultLine l ON l.RefId = s.ScheduleId
                                      INNER JOIN @Cutoffs x ON x.CutoffID = l.CutoffID
                                 WHERE l.Category = 'LOAN')
      AND  EXISTS (SELECT 1 FROM dbo.tblEmployeeLoanSchedule z WHERE z.EmployeeLoanId = ln.EmployeeLoanId AND z.IsPaid = 0);
    PRINT 'Loans na ibinalik sa Active: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

    /* ---------- 3) Cutoffs ---------- */
    UPDATE c
    SET    c.Status = 1, c.PostedBy = NULL, c.PostedDate = NULL,
           c.ModifiedBy = 'TEST-UNPOST', c.ModifiedDate = SYSDATETIME()
    FROM   dbo.tblCutoff c
           INNER JOIN @Cutoffs x ON x.CutoffID = c.CutoffID;
    PRINT 'Cutoffs na ibinalik sa Processed: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

    /* ---------- PAGKATAPOS ---------- */
    SELECT 'PAGKATAPOS' AS Stage, l.CutoffID, r.EmployeeName, s.ScheduleId, l.Amount AS LineAmount, s.AmountDue, s.AmountPaid, s.IsPaid,
           s.PayrollRefNo
    FROM dbo.tblPayrollResultLine l
         INNER JOIN @Cutoffs x ON x.CutoffID = l.CutoffID
         INNER JOIN dbo.tblPayrollResult r ON r.ResultId = l.ResultId
         LEFT JOIN dbo.tblEmployeeLoanSchedule s ON s.ScheduleId = l.RefId
    WHERE l.Category = 'LOAN'
    ORDER BY l.CutoffID, r.EmployeeName;

    SELECT 'CUTOFFS' AS Stage, c.CutoffID, c.CutoffLabel, c.Status, c.PostedBy, c.PostedDate
    FROM dbo.tblCutoff c INNER JOIN @Cutoffs x ON x.CutoffID = c.CutoffID;

    IF @Commit = 1
    BEGIN
        COMMIT TRAN;
        PRINT 'COMMITTED: ang mga cutoff ay Processed na ulit. Buksan ulit ang Loans screen sa app.';
    END
    ELSE
    BEGIN
        ROLLBACK TRAN;
        PRINT 'PREVIEW LANG - na-rollback, WALANG nabago. Itakda ang @Commit = 1 at patakbuhin ulit para ipatupad.';
    END
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @Msg NVARCHAR(2000) = ERROR_MESSAGE();
    RAISERROR('Walang binago: %s', 16, 1, @Msg);
END CATCH
