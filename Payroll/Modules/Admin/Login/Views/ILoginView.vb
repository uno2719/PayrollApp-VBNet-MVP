Namespace Login.Views
    Public Interface ILoginView
        Property Username As String
        Property Password As String
        Property RememberMe As Boolean

        Sub ShowError(message As String)
        Sub HideError()
        Sub CloseWithSuccess()
        Sub ForcePasswordChange()
    End Interface
End Namespace