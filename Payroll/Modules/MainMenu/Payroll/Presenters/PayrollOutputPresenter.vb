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

    End Class

End Namespace
