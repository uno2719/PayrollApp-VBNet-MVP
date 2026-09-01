Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Contracts
    Public Interface IThemeSettingsService
        Function Load() As ThemeSettings
        Sub Save(settings As ThemeSettings)
    End Interface
End Namespace