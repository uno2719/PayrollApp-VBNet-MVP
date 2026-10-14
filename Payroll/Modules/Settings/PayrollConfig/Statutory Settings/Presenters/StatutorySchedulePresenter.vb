Imports System.Globalization
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models
Imports Payroll.StatutorySettings.Services
Imports Payroll.StatutorySettings.Views

Namespace StatutorySettings.Presenters

    ''' <summary>
    ''' Deduction Schedule: bawat active na pay cycle ay may matrix (agency x period) ng share % ng
    ''' buwanang contribution. Habang nasa Edit mode, nananatili sa memorya ang mga binago sa bawat pay cycle
    ''' (pwedeng lumipat-lipat ng pay cycle), at sabay-sabay silang sine-save sa Update.
    ''' </summary>
    Public Class StatutorySchedulePresenter

        Private ReadOnly _view As IStatutoryScheduleView
        Private ReadOnly _service As IStatutoryScheduleService
        Private ReadOnly _userName As String

        Private _setup As New StatutoryScheduleSetup()
        Private _selectedCycle As String = ""
        Private _isEditing As Boolean = False

        Public Sub New(view As IStatutoryScheduleView, service As IStatutoryScheduleService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        ' =============================================
        ' LOAD
        ' =============================================
        Public Async Function LoadAsync() As Task
            _isEditing = False

            Await _service.EnsureDefaultsAsync(_userName)
            _setup = Await _service.GetSetupAsync()

            _view.BindCycles(_setup.Cycles)
            _view.VoluntaryMode = _setup.VoluntaryMode

            If _setup.Cycles.Count = 0 Then
                _selectedCycle = ""
                _view.ClearFields()
                _view.ShowPreview("There is no active pay cycle with a cutoff pattern. Set one up in Payroll Settings > Pay Cycle first.", True)
                _view.SetFormMode(False)
                Return
            End If

            Dim target = _selectedCycle
            If Not _setup.Cycles.Any(Function(c) String.Equals(c.PayCycleType, target, StringComparison.OrdinalIgnoreCase)) Then
                target = _setup.Cycles(0).PayCycleType
            End If

            _view.FocusCycle(target)
            ShowCycle(target)
            _view.SetFormMode(False)
        End Function

        Private Sub ShowCycle(cycleType As String)
            Dim info = FindCycle(cycleType)
            If info Is Nothing Then Return

            _selectedCycle = info.PayCycleType
            _view.BindSchedule(info, _setup.Rows(info.PayCycleType))
            UpdatePreview()
        End Sub

        Private Function FindCycle(cycleType As String) As StatutoryScheduleCycleInfo
            For Each c In _setup.Cycles
                If String.Equals(c.PayCycleType, cycleType, StringComparison.OrdinalIgnoreCase) Then Return c
            Next
            Return Nothing
        End Function

        ''' <summary>Ang laman ng grid ngayon ay isinusulat pabalik sa working copy ng napiling pay cycle.</summary>
        Private Sub CaptureCurrent()
            If String.IsNullOrEmpty(_selectedCycle) Then Return

            _view.CommitPendingEdits()
            Dim rows = _view.GetSchedule()
            If rows.Count > 0 Then _setup.Rows(_selectedCycle) = rows
        End Sub

        ' =============================================
        ' SELECTION
        ' =============================================
        Public Sub SelectCycle(payCycleType As String)
            If String.Equals(payCycleType, _selectedCycle, StringComparison.OrdinalIgnoreCase) Then Return
            If FindCycle(payCycleType) Is Nothing Then Return

            If _isEditing Then CaptureCurrent()
            ShowCycle(payCycleType)
        End Sub

        ' =============================================
        ' EDIT / SAVE / CANCEL
        ' =============================================
        Public Sub StartEdit()
            If _setup.Cycles.Count = 0 Then
                _view.ShowError("There is no active pay cycle with a cutoff pattern to set a schedule for.")
                Return
            End If

            _isEditing = True
            _view.SetFormMode(True)
        End Sub

        Public Async Function SaveAsync() As Task
            CaptureCurrent()
            _setup.VoluntaryMode = _view.VoluntaryMode

            Try
                Dim result = Await _service.SaveAsync(_setup, _userName)

                If Not result.Success Then
                    UpdatePreview()     ' ipakita sa preview kung alin ang mali
                    _view.ShowError(result.ErrorMessage)
                    Return      ' nasa Edit mode pa rin, para maayos ang mali
                End If

                _view.ShowMessage("Deduction schedule saved.")
                Await LoadAsync()

            Catch ex As Exception
                _view.ShowError("Couldn't save the deduction schedule. " & ex.Message)
            End Try
        End Function

        Public Async Function CancelEditAsync() As Task
            Await LoadAsync()
        End Function

        ' =============================================
        ' QUICK SET (First / Last / Split / Not deducted)
        ' =============================================
        Public Sub ApplyPresetToFocused(preset As SchedulePreset)
            If Not _isEditing Then Return

            Dim agency = _view.FocusedAgency
            Dim info = FindCycle(_selectedCycle)
            If String.IsNullOrEmpty(agency) OrElse info Is Nothing Then
                _view.ShowError("Select an agency row first.")
                Return
            End If

            _view.SetAgencyShares(agency, StatutoryScheduleRules.ApplyPreset(preset, info.Periods.Count))
            UpdatePreview()
        End Sub

        ' =============================================
        ' PREVIEW - "ano ang kakaltasin sa bawat payday"
        ' =============================================
        Public Sub UpdatePreview()
            Dim info = FindCycle(_selectedCycle)
            If info Is Nothing Then
                _view.ShowPreview("", False)
                Return
            End If

            Dim rows As List(Of StatutoryScheduleRow) = If(_isEditing, _view.GetSchedule(), _setup.Rows(info.PayCycleType))
            Dim lines As New List(Of String)()
            Dim hasProblem = False

            For i = 0 To info.Periods.Count - 1
                Dim period = info.Periods(i)
                Dim parts As New List(Of String)()

                For Each row In rows
                    If i < row.Shares.Count AndAlso row.Shares(i) > 0D Then
                        parts.Add($"{row.AgencyName} {row.Shares(i).ToString("0.##", CultureInfo.InvariantCulture)}%")
                    End If
                Next

                Dim what = If(parts.Count = 0, "no statutory deductions", String.Join(", ", parts))
                lines.Add($"Pays {period.PayDayText} (cutoff {period.RangeText}):  {what}")
            Next

            For Each row In rows
                If Not StatutoryScheduleRules.IsValidTotal(row.Total) Then
                    hasProblem = True
                    lines.Add($"Check {row.AgencyName}: the shares total {row.Total.ToString("0.##", CultureInfo.InvariantCulture)}%. They must total 100%, or 0% if not deducted.")
                End If
            Next

            _view.ShowPreview(String.Join(Environment.NewLine, lines), hasProblem)
        End Sub

    End Class

End Namespace
