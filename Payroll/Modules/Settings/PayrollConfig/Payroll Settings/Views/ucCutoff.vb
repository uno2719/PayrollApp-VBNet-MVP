Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Extensions
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

Public Class ucCutoff
    Implements ICutoffMaintenanceView
    Implements IAsyncLoadable

    Private Const AllLabel As String = "(All)"

    Private _presenter As CutoffPresenter
    Private _generateDialogFactory As Func(Of frmGenerateCutoff)
    Private _isEditing As Boolean = False
    Private _suppressFilter As Boolean = False

    ' Same indexing scheme as ucPayrollRateEntry's BTN_NEW/BTN_EDIT/BTN_REFRESH —
    ' matches wbpMainCommands.Buttons.AddRange order in the Designer:
    ' [0]=sep, [1]=New/Update, [2]=Edit/Cancel, [3]=sep, [4]=Refresh, [5]=sep
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_REFRESH As Integer = 4

    Public Sub New()
        InitializeComponent()

        ' Ang Cycle Type at Year ay nanggagaling sa Generate dialog - hindi na ine-edit dito.
        cboCycleType.Enabled = False
        numCutoffYear.Enabled = False

        ' Iisang display format ng petsa (AppConstants.DisplayDateFormat)
        dateCutoffStart.ApplyDisplayDateFormat()
        dateCutoffEnd.ApplyDisplayDateFormat()
        datePayDate.ApplyDisplayDateFormat()
        colCutoffStart.ApplyDisplayDateFormat()
        colCutoffEnd.ApplyDisplayDateFormat()
        colPayDate.ApplyDisplayDateFormat()
    End Sub

    ''' <param name="generateDialogFactory">Gumagawa ng Generate Cut-off dialog (kumpleto na ang presenter) tuwing pipindutin ang [New].</param>
    Public Sub SetPresenter(presenter As CutoffPresenter, generateDialogFactory As Func(Of frmGenerateCutoff))
        _presenter = presenter
        _generateDialogFactory = generateDialogFactory
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
                    Await OpenGenerateDialogAsync()
                End If

            Case "Edit"
                If _isEditing Then
                    _presenter.CancelEdit()
                Else
                    _presenter.StartEdit()
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If
        End Select
    End Sub

    Private Async Function OpenGenerateDialogAsync() As Task
        If _generateDialogFactory Is Nothing Then Return

        Using dialog = _generateDialogFactory.Invoke()
            If dialog.ShowDialog(Me.FindForm()) = DialogResult.OK Then
                Await _presenter.LoadAsync()
            End If
        End Using
    End Function

    Private Sub gridView_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridView.FocusedRowChanged

        Dim item = TryCast(gridView.GetRow(e.FocusedRowHandle), CutoffModel)
        If item IsNot Nothing Then
            _presenter.SelectItem(item.CutoffID)
        End If
    End Sub

    Private Sub FilterChanged(sender As Object, e As EventArgs) _
        Handles cboFilterCycle.EditValueChanged, cboFilterYear.EditValueChanged

        If _suppressFilter OrElse _presenter Is Nothing Then Return
        _presenter.ApplyFilter()
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

    ' --- Filter: "(All)" = Nothing ---
    Public Property FilterCycleType As String Implements ICutoffMaintenanceView.FilterCycleType
        Get
            Dim text = CStr(If(cboFilterCycle.EditValue, ""))
            Return If(text = AllLabel OrElse text = "", Nothing, text)
        End Get
        Set(value As String)
            _suppressFilter = True
            cboFilterCycle.EditValue = If(String.IsNullOrEmpty(value) OrElse Not cboFilterCycle.Properties.Items.Contains(value), AllLabel, value)
            _suppressFilter = False
        End Set
    End Property

    Public Property FilterYear As Integer? Implements ICutoffMaintenanceView.FilterYear
        Get
            Dim text = CStr(If(cboFilterYear.EditValue, ""))
            Dim parsed As Integer
            If Integer.TryParse(text, parsed) Then Return parsed
            Return Nothing
        End Get
        Set(value As Integer?)
            _suppressFilter = True
            Dim text = If(value.HasValue, value.Value.ToString(), AllLabel)
            cboFilterYear.EditValue = If(cboFilterYear.Properties.Items.Contains(text), text, AllLabel)
            _suppressFilter = False
        End Set
    End Property

    Public Sub BindFilters(cycleTypes As List(Of String), years As List(Of Integer)) Implements ICutoffMaintenanceView.BindFilters
        _suppressFilter = True

        cboFilterCycle.Properties.Items.Clear()
        cboFilterCycle.Properties.Items.Add(AllLabel)
        For Each cycleType As String In cycleTypes
            cboFilterCycle.Properties.Items.Add(cycleType)
        Next

        cboFilterYear.Properties.Items.Clear()
        cboFilterYear.Properties.Items.Add(AllLabel)
        For Each year As Integer In years
            cboFilterYear.Properties.Items.Add(year.ToString())
        Next

        _suppressFilter = False
    End Sub

    Public Sub BindList(items As List(Of CutoffModel)) Implements ICutoffMaintenanceView.BindList
        gridControl.DataSource = items
    End Sub

    Public Sub SetFormMode(isEditable As Boolean) Implements ICutoffMaintenanceView.SetFormMode
        _isEditing = isEditable

        grpDetails.Enabled = isEditable
        grpFilters.Enabled = Not isEditable
        gridControl.Enabled = Not isEditable

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Cutoff"

            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Generate Cutoffs for a Pay Cycle and Year"

            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

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

#End Region

End Class
