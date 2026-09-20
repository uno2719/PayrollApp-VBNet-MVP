' ============================================================
' Modules/Admin/ModuleManagement/Models/ModuleCatalogModel.vb
' ============================================================
' Ito ang FULL na representasyon ng isang tblModules row - kasama
' na ang mga field na para lang sa CRUD screen na ito (audit trail,
' UsersWithAccessCount).
'
' Sinasadya kong HIWALAY ito sa Users.Models.ModuleInfo (yung
' gamit ng Access Editor at ng dating Users screen) - ang ModuleInfo
' ay sadyang payat, para lang sa dropdown/lookup na pangangailangan.
' Kung pagsasamahin ko sila, mapipilitan akong dalhin ang buong
' audit trail kahit saang lugar gumagamit ng ModuleInfo.
' ============================================================
Namespace ModuleManagement.Models

    Public Class ModuleCatalogModel

        Public Property RecordId As Integer
        Public Property ModuleCode As String
        Public Property ModuleName As String
        Public Property GroupName As String
        Public Property SortOrder As Integer
        Public Property IsActive As Boolean

        ' COMPUTED - ilang user ang may access dito (View o Edit man).
        ' Ginagamit bilang babala bago mag-Delete o mag-Deactivate.
        Public Property UsersWithAccessCount As Integer

        Public Property CreatedAt As DateTime?
        Public Property CreatedBy As String
        Public Property UpdatedAt As DateTime?
        Public Property UpdatedBy As String

        Public ReadOnly Property StatusText As String
            Get
                Return If(IsActive, "Active", "Inactive")
            End Get
        End Property

    End Class

End Namespace