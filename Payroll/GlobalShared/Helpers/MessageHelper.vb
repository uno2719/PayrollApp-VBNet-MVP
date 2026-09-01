Imports DevExpress.XtraEditors

Namespace GlobalShared.Helpers
    Public Module MessageHelper
        Public Sub ShowInfo(msg As String)
            XtraMessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Public Sub ShowWarning(msg As String)
            XtraMessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Sub

        Public Sub ShowError(msg As String)
            XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub
    End Module
End Namespace
