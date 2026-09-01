Namespace DBConnection.Views
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class ucDatabaseConnectionSettings
        Inherits System.Windows.Forms.UserControl

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
            lblHeader = New DevExpress.XtraEditors.LabelControl()
            lblServerAddress = New DevExpress.XtraEditors.LabelControl()
            txtServerAddress = New DevExpress.XtraEditors.TextEdit()
            lblDatabaseName = New DevExpress.XtraEditors.LabelControl()
            txtDatabaseName = New DevExpress.XtraEditors.TextEdit()
            lblSqlUsername = New DevExpress.XtraEditors.LabelControl()
            txtSqlUsername = New DevExpress.XtraEditors.TextEdit()
            lblSqlPassword = New DevExpress.XtraEditors.LabelControl()
            txtSqlPassword = New DevExpress.XtraEditors.ButtonEdit()
            lblStatusMessage = New DevExpress.XtraEditors.LabelControl()
            btnTestConnection = New DevExpress.XtraEditors.SimpleButton()
            btnSave = New DevExpress.XtraEditors.SimpleButton()
            CType(txtServerAddress.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(txtDatabaseName.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(txtSqlUsername.Properties, ComponentModel.ISupportInitialize).BeginInit()
            CType(txtSqlPassword.Properties, ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            ' 
            ' lblHeader
            ' 
            lblHeader.Appearance.Font = New Font("Segoe UI", 13.0F)
            lblHeader.Appearance.Options.UseFont = True
            lblHeader.Location = New Point(20, 16)
            lblHeader.Name = "lblHeader"
            lblHeader.Size = New Size(230, 23)
            lblHeader.TabIndex = 0
            lblHeader.Text = "Database Connection Settings"
            ' 
            ' lblServerAddress
            ' 
            lblServerAddress.Location = New Point(20, 58)
            lblServerAddress.Name = "lblServerAddress"
            lblServerAddress.Size = New Size(74, 13)
            lblServerAddress.TabIndex = 1
            lblServerAddress.Text = "Server Address"
            ' 
            ' txtServerAddress
            ' 
            txtServerAddress.Location = New Point(20, 76)
            txtServerAddress.Name = "txtServerAddress"
            txtServerAddress.Properties.NullValuePrompt = "e.g. 192.168.1.50 o PAYROLL-DB"
            txtServerAddress.Size = New Size(340, 20)
            txtServerAddress.TabIndex = 2
            ' 
            ' lblDatabaseName
            ' 
            lblDatabaseName.Location = New Point(20, 110)
            lblDatabaseName.Name = "lblDatabaseName"
            lblDatabaseName.Size = New Size(76, 13)
            lblDatabaseName.TabIndex = 3
            lblDatabaseName.Text = "Database Name"
            ' 
            ' txtDatabaseName
            ' 
            txtDatabaseName.Location = New Point(20, 128)
            txtDatabaseName.Name = "txtDatabaseName"
            txtDatabaseName.Properties.NullValuePrompt = "e.g. PayrollDB"
            txtDatabaseName.Size = New Size(340, 20)
            txtDatabaseName.TabIndex = 4
            ' 
            ' lblSqlUsername
            ' 
            lblSqlUsername.Location = New Point(20, 162)
            lblSqlUsername.Name = "lblSqlUsername"
            lblSqlUsername.Size = New Size(70, 13)
            lblSqlUsername.TabIndex = 5
            lblSqlUsername.Text = "SQL Username"
            ' 
            ' txtSqlUsername
            ' 
            txtSqlUsername.Location = New Point(20, 180)
            txtSqlUsername.Name = "txtSqlUsername"
            txtSqlUsername.Size = New Size(340, 20)
            txtSqlUsername.TabIndex = 6
            ' 
            ' lblSqlPassword
            ' 
            lblSqlPassword.Location = New Point(20, 214)
            lblSqlPassword.Name = "lblSqlPassword"
            lblSqlPassword.Size = New Size(68, 13)
            lblSqlPassword.TabIndex = 7
            lblSqlPassword.Text = "SQL Password"
            ' 
            ' txtSqlPassword
            ' 
            txtSqlPassword.Location = New Point(20, 232)
            txtSqlPassword.Name = "txtSqlPassword"
            txtSqlPassword.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph)})
            txtSqlPassword.Properties.NullValuePrompt = "Blangko lang kung hindi papalitan"
            txtSqlPassword.Properties.PasswordChar = "●"c
            txtSqlPassword.Size = New Size(340, 20)
            txtSqlPassword.TabIndex = 8
            ' 
            ' lblStatusMessage
            ' 
            lblStatusMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
            lblStatusMessage.Location = New Point(20, 268)
            lblStatusMessage.Name = "lblStatusMessage"
            lblStatusMessage.Size = New Size(340, 26)
            lblStatusMessage.TabIndex = 9
            ' 
            ' btnTestConnection
            ' 
            btnTestConnection.Location = New Point(20, 302)
            btnTestConnection.Name = "btnTestConnection"
            btnTestConnection.Size = New Size(165, 34)
            btnTestConnection.TabIndex = 10
            btnTestConnection.Text = "Test Connection"
            ' 
            ' btnSave
            ' 
            btnSave.Location = New Point(195, 302)
            btnSave.Name = "btnSave"
            btnSave.Size = New Size(165, 34)
            btnSave.TabIndex = 11
            btnSave.Text = "Save"
            ' 
            ' ucDatabaseConnectionSettings
            ' 
            Controls.Add(btnSave)
            Controls.Add(btnTestConnection)
            Controls.Add(lblStatusMessage)
            Controls.Add(txtSqlPassword)
            Controls.Add(lblSqlPassword)
            Controls.Add(txtSqlUsername)
            Controls.Add(lblSqlUsername)
            Controls.Add(txtDatabaseName)
            Controls.Add(lblDatabaseName)
            Controls.Add(txtServerAddress)
            Controls.Add(lblServerAddress)
            Controls.Add(lblHeader)
            AutoScaleDimensions = New SizeF(7.0!, 15.0!)
            AutoScaleMode = AutoScaleMode.Font

            Name = "ucDatabaseConnectionSettings"
            Size = New Size(380, 360)
            CType(txtServerAddress.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(txtDatabaseName.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(txtSqlUsername.Properties, ComponentModel.ISupportInitialize).EndInit()
            CType(txtSqlPassword.Properties, ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)
            PerformLayout()

        End Sub

        Friend WithEvents lblHeader As DevExpress.XtraEditors.LabelControl
        Friend WithEvents lblServerAddress As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtServerAddress As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblDatabaseName As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtDatabaseName As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblSqlUsername As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtSqlUsername As DevExpress.XtraEditors.TextEdit
        Friend WithEvents lblSqlPassword As DevExpress.XtraEditors.LabelControl
        Friend WithEvents txtSqlPassword As DevExpress.XtraEditors.ButtonEdit
        Friend WithEvents lblStatusMessage As DevExpress.XtraEditors.LabelControl
        Friend WithEvents btnTestConnection As DevExpress.XtraEditors.SimpleButton
        Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton

    End Class
End Namespace