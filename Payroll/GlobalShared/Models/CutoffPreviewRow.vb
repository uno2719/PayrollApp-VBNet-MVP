' File: GlobalShared/Models/CutoffPreviewRow.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row sa preview ng Generate Cut-off dialog: ang cutoff na GAGAWIN
    ''' (hindi pa naka-save), at kung may kapareho/overlap na sa database.
    ''' </summary>
    Public Class CutoffPreviewRow
        Public Property PeriodNo As Integer
        Public Property CutoffStart As Date
        Public Property CutoffEnd As Date
        Public Property PayDate As Date?
        Public Property AlreadyExists As Boolean

        Public ReadOnly Property StatusText As String
            Get
                Return If(AlreadyExists, "Exists (skipped)", "New")
            End Get
        End Property
    End Class

End Namespace
