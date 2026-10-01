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

    ' Both child tabs load up front (Input's Cutoff list + Output's Cutoff filter
    ' are both cheap); only the grid DATA for Input loads lazily, on Cutoff selection.
    Public Overrides Async Function LoadFormAsync() As Task Implements IAsyncLoadable.LoadFormAsync
        Await _inputEntryView.LoadFormAsync()
        Await _outputView.LoadFormAsync()
    End Function

End Class