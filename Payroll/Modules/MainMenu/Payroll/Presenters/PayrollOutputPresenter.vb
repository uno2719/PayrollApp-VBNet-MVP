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

        ''' <summary>
        ''' I-reload ang laman ng combo ayon sa mode: per-cutoff, o "Group by pay date".
        ''' Pinapanatili ng view ang napili kung nandoon pa rin.
        ''' </summary>
        Public Async Function LoadAsync() As Task
            If _view.IsGroupByPayDate() Then
                Dim groups = Await _service.GetPayDateGroupsAsync()
                _view.DisplayPayDates(groups)
            Else
                Dim cutoffs = Await _service.GetResultCutoffsAsync()
                _view.DisplayCutoffs(cutoffs)
            End If
        End Function

        Public Async Function LoadSelectionAsync(cutoffIds As IList(Of Integer)) As Task
            Dim sheet = Await _service.GetResultsForCutoffsAsync(cutoffIds)
            _view.DisplayResults(sheet.Headers, sheet.Lines)
        End Function

        ''' <summary>
        ''' Export: plan -> (Skip/Cancel kung may lalaktawan) -> (Batch kung BDO Direct) -> Save dialog -> sulat ng file + log.
        ''' kind = ExportFormat.BdoConverter / BdoDirect / OtherLists
        ''' </summary>
        Public Async Function ExportAsync(kind As String) As Task
            Dim cutoffIds = _view.GetSelectedCutoffIds()
            If cutoffIds.Count = 0 Then
                _view.ShowMessage("Select a cutoff (or pay date) first.")
                Return
            End If

            Try
                Dim plan = Await _service.BuildExportPlanAsync(cutoffIds, kind)

                If plan.Rows.Count = 0 Then
                    If plan.Skipped.Count > 0 Then
                        _view.ShowError("Nothing can be exported for this list." & Environment.NewLine & Environment.NewLine &
                                        plan.SkippedText(asPrompt:=False))
                    Else
                        _view.ShowMessage("Nobody is paid through this list in the selection.")
                    End If
                    Return
                End If

                If plan.Skipped.Count > 0 Then
                    If Not _view.AskConfirm(plan.SkippedText(), "Employees that will be skipped") Then Return
                End If

                ' BDO Direct: CompanyCode + MMDDYY + Batch
                If kind = ExportFormat.BdoDirect Then
                    Dim naming = Await _service.GetBdoDirectNamingAsync()
                    Dim batch = AskBatch(naming)
                    If Not batch.HasValue Then Return
                    plan.SuggestedFileName = naming.FileNameFor(batch.Value)
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

        ''' <summary>Batch number ng BDO: buong numero 1 hanggang 10, hindi pwedeng ulitin sa isang araw. Nothing kapag kinansela.</summary>
        Private Function AskBatch(naming As BdoDirectNaming) As Integer?
            Dim used = If(naming.UsedBatches.Count > 0,
                          "Already used today (in this app): " & String.Join(", ", naming.UsedBatches),
                          "No batch used yet today in this app.")
            Dim defaultText = If(naming.NextFreeBatch > 0, naming.NextFreeBatch.ToString(), "")

            Do
                Dim answer = _view.AskInput(
                    $"BDO batch number (whole number, 1 to {BdoDirectNaming.MaxBatch}, unique per day)." & Environment.NewLine & used & Environment.NewLine &
                    $"File name will start with {naming.FilePrefix()}",
                    "BDO Batch", defaultText)

                If String.IsNullOrWhiteSpace(answer) Then Return Nothing

                Dim n As Integer
                If Not Integer.TryParse(answer.Trim(), n) OrElse n < 1 OrElse n > BdoDirectNaming.MaxBatch Then
                    _view.ShowError($"The batch must be a whole number from 1 to {BdoDirectNaming.MaxBatch}.")
                    Continue Do
                End If

                If naming.UsedBatches.Contains(n) Then
                    Dim useAnyway = _view.AskConfirm(
                        $"Batch {n} was already used today for a BDO file made in this app. BDO needs a unique batch number per day. Use it anyway?",
                        "BDO Batch")
                    If Not useAnyway Then Continue Do
                End If

                Return n
            Loop
        End Function

        ''' <summary>
        ''' Post: sinusuri -> Confirm (FINAL) -> isang transaction -> reload.
        ''' Sa "Group by pay date", lahat ng Processed na cutoff ng pay date ay sabay na ipo-post (all-or-nothing).
        ''' </summary>
        Public Async Function PostAsync() As Task
            Dim cutoffIds = _view.GetSelectedCutoffIds()
            If cutoffIds.Count = 0 Then
                _view.ShowMessage("Select a cutoff (or pay date) first.")
                Return
            End If

            Dim payDate As Date? = Nothing
            If _view.IsGroupByPayDate() Then payDate = _view.GetSelectedPayDate()

            Try
                Dim state = Await _service.GetPostStateAsync(cutoffIds, payDate)

                If state.IsPosted Then
                    _view.ShowMessage("Already Posted.")
                    Return
                End If
                If Not state.CanPost Then
                    _view.ShowError(state.BlockedText())
                    Return
                End If
                If Not _view.AskConfirm(state.ConfirmText(), "Post") Then Return

                Dim result = Await _service.PostAsync(cutoffIds, payDate, AppSession.CurrentUser)

                _view.ShowMessage($"Posted {result.CutoffsPosted} cutoff(s)." & Environment.NewLine &
                                  $"Loan installments marked paid: {result.LoanInstallmentsPaid} ({result.PayrollRefNo})" &
                                  If(result.LoansCompleted > 0, Environment.NewLine & $"Loans completed: {result.LoansCompleted}", ""))

                Await LoadAsync()
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace
