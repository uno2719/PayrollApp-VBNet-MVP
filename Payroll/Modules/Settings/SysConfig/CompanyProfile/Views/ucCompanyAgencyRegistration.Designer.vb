' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompanyAgencyRegistration.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucCompanyAgencyRegistration
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
        PanelControl1 = New DevExpress.XtraEditors.PanelControl()
        lblTabPageTitle = New DevExpress.XtraEditors.LabelControl()
        btnSave = New DevExpress.XtraEditors.SimpleButton()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        memoRemarks = New DevExpress.XtraEditors.MemoEdit()
        lblRemarks = New DevExpress.XtraEditors.LabelControl()
        txtPersonInCharge2Email = New DevExpress.XtraEditors.TextEdit()
        lblPersonInCharge2Email = New DevExpress.XtraEditors.LabelControl()
        txtPersonInCharge2Position = New DevExpress.XtraEditors.TextEdit()
        lblPersonInCharge2Position = New DevExpress.XtraEditors.LabelControl()
        lookupPersonInCharge2 = New DevExpress.XtraEditors.LookUpEdit()
        lblPersonInCharge2 = New DevExpress.XtraEditors.LabelControl()
        txtPersonInCharge1Email = New DevExpress.XtraEditors.TextEdit()
        lblPersonInCharge1Email = New DevExpress.XtraEditors.LabelControl()
        txtPersonInCharge1Position = New DevExpress.XtraEditors.TextEdit()
        lblPersonInCharge1Position = New DevExpress.XtraEditors.LabelControl()
        lookupPersonInCharge1 = New DevExpress.XtraEditors.LookUpEdit()
        lblPersonInCharge1 = New DevExpress.XtraEditors.LabelControl()
        txtContactPersonEmail = New DevExpress.XtraEditors.TextEdit()
        lblContactPersonEmail = New DevExpress.XtraEditors.LabelControl()
        txtContactPersonPosition = New DevExpress.XtraEditors.TextEdit()
        lblContactPersonPosition = New DevExpress.XtraEditors.LabelControl()
        lookupContactPerson = New DevExpress.XtraEditors.LookUpEdit()
        lblContactPerson = New DevExpress.XtraEditors.LabelControl()
        txtFaxNo = New DevExpress.XtraEditors.TextEdit()
        lblFaxNo = New DevExpress.XtraEditors.LabelControl()
        txtTelephoneNo = New DevExpress.XtraEditors.TextEdit()
        lblTelephoneNo = New DevExpress.XtraEditors.LabelControl()
        txtPostCode = New DevExpress.XtraEditors.TextEdit()
        lblPostCode = New DevExpress.XtraEditors.LabelControl()
        txtCountry = New DevExpress.XtraEditors.TextEdit()
        lblCountry = New DevExpress.XtraEditors.LabelControl()
        txtAddress3 = New DevExpress.XtraEditors.TextEdit()
        lblAddress3 = New DevExpress.XtraEditors.LabelControl()
        txtAddress2 = New DevExpress.XtraEditors.TextEdit()
        lblAddress2 = New DevExpress.XtraEditors.LabelControl()
        txtAddress1 = New DevExpress.XtraEditors.TextEdit()
        lblAddress1 = New DevExpress.XtraEditors.LabelControl()
        txtBranch = New DevExpress.XtraEditors.TextEdit()
        lblBranch = New DevExpress.XtraEditors.LabelControl()
        txtRegistrationNo = New DevExpress.XtraEditors.TextEdit()
        lblRegistrationNo = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPersonInCharge2Email.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPersonInCharge2Position.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupPersonInCharge2.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPersonInCharge1Email.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPersonInCharge1Position.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupPersonInCharge1.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtContactPersonEmail.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtContactPersonPosition.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(lookupContactPerson.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtFaxNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtTelephoneNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPostCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCountry.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress3.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress2.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress1.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtBranch.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtRegistrationNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        '
        ' PanelControl1
        '
        PanelControl1.Controls.Add(lblTabPageTitle)
        PanelControl1.Controls.Add(btnSave)
        PanelControl1.Dock = DockStyle.Top
        PanelControl1.Location = New Point(4, 4)
        PanelControl1.Margin = New Padding(3, 2, 3, 2)
        PanelControl1.Name = "PanelControl1"
        PanelControl1.Size = New Size(1069, 56)
        PanelControl1.TabIndex = 0
        '
        ' lblTabPageTitle
        '
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(280, 30)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "AGENCY REGISTRATION"
        '
        ' btnSave
        '
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.Location = New Point(969, 12)
        btnSave.Margin = New Padding(3, 2, 3, 2)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(90, 32)
        btnSave.TabIndex = 1
        btnSave.Text = "Save"
        '
        ' grpDetails
        '
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(memoRemarks)
        grpDetails.Controls.Add(lblRemarks)
        grpDetails.Controls.Add(txtPersonInCharge2Email)
        grpDetails.Controls.Add(lblPersonInCharge2Email)
        grpDetails.Controls.Add(txtPersonInCharge2Position)
        grpDetails.Controls.Add(lblPersonInCharge2Position)
        grpDetails.Controls.Add(lookupPersonInCharge2)
        grpDetails.Controls.Add(lblPersonInCharge2)
        grpDetails.Controls.Add(txtPersonInCharge1Email)
        grpDetails.Controls.Add(lblPersonInCharge1Email)
        grpDetails.Controls.Add(txtPersonInCharge1Position)
        grpDetails.Controls.Add(lblPersonInCharge1Position)
        grpDetails.Controls.Add(lookupPersonInCharge1)
        grpDetails.Controls.Add(lblPersonInCharge1)
        grpDetails.Controls.Add(txtContactPersonEmail)
        grpDetails.Controls.Add(lblContactPersonEmail)
        grpDetails.Controls.Add(txtContactPersonPosition)
        grpDetails.Controls.Add(lblContactPersonPosition)
        grpDetails.Controls.Add(lookupContactPerson)
        grpDetails.Controls.Add(lblContactPerson)
        grpDetails.Controls.Add(txtFaxNo)
        grpDetails.Controls.Add(lblFaxNo)
        grpDetails.Controls.Add(txtTelephoneNo)
        grpDetails.Controls.Add(lblTelephoneNo)
        grpDetails.Controls.Add(txtPostCode)
        grpDetails.Controls.Add(lblPostCode)
        grpDetails.Controls.Add(txtCountry)
        grpDetails.Controls.Add(lblCountry)
        grpDetails.Controls.Add(txtAddress3)
        grpDetails.Controls.Add(lblAddress3)
        grpDetails.Controls.Add(txtAddress2)
        grpDetails.Controls.Add(lblAddress2)
        grpDetails.Controls.Add(txtAddress1)
        grpDetails.Controls.Add(lblAddress1)
        grpDetails.Controls.Add(txtBranch)
        grpDetails.Controls.Add(lblBranch)
        grpDetails.Controls.Add(txtRegistrationNo)
        grpDetails.Controls.Add(lblRegistrationNo)
        grpDetails.Dock = DockStyle.Fill
        grpDetails.Location = New Point(4, 60)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(1061, 400)
        grpDetails.TabIndex = 1
        grpDetails.Text = " REGISTRATION DETAILS"
        '
        ' Column 1 (x=24): Registration No./TIN, Branch/RDO, Address 1-3, Country
        '
        lblRegistrationNo.Location = New Point(24, 34)
        lblRegistrationNo.Name = "lblRegistrationNo"
        lblRegistrationNo.Size = New Size(78, 13)
        lblRegistrationNo.TabIndex = 0
        lblRegistrationNo.Text = "Registration No."
        txtRegistrationNo.Location = New Point(24, 52)
        txtRegistrationNo.Margin = New Padding(3, 2, 3, 2)
        txtRegistrationNo.Name = "txtRegistrationNo"
        txtRegistrationNo.Size = New Size(230, 20)
        txtRegistrationNo.TabIndex = 1
        lblBranch.Location = New Point(24, 84)
        lblBranch.Name = "lblBranch"
        lblBranch.Size = New Size(35, 13)
        lblBranch.TabIndex = 2
        lblBranch.Text = "Branch"
        txtBranch.Location = New Point(24, 102)
        txtBranch.Margin = New Padding(3, 2, 3, 2)
        txtBranch.Name = "txtBranch"
        txtBranch.Size = New Size(230, 20)
        txtBranch.TabIndex = 3
        lblAddress1.Location = New Point(24, 134)
        lblAddress1.Name = "lblAddress1"
        lblAddress1.Size = New Size(51, 13)
        lblAddress1.TabIndex = 4
        lblAddress1.Text = "Address 1"
        txtAddress1.Location = New Point(24, 152)
        txtAddress1.Margin = New Padding(3, 2, 3, 2)
        txtAddress1.Name = "txtAddress1"
        txtAddress1.Size = New Size(230, 20)
        txtAddress1.TabIndex = 5
        lblAddress2.Location = New Point(24, 184)
        lblAddress2.Name = "lblAddress2"
        lblAddress2.Size = New Size(51, 13)
        lblAddress2.TabIndex = 6
        lblAddress2.Text = "Address 2"
        txtAddress2.Location = New Point(24, 202)
        txtAddress2.Margin = New Padding(3, 2, 3, 2)
        txtAddress2.Name = "txtAddress2"
        txtAddress2.Size = New Size(230, 20)
        txtAddress2.TabIndex = 7
        lblAddress3.Location = New Point(24, 234)
        lblAddress3.Name = "lblAddress3"
        lblAddress3.Size = New Size(51, 13)
        lblAddress3.TabIndex = 8
        lblAddress3.Text = "Address 3"
        txtAddress3.Location = New Point(24, 252)
        txtAddress3.Margin = New Padding(3, 2, 3, 2)
        txtAddress3.Name = "txtAddress3"
        txtAddress3.Size = New Size(230, 20)
        txtAddress3.TabIndex = 9
        lblCountry.Location = New Point(24, 284)
        lblCountry.Name = "lblCountry"
        lblCountry.Size = New Size(40, 13)
        lblCountry.TabIndex = 10
        lblCountry.Text = "Country"
        txtCountry.Location = New Point(24, 302)
        txtCountry.Margin = New Padding(3, 2, 3, 2)
        txtCountry.Name = "txtCountry"
        txtCountry.Size = New Size(230, 20)
        txtCountry.TabIndex = 11
        '
        ' Column 2 (x=280): Post Code, Telephone No., Fax No., Contact Person, Position, Email
        '
        lblPostCode.Location = New Point(280, 34)
        lblPostCode.Name = "lblPostCode"
        lblPostCode.Size = New Size(52, 13)
        lblPostCode.TabIndex = 12
        lblPostCode.Text = "Post Code"
        txtPostCode.Location = New Point(280, 52)
        txtPostCode.Margin = New Padding(3, 2, 3, 2)
        txtPostCode.Name = "txtPostCode"
        txtPostCode.Size = New Size(230, 20)
        txtPostCode.TabIndex = 13
        lblTelephoneNo.Location = New Point(280, 84)
        lblTelephoneNo.Name = "lblTelephoneNo"
        lblTelephoneNo.Size = New Size(66, 13)
        lblTelephoneNo.TabIndex = 14
        lblTelephoneNo.Text = "Telephone No."
        txtTelephoneNo.Location = New Point(280, 102)
        txtTelephoneNo.Margin = New Padding(3, 2, 3, 2)
        txtTelephoneNo.Name = "txtTelephoneNo"
        txtTelephoneNo.Size = New Size(230, 20)
        txtTelephoneNo.TabIndex = 15
        lblFaxNo.Location = New Point(280, 134)
        lblFaxNo.Name = "lblFaxNo"
        lblFaxNo.Size = New Size(41, 13)
        lblFaxNo.TabIndex = 16
        lblFaxNo.Text = "Fax No."
        txtFaxNo.Location = New Point(280, 152)
        txtFaxNo.Margin = New Padding(3, 2, 3, 2)
        txtFaxNo.Name = "txtFaxNo"
        txtFaxNo.Size = New Size(230, 20)
        txtFaxNo.TabIndex = 17
        lblContactPerson.Location = New Point(280, 184)
        lblContactPerson.Name = "lblContactPerson"
        lblContactPerson.Size = New Size(72, 13)
        lblContactPerson.TabIndex = 18
        lblContactPerson.Text = "Contact Person"
        lookupContactPerson.Location = New Point(280, 202)
        lookupContactPerson.Margin = New Padding(3, 2, 3, 2)
        lookupContactPerson.Name = "lookupContactPerson"
        lookupContactPerson.Size = New Size(230, 20)
        lookupContactPerson.TabIndex = 19
        lblContactPersonPosition.Location = New Point(280, 234)
        lblContactPersonPosition.Name = "lblContactPersonPosition"
        lblContactPersonPosition.Size = New Size(40, 13)
        lblContactPersonPosition.TabIndex = 20
        lblContactPersonPosition.Text = "Position"
        txtContactPersonPosition.Location = New Point(280, 252)
        txtContactPersonPosition.Margin = New Padding(3, 2, 3, 2)
        txtContactPersonPosition.Name = "txtContactPersonPosition"
        txtContactPersonPosition.Size = New Size(230, 20)
        txtContactPersonPosition.TabIndex = 21
        lblContactPersonEmail.Location = New Point(280, 284)
        lblContactPersonEmail.Name = "lblContactPersonEmail"
        lblContactPersonEmail.Size = New Size(70, 13)
        lblContactPersonEmail.TabIndex = 22
        lblContactPersonEmail.Text = "Email Address"
        txtContactPersonEmail.Location = New Point(280, 302)
        txtContactPersonEmail.Margin = New Padding(3, 2, 3, 2)
        txtContactPersonEmail.Name = "txtContactPersonEmail"
        txtContactPersonEmail.Size = New Size(230, 20)
        txtContactPersonEmail.TabIndex = 23
        '
        ' Column 3 (x=536): Person-in-charge 1 & 2 (Name via LookUpEdit, Position, Email)
        '
        lblPersonInCharge1.Location = New Point(536, 34)
        lblPersonInCharge1.Name = "lblPersonInCharge1"
        lblPersonInCharge1.Size = New Size(91, 13)
        lblPersonInCharge1.TabIndex = 24
        lblPersonInCharge1.Text = "Person-in-charge 1"
        lookupPersonInCharge1.Location = New Point(536, 52)
        lookupPersonInCharge1.Margin = New Padding(3, 2, 3, 2)
        lookupPersonInCharge1.Name = "lookupPersonInCharge1"
        lookupPersonInCharge1.Size = New Size(230, 20)
        lookupPersonInCharge1.TabIndex = 25
        lblPersonInCharge1Position.Location = New Point(536, 84)
        lblPersonInCharge1Position.Name = "lblPersonInCharge1Position"
        lblPersonInCharge1Position.Size = New Size(40, 13)
        lblPersonInCharge1Position.TabIndex = 26
        lblPersonInCharge1Position.Text = "Position"
        txtPersonInCharge1Position.Location = New Point(536, 102)
        txtPersonInCharge1Position.Margin = New Padding(3, 2, 3, 2)
        txtPersonInCharge1Position.Name = "txtPersonInCharge1Position"
        txtPersonInCharge1Position.Size = New Size(230, 20)
        txtPersonInCharge1Position.TabIndex = 27
        lblPersonInCharge1Email.Location = New Point(536, 134)
        lblPersonInCharge1Email.Name = "lblPersonInCharge1Email"
        lblPersonInCharge1Email.Size = New Size(70, 13)
        lblPersonInCharge1Email.TabIndex = 28
        lblPersonInCharge1Email.Text = "Email Address"
        txtPersonInCharge1Email.Location = New Point(536, 152)
        txtPersonInCharge1Email.Margin = New Padding(3, 2, 3, 2)
        txtPersonInCharge1Email.Name = "txtPersonInCharge1Email"
        txtPersonInCharge1Email.Size = New Size(230, 20)
        txtPersonInCharge1Email.TabIndex = 29
        lblPersonInCharge2.Location = New Point(536, 184)
        lblPersonInCharge2.Name = "lblPersonInCharge2"
        lblPersonInCharge2.Size = New Size(91, 13)
        lblPersonInCharge2.TabIndex = 30
        lblPersonInCharge2.Text = "Person-in-charge 2"
        lookupPersonInCharge2.Location = New Point(536, 202)
        lookupPersonInCharge2.Margin = New Padding(3, 2, 3, 2)
        lookupPersonInCharge2.Name = "lookupPersonInCharge2"
        lookupPersonInCharge2.Size = New Size(230, 20)
        lookupPersonInCharge2.TabIndex = 31
        lblPersonInCharge2Position.Location = New Point(536, 234)
        lblPersonInCharge2Position.Name = "lblPersonInCharge2Position"
        lblPersonInCharge2Position.Size = New Size(40, 13)
        lblPersonInCharge2Position.TabIndex = 32
        lblPersonInCharge2Position.Text = "Position"
        txtPersonInCharge2Position.Location = New Point(536, 252)
        txtPersonInCharge2Position.Margin = New Padding(3, 2, 3, 2)
        txtPersonInCharge2Position.Name = "txtPersonInCharge2Position"
        txtPersonInCharge2Position.Size = New Size(230, 20)
        txtPersonInCharge2Position.TabIndex = 33
        lblPersonInCharge2Email.Location = New Point(536, 284)
        lblPersonInCharge2Email.Name = "lblPersonInCharge2Email"
        lblPersonInCharge2Email.Size = New Size(70, 13)
        lblPersonInCharge2Email.TabIndex = 34
        lblPersonInCharge2Email.Text = "Email Address"
        txtPersonInCharge2Email.Location = New Point(536, 302)
        txtPersonInCharge2Email.Margin = New Padding(3, 2, 3, 2)
        txtPersonInCharge2Email.Name = "txtPersonInCharge2Email"
        txtPersonInCharge2Email.Size = New Size(230, 20)
        txtPersonInCharge2Email.TabIndex = 35
        '
        ' Column 4 (x=792): Remarks - MemoEdit
        '
        lblRemarks.Location = New Point(792, 34)
        lblRemarks.Name = "lblRemarks"
        lblRemarks.Size = New Size(45, 13)
        lblRemarks.TabIndex = 36
        lblRemarks.Text = "Remarks"
        memoRemarks.Location = New Point(792, 52)
        memoRemarks.Margin = New Padding(3, 2, 3, 2)
        memoRemarks.Name = "memoRemarks"
        memoRemarks.Size = New Size(230, 270)
        memoRemarks.TabIndex = 37
        '
        ' ucCompanyAgencyRegistration
        '
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpDetails)
        Controls.Add(PanelControl1)
        Name = "ucCompanyAgencyRegistration"
        Padding = New Padding(4)
        Size = New Size(1077, 464)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(memoRemarks.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPersonInCharge2Email.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPersonInCharge2Position.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupPersonInCharge2.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPersonInCharge1Email.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPersonInCharge1Position.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupPersonInCharge1.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtContactPersonEmail.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtContactPersonPosition.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(lookupContactPerson.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtFaxNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtTelephoneNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPostCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCountry.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress3.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress2.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress1.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtBranch.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtRegistrationNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents memoRemarks As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblRemarks As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPersonInCharge2Email As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPersonInCharge2Email As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPersonInCharge2Position As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPersonInCharge2Position As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupPersonInCharge2 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblPersonInCharge2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPersonInCharge1Email As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPersonInCharge1Email As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPersonInCharge1Position As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPersonInCharge1Position As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupPersonInCharge1 As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblPersonInCharge1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtContactPersonEmail As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblContactPersonEmail As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtContactPersonPosition As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblContactPersonPosition As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lookupContactPerson As DevExpress.XtraEditors.LookUpEdit
    Friend WithEvents lblContactPerson As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtFaxNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblFaxNo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTelephoneNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblTelephoneNo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPostCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPostCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCountry As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCountry As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtBranch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblBranch As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtRegistrationNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblRegistrationNo As DevExpress.XtraEditors.LabelControl

End Class