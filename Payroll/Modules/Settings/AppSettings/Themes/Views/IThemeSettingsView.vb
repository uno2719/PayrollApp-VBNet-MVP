' File: Modules/Settings/AppSettings/Themes/Views/IThemeSettingsView.vb
Imports Payroll.GlobalShared.Models
Imports Payroll.Themes.Models

Namespace Themes.Views
    Public Interface IThemeSettingsView
        ' ---- user intent (View -> Presenter) ----
        Event SkinPicked(skinName As String)
        Event FontSizeChanged(size As Single)
        Event RoundedChanged(value As Boolean)
        Event FavoriteToggled(skinName As String)
        Event UndoClicked()
        Event ResetClicked()
        ''' <summary>Kapag may nagbago ng skin sa LABAS ng screen na ito (hal. ang skin button sa taas).</summary>
        Event ExternalStyleChanged(skinName As String, paletteName As String)

        ' ---- rendering (Presenter -> View) ----
        Sub ShowSkins(skins As IReadOnlyList(Of ThemeSkinItem))
        Sub ShowCurrent(settings As ThemeSettings)
        Sub SetUndoEnabled(enabled As Boolean)
        Sub ShowToast(text As String)
    End Interface
End Namespace
