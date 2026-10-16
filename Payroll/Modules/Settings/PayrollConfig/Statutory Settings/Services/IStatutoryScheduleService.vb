Imports Payroll.GlobalShared.Models

Namespace StatutorySettings.Services

    ''' <summary>Isang period ng pay cycle, para sa header ng matrix (hal. "P2: 16 - EOM (pays EOM)").</summary>
    Public Class StatutorySchedulePeriodInfo
        Public Property PeriodNo As Integer
        Public Property RangeText As String
        Public Property PayDayText As String

        Public ReadOnly Property Caption As String
            Get
                Return $"P{PeriodNo}: {RangeText} (pays {PayDayText})"
            End Get
        End Property
    End Class

    ''' <summary>Isang active na pay cycle na may cutoff pattern.</summary>
    Public Class StatutoryScheduleCycleInfo
        Public Property PayCycleType As String
        Public Property PatternText As String
        Public Property Periods As List(Of StatutorySchedulePeriodInfo) = New List(Of StatutorySchedulePeriodInfo)()
    End Class

    ''' <summary>Isang hilera ng matrix: isang agency, at ang share % nito sa bawat period (index 0 = unang period).</summary>
    Public Class StatutoryScheduleRow
        Public Property Agency As String
        Public Property Shares As List(Of Decimal) = New List(Of Decimal)()

        Public ReadOnly Property AgencyName As String
            Get
                Return StatutoryAgency.DisplayName(Agency)
            End Get
        End Property

        Public ReadOnly Property Total As Decimal
            Get
                Dim sum As Decimal = 0D
                For Each s In Shares
                    sum += s
                Next
                Return sum
            End Get
        End Property
    End Class

    ''' <summary>Buong laman ng screen: mga active na pay cycle, ang matrix ng bawat isa, at ang Pag-IBIG voluntary mode.</summary>
    Public Class StatutoryScheduleSetup
        Public Property Cycles As List(Of StatutoryScheduleCycleInfo) = New List(Of StatutoryScheduleCycleInfo)()

        ''' <summary>Pay cycle -> mga row ng matrix (SSS, PhilHealth, Pag-IBIG).</summary>
        Public Property Rows As Dictionary(Of String, List(Of StatutoryScheduleRow)) =
            New Dictionary(Of String, List(Of StatutoryScheduleRow))(StringComparer.OrdinalIgnoreCase)

        Public Property VoluntaryMode As PagIbigVoluntaryMode = PagIbigVoluntaryMode.ReplaceTable
    End Class

    Public Class StatutoryScheduleSaveResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
    End Class

    Public Interface IStatutoryScheduleService

        ''' <summary>
        ''' Kapag ang isang active na pay cycle ay walang schedule para sa isang agency, ilagay ang default
        ''' (SSS = huling period, PhilHealth/Pag-IBIG = unang period). Hindi ginagalaw ang mayroon na.
        ''' </summary>
        Function EnsureDefaultsAsync(userName As String) As Task

        Function GetSetupAsync() As Task(Of StatutoryScheduleSetup)

        ''' <summary>Nothing kung ayos; kung hindi, ang mensahe ng problema.</summary>
        Function Validate(setup As StatutoryScheduleSetup) As String

        Function SaveAsync(setup As StatutoryScheduleSetup, userName As String) As Task(Of StatutoryScheduleSaveResult)
    End Interface

End Namespace
