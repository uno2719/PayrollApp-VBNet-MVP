Namespace Employee.Models
    Public Class EmployeeEarningsModel
        Public Property EarningsId As Integer
        Public Property RecordId As Integer
        Public Property BasicSalary As Decimal?
        Public Property DailyRate As Decimal?
        Public Property HourlyRate As Decimal?
        Public Property DaysInYear As Integer?
        Public Property WorkHourPer As Decimal?
        Public Property PayrollFlag As Boolean
        Public Property MinimumWage As Boolean
        Public Property PayCycle As String
        Public Property TaxFlag As String
        Public Property PayBy As String
        Public Property BankId As Integer?
        Public Property BankName As String
        Public Property BankAccount As String

        ' Audit
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace