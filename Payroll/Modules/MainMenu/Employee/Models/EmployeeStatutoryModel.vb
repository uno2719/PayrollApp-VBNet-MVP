Namespace Employee.Models
    Public Class EmployeeStatutoryModel
        Public Property StatutoryId As Integer
        Public Property RecordId As Integer
        Public Property TIN As String
        Public Property SSSNo As String
        Public Property PagIBIGNo As String
        Public Property PagIBIGVoluntary As Decimal?
        Public Property PhilHealthNo As String
        Public Property FixTax As Decimal?

        ' Audit
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace