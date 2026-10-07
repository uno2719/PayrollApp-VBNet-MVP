Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface ICutoffService
        Function GetAllAsync() As Task(Of List(Of CutoffModel))
        Function SaveAsync(item As CutoffModel, userName As String) As Task(Of PayrollSettingsSaveResult)

        ''' <summary>Mga Active na pay cycle (para sa filter at sa Generate dialog).</summary>
        Function GetActiveCycleTypesAsync() As Task(Of List(Of String))

        ''' <summary>Maikling paglalarawan ng pattern ng pay cycle, hal. "21 (prev month)-5, paid 15 | 6-20, paid EOM".</summary>
        Function GetPatternDescriptionAsync(cycleType As String) As Task(Of String)

        ''' <summary>
        ''' Ipinapakita ang mga cutoff na GAGAWIN para sa taon (ayon sa pattern ng pay cycle sa
        ''' Pay Cycle Settings), at kung alin sa mga iyon ang mayroon na sa database.
        ''' Walang sine-save.
        ''' </summary>
        Function PreviewForYearAsync(cycleType As String, year As Integer) As Task(Of List(Of CutoffPreviewRow))

        ''' <summary>
        ''' Ginagawa ang lahat ng cutoff ng pay cycle para sa taon (ayon sa pattern, kasama ang
        ''' PayDate). Ang overlap sa existing Cutoff ng parehong CycleType ay nilalaktawan, hindi dinodoble.
        ''' Nagbabalik ng bilang ng nagawa.
        ''' </summary>
        Function GenerateForYearAsync(cycleType As String, year As Integer, userName As String) As Task(Of Integer)
    End Interface
End Namespace
