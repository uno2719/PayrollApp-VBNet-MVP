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
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Public Class frmLoanDetails

    Private ReadOnly _detail As EmployeeLoanDetailModel

    Public Sub New(detail As EmployeeLoanDetailModel)
        InitializeComponent()
        _detail = detail
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

    End Sub

    Private Sub PopulateSummary()

        Dim loan = _detail.Loan

        lblValTotalLoan.Text = loan.TotalLoanAmount.ToString("n2")
        lblValTotalPaid.Text = loan.TotalPaid.ToString("n2")
        lblValBalance.Text = loan.Balance.ToString("n2")
        lblValAmortization.Text = loan.TotalAmortization.ToString("n2")

        lblValStartDate.Text = loan.LoanStartDate.ToString("MMM dd, yyyy")
        lblValFrequency.Text = If(loan.Frequency, "-")

        lblValMethod.Text = $"{loan.InterestMethod} / {loan.AmortizationMethod}" &
                            If(loan.InterestRate > 0D, $"  @ {loan.InterestRate:n2}%", "")

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
            Case Else : Return Color.Gray
        End Select
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Close()
    End Sub

End Class