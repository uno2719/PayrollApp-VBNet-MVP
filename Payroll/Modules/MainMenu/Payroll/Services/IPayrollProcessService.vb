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

        ' ---------------- Pay-date mode / Export / Post (Output tab) ----------------

        ''' <summary>Mga pay date na may processed cutoff, at ang mga cutoff nito ("Group by pay date").</summary>
        Function GetPayDateGroupsAsync() As Task(Of List(Of PayDateGroupModel))

        ''' <summary>Results ng isa o higit pang cutoff nang sabay (may CutoffLabel ang bawat empleyado).</summary>
        Function GetResultsForCutoffsAsync(cutoffIds As IList(Of Integer)) As Task(Of PayrollResultSheet)

        ''' <summary>
        ''' Inihahanda ang export ng mga napiling cutoff (isa, o lahat ng cutoff ng pay date).
        ''' Nagbabalik ng mga row na isasama at ng mga lalaktawan (Net &lt;= 0, kulang/mali ang account, walang Pay By).
        ''' </summary>
        Function BuildExportPlanAsync(cutoffIds As IList(Of Integer), kind As String) As Task(Of PayrollExportPlan)

        ''' <summary>Company Code at mga batch na nagamit na ngayong araw para sa pangalan ng BDO Direct .txt. Throws kung walang Customer ID.</summary>
        Function GetBdoDirectNamingAsync() As Task(Of BdoDirectNaming)

        ''' <summary>Isinusulat ang file at pagkatapos ay ang export log (kapag nagtagumpay lang ang pagsulat ng file).</summary>
        Function WriteExportAsync(plan As PayrollExportPlan, filePath As String, userName As String) As Task

        ''' <summary>
        ''' Pwede na bang i-Post? payDate ay may laman lang sa "Group by pay date" (lahat ng cutoff ng pay date ay sabay na ipo-post,
        ''' all-or-nothing). Kasama ang mga dahilan kung hindi pa, at ang buod para sa Confirm.
        ''' </summary>
        Function GetPostStateAsync(cutoffIds As IList(Of Integer), payDate As Date?) As Task(Of PayrollPostState)

        ''' <summary>Admin lang. FINAL: walang Unpost. Throws kapag may hadlang.</summary>
        Function PostAsync(cutoffIds As IList(Of Integer), payDate As Date?, userName As String) As Task(Of PayrollPostResult)

    End Interface

End Namespace
