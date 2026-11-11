<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucEmployeesDocuments
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
        lblSummary = New DevExpress.XtraEditors.LabelControl()
        btnDelete = New DevExpress.XtraEditors.SimpleButton()
        btnSaveCopy = New DevExpress.XtraEditors.SimpleButton()
        btnOpen = New DevExpress.XtraEditors.SimpleButton()
        btnAdd = New DevExpress.XtraEditors.SimpleButton()
        gridDocuments = New DevExpress.XtraGrid.GridControl()
        gridviewDocuments = New DevExpress.XtraGrid.Views.Grid.GridView()
        CType(pnlTop, ComponentModel.ISupportInitialize).BeginInit()
        pnlTop.SuspendLayout()
        CType(gridDocuments, ComponentModel.ISupportInitialize).BeginInit()
        CType(gridviewDocuments, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' pnlTop
        ' 
        pnlTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        pnlTop.Controls.Add(lblSummary)
        pnlTop.Controls.Add(btnDelete)
        pnlTop.Controls.Add(btnSaveCopy)
        pnlTop.Controls.Add(btnOpen)
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
        btnAdd.Size = New Size(100, 28)
        btnAdd.TabIndex = 0
        btnAdd.Text = "Add..."
        btnAdd.ToolTip = "Add 201 file(s): choose the category first, then the file(s)"
        ' 
        ' btnOpen
        ' 
        btnOpen.Enabled = False
        btnOpen.Location = New Point(106, 8)
        btnOpen.Name = "btnOpen"
        btnOpen.Size = New Size(90, 28)
        btnOpen.TabIndex = 1
        btnOpen.Text = "Open"
        btnOpen.ToolTip = "Open the selected file (double-click also works)"
        ' 
        ' btnSaveCopy
        ' 
        btnSaveCopy.Enabled = False
        btnSaveCopy.Location = New Point(202, 8)
        btnSaveCopy.Name = "btnSaveCopy"
        btnSaveCopy.Size = New Size(110, 28)
        btnSaveCopy.TabIndex = 2
        btnSaveCopy.Text = "Save a copy..."
        btnSaveCopy.ToolTip = "Save a copy of the selected file to this PC"
        ' 
        ' btnDelete
        ' 
        btnDelete.Enabled = False
        btnDelete.Location = New Point(318, 8)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(90, 28)
        btnDelete.TabIndex = 3
        btnDelete.Text = "Delete"
        btnDelete.ToolTip = "Delete the selected file (asks for your password)"
        ' 
        ' lblSummary
        ' 
        lblSummary.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        lblSummary.Appearance.ForeColor = Color.Gray
        lblSummary.Appearance.Options.UseForeColor = True
        lblSummary.Appearance.Options.UseTextOptions = True
        lblSummary.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        lblSummary.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        lblSummary.Location = New Point(420, 14)
        lblSummary.Name = "lblSummary"
        lblSummary.Size = New Size(380, 16)
        lblSummary.TabIndex = 4
        lblSummary.Text = ""
        ' 
        ' gridDocuments
        ' 
        gridDocuments.Dock = DockStyle.Fill
        gridDocuments.Location = New Point(0, 44)
        gridDocuments.MainView = gridviewDocuments
        gridDocuments.Name = "gridDocuments"
        gridDocuments.Size = New Size(800, 356)
        gridDocuments.TabIndex = 1
        gridDocuments.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {gridviewDocuments})
        ' 
        ' gridviewDocuments
        ' 
        gridviewDocuments.GridControl = gridDocuments
        gridviewDocuments.Name = "gridviewDocuments"
        ' 
        ' ucEmployeesDocuments
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(gridDocuments)
        Controls.Add(pnlTop)
        Name = "ucEmployeesDocuments"
        Size = New Size(800, 400)
        CType(pnlTop, ComponentModel.ISupportInitialize).EndInit()
        pnlTop.ResumeLayout(False)
        CType(gridDocuments, ComponentModel.ISupportInitialize).EndInit()
        CType(gridviewDocuments, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlTop As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblSummary As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnDelete As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnSaveCopy As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnOpen As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAdd As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents gridDocuments As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridviewDocuments As DevExpress.XtraGrid.Views.Grid.GridView

End Class
