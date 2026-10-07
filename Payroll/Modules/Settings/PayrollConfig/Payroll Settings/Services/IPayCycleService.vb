Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Services
    Public Interface IPayCycleService
        Function GetAllAsync() As Task(Of List(Of PayCycleModel))

        ''' <summary>Rate basis lock: Daily (laging daily rate) o may processed cutoff na ang pay cycle.</summary>
        Function GetRateBasisLockReasonAsync(payCycleType As String) As Task(Of String)

        Function SaveAsync(item As PayCycleModel, userName As String) As Task(Of PayrollSettingsSaveResult)
    End Interface
End Namespace
