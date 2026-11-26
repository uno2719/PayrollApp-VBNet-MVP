' File: GlobalShared/Models/PayslipModels.vb
Namespace GlobalShared.Models

    ''' <summary>Isang row sa employee grid ng Reports > Payslip (isang empleyado na may result sa cutoff).</summary>
    Public Class PayslipEmployeeRow
        Public Property Selected As Boolean

        Public Property ResultId As Integer
        Public Property CutoffID As Integer
        ''' <summary>Pangalan ng cutoff (ipinapakita sa grid kapag Group by pay date).</summary>
        Public Property CutoffLabel As String
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property DepartmentName As String
        Public Property PositionName As String
        Public Property PayBy As String
        Public Property EmailAddress As String
        Public Property NetPay As Decimal

        ''' <summary>Huling pagpapadala (galing sa tblPayslipEmailLog). Nothing = hindi pa napapadalhan.</summary>
        Public Property LastEmailDate As DateTime?
        Public Property LastEmailSuccess As Boolean?
        Public Property LastEmailError As String

        Public ReadOnly Property HasEmail As Boolean
            Get
                Return Not String.IsNullOrWhiteSpace(EmailAddress)
            End Get
        End Property

        Public ReadOnly Property EmailStatusText As String
            Get
                If Not LastEmailDate.HasValue Then Return If(HasEmail, "Not sent", "No email address")
                Dim stamp = LastEmailDate.Value.ToString("MMM d, yyyy h:mm tt")
                If LastEmailSuccess.GetValueOrDefault() Then Return "Sent " & stamp
                Return "Failed " & stamp & If(String.IsNullOrWhiteSpace(LastEmailError), "", ": " & LastEmailError)
            End Get
        End Property
    End Class

    ''' <summary>Live na detalye ng empleyado para sa header ng payslip (hindi snapshot).</summary>
    Public Class PayslipEmployeeInfo
        Public Property RecordId As Integer
        Public Property PositionName As String
        Public Property DepartmentName As String
        Public Property BranchName As String
        Public Property SSSNo As String
        Public Property PhilHealthNo As String
        Public Property PagIBIGNo As String
        Public Property TIN As String
        Public Property PayBy As String
        Public Property EmailAddress As String
        Public Property BirthDate As Date?
    End Class

    ''' <summary>Detalye ng LOAN line: pang-ilang hulog sa kabuuan (para sa "(5/24)").</summary>
    Public Class PayslipLoanLineInfo
        Public Property LineId As Integer
        Public Property EmployeeLoanId As Integer
        Public Property InstallmentNo As Integer
        Public Property TotalInstallments As Integer
    End Class

    ''' <summary>Balance ng isang loan as of pay date ng cutoff (galing sa repository, bago ibawas ang cutoff kung hindi pa Posted).</summary>
    Public Class PayslipLoanBalance
        Public Property RecordId As Integer
        Public Property EmployeeLoanId As Integer
        Public Property LoanCode As String
        Public Property LoanName As String
        Public Property Status As String
        Public Property TotalInstallments As Integer
        Public Property PaidInstallments As Integer
        Public Property Balance As Decimal
    End Class

    ''' <summary>YTD ng isang empleyado (Posted cutoffs ng parehong taon bago ang pay date + ang cutoff na ito).</summary>
    Public Class PayslipYtd
        Public Property RecordId As Integer
        Public Property Gross As Decimal
        Public Property Contributions As Decimal
        Public Property Tax As Decimal
        ''' <summary>Kabuuang allowance lines sa parehong saklaw - ibinabawas sa Gross kapag naka-exclude ang allowance.</summary>
        Public Property Allowance As Decimal
    End Class

    ''' <summary>Company header ng payslip.</summary>
    Public Class PayslipCompanyInfo
        Public Property CompanyName As String
        Public Property AddressText As String
        Public Property ContactText As String
        ''' <summary>Buong path ng logo (shared folder muna, saka lokal). Blangko kung wala.</summary>
        Public Property LogoFullPath As String
    End Class

    ''' <summary>Lahat ng hilaw na data ng isang cutoff para makabuo ng payslips.</summary>
    Public Class PayslipSource
        Public Property Cutoff As CutoffModel
        Public Property Company As PayslipCompanyInfo
        Public Property Headers As List(Of PayrollResultModel) = New List(Of PayrollResultModel)()
        Public Property Lines As List(Of PayrollResultLineModel) = New List(Of PayrollResultLineModel)()
        Public Property Employees As List(Of PayslipEmployeeInfo) = New List(Of PayslipEmployeeInfo)()
        Public Property LoanLines As List(Of PayslipLoanLineInfo) = New List(Of PayslipLoanLineInfo)()
        Public Property LoanBalances As List(Of PayslipLoanBalance) = New List(Of PayslipLoanBalance)()
        Public Property Ytd As List(Of PayslipYtd) = New List(Of PayslipYtd)()
        Public Property AllowanceCodes As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    End Class

    ''' <summary>Mga option ng pag-generate (galing sa screen).</summary>
    Public Class PayslipOptions
        Public Property IncludeAllowance As Boolean = True
        Public Property ShowYtd As Boolean = True
        ''' <summary>Email lang: lagyan ng password ang PDF (birthdate MMDDYYYY; Employee ID kung walang birthdate).</summary>
        Public Property ProtectPdf As Boolean = True
        Public Property PrintedBy As String
    End Class

    ''' <summary>
    ''' Mga nabuong payslip (handa nang i-preview / i-print / i-export / i-email). Isang cutoff, o ilang cutoff kapag
    ''' Group by pay date (pinagsama sa iisang batch, naka-sort ayon sa pangalan).
    ''' </summary>
    Public Class PayslipBatch
        ''' <summary>Unang cutoff (pang-display / pangalan ng file). Nasa Cutoffs ang lahat.</summary>
        Public Property Cutoff As CutoffModel
        Public Property Cutoffs As List(Of CutoffModel) = New List(Of CutoffModel)()
        Public Property Company As PayslipCompanyInfo
        ''' <summary>True kung may kahit isang cutoff na hindi pa Posted (may DRAFT na payslip).</summary>
        Public Property IsDraft As Boolean
        Public Property Documents As List(Of PayslipDocument) = New List(Of PayslipDocument)()

        ''' <summary>Lahat ng payslip ay DRAFT (para sa watermark ng buong report).</summary>
        Public ReadOnly Property AllDraft As Boolean
            Get
                Return Documents.Count > 0 AndAlso Documents.All(Function(d) d.IsDraft)
            End Get
        End Property

        Public ReadOnly Property DisplayLabel As String
            Get
                If Cutoffs.Count > 1 Then
                    Return "Pay date " & If(Cutoff.PayDate.HasValue, Cutoff.PayDate.Value, Cutoff.CutoffEnd).ToString("MMM d, yyyy")
                End If
                Return If(Cutoff?.DisplayLabel, "")
            End Get
        End Property

        ''' <summary>Pinagsasama ang mga batch ng bawat cutoff sa iisa (Group by pay date).</summary>
        Public Shared Function Merge(batches As IList(Of PayslipBatch)) As PayslipBatch
            If batches Is Nothing OrElse batches.Count = 0 Then Return Nothing
            If batches.Count = 1 Then Return batches(0)
            Dim merged As New PayslipBatch With {
                .Cutoff = batches(0).Cutoff,
                .Cutoffs = batches.Select(Function(b) b.Cutoff).ToList(),
                .Company = batches(0).Company,
                .IsDraft = batches.Any(Function(b) b.IsDraft),
                .Documents = batches.SelectMany(Function(b) b.Documents).
                    OrderBy(Function(d) d.EmployeeName).ThenBy(Function(d) d.EmployeeNo).ToList()
            }
            Return merged
        End Function
    End Class

    ' ======================================================================
    ' Ang ibinibigay sa rptPayslip (isang PayslipDocument = isang payslip/page)
    ' ======================================================================

    Public Enum PayslipCellKind
        Blank = 0
        Line = 1          ' karaniwang linya (name + amount)
        GroupHeader = 2   ' "Statutory" / "Loans" / "Others" sa Deductions
        GroupLine = 3     ' linya sa ilalim ng group (naka-indent)
        Total = 4         ' Gross pay / Total deductions
        Net = 5           ' Net pay (naka-highlight)
        Outside = 6       ' labas sa kahon (walang border) - hal. kaliwang bahagi ng Net pay row
    End Enum

    ''' <summary>
    ''' Isang row ng magkatabing Earnings (kaliwa) at Deductions (kanan). Pinagpapares ang dalawang listahan para
    ''' iisang DetailReportBand lang ang kailangan at laging magkapantay ang mga row at ang totals.
    ''' </summary>
    Public Class PayslipPairRow
        Public Property LeftText As String
        Public Property LeftAmount As Decimal?
        Public Property LeftKind As PayslipCellKind
        Public Property RightText As String
        Public Property RightAmount As Decimal?
        Public Property RightKind As PayslipCellKind
    End Class

    Public Class PayslipLoanRow
        Public Property LoanName As String
        Public Property Progress As String
        Public Property Balance As Decimal?
    End Class

    ''' <summary>Footer ng payslip (YTD + printed by + mga paalala). Isang row lang kada payslip.</summary>
    Public Class PayslipFooterRow
        Public Property ShowYtd As Boolean
        Public Property YtdGross As Decimal
        Public Property YtdContributions As Decimal
        Public Property YtdTax As Decimal
        Public Property PrintedText As String
        Public Property AllowanceNote As String
        Public Property ShowReceivedBy As Boolean
    End Class

    Public Class PayslipDocument
        Public Property CutoffID As Integer
        Public Property RecordId As Integer
        Public Property IsDraft As Boolean

        ' Company
        Public Property CompanyName As String
        Public Property CompanyAddress As String

        ' Employee
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property Designation As String
        Public Property Department As String
        Public Property Branch As String
        Public Property SSSNo As String
        Public Property PhilHealthNo As String
        Public Property PagIBIGNo As String
        Public Property TIN As String

        ' Period
        Public Property PayPeriod As String
        Public Property PayDateText As String
        Public Property PayCycle As String
        Public Property RefNo As String

        ' Totals (pagkatapos i-apply ang Include allowance)
        Public Property GrossPay As Decimal
        Public Property TotalDeduction As Decimal
        Public Property NetPay As Decimal
        Public Property ExcludedAllowance As Decimal

        Public Property Rows As List(Of PayslipPairRow) = New List(Of PayslipPairRow)()
        Public Property Loans As List(Of PayslipLoanRow) = New List(Of PayslipLoanRow)()
        Public Property Footer As List(Of PayslipFooterRow) = New List(Of PayslipFooterRow)()

        ' Para sa email (hindi ipinapakita)
        Public Property EmailAddress As String
        Public Property BirthDate As Date?
        Public Property PayDate As Date
    End Class

    ''' <summary>Isang entry sa tblPayslipEmailLog.</summary>
    Public Class PayslipEmailLogEntry
        Public Property CutoffID As Integer
        Public Property RecordId As Integer
        Public Property EmailAddress As String
        Public Property IsSuccess As Boolean
        Public Property ErrorMessage As String
        Public Property IncludeAllowance As Boolean
        Public Property SentBy As String
    End Class

    ''' <summary>Resulta ng pagpapadala sa isang empleyado.</summary>
    Public Class PayslipEmailResult
        Public Property RecordId As Integer
        Public Property EmployeeName As String
        Public Property EmailAddress As String
        Public Property Success As Boolean
        Public Property Message As String
    End Class

    ''' <summary>Progress habang nagpapadala (para sa progress bar ng screen).</summary>
    Public Class PayslipEmailProgress
        Public Property Done As Integer
        Public Property Total As Integer
        Public Property CurrentName As String
    End Class

End Namespace
