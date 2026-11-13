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
               _movementType = Employee.Models.MovementTypes.SalaryAdjustment
    End Function

    Private Function InfoText() As String
        Select Case _movementType
            Case Employee.Models.MovementTypes.Regularize
                Dim current = If(String.IsNullOrWhiteSpace(_employment.EmployeeStatus), "not set", _employment.EmployeeStatus.Trim())
                Return $"Changes Employment Type from {current} to Regular and sets Date Regularization to the effective date."

            Case Employee.Models.MovementTypes.Promote
                Return "Changes Position (and Category / Job Class if needed) and sets Date Last Promoted. " &
                       "Salary is not changed here."

            Case Employee.Models.MovementTypes.Transfer
                Return "Changes Branch and/or Department and sets Date Last Transferred."

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

        PlaceRow(y, lblEffective, dtpEffective, 20)

        Select Case _movementType
            Case Employee.Models.MovementTypes.Promote
                PlaceRow(y, lblPosition, lookPosition, 20)
                PlaceRow(y, lblCategory, lookCategory, 20)
                PlaceRow(y, lblJobClass, lookJobClass, 20)

            Case Employee.Models.MovementTypes.Transfer
                PlaceRow(y, lblBranch, lookBranch, 20)
                PlaceRow(y, lblDepartment, lookDepartment, 20)

            Case Employee.Models.MovementTypes.Rehire
                PlaceRow(y, lblEmploymentType, cboEmploymentType, 20)

            Case Employee.Models.MovementTypes.SalaryAdjustment
                PlaceRow(y, lblCurrentRate, lblCurrentRateValue, 20)
                PlaceRow(y, lblNewRate, spnNewRate, 20)
        End Select

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

        Return req
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

        Dim req = BuildRequest()

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
                         "The Daily and Hourly rates will be recomputed. This is recorded in History."

            Dim answer = XtraMessageBox.Show(Me, prompt, Text,
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                             MessageBoxDefaultButton.Button2)
            If answer <> DialogResult.Yes Then Return
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
