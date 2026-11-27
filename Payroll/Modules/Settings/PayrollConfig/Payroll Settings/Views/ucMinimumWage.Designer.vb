<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucMinimumWage
    Inherits DevExpress.XtraEditors.XtraUserControl

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
        lblIntro = New DevExpress.XtraEditors.LabelControl()
        pnlRates = New DevExpress.XtraEditors.PanelControl()
        gridRates = New DevExpress.XtraGrid.GridControl()
        gridviewRates = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlRatesTop = New DevExpress.XtraEditors.PanelControl()
        btnDeleteRate = New DevExpress.XtraEditors.SimpleButton()
        btnEditRate = New DevExpress.XtraEditors.SimpleButton()
        btnAddRate = New DevExpress.XtraEditors.SimpleButton()
        lblRatesTitle = New DevExpress.XtraEditors.LabelControl()
        pnlBranches = New DevExpress.XtraEditors.PanelControl()
        gridBranches = New DevExpress.XtraGrid.GridControl()
        gridviewBranches = New DevExpress.XtraGrid.Views.Grid.GridView()
        pnlBranchesTop = New DevExpress.XtraEditors.PanelControl()
        lblBranchesTitle = New DevExpress.XtraEditors.LabelControl()
        CType(pnlRates, ComponentModel.ISupportInitialize).BeginInit()
        pnlRates.SuspendLayout()
        CType(gridRates, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewRates, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlRatesTop, ComponentModel.ISupportInitialize).BeginInit()
        pnlRatesTop.SuspendLayout()
        CType(pnlBranches, ComponentModel.ISupportInitialize).BeginInit()
        pnlBranches.SuspendLayout()
        CType(gridBranches, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewBranches, ComponentModel.ISupportInitialize).BeginInit()
        CType(pnlBranchesTop, ComponentModel.ISupportInitialize).BeginInit()
        pnlBranchesTop.SuspendLayout()
        SuspendLayout()
        ' 
        ' lblIntro
        ' 
        lblIntro.Appearance.ForeColor = Color.DimGray
        lblIntro.Appearance.Options.UseForeColor = True
        lblIntro.Appearance.Options.UseTextOptions = True
        lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblIntro.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblIntro.Dock = DockStyle.Top
        lblIntro.Location = New Point(0, 0)
        lblIntro.Name = "lblIntro"
        lblIntro.Padding = New Padding(4, 6, 4, 6)
        lblIntro.Size = New Size(1000, 44)
        lblIntro.TabIndex = 0
        lblIntro.Text = "Minimum wage per Wage Area, and the Wage Area of each store. Used only as a reminder for the Minimum Wage Earner (tax-exempt) checkbox: Salary adjustment, Transfer, Change tax status, and the Employee header alert. The checkbox itself decides the tax."
        ' 
        ' pnlRates
        ' 
        pnlRates.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlRates.Controls.Add(gridRates)
        pnlRates.Controls.Add(pnlRatesTop)
        pnlRates.Dock = DockStyle.Left
        pnlRates.Location = New Point(0, 44)
        pnlRates.Name = "pnlRates"
        pnlRates.Padding = New Padding(0, 0, 10, 0)
        pnlRates.Size = New Size(560, 456)
        pnlRates.TabIndex = 1
        ' 
        ' pnlRatesTop
        ' 
        pnlRatesTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlRatesTop.Controls.Add(btnDeleteRate)
        pnlRatesTop.Controls.Add(btnEditRate)
        pnlRatesTop.Controls.Add(btnAddRate)
        pnlRatesTop.Controls.Add(lblRatesTitle)
        pnlRatesTop.Dock = DockStyle.Top
        pnlRatesTop.Location = New Point(0, 0)
        pnlRatesTop.Name = "pnlRatesTop"
        pnlRatesTop.Size = New Size(550, 40)
        pnlRatesTop.TabIndex = 0
        ' 
        ' lblRatesTitle
        ' 
        lblRatesTitle.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblRatesTitle.Appearance.Options.UseFont = True
        lblRatesTitle.Location = New Point(4, 12)
        lblRatesTitle.Name = "lblRatesTitle"
        lblRatesTitle.Size = New Size(140, 17)
        lblRatesTitle.TabIndex = 0
        lblRatesTitle.Text = "Minimum Wage Rates"
        ' 
        ' btnAddRate
        ' 
        btnAddRate.Location = New Point(250, 6)
        btnAddRate.Name = "btnAddRate"
        btnAddRate.Size = New Size(90, 28)
        btnAddRate.TabIndex = 1
        btnAddRate.Text = "Add..."
        ' 
        ' btnEditRate
        ' 
        btnEditRate.Location = New Point(346, 6)
        btnEditRate.Name = "btnEditRate"
        btnEditRate.Size = New Size(90, 28)
        btnEditRate.TabIndex = 2
        btnEditRate.Text = "Edit..."
        ' 
        ' btnDeleteRate
        ' 
        btnDeleteRate.Location = New Point(442, 6)
        btnDeleteRate.Name = "btnDeleteRate"
        btnDeleteRate.Size = New Size(90, 28)
        btnDeleteRate.TabIndex = 3
        btnDeleteRate.Text = "Delete"
        ' 
        ' gridRates
        ' 
        gridRates.Dock = DockStyle.Fill
        gridRates.Location = New Point(0, 40)
        gridRates.MainView = gridviewRates
        gridRates.Name = "gridRates"
        gridRates.Size = New Size(550, 416)
        gridRates.TabIndex = 1
        gridRates.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewRates})
        ' 
        ' gridviewRates
        ' 
        gridviewRates.GridControl = gridRates
        gridviewRates.Name = "gridviewRates"
        ' 
        ' pnlBranches
        ' 
        pnlBranches.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlBranches.Controls.Add(gridBranches)
        pnlBranches.Controls.Add(pnlBranchesTop)
        pnlBranches.Dock = DockStyle.Fill
        pnlBranches.Location = New Point(560, 44)
        pnlBranches.Name = "pnlBranches"
        pnlBranches.Size = New Size(440, 456)
        pnlBranches.TabIndex = 2
        ' 
        ' pnlBranchesTop
        ' 
        pnlBranchesTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlBranchesTop.Controls.Add(lblBranchesTitle)
        pnlBranchesTop.Dock = DockStyle.Top
        pnlBranchesTop.Location = New Point(0, 0)
        pnlBranchesTop.Name = "pnlBranchesTop"
        pnlBranchesTop.Size = New Size(440, 40)
        pnlBranchesTop.TabIndex = 0
        ' 
        ' lblBranchesTitle
        ' 
        lblBranchesTitle.Appearance.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblBranchesTitle.Appearance.Options.UseFont = True
        lblBranchesTitle.Location = New Point(4, 12)
        lblBranchesTitle.Name = "lblBranchesTitle"
        lblBranchesTitle.Size = New Size(260, 17)
        lblBranchesTitle.TabIndex = 0
        lblBranchesTitle.Text = "Stores / Branches (click Wage Area to set)"
        ' 
        ' gridBranches
        ' 
        gridBranches.Dock = DockStyle.Fill
        gridBranches.Location = New Point(0, 40)
        gridBranches.MainView = gridviewBranches
        gridBranches.Name = "gridBranches"
        gridBranches.Size = New Size(440, 416)
        gridBranches.TabIndex = 1
        gridBranches.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewBranches})
        ' 
        ' gridviewBranches
        ' 
        gridviewBranches.GridControl = gridBranches
        gridviewBranches.Name = "gridviewBranches"
        ' 
        ' ucMinimumWage
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(pnlBranches)
        Controls.Add(pnlRates)
        Controls.Add(lblIntro)
        Name = "ucMinimumWage"
        Size = New Size(1000, 500)
        CType(pnlRates, ComponentModel.ISupportInitialize).EndInit()
        pnlRates.ResumeLayout(False)
        CType(gridRates, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewRates, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlRatesTop, ComponentModel.ISupportInitialize).EndInit()
        pnlRatesTop.ResumeLayout(False)
        pnlRatesTop.PerformLayout()
        CType(pnlBranches, ComponentModel.ISupportInitialize).EndInit()
        pnlBranches.ResumeLayout(False)
        CType(gridBranches, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewBranches, ComponentModel.ISupportInitialize).EndInit()
        CType(pnlBranchesTop, ComponentModel.ISupportInitialize).EndInit()
        pnlBranchesTop.ResumeLayout(False)
        pnlBranchesTop.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents lblIntro As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlRates As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pnlRatesTop As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblRatesTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnAddRate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnEditRate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnDeleteRate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridRates As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewRates As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents pnlBranches As DevExpress.XtraEditors.PanelControl
    Friend WithEvents pnlBranchesTop As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblBranchesTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents gridBranches As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewBranches As DevExpress.XtraGrid.Views.Grid.GridView

End Class
