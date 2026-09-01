Namespace DBConnection.Services

    ' Command-line tool lang ito - hindi kailangan ng DB connection dito,
    ' kaya gagana ito kahit sirang-sira ang connection settings mo.
    Public Module SettingsPinTool

        Public Function SetSettingsPin(newPin As String) As String

            If String.IsNullOrWhiteSpace(newPin) OrElse newPin.Length < 4 Then
                Return "The PIN must be at least 4 characters."
            End If

            Dim pinService As New SettingsPinService()
            pinService.SetPin(newPin)

            Return "Successfully set the Application Settings PIN."

        End Function

    End Module

End Namespace