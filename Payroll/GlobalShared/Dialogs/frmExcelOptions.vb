' Maliit na reusable popup - dito lang dinideklara ang 2 pilian (Import/Export)
' kapag ni-click ang "Excel" button ng kahit anong module. Tinatawag ito ng
' ExcelHelper.PromptImportOrExport() sa ExcelHelper.vb - hindi direkta ng View.

Public Class frmExcelOptions

    Public Sub New(Optional title As String = "Excel Options")
        InitializeComponent()
        Me.Text = title
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

End Class