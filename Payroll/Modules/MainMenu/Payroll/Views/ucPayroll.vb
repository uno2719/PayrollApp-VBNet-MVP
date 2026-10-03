Imports Payroll.GlobalShared.Base

Public Class ucPayroll
    Inherits GlobalShared.Base.ucBase
    Implements IAsyncLoadable

    Private ReadOnly _inputEntryView As ucPayrollInputEntry
    Private ReadOnly _outputView As ucPayrollOutput

    Public Sub New(inputEntryView As ucPayrollInputEntry, outputView As ucPayrollOutput)
        InitializeComponent()

        _inputEntryView = inputEntryView
        _outputView = outputView

        _inputEntryView.Dock = DockStyle.Fill
        TabNavigationPage1.Controls.Add(_inputEntryView)

        _outputView.Dock = DockStyle.Fill
        TabNavigationPage2.Controls.Add(_outputView)
    End Sub

    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Main > Payroll > {TabPane1.SelectedPage.Caption}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Payroll"
        End Get
    End Property

    ' Input tab loads up front (it's the default-selected page); Output is lazy —
    ' same EnsureLoadedAsync pattern as ucPayrollSettings' 6 Settings tabs.
    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _inputEntryView.EnsureLoadedAsync()
    End Function

    Private Async Sub TabPane1_SelectedPageChanged(sender As Object, e As EventArgs) Handles TabPane1.SelectedPageChanged
        RaiseBreadcrumbChanged()

        If TabPane1.SelectedPage Is TabNavigationPage2 Then
            Await _outputView.EnsureLoadedAsync()
        End If
    End Sub

End Class