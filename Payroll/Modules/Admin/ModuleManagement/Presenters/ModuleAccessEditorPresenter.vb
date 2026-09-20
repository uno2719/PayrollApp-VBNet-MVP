' ============================================================
' Modules/Admin/ModuleManagement/Presenters/ModuleAccessEditorPresenter.vb
' ============================================================
' PANSININ: Walang bagong Repository o Service na ginawa para dito.
' Direktang muling ginagamit ang IUserManagementService na dati nang
' pinagana ng Users Account module - GetAllUsersAsync, GetAllModulesAsync,
' GetModuleAccessAsync, at ang bagong idinagdag na SaveModuleAccessAsync
' (tingnan ang PATCH file). Iisa lang ang totoong pinagmumulan ng
' access data sa buong app, dito man o sa Users screen.
' ============================================================
Imports Payroll.Login.Models
Imports Payroll.ModuleManagement.Models
Imports Payroll.ModuleManagement.Views
Imports Payroll.Users.Services

Namespace ModuleManagement.Presenters

    Public Class ModuleAccessEditorPresenter

        Private ReadOnly _view As IModuleAccessEditorView
        Private ReadOnly _service As IUserManagementService
        Private ReadOnly _userName As String

        Private _userList As List(Of UserModel)
        Private _moduleLookup As List(Of Users.Models.ModuleInfo)

        Private _selectedUserId As Integer = 0
        Private _hasUnsavedChanges As Boolean = False

        Public Sub New(view As IModuleAccessEditorView, service As IUserManagementService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task

            _moduleLookup = Await _service.GetAllModulesAsync()

            Await RefreshUserListAsync()

        End Function

        Public Async Function RefreshUserListAsync() As Task

            ' "Active" lang - walang saysay na bigyan ng module access
            ' ang isang deactivated na user account.
            _userList = Await _service.GetAllUsersAsync("Active")

            ' Admins ay palaging full-access (bypass sa PermissionService),
            ' kaya walang saysay silang ipakita rito - itatago natin sila
            ' para hindi malito si Uno kung bakit "walang epekto" ang
            ' pag-uncheck niya sa kanilang access.
            _userList = _userList.Where(Function(u) Not u.IsAdmin).ToList()

            _view.BindUserList(_userList)

        End Function

        ' ========================================================
        ' USER SELECTED
        ' ========================================================
        Public Async Function SelectUserAsync(userId As Integer) As Task

            If _hasUnsavedChanges Then
                Dim proceed = _view.Confirm(
                    "You have unsaved changes for the current user." & Environment.NewLine &
                    "Switch users anyway? Unsaved changes will be lost.",
                    "Unsaved Changes")

                If Not proceed Then Return
            End If

            _selectedUserId = userId

            Dim user = _userList.FirstOrDefault(Function(u) u.RecordId = userId)
            If user Is Nothing Then Return

            _view.SetSelectedUserHeader($"{user.FirstName} {user.LastName} ({user.Username})", user.IsAdmin)

            Await LoadAccessGridAsync(userId)

            _hasUnsavedChanges = False
            _view.SetHasUnsavedChanges(False)
            _view.SetGridEnabled(True)

        End Function

        Private Async Function LoadAccessGridAsync(userId As Integer) As Task

            Dim accessItems = Await _service.GetModuleAccessAsync(userId)

            Dim rows = accessItems.Select(Function(item) ModuleAccessRow.FromAccessItem(item, GroupOf(item.ModuleId))).ToList()

            _view.BindAccessGrid(rows)

        End Function

        Private Function GroupOf(moduleId As Integer) As String
            Dim m = _moduleLookup?.FirstOrDefault(Function(x) x.RecordId = moduleId)
            Return If(m?.GroupName, "Other")
        End Function

        ' ========================================================
        ' DIRTY TRACKING
        ' ========================================================
        ' Tinatawag ng View tuwing may binago sa checkbox - hindi na
        ' natin kailangang tumingin nang detalyado kung ano talaga
        ' ang binago, sapat na ang "may nagbago, dapat i-warn kapag
        ' aalis nang hindi nag-save".
        Public Sub MarkDirty()
            If _selectedUserId = 0 Then Return
            _hasUnsavedChanges = True
            _view.SetHasUnsavedChanges(True)
        End Sub

        ' ========================================================
        ' BULK ACTIONS
        ' ========================================================
        Public Sub SelectAll(grantEdit As Boolean)

            If _selectedUserId = 0 Then Return

            Dim rows = _view.GetAccessGridRows()
            For Each r In rows
                r.CanView = True
                r.CanEdit = grantEdit
            Next

            _view.BindAccessGrid(rows)
            MarkDirty()

        End Sub

        Public Sub ClearAll()

            If _selectedUserId = 0 Then Return

            Dim rows = _view.GetAccessGridRows()
            For Each r In rows
                r.CanView = False
                r.CanEdit = False
            Next

            _view.BindAccessGrid(rows)
            MarkDirty()

        End Sub

        ' ========================================================
        ' COPY FROM ANOTHER USER
        ' ========================================================
        Public Async Function CopyFromAnotherUserAsync() As Task

            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            Dim candidates = _userList.Where(Function(u) u.RecordId <> _selectedUserId).ToList()

            If candidates.Count = 0 Then
                _view.ShowError("There is no other user to copy access from.")
                Return
            End If

            Dim sourceUserId = _view.PromptCopyFromUser(candidates)
            If Not sourceUserId.HasValue Then Return

            Dim sourceAccess = Await _service.GetModuleAccessAsync(sourceUserId.Value)
            Dim rows = sourceAccess.Select(Function(item) ModuleAccessRow.FromAccessItem(item, GroupOf(item.ModuleId))).ToList()

            _view.BindAccessGrid(rows)
            MarkDirty()

            Dim sourceUser = _userList.FirstOrDefault(Function(u) u.RecordId = sourceUserId.Value)
            _view.ShowMessage($"Copied access from {sourceUser?.FirstName} {sourceUser?.LastName}. " &
                              "Review the checkboxes below, then click Save to apply.")

        End Function

        ' ========================================================
        ' SAVE
        ' ========================================================
        Public Async Function SaveAsync() As Task

            If _selectedUserId = 0 Then
                _view.ShowError("Please select a user first.")
                Return
            End If

            Dim rows = _view.GetAccessGridRows()
            Dim accessItems = rows.Select(Function(r) r.ToAccessItem()).ToList()

            Await _service.SaveModuleAccessAsync(_selectedUserId, accessItems)

            _hasUnsavedChanges = False
            _view.SetHasUnsavedChanges(False)

            _view.ShowMessage("Access updated. This takes effect the next time this user logs in.")

        End Function

    End Class

End Namespace