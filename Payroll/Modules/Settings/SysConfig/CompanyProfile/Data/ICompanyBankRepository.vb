' File: Modules/Settings/SysConfig/CompanyProfile/Data/ICompanyBankRepository.vb
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Data
    Public Interface ICompanyBankRepository
        Function GetAllAsync() As Task(Of List(Of CompanyBankModel))
        Function InsertAsync(item As CompanyBankModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As CompanyBankModel, userName As String) As Task(Of Boolean)
        Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)
    End Interface
End Namespace