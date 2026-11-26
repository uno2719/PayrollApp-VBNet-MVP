Imports System.Net
Imports System.Net.Mail
Imports System.Threading
Imports Payroll.EmailSettings.Services
Imports Payroll.GlobalShared.Models
Imports Payroll.Payslips.Data

Namespace Payslips.Services

    Public Class PayslipService
        Implements IPayslipService

        Private ReadOnly _repo As IPayslipRepository
        Private ReadOnly _emailSettings As IEmailSettingsService

        Public Sub New(repo As IPayslipRepository, emailService As IEmailSettingsService)
            _repo = repo
            _emailSettings = emailService
        End Sub

        Public Function GetCutoffsAsync() As Task(Of List(Of CutoffModel)) Implements IPayslipService.GetCutoffsAsync
            Return _repo.GetCutoffsAsync()
        End Function

        Public Function GetEmployeeRowsAsync(cutoffId As Integer) As Task(Of List(Of PayslipEmployeeRow)) _
            Implements IPayslipService.GetEmployeeRowsAsync
            Return _repo.GetEmployeeRowsAsync(cutoffId)
        End Function

        Public Async Function BuildBatchAsync(cutoffId As Integer, recordIds As IList(Of Integer), options As PayslipOptions) As Task(Of PayslipBatch) _
            Implements IPayslipService.BuildBatchAsync

            Dim src = Await _repo.LoadSourceAsync(cutoffId, recordIds)
            Return PayslipDocumentBuilder.Build(src, If(options, New PayslipOptions()))
        End Function

        Public Async Function CheckEmailReadyAsync() As Task(Of String) Implements IPayslipService.CheckEmailReadyAsync
            Dim s = Await _emailSettings.GetAsync()
            If s Is Nothing OrElse String.IsNullOrWhiteSpace(s.SmtpHost) Then
                Return "Email Settings are not set up yet. Go to Settings > Email Settings, enter the SMTP account, and send a test email first."
            End If
            If String.IsNullOrWhiteSpace(s.SmtpUsername) OrElse Not IsValidEmail(s.SmtpUsername) Then
                Return "The SMTP Username in Settings > Email Settings must be a valid email address (it is used as the sender)."
            End If
            If String.IsNullOrEmpty(_emailSettings.GetDecryptedPassword(s)) Then
                Return "The SMTP password in Settings > Email Settings is missing or can't be read. Type it again and Save."
            End If
            Return Nothing
        End Function

        Public Async Function SendEmailsAsync(batch As PayslipBatch, options As PayslipOptions, userName As String,
                                              progress As IProgress(Of PayslipEmailProgress), ct As CancellationToken) As Task(Of List(Of PayslipEmailResult)) _
            Implements IPayslipService.SendEmailsAsync

            Dim results As New List(Of PayslipEmailResult)()
            Dim notReady = Await CheckEmailReadyAsync()
            If notReady IsNot Nothing Then Throw New InvalidOperationException(notReady)

            Dim settings = Await _emailSettings.GetAsync()
            Dim password = _emailSettings.GetDecryptedPassword(settings)
            Dim senderName = If(String.IsNullOrWhiteSpace(batch.Company?.CompanyName), "Payroll", batch.Company.CompanyName.Trim())
            Dim sender As New MailAddress(settings.SmtpUsername.Trim(), senderName)

            Dim total = batch.Documents.Count
            Dim done = 0

            Using client As New SmtpClient(settings.SmtpHost, settings.SmtpPort)
                client.EnableSsl = settings.UseSsl
                client.Credentials = New NetworkCredential(settings.SmtpUsername, password)

                For Each doc In batch.Documents
                    If ct.IsCancellationRequested Then Exit For
                    progress?.Report(New PayslipEmailProgress With {.Done = done, .Total = total, .CurrentName = doc.EmployeeName})

                    Dim r As New PayslipEmailResult With {
                        .RecordId = doc.RecordId, .EmployeeName = doc.EmployeeName, .EmailAddress = If(doc.EmailAddress, "").Trim()}

                    If Not IsValidEmail(r.EmailAddress) Then
                        r.Success = False
                        r.Message = If(String.IsNullOrWhiteSpace(r.EmailAddress), "No email address (Employee > Personal Info).",
                                       "Invalid email address: " & r.EmailAddress)
                        ' Walang email = hindi na nilo-log (walang sinubukang padala). Makikita pa rin sa summary.
                    Else
                        Try
                            Dim pdfPassword = If(options.ProtectPdf, PayslipPdf.PasswordFor(doc), Nothing)
                            Dim pdf = Await Task.Run(Function() PayslipPdf.ExportOne(batch, doc, pdfPassword))

                            Using ms As New IO.MemoryStream(pdf)
                                Using msg As New MailMessage()
                                    msg.From = sender
                                    msg.To.Add(New MailAddress(r.EmailAddress, doc.EmployeeName))
                                    msg.Subject = $"Payslip - {doc.PayPeriod} (Pay date {doc.PayDateText})"
                                    msg.Body = BuildBody(doc, options, senderName)
                                    msg.IsBodyHtml = False
                                    msg.Attachments.Add(New Attachment(ms, PayslipPdf.FileNameFor(doc), "application/pdf"))
                                    Await client.SendMailAsync(msg)
                                End Using
                            End Using

                            r.Success = True
                            r.Message = "Sent"
                        Catch ex As SmtpException
                            r.Success = False
                            r.Message = "SMTP error: " & ex.Message
                        Catch ex As Exception
                            r.Success = False
                            r.Message = ex.Message
                        End Try

                        Try
                            Await _repo.InsertEmailLogAsync(New PayslipEmailLogEntry With {
                                .CutoffID = doc.CutoffID, .RecordId = doc.RecordId, .EmailAddress = r.EmailAddress,
                                .IsSuccess = r.Success, .ErrorMessage = If(r.Success, Nothing, r.Message),
                                .IncludeAllowance = options.IncludeAllowance, .SentBy = userName})
                        Catch ex As Exception
                            ' Hindi dapat ihinto ang buong padala dahil lang hindi na-log; idinudugtong sa mensahe.
                            r.Message &= " (log not saved: " & ex.Message & ")"
                        End Try
                    End If

                    results.Add(r)
                    done += 1
                    progress?.Report(New PayslipEmailProgress With {.Done = done, .Total = total, .CurrentName = doc.EmployeeName})
                Next
            End Using

            Return results
        End Function

        Private Shared Function BuildBody(doc As PayslipDocument, options As PayslipOptions, companyName As String) As String
            Dim nl = Environment.NewLine
            Dim sb As New System.Text.StringBuilder()
            sb.Append("Hi ").Append(doc.EmployeeName).Append(",").Append(nl).Append(nl)
            sb.Append("Attached is your payslip for the pay period ").Append(doc.PayPeriod).
               Append(" (pay date ").Append(doc.PayDateText).Append(").").Append(nl)
            If options.ProtectPdf Then
                sb.Append(nl).Append("The PDF is password-protected. To open it, use ").Append(PayslipPdf.PasswordHintFor(doc)).Append(".").Append(nl)
            End If
            sb.Append(nl).Append("For questions about your payslip, please contact HR / Accounting.").Append(nl)
            sb.Append("This is a system-generated email. Please do not reply.").Append(nl).Append(nl)
            sb.Append(companyName)
            Return sb.ToString()
        End Function

        Private Shared Function IsValidEmail(value As String) As Boolean
            If String.IsNullOrWhiteSpace(value) Then Return False
            Try
                Dim parsed As New MailAddress(value.Trim())
                Return String.Equals(parsed.Address, value.Trim(), StringComparison.OrdinalIgnoreCase)
            Catch
                Return False
            End Try
        End Function

    End Class

End Namespace
