' ============================================================
' Modules/MainMenu/Employee/Views/frmEmployeeCorrection.vb
' ============================================================
' Actions > Correct record (ADMIN lang): pag-ayos ng MALING NAILAGAY sa mga field na naka-lock sa Employment tab
' (Employment Type, mga petsa, Position, Category, Job Class, Branch, Department).
' HINDI ito kapalit ng Promote / Transfer / Regularize / Resign...: ang tunay na pagbabago ay dumadaan pa rin doon.
' Walang Presenter (parehong dahilan ng frmEmployeeAction): tumatawag lang sa IEmployeeService.ApplyCorrectionAsync;
' ang lahat ng patakaran ay nasa Employee.Models.EmployeeEmploymentLockRules. Isang transaction; bawat field na
' nagbago ay isang row sa History (type "Correction") kasama ang Remarks mo.
' ============================================================
Imports DevExpress.XtraEditors

Public Class frmEmployeeCorrection

    Private ReadOnly _service As Employee.Services.IEmployeeService
    Private ReadOnly _header As Employee.Models.EmployeeHeaderModel
    Private _employment As Employee.Models.EmployeeEmploymentModel

    Public Sub New(service As Employee.Services.IEmployeeService,
                   header As Employee.Models.EmployeeHeaderModel)

        InitializeComponent()

        _service = service
        _header = header
    End Sub

    Private Function DateEditors() As DateEdit()
        Return {dtpJoined, dtpRegularization, dtpLastPromoted, dtpResigned, dtpRetired, dtpTerminated, dtpEndOfContract}
    End Function

    ' =============================================
    ' LOAD
    ' =============================================
    Private Async Sub frmEmployeeCorrection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Correct record"
        lblEmployee.Text = If(String.IsNullOrWhiteSpace(_header.EmployeeNo),
                              _header.FullName, $"{_header.FullName}  ({_header.EmployeeNo})")
        lblInfo.Text = "Administrator only. Use this only to fix WRONG entries. A real change (promotion, transfer, " &
                       "regularization, resignation...) must go through its own action so the History stays correct. " &
                       "Every changed field is recorded in History with your remarks."

        For Each d In DateEditors()
            d.ApplyDisplayDateFormat()
            d.Properties.MaxValue = Date.Today
        Next

        cboEmploymentType.Properties.Items.Clear()
        cboEmploymentType.Properties.Items.AddRange(Employee.Models.EmployeeMovementRules.EmploymentTypes)

        Try
            _employment = Await _service.GetEmploymentAsync(_header.RecordId)
            If _employment Is Nothing Then
                XtraMessageBox.Show(Me, "This employee has no Employment record yet. Complete and save the Employment tab first.",
                                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Close()
                Return
            End If

            BindLookup(lookPosition, Await _service.GetLookupsAsync("tblPosition"))
            BindLookup(lookCategory, Await _service.GetLookupsAsync("tblCategoryCode"))
            BindLookup(lookJobClass, Await _service.GetLookupsAsync("tblJobClass"))
            BindLookup(lookBranch, Await _service.GetLookupsAsync("tblBranch"))
            BindLookup(lookDepartment, Await _service.GetLookupsAsync("tblDepartment"))

            ' Kasalukuyang laman
            If Employee.Models.EmployeeMovementRules.IsKnownEmploymentType(_employment.EmployeeStatus) Then
                cboEmploymentType.EditValue = Employee.Models.EmployeeMovementRules.CanonicalEmploymentType(_employment.EmployeeStatus)
            Else
                cboEmploymentType.SelectedIndex = -1   ' lumang "Active"/blangko: Admin ang pipili
            End If

            SetDate(dtpJoined, _employment.DateJoined)
            SetDate(dtpRegularization, _employment.DateRegularization)
            SetDate(dtpLastPromoted, _employment.DateLastPromoted)
            SetDate(dtpResigned, _employment.DateResigned)
            SetDate(dtpRetired, _employment.DateRetired)
            SetDate(dtpTerminated, _employment.DateTerminated)
            SetDate(dtpEndOfContract, _employment.DateEndOfContract)

            ' Ang petsa ng pag-alis ay itatama lang kung mayroon na; hindi madadagdag dito (Resign / Rehire ang daan).
            dtpResigned.Enabled = _employment.DateResigned.HasValue
            dtpRetired.Enabled = _employment.DateRetired.HasValue
            dtpTerminated.Enabled = _employment.DateTerminated.HasValue
            dtpEndOfContract.Enabled = _employment.DateEndOfContract.HasValue

            lookPosition.EditValue = ToObj(_employment.PositionId)
            lookCategory.EditValue = ToObj(_employment.CategoryId)
            lookJobClass.EditValue = ToObj(_employment.JobClassId)
            lookBranch.EditValue = ToObj(_employment.BranchId)
            lookDepartment.EditValue = ToObj(_employment.DepartmentId)

            btnApply.Enabled = True

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        End Try
    End Sub

    ' =============================================
    ' HELPERS
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

    Private Shared Sub SetDate(edit As DateEdit, value As Date?)
        If value.HasValue Then
            edit.EditValue = CObj(value.Value.Date)
        Else
            edit.EditValue = Nothing
        End If
    End Sub

    Private Shared Function GetDate(edit As DateEdit) As Date?
        If edit.EditValue Is Nothing OrElse edit.EditValue Is DBNull.Value Then Return Nothing
        Return CDate(edit.EditValue).Date
    End Function

    ' =============================================
    ' APPLY
    ' =============================================
    Private Function BuildRequest() As Employee.Models.EmploymentCorrectionRequest
        Return New Employee.Models.EmploymentCorrectionRequest With {
            .RecordId = _header.RecordId,
            .EmployeeStatus = cboEmploymentType.Text.Trim(),
            .DateJoined = GetDate(dtpJoined),
            .DateRegularization = GetDate(dtpRegularization),
            .DateLastPromoted = GetDate(dtpLastPromoted),
            .DateResigned = GetDate(dtpResigned),
            .DateRetired = GetDate(dtpRetired),
            .DateTerminated = GetDate(dtpTerminated),
            .DateEndOfContract = GetDate(dtpEndOfContract),
            .PositionId = GetId(lookPosition),
            .CategoryId = GetId(lookCategory),
            .JobClassId = GetId(lookJobClass),
            .BranchId = GetId(lookBranch),
            .DepartmentId = GetId(lookDepartment),
            .Remarks = memoRemarks.Text
        }
    End Function

    Private Sub SetBusy(busy As Boolean)
        btnApply.Enabled = Not busy
        btnCancel.Enabled = Not busy
        UseWaitCursor = busy
    End Sub

    Private Async Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If _employment Is Nothing Then Return

        Dim req = BuildRequest()

        ' Parehong patakaran ng repository (inuulit sa loob ng transaction): mabilis na mensahe bago magsulat.
        Dim problem = Employee.Models.EmployeeEmploymentLockRules.ValidateCorrection(req, _employment, Date.Today)
        If problem.Length > 0 Then
            XtraMessageBox.Show(Me, problem, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim who = If(String.IsNullOrWhiteSpace(_header.FullName), "this employee", _header.FullName)
        Dim prompt = $"Apply this correction to {who}?" & vbCrLf & vbCrLf &
                     "Each changed field is recorded in History with your remarks. Only the employee record changes; " &
                     "payroll that was already processed is not recalculated."

        Dim answer = XtraMessageBox.Show(Me, prompt, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button2)
        If answer <> DialogResult.Yes Then Return

        SetBusy(True)
        Try
            Await _service.ApplyCorrectionAsync(req)
            DialogResult = DialogResult.OK
        Catch ex As Exception
            SetBusy(False)
            XtraMessageBox.Show(Me, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
