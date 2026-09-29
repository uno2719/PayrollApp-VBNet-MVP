Namespace GlobalShared.Models
    Public Class ThemeSettings
        Public Property SkinName As String = "WXI"
        Public Property PaletteName As String = Nothing

        ' --- idinagdag para sa Theme / Skin settings screen ---
        ' Naka-clamp sa 8..14 para kahit ma-edit nang mano-mano ang json,
        ' hindi masisira ang layout ng buong app.
        Private _fontSize As Single = 9.0F
        Public Property FontSize As Single
            Get
                Return _fontSize
            End Get
            Set(value As Single)
                _fontSize = Math.Max(8.0F, Math.Min(14.0F, value))
            End Set
        End Property

        Public Property RoundedCorners As Boolean = True

        Private _favorites As New List(Of String)()
        Public Property Favorites As List(Of String)
            Get
                Return _favorites
            End Get
            Set(value As List(Of String))
                _favorites = If(value, New List(Of String)())   ' null sa json -> walang crash
            End Set
        End Property

        Public Function Clone() As ThemeSettings
            Return New ThemeSettings With {
                .SkinName = SkinName,
                .PaletteName = PaletteName,
                .FontSize = FontSize,
                .RoundedCorners = RoundedCorners,
                .Favorites = New List(Of String)(Favorites)
            }
        End Function

        ''' <summary>Pareho ba ang laman? (Nothing at "" sa PaletteName ay ituturing na pareho.)</summary>
        Public Function SameAs(other As ThemeSettings) As Boolean
            If other Is Nothing Then Return False
            Return String.Equals(SkinName, other.SkinName, StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(If(PaletteName, ""), If(other.PaletteName, ""), StringComparison.OrdinalIgnoreCase) AndAlso
                   FontSize = other.FontSize AndAlso
                   RoundedCorners = other.RoundedCorners AndAlso
                   Favorites.OrderBy(Function(x) x).SequenceEqual(other.Favorites.OrderBy(Function(x) x))
        End Function
    End Class
End Namespace
