Namespace GlobalShared.Constants

    Public Class StatutoryTableInfo
        Public Property TableName As String
        Public Property IdColumn As String
    End Class

    ''' <summary>
    ''' Whitelist ng 3 Statutory tables (SSS/PhilHealth/Pag-IBIG).
    ''' Magkapareho ang ibang column names sa tatlo (SalaryFrom, EEShare,
    ''' atbp) - ang PK column lang ang nag-iiba per table, kaya IdColumn
    ''' lang ang kailangang i-map dito (hindi na kailangan ng buong
    ''' Code/Name column mapping gaya ng LookupTableInfo).
    ''' </summary>
    Public NotInheritable Class StatutorySettingsTableRegistry

        Public Shared ReadOnly Tables As New Dictionary(Of String, StatutoryTableInfo) From {
            {"tblStatutorySSS", New StatutoryTableInfo With {.TableName = "tblStatutorySSS", .IdColumn = "SSSId"}},
            {"tblStatutoryPhilHealth", New StatutoryTableInfo With {.TableName = "tblStatutoryPhilHealth", .IdColumn = "PhilHealthId"}},
            {"tblStatutoryPagIbig", New StatutoryTableInfo With {.TableName = "tblStatutoryPagIbig", .IdColumn = "PagIbigId"}}
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetInfo(tableName As String) As StatutoryTableInfo
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid statutory table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace