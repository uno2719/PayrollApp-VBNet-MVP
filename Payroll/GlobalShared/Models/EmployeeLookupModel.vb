Namespace GlobalShared.Models
    Public Class EmployeeLookupModel
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property FullName As String

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{EmployeeNo} | {FullName}"
            End Get
        End Property
    End Class
End Namespace