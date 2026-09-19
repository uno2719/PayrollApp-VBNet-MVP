Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraSplashScreen
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Security
Imports Payroll.GlobalShared.Constants
Imports System.Linq


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

        ' ========================================================
        ' MODULE ACCESS
        ' ========================================================
        ' I-override ito sa BAWAT view na gusto mong ma-secure.
        ' Ang halaga ay dapat KAPAREHO ng Tag sa frmMain.Designer.vb
        ' at ng ModuleCode sa tblModules.
        '
        ' Halimbawa sa ucEmployees.vb:
        '
        '     Public Overrides ReadOnly Property ModuleCode As String
        '         Get
        '             Return ModuleCodes.Main_Employees
        '         End Get
        '     End Property
        '
        ' Kapag hindi mo ino-override (default = ""), ang view ay
        ' ituturing na hindi secured - mananatili siyang editable.
        ' Sinasadya ito para hindi masira ang mga lumang view mo
        ' habang unti-unti mo pang dinadagdagan ng ModuleCode.
        ' ========================================================
        Public Overridable ReadOnly Property ModuleCode As String
            Get
                Return String.Empty
            End Get
        End Property

        ''' <summary>
        ''' True kung may Can Edit access si user sa module na ito.
        ''' Kung walang ModuleCode ang view (hindi naka-override),
        ''' True pa rin - hindi secured ang view na iyon.
        ''' </summary>
        Protected ReadOnly Property HasEditAccess As Boolean
            Get
                If String.IsNullOrWhiteSpace(ModuleCode) Then Return True
                Return PermissionService.CanEdit(ModuleCode)
            End Get
        End Property

        ''' <summary>
        ''' I-disable ang mga command button ng isang WindowsUIButtonPanel
        ''' kung View Only lang si user.
        '''
        ''' Tawagin ito sa DULO ng LoadFormAsync ng view - dulo talaga,
        ''' dahil marami sa mga view mo ang nag-e-enable ng buttons sa
        ''' SetFormMode(). Kung mauuna ito, mao-overwrite siya.
        '''
        ''' Ang tag na "Refresh" ay sinasadyang hindi dine-disable -
        ''' pagbabasa lang naman iyon, ligtas kahit View Only.
        ''' </summary>
        Protected Sub ApplyReadOnlyMode(
            panel As DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel,
            Optional allowedTags As String() = Nothing)

            If panel Is Nothing Then Return
            If HasEditAccess Then Return

            Dim safeTags = If(allowedTags, New String() {"Refresh", "Details", "Export"})

            ' PAALALA (bug fix): huwag i-TryCast si btn papuntang WindowsUIButton
            ' (ang concrete class). Ang .Properties ay member ng IBaseButton
            ' INTERFACE - kung ika-cast mo papunta sa concrete class, nawawala
            ' ito sa paningin ng VB.NET (explicit interface implementation).
            ' Kaya panatilihin nating naka-type sa IBaseButton si btn - dito
            ' pa rin makikita ang .Properties, gaya ng ginagamit mo na sa
            ' ibang parte ng code gaya ng wbpMainCommands.Buttons.Item(i).Properties.
            For Each btn As DevExpress.XtraEditors.ButtonPanel.IBaseButton In panel.Buttons

                ' Ang WindowsUISeparator ay wala namang Tag na sinasadya -
                ' TypeOf lang ang gamit dito bilang FILTER (hindi cast),
                ' kaya hindi nawawala ang access sa .Properties ni btn.
                If Not TypeOf btn Is DevExpress.XtraBars.Docking2010.WindowsUIButton Then
                    Continue For
                End If

                Dim tag = btn.Properties.Tag?.ToString().Trim()

                If safeTags.Any(Function(t) String.Equals(t, tag, StringComparison.OrdinalIgnoreCase)) Then
                    Continue For
                End If

                btn.Properties.Enabled = False
                btn.Properties.ToolTip = "You have View Only access to this module."

            Next

        End Sub



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
