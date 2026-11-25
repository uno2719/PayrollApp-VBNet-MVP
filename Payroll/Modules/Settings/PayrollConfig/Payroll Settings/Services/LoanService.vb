Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Class LoanService
        Implements ILoanService

        Private ReadOnly _repository As Data.ILoanRepository

        Public Sub New(repository As Data.ILoanRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of LoanModel)) _
            Implements ILoanService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As LoanModel, userName As String) As Task(Of PayrollSettingsSaveResult) _
            Implements ILoanService.SaveAsync

            If String.IsNullOrWhiteSpace(item.Code) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Code is required."}
            End If

            If String.IsNullOrWhiteSpace(item.Description) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Description is required."}
            End If

            ' --- Default na interes ---
            Dim interestError = NormalizeInterestDefaults(item)
            If interestError IsNot Nothing Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = interestError}
            End If

            Dim isDuplicate = Await _repository.CodeExistsAsync(item.Code.Trim(), item.Id)
            If isDuplicate Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = $"Code '{item.Code}' is already in use."}
            End If

            If item.Id = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILoanService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function


        ''' <summary>
        ''' Nililinis at sinusuri ang default na interes. Ibinabalik ang error message, o Nothing kung okay.
        '''   SSS Loan / PAG-IBIG Loan: REQUIRED ang Interest Method at Rate (> 0); Diminishing -> required din ang Rate basis.
        '''   Ibang type: optional, pero kapag may Rate, dapat may Interest Method.
        '''   Straight Line: walang Rate basis (flat % ng principal, kaya hindi ito ginagamit ng engine).
        ''' </summary>
        Private Shared Function NormalizeInterestDefaults(item As LoanModel) As String

            item.DefaultInterestMethod = If(String.IsNullOrWhiteSpace(item.DefaultInterestMethod), Nothing, item.DefaultInterestMethod.Trim())
            item.DefaultRatePeriod = If(String.IsNullOrWhiteSpace(item.DefaultRatePeriod), Nothing, item.DefaultRatePeriod.Trim())
            If item.DefaultInterestRate.HasValue AndAlso item.DefaultInterestRate.Value = 0D AndAlso item.DefaultInterestMethod Is Nothing Then
                item.DefaultInterestRate = Nothing
            End If

            Dim required = LoanTypeName.RequiresInterestDefaults(item.LoanType)

            If item.DefaultInterestMethod Is Nothing Then
                If required Then Return $"Interest Method is required for {item.LoanType}."
                If item.DefaultInterestRate.HasValue Then Return "Choose an Interest Method for the default Interest Rate."
                item.DefaultRatePeriod = Nothing
                Return Nothing
            End If

            If Not LoanInterestMethod.All.Contains(item.DefaultInterestMethod) Then Return "Invalid Interest Method."

            Dim rate = If(item.DefaultInterestRate, 0D)
            If rate < 0D OrElse rate > 100D Then Return "Interest Rate must be between 0 and 100 percent."
            If required AndAlso rate <= 0D Then Return $"Interest Rate is required for {item.LoanType}."
            item.DefaultInterestRate = rate

            If String.Equals(item.DefaultInterestMethod, LoanInterestMethod.Diminishing, StringComparison.OrdinalIgnoreCase) Then
                If item.DefaultRatePeriod Is Nothing Then Return "Choose the Interest Rate basis (Per Year or Per Period)."
                If Not LoanRatePeriod.All.Contains(item.DefaultRatePeriod) Then Return "Invalid Interest Rate basis."
            Else
                item.DefaultRatePeriod = Nothing
            End If

            Return Nothing

        End Function

        ' Totoong pagbura - admin lang at may in-use check (nasa repository/RecordDeleter).
        Public Async Function DeleteAsync(id As Integer) As Task(Of DeleteResult) _
            Implements ILoanService.DeleteAsync

            If Not AppSession.IsAdmin Then
                Return DeleteResult.Fail("Only administrators can delete records.")
            End If

            Return Await _repository.DeleteAsync(id)
        End Function

    End Class
End Namespace
