Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollProcessing.Services
Imports Payroll.PayrollProcessing.Views

Namespace PayrollProcessing.Presenters

    Public Class PayrollInputEntryPresenter

        Private ReadOnly _view As IPayrollInputEntryView
        Private ReadOnly _service As IPayrollInputService
        Private ReadOnly _userName As String

        Private _columns As List(Of PayrollInputColumnModel)

        Public Sub New(view As IPayrollInputEntryView, service As IPayrollInputService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Dim cutoffs = Await _service.GetCutoffsAsync()
            _view.DisplayCutoffs(cutoffs)

            _columns = Await _service.GetColumnsAsync()
            _view.DisplayColumns(_columns)
        End Function

        Public Async Function LoadCutoffDataAsync(cutoffId As Integer) As Task
            Dim table = Await _service.GetInputDataAsync(cutoffId, _columns)
            _view.DisplayData(table)
        End Function

        Public Async Function SaveAsync() As Task
            Dim cutoffId = _view.GetSelectedCutoffId()
            If Not cutoffId.HasValue Then
                _view.ShowError("Please select a Cutoff first.")
                Return
            End If

            Try
                Dim table = _view.GetEditedData()
                Await _service.SaveInputDataAsync(cutoffId.Value, table, _columns, _userName)
                _view.ShowMessage("Saved successfully.")
            Catch ex As PayrollInputValidationException
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace
