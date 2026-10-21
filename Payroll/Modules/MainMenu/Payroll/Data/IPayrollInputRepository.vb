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

        ''' <summary>BASIC (Basic Salary Code +) bilang Days Worked column; Nothing kung hindi naka-set sa General Settings.</summary>
        Function GetBasicDaysColumnAsync() As Task(Of PayrollInputColumnModel)

        ''' <summary>One column per active Overtime/Holiday/Compensation/Bonus/Deduction catalog row (the unit of each comes from its catalog row).</summary>
        Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel))

        ''' <summary>
        ''' Isang row bawat empleyadong KASAMA sa cutoff na ito (PayrollEligibilityRules: Include in payroll, pay cycle,
        ''' Date Joined, petsa ng pag-alis, at may rate), na may isang Decimal column bawat entry sa <paramref name="columns"/>,
        ''' pre-filled mula sa naka-save na Input Entry. Kasama rin ang mga empleyadong hindi naisama dahil kulang ang setup.
        ''' </summary>
        Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of PayrollInputSheet)

        ''' <summary>
        ''' Replaces all Payroll-Input-Entry-sourced transactions for this Cutoff
        ''' in one transaction (delete-then-reinsert per employee, scoped to
        ''' TxnSrc so other sources' rows are never touched).
        ''' </summary>
        Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task

    End Interface

End Namespace