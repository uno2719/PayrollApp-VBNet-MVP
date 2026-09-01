Imports Payroll.Employee.Services

Namespace Employee.Presenters
    Public Class EmployeesEmploymentPresenter
        Inherits BasePresenter(Of Employee.Views.IEmployeesEmploymentView)

        Private ReadOnly _service As Services.IEmployeeService
        Private _recordId As Integer = 0

        Public Sub New(view As Employee.Views.IEmployeesEmploymentView,
                       service As Employee.Services.IEmployeeService)
            MyBase.New(view)
            _service = service
            SubscribeEvents()
        End Sub

        Private Sub SubscribeEvents()
            AddHandler _view.OnSave, AddressOf HandleSave
            AddHandler _view.OnNew, AddressOf HandleNew
        End Sub

        ' =============================================
        ' LOAD LOOKUPS
        ' =============================================
        Public Async Sub LoadLookups()
            Try
                _view.LoadBranches(Await _service.GetLookupsAsync("tblBranch"))
                _view.LoadDepartments(Await _service.GetLookupsAsync("tblDepartment"))
                _view.LoadPositions(Await _service.GetLookupsAsync("tblPosition"))
                _view.LoadCategories(Await _service.GetLookupsAsync("tblCategoryCode"))
                _view.LoadJobClasses(Await _service.GetLookupsAsync("tblJobClass"))
                _view.LoadLeaveGroups(Await _service.GetLookupsAsync("tblLeaveGroup"))
                _view.LoadScheduleGroups(Await _service.GetLookupsAsync("tblScheduleGroup"))
                _view.LoadSuperiors(Await _service.GetEmployeeLookupAsync())
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Sub

        ' =============================================
        ' LOAD EMPLOYEE
        ' =============================================
        Public Async Sub LoadEmployee(recordId As Integer)
            Try
                _view.ShowLoading()
                _recordId = recordId

                ' Load lookups first
                LoadLookups()

                _view.ClearFields()

                Dim emp = Await _service.GetEmploymentAsync(recordId)
                If emp Is Nothing Then Return

                ' Map model → view
                '_view.RecordId = emp.EmploymentId
                _view.EmployeeStatus = If(emp.EmployeeStatus, String.Empty)
                _view.EmploymentType = If(emp.EmploymentType, String.Empty)
                _view.BranchId = emp.BranchId
                _view.DepartmentId = emp.DepartmentId
                _view.PositionId = emp.PositionId
                _view.CategoryId = emp.CategoryId
                _view.JobClassId = emp.JobClassId
                _view.SuperiorRecordId = emp.SuperiorRecordId
                _view.LeaveGroupId = emp.LeaveGroupId
                _view.ScheduleGroupId = emp.ScheduleGroupId
                _view.DateJoined = emp.DateJoined
                _view.DateRegularization = emp.DateRegularization
                _view.DateLastPromoted = emp.DateLastPromoted
                _view.DateResigned = emp.DateResigned
                _view.DateRetired = emp.DateRetired
                _view.DateTerminated = emp.DateTerminated
                _view.TimekeepingControlNo = If(emp.TimekeepingControlNo, String.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub

        ' =============================================
        ' NEW
        ' =============================================
        Private Sub HandleNew(sender As Object, e As EventArgs)
            _recordId = _view.RecordId
            LoadLookups()

            _view.EmployeeStatus = String.Empty
            _view.EmploymentType = String.Empty
            _view.BranchId = Nothing
            _view.DepartmentId = Nothing
            _view.PositionId = Nothing
            _view.CategoryId = Nothing
            _view.JobClassId = Nothing
            _view.SuperiorRecordId = Nothing
            _view.LeaveGroupId = Nothing
            _view.ScheduleGroupId = Nothing
            _view.DateJoined = Nothing
            _view.DateRegularization = Nothing
            _view.DateLastPromoted = Nothing
            _view.DateResigned = Nothing
            _view.DateRetired = Nothing
            _view.DateTerminated = Nothing
            _view.TimekeepingControlNo = String.Empty
        End Sub

        ' =============================================
        ' SAVE
        ' =============================================
        Private Async Sub HandleSave(sender As Object, e As EventArgs)
            Try
                _view.ShowLoading()

                Dim emp As New Models.EmployeeEmploymentModel With {
                    .RecordId = _recordId,
                    .EmployeeStatus = _view.EmployeeStatus,
                    .EmploymentType = _view.EmploymentType,
                    .BranchId = _view.BranchId,
                    .DepartmentId = _view.DepartmentId,
                    .PositionId = _view.PositionId,
                    .CategoryId = _view.CategoryId,
                    .JobClassId = _view.JobClassId,
                    .SuperiorRecordId = _view.SuperiorRecordId,
                    .LeaveGroupId = _view.LeaveGroupId,
                    .ScheduleGroupId = _view.ScheduleGroupId,
                    .DateJoined = _view.DateJoined,
                    .DateRegularization = _view.DateRegularization,
                    .DateLastPromoted = _view.DateLastPromoted,
                    .DateResigned = _view.DateResigned,
                    .DateRetired = _view.DateRetired,
                    .DateTerminated = _view.DateTerminated,
                    .TimekeepingControlNo = _view.TimekeepingControlNo
                }

                Await _service.SaveEmploymentAsync(emp)

                RaiseEvent OnSaveCompleted(_view, EventArgs.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub
        Public Event OnSaveCompleted As EventHandler

    End Class
End Namespace