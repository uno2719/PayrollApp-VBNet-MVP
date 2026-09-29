' File: Modules/Settings/AppSettings/Themes/Views/ThemeSkinCard.vb
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports Payroll.Themes.Models

' Root namespace sinadya (parehong convention ng ibang UserControl/custom control).

Friend Module ThemeDrawing
    Friend Function RoundRect(r As Rectangle, radius As Integer) As GraphicsPath
        Dim d = radius * 2
        Dim p As New GraphicsPath()
        p.AddArc(r.X, r.Y, d, d, 180, 90)
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90)
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90)
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90)
        p.CloseFigure()
        Return p
    End Function

    ''' <summary>t=1 ay a, t=0 ay b.</summary>
    Friend Function Blend(a As Color, b As Color, t As Double) As Color
        t = Math.Max(0.0, Math.Min(1.0, t))
        Return Color.FromArgb(CInt(a.R * t + b.R * (1 - t)),
                              CInt(a.G * t + b.G * (1 - t)),
                              CInt(a.B * t + b.B * (1 - t)))
    End Function
End Module

''' <summary>
''' Isang skin = isang card. Gumuguhit tayo ng maliit na "fake app window" gamit ang TOTOONG
''' kulay ng skin, kaya makikita mo ang itsura bago mo pa i-click.
''' </summary>
Public Class ThemeSkinCard
    Inherits Control

    Public Event Picked(skinName As String)
    Public Event FavoriteToggled(skinName As String)

    Public ReadOnly Property SkinName As String
    Public ReadOnly Property IsDark As Boolean

    Private ReadOnly _pv As ThemePreviewColors
    Private ReadOnly _nameFont As New Font("Segoe UI Semibold", 9.5F)
    Private ReadOnly _starFont As New Font("Segoe UI Symbol", 11.0F)
    Private _selected As Boolean
    Private _favorite As Boolean
    Private _hover As Boolean
    Private _starRect As Rectangle

    Public Sub New(skinName As String, isDark As Boolean, preview As ThemePreviewColors)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or
                 ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Size = New Size(180, 132)
        Cursor = Cursors.Hand
        Me.SkinName = skinName
        Me.IsDark = isDark
        _pv = preview
    End Sub

    Public Property IsSelected As Boolean
        Get
            Return _selected
        End Get
        Set(value As Boolean)
            If _selected = value Then Return
            _selected = value
            Invalidate()
        End Set
    End Property

    Public Property IsFavorite As Boolean
        Get
            Return _favorite
        End Get
        Set(value As Boolean)
            If _favorite = value Then Return
            _favorite = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim card As New Rectangle(1, 1, Width - 3, Height - 3)
        Dim accent = _pv.Accent

        ' ---- card body + border ----
        Using path = ThemeDrawing.RoundRect(card, 10)
            Using b As New SolidBrush(_pv.Control)
                g.FillPath(b, path)
            End Using
            Dim borderCol As Color =
                If(_selected, accent,
                   If(_hover, ThemeDrawing.Blend(accent, _pv.Control, 0.6),
                      ThemeDrawing.Blend(_pv.Text, _pv.Control, 0.18)))
            Using pen As New Pen(borderCol, If(_selected, 2.5F, 1.0F))
                g.DrawPath(pen, path)
            End Using
        End Using

        ' ---- mini window ----
        Dim win As New Rectangle(card.X + 10, card.Y + 10, card.Width - 20, card.Height - 48)
        Using wp = ThemeDrawing.RoundRect(win, 6)
            g.SetClip(wp)

            Using b As New SolidBrush(_pv.Window)
                g.FillRectangle(b, win)
            End Using

            ' title bar + 3 tuldok
            Dim tb As New Rectangle(win.X, win.Y, win.Width, 14)
            Using b As New SolidBrush(ThemeDrawing.Blend(_pv.Text, _pv.Control, 0.1))
                g.FillRectangle(b, tb)
            End Using
            For i = 0 To 2
                Using b As New SolidBrush(If(i = 0, accent, ThemeDrawing.Blend(_pv.Text, _pv.Control, 0.35)))
                    g.FillEllipse(b, tb.X + 6 + i * 9, tb.Y + 5, 5, 5)
                End Using
            Next

            ' sidebar
            Dim sb As New Rectangle(win.X, tb.Bottom, 36, win.Height - tb.Height)
            Using b As New SolidBrush(_pv.Control)
                g.FillRectangle(b, sb)
            End Using
            For i = 0 To 2
                Using b As New SolidBrush(If(i = 0, accent, ThemeDrawing.Blend(_pv.Text, _pv.Control, 0.3)))
                    g.FillRectangle(b, sb.X + 6, sb.Y + 8 + i * 10, 24, 5)
                End Using
            Next

            ' content: header bar, 3 "rows", button pill
            Dim cx = sb.Right + 8
            Dim cw = win.Right - cx - 8
            Using b As New SolidBrush(accent)
                g.FillRectangle(b, cx, tb.Bottom + 7, cw, 8)
            End Using
            For i = 0 To 2
                Using b As New SolidBrush(ThemeDrawing.Blend(_pv.Text, _pv.Window, 0.22))
                    g.FillRectangle(b, cx, tb.Bottom + 21 + i * 10, CInt(cw * (1 - i * 0.18)), 4)
                End Using
            Next
            Using b As New SolidBrush(accent)
                g.FillRectangle(b, win.Right - 32, win.Bottom - 13, 24, 7)
            End Using

            g.ResetClip()
            Using pen As New Pen(ThemeDrawing.Blend(_pv.Text, _pv.Window, 0.2), 1.0F)
                g.DrawPath(pen, wp)
            End Using
        End Using

        ' ---- pangalan ----
        Dim nameRect As New Rectangle(card.X + 12, win.Bottom + 6, card.Width - 44, 22)
        TextRenderer.DrawText(g, SkinName, _nameFont, nameRect, _pv.Text,
                              TextFormatFlags.Left Or TextFormatFlags.VerticalCenter Or
                              TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPadding)

        ' ---- selected badge (accent na bilog + check) ----
        If _selected Then
            Dim badge As New Rectangle(card.Right - 28, win.Bottom + 8, 18, 18)
            Using b As New SolidBrush(accent)
                g.FillEllipse(b, badge)
            End Using
            Using pen As New Pen(Color.White, 2.0F)
                g.DrawLines(pen, New Point() {
                    New Point(badge.X + 4, badge.Y + 9),
                    New Point(badge.X + 8, badge.Y + 13),
                    New Point(badge.X + 14, badge.Y + 5)})
            End Using
        End If

        ' ---- favorite star (kita lang kapag naka-hover, o kapag favorite na) ----
        _starRect = New Rectangle(card.Right - 34, card.Y + 6, 24, 24)
        If _favorite OrElse _hover Then
            Using b As New SolidBrush(Color.FromArgb(175, _pv.Control))
                g.FillEllipse(b, _starRect)
            End Using
            Dim starCol = If(_favorite, Color.FromArgb(255, 193, 7), ThemeDrawing.Blend(_pv.Text, _pv.Control, 0.7))
            TextRenderer.DrawText(g, If(_favorite, ChrW(&H2605), ChrW(&H2606)), _starFont, _starRect, starCol,
                                  TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or
                                  TextFormatFlags.NoPadding)
        End If
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        _hover = True
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        _hover = False
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        If e.Button <> MouseButtons.Left Then Return
        If _starRect.Contains(e.Location) Then
            RaiseEvent FavoriteToggled(SkinName)
        Else
            RaiseEvent Picked(SkinName)
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _nameFont.Dispose()
            _starFont.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
