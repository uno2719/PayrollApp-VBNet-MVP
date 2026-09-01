Public Class ucEmployeesStatutory
    Inherits DevExpress.XtraEditors.XtraUserControl
    Implements Employee.Views.IEmployeesStatutoryView

    Private _presenter As Employee.Presenters.EmployeesStatutoryPresenter

    Public Sub New()
        InitializeComponent()
        'Dim service = New Employee.Services.EmployeeService()
        '_presenter = New Employee.Presenters.EmployeesStatutoryPresenter(Me, service)
        'AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
    End Sub

    Public Sub SetPresenter(presenter As Employee.Presenters.EmployeesStatutoryPresenter)
        _presenter = presenter
        AddHandler _presenter.OnSaveCompleted, AddressOf RelayOnSaveCompleted
    End Sub

    ' --- Tawagin mula sa ucEmployees ---
    Public Sub LoadEmployee(recordId As Integer)
        _presenter.LoadEmployee(recordId)
    End Sub

    Public Sub RaiseNew(recordId As Integer)
        Me.RecordId = recordId
        RaiseEvent OnNew(Me, EventArgs.Empty)
    End Sub

    Public Sub RaiseSave()
        RaiseEvent OnSave(Me, EventArgs.Empty)
    End Sub

    Public Sub SetEditMode(enabled As Boolean)
        txtTIN.Properties.ReadOnly = Not enabled
        txtSSSNo.Properties.ReadOnly = Not enabled
        txtPagIBIGNo.Properties.ReadOnly = Not enabled
        txtPagIBIGVoluntary.Properties.ReadOnly = Not enabled
        txtPhilHealthNo.Properties.ReadOnly = Not enabled
        txtFixTax.Properties.ReadOnly = Not enabled
    End Sub

    Public Sub ClearFields() Implements Employee.Views.IEmployeesStatutoryView.ClearFields
        txtTIN.Text = String.Empty
        txtSSSNo.Text = String.Empty
        txtPagIBIGNo.Text = String.Empty
        txtPagIBIGVoluntary.Text = String.Empty
        txtPhilHealthNo.Text = String.Empty
        txtFixTax.Text = String.Empty
    End Sub

    Private Sub RelayOnSaveCompleted(sender As Object, e As EventArgs)
        RaiseEvent OnSaveCompleted(Me, EventArgs.Empty)
    End Sub

    ' =============================================
    ' INTERFACE PROPERTIES
    ' =============================================
    Public Property RecordId As Integer _
        Implements Employee.Views.IEmployeesStatutoryView.RecordId

    Public Property TIN As String _
        Implements Employee.Views.IEmployeesStatutoryView.TIN
        Get
            Return txtTIN.Text
        End Get
        Set(value As String)
            txtTIN.Text = value
        End Set
    End Property

    Public Property SSSNo As String _
        Implements Employee.Views.IEmployeesStatutoryView.SSSNo
        Get
            Return txtSSSNo.Text
        End Get
        Set(value As String)
            txtSSSNo.Text = value
        End Set
    End Property

    Public Property PagIBIGNo As String _
        Implements Employee.Views.IEmployeesStatutoryView.PagIBIGNo
        Get
            Return txtPagIBIGNo.Text
        End Get
        Set(value As String)
            txtPagIBIGNo.Text = value
        End Set
    End Property

    Public Property PagIBIGVoluntary As Decimal? _
        Implements Employee.Views.IEmployeesStatutoryView.PagIBIGVoluntary
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtPagIBIGVoluntary.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtPagIBIGVoluntary.Text = If(value.HasValue,
                                          value.Value.ToString("n2"),
                                          String.Empty)
        End Set
    End Property

    Public Property PhilHealthNo As String _
        Implements Employee.Views.IEmployeesStatutoryView.PhilHealthNo
        Get
            Return txtPhilHealthNo.Text
        End Get
        Set(value As String)
            txtPhilHealthNo.Text = value
        End Set
    End Property

    Public Property FixTax As Decimal? _
        Implements Employee.Views.IEmployeesStatutoryView.FixTax
        Get
            Dim result As Decimal
            If Decimal.TryParse(txtFixTax.Text, result) Then Return result
            Return Nothing
        End Get
        Set(value As Decimal?)
            txtFixTax.Text = If(value.HasValue,
                                value.Value.ToString("n2"),
                                String.Empty)
        End Set
    End Property

    ' =============================================
    ' IBaseView
    ' =============================================
    Public Sub ShowLoading() Implements IBaseView.ShowLoading
        lcStatutory.Enabled = False
    End Sub

    Public Sub HideLoading() Implements IBaseView.HideLoading
        lcStatutory.Enabled = True
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
        Implements Employee.Views.IEmployeesStatutoryView.OnSave
    Public Event OnNew As EventHandler _
        Implements Employee.Views.IEmployeesStatutoryView.OnNew
    Public Event OnSaveCompleted As EventHandler _
        Implements Employee.Views.IEmployeesStatutoryView.OnSaveCompleted

End Class