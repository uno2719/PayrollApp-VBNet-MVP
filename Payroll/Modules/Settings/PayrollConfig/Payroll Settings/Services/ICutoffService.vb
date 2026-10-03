Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface ICutoffService
        Function GetAllAsync() As Task(Of List(Of CutoffModel))
        Function SaveAsync(item As CutoffModel, userName As String) As Task(Of PayrollSettingsSaveResult)

        ''' <summary>
        ''' Generates every period of the given CycleType for the given calendar year
        ''' (the "load a whole year upfront" workflow, matching how C1Pay's admin used
        ''' to preload a year's worth of paid-for cutoffs). Periods that would overlap
        ''' an existing Cutoff of the same CycleType are skipped, not duplicated.
        ''' Not supported for "Daily" — add those one at a time instead.
        ''' </summary>
        Function GenerateForYearAsync(cycleType As String, year As Integer, userName As String) As Task(Of Integer)
    End Interface
End Namespace