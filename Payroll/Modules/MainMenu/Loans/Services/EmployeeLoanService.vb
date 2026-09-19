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

            Dim i As Double = CDbl(item.InterestRate) / 100.0
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
                item.Terms = CInt(Math.Ceiling(n))

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

                Dim i As Decimal = item.InterestRate / 100D
                Dim balance As Decimal = item.PrincipalAmount
                Dim amort As Decimal = item.TotalAmortization

                For n = 1 To terms

                    Dim interestDue = RoundMoney(balance * i)
                    Dim principalDue As Decimal
                    Dim amountDue As Decimal

                    If n = terms Then
                        ' Huling hulog: linisin ang natitirang balanse.
                        principalDue = balance
                        amountDue = principalDue + interestDue
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

            ' --- EDIT GUARD ---
            If item.Id > 0 Then
                Dim hasPayment = Await _repo.HasAnyPaymentAsync(item.Id)

                If hasPayment Then
                    ' BAKIT BAWAL?
                    '   Kapag may naitala nang kaltas, ang pagbabago ng
                    '   Principal o Terms ay nangangahulugang buburahin
                    '   ang schedule at gagawa ng bago - kasama na ang
                    '   mga rows na may bayad na. Mawawala ang kasaysayan
                    '   at hindi na magkakatugma ang payroll register sa
                    '   loan ledger.
                    Return Fail(
                        "This loan already has posted deductions and can no longer be edited." &
                        Environment.NewLine &
                        "Set it to Inactive and create a new loan entry instead.")
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
            Dim schedule = GenerateSchedule(item)
            Await _repo.ReplaceScheduleAsync(loanId, schedule, userName)

            item.Id = loanId

            Return New LoanSaveResult With {.Success = True, .NewStatus = item.Status}

        End Function

        ' ========================================================
        ' DELETE
        ' ========================================================
        Public Async Function DeleteAsync(loanId As Integer, userName As String) _
            As Task(Of LoanSaveResult) _
            Implements IEmployeeLoanService.DeleteAsync

            If loanId <= 0 Then Return Fail("Please select a loan first.")

            Dim hasPayment = Await _repo.HasAnyPaymentAsync(loanId)

            If hasPayment Then
                ' Parehong dahilan gaya ng edit guard - may kasaysayan
                ' na, hindi na pwedeng ipagkaila.
                Return Fail(
                    "This loan already has posted deductions and cannot be deleted." &
                    Environment.NewLine &
                    "Set it to Inactive instead if you want to stop the deduction.")
            End If

            Await _repo.DeleteAsync(loanId)

            Return New LoanSaveResult With {.Success = True}

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