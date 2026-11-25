' ============================================================
' Modules/MainMenu/Loans/Views/frmLoanDetails.vb
' ============================================================
' Purong DISPLAY. Walang Presenter, walang Repository, walang
' database call.
'
' BAKIT?
'   Naipasa na sa constructor ang buong EmployeeLoanDetailModel -
'   kumpleto na ang datos. Kung may sarili pa itong presenter,
'   dadagdag lang ng isang layer na walang ginagawa kundi
'   magpasa-pasa ng object. Ang MVP ay nararapat kung may
'   behavior; dito, wala - basahin at ipakita lang.
'
'   Kapag nagdagdag ka mamaya ng aksyon dito (halimbawa "Adjust
'   Installment" o "Waive Interest"), doon pa lang may saysay
'   ang presenter - at gawin na natin iyon sa panahong iyon.
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.Loans.Presenters

Public Class frmLoanDetails

    Private _detail As EmployeeLoanDetailModel

    ' Para sa dalawang aksyon (Mark paid / Undo opening payments) lang ang presenter dito;
    ' ang pagpapakita ng datos ay nananatiling purong display.
    Private ReadOnly _presenter As EmployeeLoanPresenter
    Private ReadOnly _canEdit As Boolean
    Private _dataChanged As Boolean

    ''' <summary>
    ''' True kung may na-mark/na-undo na bayad habang nakabukas ang window - para i-refresh ng ucLoans ang listahan.
    ''' </summary>
    Public ReadOnly Property DataChanged As Boolean
        Get
            Return _dataChanged
        End Get
    End Property

    Public Sub New(detail As EmployeeLoanDetailModel, presenter As EmployeeLoanPresenter, canEdit As Boolean)
        InitializeComponent()
        _detail = detail
        _presenter = presenter
        _canEdit = canEdit
    End Sub

    Private Sub frmLoanDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If _detail Is Nothing OrElse _detail.Loan Is Nothing Then
            Close()
            Return
        End If

        SetupGrid()
        PopulateHeader()
        PopulateSummary()

        gridconSchedule.DataSource = _detail.Schedule

        UpdateActionButtons()

    End Sub

    ' ========================================================
    ' OPENING PAYMENTS (C1Pay)
    ' ========================================================
    Private Sub UpdateActionButtons()

        Dim loan = _detail.Loan
        Dim schedule = _detail.Schedule

        ' "Posted" = bayad galing sa Payroll Post lang. Hindi kasama ang C1PAY, OFFSET- at CREDIT-.
        Dim hasPosted = schedule.Any(Function(s) (s.IsPaid OrElse s.AmountPaid > 0D) AndAlso
                                                 Not LoanPaymentRef.IsSystemAdjustment(s.PayrollRefNo))
        Dim openingCount = schedule.Where(Function(s) s.IsOpening).Count()

        btnMarkPaid.Visible = _canEdit
        btnUndoOpening.Visible = _canEdit

        btnMarkPaid.Enabled = _canEdit AndAlso
                              LoanStatus.IsToggleable(loan.Status) AndAlso
                              Not hasPosted AndAlso
                              openingCount < schedule.Count

        btnUndoOpening.Enabled = _canEdit AndAlso
                                 Not hasPosted AndAlso
                                 openingCount > 0 AndAlso
                                 Not loan.IsOffsetOld AndAlso
                                 Not String.Equals(loan.Status, LoanStatus.Cancelled, StringComparison.OrdinalIgnoreCase)

        ' --- Undo offset: lumalabas sa lumang loan (na-offset) at sa bagong loan (nag-offset) ---
        Dim isOffsetRelated = loan.IsOffsetOld OrElse loan.HasOffsetChildren
        btnUndoOffset.Visible = _canEdit AndAlso isOffsetRelated
        btnUndoOffset.Enabled = _canEdit AndAlso isOffsetRelated AndAlso AppSession.IsAdmin
        btnUndoOffset.ToolTip = If(AppSession.IsAdmin,
            "Admin only. The old loan becomes Active again and its remaining installments will be deducted again; any credit/refund from the offset is removed.",
            "Admin only.")

        btnMarkPaid.ToolTip = If(hasPosted,
            "Not available: this loan already has deductions posted from payroll.",
            "Mark the first installments as already paid in C1Pay (they will not be deducted again).")

        btnUndoOpening.ToolTip = If(hasPosted,
            "Not available: this loan already has deductions posted from payroll.",
            "Set all C1Pay (opening) installments back to unpaid.")

    End Sub

    Private Async Function ReloadDetailAsync() As Task

        Dim fresh = Await _presenter.GetDetailAsync(_detail.Loan.Id)
        If fresh Is Nothing OrElse fresh.Loan Is Nothing Then Close() : Return

        _detail = fresh
        _dataChanged = True

        PopulateHeader()
        PopulateSummary()
        gridconSchedule.DataSource = _detail.Schedule
        UpdateActionButtons()

    End Function

    Private Async Sub btnMarkPaid_Click(sender As Object, e As EventArgs) Handles btnMarkPaid.Click

        Dim upTo As Integer

        Using dlg As New frmLoanMarkPaid(_detail.Loan, _detail.Schedule)
            If dlg.ShowDialog(Me) <> DialogResult.OK Then Return
            upTo = dlg.UpToInstallmentNo
        End Using

        Try
            Dim result = Await _presenter.MarkOpeningPaidAsync(_detail.Loan.Id, upTo)

            If Not result.Success Then
                XtraMessageBox.Show(Me, result.ErrorMessage, "Mark Paid",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Await ReloadDetailAsync()

            XtraMessageBox.Show(Me, result.Message, "Mark Paid",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, "Mark Paid", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Async Sub btnUndoOpening_Click(sender As Object, e As EventArgs) Handles btnUndoOpening.Click

        Dim confirm = XtraMessageBox.Show(Me,
            "Set all installments marked as paid from C1Pay back to unpaid?" & Environment.NewLine &
            "They will be deducted again by the next payroll Process.",
            "Undo Opening Payments",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        If confirm <> DialogResult.Yes Then Return

        Try
            Dim result = Await _presenter.UndoOpeningPaidAsync(_detail.Loan.Id)

            If Not result.Success Then
                XtraMessageBox.Show(Me, result.ErrorMessage, "Undo Opening Payments",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Await ReloadDetailAsync()

            XtraMessageBox.Show(Me, result.Message, "Undo Opening Payments",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, "Undo Opening Payments", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ' ========================================================
    ' UNDO OFFSET (Admin + password)
    ' ========================================================
    Private Async Sub btnUndoOffset_Click(sender As Object, e As EventArgs) Handles btnUndoOffset.Click

        Dim loan = _detail.Loan

        ' Ang Undo ay laging nakabase sa BAGONG loan (ang nag-offset).
        Dim newLoanId As Integer = If(loan.IsOffsetOld, loan.OffsetByLoanId.GetValueOrDefault(), loan.Id)
        If newLoanId <= 0 Then Return

        Dim message =
            $"Undo the offset made by loan #{newLoanId}?" & Environment.NewLine & Environment.NewLine &
            "- The old loan(s) become Active again and their remaining installments will be deducted again." & Environment.NewLine &
            "- Any credit applied to the new loan's installments is removed." & Environment.NewLine &
            "- A refund entry in a Draft cutoff is removed." & Environment.NewLine & Environment.NewLine &
            "The new loan itself stays. Delete it separately if it was a mistake."

        If Not GlobalShared.Helpers.CommandBarHelper.ConfirmAdminAction(Me, "Undo Offset", message, $"undo the offset of loan #{newLoanId}") Then Return

        Try
            Dim result = Await _presenter.UndoOffsetAsync(newLoanId)

            If Not result.Success Then
                XtraMessageBox.Show(Me, result.ErrorMessage, "Undo Offset", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Await ReloadDetailAsync()

            XtraMessageBox.Show(Me, result.Message, "Undo Offset", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, "Undo Offset", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub SetupGrid()

        With gridviewSchedule
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

        ' Summary sa footer - para hindi ka na magma-manual add
        ' kung gusto mong i-verify na tugma ang kabuuan sa loan.
        colAmountDue.Summary.Clear()
        colAmountDue.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "AmountDue", "{0:n2}")

        colAmountPaid.Summary.Clear()
        colAmountPaid.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "AmountPaid", "{0:n2}")

        colPrincipalDue.Summary.Clear()
        colPrincipalDue.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "PrincipalDue", "{0:n2}")

        colInterestDue.Summary.Clear()
        colInterestDue.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "InterestDue", "{0:n2}")

    End Sub

    Private Sub PopulateHeader()

        Dim loan = _detail.Loan

        lblEmployee.Text = $"{loan.EmployeeNo}  -  {loan.EmployeeName}"
        lblLoanTitle.Text = $"{loan.LoanCode}  |  {loan.LoanDescription}  |  {loan.LoanType}"

        lblStatusBadge.Text = loan.Status?.ToUpper()
        lblStatusBadge.Appearance.ForeColor = StatusColor(loan.Status)
        lblStatusBadge.Appearance.Options.UseForeColor = True

        Text = $"Loan Details - {loan.LoanCode} ({loan.EmployeeName})"

        ' Offset info sa title bar para kita agad kung saan napunta ang lumang loan.
        If loan.IsOffsetOld AndAlso loan.OffsetByLoanId.HasValue Then
            Text &= $"  -  offset by loan #{loan.OffsetByLoanId.Value}" &
                    If(loan.OffsetDate.HasValue, $" on {loan.OffsetDate.Value.ToString(AppConstants.DisplayDateFormat)}", "")
        ElseIf loan.HasOffsetChildren Then
            Text &= $"  -  offset {loan.OffsetChildCount} older loan(s)" &
                    If(loan.OffsetCredit > 0D, $", credit {loan.OffsetCredit:n2} ({loan.OffsetCreditMode})", "")
        End If

    End Sub

    Private Sub PopulateSummary()

        Dim loan = _detail.Loan

        ' Reset - baka pula pa ang kulay mula sa naunang load (overdue) bago mag-reload.
        lblValNextDate.Appearance.ForeColor = Color.Empty

        lblValTotalLoan.Text = loan.TotalLoanAmount.ToString("n2")
        lblValTotalPaid.Text = loan.TotalPaid.ToString("n2")
        lblValBalance.Text = loan.Balance.ToString("n2")
        lblValAmortization.Text = loan.TotalAmortization.ToString("n2")

        lblValStartDate.Text = loan.LoanStartDate.ToString("MMM dd, yyyy")
        lblValFrequency.Text = If(loan.Frequency, "-")

        Dim rateText = ""
        If loan.InterestRate > 0D Then
            Dim perYear = String.Equals(loan.RatePeriod, LoanRatePeriod.PerYear, StringComparison.OrdinalIgnoreCase)
            rateText = $"  @ {loan.InterestRate:n4}%" & If(perYear, " per year", "")
        End If
        lblValMethod.Text = $"{loan.InterestMethod} / {loan.AmortizationMethod}" & rateText

        If String.Equals(loan.Frequency, LoanFrequency.EveryCycle, StringComparison.OrdinalIgnoreCase) AndAlso
           String.Equals(loan.AmortBasis, LoanAmortBasis.PerMonth, StringComparison.OrdinalIgnoreCase) Then
            lblValFrequency.Text = loan.Frequency & " (monthly amount split per cutoff)"
        End If

        ' --- KELAN ANG SUSUNOD NA KALTAS ---
        ' Ito ang pangunahing tanong ng HR pag binuksan nila ito.
        If loan.NextDeductionDate.HasValue Then

            lblValNextDate.Text = loan.NextDeductionDate.Value.ToString("MMM dd, yyyy")
            lblValNextAmount.Text = loan.NextDeductionAmount.ToString("n2")

            ' Pula kung lumipas na ang petsa pero hindi pa nakaltas -
            ' dapat mapansin ito agad, senyales ito ng hindi na-post
            ' na payroll o ng naiwang cutoff.
            If loan.NextDeductionDate.Value < Date.Today Then
                lblValNextDate.Appearance.ForeColor = Color.Firebrick
                lblValNextDate.Text &= "  (overdue)"
            End If

        ElseIf loan.IsFullyPaid Then
            lblValNextDate.Text = "None - fully paid"
            lblValNextDate.Appearance.ForeColor = Color.SeaGreen
            lblValNextAmount.Text = "0.00"
        Else
            lblValNextDate.Text = "-"
            lblValNextAmount.Text = "0.00"
        End If

        lblValNextDate.Appearance.Options.UseForeColor = True

        ' --- PROGRESO ---
        lblValProgress.Text = loan.ProgressText
        lblValRemaining.Text = loan.RemainingInstallments.ToString()

        progressPaid.Properties.Minimum = 0
        progressPaid.Properties.Maximum = Math.Max(loan.TotalInstallments, 1)
        progressPaid.EditValue = loan.PaidInstallments

        lblValRemark.Text = If(String.IsNullOrWhiteSpace(loan.Remark), "-", loan.Remark)

    End Sub

    Private Shared Function StatusColor(status As String) As Color
        Select Case status
            Case LoanStatus.Active : Return Color.SeaGreen
            Case LoanStatus.Inactive : Return Color.DarkOrange
            Case LoanStatus.Completed : Return Color.SteelBlue
            Case LoanStatus.Cancelled : Return Color.Firebrick
            Case LoanStatus.Offset : Return Color.MediumPurple
            Case Else : Return Color.Gray
        End Select
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

End Class