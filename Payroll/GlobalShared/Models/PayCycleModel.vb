' File: GlobalShared/Models/PayCycleModel.vb
Namespace GlobalShared.Models

    ''' <summary>
    ''' Paano kinukuha ang sahod ng employee sa isang pay cycle.
    '''   BasicSalary = Basic Salary (1 buwan) hinati sa bilang ng cutoff kada buwan
    '''   DailyRate   = Daily Rate x Days Worked sa cutoff (kaya may "Days Worked" input)
    ''' </summary>
    Public Enum PayRateBasis As Byte
        BasicSalary = 0
        DailyRate = 1
    End Enum

    ''' <summary>
    ''' Isang row ng "cutoff pattern" ng pay cycle. Naka-ANCHOR sa PAY MONTH:
    ''' ang bawat row ay nagsasabi kung kailan nagsisimula/nagtatapos ang cutoff
    ''' at kung anong araw ang payday, na lahat ay kaugnay ng buwan ng payday.
    '''
    ''' Halimbawa (Daily):  Row 1 = From 21 (Previous month) To 5 (This month), Pay 15
    '''                     Row 2 = From 6  (This month)     To 20 (This month), Pay EOM
    '''
    ''' Ang day value ay 1-31. Ang 31 ay itinuturing na "EOM" (End of Month):
    ''' kapag mas maikli ang buwan, kinukuha lang ang huling araw (clamp), kaya
    ''' tama ang 31 para sa Pebrero at 30-day months. Walang special sentinel.
    ''' </summary>
    Public Class PayCyclePeriodModel
        Public Property PayCyclePeriodId As Integer
        Public Property PayCycleId As Integer
        Public Property PeriodNo As Integer
        Public Property FromDay As Integer = 1
        Public Property FromMonthOffset As Integer = 0     ' -1 Previous, 0 This, 1 Next (relative sa pay month)
        Public Property ToDay As Integer = 15
        Public Property ToMonthOffset As Integer = 0
        Public Property PayDay As Integer = 15
    End Class

    ''' <summary>
    ''' Isang row ng tblPayCycle - isa para sa bawat pay cycle type
    ''' (Monthly/SemiMonthly/Weekly/Daily). Ang PayCycleType ay kapareho ng
    ''' tblEmployeeEarnings.PayCycle at tblCutoff.CycleType, kaya walang migration.
    ''' </summary>
    Public Class PayCycleModel
        Public Property PayCycleId As Integer
        Public Property PayCycleType As String
        Public Property RateBasis As PayRateBasis = PayRateBasis.BasicSalary
        Public Property IsActive As Boolean

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        ' Hindi galing sa tblPayCycle - nilo-load mula sa tblPayCyclePeriod
        Public Property Periods As List(Of PayCyclePeriodModel) = New List(Of PayCyclePeriodModel)()

        ''' <summary>
        ''' Ilan ang cutoff kada buwan = bilang ng pattern rows. Ito ang divisor
        ''' ng Basic Salary kapag BasicSalary ang rate basis (SemiMonthly = 2).
        ''' </summary>
        Public ReadOnly Property PeriodsPerMonth As Integer
            Get
                Return If(Periods Is Nothing, 0, Periods.Count)
            End Get
        End Property

        Public ReadOnly Property RateBasisText As String
            Get
                Return PayCycleChoices.RateBasisText(RateBasis)
            End Get
        End Property

        Public ReadOnly Property ActiveText As String
            Get
                Return If(IsActive, "Yes", "No")
            End Get
        End Property
    End Class

    ''' <summary>Value/Text pair para sa mga LookUpEdit/ComboBox sa pay cycle screens.</summary>
    Public Class PayCycleChoice
        Public Property Value As Integer
        Public Property Text As String
    End Class

    Public Module PayCycleChoices

        Public Const EndOfMonthDay As Integer = 31

        Public Function DayChoices() As List(Of PayCycleChoice)
            Dim list As New List(Of PayCycleChoice)()
            For d = 1 To 30
                list.Add(New PayCycleChoice With {.Value = d, .Text = d.ToString()})
            Next
            list.Add(New PayCycleChoice With {.Value = EndOfMonthDay, .Text = "EOM"})
            Return list
        End Function

        Public Function MonthOffsetChoices() As List(Of PayCycleChoice)
            Return New List(Of PayCycleChoice) From {
                New PayCycleChoice With {.Value = -1, .Text = "Previous month"},
                New PayCycleChoice With {.Value = 0, .Text = "This month"},
                New PayCycleChoice With {.Value = 1, .Text = "Next month"}
            }
        End Function

        Public Function RateBasisChoices() As List(Of PayCycleChoice)
            Return New List(Of PayCycleChoice) From {
                New PayCycleChoice With {.Value = CInt(PayRateBasis.BasicSalary), .Text = RateBasisText(PayRateBasis.BasicSalary)},
                New PayCycleChoice With {.Value = CInt(PayRateBasis.DailyRate), .Text = RateBasisText(PayRateBasis.DailyRate)}
            }
        End Function

        ''' <summary>
        ''' Iisang spelling ng pay cycle sa buong app: Monthly / SemiMonthly / Weekly / Daily.
        ''' Ang mga lumang record ay maaaring "Semi-Monthly" (may gitling) - ginagawang "SemiMonthly"
        ''' para tumugma sa tblPayCycle, tblCutoff.CycleType at sa pangalan ng tax table (tblIncomeTaxSemiMonthly).
        ''' Ang hindi kilalang pangalan ay ibinabalik nang naka-trim lang.
        ''' </summary>
        Public Function Normalize(name As String) As String
            Dim trimmed = If(name, "").Trim()
            Dim key = trimmed.Replace("-", "").Replace(" ", "").Replace("_", "")

            For Each known In {"Monthly", "SemiMonthly", "Weekly", "Daily"}
                If String.Equals(key, known, StringComparison.OrdinalIgnoreCase) Then Return known
            Next

            Return trimmed
        End Function

        Public Function RateBasisText(basis As PayRateBasis) As String
            Select Case basis
                Case PayRateBasis.DailyRate
                    Return "Daily rate (days worked)"
                Case Else
                    Return "Basic salary (1 month)"
            End Select
        End Function

        Public Function DayText(day As Integer) As String
            Return If(day >= EndOfMonthDay, "EOM", day.ToString())
        End Function

    End Module

End Namespace
