Imports Payroll.GlobalShared.Base

Public Class ucDashboard
    Inherits GlobalShared.Base.ucBase

    Public Sub New()
        InitializeComponent()

        Me.LookAndFeel.UseDefaultLookAndFeel = True
        Me.BackColor = Color.Empty
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Main"
        End Get
    End Property
    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Dashboard"
        End Get
    End Property
    Private Sub ucDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
