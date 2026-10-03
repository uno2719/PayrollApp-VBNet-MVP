Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    Public Class CutoffPresenter

        Private ReadOnly _view As Views.ICutoffMaintenanceView
        Private ReadOnly _service As Services.ICutoffService
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of CutoffModel)

        Public Sub New(view As Views.ICutoffMaintenanceView, service As Services.ICutoffService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Await LoadListAsync()

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)
        End Function

        Private Async Function LoadListAsync() As Task
            _currentList = Await _service.GetAllAsync()
            _view.BindList(_currentList)
        End Function

        Public Sub StartNew()
            _selectedId = 0
            _isNewMode = True

            _view.ClearFields()
            _view.CutoffYear = Date.Today.Year
            _view.SetFormMode(True, True)
        End Sub

        Public Sub SelectItem(cutoffId As Integer)
            _selectedId = cutoffId
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.CutoffID = cutoffId)

            If selected IsNot Nothing Then
                _view.CycleType = selected.CycleType
                _view.CutoffYear = selected.CutoffYear
                _view.CutoffStart = selected.CutoffStart
                _view.CutoffEnd = selected.CutoffEnd
                _view.PayDate = selected.PayDate
                _view.CutoffLabel = selected.CutoffLabel
                _view.Status = selected.Status
            End If

            _view.SetFormMode(False, False)
        End Sub

        Public Sub StartEdit()
            If _selectedId = 0 Then
                _view.ShowError("Please select a Cutoff first.")
                Return
            End If

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.CutoffID = _selectedId)
            If selected IsNot Nothing AndAlso selected.Status = CutoffStatus.Posted Then
                _view.ShowError("This Cutoff is Posted and can no longer be edited.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)
        End Sub

        Public Async Function SaveAsync() As Task
            Dim item As New CutoffModel With {
                .CutoffID = _selectedId,
                .CycleType = If(_view.CycleType, "").Trim(),
                .CutoffYear = _view.CutoffYear,
                .CutoffStart = _view.CutoffStart,
                .CutoffEnd = _view.CutoffEnd,
                .PayDate = _view.PayDate,
                .CutoffLabel = If(_view.CutoffLabel, "").Trim(),
                .Status = If(_isNewMode, CutoffStatus.Draft, _view.Status)
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage(If(_isNewMode, "Cutoff added.", "Cutoff updated."))

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)

            Await LoadListAsync()
        End Function

        Public Sub CancelEdit()
            If _selectedId > 0 AndAlso Not _isNewMode Then
                SelectItem(_selectedId)
            Else
                _selectedId = 0
                _isNewMode = False

                _view.ClearFields()
                _view.SetFormMode(False, False)
            End If
        End Sub

        ''' <summary>Reuses whatever CycleType + CutoffYear are currently set in the form.</summary>
        Public Async Function GenerateForYearAsync() As Task
            Dim cycleType = If(_view.CycleType, "").Trim()
            Dim year = _view.CutoffYear

            If String.IsNullOrWhiteSpace(cycleType) Then
                _view.ShowError("Pumili muna ng Cycle Type.")
                Return
            End If

            If Not _view.ConfirmGenerate(cycleType, year) Then Return

            Try
                Dim count = Await _service.GenerateForYearAsync(cycleType, year, _userName)
                _view.ShowMessage($"{count} na {cycleType} Cutoff ang nagawa para sa {year}.")
                Await LoadListAsync()
            Catch ex As Services.PayrollCutoffGenerateException
                _view.ShowError(ex.Message)
            End Try
        End Function

    End Class

End Namespace