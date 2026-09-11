<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucIncomeTaxTableShell
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
        tabconIncomeTax = New DevExpress.XtraTab.XtraTabControl()
        tabpageYearly = New DevExpress.XtraTab.XtraTabPage()
        tabpageMonthly = New DevExpress.XtraTab.XtraTabPage()
        tabpageSemiMonthly = New DevExpress.XtraTab.XtraTabPage()
        tabpageWeekly = New DevExpress.XtraTab.XtraTabPage()
        tabpageDaily = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconIncomeTax, ComponentModel.ISupportInitialize).BeginInit()
        tabconIncomeTax.SuspendLayout()
        SuspendLayout()
        '
        ' tabconIncomeTax
        '
        tabconIncomeTax.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconIncomeTax.Appearance.Options.UseFont = True
        tabconIncomeTax.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconIncomeTax.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconIncomeTax.AppearancePage.Header.Options.UseFont = True
        tabconIncomeTax.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconIncomeTax.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconIncomeTax.AppearancePage.HeaderActive.Options.UseFont = True
        tabconIncomeTax.Dock = DockStyle.Fill
        tabconIncomeTax.Location = New Point(4, 4)
        tabconIncomeTax.Margin = New Padding(3, 2, 3, 2)
        tabconIncomeTax.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconIncomeTax.Name = "tabconIncomeTax"
        tabconIncomeTax.SelectedTabPage = tabpageYearly
        tabconIncomeTax.Size = New Size(900, 480)
        tabconIncomeTax.TabIndex = 0
        tabconIncomeTax.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageYearly, tabpageMonthly, tabpageSemiMonthly, tabpageWeekly, tabpageDaily})
        '
        ' tabpageYearly
        '
        tabpageYearly.Margin = New Padding(3, 2, 3, 2)
        tabpageYearly.Name = "tabpageYearly"
        tabpageYearly.Size = New Size(898, 451)
        tabpageYearly.Text = "Yearly"
        '
        ' tabpageMonthly
        '
        tabpageMonthly.Margin = New Padding(3, 2, 3, 2)
        tabpageMonthly.Name = "tabpageMonthly"
        tabpageMonthly.Size = New Size(898, 451)
        tabpageMonthly.Text = "Monthly"
        '
        ' tabpageSemiMonthly
        '
        tabpageSemiMonthly.Margin = New Padding(3, 2, 3, 2)
        tabpageSemiMonthly.Name = "tabpageSemiMonthly"
        tabpageSemiMonthly.Size = New Size(898, 451)
        tabpageSemiMonthly.Text = "Semi-Monthly"
        '
        ' tabpageWeekly
        '
        tabpageWeekly.Margin = New Padding(3, 2, 3, 2)
        tabpageWeekly.Name = "tabpageWeekly"
        tabpageWeekly.Size = New Size(898, 451)
        tabpageWeekly.Text = "Weekly"
        '
        ' tabpageDaily
        '
        tabpageDaily.Margin = New Padding(3, 2, 3, 2)
        tabpageDaily.Name = "tabpageDaily"
        tabpageDaily.Size = New Size(898, 451)
        tabpageDaily.Text = "Daily"
        '
        ' ucIncomeTaxTableShell
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconIncomeTax)
        Name = "ucIncomeTaxTableShell"
        Padding = New Padding(4)
        Size = New Size(908, 488)
        CType(tabconIncomeTax, ComponentModel.ISupportInitialize).EndInit()
        tabconIncomeTax.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconIncomeTax As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageYearly As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageMonthly As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageSemiMonthly As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageWeekly As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageDaily As DevExpress.XtraTab.XtraTabPage

End Class