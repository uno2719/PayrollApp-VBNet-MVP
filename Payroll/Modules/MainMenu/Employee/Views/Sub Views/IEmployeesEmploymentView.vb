Namespace Employee.Views
    Public Interface IEmployeesEmploymentView
        Inherits IBaseView

        ' --- Identity ---
        Property RecordId As Integer

        ' --- Employment ---
        Property EmployeeStatus As String
        Property EmploymentType As String

        ' --- FK IDs ---
        Property BranchId As Integer?
        Property DepartmentId As Integer?
        Property PositionId As Integer?
        Property CategoryId As Integer?
        Property JobClassId As Integer?
        Property SuperiorRecordId As Integer?
        Property LeaveGroupId As Integer?
        Property ScheduleGroupId As Integer?

        ' --- Dates ---
        Property DateJoined As Date?
        Property DateRegularization As Date?
        Property DateLastPromoted As Date?
        Property DateResigned As Date?
        Property DateRetired As Date?
        Property DateTerminated As Date?

        ' --- Timekeeping ---
        Property TimekeepingControlNo As String

        ' --- Lookups (para i-populate ang dropdowns) ---
        Sub LoadBranches(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadDepartments(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadPositions(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadCategories(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadJobClasses(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadLeaveGroups(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadScheduleGroups(data As List(Of GlobalShared.Models.LookupModel))
        Sub LoadSuperiors(data As List(Of GlobalShared.Models.EmployeeLookupModel))


        ' --- Clear Fields ---
        Sub ClearFields()

        ' --- Events ---
        Event OnSave As EventHandler
        Event OnNew As EventHandler
        Event OnSaveCompleted As EventHandler

    End Interface
End Namespace