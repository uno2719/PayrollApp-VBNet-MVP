Namespace Employee.Views
    Public Interface IEmployeesEarningsView
        Inherits IBaseView

        ' --- Identity ---
        Property RecordId As Integer

        ' --- Earnings ---
        Property BasicSalary As Decimal?
        Property DailyRate As Decimal?
        Property HourlyRate As Decimal?
        Property DaysInYear As Integer?
        Property WorkHourPer As Decimal?
        Property PayrollFlag As Boolean
        Property MinimumWage As Boolean
        Property PayCycle As String
        Property TaxFlag As String
        Property PayBy As String
        Property BankId As Integer?
        Property BankAccount As String

        ' --- Lookup Loader ---
        Sub LoadBanks(data As List(Of GlobalShared.Models.LookupModel))

        ''' <summary>Mga Active na pay cycle (Payroll Settings > Pay Cycle) - ito ang options ng Pay Cycle at Tax Flag.</summary>
        Sub LoadPayCycles(activeCycles As List(Of String))

        ' --- Clear Fields ---
        Sub ClearFields()

        ' --- Events ---
        Event OnSave As EventHandler
        Event OnNew As EventHandler
        Event OnSaveCompleted As EventHandler

    End Interface
End Namespace