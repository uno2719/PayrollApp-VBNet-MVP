Imports Payroll.GlobalShared.Models
Imports System.Data
Imports Payroll.PayrollProcessing.Models

Namespace PayrollProcessing.Services

    Public Interface IPayrollInputService
        Function GetCutoffsAsync() As Task(Of List(Of CutoffModel))
        Function GetColumnsAsync() As Task(Of List(Of PayrollInputColumnModel))
        Function GetInputDataAsync(cutoffId As Integer, columns As List(Of PayrollInputColumnModel)) As Task(Of DataTable)

        ''' <summary>Validates, then saves. Throws PayrollInputValidationException on failure.</summary>
        Function SaveInputDataAsync(cutoffId As Integer, table As DataTable, columns As List(Of PayrollInputColumnModel), modifiedBy As String) As Task
    End Interface

    Public Class PayrollInputValidationException
        Inherits Exception

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
    End Class

End Namespace