' ============================================================
' GlobalShared/Models/LoanOffsetModels.vb
' ============================================================
' Mga data class para sa LOAN OFFSET (renewal ng parehong LoanCode, hal. SSS Salary Loan).
'
' Ang flow:
'   1. Service -> Repository.GetOffsetCandidatesAsync: mga Active na lumang loan ng parehong employee + LoanCode.
'   2. View (frmLoanOffset): pinipili ni user kung alin ang i-offset, ang offset date, ang "Balance per statement"
'      at ang gagawin sa credit (kung meron).
'   3. Service: ginagawa ang LoanOffsetPlan (lahat ng kwenta at validation), saka iisang transaction ang
'      Repository.SaveWithOffsetAsync.
' ============================================================
Namespace GlobalShared.Models

    ''' <summary>Isang hulog ng BAGONG loan na babayaran ng credit (CREDIT-{newLoanId}).</summary>
    Public Class LoanCreditAllocation
        Public Property InstallmentNo As Integer
        Public Property Amount As Decimal
    End Class

    ''' <summary>
    ''' Ang buong plano ng offset, buo na ang kwenta. Ang Repository ay nagsasagawa lang nito - walang
    ''' desisyon doon.
    ''' </summary>
    Public Class LoanOffsetPlan
        Public Property OldLoanIds As New List(Of Integer)
        Public Property OffsetDate As Date
        Public Property BalancePerStatement As Decimal?

        ''' <summary>Credit sa empleyado (0 kung wala).</summary>
        Public Property Credit As Decimal
        ''' <summary>LoanCreditMode.* (Nothing kung walang credit).</summary>
        Public Property CreditMode As String

        ''' <summary>Para sa First/Last: aling hulog ng bagong loan ang babayaran ng credit.</summary>
        Public Property CreditAllocations As New List(Of LoanCreditAllocation)

        ''' <summary>Para sa Refund: saang Draft cutoff at anong Compensation code.</summary>
        Public Property RefundCutoffId As Integer?
        Public Property RefundCode As String
    End Class

    ''' <summary>Kung saan mapupunta ang refund: ang mapped na Compensation code at ang susunod na Draft cutoff.</summary>
    Public Class LoanRefundTarget
        Public Property CompensationId As Integer?
        Public Property CompensationCode As String
        Public Property CompensationDescription As String
        Public Property InputUnit As Byte?
        Public Property CompensationIsActive As Boolean?

        Public Property PayCycle As String
        Public Property CutoffId As Integer?
        Public Property CutoffLabel As String
    End Class

    ''' <summary>Kalagayan ng refund row (tblPayrollInputTxn) at ng cutoff nito - para sa Undo.</summary>
    Public Class LoanRefundTxnState
        Public Property Found As Boolean
        Public Property CutoffStatus As Byte
        Public Property CutoffLabel As String
    End Class

    ''' <summary>Bilang ng binago ng Undo offset (para sa mensahe kay user).</summary>
    Public Class LoanOffsetUndoCounts
        Public Property OldLoans As Integer
        Public Property OldInstallments As Integer
        Public Property CreditInstallments As Integer
        Public Property RefundRemoved As Boolean
    End Class

    ''' <summary>Ang ibinabalik ng Offset dialog sa Presenter.</summary>
    Public Class LoanOffsetDialogResult
        Public Property Choice As LoanOffsetChoice = LoanOffsetChoice.Cancel
        Public Property Request As LoanOffsetRequest
    End Class

End Namespace
