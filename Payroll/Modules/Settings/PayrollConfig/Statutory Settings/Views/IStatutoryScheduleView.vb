Imports Payroll.GlobalShared.Models
Imports Payroll.StatutorySettings.Services

Namespace StatutorySettings.Views
    Public Interface IStatutoryScheduleView

        ' Listahan ng mga active na pay cycle (kaliwa)
        Sub BindCycles(items As List(Of StatutoryScheduleCycleInfo))
        Sub FocusCycle(payCycleType As String)

        ' Matrix ng napiling pay cycle - kopya ang ibinibigay sa view, kaya ang mga pagbabago
        ' ay hindi nakakaapekto sa naka-save na datos hangga't hindi pinipindot ang Update.
        Sub BindSchedule(cycle As StatutoryScheduleCycleInfo, rows As List(Of StatutoryScheduleRow))
        Function GetSchedule() As List(Of StatutoryScheduleRow)

        ''' <summary>Palitan ang shares ng isang agency sa kasalukuyang matrix (Quick set).</summary>
        Sub SetAgencyShares(agency As String, shares As List(Of Decimal))

        ''' <summary>Ang agency ng napiling hilera sa matrix (StatutoryAgency.*), o Nothing.</summary>
        ReadOnly Property FocusedAgency As String

        ''' <summary>Isara ang naka-bukas na cell editor bago basahin/i-save ang values.</summary>
        Sub CommitPendingEdits()

        Property VoluntaryMode As PagIbigVoluntaryMode

        Sub ShowPreview(text As String, isError As Boolean)

        ' State/UX
        Sub SetFormMode(isEditable As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
