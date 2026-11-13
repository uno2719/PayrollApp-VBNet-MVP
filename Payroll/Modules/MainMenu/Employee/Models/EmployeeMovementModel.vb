' File: Modules/MainMenu/Employee/Models/EmployeeMovementModel.vb
' Phase 2: movement log (tblEmployeeMovement), ang request ng Actions dialog, at ang PURONG LOGIC
' (walang database, walang control) kung kailan available at valid ang isang action.
Imports System.Globalization

Namespace Employee.Models

    ''' <summary>Mga uri ng movement. Ito ang halagang naka-store sa tblEmployeeMovement.MovementType.</summary>
    Public NotInheritable Class MovementTypes

        Private Sub New()
        End Sub

        Public Const Regularize As String = "Regularize"
        Public Const Promote As String = "Promote"
        Public Const Transfer As String = "Transfer"
        Public Const Resign As String = "Resign"
        Public Const Terminate As String = "Terminate"
        Public Const Retire As String = "Retire"
        Public Const SetInactive As String = "Set inactive"
        Public Const Reactivate As String = "Reactivate"
        Public Const Rehire As String = "Rehire"
        Public Const EndOfContract As String = "End of Contract"
        Public Const SalaryAdjustment As String = "Salary adjustment"

        Public Shared Function IsSeparation(movementType As String) As Boolean
            Return String.Equals(movementType, Resign, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(movementType, Terminate, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(movementType, Retire, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(movementType, EndOfContract, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>Ang mga movement na may halaga ng sahod (itinatago sa History kung walang Salary permission).</summary>
        Public Shared Function IsSalary(movementType As String) As Boolean
            Return String.Equals(movementType, SalaryAdjustment, StringComparison.OrdinalIgnoreCase)
        End Function

        ''' <summary>Pangalan ng action sa History (past tense).</summary>
        Public Shared Function HistoryLabel(movementType As String) As String
            Select Case movementType
                Case Regularize : Return "Regularized"
                Case Promote : Return "Promoted"
                Case Transfer : Return "Transferred"
                Case Resign : Return "Resigned"
                Case Terminate : Return "Terminated"
                Case Retire : Return "Retired"
                Case SetInactive : Return "Set inactive"
                Case Reactivate : Return "Reactivated"
                Case Rehire : Return "Rehired"
                Case EndOfContract : Return "Contract ended"
                Case SalaryAdjustment : Return "Salary adjusted"
                Case Else : Return If(movementType, String.Empty)
            End Select
        End Function

    End Class

    ''' <summary>Isang row ng tblEmployeeMovement (History).</summary>
    Public Class EmployeeMovementModel
        Public Property MovementId As Integer
        Public Property RecordId As Integer
        Public Property MovementType As String
        Public Property EffectiveDate As Date
        Public Property FromValue As String
        Public Property ToValue As String
        Public Property Remarks As String
        Public Property CreatedAt As DateTime
        Public Property CreatedBy As String
    End Class

    ''' <summary>
    ''' Ang hinihiling ng Actions dialog. Para sa Promote/Transfer, ang NewXxxId ay ang BUONG bagong halaga
    ''' (naka-prefill ang dialog ng kasalukuyan), kaya ang hindi nabago ay pareho lang ng kasalukuyan.
    ''' </summary>
    Public Class EmployeeMovementRequest
        Public Property RecordId As Integer
        Public Property MovementType As String
        Public Property EffectiveDate As Date
        Public Property Remarks As String

        ' Promote
        Public Property NewPositionId As Integer?
        Public Property NewCategoryId As Integer?
        Public Property NewJobClassId As Integer?

        ' Transfer
        Public Property NewBranchId As Integer?
        Public Property NewDepartmentId As Integer?

        ' Rehire: ang Employment Type ng bagong panahon ng serbisyo (pinipili ng HR; walang default)
        Public Property NewEmploymentType As String

        ' Salary adjustment: ang bagong halaga ng INPUT ng pay basis ng empleyado (Basic Salary para sa Basic cycle,
        ' Daily Rate para sa Daily Rate cycle). Ang Daily/Hourly ay kinukuwenta ng repository sa loob ng transaction.
        Public Property NewBasicSalary As Decimal?
        Public Property NewDailyRate As Decimal?

        ''' <summary>Itinatakda ng Service (AppSession.CurrentUser), hindi ng dialog.</summary>
        Public Property CreatedBy As String
    End Class

    ''' <summary>Kasalukuyang kalagayan ng empleyado na kailangan ng rules. Galing sa header o sa employment row.</summary>
    Public Class MovementContext
        Public Property IsActive As Boolean

        ''' <summary>Ang "Employment Type" (nakalagay pa rin sa EmployeeStatus column).</summary>
        Public Property EmploymentType As String

        Public Property DateJoined As Date?
        Public Property DateResigned As Date?
        Public Property DateRetired As Date?
        Public Property DateTerminated As Date?
        Public Property DateEndOfContract As Date?

        ' Para sa Salary adjustment lang (galing sa Earnings): meron na bang rate, at alin ang input.
        Public Property HasPayRate As Boolean
        Public Property UsesDailyRate As Boolean
        Public Property CurrentBasicSalary As Decimal?
        Public Property CurrentDailyRate As Decimal?

        Public Property PositionId As Integer?
        Public Property CategoryId As Integer?
        Public Property JobClassId As Integer?
        Public Property BranchId As Integer?
        Public Property DepartmentId As Integer?

        ''' <summary>Pang-availability lang (menu): walang FK ids.</summary>
        Public Shared Function FromHeader(h As EmployeeHeaderModel) As MovementContext
            Return New MovementContext With {
                .IsActive = h.IsActive,
                .EmploymentType = h.EmployeeStatus,
                .DateJoined = h.DateJoined,
                .DateResigned = h.DateResigned,
                .DateRetired = h.DateRetired,
                .DateTerminated = h.DateTerminated,
                .DateEndOfContract = h.DateEndOfContract,
                .HasPayRate = h.HasPayRate
            }
        End Function

        ''' <summary>Buo (may FK ids) - para sa validation ng dialog at ng repository.</summary>
        Public Shared Function FromEmployment(emp As EmployeeEmploymentModel, isActive As Boolean) As MovementContext
            Return New MovementContext With {
                .IsActive = isActive,
                .EmploymentType = emp.EmployeeStatus,
                .DateJoined = emp.DateJoined,
                .DateResigned = emp.DateResigned,
                .DateRetired = emp.DateRetired,
                .DateTerminated = emp.DateTerminated,
                .DateEndOfContract = emp.DateEndOfContract,
                .PositionId = emp.PositionId,
                .CategoryId = emp.CategoryId,
                .JobClassId = emp.JobClassId,
                .BranchId = emp.BranchId,
                .DepartmentId = emp.DepartmentId
            }
        End Function
    End Class

    ''' <summary>
    ''' PURONG LOGIC: iisang batayan ng menu (enabled/disabled), ng dialog, at ng repository (sa loob ng transaction).
    ''' Empty string = valid; kung hindi, ang mensahe para sa user.
    ''' </summary>
    Public Module EmployeeMovementRules

        Public Const MaxRemarksLength As Integer = 500

        ''' <summary>Pinakamalaking tinatanggap na Basic Salary / Daily Rate sa isang adjustment (proteksyon sa typo).</summary>
        Public Const MaxSalaryAmount As Decimal = 9999999.99D

        ''' <summary>Mga halagang tinatanggap bilang Employment Type (kapareho ng combo sa Employment tab).</summary>
        Public ReadOnly EmploymentTypes As String() =
            {"Probationary", "Regular", "Contractual", "Project-based", "Casual", "Seasonal"}

        Private ReadOnly SeparationStatuses As String() = {"Resigned", "Retired", "Terminated"}

        Public Function SameId(a As Integer?, b As Integer?) As Boolean
            If a.HasValue <> b.HasValue Then Return False
            Return Not a.HasValue OrElse a.Value = b.Value
        End Function

        Private Function Clean(value As String) As String
            Return If(value, String.Empty).Trim()
        End Function

        ''' <summary>May petsa ng pag-alis (Resigned/Retired/Terminated), o lumang status na ganoon ang nakasulat.</summary>
        Public Function IsSeparated(c As MovementContext) As Boolean
            If c.DateResigned.HasValue OrElse c.DateRetired.HasValue OrElse c.DateTerminated.HasValue OrElse
               c.DateEndOfContract.HasValue Then Return True
            Dim t As String = Clean(c.EmploymentType)
            Return SeparationStatuses.Any(Function(s) s.Equals(t, StringComparison.OrdinalIgnoreCase))
        End Function

        Public Function IsRegularType(employmentType As String) As Boolean
            Dim t As String = Clean(employmentType)
            Return t.Equals("Regular", StringComparison.OrdinalIgnoreCase) OrElse
                   t.Equals("Permanent", StringComparison.OrdinalIgnoreCase)
        End Function

        Public Function IsKnownEmploymentType(employmentType As String) As Boolean
            Dim t As String = Clean(employmentType)
            Return EmploymentTypes.Any(Function(k) k.Equals(t, StringComparison.OrdinalIgnoreCase))
        End Function

        ''' <summary>Pinakahuling petsa ng pag-alis (Resigned/Retired/Terminated). Nothing kung walang petsa.</summary>
        Public Function LastSeparationDate(c As MovementContext) As Date?
            Dim result As Date? = Nothing
            For Each d In {c.DateResigned, c.DateRetired, c.DateTerminated, c.DateEndOfContract}
                If d.HasValue AndAlso (Not result.HasValue OrElse d.Value.Date > result.Value.Date) Then result = d.Value.Date
            Next
            Return result
        End Function

        ''' <summary>"Resigned" / "Retired" / "Terminated" ng pinakahuling pag-alis; blangko kung hindi matukoy.</summary>
        Public Function LastSeparationKind(c As MovementContext) As String
            Dim last = LastSeparationDate(c)
            If last.HasValue Then
                If c.DateTerminated.HasValue AndAlso c.DateTerminated.Value.Date = last.Value Then Return "Terminated"
                If c.DateResigned.HasValue AndAlso c.DateResigned.Value.Date = last.Value Then Return "Resigned"
                If c.DateEndOfContract.HasValue AndAlso c.DateEndOfContract.Value.Date = last.Value Then Return "End of Contract"
                Return "Retired"
            End If

            ' Lumang status na nakasulat sa EmployeeStatus (walang petsa)
            Dim t As String = Clean(c.EmploymentType)
            Return SeparationStatuses.FirstOrDefault(Function(s) s.Equals(t, StringComparison.OrdinalIgnoreCase)) & String.Empty
        End Function

        ''' <summary>Ang eksaktong pagkakasulat (casing) ng Employment Type mula sa listahan; blangko kung hindi kilala.</summary>
        Public Function CanonicalEmploymentType(employmentType As String) As String
            Dim t As String = Clean(employmentType)
            Return EmploymentTypes.FirstOrDefault(Function(k) k.Equals(t, StringComparison.OrdinalIgnoreCase)) & String.Empty
        End Function

        ''' <summary>
        ''' Hindi puwede ang Reactivate sa may separation: ang petsa ng pag-alis ay mananatili at ang empleyado ay
        ''' mananatiling hindi eligible sa payroll. Ang daan ay Rehire. Empty = puwede.
        ''' </summary>
        Public Function ReactivateBlockedReason(c As MovementContext) As String
            If IsSeparated(c) Then
                Return "This employee has a separation (Resigned, Retired, Terminated, or End of Contract). Use Actions > Rehire instead of Reactivate."
            End If
            Return String.Empty
        End Function

        ''' <summary>Dapat bang naka-enable ang action sa menu para sa empleyadong ito.</summary>
        Public Function IsAvailable(movementType As String, c As MovementContext) As Boolean
            Return UnavailableReason(movementType, c).Length = 0
        End Function

        Public Function UnavailableReason(movementType As String, c As MovementContext) As String
            Select Case movementType
                Case MovementTypes.Regularize
                    If IsSeparated(c) Then Return SeparatedMessage()
                    If Not c.IsActive Then Return "This employee is Inactive. Reactivate the employee first."
                    If IsRegularType(c.EmploymentType) Then Return "This employee is already Regular."

                Case MovementTypes.Promote, MovementTypes.Transfer
                    If IsSeparated(c) Then Return SeparatedMessage()
                    If Not c.IsActive Then Return "This employee is Inactive. Reactivate the employee first."

                Case MovementTypes.Resign, MovementTypes.Terminate, MovementTypes.Retire
                    If IsSeparated(c) Then Return SeparatedMessage()

                Case MovementTypes.EndOfContract
                    If IsSeparated(c) Then Return SeparatedMessage()
                    ' HULA (hindi pa kumpirmado ng HR): hindi para sa Regular/Permanent. Palitan dito kung iba ang patakaran.
                    If IsRegularType(c.EmploymentType) Then
                        Return "End of Contract is not for Regular employees. Use Resign, Terminate, or Retire instead."
                    End If

                Case MovementTypes.SalaryAdjustment
                    ' Ang UNANG Basic Salary / Daily Rate ay itinatakda sa Earnings tab; ang Actions na ito ay para sa pagbabago.
                    If Not c.HasPayRate Then
                        Return "This employee has no Basic Salary or Daily Rate yet. Set the starting rate in the Earnings tab first."
                    End If

                Case MovementTypes.Rehire
                    If Not IsSeparated(c) Then
                        Return "Only an employee who has resigned, retired, been terminated, or whose contract ended can be rehired."
                    End If

                Case MovementTypes.SetInactive, MovementTypes.Reactivate
                    Return String.Empty

                Case Else
                    Return "Unknown action."
            End Select

            Return String.Empty
        End Function

        Private Function SeparatedMessage() As String
            Return "This employee already has a separation (Resigned, Retired, Terminated, or End of Contract), so this action is not available. " &
                   "If the employee is returning, use Actions > Rehire."
        End Function

        Private Function FormatDate(d As Date) As String
            Return d.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
        End Function

        ' Rehire: ang petsa ay dapat MAS HULI sa huling pag-alis (hindi ang Date Joined ang batayan, dahil ito
        ' mismo ang papalitan), hindi sa hinaharap, at kailangan ng Employment Type.
        Private Function ValidateRehire(req As EmployeeMovementRequest, c As MovementContext, today As Date) As String
            If req.EffectiveDate.Date > today.Date Then
                Return "The rehire date can't be in the future."
            End If

            Dim lastSep As Date? = LastSeparationDate(c)
            If lastSep.HasValue Then
                If req.EffectiveDate.Date <= lastSep.Value.Date Then
                    Return $"The rehire date must be after the last separation date ({FormatDate(lastSep.Value)})."
                End If
            ElseIf c.DateJoined.HasValue AndAlso req.EffectiveDate.Date < c.DateJoined.Value.Date Then
                Return $"The rehire date can't be earlier than Date Joined ({FormatDate(c.DateJoined.Value)})."
            End If

            If CanonicalEmploymentType(req.NewEmploymentType).Length = 0 Then
                Return "Choose the Employment Type for the rehired employee."
            End If

            Dim remarks As String = Clean(req.Remarks)
            If remarks.Length > MaxRemarksLength Then
                Return $"Remarks is too long (maximum {MaxRemarksLength} characters)."
            End If
            If LastSeparationKind(c) = "Terminated" AndAlso remarks.Length = 0 Then
                Return "Remarks is required when rehiring a terminated employee (state the reason or approval)."
            End If

            Return String.Empty
        End Function

        ''' <summary>Buong pagsusuri ng isang request laban sa kasalukuyang kalagayan.</summary>
        Public Function Validate(req As EmployeeMovementRequest, c As MovementContext, today As Date) As String
            If req Is Nothing Then Return "Nothing to save."

            Dim blocked = UnavailableReason(req.MovementType, c)
            If blocked.Length > 0 Then Return blocked

            If req.MovementType = MovementTypes.SetInactive OrElse req.MovementType = MovementTypes.Reactivate Then
                Return String.Empty
            End If

            If req.MovementType = MovementTypes.Rehire Then Return ValidateRehire(req, c, today)

            If Not c.DateJoined.HasValue Then
                Return "Date Joined is missing. Set it in the Employment tab first."
            End If
            If req.EffectiveDate.Date > today.Date Then
                Return "The effective date can't be in the future."
            End If
            If req.EffectiveDate.Date < c.DateJoined.Value.Date Then
                Return $"The effective date can't be earlier than Date Joined ({FormatDate(c.DateJoined.Value)})."
            End If

            Dim remarks As String = Clean(req.Remarks)
            If remarks.Length > MaxRemarksLength Then
                Return $"Remarks is too long (maximum {MaxRemarksLength} characters)."
            End If

            Select Case req.MovementType
                Case MovementTypes.Resign, MovementTypes.Terminate
                    If remarks.Length = 0 Then Return "Remarks is required for this action (state the reason)."

                ' End of Contract: optional ang Remarks (walang dagdag na patakaran maliban sa petsa sa itaas).

                Case MovementTypes.SalaryAdjustment
                    If remarks.Length = 0 Then Return "Remarks is required for a salary adjustment (state the reason, e.g. increase, wage order, correction)."

                    Dim current As Decimal? = If(c.UsesDailyRate, c.CurrentDailyRate, c.CurrentBasicSalary)
                    Dim proposed As Decimal? = If(c.UsesDailyRate, req.NewDailyRate, req.NewBasicSalary)
                    Dim label As String = If(c.UsesDailyRate, "Daily Rate", "Basic Salary")

                    If Not proposed.HasValue OrElse proposed.Value <= 0D Then
                        Return $"The new {label} must be greater than zero."
                    End If
                    If proposed.Value > MaxSalaryAmount Then
                        Return $"The new {label} is too large. Please check the amount."
                    End If
                    If current.HasValue AndAlso Math.Round(current.Value, 2) = Math.Round(proposed.Value, 2) Then
                        Return $"Nothing to adjust: the new {label} is the same as the current one."
                    End If

                Case MovementTypes.Promote
                    If Not req.NewPositionId.HasValue Then Return "Position is required."
                    If c.CategoryId.HasValue AndAlso Not req.NewCategoryId.HasValue Then Return "Category can't be blank."
                    If c.JobClassId.HasValue AndAlso Not req.NewJobClassId.HasValue Then Return "Job Class can't be blank."
                    If SameId(c.PositionId, req.NewPositionId) AndAlso
                       SameId(c.CategoryId, req.NewCategoryId) AndAlso
                       SameId(c.JobClassId, req.NewJobClassId) Then
                        Return "Nothing to promote: choose a different Position, Category, or Job Class."
                    End If

                Case MovementTypes.Transfer
                    If c.BranchId.HasValue AndAlso Not req.NewBranchId.HasValue Then Return "Branch can't be blank."
                    If c.DepartmentId.HasValue AndAlso Not req.NewDepartmentId.HasValue Then Return "Department can't be blank."
                    If Not req.NewBranchId.HasValue AndAlso Not req.NewDepartmentId.HasValue Then
                        Return "Choose a Branch or a Department."
                    End If
                    If SameId(c.BranchId, req.NewBranchId) AndAlso SameId(c.DepartmentId, req.NewDepartmentId) Then
                        Return "Nothing to transfer: choose a different Branch or Department."
                    End If
            End Select

            Return String.Empty
        End Function

        ''' <summary>
        ''' Ang mga rate pagkatapos ng adjustment, KAPAREHONG pormula ng Earnings tab:
        '''   Basic cycle: Daily = Basic x 12 / DaysInYear, Hourly = Daily / WorkHourPer
        '''   Daily cycle: Hourly = Daily / WorkHourPer (ang Basic ay hindi ginagamit)
        ''' Kapag kulang ang DaysInYear / WorkHourPer, mananatili ang dating Daily / Hourly.
        ''' </summary>
        Public Function ComputeAdjustedRates(usesDailyRate As Boolean,
                                             newInput As Decimal,
                                             daysInYear As Integer?,
                                             workHourPer As Decimal?,
                                             currentBasic As Decimal?,
                                             currentDaily As Decimal?,
                                             currentHourly As Decimal?) As SalaryRates
            Dim r As New SalaryRates()

            If usesDailyRate Then
                r.BasicSalary = currentBasic
                r.DailyRate = Math.Round(newInput, 2)
                r.HourlyRate = currentHourly
                If workHourPer.HasValue AndAlso workHourPer.Value > 0D Then
                    r.HourlyRate = Math.Round(r.DailyRate.Value / workHourPer.Value, 2)
                End If
            Else
                r.BasicSalary = Math.Round(newInput, 2)
                r.DailyRate = currentDaily
                r.HourlyRate = currentHourly
                If daysInYear.HasValue AndAlso daysInYear.Value > 0 Then
                    r.DailyRate = Math.Round(r.BasicSalary.Value * 12D / daysInYear.Value, 2)
                    If workHourPer.HasValue AndAlso workHourPer.Value > 0D Then
                        r.HourlyRate = Math.Round(r.DailyRate.Value / workHourPer.Value, 2)
                    End If
                End If
            End If

            Return r
        End Function

        ''' <summary>Hal. "Basic Salary: 20,000.00" (para sa From/To ng History).</summary>
        Public Function SalaryText(usesDailyRate As Boolean, amount As Decimal?) As String
            Dim label As String = If(usesDailyRate, "Daily Rate", "Basic Salary")
            If Not amount.HasValue Then Return label & ": (none)"
            Return label & ": " & amount.Value.ToString("N2", CultureInfo.InvariantCulture)
        End Function

    End Module

    ''' <summary>Resulta ng ComputeAdjustedRates.</summary>
    Public Class SalaryRates
        Public Property BasicSalary As Decimal?
        Public Property DailyRate As Decimal?
        Public Property HourlyRate As Decimal?
    End Class

End Namespace
