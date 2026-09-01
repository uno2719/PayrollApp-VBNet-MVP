Namespace DBConnection.Services
    Public Interface ISettingsPinService
        Function HasPin() As Boolean
        Function VerifyPin(enteredPin As String) As Boolean
        Sub SetPin(newPin As String)
    End Interface
End Namespace