Public Class frmProfileMenu

    Public Event ChangePasswordRequested()
    Public Event LogoutRequested()

    Private Sub frmProfileMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblSeparator.Appearance.BackColor = Color.LightGray
        lblSeparator.Appearance.Options.UseBackColor = True

        lblLogout.ImageOptions.SvgImage = My.Resources.logout2_svg
    End Sub

    ' Parang totoong dropdown - kapag na-click sa labas o nawalan ng focus,
    ' isasara na lang ito nang tahimik.
    Private Sub frmProfileMenu_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()
    End Sub

    Private Sub lblChangePassword_Click(sender As Object, e As EventArgs) Handles lblChangePassword.Click
        RaiseEvent ChangePasswordRequested()
        Me.Close()
    End Sub

    Private Sub lblLogout_Click(sender As Object, e As EventArgs) Handles lblLogout.Click
        RaiseEvent LogoutRequested()
        Me.Close()
    End Sub

    ' Simpleng hover effect - konting polish lang para mas "dropdown-like" ang dating
    Private Sub lblChangePassword_MouseEnter(sender As Object, e As EventArgs) Handles lblChangePassword.MouseEnter
        SetHover(lblChangePassword, True)
    End Sub
    Private Sub lblChangePassword_MouseLeave(sender As Object, e As EventArgs) Handles lblChangePassword.MouseLeave
        SetHover(lblChangePassword, False)
    End Sub
    Private Sub lblLogout_MouseEnter(sender As Object, e As EventArgs) Handles lblLogout.MouseEnter
        SetHover(lblLogout, True)
    End Sub
    Private Sub lblLogout_MouseLeave(sender As Object, e As EventArgs) Handles lblLogout.MouseLeave
        SetHover(lblLogout, False)
    End Sub

    Private Sub SetHover(lbl As DevExpress.XtraEditors.LabelControl, isHovering As Boolean)
        If isHovering Then
            lbl.Appearance.BackColor = Color.WhiteSmoke
        Else
            lbl.Appearance.BackColor = Color.White
        End If
        lbl.Appearance.Options.UseBackColor = True
    End Sub

End Class