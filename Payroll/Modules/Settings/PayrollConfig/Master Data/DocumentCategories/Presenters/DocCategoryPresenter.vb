Imports Payroll.DocumentCategories.Models

Namespace DocumentCategories.Presenters

    Public Class DocCategoryPresenter

        Private ReadOnly _view As Views.IDocCategoryMaintenanceView
        Private ReadOnly _service As Services.IDocCategoryService
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of DocCategoryModel)

        Public Sub New(
            view As Views.IDocCategoryMaintenanceView,
            service As Services.IDocCategoryService,
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

        ' Display lang - hindi pa edit mode.
        Public Sub SelectItem(id As Integer)

            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.CategoryName = selected.Name
                _view.IsRequired = selected.IsRequired
                _view.SortOrder = selected.SortOrder
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(False, False)

        End Sub

        Public Sub StartEdit()

            If _selectedId = 0 Then
                _view.ShowError("Please select a category first.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)

        End Sub

        Public Async Function SaveAsync() As Task

            Dim item As New DocCategoryModel With {
                .Id = _selectedId,
                .Name = If(_view.CategoryName, String.Empty).Trim(),
                .IsRequired = _view.IsRequired,
                .SortOrder = _view.SortOrder,
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If _isNewMode Then
                _view.ShowMessage("Category added.")
            Else
                _view.ShowMessage("Category updated.")
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

        ''' <summary>
        ''' True kung ang pag-save ngayon ay babago sa "Profile %" ng mga empleyado: bagong Required category, o
        ''' pagpalit ng Required sa existing. Ginagamit ng View para magtanong bago mag-save.
        ''' </summary>
        Public Function SaveChangesProfileCompleteness() As Boolean
            If _isNewMode Then Return _view.IsRequired

            Dim selected = _currentList?.FirstOrDefault(Function(x) x.Id = _selectedId)
            If selected Is Nothing Then Return False
            Return selected.IsRequired <> _view.IsRequired
        End Function

        Public Async Function ToggleActiveSelectedAsync() As Task

            If _selectedId = 0 Then
                _view.ShowError("Please select a category first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive

            Await _service.SetActiveStatusAsync(_selectedId, newStatus, _userName)

            _view.IsActive = newStatus

            If newStatus Then
                _view.ShowMessage("Category reactivated.")
            Else
                _view.ShowMessage("Category deactivated.")
            End If

            Await LoadListAsync()

        End Function

        Public Function HasSelection() As Boolean
            Return _selectedId > 0
        End Function

        ' True kung ine-edit ang EXISTING na record (hindi bagong entry).
        Public Function IsEditingExisting() As Boolean
            Return (Not _isNewMode) AndAlso _selectedId > 0
        End Function

        Public Async Function DeleteSelectedAsync() As Task

            If _selectedId = 0 Then
                _view.ShowError("Please select a category first.")
                Return
            End If

            Dim result = Await _service.DeleteAsync(_selectedId)

            If Not result.Success Then
                _view.ShowError(result.Message)
                Return
            End If

            _view.ShowMessage("Category deleted.")

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)

            Await LoadListAsync()

        End Function

    End Class

End Namespace
