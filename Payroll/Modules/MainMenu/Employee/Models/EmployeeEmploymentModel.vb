Namespace Employee.Models
    Public Class EmployeeEmploymentModel
        Public Property EmploymentId As Integer
        Public Property RecordId As Integer
        Public Property EmployeeStatus As String
        Public Property EmploymentType As String

        ' FK IDs
        Public Property BranchId As Integer?
        Public Property DepartmentId As Integer?
        Public Property PositionId As Integer?
        Public Property CategoryId As Integer?
        Public Property JobClassId As Integer?
        Public Property SuperiorRecordId As Integer?
        Public Property LeaveGroupId As Integer?
        Public Property HolidayGroupId As Integer?
        Public Property ScheduleGroupId As Integer?

        ' Display names — para sa dropdowns (hindi na kailangan ng separate query)
        Public Property BranchName As String
        Public Property DepartmentName As String
        Public Property PositionName As String
        Public Property CategoryName As String
        Public Property JobClassName As String
        Public Property SuperiorName As String
        Public Property LeaveGroupName As String
        Public Property HolidayGroupName As String
        Public Property ScheduleGroupName As String

        ' Dates
        Public Property DateJoined As Date?
        Public Property DateRegularization As Date?
        Public Property DateLastPromoted As Date?
        Public Property DateLastIncremented As Date?
        Public Property DateLastTransferred As Date?
        Public Property DateResigned As Date?
        Public Property DateRetired As Date?
        Public Property DateTerminated As Date?

        ' Timekeeping
        Public Property TimekeepingControlNo As String

        ' Audit
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace