Namespace GlobalShared.Constants

    Public Class PayrollRateEntryTableInfo
        Public Property TableName As String
        Public Property IdColumn As String
    End Class

    ''' <summary>
    ''' Whitelist ng 2 tables na gumagamit ng PayrollRateEntryModel shape
    ''' (Overtime/Holiday) - parehong pattern gaya ng
    ''' PayrollFlaggedEntryTableRegistry.
    ''' </summary>
    Public NotInheritable Class PayrollRateEntryTableRegistry

        Public Shared ReadOnly Tables As New Dictionary(Of String, PayrollRateEntryTableInfo) From {
            {"tblOvertime", New PayrollRateEntryTableInfo With {.TableName = "tblOvertime", .IdColumn = "OvertimeId"}},
            {"tblHoliday", New PayrollRateEntryTableInfo With {.TableName = "tblHoliday", .IdColumn = "HolidayId"}}
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetInfo(tableName As String) As PayrollRateEntryTableInfo
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid payroll rate-entry table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace
