/* ============================================================
   TEST_ONLY_Loan_Offset_TestData.sql   (LOCAL DB LANG - huwag sa server)
   ------------------------------------------------------------
   Gumagawa ng LUMANG SSS loan para ma-test ang Offset:
     - Principal 46,512.96, walang interes, 24 x 1,938.04, Second Cycle Only (katapusan ng buwan)
     - Hulog Aug 31 2025 ... Jul 31 2027
     - #1-#15 (Aug 2025 - Oct 2026) naka-mark na bayad bilang C1PAY
     - #16-#24 (Nov 2026 - Jul 2027) hindi pa bayad = 9 x 1,938.04 = 17,442.36

   Pagkatapos, sa Loans module, gumawa ng BAGONG loan (parehong employee at Loan Code):
     Principal 40,000 | Diminishing | Based on Amort | Interest Rate 8, "Per Year" | Total Amort 1,809.09
     Frequency: Second Cycle Only (o Every Cycle + "Per Month") | Start: 01/01/2027
     -> Dapat 24 terms, interest 3,418.16, total 43,418.16. Pag-Save, lalabas ang Offset dialog.
        * Balance per statement = 0         -> walang credit
        * Balance per statement = 19,380.43 -> credit 1,938.07 (19,380.43 - 17,442.36) -> lalabas ang 3 option

   PALITAN ang dalawang value sa ibaba.
   ============================================================ */
SET NOCOUNT ON;

DECLARE @EmployeeNo VARCHAR(50) = 'PALITAN';     -- Employee No ng test employee
DECLARE @LoanCode   VARCHAR(20) = 'PALITAN';     -- Code ng SSS Salary Loan sa Payroll Settings > Loan

DECLARE @RecordId INT = (SELECT TOP 1 RecordId FROM dbo.tblEmployee WHERE EmployeeNo = @EmployeeNo AND IsDeleted = 0);

IF @RecordId IS NULL
BEGIN
    RAISERROR('Employee not found. Palitan ang @EmployeeNo.', 16, 1);
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM dbo.tblLoan WHERE Code = @LoanCode)
BEGIN
    RAISERROR('Loan Code not found in tblLoan. Palitan ang @LoanCode.', 16, 1);
    RETURN;
END

BEGIN TRANSACTION;

DECLARE @LoanId INT;

INSERT INTO dbo.tblEmployeeLoan
    (EmployeeRecordId, LoanCode, InputDate, LoanStartDate,
     Frequency, AmortizationMethod, InterestMethod, InterestRate, Terms,
     RatePeriod, AmortBasis,
     PrincipalAmount, InterestAmount, TotalLoanAmount,
     PrincipalAmortization, InterestAmortization, TotalAmortization,
     Status, Remark, OffsetCredit, CreatedAt, CreatedBy)
VALUES
    (@RecordId, @LoanCode, '2025-07-30', '2025-08-01',
     'Second Cycle Only', 'Based on Term', 'Straight Line', 0, 24,
     'Per Period', 'Per Deduction',
     46512.96, 0, 46512.96,
     1938.04, 0, 1938.04,
     'Active', N'TEST-OFFSET old SSS loan', 0, GETDATE(), 'test-script');

SET @LoanId = SCOPE_IDENTITY();

;WITH n AS (
    SELECT TOP (24) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS No
    FROM sys.all_objects
)
INSERT INTO dbo.tblEmployeeLoanSchedule
    (EmployeeLoanId, InstallmentNo, ScheduledDate, PrincipalDue, InterestDue, AmountDue,
     AmountPaid, PaidDate, IsPaid, PayrollRefNo, CreatedAt, CreatedBy)
SELECT  @LoanId,
        n.No,
        DATEADD(DAY, -1, DATEADD(MONTH, n.No, '2025-08-01')),              -- katapusan ng buwan
        1938.04, 0, 1938.04,
        CASE WHEN n.No <= 15 THEN 1938.04 ELSE 0 END,
        CASE WHEN n.No <= 15 THEN DATEADD(DAY, -1, DATEADD(MONTH, n.No, '2025-08-01')) END,
        CASE WHEN n.No <= 15 THEN 1 ELSE 0 END,
        CASE WHEN n.No <= 15 THEN 'C1PAY' END,
        GETDATE(), 'test-script'
FROM n;

COMMIT TRANSACTION;

SELECT @LoanId AS TestOldLoanId,
       SUM(CASE WHEN IsPaid = 0 THEN AmountDue - AmountPaid ELSE 0 END) AS UnpaidInSystem,   -- dapat 17,442.36
       SUM(CASE WHEN IsPaid = 0 THEN 1 ELSE 0 END) AS UnpaidCount                            -- dapat 9
FROM dbo.tblEmployeeLoanSchedule WHERE EmployeeLoanId = @LoanId;
GO

/* ------------------------------------------------------------
   PAGLILINIS (patakbuhin lang kapag tapos na ang test):
   Undo muna ang offset sa app (o i-delete ang bagong loan sa app), saka ito:

   DELETE FROM dbo.tblEmployeeLoan WHERE Remark LIKE N'TEST-OFFSET%' AND OffsetByLoanId IS NULL;
   ------------------------------------------------------------ */
