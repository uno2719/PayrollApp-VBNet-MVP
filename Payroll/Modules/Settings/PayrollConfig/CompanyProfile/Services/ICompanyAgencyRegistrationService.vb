' File: Modules/Settings/SysConfig/CompanyProfile/Services/ICompanyAgencyRegistrationService.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Interface ICompanyAgencyRegistrationService
        Function GetAsync(agencyType As String) As Task(Of CompanyAgencyRegistrationModel)
        Function SaveAsync(agencyType As String, item As CompanyAgencyRegistrationModel, userName As String) As Task(Of CompanyAgencyRegistrationSaveResult)
    End Interface

    Public Class CompanyAgencyRegistrationSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class
End Namespace