' ============================================================
' Settings > Payroll > Minimum Wage: Add / Edit ng isang rate (Wage Area + Effective Date + Daily Minimum Wage).
' Ibinabalik lang ang Result; ang pag-save ay ginagawa ng ucMinimumWage (MinimumWageService, may validation ulit).
' ============================================================
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Models

Public Class frmMinimumWageRate

    Private ReadOnly _existing As MinimumWageModel
    Private ReadOnly _areas As List(Of String)
    Private ReadOnly _allRates As List(Of MinimumWageModel)

    Public ReadOnly Property Result As MinimumWageModel

    ''' <param name="existing">Nothing = bagong rate.</param>
    Public Sub New(existing As MinimumWageModel, areas As List(Of String), allRates As List(Of MinimumWageModel))
        InitializeComponent()
        _existing = existing
        _areas = If(areas, New List(Of String)())
        _allRates = If(allRates, New List(Of MinimumWageModel)())
    End Sub

    Private Sub frmMinimumWageRate_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = If(_existing Is Nothing, "Add Minimum Wage", "Edit Minimum Wage")
        dtpEffective.ApplyDisplayDateFormat()
        cboArea.Properties.Items.AddRange(_areas.ToArray())

        If _existing Is Nothing Then
            dtpEffective.EditValue = Date.Today
        Else
            cboArea.Text = _existing.WageArea
            dtpEffective.EditValue = _existing.EffectiveDate
            spnDailyRate.Value = _existing.DailyRate
            txtWageOrder.Text = _existing.WageOrderNo
            txtRemarks.Text = _existing.Remarks
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If dtpEffective.EditValue Is Nothing OrElse dtpEffective.EditValue Is DBNull.Value Then
            XtraMessageBox.Show(Me, "Enter the Effective Date.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim m As New MinimumWageModel With {
            .MinimumWageId = If(_existing IsNot Nothing, _existing.MinimumWageId, 0),
            .WageArea = cboArea.Text.Trim(),
            .EffectiveDate = CDate(dtpEffective.EditValue).Date,
            .DailyRate = Math.Round(spnDailyRate.Value, 2),
            .WageOrderNo = txtWageOrder.Text.Trim(),
            .Remarks = txtRemarks.Text.Trim()
        }

        Dim problem = PayrollSettings.Services.MinimumWageService.Validate(m, _allRates)
        If problem.Length > 0 Then
            XtraMessageBox.Show(Me, problem, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _Result = m
        DialogResult = DialogResult.OK
    End Sub

End Class
