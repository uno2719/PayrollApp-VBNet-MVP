Imports System.Data
Imports System.Drawing
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.StatutorySettings.Presenters
Imports Payroll.StatutorySettings.Services
Imports Payroll.StatutorySettings.Views

''' <summary>
''' Statutory Deduction Schedule: para sa bawat active na pay cycle, ilang % ng BUWANANG contribution ng
''' SSS / PhilHealth / Pag-IBIG ang kinakaltas sa bawat cutoff period (hal. SSS 100% sa period 2 = katapusan).
''' Kasama rito ang setting kung paano tatratuhin ang Pag-IBIG voluntary amount ng employee.
''' </summary>
Public Class ucStatutorySchedule
    Implements IStatutoryScheduleView
    Implements IAsyncLoadable

    Private Const ColAgencyKey As String = "AgencyKey"
    Private Const ColAgency As String = "Agency"
    Private Const ColTotal As String = "Total"

    ' Index ng mga button sa wbpMainCommands (ayon sa pagkakasunod sa Designer):
    ' [0]=sep, [1]=Edit/Update, [2]=Cancel, [3]=sep, [4]=Refresh, [5]=sep
    Private Const BTN_EDIT As Integer = 1
    Private Const BTN_CANCEL As Integer = 2
    Private Const BTN_REFRESH As Integer = 4

    Private _presenter As StatutorySchedulePresenter
    Private _isEditing As Boolean = False
    Private _suppressEvents As Boolean = False
    Private _table As DataTable
    Private _periodCount As Integer = 0
    Private ReadOnly _repoShare As New RepositoryItemSpinEdit()

    ' Ang pagkakasunod ng dropdown ang batayan ng index -> mode (hindi ang numero ng enum).
    Private ReadOnly _voluntaryModes As List(Of PagIbigVoluntaryMode) = PagIbigVoluntaryModes.All()

    Public Sub New()
        InitializeComponent()
        ConfigureEditors()
    End Sub

    Public Sub SetPresenter(presenter As StatutorySchedulePresenter)
        _presenter = presenter
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Settings > Payroll Setup > Statutory > Deduction Schedule"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Deduction Schedule"
        End Get
    End Property

    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Settings_Statutory
        End Get
    End Property

    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _presenter.LoadAsync()
        ApplyReadOnlyMode(wbpMainCommands)
    End Function

    ' =============================================
    ' EDITORS
    ' =============================================
    Private Sub ConfigureEditors()
        ' Share % ng bawat period: 0 - 100
        _repoShare.IsFloatValue = True
        _repoShare.MinValue = 0D
        _repoShare.MaxValue = 100D
        _repoShare.Increment = 5D
        _repoShare.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        _repoShare.Mask.EditMask = "n2"
        _repoShare.Mask.UseMaskAsDisplayFormat = True
        gridSchedule.RepositoryItems.Add(_repoShare)

        cboVoluntaryMode.Properties.Items.Clear()
        For Each mode In _voluntaryModes
            cboVoluntaryMode.Properties.Items.Add(PagIbigVoluntaryModes.Text(mode))
        Next
        cboVoluntaryMode.SelectedIndex = 0
        ShowVoluntaryNote()

        gridViewSchedule.OptionsBehavior.Editable = False
    End Sub

    Private Shared Function PeriodColumn(periodNo As Integer) As String
        Return "P" & periodNo.ToString()
    End Function

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
                    _presenter.StartEdit()
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

    Private Sub gridViewCycles_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridViewCycles.FocusedRowChanged

        If _presenter Is Nothing OrElse _suppressEvents Then Return

        Dim item = TryCast(gridViewCycles.GetRow(e.FocusedRowHandle), StatutoryScheduleCycleInfo)
        If item IsNot Nothing Then _presenter.SelectCycle(item.PayCycleType)
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        _presenter.ApplyPresetToFocused(SchedulePreset.First)
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        _presenter.ApplyPresetToFocused(SchedulePreset.Last)
    End Sub

    Private Sub btnSplit_Click(sender As Object, e As EventArgs) Handles btnSplit.Click
        _presenter.ApplyPresetToFocused(SchedulePreset.Split)
    End Sub

    Private Sub btnNone_Click(sender As Object, e As EventArgs) Handles btnNone.Click
        _presenter.ApplyPresetToFocused(SchedulePreset.NotDeducted)
    End Sub

    Private Sub cboVoluntaryMode_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles cboVoluntaryMode.SelectedIndexChanged

        ShowVoluntaryNote()
    End Sub

    Private Function SelectedVoluntaryMode() As PagIbigVoluntaryMode
        Dim index = cboVoluntaryMode.SelectedIndex
        If index < 0 OrElse index >= _voluntaryModes.Count Then Return _voluntaryModes(0)
        Return _voluntaryModes(index)
    End Function

    Private Sub ShowVoluntaryNote()
        lblVoluntaryNote.Text = PagIbigVoluntaryModes.Describe(SelectedVoluntaryMode())
    End Sub

    ' =============================================
    ' GRID EVENTS
    ' =============================================
    Private Sub gridViewSchedule_CellValueChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) _
        Handles gridViewSchedule.CellValueChanged

        If _suppressEvents OrElse _table Is Nothing Then Return
        If e.Column.FieldName = ColTotal OrElse e.Column.FieldName = ColAgency OrElse e.Column.FieldName = ColAgencyKey Then Return

        Dim dataRow = gridViewSchedule.GetDataRow(e.RowHandle)
        If dataRow Is Nothing Then Return

        RecomputeTotal(dataRow)
        If _presenter IsNot Nothing Then _presenter.UpdatePreview()
    End Sub

    ' Pula kapag hindi 100% (o 0%) ang total ng hilera; berde kung 100%; abo kung 0%.
    Private Sub gridViewSchedule_RowCellStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) _
        Handles gridViewSchedule.RowCellStyle

        If _table Is Nothing OrElse e.Column.FieldName <> ColTotal Then Return

        Dim raw = gridViewSchedule.GetRowCellValue(e.RowHandle, ColTotal)
        If raw Is Nothing OrElse IsDBNull(raw) Then Return

        Dim total = Convert.ToDecimal(raw)
        If Not StatutoryScheduleRules.IsValidTotal(total) Then
            e.Appearance.ForeColor = Color.Firebrick
        ElseIf total > 0D Then
            e.Appearance.ForeColor = Color.SeaGreen
        Else
            e.Appearance.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub RecomputeTotal(dataRow As DataRow)
        Dim sum As Decimal = 0D
        For i = 1 To _periodCount
            sum += CellDecimal(dataRow, PeriodColumn(i))
        Next
        dataRow(ColTotal) = sum
    End Sub

    Private Shared Function CellDecimal(dataRow As DataRow, columnName As String) As Decimal
        Dim value = dataRow(columnName)
        If value Is Nothing OrElse IsDBNull(value) Then Return 0D
        Return Convert.ToDecimal(value)
    End Function

#Region "IStatutoryScheduleView"

    Public Sub BindCycles(items As List(Of StatutoryScheduleCycleInfo)) Implements IStatutoryScheduleView.BindCycles
        _suppressEvents = True
        gridCycles.DataSource = items
        _suppressEvents = False
    End Sub

    Public Sub FocusCycle(payCycleType As String) Implements IStatutoryScheduleView.FocusCycle
        _suppressEvents = True
        Dim handle = gridViewCycles.LocateByValue("PayCycleType", payCycleType)
        If handle >= 0 Then gridViewCycles.FocusedRowHandle = handle
        _suppressEvents = False
    End Sub

    Public Sub BindSchedule(cycle As StatutoryScheduleCycleInfo, rows As List(Of StatutoryScheduleRow)) _
        Implements IStatutoryScheduleView.BindSchedule

        _suppressEvents = True

        _periodCount = cycle.Periods.Count

        Dim table As New DataTable()
        table.Columns.Add(ColAgencyKey, GetType(String))
        table.Columns.Add(ColAgency, GetType(String))
        For i = 1 To _periodCount
            table.Columns.Add(PeriodColumn(i), GetType(Decimal))
        Next
        table.Columns.Add(ColTotal, GetType(Decimal))

        For Each row In rows
            Dim dataRow = table.NewRow()
            dataRow(ColAgencyKey) = row.Agency
            dataRow(ColAgency) = row.AgencyName
            For i = 0 To _periodCount - 1
                dataRow(PeriodColumn(i + 1)) = If(i < row.Shares.Count, row.Shares(i), 0D)
            Next
            dataRow(ColTotal) = row.Total
            table.Rows.Add(dataRow)
        Next

        _table = table
        gridViewSchedule.Columns.Clear()
        gridSchedule.DataSource = _table
        gridViewSchedule.PopulateColumns()

        gridViewSchedule.Columns(ColAgencyKey).Visible = False

        With gridViewSchedule.Columns(ColAgency)
            .Caption = "Agency"
            .OptionsColumn.AllowEdit = False
            .Width = 110
            .VisibleIndex = 0
        End With

        For i = 1 To _periodCount
            With gridViewSchedule.Columns(PeriodColumn(i))
                .Caption = cycle.Periods(i - 1).Caption
                .ColumnEdit = _repoShare
                .Width = 190
                .VisibleIndex = i
            End With
        Next

        With gridViewSchedule.Columns(ColTotal)
            .Caption = "Total %"
            .OptionsColumn.AllowEdit = False
            .DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            .DisplayFormat.FormatString = "n2"
            .Width = 80
            .VisibleIndex = _periodCount + 1
        End With

        gridViewSchedule.OptionsBehavior.Editable = _isEditing
        lblPattern.Text = "Cutoff pattern: " & cycle.PatternText

        _suppressEvents = False
    End Sub

    Public Function GetSchedule() As List(Of StatutoryScheduleRow) Implements IStatutoryScheduleView.GetSchedule
        Dim result As New List(Of StatutoryScheduleRow)()
        If _table Is Nothing Then Return result

        For Each dataRow As DataRow In _table.Rows
            Dim row As New StatutoryScheduleRow With {.Agency = dataRow(ColAgencyKey).ToString()}
            For i = 1 To _periodCount
                row.Shares.Add(CellDecimal(dataRow, PeriodColumn(i)))
            Next
            result.Add(row)
        Next

        Return result
    End Function

    Public Sub SetAgencyShares(agency As String, shares As List(Of Decimal)) Implements IStatutoryScheduleView.SetAgencyShares
        If _table Is Nothing Then Return

        For Each dataRow As DataRow In _table.Rows
            If String.Equals(dataRow(ColAgencyKey).ToString(), agency, StringComparison.OrdinalIgnoreCase) Then
                For i = 1 To _periodCount
                    dataRow(PeriodColumn(i)) = If(i - 1 < shares.Count, shares(i - 1), 0D)
                Next
                RecomputeTotal(dataRow)
                Exit For
            End If
        Next
    End Sub

    Public ReadOnly Property FocusedAgency As String Implements IStatutoryScheduleView.FocusedAgency
        Get
            Dim dataRow = gridViewSchedule.GetFocusedDataRow()
            If dataRow Is Nothing Then Return Nothing
            Return dataRow(ColAgencyKey).ToString()
        End Get
    End Property

    Public Sub CommitPendingEdits() Implements IStatutoryScheduleView.CommitPendingEdits
        gridViewSchedule.CloseEditor()
        gridViewSchedule.UpdateCurrentRow()
    End Sub

    Public Property VoluntaryMode As PagIbigVoluntaryMode Implements IStatutoryScheduleView.VoluntaryMode
        Get
            Return SelectedVoluntaryMode()
        End Get
        Set(value As PagIbigVoluntaryMode)
            Dim index = _voluntaryModes.IndexOf(value)
            cboVoluntaryMode.SelectedIndex = If(index < 0, 0, index)
        End Set
    End Property

    Public Sub ShowPreview(text As String, isError As Boolean) Implements IStatutoryScheduleView.ShowPreview
        lblPreview.Text = text
        lblPreview.Appearance.ForeColor = If(isError, Color.Firebrick, Color.DimGray)
    End Sub

    Public Sub SetFormMode(isEditable As Boolean) Implements IStatutoryScheduleView.SetFormMode
        _isEditing = isEditable

        gridViewSchedule.OptionsBehavior.Editable = isEditable
        cboVoluntaryMode.Enabled = isEditable
        pnlQuickSet.Enabled = isEditable

        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Update"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Save the deduction schedule"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit the deduction schedule"
        End If

        wbpMainCommands.Buttons.Item(BTN_CANCEL).Properties.Enabled = isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable
    End Sub

    Public Sub ClearFields() Implements IStatutoryScheduleView.ClearFields
        _suppressEvents = True
        _table = Nothing
        _periodCount = 0
        gridViewSchedule.Columns.Clear()
        gridSchedule.DataSource = Nothing
        lblPattern.Text = ""
        lblPreview.Text = ""
        _suppressEvents = False
    End Sub

    Public Sub ShowMessage(message As String) Implements IStatutoryScheduleView.ShowMessage
        MyBase.ShowMessage(message)
    End Sub

    Public Sub ShowError(message As String) Implements IStatutoryScheduleView.ShowError
        MyBase.ShowError(message)
    End Sub

#End Region

End Class
