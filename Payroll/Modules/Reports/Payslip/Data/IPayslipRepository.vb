Imports Payroll.GlobalShared.Models

Namespace Payslips.Data

    Public Interface IPayslipRepository

        ''' <summary>Mga cutoff na may results at Processed o Posted (pinakabago ang una).</summary>
        Function GetCutoffsAsync() As Task(Of List(Of CutoffModel))

        ''' <summary>Mga empleyado ng cutoff para sa grid (kasama ang email address at huling email status).</summary>
        Function GetEmployeeRowsAsync(cutoffId As Integer) As Task(Of List(Of PayslipEmployeeRow))

        ''' <summary>Lahat ng kailangan para makabuo ng payslips ng mga napiling empleyado sa isang cutoff.</summary>
        Function LoadSourceAsync(cutoffId As Integer, recordIds As IList(Of Integer)) As Task(Of PayslipSource)

        Function InsertEmailLogAsync(entry As PayslipEmailLogEntry) As Task

    End Interface

End Namespace
