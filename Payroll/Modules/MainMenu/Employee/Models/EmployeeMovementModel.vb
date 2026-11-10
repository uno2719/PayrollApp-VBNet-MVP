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

        Public Shared Function IsSeparation(movementType As String) As Boolean
            Return String.Equals(movementType, Resign, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(movementType, Terminate, StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(movementType, Retire, StringComparison.OrdinalIgnoreCase)
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
                .DateTerminated = h.DateTerminated
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
            If c.DateResigned.HasValue OrElse c.DateRetired.HasValue OrElse c.DateTerminated.HasValue Then Return True
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

                Case MovementTypes.SetInactive, MovementTypes.Reactivate
                    Return String.Empty

                Case Else
                    Return "Unknown action."
            End Select

            Return String.Empty
        End Function

        Private Function SeparatedMessage() As String
            Return "This employee already has a separation (Resigned, Retired, or Terminated), so this action is not available."
        End Function

        Private Function FormatDate(d As Date) As String
            Return d.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
        End Function

        ''' <summary>Buong pagsusuri ng isang request laban sa kasalukuyang kalagayan.</summary>
        Public Function Validate(req As EmployeeMovementRequest, c As MovementContext, today As Date) As String
            If req Is Nothing Then Return "Nothing to save."

            Dim blocked = UnavailableReason(req.MovementType, c)
            If blocked.Length > 0 Then Return blocked

            If req.MovementType = MovementTypes.SetInactive OrElse req.MovementType = MovementTypes.Reactivate Then
                Return String.Empty
            End If

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

    End Module

End Namespace
