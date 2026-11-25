/* ============================================================
   2026-11-25_Loan_OpeningPaid_Check.sql
   READ-ONLY. Walang binabago - pang-check lang bago/pagkatapos subukan ang
   "Already paid (C1Pay)" at "Mark paid up to #N" sa Loans module.
   Walang kailangang i-run para gumana ang feature (walang bagong table/column).
   ============================================================ */

-- 1) Sapat ba ang haba ng PayrollRefNo para sa 'C1PAY' (5 chars)? (kailangan >= 5, o -1 kung MAX)
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM   INFORMATION_SCHEMA.COLUMNS
WHERE  TABLE_NAME = 'tblEmployeeLoanSchedule'
  AND  COLUMN_NAME IN ('PayrollRefNo', 'PaidDate', 'AmountPaid', 'IsPaid');

-- 2) Lahat ng opening (C1PAY) payments, bawat loan: ilan, magkano, at hanggang anong hulog
SELECT  e.EmployeeNo,
        e.FirstName + ' ' + e.LastName   AS EmployeeName,
        l.EmployeeLoanId,
        l.LoanCode,
        l.Status,
        l.TotalLoanAmount,
        COUNT(*)                         AS OpeningInstallments,
        MAX(s.InstallmentNo)             AS UpToInstallmentNo,
        SUM(s.AmountPaid)                AS OpeningAmountPaid,
        l.TotalLoanAmount - SUM(s.AmountPaid) AS BalanceIfOnlyOpening
FROM    dbo.tblEmployeeLoanSchedule s
        INNER JOIN dbo.tblEmployeeLoan l ON l.EmployeeLoanId = s.EmployeeLoanId
        LEFT  JOIN dbo.tblEmployee     e ON e.RecordId       = l.EmployeeRecordId
WHERE   s.PayrollRefNo = 'C1PAY'
GROUP BY e.EmployeeNo, e.FirstName, e.LastName, l.EmployeeLoanId, l.LoanCode, l.Status, l.TotalLoanAmount
ORDER BY e.EmployeeNo, l.EmployeeLoanId;

-- 3) May butas ba sa pagkakasunod-sunod? (may unpaid na hulog bago ang isang bayad na hulog) -> dapat WALANG lumabas
SELECT  s.EmployeeLoanId, s.InstallmentNo, s.ScheduledDate, s.IsPaid, s.PayrollRefNo
FROM    dbo.tblEmployeeLoanSchedule s
WHERE   s.IsPaid = 0
  AND   EXISTS (SELECT 1 FROM dbo.tblEmployeeLoanSchedule z
                WHERE  z.EmployeeLoanId = s.EmployeeLoanId
                  AND  z.InstallmentNo  > s.InstallmentNo
                  AND  z.IsPaid = 1)
ORDER BY s.EmployeeLoanId, s.InstallmentNo;
