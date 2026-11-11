' File: GlobalShared/Helpers/SharedFileStorage.vb
' ============================================================
' Pang-SHARED na file storage (Employee 201 Files at Photo). Iba ito sa FileStorageHelper (Company Logo):
' ang FileStorageHelper ay nagse-save sa Uploads\ sa loob ng app folder ng BAWAT PC, kaya hindi makikita ng
' ibang user ang file. Dito, ang ROOT ay shared folder (UNC path) na itinakda sa Settings > General Settings
' (tblGeneralSettings.FileStorageRoot), at RELATIVE path lang ang iniimbak sa database.
'
' Istruktura sa loob ng root:
'     Employees\<RecordId 6 digits>\201Files\<yyyyMMddHHmmssfff>_<pangalan ng file>
'     Employees\<RecordId 6 digits>\Photo\photo_<yyyyMMddHHmmssfff>.jpg
'     CompanyLogo\CompanyLogo_<yyyyMMddHHmmss>.<ext>      (parehong anyo ng lumang FileStorageHelper)
' ISANG root lang para sa lahat (isang setting, isang backup, isang permission sa folder).
' RecordId ang ginagamit (hindi EmployeeNo) para hindi masira ang path kapag pinalitan ang EmployeeNo.
' ============================================================
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO

Namespace GlobalShared.Helpers

    Public Module SharedFileStorage

        Public ReadOnly AllowedDocumentExtensions As String() = {".pdf", ".jpg", ".jpeg", ".png", ".docx"}
        Public ReadOnly AllowedPhotoExtensions As String() = {".jpg", ".jpeg", ".png"}

        ''' <summary>Pinakamalaking sukat ng isang 201 file (10 MB).</summary>
        Public Const MaxDocumentBytes As Long = 10L * 1024L * 1024L

        ''' <summary>Ang photo ay ire-resize para ang pinakamahabang gilid ay hindi lalampas dito (pixels).</summary>
        Public Const PhotoMaxSide As Integer = 400

        ' ---------- ROOT ----------

        ''' <summary>
        ''' Empty string = magagamit ang root mula sa PC na ito (umiiral at nasusulatan). Kung hindi, ang mensahe
        ''' para sa user. Nagsusulat/nagbubura ito ng maliit na test file sa root.
        ''' </summary>
        Public Function ValidateRoot(root As String) As String
            If String.IsNullOrWhiteSpace(root) Then
                Return "The Files Folder is not set. An administrator must set it in Settings > General Settings."
            End If

            Dim r As String = root.Trim()
            If Not Path.IsPathRooted(r) Then
                Return "The Files Folder must be a full path, such as \\server\share\PayrollFiles."
            End If

            Try
                If Not Directory.Exists(r) Then
                    Return $"This PC can't reach the Files Folder: {r}"
                End If

                Dim probe As String = Path.Combine(r, $".write_test_{Guid.NewGuid():N}.tmp")
                File.WriteAllText(probe, "ok")
                File.Delete(probe)
                Return String.Empty
            Catch ex As Exception
                Return $"This PC can't write to the Files Folder ({ex.Message})"
            End Try
        End Function

        ' ---------- PATHS ----------

        Private Function EmployeeFolder(recordId As Integer) As String
            Return Path.Combine("Employees", recordId.ToString("D6"))
        End Function

        Public Function GetFullPath(root As String, relativePath As String) As String
            If String.IsNullOrWhiteSpace(root) OrElse String.IsNullOrWhiteSpace(relativePath) Then Return String.Empty
            Return Path.Combine(root.Trim(), relativePath)
        End Function

        Public Function SanitizeFileName(name As String) As String
            Dim cleaned As String = If(name, String.Empty)
            For Each c As Char In Path.GetInvalidFileNameChars()
                cleaned = cleaned.Replace(c, "_"c)
            Next
            cleaned = cleaned.Trim()
            If cleaned.Length = 0 Then cleaned = "file"

            ' Iwasan ang sobrang habang path (lumang limit na 260 characters sa Windows)
            If cleaned.Length > 100 Then
                Dim ext As String = Path.GetExtension(cleaned)
                If ext.Length > 10 Then ext = String.Empty
                cleaned = cleaned.Substring(0, 100 - ext.Length) & ext
            End If
            Return cleaned
        End Function

        ' ---------- VALIDATION ----------

        ''' <summary>Empty string = puwede. Sinusuri ang uri (extension), pag-iral, at sukat ng 201 file.</summary>
        Public Function ValidateDocumentFile(filePath As String) As String
            If String.IsNullOrWhiteSpace(filePath) OrElse Not File.Exists(filePath) Then
                Return "the file can't be found."
            End If

            Dim ext As String = Path.GetExtension(filePath).ToLowerInvariant()
            If Not AllowedDocumentExtensions.Contains(ext) Then
                Return $"file type '{ext}' is not allowed (allowed: {String.Join(", ", AllowedDocumentExtensions)})."
            End If

            Dim size As Long = New FileInfo(filePath).Length
            If size <= 0 Then Return "the file is empty."
            If size > MaxDocumentBytes Then
                Return $"the file is too large ({size / 1048576.0:N1} MB; maximum is {MaxDocumentBytes \ 1048576L} MB)."
            End If

            Return String.Empty
        End Function

        ' ---------- SAVE / DELETE / LOAD ----------

        ''' <summary>Kinokopya ang file sa root; ibinabalik ang RELATIVE path na ise-save sa database.</summary>
        Public Function SaveDocument(root As String, recordId As Integer, sourceFilePath As String) As String
            Dim folder As String = Path.Combine(EmployeeFolder(recordId), "201Files")
            Dim targetFolder As String = Path.Combine(root.Trim(), folder)
            Directory.CreateDirectory(targetFolder)

            Dim storedName As String = $"{DateTime.Now:yyyyMMddHHmmssfff}_{SanitizeFileName(Path.GetFileName(sourceFilePath))}"
            File.Copy(sourceFilePath, Path.Combine(targetFolder, storedName), overwrite:=False)

            Return Path.Combine(folder, storedName)
        End Function

        ''' <summary>
        ''' Ire-resize ang larawan (max PhotoMaxSide sa pinakamahabang gilid) at ise-save bilang JPEG (quality 85).
        ''' Ibinabalik ang RELATIVE path. (Hindi inaayos ang EXIF rotation ng larawan galing sa cellphone.)
        ''' </summary>
        Public Function SavePhoto(root As String, recordId As Integer, sourceImagePath As String) As String
            Dim folder As String = Path.Combine(EmployeeFolder(recordId), "Photo")
            Dim targetFolder As String = Path.Combine(root.Trim(), folder)
            Directory.CreateDirectory(targetFolder)

            Dim storedName As String = $"photo_{DateTime.Now:yyyyMMddHHmmssfff}.jpg"
            Dim targetPath As String = Path.Combine(targetFolder, storedName)

            ' Basahin muna sa memory para hindi ma-lock ang source file.
            Using ms As New MemoryStream(File.ReadAllBytes(sourceImagePath))
                Using src As Image = Image.FromStream(ms)
                    Dim scale As Double = Math.Min(1.0R, PhotoMaxSide / CDbl(Math.Max(src.Width, src.Height)))
                    Dim w As Integer = Math.Max(1, CInt(Math.Round(src.Width * scale)))
                    Dim h As Integer = Math.Max(1, CInt(Math.Round(src.Height * scale)))

                    Using bmp As New Bitmap(w, h)
                        Using g As Graphics = Graphics.FromImage(bmp)
                            g.Clear(Color.White)
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic
                            g.SmoothingMode = SmoothingMode.HighQuality
                            g.DrawImage(src, 0, 0, w, h)
                        End Using

                        Dim jpegCodec As ImageCodecInfo =
                            ImageCodecInfo.GetImageEncoders().First(Function(c) c.FormatID = ImageFormat.Jpeg.Guid)
                        Using ep As New EncoderParameters(1)
                            ep.Param(0) = New EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 85L)
                            bmp.Save(targetPath, jpegCodec, ep)
                        End Using
                    End Using
                End Using
            End Using

            Return Path.Combine(folder, storedName)
        End Function

        ''' <summary>
        ''' Generic na kopya papunta sa root\subFolder (hal. Company Logo). Parehong anyo ng pangalan at ng relative path
        ''' ng lumang FileStorageHelper.SaveFile, kaya VALID PA RIN ang mga relative path na nasa database na.
        ''' </summary>
        Public Function SaveFile(root As String, sourceFilePath As String, subFolder As String, fileNamePrefix As String) As String
            Dim targetFolder As String = Path.Combine(root.Trim(), subFolder)
            Directory.CreateDirectory(targetFolder)

            Dim fileName As String = $"{fileNamePrefix}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(sourceFilePath)}"
            File.Copy(sourceFilePath, Path.Combine(targetFolder, fileName), overwrite:=True)

            Return Path.Combine(subFolder, fileName)
        End Function

        Public Sub DeleteFile(root As String, relativePath As String)
            Dim full As String = GetFullPath(root, relativePath)
            If full.Length > 0 AndAlso File.Exists(full) Then File.Delete(full)
        End Sub

        ''' <summary>Nilo-load ang larawan sa memory (hindi nila-lock ang file). Nothing kung wala ang file.</summary>
        Public Function LoadImage(root As String, relativePath As String) As Image
            Dim full As String = GetFullPath(root, relativePath)
            If full.Length = 0 OrElse Not File.Exists(full) Then Return Nothing

            Using ms As New MemoryStream(File.ReadAllBytes(full))
                Using img As Image = Image.FromStream(ms)
                    Return New Bitmap(img)
                End Using
            End Using
        End Function

    End Module

End Namespace
