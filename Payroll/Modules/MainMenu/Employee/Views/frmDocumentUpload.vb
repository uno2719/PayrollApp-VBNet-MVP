' ============================================================
' Modules/MainMenu/Employee/Views/frmDocumentUpload.vb
' ============================================================
' Category picker BAGO ang pagpili ng file: ito ang unang hakbang ng Add sa 201 Files tab. Ibinabalik lang ang
' napiling category at remarks; ang pagpili ng file at ang pag-upload ay ginagawa ng ucEmployeesDocuments.
' ============================================================
Imports DevExpress.XtraEditors

Public Class frmDocumentUpload

    Private ReadOnly _categories As List(Of Employee.Models.DocumentCategoryModel)
    Private ReadOnly _employeeName As String

    Public ReadOnly Property CategoryId As Integer
        Get
            If lookCategory.EditValue Is Nothing OrElse lookCategory.EditValue Is DBNull.Value Then Return 0
            Return CInt(lookCategory.EditValue)
        End Get
    End Property

    Public ReadOnly Property CategoryName As String
        Get
            Return lookCategory.Text
        End Get
    End Property

    Public ReadOnly Property Remarks As String
        Get
            Return memoRemarks.Text.Trim()
        End Get
    End Property

    Public Sub New(categories As List(Of Employee.Models.DocumentCategoryModel), employeeName As String)
        InitializeComponent()
        _categories = categories
        _employeeName = employeeName
    End Sub

    Private Sub frmDocumentUpload_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblEmployee.Text = If(String.IsNullOrWhiteSpace(_employeeName), "Employee", _employeeName)

        With lookCategory.Properties
            .DataSource = _categories
            .ValueMember = "CategoryId"
            .DisplayMember = "CategoryName"
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo("CategoryName", "Category"))
            .ShowHeader = False
            .NullText = "Select a category..."
        End With
    End Sub

    Private Sub btnChoose_Click(sender As Object, e As EventArgs) Handles btnChoose.Click
        If CategoryId <= 0 Then
            XtraMessageBox.Show(Me, "Choose a category first.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            lookCategory.Focus()
            Return
        End If

        If memoRemarks.Text.Trim().Length > 500 Then
            XtraMessageBox.Show(Me, "Remarks is too long (maximum 500 characters).", Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        DialogResult = DialogResult.OK
    End Sub

End Class
