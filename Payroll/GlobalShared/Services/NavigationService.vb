Imports DevExpress.Utils.Animation
Imports System.Linq
Imports DevExpress.XtraSplashScreen
Imports DevExpress.Utils



Public Class NavigationService

    Private ReadOnly _container As Control
    Private ReadOnly _tm As TransitionManager

    Private ReadOnly _modules As New Dictionary(Of Type, GlobalShared.Base.ucBase)
    Private ReadOnly _history As New Stack(Of Type)
    Private ReadOnly _forward As New Stack(Of Type)

    Private _default As Type = Nothing
    Private _current As Type = Nothing

    Private _isNavigationChanged As Boolean
    Public ReadOnly Property IsNavigationChanged As Boolean
        Get
            Return _isNavigationChanged
        End Get
    End Property

    Private _breadcrumbCallback As Action(Of String)
    Private _titleCallback As Action(Of String)

    Private _transition As Transition
    Private _loadingHandle As IOverlaySplashScreenHandle
    Private isAnimating As Boolean = False

    Public Sub New(container As Control,
               tm As DevExpress.Utils.Animation.TransitionManager,
               breadcrumbCallback As Action(Of String),
               titleCallback As Action(Of String))

        _container = container
        _tm = tm
        _breadcrumbCallback = breadcrumbCallback
        _titleCallback = titleCallback

        InitializeTransition()
    End Sub

    Private Sub InitializeTransition()

        _transition = New Transition()
        _transition.Control = _container
        _transition.TransitionType = New SlideFadeTransition()

        _tm.Transitions.Clear()
        _tm.Transitions.Add(_transition)

    End Sub

    Private Sub ShowSkeleton_()
        If _loadingHandle Is Nothing Then
            _loadingHandle = SplashScreenManager.ShowOverlayForm(_container)
        End If
    End Sub
    Private Sub ShowSkeleton()

        If _loadingHandle IsNot Nothing Then Return

        Dim options As New OverlayWindowOptions()

        ' 🔥 Background (slightly transparent)
        options.BackColor = Color.FromArgb(240, 240, 240)
        options.Opacity = 0.7

        ' 🔥 Foreground animation color (shimmer feel)
        options.ForeColor = Color.Gray

        ' 🔥 Fade in/out (important for smoothness)
        options.AnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Default

        _loadingHandle = SplashScreenManager.ShowOverlayForm(_container, options)

    End Sub

    Private Sub HideSkeleton()
        If _loadingHandle IsNot Nothing Then
            SplashScreenManager.CloseOverlayForm(_loadingHandle)
            _loadingHandle = Nothing
        End If
    End Sub

    ' Sa NavigationService.vb — dagdag na overload lang
    ' NavigationService.vb

    ' ✅ ORIGINAL — para sa mga may parameterless constructor (ucDashboard, etc.)
    Public Sub NavigateTo(Of T As {GlobalShared.Base.ucBase, New})()

        Dim viewType = GetType(T)
        If _current Is viewType Then
            _isNavigationChanged = _history.Count > 0
            Return
        End If

        _isNavigationChanged = True

        If _current IsNot Nothing Then
            _history.Push(_current)
            _forward.Clear()
        End If

        ShowView(viewType, False)
        _current = viewType

    End Sub

    ' ✅ BAGONG OVERLOAD — para sa mga may factory (ucEmployees, ucPayroll, etc.)
    Public Sub NavigateTo(Of T As {GlobalShared.Base.ucBase})(factory As Func(Of T))

        Dim viewType = GetType(T)
        If _current Is viewType Then
            _isNavigationChanged = _history.Count > 0
            Return
        End If

        _isNavigationChanged = True

        If _current IsNot Nothing Then
            _history.Push(_current)
            _forward.Clear()
        End If

        ' I-create ang view gamit ang factory kung wala pa sa cache
        If Not _modules.ContainsKey(viewType) Then
            Dim view = factory()
            view.Dock = DockStyle.Fill
            _container.Controls.Add(view)
            _modules.Add(viewType, view)
        End If

        ShowView(viewType, False)
        _current = viewType

    End Sub

    Public Sub GoBack()

        If _history.Count = 0 Then

            If _default IsNot Nothing AndAlso _current IsNot _default Then
                ShowView(_default, True)
                _current = _default
            End If

            Return
        End If

        _forward.Push(_current)

        Dim prev = _history.Pop()

        ShowView(prev, True)

        _current = prev

    End Sub

    Public Sub GoForward()

        If _forward.Count = 0 Then Return

        _history.Push(_current)

        Dim nxt = _forward.Pop()

        ShowView(nxt, False)

        _current = nxt

    End Sub

    Public Sub SetDefault(Of T As {GlobalShared.Base.ucBase, New})()

        _default = GetType(T)

        ShowView(_default, False, False)

        _current = _default

    End Sub


    Private Async Sub ShowView(viewType As Type, isBack As Boolean, Optional withAnim As Boolean = True)

        Dim view As GlobalShared.Base.ucBase = Nothing

        ' =========================
        ' GET / CREATE VIEW
        ' =========================
        If Not _modules.TryGetValue(viewType, view) Then
            view = CType(Activator.CreateInstance(viewType), GlobalShared.Base.ucBase)

            If view Is Nothing Then
                MessageBox.Show("Failed to create: " & viewType.Name)
                Return
            End If


            view.Dock = DockStyle.Fill

            _container.Controls.Add(view)
            _modules.Add(viewType, view)
        End If


        ' =========================
        ' TRANSITION START
        ' =========================
        If withAnim Then

            ''Check if nasa animation pa sya
            If isAnimating Then Return
            isAnimating = True

            '' SKELETON START
            'ShowSkeleton()
            Dim anim As New SlideFadeTransition()

            anim.Parameters.FrameInterval = 1500
            anim.Parameters.FrameCount = 2400
            anim.Parameters.EffectOptions =
            If(isBack,
               PushEffectOptions.FromRight,
               PushEffectOptions.FromLeft)

            _transition.TransitionType = anim
            _tm.StartTransition(_container)

        End If

        ' =========================
        ' SHOW VIEW
        ' =========================
        view.BringToFront()

        ' =========================
        ' LOAD DATA (ASYNC SAFE)
        ' =========================
        Await view.EnsureLoadedAsync()

        ' =========================
        ' UI UPDATE
        ' =========================
        _breadcrumbCallback?.Invoke(view.Breadcrumb)
        _titleCallback?.Invoke(view.PageTitle)

        RemoveHandler view.BreadcrumbChanged, AddressOf OnBreadcrumbChanged
        AddHandler view.BreadcrumbChanged, AddressOf OnBreadcrumbChanged

        ' =========================
        ' END TRANSITION
        ' =========================
        If withAnim Then
            _tm.EndTransition()

            Await Task.Delay(150)
            isAnimating = False
            'HideSkeleton()
        End If

    End Sub

    Private Sub OnBreadcrumbChanged(sender As Object, e As EventArgs)

        Dim view = TryCast(sender, GlobalShared.Base.ucBase)

        If view IsNot Nothing Then
            _breadcrumbCallback?.Invoke(view.Breadcrumb)
        End If

    End Sub

    Public Function CanGoBack() As Boolean
        Return _history.Count > 0 OrElse (_current IsNot _default)
    End Function

    Public Function CanGoForward() As Boolean
        Return _forward.Count > 0 'OrElse (_current IsNot _default)
    End Function

End Class