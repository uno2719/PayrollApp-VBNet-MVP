<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucModuleManagement
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

        tabconModuleManagement = New DevExpress.XtraTab.XtraTabControl()
        tabpageCatalog = New DevExpress.XtraTab.XtraTabPage()
        tabpageAccessEditor = New DevExpress.XtraTab.XtraTabPage()

        CType(tabconModuleManagement, ComponentModel.ISupportInitialize).BeginInit()
        tabconModuleManagement.SuspendLayout()
        SuspendLayout()
        '
        ' tabconModuleManagement
        '
        tabconModuleManagement.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconModuleManagement.Appearance.Options.UseFont = True
        tabconModuleManagement.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconModuleManagement.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconModuleManagement.AppearancePage.Header.Options.UseFont = True
        tabconModuleManagement.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconModuleManagement.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconModuleManagement.AppearancePage.HeaderActive.Options.UseFont = True
        tabconModuleManagement.Dock = DockStyle.Fill
        tabconModuleManagement.Location = New Point(4, 4)
        tabconModuleManagement.Margin = New Padding(3, 2, 3, 2)
        tabconModuleManagement.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconModuleManagement.Name = "tabconModuleManagement"
        tabconModuleManagement.SelectedTabPage = tabpageCatalog
        tabconModuleManagement.Size = New Size(960, 580)
        tabconModuleManagement.TabIndex = 0
        tabconModuleManagement.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageCatalog, tabpageAccessEditor})
        '
        ' tabpageCatalog
        '
        tabpageCatalog.Margin = New Padding(3, 2, 3, 2)
        tabpageCatalog.Name = "tabpageCatalog"
        tabpageCatalog.Size = New Size(958, 551)
        tabpageCatalog.Text = "Module Catalog"
        '
        ' tabpageAccessEditor
        '
        tabpageAccessEditor.Margin = New Padding(3, 2, 3, 2)
        tabpageAccessEditor.Name = "tabpageAccessEditor"
        tabpageAccessEditor.Size = New Size(958, 551)
        tabpageAccessEditor.Text = "Access Editor"
        '
        ' ucModuleManagement
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconModuleManagement)
        Name = "ucModuleManagement"
        Padding = New Padding(4)
        Size = New Size(968, 588)
        CType(tabconModuleManagement, ComponentModel.ISupportInitialize).EndInit()
        tabconModuleManagement.ResumeLayout(False)
        ResumeLayout(False)

    End Sub

    Friend WithEvents tabconModuleManagement As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageCatalog As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageAccessEditor As DevExpress.XtraTab.XtraTabPage

End Class