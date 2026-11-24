/* =====================================================================
   TEST ONLY - HUWAG GAMITIN SA PRODUCTION / SA TOTOONG PAYROLL.
   Ibinabalik ang isang Posted na cutoff sa Processed (kabaligtaran ng Post Cutoff sa app).
   Walang Unpost sa app dahil sa desisyon ng accounting head; ito ay para sa pagte-test mo lang.

   PAANO GAMITIN
     1) Itakda ang @CutoffID sa baba.
     2) Patakbuhin nang buo. Default ay PREVIEW: ipapakita ang mga mababago, tapos ROLLBACK (walang nagbabago).
     3) Kung ayos ang preview, itakda ang @Commit = 1 at patakbuhin ulit.

   ANO ANG IBINABALIK
     - tblCutoff: Status = 1 (Processed), PostedBy/PostedDate = NULL
     - Loans: ang mga installment na minarkahan ng Post na ito (PayrollRefNo = 'PR-{CutoffID}') ay ibabalik:
              AmountPaid -= kinaltas sa cutoff, IsPaid / PaidDate / PayrollRefNo ayon doon
     - Loan na naging Completed dahil sa Post na ito (may hindi na bayad na installment ulit) ay ibabalik sa Active
   HINDI ginagalaw: tblPayrollExportLog (valid pa rin ang export hangga't hindi nire-re-process ang cutoff)

   SAFETY: titigil kung hindi Posted ang cutoff, o kung may mas bagong Posted na cutoff ng parehong pay cycle.
   ===================================================================== */
SET NOCOUNT ON;

DECLARE @CutoffID INT = 0;      -- <== ILAGAY ANG CutoffID DITO
DECLARE @Commit   BIT = 0;      -- 0 = preview lang (ROLLBACK), 1 = i-commit

DECLARE @Ref VARCHAR(50) = 'PR-' + CAST(@CutoffID AS VARCHAR(20));

BEGIN TRY
    BEGIN TRAN;

    DECLARE @Status TINYINT, @CycleType NVARCHAR(100), @CutoffStart DATE;
    SELECT @Status = Status, @CycleType = CycleType, @CutoffStart = CutoffStart
    FROM dbo.tblCutoff WITH (UPDLOCK, ROWLOCK)
    WHERE CutoffID = @CutoffID;

    IF @Status IS NULL      RAISERROR('Walang cutoff na may ganitong CutoffID.', 16, 1);
    IF @Status <> 2         RAISERROR('Hindi Posted ang cutoff na ito (Status <> 2). Walang gagawin.', 16, 1);

    IF EXISTS (SELECT 1 FROM dbo.tblCutoff
               WHERE CutoffID <> @CutoffID AND Status = 2
                 AND CycleType = @CycleType AND CutoffStart > @CutoffStart)
        RAISERROR('May mas bagong Posted na cutoff ng parehong pay cycle. I-unpost muna iyon.', 16, 1);

    -- PREVIEW: mga loan installment na ibabalik
    SELECT 'LOAN INSTALLMENTS NA IBABALIK' AS Preview, s.ScheduleId, s.EmployeeLoanId, s.AmountDue,
           s.AmountPaid AS AmountPaid_Now, s.AmountPaid - l.Amount AS AmountPaid_After, s.IsPaid AS IsPaid_Now
    FROM dbo.tblEmployeeLoanSchedule s
         INNER JOIN dbo.tblPayrollResultLine l ON l.RefId = s.ScheduleId
    WHERE l.CutoffID = @CutoffID AND l.Category = 'LOAN' AND s.PayrollRefNo = @Ref;

    -- 1) Loan installments
    UPDATE s
    SET    s.AmountPaid   = CASE WHEN s.AmountPaid - l.Amount < 0 THEN 0 ELSE s.AmountPaid - l.Amount END,
           s.IsPaid       = CASE WHEN s.AmountPaid - l.Amount >= s.AmountDue THEN 1 ELSE 0 END,
           s.PaidDate     = CASE WHEN s.AmountPaid - l.Amount <= 0 THEN NULL ELSE s.PaidDate END,
           s.PayrollRefNo = CASE WHEN s.AmountPaid - l.Amount <= 0 THEN NULL ELSE s.PayrollRefNo END,
           s.UpdatedAt    = GETDATE(),
           s.UpdatedBy    = 'TEST-UNPOST'
    FROM   dbo.tblEmployeeLoanSchedule s
           INNER JOIN dbo.tblPayrollResultLine l ON l.RefId = s.ScheduleId
    WHERE  l.CutoffID = @CutoffID AND l.Category = 'LOAN' AND s.PayrollRefNo = @Ref;
    PRINT 'Installments na naibalik: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

    -- 2) Loans na naging Completed dahil sa Post na ito -> Active ulit
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
                                 WHERE l.CutoffID = @CutoffID AND l.Category = 'LOAN')
      AND  EXISTS (SELECT 1 FROM dbo.tblEmployeeLoanSchedule x
                   WHERE x.EmployeeLoanId = ln.EmployeeLoanId AND x.IsPaid = 0);
    PRINT 'Loans na ibinalik sa Active: ' + CAST(@@ROWCOUNT AS VARCHAR(20));

    -- 3) Cutoff
    UPDATE dbo.tblCutoff
    SET    Status = 1, PostedBy = NULL, PostedDate = NULL,
           ModifiedBy = 'TEST-UNPOST', ModifiedDate = SYSDATETIME()
    WHERE  CutoffID = @CutoffID;

    SELECT 'CUTOFF PAGKATAPOS' AS Preview, CutoffID, CutoffLabel, Status, PostedBy, PostedDate
    FROM dbo.tblCutoff WHERE CutoffID = @CutoffID;

    IF @Commit = 1
    BEGIN
        COMMIT TRAN;
        PRINT 'COMMITTED: ang cutoff ay Processed na ulit.';
    END
    ELSE
    BEGIN
        ROLLBACK TRAN;
        PRINT 'PREVIEW LANG - na-rollback. Itakda ang @Commit = 1 para ipatupad.';
    END
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @Msg NVARCHAR(2000) = ERROR_MESSAGE();
    RAISERROR('Walang binago: %s', 16, 1, @Msg);
END CATCH
