' File: GlobalShared/Helpers/MinimumWageRules.vb
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    ''' <summary>Paano ang rate ng empleyado kumpara sa minimum wage ng Wage Area ng branch niya.</summary>
    Public Enum WageComparison
        Unknown = 0       ' walang Wage Area ang branch, o walang rate na naka-set para sa petsang iyon
        BelowMinimum = 1  ' mas mababa pa sa minimum wage (paalala: baka mali ang sahod o luma na)
        AtMinimum = 2     ' kapareho ng minimum wage = Minimum Wage Earner
        AboveMinimum = 3  ' mas mataas = HINDI Minimum Wage Earner
    End Enum

    ''' <summary>
    ''' PURE LOGIC LANG: minimum wage (MWE / tax-exempt) na paalala.
    '''
    ''' Ang batayan ng TAX ay ang checkbox na "Minimum Wage Earner" sa Earnings (manual, gaya ng C1Pay). Ang module na ito ay
    ''' NAGMUMUNGKAHI lang: ang Wage Area ay galing sa branch/store ng empleyado (tblBranch.WageArea), at ang minimum wage ay ang
    ''' rate ng Wage Area na epektibo sa petsa (tblMinimumWage). Kaya kapag lumipat ng store sa ibang Wage Area, ang
    ''' Transfer ang magpapaalala.
    '''
    ''' Daily equivalent ng empleyado = ang Daily Rate sa Earnings (sa Basic Salary cycle: Basic x 12 / Days in Year).
    ''' MWE = Daily equivalent &lt;= minimum wage (may 0.01 na palugit para sa rounding).
    ''' </summary>
    Public Module MinimumWageRules

        Public Const Tolerance As Decimal = 0.01D

        ''' <summary>Ang rate na epektibo sa petsa (pinakahuling EffectiveDate na hindi lampas sa onDate). Nothing kung wala.</summary>
        Public Function RateOn(rates As IEnumerable(Of MinimumWageModel), wageArea As String, onDate As Date) As MinimumWageModel
            If rates Is Nothing OrElse String.IsNullOrWhiteSpace(wageArea) Then Return Nothing
            Dim area = wageArea.Trim()
            Return rates.Where(Function(r) String.Equals(If(r.WageArea, "").Trim(), area, StringComparison.OrdinalIgnoreCase) AndAlso
                                           r.EffectiveDate.Date <= onDate.Date).
                         OrderByDescending(Function(r) r.EffectiveDate).
                         FirstOrDefault()
        End Function

        Public Function Compare(dailyEquivalent As Decimal?, minimum As MinimumWageModel) As WageComparison
            If minimum Is Nothing OrElse Not dailyEquivalent.HasValue OrElse dailyEquivalent.Value <= 0D Then Return WageComparison.Unknown
            Dim diff = dailyEquivalent.Value - minimum.DailyRate
            If diff > Tolerance Then Return WageComparison.AboveMinimum
            If diff < -Tolerance Then Return WageComparison.BelowMinimum
            Return WageComparison.AtMinimum
        End Function

        ''' <summary>Mungkahing halaga ng "Minimum Wage Earner" checkbox. Nothing = walang mungkahi (hindi alam).</summary>
        Public Function SuggestedMwe(comparison As WageComparison) As Boolean?
            Select Case comparison
                Case WageComparison.AboveMinimum : Return False
                Case WageComparison.AtMinimum, WageComparison.BelowMinimum : Return True
                Case Else : Return Nothing
            End Select
        End Function

        ''' <summary>Paliwanag para sa dialog, hal. "Daily ₱800.00 is above the NCR minimum wage ₱755.00 (...): not a Minimum Wage Earner."</summary>
        Public Function ExplainText(dailyEquivalent As Decimal?, minimum As MinimumWageModel, wageArea As String) As String
            If String.IsNullOrWhiteSpace(wageArea) Then
                Return "The branch has no Wage Area yet (Settings > Payroll > Minimum Wage), so set Minimum Wage Earner manually."
            End If
            If minimum Is Nothing Then
                Return $"No minimum wage is set for {wageArea.Trim()} on this date (Settings > Payroll > Minimum Wage). Set Minimum Wage Earner manually."
            End If
            If Not dailyEquivalent.HasValue OrElse dailyEquivalent.Value <= 0D Then
                Return $"Minimum wage: {minimum.DisplayText}."
            End If

            Dim daily = $"Daily ₱{dailyEquivalent.Value:N2}"
            Select Case Compare(dailyEquivalent, minimum)
                Case WageComparison.AboveMinimum
                    Return $"{daily} is above the minimum wage {minimum.DisplayText}: NOT a Minimum Wage Earner (taxable)."
                Case WageComparison.AtMinimum
                    Return $"{daily} is the minimum wage {minimum.DisplayText}: Minimum Wage Earner (tax-exempt)."
                Case Else
                    Return $"{daily} is BELOW the minimum wage {minimum.DisplayText}. Please check the salary."
            End Select
        End Function

    End Module

End Namespace
