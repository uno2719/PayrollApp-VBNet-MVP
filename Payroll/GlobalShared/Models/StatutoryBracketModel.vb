Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang salary-bracket row mula sa alinman sa 3 Statutory tables
    ''' (SSS, PhilHealth, Pag-IBIG) - magkatulad ang shape ng tatlo,
    ''' kaya isang model class lang ang kailangan, gaya ng ginawa natin
    ''' kay LookupModel para sa 8 Master Data tables.
    ''' </summary>
    Public Class StatutoryBracketModel
        Public Property Id As Integer
        Public Property SalaryFrom As Decimal
        Public Property SalaryTo As Decimal
        Public Property EEShare As Decimal
        Public Property EEContriType As String
        Public Property ERShare As Decimal
        Public Property ERContriType As String
        Public Property ECCAmount As Decimal
        Public Property EEMPF As Decimal
        Public Property ERMPF As Decimal
        Public Property IsActive As Boolean = True
    End Class
End Namespace