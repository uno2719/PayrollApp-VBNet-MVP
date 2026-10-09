Imports System.ComponentModel
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

''' <summary>
''' Pay Cycle Settings: bawat pay cycle (Monthly/SemiMonthly/Weekly/Daily) ay may
''' Rate Basis (Basic Salary o Daily Rate) at Cutoff Pattern (From/To/Pay day ng
''' bawat period, naka-anchor sa pay month). Ito ang pinagmumulan ng Generate
''' Cut-off sa Cutoff tab.
''' </summary>
Public Class ucPayCycle
    Implements IPayCycleMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As PayCyclePresenter
    Private _isEditing As Boolean = False
    Private _rateBasisLocked As Boolean = False
    Private _suppressEvents As Boolean = False
    Private _periods As New BindingList(Of PayCyclePeriodModel)()

    ' Index ng mga button sa wbpMainCommands (ayon sa pagkakasunod sa Designer):
    ' [0]=sep, [1]=Edit/Update, [2]=Cancel, [3]=sep, [4]=Refresh, [5]=sep
    Private Const BTN_EDIT As Integer = 1
    Private Const BTN_CANCEL As Integer = 2
    Private Const BTN_REFRESH As Integer = 4

    Public Sub New()
        InitializeComponent()
        ConfigureEditors()
    End Sub

    Public Sub SetPresenter(presenter As PayCyclePresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Settings > Pay Cycle"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Pay Cycle"
        End Get
    End Property

    ' Iisang permission ang Pay Cycle at Cutoff (parehong cutoff setup)
    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Settings_CutOff
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _presenter.LoadAsync()
        ApplyReadOnlyMode(wbpMainCommands)
    End Function

    ' =============================================
    ' EDITORS - lookups ng pattern grid at rate basis
    ' =============================================
    Private Sub ConfigureEditors()
        ConfigureChoiceLookup(repoDay, PayCycleChoices.DayChoices())
        ConfigureChoiceLookup(repoMonth, PayCycleChoices.MonthOffsetChoices())

        repoDay.DropDownRows = 12
        repoMonth.DropDownRows = 3

        cboRateBasis.Properties.Items.Clear()
        For Each choice In PayCycleChoices.RateBasisChoices()
            cboRateBasis.Properties.Items.Add(choice.Text)
        Next

        gridViewPeriods.OptionsBehavior.Editable = False
    End Sub

    Private Shared Sub ConfigureChoiceLookup(repo As RepositoryItemLookUpEdit, choices As List(Of PayCycleChoice))
        repo.DataSource = choices
        repo.DisplayMember = "Text"
        repo.ValueMember = "Value"
        repo.Columns.Clear()
        repo.Columns.Add(New LookUpColumnInfo("Text"))
        repo.ShowHeader = False
        repo.NullText = ""
        repo.TextEditStyle = TextEditStyles.DisableTextEditor
    End Sub

    ' =============================================
    ' BUTTON COMMANDS
    ' =============================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Select Case tag
            Case "Edit"
                If _isEditing Then
                    Await _presenter.SaveAsync()
                Else
                    Await _presenter.StartEditAsync()
                End If

            Case "Cancel"
                If _isEditing Then
                    Await _presenter.CancelEditAsync()
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If
        End Select
    End Sub

    Private Async Sub gridViewCycles_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridViewCycles.FocusedRowChanged

        If _presenter Is Nothing Then Return

        Dim item = TryCast(gridViewCycles.GetRow(e.FocusedRowHandle), PayCycleModel)
        If item IsNot Nothing Then
            Await _presenter.SelectItemAsync(item.PayCycleType)
        End If
    End Sub

    Private Sub gridViewPeriods_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) _
        Handles gridViewPeriods.CellValueChanged

        If _presenter IsNot Nothing AndAlso Not _suppressEvents Then _presenter.UpdatePreview()
    End Sub

    Private Sub cboRateBasis_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cboRateBasis.SelectedIndexChanged

        If _presenter IsNot Nothing AndAlso Not _suppressEvents Then _presenter.UpdatePreview()
    End Sub

    Private Sub btnAddPeriod_Click(sender As Object, e As EventArgs) Handles btnAddPeriod.Click
        Dim newRow As New PayCyclePeriodModel With {.PeriodNo = _periods.Count + 1}

        ' Kung may nauna nang period, ituloy mula sa dulo nito (karaniwang paghahati ng buwan)
        Dim last = _periods.LastOrDefault()
        If last IsNot Nothing AndAlso last.ToDay < PayCycleChoices.EndOfMonthDay Then
            newRow.FromDay = last.ToDay + 1
            newRow.FromMonthOffset = last.ToMonthOffset
            newRow.ToDay = PayCycleChoices.EndOfMonthDay
            newRow.ToMonthOffset = last.ToMonthOffset
            newRow.PayDay = PayCycleChoices.EndOfMonthDay
        End If

        _periods.Add(newRow)
        Renumber()
        _presenter.UpdatePreview()
    End Sub

    Private Sub btnRemovePeriod_Click(sender As Object, e As EventArgs) Handles btnRemovePeriod.Click
        Dim handle = gridViewPeriods.FocusedRowHandle
        If handle < 0 Then Return

        _periods.RemoveAt(gridViewPeriods.GetDataSourceRowIndex(handle))
        Renumber()
        _presenter.UpdatePreview()
    End Sub

    Private Sub Renumber()
        For i = 0 To _periods.Count - 1
            _periods(i).PeriodNo = i + 1
        Next
        gridViewPeriods.RefreshData()
    End Sub

#Region "IPayCycleMaintenanceView"

    Public Property PayCycleType As String Implements IPayCycleMaintenanceView.PayCycleType
        Get
            Return txtPayCycle.Text
        End Get
        Set(value As String)
            txtPayCycle.Text = value
        End Set
    End Property

    Public Property RateBasis As PayRateBasis Implements IPayCycleMaintenanceView.RateBasis
        Get
            Return CType(Math.Max(cboRateBasis.SelectedIndex, 0), PayRateBasis)
        End Get
        Set(value As PayRateBasis)
            _suppressEvents = True
            cboRateBasis.SelectedIndex = CInt(value)
            _suppressEvents = False
        End Set
    End Property

    Public Property IsActive As Boolean Implements IPayCycleMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    Public Sub BindPeriods(items As List(Of PayCyclePeriodModel)) Implements IPayCycleMaintenanceView.BindPeriods
        _suppressEvents = True
        _periods = New BindingList(Of PayCyclePeriodModel)(items)
        gridControlPeriods.DataSource = _periods
        _suppressEvents = False
    End Sub

    Public Function GetPeriods() As List(Of PayCyclePeriodModel) Implements IPayCycleMaintenanceView.GetPeriods
        Return _periods.ToList()
    End Function

    Public Sub CommitPendingEdits() Implements IPayCycleMaintenanceView.CommitPendingEdits
        gridViewPeriods.CloseEditor()
        gridViewPeriods.UpdateCurrentRow()
    End Sub

    Public Sub BindList(items As List(Of PayCycleModel)) Implements IPayCycleMaintenanceView.BindList
        gridCycles.DataSource = items
    End Sub

    Public Sub FocusCycle(payCycleType As String) Implements IPayCycleMaintenanceView.FocusCycle
        Dim handle = gridViewCycles.LocateByValue("PayCycleType", payCycleType)
        If handle >= 0 Then gridViewCycles.FocusedRowHandle = handle
    End Sub

    Public Sub SetFormMode(isEditable As Boolean) Implements IPayCycleMaintenanceView.SetFormMode
        _isEditing = isEditable

        chkActive.Enabled = isEditable
        cboRateBasis.Enabled = isEditable AndAlso Not _rateBasisLocked
        gridViewPeriods.OptionsBehavior.Editable = isEditable
        btnAddPeriod.Enabled = isEditable
        btnRemovePeriod.Enabled = isEditable
        gridCycles.Enabled = Not isEditable

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Update"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Save changes to this pay cycle"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

        wbpMainCommands.Buttons.Item(BTN_CANCEL).Properties.Enabled = isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable
    End Sub

    Public Sub SetRateBasisLock(isLocked As Boolean, reason As String) Implements IPayCycleMaintenanceView.SetRateBasisLock
        _rateBasisLocked = isLocked
        lblLockNote.Text = If(isLocked, reason, "")
        lblLockNote.Visible = isLocked
        cboRateBasis.Enabled = _isEditing AndAlso Not isLocked
    End Sub

    Public Sub ShowRateBasisNote(text As String) Implements IPayCycleMaintenanceView.ShowRateBasisNote
        lblRateNote.Text = text
    End Sub

    Public Sub ShowPatternPreview(text As String, isError As Boolean) Implements IPayCycleMaintenanceView.ShowPatternPreview
        lblPreview.Text = text
        lblPreview.Appearance.ForeColor = If(isError, Color.Firebrick, Color.DimGray)
    End Sub

    Public Sub ClearFields() Implements IPayCycleMaintenanceView.ClearFields
        txtPayCycle.Text = ""
        _suppressEvents = True
        cboRateBasis.SelectedIndex = 0
        _suppressEvents = False
        chkActive.Checked = False
        BindPeriods(New List(Of PayCyclePeriodModel)())
        lblRateNote.Text = ""
        lblLockNote.Text = ""
        lblPreview.Text = ""
    End Sub

    Public Sub ShowMessage(message As String) Implements IPayCycleMaintenanceView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements IPayCycleMaintenanceView.ShowError
        MyBase.ShowError(message)
    End Sub

    Public Function ConfirmDeactivate(message As String) As Boolean Implements IPayCycleMaintenanceView.ConfirmDeactivate
        Return XtraMessageBox.Show(Me.FindForm(), message, "Deactivate Pay Cycle",
                                   MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes
    End Function

#End Region

End Class
