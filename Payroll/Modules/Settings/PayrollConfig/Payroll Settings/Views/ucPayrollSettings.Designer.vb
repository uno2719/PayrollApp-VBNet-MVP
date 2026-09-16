<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucPayrollSettings
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
        tabconPayrollSettings = New DevExpress.XtraTab.XtraTabControl()
        tabpageCompensation = New DevExpress.XtraTab.XtraTabPage()
        tabpageDeduction = New DevExpress.XtraTab.XtraTabPage()
        tabpageOvertime = New DevExpress.XtraTab.XtraTabPage()
        tabpageHoliday = New DevExpress.XtraTab.XtraTabPage()
        tabpageBonus = New DevExpress.XtraTab.XtraTabPage()
        tabpageLoan = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconPayrollSettings, ComponentModel.ISupportInitialize).BeginInit()
        tabconPayrollSettings.SuspendLayout()
        SuspendLayout()
        ' 
        ' tabconPayrollSettings
        ' 
        tabconPayrollSettings.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconPayrollSettings.Appearance.Options.UseFont = True
        tabconPayrollSettings.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconPayrollSettings.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconPayrollSettings.AppearancePage.Header.Options.UseFont = True
        tabconPayrollSettings.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconPayrollSettings.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconPayrollSettings.AppearancePage.HeaderActive.Options.UseFont = True
        tabconPayrollSettings.Dock = DockStyle.Fill
        tabconPayrollSettings.Location = New Point(4, 4)
        tabconPayrollSettings.Margin = New Padding(3, 2, 3, 2)
        tabconPayrollSettings.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconPayrollSettings.Name = "tabconPayrollSettings"
        tabconPayrollSettings.SelectedTabPage = tabpageCompensation
        tabconPayrollSettings.Size = New Size(900, 480)
        tabconPayrollSettings.TabIndex = 0
        tabconPayrollSettings.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageCompensation, tabpageDeduction, tabpageOvertime, tabpageHoliday, tabpageBonus, tabpageLoan})
        ' 
        ' tabpageCompensation
        ' 
        tabpageCompensation.Margin = New Padding(3, 2, 3, 2)
        tabpageCompensation.Name = "tabpageCompensation"
        tabpageCompensation.Size = New Size(898, 451)
        tabpageCompensation.Text = "Compensation"
        ' 
        ' tabpageDeduction
        ' 
        tabpageDeduction.Margin = New Padding(3, 2, 3, 2)
        tabpageDeduction.Name = "tabpageDeduction"
        tabpageDeduction.Size = New Size(898, 451)
        tabpageDeduction.Text = "Deduction"
        ' 
        ' tabpageOvertime
        ' 
        tabpageOvertime.Margin = New Padding(3, 2, 3, 2)
        tabpageOvertime.Name = "tabpageOvertime"
        tabpageOvertime.Size = New Size(898, 451)
        tabpageOvertime.Text = "Overtime"
        ' 
        ' tabpageHoliday
        ' 
        tabpageHoliday.Margin = New Padding(3, 2, 3, 2)
        tabpageHoliday.Name = "tabpageHoliday"
        tabpageHoliday.Size = New Size(898, 451)
        tabpageHoliday.Text = "Holiday"
        ' 
        ' tabpageBonus
        ' 
        tabpageBonus.Margin = New Padding(3, 2, 3, 2)
        tabpageBonus.Name = "tabpageBonus"
        tabpageBonus.Size = New Size(898, 451)
        tabpageBonus.Text = "Bonus"
        ' 
        ' tabpageLoan
        ' 
        tabpageLoan.Margin = New Padding(3, 2, 3, 2)
        tabpageLoan.Name = "tabpageLoan"
        tabpageLoan.Size = New Size(898, 451)
        tabpageLoan.Text = "Loan"
        ' 
        ' ucPayrollSettings
        ' 
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconPayrollSettings)
        Name = "ucPayrollSettings"
        Padding = New Padding(4)
        Size = New Size(908, 488)
        CType(tabconPayrollSettings, ComponentModel.ISupportInitialize).EndInit()
        tabconPayrollSettings.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconPayrollSettings As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageCompensation As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageDeduction As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageOvertime As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageHoliday As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageBonus As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageLoan As DevExpress.XtraTab.XtraTabPage

End Class
