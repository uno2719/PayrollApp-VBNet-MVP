' ============================================================
' Modules/MainMenu/Employee/Views/frmFixTransaction.vb
' ============================================================
' Add / Edit ng isang Fix Transaction (Employee > Fix Transactions tab). Ibinabalik lang ang Result (model);
' ang pag-save ay ginagawa ng ucEmployeesFixTransactions sa pamamagitan ng IEmployeeFixTxnService (na nagva-validate ulit).
' ============================================================
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Public Class frmFixTransaction

    Private ReadOnly _options As List(Of FixTxnCodeOption)
    Private ReadOnly _others As List(Of EmployeeFixTxnModel)
    Private ReadOnly _employeeName As String
    Private ReadOnly _recordId As Integer
    Private ReadOnly _existing As EmployeeFixTxnModel
    Private _loading As Boolean

    ''' <summary>Ang na-fill na entry (valid na sa local check) kapag DialogResult.OK.</summary>
    Public ReadOnly Property Result As EmployeeFixTxnModel

    ''' <param name="existing">Nothing = bagong entry.</param>
    Public Sub New(options As List(Of FixTxnCodeOption), others As List(Of EmployeeFixTxnModel),
                   employeeName As String, recordId As Integer, existing As EmployeeFixTxnModel)
        InitializeComponent()
        _options = If(options, New List(Of FixTxnCodeOption)())
        _others = If(others, New List(Of EmployeeFixTxnModel)())
        _employeeName = employeeName
        _recordId = recordId
        _existing = existing
    End Sub

    Private Sub frmFixTransaction_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _loading = True
        Try
            lblEmployee.Text = If(String.IsNullOrWhiteSpace(_employeeName), "Employee", _employeeName)
            Text = If(_existing Is Nothing, "Add Fix Transaction", "Edit Fix Transaction")

            cboType.Properties.Items.AddRange(FixTxnType.All)
            cboFrequency.Properties.Items.AddRange(FixTxnFrequency.All)
            cboPayOn.Properties.Items.AddRange(FixTxnPayOn.All)
            For m = 1 To 12
                cboStartMonth.Properties.Items.Add(FixTransactionRules.MonthName(m))
                cboEndMonth.Properties.Items.Add(FixTransactionRules.MonthName(m))
            Next

            With lookCode.Properties
                .ValueMember = NameOf(FixTxnCodeOption.Code)
                .DisplayMember = NameOf(FixTxnCodeOption.DisplayText)
                .Columns.Clear()
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(FixTxnCodeOption.Code), "Code", 70))
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(NameOf(FixTxnCodeOption.Description), "Description", 200))
                .NullText = "Select a code..."
                .ShowFooter = False
            End With

            If _existing Is Nothing Then
                Dim today = Date.Today
                cboType.SelectedIndex = 0
                cboStartMonth.SelectedIndex = today.Month - 1
                spnStartYear.Value = today.Year
                cboEndMonth.SelectedIndex = today.Month - 1
                spnEndYear.Value = today.Year
                chkHasEnd.Checked = False
                cboFrequency.Text = FixTxnFrequency.EveryCycle
                cboPayOn.Text = FixTxnPayOn.FirstCycle
                FillCodes()
            Else
                cboType.Text = _existing.TypeText
                FillCodes()
                lookCode.EditValue = _existing.TxnCode
                spnAmount.Value = _existing.Amount
                cboStartMonth.SelectedIndex = _existing.StartMonth - 1
                spnStartYear.Value = _existing.StartYear
                chkHasEnd.Checked = _existing.EndKey.HasValue
                cboEndMonth.SelectedIndex = If(_existing.EndMonth, _existing.StartMonth) - 1
                spnEndYear.Value = If(_existing.EndYear, _existing.StartYear)
                cboFrequency.Text = _existing.Frequency
                cboPayOn.Text = If(_existing.PayOn, FixTxnPayOn.FirstCycle)
                memoRemarks.Text = _existing.Remarks

                ' Nagamit na sa payroll: hindi na mapapalitan ang code (service rule din ito).
                If _existing.IsUsed Then
                    cboType.Enabled = False
                    lookCode.Enabled = False
                    lookCode.ToolTip = "Already used in payroll: the code can't be changed. Set an End (or Inactive) and add a new entry instead."
                End If
            End If
        Finally
            _loading = False
        End Try

        UpdateEndFields()
        UpdatePayOn()
        UpdateUnit()
        UpdateSchedule()
    End Sub

    ' =============================================
    ' DEPENDENT FIELDS
    ' =============================================

    ''' <summary>Mga code ng napiling Transaction Type (active lang, maliban sa kasalukuyang code ng ine-edit).</summary>
    Private Sub FillCodes()
        Dim type = cboType.Text
        Dim keepCode = If(_existing IsNot Nothing, _existing.TxnCode, Nothing)
        Dim keepCategory = If(_existing IsNot Nothing, _existing.TxnCategory, Nothing)

        Dim list = _options.Where(Function(o) String.Equals(o.TypeText, type, StringComparison.OrdinalIgnoreCase) AndAlso
                                              (o.IsActive OrElse FixTransactionRules.SameCode(o.TxnCategory, o.Code, keepCategory, keepCode))).
                            OrderBy(Function(o) o.Code).ToList()

        ' Ang code ng ine-edit na wala na sa listahan (na-delete o na-map sa Code Mapping): ipakita pa rin para ma-edit ang iba.
        If _existing IsNot Nothing AndAlso String.Equals(_existing.TypeText, type, StringComparison.OrdinalIgnoreCase) AndAlso
           Not list.Any(Function(o) FixTransactionRules.SameCode(o.TxnCategory, o.Code, keepCategory, keepCode)) Then
            list.Insert(0, New FixTxnCodeOption With {
                .TxnCategory = _existing.TxnCategory, .Code = _existing.TxnCode,
                .Description = If(_existing.Description, _existing.TxnCode) & " (not available)",
                .InputUnit = _existing.InputUnit, .IsPayslipAllowance = _existing.IsPayslipAllowance, .IsActive = False})
        End If

        Dim previous = lookCode.EditValue
        lookCode.Properties.DataSource = list
        If previous IsNot Nothing AndAlso list.Any(Function(o) String.Equals(o.Code, CStr(previous), StringComparison.OrdinalIgnoreCase)) Then
            lookCode.EditValue = previous
        Else
            lookCode.EditValue = Nothing
        End If
    End Sub

    Private Function SelectedOption() As FixTxnCodeOption
        If lookCode.EditValue Is Nothing OrElse lookCode.EditValue Is DBNull.Value Then Return Nothing
        Dim list = TryCast(lookCode.Properties.DataSource, List(Of FixTxnCodeOption))
        If list Is Nothing Then Return Nothing
        Return list.FirstOrDefault(Function(o) String.Equals(o.Code, CStr(lookCode.EditValue), StringComparison.OrdinalIgnoreCase))
    End Function

    Private Sub UpdateEndFields()
        cboEndMonth.Enabled = chkHasEnd.Checked
        spnEndYear.Enabled = chkHasEnd.Checked
        lblOngoing.Visible = Not chkHasEnd.Checked
    End Sub

    Private Sub UpdatePayOn()
        Dim everyCycle = FixTxnFrequency.IntervalMonths(cboFrequency.Text) = 0
        cboPayOn.Enabled = Not everyCycle
        If Not everyCycle AndAlso String.IsNullOrWhiteSpace(cboPayOn.Text) Then cboPayOn.Text = FixTxnPayOn.FirstCycle
    End Sub

    ''' <summary>Peso o dami: ayon sa Input Unit ng code (kapareho ng cell sa Payroll Input Entry).</summary>
    Private Sub UpdateUnit()
        Dim opt = SelectedOption()
        Dim unit = If(opt Is Nothing, PayrollInputUnit.Amount, opt.InputUnit)
        Select Case unit
            Case PayrollInputUnit.Amount
                lblAmount.Text = "Amount *"
                lblUnit.Text = "pesos per release"
            Case Else
                lblAmount.Text = PayrollInputUnits.Text(unit) & " *"
                lblUnit.Text = $"{PayrollInputUnits.Text(unit).ToLowerInvariant()} per release (x employee rate)"
        End Select
    End Sub

    Private Sub UpdateSchedule()
        Dim m = BuildModel()
        Dim range = $"{FixTransactionRules.PeriodText(m.StartYear, m.StartMonth)} to " &
                    If(m.EndKey.HasValue, FixTransactionRules.PeriodText(m.EndYear, m.EndMonth), "ongoing")
        lblSchedule.Text = $"{FixTransactionRules.ScheduleText(m)}; {range} (by pay date)."
    End Sub

    Private Sub cboType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboType.SelectedIndexChanged
        If _loading Then Return
        FillCodes()
        UpdateUnit()
    End Sub

    Private Sub lookCode_EditValueChanged(sender As Object, e As EventArgs) Handles lookCode.EditValueChanged
        If _loading Then Return
        UpdateUnit()
    End Sub

    Private Sub chkHasEnd_CheckedChanged(sender As Object, e As EventArgs) Handles chkHasEnd.CheckedChanged
        If _loading Then Return
        UpdateEndFields()
        UpdateSchedule()
    End Sub

    Private Sub cboFrequency_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFrequency.SelectedIndexChanged
        If _loading Then Return
        UpdatePayOn()
        UpdateSchedule()
    End Sub

    Private Sub ScheduleInputs_Changed(sender As Object, e As EventArgs) _
        Handles cboPayOn.SelectedIndexChanged, cboStartMonth.SelectedIndexChanged, cboEndMonth.SelectedIndexChanged,
                spnStartYear.EditValueChanged, spnEndYear.EditValueChanged
        If _loading Then Return
        UpdateSchedule()
    End Sub

    ' =============================================
    ' SAVE
    ' =============================================

    Private Function BuildModel() As EmployeeFixTxnModel
        Dim opt = SelectedOption()
        Dim m As New EmployeeFixTxnModel With {
            .FixTxnId = If(_existing IsNot Nothing, _existing.FixTxnId, 0),
            .RecordId = _recordId,
            .TxnCategory = If(opt IsNot Nothing, opt.TxnCategory, Nothing),
            .TxnCode = If(opt IsNot Nothing, opt.Code, Nothing),
            .InputUnit = If(opt IsNot Nothing, opt.InputUnit, PayrollInputUnit.Amount),
            .IsPayslipAllowance = opt IsNot Nothing AndAlso opt.IsPayslipAllowance,
            .Description = If(opt IsNot Nothing, opt.Description, Nothing),
            .Amount = Math.Round(spnAmount.Value, 2),
            .StartMonth = cboStartMonth.SelectedIndex + 1,
            .StartYear = CInt(spnStartYear.Value),
            .Frequency = cboFrequency.Text,
            .PayOn = If(FixTxnFrequency.IntervalMonths(cboFrequency.Text) = 0, Nothing, cboPayOn.Text),
            .IsActive = If(_existing IsNot Nothing, _existing.IsActive, True),
            .IsUsed = _existing IsNot Nothing AndAlso _existing.IsUsed,
            .Remarks = memoRemarks.Text.Trim()
        }
        If chkHasEnd.Checked Then
            m.EndMonth = cboEndMonth.SelectedIndex + 1
            m.EndYear = CInt(spnEndYear.Value)
        End If
        Return m
    End Function

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Dim m = BuildModel()
        Dim problem = FixTransactionRules.Validate(m, _others)
        If problem.Length > 0 Then
            XtraMessageBox.Show(Me, problem, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        _Result = m
        DialogResult = DialogResult.OK
    End Sub

End Class
