Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Presenters

    Public Class StatutorySettingsPresenter

        Private ReadOnly _view As Views.IStatutorySettingsMaintenanceView
        Private ReadOnly _service As Services.IStatutorySettingsService
        Private ReadOnly _tableName As String
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of StatutoryBracketModel)

        Public Sub New(
            view As Views.IStatutorySettingsMaintenanceView,
            service As Services.IStatutorySettingsService,
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

        ' Tinatawag ng View kapag pinili ang isang row - display lang,
        ' hindi pa edit mode, kagaya ng LookupPresenter.
        Public Sub SelectItem(id As Integer)

            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(
                Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.SalaryFrom = selected.SalaryFrom
                _view.SalaryTo = selected.SalaryTo
                _view.EEShare = selected.EEShare
                _view.EEContriType = selected.EEContriType
                _view.ERShare = selected.ERShare
                _view.ERContriType = selected.ERContriType
                _view.ECCAmount = selected.ECCAmount
                _view.EEMPF = selected.EEMPF
                _view.ERMPF = selected.ERMPF
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(False, False)

        End Sub

        Public Sub StartEdit()

            If _selectedId = 0 Then
                _view.ShowError("Please select a bracket first.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)

        End Sub

        Public Async Function SaveAsync() As Task

            Dim item As New StatutoryBracketModel With {
                .Id = _selectedId,
                .SalaryFrom = _view.SalaryFrom,
                .SalaryTo = _view.SalaryTo,
                .EEShare = _view.EEShare,
                .EEContriType = If(_view.EEContriType, "").Trim(),
                .ERShare = _view.ERShare,
                .ERContriType = If(_view.ERContriType, "").Trim(),
                .ECCAmount = _view.ECCAmount,
                .EEMPF = _view.EEMPF,
                .ERMPF = _view.ERMPF,
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(_tableName, item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If _isNewMode Then
                _view.ShowMessage("Bracket added.")
            Else
                _view.ShowMessage("Bracket updated.")
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
                _view.ShowError("Please select a bracket first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive

            Await _service.SetActiveStatusAsync(_tableName, _selectedId, newStatus, _userName)

            _view.IsActive = newStatus

            If newStatus Then
                _view.ShowMessage("Bracket reactivated.")
            Else
                _view.ShowMessage("Bracket deactivated.")
            End If

            Await LoadListAsync()

        End Function

    End Class

End Namespace