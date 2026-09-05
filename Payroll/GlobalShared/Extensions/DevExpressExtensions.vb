Imports System.Runtime.CompilerServices
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraEditors

Namespace GlobalShared.Extensions
    Public Module DevExpressExtensions

        ' Extension para makuha agad ang DTO sa focused row ng Grid
        <Extension()>
        Public Function GetFocusedRowData(Of T)(view As GridView) As T
            Return CType(view.GetFocusedRow(), T)
        End Function

        ' Extension para mabilisang setup ng Currency format sa TextEdit
        <Extension()>
        Public Sub SetAsCurrency(edit As TextEdit)
            edit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric
            edit.Properties.Mask.EditMask = "c2" ' Currency with 2 decimals
            edit.Properties.Mask.UseMaskAsDisplayFormat = True
        End Sub

        ' Extension para i-clear ang lahat ng TextEdits sa loob ng isang Container
        <Extension()>
        Public Sub ClearAllTextEdits(container As Control)
            For Each ctrl As Control In container.Controls
                If TypeOf ctrl Is TextEdit Then
                    DirectCast(ctrl, TextEdit).EditValue = Nothing
                End If
                If ctrl.HasChildren Then ClearAllTextEdits(ctrl)
            Next
        End Sub

        ' Para sa SearchLookUpEdit — clear ang selected value
        <Extension()>
        Public Sub ClearValue(sle As SearchLookUpEdit)
            sle.EditValue = Nothing
        End Sub

        ' Extension para hindi na mag-auto-populate ng LAHAT ng properties sa popup
        ' grid ng isang SearchLookUpEdit — ikaw mismo ang magsasabi kung anong
        ' fields/captions ang ipapakita, kahit anong model type ang bound.
        <Extension()>
        Public Sub ConfigureLookupColumns(
            sle As SearchLookUpEdit,
            ParamArray columns() As (FieldName As String, Caption As String))

            With sle.Properties.View
                .OptionsBehavior.AutoPopulateColumns = False
                .Columns.Clear()
                For Each col In columns
                    .Columns.AddVisible(col.FieldName, col.Caption)
                Next
            End With

        End Sub

    End Module
End Namespace
