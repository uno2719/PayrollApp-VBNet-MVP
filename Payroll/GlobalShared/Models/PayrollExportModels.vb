' File: GlobalShared/Models/PayrollExportModels.vb
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

    ''' <summary>Resulta ng pagsusuri kung pwede nang i-Post ang cutoff.</summary>
    Public Class PayrollPostState
        Public Property CutoffID As Integer
        Public Property CutoffLabel As String
        Public Property IsPosted As Boolean
        Public Property CanPost As Boolean
        Public Property BlockReasons As List(Of String) = New List(Of String)()
        Public Property EmployeeCount As Integer
        Public Property TotalNet As Decimal
        Public Property LoanInstallments As Integer
        Public Property RequiredGroups As List(Of String) = New List(Of String)()

        Public Function BlockedText() As String
            Dim nl = Environment.NewLine
            Return "This cutoff cannot be posted yet:" & nl & nl &
                   String.Join(nl, BlockReasons.Select(Function(r) "- " & r))
        End Function

        Public Function ConfirmText() As String
            Dim nl = Environment.NewLine
            Return "Post this cutoff?" & nl & nl &
                   "Cutoff: " & CutoffLabel & nl &
                   "Employees: " & EmployeeCount & nl &
                   "Total Net Pay: " & TotalNet.ToString("N2") & nl &
                   "Loan installments to be marked paid: " & LoanInstallments & nl & nl &
                   "POSTING IS FINAL. The cutoff will be locked (no more Input edits or re-processing) and it cannot be unposted." & nl & nl &
                   "Click OK to post, or Cancel to go back."
        End Function
    End Class

    ''' <summary>Resulta ng matagumpay na Post.</summary>
    Public Class PayrollPostResult
        Public Property LoanInstallmentsPaid As Integer
        Public Property LoansCompleted As Integer
        Public Property PayrollRefNo As String
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
