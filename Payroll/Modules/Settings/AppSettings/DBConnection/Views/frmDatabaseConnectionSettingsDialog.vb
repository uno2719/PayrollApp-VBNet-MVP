Public Class frmDatabaseConnectionSettingsDialog

    Private ReadOnly _settingsView As Payroll.DBConnection.Views.ucDatabaseConnectionSettings

    ''' <summary>True kapag nagbago ang Server o Database Name na na-save habang bukas ang dialog.</summary>
    Public ReadOnly Property TargetChanged As Boolean
        Get
            Return _settingsView.TargetChanged
        End Get
    End Property

    Public Sub New(settingsView As Payroll.DBConnection.Views.ucDatabaseConnectionSettings)
        InitializeComponent()

        _settingsView = settingsView

        settingsView.Dock = DockStyle.Fill
        ' settingsView.Padding = New Padding(10)
        Me.Controls.Add(settingsView)
    End Sub

    Private Sub frmDatabaseConnectionSettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Padding = New Padding(10)
    End Sub
End Class