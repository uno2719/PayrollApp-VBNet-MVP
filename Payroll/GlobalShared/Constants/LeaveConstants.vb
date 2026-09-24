' ============================================================
' GlobalShared/Constants/LeaveConstants.vb
' ============================================================
' Mga fixed dropdown value ng Leave Rule tab - parehong pattern
' gaya ng LoanConstants (Const + "All" array para sa combo
' binding). Ang laman dito ay base lang sa nakita sa C1Pay
' reference screenshot - dagdagan na lang kung kailangan mamaya.
' ============================================================
Namespace GlobalShared.Constants

    Public Module LeaveCategory
        Public Const Annual As String = "Annual"
        Public Const Maternity As String = "Maternity"
        Public Const Paternity As String = "Paternity"
        Public Const Unpaid As String = "Unpaid"

        Public ReadOnly All As String() = {Annual, Maternity, Paternity, Unpaid}
    End Module

    Public Module LeaveEntitlementMethod
        Public Const Lumpsum As String = "Lumpsum"

        ' Isa lang ang nakita sa reference - pero hiwalay pa rin
        ' natin sa sariling "All" array (hindi direktang ini-inline
        ' sa Designer) para hindi kailangang hanapin at palitan sa
        ' maraming lugar kapag nag-expand ito mamaya.
        Public ReadOnly All As String() = {Lumpsum}
    End Module

    ''' <summary>
    ''' Ginagamit ng "Compute Based On", "Plot Based On", at
    ''' "Anniversary Plot On" - parehong listahan ng values ang
    ''' tatlo, kaya iisang module lang.
    ''' </summary>
    Public Module LeaveDateBasis
        Public Const DateJoined As String = "Date Joined"
        Public Const DateRegularized As String = "Date Regularized"

        Public ReadOnly All As String() = {DateJoined, DateRegularized}
    End Module

    Public Module LeaveUnitOfMeasure
        Public Const Days As String = "Days"
        Public Const Hours As String = "Hours"

        Public ReadOnly All As String() = {Days, Hours}
    End Module

End Namespace
