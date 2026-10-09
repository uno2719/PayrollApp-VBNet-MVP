Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Presenters

    Public Class PayCyclePresenter

        Private ReadOnly _view As Views.IPayCycleMaintenanceView
        Private ReadOnly _service As Services.IPayCycleService
        Private ReadOnly _userName As String

        Private _list As List(Of PayCycleModel) = New List(Of PayCycleModel)()
        Private _selectedType As String = ""
        Private _isEditing As Boolean = False

        ' Pang-guard laban sa magkakapatong na async selection (hal. FocusedRowChanged
        ' habang may isa pang SelectItemAsync na hindi pa tapos) - ang pinakahuling
        ' tawag lang ang pinapayagang mag-apply ng lock state sa view.
        Private _selectTicket As Integer = 0

        Public Sub New(view As Views.IPayCycleMaintenanceView, service As Services.IPayCycleService, userName As String)
            _view = view
            _service = service
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            _isEditing = False
            _list = Await _service.GetAllAsync()
            _view.BindList(_list)

            Dim target = If(_list.Any(Function(c) c.PayCycleType = _selectedType),
                            _selectedType,
                            _list.FirstOrDefault()?.PayCycleType)

            If target Is Nothing Then
                _view.ClearFields()
                _view.SetFormMode(False)
                Return
            End If

            _view.FocusCycle(target)
            Await SelectItemAsync(target)
        End Function

        Public Async Function SelectItemAsync(payCycleType As String) As Task
            If _isEditing Then Return

            Dim cycle = _list.FirstOrDefault(Function(c) c.PayCycleType = payCycleType)
            If cycle Is Nothing Then Return

            _selectedType = cycle.PayCycleType
            _selectTicket += 1
            Dim ticket = _selectTicket

            _view.PayCycleType = cycle.PayCycleType
            _view.RateBasis = cycle.RateBasis
            _view.IsActive = cycle.IsActive
            _view.BindPeriods(ClonePeriods(cycle.Periods))
            _view.SetFormMode(False)
            UpdatePreview()

            Dim reason = Await _service.GetRateBasisLockReasonAsync(cycle.PayCycleType)
            If ticket <> _selectTicket Then Return

            _view.SetRateBasisLock(reason IsNot Nothing, reason)
        End Function

        Public Async Function StartEditAsync() As Task
            If String.IsNullOrEmpty(_selectedType) Then
                _view.ShowError("Please select a pay cycle first.")
                Return
            End If

            ' Sariwang check sa database - baka may na-process na habang nakabukas ang screen
            Dim reason = Await _service.GetRateBasisLockReasonAsync(_selectedType)

            _isEditing = True
            _view.SetRateBasisLock(reason IsNot Nothing, reason)
            _view.SetFormMode(True)
        End Function

        Public Async Function SaveAsync() As Task
            If Not _isEditing Then Return

            _view.CommitPendingEdits()

            Dim item As New PayCycleModel With {
                .PayCycleType = _selectedType,
                .RateBasis = _view.RateBasis,
                .IsActive = _view.IsActive,
                .Periods = _view.GetPeriods()
            }

            Dim result = Await _service.SaveAsync(item, _userName)

            ' May employee pang naka-assign sa pay cycle na idi-deactivate - hihingan ng kumpirmasyon
            If Not result.Success AndAlso result.NeedsConfirmation Then
                If Not _view.ConfirmDeactivate(result.ErrorMessage) Then Return   ' nasa edit mode pa rin
                result = Await _service.SaveAsync(item, _userName, True)
            End If

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            _view.ShowMessage($"{_selectedType} pay cycle updated.")
            Await LoadAsync()
        End Function

        Public Async Function CancelEditAsync() As Task
            _isEditing = False
            Await SelectItemAsync(_selectedType)
        End Function

        ''' <summary>
        ''' Tinatawag ng view tuwing may nagbago sa pattern grid o sa rate basis.
        ''' Pareho ang generator at validator na gagamitin ng Cutoff generation,
        ''' kaya ang nakikita sa preview ay ang mismong lalabas pag nag-generate.
        ''' </summary>
        Public Sub UpdatePreview()
            Dim rows = _view.GetPeriods()

            If _view.RateBasis = PayRateBasis.DailyRate Then
                _view.ShowRateBasisNote("Pay per cutoff = daily rate x days worked. Payroll Input Entry asks for Days Worked for employees on this pay cycle.")
            Else
                _view.ShowRateBasisNote($"Pay per cutoff = basic salary / {Math.Max(rows.Count, 1)} (cutoffs per month).")
            End If

            Dim problem = PayCyclePatternHelper.Validate(rows)
            If problem IsNot Nothing Then
                _view.ShowPatternPreview(problem, True)
                Return
            End If

            Dim today = Date.Today
            Dim fmt = AppConstants.DisplayDateFormat
            Dim parts = PayCyclePatternHelper.BuildForMonth(rows, today.Year, today.Month).
                Select(Function(p) $"{p.CutoffStart.ToString(fmt)} - {p.CutoffEnd.ToString(fmt)} (paid {p.PayDate.ToString(fmt)})").
                ToList()

            _view.ShowPatternPreview($"Preview for {today:MMMM yyyy}:  {String.Join("   |   ", parts)}", False)
        End Sub

        Private Shared Function ClonePeriods(source As List(Of PayCyclePeriodModel)) As List(Of PayCyclePeriodModel)
            Return If(source, New List(Of PayCyclePeriodModel)()).
                Select(Function(p) New PayCyclePeriodModel With {
                    .PayCyclePeriodId = p.PayCyclePeriodId,
                    .PayCycleId = p.PayCycleId,
                    .PeriodNo = p.PeriodNo,
                    .FromDay = p.FromDay,
                    .FromMonthOffset = p.FromMonthOffset,
                    .ToDay = p.ToDay,
                    .ToMonthOffset = p.ToMonthOffset,
                    .PayDay = p.PayDay
                }).ToList()
        End Function

    End Class

End Namespace
