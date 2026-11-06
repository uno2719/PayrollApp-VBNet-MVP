Namespace GlobalShared.Models

    ''' <summary>
    ''' Resulta ng totoong pagbura (hard delete) ng isang Settings record.
    ''' Message ay handa nang ipakita sa user (hal. kung bakit hindi nabura).
    ''' </summary>
    Public Class DeleteResult
        Public Property Success As Boolean
        Public Property Message As String

        Public Shared Function Ok() As DeleteResult
            Return New DeleteResult With {.Success = True}
        End Function

        Public Shared Function Fail(message As String) As DeleteResult
            Return New DeleteResult With {.Success = False, .Message = message}
        End Function
    End Class

End Namespace
