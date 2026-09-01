Namespace Login.Data
    Public Interface IUserRepository
        Function GetByUsernameAsync(username As String) As Task(Of Models.UserModel)
        Function InsertAsync(model As Models.UserModel) As Task(Of Integer)
        Function UpdateLastLoginAsync(recordId As Integer) As Task(Of Boolean)
        Function IncrementFailedLoginAsync(recordId As Integer) As Task(Of Boolean)
        Function LockAccountAsync(recordId As Integer, lockUntil As DateTime) As Task(Of Boolean)
        Function ResetPasswordAsync(recordId As Integer, newHash As String, newSalt As String) As Task(Of Boolean)

        ' ============================================================
        ' Mga bagong method para sa Users Management module - iisa lang
        ' na Repository, dahil parehong gumagalaw sa tblUsers, pero
        ' magkaibang Service/Presenter ang gumagamit (Login vs Users).
        ' ============================================================
        Function GetAllUsersAsync(filter As String) As Task(Of List(Of Models.UserModel))
        Function IsUsernameTakenAsync(username As String) As Task(Of Boolean)
        Function UpdateIsAdminAsync(recordId As Integer, isAdmin As Boolean) As Task(Of Boolean)
        Function DeactivateAsync(recordId As Integer) As Task(Of Boolean)
        Function ActivateAsync(recordId As Integer) As Task(Of Boolean)

        Function GetAllModulesAsync() As Task(Of List(Of Users.Models.ModuleInfo))
        Function GetModuleAccessAsync(userId As Integer) As Task(Of List(Of Users.Models.ModuleAccessItem))
        Function SaveModuleAccessAsync(userId As Integer, accessList As List(Of Users.Models.ModuleAccessItem)) As Task(Of Boolean)

        ' Hard soft-delete (IsDeleted=1) - iba ito sa Deactivate (IsActive=0)
        Function DeleteAsync(recordId As Integer) As Task(Of Boolean)

        ' Ginagamit ng Admin "Reset Password" button - iba ito sa self-service
        ' ResetPasswordAsync dahil dapat MustChangePassword=1 pagkatapos
        ' (kabaligtaran ng self-service, na nagse-set ng 0).
        Function AdminResetPasswordAsync(recordId As Integer, newHash As String, newSalt As String) As Task(Of Boolean)
    End Interface
End Namespace