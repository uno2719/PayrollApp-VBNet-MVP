' ============================================================
' Modules/MainMenu/Employee/Views/frmEmployeeLoansQuickView.vb
' ============================================================
' READ-ONLY na popup: mga loan ng isang empleyado (Active / Inactive / Completed / All).
' Walang Presenter - basa lang ang ginagawa nito (parehong dahilan ng frmLoanDetails).
' Pagdaragdag/pag-edit ng loan ay nasa Loans module pa rin.
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports Payroll.GlobalShared.Models

Public Class frmEmployeeLoansQuickView

    Public Class LoanRow
        Public Property Loan As String
        Public Property Paid As String
        Public Property TotalAmount As Decimal
        Public Property Balance As Decimal
        Public Property NextDate As Date?
        Public Property NextAmount As Decimal
        Public Property Status As String
    End Class

    Private ReadOnly _loanService As Loans.Services.IEmployeeLoanService
    Private ReadOnly _employeeRecordId As Integer
    Private ReadOnly _employeeNo As String
    Private ReadOnly _fullName As String
    Private _initializing As Boolean = True

    ''' <summary>True kung pinindot ang "Open in Loans module" (isinara ang dialog para pumunta sa Loans).</summary>
    Public Property OpenInLoansRequested As Boolean

    Public Sub New(loanService As Loans.Services.IEmployeeLoanService,
                   employeeRecordId As Integer,
                   employeeNo As String,
                   fullName As String)

        InitializeComponent()

        _loanService = loanService
        _employeeRecordId = employeeRecordId
        _employeeNo = employeeNo
        _fullName = fullName
    End Sub

    Private Sub btnOpenLoans_Click(sender As Object, e As EventArgs) Handles btnOpenLoans.Click
        OpenInLoansRequested = True
        DialogResult = DialogResult.OK
    End Sub

    Private Async Sub frmEmployeeLoansQuickView_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblEmployee.Text = If(String.IsNullOrWhiteSpace(_employeeNo), _fullName, $"{_fullName}  ({_employeeNo})")

        ' Makikita lang ang button kung may access ang user sa Loans module (Admin laging meron).
        btnOpenLoans.Visible = GlobalShared.Security.PermissionService.CanView(GlobalShared.Constants.ModuleCodes.Main_Loan)

        cboShow.Properties.Items.AddRange(New Object() {"Active", "Inactive", "Completed", "All"})
        cboShow.SelectedIndex = 0
        _initializing = False

        With gridviewLoans
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

        Await LoadAsync()
    End Sub

    Private Async Sub cboShow_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboShow.SelectedIndexChanged
        If _initializing Then Return
        Await LoadAsync()
    End Sub

    Private Async Function LoadAsync() As Task
        Try
            Dim filter = If(cboShow.EditValue?.ToString(), "Active")
            Dim loans = Await _loanService.GetByEmployeeAsync(_employeeRecordId, filter)

            Dim rows = loans.Select(Function(l) New LoanRow With {
                .Loan = If(String.IsNullOrWhiteSpace(l.LoanDescription), l.LoanCode, l.LoanDescription),
                .Paid = $"{l.PaidInstallments} of {l.TotalInstallments}",
                .TotalAmount = l.TotalLoanAmount,
                .Balance = l.Balance,
                .NextDate = l.NextDeductionDate,
                .NextAmount = l.NextDeductionAmount,
                .Status = l.Status
            }).ToList()

            gridLoans.DataSource = rows
            FormatColumns()

            Dim totalBalance = loans.Sum(Function(l) l.Balance)
            lblSummary.Text = $"{rows.Count} {If(rows.Count = 1, "loan", "loans")}   |   Total balance: ₱{totalBalance:N2}"

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, "Loans", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub FormatColumns()
        SetColumn("Loan", "Loan", Nothing, Nothing)
        SetColumn("Paid", "Paid", Nothing, Nothing)
        SetColumn("TotalAmount", "Total Amount", DevExpress.Utils.FormatType.Numeric, "n2")
        SetColumn("Balance", "Balance", DevExpress.Utils.FormatType.Numeric, "n2")
        SetColumn("NextDate", "Next Deduction", DevExpress.Utils.FormatType.DateTime, "MM/dd/yyyy")
        SetColumn("NextAmount", "Next Amount", DevExpress.Utils.FormatType.Numeric, "n2")
        SetColumn("Status", "Status", Nothing, Nothing)
    End Sub

    Private Sub SetColumn(fieldName As String, caption As String,
                          formatType As DevExpress.Utils.FormatType?, formatString As String)
        Dim col = gridviewLoans.Columns(fieldName)
        If col Is Nothing Then Return
        col.Caption = caption
        If formatType.HasValue Then
            col.DisplayFormat.FormatType = formatType.Value
            col.DisplayFormat.FormatString = formatString
        End If
    End Sub

End Class
