Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Views
    Public Interface ILeaveTypeMaintenanceView

        ' Form fields
        Property Code As String
        Property Name As String
        Property Category As String
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of LeaveTypeModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
