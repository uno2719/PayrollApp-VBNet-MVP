' File: Modules/MainMenu/Employee/Models/ProfileCompleteness.vb
' Phase 3: "Profile %" ng Employee header. PURONG LOGIC (walang database, walang control): binibilang ang
' mga item na dapat may laman, at ang mga kulang ay inililista para sa tooltip. Pantay ang bigat ng bawat item.
' ANG LISTAHAN NG MGA ITEM AY HULA (para sa HR na kumpirmahin) - isang lugar lang ito para baguhin.
Namespace Employee.Models

    Public Class ProfileCompletenessResult
        Public Property Total As Integer
        Public Property Filled As Integer
        Public ReadOnly Property Missing As New List(Of String)

        Public ReadOnly Property Percent As Integer
            Get
                If Total <= 0 Then Return 100
                Return CInt(Math.Floor(Filled * 100.0 / Total))
            End Get
        End Property
    End Class

    Public Module ProfileCompleteness

        Public Function Evaluate(h As EmployeeHeaderModel) As ProfileCompletenessResult
            Dim r As New ProfileCompletenessResult()

            ' Personal Info
            Check(r, "Birth date", h.HasBirthDate)
            Check(r, "Gender", h.HasGender)
            Check(r, "Civil status", h.HasCivilStatus)
            Check(r, "Contact no.", h.HasContactNo)
            Check(r, "Email", h.HasEmail)
            Check(r, "Permanent address", h.HasPermanentAddress)

            ' Employment
            Check(r, "Employment Type", EmployeeMovementRules.IsKnownEmploymentType(h.EmployeeStatus))
            Check(r, "Date Joined", h.DateJoined.HasValue)
            Check(r, "Position", Not String.IsNullOrWhiteSpace(h.PositionName))
            Check(r, "Department", Not String.IsNullOrWhiteSpace(h.DepartmentName))
            Check(r, "Branch", Not String.IsNullOrWhiteSpace(h.BranchName))

            ' Earnings
            Check(r, "Pay cycle", h.HasEarnings AndAlso Not String.IsNullOrWhiteSpace(h.PayCycle))
            Check(r, "Pay rate", h.HasPayRate)
            Check(r, "Pay by", h.HasPayBy)

            ' Statutory
            Check(r, "TIN", h.HasTIN)
            Check(r, "SSS no.", h.HasSSSNo)
            Check(r, "Pag-IBIG no.", h.HasPagIBIGNo)
            Check(r, "PhilHealth no.", h.HasPhilHealthNo)

            ' Photo
            Check(r, "Photo", h.HasPhoto)

            ' 201 Files: isang item kada REQUIRED na category (kung nabasa ang listahan)
            If h.DocumentsKnown Then
                Dim missing As List(Of String) = If(h.MissingRequiredDocuments, New List(Of String)())
                r.Total += h.RequiredDocumentCount
                r.Filled += Math.Max(0, h.RequiredDocumentCount - missing.Count)
                For Each name In missing
                    r.Missing.Add($"201 File: {name}")
                Next
            End If

            Return r
        End Function

        Private Sub Check(r As ProfileCompletenessResult, label As String, ok As Boolean)
            r.Total += 1
            If ok Then
                r.Filled += 1
            Else
                r.Missing.Add(label)
            End If
        End Sub

    End Module

End Namespace
