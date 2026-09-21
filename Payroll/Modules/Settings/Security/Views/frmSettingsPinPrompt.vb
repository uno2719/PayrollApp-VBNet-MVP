Public Class frmSettingsPinPrompt

    Public ReadOnly Property EnteredPin As String
        Get
            Return txtPin.Text
        End Get
    End Property

    Private Sub frmSettingsPinPrompt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPin.Properties.PasswordChar = "●"c
        txtPin.Focus()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class