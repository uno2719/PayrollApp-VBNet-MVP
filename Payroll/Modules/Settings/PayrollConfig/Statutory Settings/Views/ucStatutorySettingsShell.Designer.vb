<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucStatutorySettingsShell
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
        tabconStatutory = New DevExpress.XtraTab.XtraTabControl()
        tabpageSSS = New DevExpress.XtraTab.XtraTabPage()
        tabpagePhilHealth = New DevExpress.XtraTab.XtraTabPage()
        tabpagePagIbig = New DevExpress.XtraTab.XtraTabPage()
        CType(tabconStatutory, ComponentModel.ISupportInitialize).BeginInit()
        tabconStatutory.SuspendLayout()
        SuspendLayout()
        '
        ' tabconStatutory
        '
        tabconStatutory.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        tabconStatutory.Appearance.Options.UseFont = True
        tabconStatutory.AppearancePage.Header.Font = New Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        tabconStatutory.AppearancePage.Header.FontStyleDelta = FontStyle.Bold
        tabconStatutory.AppearancePage.Header.Options.UseFont = True
        tabconStatutory.AppearancePage.HeaderActive.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        tabconStatutory.AppearancePage.HeaderActive.FontStyleDelta = FontStyle.Bold
        tabconStatutory.AppearancePage.HeaderActive.Options.UseFont = True
        tabconStatutory.Dock = DockStyle.Fill
        tabconStatutory.Location = New Point(4, 4)
        tabconStatutory.Margin = New Padding(3, 2, 3, 2)
        tabconStatutory.MultiLine = DevExpress.Utils.DefaultBoolean.True
        tabconStatutory.Name = "tabconStatutory"
        tabconStatutory.SelectedTabPage = tabpageSSS
        tabconStatutory.Size = New Size(900, 480)
        tabconStatutory.TabIndex = 0
        tabconStatutory.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {tabpageSSS, tabpagePhilHealth, tabpagePagIbig})
        '
        ' tabpageSSS
        '
        tabpageSSS.Margin = New Padding(3, 2, 3, 2)
        tabpageSSS.Name = "tabpageSSS"
        tabpageSSS.Size = New Size(898, 451)
        tabpageSSS.Text = "SSS"
        '
        ' tabpagePhilHealth
        '
        tabpagePhilHealth.Margin = New Padding(3, 2, 3, 2)
        tabpagePhilHealth.Name = "tabpagePhilHealth"
        tabpagePhilHealth.Size = New Size(898, 451)
        tabpagePhilHealth.Text = "PhilHealth"
        '
        ' tabpagePagIbig
        '
        tabpagePagIbig.Margin = New Padding(3, 2, 3, 2)
        tabpagePagIbig.Name = "tabpagePagIbig"
        tabpagePagIbig.Size = New Size(898, 451)
        tabpagePagIbig.Text = "Pag-IBIG"
        '
        ' ucStatutorySettingsShell
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(tabconStatutory)
        Name = "ucStatutorySettingsShell"
        Padding = New Padding(4)
        Size = New Size(908, 488)
        CType(tabconStatutory, ComponentModel.ISupportInitialize).EndInit()
        tabconStatutory.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents tabconStatutory As DevExpress.XtraTab.XtraTabControl
    Friend WithEvents tabpageSSS As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePhilHealth As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents tabpagePagIbig As DevExpress.XtraTab.XtraTabPage

End Class