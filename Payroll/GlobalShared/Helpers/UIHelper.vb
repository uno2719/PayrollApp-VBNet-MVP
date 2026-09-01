Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils

Namespace GlobalShared.Helpers
    Public Module UIHelper

        Sub New()
            DevExpress.UserSkins.BonusSkins.Register()
            DevExpress.Skins.SkinManager.EnableFormSkins()
        End Sub

        Public Sub InitializeGrid(view As GridView)
            view.OptionsBehavior.Editable = False
            view.OptionsView.ShowAutoFilterRow = True
            view.OptionsView.ShowGroupPanel = False
            view.OptionsSelection.EnableAppearanceFocusedCell = False
            view.FocusRectStyle = DrawFocusRectStyle.RowFocus

            view.BestFitColumns()
        End Sub

        Public Sub ShowInfo(msg As String)
            DevExpress.XtraEditors.XtraMessageBox.Show(msg, "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ' ✅ Currency format
        Public Function FormatCurrency(amount As Decimal) As String
            Return amount.ToString("₱#,##0.00")
        End Function

    End Module
End Namespace
