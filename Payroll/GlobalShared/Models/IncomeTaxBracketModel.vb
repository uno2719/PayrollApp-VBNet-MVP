Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang salary-bracket row mula sa alinman sa 5 Income Tax Table
    ''' tabs (Yearly, Monthly, Semi-Monthly, Weekly, Daily) - magkatulad
    ''' ang shape ng lima, kaya isang model class lang ang kailangan,
    ''' gaya ng ginawa natin kay StatutoryBracketModel para sa
    ''' SSS/PhilHealth/Pag-IBIG.
    ''' </summary>
    Public Class IncomeTaxBracketModel
        Public Property Id As Integer
        Public Property SalaryFrom As Decimal
        Public Property SalaryTo As Decimal
        Public Property TaxPercentage As Decimal
        Public Property FixTaxAmount As Decimal
        Public Property IsActive As Boolean = True
    End Class
End Namespace