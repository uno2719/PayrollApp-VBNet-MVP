' ============================================================
' Modules/MainMenu/Employee/Views/frmEmployeeHistory.vb
' ============================================================
' READ-ONLY na popup: movement log ng isang empleyado (tblEmployeeMovement), pinakabago muna.
' Walang Presenter - basa lang ang ginagawa nito (parehong dahilan ng frmEmployeeLoansQuickView).
' Ang unang linya na "Joined" ay galing sa Date Joined ng Employment tab, hindi sa log.
' Ang mga petsa sa Employment tab bago ang Phase 2 ay hindi awtomatikong naisasalin sa log.
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmEmployeeHistory

    Public Class HistoryRow
        Public Property EffectiveDate As Date
        Public Property Action As String
        Public Property FromValue As String
        Public Property ToValue As String
        Public Property Remarks As String
        Public Property RecordedBy As String
        Public Property RecordedOn As Date?
    End Class

    Private ReadOnly _service As Employee.Services.IEmployeeService
    Private ReadOnly _recordId As Integer
    Private ReadOnly _employeeNo As String
    Private ReadOnly _fullName As String
    Private ReadOnly _dateJoined As Date?

    Public Sub New(service As Employee.Services.IEmployeeService,
                   recordId As Integer,
                   employeeNo As String,
                   fullName As String,
                   dateJoined As Date?)

        InitializeComponent()

        _service = service
        _recordId = recordId
        _employeeNo = employeeNo
        _fullName = fullName
        _dateJoined = dateJoined
    End Sub

    Private Async Sub frmEmployeeHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblEmployee.Text = If(String.IsNullOrWhiteSpace(_employeeNo), _fullName, $"{_fullName}  ({_employeeNo})")

        With gridviewHistory
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With

        Await LoadAsync()
    End Sub

    Private Async Function LoadAsync() As Task
        Try
            Dim movements = Await _service.GetMovementHistoryAsync(_recordId)

            Dim rows = movements.Select(Function(m) New HistoryRow With {
                .EffectiveDate = m.EffectiveDate,
                .Action = Employee.Models.MovementTypes.HistoryLabel(m.MovementType),
                .FromValue = m.FromValue,
                .ToValue = m.ToValue,
                .Remarks = m.Remarks,
                .RecordedBy = m.CreatedBy,
                .RecordedOn = m.CreatedAt
            }).ToList()

            Dim loggedCount = rows.Count

            If _dateJoined.HasValue Then
                rows.Add(New HistoryRow With {
                    .EffectiveDate = _dateJoined.Value.Date,
                    .Action = "Joined",
                    .Remarks = "From the Employment tab (Date Joined)"
                })
            End If

            gridHistory.DataSource = rows
            FormatColumns()

            lblSummary.Text = If(loggedCount = 0,
                                 "No movements recorded yet.",
                                 $"{loggedCount} {If(loggedCount = 1, "movement", "movements")} recorded")

        Catch ex As Exception
            XtraMessageBox.Show(Me, ex.Message, "Employment History", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub FormatColumns()
        SetColumn("EffectiveDate", "Effective Date", DevExpress.Utils.FormatType.DateTime, "MM/dd/yyyy")
        SetColumn("Action", "Action", Nothing, Nothing)
        SetColumn("FromValue", "From", Nothing, Nothing)
        SetColumn("ToValue", "To", Nothing, Nothing)
        SetColumn("Remarks", "Remarks", Nothing, Nothing)
        SetColumn("RecordedBy", "Recorded By", Nothing, Nothing)
        SetColumn("RecordedOn", "Recorded On", DevExpress.Utils.FormatType.DateTime, "MM/dd/yyyy hh:mm tt")
    End Sub

    Private Sub SetColumn(fieldName As String, caption As String,
                          formatType As DevExpress.Utils.FormatType?, formatString As String)
        Dim col = gridviewHistory.Columns(fieldName)
        If col Is Nothing Then Return
        col.Caption = caption
        If formatType.HasValue Then
            col.DisplayFormat.FormatType = formatType.Value
            col.DisplayFormat.FormatString = formatString
        End If
    End Sub

End Class
