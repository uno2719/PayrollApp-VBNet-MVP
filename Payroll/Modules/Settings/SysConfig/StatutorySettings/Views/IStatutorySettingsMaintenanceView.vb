Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Views
    Public Interface IStatutorySettingsMaintenanceView

        ' Form fields
        Property SalaryFrom As Decimal
        Property SalaryTo As Decimal
        Property EEShare As Decimal
        Property EEContriType As String
        Property ERShare As Decimal
        Property ERContriType As String
        Property ECCAmount As Decimal
        Property EEMPF As Decimal
        Property ERMPF As Decimal
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of StatutoryBracketModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace