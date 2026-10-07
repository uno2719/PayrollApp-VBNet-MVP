Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    Public Class GenerateCutoffPresenter

        Private ReadOnly _view As Views.IGenerateCutoffView
        Private ReadOnly _service As Services.ICutoffService
        Private ReadOnly _userName As String

        ' Ang pinakahuling RefreshPreview lang ang pinapayagang mag-apply sa view
        ' (kapag mabilis magpalit ng year, baka mauna matapos ang mas lumang tawag).
        Private _previewTicket As Integer = 0

        Public Sub New(view As Views.IGenerateCutoffView, service As Services.ICutoffService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Dim cycles = Await _service.GetActiveCycleTypesAsync()
            _view.BindCycles(cycles)
            _view.CutoffYear = Date.Today.Year

            If cycles.Count = 0 Then
                _view.ShowSummary("No active pay cycle. Activate one in Pay Cycle Settings first.")
                _view.SetGenerateEnabled(False)
                Return
            End If

            _view.CycleType = cycles(0)
            Await RefreshPreviewAsync()
        End Function

        Public Async Function RefreshPreviewAsync() As Task
            Dim cycleType = If(_view.CycleType, "").Trim()
            Dim year = _view.CutoffYear

            _previewTicket += 1
            Dim ticket = _previewTicket

            If cycleType = "" Then
                _view.BindPreview(New List(Of CutoffPreviewRow)())
                _view.SetGenerateEnabled(False)
                Return
            End If

            Try
                Dim pattern = Await _service.GetPatternDescriptionAsync(cycleType)
                Dim rows = Await _service.PreviewForYearAsync(cycleType, year)
                If ticket <> _previewTicket Then Return

                Dim newCount = rows.Where(Function(r) Not r.AlreadyExists).Count()
                Dim existingCount = rows.Count - newCount

                _view.ShowPatternText(If(pattern = "", "", "Pattern (Pay Cycle Settings):  " & pattern))
                _view.BindPreview(rows)
                _view.ShowSummary($"{newCount} new, {existingCount} already exist")
                _view.SetGenerateEnabled(newCount > 0)

            Catch ex As Services.PayrollCutoffGenerateException
                If ticket <> _previewTicket Then Return
                _view.ShowPatternText("")
                _view.BindPreview(New List(Of CutoffPreviewRow)())
                _view.ShowSummary(ex.Message)
                _view.SetGenerateEnabled(False)
            End Try
        End Function

        Public Async Function GenerateAsync() As Task
            Dim cycleType = If(_view.CycleType, "").Trim()
            Dim year = _view.CutoffYear

            If cycleType = "" Then
                _view.ShowError("Select a pay cycle first.")
                Return
            End If

            Try
                Dim count = Await _service.GenerateForYearAsync(cycleType, year, _userName)
                _view.ShowMessage($"{count} {cycleType} cutoff(s) created for {year}.")
                _view.CloseWithSuccess()
            Catch ex As Services.PayrollCutoffGenerateException
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace
