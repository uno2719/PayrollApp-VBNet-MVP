Namespace GlobalShared.Constants

    Public Class PayrollFlaggedEntryTableInfo
        Public Property TableName As String
        Public Property IdColumn As String
    End Class

    ''' <summary>
    ''' Whitelist ng 2 tables na gumagamit ng PayrollFlaggedEntryModel
    ''' shape (Deduction/Bonus). Magkapareho ang column names sa dalawa -
    ''' ang PK column lang ang nag-iiba per table, kaya IdColumn lang ang
    ''' kailangang i-map dito (parehong pattern gaya ng
    ''' StatutorySettingsTableRegistry).
    ''' </summary>
    Public NotInheritable Class PayrollFlaggedEntryTableRegistry

        Public Shared ReadOnly Tables As New Dictionary(Of String, PayrollFlaggedEntryTableInfo) From {
            {"tblDeduction", New PayrollFlaggedEntryTableInfo With {.TableName = "tblDeduction", .IdColumn = "DeductionId"}},
            {"tblBonus", New PayrollFlaggedEntryTableInfo With {.TableName = "tblBonus", .IdColumn = "BonusId"}}
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetInfo(tableName As String) As PayrollFlaggedEntryTableInfo
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid payroll flagged-entry table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace
