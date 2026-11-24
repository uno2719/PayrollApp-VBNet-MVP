Imports Payroll.GlobalShared.Models

Namespace PayrollProcessing.Data

    ''' <summary>Mga empleyadong kasama sa computation, at ang mga hindi naisama dahil kulang ang setup.</summary>
    Public Class PayrollEmployeeLoad
        Public Property Employees As List(Of PayrollEmployeeInput) = New List(Of PayrollEmployeeInput)()
        Public Property Excluded As List(Of PayrollEligibilityWarning) = New List(Of PayrollEligibilityWarning)()
    End Class

    Public Interface IPayrollProcessRepository

        ''' <summary>
        ''' Mga kasamang empleyado (PayrollEligibilityRules) kasama ang rates, Input Entry, mga due na loan hulog at ang
        ''' SSS/PhilHealth/Pag-IBIG base ng mga naunang cutoff ng pay month (galing sa naka-save na results).
        ''' </summary>
        ''' <param name="loanLowerBound">Ang mga hulog na ang petsa ay HANGGANG dito (kasama) ay itinuturing na overdue (hindi awtomatikong kakaltasin).</param>
        Function LoadEmployeesAsync(cutoff As CutoffModel, basis As PayRateBasis,
                                    priorCutoffIds As IList(Of Integer), loanLowerBound As Date) As Task(Of PayrollEmployeeLoad)

        ''' <summary>
        ''' Isang transaction: buburahin ang lumang results ng cutoff, ilalagay ang bago, at itatakda ang cutoff sa Processed.
        ''' Hindi pwede kung Posted o Closed na.
        ''' </summary>
        Function SaveResultsAsync(cutoffId As Integer, results As List(Of PayrollEmployeeResult), userName As String) As Task

        ''' <summary>Mga cutoff na may naka-save na results (para sa Output tab).</summary>
        Function GetCutoffsWithResultsAsync() As Task(Of List(Of CutoffModel))

        Function GetResultsAsync(cutoffId As Integer) As Task(Of PayrollResultSheet)

        ' ---------------- Export at Post ----------------

        ''' <summary>Net pay ng bawat empleyado sa mga cutoff, kasama ang Pay By / Bank (live galing sa Employee > Earnings).</summary>
        Function GetPayoutRowsAsync(cutoffIds As IList(Of Integer)) As Task(Of List(Of PayrollPayoutRow))

        ''' <summary>Isinusulat ang export log (isang row kada cutoff kada listahan). Ang ProcessedStamp ay kinukuha sa tblCutoff.LastProcessedDate.</summary>
        Function InsertExportLogAsync(entries As List(Of PayrollExportLogEntry)) As Task

        ''' <summary>Bilang ng may babala, bilang ng loan installments, at mga listahang valid na ang export.</summary>
        Function GetPostFactsAsync(cutoffId As Integer) As Task(Of PayrollPostFacts)

        ''' <summary>
        ''' Isang transaction para sa lahat ng hiniling na cutoff: sinusuri ulit ang status, babala, export at loans;
        ''' minamarkahang bayad ang loan installments (PayrollRefNo = PR-{CutoffID}); ang loan na kumpleto na ay nagiging
        ''' Completed; Status = Posted. FINAL - walang Unpost. All-or-nothing. Throws InvalidOperationException (may malinaw
        ''' na mensahe, may pangalan ng cutoff) kung may hadlang.
        ''' </summary>
        Function PostCutoffsAsync(requests As IList(Of PayrollPostRequest), userName As String) As Task(Of PayrollPostResult)

        ' ---------------- Pay-date mode / BDO naming ----------------

        ''' <summary>Results (mga empleyado at linya) ng ilang cutoff nang sabay - para sa "Group by pay date".</summary>
        Function GetResultsForCutoffsAsync(cutoffIds As IList(Of Integer)) As Task(Of PayrollResultSheet)

        ''' <summary>Customer ID ng aktibong BDO row sa tblCompanyBank (= Company Code ng BDO Payroll Converter). Nothing kung wala.</summary>
        Function GetBdoCompanyCodeAsync() As Task(Of String)

        ''' <summary>Mga pangalan ng BDO Direct file sa export log na nagsisimula sa prefix (para malaman ang mga batch na nagamit na).</summary>
        Function GetBdoDirectFileNamesAsync(prefix As String) As Task(Of List(Of String))

    End Interface

End Namespace
