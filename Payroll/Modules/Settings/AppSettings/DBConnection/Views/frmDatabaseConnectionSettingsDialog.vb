Public Class frmDatabaseConnectionSettingsDialog

    Public Sub New(settingsView As Payroll.DBConnection.Views.ucDatabaseConnectionSettings)
        InitializeComponent()

        settingsView.Dock = DockStyle.Fill
        ' settingsView.Padding = New Padding(10)
        Me.Controls.Add(settingsView)
    End Sub

    Private Sub frmDatabaseConnectionSettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Padding = New Padding(10)
    End Sub
End Class