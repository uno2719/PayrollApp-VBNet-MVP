/* ============================================================================
   Settings > Delete  -  PREFLIGHT (read-only, idempotent, pwedeng ulit-ulitin)
   Sinusuri kung nasa DB ang lahat ng table/column na ginagamit ng bagong
   in-use check at ng DELETE. Walang binabago sa data o schema.

   Resulta:  walang lumabas na row  = OK
             "MISSING COLUMN"       = ayusin muna bago gamitin ang Delete ng module na iyon
             "MISSING TABLE (skipped at runtime)" = para sa tblPayrollInputTxn / tblPayrollResult*
                  ay OK lang: nilalaktawan ng code ang check kung wala pa ang table.
   ============================================================================ */
SET NOCOUNT ON;

DECLARE @need TABLE (TableName sysname, ColumnName sysname, Used NVARCHAR(80));

INSERT INTO @need (TableName, ColumnName, Used) VALUES
 -- Master Data lookups (id + code) --------------------------------------------
 ('tblBranch','BranchId','Master Data'),('tblBranch','BranchCode','Master Data'),
 ('tblDepartment','DepartmentId','Master Data'),('tblDepartment','DepartmentCode','Master Data'),
 ('tblPosition','PositionId','Master Data'),('tblPosition','PositionCode','Master Data'),
 ('tblCategoryCode','CategoryId','Master Data'),('tblCategoryCode','CategoryCode','Master Data'),
 ('tblJobClass','JobClassId','Master Data'),('tblJobClass','JobClassCode','Master Data'),
 ('tblHolidayGroup','HolidayGroupId','Master Data'),('tblHolidayGroup','HolidayGroupCode','Master Data'),
 ('tblScheduleGroup','ScheduleGroupId','Master Data'),('tblScheduleGroup','ScheduleGroupCode','Master Data'),
 ('tblLeaveGroup','LeaveGroupId','Master Data'),('tblLeaveGroup','LeaveGroupCode','Master Data'),
 ('tblBank','BankId','Master Data'),('tblBank','BankCode','Master Data'),
 -- tumuturo sa lookups -----------------------------------------------------------
 ('tblEmployeeEmployment','BranchId','in-use: Master Data'),('tblEmployeeEmployment','DepartmentId','in-use: Master Data'),
 ('tblEmployeeEmployment','PositionId','in-use: Master Data'),('tblEmployeeEmployment','CategoryId','in-use: Master Data'),
 ('tblEmployeeEmployment','JobClassId','in-use: Master Data'),('tblEmployeeEmployment','HolidayGroupId','in-use: Master Data'),
 ('tblEmployeeEmployment','ScheduleGroupId','in-use: Master Data'),('tblEmployeeEmployment','LeaveGroupId','in-use: Master Data'),
 ('tblEmployeeEarnings','BankId','in-use: Master Data > Bank'),
 -- Company Bank --------------------------------------------------------------------
 ('tblCompanyBank','BankId','Company Bank'),('tblCompanyBank','BankCode','Company Bank'),
 -- Leave Settings ------------------------------------------------------------------
 ('tblLeaveType','LeaveTypeId','Leave Type'),('tblLeaveType','LeaveTypeCode','Leave Type'),
 ('tblLeaveRule','LeaveRuleId','Leave Rule'),('tblLeaveRule','LeaveGroupId','in-use: Leave Group/Type'),
 ('tblLeaveRule','LeaveTypeId','in-use: Leave Type'),('tblLeaveRuleBracket','LeaveRuleId','Leave Rule brackets'),
 -- Tax / Statutory ------------------------------------------------------------------
 ('tblIncomeTaxYearly','IncomeTaxYearlyId','Tax Table'),('tblIncomeTaxMonthly','IncomeTaxMonthlyId','Tax Table'),
 ('tblIncomeTaxSemiMonthly','IncomeTaxSemiMonthlyId','Tax Table'),('tblIncomeTaxWeekly','IncomeTaxWeeklyId','Tax Table'),
 ('tblIncomeTaxDaily','IncomeTaxDailyId','Tax Table'),
 ('tblStatutorySSS','SSSId','Statutory'),('tblStatutoryPhilHealth','PhilHealthId','Statutory'),
 ('tblStatutoryPagIbig','PagIbigId','Statutory'),
 ('tblPayrollResultBracket','SourceTable','in-use: Tax/Statutory'),('tblPayrollResultBracket','SourceId','in-use: Tax/Statutory'),
 -- Payroll Settings ------------------------------------------------------------------
 ('tblCompensation','CompensationId','Payroll Settings'),('tblCompensation','Code','Payroll Settings'),
 ('tblDeduction','DeductionId','Payroll Settings'),('tblDeduction','Code','Payroll Settings'),
 ('tblBonus','BonusId','Payroll Settings'),('tblBonus','Code','Payroll Settings'),
 ('tblOvertime','OvertimeId','Payroll Settings'),('tblOvertime','Code','Payroll Settings'),
 ('tblHoliday','HolidayId','Payroll Settings'),('tblHoliday','Code','Payroll Settings'),
 ('tblLoan','LoanId','Payroll Settings'),('tblLoan','Code','Payroll Settings'),
 ('tblEmployeeLoan','LoanCode','in-use: Loan'),
 ('tblPayrollInputTxn','TxnCategory','in-use: Payroll Settings'),('tblPayrollInputTxn','TxnCode','in-use: Payroll Settings'),
 ('tblPayrollResultLine','Category','in-use: Payroll Settings'),('tblPayrollResultLine','Code','in-use: Payroll Settings'),
 ('tblGeneralSettings','BasicSalaryCodePlusId','in-use: Compensation'),
 ('tblGeneralSettings','BasicSalaryCodeMinusId','in-use: Deduction'),('tblGeneralSettings','AbsentCodeId','in-use: Deduction'),
 ('tblGeneralSettings','LateInCodeId','in-use: Deduction'),('tblGeneralSettings','EarlyOutCodeId','in-use: Deduction');

SELECT n.TableName, n.ColumnName, n.Used,
       CASE WHEN OBJECT_ID(N'dbo.' + n.TableName, N'U') IS NULL THEN 'MISSING TABLE (skipped at runtime if in-use table)'
            ELSE 'MISSING COLUMN' END AS Problem
FROM @need n
WHERE OBJECT_ID(N'dbo.' + n.TableName, N'U') IS NULL
   OR NOT EXISTS (SELECT 1 FROM sys.columns c
                  WHERE c.object_id = OBJECT_ID(N'dbo.' + n.TableName, N'U') AND c.name = n.ColumnName)
ORDER BY n.TableName, n.ColumnName;

IF @@ROWCOUNT = 0 PRINT 'Preflight OK - kumpleto ang lahat ng table/column.';
