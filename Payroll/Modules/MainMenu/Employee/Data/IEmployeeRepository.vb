Namespace Employee.Data
    Public Interface IEmployeeRepository



        ' --- Personal Info ---
        Function GetAllRecordsAsync(Optional filter As String = "Active") As Task(Of List(Of Models.EmployeeModel))
        Function GetByIdAsync(recordId As Integer) As Task(Of Models.EmployeeModel)
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

        ''' <summary>Mga pay cycle na Active sa Payroll Settings > Pay Cycle (tblPayCycle).</summary>
        Function GetActivePayCyclesAsync() As Task(Of List(Of String))

        ''' <summary>Lahat ng pay cycle (Active man o hindi) kasama ang Rate Basis: Basic Salary o Daily Rate ang input sa Earnings tab.</summary>
        Function GetPayCycleInfoAsync() As Task(Of List(Of Models.PayCycleInfoModel))

        ''' <summary>General Settings > Daily Rate Monthly Factor (default 30): Daily Rate x factor = buwanang katumbas.</summary>
        Function GetDailyRateMonthlyFactorAsync() As Task(Of Decimal)
        Function GetEmployeeLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeLookupModel))
        Function GetEmployeeContactLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeContactLookupModel))


    End Interface
End Namespace
