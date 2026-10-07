Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    Public Interface IPayCycleRepository
        ''' <summary>Lahat ng pay cycle, kasama ang cutoff pattern (Periods) ng bawat isa.</summary>
        Function GetAllAsync() As Task(Of List(Of PayCycleModel))

        ''' <summary>Isang pay cycle ayon sa type (hal. "Daily"), o Nothing kung wala.</summary>
        Function GetByTypeAsync(payCycleType As String) As Task(Of PayCycleModel)

        ''' <summary>
        ''' I-save ang RateBasis/IsActive at PALITAN ang buong cutoff pattern, sa iisang
        ''' transaction (kung may pumalya, walang magbabago).
        ''' </summary>
        Function SaveAsync(item As PayCycleModel, userName As String) As Task(Of Boolean)

        ''' <summary>True kung may Cutoff na ang Status ay Processed o mas mataas para sa pay cycle na ito.</summary>
        Function HasProcessedCutoffAsync(payCycleType As String) As Task(Of Boolean)
    End Interface
End Namespace
