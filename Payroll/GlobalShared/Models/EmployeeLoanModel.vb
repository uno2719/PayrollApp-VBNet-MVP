' ============================================================
' GlobalShared/Models/EmployeeLoanModel.vb
' ============================================================
' Isang hilera mula sa tblEmployeeLoan, PLUS ilang computed na
' column na galing sa JOIN/subquery ng repository.
'
' BAKIT MAY COMPUTED PROPERTIES DITO?
'   Yung Balance, PaidCount, NextDeductionDate ay hindi naka-store
'   sa tblEmployeeLoan - kinukuwenta sila mula sa schedule table.
'   Pwede sana nating gawing hiwalay na "ListItem" class, pero
'   ibig sabihin noon ay dalawang model na halos magkapareho.
'
'   Mas simple: iisang model, ang mga derived na field ay
'   nilalagyan lang ng halaga ng SELECT na may subqueries. Kapag
'   ang tumawag ay isang plain na GetById (walang subqueries),
'   zero lang sila - at yun ay tama namang default.
' ============================================================
Namespace GlobalShared.Models

    Public Class EmployeeLoanModel

        ' --- Identity ---
        Public Property Id As Integer
        Public Property EmployeeRecordId As Integer
        Public Property LoanCode As String

        ' --- Petsa ---
        Public Property InputDate As Date = Date.Today
        Public Property LoanStartDate As Date = Date.Today

        ' --- Paraan ng kaltas ---
        Public Property Frequency As String
        Public Property AmortizationMethod As String
        Public Property InterestMethod As String
        Public Property InterestRate As Decimal
        Public Property Terms As Integer

        ' --- Halaga ---
        Public Property PrincipalAmount As Decimal
        Public Property InterestAmount As Decimal
        Public Property TotalLoanAmount As Decimal

        Public Property PrincipalAmortization As Decimal
        Public Property InterestAmortization As Decimal
        Public Property TotalAmortization As Decimal

        ' --- Estado ---
        Public Property Status As String = Constants.LoanStatus.Active
        Public Property CompletedAt As DateTime?
        Public Property Remark As String

        ' --- Audit ---
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ' ========================================================
        ' COMPUTED - pinupuno lang ng list query
        ' ========================================================
        Public Property LoanDescription As String
        Public Property LoanType As String

        Public Property EmployeeNo As String
        Public Property EmployeeName As String

        Public Property TotalInstallments As Integer
        Public Property PaidInstallments As Integer
        Public Property TotalPaid As Decimal
        Public Property NextDeductionDate As Date?
        Public Property NextDeductionAmount As Decimal

        Public ReadOnly Property Balance As Decimal
            Get
                Dim bal = TotalLoanAmount - TotalPaid
                Return If(bal < 0D, 0D, bal)
            End Get
        End Property

        Public ReadOnly Property IsFullyPaid As Boolean
            Get
                If TotalInstallments <= 0 Then Return False
                Return PaidInstallments >= TotalInstallments
            End Get
        End Property

        Public ReadOnly Property RemainingInstallments As Integer
            Get
                Dim remaining = TotalInstallments - PaidInstallments
                Return If(remaining < 0, 0, remaining)
            End Get
        End Property

        ''' <summary>
        ''' Para sa display: "3 of 24 paid"
        ''' </summary>
        Public ReadOnly Property ProgressText As String
            Get
                Return $"{PaidInstallments} of {TotalInstallments} paid"
            End Get
        End Property

    End Class

End Namespace