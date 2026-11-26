Imports System.Threading
Imports Payroll.GlobalShared.Models

Namespace Payslips.Services

    Public Interface IPayslipService

        Function GetCutoffsAsync() As Task(Of List(Of CutoffModel))

        Function GetEmployeeRowsAsync(cutoffIds As IList(Of Integer)) As Task(Of List(Of PayslipEmployeeRow))

        ''' <summary>
        ''' Bumubuo ng payslips ng mga napiling row (data lang - ang rptPayslip.Create ang gagawa ng report). Pwedeng galing
        ''' sa ilang cutoff (Group by pay date): binubuo kada cutoff saka pinagsasama.
        ''' </summary>
        Function BuildBatchAsync(targets As IList(Of PayslipEmployeeRow), options As PayslipOptions) As Task(Of PayslipBatch)

        ''' <summary>Nothing kapag handa ang Email Settings; kung hindi, ang dahilan (para sa mensahe sa user).</summary>
        Function CheckEmailReadyAsync() As Task(Of String)

        ''' <summary>
        ''' Isang PDF kada empleyado, ipinapadala sa EmailAddress nila. Nilalaktawan ang walang email. Bawat padala (o pagpalya)
        ''' ay naka-log sa tblPayslipEmailLog. Pwedeng ihinto (ct) - ang mga naipadala na ay mananatiling naipadala.
        ''' </summary>
        Function SendEmailsAsync(batch As PayslipBatch, options As PayslipOptions, userName As String,
                                 progress As IProgress(Of PayslipEmailProgress), ct As CancellationToken) As Task(Of List(Of PayslipEmailResult))

    End Interface

End Namespace
