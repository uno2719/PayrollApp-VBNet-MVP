Namespace GlobalShared.Constants
    Public Module AppConstants
        Public Const TAX_RATE As Decimal = 0.12D
        Public Const SYSTEM_NAME As String = "Pacsports Payroll"

        ' Iisang display format ng petsa sa grids at date pickers. Display lang ito -
        ' real na Date ang naka-save sa database. Kapag may "Date Format" setting na
        ' sa hinaharap, dito (at sa iisang lugar lang) babasahin ang value.
        Public Const DisplayDateFormat As String = "MM/dd/yyyy"


    End Module
End Namespace

