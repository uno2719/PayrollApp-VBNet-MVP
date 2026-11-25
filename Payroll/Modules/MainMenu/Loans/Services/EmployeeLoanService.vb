' ============================================================
' Modules/MainMenu/Loans/Services/EmployeeLoanService.vb
' ============================================================
' DITO NAKATIRA ANG LAHAT NG MATEMATIKA AT RULES NG LOAN.
'
' Sinasadya kong walang kahit isang kalkulasyon sa View o sa
' Presenter. Ang dahilan: ang parehong kwenta ay kakailanganin
' ulit ng Payroll module mamaya kapag nagpo-post na ng kaltas.
' Kung nasa View ang formula, kokopyahin mo siya - at sa sandaling
' may dalawang kopya, may araw na magkakaiba sila.
' ============================================================
Imports Payroll.GlobalShared.Constants
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace Loans.Services

    Public Class EmployeeLoanService
        Implements IEmployeeLoanService

        Private ReadOnly _repo As Data.IEmployeeLoanRepository

        ' Proteksyon lang laban sa aksidenteng 999999 na Terms na
        ' gagawa ng milyong rows sa schedule table.
        Private Const MaxTerms As Integer = 600

        Public Sub New(repo As Data.IEmployeeLoanRepository)
            _repo = repo
        End Sub

        ' ========================================================
        ' READ
        ' ========================================================
        Public Async Function GetByEmployeeAsync(employeeRecordId As Integer, statusFilter As String) _
            As Task(Of List(Of EmployeeLoanModel)) _
            Implements IEmployeeLoanService.GetByEmployeeAsync

            If employeeRecordId <= 0 Then Return New List(Of EmployeeLoanModel)

            Return Await _repo.GetByEmployeeAsync(employeeRecordId, statusFilter)

        End Function

        Public Async Function GetDetailAsync(loanId As Integer) As Task(Of EmployeeLoanDetailModel) _
            Implements IEmployeeLoanService.GetDetailAsync

            Dim loan = Await _repo.GetByIdAsync(loanId)
            If loan Is Nothing Then Return Nothing

            Dim schedule = Await _repo.GetScheduleAsync(loanId)

            Return New EmployeeLoanDetailModel With {
                .Loan = loan,
                .Schedule = schedule
            }

        End Function

        ' ========================================================
        ' COMPUTE
        ' ========================================================
        ' ANG APAT NA KOMBINASYON:
        '
        '   Straight Line + Based on Term    -> alam ang Terms, hanapin ang hulog
        '   Straight Line + Based on Amort   -> alam ang hulog, hanapin ang Terms
        '   Diminishing   + Based on Term    -> annuity formula
        '   Diminishing   + Based on Amort   -> baliktad na annuity (logarithm)
        '
        ' PAALALA TUNGKOL SA INTEREST RATE:
        '   Itinuturing ko ang InterestRate bilang PER-PERIOD rate
        '   para sa Diminishing (tulad ng SSS/Pag-IBIG salary loan),
        '   at bilang FLAT na rate sa KABUUAN ng utang para sa
        '   Straight Line.
        '
        '   Kausapin mo ang accounting officer ninyo kung tugma ito
        '   sa aktwal ninyong practice. Isang lugar lang ito na
        '   babaguhin (ComputeStraightLine / ComputeDiminishing) -
        '   kaya kong linisin agad kung iba pala.
        ' ========================================================
        Public Function Compute(item As EmployeeLoanModel) As LoanComputationResult _
            Implements IEmployeeLoanService.Compute

            Dim result As New LoanComputationResult With {.IsComplete = False}

            If item Is Nothing Then Return result

            ' Linisin muna ang negatibo - mas mabuting mag-zero kaysa
            ' mag-produce ng negatibong amortization.
            If item.PrincipalAmount < 0D Then item.PrincipalAmount = 0D
            If item.InterestRate < 0D Then item.InterestRate = 0D
            If item.Terms < 0 Then item.Terms = 0
            If item.TotalAmortization < 0D Then item.TotalAmortization = 0D

            If item.PrincipalAmount <= 0D Then
                ZeroOutComputed(item)
                result.Warning = "Principal Amount is required."
                Return result
            End If

            Dim isDiminishing = String.Equals(item.InterestMethod,
                                              LoanInterestMethod.Diminishing,
                                              StringComparison.OrdinalIgnoreCase)

            If isDiminishing Then
                Return ComputeDiminishing(item)
            Else
                Return ComputeStraightLine(item)
            End If

        End Function

        ' --------------------------------------------------------
        ' STRAIGHT LINE
        ' --------------------------------------------------------
        ' Interest = Principal x Rate%  (isang beses, para sa buong utang)
        ' Pantay-pantay ang hati sa lahat ng hulog.
        ' --------------------------------------------------------
        Private Function ComputeStraightLine(item As EmployeeLoanModel) As LoanComputationResult

            Dim result As New LoanComputationResult

            item.InterestAmount = RoundMoney(item.PrincipalAmount * (item.InterestRate / 100D))
            item.TotalLoanAmount = item.PrincipalAmount + item.InterestAmount

            If String.Equals(item.AmortizationMethod, LoanAmortizationMethod.BasedOnAmort,
                             StringComparison.OrdinalIgnoreCase) Then

                ' Alam natin ang hulog, hanapin ang bilang ng termino.
                If item.TotalAmortization <= 0D Then
                    item.Terms = 0
                    result.Warning = "Total Amortization is required when using 'Based on Amort'."
                    Return result
                End If

                ' Ceiling - kung may sobra, may karagdagang (mas maliit
                ' na) huling hulog. Hindi Floor: iiwan ka noon ng
                ' hindi nababayarang tira.
                item.Terms = CInt(Math.Ceiling(item.TotalLoanAmount / item.TotalAmortization))

            Else

                ' Alam natin ang termino, hanapin ang hulog.
                If item.Terms <= 0 Then
                    item.TotalAmortization = 0D
                    result.Warning = "Terms is required when using 'Based on Term'."
                    Return result
                End If

                item.TotalAmortization = RoundMoney(item.TotalLoanAmount / item.Terms)

            End If

            If item.Terms > MaxTerms Then
                result.Warning = $"Terms exceeds the maximum of {MaxTerms}."
                Return result
            End If

            item.PrincipalAmortization = RoundMoney(item.PrincipalAmount / item.Terms)
            item.InterestAmortization = RoundMoney(item.InterestAmount / item.Terms)

            result.IsComplete = True
            Return result

        End Function

        ' --------------------------------------------------------
        ' DIMINISHING
        ' --------------------------------------------------------
        ' Annuity formula:   A = P x i / (1 - (1+i)^-n)
        ' Baliktad:          n = -ln(1 - P x i / A) / ln(1+i)
        '
        ' Bawat hulog, ang interes ay base sa NATITIRANG balanse -
        ' kaya bumababa ang interes habang tumatagal, at tumataas
        ' ang bahaging napupunta sa principal.
        ' --------------------------------------------------------
        Private Function ComputeDiminishing(item As EmployeeLoanModel) As LoanComputationResult

            Dim result As New LoanComputationResult

            Dim i As Double = CDbl(PeriodRate(item))
            Dim p As Double = CDbl(item.PrincipalAmount)

            ' --- Walang interes? Straight line na lang effectively. ---
            If i <= 0.0 Then
                Return ComputeStraightLine(item)
            End If

            If String.Equals(item.AmortizationMethod, LoanAmortizationMethod.BasedOnAmort,
                             StringComparison.OrdinalIgnoreCase) Then

                Dim a As Double = CDbl(item.TotalAmortization)

                If a <= 0.0 Then
                    result.Warning = "Total Amortization is required when using 'Based on Amort'."
                    Return result
                End If

                ' ANG PINAKAMAHALAGANG CHECK DITO:
                ' Kung ang hulog ay hindi lalampas sa interes ng unang
                ' period (P x i), ang balanse ay HINDI kailanman bababa.
                ' Walang solusyon ang formula - infinite ang utang.
                If a <= p * i Then
                    result.Warning =
                        "The amortization is too small - it does not even cover the interest. " &
                        "The loan would never be fully paid. Please increase it."
                    Return result
                End If

                Dim n As Double = -Math.Log(1.0 - (p * i / a)) / Math.Log(1.0 + i)
                item.Terms = TermsFromN(n, a)

            Else

                If item.Terms <= 0 Then
                    result.Warning = "Terms is required when using 'Based on Term'."
                    Return result
                End If

                Dim n As Double = CDbl(item.Terms)
                Dim a As Double = p * i / (1.0 - Math.Pow(1.0 + i, -n))

                item.TotalAmortization = RoundMoney(CDec(a))

            End If

            If item.Terms > MaxTerms Then
                result.Warning = $"Terms exceeds the maximum of {MaxTerms}."
                Return result
            End If

            ' Ang totoong kabuuang interes ay HINDI simpleng P x rate -
            ' kailangan nating patakbuhin ang buong hulugan para malaman.
            ' Kaya ginagamit natin dito ang mismong schedule generator:
            ' iisang pinagmumulan ng katotohanan, walang dalawang bersyon
            ' ng kwenta na pwedeng maghiwalay.
            Dim preview = BuildInstallments(item)

            item.InterestAmount = RoundMoney(preview.Sum(Function(s) s.InterestDue))
            item.TotalLoanAmount = item.PrincipalAmount + item.InterestAmount

            ' Sa diminishing, ang hati ng unang hulog ang ipinapakita -
            ' nagbabago naman kasi ito kada period. Ang buong detalye
            ' ay makikita sa Details window.
            Dim first = preview.FirstOrDefault()
            If first IsNot Nothing Then
                item.PrincipalAmortization = first.PrincipalDue
                item.InterestAmortization = first.InterestDue
            End If

            result.IsComplete = True
            Return result

        End Function

        ' ========================================================
        ' SCHEDULE GENERATION
        ' ========================================================
        Public Function GenerateSchedule(item As EmployeeLoanModel) As List(Of EmployeeLoanScheduleModel) _
            Implements IEmployeeLoanService.GenerateSchedule

            Dim installments = BuildInstallments(item)

            ' Every Cycle + Per Month: ang bawat BUWANANG hulog ay hinahati sa dalawang kaltas (ika-15 at katapusan).
            If IsEveryCyclePerMonth(item) Then installments = SplitPerMonth(installments)

            Dim dates = BuildDeductionDates(item.LoanStartDate, item.Frequency, installments.Count)

            For idx = 0 To installments.Count - 1
                installments(idx).InstallmentNo = idx + 1
                installments(idx).ScheduledDate = dates(idx)
            Next

            Return installments

        End Function

        ' --------------------------------------------------------
        ' Ang halaga ng bawat hulog (wala pang petsa)
        ' --------------------------------------------------------
        ' ANG PROBLEMA NG ROUNDING:
        '   16,885.80 / 24 = 703.575 -> 703.58
        '   703.58 x 24 = 16,885.92  <- 12 sentimo na sobra!
        '
        '   Kaya ang HULING hulog ay hindi kinukuwenta - ito ang
        '   natitira. Kung ano ang kulang o sobra, doon napupunta.
        '   Sa ganitong paraan, ang kabuuan ng schedule ay LAGING
        '   eksaktong katumbas ng TotalLoanAmount. Mahalaga ito -
        '   kung hindi, may mga utang na "bayad na" pero may naiwang
        '   2 sentimo, o kaya nasosobrahan ng kaltas ang empleyado.
        ' --------------------------------------------------------
        Private Function BuildInstallments(item As EmployeeLoanModel) As List(Of EmployeeLoanScheduleModel)

            Dim list As New List(Of EmployeeLoanScheduleModel)

            If item Is Nothing OrElse item.Terms <= 0 Then Return list

            Dim terms = Math.Min(item.Terms, MaxTerms)

            Dim isDiminishing = String.Equals(item.InterestMethod,
                                              LoanInterestMethod.Diminishing,
                                              StringComparison.OrdinalIgnoreCase) AndAlso
                                item.InterestRate > 0D

            If isDiminishing Then

                Dim i As Decimal = PeriodRate(item)
                Dim balance As Decimal = item.PrincipalAmount
                Dim amort As Decimal = item.TotalAmortization
                Dim isBasedOnAmort = String.Equals(item.AmortizationMethod, LoanAmortizationMethod.BasedOnAmort,
                                                   StringComparison.OrdinalIgnoreCase)

                For n = 1 To terms

                    Dim interestDue = RoundMoney(balance * i)
                    Dim principalDue As Decimal
                    Dim amountDue As Decimal

                    If n = terms Then
                        ' Huling hulog: linisin ang natitirang balanse.
                        principalDue = balance
                        amountDue = principalDue + interestDue

                        ' SSS-STYLE NA HULING HULOG (Based on Amort lang):
                        '   Kapag halos kapareho na ng amortization ang huling hulog (diperensya <= LastRowTolerance),
                        '   pantay na amortization pa rin ang singil at ang INTERES ang nag-a-adjust.
                        '   Ebidensya: SSS statement, row 24 = 1,797.13 principal + 11.96 interest = 1,809.09
                        '   (ang kuwentong 1,797.13 x rate ay 11.98). Kaya tugma ang total sa 43,418.16.
                        If isBasedOnAmort AndAlso amort > 0D AndAlso
                           Math.Abs(amountDue - amort) <= LastRowTolerance AndAlso
                           amort - principalDue >= 0D Then
                            interestDue = amort - principalDue
                            amountDue = amort
                        End If
                    Else
                        principalDue = RoundMoney(amort - interestDue)

                        ' Bantay: baka lumampas sa natitira (nangyayari
                        ' ito kapag may rounding drift sa dulo).
                        If principalDue > balance Then principalDue = balance

                        amountDue = principalDue + interestDue
                    End If

                    list.Add(New EmployeeLoanScheduleModel With {
                        .InstallmentNo = n,
                        .PrincipalDue = principalDue,
                        .InterestDue = interestDue,
                        .AmountDue = amountDue
                    })

                    balance -= principalDue

                    ' Bayad na bago pa maubos ang terms? Tapusin na.
                    If balance <= 0D Then Exit For

                Next

            Else

                ' --- STRAIGHT LINE: pantay-pantay, tira sa huli ---
                Dim runningPrincipal As Decimal = 0D
                Dim runningInterest As Decimal = 0D

                For n = 1 To terms

                    Dim principalDue As Decimal
                    Dim interestDue As Decimal

                    If n = terms Then
                        principalDue = item.PrincipalAmount - runningPrincipal
                        interestDue = item.InterestAmount - runningInterest
                    Else
                        principalDue = item.PrincipalAmortization
                        interestDue = item.InterestAmortization
                    End If

                    runningPrincipal += principalDue
                    runningInterest += interestDue

                    list.Add(New EmployeeLoanScheduleModel With {
                        .InstallmentNo = n,
                        .PrincipalDue = principalDue,
                        .InterestDue = interestDue,
                        .AmountDue = principalDue + interestDue
                    })

                Next

            End If

            Return list

        End Function

        ' --------------------------------------------------------
        ' Ang PETSA ng bawat kaltas
        ' --------------------------------------------------------
        ' PANSAMANTALA ITO.
        '
        '   Wala pa tayong Cutoff module (aceSettingsCutOff - hindi pa
        '   naka-wire). Kapag nagawa na iyon, ang tamang gawin ay
        '   kunin ang AKTWAL na cutoff dates mula sa tblCutoff imbes
        '   na hulaan sa ika-15 at katapusan.
        '
        '   Ihiwalay ko na siya ngayon bilang sarili niyang function
        '   para isang method lang ang papalitan mo mamaya - walang
        '   ibang bahagi ng module ang masisira.
        ' --------------------------------------------------------
        Private Function BuildDeductionDates(startDate As Date,
                                             frequency As String,
                                             count As Integer) As List(Of Date)

            Dim dates As New List(Of Date)
            If count <= 0 Then Return dates

            Select Case frequency

                Case LoanFrequency.EveryCycle
                    ' Dalawang kaltas kada buwan: ika-15 at katapusan.
                    Dim cursor = startDate
                    While dates.Count < count
                        Dim mid = New Date(cursor.Year, cursor.Month, 15)
                        Dim endOfMonth = New Date(cursor.Year, cursor.Month,
                                                  Date.DaysInMonth(cursor.Year, cursor.Month))

                        If mid >= startDate AndAlso dates.Count < count Then dates.Add(mid)
                        If endOfMonth >= startDate AndAlso dates.Count < count Then dates.Add(endOfMonth)

                        cursor = cursor.AddMonths(1)
                    End While

                Case LoanFrequency.SecondCycleOnly
                    Dim cursor = startDate
                    While dates.Count < count
                        Dim endOfMonth = New Date(cursor.Year, cursor.Month,
                                                  Date.DaysInMonth(cursor.Year, cursor.Month))
                        If endOfMonth >= startDate Then dates.Add(endOfMonth)
                        cursor = cursor.AddMonths(1)
                    End While

                Case LoanFrequency.FirstCycleOnly
                    Dim cursor = startDate
                    While dates.Count < count
                        Dim mid = New Date(cursor.Year, cursor.Month, 15)
                        If mid >= startDate Then dates.Add(mid)
                        cursor = cursor.AddMonths(1)
                    End While

                Case Else   ' Monthly - sundin ang araw ng LoanStartDate
                    For n = 0 To count - 1
                        dates.Add(startDate.AddMonths(n))
                    Next

            End Select

            ' Safety net - kung sakaling may nakulang dahil sa edge
            ' case sa petsa, punan ng buwanang agwat. Mas mabuting
            ' may petsa kahit approximate kaysa mag-crash ang grid.
            While dates.Count < count
                dates.Add(dates.Last().AddMonths(1))
            End While

            Return dates.Take(count).ToList()

        End Function

        ' ========================================================
        ' SAVE
        ' ========================================================
        Public Async Function SaveAsync(item As EmployeeLoanModel, userName As String) _
            As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.SaveAsync

            ' --- VALIDATION ---
            If item.EmployeeRecordId <= 0 Then
                Return Fail("Please select an employee first.")
            End If

            If String.IsNullOrWhiteSpace(item.LoanCode) Then
                Return Fail("Loan Code is required.")
            End If

            If item.PrincipalAmount <= 0D Then
                Return Fail("Principal Amount must be greater than zero.")
            End If

            ' --- COMPUTE (bago i-save, laging kinukuwenta ulit) ---
            ' Hindi tayo umaasa sa kung ano ang nasa textbox. Baka
            ' binago ni user ang isang field at hindi na-trigger ang
            ' live recompute. Dito ang huling salita.
            Dim computation = Compute(item)

            If Not computation.IsComplete Then
                Return Fail(If(computation.Warning, "Unable to compute the loan amortization."))
            End If

            If item.Terms <= 0 Then
                Return Fail("Terms must be greater than zero.")
            End If

            ' --- SCHEDULE (ginagawa na bago mag-save, para ma-validate ang opening payments) ---
            Dim schedule = GenerateSchedule(item)

            ' --- OPENING PAYMENTS (bagong loan lang) ---
            Dim openingPaid As Integer = If(item.Id = 0, item.OpeningPaidInstallments, 0)

            If openingPaid < 0 Then
                Return Fail("Installments already paid cannot be negative.")
            End If

            If openingPaid > schedule.Count Then
                Return Fail($"Installments already paid ({openingPaid}) cannot be more than the number of installments ({schedule.Count}).")
            End If

            ' --- OFFSET (bagong loan lang) ---
            ' Kapag may OffsetRequest, iisang transaction ang lahat: insert ng loan + schedule + offset ng luma +
            ' credit. Tingnan ang SaveWithOffsetAsync sa ibaba.
            If item.Id = 0 AndAlso item.OffsetRequest IsNot Nothing Then
                If openingPaid > 0 Then
                    Return Fail("Opening payments (C1Pay) and Offset can't be saved together." & Environment.NewLine &
                                "Save the loan with the offset first, then use Details > Mark paid if needed.")
                End If
                Return Await SaveWithOffsetAsync(item, schedule, userName)
            End If

            ' --- EDIT GUARD ---
            If item.Id > 0 Then
                Dim existing = Await _repo.GetByIdAsync(item.Id)
                If existing Is Nothing Then Return Fail("Loan not found. The list may be out of date - please refresh.")

                If existing.IsOffsetOld Then
                    Return Fail($"This loan was offset by loan #{existing.OffsetByLoanId} and can no longer be edited." &
                                Environment.NewLine & "Open Details and use 'Undo offset' first (Admin only).")
                End If

                If existing.HasOffsetChildren Then
                    Return Fail("This loan paid off (offset) an older loan, so its schedule can't be regenerated." &
                                Environment.NewLine & "Open Details and use 'Undo offset' first (Admin only), then edit.")
                End If

                Dim hasPayment = Await _repo.HasAnyPaymentAsync(item.Id)

                If hasPayment Then
                    ' BAKIT BAWAL?
                    '   Kapag may naitala nang kaltas, ang pagbabago ng
                    '   Principal o Terms ay nangangahulugang buburahin
                    '   ang schedule at gagawa ng bago - kasama na ang
                    '   mga rows na may bayad na. Mawawala ang kasaysayan
                    '   at hindi na magkakatugma ang payroll register sa
                    '   loan ledger.
                    If Await _repo.HasPostedPaymentAsync(item.Id) Then
                        Return Fail(
                            "This loan already has posted deductions and can no longer be edited." &
                            Environment.NewLine &
                            "Set it to Inactive and create a new loan entry instead.")
                    End If

                    ' Opening payments (C1Pay) lang ang bayad - pwedeng ibalik muna.
                    Return Fail(
                        "This loan has opening payments from C1Pay and can't be edited as is." &
                        Environment.NewLine &
                        "Open Details and click 'Undo opening payments' first, edit the loan, then mark the paid installments again.")
                End If
            End If

            ' --- PERSIST ---
            Dim loanId As Integer

            If item.Id = 0 Then
                item.InputDate = Date.Today
                item.Status = LoanStatus.Active      ' <-- automatic Active pag bagong dagdag
                loanId = Await _repo.InsertAsync(item, userName)
            Else
                Await _repo.UpdateAsync(item, userName)
                loanId = item.Id
            End If

            ' --- SCHEDULE ---
            Await _repo.ReplaceScheduleAsync(loanId, schedule, userName)

            item.Id = loanId

            ' --- OPENING PAYMENTS ---
            Dim message As String = Nothing
            Dim marked As Integer = 0

            If openingPaid > 0 Then
                marked = Await _repo.MarkOpeningPaidAsync(loanId, openingPaid, userName)

                message = $"{marked} installment(s) marked as already paid (C1Pay)."

                If openingPaid >= schedule.Count Then
                    Await _repo.SetStatusAsync(loanId, LoanStatus.Completed, userName)
                    item.Status = LoanStatus.Completed
                    message &= " All installments are paid, so the loan is Completed."
                End If
            End If

            Return New LoanSaveResult With {
                .Success = True,
                .NewStatus = item.Status,
                .Message = message,
                .AffectedCount = marked
            }

        End Function

        ' ========================================================
        ' DELETE
        ' ========================================================
        Public Async Function DeleteAsync(loanId As Integer, userName As String) _
            As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.DeleteAsync

            If loanId <= 0 Then Return Fail("Please select a loan first.")

            Dim loan = Await _repo.GetByIdAsync(loanId)
            If loan Is Nothing Then Return Fail("Loan not found. The list may be out of date - please refresh.")

            If loan.IsOffsetOld Then
                Return Fail($"This loan was offset by loan #{loan.OffsetByLoanId} and cannot be deleted." &
                            Environment.NewLine & "Delete the new loan instead (that also undoes the offset).")
            End If

            ' Bagong loan na nag-offset: ibalik muna ang offset (lumang loan -> Active ulit, credit/refund tinatanggal).
            ' Ang View na ang nag-check ng Admin + password bago tumawag dito.
            Dim undoMessage As String = Nothing
            If loan.HasOffsetChildren Then
                Dim undo = Await UndoOffsetAsync(loanId, userName)
                If Not undo.Success Then Return undo
                undoMessage = undo.Message
            End If

            Dim hasPayment = Await _repo.HasAnyPaymentAsync(loanId)

            If hasPayment Then
                ' Parehong dahilan gaya ng edit guard - may kasaysayan
                ' na, hindi na pwedeng ipagkaila.
                If Await _repo.HasPostedPaymentAsync(loanId) Then
                    Return Fail(
                        "This loan already has posted deductions and cannot be deleted." &
                        Environment.NewLine &
                        "Set it to Inactive instead if you want to stop the deduction.")
                End If

                Return Fail(
                    "This loan has opening payments from C1Pay." &
                    Environment.NewLine &
                    "Open Details and click 'Undo opening payments' first if you really want to delete it.")
            End If

            Await _repo.DeleteAsync(loanId)

            Return New LoanSaveResult With {.Success = True, .Message = undoMessage}

        End Function

        ' ========================================================
        ' TOGGLE ACTIVE / INACTIVE
        ' ========================================================
        ' ITO ANG HINIHILING MONG APPROACH:
        '   - Bagong loan          -> automatic Active (nasa SaveAsync)
        '   - Bayad na lahat       -> automatic Completed (nasa RecordPaymentAsync)
        '   - Manual na hinto      -> ito, Active <-> Inactive
        '
        '   Ang Completed ay HINDI pwedeng i-toggle pabalik. Kung
        '   kailangan pa ng dagdag na kaltas, bagong loan entry - hindi
        '   binubuhay ang luma, dahil tapos na ang kasaysayan niya.
        ' ========================================================
        Public Async Function ToggleActiveStatusAsync(loanId As Integer, userName As String) _
            As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.ToggleActiveStatusAsync

            If loanId <= 0 Then Return Fail("Please select a loan first.")

            Dim loan = Await _repo.GetByIdAsync(loanId)
            If loan Is Nothing Then Return Fail("Loan not found.")

            If Not LoanStatus.IsToggleable(loan.Status) Then
                Return Fail($"This loan is already marked as {loan.Status} and can no longer be changed.")
            End If

            Dim newStatus = If(LoanStatus.IsDeductible(loan.Status),
                               LoanStatus.Inactive,
                               LoanStatus.Active)

            Await _repo.SetStatusAsync(loanId, newStatus, userName)

            Return New LoanSaveResult With {.Success = True, .NewStatus = newStatus}

        End Function

        ' ========================================================
        ' OPENING PAYMENTS (nabayaran na sa C1Pay)
        ' ========================================================
        ' Para sa mga loan na nagsimula bago ginamit ang app na ito: ang mga unang hulog ay nakaltas na sa C1Pay,
        ' kaya minamarkahan natin silang bayad (PayrollRefNo = 'C1PAY', PaidDate = ScheduledDate) para hindi
        ' sila makaltas ulit ng Process.
        '
        ' Mga rule:
        '   - Active/Inactive lang ang loan (hindi Completed/Cancelled).
        '   - Bawal kung may bayad na galing sa Payroll Post na - para walang butas sa pagkakasunod-sunod ng hulog.
        '   - Pataas lang ang Mark; para magbawas o magbago, Undo muna tapos Mark ulit.
        ' ========================================================
        Public Async Function MarkOpeningPaidAsync(loanId As Integer, upToInstallmentNo As Integer, userName As String) _
            As Task(Of LoanSaveResult) Implements IEmployeeLoanService.MarkOpeningPaidAsync

            If loanId <= 0 Then Return Fail("Please select a loan first.")
            If upToInstallmentNo <= 0 Then Return Fail("Enter an installment number of 1 or higher.")

            Dim loan = Await _repo.GetByIdAsync(loanId)
            If loan Is Nothing Then Return Fail("Loan not found.")

            If Not LoanStatus.IsToggleable(loan.Status) Then
                Return Fail($"This loan is already {loan.Status} and can no longer be changed.")
            End If

            Dim schedule = Await _repo.GetScheduleAsync(loanId)

            If upToInstallmentNo > schedule.Count Then
                Return Fail($"This loan only has {schedule.Count} installment(s).")
            End If

            If Await _repo.HasPostedPaymentAsync(loanId) Then
                Return Fail("This loan already has deductions posted from payroll. " &
                            "Opening payments can only be set before the first posted deduction.")
            End If

            Dim alreadyMarked = schedule.Where(Function(s) s.IsOpening).
                                         Select(Function(s) s.InstallmentNo).
                                         DefaultIfEmpty(0).Max()

            If upToInstallmentNo <= alreadyMarked Then
                Return Fail($"Installments up to #{alreadyMarked} are already marked as paid." & Environment.NewLine &
                            "To lower the number, use 'Undo opening payments' first, then mark again.")
            End If

            Dim amount = schedule.Where(Function(s) s.InstallmentNo <= upToInstallmentNo AndAlso Not s.IsOpening).
                                  Sum(Function(s) s.AmountDue)

            Dim marked = Await _repo.MarkOpeningPaidAsync(loanId, upToInstallmentNo, userName)

            Dim newStatus = loan.Status

            If upToInstallmentNo >= schedule.Count Then
                Await _repo.SetStatusAsync(loanId, LoanStatus.Completed, userName)
                newStatus = LoanStatus.Completed
            End If

            Return New LoanSaveResult With {
                .Success = True,
                .NewStatus = newStatus,
                .AffectedCount = marked,
                .Message = $"{marked} installment(s) marked as paid (C1PAY), total {amount:n2}." &
                           If(newStatus = LoanStatus.Completed, " The loan is now Completed.", "")
            }

        End Function

        Public Async Function UndoOpeningPaidAsync(loanId As Integer, userName As String) _
            As Task(Of LoanSaveResult) Implements IEmployeeLoanService.UndoOpeningPaidAsync

            If loanId <= 0 Then Return Fail("Please select a loan first.")

            Dim loan = Await _repo.GetByIdAsync(loanId)
            If loan Is Nothing Then Return Fail("Loan not found.")

            If String.Equals(loan.Status, LoanStatus.Cancelled, StringComparison.OrdinalIgnoreCase) Then
                Return Fail("This loan is Cancelled and can no longer be changed.")
            End If

            If loan.IsOffsetOld Then
                Return Fail($"This loan was offset by loan #{loan.OffsetByLoanId}. Use 'Undo offset' first.")
            End If

            If Await _repo.HasPostedPaymentAsync(loanId) Then
                Return Fail("This loan already has deductions posted from payroll, so the opening payments can no longer be undone.")
            End If

            Dim schedule = Await _repo.GetScheduleAsync(loanId)

            If Not schedule.Any(Function(s) s.IsOpening) Then
                Return Fail("This loan has no opening (C1Pay) payments to undo.")
            End If

            Dim undone = Await _repo.UndoOpeningPaidAsync(loanId, userName)

            Dim newStatus = loan.Status

            ' Walang posted payment, kaya ang Completed ay galing lang sa opening payments - ibalik sa Active.
            If String.Equals(loan.Status, LoanStatus.Completed, StringComparison.OrdinalIgnoreCase) Then
                Await _repo.SetStatusAsync(loanId, LoanStatus.Active, userName)
                newStatus = LoanStatus.Active
            End If

            Return New LoanSaveResult With {
                .Success = True,
                .NewStatus = newStatus,
                .AffectedCount = undone,
                .Message = $"{undone} opening payment(s) undone."
            }

        End Function

        ' ========================================================
        ' OFFSET (RENEWAL NG PAREHONG LOAN CODE)
        ' ========================================================
        ' Halimbawa: SSS Salary Loan na ni-renew. Ang bagong loan (40,000) ay may bawas na balanse ng luma
        ' (19,380.43), kaya ang natitirang hulog ng luma ay HINDI na dapat kaltasan.
        '
        ' Ang ginagawa ng Offset sa lumang loan (iisang transaction kasama ang pag-save ng bago):
        '   - lahat ng hindi pa bayad na hulog: AmountPaid = AmountDue, PaidDate = offset date,
        '     PayrollRefNo = OFFSET-{bagongLoanId}
        '   - Status = Offset, OffsetByLoanId/OffsetDate, at dagdag na linya sa Remark
        ' Hindi na siya kukunin ng Process dahil Status = 'Active' lang ang kinakaltasan.
        ' ========================================================
        Public Async Function GetOffsetCandidatesAsync(employeeRecordId As Integer, loanCode As String,
                                                       excludeLoanId As Integer) _
            As Task(Of List(Of LoanOffsetCandidate)) Implements IEmployeeLoanService.GetOffsetCandidatesAsync

            If employeeRecordId <= 0 OrElse String.IsNullOrWhiteSpace(loanCode) Then
                Return New List(Of LoanOffsetCandidate)
            End If

            Return Await _repo.GetOffsetCandidatesAsync(employeeRecordId, loanCode, excludeLoanId)

        End Function

        Private Async Function SaveWithOffsetAsync(item As EmployeeLoanModel,
                                                   schedule As List(Of EmployeeLoanScheduleModel),
                                                   userName As String) As Task(Of LoanSaveResult)

            Dim req = item.OffsetRequest

            If req.OldLoanIds Is Nothing OrElse req.OldLoanIds.Count = 0 Then
                Return Fail("Select at least one old loan to offset, or choose 'Keep both'.")
            End If

            ' Basahin ulit mula sa database - baka may nagbago mula nang buksan ang dialog.
            Dim candidates = Await _repo.GetOffsetCandidatesAsync(item.EmployeeRecordId, item.LoanCode, 0)
            Dim selected = candidates.Where(Function(c) req.OldLoanIds.Contains(c.LoanId)).ToList()

            If selected.Count <> req.OldLoanIds.Distinct().Count() Then
                Return Fail("One of the selected loans is no longer Active or has no unpaid installments." &
                            Environment.NewLine & "Close this message and save again to refresh the list.")
            End If

            Dim blocked = selected.FirstOrDefault(Function(c) c.IsBlocked)
            If blocked IsNot Nothing Then
                Return Fail($"Loan #{blocked.LoanId} can't be offset yet:" & Environment.NewLine & blocked.BlockedReason)
            End If

            Dim earliestStart = selected.Min(Function(c) c.LoanStartDate)
            If req.OffsetDate.Date < earliestStart.Date Then
                Return Fail("The offset date can't be earlier than the start date of the old loan.")
            End If

            ' --- CREDIT ---
            Dim unpaid = selected.Sum(Function(c) c.UnpaidAmount)
            Dim credit = LoanOffsetRules.ComputeCredit(req.BalancePerStatement, unpaid)

            Dim plan As New LoanOffsetPlan With {
                .OldLoanIds = selected.Select(Function(c) c.LoanId).ToList(),
                .OffsetDate = req.OffsetDate.Date,
                .BalancePerStatement = req.BalancePerStatement,
                .Credit = credit
            }

            Dim creditNote As String = Nothing

            If credit > 0D Then

                Select Case req.CreditMode

                    Case LoanCreditMode.ApplyToFirst, LoanCreditMode.ApplyToLast
                        Dim fromLast = (req.CreditMode = LoanCreditMode.ApplyToLast)
                        Dim allocations = LoanOffsetRules.AllocateCredit(schedule, credit, fromLast)
                        If allocations Is Nothing Then
                            Return Fail($"The credit ({credit:n2}) is bigger than the whole new loan ({item.TotalLoanAmount:n2}). " &
                                        "Use 'Refund through payroll' instead.")
                        End If
                        plan.CreditAllocations = allocations
                        creditNote = $"Credit {credit:n2} applied to installment(s) " &
                                     String.Join(", ", allocations.Select(Function(a) "#" & a.InstallmentNo)) & "."

                    Case LoanCreditMode.RefundThroughPayroll
                        Dim target = Await _repo.GetRefundTargetAsync(item.EmployeeRecordId, plan.OffsetDate)

                        If target Is Nothing OrElse Not target.CompensationId.HasValue Then
                            Return Fail("No 'Loan Refund Code' is set." & Environment.NewLine &
                                        "Set it in Settings > General > Code Mapping (a Compensation with Input unit = Amount), then save again.")
                        End If
                        If String.IsNullOrWhiteSpace(target.CompensationCode) Then
                            Return Fail("The 'Loan Refund Code' in General Settings points to a Compensation that no longer exists.")
                        End If
                        If target.InputUnit.HasValue AndAlso target.InputUnit.Value <> CByte(PayrollInputUnit.Amount) Then
                            Return Fail($"The Loan Refund Code '{target.CompensationCode}' must use Input unit = Amount (Payroll Settings > Compensation).")
                        End If
                        If String.IsNullOrWhiteSpace(target.PayCycle) Then
                            Return Fail("This employee has no Pay Cycle (Employee > Earnings), so there is no cutoff to put the refund in.")
                        End If
                        If Not target.CutoffId.HasValue Then
                            Return Fail($"There is no Draft {target.PayCycle} cutoff ending on or after {plan.OffsetDate.ToString(AppConstants.DisplayDateFormat)}." &
                                        Environment.NewLine & "Generate the cutoffs first (Payroll Settings > Cutoff), then save again.")
                        End If

                        plan.RefundCutoffId = target.CutoffId
                        plan.RefundCode = target.CompensationCode
                        creditNote = $"Credit {credit:n2} will be refunded in cutoff {target.CutoffLabel} as '{target.CompensationCode}'."

                    Case Else
                        Return Fail($"There is a credit of {credit:n2} for the employee. Choose what to do with it.")

                End Select

                plan.CreditMode = req.CreditMode

            End If

            ' --- PERSIST (iisang transaction) ---
            item.InputDate = Date.Today
            item.Status = LoanStatus.Active
            item.OffsetCredit = credit
            item.OffsetCreditMode = plan.CreditMode

            Dim newId = Await _repo.SaveWithOffsetAsync(item, schedule, plan, userName)
            item.Id = newId

            Dim message = $"{selected.Count} old loan(s) offset ({String.Join(", ", selected.Select(Function(c) "#" & c.LoanId))}): " &
                          $"{selected.Sum(Function(c) c.UnpaidCount)} unpaid installment(s), total {unpaid:n2}, marked as paid by this loan."
            If creditNote IsNot Nothing Then message &= Environment.NewLine & creditNote

            Return New LoanSaveResult With {
                .Success = True,
                .NewStatus = item.Status,
                .Message = message,
                .AffectedCount = selected.Count
            }

        End Function

        ''' <summary>
        ''' Ibinabalik ang offset na ginawa ng BAGONG loan (newLoanId): ang lumang loan ay Active ulit at
        ''' kakaltasan na ulit, ang credit sa bagong loan ay tinatanggal, at ang refund row (kung Draft pa ang
        ''' cutoff) ay binubura. Admin + password - nasa View ang check na iyon.
        ''' </summary>
        Public Async Function UndoOffsetAsync(newLoanId As Integer, userName As String) As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.UndoOffsetAsync

            If newLoanId <= 0 Then Return Fail("Please select a loan first.")

            Dim loan = Await _repo.GetByIdAsync(newLoanId)
            If loan Is Nothing Then Return Fail("Loan not found.")

            If Not loan.HasOffsetChildren Then
                Return Fail("This loan did not offset any older loan, so there is nothing to undo.")
            End If

            ' Kapag may kaltas na mula sa payroll ang bagong loan, hindi na malinis ibalik: ang PR- row ay
            ' maaaring na-overwrite na ang CREDIT- ref, at ang luma ay kakaltasan ulit nang sabay.
            If Await _repo.HasPostedPaymentAsync(newLoanId) Then
                Return Fail("This loan already has deductions posted from payroll, so the offset can no longer be undone.")
            End If

            If loan.OffsetRefundTxnId.HasValue Then
                Dim refund = Await _repo.GetRefundTxnStateAsync(loan.OffsetRefundTxnId.Value)
                If refund.Found AndAlso refund.CutoffStatus <> CByte(CutoffStatus.Draft) Then
                    Return Fail($"The refund of this offset is already in cutoff {refund.CutoffLabel}, which is no longer Draft." &
                                Environment.NewLine & "The offset can no longer be undone.")
                End If
            End If

            Dim counts = Await _repo.UndoOffsetAsync(newLoanId, userName)

            Dim message = $"Offset undone: {counts.OldLoans} old loan(s) are Active again ({counts.OldInstallments} installment(s) set back to unpaid)."
            If counts.CreditInstallments > 0 Then message &= $" Credit removed from {counts.CreditInstallments} installment(s) of this loan."
            If counts.RefundRemoved Then message &= " The refund entry was removed from the payroll input."

            Return New LoanSaveResult With {.Success = True, .NewStatus = loan.Status, .Message = message, .AffectedCount = counts.OldLoans}

        End Function

        ' ========================================================
        ' RECORD PAYMENT
        ' ========================================================
        ' Tatawagin ito ng Payroll module kapag na-post na ang cutoff.
        ' Dito rin nangyayari ang automatic na paglipat sa Completed.
        ' ========================================================
        Public Async Function RecordPaymentAsync(scheduleId As Integer, loanId As Integer,
                                                 amountPaid As Decimal, paidDate As Date,
                                                 payrollRefNo As String, userName As String) _
            As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.RecordPaymentAsync

            Await _repo.MarkInstallmentPaidAsync(scheduleId, amountPaid, paidDate, payrollRefNo, userName)

            ' Basahin ulit - kailangan nating makita ang BAGONG bilang
            ' ng bayad na installments, hindi yung nasa memory kanina.
            Dim loan = Await _repo.GetByIdAsync(loanId)

            If loan IsNot Nothing AndAlso loan.IsFullyPaid AndAlso
               Not String.Equals(loan.Status, LoanStatus.Completed, StringComparison.OrdinalIgnoreCase) Then

                Await _repo.SetStatusAsync(loanId, LoanStatus.Completed, userName)

                Return New LoanSaveResult With {.Success = True, .NewStatus = LoanStatus.Completed}
            End If

            Return New LoanSaveResult With {.Success = True, .NewStatus = loan?.Status}

        End Function

        ' ========================================================
        ' HELPERS
        ' ========================================================
        ' --------------------------------------------------------
        ' RATE, TERMS AT PER-MONTH SPLIT
        ' --------------------------------------------------------

        ' Hanggang ilang piso ang pwedeng diperensya ng huling hulog sa amortization bago ito gawing
        ' pantay (interes ang mag-a-adjust). Tingnan ang BuildInstallments.
        Private Const LastRowTolerance As Decimal = 1D

        ''' <summary>
        ''' Ilang hulog kada taon. Every Cycle na kada kaltas ang amortization = 24; lahat ng iba = 12
        ''' (Monthly, First/Second Cycle Only, at Every Cycle na Per Month - buwanan ang kwenta ng interes doon).
        ''' </summary>
        Friend Shared Function PeriodsPerYear(item As EmployeeLoanModel) As Integer
            Dim everyCycle = String.Equals(item.Frequency, LoanFrequency.EveryCycle, StringComparison.OrdinalIgnoreCase)
            Return If(everyCycle AndAlso Not IsEveryCyclePerMonth(item), 24, 12)
        End Function

        ''' <summary>
        ''' Ang rate KADA HULOG bilang fraction (hal. 0.0066666...). Per Year = rate / 100 / hulog-kada-taon,
        ''' buong Decimal precision - kaya 8% per year ay eksaktong 266.67 na interes sa 40,000 (hindi 266.68
        ''' na lumalabas kapag 0.6667 ang tinype, dahil 4 decimals lang ang Interest Rate field).
        ''' </summary>
        Friend Shared Function PeriodRate(item As EmployeeLoanModel) As Decimal
            Dim rate = item.InterestRate / 100D
            If String.Equals(item.RatePeriod, LoanRatePeriod.PerYear, StringComparison.OrdinalIgnoreCase) Then
                Return rate / PeriodsPerYear(item)
            End If
            Return rate
        End Function

        Friend Shared Function IsEveryCyclePerMonth(item As EmployeeLoanModel) As Boolean
            Return String.Equals(item.Frequency, LoanFrequency.EveryCycle, StringComparison.OrdinalIgnoreCase) AndAlso
                   String.Equals(item.AmortBasis, LoanAmortBasis.PerMonth, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>
        ''' Bilang ng hulog mula sa annuity formula (n na may decimal).
        ''' BAKIT HINDI LAGING CEILING?
        '''   Naka-round sa sentimo ang amortization (1,809.09), kaya ang n ay halos laging may maliit na sobra
        '''   (24.00002). Ang Ceiling ay gagawa ng ika-25 na hulog na sentimo lang ang laman. Kaya kapag ang
        '''   sobra x amortization ay hindi lalampas sa LastRowTolerance (piso), ibinababa natin sa buong numero
        '''   at ang maliit na tira ay sasama sa huling hulog.
        ''' </summary>
        Private Shared Function TermsFromN(n As Double, amortization As Double) As Integer
            Dim whole = Math.Floor(n)
            If whole >= 1.0 AndAlso (n - whole) * amortization <= CDbl(LastRowTolerance) Then
                Return CInt(whole)
            End If
            Return CInt(Math.Ceiling(n))
        End Function

        ''' <summary>
        ''' Hinahati ang bawat buwanang hulog sa dalawa. Ang unang kaltas = kalahati (naka-round), ang pangalawa
        ''' = ang natira, para eksaktong buo pa rin ang buwan (hal. 1,809.09 = 904.55 + 904.54).
        ''' Hiwalay na hinahati ang principal at interes para tama ang hati sa payroll register.
        ''' </summary>
        Private Shared Function SplitPerMonth(monthly As List(Of EmployeeLoanScheduleModel)) As List(Of EmployeeLoanScheduleModel)
            Dim rows As New List(Of EmployeeLoanScheduleModel)
            For Each m In monthly
                Dim p1 = RoundMoney(m.PrincipalDue / 2D)
                Dim i1 = RoundMoney(m.InterestDue / 2D)
                Dim p2 = m.PrincipalDue - p1
                Dim i2 = m.InterestDue - i1
                rows.Add(New EmployeeLoanScheduleModel With {.PrincipalDue = p1, .InterestDue = i1, .AmountDue = p1 + i1})
                rows.Add(New EmployeeLoanScheduleModel With {.PrincipalDue = p2, .InterestDue = i2, .AmountDue = p2 + i2})
            Next
            For idx = 0 To rows.Count - 1
                rows(idx).InstallmentNo = idx + 1
            Next
            Return rows
        End Function

        Private Shared Function Fail(message As String) As LoanSaveResult
            Return New LoanSaveResult With {.Success = False, .ErrorMessage = message}
        End Function

        ' MidpointRounding.AwayFromZero - sinasadya ito.
        ' Ang default ng .NET ay "banker's rounding" (0.125 -> 0.12),
        ' na hindi inaasahan ng mga taong-pera. Ang 0.125 ay dapat
        ' 0.13 sa payroll.
        Private Shared Function RoundMoney(value As Decimal) As Decimal
            Return Math.Round(value, 2, MidpointRounding.AwayFromZero)
        End Function

        Private Shared Sub ZeroOutComputed(item As EmployeeLoanModel)
            item.InterestAmount = 0D
            item.TotalLoanAmount = 0D
            item.PrincipalAmortization = 0D
            item.InterestAmortization = 0D
            item.TotalAmortization = 0D
        End Sub

    End Class

End Namespace