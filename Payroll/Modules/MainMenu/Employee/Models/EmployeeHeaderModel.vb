Namespace Employee.Models

    ''' <summary>
    ''' Lahat ng kailangan ng HEADER ng Employee screen sa IISANG query:
    ''' pangalan, status, mga petsa, org (position/department/branch),
    ''' at pay setup. Read-only na model - hindi ito ang ginagamit sa pag-save.
    ''' </summary>
    Public Class EmployeeHeaderModel

        Public Property RecordId As Integer
        Public Property EmployeeNo As String
        Public Property FirstName As String
        Public Property MiddleName As String
        Public Property LastName As String
        Public Property IsActive As Boolean

        ' --- tblEmployeeEmployment (maaaring wala pa ang row: lahat Nothing) ---
        Public Property EmployeeStatus As String
        Public Property DateJoined As Date?
        Public Property DateResigned As Date?
        Public Property DateRetired As Date?
        Public Property DateTerminated As Date?

        ' --- Lookups ---
        Public Property PositionName As String
        Public Property DepartmentName As String
        Public Property BranchName As String

        ' --- tblEmployeeEarnings (maaaring wala pa ang row) ---
        Public Property HasEarnings As Boolean
        Public Property PayCycle As String
        Public Property PayrollFlag As Boolean

        ' --- General Settings ---
        ''' <summary>Galing sa tblGeneralSettings.ProbationMonths. Nothing = hindi mabasa (default ng presentation ang gagamitin).</summary>
        Public Property ProbationMonths As Integer?

        Public ReadOnly Property FullName As String
            Get
                Return String.Join(" ", {FirstName, MiddleName, LastName}.
                    Where(Function(p) Not String.IsNullOrWhiteSpace(p)).
                    Select(Function(p) p.Trim()))
            End Get
        End Property

    End Class

End Namespace
