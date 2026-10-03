Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    Public Interface ICutoffRepository
        Function GetAllAsync() As Task(Of List(Of CutoffModel))
        Function OverlapExistsAsync(cycleType As String, cutoffStart As Date, cutoffEnd As Date, excludeId As Integer) As Task(Of Boolean)
        Function InsertAsync(item As CutoffModel, userName As String) As Task(Of Integer)
        Function UpdateAsync(item As CutoffModel, userName As String) As Task(Of Boolean)

        ''' <summary>Bulk insert for "Generate for Year" — skips any period that would overlap an existing Cutoff of the same CycleType.</summary>
        Function BulkInsertAsync(items As List(Of CutoffModel), userName As String) As Task(Of Integer)
    End Interface
End Namespace