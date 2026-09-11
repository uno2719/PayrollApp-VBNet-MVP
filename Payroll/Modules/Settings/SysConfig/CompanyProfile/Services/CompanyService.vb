' File: Modules/Settings/SysConfig/CompanyProfile/Services/CompanyService.vb
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Class CompanyService
        Implements ICompanyService

        Private ReadOnly _repository As Data.ICompanyRepository

        Public Sub New(repository As Data.ICompanyRepository)
            _repository = repository
        End Sub

        Public Async Function GetAsync() As Task(Of CompanyModel) _
            Implements ICompanyService.GetAsync

            Return Await _repository.GetAsync()
        End Function

        Public Async Function SaveAsync(item As CompanyModel, userName As String) As Task(Of CompanySaveResult) _
            Implements ICompanyService.SaveAsync

            If String.IsNullOrWhiteSpace(item.CompanyCode) Then
                Return New CompanySaveResult With {.Success = False, .ErrorMessage = "Company Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.CompanyName) Then
                Return New CompanySaveResult With {.Success = False, .ErrorMessage = "Company Name is required."}
            End If

            If Not String.IsNullOrWhiteSpace(item.ContactPersonEmail) AndAlso Not ValidationHelper.IsValidEmail(item.ContactPersonEmail) Then
                Return New CompanySaveResult With {.Success = False, .ErrorMessage = "Contact Person's email address is invalid."}
            End If

            ' CompanyId = 0 pag wala pang na-Insert kailanman (bagong install) -
            ' iisa lang ang row na ito magpakailanman, kaya first Save = Insert,
            ' lahat ng susunod = Update.
            If item.CompanyId = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New CompanySaveResult With {.Success = True}
        End Function

    End Class
End Namespace