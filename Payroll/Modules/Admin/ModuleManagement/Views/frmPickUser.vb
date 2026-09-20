' ============================================================
' Modules/Admin/ModuleManagement/Views/frmPickUser.vb
' ============================================================
' Maliit na reusable picker dialog - ginawa natin ito mismo
' imbes na ipilit ang XtraInputBox, dahil magkaiba-iba pala ang
' available na overloads ng XtraInputBox.Show depende sa bersyon
' ng DevExpress na naka-install, at ilan sa mga ito ay hindi
' tumatanggap ng custom na RepositoryItem/dropdown. Sarili nating
' gawa na simpleng ComboBoxEdit + OK/Cancel ang pinaka-ligtas na
' paraan - walang overload guessing, garantisadong gagana.
' ============================================================
Public Class frmPickUser

    Private ReadOnly _displayItems As String()

    Public Sub New(prompt As String, choices As List(Of String))

        InitializeComponent()

        lblPrompt.Text = prompt
        _displayItems = choices.ToArray()

        cboUsers.Properties.Items.Clear()
        cboUsers.Properties.Items.AddRange(_displayItems)

        If _displayItems.Length > 0 Then
            cboUsers.SelectedIndex = 0
        End If

    End Sub

    ''' <summary>
    ''' Index (sa loob ng orihinal na "choices" list na ipinasa sa
    ''' constructor) ng piniling item - o Nothing kung wala pang
    ''' napili (dapat hindi mangyari kung OK ang pinindot, pero
    ''' depensa pa rin natin ito).
    ''' </summary>
    Public ReadOnly Property SelectedIndex As Integer?
        Get
            If cboUsers.SelectedIndex < 0 Then Return Nothing
            Return cboUsers.SelectedIndex
        End Get
    End Property

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If cboUsers.SelectedIndex < 0 Then
            DevExpress.XtraEditors.XtraMessageBox.Show(
                "Please select a user.", "Copy Access From",
                MessageBoxButtons.OK, MessageBoxIcon.Warning)
            DialogResult = DialogResult.None
            Return
        End If

        DialogResult = DialogResult.OK

    End Sub

End Class