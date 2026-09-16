' File: Modules/Settings/SysConfig/CompanyProfile/Services/ICompanyService.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Interface ICompanyService
        Function GetAsync() As Task(Of CompanyModel)
        Function SaveAsync(item As CompanyModel, userName As String) As Task(Of CompanySaveResult)
    End Interface

    Public Class CompanySaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace