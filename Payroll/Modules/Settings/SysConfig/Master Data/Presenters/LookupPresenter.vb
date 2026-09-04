Imports Payroll.GlobalShared.Models

Namespace Lookups.Presenters

    Public Class LookupPresenter

        Private ReadOnly _view As Views.ILookupMaintenanceView
        Private ReadOnly _service As Services.ILookupService
        Private ReadOnly _tableName As String
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of LookupModel)

        Public Sub New(
            view As Views.ILookupMaintenanceView,
            service As Services.ILookupService,
            tableName As String,
            userName As String)

            _view = view
            _service = service
            _tableName = tableName
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
            _currentList = Await _service.GetAllAsync(_tableName)
            _view.BindList(_currentList)
        End Function

        Public Sub StartNew()

            _selectedId = 0
            _isNewMode = True

            _view.ClearFields()
            _view.IsActive = True
            _view.SetFormMode(True, True)

        End Sub

        ' Called by the View when a row is selected.
        ' IMPORTANT:
        ' Selecting a row only displays the record.
        ' It does NOT enter edit mode.
        Public Sub SelectItem(id As Integer)

            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(
                Function(x) x.Id = id)

            If selected IsNot Nothing Then

                _view.Code = selected.Code
                _view.Name = selected.Name
                _view.IsActive = selected.IsActive

            End If

            ' Read-only mode after selecting a row.
            _view.SetFormMode(False, False)

        End Sub

        ' Called when the user clicks Edit.
        Public Sub StartEdit()

            If _selectedId = 0 Then
                _view.ShowError("Please select an entry first.")
                Return
            End If

            _isNewMode = False

            _view.SetFormMode(True, False)

        End Sub

        Public Async Function SaveAsync() As Task

            Dim item As New LookupModel With {
                .Id = _selectedId,
                .Code = If(_view.Code, "").Trim(),
                .Name = If(_view.Name, "").Trim(),
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(
                _tableName,
                item,
                _userName)

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

                ' Restore the selected record and return to read-only mode.
                SelectItem(_selectedId)

            Else

                ' Cancel New.
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

            Await _service.SetActiveStatusAsync(
                _tableName,
                _selectedId,
                newStatus,
                _userName)

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
