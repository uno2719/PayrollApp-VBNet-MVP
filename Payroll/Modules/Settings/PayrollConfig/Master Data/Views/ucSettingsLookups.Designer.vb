<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucSettingsLookups
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
        tabconMasterData = New DevExpress.XtraTab.XtraTabControl()
        tabpageBranch = New DevExpress.XtraTab.XtraTabPage()
        tabpageDepartment = New DevExpress.XtraTab.XtraTabPage()
        tabpagePosition = New DevExpress.XtraTab.XtraTabPage()
        tabpageCategory = New DevExpress.XtraTab.XtraTabPage()
        tabpageJobClass = New DevExpress.XtraTab.XtraTabPage()
        tabpageHolidayGroup = New DevExpress.XtraTab.XtraTabPage()
        tabpageScheduleGroup = New DevExpress.XtraTab.XtraTabPage()
        tabpageBank = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconMasterData, ComponentModel.ISupportInitialize).BeginInit()
        tabconMasterData.SuspendLayout()
        SuspendLayout()
        '
        ' tabconMasterData
        '
        tabconMasterData.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconMasterData.Appearance.Options.UseFont = True
        tabconMasterData.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconMasterData.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconMasterData.AppearancePage.Header.Options.UseFont = True
        tabconMasterData.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconMasterData.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconMasterData.AppearancePage.HeaderActive.Options.UseFont = True
        tabconMasterData.Dock = DockStyle.Fill
        tabconMasterData.Location = New Point(4, 4)
        tabconMasterData.Margin = New Padding(3, 2, 3, 2)
        tabconMasterData.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconMasterData.Name = "tabconMasterData"
        tabconMasterData.SelectedTabPage = tabpageBranch
        tabconMasterData.Size = New Size(900, 480)
        tabconMasterData.TabIndex = 0
        tabconMasterData.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageBranch, tabpageDepartment, tabpagePosition, tabpageCategory, tabpageJobClass, tabpageHolidayGroup, tabpageScheduleGroup, tabpageBank})
        '
        ' tabpageBranch
        '
        tabpageBranch.Margin = New Padding(3, 2, 3, 2)
        tabpageBranch.Name = "tabpageBranch"
        tabpageBranch.Size = New Size(898, 451)
        tabpageBranch.Text = "Branch"
        '
        ' tabpageDepartment
        '
        tabpageDepartment.Margin = New Padding(3, 2, 3, 2)
        tabpageDepartment.Name = "tabpageDepartment"
        tabpageDepartment.Size = New Size(898, 451)
        tabpageDepartment.Text = "Department"
        '
        ' tabpagePosition
        '
        tabpagePosition.Margin = New Padding(3, 2, 3, 2)
        tabpagePosition.Name = "tabpagePosition"
        tabpagePosition.Size = New Size(898, 451)
        tabpagePosition.Text = "Position"
        '
        ' tabpageCategory
        '
        tabpageCategory.Margin = New Padding(3, 2, 3, 2)
        tabpageCategory.Name = "tabpageCategory"
        tabpageCategory.Size = New Size(898, 451)
        tabpageCategory.Text = "Category"
        '
        ' tabpageJobClass
        '
        tabpageJobClass.Margin = New Padding(3, 2, 3, 2)
        tabpageJobClass.Name = "tabpageJobClass"
        tabpageJobClass.Size = New Size(898, 451)
        tabpageJobClass.Text = "Job Class"
        '
        ' tabpageHolidayGroup
        '
        tabpageHolidayGroup.Margin = New Padding(3, 2, 3, 2)
        tabpageHolidayGroup.Name = "tabpageHolidayGroup"
        tabpageHolidayGroup.Size = New Size(898, 451)
        tabpageHolidayGroup.Text = "Holiday Group"
        '
        ' tabpageScheduleGroup
        '
        tabpageScheduleGroup.Margin = New Padding(3, 2, 3, 2)
        tabpageScheduleGroup.Name = "tabpageScheduleGroup"
        tabpageScheduleGroup.Size = New Size(898, 451)
        tabpageScheduleGroup.Text = "Schedule Group"
        '
        ' tabpageBank
        '
        tabpageBank.Margin = New Padding(3, 2, 3, 2)
        tabpageBank.Name = "tabpageBank"
        tabpageBank.Size = New Size(898, 451)
        tabpageBank.Text = "Bank"
        '
        ' ucSettingsLookups
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconMasterData)
        Name = "ucSettingsLookups"
        Padding = New Padding(4)
        Size = New Size(908, 488)
        CType(tabconMasterData, ComponentModel.ISupportInitialize).EndInit()
        tabconMasterData.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconMasterData As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageBranch As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageDepartment As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePosition As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageCategory As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageJobClass As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageHolidayGroup As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageScheduleGroup As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageBank As DevExpress.XtraTab.XtraTabPage

End Class