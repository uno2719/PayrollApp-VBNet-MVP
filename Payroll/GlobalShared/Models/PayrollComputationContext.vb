' File: GlobalShared/Models/PayrollComputationContext.vb
Namespace GlobalShared.Models

    ''' <summary>Isang row ng catalog (Overtime/Holiday/Compensation/Bonus/Deduction) na kailangan ng computation.</summary>
    Public Class PayrollCatalogItem
        Public Property Category As String          ' ResultCategory.Overtime, ...
        Public Property Code As String
        Public Property Description As String
        Public Property InputUnit As PayrollInputUnit
        Public Property Multiplier As Decimal = 1D  ' Overtime/Holiday "Rate"; 1 sa iba
        Public Property TaxFlag As Boolean
        Public Property SSSFlag As Boolean
        Public Property PhilHealthFlag As Boolean
        Public Property PagIbigFlag As Boolean
        Public Property IsActive As Boolean = True

        ''' <summary>tblCompensation.Component2316 (COLA, NDIF, HOLI, ...). Para sa MWE: NDIF/HOLI/COLA ay exempt.</summary>
        Public Property Component2316 As String
    End Class

    ''' <summary>Isang entry mula sa tblPayrollInputTxn (Input Entry), o mula sa Employee > Fix Transactions (may FixTxnId).</summary>
    Public Class PayrollTxnInput
        Public Property RecordId As Integer
        Public Property TxnCategory As String
        Public Property TxnCode As String
        Public Property Qty As Decimal
        Public Property Rate As Decimal

        ''' <summary>Nothing = galing sa Input Entry; may laman = galing sa tblEmployeeFixTxn (FixTransactionRules.MergeTxns).</summary>
        Public Property FixTxnId As Integer?
    End Class

    ''' <summary>Isang hulog ng loan na pwedeng kaltasin sa cutoff na ito.</summary>
    Public Class PayrollLoanDue
        Public Property RecordId As Integer
        Public Property ScheduleId As Integer
        Public Property EmployeeLoanId As Integer
        Public Property LoanCode As String
        Public Property LoanDescription As String
        Public Property InstallmentNo As Integer
        Public Property ScheduledDate As Date
        Public Property BalanceDue As Decimal

        ''' <summary>True kung lampas na ang petsa bago ang window ng cutoff na ito: hindi awtomatikong kakaltasin, babala lang.</summary>
        Public Property IsOverdue As Boolean
    End Class

    ''' <summary>Kabuuan ng mga base ng mga naunang cutoff sa parehong pay month (galing sa naka-save na results).</summary>
    Public Class PayrollPriorBases
        Public Property RecordId As Integer
        Public Property SSSBase As Decimal
        Public Property PhilHealthBase As Decimal
        Public Property PagIbigBase As Decimal
    End Class

    ''' <summary>Lahat ng datos ng isang empleyado na kailangan ng computation (isang row bawat empleyadong kasama).</summary>
    Public Class PayrollEmployeeInput
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property PayCycle As String

        Public Property BasicSalary As Decimal?
        Public Property DailyRate As Decimal?
        Public Property HourlyRate As Decimal?
        Public Property DaysInYear As Integer?
        Public Property WorkHourPer As Decimal?
        Public Property MinimumWage As Boolean

        Public Property PagIBIGVoluntary As Decimal?
        Public Property FixTax As Decimal?         ' hindi pa ginagamit (naka-snapshot lang sa hinaharap)

        Public Property DateJoined As Date?
        Public Property DateResigned As Date?
        Public Property DateRetired As Date?
        Public Property DateTerminated As Date?
        Public Property DateEndOfContract As Date?

        ''' <summary>Input Entry lang (tblPayrollInputTxn). Ang Fix Transactions ay idinudugtong ng engine (MergeTxns).</summary>
        Public Property Txns As List(Of PayrollTxnInput) = New List(Of PayrollTxnInput)()

        ''' <summary>Lahat ng ACTIVE na Fix Transaction ng empleyado (sinasala ng engine ayon sa cutoff).</summary>
        Public Property FixTxns As List(Of EmployeeFixTxnModel) = New List(Of EmployeeFixTxnModel)()
        Public Property Loans As List(Of PayrollLoanDue) = New List(Of PayrollLoanDue)()
        Public Property Prior As PayrollPriorBases = New PayrollPriorBases()
    End Class

    ''' <summary>Lahat ng kailangan ng PayrollComputationRules para mag-compute ng isang cutoff (walang database sa loob ng engine).</summary>
    Public Class PayrollComputationContext
        Public Property Cutoff As CutoffModel
        Public Property PayCycle As PayCycleModel
        Public Property Settings As GeneralSettingsModel
        Public Property BasicCode As CompensationModel

        ' Key = PayrollComputationRules.CatalogKey(category, code)
        Public Property Catalog As Dictionary(Of String, PayrollCatalogItem) = New Dictionary(Of String, PayrollCatalogItem)(StringComparer.OrdinalIgnoreCase)

        Public Property Schedule As List(Of StatutoryScheduleModel) = New List(Of StatutoryScheduleModel)()
        Public Property SssBrackets As List(Of StatutoryBracketModel) = New List(Of StatutoryBracketModel)()
        Public Property PhilHealthBrackets As List(Of StatutoryBracketModel) = New List(Of StatutoryBracketModel)()
        Public Property PagIbigBrackets As List(Of StatutoryBracketModel) = New List(Of StatutoryBracketModel)()

        Public Property TaxTableName As String
        Public Property TaxBrackets As List(Of IncomeTaxBracketModel) = New List(Of IncomeTaxBracketModel)()

        Public Property Employees As List(Of PayrollEmployeeInput) = New List(Of PayrollEmployeeInput)()
    End Class

End Namespace
