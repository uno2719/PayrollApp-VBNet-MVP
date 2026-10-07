Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa Deduction o Bonus table - magkaparehong shape
    ''' ang dalawa (Code/Description + 4 flags + Active), kaya isang
    ''' model class lang ang kailangan, gaya ng ginawa kay LookupModel
    ''' para sa 8 Master Data tables at StatutoryBracketModel para sa 3
    ''' Statutory tables.
    ''' </summary>
    Public Class PayrollFlaggedEntryModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Description As String
        Public Property TaxFlag As Boolean
        Public Property SSSFlag As Boolean
        Public Property PhilHealthFlag As Boolean
        Public Property PagIbigFlag As Boolean
        Public Property IsActive As Boolean = True
        Public Property IsEssential As Boolean

        ' Anong klaseng value ang tinatanggap ng column na ito sa Payroll Input Entry
        Public Property InputUnit As PayrollInputUnit = PayrollInputUnit.Amount
        Public ReadOnly Property InputUnitText As String
            Get
                Return PayrollInputUnits.Text(InputUnit)
            End Get
        End Property

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
