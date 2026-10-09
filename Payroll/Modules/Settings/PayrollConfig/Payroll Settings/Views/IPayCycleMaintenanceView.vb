Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface IPayCycleMaintenanceView

        ' Detalye ng napiling pay cycle
        Property PayCycleType As String
        Property RateBasis As PayRateBasis
        Property IsActive As Boolean

        ' Cutoff pattern (grid sa ilalim) - kopya ang ibinibigay sa view, kaya ang
        ' mga pagbabago ay hindi nakakaapekto sa list hangga't hindi na-save.
        Sub BindPeriods(items As List(Of PayCyclePeriodModel))
        Function GetPeriods() As List(Of PayCyclePeriodModel)

        ''' <summary>Isara ang naka-bukas na cell editor ng pattern grid bago basahin/i-save ang values.</summary>
        Sub CommitPendingEdits()

        ' Listahan ng mga pay cycle (grid sa kaliwa)
        Sub BindList(items As List(Of PayCycleModel))
        Sub FocusCycle(payCycleType As String)

        ' State/UX
        Sub SetFormMode(isEditable As Boolean)
        Sub SetRateBasisLock(isLocked As Boolean, reason As String)
        Sub ShowRateBasisNote(text As String)
        Sub ShowPatternPreview(text As String, isError As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

        ''' <summary>Yes/No: i-deactivate ba ang pay cycle na may employee pang naka-assign?</summary>
        Function ConfirmDeactivate(message As String) As Boolean

    End Interface
End Namespace
