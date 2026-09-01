Imports System.Text.RegularExpressions

Namespace GlobalShared.Helpers
    Public Module ValidationHelper
        Public Function IsValidEmail(email As String) As Boolean
            If String.IsNullOrWhiteSpace(email) Then Return False
            Dim pattern As String = "^[^@\s]+@[^@\s]+\.[^@\s]+$"
            Return Regex.IsMatch(email, pattern)
        End Function

        Public Function IsNumeric(value As String) As Boolean
            Return Decimal.TryParse(value, Nothing)
        End Function
    End Module
End Namespace
