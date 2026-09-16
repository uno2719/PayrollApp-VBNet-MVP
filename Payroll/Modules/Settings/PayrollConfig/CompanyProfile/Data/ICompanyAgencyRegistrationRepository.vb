' File: Modules/Settings/SysConfig/CompanyProfile/Data/ICompanyAgencyRegistrationRepository.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Interface ICompanyAgencyRegistrationRepository
        Function GetAsync(agencyType As String) As Task(Of CompanyAgencyRegistrationModel)
        Function InsertAsync(agencyType As String, item As CompanyAgencyRegistrationModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As CompanyAgencyRegistrationModel, userName As String) As Task(Of Boolean)
    End Interface
End Namespace