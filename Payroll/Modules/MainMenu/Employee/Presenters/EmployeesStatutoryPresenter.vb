Namespace Employee.Presenters
    Public Class EmployeesStatutoryPresenter
        Inherits BasePresenter(Of Employee.Views.IEmployeesStatutoryView)

        Private ReadOnly _service As Employee.Services.IEmployeeService
        Private _recordId As Integer = 0

        Public Sub New(view As Employee.Views.IEmployeesStatutoryView,
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
        ' LOAD
        ' =============================================
        Public Async Sub LoadEmployee(recordId As Integer)
            Try
                _view.ShowLoading()
                _recordId = recordId

                ' ✅ I-clear muna bago mag-load
                _view.ClearFields()

                Dim statutory = Await _service.GetStatutoryAsync(recordId)
                If statutory Is Nothing Then Return

                ' _view.RecordId = statutory.StatutoryId
                _view.TIN = If(statutory.TIN, String.Empty)
                _view.SSSNo = If(statutory.SSSNo, String.Empty)
                _view.PagIBIGNo = If(statutory.PagIBIGNo, String.Empty)
                _view.PagIBIGVoluntary = statutory.PagIBIGVoluntary
                _view.PhilHealthNo = If(statutory.PhilHealthNo, String.Empty)
                _view.FixTax = statutory.FixTax

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
            _view.TIN = String.Empty
            _view.SSSNo = String.Empty
            _view.PagIBIGNo = String.Empty
            _view.PagIBIGVoluntary = Nothing
            _view.PhilHealthNo = String.Empty
            _view.FixTax = Nothing
        End Sub

        ' =============================================
        ' SAVE
        ' =============================================
        Private Async Sub HandleSave(sender As Object, e As EventArgs)
            Try
                _view.ShowLoading()

                Dim statutory As New Employee.Models.EmployeeStatutoryModel With {
                    .RecordId = _recordId,
                    .TIN = _view.TIN,
                    .SSSNo = _view.SSSNo,
                    .PagIBIGNo = _view.PagIBIGNo,
                    .PagIBIGVoluntary = _view.PagIBIGVoluntary,
                    .PhilHealthNo = _view.PhilHealthNo,
                    .FixTax = _view.FixTax
                }

                Await _service.SaveStatutoryAsync(statutory)
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