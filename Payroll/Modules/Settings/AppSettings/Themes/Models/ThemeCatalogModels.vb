' File: Modules/Settings/AppSettings/Themes/Models/ThemeCatalogModels.vb
Imports System.Drawing

Namespace Themes.Models

    ''' <summary>Ang ilang kulay lang na kailangan para gumuhit ng mini "wireframe" ng isang skin sa card.</summary>
    Public NotInheritable Class ThemePreviewColors
        Public ReadOnly Property Control As Color
        Public ReadOnly Property Window As Color
        Public ReadOnly Property Text As Color
        Public ReadOnly Property Accent As Color
        Public ReadOnly Property IsDark As Boolean

        Public Sub New(ctrl As Color, wnd As Color, txt As Color, acc As Color)
            Me.Control = ctrl
            Me.Window = wnd
            Me.Text = txt
            Me.Accent = acc
            ' perceived luminance ng window background ang nagdedesisyon kung light o dark
            Dim lum = (0.299 * wnd.R + 0.587 * wnd.G + 0.114 * wnd.B) / 255.0
            Me.IsDark = lum < 0.5
        End Sub
    End Class

    Public NotInheritable Class ThemeSkinItem
        Public ReadOnly Property Name As String
        Public ReadOnly Property Preview As ThemePreviewColors
        Public ReadOnly Property IsDark As Boolean

        Public Sub New(skinName As String, pv As ThemePreviewColors)
            Me.Name = skinName
            Me.Preview = pv
            Me.IsDark = pv.IsDark
        End Sub
    End Class

End Namespace
