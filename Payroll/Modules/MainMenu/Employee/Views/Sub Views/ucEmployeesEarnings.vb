Imports Payroll.Employee.Views

Public Class ucEmployeesEarnings
    Inherits DevExpress.XtraEditors.XtraUserControl
    Implements Employee.Views.IEmployeesEarningsView

    Private _presenter As Employee.Presenters.EmployeesEarningsPresenter
    Private _selectedRecordId As Integer = 0

    Public Sub New()
        InitializeComponent()
        'Dim service = New Employee.Services.EmployeeService()
        '_presenter = New Employee.Presenters.EmployeesEarningsPresenter(Me, service)
        'AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
    End Sub

    Public Sub SetPresenter(presenter As Employee.Presenters.EmployeesEarningsPresenter)
        _presenter = presenter
        AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
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
        txtBasicSalary.Properties.ReadOnly = Not enabled
        txtDailyRate.Properties.ReadOnly = Not enabled
        txtHourlyRate.Properties.ReadOnly = Not enabled
        txtDaysInYear.Properties.ReadOnly = Not enabled
        txtWorkHourPer.Properties.ReadOnly = Not enabled
        chkPayrollFlag.Properties.ReadOnly = Not enabled
        chkMinimumWage.Properties.ReadOnly = Not enabled
        cboPayCycle.Properties.ReadOnly = Not enabled
        cboTaxFlag.Properties.ReadOnly = Not enabled
        cboPayBy.Properties.ReadOnly = Not enabled
        sleBank.Properties.ReadOnly = Not enabled
        txtBankAccount.Properties.ReadOnly = Not enabled
    End Sub

    Public Sub ClearFields() Implements Employee.Views.IEmployeesEarningsView.ClearFields
        txtBasicSalary.Text = String.Empty
        txtDailyRate.Text = String.Empty
        txtHourlyRate.Text = String.Empty
        txtDaysInYear.Text = String.Empty
        txtWorkHourPer.Text = String.Empty
        chkPayrollFlag.Checked = False
        chkMinimumWage.Checked = False
        cboPayCycle.Text = String.Empty
        cboTaxFlag.Text = String.Empty
        cboPayBy.Text = String.Empty
        sleBank.EditValue = Nothing
        txtBankAccount.Text = String.Empty
    End Sub

    Private Sub RelayOnSaveCompleted(sender As Object, e As EventArgs)
        RaiseEvent OnSaveCompleted(Me, EventArgs.Empty)
    End Sub


    ' =============================================
    ' AUTO-COMPUTE
    ' =============================================
    Private Sub ComputeRates()
        Dim basicSalary As Decimal
        Dim daysInYear As Integer
        Dim workHourPer As Decimal

        If Not Decimal.TryParse(txtBasicSalary.Text, basicSalary) Then Return
        If Not Integer.TryParse(txtDaysInYear.Text, daysInYear) OrElse daysInYear = 0 Then Return
        If Not Decimal.TryParse(txtWorkHourPer.Text, workHourPer) OrElse workHourPer = 0 Then Return

        Dim dailyRate = Math.Round(basicSalary * 12 / daysInYear, 2)
        Dim hourlyRate = Math.Round(dailyRate / workHourPer, 2)

        txtDailyRate.Text = dailyRate.ToString("n2")
        txtHourlyRate.Text = hourlyRate.ToString("n2")
    End Sub

    Private Sub txtBasicSalary_EditValueChanged(sender As Object, e As EventArgs) _
        Handles txtBasicSalary.EditValueChanged
        ComputeRates()
    End Sub

    Private Sub txtDaysInYear_EditValueChanged(sender As Object, e As EventArgs) _
        Handles txtDaysInYear.EditValueChanged
        ComputeRates()
    End Sub

    Private Sub txtWorkHourPer_EditValueChanged(sender As Object, e As EventArgs) _
        Handles txtWorkHourPer.EditValueChanged
        ComputeRates()
    End Sub


    ' =============================================
    ' INTERFACE PROPERTIES
    ' =============================================
    Public Property RecordId As Integer _
        Implements Employee.Views.IEmployeesEarningsView.RecordId

    Public Property BasicSalary As Decimal? _
        Implements Employee.Views.IEmployeesEarningsView.BasicSalary
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtBasicSalary.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtBasicSalary.Text = If(value.HasValue, value.Value.ToString("n2"), String.Empty)
        End Set
    End Property

    Public Property DailyRate As Decimal? _
        Implements Employee.Views.IEmployeesEarningsView.DailyRate
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtDailyRate.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtDailyRate.Text = If(value.HasValue, value.Value.ToString("n2"), String.Empty)
        End Set
    End Property

    Public Property HourlyRate As Decimal? _
        Implements Employee.Views.IEmployeesEarningsView.HourlyRate
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtHourlyRate.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtHourlyRate.Text = If(value.HasValue, value.Value.ToString("n2"), String.Empty)
        End Set
    End Property

    Public Property DaysInYear As Integer? _
        Implements Employee.Views.IEmployeesEarningsView.DaysInYear
        Get
            Dim result As Integer
            If Integer.TryParse(txtDaysInYear.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Integer?)
            txtDaysInYear.Text = If(value.HasValue, value.Value.ToString(), String.Empty)
        End Set
    End Property

    Public Property WorkHourPer As Decimal? _
        Implements Employee.Views.IEmployeesEarningsView.WorkHourPer
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtWorkHourPer.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtWorkHourPer.Text = If(value.HasValue, value.Value.ToString("f3"), String.Empty)
        End Set
    End Property

    Public Property PayrollFlag As Boolean _
        Implements Employee.Views.IEmployeesEarningsView.PayrollFlag
        Get
            Return chkPayrollFlag.Checked
        End Get
        Set(value As Boolean)
            chkPayrollFlag.Checked = value
        End Set
    End Property

    Public Property MinimumWage As Boolean _
        Implements Employee.Views.IEmployeesEarningsView.MinimumWage
        Get
            Return chkMinimumWage.Checked
        End Get
        Set(value As Boolean)
            chkMinimumWage.Checked = value
        End Set
    End Property

    Public Property PayCycle As String _
        Implements Employee.Views.IEmployeesEarningsView.PayCycle
        Get
            Return cboPayCycle.Text
        End Get
        Set(value As String)
            cboPayCycle.Text = value
        End Set
    End Property

    Public Property TaxFlag As String _
        Implements Employee.Views.IEmployeesEarningsView.TaxFlag
        Get
            Return cboTaxFlag.Text
        End Get
        Set(value As String)
            cboTaxFlag.Text = value
        End Set
    End Property

    Public Property PayBy As String _
        Implements Employee.Views.IEmployeesEarningsView.PayBy
        Get
            Return cboPayBy.Text
        End Get
        Set(value As String)
            cboPayBy.Text = value
        End Set
    End Property

    Public Property BankId As Integer? _
        Implements Employee.Views.IEmployeesEarningsView.BankId
        Get
            If sleBank.EditValue Is Nothing OrElse
               sleBank.EditValue Is DBNull.Value Then Return Nothing
            Return CType(sleBank.EditValue, Integer)
        End Get
        Set(value As Integer?)
            sleBank.EditValue = If(value.HasValue, CObj(value.Value), Nothing)
        End Set
    End Property

    Public Property BankAccount As String _
        Implements Employee.Views.IEmployeesEarningsView.BankAccount
        Get
            Return txtBankAccount.Text
        End Get
        Set(value As String)
            txtBankAccount.Text = value
        End Set
    End Property


    ' =============================================
    ' LOOKUP LOADER
    ' =============================================
    Private _banksLoaded As Boolean = False
    Public Sub LoadBanks(data As List(Of GlobalShared.Models.LookupModel)) _
        Implements Employee.Views.IEmployeesEarningsView.LoadBanks

        ' ✅ I-load ang DataSource ONCE lang
        If Not _banksLoaded Then
            sleBank.Properties.DataSource = data
            sleBank.Properties.ValueMember = "Id"
            sleBank.Properties.DisplayMember = "Name"
            _banksLoaded = True
        End If

    End Sub


    ' =============================================
    ' IBaseView
    ' =============================================
    Public Sub ShowLoading() Implements IBaseView.ShowLoading
        lcEarnings.Enabled = False
    End Sub

    Public Sub HideLoading() Implements IBaseView.HideLoading
        lcEarnings.Enabled = True
    End Sub

    Public Sub ShowError(msg As String) Implements IBaseView.ShowError
        DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub ShowMessage(msg As String) Implements IBaseView.ShowMessage
        DevExpress.XtraEditors.XtraMessageBox.Show(msg, "Information",
            MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'Private Sub IEmployeesEarningsView_ClearFields() Implements IEmployeesEarningsView.ClearFields
    '    ClearFields()
    'End Sub

    Private Sub cboPayBy_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboPayBy.SelectedIndexChanged
        sleBank.Enabled = cboPayBy.SelectedIndex = 0
        txtBankAccount.Enabled = cboPayBy.SelectedIndex = 0
        If cboPayBy.SelectedIndex > 0 Then
            sleBank.EditValue = Nothing
            txtBankAccount.Clear()
        End If
    End Sub

    ' =============================================
    ' EVENTS
    ' =============================================
    Public Event OnSave As EventHandler _
        Implements Employee.Views.IEmployeesEarningsView.OnSave
    Public Event OnNew As EventHandler _
        Implements Employee.Views.IEmployeesEarningsView.OnNew
    Public Event OnSaveCompleted As EventHandler _
        Implements Employee.Views.IEmployeesEarningsView.OnSaveCompleted

End Class