' File: Modules/MainMenu/Employee/Services/EmployeeFixTxnService.vb
' ============================================================
' Employee > Fix Transactions: paulit-ulit na Allowance / Compensation / Bonus / Deduction ng bawat empleyado.
' Ang Process ng payroll ang kusang kumukuha nito (FixTransactionRules.MergeTxns); walang "Push".
' Permission: "Employee Salary" (main_Employees_Salary) - View para makita, Can Edit para magbago.
' ============================================================
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace Employee.Services

    Public Class EmployeeFixTxnService
        Implements IEmployeeFixTxnService

        Private ReadOnly _repo As Data.IEmployeeFixTxnRepository

        Public Sub New()
            _repo = New Data.EmployeeFixTxnRepository()
        End Sub

        Public Sub New(repo As Data.IEmployeeFixTxnRepository)
            _repo = repo
        End Sub

        ' ---------- PERMISSIONS (huling harang kahit lusutan ang UI) ----------
        Public Shared Function CanView() As Boolean
            Return GlobalShared.Security.PermissionService.CanView(ModuleCodes.Main_Employees_Salary)
        End Function

        Public Shared Function CanEdit() As Boolean
            Return GlobalShared.Security.PermissionService.CanEdit(ModuleCodes.Main_Employees_Salary)
        End Function

        Private Shared Sub RequireView()
            If Not CanView() Then Throw New UnauthorizedAccessException("You don't have access to Fix Transactions (Employee Salary).")
        End Sub

        Private Shared Sub RequireEdit()
            If Not CanEdit() Then Throw New UnauthorizedAccessException("You are not allowed to change Fix Transactions (Employee Salary).")
        End Sub

        ' ---------- READ ----------
        Public Async Function GetByEmployeeAsync(recordId As Integer) As Task(Of List(Of EmployeeFixTxnModel)) _
            Implements IEmployeeFixTxnService.GetByEmployeeAsync
            RequireView()
            If recordId <= 0 Then Return New List(Of EmployeeFixTxnModel)()
            Return Await _repo.GetByEmployeeAsync(recordId)
        End Function

        Public Async Function GetCodeOptionsAsync() As Task(Of List(Of FixTxnCodeOption)) _
            Implements IEmployeeFixTxnService.GetCodeOptionsAsync
            RequireView()
            Return Await _repo.GetCodeOptionsAsync()
        End Function

        ' ---------- SAVE ----------
        Public Async Function SaveAsync(item As EmployeeFixTxnModel) As Task(Of Integer) _
            Implements IEmployeeFixTxnService.SaveAsync

            RequireEdit()
            If item Is Nothing Then Throw New InvalidOperationException("Nothing to save.")

            item.TxnCategory = If(item.TxnCategory, "").Trim().ToUpperInvariant()
            item.TxnCode = If(item.TxnCode, "").Trim()

            Dim existing As EmployeeFixTxnModel = Nothing
            If item.FixTxnId > 0 Then
                existing = Await _repo.GetByIdAsync(item.FixTxnId)
                If existing Is Nothing Then Throw New InvalidOperationException("This Fix Transaction no longer exists. Refresh the list.")
                If existing.RecordId <> item.RecordId Then Throw New InvalidOperationException("This Fix Transaction belongs to another employee.")
                item.IsActive = existing.IsActive   ' ang Active ay sa Set Active / Set Inactive lang binabago

                ' Nagamit na sa payroll: hindi na pwedeng palitan ang code (para tama pa rin ang lumang results). Bago na lang.
                Dim codeChanged = Not FixTransactionRules.SameCode(existing.TxnCategory, existing.TxnCode, item.TxnCategory, item.TxnCode)
                If existing.IsUsed AndAlso codeChanged Then
                    Throw New InvalidOperationException(
                        "This entry was already used in payroll, so its Transaction Type / Code can't be changed. " &
                        "Set an End on it (or set it Inactive) and add a new entry for the other code.")
                End If
            End If

            ' Ang code ay dapat nasa listahan (hindi naka-map sa Code Mapping). Ang bago/ibang code ay dapat active.
            Dim options = Await _repo.GetCodeOptionsAsync()
            ' (Ang lumang code ng ine-edit ay pinapayagan pa rin, hal. para lang lagyan ng End.)
            Dim opt = options.FirstOrDefault(Function(o) FixTransactionRules.SameCode(o.TxnCategory, o.Code, item.TxnCategory, item.TxnCode))
            Dim keptSameCode = existing IsNot Nothing AndAlso
                               FixTransactionRules.SameCode(existing.TxnCategory, existing.TxnCode, item.TxnCategory, item.TxnCode)
            If Not keptSameCode Then
                If opt Is Nothing Then
                    Throw New InvalidOperationException($"Code '{item.TxnCode}' can't be used here (it no longer exists or it is mapped in General Settings > Code Mapping).")
                End If
                If Not opt.IsActive Then Throw New InvalidOperationException($"Code '{item.TxnCode}' is inactive in Payroll Settings.")
            End If
            If opt IsNot Nothing Then item.TxnCode = opt.Code   ' eksaktong spelling ng catalog

            Dim others = Await _repo.GetByEmployeeAsync(item.RecordId)
            Dim problem = FixTransactionRules.Validate(item, others)
            If problem.Length > 0 Then Throw New InvalidOperationException(problem)

            Dim user = AppSession.CurrentUser
            If item.FixTxnId > 0 Then
                Await _repo.UpdateAsync(item, user)
                Return item.FixTxnId
            End If
            item.IsActive = True
            Return Await _repo.InsertAsync(item, user)
        End Function

        ' ---------- ACTIVE / INACTIVE ----------
        Public Async Function SetActiveAsync(fixTxnId As Integer, isActive As Boolean) As Task _
            Implements IEmployeeFixTxnService.SetActiveAsync

            RequireEdit()
            Dim item = Await _repo.GetByIdAsync(fixTxnId)
            If item Is Nothing Then Throw New InvalidOperationException("This Fix Transaction no longer exists. Refresh the list.")
            If item.IsActive = isActive Then Return

            If isActive Then
                ' Sinusuri ulit: baka may ibang active na entry ng parehong code sa parehong panahon.
                item.IsActive = True
                Dim others = Await _repo.GetByEmployeeAsync(item.RecordId)
                Dim problem = FixTransactionRules.Validate(item, others)
                If problem.Length > 0 Then Throw New InvalidOperationException(problem)
            End If

            Await _repo.SetActiveAsync(fixTxnId, isActive, AppSession.CurrentUser)
        End Function

        ' ---------- DELETE ----------
        Public Async Function DeleteAsync(fixTxnId As Integer) As Task _
            Implements IEmployeeFixTxnService.DeleteAsync

            RequireEdit()
            Dim deleted = Await _repo.DeleteIfUnusedAsync(fixTxnId)
            If Not deleted Then
                Throw New InvalidOperationException(
                    "This entry was already used in payroll (or no longer exists), so it can't be deleted. Set it Inactive instead.")
            End If
        End Function

    End Class

End Namespace
