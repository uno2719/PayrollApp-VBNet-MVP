Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Views
    Public Interface ILoanMaintenanceView

        ' Form fields
        Property Code As String
        Property Description As String
        Property LoanType As String
        Property IsActive As Boolean

        ' Grid
        Sub BindList(items As List(Of LoanModel))

        ' State/UX
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)

    End Interface
End Namespace
