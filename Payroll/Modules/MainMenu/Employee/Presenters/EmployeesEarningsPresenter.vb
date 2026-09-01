Imports Payroll.Employee.Services

Namespace Employee.Presenters
    Public Class EmployeesEarningsPresenter
        Inherits BasePresenter(Of Employee.Views.IEmployeesEarningsView)

        Private ReadOnly _service As Employee.Services.IEmployeeService
        Private _recordId As Integer = 0

        Public Sub New(view As Employee.Views.IEmployeesEarningsView,
                       service As Employee.Services.IEmployeeService)
            MyBase.New(view)
            _service = service
            SubscribeEvents()
        End Sub

        Private Sub SubscribeEvents()
            AddHandler _view.OnSave, AddressOf HandleSave
            AddHandler _view.OnNew, AddressOf HandleNew
        End Sub

        ' =============================================
        ' LOAD LOOKUPS
        ' =============================================
        Public Async Sub LoadLookups()
            Try
                _view.LoadBanks(Await _service.GetLookupsAsync("tblBank"))
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Sub
        Public Async Function LoadLookupsAsync() As Task ' ✅ Gawing Async Function
            Try
                _view.LoadBanks(Await _service.GetLookupsAsync("tblBank"))
            Catch ex As Exception
                _view.ShowError(ex.Message)
            End Try
        End Function

        ' =============================================
        ' LOAD
        ' =============================================
        Public Async Sub LoadEmployee(recordId As Integer)
            Try
                _view.ShowLoading()
                _recordId = recordId

                _view.ClearFields()

                LoadLookups()
                ' Await LoadLookupsAsync()

                Dim earnings = Await _service.GetEarningsAsync(recordId)
                If earnings Is Nothing Then
                    _view.RecordId = 0
                    _view.BasicSalary = Nothing
                    _view.DailyRate = Nothing
                    _view.HourlyRate = Nothing
                    _view.DaysInYear = 365
                    _view.WorkHourPer = 8
                    _view.PayrollFlag = True
                    _view.MinimumWage = False
                    Return
                End If

                '_view.RecordId = earnings.EarningsId
                _view.BasicSalary = earnings.BasicSalary
                _view.DailyRate = earnings.DailyRate
                _view.HourlyRate = earnings.HourlyRate
                _view.DaysInYear = earnings.DaysInYear
                _view.WorkHourPer = earnings.WorkHourPer
                _view.PayrollFlag = earnings.PayrollFlag
                _view.MinimumWage = earnings.MinimumWage
                _view.PayCycle = If(earnings.PayCycle, String.Empty)
                _view.TaxFlag = If(earnings.TaxFlag, String.Empty)
                _view.PayBy = If(earnings.PayBy, String.Empty)
                _view.BankId = earnings.BankId
                _view.BankAccount = If(earnings.BankAccount, String.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub

        ' =============================================
        ' NEW
        ' =============================================
        Private Sub HandleNew(sender As Object, e As EventArgs)
            _recordId = _view.RecordId
            LoadLookups()
            _view.BasicSalary = Nothing
            _view.DailyRate = Nothing
            _view.HourlyRate = Nothing
            _view.DaysInYear = 365
            _view.WorkHourPer = 8
            _view.PayrollFlag = True
            _view.MinimumWage = False
            _view.PayCycle = String.Empty
            _view.TaxFlag = String.Empty
            _view.PayBy = String.Empty
            _view.BankId = Nothing
            _view.BankAccount = String.Empty
        End Sub

        ' =============================================
        ' SAVE
        ' =============================================
        Private Async Sub HandleSave(sender As Object, e As EventArgs)
            Try
                _view.ShowLoading()

                Dim earnings As New Employee.Models.EmployeeEarningsModel With {
                    .RecordId = _recordId,
                    .BasicSalary = _view.BasicSalary,
                    .DailyRate = _view.DailyRate,
                    .HourlyRate = _view.HourlyRate,
                    .DaysInYear = _view.DaysInYear,
                    .WorkHourPer = _view.WorkHourPer,
                    .PayrollFlag = _view.PayrollFlag,
                    .MinimumWage = _view.MinimumWage,
                    .PayCycle = _view.PayCycle,
                    .TaxFlag = _view.TaxFlag,
                    .PayBy = _view.PayBy,
                    .BankId = _view.BankId,
                    .BankAccount = _view.BankAccount
                }

                Await _service.SaveEarningsAsync(earnings)
                RaiseEvent OnSaveCompleted(_view, EventArgs.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub

        Public Event OnSaveCompleted As EventHandler

    End Class
End Namespace