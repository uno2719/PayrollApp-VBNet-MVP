Imports Payroll.Employee.Presenters

Public Class ucEmployeesEmployment
    Inherits DevExpress.XtraEditors.XtraUserControl
    Implements Employee.Views.IEmployeesEmploymentView

    Private _presenter As Employee.Presenters.EmployeesEmploymentPresenter
    Private _selectedRecordId As Integer = 0

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetPresenter(presenter As Employee.Presenters.EmployeesEmploymentPresenter)
        _presenter = presenter
        AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
    End Sub

    Private Sub RelayOnSaveCompleted(sender As Object, e As EventArgs)
        RaiseEvent OnSaveCompleted(Me, EventArgs.Empty)
    End Sub


    ' --- Tawagin mula sa ucEmployees ---
    Public Sub LoadEmployee(recordId As Integer)
        _selectedRecordId = recordId
        _presenter.LoadEmployee(recordId)
    End Sub

    Public Sub RaiseNew(recordId As Integer)
        _selectedRecordId = recordId
        Me.RecordId = recordId
        RaiseEvent OnNew(Me, EventArgs.Empty)
    End Sub

    Public Sub RaiseSave()
        RaiseEvent OnSave(Me, EventArgs.Empty)
    End Sub

    Public Sub SetEditMode(enabled As Boolean)
        cboEmployeeStatus.Properties.ReadOnly = Not enabled
        cboEmploymentType.Properties.ReadOnly = Not enabled
        sleBranch.Properties.ReadOnly = Not enabled
        sleCategoryCode.Properties.ReadOnly = Not enabled
        sleDepartment.Properties.ReadOnly = Not enabled
        sleJobClass.Properties.ReadOnly = Not enabled
        sleLeaveGroup.Properties.ReadOnly = Not enabled
        slePosition.Properties.ReadOnly = Not enabled
        sleScheduleGroup.Properties.ReadOnly = Not enabled
        sleSuperior.Properties.ReadOnly = Not enabled
        txtControlNo.Properties.ReadOnly = Not enabled
        dtpDateJoined.Properties.ReadOnly = Not enabled
        dtpDateRegularization.Properties.ReadOnly = Not enabled
        dtpDateLastPromoted.Properties.ReadOnly = Not enabled
        dtpDateResigned.Properties.ReadOnly = Not enabled
        dtpDateRetired.Properties.ReadOnly = Not enabled
        dtpDateTerminated.Properties.ReadOnly = Not enabled
    End Sub

    ' =============================================
    ' INTERFACE PROPERTIES
    ' =============================================
    Public Property RecordId As Integer _
        Implements Employee.Views.IEmployeesEmploymentView.RecordId

    Public Property EmployeeStatus As String _
        Implements Employee.Views.IEmployeesEmploymentView.EmployeeStatus
        Get
            Return cboEmployeeStatus.Text
        End Get
        Set(value As String)
            cboEmployeeStatus.Text = value
        End Set
    End Property

    Public Property EmploymentType As String _
    Implements Employee.Views.IEmployeesEmploymentView.EmploymentType
        Get
            Return cboEmploymentType.Text
        End Get
        Set(value As String)
            cboEmploymentType.Text = value
        End Set
    End Property

    ' ✅ BranchId
    Public Property BranchId As Integer? _
     Implements Employee.Views.IEmployeesEmploymentView.BranchId
        Get
            If sleBranch.EditValue Is Nothing OrElse
                sleBranch.EditValue Is DBNull.Value Then Return Nothing

            Return CType(sleBranch.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleBranch.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DepartmentId
    Public Property DepartmentId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.DepartmentId
        Get
            If sleDepartment.EditValue Is Nothing OrElse
           sleDepartment.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleDepartment.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleDepartment.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ PositionId
    Public Property PositionId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.PositionId
        Get
            If slePosition.EditValue Is Nothing OrElse
           slePosition.EditValue Is DBNull.Value Then Return Nothing
            Return CType(slePosition.EditValue, Integer)
        End Get
        Set(value As Integer?)
            slePosition.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ CategoryId
    Public Property CategoryId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.CategoryId
        Get
            If sleCategoryCode.EditValue Is Nothing OrElse
           sleCategoryCode.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleCategoryCode.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleCategoryCode.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ JobClassId
    Public Property JobClassId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.JobClassId
        Get
            If sleJobClass.EditValue Is Nothing OrElse
           sleJobClass.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleJobClass.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleJobClass.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ SuperiorRecordId
    Public Property SuperiorRecordId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.SuperiorRecordId
        Get
            If sleSuperior.EditValue Is Nothing OrElse
           sleSuperior.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleSuperior.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleSuperior.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ LeaveGroupId
    Public Property LeaveGroupId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.LeaveGroupId
        Get
            If sleLeaveGroup.EditValue Is Nothing OrElse
           sleLeaveGroup.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleLeaveGroup.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleLeaveGroup.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ ScheduleGroupId
    Public Property ScheduleGroupId As Integer? _
    Implements Employee.Views.IEmployeesEmploymentView.ScheduleGroupId
        Get
            If sleScheduleGroup.EditValue Is Nothing OrElse
           sleScheduleGroup.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleScheduleGroup.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleScheduleGroup.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateJoined
    Public Property DateJoined As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateJoined
        Get
            If dtpDateJoined.EditValue Is Nothing OrElse
           dtpDateJoined.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateJoined.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateJoined.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateRegularization
    Public Property DateRegularization As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateRegularization
        Get
            If dtpDateRegularization.EditValue Is Nothing OrElse
           dtpDateRegularization.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateRegularization.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateRegularization.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateLastPromoted
    Public Property DateLastPromoted As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateLastPromoted
        Get
            If dtpDateLastPromoted.EditValue Is Nothing OrElse
           dtpDateLastPromoted.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateLastPromoted.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateLastPromoted.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateResigned
    Public Property DateResigned As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateResigned
        Get
            If dtpDateResigned.EditValue Is Nothing OrElse
           dtpDateResigned.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateResigned.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateResigned.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateRetired
    Public Property DateRetired As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateRetired
        Get
            If dtpDateRetired.EditValue Is Nothing OrElse
           dtpDateRetired.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateRetired.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateRetired.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    ' ✅ DateTerminated
    Public Property DateTerminated As Date? _
    Implements Employee.Views.IEmployeesEmploymentView.DateTerminated
        Get
            If dtpDateTerminated.EditValue Is Nothing OrElse
           dtpDateTerminated.EditValue Is DBNull.Value Then Return Nothing
            Return CType(dtpDateTerminated.EditValue, Date)
        End Get
        Set(value As Date?)
            dtpDateTerminated.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    Public Property TimekeepingControlNo As String _
        Implements Employee.Views.IEmployeesEmploymentView.TimekeepingControlNo
        Get
            Return txtControlNo.Text
        End Get
        Set(value As String)
            txtControlNo.Text = value
        End Set
    End Property

    ' =============================================
    ' LOOKUP LOADERS — i-populate ang dropdowns
    ' =============================================
    Public Sub LoadBranches(
        data As List(Of GlobalShared.Models.LookupModel)) _
        Implements Employee.Views.IEmployeesEmploymentView.LoadBranches

        With sleBranch.Properties
            .DataSource = Nothing
            .DisplayMember = "Name"
            .ValueMember = "Id"
            .DataSource = data
        End With

        sleBranch.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))

    End Sub

    Public Sub LoadDepartments(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadDepartments
        sleDepartment.Properties.DataSource = data
        sleDepartment.Properties.ValueMember = "Id"
        sleDepartment.Properties.DisplayMember = "Name"
        sleDepartment.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadPositions(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadPositions
        slePosition.Properties.DataSource = data
        slePosition.Properties.ValueMember = "Id"
        slePosition.Properties.DisplayMember = "Name"
        slePosition.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadCategories(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadCategories
        sleCategoryCode.Properties.DataSource = data
        sleCategoryCode.Properties.ValueMember = "Id"
        sleCategoryCode.Properties.DisplayMember = "Name"
        sleCategoryCode.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadJobClasses(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadJobClasses
        sleJobClass.Properties.DataSource = data
        sleJobClass.Properties.ValueMember = "Id"
        sleJobClass.Properties.DisplayMember = "Name"
        sleJobClass.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadLeaveGroups(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadLeaveGroups
        sleLeaveGroup.Properties.DataSource = data
        sleLeaveGroup.Properties.ValueMember = "Id"
        sleLeaveGroup.Properties.DisplayMember = "Name"
        sleLeaveGroup.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadScheduleGroups(data As List(Of GlobalShared.Models.LookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadScheduleGroups
        sleScheduleGroup.Properties.DataSource = data
        sleScheduleGroup.Properties.ValueMember = "Id"
        sleScheduleGroup.Properties.DisplayMember = "Name"
        sleScheduleGroup.ConfigureLookupColumns(("Code", "Code"), ("Name", "Name"))
    End Sub

    Public Sub LoadSuperiors(data As List(Of GlobalShared.Models.EmployeeLookupModel)) _
    Implements Employee.Views.IEmployeesEmploymentView.LoadSuperiors

        sleSuperior.Properties.DataSource = data
        sleSuperior.Properties.ValueMember = "RecordId"
        sleSuperior.Properties.DisplayMember = "DisplayText"
        sleSuperior.ConfigureLookupColumns(
        ("EmployeeNo", "Employee No."),
        ("FullName", "Full Name"))

    End Sub

    Public Sub ClearFields() Implements Employee.Views.IEmployeesEmploymentView.ClearFields
        cboEmployeeStatus.Text = String.Empty
        cboEmploymentType.Text = String.Empty
        sleBranch.EditValue = Nothing
        sleDepartment.EditValue = Nothing
        slePosition.EditValue = Nothing
        sleCategoryCode.EditValue = Nothing
        sleJobClass.EditValue = Nothing
        sleLeaveGroup.EditValue = Nothing
        sleSuperior.EditValue = Nothing
        sleScheduleGroup.EditValue = Nothing
        dtpDateJoined.EditValue = Nothing
        dtpDateRegularization.EditValue = Nothing
        dtpDateLastPromoted.EditValue = Nothing
        dtpDateResigned.EditValue = Nothing
        dtpDateRetired.EditValue = Nothing
        dtpDateTerminated.EditValue = Nothing
        txtControlNo.Text = String.Empty
    End Sub

    ' =============================================
    ' IBaseView
    ' =============================================
    Public Sub ShowLoading() Implements IBaseView.ShowLoading
        lcEmployment.Enabled = False
    End Sub

    Public Sub HideLoading() Implements IBaseView.HideLoading
        lcEmployment.Enabled = True
    End Sub

    Public Sub ShowError(msg As String) Implements IBaseView.ShowError
        DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub ShowMessage(msg As String) Implements IBaseView.ShowMessage
        DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Information",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =============================================
    ' EVENTS
    ' =============================================
    Public Event OnSave As EventHandler _
        Implements Employee.Views.IEmployeesEmploymentView.OnSave
    Public Event OnNew As EventHandler _
        Implements Employee.Views.IEmployeesEmploymentView.OnNew
    Public Event OnSaveCompleted As EventHandler _
        Implements Employee.Views.IEmployeesEmploymentView.OnSaveCompleted

End Class