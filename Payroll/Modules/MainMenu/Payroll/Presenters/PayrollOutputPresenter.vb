Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollProcessing.Services
Imports Payroll.PayrollProcessing.Views

Namespace PayrollProcessing.Presenters

    Public Class PayrollOutputPresenter

        Private ReadOnly _view As IPayrollOutputView
        Private ReadOnly _service As IPayrollProcessService

        Public Sub New(view As IPayrollOutputView, service As IPayrollProcessService)
            _view = view
            _service = service
        End Sub

        ''' <summary>I-reload ang listahan ng cutoff na may results; pinapanatili ng view ang napili kung nandoon pa rin.</summary>
        Public Async Function LoadAsync() As Task
            Dim cutoffs = Await _service.GetResultCutoffsAsync()
            _view.DisplayCutoffs(cutoffs)
        End Function

        Public Async Function LoadCutoffAsync(cutoffId As Integer) As Task
            Dim sheet = Await _service.GetResultsAsync(cutoffId)
            _view.DisplayResults(sheet.Headers, sheet.Lines)
        End Function

        ''' <summary>
        ''' Export: plan -> (Skip/Cancel kung may lalaktawan) -> Save dialog -> sulat ng file + log.
        ''' kind = ExportFormat.BdoConverter / BdoDirect / OtherLists
        ''' </summary>
        Public Async Function ExportAsync(kind As String) As Task
            Dim cutoffId = _view.GetSelectedCutoffId()
            If Not cutoffId.HasValue Then
                _view.ShowMessage("Select a cutoff first.")
                Return
            End If

            Try
                Dim plan = Await _service.BuildExportPlanAsync(cutoffId.Value, _view.GetCombineSamePayDate(), kind)

                If plan.Rows.Count = 0 Then
                    If plan.Skipped.Count > 0 Then
                        _view.ShowError("Nothing can be exported for this list." & Environment.NewLine & Environment.NewLine &
                                        plan.SkippedText(asPrompt:=False))
                    Else
                        _view.ShowMessage("Nobody is paid through this list in the selected cutoff.")
                    End If
                    Return
                End If

                If plan.Skipped.Count > 0 Then
                    If Not _view.AskConfirm(plan.SkippedText(), "Employees that will be skipped") Then Return
                End If

                Dim path = _view.AskSaveFile(plan.SuggestedFileName, plan.FileFilter, plan.DefaultExt)
                If String.IsNullOrWhiteSpace(path) Then Return

                Await _service.WriteExportAsync(plan, path, AppSession.CurrentUser)

                _view.ShowMessage($"Saved: {path}" & Environment.NewLine &
                                  $"Employees: {plan.Rows.Count}    Total: {plan.TotalAmount:N2}" &
                                  If(plan.Skipped.Count > 0, Environment.NewLine & $"Skipped: {plan.Skipped.Count}", ""))
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Function

        ''' <summary>Post: sinusuri -> Confirm (FINAL) -> isang transaction -> reload.</summary>
        Public Async Function PostAsync() As Task
            Dim cutoffId = _view.GetSelectedCutoffId()
            If Not cutoffId.HasValue Then
                _view.ShowMessage("Select a cutoff first.")
                Return
            End If

            Try
                Dim state = Await _service.GetPostStateAsync(cutoffId.Value)

                If state.IsPosted Then
                    _view.ShowMessage("This cutoff is already Posted.")
                    Return
                End If
                If Not state.CanPost Then
                    _view.ShowError(state.BlockedText())
                    Return
                End If
                If Not _view.AskConfirm(state.ConfirmText(), "Post Cutoff") Then Return

                Dim result = Await _service.PostAsync(cutoffId.Value, AppSession.CurrentUser)

                _view.ShowMessage("Cutoff posted." & Environment.NewLine &
                                  $"Loan installments marked paid: {result.LoanInstallmentsPaid} ({result.PayrollRefNo})" &
                                  If(result.LoansCompleted > 0, Environment.NewLine & $"Loans completed: {result.LoansCompleted}", ""))

                Await LoadAsync()
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace
