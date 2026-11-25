' ============================================================
' Modules/MainMenu/Loans/Views/frmLoanOffset.vb
' ============================================================
' Lumalabas pagka-Save ng BAGONG loan kapag may Active na loan ang parehong employee na pareho ang Loan Code
' (hal. SSS Salary Loan renewal).
'
'   Offset selected = ang natitirang hulog ng napiling lumang loan ay babayaran ng bagong loan
'                     (hindi na kakaltasan; Status ng luma = Offset)
'   Keep both       = ordinaryong save; parehong kakaltasan
'   Cancel          = bumalik sa form, walang mase-save
'
' Purong display + input. Ang preview ng credit ay galing sa LoanOffsetRules (kapareho ng ginagamit ng Service),
' pero ang Service pa rin ang huling salita: muli nitong binabasa ang database bago mag-save.
' ============================================================
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Public Class frmLoanOffset

    Private ReadOnly _candidates As BindingList(Of LoanOffsetCandidate)
    Private ReadOnly _newLoan As EmployeeLoanModel
    Private _isLoading As Boolean = True

    ''' <summary>Ang pinili ni user. Cancel kapag isinara ang window.</summary>
    Public Property Result As New LoanOffsetDialogResult With {.Choice = LoanOffsetChoice.Cancel}

    Public Sub New(candidates As List(Of LoanOffsetCandidate), newLoan As EmployeeLoanModel)
        InitializeComponent()
        _candidates = New BindingList(Of LoanOffsetCandidate)(If(candidates, New List(Of LoanOffsetCandidate)))
        _newLoan = newLoan
    End Sub

    Private Sub frmLoanOffset_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim code = If(_candidates.FirstOrDefault()?.LoanCode, _newLoan?.LoanCode)

        lblIntro.Text =
            $"This employee already has {_candidates.Count} active '{code}' loan(s). If the new loan already includes the " &
            "old balance (e.g. SSS renewal), choose Offset: the old loan's remaining installments are marked paid by the " &
            "new loan and will no longer be deducted."

        gridconOldLoans.DataSource = _candidates
        dtOffsetDate.DateTime = Date.Today
        spnBalance.Value = 0D
        rgCreditMode.EditValue = Nothing

        _isLoading = False
        UpdatePreview()

    End Sub

    ' ========================================================
    ' GRID: ang naka-block (nasa Processed na cutoff) ay hindi pwedeng i-check
    ' ========================================================
    Private Sub gridviewOldLoans_ShowingEditor(sender As Object, e As CancelEventArgs) Handles gridviewOldLoans.ShowingEditor
        Dim row = TryCast(gridviewOldLoans.GetFocusedRow(), LoanOffsetCandidate)
        If row IsNot Nothing AndAlso row.IsBlocked Then e.Cancel = True
    End Sub

    Private Sub gridviewOldLoans_RowStyle(sender As Object, e As RowStyleEventArgs) Handles gridviewOldLoans.RowStyle
        If e.RowHandle < 0 Then Return
        Dim row = TryCast(gridviewOldLoans.GetRow(e.RowHandle), LoanOffsetCandidate)
        If row IsNot Nothing AndAlso row.IsBlocked Then e.Appearance.ForeColor = Color.Gray
    End Sub

    ' Para agad mag-update ang preview pagka-click ng checkbox (hindi na hihintayin umalis sa cell).
    Private Sub gridviewOldLoans_CellValueChanging(sender As Object, e As CellValueChangedEventArgs) Handles gridviewOldLoans.CellValueChanging
        If e.Column Is colSelected Then
            gridviewOldLoans.PostEditor()
            gridviewOldLoans.UpdateCurrentRow()
        End If
    End Sub

    Private Sub gridviewOldLoans_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles gridviewOldLoans.CellValueChanged
        UpdatePreview()
    End Sub

    Private Sub Inputs_EditValueChanged(sender As Object, e As EventArgs) Handles spnBalance.EditValueChanged, dtOffsetDate.EditValueChanged
        UpdatePreview()
    End Sub

    ' ========================================================
    ' PREVIEW
    ' ========================================================
    Private Function SelectedRows() As List(Of LoanOffsetCandidate)
        Return _candidates.Where(Function(c) c.Selected AndAlso Not c.IsBlocked).ToList()
    End Function

    Private Function BalancePerStatement() As Decimal?
        Return If(spnBalance.Value > 0D, spnBalance.Value, CType(Nothing, Decimal?))
    End Function

    Private Function CurrentCredit() As Decimal
        Dim unpaid = SelectedRows().Sum(Function(c) c.UnpaidAmount)
        Return LoanOffsetRules.ComputeCredit(BalancePerStatement(), unpaid)
    End Function

    Private Sub UpdatePreview()

        If _isLoading Then Return

        Dim selected = SelectedRows()
        Dim unpaid = selected.Sum(Function(c) c.UnpaidAmount)
        Dim balance = BalancePerStatement()
        Dim credit = CurrentCredit()

        lblSummary.Text =
            $"Selected: {selected.Count} loan(s), {selected.Sum(Function(c) c.UnpaidCount)} unpaid installment(s), {unpaid:n2} in this system." &
            Environment.NewLine &
            $"New loan: {If(_newLoan?.TotalLoanAmount, 0D):n2} total, {If(_newLoan?.TotalAmortization, 0D):n2} amortization."

        If credit > 0D Then
            lblCredit.Text = $"Credit to employee: {credit:n2}  (statement balance {balance.Value:n2} is higher than the unpaid {unpaid:n2} here, " &
                             "so the employee already paid this part). Choose what to do with it:"
            lblCredit.Appearance.ForeColor = Color.DarkGreen
        ElseIf balance.HasValue AndAlso balance.Value < unpaid Then
            lblCredit.Text = $"No credit. The unpaid amount here is {unpaid - balance.Value:n2} higher than the statement; " &
                             "the offset simply closes those installments."
            lblCredit.Appearance.ForeColor = Color.DimGray
        Else
            lblCredit.Text = "No credit to the employee."
            lblCredit.Appearance.ForeColor = Color.DimGray
        End If

        lblCredit.Appearance.Options.UseForeColor = True

        rgCreditMode.Enabled = (credit > 0D)
        If credit <= 0D Then rgCreditMode.EditValue = Nothing

        btnOffset.Enabled = selected.Count > 0

    End Sub

    ' ========================================================
    ' BUTTONS
    ' ========================================================
    Private Sub btnOffset_Click(sender As Object, e As EventArgs) Handles btnOffset.Click

        gridviewOldLoans.PostEditor()
        gridviewOldLoans.UpdateCurrentRow()

        Dim selected = SelectedRows()
        If selected.Count = 0 Then
            XtraMessageBox.Show(Me, "Tick at least one old loan to offset, or choose 'Keep both'.", Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If dtOffsetDate.EditValue Is Nothing Then
            XtraMessageBox.Show(Me, "Enter the offset date.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim credit = CurrentCredit()
        Dim mode = TryCast(rgCreditMode.EditValue, String)

        If credit > 0D Then
            If String.IsNullOrWhiteSpace(mode) Then
                XtraMessageBox.Show(Me, $"There is a credit of {credit:n2} for the employee. Choose what to do with it.", Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If mode <> LoanCreditMode.RefundThroughPayroll AndAlso _newLoan IsNot Nothing AndAlso credit > _newLoan.TotalLoanAmount Then
                XtraMessageBox.Show(Me, $"The credit ({credit:n2}) is bigger than the whole new loan ({_newLoan.TotalLoanAmount:n2}). " &
                                    "Use 'Refund through payroll' instead.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        Else
            mode = Nothing
        End If

        Dim confirmText =
            $"Offset {selected.Count} old loan(s): {selected.Sum(Function(c) c.UnpaidCount)} unpaid installment(s), " &
            $"{selected.Sum(Function(c) c.UnpaidAmount):n2}, will be marked paid by this new loan as of " &
            $"{dtOffsetDate.DateTime.ToString(AppConstants.DisplayDateFormat)} and will no longer be deducted." &
            Environment.NewLine & Environment.NewLine & "Continue?"

        If XtraMessageBox.Show(Me, confirmText, Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) <> DialogResult.Yes Then Return

        Result = New LoanOffsetDialogResult With {
            .Choice = LoanOffsetChoice.Offset,
            .Request = New LoanOffsetRequest With {
                .OldLoanIds = selected.Select(Function(c) c.LoanId).ToList(),
                .OffsetDate = dtOffsetDate.DateTime.Date,
                .BalancePerStatement = BalancePerStatement(),
                .CreditMode = mode
            }
        }

        DialogResult = DialogResult.OK
        Close()

    End Sub

    Private Sub btnKeepBoth_Click(sender As Object, e As EventArgs) Handles btnKeepBoth.Click
        Result = New LoanOffsetDialogResult With {.Choice = LoanOffsetChoice.KeepBoth}
        DialogResult = DialogResult.OK
        Close()
    End Sub

End Class
