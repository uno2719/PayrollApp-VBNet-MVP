' File: GlobalShared/Models/PayCycleCounterModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblPayCycleCounter - eksaktong 4 na row lang
    ''' (Monthly/SemiMonthly/Weekly/Daily), bawat isa hawak ang "current
    ''' position" (Year/Period/Cutoff code) ng pay cycle type na 'yon.
    ''' Edit-in-place lang ito sa UI - walang Add/Delete, kaya walang
    ''' IsActive/soft-delete dito gaya ng ibang lookup tables.
    ''' </summary>
    Public Class PayCycleCounterModel
        Public Property PayCycleCounterId As Integer
        Public Property PayCycleType As String
        Public Property CurrentYear As Integer
        Public Property CurrentPeriod As Integer
        Public Property CurrentCutoffCode As String

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace