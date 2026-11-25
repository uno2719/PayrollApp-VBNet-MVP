' ============================================================
' Modules/MainMenu/Loans/Views/frmLoanMarkPaid.vb
' ============================================================
' Maliit na dialog: "Hanggang hulog #ilan ang nabayaran na sa C1Pay?"
'
' Purong display + input. Walang database call - ang buong schedule ay naipasa na sa constructor.
' Ang preview (kabuuang imamarkang bayad, balanse pagkatapos, susunod na kaltas) ay para makumpara agad ni user
' sa C1Pay bago mag-OK. Ang AKTWAL na rules (bawal kung may posted na, atbp.) ay nasa EmployeeLoanService.
' ============================================================
Imports Payroll.GlobalShared.Models

Public Class frmLoanMarkPaid

    Private ReadOnly _loan As EmployeeLoanModel
    Private ReadOnly _schedule As List(Of EmployeeLoanScheduleModel)

    ''' <summary>
    ''' Ang napiling numero ng hulog (ang lahat ng hindi pa bayad mula #1 hanggang dito ay mamarkahang bayad).
    ''' </summary>
    Public ReadOnly Property UpToInstallmentNo As Integer
        Get
            Return CInt(spnUpTo.Value)
        End Get
    End Property

    Public Sub New(loan As EmployeeLoanModel, schedule As List(Of EmployeeLoanScheduleModel))
        InitializeComponent()
        _loan = loan
        _schedule = If(schedule, New List(Of EmployeeLoanScheduleModel))
    End Sub

    Private Sub frmLoanMarkPaid_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim total = _schedule.Count

        ' Ang pinakamataas na hulog na naka-mark na (C1PAY). Pataas lang ang Mark.
        Dim alreadyMarked = _schedule.Where(Function(s) s.IsOpening).
                                      Select(Function(s) s.InstallmentNo).
                                      DefaultIfEmpty(0).Max()

        ' Mungkahi: ilang hulog ang ScheduledDate ay hanggang ngayong araw na.
        Dim dueSoFar = _schedule.Where(Function(s) s.ScheduledDate.Date <= Date.Today).Count()

        spnUpTo.Properties.MinValue = alreadyMarked + 1
        spnUpTo.Properties.MaxValue = Math.Max(total, alreadyMarked + 1)

        Dim suggested = Math.Min(Math.Max(dueSoFar, alreadyMarked + 1), Math.Max(total, 1))
        spnUpTo.Value = suggested

        lblHint.Text = $"Installments scheduled on or before today: {dueSoFar} of {total}. " &
                       "Use the same count that C1Pay shows as paid."

        UpdatePreview()

    End Sub

    Private Sub spnUpTo_EditValueChanged(sender As Object, e As EventArgs) Handles spnUpTo.EditValueChanged
        UpdatePreview()
    End Sub

    Private Sub UpdatePreview()

        ' Maaaring tawagin ito (via EditValueChanged) habang tumatakbo pa ang InitializeComponent,
        ' bago pa naitatakda ang _loan/_schedule sa constructor.
        If _loan Is Nothing OrElse _schedule Is Nothing Then Return

        Dim n = UpToInstallmentNo

        Dim toMark = _schedule.Where(Function(s) s.InstallmentNo <= n AndAlso Not s.IsPaid).ToList()
        Dim amount = toMark.Sum(Function(s) s.AmountDue)

        Dim lastRow = _schedule.FirstOrDefault(Function(s) s.InstallmentNo = n)
        Dim nextRow = _schedule.Where(Function(s) s.InstallmentNo > n AndAlso Not s.IsPaid).
                                OrderBy(Function(s) s.InstallmentNo).FirstOrDefault()

        Dim balanceAfter = _loan.TotalLoanAmount - (_loan.TotalPaid + amount)
        If balanceAfter < 0D Then balanceAfter = 0D

        Dim text As String =
            $"Will mark {toMark.Count} installment(s) as paid, total {amount:n2}." & Environment.NewLine

        If lastRow IsNot Nothing Then
            text &= $"Last one: #{n}, scheduled {lastRow.ScheduledDate:MMM dd, yyyy}." & Environment.NewLine
        End If

        text &= $"Balance after: {balanceAfter:n2}" & Environment.NewLine

        If nextRow IsNot Nothing Then
            text &= $"Next deduction: #{nextRow.InstallmentNo} on {nextRow.ScheduledDate:MMM dd, yyyy}, {nextRow.AmountDue:n2}"
        Else
            text &= "Next deduction: none - the loan will be fully paid."
        End If

        lblPreview.Text = text

    End Sub

End Class
