' File: Modules/Settings/AppSettings/EmailSettings/Data/EmailSettingsRepository.vb
Imports Dapper
Imports Payroll.GlobalShared.Base
Imports Payroll.GlobalShared.Models

Namespace EmailSettings.Data

    ''' <summary>
    ''' Singleton-row repository, kaparehong hugis ng
    ''' GeneralSettingsRepository: TOP 1 na SELECT, walang CodeExists o
    ''' SetActiveStatus dahil hindi ito CRUD list.
    '''
    ''' Ang naka-store sa SmtpPasswordEncrypted ay encrypted na - ang
    ''' Service ang nag-e-encrypt bago tumawag dito. Walang alam ang
    ''' repository tungkol sa encryption.
    ''' </summary>
    Public Class EmailSettingsRepository
        Inherits BaseRepository(Of EmailSettingsModel)
        Implements IEmailSettingsRepository

        Private Const TableName As String = "tblEmailSettings"

        Public Async Function GetAsync() As Task(Of EmailSettingsModel) _
            Implements IEmailSettingsRepository.GetAsync

            Dim sql = $"
                SELECT TOP 1
                       EmailSettingsId,
                       SmtpHost, SmtpPort, SmtpUsername,
                       SmtpPasswordEncrypted, UseSsl,
                       CreatedAt, CreatedBy, UpdatedAt, UpdatedBy
                FROM {TableName}
                ORDER BY EmailSettingsId"

            Using conn = GetConnection()
                Return Await conn.QuerySingleOrDefaultAsync(Of EmailSettingsModel)(sql)
            End Using
        End Function

        Public Async Function InsertAsync(item As EmailSettingsModel, userName As String) As Task(Of Integer) _
            Implements IEmailSettingsRepository.InsertAsync

            Dim sql = $"
                INSERT INTO {TableName}
                    (SmtpHost, SmtpPort, SmtpUsername,
                     SmtpPasswordEncrypted, UseSsl,
                     CreatedAt, CreatedBy)
                OUTPUT INSERTED.EmailSettingsId
                VALUES
                    (@SmtpHost, @SmtpPort, @SmtpUsername,
                     @SmtpPasswordEncrypted, @UseSsl,
                     GETDATE(), @UserName)"

            Using conn = GetConnection()
                Return Await conn.ExecuteScalarAsync(Of Integer)(sql, New With {
                    item.SmtpHost, item.SmtpPort, item.SmtpUsername,
                    item.SmtpPasswordEncrypted, item.UseSsl,
                    userName
                })
            End Using
        End Function

        Public Async Function UpdateAsync(item As EmailSettingsModel, userName As String) As Task(Of Boolean) _
            Implements IEmailSettingsRepository.UpdateAsync

            Dim sql = $"
                UPDATE {TableName}
                SET SmtpHost              = @SmtpHost,
                    SmtpPort              = @SmtpPort,
                    SmtpUsername          = @SmtpUsername,
                    SmtpPasswordEncrypted = @SmtpPasswordEncrypted,
                    UseSsl                = @UseSsl,
                    UpdatedAt             = GETDATE(),
                    UpdatedBy             = @UserName
                WHERE EmailSettingsId = @EmailSettingsId"

            Dim rows = Await MyBase.ExecuteAsync(sql, New With {
                item.EmailSettingsId,
                item.SmtpHost, item.SmtpPort, item.SmtpUsername,
                item.SmtpPasswordEncrypted, item.UseSsl,
                userName
            })
            Return rows > 0
        End Function

    End Class
End Namespace
