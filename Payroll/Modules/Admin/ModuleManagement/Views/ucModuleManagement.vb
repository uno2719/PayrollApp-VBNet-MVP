' ============================================================
' Modules/Admin/ModuleManagement/Views/ucModuleManagement.vb
' ============================================================
' Shell lang ito - kapareho ng pattern ng ucStatutorySettingsShell.
' Dalawang INJECTED na child view (ucModuleCatalog, ucModuleAccessEditor),
' bawat isa may sariling Presenter na na-wire na sa AppComposition.
' Lazy-load ang pangalawang tab (Access Editor) - unang tab lang
' (Catalog) ang agad kumakarga.
' ============================================================
Imports System.Linq

Public Class ucModuleManagement
    Implements IAsyncLoadable

    Private _currentTab As String = "Module Catalog"

    Private ReadOnly _ucCatalog As ucModuleCatalog
    Private ReadOnly _ucAccessEditor As ucModuleAccessEditor

    Public Sub New(catalogView As ucModuleCatalog, accessEditorView As ucModuleAccessEditor)

        InitializeComponent()

        _ucCatalog = catalogView
        _ucAccessEditor = accessEditorView

        DockAllViews()

    End Sub

    Private Sub DockAllViews()

        _ucCatalog.Dock = DockStyle.Fill
        tabpageCatalog.Controls.Add(_ucCatalog)

        _ucAccessEditor.Dock = DockStyle.Fill
        tabpageAccessEditor.Controls.Add(_ucAccessEditor)

    End Sub

    ' ============================================================
    ' IDENTITY
    ' ============================================================
    Public Overrides ReadOnly Property Breadcrumb As String
        Get
            Return $"Administration > Module Management > {_currentTab}"
        End Get
    End Property

    Public Overrides ReadOnly Property PageTitle As String
        Get
            Return "Module Management"
        End Get
    End Property

    ' Ito ang tumutugma sa aceAdminModules.Tag na nakascaffold na
    ' sa frmMain.Designer.vb. Ang buong Administration section ay
    ' Admin-only na rin sa AuthorizationService/ElementClick guard,
    ' pero ito pa rin ang tamang ModuleCode kung sakaling gusto mo
    ' pang gawing per-user-assignable ito balang araw sa halip na
    ' Admin-only blanket rule.
    Public Overrides ReadOnly Property ModuleCode As String
        Get
            Return Payroll.GlobalShared.Constants.ModuleCodes.Admin_Modules
        End Get
    End Property

    ' ============================================================
    ' LOAD - unang tab lang agad, gaya ng ginawa sa
    ' ucStatutorySettingsShell.
    ' ============================================================
    Public Overrides Async Function LoadFormAsync() As Task _
        Implements IAsyncLoadable.LoadFormAsync

        Await _ucCatalog.EnsureLoadedAsync()

    End Function

    Private Async Sub tabconModuleManagement_SelectedPageChanged(
        sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) _
        Handles tabconModuleManagement.SelectedPageChanged

        _currentTab = tabconModuleManagement.SelectedTabPage.Text
        RaiseBreadcrumbChanged()

        Dim activeView = TryCast(
            tabconModuleManagement.SelectedTabPage.Controls.Cast(Of Control).FirstOrDefault(),
            GlobalShared.Base.ucBase)

        If activeView IsNot Nothing Then
            Await activeView.EnsureLoadedAsync()
        End If

    End Sub

End Class