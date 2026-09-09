Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    ' Iisang table lang si Compensation, kaya walang tableName parameter
    ' dito (kumpara sa PayrollFlaggedEntryPresenter/PayrollRateEntryPresenter
    ' na 2 tables ang pinagsisilbihan).
    Public Class CompensationPresenter

        Private ReadOnly _view As Views.ICompensationMaintenanceView
        Private ReadOnly _service As Services.ICompensationService
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of CompensationModel)

        Public Sub New(
            view As Views.ICompensationMaintenanceView,
            service As Services.ICompensationService,
            userName As String)

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
            _view.IsActive = True
            _view.SetFormMode(True, True)
        End Sub

        ' Called by the View when a row is selected. Display lang -
        ' hindi pa edit mode.
        Public Sub SelectItem(id As Integer)
            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.Code = selected.Code
                _view.Description = selected.Description
                _view.TaxFlag = selected.TaxFlag
                _view.SSSFlag = selected.SSSFlag
                _view.PhilHealthFlag = selected.PhilHealthFlag
                _view.PagIbigFlag = selected.PagIbigFlag
                _view.Component2316 = selected.Component2316
                _view.DeminimisFlag = selected.DeminimisFlag
                _view.CeilingAmount = selected.CeilingAmount
                _view.Frequency = selected.Frequency
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(False, False)
        End Sub

        Public Sub StartEdit()
            If _selectedId = 0 Then
                _view.ShowError("Please select an entry first.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)
        End Sub

        Public Async Function SaveAsync() As Task
            Dim item As New CompensationModel With {
                .Id = _selectedId,
                .Code = If(_view.Code, "").Trim(),
                .Description = If(_view.Description, "").Trim(),
                .TaxFlag = _view.TaxFlag,
                .SSSFlag = _view.SSSFlag,
                .PhilHealthFlag = _view.PhilHealthFlag,
                .PagIbigFlag = _view.PagIbigFlag,
                .Component2316 = _view.Component2316,
                .DeminimisFlag = _view.DeminimisFlag,
                .CeilingAmount = _view.CeilingAmount,
                .Frequency = _view.Frequency,
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If _isNewMode Then
                _view.ShowMessage("Entry added.")
            Else
                _view.ShowMessage("Entry updated.")
            End If

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

        ' Soft-delete/reactivate only.
        Public Async Function ToggleActiveSelectedAsync() As Task
            If _selectedId = 0 Then
                _view.ShowError("Please select an entry first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive

            Await _service.SetActiveStatusAsync(_selectedId, newStatus, _userName)

            _view.IsActive = newStatus

            If newStatus Then
                _view.ShowMessage("Entry reactivated.")
            Else
                _view.ShowMessage("Entry deactivated.")
            End If

            Await LoadListAsync()
        End Function

    End Class

End Namespace
