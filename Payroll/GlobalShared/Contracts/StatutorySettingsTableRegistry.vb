Namespace GlobalShared.Constants

    ''' <summary>
    ''' Whitelist ng 3 Statutory tables (SSS/PhilHealth/Pag-IBIG). Hindi
    ''' na kailangan ng per-column mapping gaya ng LookupTableRegistry
    ''' dahil magkapareho ang column names sa tatlo (SalaryFrom, SalaryTo,
    ''' EEShare, atbp) - iba lang ang laman.
    '''
    ''' PAALALA: i-verify/i-adjust ang mga TableName dito laban sa
    ''' aktwal na pangalan ng tables mo sa DB - inassume ko lang muna
    ''' itong pattern, wala akong access doon.
    ''' </summary>
    Public NotInheritable Class StatutorySettingsTableRegistry

        Public Shared ReadOnly Tables As New Dictionary(Of String, String) From {
            {"tblStatutorySSS", "tblStatutorySSS"},
            {"tblStatutoryPhilHealth", "tblStatutoryPhilHealth"},
            {"tblStatutoryPagIbig", "tblStatutoryPagIbig"}
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetTableName(tableName As String) As String
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid statutory table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace