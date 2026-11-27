' File: GlobalShared/Models/MinimumWageModel.vb
Namespace GlobalShared.Models

    ''' <summary>Isang row ng tblMinimumWage: minimum wage ng isang Wage Area simula sa EffectiveDate.</summary>
    Public Class MinimumWageModel
        Public Property MinimumWageId As Integer
        Public Property WageArea As String          ' hal. "NCR", "CAR", "CALABARZON - Component City"
        Public Property EffectiveDate As Date
        Public Property DailyRate As Decimal
        Public Property WageOrderNo As String
        Public Property Remarks As String
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ''' <summary>Hal. "NCR ₱755.00 (WO NCR-28, eff. 09/26/2026)".</summary>
        Public ReadOnly Property DisplayText As String
            Get
                Dim wo = If(String.IsNullOrWhiteSpace(WageOrderNo), "", $"WO {WageOrderNo.Trim()}, ")
                Return $"{WageArea} ₱{DailyRate:N2} ({wo}eff. {EffectiveDate:MM/dd/yyyy})"
            End Get
        End Property
    End Class

    ''' <summary>Isang branch/store at ang Wage Area nito (Settings > Payroll > Minimum Wage).</summary>
    Public Class BranchWageAreaModel
        Public Property BranchId As Integer
        Public Property BranchCode As String
        Public Property BranchName As String
        Public Property WageArea As String
    End Class

End Namespace
