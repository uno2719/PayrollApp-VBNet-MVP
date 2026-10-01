<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayroll
    Inherits GlobalShared.Base.ucBase

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TabPane1 = New DevExpress.XtraBars.Navigation.TabPane()
        TabNavigationPage1 = New DevExpress.XtraBars.Navigation.TabNavigationPage()
        TabNavigationPage2 = New DevExpress.XtraBars.Navigation.TabNavigationPage()
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
        TabPane1.Dock = DockStyle.Fill
        TabPane1.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TabPane1.Location = New Point(0, 0)
        TabPane1.Name = "TabPane1"
        TabPane1.PageProperties.AppearanceCaption.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TabPane1.PageProperties.AppearanceCaption.Options.UseFont = True
        TabPane1.Pages.AddRange(New DevExpress.XtraBars.Navigation.NavigationPageBase() {TabNavigationPage1, TabNavigationPage2})
        TabPane1.RegularSize = New Size(1293, 722)
        TabPane1.SelectedPage = TabNavigationPage1
        TabPane1.Size = New Size(1293, 722)
        TabPane1.TabIndex = 2
        TabPane1.Text = "TabPane1"
        '
        ' TabNavigationPage1 (Input — also where "Process" runs)
        '
        TabNavigationPage1.Caption = "Input"
        TabNavigationPage1.Name = "TabNavigationPage1"
        TabNavigationPage1.Size = New Size(1293, 689)
        '
        ' TabNavigationPage2 (Output)
        '
        TabNavigationPage2.Caption = "Output"
        TabNavigationPage2.Name = "TabNavigationPage2"
        TabNavigationPage2.Size = New Size(1293, 689)
        '
        ' ucPayroll
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(TabPane1)
        Name = "ucPayroll"
        Size = New Size(1293, 722)
        CType(TabPane1, ComponentModel.ISupportInitialize).EndInit()
        TabPane1.ResumeLayout(False)
        TabNavigationPage1.ResumeLayout(False)
        TabNavigationPage2.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents TabPane1 As DevExpress.XtraBars.Navigation.TabPane
    Friend WithEvents TabNavigationPage1 As DevExpress.XtraBars.Navigation.TabNavigationPage
    Friend WithEvents TabNavigationPage2 As DevExpress.XtraBars.Navigation.TabNavigationPage

End Class