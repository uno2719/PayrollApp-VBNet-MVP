Namespace DBConnection.Views
    Public Interface IDatabaseConnectionSettingsView
        Property ServerAddress As String
        Property DatabaseName As String
        Property AuthenticationType As Models.DbAuthenticationType
        Property SqlUsername As String
        Property SqlPassword As String

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
        Sub ShowTestConnectionResult(success As Boolean, message As String)

        ''' <summary>Yes/No na tanong (default: No). True kapag Yes.</summary>
        Function ConfirmSaveAnyway(message As String) As Boolean

        ''' <summary>Nilolock ang mga button habang sinusubukan ang connection at nagse-save.</summary>
        Sub SetBusy(isBusy As Boolean)
    End Interface
End Namespace