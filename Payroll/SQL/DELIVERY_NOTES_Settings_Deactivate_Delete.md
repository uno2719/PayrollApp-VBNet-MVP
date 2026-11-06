# Settings - Deactivate / Delete rework (10 views)

## Paano i-apply
1. (Optional pero inirerekomenda) Patakbuhin ang `SQL/2026-11-06_Settings_DeleteCheck_Preflight.sql` sa Payroll DB. Read-only at idempotent. Walang lumabas na row = OK. Walang schema change sa delivery na ito.
2. I-extract ang zip sa ibabaw ng repo (kasama ang `.Designer.vb`) **o** `git apply --ignore-whitespace settings-deactivate-delete.patch`.
3. Kung hindi na-register ang 2 bagong icon, i-check ang Project Properties > Resources: dapat may `icon_deactivate_24` at `icon_reactivate_24` (naka-register na sa `My Project/Resources.resx` at `Resources.Designer.vb`; ang PNG ay nasa `Resources/`).
4. Compile. Tingnan ang bawat module na hindi nagoverlap ang command bar (lumapad ng 125 px).

## Command bar (lahat ng 10 views)
New | Edit | **Deactivate/Reactivate** | **Delete** | Refresh
- Ang Deactivate ay ang dating "Delete" (soft). Isang Yes/No confirm pa rin. Nagbabago ang caption/icon ayon sa Status ng napiling row.
- Ang Delete ay totoong pagbura:
  1. Admin lang (`AppSession.IsAdmin`). Naka-disable sa lahat ng iba, kahit Can Edit. Ini-enforce din sa Service at sa `RecordDeleter` (huling harang).
  2. Confirm 1 (Yes/No, default **No**) -> Confirm 2 "FINAL WARNING" (default **No**) -> password ng naka-login na user.
  3. In-use check sa loob ng iisang transaction. Kapag may gumagamit (kahit draft entries), hindi buburahin; sasabihin kung ilan at ituturo sa Deactivate.
  4. Safety net: SQL error 547 (FK) -> malinaw na message.
- Status: wala na ang "Active" checkbox; may **Status: Active / Inactive** na label (berde/pula).
- Pag ine-edit at sine-save ang **Inactive** na record: Yes = i-save at gawing Active, No = i-save pero manatiling Inactive (default), Cancel = bumalik sa edit. Ang bagong entry ay laging Active.

## Na-ayos na bug (kasama)
Ang `SetFormMode` ay nag-e-enable ulit ng Delete kahit View Only ang user (nao-overwrite ang `ApplyReadOnlyMode`). Ngayon ay `HasEditAccess` ang binabantayan para sa Deactivate, at admin para sa Delete.

## Ano ang haharang sa Delete (base sa FK report ng DB at sa code)
- Master Data: `tblEmployeeEmployment` (Branch/Department/Position/Category/JobClass/HolidayGroup/ScheduleGroup/LeaveGroup), `tblEmployeeEarnings.BankId` (Bank), `tblLeaveRule` (Leave Group)
- Company Bank: wala (walang tumuturo sa tblCompanyBank)
- Leave Type: `tblLeaveRule`. Leave Rule: buburahin kasama ang brackets (parehong transaction)
- Tax / Statutory brackets: `tblPayrollResultBracket (SourceTable, SourceId)`
- Compensation / Deduction / Bonus / Overtime / Holiday: `tblPayrollInputTxn`, `tblPayrollResultLine`; Compensation at Deduction din: `tblGeneralSettings` code mapping
- Loan: `tblEmployeeLoan.LoanCode`, `tblPayrollResultLine`

## Bago / binagong files
Bago: GlobalShared/Models/DeleteResult.vb, GlobalShared/Base/RecordDeleter.vb, GlobalShared/Helpers/CommandBarHelper.vb,
GlobalShared/Dialogs/frmConfirmPassword.vb + .Designer.vb, Resources/icon_deactivate_24.png, Resources/icon_reactivate_24.png,
SQL/2026-11-06_Settings_DeleteCheck_Preflight.sql
Binago: Login/Services (IAuthenticationService + AuthenticationService: `VerifyCurrentUserPasswordAsync`), My Project/Resources.resx + .Designer.vb,
at para sa bawat isa sa 10 modules: View + .Designer.vb, Presenter, Service + interface, Repository + interface.

## Mga alam na limitasyon
- Hindi ako nakapag-compile (walang DevExpress sa environment ko); na-review ko ang diff at nag-balance check lang. Kung may compile error, ipadala ang eksaktong message.
- Ang maling password sa Delete prompt ay binibilang sa parehong lockout rule ng login (5 mali = 15 min lock). Hindi nire-reset ng tamang password ang counter; login lang ang nag-re-reset.
- Hindi ginalaw: ucCompany, ucCompanyAgencyRegistration, ucCutoff, ucPayCycle, ucStatutorySchedule.
