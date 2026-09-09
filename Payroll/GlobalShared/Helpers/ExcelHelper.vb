Imports ClosedXML.Excel

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' Generic Excel read/write helper - ginagamit ito ng kahit anong module
    ''' (StatutorySettings ngayon, Master Data/Payroll Settings sa susunod) para
    ''' sa Import/Export feature. Dictionary lang (header name -> cell value) ang
    ''' binabalik nito per row - ang actual na mapping papunta sa specific Model
    ''' (StatutoryBracketModel, LookupModel, atbp.) ay nasa Presenter pa rin ng
    ''' bawat module, gaya ng existing SaveAsync() pattern - hindi nito kailangang
    ''' malaman ang shape ng bawat Model.
    ''' </summary>
    Public Module ExcelHelper

        ' =============================================
        ' FILE DIALOGS - iisa lang dito para consistent ang
        ' filter/title sa lahat ng module na gagamit nito.
        ' =============================================
        Public Function PromptOpenExcelFile(Optional title As String = "Select Excel File") As String
            Using dlg As New OpenFileDialog With {
                .Title = title,
                .Filter = "Excel Files (*.xlsx)|*.xlsx",
                .CheckFileExists = True
            }
                Return If(dlg.ShowDialog() = DialogResult.OK, dlg.FileName, Nothing)
            End Using
        End Function

        Public Function PromptSaveExcelFile(defaultFileName As String, Optional title As String = "Save Excel File") As String
            Using dlg As New SaveFileDialog With {
                .Title = title,
                .Filter = "Excel Files (*.xlsx)|*.xlsx",
                .FileName = defaultFileName,
                .DefaultExt = "xlsx",
                .AddExtension = True
            }
                Return If(dlg.ShowDialog() = DialogResult.OK, dlg.FileName, Nothing)
            End Using
        End Function

        ' =============================================
        ' READ - unang row (row 1) = headers, sunod na rows = data.
        ' Case-insensitive ang key ng dictionary (header name) para
        ' hindi masira ang import kung nagpalit ng letter case sa Excel.
        ' Nilaktawan din ang mga completely blank na row.
        ' =============================================
        Public Function ReadWorksheet(filePath As String) As List(Of Dictionary(Of String, String))

            Dim result As New List(Of Dictionary(Of String, String))

            Using workbook As New XLWorkbook(filePath)
                Dim ws = workbook.Worksheet(1)

                Dim lastRowUsed = ws.LastRowUsed()
                Dim lastColUsed = ws.LastColumnUsed()
                If lastRowUsed Is Nothing OrElse lastColUsed Is Nothing Then Return result

                Dim lastRow = lastRowUsed.RowNumber()
                Dim lastCol = lastColUsed.ColumnNumber()

                ' Row 1 = headers
                Dim headers As New List(Of String)
                For col = 1 To lastCol
                    headers.Add(ws.Cell(1, col).GetString().Trim())
                Next

                ' Row 2 pababa = data
                For r = 2 To lastRow

                    Dim isBlankRow As Boolean = True
                    For col = 1 To lastCol
                        If Not String.IsNullOrWhiteSpace(ws.Cell(r, col).GetString()) Then
                            isBlankRow = False
                            Exit For
                        End If
                    Next
                    If isBlankRow Then Continue For

                    Dim dict As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
                    For col = 1 To lastCol
                        Dim header = headers(col - 1)
                        If String.IsNullOrWhiteSpace(header) Then Continue For
                        dict(header) = ws.Cell(r, col).GetString().Trim()
                    Next
                    result.Add(dict)

                Next
            End Using

            Return result
        End Function

        ' =============================================
        ' WRITE - simpleng single-sheet export. Pareho itong
        ' ginagamit bilang "template" (headers lang, walang data)
        ' at bilang full data export (headers + current rows).
        ' =============================================
        Public Sub WriteWorksheet(filePath As String, sheetName As String, headers As List(Of String), rows As List(Of List(Of String)))

            Using workbook As New XLWorkbook()
                Dim ws = workbook.Worksheets.Add(SanitizeSheetName(sheetName))

                For col = 0 To headers.Count - 1
                    Dim cell = ws.Cell(1, col + 1)
                    cell.Value = headers(col)
                    cell.Style.Font.Bold = True
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray
                Next

                For r = 0 To rows.Count - 1
                    For c = 0 To rows(r).Count - 1
                        ws.Cell(r + 2, c + 1).Value = rows(r)(c)
                    Next
                Next

                ws.SheetView.FreezeRows(1)
                ws.Columns().AdjustToContents()

                workbook.SaveAs(filePath)
            End Using

        End Sub

        ' Excel sheet names: max 31 chars, bawal ang \ / ? * [ ] :
        Private Function SanitizeSheetName(name As String) As String
            Dim invalid = New Char() {"\"c, "/"c, "?"c, "*"c, "["c, "]"c, ":"c}
            Dim clean = name
            For Each ch In invalid
                clean = clean.Replace(ch, "-"c)
            Next
            Return If(clean.Length > 31, clean.Substring(0, 31), clean)
        End Function

        ' =============================================
        ' PARSING HELPERS - ginagamit ng Presenter pagka-map
        ' ng isang Dictionary row papunta sa specific Model.
        ' =============================================
        Public Function GetValueOrEmpty(row As Dictionary(Of String, String), key As String) As String
            Dim value As String = Nothing
            If row.TryGetValue(key, value) Then Return value
            Return String.Empty
        End Function

        Public Function ParseDecimalOrDefault(value As String, Optional defaultValue As Decimal = 0D) As Decimal
            Dim result As Decimal
            If Decimal.TryParse(value, result) Then Return result
            Return defaultValue
        End Function

        Public Function ParseBoolOrDefault(value As String, Optional defaultValue As Boolean = True) As Boolean
            If String.IsNullOrWhiteSpace(value) Then Return defaultValue

            Select Case value.Trim().ToUpperInvariant()
                Case "TRUE", "1", "YES", "ACTIVE", "Y"
                    Return True
                Case "FALSE", "0", "NO", "INACTIVE", "N"
                    Return False
                Case Else
                    Return defaultValue
            End Select
        End Function

    End Module

End Namespace