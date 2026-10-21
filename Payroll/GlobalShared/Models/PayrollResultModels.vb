' File: GlobalShared/Models/PayrollResultModels.vb
Namespace GlobalShared.Models

    ''' <summary>Uri ng linya sa tblPayrollResultLine.</summary>
    Public NotInheritable Class ResultLineType
        Public Const Earning As String = "EARNING"
        Public Const Deduction As String = "DEDUCTION"
        Public Const Employer As String = "EMPLOYER"     ' employer share - hindi bahagi ng net pay
    End Class

    ''' <summary>
    ''' Category ng linya. Ang Overtime/Holiday/Compensation/Bonus/Deduction ay kapareho ng TxnCategory ng
    ''' tblPayrollInputTxn (PayrollInputUnits.Category...), ang iba ay galing sa computation.
    ''' </summary>
    Public NotInheritable Class ResultCategory
        Public Const Basic As String = "BASIC"
        Public Const Overtime As String = PayrollInputUnits.CategoryOvertime
        Public Const Holiday As String = PayrollInputUnits.CategoryHoliday
        Public Const Compensation As String = PayrollInputUnits.CategoryCompensation
        Public Const Bonus As String = PayrollInputUnits.CategoryBonus
        Public Const Deduction As String = PayrollInputUnits.CategoryDeduction
        Public Const SSS As String = "SSS"
        Public Const PhilHealth As String = "PHILHEALTH"
        Public Const PagIbig As String = "PAGIBIG"
        Public Const Tax As String = "TAX"
        Public Const Loan As String = "LOAN"
    End Class

    ''' <summary>Isang row ng tblPayrollResult: ang resulta ng isang employee sa isang cutoff.</summary>
    Public Class PayrollResultModel
        Public Property ResultId As Integer
        Public Property CutoffID As Integer
        Public Property RecordId As Integer

        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property PayCycle As String
        Public Property RateBasis As Byte
        Public Property PeriodInMonth As Integer?
        Public Property DaysWorked As Decimal?

        ' Snapshot ng rates
        Public Property BasicSalary As Decimal?
        Public Property DailyRate As Decimal?
        Public Property HourlyRate As Decimal?
        Public Property DaysInYear As Integer?
        Public Property WorkHourPer As Decimal?
        Public Property DailyRateMonthlyFactor As Decimal?
        Public Property IsMinimumWage As Boolean

        Public Property BasicPay As Decimal
        Public Property GrossPay As Decimal

        ' Bases (cutoff na ito) at ang aktwal na basehan ng bracket lookup
        Public Property TaxGross As Decimal
        Public Property SSSBase As Decimal
        Public Property PhilHealthBase As Decimal
        Public Property PagIbigBase As Decimal
        Public Property SSSLookupBase As Decimal
        Public Property PhilHealthLookupBase As Decimal
        Public Property PagIbigLookupBase As Decimal

        ' Employee contributions na kinaltas sa cutoff na ito
        Public Property SSSEE As Decimal
        Public Property SSSMPFEE As Decimal
        Public Property PhilHealthEE As Decimal
        Public Property PagIbigEE As Decimal

        ' Employer share
        Public Property SSSER As Decimal
        Public Property SSSMPFER As Decimal
        Public Property ECC As Decimal
        Public Property PhilHealthER As Decimal
        Public Property PagIbigER As Decimal

        Public Property TaxableIncome As Decimal
        Public Property WithholdingTax As Decimal
        Public Property LoanDeduction As Decimal
        Public Property OtherDeduction As Decimal
        Public Property TotalDeduction As Decimal
        Public Property NetPay As Decimal

        Public Property HasWarnings As Boolean
        Public Property WarningText As String

        Public Property ProcessedBy As String
        Public Property ProcessedDate As DateTime?

        ''' <summary>Kabuuang EE contributions (SSS + MPF + PhilHealth + Pag-IBIG) para sa display.</summary>
        Public ReadOnly Property ContributionEE As Decimal
            Get
                Return SSSEE + SSSMPFEE + PhilHealthEE + PagIbigEE
            End Get
        End Property
    End Class

    ''' <summary>Isang row ng tblPayrollResultLine.</summary>
    Public Class PayrollResultLineModel
        Public Property LineId As Integer
        Public Property ResultId As Integer
        Public Property CutoffID As Integer
        Public Property RecordId As Integer
        Public Property SeqNo As Integer
        Public Property LineType As String
        Public Property Category As String
        Public Property Code As String
        Public Property Description As String
        Public Property Qty As Decimal
        Public Property Unit As String
        Public Property RateUsed As Decimal
        Public Property Amount As Decimal
        Public Property TaxFlag As Boolean
        Public Property SSSFlag As Boolean
        Public Property PhilHealthFlag As Boolean
        Public Property PagIbigFlag As Boolean
        Public Property RefId As Integer?
        Public Property Remark As String
    End Class

    ''' <summary>Isang row ng tblPayrollResultBracket: snapshot ng bracket na ginamit.</summary>
    Public Class PayrollResultBracketModel
        Public Property BracketSnapId As Integer
        Public Property ResultId As Integer
        Public Property Agency As String
        Public Property SourceTable As String
        Public Property SourceId As Integer?
        Public Property BasisAmount As Decimal
        Public Property SalaryFrom As Decimal
        Public Property SalaryTo As Decimal
        Public Property EEShare As Decimal?
        Public Property EEContriType As String
        Public Property ERShare As Decimal?
        Public Property ERContriType As String
        Public Property ECCAmount As Decimal?
        Public Property EEMPF As Decimal?
        Public Property ERMPF As Decimal?
        Public Property TaxPercentage As Decimal?
        Public Property FixTaxAmount As Decimal?
        Public Property MonthlyEE As Decimal?
        Public Property MonthlyER As Decimal?
        Public Property SharePercent As Decimal?
    End Class

    ''' <summary>Resulta ng computation ng isang employee, bago i-save (header + mga linya + mga bracket + babala).</summary>
    Public Class PayrollEmployeeResult
        Public Property Header As PayrollResultModel = New PayrollResultModel()
        Public Property Lines As List(Of PayrollResultLineModel) = New List(Of PayrollResultLineModel)()
        Public Property Brackets As List(Of PayrollResultBracketModel) = New List(Of PayrollResultBracketModel)()
        Public Property Warnings As List(Of String) = New List(Of String)()
    End Class

    ''' <summary>
    ''' Ang buong computation ng isang cutoff, handa nang ipakita (summary) at i-save pagkatapos mag-Confirm.
    ''' </summary>
    Public Class PayrollRunPreview
        Public Property Cutoff As CutoffModel
        Public Property Results As List(Of PayrollEmployeeResult) = New List(Of PayrollEmployeeResult)()

        ''' <summary>Mga empleyadong hindi naisama dahil kulang ang setup (galing sa PayrollEligibilityRules).</summary>
        Public Property Excluded As List(Of PayrollEligibilityWarning) = New List(Of PayrollEligibilityWarning)()

        Public ReadOnly Property EmployeeCount As Integer
            Get
                Return Results.Count
            End Get
        End Property

        Public ReadOnly Property TotalGross As Decimal
            Get
                Return Results.Sum(Function(r) r.Header.GrossPay)
            End Get
        End Property

        Public ReadOnly Property TotalDeduction As Decimal
            Get
                Return Results.Sum(Function(r) r.Header.TotalDeduction)
            End Get
        End Property

        Public ReadOnly Property TotalNet As Decimal
            Get
                Return Results.Sum(Function(r) r.Header.NetPay)
            End Get
        End Property

        Public ReadOnly Property WarningCount As Integer
            Get
                Return Results.Where(Function(r) r.Warnings.Count > 0).Count()
            End Get
        End Property

        ''' <summary>Teksto para sa Confirm dialog: buod + unang ilang babala.</summary>
        Public Function SummaryText(Optional maxWarningLines As Integer = 8) As String
            Dim nl = Environment.NewLine
            Dim sb As New System.Text.StringBuilder()

            sb.Append("Cutoff: ").Append(Cutoff.DisplayLabel).Append(nl)
            sb.Append("Employees: ").Append(EmployeeCount).Append(nl)
            sb.Append("Total Gross: ").Append(TotalGross.ToString("N2")).Append(nl)
            sb.Append("Total Deductions: ").Append(TotalDeduction.ToString("N2")).Append(nl)
            sb.Append("Total Net Pay: ").Append(TotalNet.ToString("N2")).Append(nl)

            If Excluded.Count > 0 Then
                sb.Append(nl).Append(Excluded.Count).Append(" employee(s) excluded (incomplete setup).").Append(nl)
            End If

            If WarningCount > 0 Then
                sb.Append(nl).Append(WarningCount).Append(" employee(s) with warnings:").Append(nl)
                Dim shown = 0
                For Each r In Results
                    If r.Warnings.Count = 0 Then Continue For
                    If shown >= maxWarningLines Then
                        sb.Append("...and ").Append(WarningCount - shown).Append(" more (see Payroll > Output).").Append(nl)
                        Exit For
                    End If
                    sb.Append(r.Header.EmployeeNo).Append("  ").Append(r.Header.EmployeeName).Append(": ").
                       Append(r.Warnings(0))
                    If r.Warnings.Count > 1 Then sb.Append($" (+{r.Warnings.Count - 1} more)")
                    sb.Append(nl)
                    shown += 1
                Next
            End If

            sb.Append(nl).Append("Save this computation and mark the cutoff as Processed?")
            Return sb.ToString()
        End Function
    End Class

    ''' <summary>Laman ng Output tab para sa isang cutoff.</summary>
    Public Class PayrollResultSheet
        Public Property Headers As List(Of PayrollResultModel) = New List(Of PayrollResultModel)()
        Public Property Lines As List(Of PayrollResultLineModel) = New List(Of PayrollResultLineModel)()
    End Class

End Namespace
