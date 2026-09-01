Namespace Employee.Services
    Public Class EmployeeService
        Implements IEmployeeService

        Private ReadOnly _repo As Data.IEmployeeRepository

        Public Sub New()
            _repo = New Data.EmployeeRepository()
        End Sub

        ' Constructor overload — para sa testing (dependency injection)
        Public Sub New(repo As Data.IEmployeeRepository)
            _repo = repo
        End Sub

        ' =============================================
        ' PERSONAL INFO
        ' =============================================

        Public Async Function GetAllRecordsAsync(Optional filter As String = "Active") As Task(Of List(Of Models.EmployeeModel)) _
            Implements IEmployeeService.GetAllRecordsAsync
            Return Await _repo.GetAllRecordsAsync(filter)
        End Function

        Public Async Function GetByIdAsync(recordId As Integer, Optional filter As String = "Active") As Task(Of Models.EmployeeModel) _
            Implements IEmployeeService.GetByIdAsync
            Return Await _repo.GetByIdAsync(recordId)
        End Function

        Public Async Function InsertAsync(model As Models.EmployeeModel) As Task(Of Integer) _
            Implements IEmployeeService.InsertAsync

            ' Validation
            If String.IsNullOrWhiteSpace(model.EmployeeNo) Then
                Throw New Exception("Employee No. is required.")
            End If
            If String.IsNullOrWhiteSpace(model.FirstName) Then
                Throw New Exception("First Name is required.")
            End If
            If String.IsNullOrWhiteSpace(model.LastName) Then
                Throw New Exception("Last Name is required.")
            End If
            If String.IsNullOrWhiteSpace(model.EmailAddress) Then
                Throw New Exception("Email Address is required.")
            End If

            ' Set audit fields
            model.CreatedBy = AppSession.CurrentUser

            Return Await _repo.InsertAsync(model)

        End Function

        Public Async Function UpdateAsync(model As Models.EmployeeModel) As Task(Of Boolean) _
            Implements IEmployeeService.UpdateAsync

            ' Validation
            If String.IsNullOrWhiteSpace(model.EmployeeNo) Then
                Throw New Exception("Employee No. is required.")
            End If
            If String.IsNullOrWhiteSpace(model.FirstName) Then
                Throw New Exception("First Name is required.")
            End If
            If String.IsNullOrWhiteSpace(model.LastName) Then
                Throw New Exception("Last Name is required.")
            End If

            ' Set audit fields
            model.UpdatedBy = AppSession.CurrentUser

            Return Await _repo.UpdateAsync(model)

        End Function

        Public Async Function DeleteAsync(recordId As Integer) As Task(Of Boolean) _
            Implements IEmployeeService.DeleteAsync
            Return Await _repo.DeleteAsync(recordId)
        End Function

        ' =============================================
        ' EMPLOYMENT
        ' =============================================

        Public Async Function GetEmploymentAsync(recordId As Integer) As Task(Of Models.EmployeeEmploymentModel) _
            Implements IEmployeeService.GetEmploymentAsync
            Return Await _repo.GetEmploymentAsync(recordId)
        End Function

        Public Async Function SaveEmploymentAsync(model As Models.EmployeeEmploymentModel) As Task(Of Boolean) _
            Implements IEmployeeService.SaveEmploymentAsync

            ' Set audit fields
            If model.EmploymentId = 0 Then
                model.CreatedBy = AppSession.CurrentUser
            Else
                model.UpdatedBy = AppSession.CurrentUser
            End If

            Return Await _repo.SaveEmploymentAsync(model)

        End Function

        ' =============================================
        ' EARNINGS
        ' =============================================

        Public Async Function GetEarningsAsync(recordId As Integer) As Task(Of Models.EmployeeEarningsModel) _
            Implements IEmployeeService.GetEarningsAsync
            Return Await _repo.GetEarningsAsync(recordId)
        End Function

        Public Async Function SaveEarningsAsync(model As Models.EmployeeEarningsModel) As Task(Of Boolean) _
            Implements IEmployeeService.SaveEarningsAsync

            ' Validation
            If model.BasicSalary <= 0 Then
                Throw New Exception("Basic Salary must be greater than zero.")
            End If

            ' Set audit fields
            If model.EarningsId = 0 Then
                model.CreatedBy = AppSession.CurrentUser
            Else
                model.UpdatedBy = AppSession.CurrentUser
            End If

            Return Await _repo.SaveEarningsAsync(model)

        End Function

        ' =============================================
        ' STATUTORY
        ' =============================================

        Public Async Function GetStatutoryAsync(recordId As Integer) As Task(Of Models.EmployeeStatutoryModel) _
            Implements IEmployeeService.GetStatutoryAsync
            Return Await _repo.GetStatutoryAsync(recordId)
        End Function

        Public Async Function SaveStatutoryAsync(model As Models.EmployeeStatutoryModel) As Task(Of Boolean) _
            Implements IEmployeeService.SaveStatutoryAsync

            ' Set audit fields
            If model.StatutoryId = 0 Then
                model.CreatedBy = AppSession.CurrentUser
            Else
                model.UpdatedBy = AppSession.CurrentUser
            End If

            Return Await _repo.SaveStatutoryAsync(model)

        End Function


        ' =============================================
        ' LOOKUPS
        ' =============================================

        Public Async Function GetLookupsAsync(tableName As String) As Task(Of List(Of GlobalShared.Models.LookupModel)) _
            Implements IEmployeeService.GetLookupsAsync
            Return Await _repo.GetLookupsAsync(tableName)
        End Function

        Public Async Function GetEmployeeLookupAsync() As Task(Of List(Of GlobalShared.Models.EmployeeLookupModel)) _
    Implements IEmployeeService.GetEmployeeLookupAsync
            Return Await _repo.GetEmployeeLookupAsync()
        End Function

    End Class
End Namespace