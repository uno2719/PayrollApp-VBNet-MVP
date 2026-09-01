Imports Payroll.Employee.Services

Namespace Employee.Presenters
    Public Class EmployeesPersonalInfoPresenter
        Inherits BasePresenter(Of Employee.Views.IEmployeesPersonalInfoView)

        Private ReadOnly _service As Services.IEmployeeService
        Private _isNew As Boolean = False

        Public Sub New(view As Employee.Views.IEmployeesPersonalInfoView,
                       service As Services.IEmployeeService)
            MyBase.New(view)
            _service = service
            SubscribeEvents()
        End Sub

        Private Sub SubscribeEvents()
            AddHandler _view.OnSave, AddressOf HandleSave
            AddHandler _view.OnDelete, AddressOf HandleDelete
            AddHandler _view.OnNew, AddressOf HandleNew
        End Sub

        ' =============================================
        ' LOAD
        ' =============================================
        Public Async Sub LoadEmployee(recordId As Integer)
            Try
                _view.ShowLoading()
                _isNew = False

                Dim emp = Await _service.GetByIdAsync(recordId)
                If emp Is Nothing Then Return

                ' Map model → view
                _view.RecordId = emp.RecordId
                _view.EmployeeNo = emp.EmployeeNo
                _view.FirstName = emp.FirstName
                _view.MiddleName = If(emp.MiddleName, String.Empty)
                _view.LastName = emp.LastName
                _view.Suffix = If(emp.Suffix, String.Empty)
                _view.EmailAddress = If(emp.EmailAddress, String.Empty)
                _view.ContactNo = If(emp.ContactNo, String.Empty)
                _view.BirthDate = If(emp.BirthDate.HasValue,
                                     New DateTimeOffset?(New DateTimeOffset(emp.BirthDate.Value)),
                                     Nothing)
                _view.BirthPlace = If(emp.BirthPlace, String.Empty)
                _view.Gender = If(emp.Gender, String.Empty)
                _view.CivilStatus = If(emp.CivilStatus, String.Empty)
                _view.Religion = If(emp.Religion, String.Empty)
                _view.Citizenship = If(emp.Citizenship, String.Empty)
                _view.MailAddress1 = If(emp.MailAddress1, String.Empty)
                _view.MailAddress2 = If(emp.MailAddress2, String.Empty)
                _view.MailZipCode = If(emp.MailZipCode, String.Empty)
                _view.PermanentAddress1 = If(emp.PermanentAddress1, String.Empty)
                _view.PermanentAddress2 = If(emp.PermanentAddress2, String.Empty)
                _view.PermanentZipCode = If(emp.PermanentZipCode, String.Empty)

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
            _isNew = True
            _view.RecordId = 0
            _view.EmployeeNo = String.Empty
            _view.FirstName = String.Empty
            _view.MiddleName = String.Empty
            _view.LastName = String.Empty
            _view.Suffix = String.Empty
            _view.EmailAddress = String.Empty
            _view.ContactNo = String.Empty
            _view.BirthDate = Nothing
            _view.BirthPlace = String.Empty
            _view.Gender = String.Empty
            _view.CivilStatus = String.Empty
            _view.Religion = String.Empty
            _view.Citizenship = String.Empty
            _view.MailAddress1 = String.Empty
            _view.MailAddress2 = String.Empty
            _view.MailZipCode = String.Empty
            _view.PermanentAddress1 = String.Empty
            _view.PermanentAddress2 = String.Empty
            _view.PermanentZipCode = String.Empty
        End Sub

        ' =============================================
        ' SAVE (Insert or Update)
        ' =============================================
        Private Async Sub HandleSave(sender As Object, e As EventArgs)
            Try
                _view.ShowLoading()

                ' Map view → model
                Dim emp As New Models.EmployeeModel With {
                    .RecordId = _view.RecordId,
                    .EmployeeNo = _view.EmployeeNo,
                    .FirstName = _view.FirstName,
                    .MiddleName = _view.MiddleName,
                    .LastName = _view.LastName,
                    .Suffix = _view.Suffix,
                    .EmailAddress = _view.EmailAddress,
                    .ContactNo = _view.ContactNo,
                    .BirthDate = If(_view.BirthDate.HasValue,
                                    New Date?(_view.BirthDate.Value.Date),
                                    Nothing),
                    .BirthPlace = _view.BirthPlace,
                    .Gender = _view.Gender,
                    .CivilStatus = _view.CivilStatus,
                    .Religion = _view.Religion,
                    .Citizenship = _view.Citizenship,
                    .MailAddress1 = _view.MailAddress1,
                    .MailAddress2 = _view.MailAddress2,
                    .MailZipCode = _view.MailZipCode,
                    .PermanentAddress1 = _view.PermanentAddress1,
                    .PermanentAddress2 = _view.PermanentAddress2,
                    .PermanentZipCode = _view.PermanentZipCode,
                    .IsActive = True
                }

                If _isNew Then
                    Dim newId = Await _service.InsertAsync(emp)
                    _view.RecordId = newId
                    _isNew = False
                    '  _view.ShowMessage("Employee saved successfully!")
                Else
                    Await _service.UpdateAsync(emp)
                    ' _view.ShowMessage("Employee updated successfully!")
                End If

                RaiseEvent OnSaveCompleted(Me, EventArgs.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub
        Public Event OnSaveCompleted As EventHandler

        ' =============================================
        ' DELETE
        ' =============================================
        Private Async Sub HandleDelete(sender As Object, e As EventArgs)
            Try
                If _view.RecordId = 0 Then Return

                _view.ShowLoading()
                Await _service.DeleteAsync(_view.RecordId)
                _view.ShowMessage("Employee deleted successfully!")
                HandleNew(Nothing, EventArgs.Empty)

            Catch ex As Exception
                _view.ShowError(ex.Message)
            Finally
                _view.HideLoading()
            End Try
        End Sub

    End Class
End Namespace