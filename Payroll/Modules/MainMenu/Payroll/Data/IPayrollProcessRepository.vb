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

    End Interface

End Namespace
