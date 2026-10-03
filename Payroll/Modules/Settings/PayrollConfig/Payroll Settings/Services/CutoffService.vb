Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace PayrollSettings.Services

    Public Class CutoffService
        Implements ICutoffService

        Private ReadOnly _repository As ICutoffRepository

        Public Sub New(repository As ICutoffRepository)
            _repository = repository
        End Sub

        Public Function GetAllAsync() As Task(Of List(Of CutoffModel)) Implements ICutoffService.GetAllAsync
            Return _repository.GetAllAsync()
        End Function

        Public Async Function SaveAsync(item As CutoffModel, userName As String) As Task(Of PayrollSettingsSaveResult) Implements ICutoffService.SaveAsync
            If item.CutoffEnd < item.CutoffStart Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Cutoff End date cannot be earlier than Cutoff Start date."}
            End If

            If String.IsNullOrWhiteSpace(item.CycleType) Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = "Cycle Type is required."}
            End If

            Dim overlaps = Await _repository.OverlapExistsAsync(item.CycleType, item.CutoffStart, item.CutoffEnd, item.CutoffID)
            If overlaps Then
                Return New PayrollSettingsSaveResult With {.Success = False, .ErrorMessage = $"This date range overlaps an existing {item.CycleType} Cutoff."}
            End If

            If item.CutoffID = 0 Then
                Await _repository.InsertAsync(item, userName)
            Else
                Await _repository.UpdateAsync(item, userName)
            End If

            Return New PayrollSettingsSaveResult With {.Success = True}
        End Function

        Public Function GenerateForYearAsync(cycleType As String, year As Integer, userName As String) As Task(Of Integer) _
            Implements ICutoffService.GenerateForYearAsync

            Dim periods = BuildPeriodsForYear(cycleType, year)
            Return _repository.BulkInsertAsync(periods, userName)
        End Function

        Private Function BuildPeriodsForYear(cycleType As String, year As Integer) As List(Of CutoffModel)
            Dim periods As New List(Of CutoffModel)

            Select Case cycleType
                Case "Monthly"
                    For monthNum = 1 To 12
                        Dim start = New Date(year, monthNum, 1)
                        Dim [end] = New Date(year, monthNum, Date.DaysInMonth(year, monthNum))
                        periods.Add(NewPeriod(cycleType, year, start, [end]))
                    Next

                Case "SemiMonthly"
                    For monthNum = 1 To 12
                        Dim firstStart = New Date(year, monthNum, 1)
                        Dim firstEnd = New Date(year, monthNum, 15)
                        periods.Add(NewPeriod(cycleType, year, firstStart, firstEnd))

                        Dim secondStart = New Date(year, monthNum, 16)
                        Dim secondEnd = New Date(year, monthNum, Date.DaysInMonth(year, monthNum))
                        periods.Add(NewPeriod(cycleType, year, secondStart, secondEnd))
                    Next

                Case "Weekly"
                    Dim cursor = New Date(year, 1, 1)
                    While cursor.Year <= year
                        Dim weekEnd = cursor.AddDays(6)
                        If cursor.Year <> year AndAlso weekEnd.Year <> year Then Exit While
                        periods.Add(NewPeriod(cycleType, year, cursor, weekEnd))
                        cursor = weekEnd.AddDays(1)
                        If cursor.Year > year Then Exit While
                    End While

                Case Else
                    Throw New PayrollCutoffGenerateException(
                        $"Batch generation isn't supported for '{cycleType}' — add those Cutoffs one at a time instead.")
            End Select

            Return periods
        End Function

        Private Function NewPeriod(cycleType As String, year As Integer, start As Date, [end] As Date) As CutoffModel
            Return New CutoffModel With {
                .CycleType = cycleType,
                .CutoffYear = year,
                .CutoffStart = start,
                .CutoffEnd = [end],
                .CutoffLabel = $"{cycleType} {start:MMM d} - {[end]:MMM d, yyyy}",
                .Status = CutoffStatus.Draft
            }
        End Function

    End Class

    Public Class PayrollCutoffGenerateException
        Inherits Exception
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
    End Class

End Namespace