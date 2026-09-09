Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    ' Iisang table lang si Compensation (tblCompensation), kaya walang
    ' TableRegistry indirection dito - direkta na lang Const, gaya nung
    ' ginawa sa Loan. Ang Registry pattern (LookupTableRegistry,
    ' PayrollFlaggedEntryTableRegistry, atbp) ay para lang sa mga
    ' Repository na naka-parameterize sa MAHIGIT SA ISANG table.
    Public Class CompensationRepository
        Inherits BaseRepository(Of CompensationModel)
        Implements ICompensationRepository

        Private Const TableName As String = "tblCompensation"
        Private Const IdColumn As String = "CompensationId"

        ' --- READ ---
        Public Async Function GetAllAsync() As Task(Of List(Of CompensationModel)) _
            Implements ICompensationRepository.GetAllAsync

            Dim sql = $"
                SELECT {IdColumn} AS Id, Code, Description,
                       TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag,
                       Component2316, DeminimisFlag, CeilingAmount, Frequency,
                       IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {TableName}
                ORDER BY Code"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK (used before Insert/Update) ---
        Public Async Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements ICompensationRepository.CodeExistsAsync

            Dim sql = $"
                SELECT COUNT(1)
                FROM {TableName}
                WHERE Code = @Code
                  AND {IdColumn} <> @ExcludeId"

            Using conn = GetConnection()
                Dim count = Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {code, excludeId})
                Return count > 0
            End Using
        End Function

        ' --- INSERT ---
        Public Async Function InsertAsync(item As CompensationModel, userName As String) As Task(Of Integer) _
            Implements ICompensationRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName}
                    (Code, Description, TaxFlag, SSSFlag, PhilHealthFlag, PagIbigFlag,
                     Component2316, DeminimisFlag, CeilingAmount, Frequency,
                     IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{IdColumn}
                VALUES
                    (@Code, @Description, @TaxFlag, @SSSFlag, @PhilHealthFlag, @PagIbigFlag,
                     @Component2316, @DeminimisFlag, @CeilingAmount, @Frequency,
                     @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                    item.Component2316, item.DeminimisFlag, item.CeilingAmount, item.Frequency,
                    item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As CompensationModel, userName As String) As Task(Of Boolean) _
            Implements ICompensationRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET Code = @Code,
                    Description = @Description,
                    TaxFlag = @TaxFlag,
                    SSSFlag = @SSSFlag,
                    PhilHealthFlag = @PhilHealthFlag,
                    PagIbigFlag = @PagIbigFlag,
                    Component2316 = @Component2316,
                    DeminimisFlag = @DeminimisFlag,
                    CeilingAmount = @CeilingAmount,
                    Frequency = @Frequency,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Description, item.TaxFlag, item.SSSFlag, item.PhilHealthFlag, item.PagIbigFlag,
                item.Component2316, item.DeminimisFlag, item.CeilingAmount, item.Frequency,
                item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ICompensationRepository.SetActiveStatusAsync

            Dim sql = $"
                UPDATE {TableName}
                SET IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {id, isActive, userName})
            Return rows > 0
        End Function

    End Class
End Namespace
