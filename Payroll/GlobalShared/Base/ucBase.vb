Imports DevExpress.XtraEditors
Imports DevExpress.XtraSplashScreen
Imports Payroll.GlobalShared.Base
Imports System.ComponentModel

Namespace GlobalShared.Base
    ' Ang DesignerGenerated attribute ay tumutulong para makita ang UI properties gaya ng Dock
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Public Class ucBase
        Inherits XtraUserControl ' <--- Siguraduhin na ito ang gamit
        Implements IBaseView

        Private _isLoaded As Boolean = False
        Private _isLoading As Boolean = False

        Public Sub New()
            InitializeComponent()
            DevExpress.UserSkins.BonusSkins.Register()
            DevExpress.Skins.SkinManager.EnableFormSkins()
            'Me.Appearance.BackColor = Color.Transparent
            'Me.Appearance.Options.UseBackColor = True

            Me.LookAndFeel.UseDefaultLookAndFeel = True
            Me.BackColor = Color.Empty
        End Sub

        Public Async Function EnsureLoadedAsync() As Task
            If _isLoaded OrElse _isLoading Then Return

            _isLoading = True

            Try
                Await LoadFormAsync()
                _isLoaded = True
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace)
            Finally
                _isLoading = False
            End Try
        End Function

        Public Sub ResetLoadState()
            _isLoaded = False
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            Me.LookAndFeel.UseDefaultLookAndFeel = True
            Me.BackColor = Color.Empty
            'Me.Appearance.BackColor = DevExpress.LookAndFeel.UserLookAndFeel.Default.Skin
        End Sub


        ' Para sa Overlay Loading Screen (v25.2 style)
        Private _handle As IOverlaySplashScreenHandle

        ' 🔥 Event para mag-notify ng changes sa breadcrumb
        Public Event BreadcrumbChanged As EventHandler
        Public Overridable ReadOnly Property Breadcrumb As String
            Get
                Return "Main"
            End Get
        End Property

        Public Overridable ReadOnly Property PageTitle As String
            Get
                Return "Dashboard"
            End Get
        End Property

        Protected Sub RaiseBreadcrumbChanged()
            RaiseEvent BreadcrumbChanged(Me, EventArgs.Empty)
        End Sub

        Public Sub ShowLoading() Implements IBaseView.ShowLoading
            _handle = SplashScreenManager.ShowOverlayForm(Me)
        End Sub

        Public Sub HideLoading() Implements IBaseView.HideLoading
            If _handle IsNot Nothing Then
                SplashScreenManager.CloseOverlayForm(_handle)
            End If
        End Sub

        Public Sub ShowError(msg As String) Implements IBaseView.ShowError
            XtraMessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Public Sub ShowMessage(msg As String) Implements IBaseView.ShowMessage
            XtraMessageBox.Show(msg, "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub


        Public Overridable Async Function LoadFormAsync() As Task
            Try
                Await Task.CompletedTask
            Catch ex As Exception
                MessageBox.Show(ex.Message & vbCrLf & ex.StackTrace)
            End Try
        End Function

        Private Sub InitializeComponent()
            SuspendLayout()
            ' 
            ' ucBase
            ' 
            DoubleBuffered = True
            Name = "ucBase"
            Size = New Size(641, 356)
            ResumeLayout(False)

        End Sub

    End Class
End Namespace
