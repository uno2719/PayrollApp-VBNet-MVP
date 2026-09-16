' File: Modules/Settings/SysConfig/GeneralSettings/Views/IGeneralSettingsView.vb
Imports Payroll.GlobalShared.Models

Namespace GeneralSettings.Views
    Public Interface IGeneralSettingsView

        ' --- Payroll Parameters ---
        Property BonusCeiling As Decimal
        Property TotalDaysPerYear As Integer
        Property WorkHourPerDay As Decimal
        Property AmountPrecision As Integer
        Property PercentPrecision As Integer

        ' --- Code Mapping ---
        Property BasicSalaryCodePlusId As Integer?
        Property BasicSalaryCodeMinusId As Integer?
        Property AbsentCodeId As Integer?
        Property LateInCodeId As Integer?
        Property EarlyOutCodeId As Integer?

        ' --- Statutory Basis ---
        Property SSSBasedOn As String
        Property PhilHealthBasedOn As String

        ' Binibind ang dalawang listahan sa lahat ng code dropdowns.
        ' Ginagamit natin ang LookupModel (Id / Code / Name) kahit
        ' Compensation at Deduction ang pinanggalingan - ang Presenter
        ' ang nagma-map, para hindi na kailangang alamin ng View kung
        ' anong table ang pinagmulan.
        Sub SetCompensationCodes(codes As List(Of LookupModel))
        Sub SetDeductionCodes(codes As List(Of LookupModel))

        Sub ShowMessage(message As String)
        Sub ShowError(message As String)
    End Interface
End Namespace