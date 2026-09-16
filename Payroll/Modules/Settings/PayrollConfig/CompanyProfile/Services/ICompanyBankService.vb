' File: Modules/Settings/SysConfig/CompanyProfile/Services/ICompanyBankService.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Interface ICompanyBankService
        Function GetAllAsync() As Task(Of List(Of CompanyBankModel))
        Function SaveAsync(item As CompanyBankModel, userName As String) As Task(Of CompanyBankSaveResult)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface

    Public Class CompanyBankSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace