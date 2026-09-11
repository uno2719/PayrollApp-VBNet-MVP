Namespace GlobalShared.Constants

    Public Class IncomeTaxTableInfo
        Public Property TableName As String
        Public Property IdColumn As String
    End Class

    ''' <summary>
    ''' Whitelist ng 5 Income Tax tables (Yearly/Monthly/Semi-Monthly/
    ''' Weekly/Daily). Magkapareho ang ibang column names sa lima
    ''' (SalaryFrom, TaxPercentage, atbp) - ang PK column lang ang
    ''' nag-iiba per table, kaya IdColumn lang ang kailangang i-map
    ''' dito, gaya ng StatutorySettingsTableRegistry.
    ''' </summary>
    Public NotInheritable Class IncomeTaxTableRegistry

        Public Shared ReadOnly Tables As New Dictionary(Of String, IncomeTaxTableInfo) From {
            {"tblIncomeTaxYearly", New IncomeTaxTableInfo With {.TableName = "tblIncomeTaxYearly", .IdColumn = "IncomeTaxYearlyId"}},
            {"tblIncomeTaxMonthly", New IncomeTaxTableInfo With {.TableName = "tblIncomeTaxMonthly", .IdColumn = "IncomeTaxMonthlyId"}},
            {"tblIncomeTaxSemiMonthly", New IncomeTaxTableInfo With {.TableName = "tblIncomeTaxSemiMonthly", .IdColumn = "IncomeTaxSemiMonthlyId"}},
            {"tblIncomeTaxWeekly", New IncomeTaxTableInfo With {.TableName = "tblIncomeTaxWeekly", .IdColumn = "IncomeTaxWeeklyId"}},
            {"tblIncomeTaxDaily", New IncomeTaxTableInfo With {.TableName = "tblIncomeTaxDaily", .IdColumn = "IncomeTaxDailyId"}}
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetInfo(tableName As String) As IncomeTaxTableInfo
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid income tax table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace