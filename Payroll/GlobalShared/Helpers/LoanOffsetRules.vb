' ============================================================
' GlobalShared/Helpers/LoanOffsetRules.vb
' ============================================================
' Purong kwenta ng LOAN OFFSET - walang database, walang UI.
' Ginagamit ng frmLoanOffset (live na preview habang nagta-type) AT ng EmployeeLoanService (ang huling salita
' bago mag-save), kaya iisa lang ang formula at hindi sila magkakaiba.
' ============================================================
Imports Payroll.GlobalShared.Models

Namespace GlobalShared.Helpers

    Public Module LoanOffsetRules

        ''' <summary>
        ''' CREDIT = Balance per statement - natitirang hindi bayad sa system (kung positibo).
        '''
        ''' BAKIT GANITO ANG DIREKSYON?
        '''   Ang statement (hal. SSS, as of 11/04/2026) ang halagang talagang ibinawas sa bagong loan para
        '''   bayaran ang luma. Kung may kinaltas pa ang payroll sa lumang loan PAGKATAPOS ng as-of date na iyon
        '''   (hal. Nov 15 cutoff), mas maliit na ang natitira sa system kaysa sa statement - ibig sabihin
        '''   dalawang beses nabayaran ang bahaging iyon. Ang diperensya ang credit ng empleyado.
        '''
        '''   Kapag mas MALAKI ang natitira sa system kaysa statement, walang credit - isinasara lang ng offset
        '''   ang mga hulog (hal. interes na hindi na sinisingil ng SSS).
        '''
        '''   Walang Balance per statement = walang credit.
        ''' </summary>
        Public Function ComputeCredit(balancePerStatement As Decimal?, unpaidInSystem As Decimal) As Decimal
            If Not balancePerStatement.HasValue OrElse balancePerStatement.Value <= 0D Then Return 0D
            Dim credit = Math.Round(balancePerStatement.Value - unpaidInSystem, 2, MidpointRounding.AwayFromZero)
            Return If(credit > 0D, credit, 0D)
        End Function

        ''' <summary>
        ''' Hinahati ang credit sa mga hulog ng bagong loan, mula sa una (fromLast = False) o mula sa huli
        ''' (fromLast = True). Kapag lumampas ang credit sa isang hulog, dadaloy ito sa susunod.
        ''' Ibinabalik ang Nothing kung mas malaki ang credit kaysa sa buong loan (hindi kasya).
        ''' </summary>
        Public Function AllocateCredit(schedule As IEnumerable(Of EmployeeLoanScheduleModel),
                                       credit As Decimal,
                                       fromLast As Boolean) As List(Of LoanCreditAllocation)

            Dim result As New List(Of LoanCreditAllocation)
            If credit <= 0D OrElse schedule Is Nothing Then Return result

            Dim rows = If(fromLast,
                          schedule.OrderByDescending(Function(s) s.InstallmentNo),
                          schedule.OrderBy(Function(s) s.InstallmentNo)).ToList()

            Dim remaining = credit
            For Each row In rows
                If remaining <= 0D Then Exit For
                Dim open = row.AmountDue - row.AmountPaid
                If open <= 0D Then Continue For

                Dim take = Math.Min(open, remaining)
                result.Add(New LoanCreditAllocation With {.InstallmentNo = row.InstallmentNo, .Amount = take})
                remaining -= take
            Next

            If remaining > 0D Then Return Nothing
            Return result

        End Function

        ''' <summary>Ang idadagdag sa Remark ng LUMANG loan (Remark ay nvarchar(1000)).</summary>
        Public Function BuildOldLoanRemark(newLoanId As Integer, offsetDate As Date, balancePerStatement As Decimal?) As String
            Dim text = $"Offset by loan #{newLoanId} on {offsetDate.ToString(Constants.AppConstants.DisplayDateFormat)}"
            If balancePerStatement.HasValue AndAlso balancePerStatement.Value > 0D Then
                text &= $"; balance per statement {balancePerStatement.Value:n2}"
            End If
            Return text
        End Function

    End Module

End Namespace
