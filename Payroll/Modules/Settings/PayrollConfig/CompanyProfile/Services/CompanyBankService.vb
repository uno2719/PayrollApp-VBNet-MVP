' File: Modules/Settings/SysConfig/CompanyProfile/Services/CompanyBankService.vb
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Class CompanyBankService
        Implements ICompanyBankService

        Private ReadOnly _repository As Data.ICompanyBankRepository

        Public Sub New(repository As Data.ICompanyBankRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of CompanyBankModel)) _
            Implements ICompanyBankService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As CompanyBankModel, userName As String) As Task(Of CompanyBankSaveResult) _
            Implements ICompanyBankService.SaveAsync

            If String.IsNullOrWhiteSpace(item.BankName) Then
                Return New CompanyBankSaveResult With {.Success = False, .ErrorMessage = "Bank is required."}
            End If

            If String.IsNullOrWhiteSpace(item.AccountNo) Then
                Return New CompanyBankSaveResult With {.Success = False, .ErrorMessage = "Account No. is required."}
            End If

            If Not String.IsNullOrWhiteSpace(item.ContactPersonEmail) AndAlso Not ValidationHelper.IsValidEmail(item.ContactPersonEmail) Then
                Return New CompanyBankSaveResult With {.Success = False, .ErrorMessage = "Contact Person's email address is invalid."}
            End If

            If item.BankId = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New CompanyBankSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ICompanyBankService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function

    End Class
End Namespace