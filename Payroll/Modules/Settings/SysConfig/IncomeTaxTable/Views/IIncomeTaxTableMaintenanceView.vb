Imports Payroll.GlobalShared.Models

Namespace IncomeTaxTable.Views
    Public Interface IIncomeTaxTableMaintenanceView

        ' Form fields
        Property SalaryFrom As Decimal
        Property SalaryTo As Decimal
        Property TaxPercentage As Decimal
        Property FixTaxAmount As Decimal
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of IncomeTaxBracketModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace