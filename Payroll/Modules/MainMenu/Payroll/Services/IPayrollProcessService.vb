Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Services

    Public Interface IPayrollProcessService

        ''' <summary>
        ''' Sinusuri ang cutoff at ang mga setting, kinukuha ang lahat ng datos at kino-compute ang bawat empleyado SA MEMORY LANG
        ''' (walang naisusulat sa database). Throws PayrollInputValidationException kung may hadlang (hal. kulang ang setting).
        ''' </summary>
        Function BuildAsync(cutoffId As Integer) As Task(Of PayrollRunPreview)

        ''' <summary>Isang transaction: papalitan ang results ng cutoff at itatakda itong Processed. Hindi pwede kung Posted.</summary>
        Function SaveAsync(preview As PayrollRunPreview, userName As String) As Task

        ''' <summary>Mga cutoff na may naka-save na results (Output tab).</summary>
        Function GetResultCutoffsAsync() As Task(Of List(Of CutoffModel))

        Function GetResultsAsync(cutoffId As Integer) As Task(Of PayrollResultSheet)

        ' ---------------- Export at Post (Output tab) ----------------

        ''' <summary>
        ''' Inihahanda ang export: kung combineSamePayDate, kasama ang lahat ng cutoff (may results) na pareho ang Pay Date.
        ''' Nagbabalik ng mga row na isasama at ng mga lalaktawan (Net &lt;= 0, kulang/mali ang account, walang Pay By).
        ''' </summary>
        Function BuildExportPlanAsync(cutoffId As Integer, combineSamePayDate As Boolean, kind As String) As Task(Of PayrollExportPlan)

        ''' <summary>Isinusulat ang file at pagkatapos ay ang export log (kapag nagtagumpay lang ang pagsulat ng file).</summary>
        Function WriteExportAsync(plan As PayrollExportPlan, filePath As String, userName As String) As Task

        ''' <summary>Pwede na bang i-Post? Kasama ang mga dahilan kung hindi pa, at ang buod para sa Confirm.</summary>
        Function GetPostStateAsync(cutoffId As Integer) As Task(Of PayrollPostState)

        ''' <summary>Admin lang. FINAL: walang Unpost. Throws kapag may hadlang.</summary>
        Function PostAsync(cutoffId As Integer, userName As String) As Task(Of PayrollPostResult)

    End Interface

End Namespace
