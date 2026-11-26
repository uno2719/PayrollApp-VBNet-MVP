Imports System.Threading
Imports Payroll.GlobalShared.Models

Namespace Payslips.Services

    Public Interface IPayslipService

        Function GetCutoffsAsync() As Task(Of List(Of CutoffModel))

        Function GetEmployeeRowsAsync(cutoffId As Integer) As Task(Of List(Of PayslipEmployeeRow))

        ''' <summary>Bumubuo ng payslips ng mga napiling empleyado (data lang - ang rptPayslip.Create ang gagawa ng report).</summary>
        Function BuildBatchAsync(cutoffId As Integer, recordIds As IList(Of Integer), options As PayslipOptions) As Task(Of PayslipBatch)

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
