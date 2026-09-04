Namespace GlobalShared.Models

    ''' <summary>
    ''' A single row from any lookup table (Branch, Department, Position, etc).
    ''' Used both for read-only dropdown binding (Employee module — only
    ''' Id/Code/Name get populated there) and for the Settings > Master Data
    ''' CRUD screens (which also use IsActive + the audit fields below).
    ''' </summary>
    Public Class LookupModel
        Public Property Id As Integer
        Public Property Code As String
        Public Property Name As String
        Public Property IsActive As Boolean = True

        ' Audit — only populated/used by the Master Data CRUD screens
        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String
    End Class
End Namespace