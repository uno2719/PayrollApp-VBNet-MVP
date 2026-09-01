Namespace GlobalShared.Base
    ' Ang "TView" ay placeholder para sa interface (hal. IEmployeeView)
    Public MustInherit Class BasePresenter(Of TView)
        Protected ReadOnly _view As TView

        Protected Sub New(view As TView)
            _view = view
        End Sub

        ' Pwede mong lagyan ng common logic dito, halimbawa logging
        Protected Sub LogAction(actionName As String)
            Console.WriteLine($"Action: {actionName} executed at {DateTime.Now}")
        End Sub
    End Class
End Namespace
