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
        Property PayBy As String
        Property BankId As Integer?
        Property BankAccount As String

        ' --- Lookup Loader ---
        Sub LoadBanks(data As List(Of GlobalShared.Models.LookupModel))

        ''' <summary>Mga Active na pay cycle (Payroll Settings > Pay Cycle) - ito ang options ng Pay Cycle.</summary>
        Sub LoadPayCycles(activeCycles As List(Of String))

        ''' <summary>
        ''' Rate Basis ng bawat pay cycle (Basic Salary o Daily Rate) at ang Daily Rate Monthly Factor
        ''' (General Settings, default 30). Dito nakabatay kung alin ang input ng Earnings tab.
        ''' </summary>
        Sub ConfigureRateInputs(bases As Dictionary(Of String, GlobalShared.Models.PayRateBasis), dailyRateMonthlyFactor As Decimal)

        ' --- Clear Fields ---
        Sub ClearFields()

        ' --- Events ---
        Event OnSave As EventHandler
        Event OnNew As EventHandler
        Event OnSaveCompleted As EventHandler

    End Interface
End Namespace