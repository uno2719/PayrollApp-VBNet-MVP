Imports System.IO
Imports DevExpress.XtraPrinting
Imports Payroll.GlobalShared.Helpers
Imports Payroll.GlobalShared.Models

Namespace Payslips.Services

    ''' <summary>PDF ng payslip: pangalan ng file, password, at pag-export (isa o lahat).</summary>
    Public Module PayslipPdf

        ''' <summary>Hal. "Payslip_PS-0012_DelaCruzJuan_2026-11-30.pdf".</summary>
        Public Function FileNameFor(doc As PayslipDocument) As String
            Dim name = SharedFileStorage.SanitizeFileName(If(doc.EmployeeName, "").Replace(",", "").Replace(" ", ""))
            Dim no = SharedFileStorage.SanitizeFileName(If(doc.EmployeeNo, doc.RecordId.ToString()))
            Return $"Payslip_{no}_{name}_{doc.PayDate:yyyy-MM-dd}.pdf"
        End Function

        ''' <summary>Birthdate (MMDDYYYY) kung meron; kung wala, ang Employee ID.</summary>
        Public Function PasswordFor(doc As PayslipDocument) As String
            If doc.BirthDate.HasValue Then Return doc.BirthDate.Value.ToString("MMddyyyy")
            Return If(doc.EmployeeNo, "").Trim()
        End Function

        Public Function PasswordHintFor(doc As PayslipDocument) As String
            If doc.BirthDate.HasValue Then Return "your birthdate in MMDDYYYY format (e.g. 03152001 for March 15, 2001)"
            Return "your Employee ID"
        End Function

        ''' <summary>Isang PDF lang para sa isang empleyado (pang-email o "PDF per employee").</summary>
        Public Function ExportOne(batch As PayslipBatch, doc As PayslipDocument, Optional password As String = Nothing) As Byte()
            Using rpt = rptPayslip.Create(batch, {doc})
                Dim opts As New PdfExportOptions()
                If Not String.IsNullOrEmpty(password) Then
                    opts.PasswordSecurityOptions.OpenPassword = password
                End If
                Using ms As New MemoryStream()
                    rpt.ExportToPdf(ms, opts)
                    Return ms.ToArray()
                End Using
            End Using
        End Function

        ''' <summary>Lahat ng payslip sa iisang PDF file.</summary>
        Public Sub ExportCombined(batch As PayslipBatch, filePath As String)
            Using rpt = rptPayslip.Create(batch)
                rpt.ExportToPdf(filePath)
            End Using
        End Sub

        ''' <summary>Isang PDF kada empleyado sa isang folder. Ibinabalik ang bilang ng nagawang file.</summary>
        Public Function ExportEach(batch As PayslipBatch, folder As String) As Integer
            Directory.CreateDirectory(folder)
            Dim count = 0
            For Each doc In batch.Documents
                File.WriteAllBytes(Path.Combine(folder, FileNameFor(doc)), ExportOne(batch, doc))
                count += 1
            Next
            Return count
        End Function

    End Module

End Namespace
