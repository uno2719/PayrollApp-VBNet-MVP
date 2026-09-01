<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucPayroll
    Inherits GlobalShared.Base.ucBase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        TabPane1 = New DevExpress.XtraBars.Navigation.TabPane()
        TabNavigationPage1 = New DevExpress.XtraBars.Navigation.TabNavigationPage()
        TextBox1 = New TextBox()
        LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        TabNavigationPage2 = New DevExpress.XtraBars.Navigation.TabNavigationPage()
        LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        TabNavigationPage3 = New DevExpress.XtraBars.Navigation.TabNavigationPage()
        CType(TabPane1, ComponentModel.ISupportInitialize).BeginInit()
        TabPane1.SuspendLayout()
        TabNavigationPage1.SuspendLayout()
        TabNavigationPage2.SuspendLayout()
        SuspendLayout()
        ' 
        ' TabPane1
        ' 
        TabPane1.Appearance.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TabPane1.Appearance.Options.UseFont = True
        TabPane1.Controls.Add(TabNavigationPage1)
        TabPane1.Controls.Add(TabNavigationPage2)
        TabPane1.Controls.Add(TabNavigationPage3)
        TabPane1.Dock = DockStyle.Fill
        TabPane1.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TabPane1.Location = New Point(0, 0)
        TabPane1.Name = "TabPane1"
        TabPane1.PageProperties.AppearanceCaption.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TabPane1.PageProperties.AppearanceCaption.Options.UseFont = True
        TabPane1.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {TabNavigationPage1, TabNavigationPage2, TabNavigationPage3})
        TabPane1.RegularSize = New Size(1293, 722)
        TabPane1.SelectedPage = TabNavigationPage1
        TabPane1.Size = New Size(1293, 722)
        TabPane1.TabIndex = 2
        TabPane1.Text = "TabPane1"
        ' 
        ' TabNavigationPage1
        ' 
        TabNavigationPage1.Caption = "TabNavigationPage1"
        TabNavigationPage1.Controls.Add(TextBox1)
        TabNavigationPage1.Controls.Add(LabelControl1)
        TabNavigationPage1.Name = "TabNavigationPage1"
        TabNavigationPage1.Size = New Size(1293, 689)
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(247, 91)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(214, 21)
        TextBox1.TabIndex = 1
        ' 
        ' LabelControl1
        ' 
        LabelControl1.Location = New Point(106, 81)
        LabelControl1.Name = "LabelControl1"
        LabelControl1.Size = New Size(33, 13)
        LabelControl1.TabIndex = 0
        LabelControl1.Text = "Page 1"
        ' 
        ' TabNavigationPage2
        ' 
        TabNavigationPage2.Caption = "TabNavigationPage2"
        TabNavigationPage2.Controls.Add(LabelControl2)
        TabNavigationPage2.Name = "TabNavigationPage2"
        TabNavigationPage2.Size = New Size(908, 472)
        ' 
        ' LabelControl2
        ' 
        LabelControl2.Location = New Point(383, 174)
        LabelControl2.Name = "LabelControl2"
        LabelControl2.Size = New Size(33, 13)
        LabelControl2.TabIndex = 1
        LabelControl2.Text = "Page 2"
        ' 
        ' TabNavigationPage3
        ' 
        TabNavigationPage3.Caption = "TabNavigationPage3"
        TabNavigationPage3.Name = "TabNavigationPage3"
        TabNavigationPage3.Size = New Size(908, 472)
        ' 
        ' ucPayroll
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(TabPane1)
        Name = "ucPayroll"
        Size = New Size(1293, 722)
        CType(TabPane1, ComponentModel.ISupportInitialize).EndInit()
        TabPane1.ResumeLayout(False)
        TabNavigationPage1.ResumeLayout(False)
        TabNavigationPage1.PerformLayout()
        TabNavigationPage2.ResumeLayout(False)
        TabNavigationPage2.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents TabPane1 As DevExpress.XtraBars.Navigation.TabPane
    Friend WithEvents TabNavigationPage1 As DevExpress.XtraBars.Navigation.TabNavigationPage
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabNavigationPage2 As DevExpress.XtraBars.Navigation.TabNavigationPage
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabNavigationPage3 As DevExpress.XtraBars.Navigation.TabNavigationPage

End Class
