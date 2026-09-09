Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa Overtime o Holiday table - magkaparehong shape
    ''' ang dalawa (Code/Description + 4 flags + Rate + Map Code + Active).
    ''' Ang Holiday ay hiniwalay mula sa Overtime (dating magkasama sa
    ''' isang C1Pay tab) papunta sa sarili nitong table/tab, pero dahil
    ''' magkapareho pa rin ang structure, isang model class + isang View
    ''' class lang din ang kailangan (2 instances), gaya ng ginawa sa
    ''' Deduction/Bonus.
    ''' </summary>
    Public Class PayrollRateEntryModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Description As String
        Public Property TaxFlag As Boolean
        Public Property SSSFlag As Boolean
        Public Property PhilHealthFlag As Boolean
        Public Property PagIbigFlag As Boolean
        Public Property Rate As Decimal
        Public Property MapCode As String
        Public Property IsActive As Boolean = True

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace
