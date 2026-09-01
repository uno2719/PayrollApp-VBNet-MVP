Imports DevExpress.XtraEditors
Imports Payroll.Employee.Presenters
Imports Payroll.Employee.Views


Public Class ucEmployeesPersonalInfo
    Implements Employee.Views.IEmployeesPersonalInfoView

    Private _presenter As Employee.Presenters.EmployeesPersonalInfoPresenter

    Public Sub New()
        InitializeComponent()

    End Sub

    Public Sub SetPresenter(presenter As Employee.Presenters.EmployeesPersonalInfoPresenter)
        _presenter = presenter
        AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
    End Sub


    Private Sub RelayOnSaveCompleted(sender As Object, e As EventArgs)
        RaiseEvent OnSaveCompleted(Me, EventArgs.Empty)
    End Sub

    ' --- Tawagin mula sa ucEmployees ---
    Public Sub LoadEmployee(recordId As Integer)
        _presenter.LoadEmployee(recordId)
    End Sub

    ' I-dagdag sa ucEmployeesPersonalInfo.vb

    Public Sub RaiseSave()
        RaiseEvent OnSave(Me, EventArgs.Empty)
    End Sub

    Public Sub RaiseNew()
        RestoreValidation()
        RaiseEvent OnNew(Me, EventArgs.Empty)
    End Sub

    Public Sub SetEditMode(enabled As Boolean)
        cboGender.Properties.ReadOnly = Not enabled
        cboMaritalStatus.Properties.ReadOnly = Not enabled
        cboReligion.Properties.ReadOnly = Not enabled
        chkSameAddress.Properties.ReadOnly = Not enabled
        dtpBirthdate.Properties.ReadOnly = Not enabled
        txtBirthPlace.Properties.ReadOnly = Not enabled
        txtCitizenship.Properties.ReadOnly = Not enabled
        txtContactNo.Properties.ReadOnly = Not enabled
        txtEmailAdd.Properties.ReadOnly = Not enabled
        txtEmpNo.Properties.ReadOnly = Not enabled
        txtFirstName.Properties.ReadOnly = Not enabled
        txtLastName.Properties.ReadOnly = Not enabled
        txtMailAddress1.Properties.ReadOnly = Not enabled
        txtMailAddress2.Properties.ReadOnly = Not enabled
        txtMailZipCode.Properties.ReadOnly = Not enabled
        txtMiddleName.Properties.ReadOnly = Not enabled
        txtPermanentAddress1.Properties.ReadOnly = Not enabled
        txtPermanentAddress2.Properties.ReadOnly = Not enabled
        txtPermanentZipCode.Properties.ReadOnly = Not enabled
        txtSuffix.Properties.ReadOnly = Not enabled
    End Sub


    Public Sub ClearValidation()
        DxValidationProvider1.SetValidationRule(txtEmpNo, Nothing)
        txtEmpNo.ErrorText = String.Empty
    End Sub
    Public Sub RestoreValidation()
        Dim rule As New DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule()
        rule.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank
        rule.ErrorText = "Please enter a valid Employee No."
        rule.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning
        DxValidationProvider1.SetValidationRule(txtEmpNo, rule)
    End Sub


    ' =============================================
    ' INTERFACE PROPERTIES
    ' =============================================
    Public Property RecordId As Integer _
        Implements Employee.Views.IEmployeesPersonalInfoView.RecordId
        Get
            Dim result As Integer
            ' ✅ I-store ang parsed value sa result
            If Integer.TryParse(txtRecordID.Text, result) Then
                Return result
            End If
            Return 0
        End Get
        Set(value As Integer)
            txtRecordID.Text = If(value = 0, String.Empty, value.ToString())
        End Set
    End Property

    Public Property EmployeeNo As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.EmployeeNo
        Get
            Return txtEmpNo.Text
        End Get
        Set(value As String)
            txtEmpNo.Text = value
        End Set
    End Property

    Public Property FirstName As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.FirstName
        Get
            Return txtFirstName.Text
        End Get
        Set(value As String)
            txtFirstName.Text = value
        End Set
    End Property

    Public Property MiddleName As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.MiddleName
        Get
            Return txtMiddleName.Text
        End Get
        Set(value As String)
            txtMiddleName.Text = value
        End Set
    End Property

    Public Property LastName As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.LastName
        Get
            Return txtLastName.Text
        End Get
        Set(value As String)
            txtLastName.Text = value
        End Set
    End Property

    Public Property Suffix As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.Suffix
        Get
            Return txtSuffix.Text
        End Get
        Set(value As String)
            txtSuffix.Text = value
        End Set
    End Property

    Public Property EmailAddress As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.EmailAddress
        Get
            Return txtEmailAdd.Text
        End Get
        Set(value As String)
            txtEmailAdd.Text = value
        End Set
    End Property

    Public Property ContactNo As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.ContactNo
        Get
            Return txtContactNo.Text
        End Get
        Set(value As String)
            txtContactNo.Text = value
        End Set
    End Property

    Public Property BirthDate As DateTimeOffset? _
        Implements Employee.Views.IEmployeesPersonalInfoView.BirthDate
        Get
            Return dtpBirthdate.EditValue
        End Get
        Set(value As DateTimeOffset?)
            dtpBirthdate.EditValue = If(value.HasValue, value, Nothing)
        End Set
    End Property

    Public Property BirthPlace As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.BirthPlace
        Get
            Return txtBirthPlace.Text
        End Get
        Set(value As String)
            txtBirthPlace.Text = value
        End Set
    End Property

    Public Property Gender As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.Gender
        Get
            Return cboGender.Text
        End Get
        Set(value As String)
            cboGender.Text = value
        End Set
    End Property

    Public Property CivilStatus As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.CivilStatus
        Get
            Return cboMaritalStatus.Text
        End Get
        Set(value As String)
            cboMaritalStatus.Text = value
        End Set
    End Property

    Public Property Religion As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.Religion
        Get
            Return cboReligion.Text
        End Get
        Set(value As String)
            cboReligion.Text = value
        End Set
    End Property

    Public Property Citizenship As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.Citizenship
        Get
            Return txtCitizenship.Text
        End Get
        Set(value As String)
            txtCitizenship.Text = value
        End Set
    End Property

    Public Property MailAddress1 As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.MailAddress1
        Get
            Return txtMailAddress1.Text
        End Get
        Set(value As String)
            txtMailAddress1.Text = value
        End Set
    End Property

    Public Property MailAddress2 As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.MailAddress2
        Get
            Return txtMailAddress2.Text
        End Get
        Set(value As String)
            txtMailAddress2.Text = value
        End Set
    End Property

    Public Property MailZipCode As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.MailZipCode
        Get
            Return txtMailZipCode.Text
        End Get
        Set(value As String)
            txtMailZipCode.Text = value
        End Set
    End Property

    Public Property PermanentAddress1 As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.PermanentAddress1
        Get
            Return txtPermanentAddress1.Text
        End Get
        Set(value As String)
            txtPermanentAddress1.Text = value
        End Set
    End Property

    Public Property PermanentAddress2 As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.PermanentAddress2
        Get
            Return txtPermanentAddress2.Text
        End Get
        Set(value As String)
            txtPermanentAddress2.Text = value
        End Set
    End Property

    Public Property PermanentZipCode As String _
        Implements Employee.Views.IEmployeesPersonalInfoView.PermanentZipCode
        Get
            Return txtPermanentZipCode.Text
        End Get
        Set(value As String)
            txtPermanentZipCode.Text = value
        End Set
    End Property

    ' =============================================
    ' IBaseView
    ' =============================================
    Public Sub ShowLoading() Implements IBaseView.ShowLoading
        lcPersonalInfo.Enabled = False
    End Sub

    Public Sub HideLoading() Implements IBaseView.HideLoading
        lcPersonalInfo.Enabled = True
    End Sub

    Public Sub ShowError(msg As String) Implements IBaseView.ShowError
        XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub ShowMessage(msg As String) Implements IBaseView.ShowMessage
        XtraMessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' =============================================
    ' EVENTS
    ' =============================================
    Public Event OnSave As EventHandler _
        Implements Employee.Views.IEmployeesPersonalInfoView.OnSave
    Public Event OnDelete As EventHandler _
        Implements Employee.Views.IEmployeesPersonalInfoView.OnDelete
    Public Event OnNew As EventHandler _
        Implements Employee.Views.IEmployeesPersonalInfoView.OnNew
    Public Event OnSaveCompleted As EventHandler _
        Implements Employee.Views.IEmployeesPersonalInfoView.OnSaveCompleted

    ' =============================================
    ' EXISTING LOGIC
    ' =============================================
    Private Sub chkSameAddress_CheckedChanged(sender As Object, e As EventArgs) _
        Handles chkSameAddress.CheckedChanged

        txtPermanentAddress1.Clear()
        txtPermanentAddress2.Clear()
        txtPermanentZipCode.Clear()

        If DirectCast(sender, CheckEdit).Checked Then
            txtPermanentAddress1.Text = txtMailAddress1.Text
            txtPermanentAddress2.Text = txtMailAddress2.Text
            txtPermanentZipCode.Text = txtMailZipCode.Text
        End If

    End Sub

    Public Sub ClearFields() Implements Employee.Views.IEmployeesPersonalInfoView.ClearFields
        txtEmpNo.Text = String.Empty
        txtFirstName.Text = String.Empty
        txtMiddleName.Text = String.Empty
        txtLastName.Text = String.Empty
        txtSuffix.Text = String.Empty
        txtEmailAdd.Text = String.Empty
        txtContactNo.Text = String.Empty
        dtpBirthdate.EditValue = Nothing
        txtBirthPlace.Text = String.Empty
        cboGender.Text = String.Empty
        cboMaritalStatus.Text = String.Empty
        cboReligion.Text = String.Empty
        txtCitizenship.Text = String.Empty
        txtMailAddress1.Text = String.Empty
        txtMailAddress2.Text = String.Empty
        txtMailZipCode.Text = String.Empty
        txtPermanentAddress1.Text = String.Empty
        txtPermanentAddress2.Text = String.Empty
        txtPermanentZipCode.Text = String.Empty
        txtRecordID.Text = String.Empty
    End Sub


End Class


