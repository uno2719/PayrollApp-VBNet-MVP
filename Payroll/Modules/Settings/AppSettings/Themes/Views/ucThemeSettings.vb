' File: Modules/Settings/AppSettings/Themes/Views/ucThemeSettings.vb
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.LookAndFeel
Imports Payroll.GlobalShared.Models
Imports Payroll.Themes.Models
Imports Payroll.Themes.Presenters
Imports Payroll.Themes.Views

' UserControl: root namespace (project convention).
Public Class ucThemeSettings
    Implements IThemeSettingsView, IAsyncLoadable

    Public Event SkinPicked(skinName As String) Implements IThemeSettingsView.SkinPicked
    Public Event FontSizeChanged(size As Single) Implements IThemeSettingsView.FontSizeChanged
    Public Event RoundedChanged(value As Boolean) Implements IThemeSettingsView.RoundedChanged
    Public Event FavoriteToggled(skinName As String) Implements IThemeSettingsView.FavoriteToggled
    Public Event UndoClicked() Implements IThemeSettingsView.UndoClicked
    Public Event ResetClicked() Implements IThemeSettingsView.ResetClicked
    Public Event ExternalStyleChanged(skinName As String, paletteName As String) Implements IThemeSettingsView.ExternalStyleChanged

    Private _presenter As ThemeSettingsPresenter
    Private ReadOnly _cards As New Dictionary(Of String, ThemeSkinCard)(StringComparer.OrdinalIgnoreCase)
    Private _updating As Boolean        ' True habang programmatic ang pagbabago ng controls -> huwag i-echo pabalik ang events
    Private _hooked As Boolean

    Public Sub SetPresenter(presenter As ThemeSettingsPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Theme / Skin"
        End Get
    End Property

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Application Settings > Theme / Skin"
        End Get
    End Property

    ' Personal na preference ng Windows user ang theme (nasa %AppData%), kaya walang
    ' HasEditAccess restriction dito - ang nav permission (View) ang sapat na gate.
    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Themes
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        If Not _hooked Then
            _hooked = True
            ' pakinggan din ang skin button sa taas (direkta nitong binabago ang UserLookAndFeel.Default)
            AddHandler UserLookAndFeel.Default.StyleChanged, AddressOf OnLafChanged
            ' Static ang UserLookAndFeel.Default - kailangang i-unhook, kundi hindi na mamamatay ang control na ito.
            AddHandler Me.Disposed, AddressOf OnViewDisposed
        End If
        _presenter.Load()
        Await Task.CompletedTask
    End Function

    Private Sub OnViewDisposed(sender As Object, e As EventArgs)
        RemoveHandler UserLookAndFeel.Default.StyleChanged, AddressOf OnLafChanged
    End Sub

    Private Sub OnLafChanged(sender As Object, e As EventArgs)
        If IsDisposed OrElse Not IsHandleCreated Then Return
        ' ActiveSkinName ay "WXI" pa rin kahit Compact ang mode - kailangang tignan
        ' muna ang CompactUIModeForced (tingnan din ang parehong bagay sa
        ' Application.vb.OnThemeChanged) para tama ang lumalabas na naka-select
        ' sa gallery, kahit galing sa top button o sa palette dialog ang pagbabago.
        Dim skinName = If(UserLookAndFeel.Default.CompactUIModeForced,
                           "WXI Compact",
                           UserLookAndFeel.Default.ActiveSkinName)
        RaiseEvent ExternalStyleChanged(skinName, UserLookAndFeel.Default.ActiveSvgPaletteName)
    End Sub

    ' ------------------------------------------------------------------ IThemeSettingsView

    Public Sub ShowSkins(skins As IReadOnlyList(Of ThemeSkinItem)) Implements IThemeSettingsView.ShowSkins
        flowSkins.SuspendLayout()
        For Each c In _cards.Values
            c.Dispose()                 ' HINDI nagdi-dispose ang Controls.Clear()
        Next
        flowSkins.Controls.Clear()
        _cards.Clear()

        For Each s In skins
            Dim card As New ThemeSkinCard(s.Name, s.IsDark, s.Preview)
            card.Margin = New Padding(8)
            AddHandler card.Picked, Sub(name As String) If Not _updating Then RaiseEvent SkinPicked(name)
            AddHandler card.FavoriteToggled, Sub(name As String) RaiseEvent FavoriteToggled(name)
            _cards(s.Name) = card
            flowSkins.Controls.Add(card)
        Next
        flowSkins.ResumeLayout()
        ApplyFilter()
    End Sub

    Public Sub ShowCurrent(settings As ThemeSettings) Implements IThemeSettingsView.ShowCurrent
        _updating = True
        Try
            For Each kv In _cards
                kv.Value.IsSelected = String.Equals(kv.Key, settings.SkinName, StringComparison.OrdinalIgnoreCase)
                kv.Value.IsFavorite = settings.Favorites.Contains(kv.Key, StringComparer.OrdinalIgnoreCase)
            Next
            lblCurrentSkin.Text = settings.SkinName
            lblCurrentPalette.Text = If(String.IsNullOrEmpty(settings.PaletteName), "Default palette", settings.PaletteName)

            Dim size = CInt(Math.Round(settings.FontSize))
            trackFont.Value = Math.Max(trackFont.Properties.Minimum, Math.Min(trackFont.Properties.Maximum, size))
            lblFontValue.Text = size.ToString() & " pt"
            chkRounded.Checked = settings.RoundedCorners

            Dim sel As ThemeSkinCard = Nothing
            If _cards.TryGetValue(settings.SkinName, sel) AndAlso sel.Visible Then
                flowSkins.ScrollControlIntoView(sel)     ' nag-si-scroll lang kung nasa labas ng screen ang card
            End If
        Finally
            _updating = False
        End Try
        ApplyFilter()       ' dapat sumabay ang "Favorites" filter kapag may na-toggle na star
    End Sub

    Public Sub SetUndoEnabled(enabled As Boolean) Implements IThemeSettingsView.SetUndoEnabled
        btnUndo.Enabled = enabled
    End Sub

    Public Sub ShowToast(text As String) Implements IThemeSettingsView.ShowToast
        lblStatus.Text = ChrW(&H2713) & " " & text
        tmrStatus.Stop()
        tmrStatus.Start()
    End Sub

    ' ------------------------------------------------------------------ filtering

    Private Sub ApplyFilter()
        Dim q = If(txtSearch.Text, "").Trim()
        Dim mode = If(rgFilter.EditValue Is Nothing, 0, Convert.ToInt32(rgFilter.EditValue))

        flowSkins.SuspendLayout()
        For Each card In _cards.Values
            Dim ok = (q.Length = 0 OrElse card.SkinName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
            Select Case mode
                Case 1 : ok = ok AndAlso Not card.IsDark
                Case 2 : ok = ok AndAlso card.IsDark
                Case 3 : ok = ok AndAlso card.IsFavorite
            End Select
            card.Visible = ok
        Next
        flowSkins.ResumeLayout()
    End Sub

    Private Sub txtSearch_EditValueChanged(sender As Object, e As EventArgs) Handles txtSearch.EditValueChanged
        ApplyFilter()
    End Sub

    Private Sub rgFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rgFilter.SelectedIndexChanged
        ApplyFilter()
    End Sub

    ' ------------------------------------------------------------------ control -> events

    Private Sub trackFont_EditValueChanged(sender As Object, e As EventArgs) Handles trackFont.EditValueChanged
        ' Kumakapit lang ito sa preview label habang hinihila ang slider - HINDI pa
        ' nag-a-apply/save. Ang totoong commit ay nasa trackFont_MouseUp (isang beses
        ' lang, pagbitaw ng mouse), kasi walang "continuous update off" na property
        ' ang RepositoryItemTrackBar.
        If _updating Then Return
        lblFontValue.Text = trackFont.Value.ToString() & " pt"
    End Sub

    Private Sub trackFont_MouseUp(sender As Object, e As MouseEventArgs) Handles trackFont.MouseUp
        If _updating Then Return
        RaiseEvent FontSizeChanged(CSng(trackFont.Value))
    End Sub

    Private Sub btnChoosePalette_Click(sender As Object, e As EventArgs) Handles btnChoosePalette.Click
        ' Opisyal na DevExpress dialog - dito nakadepende ang pagbibigay ng listahan
        ' ng available palettes ng kasalukuyang skin, kaya hindi na tayo mismo
        ' nagpi-plantsa ng sariling enumeration (doon nagkaproblema dati).
        Using dialog As New DevExpress.Customization.SvgSkinPaletteSelector(Me.FindForm())
            dialog.ShowDialog()
        End Using
        ' Ang UserLookAndFeel.Default.StyleChanged ang mag-fi-fire nito (kahit
        ' walang "Apply" na pinindot) - susunod dito ang OnLafChanged sa itaas.
    End Sub

    Private Sub chkRounded_CheckedChanged(sender As Object, e As EventArgs) Handles chkRounded.CheckedChanged
        If _updating Then Return
        RaiseEvent RoundedChanged(chkRounded.Checked)
    End Sub

    Private Sub btnUndo_Click(sender As Object, e As EventArgs) Handles btnUndo.Click
        RaiseEvent UndoClicked()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        RaiseEvent ResetClicked()
    End Sub

    Private Sub tmrStatus_Tick(sender As Object, e As EventArgs) Handles tmrStatus.Tick
        tmrStatus.Stop()
        lblStatus.Text = ""
    End Sub

End Class
