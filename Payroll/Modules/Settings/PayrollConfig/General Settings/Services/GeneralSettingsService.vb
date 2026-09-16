' File: Modules/Settings/SysConfig/GeneralSettings/Services/GeneralSettingsService.vb
Imports Payroll.GlobalShared.Models

Namespace GeneralSettings.Services
    Public Class GeneralSettingsService
        Implements IGeneralSettingsService

        Private ReadOnly _repository As Data.IGeneralSettingsRepository

        ' Ang dalawang ito lang ang tinatanggap na value para sa SSS /
        ' PhilHealth basis - kapareho ng nasa ComboBoxEdit sa View. Dito
        ' din naka-sentro ang listahan para hindi mag-drift ang UI at
        ' ang validation (kung magdagdag ka ng bagong basis, dalawang
        ' lugar lang ang babaguhin: dito at sa ucGeneralSettings).
        Private Shared ReadOnly AllowedStatutoryBasis As String() = {
            "Progressive Earnings", "Fixed Basic"
        }

        Public Sub New(repository As Data.IGeneralSettingsRepository)
            _repository = repository
        End Sub

        Public Async Function GetAsync() As Task(Of GeneralSettingsModel) _
            Implements IGeneralSettingsService.GetAsync

            Return Await _repository.GetAsync()
        End Function

        Public Async Function SaveAsync(item As GeneralSettingsModel, userName As String) As Task(Of GeneralSettingsSaveResult) _
            Implements IGeneralSettingsService.SaveAsync

            ' --- Payroll Parameters ---
            If item.BonusCeiling < 0D Then
                Return Fail("Bonus Ceiling cannot be negative.")
            End If

            ' 365 o 366 ang karaniwan, pero may kompanyang gumagamit ng
            ' 313 / 261 (working days) bilang divisor - kaya maluwag ang
            ' range, basta hindi zero o imposible.
            If item.TotalDaysPerYear < 1 OrElse item.TotalDaysPerYear > 366 Then
                Return Fail("Total Days Per Year must be between 1 and 366.")
            End If

            If item.WorkHourPerDay <= 0D OrElse item.WorkHourPerDay > 24D Then
                Return Fail("Work Hour Per Day must be greater than 0 and not more than 24.")
            End If

            If item.AmountPrecision < 0 OrElse item.AmountPrecision > 6 Then
                Return Fail("Amount Precision must be between 0 and 6.")
            End If

            If item.PercentPrecision < 0 OrElse item.PercentPrecision > 6 Then
                Return Fail("Percent Precision must be between 0 and 6.")
            End If

            ' --- Code Mapping ---
            ' Basic Salary Code (+) lang ang required: wala kang maibabatay
            ' na earnings kung wala nito. Ang iba ay opsyonal - may mga
            ' kompanyang walang Early Out o Late policy, at pwedeng hindi
            ' pa na-set up ang Deduction codes sa unang Save.
            If Not item.BasicSalaryCodePlusId.HasValue Then
                Return Fail("Basic Salary Code (+) is required.")
            End If

            ' --- Statutory Basis ---
            If Not AllowedStatutoryBasis.Contains(item.SSSBasedOn) Then
                Return Fail("SSS Based on must be either 'Progressive Earnings' or 'Fixed Basic'.")
            End If

            If Not AllowedStatutoryBasis.Contains(item.PhilHealthBasedOn) Then
                Return Fail("PhilHealth Based on must be either 'Progressive Earnings' or 'Fixed Basic'.")
            End If

            ' GeneralSettingsId = 0 pag wala pang na-Insert kailanman
            ' (bagong install) - iisa lang ang row na ito magpakailanman,
            ' kaya first Save = Insert, lahat ng susunod = Update.
            If item.GeneralSettingsId = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New GeneralSettingsSaveResult With {.Success = True}
        End Function

        Private Shared Function Fail(message As String) As GeneralSettingsSaveResult
            Return New GeneralSettingsSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

    End Class
End Namespace