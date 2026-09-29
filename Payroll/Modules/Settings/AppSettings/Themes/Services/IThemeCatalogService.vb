' File: Modules/Settings/AppSettings/Themes/Services/IThemeCatalogService.vb
Imports Payroll.GlobalShared.Models
Imports Payroll.Themes.Models

Namespace Themes.Services
    Public Interface IThemeCatalogService
        Function Load() As ThemeSettings
        Sub Save(settings As ThemeSettings)
        ''' <summary>Ipinapasok sa DevExpress ang settings - agad na apektado ang LAHAT ng bukas na form.</summary>
        Sub Apply(settings As ThemeSettings)
        Function GetSkins() As IReadOnlyList(Of ThemeSkinItem)
    End Interface
End Namespace
