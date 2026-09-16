' File: Modules/Settings/SysConfig/CompanyProfile/Views/ICompanyBankMaintenanceView.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Views
    Public Interface ICompanyBankMaintenanceView
        Property BankName As String
        Property BankCode As String
        Property AccountName As String
        Property AccountNo As String
        Property Branch As String
        Property Address1 As String
        Property Address2 As String
        Property Address3 As String
        Property Country As String
        Property PostCode As String
        Property TelephoneNo As String
        Property FaxNo As String

        Property ContactPersonRecordId As Integer?
        Property ContactPersonEmail As String

        Property PersonInCharge1RecordId As Integer?
        Property PersonInCharge1Email As String

        Property PersonInCharge2RecordId As Integer?
        Property PersonInCharge2Email As String

        Property SwiftCode As String
        Property BranchNo As String
        Property CustomerID As String
        Property IsActive As Boolean

        Sub SetEmployeeList(employees As List(Of EmployeeContactLookupModel))
        Sub BindList(items As List(Of CompanyBankModel))
        Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean)
        Sub ClearFields()
        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace