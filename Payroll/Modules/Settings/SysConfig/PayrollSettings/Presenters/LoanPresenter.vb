Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    ' Iisang table lang si Loan.
    Public Class LoanPresenter

        Private ReadOnly _view As Views.ILoanMaintenanceView
        Private ReadOnly _service As Services.ILoanService
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of LoanModel)

        Public Sub New(
            view As Views.ILoanMaintenanceView,
            service As Services.ILoanService,
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

        Public Sub SelectItem(id As Integer)
            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.Code = selected.Code
                _view.Description = selected.Description
                _view.LoanType = selected.LoanType
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
            Dim item As New LoanModel With {
                .Id = _selectedId,
                .Code = If(_view.Code, "").Trim(),
                .Description = If(_view.Description, "").Trim(),
                .LoanType = _view.LoanType,
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
