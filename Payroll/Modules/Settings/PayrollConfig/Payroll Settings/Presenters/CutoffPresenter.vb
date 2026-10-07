Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    Public Class CutoffPresenter

        Private ReadOnly _view As Views.ICutoffMaintenanceView
        Private ReadOnly _service As Services.ICutoffService
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _allCutoffs As List(Of CutoffModel) = New List(Of CutoffModel)()
        Private _filtersInitialized As Boolean = False

        Public Sub New(view As Views.ICutoffMaintenanceView, service As Services.ICutoffService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            _allCutoffs = Await _service.GetAllAsync()
            Dim activeCycles = Await _service.GetActiveCycleTypesAsync()

            ' Tandaan ang kasalukuyang filter para hindi mawala pag nag-Refresh
            Dim previousCycle = _view.FilterCycleType
            Dim previousYear = _view.FilterYear

            ' Active pay cycles muna, tapos anumang cycle na may existing data pa (kahit Inactive na)
            Dim cycleTypes = activeCycles.Concat(_allCutoffs.Select(Function(c) c.CycleType)).Distinct().ToList()
            Dim years = _allCutoffs.Select(Function(c) c.CutoffYear).
                Concat({Date.Today.Year}).Distinct().OrderByDescending(Function(y) y).ToList()

            _view.BindFilters(cycleTypes, years)

            If _filtersInitialized Then
                _view.FilterCycleType = previousCycle
                _view.FilterYear = previousYear
            Else
                ' Unang bukas: lahat ng pay cycle, pero ng kasalukuyang taon lang
                _view.FilterCycleType = Nothing
                _view.FilterYear = Date.Today.Year
                _filtersInitialized = True
            End If

            ApplyFilter()
        End Function

        ''' <summary>I-display lang ang cutoff ng napiling Pay Cycle at Year.</summary>
        Public Sub ApplyFilter()
            Dim cycle = _view.FilterCycleType
            Dim year = _view.FilterYear

            Dim filtered = _allCutoffs.
                Where(Function(c) String.IsNullOrEmpty(cycle) OrElse c.CycleType = cycle).
                Where(Function(c) Not year.HasValue OrElse c.CutoffYear = year.Value).
                OrderBy(Function(c) c.CycleType).ThenBy(Function(c) c.CutoffStart).ToList()

            _selectedId = 0
            _view.ClearFields()
            _view.SetFormMode(False)
            _view.BindList(filtered)

            If filtered.Count > 0 Then
                SelectItem(filtered(0).CutoffID)
            End If
        End Sub

        Public Sub SelectItem(cutoffId As Integer)
            _selectedId = cutoffId

            Dim selected = _allCutoffs.FirstOrDefault(Function(x) x.CutoffID = cutoffId)

            If selected IsNot Nothing Then
                _view.CycleType = selected.CycleType
                _view.CutoffYear = selected.CutoffYear
                _view.CutoffStart = selected.CutoffStart
                _view.CutoffEnd = selected.CutoffEnd
                _view.PayDate = selected.PayDate
                _view.CutoffLabel = selected.CutoffLabel
                _view.Status = selected.Status
            End If

            _view.SetFormMode(False)
        End Sub

        Public Sub StartEdit()
            If _selectedId = 0 Then
                _view.ShowError("Please select a Cutoff first.")
                Return
            End If

            Dim selected = _allCutoffs.FirstOrDefault(Function(x) x.CutoffID = _selectedId)
            If selected IsNot Nothing AndAlso selected.Status = CutoffStatus.Posted Then
                _view.ShowError("This Cutoff is Posted and can no longer be edited.")
                Return
            End If

            _view.SetFormMode(True)
        End Sub

        Public Async Function SaveAsync() As Task
            If _selectedId = 0 Then
                _view.ShowError("Please select a Cutoff first.")
                Return
            End If

            Dim item As New CutoffModel With {
                .CutoffID = _selectedId,
                .CycleType = If(_view.CycleType, "").Trim(),
                .CutoffYear = _view.CutoffYear,
                .CutoffStart = _view.CutoffStart,
                .CutoffEnd = _view.CutoffEnd,
                .PayDate = _view.PayDate,
                .CutoffLabel = If(_view.CutoffLabel, "").Trim(),
                .Status = _view.Status
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage("Cutoff updated.")
            Await LoadAsync()
        End Function

        Public Sub CancelEdit()
            If _selectedId > 0 Then
                SelectItem(_selectedId)
            Else
                _view.ClearFields()
                _view.SetFormMode(False)
            End If
        End Sub

    End Class

End Namespace
