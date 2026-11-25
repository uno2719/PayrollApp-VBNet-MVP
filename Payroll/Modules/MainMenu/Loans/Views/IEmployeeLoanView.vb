' ============================================================
' Modules/MainMenu/Loans/Views/IEmployeeLoanView.vb
' ============================================================
' Ang kontrata sa pagitan ng ucLoans at ng Presenter.
'
' Pansinin na walang DevExpress dito - walang GridControl, walang
' TextEdit. Purong data at intent lang. Ganito dapat: dapat
' kayang palitan ang buong UI nang hindi nagagalaw ang Presenter.
' ============================================================
Imports Payroll.GlobalShared.Models
Imports Payroll.Employee.Models

Namespace Loans.Views

    Public Interface IEmployeeLoanView

        ' ---------- LISTS ----------
        Sub BindEmployeeList(items As List(Of EmployeeModel))
        Sub BindLoanList(items As List(Of EmployeeLoanModel))
        Sub BindLoanCodes(items As List(Of LoanModel))

        Sub SetEmployeeHeader(employeeNo As String, fullName As String)

        ' ---------- FORM FIELDS ----------
        Property LoanCode As String
        Property InputDate As Date
        Property LoanStartDate As Date
        Property Frequency As String
        Property AmortizationMethod As String
        Property InterestMethod As String
        Property InterestRate As Decimal
        Property Terms As Integer

        ' "Per Period" / "Per Year" (LoanRatePeriod) at "Per Deduction" / "Per Month" (LoanAmortBasis).
        Property RatePeriod As String
        Property AmortBasis As String
        Property Remark As String

        ' Ilang hulog ang nabayaran na sa C1Pay (bagong loan lang)
        Property OpeningPaidInstallments As Integer

        Property PrincipalAmount As Decimal
        Property InterestAmount As Decimal
        Property TotalLoanAmount As Decimal
        Property PrincipalAmortization As Decimal
        Property InterestAmortization As Decimal
        Property TotalAmortization As Decimal

        Property StatusText As String

        ' ---------- STATE / UX ----------
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()

        ''' <summary>
        ''' Kinokontrol ang dalawang contextual button na lumalabas lang
        ''' kapag may napiling loan sa grid: "Details" at "Set Active/Inactive".
        ''' Ang toggleCaption ay nagbabago base sa kasalukuyang status.
        ''' </summary>
        Sub SetLoanSelected(hasSelection As Boolean, canToggle As Boolean, toggleCaption As String)

        ''' <summary>
        ''' Kapag "Based on Term", ang Terms ay editable at ang Total
        ''' Amortization ay computed (read-only). Kapag "Based on Amort",
        ''' baliktad. Hindi dapat pwedeng i-edit ni user ang dalawa
        ''' nang sabay - magkakasalungat.
        ''' </summary>
        Sub ApplyAmortizationMethodLayout(isBasedOnTerm As Boolean)

        Sub ShowLoanDetails(detail As EmployeeLoanDetailModel)

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

        ' --- Offset (renewal) ---
        ' Ipinapakita ang Offset dialog: may Active na loan na pareho ang LoanCode. Ang newLoan ay kuwentado na
        ' (para sa preview at sa check kung kasya ang credit).
        Function AskOffset(candidates As List(Of LoanOffsetCandidate), newLoan As EmployeeLoanModel) As LoanOffsetDialogResult

        ' Admin-only na aksyon: mensahe (Yes/No, default No) + password ng naka-login na Admin.
        Function ConfirmAdminWithPassword(title As String, message As String, actionDescription As String) As Boolean

    End Interface

End Namespace