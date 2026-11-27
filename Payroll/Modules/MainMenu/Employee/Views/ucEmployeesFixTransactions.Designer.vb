<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucEmployeesFixTransactions
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
        pnlTop = New DevExpress.XtraEditors.PanelControl()
        chkShowInactive = New DevExpress.XtraEditors.CheckEdit()
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        btnDelete = New DevExpress.XtraEditors.SimpleButton()
        btnToggleActive = New DevExpress.XtraEditors.SimpleButton()
        btnEdit = New DevExpress.XtraEditors.SimpleButton()
        btnAdd = New DevExpress.XtraEditors.SimpleButton()
        gridFix = New DevExpress.XtraGrid.GridControl()
        gridviewFix = New DevExpress.XtraGrid.Views.Grid.GridView()
        lblHelp = New DevExpress.XtraEditors.LabelControl()
        CType(pnlTop, ComponentModel.ISupportInitialize).BeginInit()
        pnlTop.SuspendLayout()
        CType(chkShowInactive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridFix, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewFix, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' pnlTop
        '
        pnlTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlTop.Controls.Add(chkShowInactive)
        pnlTop.Controls.Add(lblSummary)
        pnlTop.Controls.Add(btnDelete)
        pnlTop.Controls.Add(btnToggleActive)
        pnlTop.Controls.Add(btnEdit)
        pnlTop.Controls.Add(btnAdd)
        pnlTop.Dock = DockStyle.Top
        pnlTop.Location = New Point(0, 0)
        pnlTop.Name = "pnlTop"
        pnlTop.Size = New Size(800, 44)
        pnlTop.TabIndex = 0
        '
        ' btnAdd
        '
        btnAdd.Enabled = False
        btnAdd.Location = New Point(0, 8)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(90, 28)
        btnAdd.TabIndex = 0
        btnAdd.Text = "Add..."
        btnAdd.ToolTip = "Add a recurring allowance, compensation, bonus or deduction for this employee"
        '
        ' btnEdit
        '
        btnEdit.Enabled = False
        btnEdit.Location = New Point(96, 8)
        btnEdit.Name = "btnEdit"
        btnEdit.Size = New Size(90, 28)
        btnEdit.TabIndex = 1
        btnEdit.Text = "Edit..."
        btnEdit.ToolTip = "Edit the selected entry (double-click also works)"
        '
        ' btnToggleActive
        '
        btnToggleActive.Enabled = False
        btnToggleActive.Location = New Point(192, 8)
        btnToggleActive.Name = "btnToggleActive"
        btnToggleActive.Size = New Size(100, 28)
        btnToggleActive.TabIndex = 2
        btnToggleActive.Text = "Set Inactive"
        btnToggleActive.ToolTip = "An inactive entry is no longer given in payroll"
        '
        ' btnDelete
        '
        btnDelete.Enabled = False
        btnDelete.Location = New Point(298, 8)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(90, 28)
        btnDelete.TabIndex = 3
        btnDelete.Text = "Delete"
        btnDelete.ToolTip = "Only for entries not yet used in payroll (use Set Inactive for the others)"
        '
        ' chkShowInactive
        '
        chkShowInactive.Location = New Point(398, 12)
        chkShowInactive.Name = "chkShowInactive"
        chkShowInactive.Properties.Caption = "Show inactive"
        chkShowInactive.Size = New Size(110, 20)
        chkShowInactive.TabIndex = 4
        '
        ' lblSummary
        '
        lblSummary.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblSummary.Appearance.ForeColor = Color.Gray
        lblSummary.Appearance.Options.UseForeColor = True
        lblSummary.Appearance.Options.UseTextOptions = True
        lblSummary.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        lblSummary.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblSummary.Location = New Point(520, 14)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(280, 16)
        lblSummary.TabIndex = 5
        lblSummary.Text = ""
        '
        ' gridFix
        '
        gridFix.Dock = DockStyle.Fill
        gridFix.Location = New Point(0, 44)
        gridFix.MainView = gridviewFix
        gridFix.Name = "gridFix"
        gridFix.Size = New Size(800, 322)
        gridFix.TabIndex = 1
        gridFix.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewFix})
        '
        ' gridviewFix
        '
        gridviewFix.GridControl = gridFix
        gridviewFix.Name = "gridviewFix"
        '
        ' lblHelp
        '
        lblHelp.Appearance.ForeColor = Color.Gray
        lblHelp.Appearance.Options.UseForeColor = True
        lblHelp.Appearance.Options.UseTextOptions = True
        lblHelp.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
        lblHelp.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblHelp.Dock = DockStyle.Bottom
        lblHelp.Location = New Point(0, 366)
        lblHelp.Name = "lblHelp"
        lblHelp.Padding = New Padding(0, 4, 0, 0)
        lblHelp.Size = New Size(800, 34)
        lblHelp.TabIndex = 2
        lblHelp.Text = "Given automatically when payroll is processed. In Payroll Input Entry, a blank cell follows this; a typed amount replaces it for that cutoff only; 0 skips it for that cutoff."
        '
        ' ucEmployeesFixTransactions
        '
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridFix)
        Controls.Add(lblHelp)
        Controls.Add(pnlTop)
        Name = "ucEmployeesFixTransactions"
        Size = New Size(800, 400)
        CType(pnlTop, ComponentModel.ISupportInitialize).EndInit()
        pnlTop.ResumeLayout(False)
        CType(chkShowInactive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(gridFix, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewFix, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlTop As DevExpress.XtraEditors.PanelControl
    Friend WithEvents chkShowInactive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnDelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnToggleActive As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnEdit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAdd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridFix As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewFix As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblHelp As DevExpress.XtraEditors.LabelControl

End Class
