Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Extensions
Imports Payroll.GlobalShared.Models
Imports Payroll.IncomeTaxTable.Presenters
Imports Payroll.IncomeTaxTable.Views

Public Class ucIncomeTaxTable
    Implements IIncomeTaxTableMaintenanceView
    Implements IAsyncLoadable

    Private _presenter As IncomeTaxTablePresenter
    Private _isEditing As Boolean = False
    Private _isNewRecord As Boolean = False
    Private _tabTitle As String = "Yearly"

    ' index 0 = separator
    Private Const BTN_NEW As Integer = 1
    Private Const BTN_EDIT As Integer = 2
    Private Const BTN_DELETE As Integer = 3
    ' index 4 = separator
    Private Const BTN_REFRESH As Integer = 5
    ' index 6 = separator

    ' Tinatawag ito ng AppComposition kapag ginagawa yung 5 instances
    ' (Yearly/Monthly/Semi-Monthly/Weekly/Daily) - parehong pattern
    ' gaya ng Statutory Settings.
    Public Sub SetPresenter(presenter As IncomeTaxTablePresenter, tabTitle As String)
        _presenter = presenter
        _tabTitle = tabTitle
        lblTabPageTitle.Text = tabTitle
    End Sub

    ' =============================================
    ' BREADCRUMB / TITLE - dynamic per instance
    ' =============================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Settings > Payroll Setup > Income Tax Table > {_tabTitle}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return _tabTitle
        End Get
    End Property

    ' =============================================
    ' LOAD (lazy - isang beses lang per tab, gaya ng Statutory)
    ' =============================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        SetupCommandImages()
        SetupGrid()
        SetupNumericFields()

        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            DisplayValidationError(ex.Message)
        End Try

    End Function

    Private Sub SetupGrid()
        With gridviewIncomeTaxList
            .OptionsBehavior.Editable = False
            .OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None
            .OptionsView.ShowAutoFilterRow = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
        End With
    End Sub

    ' Currency mask sa peso-value fields, plain 2-decimal number sa
    ' Tax Percentage (hindi ito peso, kaya walang currency symbol) -
    ' isang beses lang i-set sa Load, hindi kailangan ulitin sa Designer.
    Private Sub SetupNumericFields()
        txtSalaryFrom.SetAsCurrency()
        txtSalaryTo.SetAsCurrency()
        txtFixTaxAmount.SetAsCurrency()

        txtTaxPercentage.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
        txtTaxPercentage.Properties.Mask.EditMask = "n2"
        txtTaxPercentage.Properties.Mask.UseMaskAsDisplayFormat = True
    End Sub

    Private Sub SetupCommandImages()

        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
        wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"

        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
        wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"

        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ImageOptions.Image = My.Resources.icon_delete_32
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.ToolTip = "Delete Selected"

        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ImageOptions.Image = My.Resources.icon_refresh_24
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.ToolTip = "Reload from Database"

        ' Shared Excel SVG na ginagamit na rin ng ibang module - kaya
        ' hindi na kailangan ng duplicate na resx entry para dito.
        btnExcel.ImageOptions.SvgImage = My.Resources.excel2_svgrep

    End Sub

    ' =============================================
    ' IIncomeTaxTableMaintenanceView - FORM FIELDS
    ' Peso-value fields: TextEdit na naka-Numeric mask (SetAsCurrency),
    ' kaya Decimal na agad ang EditValue - walang manual string parsing.
    ' =============================================
    Public Property SalaryFrom As Decimal Implements IIncomeTaxTableMaintenanceView.SalaryFrom
        Get
            Return If(txtSalaryFrom.EditValue Is Nothing, 0D, Convert.ToDecimal(txtSalaryFrom.EditValue))
        End Get
        Set(value As Decimal)
            txtSalaryFrom.EditValue = value
        End Set
    End Property

    Public Property SalaryTo As Decimal Implements IIncomeTaxTableMaintenanceView.SalaryTo
        Get
            Return If(txtSalaryTo.EditValue Is Nothing, 0D, Convert.ToDecimal(txtSalaryTo.EditValue))
        End Get
        Set(value As Decimal)
            txtSalaryTo.EditValue = value
        End Set
    End Property

    Public Property TaxPercentage As Decimal Implements IIncomeTaxTableMaintenanceView.TaxPercentage
        Get
            Return If(txtTaxPercentage.EditValue Is Nothing, 0D, Convert.ToDecimal(txtTaxPercentage.EditValue))
        End Get
        Set(value As Decimal)
            txtTaxPercentage.EditValue = value
        End Set
    End Property

    Public Property FixTaxAmount As Decimal Implements IIncomeTaxTableMaintenanceView.FixTaxAmount
        Get
            Return If(txtFixTaxAmount.EditValue Is Nothing, 0D, Convert.ToDecimal(txtFixTaxAmount.EditValue))
        End Get
        Set(value As Decimal)
            txtFixTaxAmount.EditValue = value
        End Set
    End Property

    Public Property IsActive As Boolean Implements IIncomeTaxTableMaintenanceView.IsActive
        Get
            Return chkActive.Checked
        End Get
        Set(value As Boolean)
            chkActive.Checked = value
        End Set
    End Property

    ' =============================================
    ' IIncomeTaxTableMaintenanceView - GRID
    ' =============================================
    Public Sub BindList(items As List(Of IncomeTaxBracketModel)) Implements IIncomeTaxTableMaintenanceView.BindList
        gridconIncomeTaxList.DataSource = items
    End Sub

    ' =============================================
    ' IIncomeTaxTableMaintenanceView - STATE / UX
    ' =============================================
    Public Sub SetFormMode(isEditable As Boolean, isNewRecord As Boolean) _
        Implements IIncomeTaxTableMaintenanceView.SetFormMode

        _isEditing = isEditable
        _isNewRecord = isNewRecord

        '========================================
        ' Fields
        '========================================
        txtSalaryFrom.Properties.ReadOnly = Not isEditable
        txtSalaryTo.Properties.ReadOnly = Not isEditable
        txtTaxPercentage.Properties.ReadOnly = Not isEditable
        txtFixTaxAmount.Properties.ReadOnly = Not isEditable
        chkActive.Properties.ReadOnly = Not isEditable

        gridconIncomeTaxList.Enabled = Not isEditable

        '========================================
        ' NEW / SAVE / UPDATE BUTTON
        '========================================
        If isEditable Then
            If isNewRecord Then
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Save"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_save_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Save New Entry"
            Else
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " Update"
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_saveAs_24
                wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Amend Record"
            End If
        Else
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.Caption = " New"
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ImageOptions.Image = My.Resources.icon_add_property_24_png
            wbpMainCommands.Buttons.Item(BTN_NEW).Properties.ToolTip = "Add New Entry"
        End If

        '========================================
        ' EDIT / CANCEL BUTTON
        '========================================
        If isEditable Then
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Cancel"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_cancel_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Cancel"
        Else
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.Caption = " Edit"
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ImageOptions.Image = My.Resources.icon_edit_property_24
            wbpMainCommands.Buttons.Item(BTN_EDIT).Properties.ToolTip = "Edit Selected"
        End If

        '========================================
        ' DELETE / REFRESH
        '========================================
        wbpMainCommands.Buttons.Item(BTN_DELETE).Properties.Enabled = Not isEditable
        wbpMainCommands.Buttons.Item(BTN_REFRESH).Properties.Enabled = Not isEditable

    End Sub

    Public Sub ClearFields() Implements IIncomeTaxTableMaintenanceView.ClearFields
        txtSalaryFrom.EditValue = Nothing
        txtSalaryTo.EditValue = Nothing
        txtTaxPercentage.EditValue = Nothing
        txtFixTaxAmount.EditValue = Nothing
        chkActive.Checked = True
    End Sub

    Public Sub DisplayInfo(message As String) Implements IIncomeTaxTableMaintenanceView.ShowMessage
        ShowMessage(message)
    End Sub

    Public Sub DisplayValidationError(message As String) Implements IIncomeTaxTableMaintenanceView.ShowError
        ShowError(message)
    End Sub

    ' =============================================
    ' GRID SELECTION
    ' =============================================
    Private Sub gridviewIncomeTaxList_FocusedRowChanged(
        sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewIncomeTaxList.FocusedRowChanged

        If _isEditing Then Return

        Dim id = gridviewIncomeTaxList.GetFocusedRowCellValue("Id")
        If id Is Nothing Then Return

        _presenter.SelectItem(CInt(id))
    End Sub

    Private Sub gridviewIncomeTaxList_Click(sender As Object, e As EventArgs) _
        Handles gridviewIncomeTaxList.Click

        If _isEditing Then Return
        If gridviewIncomeTaxList.SelectedRowsCount = 0 Then Return

        Dim id = gridviewIncomeTaxList.GetFocusedRowCellValue("Id")
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
                If Not _isEditing Then
                    Dim action = If(IsActive, "deactivate", "reactivate")

                    Dim confirm = XtraMessageBox.Show($"Are you sure you want to {action} this bracket?",
                                                        "Confirm",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question)

                    If confirm = DialogResult.Yes Then
                        Await _presenter.ToggleActiveSelectedAsync()
                    End If
                End If

            Case "Refresh"
                If Not _isEditing Then
                    Await _presenter.LoadAsync()
                End If

        End Select

    End Sub

    ' =============================================
    ' EXCEL IMPORT / EXPORT (hiwalay sa wbpMainCommands, kaya
    ' sarili niyang SimpleButton sa loob ng grpDetails) - iisang
    ' button lang, may lalabas na prompt para pumili ng Import o
    ' Export.
    ' =============================================
    Private Async Sub btnExcel_Click(sender As Object, e As EventArgs) _
        Handles btnExcel.Click

        If _isEditing Then Return

        Select Case ExcelHelper.PromptImportOrExport($"{_tabTitle} - Import Data Options")

            Case ExcelAction.Import
                Dim filePath = ExcelHelper.PromptOpenExcelFile($"Import {_tabTitle} Brackets from Excel")
                If String.IsNullOrEmpty(filePath) Then Return

                Try
                    Await _presenter.ImportFromExcelAsync(filePath)
                Catch ex As Exception
                    DisplayValidationError(ex.Message)
                End Try

            Case ExcelAction.Export
                Dim defaultFileName = $"{_tabTitle}_Template.xlsx"
                Dim filePath = ExcelHelper.PromptSaveExcelFile(defaultFileName, $"Export {_tabTitle} Brackets to Excel")
                If String.IsNullOrEmpty(filePath) Then Return

                Try
                    Await _presenter.ExportToExcelAsync(filePath, _tabTitle)
                Catch ex As Exception
                    DisplayValidationError(ex.Message)
                End Try

        End Select

    End Sub

End Class