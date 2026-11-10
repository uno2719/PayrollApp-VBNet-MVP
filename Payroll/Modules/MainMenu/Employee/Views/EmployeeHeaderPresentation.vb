' File: Modules/MainMenu/Employee/Views/EmployeeHeaderPresentation.vb
Imports System.Globalization

Namespace Employee.Views

    ''' <summary>Kulay ng isang chip: Back (background), Fore (text), Dot (tuldok; Color.Empty = wala).</summary>
    Public Structure ChipPalette
        Public ReadOnly Back As Color
        Public ReadOnly Fore As Color
        Public ReadOnly Dot As Color

        Public Sub New(back As Color, fore As Color, dot As Color)
            Me.Back = back
            Me.Fore = fore
            Me.Dot = dot
        End Sub
    End Structure

    Public Class HeaderChipState
        Public Property Text As String = String.Empty
        Public Property Visible As Boolean = True
        Public Property Back As Color
        Public Property Fore As Color
        Public Property Dot As Color = Color.Empty
    End Class

    ''' <summary>Lahat ng ipapakita ng header - handa nang i-apply ng view (walang logic sa view).</summary>
    Public Class EmployeeHeaderState
        Public Property FullName As String = String.Empty
        Public Property SubLine As String = String.Empty

        Public Property Status As New HeaderChipState()
        Public Property Kind As New HeaderChipState()
        Public Property Joined As New HeaderChipState()
        Public Property PayCycle As New HeaderChipState()
        Public Property PayrollChip As New HeaderChipState()

        ''' <summary>Walang laman = itago ang alert strip.</summary>
        Public Property AlertText As String = String.Empty
        Public Property AlertBack As Color
        Public Property AlertFore As Color

        Public Property LoansCaption As String = "Loans"
        Public Property ToggleActiveCaption As String = "Set inactive"
    End Class

    ''' <summary>
    ''' PURONG LOGIC (walang database, walang control): mula sa header model + active loans,
    ''' kinukuwenta kung anong chips, kulay, at alert ang lalabas. Kaya madaling i-test
    ''' at madaling baguhin ang mga patakaran nang hindi ginagalaw ang form.
    ''' </summary>
    Public Module EmployeeHeaderPresentation

        ''' <summary>
        ''' Fallback lang kapag hindi mabasa ang General Settings (hal. hindi pa na-run ang SQL).
        ''' Ang tunay na halaga ay galing sa Settings > General Settings > Probation Period.
        ''' </summary>
        Public Const DefaultProbationMonths As Integer = 5

        ' --- Palette (pareho ng mockup) ---
        Public ReadOnly PalGreen As New ChipPalette(Color.FromArgb(234, 243, 222), Color.FromArgb(39, 80, 10), Color.FromArgb(99, 153, 34))
        Public ReadOnly PalAmber As New ChipPalette(Color.FromArgb(250, 238, 218), Color.FromArgb(99, 56, 6), Color.FromArgb(186, 117, 23))
        Public ReadOnly PalBlue As New ChipPalette(Color.FromArgb(230, 241, 251), Color.FromArgb(12, 68, 124), Color.FromArgb(55, 138, 221))
        Public ReadOnly PalGray As New ChipPalette(Color.FromArgb(241, 239, 232), Color.FromArgb(68, 68, 65), Color.FromArgb(136, 135, 128))
        Public ReadOnly PalRed As New ChipPalette(Color.FromArgb(252, 235, 235), Color.FromArgb(121, 31, 31), Color.FromArgb(226, 75, 74))
        Public ReadOnly PalTeal As New ChipPalette(Color.FromArgb(225, 245, 238), Color.FromArgb(8, 80, 65), Color.FromArgb(29, 158, 117))

        ''' <summary>
        ''' Mga "uri ng employment" (kind) na kinikilala. Nakalagay pa rin ang mga ito sa EmployeeStatus column.
        ''' Ang "Active" at ang mga separation na halaga (Resigned/Retired/Terminated) ay HINDI kind.
        ''' </summary>
        Private ReadOnly KnownKinds As String() =
            {"Probationary", "Regular", "Permanent", "Contractual", "Project-based", "Casual", "Seasonal"}

        ''' <summary>Regular=berde, Probationary=amber, iba pang kind (Contractual, Project-based, Casual, Seasonal)=asul.</summary>
        Public Function KindPalette(kind As String) As ChipPalette
            Select Case If(kind, String.Empty).Trim().ToLowerInvariant()
                Case "regular", "permanent" : Return PalGreen
                Case "probationary" : Return PalAmber
                Case Else : Return PalBlue
            End Select
        End Function

        Private Class SeparationInfo
            Public Label As String
            Public OnDate As Date?
        End Class

        Private Function GetSeparation(h As Models.EmployeeHeaderModel) As SeparationInfo
            Dim st = If(h.EmployeeStatus, String.Empty).Trim()

            If st.Equals("Resigned", StringComparison.OrdinalIgnoreCase) Then
                Return New SeparationInfo With {.Label = "Resigned", .OnDate = h.DateResigned}
            End If
            If st.Equals("Retired", StringComparison.OrdinalIgnoreCase) Then
                Return New SeparationInfo With {.Label = "Retired", .OnDate = h.DateRetired}
            End If
            If st.Equals("Terminated", StringComparison.OrdinalIgnoreCase) Then
                Return New SeparationInfo With {.Label = "Terminated", .OnDate = h.DateTerminated}
            End If

            ' Walang separation na status, pero baka may petsa na naka-set.
            If h.DateTerminated.HasValue Then Return New SeparationInfo With {.Label = "Terminated", .OnDate = h.DateTerminated}
            If h.DateResigned.HasValue Then Return New SeparationInfo With {.Label = "Resigned", .OnDate = h.DateResigned}
            If h.DateRetired.HasValue Then Return New SeparationInfo With {.Label = "Retired", .OnDate = h.DateRetired}

            Return Nothing
        End Function

        Public Function FormatDate(d As Date) As String
            Return d.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
        End Function

        ''' <summary>Hal. "3 yrs 5 mos", "1 yr", "5 mos". Walang laman kung hindi pa nagsisimula.</summary>
        Public Function FormatTenure(joined As Date, untilDate As Date) As String
            Dim s = joined.Date
            Dim e = untilDate.Date
            If e < s Then Return String.Empty

            Dim months = (e.Year - s.Year) * 12 + (e.Month - s.Month)
            If e.Day < s.Day Then months -= 1
            If months < 0 Then months = 0

            Dim y = months \ 12
            Dim m = months Mod 12
            If y = 0 AndAlso m = 0 Then Return "less than 1 mo"

            Dim parts As New List(Of String)
            If y > 0 Then parts.Add($"{y} {If(y = 1, "yr", "yrs")}")
            If m > 0 Then parts.Add($"{m} {If(m = 1, "mo", "mos")}")
            Return String.Join(" ", parts)
        End Function

        Private Function MakeChip(text As String, pal As ChipPalette) As HeaderChipState
            Return New HeaderChipState With {.Text = text, .Visible = True, .Back = pal.Back, .Fore = pal.Fore, .Dot = pal.Dot}
        End Function

        Private Function HiddenChip() As HeaderChipState
            Return New HeaderChipState With {.Visible = False}
        End Function

        Private Function Money(value As Decimal) As String
            Return "₱" & value.ToString("N2", CultureInfo.InvariantCulture)
        End Function

        Public Function Build(h As Models.EmployeeHeaderModel,
                              activeLoans As IEnumerable(Of GlobalShared.Models.EmployeeLoanModel),
                              today As Date) As EmployeeHeaderState

            Dim loans = If(activeLoans, Enumerable.Empty(Of GlobalShared.Models.EmployeeLoanModel)()).ToList()
            Dim sep = GetSeparation(h)
            Dim statusText = If(h.EmployeeStatus, String.Empty).Trim()

            Dim state As New EmployeeHeaderState With {.FullName = h.FullName}

            ' ---------- Sub-line: EmployeeNo | Position | Department | Branch ----------
            Dim sub1 As New List(Of String)
            If Not String.IsNullOrWhiteSpace(h.EmployeeNo) Then sub1.Add(h.EmployeeNo.Trim())
            If Not String.IsNullOrWhiteSpace(h.PositionName) Then sub1.Add(StrConv(h.PositionName.Trim(), VbStrConv.ProperCase))
            If Not String.IsNullOrWhiteSpace(h.DepartmentName) Then sub1.Add(h.DepartmentName.Trim())
            If Not String.IsNullOrWhiteSpace(h.BranchName) Then sub1.Add(h.BranchName.Trim())
            state.SubLine = String.Join(" | ", sub1)

            ' ---------- Chips ----------
            ' 1) LIFECYCLE chip - DERIVED, hindi naka-store, kaya hindi sila puwedeng magkasalungat
            '    (dati: "Active" + "Inactive" sabay). Separated > Inactive > Active.
            If sep IsNot Nothing Then
                state.Status = MakeChip(sep.Label, If(sep.Label = "Terminated", PalRed, PalGray))
            ElseIf Not h.IsActive Then
                state.Status = MakeChip("Inactive", New ChipPalette(PalGray.Back, PalGray.Fore, PalRed.Dot))
            Else
                state.Status = MakeChip("Active", PalTeal)
            End If

            ' 2) EMPLOYMENT TYPE chip (kind: Probationary, Regular, Contractual...). Ang lumang "Active" o
            '    blangko ay hindi kind; kung separated na, hindi na mahalaga ang kind kaya itinatago.
            Dim isKnownKind = KnownKinds.Any(Function(k) k.Equals(statusText, StringComparison.OrdinalIgnoreCase))
            If isKnownKind Then
                state.Kind = MakeChip(statusText, KindPalette(statusText))
            ElseIf sep IsNot Nothing Then
                state.Kind = HiddenChip()
            Else
                state.Kind = MakeChip("Type not set", PalGray)
            End If

            If h.DateJoined.HasValue Then
                Dim endDate = If(sep IsNot Nothing AndAlso sep.OnDate.HasValue, sep.OnDate.Value, today)
                Dim tenure = FormatTenure(h.DateJoined.Value, endDate)
                Dim joinedText = $"Joined {FormatDate(h.DateJoined.Value)}" & If(tenure.Length > 0, $"  ·  {tenure}", String.Empty)
                state.Joined = MakeChip(joinedText, PalGray)
            Else
                state.Joined = MakeChip("No date joined", PalAmber)
            End If

            Dim hasPayCycle = h.HasEarnings AndAlso Not String.IsNullOrWhiteSpace(h.PayCycle)
            If hasPayCycle Then
                state.PayCycle = MakeChip(h.PayCycle.Trim(), PalGray)
            Else
                state.PayCycle = MakeChip("No pay cycle", PalAmber)
            End If

            If h.HasEarnings Then
                Dim dot = If(h.PayrollFlag, PalGreen.Dot, PalGray.Dot)
                state.PayrollChip = MakeChip(If(h.PayrollFlag, "In payroll", "Not in payroll"),
                                         New ChipPalette(PalGray.Back, PalGray.Fore, dot))
            Else
                state.PayrollChip = HiddenChip()
            End If

            ' ---------- Buttons ----------
            state.LoansCaption = If(loans.Count > 0, $"Loans ({loans.Count})", "Loans")
            state.ToggleActiveCaption = If(h.IsActive, "Set inactive", "Reactivate")

            ' ---------- Alert strip (isang linya; pinakamahalaga muna) ----------
            Dim msgs As New List(Of String)
            Dim anyWarning As Boolean = False
            Dim probationOnly As Boolean = False

            If sep IsNot Nothing Then
                msgs.Add(If(sep.OnDate.HasValue,
                            $"{sep.Label} on {FormatDate(sep.OnDate.Value)}.",
                            $"{sep.Label} (no date set)."))
            ElseIf Not h.IsActive Then
                ' Kumpirmado sa PayrollEligibilityRules: Inactive + walang petsa ng pag-alis = hindi kasama sa payroll.
                msgs.Add("Inactive: hidden from the Active list and not included in payroll.")
            End If

            If loans.Count > 0 AndAlso (sep IsNot Nothing OrElse Not h.IsActive) Then
                Dim bal = loans.Sum(Function(l) l.Balance)
                msgs.Add($"{loans.Count} active {If(loans.Count = 1, "loan", "loans")} with a balance of {Money(bal)}.")
                anyWarning = True
            End If

            If Not h.DateJoined.HasValue Then
                msgs.Add("Date joined is missing, so payroll can't include this employee (Employment tab).")
                anyWarning = True
            End If

            If Not hasPayCycle Then
                msgs.Add("No pay cycle set (Earnings tab), so the employee won't appear in Payroll Input.")
                anyWarning = True
            End If

            If sep Is Nothing AndAlso Not isKnownKind Then
                msgs.Add("Employment Type is not set (Employment tab).")
                anyWarning = True
            End If

            If sep Is Nothing AndAlso h.DateJoined.HasValue AndAlso
               statusText.Equals("Probationary", StringComparison.OrdinalIgnoreCase) Then

                Dim probationMonths = If(h.ProbationMonths.HasValue AndAlso h.ProbationMonths.Value > 0,
                                         h.ProbationMonths.Value, DefaultProbationMonths)
                Dim due = h.DateJoined.Value.Date.AddMonths(probationMonths)
                Dim days = CInt((due - today.Date).TotalDays)

                If days > 30 Then
                    msgs.Add($"{probationMonths}-month probation ends {FormatDate(due)} ({days} days left).")
                    probationOnly = True
                ElseIf days >= 0 Then
                    msgs.Add($"Probation ends {FormatDate(due)} ({days} {If(days = 1, "day", "days")} left). Regularization is due soon (Actions > Regularize).")
                    anyWarning = True
                Else
                    msgs.Add($"Probation ended {FormatDate(due)} ({-days} {If(-days = 1, "day", "days")} ago). Regularization is overdue (Actions > Regularize).")
                    anyWarning = True
                End If
            End If

            If msgs.Count > 0 Then
                Dim pal = If(anyWarning, PalAmber, If(probationOnly AndAlso msgs.Count = 1, PalBlue, PalGray))
                state.AlertText = String.Join("  ", msgs)
                state.AlertBack = pal.Back
                state.AlertFore = pal.Fore
            End If

            Return state
        End Function

    End Module

End Namespace
