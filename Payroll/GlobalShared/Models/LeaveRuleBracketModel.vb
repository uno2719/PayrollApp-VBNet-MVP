Namespace GlobalShared.Models

    ''' <summary>
    ''' Isang Years-of-Service bracket row mula sa tblLeaveRuleBracket -
    ''' laging anak ng isang LeaveRuleModel (LeaveRuleId). Buo itong
    ''' dini-delete at ini-insert ulit sa bawat Save ng parent Rule
    ''' (tingnan ang ILeaveRuleRepository.ReplaceBracketsAsync) -
    ''' parehong desisyon at dahilan gaya ng
    ''' EmployeeLoanRepository.ReplaceScheduleAsync: mas simple at mas
    ''' malinaw ang "buong palit" kaysa sa row-by-row diffing.
    ''' </summary>
    Public Class LeaveRuleBracketModel
        Public Property Id As Integer
        Public Property LeaveRuleId As Integer

        Public Property YosFrom As Decimal
        Public Property YosTo As Decimal
        Public Property Entitlement As Decimal
        Public Property AnniversaryCredit As Decimal
        Public Property Prorate As Boolean
        Public Property ForfeitOnMonth As Integer

        ' BF = "Bring Forward" (pwede bang i-carry over ang di-
        ' nagamit na balance), BFMax = max na days na pwedeng
        ' i-carry over. Parehong pangalan gaya ng nakita sa C1Pay
        ' reference (BF / BF Max column headers).
        Public Property BF As Boolean
        Public Property BFMax As Decimal
    End Class
End Namespace
