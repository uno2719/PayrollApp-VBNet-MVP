<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmProfileMenu
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProfileMenu))
        lblChangePassword = New DevExpress.XtraEditors.LabelControl()
        lblSeparator = New DevExpress.XtraEditors.PanelControl()
        lblLogout = New DevExpress.XtraEditors.LabelControl()
        CType(lblSeparator, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblChangePassword
        ' 
        lblChangePassword.Appearance.Options.UseTextOptions = True
        lblChangePassword.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        lblChangePassword.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblChangePassword.Cursor = Cursors.Hand
        lblChangePassword.Dock = DockStyle.Top
        lblChangePassword.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        lblChangePassword.ImageOptions.SvgImage = CType(resources.GetObject("lblChangePassword.ImageOptions.SvgImage"), DevExpress.Utils.Svg.SvgImage)
        lblChangePassword.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None
        lblChangePassword.ImageOptions.SvgImageSize = New Size(16, 16)
        lblChangePassword.IndentBetweenImageAndText = 10
        lblChangePassword.Location = New Point(0, 0)
        lblChangePassword.Name = "lblChangePassword"
        lblChangePassword.Padding = New Padding(12, 0, 0, 0)
        lblChangePassword.Size = New Size(180, 36)
        lblChangePassword.TabIndex = 0
        lblChangePassword.Text = "Change Password"
        ' 
        ' lblSeparator
        ' 
        lblSeparator.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        lblSeparator.Dock = DockStyle.Top
        lblSeparator.Location = New Point(0, 36)
        lblSeparator.Name = "lblSeparator"
        lblSeparator.Size = New Size(180, 1)
        lblSeparator.TabIndex = 1
        ' 
        ' lblLogout
        ' 
        lblLogout.Appearance.Options.UseTextOptions = True
        lblLogout.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        lblLogout.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblLogout.Cursor = Cursors.Hand
        lblLogout.Dock = DockStyle.Top
        lblLogout.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter
        lblLogout.ImageOptions.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None
        lblLogout.ImageOptions.SvgImageSize = New Size(16, 16)
        lblLogout.IndentBetweenImageAndText = 10
        lblLogout.Location = New Point(0, 37)
        lblLogout.Name = "lblLogout"
        lblLogout.Padding = New Padding(12, 0, 0, 0)
        lblLogout.Size = New Size(180, 36)
        lblLogout.TabIndex = 2
        lblLogout.Text = "Logout"
        ' 
        ' frmProfileMenu
        ' 
        AutoScaleMode = AutoScaleMode.None
        ClientSize = New Size(180, 73)
        Controls.Add(lblLogout)
        Controls.Add(lblSeparator)
        Controls.Add(lblChangePassword)
        FormBorderStyle = FormBorderStyle.None
        Name = "frmProfileMenu"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.Manual
        CType(lblSeparator, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    Friend WithEvents lblChangePassword As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSeparator As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblLogout As DevExpress.XtraEditors.LabelControl

End Class