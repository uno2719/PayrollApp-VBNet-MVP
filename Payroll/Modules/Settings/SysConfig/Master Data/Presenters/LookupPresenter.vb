Imports System.Linq
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

        ' tableName: alin sa 8 na-maintain na tables ang pag-aari ng
        ' instance na ito (tblBranch, tblDepartment, atbp — mula sa
        ' LookupTableRegistry.MaintainedTables). Bawat tab sa
        ' ucSettingsLookups ay may sariling Presenter na naka-configure
        ' sa ibang tableName, kahit iisang class lang ito.
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

        ' Tinatawag ng View kapag pumili ng row sa grid (FocusedRowChanged).
        Public Sub SelectItem(id As Integer)
            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = id)
            If selected IsNot Nothing Then
                _view.Code = selected.Code
                _view.Name = selected.Name
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(True, False)
        End Sub

        Public Async Function SaveAsync() As Task
            Dim item As New LookupModel With {
                .Id = _selectedId,
                .Code = If(_view.Code, "").Trim(),
                .Name = If(_view.Name, "").Trim(),
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(_tableName, item, _userName)
            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage(If(_isNewMode, "Entry added.", "Entry updated."))
            _view.ClearFields()
            _view.SetFormMode(False, False)
            _isNewMode = False
            _selectedId = 0
            Await LoadListAsync()
        End Function

        Public Sub CancelEdit()
            _view.ClearFields()
            _view.SetFormMode(False, False)
            _isNewMode = False
            _selectedId = 0
        End Sub

        ' Soft-delete/reactivate toggle lang - IsActive 0/1, HINDI hard
        ' DELETE. FK-referenced kasi ang mga tables na ito ng existing
        ' Employee records (tblEmployeeEmployment.BranchId, atbp).
        Public Async Function ToggleActiveSelectedAsync() As Task
            If _selectedId = 0 Then
                _view.ShowError("Please select an entry first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive
            Await _service.SetActiveStatusAsync(_tableName, _selectedId, newStatus, _userName)
            _view.IsActive = newStatus
            _view.ShowMessage(If(newStatus, "Entry reactivated.", "Entry deactivated."))
            Await LoadListAsync()
        End Function

    End Class
End Namespace