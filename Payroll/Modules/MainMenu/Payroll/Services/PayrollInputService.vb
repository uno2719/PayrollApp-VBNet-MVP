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

        Public Function CreateCutoffAsync(cutoff As CutoffModel) As Task(Of Integer) Implements IPayrollInputService.CreateCutoffAsync
            If cutoff.CutoffEnd < cutoff.CutoffStart Then
                Throw New PayrollInputValidationException("Cutoff End date cannot be earlier than Cutoff Start date.")
            End If
            Return _repository.CreateCutoffAsync(cutoff)
        End Function

        Public Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel)) Implements IPayrollInputService.GetColumnsAsync
            Return _repository.GetColumnsAsync()
        End Function

        Public Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of DataTable) _
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
            If cutoff.Status = CutoffStatus.Posted Then
                Throw New PayrollInputValidationException("This Cutoff is already Posted and can no longer be edited.")
            End If

            For Each row As DataRow In table.Rows
                For Each col In columns
                    If Convert.ToDecimal(row(col.ColumnName)) < 0D Then
                        Throw New PayrollInputValidationException($"{row("EmployeeNo")} - {row("EmployeeName")}: negative values are not allowed ({col.Caption}).")
                    End If
                Next
            Next

            Await _repository.SaveInputDataAsync(cutoffId, table, columns, modifiedBy)
        End Function

    End Class

End Namespace