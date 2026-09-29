' File: Modules/Settings/AppSettings/Themes/Services/ThemeCatalogService.vb
Imports System.Drawing
Imports DevExpress.LookAndFeel
Imports DevExpress.Skins
Imports Payroll.GlobalShared.Contracts
Imports Payroll.GlobalShared.Models
Imports Payroll.Themes.Models

Namespace Themes.Services

    ''' <summary>
    ''' Ang persistence (themesettings.json) ay hawak pa rin ng existing na
    ''' IThemeSettingsService / JsonThemeSettingsService - hindi tayo gumawa ng
    ''' pangalawang file. Ang trabaho lang ng class na ito: mag-apply sa DevExpress
    ''' at magbigay ng listahan ng skins/palettes para sa gallery.
    ''' </summary>
    Public Class ThemeCatalogService
        Implements IThemeCatalogService

        Private Const FallbackSkin As String = "WXI"

        Private ReadOnly _persistence As IThemeSettingsService
        Private _skinCache As List(Of ThemeSkinItem)

        Public Sub New(persistence As IThemeSettingsService)
            _persistence = persistence
        End Sub

        Public Function Load() As ThemeSettings Implements IThemeCatalogService.Load
            Return _persistence.Load()
        End Function

        Public Sub Save(settings As ThemeSettings) Implements IThemeCatalogService.Save
            _persistence.Save(settings)
        End Sub

        Private Const CompactSkinLabel As String = "WXI Compact"

        Public Sub Apply(settings As ThemeSettings) Implements IThemeCatalogService.Apply
            ' 1) global font + rounded corners (WindowsFormsSettings = app-wide defaults,
            '    kapareho ng ginagawa ng MyApplication.OnInitialize sa startup)
            Dim f As New Font("Segoe UI", settings.FontSize)
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultFont = f
            DevExpress.XtraEditors.WindowsFormsSettings.DefaultMenuFont = f
            DevExpress.XtraEditors.WindowsFormsSettings.AllowRoundedWindowCorners =
                If(settings.RoundedCorners, DevExpress.Utils.DefaultBoolean.True, DevExpress.Utils.DefaultBoolean.False)

            ' 2) skin + palette. "WXI Compact" ay HINDI naka-rehistro bilang sarili
            ' niyang SkinContainer sa SkinManager.Default.Skins - variant lang ito ng
            ' WXI (parehong palette pero mas siksik ang spacing), kaya may sariling
            ' enum overload ito sa halip na ang normal na string-based SetSkinStyle.
            If String.Equals(settings.SkinName, CompactSkinLabel, StringComparison.OrdinalIgnoreCase) Then
                UserLookAndFeel.Default.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.WXICompact)
            Else
                ' Babalik sa WXI kung wala na ang naka-save na skin.
                Dim skin = If(SkinExists(settings.SkinName), settings.SkinName, FallbackSkin)
                If String.IsNullOrEmpty(settings.PaletteName) Then
                    UserLookAndFeel.Default.SetSkinStyle(skin)
                Else
                    UserLookAndFeel.Default.SetSkinStyle(skin, settings.PaletteName)
                End If
            End If
        End Sub

        Public Function GetSkins() As IReadOnlyList(Of ThemeSkinItem) Implements IThemeCatalogService.GetSkins
            If _skinCache Is Nothing Then
                Dim list As New List(Of ThemeSkinItem)()
                For Each sc As SkinContainer In SkinManager.Default.Skins
                    Try
                        list.Add(New ThemeSkinItem(sc.SkinName, BuildPreview(sc.SkinName, "")))
                    Catch
                        ' kung hindi ma-preview ang isang skin, hindi lang siya ipapakita
                    End Try
                Next

                ' Manual na idinagdag: hindi ito nakikita sa SkinManager.Default.Skins
                ' (tingnan ang paliwanag sa Apply), pero parehong preview colors lang
                ' naman ng WXI ang gagamitin - ang pinagkaiba ng Compact ay spacing,
                ' hindi kulay.
                Dim wxi = list.FirstOrDefault(Function(s) String.Equals(s.Name, "WXI", StringComparison.OrdinalIgnoreCase))
                If wxi IsNot Nothing Then
                    list.Add(New ThemeSkinItem(CompactSkinLabel, wxi.Preview))
                End If

                list.Sort(Function(a, b) String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase))
                _skinCache = list
            End If
            Return _skinCache
        End Function

        ' ------------------------------------------------------------------

        Private Shared Function SkinExists(name As String) As Boolean
            For Each sc As SkinContainer In SkinManager.Default.Skins
                If String.Equals(sc.SkinName, name, StringComparison.OrdinalIgnoreCase) Then Return True
            Next
            Return False
        End Function

        ''' <summary>
        ''' Binabasa ang kulay ng skin NANG HINDI ito ina-apply sa app: private na UserLookAndFeel ang gamit.
        ''' Kailangang False ang UseDefaultLookAndFeel, kundi susundan lang nito ang UserLookAndFeel.Default
        ''' at lahat ng card ay magmumukhang kasalukuyang skin.
        ''' </summary>
        Private Shared Function BuildPreview(skinName As String, paletteName As String) As ThemePreviewColors
            Using laf As New UserLookAndFeel(Nothing)
                laf.UseDefaultLookAndFeel = False
                If String.IsNullOrEmpty(paletteName) Then
                    laf.SetSkinStyle(skinName)
                Else
                    laf.SetSkinStyle(skinName, paletteName)
                End If
                Dim colors = CommonSkins.GetSkin(laf).Colors
                Return New ThemePreviewColors(
                    Pick(colors, CommonColors.Control, Color.Gainsboro),
                    Pick(colors, CommonColors.Window, Color.White),
                    Pick(colors, CommonColors.WindowText, Color.Black),
                    Pick(colors, "Highlight", Color.SteelBlue))
            End Using
        End Function

        Private Shared Function Pick(colors As SkinColors, name As String, fallback As Color) As Color
            Try
                Dim c As Color = colors(name)
                If c.IsEmpty OrElse c.A = 0 Then Return fallback
                Return c
            Catch
                Return fallback
            End Try
        End Function

    End Class

End Namespace
