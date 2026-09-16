Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace PayrollSettings.Data
    ' Iisang table lang si Loan (tblLoan) - walang Registry indirection,
    ' gaya ng ginawa sa Compensation.
    Public Class LoanRepository
        Inherits BaseRepository(Of LoanModel)
        Implements ILoanRepository

        Private Const TableName As String = "tblLoan"
        Private Const IdColumn As String = "LoanId"

        ' --- READ ---
        Public Async Function GetAllAsync() As Task(Of List(Of LoanModel)) _
            Implements ILoanRepository.GetAllAsync

            Dim sql = $"
                SELECT {IdColumn} AS Id, Code, Description, LoanType,
                       IsActive, CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {TableName}
                ORDER BY Code"

            Return Await MyBase.GetAllAsync(sql)
        End Function

        ' --- DUPLICATE CODE CHECK (used before Insert/Update) ---
        Public Async Function CodeExistsAsync(code As String, excludeId As Integer) As Task(Of Boolean) _
            Implements ILoanRepository.CodeExistsAsync

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
        Public Async Function InsertAsync(item As LoanModel, userName As String) As Task(Of Integer) _
            Implements ILoanRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName} (Code, Description, LoanType, IsActive, CreatedAt, CreatedBy)
                OUTPUT INSERTED.{IdColumn}
                VALUES (@Code, @Description, @LoanType, @IsActive, GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.Code, item.Description, item.LoanType, item.IsActive, userName
                })
            End Using
        End Function

        ' --- UPDATE ---
        Public Async Function UpdateAsync(item As LoanModel, userName As String) As Task(Of Boolean) _
            Implements ILoanRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET Code = @Code,
                    Description = @Description,
                    LoanType = @LoanType,
                    IsActive = @IsActive,
                    UpdatedAt = GETDATE(),
                    UpdatedBy = @UserName
                WHERE {IdColumn} = @Id"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.Id, item.Code, item.Description, item.LoanType, item.IsActive, userName
            })
            Return rows > 0
        End Function

        ' --- SOFT DELETE / REACTIVATE ---
        Public Async Function SetActiveStatusAsync(id As Integer, isActive As Boolean, userName As String) As Task(Of Boolean) _
            Implements ILoanRepository.SetActiveStatusAsync

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
