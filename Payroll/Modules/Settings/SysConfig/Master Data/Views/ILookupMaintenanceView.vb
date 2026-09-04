Imports Payroll.GlobalShared.Models

Namespace Lookups.Views
    Public Interface ILookupMaintenanceView

        ' Form fields
        Property Code As String
        Property Name As String
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of LookupModel))

        ' State/UX
        ' isNewRecord: True = New (blangko, editable), False = Edit (existing
        ' row selected). Ginagaya ang parehong pattern ng IUsersView.
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace