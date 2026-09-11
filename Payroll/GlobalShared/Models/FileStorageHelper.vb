' File: GlobalShared/Helpers/FileStorageHelper.vb
Imports System.IO

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' Generic file-upload helper - ginagamit ito ng kahit anong module na
    ''' may image/document upload (Company Logo ngayon, Employee Photo at
    ''' 201 Files mamaya). Kinokopya lang ang pinili ng user papunta sa
    ''' isang Uploads\<subFolder> sa loob ng app directory, at ang RELATIVE
    ''' path lang (hindi absolute) ang ibinabalik para i-save sa DB - para
    ''' portable kahit magbago ang install location, at para madali na lang
    ''' palitan ng UNC path sa RootFolder balang araw (kapag na-configure na
    ''' sa General > Others, gaya ng "Logo/Avatar Repository" ng C1Pay).
    ''' </summary>
    Public Module FileStorageHelper

        Private ReadOnly RootFolder As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uploads")

        ' --- SAVE (copy mula sa OpenFileDialog papunta sa Uploads\subFolder) ---
        Public Function SaveFile(sourceFilePath As String, subFolder As String, fileNamePrefix As String) As String
            Dim targetFolder = Path.Combine(RootFolder, subFolder)
            Directory.CreateDirectory(targetFolder)

            Dim ext = Path.GetExtension(sourceFilePath)
            Dim fileName = $"{fileNamePrefix}_{DateTime.Now:yyyyMMddHHmmss}{ext}"
            Dim targetPath = Path.Combine(targetFolder, fileName)

            File.Copy(sourceFilePath, targetPath, overwrite:=True)

            ' Relative path lang ang i-store sa DB (hal. "CompanyLogo\CompanyLogo_20260911153000.png")
            Return Path.Combine(subFolder, fileName)
        End Function

        ' --- RESOLVE (relative path mula DB -> full path para i-load sa PictureBox) ---
        Public Function GetFullPath(relativePath As String) As String
            If String.IsNullOrWhiteSpace(relativePath) Then Return String.Empty
            Return Path.Combine(RootFolder, relativePath)
        End Function

        ' --- DELETE (kapag pinalitan ng bagong logo/photo ang luma) ---
        Public Sub DeleteFile(relativePath As String)
            If String.IsNullOrWhiteSpace(relativePath) Then Return
            Dim fullPath = GetFullPath(relativePath)
            If File.Exists(fullPath) Then File.Delete(fullPath)
        End Sub

    End Module
End Namespace