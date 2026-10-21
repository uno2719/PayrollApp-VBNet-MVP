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

    End Interface

End Namespace
