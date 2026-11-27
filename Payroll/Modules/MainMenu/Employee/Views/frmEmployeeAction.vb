' ============================================================
' Modules/MainMenu/Employee/Views/frmEmployeeAction.vb
' ============================================================
' Phase 2: isang dialog para sa Regularize / Promote / Transfer / Resign / Terminate / Retire.
' Walang Presenter (parehong dahilan ng frmEmployeeLoansQuickView): ang dialog ay tumatawag lang sa
' IEmployeeService.ApplyMovementAsync, at ang LAHAT ng patakaran (valid ba, kailan available) ay nasa
' Employee.Models.EmployeeMovementRules - iisang batayan ng menu, ng dialog, at ng repository.
' Isang transaction ang pagsusulat (tblEmployeeEmployment + tblEmployee.IsActive + tblEmployeeMovement).
' ============================================================
Imports DevExpress.XtraEditors

Public Class frmEmployeeAction

    Private ReadOnly _service As Employee.Services.IEmployeeService
    Private ReadOnly _header As Employee.Models.EmployeeHeaderModel
    Private ReadOnly _movementType As String
    Private ReadOnly _activeLoanCount As Integer
    Private ReadOnly _activeLoanBalance As Decimal

    Private _employment As Employee.Models.EmployeeEmploymentModel
    Private _context As Employee.Models.MovementContext

    ' Salary adjustment lang: kasalukuyang Earnings at kung Daily Rate o Basic Salary ang input ng pay cycle niya.
    Private _earnings As Employee.Models.EmployeeEarningsModel
    Private _usesDailyRate As Boolean

    ' Pay cycle change (Actions > Change pay cycle, o ang opsyonal na bahagi ng Regularize / Promote): Rate Basis ng bawat cycle,
    ' at kung ihahandog ba ang pagpalit (may rate na, may Employee Salary, may ibang active cycle na mapagpipilian).
    Private _cycleBases As Dictionary(Of String, GlobalShared.Models.PayRateBasis)
    Private _cycleOffered As Boolean

    ' Minimum Wage Earner (tax-exempt): Change tax status, Salary adjustment, Transfer. Ang mungkahi ay galing sa minimum wage
    ' ng Wage Area ng branch (Settings > Payroll > Minimum Wage); ang checkbox pa rin ang masusunod (manual, gaya ng C1Pay).
    Private _mweOffered As Boolean
    Private _wageRef As Employee.Models.WageReference
    Private _mweUserTouched As Boolean
    Private _settingMwe As Boolean

    Public Sub New(service As Employee.Services.IEmployeeService,
                   header As Employee.Models.EmployeeHeaderModel,
                   movementType As String,
                   activeLoanCount As Integer,
                   activeLoanBalance As Decimal)

        InitializeComponent()

        _service = service
        _header = header
        _movementType = movementType
        _activeLoanCount = activeLoanCount
        _activeLoanBalance = activeLoanBalance
    End Sub

    ' =============================================
    ' LOAD
    ' =============================================
    Private Async Sub frmEmployeeAction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = DialogTitle()
        btnApply.Text = ApplyCaption()
        lblEmployee.Text = If(String.IsNullOrWhiteSpace(_header.EmployeeNo),
                              _header.FullName, $"{_header.FullName}  ({_header.EmployeeNo})")
        lblRemarks.Text = If(RemarksRequired(), "Remarks *", "Remarks")
        dtpEffective.ApplyDisplayDateFormat()

        ' Ipakita agad ang tamang hugis; ang datos ay susunod.
        LayoutRows()

        Try
            _employment = Await _service.GetEmploymentAsync(_header.RecordId)
            If _employment Is Nothing Then
                XtraMessageBox.Show(Me, "This employee has no Employment record yet. Complete and save the Employment tab first.",
                                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Close()
                Return
            End If

            _context = Employee.Models.MovementContext.FromEmployment(_employment, _header.IsActive)

            If Employee.Models.MovementTypes.IsSalary(_movementType) Then
                Await LoadSalaryContextAsync()
            End If

            If _movementType = Employee.Models.MovementTypes.PayCycleChange OrElse
               SupportsCycleOption() Then
                Await LoadPayCycleContextAsync()
            End If

            If SupportsMwe() Then Await LoadMweContextAsync()

            If _movementType = Employee.Models.MovementTypes.ContractEndChange Then
                lblNewDate.Text = "New Contract End *"
                dtpNewDate.ApplyDisplayDateFormat()
                dtpNewDate.EditValue = If(_employment.ContractEndDate.HasValue, CObj(_employment.ContractEndDate.Value), Nothing)
                If _employment.DateJoined.HasValue Then dtpNewDate.Properties.MinValue = _employment.DateJoined.Value.Date
            End If

            lblRemarks.Text = If(RemarksRequired(), "Remarks *", "Remarks")

            ' Nagbago na ba ang kalagayan mula nang buksan ang menu? (hal. ibang user ang nag-resign sa kanya)
            Dim blocked = Employee.Models.EmployeeMovementRules.UnavailableReason(_movementType, _context)
            If blocked.Length > 0 Then
                XtraMessageBox.Show(Me, blocked, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Close()
                Return
            End If

            ' Effective date: hindi mas maaga sa Date Joined (sa Rehire: mas huli sa huling pag-alis), hindi sa hinaharap.
            If _movementType = Employee.Models.MovementTypes.Rehire Then
                Dim lastSep = Employee.Models.EmployeeMovementRules.LastSeparationDate(_context)
                If lastSep.HasValue AndAlso lastSep.Value.Date.AddDays(1) <= Date.Today Then
                    dtpEffective.Properties.MinValue = lastSep.Value.Date.AddDays(1)
                End If
                lblEffective.Text = "Rehire date *"
                cboEmploymentType.Properties.Items.Clear()
                cboEmploymentType.Properties.Items.AddRange(Employee.Models.EmployeeMovementRules.EmploymentTypes)
                cboEmploymentType.SelectedIndex = -1   ' walang default: HR ang pipili
            ElseIf _employment.DateJoined.HasValue AndAlso _employment.DateJoined.Value.Date <= Date.Today Then
                dtpEffective.Properties.MinValue = _employment.DateJoined.Value.Date
            End If
            dtpEffective.Properties.MaxValue = Date.Today
            dtpEffective.EditValue = Date.Today

            Select Case _movementType
                Case Employee.Models.MovementTypes.Promote
                    BindLookup(lookPosition, Await _service.GetLookupsAsync("tblPosition"))
                    BindLookup(lookCategory, Await _service.GetLookupsAsync("tblCategoryCode"))
                    BindLookup(lookJobClass, Await _service.GetLookupsAsync("tblJobClass"))
                    lookPosition.EditValue = ToObj(_employment.PositionId)
                    lookCategory.EditValue = ToObj(_employment.CategoryId)
                    lookJobClass.EditValue = ToObj(_employment.JobClassId)

                Case Employee.Models.MovementTypes.Transfer
                    BindLookup(lookBranch, Await _service.GetLookupsAsync("tblBranch"))
                    BindLookup(lookDepartment, Await _service.GetLookupsAsync("tblDepartment"))
                    lookBranch.EditValue = ToObj(_employment.BranchId)
                    lookDepartment.EditValue = ToObj(_employment.DepartmentId)
            End Select

            lblInfo.Text = InfoText()
            LayoutRows()
            btnApply.Enabled = True

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        End Try
    End Sub

    ' Binabasa ang kasalukuyang Earnings at ang rate basis ng Pay Cycle (parehong paraan ng Earnings tab).
    Private Async Function LoadSalaryContextAsync() As Task
        _earnings = Await _service.GetEarningsAsync(_header.RecordId)

        _usesDailyRate = False
        If _earnings IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(_earnings.PayCycle) Then
            Dim bases = Await _service.GetPayCycleRateBasesAsync()
            Dim basis As GlobalShared.Models.PayRateBasis = GlobalShared.Models.PayRateBasis.BasicSalary
            If bases.TryGetValue(GlobalShared.Models.PayCycleChoices.Normalize(_earnings.PayCycle), basis) Then
                _usesDailyRate = (basis = GlobalShared.Models.PayRateBasis.DailyRate)
            End If
        End If

        If _earnings IsNot Nothing Then
            _context.UsesDailyRate = _usesDailyRate
            _context.CurrentBasicSalary = _earnings.BasicSalary
            _context.CurrentDailyRate = _earnings.DailyRate
            _context.HasPayRate = (_earnings.BasicSalary.HasValue AndAlso _earnings.BasicSalary.Value > 0D) OrElse
                                  (_earnings.DailyRate.HasValue AndAlso _earnings.DailyRate.Value > 0D)
        End If

        Dim rateName As String = If(_usesDailyRate, "Daily Rate", "Basic Salary")
        Dim current As Decimal? = If(_usesDailyRate, _context.CurrentDailyRate, _context.CurrentBasicSalary)
        lblCurrentRate.Text = "Current " & rateName
        lblCurrentRateValue.Text = If(current.HasValue, "₱" & current.Value.ToString("N2"), "(none)")
        lblNewRate.Text = "New " & rateName & " *"
        spnNewRate.EditValue = Nothing
    End Function

    ' Pay cycle change: binabasa ang kasalukuyang Earnings, ang Rate Basis ng bawat cycle at ang mga Active na cycle na
    ' mapagpipilian (hindi kasama ang kasalukuyan). Sa Regularize / Promote, ihahandog lang ang pagpalit kung may rate na (naka-lock na
    ' ang Earnings; kung wala pa, bukas pa ang Pay Cycle sa Earnings tab) at may "Employee Salary" (Can Edit).
    Private Async Function LoadPayCycleContextAsync() As Task
        _cycleOffered = False
        _earnings = Await _service.GetEarningsAsync(_header.RecordId)
        If _earnings Is Nothing Then Return   ' walang Earnings: Change pay cycle ay haharangin ng rules; Regularize ay walang opsyon

        _cycleBases = Await _service.GetPayCycleRateBasesAsync()
        Dim activeCycles = Await _service.GetActivePayCyclesAsync()

        Dim currentName As String = GlobalShared.Models.PayCycleChoices.Normalize(_earnings.PayCycle)
        Dim basis As GlobalShared.Models.PayRateBasis = GlobalShared.Models.PayRateBasis.BasicSalary
        If currentName.Length > 0 Then _cycleBases.TryGetValue(currentName, basis)
        Dim currentDaily As Boolean = (basis = GlobalShared.Models.PayRateBasis.DailyRate)

        _context.PayCycle = _earnings.PayCycle
        _context.UsesDailyRate = currentDaily
        _context.CurrentBasicSalary = _earnings.BasicSalary
        _context.CurrentDailyRate = _earnings.DailyRate
        _context.HasPayRate = (_earnings.BasicSalary.HasValue AndAlso _earnings.BasicSalary.Value > 0D) OrElse
                              (_earnings.DailyRate.HasValue AndAlso _earnings.DailyRate.Value > 0D)

        cboNewCycle.Properties.Items.Clear()
        For Each cycle In activeCycles
            If Not Employee.Models.EmployeeMovementRules.SamePayCycle(cycle, _earnings.PayCycle) Then
                cboNewCycle.Properties.Items.Add(cycle)
            End If
        Next
        cboNewCycle.SelectedIndex = -1

        Dim canEditSalary As Boolean =
            GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Salary)
        _cycleOffered = _context.HasPayRate AndAlso canEditSalary AndAlso cboNewCycle.Properties.Items.Count > 0

        Dim currentRate As Decimal? = If(currentDaily, _earnings.DailyRate, _earnings.BasicSalary)
        Dim rateName As String = If(currentDaily, "Daily Rate", "Basic Salary")
        lblCurrentRate.Text = "Current Pay Cycle"
        lblCurrentRateValue.Text = If(currentName.Length = 0, "(none)", currentName) & " - " & rateName & " " &
                                   If(currentRate.HasValue, "₱" & currentRate.Value.ToString("N2"), "(none)")
        lblNewRate.Text = "New rate *"
        spnNewRate.EditValue = Nothing
    End Function

    ' =============================================
    ' MINIMUM WAGE EARNER (tax-exempt)
    ' =============================================
    Private Function SupportsMwe() As Boolean
        Return _movementType = Employee.Models.MovementTypes.TaxStatusChange OrElse
               _movementType = Employee.Models.MovementTypes.SalaryAdjustment OrElse
               _movementType = Employee.Models.MovementTypes.Transfer
    End Function

    Private Async Function LoadMweContextAsync() As Task
        _mweOffered = False
        If _earnings Is Nothing Then _earnings = Await _service.GetEarningsAsync(_header.RecordId)
        If _earnings Is Nothing Then Return

        _context.MinimumWage = _earnings.MinimumWage
        _context.HasPayRate = (_earnings.BasicSalary.HasValue AndAlso _earnings.BasicSalary.Value > 0D) OrElse
                              (_earnings.DailyRate.HasValue AndAlso _earnings.DailyRate.Value > 0D)

        Dim canEditSalary As Boolean =
            GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Salary)
        _mweOffered = canEditSalary AndAlso _context.HasPayRate

        _wageRef = Await _service.GetWageReferenceAsync(_employment.BranchId, Date.Today)

        _settingMwe = True
        chkMinimumWage.Checked = _earnings.MinimumWage
        _settingMwe = False
        _mweUserTouched = False

        If _movementType = Employee.Models.MovementTypes.TaxStatusChange Then
            ' Ang mungkahi agad (kung alam), para isang click na lang.
            Dim suggestion = GlobalShared.Helpers.MinimumWageRules.SuggestedMwe(
                GlobalShared.Helpers.MinimumWageRules.Compare(_earnings.DailyRate, _wageRef.Rate))
            _settingMwe = True
            chkMinimumWage.Checked = If(suggestion.HasValue AndAlso suggestion.Value <> _earnings.MinimumWage,
                                        suggestion.Value, Not _earnings.MinimumWage)
            _settingMwe = False
            lblCurrentRate.Text = "Current tax status"
            lblCurrentRateValue.Text = Employee.Models.EmployeeMovementRules.TaxStatusText(_earnings.MinimumWage)
        End If
        UpdateMweHint()
    End Function

    ''' <summary>Ang daily equivalent na ihahambing sa minimum wage: bagong rate (Salary adjustment) o ang kasalukuyan.</summary>
    Private Function DailyEquivalentForMwe() As Decimal?
        If _earnings Is Nothing Then Return Nothing
        If _movementType = Employee.Models.MovementTypes.SalaryAdjustment Then
            Dim entered = ReadNewRate()
            If entered.HasValue AndAlso entered.Value > 0D Then
                Dim rates = Employee.Models.EmployeeMovementRules.ComputeAdjustedRates(
                    _usesDailyRate, entered.Value, _earnings.DaysInYear, _earnings.WorkHourPer,
                    _earnings.BasicSalary, _earnings.DailyRate, _earnings.HourlyRate)
                Return rates.DailyRate
            End If
        End If
        Return _earnings.DailyRate
    End Function

    ''' <summary>Ipakita ba ang checkbox: laging sa Change tax status; sa Salary adjustment kapag may access; sa Transfer kapag nagpalit ng branch.</summary>
    Private Function ShowMweRow() As Boolean
        If Not _mweOffered Then Return False
        If _movementType = Employee.Models.MovementTypes.Transfer Then
            Return Not Employee.Models.EmployeeMovementRules.SameId(_employment.BranchId, GetId(lookBranch))
        End If
        Return True
    End Function

    ''' <summary>Paliwanag + kusang mungkahi (hangga't hindi pa ginagalaw ng user ang checkbox).</summary>
    Private Sub UpdateMweHint()
        If _wageRef Is Nothing Then Return
        Dim daily = DailyEquivalentForMwe()
        lblMweHint.Text = GlobalShared.Helpers.MinimumWageRules.ExplainText(daily, _wageRef.Rate, _wageRef.WageArea)

        If _movementType <> Employee.Models.MovementTypes.TaxStatusChange AndAlso Not _mweUserTouched Then
            Dim suggestion = GlobalShared.Helpers.MinimumWageRules.SuggestedMwe(
                GlobalShared.Helpers.MinimumWageRules.Compare(daily, _wageRef.Rate))
            If suggestion.HasValue Then
                _settingMwe = True
                chkMinimumWage.Checked = suggestion.Value
                _settingMwe = False
            End If
        End If
    End Sub

    Private Sub chkMinimumWage_CheckedChanged(sender As Object, e As EventArgs) Handles chkMinimumWage.CheckedChanged
        If Not _settingMwe Then _mweUserTouched = True
    End Sub

    Private Sub spnNewRate_EditValueChanged(sender As Object, e As EventArgs) Handles spnNewRate.EditValueChanged
        If _movementType = Employee.Models.MovementTypes.SalaryAdjustment AndAlso _mweOffered Then UpdateMweHint()
    End Sub

    Private Async Sub lookBranch_EditValueChanged(sender As Object, e As EventArgs) Handles lookBranch.EditValueChanged
        If _movementType <> Employee.Models.MovementTypes.Transfer OrElse Not _mweOffered OrElse _employment Is Nothing Then Return
        Try
            _wageRef = Await _service.GetWageReferenceAsync(GetId(lookBranch), Date.Today)
            If Employee.Models.EmployeeMovementRules.SameId(_employment.BranchId, GetId(lookBranch)) Then
                ' Bumalik sa dating branch: ibalik ang kasalukuyang status.
                _settingMwe = True
                chkMinimumWage.Checked = _earnings.MinimumWage
                _settingMwe = False
                _mweUserTouched = False
            Else
                UpdateMweHint()
            End If
        Catch ex As Exception
            lblMweHint.Text = ex.Message
        End Try
        LayoutRows()
    End Sub

    ''' <summary>Ang bagong Minimum Wage Earner na isasama sa request (Nothing = walang pagbabago o hindi ipinakita).</summary>
    Private Function MweRequestValue() As Boolean?
        If Not ShowMweRow() OrElse _earnings Is Nothing Then Return Nothing
        If _movementType = Employee.Models.MovementTypes.TaxStatusChange Then Return chkMinimumWage.Checked
        If chkMinimumWage.Checked = _earnings.MinimumWage Then Return Nothing
        Return chkMinimumWage.Checked
    End Function

    ' Daily Rate ba ang pay basis ng napiling BAGONG cycle? (Basic Salary kung hindi pa napili / hindi kilala)
    Private Function SelectedCycleUsesDaily() As Boolean
        Dim basis As GlobalShared.Models.PayRateBasis = GlobalShared.Models.PayRateBasis.BasicSalary
        Dim cycleName As String = GlobalShared.Models.PayCycleChoices.Normalize(cboNewCycle.Text)
        If _cycleBases IsNot Nothing AndAlso cycleName.Length > 0 Then _cycleBases.TryGetValue(cycleName, basis)
        Return basis = GlobalShared.Models.PayRateBasis.DailyRate
    End Function

    ' Magpapalit ba ng Pay Cycle ang request na ito? (Change pay cycle: laging; Regularize: kung naka-tick)
    Private Function WantsCycleChange() As Boolean
        If _movementType = Employee.Models.MovementTypes.PayCycleChange Then Return True
        Return SupportsCycleOption() AndAlso _cycleOffered AndAlso chkChangePayCycle.Checked
    End Function

    ' Mga action na may opsyonal na "Also change Pay Cycle": Regularize at Promote (ang Change pay cycle ay laging mayroon).
    Private Function SupportsCycleOption() As Boolean
        Return _movementType = Employee.Models.MovementTypes.Regularize OrElse
               _movementType = Employee.Models.MovementTypes.Promote
    End Function

    Private Sub cboNewCycle_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboNewCycle.SelectedIndexChanged
        ' Ang rate na hihingin ay ayon sa pay basis ng BAGONG cycle (Basic Salary o Daily Rate); burahin ang naunang halaga.
        If String.IsNullOrWhiteSpace(cboNewCycle.Text) Then
            lblNewRate.Text = "New rate *"
        Else
            lblNewRate.Text = "New " & If(SelectedCycleUsesDaily(), "Daily Rate", "Basic Salary") & " *"
        End If
        spnNewRate.EditValue = Nothing
    End Sub

    Private Sub chkChangePayCycle_CheckedChanged(sender As Object, e As EventArgs) Handles chkChangePayCycle.CheckedChanged
        LayoutRows()
    End Sub

    ' Ang BAGONG cycle ay galing sa listahan ng mga Active (nasa combo lang ang mga iyon); inuulit ng repository ang pagsusuri
    ' sa loob ng transaction laban sa tblPayCycle.
    Private Sub FillNewCycleContext()
        Dim basis As GlobalShared.Models.PayRateBasis = GlobalShared.Models.PayRateBasis.BasicSalary
        Dim cycleName As String = GlobalShared.Models.PayCycleChoices.Normalize(cboNewCycle.Text)
        Dim found As Boolean = _cycleBases IsNot Nothing AndAlso cycleName.Length > 0 AndAlso _cycleBases.TryGetValue(cycleName, basis)

        _context.NewCycleFound = found
        _context.NewCycleActive = found
        _context.NewCycleUsesDailyRate = found AndAlso (basis = GlobalShared.Models.PayRateBasis.DailyRate)
    End Sub

    Private Function ReadNewRate() As Decimal?
        If spnNewRate.EditValue Is Nothing OrElse spnNewRate.EditValue Is DBNull.Value Then Return Nothing
        Return Convert.ToDecimal(spnNewRate.EditValue)
    End Function

    ' =============================================
    ' TEXTS (ayon sa action)
    ' =============================================
    Private Function DialogTitle() As String
        Select Case _movementType
            Case Employee.Models.MovementTypes.Regularize : Return "Regularize Employee"
            Case Employee.Models.MovementTypes.Promote : Return "Promote Employee"
            Case Employee.Models.MovementTypes.Transfer : Return "Transfer Employee"
            Case Employee.Models.MovementTypes.Resign : Return "Record Resignation"
            Case Employee.Models.MovementTypes.Terminate : Return "Record Termination"
            Case Employee.Models.MovementTypes.Retire : Return "Record Retirement"
            Case Employee.Models.MovementTypes.Rehire : Return "Rehire Employee"
            Case Employee.Models.MovementTypes.EndOfContract : Return "Record End of Contract"
            Case Employee.Models.MovementTypes.SalaryAdjustment : Return "Salary Adjustment"
            Case Employee.Models.MovementTypes.PayCycleChange : Return "Change Pay Cycle"
            Case Employee.Models.MovementTypes.ConvertToProbationary : Return "Convert to Probationary"
            Case Employee.Models.MovementTypes.ContractEndChange : Return "Change Contract End"
            Case Employee.Models.MovementTypes.TaxStatusChange : Return "Change Tax Status (Minimum Wage Earner)"
            Case Else : Return "Employee Action"
        End Select
    End Function

    Private Function ApplyCaption() As String
        Select Case _movementType
            Case Employee.Models.MovementTypes.Regularize : Return "Regularize"
            Case Employee.Models.MovementTypes.Promote : Return "Promote"
            Case Employee.Models.MovementTypes.Transfer : Return "Transfer"
            Case Employee.Models.MovementTypes.Resign : Return "Resign"
            Case Employee.Models.MovementTypes.Terminate : Return "Terminate"
            Case Employee.Models.MovementTypes.Retire : Return "Retire"
            Case Employee.Models.MovementTypes.Rehire : Return "Rehire"
            Case Employee.Models.MovementTypes.EndOfContract : Return "End Contract"
            Case Employee.Models.MovementTypes.SalaryAdjustment : Return "Adjust Salary"
            Case Employee.Models.MovementTypes.PayCycleChange : Return "Change Cycle"
            Case Employee.Models.MovementTypes.ConvertToProbationary : Return "Convert"
            Case Employee.Models.MovementTypes.ContractEndChange : Return "Save"
            Case Employee.Models.MovementTypes.TaxStatusChange : Return "Save"
            Case Else : Return "Apply"
        End Select
    End Function

    Private Function RemarksRequired() As Boolean
        If _movementType = Employee.Models.MovementTypes.Rehire Then
            ' Required lang kapag na-terminate ang huling pag-alis (hula: para may dahilan/approval na nakatala)
            Return _context IsNot Nothing AndAlso Employee.Models.EmployeeMovementRules.LastSeparationKind(_context) = "Terminated"
        End If
        ' End of Contract: optional ang Remarks. Salary adjustment: required (dahilan ng pagbabago ng sahod).
        Return _movementType = Employee.Models.MovementTypes.Resign OrElse
               _movementType = Employee.Models.MovementTypes.Terminate OrElse
               _movementType = Employee.Models.MovementTypes.SalaryAdjustment OrElse
               _movementType = Employee.Models.MovementTypes.PayCycleChange OrElse
               _movementType = Employee.Models.MovementTypes.TaxStatusChange
    End Function

    Private Function InfoText() As String
        Select Case _movementType
            Case Employee.Models.MovementTypes.Regularize
                Dim current = If(String.IsNullOrWhiteSpace(_employment.EmployeeStatus), "not set", _employment.EmployeeStatus.Trim())
                Dim info = $"Changes Employment Type from {current} to Regular and sets Date Regularization to the effective date."
                If _cycleOffered Then
                    info &= " To move the employee to another Pay Cycle in the same step (for example Daily to SemiMonthly), " &
                            "tick ""Also change Pay Cycle"" and enter the rate of the new cycle."
                End If
                Return info

            Case Employee.Models.MovementTypes.PayCycleChange
                Return "Changes the Pay Cycle in the Earnings tab and sets the Basic Salary or Daily Rate of the new cycle; the " &
                       "Daily and Hourly rates are recomputed. The old cycle and rate stay in History. It applies to payroll that is " &
                       "processed from now on - cutoffs already processed are not recomputed. Not allowed while the employee still " &
                       "has input or results in a Draft or Processed cutoff of the current Pay Cycle."

            Case Employee.Models.MovementTypes.Promote
                If _cycleOffered Then
                    Return "Changes Position (and Category / Job Class if needed) and sets Date Last Promoted. " &
                           "Salary is not changed here (use Actions > Salary adjustment), unless you tick ""Also change Pay Cycle"" " &
                           "and enter the rate of the new cycle."
                End If
                Return "Changes Position (and Category / Job Class if needed) and sets Date Last Promoted. " &
                       "Salary is not changed here."

            Case Employee.Models.MovementTypes.Transfer
                Return "Changes Branch and/or Department and sets Date Last Transferred. When the new store is in another " &
                       "Wage Area, the Minimum Wage Earner (tax-exempt) status is checked against that area's minimum wage."

            Case Employee.Models.MovementTypes.ConvertToProbationary
                Dim current = If(String.IsNullOrWhiteSpace(_employment.EmployeeStatus), "not set", _employment.EmployeeStatus.Trim())
                Return $"Changes Employment Type from {current} to Probationary. The probation period starts on the effective date " &
                       "(not on Date Joined), so the regularization reminder counts from that date."

            Case Employee.Models.MovementTypes.ContractEndChange
                Dim currentEnd = If(_employment.ContractEndDate.HasValue, _employment.ContractEndDate.Value.ToString("MM/dd/yyyy"), "(none)")
                Return $"Changes the planned Contract End (now {currentEnd}), e.g. for an extension. This is only for the reminder: " &
                       "the employee stays active until Actions > End of Contract is recorded."

            Case Employee.Models.MovementTypes.TaxStatusChange
                Return "Sets whether withholding tax is exempt as a Minimum Wage Earner (the checkbox in the Earnings tab). The minimum wage " &
                       "shown below comes from the Wage Area of the employee's branch (Settings > Payroll > Minimum Wage)."

            Case Employee.Models.MovementTypes.Resign
                Return "Sets Date Resigned and marks the employee Inactive. The Employment Type is kept on record."

            Case Employee.Models.MovementTypes.Terminate
                Return "Sets Date Terminated and marks the employee Inactive. The Employment Type is kept on record."

            Case Employee.Models.MovementTypes.Retire
                Return "Sets Date Retired and marks the employee Inactive. The Employment Type is kept on record."

            Case Employee.Models.MovementTypes.EndOfContract
                Return "Sets Date End of Contract and marks the employee Inactive. The Employment Type is kept on record. " &
                       "If the employee is renewed or returns later, use Actions > Rehire."

            Case Employee.Models.MovementTypes.SalaryAdjustment
                Return "Changes the " & If(_usesDailyRate, "Daily Rate", "Basic Salary") & " in the Earnings tab and recomputes the " &
                       "Daily and Hourly rates. The old amount stays in History. The new rate is used by payroll that is " &
                       "processed from now on - cutoffs already processed are not recomputed, and no retro pay is computed."

            Case Employee.Models.MovementTypes.Rehire
                Return "Starts a new service period: the rehire date becomes the new Date Joined (the first Date Joined is " &
                       "kept as Original Date Joined) and the employee becomes Active. The previous separation is cleared " &
                       "from the Employment tab but stays in History. Salary, loans and other details are not changed - " &
                       "please review the Earnings tab."

            Case Else
                Return String.Empty
        End Select
    End Function

    Private Function WarningText() As String
        If (Employee.Models.MovementTypes.IsSeparation(_movementType) OrElse
            _movementType = Employee.Models.MovementTypes.Rehire) AndAlso _activeLoanCount > 0 Then
            Return $"This employee still has {_activeLoanCount} active {If(_activeLoanCount = 1, "loan", "loans")} " &
                   $"with a balance of ₱{_activeLoanBalance:N2}. Loans are not changed by this action."
        End If
        Return String.Empty
    End Function

    ' =============================================
    ' LAYOUT: ipakita lang ang mga hilera ng action na ito, isalansan pababa
    ' =============================================
    Private Sub LayoutRows()
        lblInfo.Text = If(lblInfo.Text, String.Empty)
        Dim infoHeight As Integer = 0
        If lblInfo.Text.Length > 0 Then
            infoHeight = TextRenderer.MeasureText(lblInfo.Text, lblInfo.Font,
                                                  New Size(lblInfo.Width, 0), TextFormatFlags.WordBreak).Height + 4
        End If
        lblInfo.Height = infoHeight

        Dim y As Integer = lblInfo.Top + infoHeight + If(infoHeight > 0, 14, 0)

        ' Itago muna ang mga hilera ng Pay cycle; ang PlaceRow ang magpapakita ng kailangan.
        For Each ctl As Control In New Control() {chkChangePayCycle, lblCurrentRate, lblCurrentRateValue,
                                                  lblNewCycle, cboNewCycle, lblNewRate, spnNewRate}
            ctl.Visible = False
        Next

        PlaceRow(y, lblEffective, dtpEffective, 20)

        Select Case _movementType
            Case Employee.Models.MovementTypes.Regularize
                PlaceCycleOption(y)

            Case Employee.Models.MovementTypes.PayCycleChange
                PlaceCycleRows(y)

            Case Employee.Models.MovementTypes.Promote
                PlaceRow(y, lblPosition, lookPosition, 20)
                PlaceRow(y, lblCategory, lookCategory, 20)
                PlaceRow(y, lblJobClass, lookJobClass, 20)
                PlaceCycleOption(y)

            Case Employee.Models.MovementTypes.Transfer
                PlaceRow(y, lblBranch, lookBranch, 20)
                PlaceRow(y, lblDepartment, lookDepartment, 20)

            Case Employee.Models.MovementTypes.Rehire
                PlaceRow(y, lblEmploymentType, cboEmploymentType, 20)

            Case Employee.Models.MovementTypes.SalaryAdjustment
                PlaceRow(y, lblCurrentRate, lblCurrentRateValue, 20)
                PlaceRow(y, lblNewRate, spnNewRate, 20)

            Case Employee.Models.MovementTypes.ContractEndChange
                PlaceRow(y, lblNewDate, dtpNewDate, 20)

            Case Employee.Models.MovementTypes.TaxStatusChange
                PlaceRow(y, lblCurrentRate, lblCurrentRateValue, 20)
        End Select

        PlaceMweRows(y)

        PlaceRow(y, lblRemarks, memoRemarks, 70)

        Dim warning = WarningText()
        If warning.Length > 0 Then
            lblWarning.Text = warning
            Dim h = TextRenderer.MeasureText(warning, lblWarning.Font,
                                             New Size(lblWarning.Width - 16, 0), TextFormatFlags.WordBreak).Height + 14
            lblWarning.Location = New Point(20, y)
            lblWarning.Height = h
            lblWarning.Visible = True
            y += h + 10
        Else
            lblWarning.Visible = False
        End If

        btnApply.Top = y + 6
        btnCancel.Top = y + 6
        ClientSize = New Size(ClientSize.Width, btnApply.Bottom + 16)
    End Sub

    ' Ang "Minimum Wage Earner" checkbox at ang paliwanag sa ilalim nito.
    Private Sub PlaceMweRows(ByRef y As Integer)
        Dim show = SupportsMwe() AndAlso ShowMweRow()
        chkMinimumWage.Visible = show
        lblMweHint.Visible = show
        If Not show Then Return

        chkMinimumWage.Location = New Point(148, y)
        y += 24
        Dim text = If(lblMweHint.Text, String.Empty)
        Dim h = If(text.Length = 0, 0, TextRenderer.MeasureText(text, lblMweHint.Font, New Size(300, 0), TextFormatFlags.WordBreak).Height + 4)
        lblMweHint.Location = New Point(150, y)
        lblMweHint.Size = New Size(300, h)
        y += h + 10
    End Sub

    ' Ang checkbox na "Also change Pay Cycle" (Regularize / Promote), at ang mga hilera ng cycle kapag naka-tick.
    Private Sub PlaceCycleOption(ByRef y As Integer)
        If Not _cycleOffered Then Return

        chkChangePayCycle.Visible = True
        chkChangePayCycle.Location = New Point(20, y)
        chkChangePayCycle.Size = New Size(430, 20)
        y += 30
        If chkChangePayCycle.Checked Then PlaceCycleRows(y)
    End Sub

    ' Kasalukuyang Pay Cycle at rate (basa lang), ang bagong Pay Cycle, at ang bagong rate.
    Private Sub PlaceCycleRows(ByRef y As Integer)
        PlaceRow(y, lblCurrentRate, lblCurrentRateValue, 20)
        PlaceRow(y, lblNewCycle, cboNewCycle, 20)
        PlaceRow(y, lblNewRate, spnNewRate, 20)
    End Sub

    Private Shared Sub PlaceRow(ByRef y As Integer, lbl As Control, editor As Control, editorHeight As Integer)
        lbl.Visible = True
        editor.Visible = True
        lbl.Location = New Point(20, y + 3)
        editor.Location = New Point(150, y)
        editor.Size = New Size(300, editorHeight)
        y += editorHeight + 10
    End Sub

    ' =============================================
    ' LOOKUPS
    ' =============================================
    Private Shared Sub BindLookup(edit As LookUpEdit, data As List(Of GlobalShared.Models.LookupModel))
        With edit.Properties
            .DataSource = data
            .ValueMember = "Id"
            .DisplayMember = "Name"
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 70))
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name"))
            .NullText = String.Empty
        End With
    End Sub

    Private Shared Function ToObj(value As Integer?) As Object
        If value.HasValue Then Return CObj(value.Value)
        Return Nothing
    End Function

    Private Shared Function GetId(edit As LookUpEdit) As Integer?
        If edit.EditValue Is Nothing OrElse edit.EditValue Is DBNull.Value Then Return Nothing
        Return CInt(edit.EditValue)
    End Function

    ' =============================================
    ' APPLY
    ' =============================================
    Private Function BuildRequest() As Employee.Models.EmployeeMovementRequest
        Dim req As New Employee.Models.EmployeeMovementRequest With {
            .RecordId = _header.RecordId,
            .MovementType = _movementType,
            .EffectiveDate = CDate(dtpEffective.EditValue),
            .Remarks = memoRemarks.Text
        }

        Select Case _movementType
            Case Employee.Models.MovementTypes.Promote
                req.NewPositionId = GetId(lookPosition)
                req.NewCategoryId = GetId(lookCategory)
                req.NewJobClassId = GetId(lookJobClass)

            Case Employee.Models.MovementTypes.Transfer
                req.NewBranchId = GetId(lookBranch)
                req.NewDepartmentId = GetId(lookDepartment)

            Case Employee.Models.MovementTypes.Rehire
                req.NewEmploymentType = cboEmploymentType.Text.Trim()

            Case Employee.Models.MovementTypes.ContractEndChange
                If dtpNewDate.EditValue IsNot Nothing AndAlso dtpNewDate.EditValue IsNot DBNull.Value Then
                    req.NewContractEndDate = CDate(dtpNewDate.EditValue).Date
                End If

            Case Employee.Models.MovementTypes.SalaryAdjustment
                Dim entered As Decimal? = Nothing
                If spnNewRate.EditValue IsNot Nothing AndAlso spnNewRate.EditValue IsNot DBNull.Value Then
                    entered = Convert.ToDecimal(spnNewRate.EditValue)
                End If
                If _usesDailyRate Then
                    req.NewDailyRate = entered
                Else
                    req.NewBasicSalary = entered
                End If
        End Select

        If SupportsMwe() Then req.NewMinimumWage = MweRequestValue()

        ' Pay cycle change (o Regularize / Promote na naka-tick): ang rate ay ayon sa pay basis ng BAGONG cycle.
        If WantsCycleChange() Then
            req.NewPayCycle = cboNewCycle.Text.Trim()
            If SelectedCycleUsesDaily() Then
                req.NewDailyRate = ReadNewRate()
            Else
                req.NewBasicSalary = ReadNewRate()
            End If
        End If

        Return req
    End Function

    ''' <summary>Dagdag na pangungusap sa Confirm kapag magbabago rin ang Minimum Wage Earner.</summary>
    Private Function MweChangeSentence(req As Employee.Models.EmployeeMovementRequest) As String
        If Not req.NewMinimumWage.HasValue OrElse _earnings Is Nothing Then Return String.Empty
        Return vbCrLf & vbCrLf & "Tax status will also change: " &
               Employee.Models.EmployeeMovementRules.TaxStatusText(_earnings.MinimumWage) & " -> " &
               Employee.Models.EmployeeMovementRules.TaxStatusText(req.NewMinimumWage.Value) & " (separate History row)."
    End Function

    Private Sub SetBusy(busy As Boolean)
        btnApply.Enabled = Not busy
        btnCancel.Enabled = Not busy
        UseWaitCursor = busy
    End Sub

    Private Async Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If _employment Is Nothing OrElse _context Is Nothing Then Return

        If dtpEffective.EditValue Is Nothing OrElse dtpEffective.EditValue Is DBNull.Value Then
            XtraMessageBox.Show(Me, "Effective date is required.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            dtpEffective.Focus()
            Return
        End If

        ' Naka-tick ang "Also change Pay Cycle" pero walang napili: huwag hayaang mag-Regularize nang walang pagpalit ng cycle.
        If WantsCycleChange() AndAlso String.IsNullOrWhiteSpace(cboNewCycle.Text) Then
            XtraMessageBox.Show(Me, "Choose the new Pay Cycle" &
                                If(_movementType <> Employee.Models.MovementTypes.PayCycleChange,
                                   ", or untick ""Also change Pay Cycle"".", "."),
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboNewCycle.Focus()
            Return
        End If

        Dim req = BuildRequest()
        If WantsCycleChange() Then FillNewCycleContext()

        ' Parehong patakaran ng repository (na uulit-ulitin sa loob ng transaction): mabilis na mensahe bago magsulat.
        Dim problem = Employee.Models.EmployeeMovementRules.Validate(req, _context, Date.Today)
        If problem.Length > 0 Then
            XtraMessageBox.Show(Me, problem, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Employee.Models.MovementTypes.IsSeparation(_movementType) Then
            Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
            Dim verb As String = If(_movementType = Employee.Models.MovementTypes.EndOfContract,
                                    "Record End of Contract for", ApplyCaption())
            Dim prompt = $"{verb} {who} effective {req.EffectiveDate:MM/dd/yyyy}?" & vbCrLf & vbCrLf &
                         "The employee will be set to Inactive and removed from the Active list. " &
                         "Payroll still includes cutoffs that start on or before the effective date." & vbCrLf & vbCrLf &
                         "Actions > Reactivate can make the employee Active again, but the separation date stays " &
                         "on record until it is changed in the Employment tab."

            Dim answer = XtraMessageBox.Show(Me, prompt, Text,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)
            If answer <> DialogResult.Yes Then Return
        End If

        If Employee.Models.MovementTypes.IsSalary(_movementType) Then
            Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
            Dim rateName As String = If(_usesDailyRate, "Daily Rate", "Basic Salary")
            Dim newValue As Decimal = If(_usesDailyRate, req.NewDailyRate.GetValueOrDefault(), req.NewBasicSalary.GetValueOrDefault())
            Dim prompt = $"Change the {rateName} of {who} from {lblCurrentRateValue.Text} to ₱{newValue:N2} " &
                         $"(effective {req.EffectiveDate:MM/dd/yyyy})?" & vbCrLf & vbCrLf &
                         "The Daily and Hourly rates will be recomputed. This is recorded in History." &
                         MweChangeSentence(req)

            Dim answer = XtraMessageBox.Show(Me, prompt, Text,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)
            If answer <> DialogResult.Yes Then Return
        End If

        If WantsCycleChange() Then
            Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
            Dim usesDaily As Boolean = SelectedCycleUsesDaily()
            Dim rateName As String = If(usesDaily, "Daily Rate", "Basic Salary")
            Dim newValue As Decimal = If(usesDaily, req.NewDailyRate.GetValueOrDefault(), req.NewBasicSalary.GetValueOrDefault())
            Dim oldCycle As String = GlobalShared.Models.PayCycleChoices.Normalize(If(_earnings Is Nothing, "", _earnings.PayCycle))
            If oldCycle.Length = 0 Then oldCycle = "(none)"

            Dim lead As String
            Select Case _movementType
                Case Employee.Models.MovementTypes.Regularize
                    lead = $"Regularize {who} and change the Pay Cycle"
                Case Employee.Models.MovementTypes.Promote
                    lead = $"Promote {who} and change the Pay Cycle"
                Case Else
                    lead = $"Change the Pay Cycle of {who}"
            End Select
            Dim recorded As String = If(_movementType = Employee.Models.MovementTypes.PayCycleChange,
                                        "This is recorded in History.",
                                        "Both changes are recorded in History (the " &
                                        Employee.Models.MovementTypes.HistoryLabel(_movementType).ToLowerInvariant() &
                                        " and the pay cycle change).")
            Dim prompt = $"{lead} from {oldCycle} to {req.NewPayCycle} (effective {req.EffectiveDate:MM/dd/yyyy}), " &
                         $"with a new {rateName} of ₱{newValue:N2}?" & vbCrLf & vbCrLf &
                         "The Daily and Hourly rates will be recomputed. " & recorded

            Dim answer = XtraMessageBox.Show(Me, prompt, Text,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)
            If answer <> DialogResult.Yes Then Return
        End If

        If _movementType = Employee.Models.MovementTypes.Transfer AndAlso req.NewMinimumWage.HasValue Then
            If XtraMessageBox.Show(Me, "Transfer this employee?" & MweChangeSentence(req), Text,
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        End If

        If _movementType = Employee.Models.MovementTypes.ConvertToProbationary Then
            Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
            If XtraMessageBox.Show(Me, $"Convert {who} to Probationary effective {req.EffectiveDate:MM/dd/yyyy}?" & vbCrLf & vbCrLf &
                                   "The probation period will be counted from this date. This is recorded in History.", Text,
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return
        End If

        If _movementType = Employee.Models.MovementTypes.Rehire Then
            Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
            Dim prompt = $"Rehire {who} effective {req.EffectiveDate:MM/dd/yyyy} as {req.NewEmploymentType}?" & vbCrLf & vbCrLf &
                         "The rehire date becomes the new Date Joined, the employee becomes Active, and the previous " &
                         "separation date is cleared (it stays in History)."

            Dim answer = XtraMessageBox.Show(Me, prompt, Text,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)
            If answer <> DialogResult.Yes Then Return
        End If

        SetBusy(True)
        Try
            Await _service.ApplyMovementAsync(req)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            SetBusy(False)
            XtraMessageBox.Show(Me, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
