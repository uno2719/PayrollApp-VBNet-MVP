Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface ICompensationMaintenanceView

        ' Form fields
        Property Code As String
        Property Description As String
        Property TaxFlag As Boolean
        Property SSSFlag As Boolean
        Property PhilHealthFlag As Boolean
        Property PagIbigFlag As Boolean
        Property Component2316 As String
        Property DeminimisFlag As Boolean
        Property CeilingAmount As Decimal
        Property Frequency As String
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of CompensationModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
