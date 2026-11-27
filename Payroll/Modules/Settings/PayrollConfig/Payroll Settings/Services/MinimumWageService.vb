' File: Modules/Settings/PayrollConfig/Payroll Settings/Services/MinimumWageService.vb
' ============================================================
' Settings > Payroll > Minimum Wage
'   1) Mga rate bawat Wage Area at petsa (hal. NCR 755.00 mula 09/26/2026, WO NCR-28).
'      Ang Wage Area ay malayang pangalan para kasya ang mga rehiyong may iba-ibang rate ayon sa klase ng lugar
'      (hal. "CALABARZON - Component City" at "CALABARZON - 1st Class Municipality").
'   2) Aling Wage Area ang bawat branch/store (tblBranch.WageArea).
' Ginagamit lang ito bilang PAALALA para sa "Minimum Wage Earner" (tax-exempt) checkbox ng empleyado
' (Salary adjustment, Transfer, Change tax status, at ang alert sa Employee header). Ang checkbox pa rin ang batayan ng tax.
' Permission: Settings > Payroll (settings_Payroll) Can Edit.
' ============================================================
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class MinimumWageService

        Public Const MaxAreaLength As Integer = 60
        Public Const MaxDailyRate As Decimal = 99999.99D

        Private ReadOnly _repo As IMinimumWageRepository

        Public Sub New()
            _repo = New MinimumWageRepository()
        End Sub

        Public Sub New(repo As IMinimumWageRepository)
            _repo = repo
        End Sub

        Public Shared Function CanEdit() As Boolean
            Return GlobalShared.Security.PermissionService.CanEdit(ModuleCodes.Settings_Payroll)
        End Function

        Private Shared Sub RequireEdit()
            If Not CanEdit() Then Throw New UnauthorizedAccessException("You are not allowed to change Payroll Settings.")
        End Sub

        Public Function GetRatesAsync() As Task(Of List(Of MinimumWageModel))
            Return _repo.GetRatesAsync()
        End Function

        Public Function GetBranchesAsync() As Task(Of List(Of BranchWageAreaModel))
            Return _repo.GetBranchesAsync()
        End Function

        ''' <summary>Empty = valid.</summary>
        Public Shared Function Validate(item As MinimumWageModel, existing As IEnumerable(Of MinimumWageModel)) As String
            If item Is Nothing Then Return "Nothing to save."
            Dim area = If(item.WageArea, "").Trim()
            If area.Length = 0 Then Return "Wage Area is required (e.g. NCR, CAR, CALABARZON - Component City)."
            If area.Length > MaxAreaLength Then Return $"Wage Area is too long (maximum {MaxAreaLength} characters)."
            If item.DailyRate <= 0D Then Return "Daily minimum wage must be more than 0."
            If item.DailyRate > MaxDailyRate Then Return "Daily minimum wage is too large. Please check the amount."
            If item.EffectiveDate.Year < 2000 Then Return "Enter the Effective Date."
            If If(item.WageOrderNo, "").Trim().Length > 50 Then Return "Wage Order No. is too long (maximum 50 characters)."
            If If(item.Remarks, "").Trim().Length > 300 Then Return "Remarks is too long (maximum 300 characters)."

            If existing IsNot Nothing AndAlso existing.Any(Function(r) r.MinimumWageId <> item.MinimumWageId AndAlso
                                                                     String.Equals(If(r.WageArea, "").Trim(), area, StringComparison.OrdinalIgnoreCase) AndAlso
                                                                     r.EffectiveDate.Date = item.EffectiveDate.Date) Then
                Return $"{area} already has a rate effective {item.EffectiveDate:MM/dd/yyyy}. Edit that row instead."
            End If
            Return String.Empty
        End Function

        Public Async Function SaveRateAsync(item As MinimumWageModel) As Task(Of Integer)
            RequireEdit()
            item.WageArea = If(item.WageArea, "").Trim()
            item.WageOrderNo = If(String.IsNullOrWhiteSpace(item.WageOrderNo), Nothing, item.WageOrderNo.Trim())
            item.Remarks = If(String.IsNullOrWhiteSpace(item.Remarks), Nothing, item.Remarks.Trim())
            item.DailyRate = Math.Round(item.DailyRate, 2)

            Dim problem = Validate(item, Await _repo.GetRatesAsync())
            If problem.Length > 0 Then Throw New InvalidOperationException(problem)

            If item.MinimumWageId > 0 Then
                Await _repo.UpdateRateAsync(item, AppSession.CurrentUser)
                Return item.MinimumWageId
            End If
            Return Await _repo.InsertRateAsync(item, AppSession.CurrentUser)
        End Function

        Public Async Function DeleteRateAsync(minimumWageId As Integer) As Task
            RequireEdit()
            Await _repo.DeleteRateAsync(minimumWageId)
        End Function

        Public Async Function SetBranchWageAreaAsync(branchId As Integer, wageArea As String) As Task
            RequireEdit()
            If If(wageArea, "").Trim().Length > MaxAreaLength Then
                Throw New InvalidOperationException($"Wage Area is too long (maximum {MaxAreaLength} characters).")
            End If
            Await _repo.SetBranchWageAreaAsync(branchId, wageArea, AppSession.CurrentUser)
        End Function

    End Class

End Namespace
