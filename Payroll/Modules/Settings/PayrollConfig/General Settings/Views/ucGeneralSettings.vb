' File: Modules/Settings/SysConfig/GeneralSettings/Views/ucGeneralSettings.vb
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports Payroll.GeneralSettings.Presenters
Imports Payroll.GeneralSettings.Views
Imports Payroll.GlobalShared.Models

Public Class ucGeneralSettings
    Implements IGeneralSettingsView, IAsyncLoadable

    Private _presenter As GeneralSettingsPresenter

    Public Sub SetPresenter(presenter As GeneralSettingsPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "General Settings"
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        ' Ang dalawang static dropdowns ay pinupunan dito (hindi sa
        ' Designer) para nasa isang lugar lang ang listahan sa code -
        ' parehong ginawa natin sa cboLoanType sa ucLoan.
        cboSSSBasedOn.Properties.Items.Clear()
        cboSSSBasedOn.Properties.Items.AddRange({"Progressive Earnings", "Fixed Basic"})

        cboPhilHealthBasedOn.Properties.Items.Clear()
        cboPhilHealthBasedOn.Properties.Items.AddRange({"Progressive Earnings", "Fixed Basic"})

        Await _presenter.LoadAsync()

        btnSave.Enabled = HasEditAccess
    End Function

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_General
        End Get
    End Property

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Await _presenter.SaveAsync()
    End Sub

    ' === Helpers ===

    ' Hindi pwede ang TryCast sa Nullable(Of Integer) dahil value type
    ' siya - ito ang ginagamit natin sa halip (kapareho ng ucCompany).
    Private Shared Function ToNullableInt(value As Object) As Integer?
        If value Is Nothing OrElse IsDBNull(value) Then
            Return Nothing
        End If
        Return Convert.ToInt32(value)
    End Function

    Private Shared Function ToDecimal(text As String) As Decimal
        Dim parsed As Decimal
        Return If(Decimal.TryParse(text, parsed), parsed, 0D)
    End Function

    Private Shared Function ToInteger(text As String) As Integer
        Dim parsed As Integer
        Return If(Integer.TryParse(text, parsed), parsed, 0)
    End Function

    ' Iisang hitsura ang lahat ng code dropdowns - dito naka-sentro para
    ' hindi paulit-ulit ang parehong 8 linya nang limang beses.
    Private Shared Sub BindCodeLookup(editor As LookUpEdit, source As List(Of LookupModel))
        With editor.Properties
            .DataSource = source
            .DisplayMember = "Code"
            .ValueMember = "Id"
            .NullText = "[None]"
            .Columns.Clear()
            .Columns.Add(New LookUpColumnInfo("Code", "Code", 90))
            .Columns.Add(New LookUpColumnInfo("Name", "Description", 220))
        End With
    End Sub

    ' === IGeneralSettingsView : Payroll Parameters ===

    Public Property BonusCeiling As Decimal Implements IGeneralSettingsView.BonusCeiling
        Get
            Return ToDecimal(txtBonusCeiling.Text)
        End Get
        Set(value As Decimal)
            txtBonusCeiling.Text = value.ToString("0.00")
        End Set
    End Property

    Public Property TotalDaysPerYear As Integer Implements IGeneralSettingsView.TotalDaysPerYear
        Get
            Return ToInteger(txtTotalDaysPerYear.Text)
        End Get
        Set(value As Integer)
            txtTotalDaysPerYear.Text = value.ToString()
        End Set
    End Property

    Public Property WorkHourPerDay As Decimal Implements IGeneralSettingsView.WorkHourPerDay
        Get
            Return ToDecimal(txtWorkHourPerDay.Text)
        End Get
        Set(value As Decimal)
            txtWorkHourPerDay.Text = value.ToString("0.00")
        End Set
    End Property

    Public Property AmountPrecision As Integer Implements IGeneralSettingsView.AmountPrecision
        Get
            Return ToInteger(txtAmountPrecision.Text)
        End Get
        Set(value As Integer)
            txtAmountPrecision.Text = value.ToString()
        End Set
    End Property

    Public Property PercentPrecision As Integer Implements IGeneralSettingsView.PercentPrecision
        Get
            Return ToInteger(txtPercentPrecision.Text)
        End Get
        Set(value As Integer)
            txtPercentPrecision.Text = value.ToString()
        End Set
    End Property

    ' === IGeneralSettingsView : Code Mapping ===

    Public Property BasicSalaryCodePlusId As Integer? Implements IGeneralSettingsView.BasicSalaryCodePlusId
        Get
            Return ToNullableInt(lookupBasicSalaryCodePlus.EditValue)
        End Get
        Set(value As Integer?)
            lookupBasicSalaryCodePlus.EditValue = value
        End Set
    End Property

    Public Property BasicSalaryCodeMinusId As Integer? Implements IGeneralSettingsView.BasicSalaryCodeMinusId
        Get
            Return ToNullableInt(lookupBasicSalaryCodeMinus.EditValue)
        End Get
        Set(value As Integer?)
            lookupBasicSalaryCodeMinus.EditValue = value
        End Set
    End Property

    Public Property AbsentCodeId As Integer? Implements IGeneralSettingsView.AbsentCodeId
        Get
            Return ToNullableInt(lookupAbsentCode.EditValue)
        End Get
        Set(value As Integer?)
            lookupAbsentCode.EditValue = value
        End Set
    End Property

    Public Property LateInCodeId As Integer? Implements IGeneralSettingsView.LateInCodeId
        Get
            Return ToNullableInt(lookupLateInCode.EditValue)
        End Get
        Set(value As Integer?)
            lookupLateInCode.EditValue = value
        End Set
    End Property

    Public Property EarlyOutCodeId As Integer? Implements IGeneralSettingsView.EarlyOutCodeId
        Get
            Return ToNullableInt(lookupEarlyOutCode.EditValue)
        End Get
        Set(value As Integer?)
            lookupEarlyOutCode.EditValue = value
        End Set
    End Property

    ' === IGeneralSettingsView : Statutory Basis ===

    Public Property SSSBasedOn As String Implements IGeneralSettingsView.SSSBasedOn
        Get
            Return cboSSSBasedOn.Text
        End Get
        Set(value As String)
            cboSSSBasedOn.Text = value
        End Set
    End Property

    Public Property PhilHealthBasedOn As String Implements IGeneralSettingsView.PhilHealthBasedOn
        Get
            Return cboPhilHealthBasedOn.Text
        End Get
        Set(value As String)
            cboPhilHealthBasedOn.Text = value
        End Set
    End Property

    ' === IGeneralSettingsView : Lookups ===

    Public Sub SetCompensationCodes(codes As List(Of LookupModel)) Implements IGeneralSettingsView.SetCompensationCodes
        BindCodeLookup(lookupBasicSalaryCodePlus, codes)
    End Sub

    Public Sub SetDeductionCodes(codes As List(Of LookupModel)) Implements IGeneralSettingsView.SetDeductionCodes
        ' Iisang listahan lang ang binibind sa apat na dropdown - read-only
        ' binding ito, kaya walang problema sa pagbabahagi ng parehong List.
        BindCodeLookup(lookupBasicSalaryCodeMinus, codes)
        BindCodeLookup(lookupAbsentCode, codes)
        BindCodeLookup(lookupLateInCode, codes)
        BindCodeLookup(lookupEarlyOutCode, codes)
    End Sub

    ' === IGeneralSettingsView : Messages ===

    Public Sub ShowMessage(message As String) Implements IGeneralSettingsView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements IGeneralSettingsView.ShowError
        MyBase.ShowError(message)
    End Sub

End Class