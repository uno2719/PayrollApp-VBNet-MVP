Imports System.Threading
Imports DevExpress.XtraReports.UI
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.Payslips.Services
Imports Payroll.Payslips.Views

Namespace Payslips.Presenters

    Public Class PayslipPresenter

        Private ReadOnly _view As IPayslipView
        Private ReadOnly _service As IPayslipService
        Private _emailCts As CancellationTokenSource

        Public Sub New(view As IPayslipView, service As IPayslipService)
            _view = view
            _service = service
        End Sub

        Public ReadOnly Property IsSending As Boolean
            Get
                Return _emailCts IsNot Nothing
            End Get
        End Property

        ' =============================================
        ' LOAD
        ' =============================================

        Public Async Function LoadAsync(Optional selectCutoffId As Integer? = Nothing) As Task
            Dim cutoffs = Await _service.GetCutoffsAsync()
            _view.DisplayCutoffs(cutoffs, selectCutoffId)
        End Function

        Public Async Function LoadEmployeesAsync() As Task
            Dim c = _view.GetSelectedCutoff()
            _view.ShowPreview(Nothing)
            If c Is Nothing Then
                _view.DisplayEmployees(New List(Of PayslipEmployeeRow)())
                Return
            End If
            _view.DisplayEmployees(Await _service.GetEmployeeRowsAsync(c.CutoffID))
        End Function

        ' =============================================
        ' PREVIEW / PRINT / EXPORT
        ' =============================================

        ''' <summary>Payslips ng target (naka-check o lahat), o ng naka-focus lang kapag singleFocused.</summary>
        Private Async Function BuildAsync(singleFocused As Boolean) As Task(Of PayslipBatch)
            Dim c = _view.GetSelectedCutoff()
            If c Is Nothing Then
                _view.ShowMessage("Select a cutoff first.")
                Return Nothing
            End If

            Dim rows As List(Of PayslipEmployeeRow)
            If singleFocused Then
                Dim f = _view.GetFocusedRow()
                rows = If(f Is Nothing, New List(Of PayslipEmployeeRow)(), New List(Of PayslipEmployeeRow) From {f})
            Else
                rows = _view.GetTargetRows()
            End If
            If rows.Count = 0 Then
                _view.ShowMessage("No employees in this cutoff.")
                Return Nothing
            End If

            Dim options = _view.GetOptions()
            options.PrintedBy = AppSession.CurrentUser
            Return Await _service.BuildBatchAsync(c.CutoffID, rows.Select(Function(r) r.RecordId).ToList(), options)
        End Function

        Public Async Function PreviewAsync(singleFocused As Boolean) As Task
            _view.SetBusy(True)
            Try
                Dim batch = Await BuildAsync(singleFocused)
                If batch Is Nothing Then Return
                Dim rpt = rptPayslip.Create(batch)
                rpt.CreateDocument()
                _view.ShowPreview(rpt)
            Catch ex As Exception
                _view.ShowError("Could not build the payslip preview:" & Environment.NewLine & ex.Message)
            Finally
                _view.SetBusy(False)
            End Try
        End Function

        ''' <summary>Hiwalay na preview window ng DevExpress (may sariling toolbar: zoom, search, print, export sa ibang format).</summary>
        Public Async Function OpenViewerAsync() As Task
            Dim batch As PayslipBatch = Nothing
            _view.SetBusy(True)
            Try
                batch = Await BuildAsync(False)
            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.SetBusy(False)
            End Try
            If batch Is Nothing Then Return

            Using tool As New ReportPrintTool(rptPayslip.Create(batch))
                tool.ShowPreviewDialog()
            End Using
        End Function

        Public Async Function PrintAsync() As Task
            Dim batch As PayslipBatch = Nothing
            _view.SetBusy(True)
            Try
                batch = Await BuildAsync(False)
            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.SetBusy(False)
            End Try
            If batch Is Nothing Then Return

            Using tool As New ReportPrintTool(rptPayslip.Create(batch))
                tool.PrintDialog()
            End Using
        End Function

        Public Async Function ExportCombinedAsync() As Task
            Dim c = _view.GetSelectedCutoff()
            If c Is Nothing Then
                _view.ShowMessage("Select a cutoff first.")
                Return
            End If

            Dim payDate = If(c.PayDate.HasValue, c.PayDate.Value, c.CutoffEnd)
            Dim path = _view.AskSaveFile($"Payslips_{SafeLabel(c)}_{payDate:yyyy-MM-dd}.pdf")
            If String.IsNullOrWhiteSpace(path) Then Return

            _view.SetBusy(True)
            Try
                Dim batch = Await BuildAsync(False)
                If batch Is Nothing Then Return
                Await Task.Run(Sub() PayslipPdf.ExportCombined(batch, path))
                _view.SetBusy(False)
                _view.ShowMessage($"Saved {batch.Documents.Count} payslip(s) to:{Environment.NewLine}{path}")
            Catch ex As Exception
                _view.SetBusy(False)
                _view.ShowError("Export failed:" & Environment.NewLine & ex.Message)
            Finally
                _view.SetBusy(False)
            End Try
        End Function

        Public Async Function ExportEachAsync() As Task
            Dim folder = _view.AskFolder("Choose the folder for the payslip PDFs (one file per employee).")
            If String.IsNullOrWhiteSpace(folder) Then Return

            _view.SetBusy(True)
            Try
                Dim batch = Await BuildAsync(False)
                If batch Is Nothing Then Return
                Dim count = Await Task.Run(Function() PayslipPdf.ExportEach(batch, folder))
                _view.SetBusy(False)
                _view.ShowMessage($"Saved {count} PDF file(s) to:{Environment.NewLine}{folder}")
            Catch ex As Exception
                _view.SetBusy(False)
                _view.ShowError("Export failed:" & Environment.NewLine & ex.Message)
            Finally
                _view.SetBusy(False)
            End Try
        End Function

        ' =============================================
        ' EMAIL
        ' =============================================

        ''' <summary>
        ''' Ipadala ang PDF sa email ng bawat empleyado. singleFocused = ang naka-focus lang; kung hindi, ang naka-check
        ''' (o lahat kung walang naka-check). Posted na cutoff lang (hindi ipinapadala ang DRAFT).
        ''' </summary>
        Public Async Function EmailAsync(singleFocused As Boolean) As Task
            If IsSending Then Return

            Dim c = _view.GetSelectedCutoff()
            If c Is Nothing Then
                _view.ShowMessage("Select a cutoff first.")
                Return
            End If
            If c.Status <> CutoffStatus.Posted Then
                _view.ShowError("Only Posted cutoffs can be emailed. This cutoff is " & c.Status.ToString() &
                                " - its payslips are still a DRAFT. Post it in Payroll > Output first.")
                Return
            End If

            Dim rows As List(Of PayslipEmployeeRow)
            If singleFocused Then
                Dim f = _view.GetFocusedRow()
                rows = If(f Is Nothing, New List(Of PayslipEmployeeRow)(), New List(Of PayslipEmployeeRow) From {f})
            Else
                rows = _view.GetTargetRows()
            End If
            If rows.Count = 0 Then
                _view.ShowMessage("Select an employee first.")
                Return
            End If

            Dim withEmail = rows.Where(Function(r) r.HasEmail).ToList()
            Dim noEmail = rows.Count - withEmail.Count
            If withEmail.Count = 0 Then
                _view.ShowError(If(rows.Count = 1, rows(0).EmployeeName & " has no email address.",
                                   "None of the selected employees has an email address.") & Environment.NewLine &
                                "Add it in Employees > Personal Info.")
                Return
            End If

            Dim notReady = Await _service.CheckEmailReadyAsync()
            If notReady IsNot Nothing Then
                _view.ShowError(notReady)
                Return
            End If

            Dim options = _view.GetOptions()
            options.PrintedBy = AppSession.CurrentUser
            Dim alreadySent = withEmail.Where(Function(r) r.LastEmailSuccess.GetValueOrDefault()).Count()

            Dim nl = Environment.NewLine
            Dim msg = $"Send payslips by email for {c.DisplayLabel}?" & nl & nl &
                      $"Employees with email: {withEmail.Count}" & nl &
                      If(noEmail > 0, $"No email address (will be skipped): {noEmail}" & nl, "") &
                      If(alreadySent > 0, $"Already emailed before (will receive another copy): {alreadySent}" & nl, "") &
                      nl & "Include allowance: " & If(options.IncludeAllowance, "Yes", "No") & nl &
                      "Password-protected PDF: " & If(options.ProtectPdf, "Yes (birthdate MMDDYYYY, or Employee ID)", "No")
            If Not _view.AskConfirm(msg, "Email payslips") Then Return

            _emailCts = New CancellationTokenSource()
            Dim results As List(Of PayslipEmailResult) = Nothing
            Try
                _view.SetBusy(True)
                Dim batch = Await _service.BuildBatchAsync(c.CutoffID, withEmail.Select(Function(r) r.RecordId).ToList(), options)
                _view.SetBusy(False)

                _view.SetEmailProgress(True, 0, batch.Documents.Count, "Preparing...")
                Dim progress As New Progress(Of PayslipEmailProgress)(
                    Sub(p) _view.SetEmailProgress(True, p.Done, p.Total, $"Sending {Math.Min(p.Done + 1, p.Total)} of {p.Total}: {p.CurrentName}"))

                results = Await _service.SendEmailsAsync(batch, options, AppSession.CurrentUser, progress, _emailCts.Token)
            Catch ex As Exception
                _view.SetBusy(False)
                _view.ShowError("Sending stopped:" & nl & ex.Message)
            Finally
                Dim cancelled = _emailCts IsNot Nothing AndAlso _emailCts.IsCancellationRequested
                _emailCts?.Dispose()
                _emailCts = Nothing
                _view.SetEmailProgress(False, 0, 0, Nothing)
                If cancelled Then _view.ShowMessage("Sending was stopped. Employees already sent are kept in the log.")
            End Try

            If results IsNot Nothing Then ShowEmailSummary(results, noEmail)

            Try
                Await LoadEmployeesAsync()
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Function

        Public Sub CancelEmail()
            _emailCts?.Cancel()
        End Sub

        Private Sub ShowEmailSummary(results As List(Of PayslipEmailResult), skippedNoEmail As Integer)
            Dim nl = Environment.NewLine
            Dim sent = results.Where(Function(r) r.Success).Count()
            Dim failed = results.Where(Function(r) Not r.Success).ToList()

            Dim sb As New System.Text.StringBuilder()
            sb.Append("Sent: ").Append(sent).Append(nl)
            If skippedNoEmail > 0 Then sb.Append("Skipped (no email address): ").Append(skippedNoEmail).Append(nl)
            If failed.Count > 0 Then
                sb.Append("Failed: ").Append(failed.Count).Append(nl).Append(nl)
                For Each f In failed.Take(10)
                    sb.Append("- ").Append(f.EmployeeName).Append(" <").Append(f.EmailAddress).Append(">: ").Append(f.Message).Append(nl)
                Next
                If failed.Count > 10 Then sb.Append("... and ").Append(failed.Count - 10).Append(" more (see the Email status column).").Append(nl)
                sb.Append(nl).Append("Fix the problem, check the failed rows, and send again.")
                _view.ShowError(sb.ToString())
            Else
                _view.ShowMessage(sb.ToString())
            End If
        End Sub

        Private Shared Function SafeLabel(c As CutoffModel) As String
            Dim label = If(String.IsNullOrWhiteSpace(c.CutoffLabel), c.CycleType, c.CutoffLabel)
            Return SharedFileStorage.SanitizeFileName(If(label, "Cutoff").Replace(" ", ""))
        End Function

    End Class

End Namespace
