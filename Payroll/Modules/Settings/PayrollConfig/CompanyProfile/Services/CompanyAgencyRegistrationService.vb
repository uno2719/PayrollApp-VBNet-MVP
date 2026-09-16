' File: Modules/Settings/SysConfig/CompanyProfile/Services/CompanyAgencyRegistrationService.vb
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace CompanyProfile.Services
    Public Class CompanyAgencyRegistrationService
        Implements ICompanyAgencyRegistrationService

        Private ReadOnly _repository As Data.ICompanyAgencyRegistrationRepository

        Public Sub New(repository As Data.ICompanyAgencyRegistrationRepository)
            _repository = repository
        End Sub

        Public Async Function GetAsync(agencyType As String) As Task(Of CompanyAgencyRegistrationModel) _
            Implements ICompanyAgencyRegistrationService.GetAsync

            Return Await _repository.GetAsync(agencyType)
        End Function

        Public Async Function SaveAsync(agencyType As String, item As CompanyAgencyRegistrationModel, userName As String) As Task(Of CompanyAgencyRegistrationSaveResult) _
            Implements ICompanyAgencyRegistrationService.SaveAsync

            Dim info = CompanyAgencyTypeRegistry.GetInfo(agencyType)

            ' Dynamic yung error message dahil iba-iba ang label per agency
            ' (SSS No. / PhilHealth No. / Pag-IBIG No. / TIN) - kunin natin
            ' mismo sa registry, hindi na natin i-hahardcode dito.
            If String.IsNullOrWhiteSpace(item.RegistrationNo) Then
                Return New CompanyAgencyRegistrationSaveResult With {.Success = False, .ErrorMessage = $"{info.RegistrationNoLabel} is required."}
            End If

            If Not String.IsNullOrWhiteSpace(item.ContactPersonEmail) AndAlso Not ValidationHelper.IsValidEmail(item.ContactPersonEmail) Then
                Return New CompanyAgencyRegistrationSaveResult With {.Success = False, .ErrorMessage = "Contact Person's email address is invalid."}
            End If

            If Not String.IsNullOrWhiteSpace(item.PersonInCharge1Email) AndAlso Not ValidationHelper.IsValidEmail(item.PersonInCharge1Email) Then
                Return New CompanyAgencyRegistrationSaveResult With {.Success = False, .ErrorMessage = "Person-in-charge 1's email address is invalid."}
            End If

            If Not String.IsNullOrWhiteSpace(item.PersonInCharge2Email) AndAlso Not ValidationHelper.IsValidEmail(item.PersonInCharge2Email) Then
                Return New CompanyAgencyRegistrationSaveResult With {.Success = False, .ErrorMessage = "Person-in-charge 2's email address is invalid."}
            End If

            If item.AgencyRegistrationId = 0 Then
                Await _repository.InsertAsync(agencyType, item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New CompanyAgencyRegistrationSaveResult With {.Success = True}
        End Function

    End Class
End Namespace