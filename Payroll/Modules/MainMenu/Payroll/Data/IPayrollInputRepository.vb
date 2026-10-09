Imports Payroll.GlobalShared.Models
Imports System.Data
Imports Payroll.PayrollProcessing.Models

Namespace PayrollProcessing.Data

    Public Interface IPayrollInputRepository

        ''' <summary>Reads the Cutoff list — creating/editing Cutoffs now happens in Payroll Settings' Cutoff tab.</summary>
        Function GetCutoffsAsync() As Task(Of List(Of CutoffModel))

        ''' <summary>
        ''' True kung ang pay cycle ng Cutoff ay naka-Daily Rate basis (Pay Cycle Settings) -
        ''' ibig sabihin, kailangan ng "Days Worked" input para sa mga employee nito.
        ''' </summary>
        Function RequiresDaysWorkedAsync(cutoffId As Integer) As Task(Of Boolean)

        ''' <summary>One column per active Overtime/Holiday/Compensation/Bonus/Deduction catalog row (the unit of each comes from its catalog row).</summary>
        Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel))

        ''' <summary>
        ''' A row per employee eligible for this Cutoff's CycleType, with one
        ''' Decimal column per entry in <paramref name="columns"/>, pre-filled
        ''' from any previously saved Payroll Input Entry values.
        ''' </summary>
        Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of DataTable)

        ''' <summary>
        ''' Replaces all Payroll-Input-Entry-sourced transactions for this Cutoff
        ''' in one transaction (delete-then-reinsert per employee, scoped to
        ''' TxnSrc so other sources' rows are never touched).
        ''' </summary>
        Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task

    End Interface

End Namespace