' File: GlobalShared/Controls/StatusChip.vb
Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Namespace GlobalShared.Controls

    ''' <summary>
    ''' Maliit na "pill" na may kulay (at optional na tuldok) - para sa status chips
    ''' sa mga header (hal. Regular / Probationary / Inactive).
    '''
    ''' BAKIT CUSTOM CONTROL AT HINDI LabelControl?
    '''   Ang LabelControl ay walang rounded na background. Ang chip na ito ay
    '''   kusang gumuguhit ng sarili niyang kulay (back/fore/dot), kaya maganda
    '''   ang contrast sa kahit anong DevExpress skin (light man o dark).
    '''
    ''' Kusa rin siyang nagse-set ng sarili niyang Size mula sa Text, kaya
    ''' bagay sa FlowLayoutPanel (walang AutoSize na kailangang i-setup).
    ''' </summary>
    <DesignerCategory("Code")>
    Public Class StatusChip
        Inherits Control

        Private _chipBackColor As Color = Color.FromArgb(241, 239, 232)
        Private _chipForeColor As Color = Color.FromArgb(68, 68, 65)
        Private _dotColor As Color = Color.Empty
        Private _bold As Boolean = False

        Private Const MeasureFlags As TextFormatFlags =
            TextFormatFlags.NoPadding Or TextFormatFlags.NoPrefix Or TextFormatFlags.SingleLine

        Private Const DrawFlags As TextFormatFlags =
            TextFormatFlags.NoPadding Or TextFormatFlags.NoPrefix Or TextFormatFlags.SingleLine Or
            TextFormatFlags.Left Or TextFormatFlags.VerticalCenter

        Public Sub New()
            SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                     ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or
                     ControlStyles.SupportsTransparentBackColor, True)
            SetStyle(ControlStyles.Selectable, False)

            BackColor = Color.Transparent
            Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point)
            Margin = New Padding(0, 0, 6, 0)
            TabStop = False
        End Sub

        ''' <summary>Bold ang text (hal. para sa pangunahing status chip).</summary>
        Public Property Bold As Boolean
            Get
                Return _bold
            End Get
            Set(value As Boolean)
                _bold = value
                RefreshSize()
                Invalidate()
            End Set
        End Property

        ''' <summary>Itakda ang kulay nang sabay-sabay. dot = Color.Empty kung walang tuldok.</summary>
        Public Sub SetColors(back As Color, fore As Color, dot As Color)
            _chipBackColor = back
            _chipForeColor = fore
            _dotColor = dot
            RefreshSize()
            Invalidate()
        End Sub

        Private Function DpiScale() As Single
            Return DeviceDpi / 96.0F
        End Function

        Private Function ChipFont() As Font
            Return New Font(Font, If(_bold, FontStyle.Bold, FontStyle.Regular))
        End Function

        Private Sub RefreshSize()
            If String.IsNullOrEmpty(Text) Then Return

            Dim s = DpiScale()
            Using f = ChipFont()
                Dim t = TextRenderer.MeasureText(Text, f, New Size(Integer.MaxValue, Integer.MaxValue), MeasureFlags)
                Dim padX = CInt(Math.Round(10 * s))
                Dim padY = CInt(Math.Round(3 * s))
                Dim dotW = If(_dotColor.IsEmpty, 0, CInt(Math.Round(14 * s)))
                Size = New Size(t.Width + padX * 2 + dotW, t.Height + padY * 2)
            End Using
        End Sub

        Protected Overrides Sub OnTextChanged(e As EventArgs)
            MyBase.OnTextChanged(e)
            RefreshSize()
            Invalidate()
        End Sub

        Protected Overrides Sub OnFontChanged(e As EventArgs)
            MyBase.OnFontChanged(e)
            RefreshSize()
            Invalidate()
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            RefreshSize()
        End Sub

        Protected Overrides Sub OnDpiChangedAfterParent(e As EventArgs)
            MyBase.OnDpiChangedAfterParent(e)
            RefreshSize()
            Invalidate()
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            If Width <= 1 OrElse Height <= 1 Then Return

            Dim g = e.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias

            Dim s = DpiScale()
            Dim rect As New Rectangle(0, 0, Width - 1, Height - 1)

            Using path = RoundedRectangle(rect, CInt(Math.Round(6 * s)))
                Using b As New SolidBrush(_chipBackColor)
                    g.FillPath(b, path)
                End Using
            End Using

            Dim x = CInt(Math.Round(10 * s))

            If Not _dotColor.IsEmpty Then
                Dim d = CInt(Math.Round(7 * s))
                Using b As New SolidBrush(_dotColor)
                    g.FillEllipse(b, x, (Height - d) \ 2, d, d)
                End Using
                x += CInt(Math.Round(14 * s))
            End If

            Using f = ChipFont()
                Dim textRect As New Rectangle(x, 0, Math.Max(1, Width - x - CInt(Math.Round(10 * s))), Height)
                TextRenderer.DrawText(g, Text, f, textRect, _chipForeColor, DrawFlags)
            End Using
        End Sub

        Private Shared Function RoundedRectangle(r As Rectangle, radius As Integer) As GraphicsPath
            Dim d = radius * 2
            Dim path As New GraphicsPath()
            path.AddArc(r.X, r.Y, d, d, 180, 90)
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90)
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90)
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90)
            path.CloseFigure()
            Return path
        End Function

    End Class

End Namespace
