Imports Payroll.GlobalShared.Base

Public Class ucPayroll
    Inherits GlobalShared.Base.ucBase
    'Implements IAsyncLoadable

    Private _currentTab As String = "Test"

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Main > Payroll > {_currentTab}"
        End Get
    End Property
    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payroll"
        End Get
    End Property
    Private Sub ucPayroll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub
End Class
