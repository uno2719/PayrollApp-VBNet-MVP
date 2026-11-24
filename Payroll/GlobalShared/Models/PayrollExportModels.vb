' File: GlobalShared/Models/PayrollExportModels.vb
Imports System.Globalization

Namespace GlobalShared.Models

    ''' <summary>Mga listahan ng payout. Isang empleyado = isang listahan lang (galing sa Pay By + Bank sa Employee > Earnings).</summary>
    Public NotInheritable Class PayoutGroup
        Public Const Bdo As String = "BDO"
        Public Const OtherBank As String = "OTHERBANK"
        Public Const Cash As String = "CASH"
        Public Const Cheque As String = "CHEQUE"
        ''' <summary>Walang Pay By o walang Bank: hindi mapupunta sa kahit anong listahan.</summary>
        Public Const Unassigned As String = "UNASSIGNED"

        Public Shared Function DisplayName(grp As String) As String
            Select Case grp
                Case Bdo
                    Return "BDO"
                Case OtherBank
                    Return "Other Bank"
                Case Cash
                    Return "Cash"
                Case Cheque
                    Return "Cheque"
                Case Else
                    Return "No payout method"
            End Select
        End Function

        ''' <summary>Mga listahan na kailangang ma-export bago mag-Post.</summary>
        Public Shared ReadOnly Property Exportable As String() = New String() {Bdo, OtherBank, Cash, Cheque}
    End Class

    ''' <summary>Uri ng export file (nasa tblPayrollExportLog.Format).</summary>
    Public NotInheritable Class ExportFormat
        Public Const BdoConverter As String = "BDO_CONVERTER"   ' Excel na ipe-paste sa BDO ATM Payroll Converter
        Public Const BdoDirect As String = "BDO_DIRECT"         ' .txt (account <TAB> amount) - kapareho ng output ng Converter
        Public Const OtherLists As String = "OTHER_LISTS"       ' Excel: Other Bank / Cash (may pirma) / Cheque
    End Class

    ''' <summary>Isang empleyado sa isang cutoff, kasama ang payout setup (live galing sa tblEmployeeEarnings + tblBank).</summary>
    Public Class PayrollPayoutRow
        Public Property ResultId As Integer
        Public Property CutoffID As Integer
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property NetPay As Decimal
        Public Property PayBy As String
        Public Property BankId As Integer?
        Public Property BankCode As String
        Public Property BankName As String
        Public Property BankAccount As String

        ' Hindi galing sa DB - itinatakda ng PayrollExportBuilder
        Public Property ListGroup As String
        Public Property CutoffLabel As String
    End Class

    ''' <summary>Empleyadong hindi maisasama sa file, at ang dahilan.</summary>
    Public Class PayrollExportIssue
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property Reason As String
    End Class

    ''' <summary>Handa nang i-export: mga row na isasama, mga lalaktawan, at mungkahing pangalan ng file.</summary>
    Public Class PayrollExportPlan
        Public Property Kind As String
        Public Property Cutoffs As List(Of CutoffModel) = New List(Of CutoffModel)()
        Public Property IsCombined As Boolean
        Public Property Rows As List(Of PayrollPayoutRow) = New List(Of PayrollPayoutRow)()
        Public Property Skipped As List(Of PayrollExportIssue) = New List(Of PayrollExportIssue)()
        Public Property SuggestedFileName As String
        Public Property FileFilter As String
        Public Property DefaultExt As String

        Public ReadOnly Property TotalAmount As Decimal
            Get
                Return Rows.Sum(Function(r) r.NetPay)
            End Get
        End Property

        ''' <summary>Teksto para sa Skip/Cancel na prompt.</summary>
        Public Function SkippedText(Optional maxLines As Integer = 15, Optional asPrompt As Boolean = True) As String
            Dim nl = Environment.NewLine
            Dim sb As New System.Text.StringBuilder()
            sb.Append(Skipped.Count).Append(" employee(s) will NOT be included in the file:").Append(nl).Append(nl)
            For Each s In Skipped.Take(maxLines)
                sb.Append("- ").Append(s.EmployeeNo).Append("  ").Append(s.EmployeeName).Append(": ").Append(s.Reason).Append(nl)
            Next
            If Skipped.Count > maxLines Then
                sb.Append("... and ").Append(Skipped.Count - maxLines).Append(" more").Append(nl)
            End If
            sb.Append(nl).Append(If(asPrompt, "Click OK to skip them and continue, or Cancel to stop and fix them first.", "Fix them, then try again."))
            Return sb.ToString()
        End Function
    End Class

    ''' <summary>Isang linya sa tblPayrollExportLog.</summary>
    Public Class PayrollExportLogEntry
        Public Property CutoffID As Integer
        Public Property BatchId As Guid
        Public Property ListGroup As String
        Public Property Format As String
        Public Property FileName As String
        Public Property FilePath As String
        Public Property EmployeeCount As Integer
        Public Property TotalAmount As Decimal
        Public Property IsCombined As Boolean
        Public Property ExportedBy As String
    End Class

    ''' <summary>Mga datos na kailangan sa pagsusuri bago mag-Post (galing sa database).</summary>
    Public Class PayrollPostFacts
        Public Property WarningCount As Integer
        Public Property LoanInstallments As Integer
        ''' <summary>Mga listahang na-export na para sa KASALUKUYANG processing (ProcessedStamp = LastProcessedDate).</summary>
        Public Property ValidGroups As List(Of String) = New List(Of String)()
    End Class

    ''' <summary>Isang cutoff na ipo-post (Processed) at ang mga datos para sa Confirm.</summary>
    Public Class PayrollPostItem
        Public Property CutoffID As Integer
        Public Property CutoffLabel As String
        Public Property EmployeeCount As Integer
        Public Property TotalNet As Decimal
        Public Property LoanInstallments As Integer
        ''' <summary>Mga listahang kailangang na-export na bago mag-Post ng cutoff na ito.</summary>
        Public Property RequiredGroups As List(Of String) = New List(Of String)()
    End Class

    ''' <summary>Hiling na i-Post ang isang cutoff (para sa PostCutoffsAsync).</summary>
    Public Class PayrollPostRequest
        Public Property CutoffID As Integer
        Public Property CutoffLabel As String
        Public Property RequiredGroups As List(Of String) = New List(Of String)()
    End Class

    ''' <summary>
    ''' Resulta ng pagsusuri kung pwede nang i-Post. Pwedeng isang cutoff o ang lahat ng cutoff ng isang pay date
    ''' ("Group by pay date"): all-or-nothing - kapag may isang hindi pwede, walang mapo-post.
    ''' </summary>
    Public Class PayrollPostState
        ''' <summary>Lahat ng napili ay Posted na (walang ipo-post).</summary>
        Public Property IsPosted As Boolean
        Public Property CanPost As Boolean
        Public Property BlockReasons As List(Of String) = New List(Of String)()
        ''' <summary>Mga Processed na cutoff na ipo-post.</summary>
        Public Property Items As List(Of PayrollPostItem) = New List(Of PayrollPostItem)()
        ''' <summary>Mga cutoff sa grupo na Posted na (lalaktawan).</summary>
        Public Property AlreadyPosted As List(Of String) = New List(Of String)()

        Public ReadOnly Property EmployeeCount As Integer
            Get
                Return Items.Sum(Function(i) i.EmployeeCount)
            End Get
        End Property

        Public ReadOnly Property TotalNet As Decimal
            Get
                Return Items.Sum(Function(i) i.TotalNet)
            End Get
        End Property

        Public ReadOnly Property LoanInstallments As Integer
            Get
                Return Items.Sum(Function(i) i.LoanInstallments)
            End Get
        End Property

        Public Function BlockedText() As String
            Dim nl = Environment.NewLine
            Return "Nothing was posted. These need to be fixed first:" & nl & nl &
                   String.Join(nl, BlockReasons.Select(Function(r) "- " & r))
        End Function

        Public Function ConfirmText() As String
            Dim nl = Environment.NewLine
            Dim sb As New System.Text.StringBuilder()
            sb.Append(If(Items.Count = 1, "Post this cutoff?", "Post these " & Items.Count & " cutoffs together?")).Append(nl).Append(nl)
            For Each item In Items
                sb.Append(item.CutoffLabel).Append(":  ").Append(item.EmployeeCount).Append(" employees,  Net ")
                sb.Append(item.TotalNet.ToString("N2")).Append(",  loan installments ").Append(item.LoanInstallments).Append(nl)
            Next
            If Items.Count > 1 Then
                sb.Append(nl).Append("TOTAL:  ").Append(EmployeeCount).Append(" employees,  Net ").Append(TotalNet.ToString("N2"))
                sb.Append(",  loan installments ").Append(LoanInstallments).Append(nl)
            End If
            If AlreadyPosted.Count > 0 Then
                sb.Append(nl).Append("Already posted (skipped): ").Append(String.Join(", ", AlreadyPosted)).Append(nl)
            End If
            sb.Append(nl).Append("POSTING IS FINAL. ").Append(If(Items.Count = 1, "The cutoff", "These cutoffs"))
            sb.Append(" will be locked (no more Input edits or re-processing) and cannot be unposted.").Append(nl).Append(nl)
            sb.Append("Click OK to post, or Cancel to go back.")
            Return sb.ToString()
        End Function
    End Class

    ''' <summary>Resulta ng matagumpay na Post.</summary>
    Public Class PayrollPostResult
        Public Property LoanInstallmentsPaid As Integer
        Public Property LoansCompleted As Integer
        Public Property CutoffsPosted As Integer
        Public Property PayrollRefNo As String
    End Class

    ''' <summary>Isang pay date at lahat ng cutoff (may results) na may ganoong Pay Date - para sa "Group by pay date".</summary>
    Public Class PayDateGroupModel
        Public Property PayDate As Date
        Public Property Cutoffs As List(Of CutoffModel) = New List(Of CutoffModel)()

        Public ReadOnly Property GroupKey As String
            Get
                Return PayDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            End Get
        End Property

        Public ReadOnly Property CutoffIds As List(Of Integer)
            Get
                Return Cutoffs.Select(Function(c) c.CutoffID).ToList()
            End Get
        End Property

        Public ReadOnly Property DisplayLabel As String
            Get
                Dim parts = Cutoffs.GroupBy(Function(c) c.Status).Select(Function(g) g.Count() & " " & g.Key.ToString())
                Return PayDate.ToString("MMM d, yyyy") & "  -  " & Cutoffs.Count & If(Cutoffs.Count = 1, " cutoff", " cutoffs") &
                       " (" & String.Join(", ", parts) & ")"
            End Get
        End Property
    End Class

    ''' <summary>Pangalan ng BDO Direct .txt: CompanyCode + MMDDYY (petsa ngayon) + Batch na 2 digits (1 hanggang 10, hindi pwedeng ulitin sa isang araw).</summary>
    Public Class BdoDirectNaming
        Public Const MaxBatch As Integer = 10

        Public Property CompanyCode As String
        Public Property FileDate As Date
        ''' <summary>Mga batch na nagamit na ngayong araw ayon sa export log ng app na ito.</summary>
        Public Property UsedBatches As List(Of Integer) = New List(Of Integer)()

        ''' <summary>Pinakamaliit na batch na hindi pa nagagamit; 0 kung ubos na (1 hanggang 10).</summary>
        Public ReadOnly Property NextFreeBatch As Integer
            Get
                For b = 1 To MaxBatch
                    If Not UsedBatches.Contains(b) Then Return b
                Next
                Return 0
            End Get
        End Property

        Public Function FilePrefix() As String
            Return CompanyCode & FileDate.ToString("MMddyy", CultureInfo.InvariantCulture)
        End Function

        Public Function FileNameFor(batch As Integer) As String
            Return FilePrefix() & batch.ToString("00", CultureInfo.InvariantCulture)
        End Function
    End Class

    ''' <summary>Panloob: isang LOAN line ng cutoff at ang kasalukuyang kalagayan ng schedule nito (stale check sa Post).</summary>
    Public Class PayrollLoanCheckRow
        Public Property EmployeeName As String
        Public Property Description As String
        Public Property LineAmount As Decimal
        Public Property FoundScheduleId As Integer?
        Public Property AmountDue As Decimal?
        Public Property AmountPaid As Decimal?
        Public Property IsPaid As Boolean?
        Public Property LoanStatus As String
    End Class

End Namespace
