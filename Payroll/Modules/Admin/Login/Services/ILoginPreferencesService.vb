Imports Payroll.Login.Models

Namespace Login.Services
    Public Interface ILoginPreferencesService
        Function Load() As LoginPreferences
        Sub Save(preferences As LoginPreferences)
    End Interface
End Namespace