' ============================================================
' Modules/MainMenu/Employee/Models/EmployeeEmploymentLockRules.vb
' ============================================================
' Pure logic (walang database, walang UI) para sa:
'   1) Ano ang puwedeng baguhin ng Employment tab sa naka-save nang empleyado (lock rules)
'   2) Anong mga pagbabago ang dapat mapunta sa History
'   3) Validation ng Actions > Correct record (Admin lang)
' Iisang batayan ito ng repository (sa loob ng transaction) at ng dialog.
'
' TATLONG ANTAS:
'   A. LAGING NAKA-LOCK sa tab (Actions lang ang nagsusulat): Date Regularization, Date Last Promoted,
'      Date Resigned / Retired / Terminated / End of Contract.
'   B. NAKA-LOCK KAPAG MAY LAMAN NA: Employment Type, Date Joined, Position, Category, Job Class, Branch,
'      Department. Bukas habang gumagawa pa ng bagong empleyado (initial setup) o kapag blangko pa
'      (lumang record na kulang ang datos).
'   C. BUKAS: Work Schedule, Superior, Leave Group, Schedule Group, Timekeeping Control No.
'      Ang pagbabago sa Superior / Leave Group / Schedule Group ay nagsusulat ng "Details changed" sa History.
' ============================================================
Namespace Employee.Models

    ''' <summary>Hiling ng Actions > Correct record (pag-ayos ng maling nailagay na datos). Admin lang.</summary>
    Public Class EmploymentCorrectionRequest
        Public Property RecordId As Integer
        Public Property EmployeeStatus As String          ' = Employment Type sa screen
        Public Property DateJoined As Date?
        Public Property DateRegularization As Date?
        Public Property DateLastPromoted As Date?
        Public Property DateResigned As Date?
        Public Property DateRetired As Date?
        Public Property DateTerminated As Date?
        Public Property DateEndOfContract As Date?
        Public Property PositionId As Integer?
        Public Property CategoryId As Integer?
        Public Property JobClassId As Integer?
        Public Property BranchId As Integer?
        Public Property DepartmentId As Integer?
        Public Property Remarks As String
        Public Property CreatedBy As String
    End Class

    ''' <summary>Isang field na nagbago (isang row sa History).</summary>
    Public Class EmploymentFieldChange
        Public Property Label As String
        Public Property FromText As String
        Public Property ToText As String
    End Class

    Public Module EmployeeEmploymentLockRules

        ''' <summary>Hangganan ng FromValue / ToValue sa History (isang field bawat row kaya sapat ito).</summary>
        Public Const MaxHistoryText As Integer = 100

        ' =============================================
        ' MERGE: Employment tab save
        ' =============================================

        ''' <summary>
        ''' Binubuo ang aktwal na isusulat kapag nag-Save sa Employment tab.
        ''' cur = kasalukuyang row sa database (Nothing kung bago pa); incoming = laman ng screen.
        ''' Ang mga column na hindi hawak ng tab (DateLastIncremented, DateLastTransferred, HolidayGroupId...)
        ''' ay laging galing sa cur, kaya hindi na sila nabubura.
        ''' </summary>
        Public Function MergeForTabSave(cur As EmployeeEmploymentModel,
                                        incoming As EmployeeEmploymentModel,
                                        initialSetup As Boolean) As EmployeeEmploymentModel

            Dim m As EmployeeEmploymentModel = If(cur Is Nothing, New EmployeeEmploymentModel(), CloneEmployment(cur))
            m.RecordId = incoming.RecordId
            m.CreatedBy = incoming.CreatedBy
            m.UpdatedBy = incoming.UpdatedBy

            ' C. Bukas
            m.EmploymentType = incoming.EmploymentType
            m.SuperiorRecordId = incoming.SuperiorRecordId
            m.LeaveGroupId = incoming.LeaveGroupId
            m.ScheduleGroupId = incoming.ScheduleGroupId
            m.TimekeepingControlNo = incoming.TimekeepingControlNo

            ' B. Naka-lock kapag may laman na (bukas sa bagong record / initial setup / blangko pa)
            Dim open As Boolean = (cur Is Nothing) OrElse initialSetup

            If open OrElse Not EmployeeMovementRules.IsKnownEmploymentType(cur.EmployeeStatus) Then
                m.EmployeeStatus = incoming.EmployeeStatus
            End If
            If open OrElse Not cur.DateJoined.HasValue Then m.DateJoined = incoming.DateJoined
            If open OrElse Not cur.PositionId.HasValue Then m.PositionId = incoming.PositionId
            If open OrElse Not cur.CategoryId.HasValue Then m.CategoryId = incoming.CategoryId
            If open OrElse Not cur.JobClassId.HasValue Then m.JobClassId = incoming.JobClassId
            If open OrElse Not cur.BranchId.HasValue Then m.BranchId = incoming.BranchId
            If open OrElse Not cur.DepartmentId.HasValue Then m.DepartmentId = incoming.DepartmentId

            ' A. Laging naka-lock: nananatili ang galing sa cur (Nothing sa bagong row).
            Return m
        End Function

        Public Function CloneEmployment(src As EmployeeEmploymentModel) As EmployeeEmploymentModel
            Return New EmployeeEmploymentModel With {
                .EmploymentId = src.EmploymentId,
                .RecordId = src.RecordId,
                .EmployeeStatus = src.EmployeeStatus,
                .EmploymentType = src.EmploymentType,
                .BranchId = src.BranchId,
                .DepartmentId = src.DepartmentId,
                .PositionId = src.PositionId,
                .CategoryId = src.CategoryId,
                .JobClassId = src.JobClassId,
                .SuperiorRecordId = src.SuperiorRecordId,
                .LeaveGroupId = src.LeaveGroupId,
                .HolidayGroupId = src.HolidayGroupId,
                .ScheduleGroupId = src.ScheduleGroupId,
                .BranchName = src.BranchName,
                .DepartmentName = src.DepartmentName,
                .PositionName = src.PositionName,
                .CategoryName = src.CategoryName,
                .JobClassName = src.JobClassName,
                .SuperiorName = src.SuperiorName,
                .LeaveGroupName = src.LeaveGroupName,
                .HolidayGroupName = src.HolidayGroupName,
                .ScheduleGroupName = src.ScheduleGroupName,
                .DateJoined = src.DateJoined,
                .DateRegularization = src.DateRegularization,
                .DateLastPromoted = src.DateLastPromoted,
                .DateLastIncremented = src.DateLastIncremented,
                .DateLastTransferred = src.DateLastTransferred,
                .DateResigned = src.DateResigned,
                .DateRetired = src.DateRetired,
                .DateTerminated = src.DateTerminated,
                .DateEndOfContract = src.DateEndOfContract,
                .ContractEndDate = src.ContractEndDate,
                .DateProbationStart = src.DateProbationStart,
                .TimekeepingControlNo = src.TimekeepingControlNo,
                .CreatedAt = src.CreatedAt,
                .CreatedBy = src.CreatedBy,
                .UpdatedAt = src.UpdatedAt,
                .UpdatedBy = src.UpdatedBy
            }
        End Function

        ' =============================================
        ' HISTORY: ano ang nagbago
        ' =============================================

        ''' <summary>
        ''' Paghahambing ng dalawang bersyon ng Employment row (pareho dapat galing sa select na may mga pangalan).
        ''' Isang item bawat field na nagbago; maikli ang bawat text kaya kasya sa History.
        ''' </summary>
        Public Function DescribeChanges(before As EmployeeEmploymentModel,
                                        after As EmployeeEmploymentModel) As List(Of EmploymentFieldChange)

            Dim list As New List(Of EmploymentFieldChange)()
            If before Is Nothing OrElse after Is Nothing Then Return list

            AddText(list, "Employment Type", before.EmployeeStatus, after.EmployeeStatus)
            AddDate(list, "Date Joined", before.DateJoined, after.DateJoined)
            AddDate(list, "Date Regularization", before.DateRegularization, after.DateRegularization)
            AddDate(list, "Date Last Promoted", before.DateLastPromoted, after.DateLastPromoted)
            AddLookup(list, "Position", before.PositionId, after.PositionId, before.PositionName, after.PositionName)
            AddLookup(list, "Category Code", before.CategoryId, after.CategoryId, before.CategoryName, after.CategoryName)
            AddLookup(list, "Job Class", before.JobClassId, after.JobClassId, before.JobClassName, after.JobClassName)
            AddLookup(list, "Branch", before.BranchId, after.BranchId, before.BranchName, after.BranchName)
            AddLookup(list, "Department", before.DepartmentId, after.DepartmentId, before.DepartmentName, after.DepartmentName)
            AddLookup(list, "Superior", before.SuperiorRecordId, after.SuperiorRecordId, before.SuperiorName, after.SuperiorName)
            AddLookup(list, "Leave Group", before.LeaveGroupId, after.LeaveGroupId, before.LeaveGroupName, after.LeaveGroupName)
            AddLookup(list, "Schedule Group", before.ScheduleGroupId, after.ScheduleGroupId, before.ScheduleGroupName, after.ScheduleGroupName)
            AddDate(list, "Date Resigned", before.DateResigned, after.DateResigned)
            AddDate(list, "Date Retired", before.DateRetired, after.DateRetired)
            AddDate(list, "Date Terminated", before.DateTerminated, after.DateTerminated)
            AddDate(list, "Date End of Contract", before.DateEndOfContract, after.DateEndOfContract)
            AddDate(list, "Contract End", before.ContractEndDate, after.ContractEndDate)
            Return list
        End Function

        Private Sub AddText(list As List(Of EmploymentFieldChange), label As String, a As String, b As String)
            Dim x As String = If(a, String.Empty).Trim()
            Dim y As String = If(b, String.Empty).Trim()
            If String.Equals(x, y, StringComparison.OrdinalIgnoreCase) Then Return
            list.Add(Make(label, x, y))
        End Sub

        Private Sub AddDate(list As List(Of EmploymentFieldChange), label As String, a As Date?, b As Date?)
            If SameDate(a, b) Then Return
            list.Add(Make(label, FmtDate(a), FmtDate(b)))
        End Sub

        Private Sub AddLookup(list As List(Of EmploymentFieldChange), label As String,
                              idA As Integer?, idB As Integer?, nameA As String, nameB As String)
            If SameId(idA, idB) Then Return
            list.Add(Make(label, LookupText(idA, nameA), LookupText(idB, nameB)))
        End Sub

        Private Function LookupText(id As Integer?, name As String) As String
            If Not id.HasValue Then Return String.Empty
            If Not String.IsNullOrWhiteSpace(name) Then Return name.Trim()
            Return "#" & id.Value.ToString(Globalization.CultureInfo.InvariantCulture)
        End Function

        Private Function Make(label As String, fromValue As String, toValue As String) As EmploymentFieldChange
            Return New EmploymentFieldChange With {
                .Label = label,
                .FromText = Fit(label & ": " & OrNoneText(fromValue)),
                .ToText = Fit(label & ": " & OrNoneText(toValue))
            }
        End Function

        Private Function OrNoneText(value As String) As String
            Return If(String.IsNullOrWhiteSpace(value), "(none)", value.Trim())
        End Function

        Private Function Fit(text As String) As String
            If text.Length <= MaxHistoryText Then Return text
            Return text.Substring(0, MaxHistoryText - 1) & "…"
        End Function

        Private Function FmtDate(value As Date?) As String
            If Not value.HasValue Then Return String.Empty
            Return value.Value.ToString("MM/dd/yyyy", Globalization.CultureInfo.InvariantCulture)
        End Function

        Private Function SameDate(a As Date?, b As Date?) As Boolean
            If Not a.HasValue AndAlso Not b.HasValue Then Return True
            If a.HasValue AndAlso b.HasValue Then Return a.Value.Date = b.Value.Date
            Return False
        End Function

        Private Function SameId(a As Integer?, b As Integer?) As Boolean
            If Not a.HasValue AndAlso Not b.HasValue Then Return True
            If a.HasValue AndAlso b.HasValue Then Return a.Value = b.Value
            Return False
        End Function

        ' =============================================
        ' CORRECT RECORD (Admin lang)
        ' =============================================

        ''' <summary>
        ''' Mensahe ng problema ("" kung valid). Hindi ito pagpapalit ng Actions: para lang sa maling nailagay.
        ''' </summary>
        Public Function ValidateCorrection(req As EmploymentCorrectionRequest,
                                           cur As EmployeeEmploymentModel,
                                           today As Date) As String

            If req Is Nothing OrElse cur Is Nothing Then Return "This employee has no Employment record yet."

            If String.IsNullOrWhiteSpace(req.Remarks) Then
                Return "Remarks is required: write what was wrong and why it is being corrected."
            End If
            If req.Remarks.Trim().Length > EmployeeMovementRules.MaxRemarksLength Then
                Return $"Remarks is too long (maximum {EmployeeMovementRules.MaxRemarksLength} characters)."
            End If

            If Not EmployeeMovementRules.IsKnownEmploymentType(req.EmployeeStatus) Then
                Return "Employment Type is required. Choose one of: " &
                       String.Join(", ", EmployeeMovementRules.EmploymentTypes) & "."
            End If

            If Not req.DateJoined.HasValue Then Return "Date Joined is required."
            Dim joined As Date = req.DateJoined.Value.Date
            If joined > today.Date Then Return "Date Joined can't be in the future."

            Dim problem As String = RangeProblem("Date Regularization", req.DateRegularization, joined, today, True)
            If problem.Length > 0 Then Return problem

            ' Date Last Promoted: sapat na hindi ito sa hinaharap (puwedeng mas maaga sa Date Joined kapag na-rehire).
            If req.DateLastPromoted.HasValue AndAlso req.DateLastPromoted.Value.Date > today.Date Then
                Return "Date Last Promoted can't be in the future."
            End If

            ' Ang petsa ng pag-alis: itatama lang ang laman. Hindi madadagdag o mabubura dito (Resign / Rehire ang daan).
            problem = SeparationProblem("Date Resigned", cur.DateResigned, req.DateResigned, joined, today, "Resign")
            If problem.Length > 0 Then Return problem
            problem = SeparationProblem("Date Retired", cur.DateRetired, req.DateRetired, joined, today, "Retire")
            If problem.Length > 0 Then Return problem
            problem = SeparationProblem("Date Terminated", cur.DateTerminated, req.DateTerminated, joined, today, "Terminate")
            If problem.Length > 0 Then Return problem
            problem = SeparationProblem("Date End of Contract", cur.DateEndOfContract, req.DateEndOfContract, joined, today, "End of Contract")
            If problem.Length > 0 Then Return problem

            If Not req.PositionId.HasValue Then Return "Position is required."
            If Not req.CategoryId.HasValue Then Return "Category Code is required."
            If Not req.JobClassId.HasValue Then Return "Job Class is required."
            If Not req.BranchId.HasValue Then Return "Branch is required."
            If Not req.DepartmentId.HasValue Then Return "Department is required."

            If Not HasAnyChange(cur, req) Then Return "Nothing was changed."
            Return String.Empty
        End Function

        Private Function RangeProblem(label As String, value As Date?, joined As Date, today As Date,
                                      mustNotPrecedeJoined As Boolean) As String
            If Not value.HasValue Then Return String.Empty
            If value.Value.Date > today.Date Then Return $"{label} can't be in the future."
            If mustNotPrecedeJoined AndAlso value.Value.Date < joined Then
                Return $"{label} can't be earlier than Date Joined."
            End If
            Return String.Empty
        End Function

        Private Function SeparationProblem(label As String, current As Date?, proposed As Date?,
                                           joined As Date, today As Date, actionName As String) As String
            If current.HasValue <> proposed.HasValue Then
                Return $"{label} can't be added or removed here. Use Actions > {actionName} (or Rehire) instead."
            End If
            Return RangeProblem(label, proposed, joined, today, True)
        End Function

        ''' <summary>May nagbago ba talaga kumpara sa kasalukuyan?</summary>
        Public Function HasAnyChange(cur As EmployeeEmploymentModel, req As EmploymentCorrectionRequest) As Boolean
            If Not String.Equals(If(cur.EmployeeStatus, String.Empty).Trim(), If(req.EmployeeStatus, String.Empty).Trim(),
                                 StringComparison.OrdinalIgnoreCase) Then Return True
            If Not SameDate(cur.DateJoined, req.DateJoined) Then Return True
            If Not SameDate(cur.DateRegularization, req.DateRegularization) Then Return True
            If Not SameDate(cur.DateLastPromoted, req.DateLastPromoted) Then Return True
            If Not SameDate(cur.DateResigned, req.DateResigned) Then Return True
            If Not SameDate(cur.DateRetired, req.DateRetired) Then Return True
            If Not SameDate(cur.DateTerminated, req.DateTerminated) Then Return True
            If Not SameDate(cur.DateEndOfContract, req.DateEndOfContract) Then Return True
            If Not SameId(cur.PositionId, req.PositionId) Then Return True
            If Not SameId(cur.CategoryId, req.CategoryId) Then Return True
            If Not SameId(cur.JobClassId, req.JobClassId) Then Return True
            If Not SameId(cur.BranchId, req.BranchId) Then Return True
            If Not SameId(cur.DepartmentId, req.DepartmentId) Then Return True
            Return False
        End Function

    End Module
End Namespace
