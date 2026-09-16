' File: Modules/Settings/SysConfig/GeneralSettings/Presenters/GeneralSettingsPresenter.vb
Imports System.Linq
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Data

Namespace GeneralSettings.Presenters
    Public Class GeneralSettingsPresenter

        Private ReadOnly _view As Views.IGeneralSettingsView
        Private ReadOnly _service As Services.IGeneralSettingsService

        ' Direktang repository ang tinatanggap para sa dropdown lookups -
        ' parehong pattern ng CompanyPresenter na tumatanggap ng
        ' IEmployeeRepository para sa Contact Person picker. Hindi na
        ' kailangan ng bagong Service layer para sa read-only na listahan.
        Private ReadOnly _compensationRepository As ICompensationRepository
        Private ReadOnly _flaggedEntryRepository As IPayrollFlaggedEntryRepository

        Private ReadOnly _userName As String

        Private _current As GeneralSettingsModel

        Public Sub New(
            view As Views.IGeneralSettingsView,
            service As Services.IGeneralSettingsService,
            compensationRepository As ICompensationRepository,
            flaggedEntryRepository As IPayrollFlaggedEntryRepository,
            userName As String)

            _view = view
            _service = service
            _compensationRepository = compensationRepository
            _flaggedEntryRepository = flaggedEntryRepository
            _userName = userName
        End Sub

        Public Async Function LoadAsync() As Task
            ' I-bind muna ang dalawang code lists bago i-set ang mga Id -
            ' kailangan ito ng LookUpEdit para maipakita ang tamang
            ' selection (kapareho ng SetEmployeeList sa CompanyPresenter).
            Await LoadCodeListsAsync()

            _current = Await _service.GetAsync()
            If _current Is Nothing Then
                ' Bagong install pa - mga default na nakalagay na sa model
                ' ang ipapakita, at Insert ang mangyayari sa unang Save.
                _current = New GeneralSettingsModel()
            End If

            _view.BonusCeiling = _current.BonusCeiling
            _view.TotalDaysPerYear = _current.TotalDaysPerYear
            _view.WorkHourPerDay = _current.WorkHourPerDay
            _view.AmountPrecision = _current.AmountPrecision
            _view.PercentPrecision = _current.PercentPrecision

            _view.BasicSalaryCodePlusId = _current.BasicSalaryCodePlusId
            _view.BasicSalaryCodeMinusId = _current.BasicSalaryCodeMinusId
            _view.AbsentCodeId = _current.AbsentCodeId
            _view.LateInCodeId = _current.LateInCodeId
            _view.EarlyOutCodeId = _current.EarlyOutCodeId

            _view.SSSBasedOn = _current.SSSBasedOn
            _view.PhilHealthBasedOn = _current.PhilHealthBasedOn
        End Function

        Public Async Function SaveAsync() As Task
            _current.BonusCeiling = _view.BonusCeiling
            _current.TotalDaysPerYear = _view.TotalDaysPerYear
            _current.WorkHourPerDay = _view.WorkHourPerDay
            _current.AmountPrecision = _view.AmountPrecision
            _current.PercentPrecision = _view.PercentPrecision

            _current.BasicSalaryCodePlusId = _view.BasicSalaryCodePlusId
            _current.BasicSalaryCodeMinusId = _view.BasicSalaryCodeMinusId
            _current.AbsentCodeId = _view.AbsentCodeId
            _current.LateInCodeId = _view.LateInCodeId
            _current.EarlyOutCodeId = _view.EarlyOutCodeId

            _current.SSSBasedOn = _view.SSSBasedOn
            _current.PhilHealthBasedOn = _view.PhilHealthBasedOn

            Dim result = Await _service.SaveAsync(_current, _userName)

            If Not result.Success Then
                _view.ShowError(result.ErrorMessage)
                Return
            End If

            ' I-reload para makuha ang bagong GeneralSettingsId pagkatapos
            ' ng unang Insert - kung hindi, mag-i-Insert ulit ang susunod
            ' na Save at magkakaroon ng pangalawang row.
            _current = Await _service.GetAsync()
            _view.ShowMessage("General Settings saved.")
        End Function

        ' Active codes lang ang ipinapakita sa dropdown - kung na-deactivate
        ' ang isang code, hindi na siya dapat mapili bilang bagong mapping.
        ' (Kung naka-save na siya dati, mananatili pa rin ang Id sa database;
        ' blangko lang ang ipapakita ng LookUpEdit.)
        Private Async Function LoadCodeListsAsync() As Task
            Dim compensations = Await _compensationRepository.GetAllAsync()
            _view.SetCompensationCodes(
                compensations _
                    .Where(Function(c) c.IsActive) _
                    .Select(Function(c) New LookupModel With {
                        .Id = c.Id, .Code = c.Code, .Name = c.Description}) _
                    .ToList())

            Dim deductions = Await _flaggedEntryRepository.GetAllAsync("tblDeduction")
            _view.SetDeductionCodes(
                deductions _
                    .Where(Function(d) d.IsActive) _
                    .Select(Function(d) New LookupModel With {
                        .Id = d.Id, .Code = d.Code, .Name = d.Description}) _
                    .ToList())
        End Function

    End Class
End Namespace