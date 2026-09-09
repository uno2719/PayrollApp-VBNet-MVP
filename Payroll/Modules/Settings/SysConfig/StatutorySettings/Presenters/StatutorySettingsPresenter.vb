Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Presenters

    Public Class StatutorySettingsPresenter

        Private ReadOnly _view As Views.IStatutorySettingsMaintenanceView
        Private ReadOnly _service As Services.IStatutorySettingsService
        Private ReadOnly _tableName As String
        Private ReadOnly _userName As String

        Private _selectedId As Integer = 0
        Private _isNewMode As Boolean = False
        Private _currentList As List(Of StatutoryBracketModel)

        Public Sub New(
            view As Views.IStatutorySettingsMaintenanceView,
            service As Services.IStatutorySettingsService,
            tableName As String,
            userName As String)

            _view = view
            _service = service
            _tableName = tableName
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            Await LoadListAsync()

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)
        End Function

        Private Async Function LoadListAsync() As Task
            _currentList = Await _service.GetAllAsync(_tableName)
            _view.BindList(_currentList)
        End Function

        Public Sub StartNew()

            _selectedId = 0
            _isNewMode = True

            _view.ClearFields()
            _view.IsActive = True
            _view.SetFormMode(True, True)

        End Sub

        ' Tinatawag ng View kapag pinili ang isang row - display lang,
        ' hindi pa edit mode, kagaya ng LookupPresenter.
        Public Sub SelectItem(id As Integer)

            _selectedId = id
            _isNewMode = False

            Dim selected = _currentList?.FirstOrDefault(
                Function(x) x.Id = id)

            If selected IsNot Nothing Then
                _view.SalaryFrom = selected.SalaryFrom
                _view.SalaryTo = selected.SalaryTo
                _view.EEShare = selected.EEShare
                _view.EEContriType = selected.EEContriType
                _view.ERShare = selected.ERShare
                _view.ERContriType = selected.ERContriType
                _view.ECCAmount = selected.ECCAmount
                _view.EEMPF = selected.EEMPF
                _view.ERMPF = selected.ERMPF
                _view.IsActive = selected.IsActive
            End If

            _view.SetFormMode(False, False)

        End Sub

        Public Sub StartEdit()

            If _selectedId = 0 Then
                _view.ShowError("Please select a bracket first.")
                Return
            End If

            _isNewMode = False
            _view.SetFormMode(True, False)

        End Sub

        Public Async Function SaveAsync() As Task

            Dim item As New StatutoryBracketModel With {
                .Id = _selectedId,
                .SalaryFrom = _view.SalaryFrom,
                .SalaryTo = _view.SalaryTo,
                .EEShare = _view.EEShare,
                .EEContriType = If(_view.EEContriType, "").Trim(),
                .ERShare = _view.ERShare,
                .ERContriType = If(_view.ERContriType, "").Trim(),
                .ECCAmount = _view.ECCAmount,
                .EEMPF = _view.EEMPF,
                .ERMPF = _view.ERMPF,
                .IsActive = _view.IsActive
            }

            Dim result = Await _service.SaveAsync(_tableName, item, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            If _isNewMode Then
                _view.ShowMessage("Bracket added.")
            Else
                _view.ShowMessage("Bracket updated.")
            End If

            _selectedId = 0
            _isNewMode = False

            _view.ClearFields()
            _view.SetFormMode(False, False)

            Await LoadListAsync()

        End Function

        Public Sub CancelEdit()

            If _selectedId > 0 AndAlso Not _isNewMode Then
                SelectItem(_selectedId)
            Else
                _selectedId = 0
                _isNewMode = False

                _view.ClearFields()
                _view.SetFormMode(False, False)
            End If

        End Sub

        Public Async Function ToggleActiveSelectedAsync() As Task

            If _selectedId = 0 Then
                _view.ShowError("Please select a bracket first.")
                Return
            End If

            Dim newStatus = Not _view.IsActive

            Await _service.SetActiveStatusAsync(_tableName, _selectedId, newStatus, _userName)

            _view.IsActive = newStatus

            If newStatus Then
                _view.ShowMessage("Bracket reactivated.")
            Else
                _view.ShowMessage("Bracket deactivated.")
            End If

            Await LoadListAsync()

        End Function

        ' =============================================
        ' EXCEL IMPORT - dumadaan pa rin bawat row sa
        ' _service.SaveAsync (kaya same overlap/range check gaya
        ' ng manual Add), kaya hindi na kailangan ng bagong
        ' validation path para lang sa import.
        ' =============================================
        Public Async Function ImportFromExcelAsync(filePath As String) As Task

            Dim rows As List(Of Dictionary(Of String, String))
            Try
                rows = ExcelHelper.ReadWorksheet(filePath)
            Catch ex As Exception
                _view.ShowError($"Hindi mabuksan ang file: {ex.Message}")
                Return
            End Try

            If rows.Count = 0 Then
                _view.ShowError("Walang laman na row na nabasa sa Excel file.")
                Return
            End If

            Dim successCount As Integer = 0
            Dim errors As New List(Of String)

            For i = 0 To rows.Count - 1
                ' +2 dahil row 1 sa Excel ay header, at 1-based ang Excel rows
                Dim excelRowNumber = i + 2
                Dim row = rows(i)

                Try
                    Dim item As New StatutoryBracketModel With {
                        .SalaryFrom = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "SalaryFrom")),
                        .SalaryTo = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "SalaryTo")),
                        .EEShare = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "EEShare")),
                        .EEContriType = ExcelHelper.GetValueOrEmpty(row, "EEContriType").Trim(),
                        .ERShare = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "ERShare")),
                        .ERContriType = ExcelHelper.GetValueOrEmpty(row, "ERContriType").Trim(),
                        .ECCAmount = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "ECCAmount")),
                        .EEMPF = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "EEMPF")),
                        .ERMPF = ExcelHelper.ParseDecimalOrDefault(ExcelHelper.GetValueOrEmpty(row, "ERMPF")),
                        .IsActive = ExcelHelper.ParseBoolOrDefault(ExcelHelper.GetValueOrEmpty(row, "IsActive"))
                    }

                    Dim result = Await _service.SaveAsync(_tableName, item, _userName)

                    If result.Success Then
                        successCount += 1
                    Else
                        errors.Add($"Row {excelRowNumber}: {result.ErrorMessage}")
                    End If

                Catch ex As Exception
                    errors.Add($"Row {excelRowNumber}: {ex.Message}")
                End Try
            Next

            Await LoadListAsync()

            Dim summary As New System.Text.StringBuilder()
            summary.AppendLine($"{successCount} of {rows.Count} row(s) na-import.")

            If errors.Count > 0 Then
                summary.AppendLine()
                summary.AppendLine("Mga hindi na-import:")
                For Each Err In errors
                    summary.AppendLine($"- {Err()}")
                Next
                _view.ShowError(summary.ToString())
            Else
                _view.ShowMessage(summary.ToString())
            End If

        End Function


        ' =============================================
        ' EXCEL EXPORT - kasabay na "template" (headers lang kapag
        ' walang laman ang list) at data backup (headers + current
        ' rows kapag may laman na).
        ' =============================================
        Public Async Function ExportToExcelAsync(filePath As String, sheetLabel As String) As Task

            Dim headers As New List(Of String) From {
                "SalaryFrom", "SalaryTo", "EEShare", "EEContriType",
                "ERShare", "ERContriType", "ECCAmount", "EEMPF", "ERMPF", "IsActive"
            }

            Dim data = Await _service.GetAllAsync(_tableName)

            Dim rows As New List(Of List(Of String))
            For Each item In data
                rows.Add(New List(Of String) From {
                    item.SalaryFrom.ToString(),
                    item.SalaryTo.ToString(),
                    item.EEShare.ToString(),
                    item.EEContriType,
                    item.ERShare.ToString(),
                    item.ERContriType,
                    item.ECCAmount.ToString(),
                    item.EEMPF.ToString(),
                    item.ERMPF.ToString(),
                    item.IsActive.ToString()
                })
            Next

            Try
                ExcelHelper.WriteWorksheet(filePath, sheetLabel, headers, rows)
                _view.ShowMessage($"Na-export sa: {filePath}")
            Catch ex As Exception
                _view.ShowError($"Hindi ma-export: {ex.Message}")
            End Try

        End Function

    End Class

End Namespace