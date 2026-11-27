' File: Modules/MainMenu/Employee/Data/IEmployeeFixTxnRepository.vb
Imports Payroll.GlobalShared.Models

Namespace Employee.Data

    Public Interface IEmployeeFixTxnRepository
        ''' <summary>Lahat ng Fix Transaction ng empleyado (active at inactive), kasama ang description/unit ng code at IsUsed.</summary>
        Function GetByEmployeeAsync(recordId As Integer) As Task(Of List(Of EmployeeFixTxnModel))
        Function GetByIdAsync(fixTxnId As Integer) As Task(Of EmployeeFixTxnModel)

        ''' <summary>
        ''' Mga code na pwedeng piliin: tblCompensation / tblBonus / tblDeduction, HINDI kasama ang mga code na naka-map sa
        ''' General Settings > Code Mapping (Basic +/-, Absent, Late In, Early Out, Loan Refund). Kasama ang inactive (IsActive = False).
        ''' </summary>
        Function GetCodeOptionsAsync() As Task(Of List(Of FixTxnCodeOption))

        Function InsertAsync(item As EmployeeFixTxnModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As EmployeeFixTxnModel, userName As String) As Task(Of Boolean)
        Function SetActiveAsync(fixTxnId As Integer, isActive As Boolean, userName As String) As Task(Of Boolean)

        ''' <summary>Binubura lang kapag HINDI pa nagamit sa payroll (walang result line). False kung hindi nabura.</summary>
        Function DeleteIfUnusedAsync(fixTxnId As Integer) As Task(Of Boolean)
    End Interface

End Namespace
