Imports Payroll.Login.Views
Imports Payroll.Login.Presenters

Public Class frmChangePassword
    Implements IChangePasswordView

    Private _presenter As ChangePasswordPresenter

    Public Sub SetPresenter(presenter As ChangePasswordPresenter)
        _presenter = presenter
    End Sub

    Public Property CurrentPassword As String Implements IChangePasswordView.CurrentPassword
        Get
            Return txtCurrentPassword.Text
        End Get
        Set(value As String)
            txtCurrentPassword.Text = value
        End Set
    End Property

    Public Property NewPassword As String Implements IChangePasswordView.NewPassword
        Get
            Return txtNewPassword.Text
        End Get
        Set(value As String)
            txtNewPassword.Text = value
        End Set
    End Property

    Public Property ConfirmNewPassword As String Implements IChangePasswordView.ConfirmNewPassword
        Get
            Return txtConfirmPassword.Text
        End Get
        Set(value As String)
            txtConfirmPassword.Text = value
        End Set
    End Property

    Public Sub ShowError(message As String) Implements IChangePasswordView.ShowError
        lblErrorMessage.Text = message
    End Sub

    Public Sub CloseWithSuccess() Implements IChangePasswordView.CloseWithSuccess
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Async Sub btnChangePassword_Click(sender As Object, e As EventArgs) Handles btnChangePassword.Click
        btnChangePassword.Enabled = False
        Try
            Await _presenter.ChangePasswordAsync()
        Catch ex As Exception
            ShowError("An unexpected error occurred. Please try again.")
        Finally
            btnChangePassword.Enabled = True
        End Try
    End Sub

End Class