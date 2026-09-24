Imports Payroll.GlobalShared.Models
Imports Payroll.Lookups.Services

Namespace LeaveSettings.Presenters

    Public Class LeaveRulePresenter

        Private ReadOnly _view As Views.ILeaveRuleMaintenanceView
        Private ReadOnly _service As Services.ILeaveRuleService
        Private ReadOnly _lookupService As ILookupService          ' para sa Leave Group combo
        Private ReadOnly _leaveTypeService As Services.ILeaveTypeService ' para sa Leave Type combo
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of LeaveRuleModel)

        Public Sub New(
            view As Views.ILeaveRuleMaintenanceView,
            service As Services.ILeaveRuleService,
            lookupService As ILookupService,
            leaveTypeService As Services.ILeaveTypeService,
            userName As String)

            _view = view
            _service = service
            _lookupService = lookupService
            _leaveTypeService = leaveTypeService
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task

            ' Combo data sources - binubuo lang isang beses (hindi
            ' babalikan kada Save/Refresh, pero simple lang naman kung
            ' isasama - hindi mabigat).
            Dim groups = Await _lookupService.GetAllAsync("tblLeaveGroup")
            _view.BindLeaveGroups(groups.Where(Function(g) g.IsActive).ToList())

            Dim types = Await _leaveTypeService.GetAllAsync()
            _view.BindLeaveTypes(types.Where(Function(t) t.IsActive).ToList())

            Await LoadListAsync()

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.BindBrackets(New List(Of LeaveRuleBracketModel))
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
            _view.BindBrackets(New List(Of LeaveRuleBracketModel))
            _view.ShowEntitlement = True
            _view.IsActive = True
            _view.SetFormMode(True, True)

        End Sub

        ' Display lang - hindi pa edit mode. Kailangan i-load ang
        ' brackets ng napiling Rule (hiwalay na tawag sa Service,
        ' dahil hindi kasama ang child rows sa list query).
        Public Async Sub SelectItem(id As Integer)

            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.LeaveGroupId = selected.LeaveGroupId
                _view.LeaveTypeId = selected.LeaveTypeId
                _view.EntitlementMethod = selected.EntitlementMethod
                _view.ComputeBasedOn = selected.ComputeBasedOn
                _view.PlotBasedOn = selected.PlotBasedOn
                _view.AnniversaryPlotOn = selected.AnniversaryPlotOn
                _view.UnitOfMeasure = selected.UnitOfMeasure
                _view.HolidayIncluded = selected.HolidayIncluded
                _view.RequireAttachment = selected.RequireAttachment
                _view.Monetize = selected.Monetize
                _view.ShowEntitlement = selected.ShowEntitlement
                _view.IsActive = selected.IsActive

                Dim brackets = Await _service.GetBracketsAsync(id)
                _view.BindBrackets(brackets)
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

            Dim item As New LeaveRuleModel With {
                .Id = _selectedId,
                .LeaveGroupId = _view.LeaveGroupId,
                .LeaveTypeId = _view.LeaveTypeId,
                .EntitlementMethod = _view.EntitlementMethod,
                .ComputeBasedOn = _view.ComputeBasedOn,
                .PlotBasedOn = _view.PlotBasedOn,
                .AnniversaryPlotOn = _view.AnniversaryPlotOn,
                .UnitOfMeasure = _view.UnitOfMeasure,
                .HolidayIncluded = _view.HolidayIncluded,
                .RequireAttachment = _view.RequireAttachment,
                .Monetize = _view.Monetize,
                .ShowEntitlement = _view.ShowEntitlement,
                .IsActive = _view.IsActive
            }

            Dim brackets = _view.GetBrackets()

            Dim result = Await _service.SaveAsync(item, brackets, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If _isNewMode Then
                _view.ShowMessage("Rule added.")
            Else
                _view.ShowMessage("Rule updated.")
            End If

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.BindBrackets(New List(Of LeaveRuleBracketModel))
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
                _view.BindBrackets(New List(Of LeaveRuleBracketModel))
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
                _view.ShowMessage("Rule reactivated.")
            Else
                _view.ShowMessage("Rule deactivated.")
            End If

            Await LoadListAsync()

        End Function

    End Class

End Namespace
