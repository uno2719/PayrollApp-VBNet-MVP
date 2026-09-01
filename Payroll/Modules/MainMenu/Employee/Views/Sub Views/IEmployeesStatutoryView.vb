Namespace Employee.Views
    Public Interface IEmployeesStatutoryView
        Inherits IBaseView

        ' --- Identity ---
        Property RecordId As Integer

        ' --- Statutory ---
        Property TIN As String
        Property SSSNo As String
        Property PagIBIGNo As String
        Property PagIBIGVoluntary As Decimal?
        Property PhilHealthNo As String
        Property FixTax As Decimal?

        ' --- Clear Fields ---
        Sub ClearFields()


        ' --- Events ---
        Event OnSave As EventHandler
        Event OnNew As EventHandler
        Event OnSaveCompleted As EventHandler

    End Interface
End Namespace