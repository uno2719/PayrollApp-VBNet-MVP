' ============================================================
' Modules/MainMenu/Employee/Views/ucEmployeesDocuments.vb
' ============================================================
' Ang "201 Files" tab ng Employee screen. HIWALAY ito sa Edit/New flow ng ibang tabs: ang upload/delete ay
' gumagana kapag Idle ang screen (ang ucEmployees ang nagdi-disable ng tab habang New/Edit).
' Walang Presenter - tumatawag lang sa IEmployeeFileService (parehong dahilan ng frmEmployeeLoansQuickView).
'
' Daloy ng Add:  frmDocumentUpload (category + remarks)  ->  OpenFileDialog (isa o higit pang file)  ->  upload.
' Delete:        Yes/No (default No)  ->  password ng kasalukuyang user  ->  soft delete.
' Lazy load:     naglo-load lang kapag nakikita ang tab (VisibleChanged), hindi sa bawat pagpili ng empleyado.
' ============================================================
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Diagnostics
Imports System.IO

Public Class ucEmployeesDocuments
    Inherits DevExpress.XtraEditors.XtraUserControl

    Public Class DocRow
        Public Property DocumentId As Integer
        Public Property Category As String
        Public Property FileName As String
        Public Property Remarks As String
        Public Property Size As String
        Public Property UploadedBy As String
        Public Property UploadedOn As DateTime
    End Class

    ''' <summary>Itinataas pagkatapos ng matagumpay na Add/Delete (para ma-refresh ang "Profile %" ng header).</summary>
    Public Event DocumentsChanged As EventHandler

    Private _service As Employee.Services.IEmployeeFileService
    Private _recordId As Integer
    Private _employeeName As String = String.Empty
    Private _loaded As Boolean
    Private _busy As Boolean

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub Initialize(service As Employee.Services.IEmployeeFileService)
        _service = service
        ConfigureGrid()
        UpdateButtons()
    End Sub

    ' =============================================
    ' TINATAWAG NG ucEmployees
    ' =============================================
    Public Sub SetRecord(recordId As Integer)
        _recordId = recordId
        _loaded = False
        gridDocuments.DataSource = Nothing
        lblSummary.Text = String.Empty
        UpdateButtons()

        If recordId > 0 AndAlso Visible Then LoadInBackground()
    End Sub

    Public Sub SetEmployeeName(name As String)
        _employeeName = If(name, String.Empty)
    End Sub

    Private Sub ucEmployeesDocuments_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible AndAlso Not _loaded AndAlso _recordId > 0 Then LoadInBackground()
    End Sub

    ' Fire-and-forget na wrapper (ang RefreshAsync mismo ang humuhuli ng mga error at nagpapakita ng mensahe).
    Private Async Sub LoadInBackground()
        Await RefreshAsync()
    End Sub

    ' =============================================
    ' PERMISSIONS
    ' =============================================
    Private Shared Function CanView() As Boolean
        Return GlobalShared.Security.PermissionService.CanView(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents)
    End Function

    Private Shared Function CanEdit() As Boolean
        Return GlobalShared.Security.PermissionService.CanEdit(GlobalShared.Constants.ModuleCodes.Main_Employees_Documents)
    End Function

    ' =============================================
    ' GRID
    ' =============================================
    Private Sub ConfigureGrid()
        With gridviewDocuments
            .OptionsBehavior.Editable = False
            .OptionsView.ShowGroupPanel = False
            .OptionsView.ColumnAutoWidth = True
            .OptionsSelection.EnableAppearanceFocusedCell = False
            .FocusRectStyle = DrawFocusRectStyle.RowFocus
        End With
    End Sub

    Private Sub FormatColumns()
        SetCaption("Category", "Category", 120)
        SetCaption("FileName", "File", 220)
        SetCaption("Remarks", "Remarks", 180)
        SetCaption("Size", "Size", 60)
        SetCaption("UploadedBy", "Uploaded By", 90)
        SetCaption("UploadedOn", "Uploaded On", 120)

        Dim colDate = gridviewDocuments.Columns("UploadedOn")
        If colDate IsNot Nothing Then
            colDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
            colDate.DisplayFormat.FormatString = "MM/dd/yyyy hh:mm tt"
        End If

        Dim colId = gridviewDocuments.Columns("DocumentId")
        If colId IsNot Nothing Then colId.Visible = False

        ' I-group ayon sa Category
        Dim colCat = gridviewDocuments.Columns("Category")
        If colCat IsNot Nothing Then colCat.GroupIndex = 0
        gridviewDocuments.ExpandAllGroups()
    End Sub

    Private Sub SetCaption(fieldName As String, caption As String, width As Integer)
        Dim col = gridviewDocuments.Columns(fieldName)
        If col Is Nothing Then Return
        col.Caption = caption
        col.Width = width
    End Sub

    Private Function SelectedRow() As DocRow
        Return TryCast(gridviewDocuments.GetFocusedRow(), DocRow)
    End Function

    Private Sub gridviewDocuments_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) _
        Handles gridviewDocuments.FocusedRowChanged
        UpdateButtons()
    End Sub

    Private Sub UpdateButtons()
        Dim hasRecord As Boolean = _recordId > 0 AndAlso _service IsNot Nothing AndAlso Not _busy
        Dim hasRow As Boolean = hasRecord AndAlso SelectedRow() IsNot Nothing

        btnAdd.Enabled = hasRecord AndAlso CanEdit()
        btnOpen.Enabled = hasRow AndAlso CanView()
        btnSaveCopy.Enabled = hasRow AndAlso CanView()
        btnDelete.Enabled = hasRow AndAlso CanEdit()
    End Sub

    Private Sub SetBusy(busy As Boolean)
        _busy = busy
        UseWaitCursor = busy
        UpdateButtons()
    End Sub

    ' =============================================
    ' LOAD
    ' =============================================
    Private Async Function RefreshAsync() As Task
        If _service Is Nothing OrElse _recordId <= 0 Then Return

        If Not CanView() Then
            gridDocuments.DataSource = Nothing
            lblSummary.Text = "You don't have access to Employee Documents."
            _loaded = True
            UpdateButtons()
            Return
        End If

        Dim requested As Integer = _recordId
        Try
            Dim docs = Await _service.GetDocumentsAsync(requested)
            If requested <> _recordId Then Return ' nagbago na ang napiling empleyado

            Dim rows = docs.Select(Function(d) New DocRow With {
                .DocumentId = d.DocumentId,
                .Category = d.CategoryName,
                .FileName = d.FileName,
                .Remarks = d.Remarks,
                .Size = d.SizeText,
                .UploadedBy = d.CreatedBy,
                .UploadedOn = d.CreatedAt
            }).ToList()

            gridDocuments.DataSource = rows
            FormatColumns()

            lblSummary.Text = If(rows.Count = 0, "No files yet.",
                                 $"{rows.Count} {If(rows.Count = 1, "file", "files")}")
            _loaded = True

        Catch ex As Exception
            lblSummary.Text = String.Empty
            XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        UpdateButtons()
    End Function

    ' =============================================
    ' ADD
    ' =============================================
    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If _recordId <= 0 OrElse _service Is Nothing Then Return

        Dim categoryId As Integer
        Dim categoryName As String
        Dim remarks As String

        Try
            Dim categories = Await _service.GetCategoriesAsync()
            If categories.Count = 0 Then
                XtraMessageBox.Show(FindForm(), "No document categories are set up yet. Run the Phase 3 SQL script.",
                                    "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Hakbang 1: category (at remarks) BAGO ang pagpili ng file
            Using dlg As New frmDocumentUpload(categories, _employeeName)
                If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
                categoryId = dlg.CategoryId
                categoryName = dlg.CategoryName
                remarks = dlg.Remarks
            End Using
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        ' Hakbang 2: ang (mga) file
        Dim paths As String()
        Using ofd As New OpenFileDialog()
            ofd.Title = $"Choose file(s) for: {categoryName}"
            ofd.Multiselect = True
            ofd.CheckFileExists = True
            ofd.Filter = "Documents (*.pdf;*.jpg;*.jpeg;*.png;*.docx)|*.pdf;*.jpg;*.jpeg;*.png;*.docx"
            If ofd.ShowDialog(FindForm()) <> DialogResult.OK Then Return
            paths = ofd.FileNames
        End Using

        ' Hakbang 3: upload
        SetBusy(True)
        Try
            Dim result = Await _service.AddDocumentsAsync(_recordId, categoryId, remarks, paths)

            If result.Failures.Count > 0 Then
                Dim msg As String = $"{result.Saved} of {paths.Length} file(s) uploaded." & vbCrLf & vbCrLf &
                                    "Not uploaded:" & vbCrLf & String.Join(vbCrLf, result.Failures)
                XtraMessageBox.Show(FindForm(), msg, "201 Files", MessageBoxButtons.OK,
                                    If(result.Saved > 0, MessageBoxIcon.Warning, MessageBoxIcon.Error))
            End If

            If result.Saved > 0 Then
                Await RefreshAsync()
                RaiseEvent DocumentsChanged(Me, EventArgs.Empty)
            End If

        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    ' =============================================
    ' OPEN / SAVE A COPY
    ' =============================================
    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        OpenSelected()
    End Sub

    Private Sub gridviewDocuments_DoubleClick(sender As Object, e As EventArgs) Handles gridviewDocuments.DoubleClick
        ' Ang double-click sa group row ay nag-e-expand lang; ang file row lang ang binubuksan.
        If SelectedRow() IsNot Nothing AndAlso btnOpen.Enabled Then OpenSelected()
    End Sub

    Private Async Sub OpenSelected()
        Dim row = SelectedRow()
        If row Is Nothing OrElse _busy Then Return

        SetBusy(True)
        Try
            Dim fullPath = Await _service.GetDocumentFullPathAsync(row.DocumentId)
            Process.Start(New ProcessStartInfo(fullPath) With {.UseShellExecute = True})
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Async Sub btnSaveCopy_Click(sender As Object, e As EventArgs) Handles btnSaveCopy.Click
        Dim row = SelectedRow()
        If row Is Nothing OrElse _busy Then Return

        Using sfd As New SaveFileDialog()
            sfd.FileName = row.FileName
            sfd.Title = "Save a copy"
            If sfd.ShowDialog(FindForm()) <> DialogResult.OK Then Return

            Dim target As String = sfd.FileName
            SetBusy(True)
            Try
                Dim source = Await _service.GetDocumentFullPathAsync(row.DocumentId)
                Await Task.Run(Sub() File.Copy(source, target, True))
            Catch ex As Exception
                XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                SetBusy(False)
            End Try
        End Using
    End Sub

    ' =============================================
    ' DELETE (soft, may password)
    ' =============================================
    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim row = SelectedRow()
        If row Is Nothing OrElse _busy Then Return

        Dim answer = XtraMessageBox.Show(FindForm(),
            $"Delete '{row.FileName}' ({row.Category})?" & vbCrLf & vbCrLf &
            "It will be removed from this list. The file itself stays in the Files Folder.",
            "Delete 201 File", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If answer <> DialogResult.Yes Then Return

        ' Kapareho ng ConfirmDelete ng ibang screens: password ng kasalukuyang user.
        Dim auth As Login.Services.IAuthenticationService =
            New Login.Services.AuthenticationService(New Login.Data.UserRepository())
        Dim username As String = GlobalShared.Constants.AppSession.CurrentUser

        Using dlg As New frmConfirmPassword($"the file '{row.FileName}'", True)
            dlg.Verifier = Function(password As String) auth.VerifyCurrentUserPasswordAsync(username, password)
            If dlg.ShowDialog(FindForm()) <> DialogResult.OK Then Return
        End Using

        SetBusy(True)
        Try
            Dim ok = Await _service.SoftDeleteDocumentAsync(row.DocumentId)
            If Not ok Then
                XtraMessageBox.Show(FindForm(), "Nothing was deleted. The file may already have been removed.",
                                    "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Await RefreshAsync()
            RaiseEvent DocumentsChanged(Me, EventArgs.Empty)
        Catch ex As Exception
            XtraMessageBox.Show(FindForm(), ex.Message, "201 Files", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            SetBusy(False)
        End Try
    End Sub

End Class
