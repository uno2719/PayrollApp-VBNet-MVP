Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models

Namespace LeaveSettings.Services
    Public Class LeaveRuleService
        Implements ILeaveRuleService

        Private ReadOnly _repository As Data.ILeaveRuleRepository

        Public Sub New(repository As Data.ILeaveRuleRepository)
            _repository = repository
        End Sub

        Public Async Function GetAllAsync() As Task(Of List(Of LeaveRuleModel)) _
            Implements ILeaveRuleService.GetAllAsync

            Return Await _repository.GetAllAsync()
        End Function

        Public Async Function GetBracketsAsync(leaveRuleId As Integer) As Task(Of List(Of LeaveRuleBracketModel)) _
            Implements ILeaveRuleService.GetBracketsAsync

            Return Await _repository.GetBracketsAsync(leaveRuleId)
        End Function

        Public Async Function SaveAsync(item As LeaveRuleModel,
                                         brackets As List(Of LeaveRuleBracketModel),
                                         userName As String) As Task(Of LeaveRuleSaveResult) _
            Implements ILeaveRuleService.SaveAsync

            ' --- HEADER validation ---
            If item.LeaveGroupId <= 0 Then
                Return Fail("Please select a Leave Group.")
            End If

            If item.LeaveTypeId <= 0 Then
                Return Fail("Please select a Leave Type.")
            End If

            If Not LeaveEntitlementMethod.All.Contains(item.EntitlementMethod) Then
                Return Fail("Please select a valid Entitlement Method.")
            End If

            If Not LeaveDateBasis.All.Contains(item.ComputeBasedOn) Then
                Return Fail("Please select a valid 'Compute Based On'.")
            End If

            If Not LeaveDateBasis.All.Contains(item.PlotBasedOn) Then
                Return Fail("Please select a valid 'Plot Based On'.")
            End If

            If Not LeaveDateBasis.All.Contains(item.AnniversaryPlotOn) Then
                Return Fail("Please select a valid 'Anniversary Plot On'.")
            End If

            If Not LeaveUnitOfMeasure.All.Contains(item.UnitOfMeasure) Then
                Return Fail("Please select a valid Unit of Measure.")
            End If

            Dim isDuplicate = Await _repository.CombinationExistsAsync(item.LeaveGroupId, item.LeaveTypeId, item.Id)
            If isDuplicate Then
                Return Fail("A Rule already exists for this Leave Group + Leave Type combination.")
            End If

            ' --- BRACKETS validation ---
            If brackets Is Nothing OrElse brackets.Count = 0 Then
                Return Fail("Please add at least one Years-of-Service bracket.")
            End If

            For Each b In brackets
                If b.YosFrom < 0 OrElse b.YosTo < 0 Then
                    Return Fail("Years of Service values cannot be negative.")
                End If

                If b.YosFrom >= b.YosTo Then
                    Return Fail($"'YOS From' ({b.YosFrom}) must be less than 'YOS To' ({b.YosTo}).")
                End If

                If b.ForfeitOnMonth < 0 OrElse b.ForfeitOnMonth > 12 Then
                    Return Fail("'Forfeit on Month' must be between 0 (no forfeiture) and 12.")
                End If

                If b.Entitlement < 0 OrElse b.AnniversaryCredit < 0 OrElse b.BFMax < 0 Then
                    Return Fail("Entitlement, Anniversary Credit, and BF Max cannot be negative.")
                End If
            Next

            ' Overlap check - in-memory lang (maliit na listahan, sabay
            ' na-e-edit lahat), parehong diwa ng StatutorySettings'
            ' OverlapExistsAsync pero hindi na kailangan ng query dahil
            ' nandito na lahat ng candidate rows sa isang working list.
            Dim sorted = brackets.OrderBy(Function(b) b.YosFrom).ToList()
            For i = 0 To sorted.Count - 2
                If sorted(i).YosTo > sorted(i + 1).YosFrom Then
                    Return Fail($"Overlapping YOS ranges: {sorted(i).YosFrom}-{sorted(i).YosTo} and {sorted(i + 1).YosFrom}-{sorted(i + 1).YosTo}.")
                End If
            Next

            Dim savedId = Await _repository.SaveWithBracketsAsync(item, brackets, userName)

            Return New LeaveRuleSaveResult With {.Success = True, .SavedId = savedId}

        End Function

        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILeaveRuleService.SetActiveStatusAsync

            Return Await _repository.SetActiveStatusAsync(id, isActive, userName)
        End Function

        Private Shared Function Fail(message As String) As LeaveRuleSaveResult
            Return New LeaveRuleSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

    End Class
End Namespace
