Imports Payroll.Login.Services

''' <summary>
''' Password re-check bago ang permanenteng Delete. Ang Verifier ang
''' nagde-decide kung tama ang password (hindi dito nakasulat ang auth logic).
''' Mananatiling bukas habang mali ang password; magsasara lang kapag tama,
''' Cancel, o kapag sinabi ng Verifier na wala nang saysay mag-retry (locked).
''' </summary>
Public Class frmConfirmPassword

    Public Property Verifier As Func(Of String, Task(Of PasswordCheckResult))

    Private ReadOnly _itemDescription As String
    Private ReadOnly _isSoftDelete As Boolean

    ' isSoftDelete = True kapag hindi totoong binubura ang record (hal. Employee: IsDeleted flag lang),
    ' para hindi "permanently" ang sabihin ng message.
    Public Sub New(itemDescription As String, Optional isSoftDelete As Boolean = False)
        InitializeComponent()
        _itemDescription = itemDescription
        _isSoftDelete = isSoftDelete
    End Sub

    Private Sub frmConfirmPassword_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblMessage.Text = If(_isSoftDelete,
            $"Enter your password to delete {_itemDescription}.",
            $"Enter your password to permanently delete {_itemDescription}.")
        txtPassword.Properties.PasswordChar = "●"c
        lblError.Text = String.Empty
        txtPassword.Focus()
    End Sub

    Private Async Sub btnConfirm_Click(sender As Object, e As EventArgs) Handles btnConfirm.Click
        If Verifier Is Nothing Then
            DialogResult = DialogResult.Cancel
            Close()
            Return
        End If

        btnConfirm.Enabled = False
        btnCancel.Enabled = False
        txtPassword.Enabled = False
        lblError.Text = String.Empty

        Dim result As PasswordCheckResult
        Try
            result = Await Verifier(txtPassword.Text)
        Catch ex As Exception
            result = New PasswordCheckResult With {.Success = False, .CloseDialog = True, .ErrorMessage = ex.Message}
        End Try

        If result.Success Then
            DialogResult = DialogResult.OK
            Close()
            Return
        End If

        If result.CloseDialog Then
            DevExpress.XtraEditors.XtraMessageBox.Show(Me, result.ErrorMessage, "Password Check",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.Cancel
            Close()
            Return
        End If

        lblError.Text = result.ErrorMessage
        txtPassword.Enabled = True
        btnConfirm.Enabled = True
        btnCancel.Enabled = True
        txtPassword.Text = String.Empty
        txtPassword.Focus()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class
