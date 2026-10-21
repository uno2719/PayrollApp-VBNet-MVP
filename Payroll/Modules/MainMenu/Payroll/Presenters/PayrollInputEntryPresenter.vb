Imports Payroll.PayrollProcessing.Models
Imports Payroll.PayrollProcessing.Services
Imports Payroll.PayrollProcessing.Views

Namespace PayrollProcessing.Presenters

    Public Class PayrollInputEntryPresenter

        Private ReadOnly _view As IPayrollInputEntryView
        Private ReadOnly _service As IPayrollInputService
        Private ReadOnly _processService As IPayrollProcessService
        Private ReadOnly _userName As String

        ' Mga column mula sa catalogs (Overtime/Holiday/Compensation/Bonus/Deduction) - pareho sa lahat ng cutoff
        Private _catalogColumns As List(Of PayrollInputColumnModel)

        ' Ang aktwal na columns ng grid ngayon = catalogs + (Days Worked kung Daily Rate ang cycle ng cutoff)
        Private _columns As List(Of PayrollInputColumnModel)
        Private _showingDaysWorked As Boolean = False

        Public Sub New(view As IPayrollInputEntryView, service As IPayrollInputService,
                       processService As IPayrollProcessService, userName As String)
            _view = view
            _service = service
            _processService = processService
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Dim cutoffs = Await _service.GetCutoffsAsync()
            _view.DisplayCutoffs(cutoffs)

            _catalogColumns = Await _service.GetColumnsAsync()
            _columns = _catalogColumns
            _showingDaysWorked = False
            _view.DisplayColumns(_columns)
        End Function

        Public Async Function LoadCutoffDataAsync(cutoffId As Integer) As Task
            ' Daily Rate cycle (hal. Daily) = may Days Worked column. Binubuo lang ulit ang grid columns
            ' kapag nagbago ang pangangailangan, para hindi mawala ang pinili ng user sa Column Chooser
            ' kapag nagpapalit-palit lang ng cutoff ng iisang klase ng pay cycle.
            Dim needsDaysWorked = Await _service.RequiresDaysWorkedAsync(cutoffId)

            If needsDaysWorked <> _showingDaysWorked Then
                _columns = If(needsDaysWorked,
                              New List(Of PayrollInputColumnModel)({CoreTxnCode.DaysWorkedColumn()}.Concat(_catalogColumns)),
                              _catalogColumns)
                _showingDaysWorked = needsDaysWorked
                _view.DisplayColumns(_columns)
            End If

            Dim sheet = Await _service.GetInputDataAsync(cutoffId, _columns)
            _view.DisplayData(sheet.Table)
            _view.DisplayEligibilityWarnings(sheet.Warnings)
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


        ''' <summary>
        ''' Process button: (1) i-save ang Input Entry, (2) i-compute lahat sa memory, (3) ipakita ang buod at humingi ng Confirm,
        ''' (4) i-save ang results at gawing Processed ang cutoff. Pwedeng ulitin hanggang hindi pa Posted.
        ''' </summary>
        Public Async Function ProcessAsync() As Task
            Dim cutoffId = _view.GetSelectedCutoffId()
            If Not cutoffId.HasValue Then
                _view.ShowError("Please select a Cutoff first.")
                Return
            End If

            Try
                Dim table = _view.GetEditedData()
                Await _service.SaveInputDataAsync(cutoffId.Value, table, _columns, _userName)

                Dim preview = Await _processService.BuildAsync(cutoffId.Value)

                If Not _view.ConfirmProcess(preview.SummaryText()) Then
                    _view.ShowMessage("Inputs were saved. The computation was NOT saved.")
                    Return
                End If

                Await _processService.SaveAsync(preview, _userName)
                _view.ShowMessage($"Processed {preview.EmployeeCount} employee(s). See the Output tab for the results.")
            Catch ex As PayrollInputValidationException
                _view.ShowError(ex.Message)
            Catch ex As InvalidOperationException
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace