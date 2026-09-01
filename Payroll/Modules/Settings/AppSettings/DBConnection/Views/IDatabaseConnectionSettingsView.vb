Namespace DBConnection.Views
    Public Interface IDatabaseConnectionSettingsView
        Property ServerAddress As String
        Property DatabaseName As String
        Property SqlUsername As String
        Property SqlPassword As String

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
        Sub ShowTestConnectionResult(success As Boolean, message As String)
    End Interface
End Namespace