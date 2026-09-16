' File: Modules/Settings/SysConfig/CompanyProfile/Data/ICompanyRepository.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Interface ICompanyRepository
        Function GetAsync() As Task(Of CompanyModel)
        Function InsertAsync(item As CompanyModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As CompanyModel, userName As String) As Task(Of Boolean)
    End Interface
End Namespace