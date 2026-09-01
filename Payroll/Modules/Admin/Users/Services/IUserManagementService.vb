Imports Payroll.Login.Models

Namespace Users.Services

    Public Interface IUserManagementService
        Function GetAllUsersAsync(filter As String) As Task(Of List(Of UserModel))
        Function GetAllModulesAsync() As Task(Of List(Of Models.ModuleInfo))
        Function GetModuleAccessAsync(userId As Integer) As Task(Of List(Of Models.ModuleAccessItem))

        Function CreateUserAsync(
            username As String, employeeNo As String, isAdmin As Boolean,
            moduleAccess As List(Of Models.ModuleAccessItem)) As Task(Of CreateUserResult)

        Function UpdateUserAsync(
            recordId As Integer, isAdmin As Boolean,
            moduleAccess As List(Of Models.ModuleAccessItem)) As Task(Of Boolean)

        Function DeactivateUserAsync(recordId As Integer) As Task(Of Boolean)
        Function ActivateUserAsync(recordId As Integer) As Task(Of Boolean)

        ' Ibinabalik ang bagong temporary password (plain text, minsan lang
        ' makikita - dito lang, hindi na naman ito ma-retrieve pagkatapos).
        Function ResetUserPasswordAsync(recordId As Integer) As Task(Of String)

        Function DeleteUserAsync(recordId As Integer) As Task(Of Boolean)
    End Interface

    Public Class CreateUserResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property TemporaryPassword As String
    End Class

End Namespace