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

        ' --- Para sa "Profile %" (presence flags lang; hindi dinadala ang aktwal na halaga, hal. sahod o TIN) ---
        Public Property HasBirthDate As Boolean
        Public Property HasGender As Boolean
        Public Property HasCivilStatus As Boolean
        Public Property HasContactNo As Boolean
        Public Property HasEmail As Boolean
        Public Property HasPermanentAddress As Boolean
        Public Property HasPayRate As Boolean
        Public Property HasPayBy As Boolean
        Public Property HasTIN As Boolean
        Public Property HasSSSNo As Boolean
        Public Property HasPagIBIGNo As Boolean
        Public Property HasPhilHealthNo As Boolean

        ''' <summary>Itinatakda ng repository (hiwalay na query): may photo ba. False kung wala pang PhotoPath column.</summary>
        Public Property HasPhoto As Boolean

        ''' <summary>True kung nabasa ang listahan ng required 201 categories (nandoon na ang mga bagong table).</summary>
        Public Property DocumentsKnown As Boolean
        Public Property RequiredDocumentCount As Integer
        Public Property MissingRequiredDocuments As List(Of String)

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
