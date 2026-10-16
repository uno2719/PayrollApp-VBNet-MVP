Namespace GlobalShared.Models

    ''' <summary>
    ''' Mga agency na may buwanang statutory contribution. Ang text value ang naka-save sa
    ''' tblStatutorySchedule.Agency (kaya "PagIbig" na walang gitling; "Pag-IBIG" ay pang-display lang).
    ''' </summary>
    Public NotInheritable Class StatutoryAgency
        Public Const SSS As String = "SSS"
        Public Const PhilHealth As String = "PhilHealth"
        Public Const PagIbig As String = "PagIbig"

        ''' <summary>Sa ganitong pagkakasunod lumalabas sa Deduction Schedule.</summary>
        Public Shared ReadOnly All As String() = {SSS, PhilHealth, PagIbig}

        Public Shared Function DisplayName(agency As String) As String
            If String.Equals(agency, PagIbig, StringComparison.OrdinalIgnoreCase) Then Return "Pag-IBIG"
            Return agency
        End Function
    End Class

    ''' <summary>
    ''' Ano ang gagawin kapag may Pag-IBIG voluntary amount ang employee (Employee > Statutory).
    ''' Isang setting para sa lahat ng pay cycle. Ang DEFAULT ay ReplaceTable: ang voluntary amount ang
    ''' kinakaltas imbes na ang table amount (ang employer share ay sumusunod pa rin sa table).
    ''' </summary>
    Public Enum PagIbigVoluntaryMode As Byte
        HigherOfTableOrVoluntary = 0
        ReplaceTable = 1
        AddOnTop = 2
    End Enum

    Public Module PagIbigVoluntaryModes

        Public Function All() As List(Of PagIbigVoluntaryMode)
            ' Ang default (Replace) ang nasa itaas ng dropdown. Ang pagkakasunod dito ay pang-display lang;
            ' ang numero ng enum ang naka-save sa database.
            Return New List(Of PagIbigVoluntaryMode) From {
                PagIbigVoluntaryMode.ReplaceTable,
                PagIbigVoluntaryMode.HigherOfTableOrVoluntary,
                PagIbigVoluntaryMode.AddOnTop
            }
        End Function

        Public Function Text(mode As PagIbigVoluntaryMode) As String
            Select Case mode
                Case PagIbigVoluntaryMode.ReplaceTable
                    Return "Replace the table amount with the voluntary amount"
                Case PagIbigVoluntaryMode.AddOnTop
                    Return "Add the voluntary amount on top of the table amount"
                Case Else
                    Return "Use the higher of the table amount or the voluntary amount"
            End Select
        End Function

        Public Function Describe(mode As PagIbigVoluntaryMode) As String
            Select Case mode
                Case PagIbigVoluntaryMode.ReplaceTable
                    Return "Employee share = the voluntary amount (the Pag-IBIG table is ignored for employees who have one)."
                Case PagIbigVoluntaryMode.AddOnTop
                    Return "Employee share = table amount + voluntary amount."
                Case Else
                    Return "Employee share = whichever is higher: the table amount or the voluntary amount."
            End Select
        End Function

    End Module

    ''' <summary>Mabilis na paraan ng pag-set ng shares ng isang agency sa isang pay cycle.</summary>
    Public Enum SchedulePreset
        First           ' 100% sa unang period
        Last            ' 100% sa huling period
        Split           ' pantay-pantay sa lahat ng period
        NotDeducted     ' 0% sa lahat (hindi kinakaltas)
    End Enum

    ''' <summary>
    ''' Isang row ng tblStatutorySchedule: kung ilang % ng BUWANANG contribution ng isang agency
    ''' ang kinakaltas sa isang period ng isang pay cycle. Halimbawa SemiMonthly / SSS / Period 2 / 100.
    ''' </summary>
    Public Class StatutoryScheduleModel
        Public Property ScheduleId As Integer
        Public Property PayCycleType As String
        Public Property Agency As String
        Public Property PeriodNo As Integer
        Public Property SharePercent As Decimal

        ' Pag-IBIG rows lang ang may laman (PagIbigVoluntaryMode bilang numero).
        Public Property VoluntaryMode As Byte?

        ' Audit
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class

End Namespace
