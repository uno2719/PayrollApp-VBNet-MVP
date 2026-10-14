Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Data
    Public Interface IStatutoryScheduleRepository

        ''' <summary>Lahat ng naka-save na schedule row (lahat ng pay cycle, agency, at period).</summary>
        Function GetAllAsync() As Task(Of List(Of StatutoryScheduleModel))

        ''' <summary>Magdagdag ng mga bagong row (ginagamit sa default seed), sa iisang transaction.</summary>
        Function InsertManyAsync(items As List(Of StatutoryScheduleModel), userName As String) As Task

        ''' <summary>
        ''' I-save ang buong schedule sa iisang transaction (kung may pumalya, walang magbabago):
        ''' update/insert ng bawat row, alisin ang mga row na lampas na sa bilang ng period ng pay cycle
        ''' (periodCounts = pay cycle -> bilang ng period), at itugma ang Pag-IBIG voluntary mode ng lahat ng Pag-IBIG rows.
        ''' </summary>
        Function SaveAllAsync(items As List(Of StatutoryScheduleModel),
                              periodCounts As Dictionary(Of String, Integer),
                              voluntaryMode As Byte,
                              userName As String) As Task

    End Interface
End Namespace
