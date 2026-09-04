Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Models
Imports Payroll.Lookups.Presenters
Imports Payroll.Lookups.Views

Public Class ucLookupMaintenance
    Implements ILookupMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As LookupPresenter
    Private _isEditing As Boolean = False
    Private _tabTitle As String = "Master Data"

    Private Const BTN_NEW As Integer = 0
    ' index 1 = separator
    Private Const BTN_SAVE As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5

    ' Tinatawag ito ng AppComposition kapag ginagawa yung 8 instances -
    ' tabTitle ay display text lang (hal. "Branch") para sa Breadcrumb/
    ' PageTitle. Ang tableName mismo ay nasa Presenter na (constructor
    ' injected doon), hindi na kailangan ulitin dito sa View.
    Public Sub SetPresenter(presenter As LookupPresenter, tabTitle As String)
        _presenter = presenter
        _tabTitle = tabTitle
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE - dynamic per instance (8 magkaibang tabTitle)
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Master Data > {_tabTitle}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return _tabTitle
        End Get
    End Property

    ' =============================================
    ' LOAD (lazy - tinatawag ito ng parent shell sa unang pagbukas
    ' ng bawat tab, hindi lahat ng 8 agad pagbukas ng Settings)
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupGrid()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try
    End Function

    ' Sadyang naka-OFF ang inline grid editing (kahit pinagana natin ang
    ' NewItemRowPosition sa Designer) - ang TOP FORM (Code/Name/Active)
    ' + buttons na lang ang single edit path, gaya ng Users/Employee
    ' modules. Iniiwasan nito ang pagkalito kung magkaiba ang laman ng
    ' grid cell at ng form kapag pareho silang pwedeng i-edit nang sabay.
    Private Sub SetupGrid()
        With gridviewLookupList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' =============================================
    ' ILookupMaintenanceView - FORM FIELDS
    ' =============================================
    Public Property Code As String Implements ILookupMaintenanceView.Code
        Get
            Return txtCode.Text
        End Get
        Set(value As String)
            txtCode.Text = value
        End Set
    End Property

    Public Property Name As String Implements ILookupMaintenanceView.Name
        Get
            Return txtName.Text
        End Get
        Set(value As String)
            txtName.Text = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements ILookupMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' ILookupMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of LookupModel)) Implements ILookupMaintenanceView.BindList
        gridconLookupList.DataSource = items
    End Sub

    ' =============================================
    ' ILookupMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements ILookupMaintenanceView.SetFormMode

        _isEditing = isEditable

        txtCode.Properties.ReadOnly = Not isEditable
        txtName.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable
        gridconLookupList.Enabled = Not isEditable

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Visible = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_SAVE).Properties.Visible = isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

        ' Ang Delete button ang siya ring "Cancel" habang nag-e-edit -
        ' parehong pattern gaya ng Edit/Save + Delete/Cancel sa Users.
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Caption =
            If(isEditable, " Cancel", " Delete")
    End Sub

    Public Sub ClearFields() Implements ILookupMaintenanceView.ClearFields
        txtCode.Text = String.Empty
        txtName.Text = String.Empty
        chkActive.Checked = True
    End Sub

    ' NOTE: DisplayInfo/DisplayValidationError (HINDI ShowMessage/
    ' ShowError) - parehong pangalan kasi ang ginagamit ng GlobalShared.
    ' Base.ucBase (minana rito), kaya iba ang pangalan dito para hindi
    ' mag-conflict - eksaktong parehong ayos ng ginawa sa ucUsers.
    ' Sa loob, tinatawag pa rin natin ang minanang ShowMessage/ShowError
    ' mismo (walang duplicate na status label na kailangang gawin dito).
    Public Sub DisplayInfo(message As String) Implements ILookupMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements ILookupMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION - dalawang handler (FocusedRowChanged AT Click),
    ' parehong pattern gaya ng ucUsers/ucEmployees.
    ' =============================================
    Private Sub gridviewLookupList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewLookupList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewLookupList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewLookupList_Click(sender As Object, e As EventArgs) _
        Handles gridviewLookupList.Click

        If _isEditing Then Return
        If gridviewLookupList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewLookupList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    ' =============================================
    ' BUTTON COMMANDS (New / Save / Delete-Cancel / Refresh)
    ' =============================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Select Case tag
            Case "New"
                _presenter.StartNew()

            Case "Save"
                Await _presenter.SaveAsync()

            Case "Delete"
                If _isEditing Then
                    ' Dito, "Delete" tag pero "Cancel" ang caption/action
                    _presenter.CancelEdit()
                Else
                    Dim action = If(IsActive, "deactivate", "reactivate")
                    Dim confirm = XtraMessageBox.Show(
                        $"Are you sure you want to {action} this entry?",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.ToggleActiveSelectedAsync()
                    End If
                End If

            Case "Refresh"
                Await _presenter.LoadAsync()
        End Select
    End Sub

End Class