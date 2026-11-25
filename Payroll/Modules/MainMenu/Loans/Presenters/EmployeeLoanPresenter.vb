' ============================================================
' Modules/MainMenu/Loans/Presenters/EmployeeLoanPresenter.vb
' ============================================================
' Ang Presenter ang nag-uugnay ng tatlong bagay:
'   - listahan ng empleyado  (EmployeeService - reused)
'   - listahan ng loan codes (PayrollSettings LoanRepository - reused)
'   - mga aktwal na utang    (EmployeeLoanService - bago)
'
' Sinusundan nito ang pattern ng CompanyPresenter mo, na tumatanggap
' din ng EmployeeRepository para sa employee picker - hindi tayo
' gumagawa ng bagong service kung meron nang gumagana.
' ============================================================
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace Loans.Presenters

    Public Class EmployeeLoanPresenter

        Private ReadOnly _view As Views.IEmployeeLoanView
        Private ReadOnly _service As Services.IEmployeeLoanService
        Private ReadOnly _employeeService As Employee.Services.IEmployeeService
        Private ReadOnly _loanCodeRepo As PayrollSettings.Data.ILoanRepository
        Private ReadOnly _userName As String

        ' --- STATE ---
        Private _selectedEmployeeId As Integer = 0
        Private _selectedLoanId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _isEditing As Boolean = False

        Private _loanList As List(Of EmployeeLoanModel)
        Private _employeeFilter As String = "Active"
        Private _loanStatusFilter As String = "Active"

        Public Sub New(view As Views.IEmployeeLoanView,
                       service As Services.IEmployeeLoanService,
                       employeeService As Employee.Services.IEmployeeService,
                       loanCodeRepo As PayrollSettings.Data.ILoanRepository,
                       userName As String)

            _view = view
            _service = service
            _employeeService = employeeService
            _loanCodeRepo = loanCodeRepo
            _userName = userName

        End Sub

        Public ReadOnly Property IsEditing As Boolean
            Get
                Return _isEditing
            End Get
        End Property

        Public ReadOnly Property SelectedLoanId As Integer
            Get
                Return _selectedLoanId
            End Get
        End Property

        ' ========================================================
        ' INITIAL LOAD
        ' ========================================================
        Public Async Function LoadAsync() As Task

            ' Ang loan codes ay galing sa Payroll Settings > Loan tab.
            ' Ang Active lang ang ipinapakita - walang saysay na payagan
            ' kang mag-assign ng code na deactivated na.
            Dim allCodes = Await _loanCodeRepo.GetAllAsync()
            _view.BindLoanCodes(allCodes.Where(Function(c) c.IsActive).ToList())

            Await LoadEmployeeListAsync()

            ResetForm()

        End Function

        Public Async Function LoadEmployeeListAsync() As Task
            Dim employees = Await _employeeService.GetAllRecordsAsync(_employeeFilter)
            _view.BindEmployeeList(employees)
        End Function

        Public Async Function SetEmployeeFilterAsync(filter As String) As Task
            _employeeFilter = filter
            Await LoadEmployeeListAsync()
        End Function

        ' ========================================================
        ' EMPLOYEE SELECTED
        ' ========================================================
        Public Async Function SelectEmployeeAsync(employeeRecordId As Integer,
                                                  employeeNo As String,
                                                  fullName As String) As Task

            If _isEditing Then Return   ' huwag hayaang mapalitan habang nag-e-edit

            _selectedEmployeeId = employeeRecordId
            _view.SetEmployeeHeader(employeeNo, fullName)

            Await LoadLoanListAsync()
            ResetForm()

        End Function

        Public Async Function SetLoanStatusFilterAsync(filter As String) As Task
            _loanStatusFilter = filter
            Await LoadLoanListAsync()
        End Function

        Public Async Function LoadLoanListAsync() As Task

            If _selectedEmployeeId <= 0 Then
                _loanList = New List(Of EmployeeLoanModel)
                _view.BindLoanList(_loanList)
                Return
            End If

            _loanList = Await _service.GetByEmployeeAsync(_selectedEmployeeId, _loanStatusFilter)
            _view.BindLoanList(_loanList)

        End Function

        ' ========================================================
        ' LOAN SELECTED
        ' ========================================================
        Public Sub SelectLoan(loanId As Integer)

            If _isEditing Then Return

            Dim loan = _loanList?.FirstOrDefault(Function(x) x.Id = loanId)
            If loan Is Nothing Then Return

            _selectedLoanId = loanId
            _isNewMode = False

            PushToView(loan)

            _view.SetFormMode(False, False)

            ' Dito lumalabas ang dalawang contextual button.
            _view.SetLoanSelected(
                hasSelection:=True,
                canToggle:=LoanStatus.IsToggleable(loan.Status),
                toggleCaption:=If(LoanStatus.IsDeductible(loan.Status),
                                  " Set Inactive", " Set Active"))

        End Sub

        ' ========================================================
        ' NEW / EDIT / CANCEL
        ' ========================================================
        Public Sub StartNew()

            If _selectedEmployeeId <= 0 Then
                _view.ShowError("Please select an employee first.")
                Return
            End If

            _selectedLoanId = 0
            _isNewMode = True
            _isEditing = True

            _view.ClearFields()

            ' Sensible na default para hindi blangko ang form.
            _view.InputDate = Date.Today
            _view.LoanStartDate = Date.Today
            _view.Frequency = LoanFrequency.EveryCycle
            _view.AmortizationMethod = LoanAmortizationMethod.BasedOnTerm
            _view.InterestMethod = LoanInterestMethod.StraightLine
            ' Bagong loan: Per Year ang rate (hal. 8 = 8% kada taon, gaya ng SSS statement) at buwanan ang
            ' amortization na hinahati sa mga cutoff ng buwan. Ang mga lumang loan ay naka-save bilang
            ' Per Period / Per Deduction kaya hindi sila nagbabago.
            _view.RatePeriod = LoanRatePeriod.PerYear
            _view.AmortBasis = LoanAmortBasis.PerMonth
            _view.StatusText = LoanStatus.Active
            _view.OpeningPaidInstallments = 0

            _view.ApplyAmortizationMethodLayout(True)
            _view.SetFormMode(True, True)
            _view.SetLoanSelected(False, False, String.Empty)

        End Sub

        Public Sub StartEdit()

            If _selectedLoanId = 0 Then
                _view.ShowError("Please select a loan first.")
                Return
            End If

            Dim loan = _loanList?.FirstOrDefault(Function(x) x.Id = _selectedLoanId)

            If loan IsNot Nothing AndAlso Not LoanStatus.IsToggleable(loan.Status) Then
                _view.ShowError($"This loan is already {loan.Status} and can no longer be edited.")
                Return
            End If

            _isNewMode = False
            _isEditing = True

            _view.ApplyAmortizationMethodLayout(
                String.Equals(_view.AmortizationMethod,
                              LoanAmortizationMethod.BasedOnTerm,
                              StringComparison.OrdinalIgnoreCase))

            _view.SetFormMode(True, False)
            _view.SetLoanSelected(False, False, String.Empty)

        End Sub

        Public Sub CancelEdit()

            _isEditing = False

            If _selectedLoanId > 0 AndAlso Not _isNewMode Then
                SelectLoan(_selectedLoanId)
            Else
                ResetForm()
            End If

        End Sub

        Private Sub ResetForm()
            _selectedLoanId = 0
            _isNewMode = False
            _isEditing = False

            _view.ClearFields()
            _view.SetFormMode(False, False)
            _view.SetLoanSelected(False, False, String.Empty)
        End Sub

        ' ========================================================
        ' LIVE RECOMPUTE
        ' ========================================================
        ' Tinatawag ng View tuwing may nagbabago sa Principal, Rate,
        ' Terms, Amortization, o sa dalawang method radio.
        '
        ' Tahimik ito - walang message box. Habang nagta-type pa lang
        ' si user, normal na kulang pa ang datos. Sa Save na lang
        ' siya sasabihan kung talagang may mali.
        ' ========================================================
        Public Sub Recompute()

            If Not _isEditing Then Return

            Dim temp = PullFromView()
            _service.Compute(temp)

            ' Ibabalik lang ang mga COMPUTED na field. Huwag mong
            ' babalikan ang mga tina-type pa ni user - mag-ju-jump
            ' ang cursor niya at hindi na siya makakapag-type.
            _view.InterestAmount = temp.InterestAmount
            _view.TotalLoanAmount = temp.TotalLoanAmount
            _view.PrincipalAmortization = temp.PrincipalAmortization
            _view.InterestAmortization = temp.InterestAmortization

            If String.Equals(temp.AmortizationMethod, LoanAmortizationMethod.BasedOnTerm,
                             StringComparison.OrdinalIgnoreCase) Then
                _view.TotalAmortization = temp.TotalAmortization
            Else
                _view.Terms = temp.Terms
            End If

        End Sub

        Public Sub OnAmortizationMethodChanged()

            Dim isBasedOnTerm = String.Equals(_view.AmortizationMethod,
                                              LoanAmortizationMethod.BasedOnTerm,
                                              StringComparison.OrdinalIgnoreCase)

            _view.ApplyAmortizationMethodLayout(isBasedOnTerm)
            Recompute()

        End Sub

        ' ========================================================
        ' SAVE
        ' ========================================================
        Public Async Function SaveAsync() As Task

            Dim item = PullFromView()
            item.Id = _selectedLoanId
            item.EmployeeRecordId = _selectedEmployeeId

            ' --- OFFSET PROMPT (bagong loan lang) ---
            ' May Active na loan na pareho ang LoanCode? Tanungin si user: Offset / Keep both / Cancel.
            If _isNewMode Then
                Dim candidates = Await _service.GetOffsetCandidatesAsync(_selectedEmployeeId, item.LoanCode, 0)

                If candidates.Count > 0 Then
                    ' Kailangan ng kumpletong kwenta ng bagong loan para sa preview (total, at kung kasya ang credit).
                    Dim preview = PullFromView()
                    preview.EmployeeRecordId = _selectedEmployeeId
                    Dim computed = _service.Compute(preview)
                    If Not computed.IsComplete Then
                        _view.ShowError(If(computed.Warning, "Unable to compute the loan amortization."))
                        Return
                    End If

                    Dim answer = _view.AskOffset(candidates, preview)

                    Select Case answer.Choice
                        Case LoanOffsetChoice.Cancel
                            Return
                        Case LoanOffsetChoice.Offset
                            item.OffsetRequest = answer.Request
                        Case Else
                            ' Keep both: ordinaryong save, parehong kakaltasan.
                    End Select
                End If
            End If

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            Dim savedMessage = If(_isNewMode,
                "Loan added. The amortization schedule has been generated.",
                "Loan updated. The amortization schedule has been regenerated.")

            If Not String.IsNullOrWhiteSpace(result.Message) Then
                savedMessage &= Environment.NewLine & result.Message
            End If

            _view.ShowMessage(savedMessage)

            _isEditing = False
            _isNewMode = False

            Await LoadLoanListAsync()

            ' Panatilihing naka-select ang kaka-save lang - para makita
            ' agad ni user ang resulta at ma-click niya ang Details.
            SelectLoan(item.Id)

        End Function

        ' ========================================================
        ' DELETE
        ' ========================================================
        Public Async Function DeleteAsync() As Task

            If _selectedLoanId = 0 Then
                _view.ShowError("Please select a loan first.")
                Return
            End If

            ' Loan na nag-offset ng lumang loan: ang pag-delete ay nagbabalik din ng offset - Admin + password.
            Dim selected = _loanList?.FirstOrDefault(Function(x) x.Id = _selectedLoanId)
            If selected IsNot Nothing AndAlso selected.HasOffsetChildren Then
                If Not _view.ConfirmAdminWithPassword(
                        "Delete Loan",
                        $"Loan #{selected.Id} paid off (offset) {selected.OffsetChildCount} older loan(s)." & Environment.NewLine &
                        "Deleting it will also UNDO the offset: the old loan(s) become Active again and will be deducted again." &
                        Environment.NewLine & Environment.NewLine & "Continue?",
                        $"delete loan #{selected.Id} and undo its offset") Then
                    Return
                End If
            End If

            Dim result = Await _service.DeleteAsync(_selectedLoanId, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage(If(String.IsNullOrWhiteSpace(result.Message), "Loan deleted.", "Loan deleted." & Environment.NewLine & result.Message))

            Await LoadLoanListAsync()
            ResetForm()

        End Function

        ' ========================================================
        ' TOGGLE ACTIVE / INACTIVE
        ' ========================================================
        Public Async Function ToggleStatusAsync() As Task

            If _selectedLoanId = 0 Then
                _view.ShowError("Please select a loan first.")
                Return
            End If

            Dim result = Await _service.ToggleActiveStatusAsync(_selectedLoanId, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage($"Loan is now {result.NewStatus}.")

            Dim keepId = _selectedLoanId
            Await LoadLoanListAsync()

            ' Kung naka-filter sa "Active" lang, mawawala sa listahan ang
            ' kaka-inactive lang - kaya i-reset na lang ang form imbes na
            ' pilitin siyang i-select ulit.
            If _loanList.Any(Function(x) x.Id = keepId) Then
                SelectLoan(keepId)
            Else
                ResetForm()
            End If

        End Function

        ' ========================================================
        ' DETAILS
        ' ========================================================
        Public Async Function ShowDetailsAsync() As Task

            If _selectedLoanId = 0 Then
                _view.ShowError("Please select a loan first.")
                Return
            End If

            Dim detail = Await _service.GetDetailAsync(_selectedLoanId)

            If detail Is Nothing Then
                _view.ShowError("Loan not found. It may have been deleted by another user.")
                Return
            End If

            _view.ShowLoanDetails(detail)

        End Function

        ' ========================================================
        ' OPENING PAYMENTS (para sa Details window)
        ' ========================================================
        ' Ang frmLoanDetails ay tumatawag dito para sa dalawang aksyon at sa pag-reload ng sariling datos.
        Public Async Function GetDetailAsync(loanId As Integer) As Task(Of EmployeeLoanDetailModel)
            Return Await _service.GetDetailAsync(loanId)
        End Function

        Public Async Function MarkOpeningPaidAsync(loanId As Integer, upToInstallmentNo As Integer) As Task(Of Services.LoanSaveResult)
            Return Await _service.MarkOpeningPaidAsync(loanId, upToInstallmentNo, _userName)
        End Function

        Public Async Function UndoOpeningPaidAsync(loanId As Integer) As Task(Of Services.LoanSaveResult)
            Return Await _service.UndoOpeningPaidAsync(loanId, _userName)
        End Function

        ''' <summary>Undo offset. Ang newLoanId ay ang BAGONG loan (ang nag-offset). Admin + password: nasa View.</summary>
        Public Async Function UndoOffsetAsync(newLoanId As Integer) As Task(Of Services.LoanSaveResult)
            Return Await _service.UndoOffsetAsync(newLoanId, _userName)
        End Function

        ''' <summary>
        ''' Tinatawag pagkasara ng Details window kung may nagbago (bayad/status) - i-refresh ang listahan.
        ''' </summary>
        Public Async Function ReloadAfterDetailsChangeAsync() As Task

            Dim keepId = _selectedLoanId

            Await LoadLoanListAsync()

            If keepId > 0 AndAlso _loanList.Any(Function(x) x.Id = keepId) Then
                SelectLoan(keepId)
            Else
                ResetForm()
            End If

        End Function

        ' ========================================================
        ' VIEW <-> MODEL
        ' ========================================================
        Private Function PullFromView() As EmployeeLoanModel

            Return New EmployeeLoanModel With {
                .Id = _selectedLoanId,
                .EmployeeRecordId = _selectedEmployeeId,
                .LoanCode = _view.LoanCode,
                .InputDate = _view.InputDate,
                .LoanStartDate = _view.LoanStartDate,
                .Frequency = _view.Frequency,
                .AmortizationMethod = _view.AmortizationMethod,
                .InterestMethod = _view.InterestMethod,
                .InterestRate = _view.InterestRate,
                .Terms = _view.Terms,
                .RatePeriod = _view.RatePeriod,
                .AmortBasis = _view.AmortBasis,
                .PrincipalAmount = _view.PrincipalAmount,
                .InterestAmount = _view.InterestAmount,
                .TotalLoanAmount = _view.TotalLoanAmount,
                .PrincipalAmortization = _view.PrincipalAmortization,
                .InterestAmortization = _view.InterestAmortization,
                .TotalAmortization = _view.TotalAmortization,
                .Remark = _view.Remark,
                .OpeningPaidInstallments = _view.OpeningPaidInstallments
            }

        End Function

        Private Sub PushToView(loan As EmployeeLoanModel)

            _view.LoanCode = loan.LoanCode
            _view.InputDate = loan.InputDate
            _view.LoanStartDate = loan.LoanStartDate
            _view.Frequency = loan.Frequency
            _view.AmortizationMethod = loan.AmortizationMethod
            _view.InterestMethod = loan.InterestMethod
            _view.InterestRate = loan.InterestRate
            _view.Terms = loan.Terms
            _view.RatePeriod = If(loan.RatePeriod, LoanRatePeriod.PerPeriod)
            _view.AmortBasis = If(loan.AmortBasis, LoanAmortBasis.PerDeduction)
            _view.PrincipalAmount = loan.PrincipalAmount
            _view.InterestAmount = loan.InterestAmount
            _view.TotalLoanAmount = loan.TotalLoanAmount
            _view.PrincipalAmortization = loan.PrincipalAmortization
            _view.InterestAmortization = loan.InterestAmortization
            _view.TotalAmortization = loan.TotalAmortization
            _view.Remark = loan.Remark
            _view.StatusText = loan.Status

        End Sub

    End Class

End Namespace