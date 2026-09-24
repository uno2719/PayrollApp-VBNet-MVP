<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucLeaveSettingsShell
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
        tabconLeaveSettings = New DevExpress.XtraTab.XtraTabControl()
        tabpageGroup = New DevExpress.XtraTab.XtraTabPage()
        tabpageType = New DevExpress.XtraTab.XtraTabPage()
        tabpageRule = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconLeaveSettings, ComponentModel.ISupportInitialize).BeginInit()
        tabconLeaveSettings.SuspendLayout()
        SuspendLayout()
        '
        ' tabconLeaveSettings
        '
        tabconLeaveSettings.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconLeaveSettings.Appearance.Options.UseFont = True
        tabconLeaveSettings.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconLeaveSettings.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconLeaveSettings.AppearancePage.Header.Options.UseFont = True
        tabconLeaveSettings.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconLeaveSettings.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconLeaveSettings.AppearancePage.HeaderActive.Options.UseFont = True
        tabconLeaveSettings.Dock = DockStyle.Fill
        tabconLeaveSettings.Location = New Point(4, 4)
        tabconLeaveSettings.Margin = New Padding(3, 2, 3, 2)
        tabconLeaveSettings.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconLeaveSettings.Name = "tabconLeaveSettings"
        tabconLeaveSettings.SelectedTabPage = tabpageGroup
        tabconLeaveSettings.Size = New Size(1200, 664)
        tabconLeaveSettings.TabIndex = 0
        tabconLeaveSettings.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageGroup, tabpageType, tabpageRule})
        '
        ' tabpageGroup
        '
        tabpageGroup.Margin = New Padding(3, 2, 3, 2)
        tabpageGroup.Name = "tabpageGroup"
        tabpageGroup.Size = New Size(1198, 635)
        tabpageGroup.Text = "Group"
        '
        ' tabpageType
        '
        tabpageType.Margin = New Padding(3, 2, 3, 2)
        tabpageType.Name = "tabpageType"
        tabpageType.Size = New Size(1198, 635)
        tabpageType.Text = "Type"
        '
        ' tabpageRule
        '
        tabpageRule.Margin = New Padding(3, 2, 3, 2)
        tabpageRule.Name = "tabpageRule"
        tabpageRule.Size = New Size(1198, 635)
        tabpageRule.Text = "Rule"
        '
        ' ucLeaveSettingsShell
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconLeaveSettings)
        Name = "ucLeaveSettingsShell"
        Padding = New Padding(4)
        Size = New Size(1208, 672)
        CType(tabconLeaveSettings, ComponentModel.ISupportInitialize).EndInit()
        tabconLeaveSettings.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconLeaveSettings As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageGroup As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageType As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageRule As DevExpress.XtraTab.XtraTabPage

End Class
