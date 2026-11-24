Imports Payroll.GlobalShared.Models
Imports System.Data
Imports Payroll.GlobalShared.Base
Imports Payroll.PayrollProcessing.Data
Imports Payroll.PayrollProcessing.Models

Namespace PayrollProcessing.Services

    Public Class PayrollInputService
        Inherits BaseService
        Implements IPayrollInputService

        Private ReadOnly _repository As IPayrollInputRepository

        Public Sub New(repository As IPayrollInputRepository)
            _repository = repository
        End Sub

        Public Function GetCutoffsAsync() As Task(Of List(Of CutoffModel)) Implements IPayrollInputService.GetCutoffsAsync
            Return _repository.GetCutoffsAsync()
        End Function

        Public Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel)) Implements IPayrollInputService.GetColumnsAsync
            Return _repository.GetColumnsAsync()
        End Function

        Public Function GetBasicDaysColumnAsync() As Task(Of PayrollInputColumnModel) _
            Implements IPayrollInputService.GetBasicDaysColumnAsync
            Return _repository.GetBasicDaysColumnAsync()
        End Function

        Public Function RequiresDaysWorkedAsync(cutoffId As Integer) As Task(Of Boolean) _
            Implements IPayrollInputService.RequiresDaysWorkedAsync
            Return _repository.RequiresDaysWorkedAsync(cutoffId)
        End Function

        Public Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of PayrollInputSheet) _
            Implements IPayrollInputService.GetInputDataAsync
            Return _repository.GetInputDataAsync(cutoffId, columns)
        End Function

        Public Async Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task _
            Implements IPayrollInputService.SaveInputDataAsync

            Dim cutoffs = Await _repository.GetCutoffsAsync()
            Dim cutoff = cutoffs.FirstOrDefault(Function(c) c.CutoffID = cutoffId)
            If cutoff Is Nothing Then
                Throw New PayrollInputValidationException("Selected Cutoff no longer exists.")
            End If
            If cutoff.Status = CutoffStatus.Closed Then
                Throw New PayrollInputValidationException("This Cutoff is Closed. Set it back to Draft in Payroll Settings > Cut-Off before entering inputs.")
            End If

            If cutoff.Status = CutoffStatus.Posted Then
                Throw New PayrollInputValidationException("This Cutoff is already Posted and can no longer be edited.")
            End If

            ' Days Worked (Daily Rate cycles) ay hindi pwedeng lumampas sa bilang ng araw ng cutoff
            Dim daysInCutoff = (cutoff.CutoffEnd.Date - cutoff.CutoffStart.Date).Days + 1

            For Each row As DataRow In table.Rows
                For Each col In columns
                    Dim value = Convert.ToDecimal(row(col.ColumnName))

                    If value < 0D Then
                        Throw New PayrollInputValidationException($"{row("EmployeeNo")} - {row("EmployeeName")}: negative values are not allowed ({col.Caption}).")
                    End If

                    If col.IsBasicDays AndAlso value > daysInCutoff Then
                        Throw New PayrollInputValidationException($"{row("EmployeeNo")} - {row("EmployeeName")}: Days Worked ({value:0.##}) cannot be more than the {daysInCutoff} days in this cutoff.")
                    End If
                Next
            Next

            Await _repository.SaveInputDataAsync(cutoffId, table, columns, modifiedBy)
        End Function

        Public Async Function CloseEmptyCutoffAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel), closedBy As String) As Task _
            Implements IPayrollInputService.CloseEmptyCutoffAsync

            Dim cutoffs = Await _repository.GetCutoffsAsync()
            Dim cutoff = cutoffs.FirstOrDefault(Function(c) c.CutoffID = cutoffId)
            If cutoff Is Nothing Then Throw New PayrollInputValidationException("Selected Cutoff no longer exists or is no longer open.")
            If cutoff.Status <> CutoffStatus.Draft Then
                Throw New PayrollInputValidationException("Only a Draft cutoff can be closed here.")
            End If

            ' Sinusuri ulit ngayon (hindi lang ang nakikita sa grid): may lalabas pa bang empleyado?
            Dim sheet = Await _repository.GetInputDataAsync(cutoffId, columns)
            If sheet.Table.Rows.Count > 0 Then
                Throw New PayrollInputValidationException($"This cutoff still has {sheet.Table.Rows.Count} employee(s), so it cannot be closed.")
            End If
            If sheet.Warnings.Count > 0 Then
                Throw New PayrollInputValidationException($"{sheet.Warnings.Count} employee(s) can't be included because their setup is incomplete. Fix their setup first, so nobody is skipped by mistake.")
            End If

            Dim closed = Await _repository.CloseEmptyDraftCutoffAsync(cutoffId, closedBy)
            If Not closed Then
                Throw New PayrollInputValidationException("The cutoff could not be closed (it may already have saved inputs or results).")
            End If
        End Function

    End Class

End Namespace