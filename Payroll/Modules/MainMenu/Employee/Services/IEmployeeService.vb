Namespace Employee.Services
    Public Interface IEmployeeService

        ' --- Personal Info ---
        Function GetAllRecordsAsync(Optional filter As String = "Active") As Task(Of List(Of Models.EmployeeModel))

        Function GetByIdAsync(recordId As Integer, Optional filter As String = "Active") As Task(Of Models.EmployeeModel)
        Function InsertAsync(model As Models.EmployeeModel) As Task(Of Integer)
        Function UpdateAsync(model As Models.EmployeeModel) As Task(Of Boolean)
        Function DeleteAsync(recordId As Integer) As Task(Of Boolean)

        ' --- Employment ---
        Function GetEmploymentAsync(recordId As Integer) As Task(Of Models.EmployeeEmploymentModel)
        Function SaveEmploymentAsync(model As Models.EmployeeEmploymentModel) As Task(Of Boolean)

        ' --- Earnings ---
        Function GetEarningsAsync(recordId As Integer) As Task(Of Models.EmployeeEarningsModel)
        Function SaveEarningsAsync(model As Models.EmployeeEarningsModel) As Task(Of Boolean)

        ' --- Statutory ---
        Function GetStatutoryAsync(recordId As Integer) As Task(Of Models.EmployeeStatutoryModel)
        Function SaveStatutoryAsync(model As Models.EmployeeStatutoryModel) As Task(Of Boolean)

        ' --- Lookups ---
        Function GetLookupsAsync(tableName As String) As Task(Of List(Of GlobalShared.Models.LookupModel))
        Function GetActivePayCyclesAsync() As Task(Of List(Of String))

        ''' <summary>Pay cycle (pangalan, walang gitling) -> Rate Basis nito. Kasama ang hindi na Active.</summary>
        Function GetPayCycleRateBasesAsync() As Task(Of Dictionary(Of String, GlobalShared.Models.PayRateBasis))

        ''' <summary>General Settings > Daily Rate Monthly Factor (default 30).</summary>
        Function GetDailyRateMonthlyFactorAsync() As Task(Of Decimal)
        Function GetEmployeeLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeLookupModel))

    End Interface
End Namespace