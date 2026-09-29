' File: Modules/Settings/AppSettings/Themes/Presenters/ThemeSettingsPresenter.vb
Imports Payroll.GlobalShared.Models
Imports Payroll.Themes.Services
Imports Payroll.Themes.Views

Namespace Themes.Presenters

    ''' <summary>
    ''' AUTO-SAVE ang design: bawat pili = apply + save agad, kapareho ng skin button sa taas
    ''' (na sinasave na ng MyApplication.OnThemeChanged). Kaya walang "Save" button -
    ''' may "Undo changes" (balik sa itsura nung binuksan mo ang screen) at "Reset to Default".
    ''' </summary>
    Public Class ThemeSettingsPresenter

        Private ReadOnly _view As IThemeSettingsView
        Private ReadOnly _service As IThemeCatalogService

        Private _sessionStart As New ThemeSettings()    ' itsura nung binuksan ang screen -> target ng Undo
        Private _working As New ThemeSettings()
        Private _loaded As Boolean
        Private _applying As Boolean                    ' True habang TAYO ang nagpapalit ng skin (tingnan ang OnExternalStyleChanged)

        Public Sub New(view As IThemeSettingsView, service As IThemeCatalogService)
            _view = view
            _service = service

            AddHandler _view.SkinPicked, AddressOf OnSkinPicked
            AddHandler _view.FontSizeChanged, AddressOf OnFontSizeChanged
            AddHandler _view.RoundedChanged, AddressOf OnRoundedChanged
            AddHandler _view.FavoriteToggled, AddressOf OnFavoriteToggled
            AddHandler _view.UndoClicked, AddressOf OnUndo
            AddHandler _view.ResetClicked, AddressOf OnReset
            AddHandler _view.ExternalStyleChanged, AddressOf OnExternalStyleChanged
        End Sub

        Public Sub Load()
            _working = _service.Load()
            _sessionStart = _working.Clone()
            _view.ShowSkins(_service.GetSkins())
            RefreshView()
            _loaded = True
        End Sub

        Private Sub OnSkinPicked(skinName As String)
            _working.SkinName = skinName
            _working.PaletteName = Nothing          ' bagong skin -> simulan sa default palette niya
            Commit()
            RefreshView()
        End Sub

        Private Sub OnFontSizeChanged(size As Single)
            _working.FontSize = size
            Commit()
            _view.SetUndoEnabled(CanUndo)
        End Sub

        Private Sub OnRoundedChanged(value As Boolean)
            _working.RoundedCorners = value
            Commit()
            _view.SetUndoEnabled(CanUndo)
        End Sub

        Private Sub OnFavoriteToggled(skinName As String)
            If _working.Favorites.Contains(skinName, StringComparer.OrdinalIgnoreCase) Then
                _working.Favorites.RemoveAll(Function(x) String.Equals(x, skinName, StringComparison.OrdinalIgnoreCase))
            Else
                _working.Favorites.Add(skinName)
            End If
            _service.Save(_working)                 ' preference lang ito - walang ia-apply sa DevExpress
            _view.ShowCurrent(_working)
            _view.SetUndoEnabled(CanUndo)
            _view.ShowToast("Saved")
        End Sub

        Private Sub OnUndo()
            _working = _sessionStart.Clone()
            Commit()
            RefreshView()
        End Sub

        Private Sub OnReset()
            Dim keepFavs = _working.Favorites       ' hindi dapat mabura ang favorites ng "reset"
            _working = New ThemeSettings() With {.Favorites = keepFavs}
            Commit()
            RefreshView()
        End Sub

        ''' <summary>
        ''' Ang skin button sa taas ay direktang nagpapalit ng UserLookAndFeel.Default. Sinusundan lang natin
        ''' (ang pag-save ay ginagawa na ng MyApplication.OnThemeChanged). Nagpa-fire din ng StyleChanged ang
        ''' sarili nating Apply, kaya may _applying guard.
        ''' </summary>
        Private Sub OnExternalStyleChanged(skinName As String, paletteName As String)
            If _applying OrElse Not _loaded Then Return
            _working.SkinName = skinName
            _working.PaletteName = If(String.IsNullOrEmpty(paletteName), Nothing, paletteName)
            RefreshView()
        End Sub

        Private ReadOnly Property CanUndo As Boolean
            Get
                Return Not _working.SameAs(_sessionStart)
            End Get
        End Property

        Private Sub Commit()
            _applying = True
            Try
                _service.Apply(_working)
            Finally
                _applying = False
            End Try
            _service.Save(_working)
            _view.ShowToast("Saved")
        End Sub

        Private Sub RefreshView()
            _view.ShowCurrent(_working)
            _view.SetUndoEnabled(CanUndo)
        End Sub

    End Class

End Namespace
