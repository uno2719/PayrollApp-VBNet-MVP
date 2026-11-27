' File: Modules/MainMenu/Employee/Services/IEmployeeFixTxnService.vb
Imports Payroll.GlobalShared.Models

Namespace Employee.Services

    Public Interface IEmployeeFixTxnService
        Function GetByEmployeeAsync(recordId As Integer) As Task(Of List(Of EmployeeFixTxnModel))
        Function GetCodeOptionsAsync() As Task(Of List(Of FixTxnCodeOption))

        ''' <summary>Bago (FixTxnId = 0) o update. Nagbabato ng InvalidOperationException kapag hindi valid.</summary>
        Function SaveAsync(item As EmployeeFixTxnModel) As Task(Of Integer)

        ''' <summary>Set Active / Set Inactive. Ang pag-Active ay sinusuri ulit laban sa overlap.</summary>
        Function SetActiveAsync(fixTxnId As Integer, isActive As Boolean) As Task

        ''' <summary>Para lang sa hindi pa nagamit sa payroll; kung nagamit na, Set Inactive ang gamitin.</summary>
        Function DeleteAsync(fixTxnId As Integer) As Task
    End Interface

End Namespace
