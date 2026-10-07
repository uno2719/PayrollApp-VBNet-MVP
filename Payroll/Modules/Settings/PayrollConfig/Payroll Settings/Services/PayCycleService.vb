Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class PayCycleService
        Implements IPayCycleService

        Public Const DailyCycle As String = "Daily"
        Public Const WeeklyCycle As String = "Weekly"

        Private ReadOnly _repository As IPayCycleRepository

        Public Sub New(repository As IPayCycleRepository)
            _repository = repository
        End Sub

        Public Function GetAllAsync() As Task(Of List(Of PayCycleModel)) Implements IPayCycleService.GetAllAsync
            Return _repository.GetAllAsync()
        End Function

        ''' <summary>
        ''' Nothing = hindi naka-lock. Kapag may laman, iyon ang dahilan na ipapakita sa user.
        ''' </summary>
        Public Async Function GetRateBasisLockReasonAsync(payCycleType As String) As Task(Of String) _
            Implements IPayCycleService.GetRateBasisLockReasonAsync

            If String.Equals(payCycleType, DailyCycle, StringComparison.OrdinalIgnoreCase) Then
                Return "Locked: the Daily pay cycle is always paid by daily rate."
            End If

            If Await _repository.HasProcessedCutoffAsync(payCycleType) Then
                Return "Locked: this pay cycle already has processed cutoffs, so its rate basis can no longer change."
            End If

            Return Nothing
        End Function

        Public Async Function SaveAsync(item As PayCycleModel, userName As String) As Task(Of PayrollSettingsSaveResult) _
            Implements IPayCycleService.SaveAsync

            If item Is Nothing OrElse String.IsNullOrWhiteSpace(item.PayCycleType) Then
                Return Fail("Pay cycle is required.")
            End If

            ' 1. Daily = laging daily rate
            If String.Equals(item.PayCycleType, DailyCycle, StringComparison.OrdinalIgnoreCase) _
               AndAlso item.RateBasis <> PayRateBasis.DailyRate Then
                Return Fail("The Daily pay cycle is always paid by daily rate and cannot be changed.")
            End If

            ' 2. Rate basis lock kapag may processed cutoff na (para hindi magbago ang lumang sweldo)
            Dim existing = Await _repository.GetByTypeAsync(item.PayCycleType)
            If existing Is Nothing Then
                Return Fail($"Pay cycle '{item.PayCycleType}' was not found. Run the 02_PayCycle.sql script first.")
            End If

            If existing.RateBasis <> item.RateBasis Then
                Dim reason = Await GetRateBasisLockReasonAsync(item.PayCycleType)
                If reason IsNot Nothing Then
                    Return Fail(reason)
                End If
            End If

            ' 3. Pattern: ang Weekly lang ang pwedeng walang pattern (every-7-days ang sarili niyang generator)
            Dim periods = If(item.Periods, New List(Of PayCyclePeriodModel)())
            Dim isEmptyWeekly = String.Equals(item.PayCycleType, WeeklyCycle, StringComparison.OrdinalIgnoreCase) AndAlso periods.Count = 0

            If Not isEmptyWeekly Then
                Dim problem = PayCyclePatternHelper.Validate(periods)
                If problem IsNot Nothing Then
                    Return Fail(problem)
                End If
            End If

            ' 4. Siguraduhing sunod-sunod ang PeriodNo (1..n) ayon sa pagkakasunod sa grid
            For i = 0 To periods.Count - 1
                periods(i).PeriodNo = i + 1
            Next
            item.Periods = periods
            item.PayCycleId = existing.PayCycleId

            Dim saved = Await _repository.SaveAsync(item, userName)
            If Not saved Then
                Return Fail("Nothing was saved. The pay cycle may have been removed.")
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Private Shared Function Fail(message As String) As PayrollSettingsSaveResult
            Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

    End Class

End Namespace
