Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' PURE LOGIC LANG (walang database, walang UI): sino ang kasama sa payroll ng isang cutoff.
    ''' Ginagamit ng Payroll Input Entry ngayon, at ng Process computation sa susunod, para iisa ang patakaran.
    '''
    ''' Mga tuntunin, sa pagkakasunod ng pagsusuri:
    '''   1. "Include in payroll" ay naka-off                          -> hindi kasama (normal)
    '''   2. ibang pay cycle ang empleyado                             -> hindi kasama (normal)
    '''   3. umalis na (Resigned/Retired/Terminated) bago ang cutoff   -> hindi kasama (normal)
    '''   4. Inactive at walang petsa ng pag-alis                      -> hindi kasama (normal)
    '''   5. walang Pay Cycle / Earnings                               -> BABALA
    '''   6. walang Date Joined                                        -> BABALA
    '''   7. Date Joined ay pagkatapos ng cutoff                       -> hindi kasama (normal)
    '''   8. walang Basic Salary (Basic cycle) / Daily Rate (Daily)    -> BABALA
    '''   9. lahat ng iba                                              -> KASAMA
    '''
    ''' PAALALA: Petsa ang batayan ng pag-alis, hindi ang IsActive. Kaya ang empleyadong na-set na Inactive
    ''' pero ang petsa ng pag-alis ay nasa o pagkatapos ng simula ng cutoff ay kasama pa rin (final pay).
    ''' </summary>
    Public Module PayrollEligibilityRules

        ''' <summary>Ang pinakamaagang petsa ng pag-alis (Resigned, Retired, o Terminated); Nothing kung wala.</summary>
        Public Function SeparationDate(c As EligibilityCandidate) As Date?
            Dim result As Date? = Nothing
            For Each d In {c.DateResigned, c.DateRetired, c.DateTerminated}
                If d.HasValue AndAlso (Not result.HasValue OrElse d.Value.Date < result.Value) Then
                    result = d.Value.Date
                End If
            Next
            Return result
        End Function

        Public Function Evaluate(c As EligibilityCandidate, cutoffCycleType As String,
                                 cutoffStart As Date, cutoffEnd As Date, basis As PayRateBasis) As EligibilityStatus

            If c.HasEarnings AndAlso Not c.PayrollFlag Then Return EligibilityStatus.ExcludedFromPayroll

            Dim hasCycle = c.HasEarnings AndAlso Not String.IsNullOrWhiteSpace(c.PayCycle)
            If hasCycle AndAlso Not String.Equals(PayCycleChoices.Normalize(c.PayCycle),
                                                  PayCycleChoices.Normalize(cutoffCycleType),
                                                  StringComparison.OrdinalIgnoreCase) Then
                Return EligibilityStatus.OtherPayCycle
            End If

            Dim separated = SeparationDate(c)
            If separated.HasValue AndAlso separated.Value < cutoffStart.Date Then Return EligibilityStatus.Separated
            If Not c.IsActive AndAlso Not separated.HasValue Then Return EligibilityStatus.Inactive

            If Not hasCycle Then Return EligibilityStatus.NoPayCycle
            If Not c.DateJoined.HasValue Then Return EligibilityStatus.NoDateJoined
            If c.DateJoined.Value.Date > cutoffEnd.Date Then Return EligibilityStatus.NotYetJoined

            If basis = PayRateBasis.DailyRate Then
                If Not c.DailyRate.HasValue OrElse c.DailyRate.Value <= 0D Then Return EligibilityStatus.NoPayRate
            Else
                If Not c.BasicSalary.HasValue OrElse c.BasicSalary.Value <= 0D Then Return EligibilityStatus.NoPayRate
            End If

            Return EligibilityStatus.Eligible
        End Function

        Public Function IsWarning(status As EligibilityStatus) As Boolean
            Return status = EligibilityStatus.NoPayCycle OrElse
                   status = EligibilityStatus.NoDateJoined OrElse
                   status = EligibilityStatus.NoPayRate
        End Function

        Public Function WarningReason(status As EligibilityStatus, basis As PayRateBasis) As String
            Select Case status
                Case EligibilityStatus.NoPayCycle
                    Return "No Pay Cycle (Employee > Earnings)"
                Case EligibilityStatus.NoDateJoined
                    Return "No Date Joined (Employee > Employment)"
                Case EligibilityStatus.NoPayRate
                    Return If(basis = PayRateBasis.DailyRate,
                              "No Daily Rate (Employee > Earnings)",
                              "No Basic Salary (Employee > Earnings)")
                Case Else
                    Return String.Empty
            End Select
        End Function

        ''' <summary>Hatiin ang lahat ng empleyado sa kasama at sa may babala. Nananatili ang pagkakasunod ng input.</summary>
        Public Function Build(candidates As IEnumerable(Of EligibilityCandidate), cutoffCycleType As String,
                              cutoffStart As Date, cutoffEnd As Date, basis As PayRateBasis) As PayrollEligibilityResult

            Dim result As New PayrollEligibilityResult()

            For Each c In candidates
                Dim status = Evaluate(c, cutoffCycleType, cutoffStart, cutoffEnd, basis)

                If status = EligibilityStatus.Eligible Then
                    result.Eligible.Add(New PayrollEligibleEmployee With {
                        .RecordId = c.RecordId, .EmployeeNo = c.EmployeeNo, .EmployeeName = c.EmployeeName})

                ElseIf IsWarning(status) Then
                    result.Warnings.Add(New PayrollEligibilityWarning With {
                        .RecordId = c.RecordId, .EmployeeNo = c.EmployeeNo, .EmployeeName = c.EmployeeName,
                        .Reason = WarningReason(status, basis)})
                End If
            Next

            Return result
        End Function

    End Module

End Namespace
