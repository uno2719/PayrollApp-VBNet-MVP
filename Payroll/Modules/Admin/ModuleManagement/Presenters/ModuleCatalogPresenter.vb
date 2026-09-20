' ============================================================
' Modules/Admin/ModuleManagement/Presenters/ModuleCatalogPresenter.vb
' ============================================================
Imports Payroll.ModuleManagement.Models
Imports Payroll.ModuleManagement.Services
Imports Payroll.ModuleManagement.Views

Namespace ModuleManagement.Presenters

    Public Class ModuleCatalogPresenter

        Private ReadOnly _view As IModuleCatalogView
        Private ReadOnly _service As IModuleCatalogService
        Private ReadOnly _userName As String

        Private _list As List(Of ModuleCatalogModel)
        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _isEditing As Boolean = False

        ' Fixed na starting choices para sa GroupName dropdown - pero
        ' editable pa rin ang combo (hindi strict lookup), kaya kung
        ' may bagong grupo si Uno na gusto niyang gawin, kaya niya.
        Private Shared ReadOnly DefaultGroups As String() = {
            "Main Menu", "Administration", "Settings", "Reports"
        }

        Public Sub New(view As IModuleCatalogView, service As IModuleCatalogService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public ReadOnly Property IsEditing As Boolean
            Get
                Return _isEditing
            End Get
        End Property

        Public Async Function LoadAsync() As Task

            _view.BindGroupChoices(DefaultGroups.ToList())

            Await RefreshListAsync()
            ResetForm()

        End Function

        Public Async Function RefreshListAsync() As Task

            _list = Await _service.GetAllAsync()
            _view.BindModuleList(_list)

        End Function

        ' ========================================================
        ' SELECT / NEW / EDIT / CANCEL
        ' ========================================================
        Public Sub SelectModule(recordId As Integer)

            If _isEditing Then Return

            Dim item = _list?.FirstOrDefault(Function(x) x.RecordId = recordId)
            If item Is Nothing Then Return

            _selectedId = recordId
            _isNewMode = False

            PushToView(item)
            _view.SetFormMode(False, False)

        End Sub

        Public Sub StartNew()

            _selectedId = 0
            _isNewMode = True
            _isEditing = True

            _view.ClearFields()
            _view.IsActive = True
            _view.SortOrder = NextSuggestedOrder()

            _view.SetFormMode(True, True)

        End Sub

        Public Sub StartEdit()

            If _selectedId = 0 Then
                _view.ShowError("Please select a module first.")
                Return
            End If

            _isNewMode = False
            _isEditing = True

            _view.SetFormMode(True, False)

        End Sub

        Public Sub CancelEdit()

            _isEditing = False

            If _selectedId > 0 AndAlso Not _isNewMode Then
                SelectModule(_selectedId)
            Else
                ResetForm()
            End If

        End Sub

        Private Sub ResetForm()
            _selectedId = 0
            _isNewMode = False
            _isEditing = False

            _view.ClearFields()
            _view.SetFormMode(False, False)
        End Sub

        ' Simpleng heuristic: susunod na "sampuhan" (10, 20, 30...) base
        ' sa pinakamataas na SortOrder ngayon, para may agwat na
        ' maaaring hipanan mo mamaya kung gusto mong maglagay sa gitna.
        Private Function NextSuggestedOrder() As Integer
            If _list Is Nothing OrElse _list.Count = 0 Then Return 10
            Return (_list.Max(Function(x) x.SortOrder) \ 10 + 1) * 10
        End Function

        ' ========================================================
        ' SAVE
        ' ========================================================
        Public Async Function SaveAsync() As Task

            Dim item = PullFromView()
            item.RecordId = _selectedId

            Dim result = Await _service.SaveAsync(item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If Not String.IsNullOrEmpty(result.WarningMessage) Then
                ' Babala lang - naipon na ang save, ipinapaalam lang natin.
                _view.ShowMessage(result.WarningMessage)
            End If

            _view.ShowMessage(If(_isNewMode, "Module added.", "Module updated."))

            _isEditing = False
            _isNewMode = False

            Await RefreshListAsync()
            SelectModule(item.RecordId)

        End Function

        ' ========================================================
        ' DELETE
        ' ========================================================
        Public Async Function DeleteAsync() As Task

            If _selectedId = 0 Then
                _view.ShowError("Please select a module first.")
                Return
            End If

            Dim confirmed = _view.Confirm(
                "Are you sure you want to delete this module entry?",
                "Confirm Delete")

            If Not confirmed Then Return

            Dim result = Await _service.DeleteAsync(_selectedId, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage("Module deleted.")

            Await RefreshListAsync()
            ResetForm()

        End Function

        ' ========================================================
        ' TOGGLE ACTIVE (mula sa checkbox sa form mismo, sa Save na)
        ' ========================================================
        ' PAALALA: Ang IsActive ay direktang field sa form (hindi
        ' hiwalay na contextual button gaya ng ginawa natin sa Loans) -
        ' dahil dito, walang side-effect ang pag-toggle nito (walang
        ' schedule na kailangang i-regenerate), kaya sapat nang isang
        ' plain checkbox sa loob ng Save flow.
        ' ========================================================

        Private Function PullFromView() As ModuleCatalogModel

            Return New ModuleCatalogModel With {
                .ModuleCode = _view.ModuleCode,
                .ModuleName = _view.ModuleName,
                .GroupName = _view.GroupName,
                .SortOrder = _view.SortOrder,
                .IsActive = _view.IsActive
            }

        End Function

        Private Sub PushToView(item As ModuleCatalogModel)

            _view.ModuleCode = item.ModuleCode
            _view.ModuleName = item.ModuleName
            _view.GroupName = item.GroupName
            _view.SortOrder = item.SortOrder
            _view.IsActive = item.IsActive

        End Sub

    End Class

End Namespace