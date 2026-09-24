Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang row mula sa tblLeaveRule - ang "HEADER" ng isang Leave
    ''' Group + Leave Type combination. Ang mismong entitlement per
    ''' years-of-service ay NASA CHILD table (LeaveRuleBracketModel),
    ''' hindi dito - parehong hiwalay-header/child na hugis gaya ng
    ''' EmployeeLoanModel + EmployeeLoanScheduleModel.
    ''' </summary>
    Public Class LeaveRuleModel
        Public Property Id As Integer
        Public Property LeaveGroupId As Integer
        Public Property LeaveTypeId As Integer

        Public Property EntitlementMethod As String
        Public Property ComputeBasedOn As String
        Public Property PlotBasedOn As String
        Public Property AnniversaryPlotOn As String
        Public Property UnitOfMeasure As String

        Public Property HolidayIncluded As Boolean
        Public Property RequireAttachment As Boolean
        Public Property Monetize As Boolean
        Public Property ShowEntitlement As Boolean = True

        Public Property IsActive As Boolean = True

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ' ========================================================
        ' COMPUTED - pinupuno lang ng list query (JOIN sa
        ' tblLeaveGroup/tblLeaveType), gaya ng ginawa sa
        ' EmployeeLoanModel.LoanDescription/LoanType.
        ' ========================================================
        Public Property LeaveGroupCode As String
        Public Property LeaveGroupName As String
        Public Property LeaveTypeCode As String
        Public Property LeaveTypeName As String
        Public Property LeaveCategory As String
    End Class
End Namespace
