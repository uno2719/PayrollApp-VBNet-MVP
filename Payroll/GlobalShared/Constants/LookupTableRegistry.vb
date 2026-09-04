Namespace GlobalShared.Constants

    ''' <summary>
    ''' Describes one lookup table's real column names, so callers never
    ''' hand-write SQL identifiers. Add a new lookup table here ONLY —
    ''' every reader (Employee dropdowns, Lookups CRUD, future modules)
    ''' shares this single list.
    ''' </summary>
    Public Class LookupTableInfo
        Public Property TableName As String
        Public Property IdColumn As String
        Public Property CodeColumn As String
        Public Property NameColumn As String
    End Class

    Public NotInheritable Class LookupTableRegistry

        ' NOTE: tblLeaveGroup is registered here (still readable for the
        ' Employee > Employment dropdown) but is intentionally NOT wired
        ' into the new Lookups/Master Data maintenance tabs — Leave gets
        ' its own future module (Group/Type/Rule/Holiday/Reason), matching
        ' the existing aceSettings_Leave nav element.
        Public Shared ReadOnly Tables As New Dictionary(Of String, LookupTableInfo) From {
            {"tblBranch", New LookupTableInfo With {.TableName = "tblBranch", .IdColumn = "BranchId", .CodeColumn = "BranchCode", .NameColumn = "BranchName"}},
            {"tblDepartment", New LookupTableInfo With {.TableName = "tblDepartment", .IdColumn = "DepartmentId", .CodeColumn = "DepartmentCode", .NameColumn = "DepartmentName"}},
            {"tblPosition", New LookupTableInfo With {.TableName = "tblPosition", .IdColumn = "PositionId", .CodeColumn = "PositionCode", .NameColumn = "PositionName"}},
            {"tblCategoryCode", New LookupTableInfo With {.TableName = "tblCategoryCode", .IdColumn = "CategoryId", .CodeColumn = "CategoryCode", .NameColumn = "CategoryName"}},
            {"tblJobClass", New LookupTableInfo With {.TableName = "tblJobClass", .IdColumn = "JobClassId", .CodeColumn = "JobClassCode", .NameColumn = "JobClassName"}},
            {"tblLeaveGroup", New LookupTableInfo With {.TableName = "tblLeaveGroup", .IdColumn = "LeaveGroupId", .CodeColumn = "LeaveGroupCode", .NameColumn = "LeaveGroupName"}},
            {"tblHolidayGroup", New LookupTableInfo With {.TableName = "tblHolidayGroup", .IdColumn = "HolidayGroupId", .CodeColumn = "HolidayGroupCode", .NameColumn = "HolidayGroupName"}},
            {"tblScheduleGroup", New LookupTableInfo With {.TableName = "tblScheduleGroup", .IdColumn = "ScheduleGroupId", .CodeColumn = "ScheduleGroupCode", .NameColumn = "ScheduleGroupName"}},
            {"tblBank", New LookupTableInfo With {.TableName = "tblBank", .IdColumn = "BankId", .CodeColumn = "BankCode", .NameColumn = "BankName"}}
        }

        ''' <summary>Tables actually wired into the Settings > Master Data tabs (excludes Leave).</summary>
        Public Shared ReadOnly MaintainedTables As String() = {
            "tblBranch", "tblDepartment", "tblPosition", "tblCategoryCode",
            "tblJobClass", "tblHolidayGroup", "tblScheduleGroup", "tblBank"
        }

        Public Shared Function IsAllowed(tableName As String) As Boolean
            Return Tables.ContainsKey(tableName)
        End Function

        Public Shared Function GetInfo(tableName As String) As LookupTableInfo
            If Not IsAllowed(tableName) Then
                Throw New ArgumentException($"Invalid lookup table: {tableName}")
            End If
            Return Tables(tableName)
        End Function

    End Class
End Namespace