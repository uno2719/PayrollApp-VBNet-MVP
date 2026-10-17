Namespace GlobalShared.Models

    ''' <summary>
    ''' Resulta ng pagsusuri kung kasama ang isang empleyado sa payroll ng isang cutoff.
    ''' Eligible = kasama. Ang mga sumunod ay hindi kasama: ang unang grupo ay NORMAL (walang babala);
    ''' ang huling grupo (NoPayCycle, NoDateJoined, NoPayRate) ay kulang ang setup, kaya may BABALA.
    ''' </summary>
    Public Enum EligibilityStatus
        Eligible = 0

        ' --- Hindi kasama, normal ---
        ExcludedFromPayroll     ' naka-off ang "Include in payroll"
        OtherPayCycle           ' ibang pay cycle ang empleyado
        Separated               ' umalis na bago magsimula ang cutoff
        Inactive                ' Inactive at walang petsa ng pag-alis
        NotYetJoined            ' Date Joined ay pagkatapos ng cutoff

        ' --- Hindi kasama, kulang ang setup (may babala) ---
        NoPayCycle
        NoDateJoined
        NoPayRate
    End Enum

    ''' <summary>Datos ng isang empleyado na kailangan sa pagsusuri (isang row ng eligibility query).</summary>
    Public Class EligibilityCandidate
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property IsActive As Boolean

        ' Earnings
        Public Property HasEarnings As Boolean
        Public Property PayCycle As String
        Public Property PayrollFlag As Boolean
        Public Property BasicSalary As Decimal?
        Public Property DailyRate As Decimal?

        ' Employment
        Public Property DateJoined As DateTime?
        Public Property DateResigned As DateTime?
        Public Property DateRetired As DateTime?
        Public Property DateTerminated As DateTime?
    End Class

    Public Class PayrollEligibleEmployee
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
    End Class

    ''' <summary>Empleyadong hindi maisasama dahil kulang ang setup, at kung ano ang kulang.</summary>
    Public Class PayrollEligibilityWarning
        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property EmployeeName As String
        Public Property Reason As String
    End Class

    Public Class PayrollEligibilityResult
        Public Property Eligible As List(Of PayrollEligibleEmployee) = New List(Of PayrollEligibleEmployee)()
        Public Property Warnings As List(Of PayrollEligibilityWarning) = New List(Of PayrollEligibilityWarning)()
    End Class

End Namespace
