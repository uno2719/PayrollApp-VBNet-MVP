Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

Public Class ucCutoff
    Implements ICutoffMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As CutoffPresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False

    ' Same indexing scheme as ucPayrollRateEntry's BTN_NEW/BTN_EDIT/BTN_REFRESH —
    ' matches wbpMainCommands.Buttons.AddRange order in the Designer.
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_GENERATE As Integer = 3
    Private Const BTN_REFRESH As Integer = 5

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub SetPresenter(presenter As CutoffPresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Settings > Cutoff"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Cutoff"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Settings_CutOff
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _presenter.LoadAsync()
    End Function

    ' =============================================
    ' BUTTON COMMANDS — same Tag-dispatch + toggle pattern as ucPayrollRateEntry
    ' =============================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Select Case tag
            Case "New"
                If _isEditing Then
                    Await _presenter.SaveAsync()
                Else
                    _presenter.StartNew()
                End If

            Case "Edit"
                If _isEditing Then
                    _presenter.CancelEdit()
                Else
                    _presenter.StartEdit()
                End If

            Case "Generate"
                If Not _isEditing Then
                    Await _presenter.GenerateForYearAsync()
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If
        End Select
    End Sub

    Private Sub gridView_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridView.FocusedRowChanged

        Dim item = TryCast(gridView.GetRow(e.FocusedRowHandle), CutoffModel)
        If item IsNot Nothing Then
            _presenter.SelectItem(item.CutoffID)
        End If
    End Sub

#Region "ICutoffMaintenanceView"

    Public Property CycleType As String Implements ICutoffMaintenanceView.CycleType
        Get
            Return CStr(If(cboCycleType.EditValue, ""))
        End Get
        Set(value As String)
            cboCycleType.EditValue = value
        End Set
    End Property

    Public Property CutoffYear As Integer Implements ICutoffMaintenanceView.CutoffYear
        Get
            Return CInt(numCutoffYear.Value)
        End Get
        Set(value As Integer)
            numCutoffYear.Value = value
        End Set
    End Property

    Public Property CutoffStart As Date Implements ICutoffMaintenanceView.CutoffStart
        Get
            Return dateCutoffStart.DateTime
        End Get
        Set(value As Date)
            dateCutoffStart.DateTime = value
        End Set
    End Property

    Public Property CutoffEnd As Date Implements ICutoffMaintenanceView.CutoffEnd
        Get
            Return dateCutoffEnd.DateTime
        End Get
        Set(value As Date)
            dateCutoffEnd.DateTime = value
        End Set
    End Property

    Public Property PayDate As Date? Implements ICutoffMaintenanceView.PayDate
        Get
            If datePayDate.EditValue Is Nothing OrElse datePayDate.EditValue Is DBNull.Value Then Return Nothing
            Return datePayDate.DateTime
        End Get
        Set(value As Date?)
            datePayDate.EditValue = If(value.HasValue, CType(value.Value, Object), Nothing)
        End Set
    End Property

    Public Property CutoffLabel As String Implements ICutoffMaintenanceView.CutoffLabel
        Get
            Return txtCutoffLabel.Text
        End Get
        Set(value As String)
            txtCutoffLabel.Text = value
        End Set
    End Property

    Public Property Status As CutoffStatus Implements ICutoffMaintenanceView.Status
        Get
            Return [Enum].Parse(GetType(CutoffStatus), lblStatusValue.Text)
        End Get
        Set(value As CutoffStatus)
            lblStatusValue.Text = value.ToString()
        End Set
    End Property

    Public Sub BindList(items As List(Of CutoffModel)) Implements ICutoffMaintenanceView.BindList
        gridControl.DataSource = items
    End Sub

    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) Implements ICutoffMaintenanceView.SetFormMode
        _isEditing = isEditable
        _isNewRecord = isNewRecord

        grpDetails.Enabled = isEditable
        gridControl.Enabled = Not isEditable

        If isEditable Then
            If isNewRecord Then
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Save"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Save New Cutoff"
            Else
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Cutoff"
            End If
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Cutoff"
        End If

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

        wbpMainCommands.Buttons.Item(BTN_GENERATE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable
    End Sub

    Public Sub ClearFields() Implements ICutoffMaintenanceView.ClearFields
        cboCycleType.EditValue = Nothing
        numCutoffYear.Value = Date.Today.Year
        dateCutoffStart.EditValue = Nothing
        dateCutoffEnd.EditValue = Nothing
        datePayDate.EditValue = Nothing
        txtCutoffLabel.Text = ""
        lblStatusValue.Text = CutoffStatus.Draft.ToString()
    End Sub

    Public Sub ShowMessage(message As String) Implements ICutoffMaintenanceView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements ICutoffMaintenanceView.ShowError
        MyBase.ShowError(message)
    End Sub

    Public Function ConfirmGenerate(cycleType As String, year As Integer) As Boolean Implements ICutoffMaintenanceView.ConfirmGenerate
        Dim result = XtraMessageBox.Show(
            $"Gagawa ng lahat ng {cycleType} Cutoff para sa {year}. Itutuloy?",
            "Generate for Year", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        Return result = DialogResult.Yes
    End Function

#End Region

End Class