Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.Employee.Models
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Models
Imports Payroll.Loans.Presenters
Imports Payroll.Loans.Views

Public Class ucLoans
    Implements IEmployeeLoanView
    Implements IAsyncLoadable

    Private _presenter As EmployeeLoanPresenter

    ' ========================================================
    ' RE-ENTRANCY GUARD
    ' ========================================================
    ' Ang Recompute ay nagsusulat pabalik sa mga SpinEdit. Ang
    ' pagsusulat na iyon ay nagti-trigger ng EditValueChanged, na
    ' tumatawag ulit ng Recompute... paikot nang paikot hanggang
    ' mag-stack overflow.
    '
    ' Itong flag ang pumuputol ng siklo: habang ang PRESENTER ang
    ' nagsusulat, binabalewala natin ang mga change event. Ang
    ' aktwal na pag-type ni user lang ang nakakalusot.
    ' ========================================================
    Private _isPushingValues As Boolean = False

    Private _isEditing As Boolean = False

    ' Button indexes sa wbpMainCommands
    ' (0=sep 1=New 2=Edit 3=Delete 4=sep 5=Details 6=Toggle 7=sep 8=Refresh 9=sep)
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    Private Const BTN_DETAILS As Integer = 5
    Private Const BTN_TOGGLE As Integer = 6
    Private Const BTN_REFRESH As Integer = 8

    Public Sub SetPresenter(presenter As EmployeeLoanPresenter)
        _presenter = presenter
    End Sub

    ' ========================================================
    ' IDENTITY
    ' ========================================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return "Main > Loans"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Employee Loans"
        End Get
    End Property

    ' ITO ANG NAG-UUGNAY SA MODULE ACCESS SYSTEM.
    ' Kapareho ng aceLoan.Tag sa frmMain.Designer.vb at ng
    ' ModuleCode sa tblModules.
    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return ModuleCodes.Main_Loan
        End Get
    End Property

    ' ========================================================
    ' LOAD
    ' ========================================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrids()
        SetupCombos()

        rgEmployeeFilter.EditValue = "Active"
        rgLoanStatusFilter.EditValue = "Active"

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

        ' Sa DULO ito - kung mauuna, mao-overwrite ng SetFormMode
        ' ang pag-disable ng mga button.
        ApplyReadOnlyMode(wbpMainCommands)

    End Function

    Private Sub SetupCommandImages()
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
    End Sub

    Private Sub SetupGrids()

        With gridviewEmployeeList
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

        With gridviewLoanList
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ShowAutoFilterRow = True
            .OptionsView.NewItemRowPosition = NewItemRowPosition.None
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

    End Sub

    Private Sub SetupCombos()

        cboFrequency.Properties.Items.Clear()
        cboFrequency.Properties.Items.AddRange(LoanFrequency.All)

        ' LookUpEdit setup - ginagawa dito sa code imbes na sa Designer
        ' para isang lugar lang tingnan kung magbabago ang columns.
        With lueLoanCode.Properties
            .DisplayMember = "Description"
            .ValueMember = "Code"
            .NullText = "-- Select Loan Code --"
            .ShowFooter = False
            .ShowHeader = True
            .SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter
            .AutoSearchColumnIndex = 1

            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", "Code", 80))
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description", 220))
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("LoanType", "Type", 120))
        End With

    End Sub

    ' ========================================================
    ' IEmployeeLoanView - LISTS
    ' ========================================================
    Public Sub BindEmployeeList(items As List(Of EmployeeModel)) _
        Implements IEmployeeLoanView.BindEmployeeList

        gridconEmployeeList.DataSource = items

        ' Itago lahat, ipakita lang ang dalawang kailangan - kapareho
        ' ng ginawa sa ucEmployees para pare-pareho ang itsura.
        For Each col As DevExpress.XtraGrid.Columns.GridColumn In gridviewEmployeeList.Columns
            col.Visible = False
        Next

        Dim colEmpNo = gridviewEmployeeList.Columns("EmployeeNo")
        If colEmpNo IsNot Nothing Then
            colEmpNo.Visible = True
            colEmpNo.Caption = "Emp. No."
            colEmpNo.VisibleIndex = 0
        End If

        Dim colFullName = gridviewEmployeeList.Columns("FullName")
        If colFullName IsNot Nothing Then
            colFullName.Visible = True
            colFullName.Caption = "Full Name"
            colFullName.VisibleIndex = 1
        End If

        gridviewEmployeeList.BestFitColumns()

    End Sub

    Public Sub BindLoanList(items As List(Of EmployeeLoanModel)) _
        Implements IEmployeeLoanView.BindLoanList

        gridconLoanList.DataSource = items

    End Sub

    Public Sub BindLoanCodes(items As List(Of LoanModel)) _
        Implements IEmployeeLoanView.BindLoanCodes

        lueLoanCode.Properties.DataSource = items

    End Sub

    Public Sub SetEmployeeHeader(employeeNo As String, fullName As String) _
        Implements IEmployeeLoanView.SetEmployeeHeader

        If String.IsNullOrWhiteSpace(fullName) Then
            lblSelectedEmployee.Text = "No employee selected"
        Else
            lblSelectedEmployee.Text = $"{employeeNo}  -  {fullName}"
        End If

    End Sub

    ' ========================================================
    ' IEmployeeLoanView - FORM FIELDS
    ' ========================================================
    Public Property LoanCode As String Implements IEmployeeLoanView.LoanCode
        Get
            Return If(lueLoanCode.EditValue?.ToString(), String.Empty)
        End Get
        Set(value As String)
            lueLoanCode.EditValue = value
        End Set
    End Property

    Public Property InputDate As Date Implements IEmployeeLoanView.InputDate
        Get
            Return If(dtInputDate.EditValue Is Nothing, Date.Today, dtInputDate.DateTime.Date)
        End Get
        Set(value As Date)
            dtInputDate.EditValue = value
        End Set
    End Property

    Public Property LoanStartDate As Date Implements IEmployeeLoanView.LoanStartDate
        Get
            Return If(dtLoanStartDate.EditValue Is Nothing, Date.Today, dtLoanStartDate.DateTime.Date)
        End Get
        Set(value As Date)
            dtLoanStartDate.EditValue = value
        End Set
    End Property

    Public Property Frequency As String Implements IEmployeeLoanView.Frequency
        Get
            Return cboFrequency.Text
        End Get
        Set(value As String)
            cboFrequency.Text = value
        End Set
    End Property

    Public Property AmortizationMethod As String Implements IEmployeeLoanView.AmortizationMethod
        Get
            Return If(rgAmortizationMethod.EditValue?.ToString(), LoanAmortizationMethod.BasedOnTerm)
        End Get
        Set(value As String)
            rgAmortizationMethod.EditValue = value
        End Set
    End Property

    Public Property InterestMethod As String Implements IEmployeeLoanView.InterestMethod
        Get
            Return If(rgInterestMethod.EditValue?.ToString(), LoanInterestMethod.StraightLine)
        End Get
        Set(value As String)
            rgInterestMethod.EditValue = value
        End Set
    End Property

    Public Property InterestRate As Decimal Implements IEmployeeLoanView.InterestRate
        Get
            Return spnInterestRate.Value
        End Get
        Set(value As Decimal)
            spnInterestRate.Value = value
        End Set
    End Property

    Public Property Terms As Integer Implements IEmployeeLoanView.Terms
        Get
            Return CInt(spnTerms.Value)
        End Get
        Set(value As Integer)
            spnTerms.Value = value
        End Set
    End Property

    Public Property Remark As String Implements IEmployeeLoanView.Remark
        Get
            Return txtRemark.Text
        End Get
        Set(value As String)
            txtRemark.Text = value
        End Set
    End Property

    Public Property PrincipalAmount As Decimal Implements IEmployeeLoanView.PrincipalAmount
        Get
            Return spnPrincipalAmount.Value
        End Get
        Set(value As Decimal)
            spnPrincipalAmount.Value = value
        End Set
    End Property

    Public Property InterestAmount As Decimal Implements IEmployeeLoanView.InterestAmount
        Get
            Return spnInterestAmount.Value
        End Get
        Set(value As Decimal)
            spnInterestAmount.Value = value
        End Set
    End Property

    Public Property TotalLoanAmount As Decimal Implements IEmployeeLoanView.TotalLoanAmount
        Get
            Return spnTotalLoanAmount.Value
        End Get
        Set(value As Decimal)
            spnTotalLoanAmount.Value = value
        End Set
    End Property

    Public Property PrincipalAmortization As Decimal Implements IEmployeeLoanView.PrincipalAmortization
        Get
            Return spnPrincipalAmortization.Value
        End Get
        Set(value As Decimal)
            spnPrincipalAmortization.Value = value
        End Set
    End Property

    Public Property InterestAmortization As Decimal Implements IEmployeeLoanView.InterestAmortization
        Get
            Return spnInterestAmortization.Value
        End Get
        Set(value As Decimal)
            spnInterestAmortization.Value = value
        End Set
    End Property

    Public Property TotalAmortization As Decimal Implements IEmployeeLoanView.TotalAmortization
        Get
            Return spnTotalAmortization.Value
        End Get
        Set(value As Decimal)
            spnTotalAmortization.Value = value
        End Set
    End Property

    Public Property StatusText As String Implements IEmployeeLoanView.StatusText
        Get
            Return lblStatusValue.Text
        End Get
        Set(value As String)
            lblStatusValue.Text = If(String.IsNullOrWhiteSpace(value), "-", value)
            lblStatusValue.Appearance.ForeColor = StatusColor(value)
            lblStatusValue.Appearance.Options.UseForeColor = True
        End Set
    End Property

    ' Kulay bilang mabilis na hudyat - mapapansin mo agad sa buong
    ' screen kung ano ang estado, hindi mo na kailangang basahin.
    Private Shared Function StatusColor(status As String) As Color
        Select Case status
            Case LoanStatus.Active : Return Color.SeaGreen
            Case LoanStatus.Inactive : Return Color.DarkOrange
            Case LoanStatus.Completed : Return Color.SteelBlue
            Case LoanStatus.Cancelled : Return Color.Firebrick
            Case Else : Return Color.Gray
        End Select
    End Function

    ' ========================================================
    ' IEmployeeLoanView - STATE / UX
    ' ========================================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IEmployeeLoanView.SetFormMode

        _isEditing = isEditable

        lueLoanCode.Properties.ReadOnly = Not isEditable
        dtLoanStartDate.Properties.ReadOnly = Not isEditable
        cboFrequency.Properties.ReadOnly = Not isEditable
        rgAmortizationMethod.Properties.ReadOnly = Not isEditable
        rgInterestMethod.Properties.ReadOnly = Not isEditable
        spnInterestRate.Properties.ReadOnly = Not isEditable
        spnPrincipalAmount.Properties.ReadOnly = Not isEditable
        txtRemark.Properties.ReadOnly = Not isEditable

        ' Ang Terms at Total Amortization ay may sarili pang rule -
        ' nakadepende sa Amortization Method. Pinangangasiwaan ito ng
        ' ApplyAmortizationMethodLayout, kaya dito, kapag hindi
        ' editable ang form, parehong naka-lock.
        If Not isEditable Then
            spnTerms.Properties.ReadOnly = True
            spnTotalAmortization.Properties.ReadOnly = True
        End If

        ' Habang nag-e-edit, bawal magpalit ng empleyado o ng loan -
        ' mawawala ang tina-type mo.
        gridconEmployeeList.Enabled = Not isEditable
        gridconLoanList.Enabled = Not isEditable
        rgEmployeeFilter.Enabled = Not isEditable
        rgLoanStatusFilter.Enabled = Not isEditable

        ' --- BUTTON CAPTIONS (New -> Save/Update, Edit -> Cancel) ---
        If isEditable Then
            If isNewRecord Then
                SetButton(BTN_NEW, " Save", My.Resources.icon_save_24, "Save New Loan")
            Else
                SetButton(BTN_NEW, " Update", My.Resources.icon_saveAs_24, "Save Changes")
            End If

            SetButton(BTN_EDIT, " Cancel", My.Resources.icon_cancel_24, "Cancel")
        Else
            SetButton(BTN_NEW, " New", My.Resources.icon_add_property_24_png, "Add New Loan")
            SetButton(BTN_EDIT, " Edit", My.Resources.icon_edit_property_24, "Edit Selected Loan")
        End If

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Private Sub SetButton(index As Integer, caption As String, img As Image, tooltip As String)
        With wbpMainCommands.Buttons.Item(index).Properties
            .Caption = caption
            .ImageOptions.Image = img
            .ToolTip = tooltip
        End With
    End Sub

    ' ========================================================
    ' CONTEXTUAL BUTTONS
    ' ========================================================
    ' Ito ang hinihiling mo: lumalabas lang ang Details at ang
    ' Set Active/Inactive kapag may napili sa gridview.
    '
    ' Ginagamit ko ang .Visible (hindi .Enabled) - kapag walang
    ' napili, wala talagang dapat makitang button doon. Mas malinis
    ' tingnan kaysa sa dalawang naka-gray na button na laging nandiyan.
    ' ========================================================
    Public Sub SetLoanSelected(hasSelection As Boolean, canToggle As Boolean, toggleCaption As String) _
        Implements IEmployeeLoanView.SetLoanSelected

        wbpMainCommands.Buttons.Item(BTN_DETAILS).Properties.Visible = hasSelection

        wbpMainCommands.Buttons.Item(BTN_TOGGLE).Properties.Visible = hasSelection AndAlso canToggle

        If hasSelection AndAlso canToggle Then
            wbpMainCommands.Buttons.Item(BTN_TOGGLE).Properties.Caption = toggleCaption
        End If

    End Sub

    Public Sub ApplyAmortizationMethodLayout(isBasedOnTerm As Boolean) _
        Implements IEmployeeLoanView.ApplyAmortizationMethodLayout

        If Not _isEditing Then Return

        ' Isa lang ang pwedeng i-type: kung alam mo ang Terms,
        ' kinukuwenta ang Amortization - at kabaliktaran.
        spnTerms.Properties.ReadOnly = Not isBasedOnTerm
        spnTotalAmortization.Properties.ReadOnly = isBasedOnTerm

        ' Bold ang nasa kontrol mo, para malinaw kung alin ang
        ' tina-type at alin ang computed.
        spnTerms.Properties.Appearance.FontStyleDelta =
            If(isBasedOnTerm, FontStyle.Bold, FontStyle.Regular)
        spnTerms.Properties.Appearance.Options.UseFont = True

        spnTotalAmortization.Properties.Appearance.FontStyleDelta =
            If(isBasedOnTerm, FontStyle.Regular, FontStyle.Bold)
        spnTotalAmortization.Properties.Appearance.Options.UseFont = True

    End Sub

    Public Sub ClearFields() Implements IEmployeeLoanView.ClearFields

        _isPushingValues = True

        Try
            lueLoanCode.EditValue = Nothing
            dtLoanStartDate.EditValue = Date.Today
            dtInputDate.EditValue = Date.Today
            cboFrequency.Text = String.Empty
            rgAmortizationMethod.EditValue = LoanAmortizationMethod.BasedOnTerm
            rgInterestMethod.EditValue = LoanInterestMethod.StraightLine

            spnInterestRate.Value = 0D
            spnTerms.Value = 0D
            spnPrincipalAmount.Value = 0D
            spnInterestAmount.Value = 0D
            spnTotalLoanAmount.Value = 0D
            spnPrincipalAmortization.Value = 0D
            spnInterestAmortization.Value = 0D
            spnTotalAmortization.Value = 0D

            txtRemark.Text = String.Empty
            StatusText = String.Empty

        Finally
            _isPushingValues = False
        End Try

    End Sub

    Public Sub ShowMessageBox(message As String) Implements IEmployeeLoanView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub ShowErrorBox(message As String) Implements IEmployeeLoanView.ShowError
        ShowError(message)
    End Sub

    ' ========================================================
    ' DETAILS WINDOW
    ' ========================================================
    Public Sub ShowLoanDetails(detail As EmployeeLoanDetailModel) _
        Implements IEmployeeLoanView.ShowLoanDetails

        If detail Is Nothing Then Return

        Using dlg As New frmLoanDetails(detail)
            dlg.ShowDialog(Me)
        End Using

    End Sub

    ' ========================================================
    ' EMPLOYEE GRID
    ' ========================================================
    Private Async Sub gridviewEmployeeList_FocusedRowChanged(
        sender As Object,
        e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewEmployeeList.FocusedRowChanged

        Await SelectFocusedEmployeeAsync()

    End Sub

    Private Async Sub gridviewEmployeeList_Click(sender As Object, e As EventArgs) _
        Handles gridviewEmployeeList.Click

        Await SelectFocusedEmployeeAsync()

    End Sub

    Private Async Function SelectFocusedEmployeeAsync() As Task

        If _isEditing Then Return
        If gridviewEmployeeList.FocusedRowHandle < 0 Then Return

        Dim recordId = gridviewEmployeeList.GetFocusedRowCellValue("RecordId")
        If recordId Is Nothing Then Return

        Dim empNo = gridviewEmployeeList.GetFocusedRowCellValue("EmployeeNo")?.ToString()
        Dim fullName = gridviewEmployeeList.GetFocusedRowCellValue("FullName")?.ToString()

        Try
            Await _presenter.SelectEmployeeAsync(CInt(recordId), empNo, fullName)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Function

    Private Async Sub rgEmployeeFilter_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles rgEmployeeFilter.SelectedIndexChanged

        If _presenter Is Nothing OrElse _isEditing Then Return

        Try
            Await _presenter.SetEmployeeFilterAsync(rgEmployeeFilter.EditValue?.ToString())
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    ' ========================================================
    ' LOAN GRID
    ' ========================================================
    Private Sub gridviewLoanList_FocusedRowChanged(
        sender As Object,
        e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewLoanList.FocusedRowChanged

        SelectFocusedLoan()

    End Sub

    Private Sub gridviewLoanList_Click(sender As Object, e As EventArgs) _
        Handles gridviewLoanList.Click

        SelectFocusedLoan()

    End Sub

    Private Sub SelectFocusedLoan()

        If _isEditing Then Return
        If gridviewLoanList.FocusedRowHandle < 0 Then Return

        Dim id = gridviewLoanList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectLoan(CInt(id))

    End Sub

    ' Double-click sa isang row = buksan agad ang Details.
    ' Mas mabilis kaysa i-click ang row tapos hanapin pa ang button.
    Private Async Sub gridviewLoanList_DoubleClick(sender As Object, e As EventArgs) _
        Handles gridviewLoanList.DoubleClick

        If _isEditing Then Return
        If gridviewLoanList.FocusedRowHandle < 0 Then Return

        Try
            Await _presenter.ShowDetailsAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    Private Async Sub rgLoanStatusFilter_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles rgLoanStatusFilter.SelectedIndexChanged

        If _presenter Is Nothing OrElse _isEditing Then Return

        Try
            Await _presenter.SetLoanStatusFilterAsync(rgLoanStatusFilter.EditValue?.ToString())
        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

    ' ========================================================
    ' LIVE RECOMPUTE TRIGGERS
    ' ========================================================
    Private Sub RecomputeTriggers_EditValueChanged(sender As Object, e As EventArgs) _
        Handles spnPrincipalAmount.EditValueChanged,
                spnInterestRate.EditValueChanged,
                spnTerms.EditValueChanged,
                spnTotalAmortization.EditValueChanged,
                rgInterestMethod.SelectedIndexChanged

        If _isPushingValues Then Return
        If _presenter Is Nothing Then Return

        _isPushingValues = True
        Try
            _presenter.Recompute()
        Finally
            _isPushingValues = False
        End Try

    End Sub

    Private Sub rgAmortizationMethod_SelectedIndexChanged(sender As Object, e As EventArgs) _
        Handles rgAmortizationMethod.SelectedIndexChanged

        If _isPushingValues Then Return
        If _presenter Is Nothing Then Return

        _isPushingValues = True
        Try
            _presenter.OnAmortizationMethodChanged()
        Finally
            _isPushingValues = False
        End Try

    End Sub

    ' ========================================================
    ' COMMAND BUTTONS
    ' ========================================================
    Private Async Sub wbpMainCommands_ButtonClick(sender As Object, e As ButtonEventArgs) _
        Handles wbpMainCommands.ButtonClick

        Dim tag = e.Button.Properties.Tag?.ToString().Trim()

        Try
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

                Case "Delete"
                    If _isEditing Then Return

                    Dim confirm = XtraMessageBox.Show(
                        "Are you sure you want to delete this loan?" & Environment.NewLine &
                        "The amortization schedule will be deleted as well.",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.DeleteAsync()
                    End If

                Case "Details"
                    Await _presenter.ShowDetailsAsync()

                Case "Toggle"
                    If _isEditing Then Return

                    Dim confirmToggle = XtraMessageBox.Show(
                        "Change the status of this loan?" & Environment.NewLine &
                        "An inactive loan is skipped during payroll deduction.",
                        "Confirm",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question)

                    If confirmToggle = DialogResult.Yes Then
                        Await _presenter.ToggleStatusAsync()
                    End If

                Case "Refresh"
                    If _isEditing Then Return
                    Await _presenter.LoadLoanListAsync()

            End Select

        Catch ex As Exception
            ShowError(ex.Message)
        End Try

    End Sub

End Class