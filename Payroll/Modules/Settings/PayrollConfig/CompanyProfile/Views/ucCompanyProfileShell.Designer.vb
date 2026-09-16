' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyProfileShell.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucCompanyProfileShell
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
        tabconCompany = New DevExpress.XtraTab.XtraTabControl()
        tabpageCompanyProfile = New DevExpress.XtraTab.XtraTabPage()
        tabpageSSS = New DevExpress.XtraTab.XtraTabPage()
        tabpagePhilHealth = New DevExpress.XtraTab.XtraTabPage()
        tabpagePagIbig = New DevExpress.XtraTab.XtraTabPage()
        tabpageBIR = New DevExpress.XtraTab.XtraTabPage()
        tabpageBank = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconCompany, ComponentModel.ISupportInitialize).BeginInit()
        tabconCompany.SuspendLayout()
        SuspendLayout()
        '
        ' tabconCompany
        '
        tabconCompany.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconCompany.Appearance.Options.UseFont = True
        tabconCompany.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconCompany.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconCompany.AppearancePage.Header.Options.UseFont = True
        tabconCompany.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconCompany.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconCompany.AppearancePage.HeaderActive.Options.UseFont = True
        tabconCompany.Dock = DockStyle.Fill
        tabconCompany.Location = New Point(4, 4)
        tabconCompany.Margin = New Padding(3, 2, 3, 2)
        tabconCompany.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconCompany.Name = "tabconCompany"
        tabconCompany.SelectedTabPage = tabpageCompanyProfile
        tabconCompany.Size = New Size(1069, 600)
        tabconCompany.TabIndex = 0
        tabconCompany.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageCompanyProfile, tabpageSSS, tabpagePhilHealth, tabpagePagIbig, tabpageBIR, tabpageBank})
        '
        ' tabpageCompanyProfile
        '
        tabpageCompanyProfile.Margin = New Padding(3, 2, 3, 2)
        tabpageCompanyProfile.Name = "tabpageCompanyProfile"
        tabpageCompanyProfile.Size = New Size(1067, 571)
        tabpageCompanyProfile.Text = "Company Profile"
        '
        ' tabpageSSS
        '
        tabpageSSS.Margin = New Padding(3, 2, 3, 2)
        tabpageSSS.Name = "tabpageSSS"
        tabpageSSS.Size = New Size(1067, 571)
        tabpageSSS.Text = "SSS"
        '
        ' tabpagePhilHealth
        '
        tabpagePhilHealth.Margin = New Padding(3, 2, 3, 2)
        tabpagePhilHealth.Name = "tabpagePhilHealth"
        tabpagePhilHealth.Size = New Size(1067, 571)
        tabpagePhilHealth.Text = "PhilHealth"
        '
        ' tabpagePagIbig
        '
        tabpagePagIbig.Margin = New Padding(3, 2, 3, 2)
        tabpagePagIbig.Name = "tabpagePagIbig"
        tabpagePagIbig.Size = New Size(1067, 571)
        tabpagePagIbig.Text = "Pag-IBIG"
        '
        ' tabpageBIR
        '
        tabpageBIR.Margin = New Padding(3, 2, 3, 2)
        tabpageBIR.Name = "tabpageBIR"
        tabpageBIR.Size = New Size(1067, 571)
        tabpageBIR.Text = "BIR"
        '
        ' tabpageBank
        '
        tabpageBank.Margin = New Padding(3, 2, 3, 2)
        tabpageBank.Name = "tabpageBank"
        tabpageBank.Size = New Size(1067, 571)
        tabpageBank.Text = "Bank"
        '
        ' ucCompanyProfileShell
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconCompany)
        Name = "ucCompanyProfileShell"
        Padding = New Padding(4)
        Size = New Size(1077, 608)
        CType(tabconCompany, ComponentModel.ISupportInitialize).EndInit()
        tabconCompany.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconCompany As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageCompanyProfile As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageSSS As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePhilHealth As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePagIbig As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageBIR As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpageBank As DevExpress.XtraTab.XtraTabPage

End Class