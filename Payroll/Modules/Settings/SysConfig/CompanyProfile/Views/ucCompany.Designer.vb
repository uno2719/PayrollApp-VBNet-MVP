' File: Modules/Settings/SysConfig/CompanyProfile/Views/ucCompany.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucCompany
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
        grpLogo = New DevExpress.XtraEditors.GroupControl()
        btnUploadLogo = New DevExpress.XtraEditors.SimpleButton()
        picLogo = New DevExpress.XtraEditors.PictureEdit()
        grpDetails = New DevExpress.XtraEditors.GroupControl()
        chkActive = New DevExpress.XtraEditors.CheckEdit()
        txtContactPersonEmail = New DevExpress.XtraEditors.TextEdit()
        lblContactPersonEmail = New DevExpress.XtraEditors.LabelControl()
        txtContactPersonPosition = New DevExpress.XtraEditors.TextEdit()
        lblContactPersonPosition = New DevExpress.XtraEditors.LabelControl()
        txtContactPerson = New DevExpress.XtraEditors.TextEdit()
        lblContactPerson = New DevExpress.XtraEditors.LabelControl()
        txtFaxNo = New DevExpress.XtraEditors.TextEdit()
        lblFaxNo = New DevExpress.XtraEditors.LabelControl()
        txtTelephoneNo = New DevExpress.XtraEditors.TextEdit()
        lblTelephoneNo = New DevExpress.XtraEditors.LabelControl()
        txtPostCode = New DevExpress.XtraEditors.TextEdit()
        lblPostCode = New DevExpress.XtraEditors.LabelControl()
        txtCountry = New DevExpress.XtraEditors.TextEdit()
        lblCountry = New DevExpress.XtraEditors.LabelControl()
        txtWebsite = New DevExpress.XtraEditors.TextEdit()
        lblWebsite = New DevExpress.XtraEditors.LabelControl()
        txtAddress3 = New DevExpress.XtraEditors.TextEdit()
        lblAddress3 = New DevExpress.XtraEditors.LabelControl()
        txtAddress2 = New DevExpress.XtraEditors.TextEdit()
        lblAddress2 = New DevExpress.XtraEditors.LabelControl()
        txtAddress1 = New DevExpress.XtraEditors.TextEdit()
        lblAddress1 = New DevExpress.XtraEditors.LabelControl()
        txtSECRegistrationNo = New DevExpress.XtraEditors.TextEdit()
        lblSECRegistrationNo = New DevExpress.XtraEditors.LabelControl()
        txtIndustry = New DevExpress.XtraEditors.TextEdit()
        lblIndustry = New DevExpress.XtraEditors.LabelControl()
        txtCompanyName = New DevExpress.XtraEditors.TextEdit()
        lblCompanyName = New DevExpress.XtraEditors.LabelControl()
        txtCompanyCode = New DevExpress.XtraEditors.TextEdit()
        lblCompanyCode = New DevExpress.XtraEditors.LabelControl()
        CType(PanelControl1, ComponentModel.ISupportInitialize).BeginInit()
        PanelControl1.SuspendLayout()
        CType(grpLogo, ComponentModel.ISupportInitialize).BeginInit()
        grpLogo.SuspendLayout()
        CType(picLogo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(grpDetails, ComponentModel.ISupportInitialize).BeginInit()
        grpDetails.SuspendLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtContactPersonEmail.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtContactPersonPosition.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtContactPerson.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtFaxNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtTelephoneNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPostCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCountry.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtWebsite.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress3.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress2.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtAddress1.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSECRegistrationNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtIndustry.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCompanyName.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtCompanyCode.Properties, ComponentModel.ISupportInitialize).BeginInit()
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
        PanelControl1.Size = New Size(1242, 56)
        PanelControl1.TabIndex = 0
        ' 
        ' lblTabPageTitle
        ' 
        lblTabPageTitle.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left
        lblTabPageTitle.Appearance.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold)
        lblTabPageTitle.Appearance.Options.UseFont = True
        lblTabPageTitle.Location = New Point(8, 17)
        lblTabPageTitle.Name = "lblTabPageTitle"
        lblTabPageTitle.Size = New Size(196, 30)
        lblTabPageTitle.TabIndex = 0
        lblTabPageTitle.Text = "COMPANY PROFILE"
        ' 
        ' btnSave
        ' 
        btnSave.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSave.Location = New Point(1142, 12)
        btnSave.Margin = New Padding(3, 2, 3, 2)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(90, 32)
        btnSave.TabIndex = 1
        btnSave.Text = "Save"
        ' 
        ' grpLogo
        ' 
        grpLogo.Appearance.Options.UseFont = True
        grpLogo.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpLogo.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpLogo.AppearanceCaption.Options.UseFont = True
        grpLogo.Controls.Add(btnUploadLogo)
        grpLogo.Controls.Add(picLogo)
        grpLogo.Dock = DockStyle.Left
        grpLogo.Location = New Point(4, 60)
        grpLogo.Margin = New Padding(3, 2, 3, 2)
        grpLogo.Name = "grpLogo"
        grpLogo.Size = New Size(200, 455)
        grpLogo.TabIndex = 1
        grpLogo.Text = " LOGO"
        ' 
        ' btnUploadLogo
        ' 
        btnUploadLogo.Location = New Point(20, 200)
        btnUploadLogo.Margin = New Padding(3, 2, 3, 2)
        btnUploadLogo.Name = "btnUploadLogo"
        btnUploadLogo.Size = New Size(160, 30)
        btnUploadLogo.TabIndex = 1
        btnUploadLogo.Text = "Upload Logo"
        ' 
        ' picLogo
        ' 
        picLogo.Location = New Point(20, 30)
        picLogo.Margin = New Padding(3, 2, 3, 2)
        picLogo.Name = "picLogo"
        picLogo.Properties.Padding = New Padding(5)
        picLogo.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        picLogo.Size = New Size(160, 160)
        picLogo.TabIndex = 0
        ' 
        ' grpDetails
        ' 
        grpDetails.Appearance.Options.UseFont = True
        grpDetails.AppearanceCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        grpDetails.AppearanceCaption.FontStyleDelta = FontStyle.Bold
        grpDetails.AppearanceCaption.Options.UseFont = True
        grpDetails.Controls.Add(chkActive)
        grpDetails.Controls.Add(txtContactPersonEmail)
        grpDetails.Controls.Add(lblContactPersonEmail)
        grpDetails.Controls.Add(txtContactPersonPosition)
        grpDetails.Controls.Add(lblContactPersonPosition)
        grpDetails.Controls.Add(txtContactPerson)
        grpDetails.Controls.Add(lblContactPerson)
        grpDetails.Controls.Add(txtFaxNo)
        grpDetails.Controls.Add(lblFaxNo)
        grpDetails.Controls.Add(txtTelephoneNo)
        grpDetails.Controls.Add(lblTelephoneNo)
        grpDetails.Controls.Add(txtPostCode)
        grpDetails.Controls.Add(lblPostCode)
        grpDetails.Controls.Add(txtCountry)
        grpDetails.Controls.Add(lblCountry)
        grpDetails.Controls.Add(txtWebsite)
        grpDetails.Controls.Add(lblWebsite)
        grpDetails.Controls.Add(txtAddress3)
        grpDetails.Controls.Add(lblAddress3)
        grpDetails.Controls.Add(txtAddress2)
        grpDetails.Controls.Add(lblAddress2)
        grpDetails.Controls.Add(txtAddress1)
        grpDetails.Controls.Add(lblAddress1)
        grpDetails.Controls.Add(txtSECRegistrationNo)
        grpDetails.Controls.Add(lblSECRegistrationNo)
        grpDetails.Controls.Add(txtIndustry)
        grpDetails.Controls.Add(lblIndustry)
        grpDetails.Controls.Add(txtCompanyName)
        grpDetails.Controls.Add(lblCompanyName)
        grpDetails.Controls.Add(txtCompanyCode)
        grpDetails.Controls.Add(lblCompanyCode)
        grpDetails.Dock = DockStyle.Fill
        grpDetails.Location = New Point(204, 60)
        grpDetails.Margin = New Padding(3, 2, 3, 2)
        grpDetails.Name = "grpDetails"
        grpDetails.Size = New Size(1042, 455)
        grpDetails.TabIndex = 2
        grpDetails.Text = " COMPANY INFORMATION"
        ' 
        ' chkActive
        ' 
        chkActive.Location = New Point(792, 184)
        chkActive.Margin = New Padding(3, 2, 3, 2)
        chkActive.Name = "chkActive"
        chkActive.Properties.Caption = "Active"
        chkActive.Size = New Size(230, 20)
        chkActive.TabIndex = 30
        ' 
        ' txtContactPersonEmail
        ' 
        txtContactPersonEmail.Location = New Point(792, 152)
        txtContactPersonEmail.Margin = New Padding(3, 2, 3, 2)
        txtContactPersonEmail.Name = "txtContactPersonEmail"
        txtContactPersonEmail.Size = New Size(230, 20)
        txtContactPersonEmail.TabIndex = 29
        ' 
        ' lblContactPersonEmail
        ' 
        lblContactPersonEmail.Location = New Point(792, 134)
        lblContactPersonEmail.Name = "lblContactPersonEmail"
        lblContactPersonEmail.Size = New Size(66, 13)
        lblContactPersonEmail.TabIndex = 28
        lblContactPersonEmail.Text = "Email Address"
        ' 
        ' txtContactPersonPosition
        ' 
        txtContactPersonPosition.Location = New Point(792, 102)
        txtContactPersonPosition.Margin = New Padding(3, 2, 3, 2)
        txtContactPersonPosition.Name = "txtContactPersonPosition"
        txtContactPersonPosition.Size = New Size(230, 20)
        txtContactPersonPosition.TabIndex = 27
        ' 
        ' lblContactPersonPosition
        ' 
        lblContactPersonPosition.Location = New Point(792, 84)
        lblContactPersonPosition.Name = "lblContactPersonPosition"
        lblContactPersonPosition.Size = New Size(37, 13)
        lblContactPersonPosition.TabIndex = 26
        lblContactPersonPosition.Text = "Position"
        ' 
        ' txtContactPerson
        ' 
        txtContactPerson.Location = New Point(792, 52)
        txtContactPerson.Margin = New Padding(3, 2, 3, 2)
        txtContactPerson.Name = "txtContactPerson"
        txtContactPerson.Size = New Size(230, 20)
        txtContactPerson.TabIndex = 25
        ' 
        ' lblContactPerson
        ' 
        lblContactPerson.Location = New Point(792, 34)
        lblContactPerson.Name = "lblContactPerson"
        lblContactPerson.Size = New Size(74, 13)
        lblContactPerson.TabIndex = 24
        lblContactPerson.Text = "Contact Person"
        ' 
        ' txtFaxNo
        ' 
        txtFaxNo.Location = New Point(536, 202)
        txtFaxNo.Margin = New Padding(3, 2, 3, 2)
        txtFaxNo.Name = "txtFaxNo"
        txtFaxNo.Size = New Size(230, 20)
        txtFaxNo.TabIndex = 23
        ' 
        ' lblFaxNo
        ' 
        lblFaxNo.Location = New Point(536, 184)
        lblFaxNo.Name = "lblFaxNo"
        lblFaxNo.Size = New Size(38, 13)
        lblFaxNo.TabIndex = 22
        lblFaxNo.Text = "Fax No."
        ' 
        ' txtTelephoneNo
        ' 
        txtTelephoneNo.Location = New Point(536, 152)
        txtTelephoneNo.Margin = New Padding(3, 2, 3, 2)
        txtTelephoneNo.Name = "txtTelephoneNo"
        txtTelephoneNo.Size = New Size(230, 20)
        txtTelephoneNo.TabIndex = 21
        ' 
        ' lblTelephoneNo
        ' 
        lblTelephoneNo.Location = New Point(536, 134)
        lblTelephoneNo.Name = "lblTelephoneNo"
        lblTelephoneNo.Size = New Size(70, 13)
        lblTelephoneNo.TabIndex = 20
        lblTelephoneNo.Text = "Telephone No."
        ' 
        ' txtPostCode
        ' 
        txtPostCode.Location = New Point(536, 102)
        txtPostCode.Margin = New Padding(3, 2, 3, 2)
        txtPostCode.Name = "txtPostCode"
        txtPostCode.Size = New Size(230, 20)
        txtPostCode.TabIndex = 19
        ' 
        ' lblPostCode
        ' 
        lblPostCode.Location = New Point(536, 84)
        lblPostCode.Name = "lblPostCode"
        lblPostCode.Size = New Size(49, 13)
        lblPostCode.TabIndex = 18
        lblPostCode.Text = "Post Code"
        ' 
        ' txtCountry
        ' 
        txtCountry.Location = New Point(536, 52)
        txtCountry.Margin = New Padding(3, 2, 3, 2)
        txtCountry.Name = "txtCountry"
        txtCountry.Size = New Size(230, 20)
        txtCountry.TabIndex = 17
        ' 
        ' lblCountry
        ' 
        lblCountry.Location = New Point(536, 34)
        lblCountry.Name = "lblCountry"
        lblCountry.Size = New Size(39, 13)
        lblCountry.TabIndex = 16
        lblCountry.Text = "Country"
        ' 
        ' txtWebsite
        ' 
        txtWebsite.Location = New Point(280, 202)
        txtWebsite.Margin = New Padding(3, 2, 3, 2)
        txtWebsite.Name = "txtWebsite"
        txtWebsite.Size = New Size(230, 20)
        txtWebsite.TabIndex = 15
        ' 
        ' lblWebsite
        ' 
        lblWebsite.Location = New Point(280, 184)
        lblWebsite.Name = "lblWebsite"
        lblWebsite.Size = New Size(39, 13)
        lblWebsite.TabIndex = 14
        lblWebsite.Text = "Website"
        ' 
        ' txtAddress3
        ' 
        txtAddress3.Location = New Point(280, 152)
        txtAddress3.Margin = New Padding(3, 2, 3, 2)
        txtAddress3.Name = "txtAddress3"
        txtAddress3.Size = New Size(230, 20)
        txtAddress3.TabIndex = 13
        ' 
        ' lblAddress3
        ' 
        lblAddress3.Location = New Point(280, 134)
        lblAddress3.Name = "lblAddress3"
        lblAddress3.Size = New Size(48, 13)
        lblAddress3.TabIndex = 12
        lblAddress3.Text = "Address 3"
        ' 
        ' txtAddress2
        ' 
        txtAddress2.Location = New Point(280, 102)
        txtAddress2.Margin = New Padding(3, 2, 3, 2)
        txtAddress2.Name = "txtAddress2"
        txtAddress2.Size = New Size(230, 20)
        txtAddress2.TabIndex = 11
        ' 
        ' lblAddress2
        ' 
        lblAddress2.Location = New Point(280, 84)
        lblAddress2.Name = "lblAddress2"
        lblAddress2.Size = New Size(48, 13)
        lblAddress2.TabIndex = 10
        lblAddress2.Text = "Address 2"
        ' 
        ' txtAddress1
        ' 
        txtAddress1.Location = New Point(280, 52)
        txtAddress1.Margin = New Padding(3, 2, 3, 2)
        txtAddress1.Name = "txtAddress1"
        txtAddress1.Size = New Size(230, 20)
        txtAddress1.TabIndex = 9
        ' 
        ' lblAddress1
        ' 
        lblAddress1.Location = New Point(280, 34)
        lblAddress1.Name = "lblAddress1"
        lblAddress1.Size = New Size(48, 13)
        lblAddress1.TabIndex = 8
        lblAddress1.Text = "Address 1"
        ' 
        ' txtSECRegistrationNo
        ' 
        txtSECRegistrationNo.Location = New Point(24, 202)
        txtSECRegistrationNo.Margin = New Padding(3, 2, 3, 2)
        txtSECRegistrationNo.Name = "txtSECRegistrationNo"
        txtSECRegistrationNo.Size = New Size(230, 20)
        txtSECRegistrationNo.TabIndex = 7
        ' 
        ' lblSECRegistrationNo
        ' 
        lblSECRegistrationNo.Location = New Point(24, 184)
        lblSECRegistrationNo.Name = "lblSECRegistrationNo"
        lblSECRegistrationNo.Size = New Size(100, 13)
        lblSECRegistrationNo.TabIndex = 6
        lblSECRegistrationNo.Text = "SEC Registration No."
        ' 
        ' txtIndustry
        ' 
        txtIndustry.Location = New Point(24, 152)
        txtIndustry.Margin = New Padding(3, 2, 3, 2)
        txtIndustry.Name = "txtIndustry"
        txtIndustry.Size = New Size(230, 20)
        txtIndustry.TabIndex = 5
        ' 
        ' lblIndustry
        ' 
        lblIndustry.Location = New Point(24, 134)
        lblIndustry.Name = "lblIndustry"
        lblIndustry.Size = New Size(41, 13)
        lblIndustry.TabIndex = 4
        lblIndustry.Text = "Industry"
        ' 
        ' txtCompanyName
        ' 
        txtCompanyName.Location = New Point(24, 102)
        txtCompanyName.Margin = New Padding(3, 2, 3, 2)
        txtCompanyName.Name = "txtCompanyName"
        txtCompanyName.Size = New Size(230, 20)
        txtCompanyName.TabIndex = 3
        ' 
        ' lblCompanyName
        ' 
        lblCompanyName.Location = New Point(24, 84)
        lblCompanyName.Name = "lblCompanyName"
        lblCompanyName.Size = New Size(75, 13)
        lblCompanyName.TabIndex = 2
        lblCompanyName.Text = "Company Name"
        ' 
        ' txtCompanyCode
        ' 
        txtCompanyCode.Location = New Point(24, 52)
        txtCompanyCode.Margin = New Padding(3, 2, 3, 2)
        txtCompanyCode.Name = "txtCompanyCode"
        txtCompanyCode.Size = New Size(230, 20)
        txtCompanyCode.TabIndex = 1
        ' 
        ' lblCompanyCode
        ' 
        lblCompanyCode.Location = New Point(24, 34)
        lblCompanyCode.Name = "lblCompanyCode"
        lblCompanyCode.Size = New Size(73, 13)
        lblCompanyCode.TabIndex = 0
        lblCompanyCode.Text = "Company Code"
        ' 
        ' ucCompany
        ' 
        AutoScaleDimensions = New SizeF(6F, 13F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(grpDetails)
        Controls.Add(grpLogo)
        Controls.Add(PanelControl1)
        Name = "ucCompany"
        Padding = New Padding(4)
        Size = New Size(1250, 519)
        CType(PanelControl1, ComponentModel.ISupportInitialize).EndInit()
        PanelControl1.ResumeLayout(False)
        PanelControl1.PerformLayout()
        CType(grpLogo, ComponentModel.ISupportInitialize).EndInit()
        grpLogo.ResumeLayout(False)
        CType(picLogo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(grpDetails, ComponentModel.ISupportInitialize).EndInit()
        grpDetails.ResumeLayout(False)
        grpDetails.PerformLayout()
        CType(chkActive.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtContactPersonEmail.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtContactPersonPosition.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtContactPerson.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtFaxNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtTelephoneNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPostCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCountry.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtWebsite.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress3.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress2.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtAddress1.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSECRegistrationNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtIndustry.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCompanyName.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtCompanyCode.Properties, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblTabPageTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnSave As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents grpLogo As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnUploadLogo As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents picLogo As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents grpDetails As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkActive As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents txtContactPersonEmail As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblContactPersonEmail As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtContactPersonPosition As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblContactPersonPosition As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtContactPerson As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblContactPerson As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtFaxNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblFaxNo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtTelephoneNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblTelephoneNo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtPostCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblPostCode As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCountry As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCountry As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtWebsite As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblWebsite As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress3 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress2 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtAddress1 As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblAddress1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtSECRegistrationNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSECRegistrationNo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtIndustry As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblIndustry As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCompanyName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCompanyName As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtCompanyCode As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblCompanyCode As DevExpress.XtraEditors.LabelControl

End Class