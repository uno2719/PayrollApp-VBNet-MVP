<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucThemeSettings
    Inherits GlobalShared.Base.ucBase

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Me.pnlHeader = New DevExpress.XtraEditors.PanelControl()
        Me.rgFilter = New DevExpress.XtraEditors.RadioGroup()
        Me.txtSearch = New DevExpress.XtraEditors.TextEdit()
        Me.lblSubtitle = New DevExpress.XtraEditors.LabelControl()
        Me.lblTitle = New DevExpress.XtraEditors.LabelControl()
        Me.pnlFooter = New DevExpress.XtraEditors.PanelControl()
        Me.lblStatus = New DevExpress.XtraEditors.LabelControl()
        Me.btnReset = New DevExpress.XtraEditors.SimpleButton()
        Me.btnUndo = New DevExpress.XtraEditors.SimpleButton()
        Me.pnlRight = New DevExpress.XtraEditors.PanelControl()
        Me.grpOptions = New DevExpress.XtraEditors.GroupControl()
        Me.chkRounded = New DevExpress.XtraEditors.CheckEdit()
        Me.lblFontValue = New DevExpress.XtraEditors.LabelControl()
        Me.trackFont = New DevExpress.XtraEditors.TrackBarControl()
        Me.lblFont = New DevExpress.XtraEditors.LabelControl()
        Me.grpPalette = New DevExpress.XtraEditors.GroupControl()
        Me.btnChoosePalette = New DevExpress.XtraEditors.SimpleButton()
        Me.lblCurrentPalette = New DevExpress.XtraEditors.LabelControl()
        Me.lblCurrentSkin = New DevExpress.XtraEditors.LabelControl()
        Me.pnlSkins = New DevExpress.XtraEditors.PanelControl()
        Me.flowSkins = New System.Windows.Forms.FlowLayoutPanel()
        Me.tmrStatus = New System.Windows.Forms.Timer(Me.components)
        CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHeader.SuspendLayout()
        CType(Me.rgFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pnlFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        CType(Me.pnlRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRight.SuspendLayout()
        CType(Me.grpOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpOptions.SuspendLayout()
        CType(Me.chkRounded.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackFont, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trackFont.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grpPalette, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpPalette.SuspendLayout()
        CType(Me.pnlSkins, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSkins.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnlHeader.Controls.Add(Me.rgFilter)
        Me.pnlHeader.Controls.Add(Me.txtSearch)
        Me.pnlHeader.Controls.Add(Me.lblSubtitle)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(980, 84)
        Me.pnlHeader.TabIndex = 3
        '
        'rgFilter
        '
        Me.rgFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rgFilter.EditValue = 0
        Me.rgFilter.Location = New System.Drawing.Point(540, 46)
        Me.rgFilter.Name = "rgFilter"
        Me.rgFilter.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.rgFilter.Properties.Columns = 4
        Me.rgFilter.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {
            New DevExpress.XtraEditors.Controls.RadioGroupItem(0, "All"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Light"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Dark"),
            New DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Favorites")})
        Me.rgFilter.Size = New System.Drawing.Size(424, 28)
        Me.rgFilter.TabIndex = 3
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(704, 14)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Properties.NullValuePrompt = "Search skins..."
        Me.txtSearch.Properties.NullValuePromptShowForEmptyValue = True
        Me.txtSearch.Size = New System.Drawing.Size(260, 28)
        Me.txtSearch.TabIndex = 2
        '
        'lblSubtitle
        '
        Me.lblSubtitle.Appearance.Options.UseFont = True
        Me.lblSubtitle.Location = New System.Drawing.Point(22, 50)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(310, 16)
        Me.lblSubtitle.TabIndex = 1
        Me.lblSubtitle.Text = "Click a skin to apply it. Changes are saved automatically."
        '
        'lblTitle
        '
        Me.lblTitle.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 18.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Appearance.Options.UseFont = True
        Me.lblTitle.Location = New System.Drawing.Point(20, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(205, 32)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Theme && Appearance"
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.lblStatus)
        Me.pnlFooter.Controls.Add(Me.btnReset)
        Me.pnlFooter.Controls.Add(Me.btnUndo)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 584)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(980, 56)
        Me.pnlFooter.TabIndex = 2
        '
        'lblStatus
        '
        Me.lblStatus.Appearance.ForeColor = System.Drawing.Color.SeaGreen
        Me.lblStatus.Appearance.Options.UseForeColor = True
        Me.lblStatus.Location = New System.Drawing.Point(320, 20)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 16)
        Me.lblStatus.TabIndex = 2
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(154, 12)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(140, 32)
        Me.btnReset.TabIndex = 1
        Me.btnReset.Text = "Reset to Default"
        '
        'btnUndo
        '
        Me.btnUndo.Enabled = False
        Me.btnUndo.Location = New System.Drawing.Point(16, 12)
        Me.btnUndo.Name = "btnUndo"
        Me.btnUndo.Size = New System.Drawing.Size(130, 32)
        Me.btnUndo.TabIndex = 0
        Me.btnUndo.Text = "Undo changes"
        '
        'pnlRight
        '
        Me.pnlRight.Controls.Add(Me.grpOptions)
        Me.pnlRight.Controls.Add(Me.grpPalette)
        Me.pnlRight.Controls.Add(Me.lblCurrentPalette)
        Me.pnlRight.Controls.Add(Me.lblCurrentSkin)
        Me.pnlRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlRight.Location = New System.Drawing.Point(680, 84)
        Me.pnlRight.Name = "pnlRight"
        Me.pnlRight.Size = New System.Drawing.Size(300, 500)
        Me.pnlRight.TabIndex = 1
        '
        'grpOptions
        '
        Me.grpOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpOptions.Controls.Add(Me.chkRounded)
        Me.grpOptions.Controls.Add(Me.lblFontValue)
        Me.grpOptions.Controls.Add(Me.trackFont)
        Me.grpOptions.Controls.Add(Me.lblFont)
        Me.grpOptions.Location = New System.Drawing.Point(12, 170)
        Me.grpOptions.Name = "grpOptions"
        Me.grpOptions.Size = New System.Drawing.Size(276, 160)
        Me.grpOptions.TabIndex = 3
        Me.grpOptions.Text = "Interface"
        '
        'chkRounded
        '
        Me.chkRounded.Location = New System.Drawing.Point(12, 112)
        Me.chkRounded.Name = "chkRounded"
        Me.chkRounded.Properties.Caption = "Rounded window corners (Windows 11)"
        Me.chkRounded.Size = New System.Drawing.Size(250, 22)
        Me.chkRounded.TabIndex = 3
        '
        'lblFontValue
        '
        Me.lblFontValue.Location = New System.Drawing.Point(214, 36)
        Me.lblFontValue.Name = "lblFontValue"
        Me.lblFontValue.Size = New System.Drawing.Size(30, 16)
        Me.lblFontValue.TabIndex = 2
        Me.lblFontValue.Text = "9 pt"
        '
        'trackFont
        '
        Me.trackFont.EditValue = 9
        Me.trackFont.Location = New System.Drawing.Point(8, 56)
        Me.trackFont.Name = "trackFont"
        Me.trackFont.Properties.LargeChange = 1
        Me.trackFont.Properties.Maximum = 14
        Me.trackFont.Properties.Minimum = 8
        Me.trackFont.Properties.ShowValueToolTip = True
        Me.trackFont.Properties.SmallChange = 1
        Me.trackFont.Properties.TickFrequency = 1
        Me.trackFont.Size = New System.Drawing.Size(258, 45)
        Me.trackFont.TabIndex = 1
        Me.trackFont.Value = 9
        '
        'lblFont
        '
        Me.lblFont.Location = New System.Drawing.Point(12, 36)
        Me.lblFont.Name = "lblFont"
        Me.lblFont.Size = New System.Drawing.Size(80, 16)
        Me.lblFont.TabIndex = 0
        Me.lblFont.Text = "Text size"
        '
        'grpPalette
        '
        Me.grpPalette.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpPalette.Controls.Add(Me.btnChoosePalette)
        Me.grpPalette.Location = New System.Drawing.Point(12, 86)
        Me.grpPalette.Name = "grpPalette"
        Me.grpPalette.Size = New System.Drawing.Size(276, 72)
        Me.grpPalette.TabIndex = 2
        Me.grpPalette.Text = "Color palette"
        '
        'btnChoosePalette
        '
        Me.btnChoosePalette.Location = New System.Drawing.Point(12, 30)
        Me.btnChoosePalette.Name = "btnChoosePalette"
        Me.btnChoosePalette.Size = New System.Drawing.Size(252, 32)
        Me.btnChoosePalette.TabIndex = 0
        Me.btnChoosePalette.Text = "Choose color palette..."
        '
        'lblCurrentPalette
        '
        Me.lblCurrentPalette.Location = New System.Drawing.Point(16, 54)
        Me.lblCurrentPalette.Name = "lblCurrentPalette"
        Me.lblCurrentPalette.Size = New System.Drawing.Size(85, 16)
        Me.lblCurrentPalette.TabIndex = 1
        Me.lblCurrentPalette.Text = "Default palette"
        '
        'lblCurrentSkin
        '
        Me.lblCurrentSkin.Appearance.Font = New System.Drawing.Font("Segoe UI Semibold", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblCurrentSkin.Appearance.Options.UseFont = True
        Me.lblCurrentSkin.Location = New System.Drawing.Point(14, 16)
        Me.lblCurrentSkin.Name = "lblCurrentSkin"
        Me.lblCurrentSkin.Size = New System.Drawing.Size(48, 30)
        Me.lblCurrentSkin.TabIndex = 0
        Me.lblCurrentSkin.Text = "WXI"
        '
        'pnlSkins
        '
        Me.pnlSkins.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.pnlSkins.Controls.Add(Me.flowSkins)
        Me.pnlSkins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSkins.Location = New System.Drawing.Point(0, 84)
        Me.pnlSkins.Name = "pnlSkins"
        Me.pnlSkins.Size = New System.Drawing.Size(680, 500)
        Me.pnlSkins.TabIndex = 0
        '
        'flowSkins
        '
        Me.flowSkins.AutoScroll = True
        Me.flowSkins.BackColor = System.Drawing.Color.Transparent
        Me.flowSkins.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flowSkins.Location = New System.Drawing.Point(0, 0)
        Me.flowSkins.Name = "flowSkins"
        Me.flowSkins.Padding = New System.Windows.Forms.Padding(10)
        Me.flowSkins.Size = New System.Drawing.Size(680, 500)
        Me.flowSkins.TabIndex = 0
        '
        'tmrStatus
        '
        Me.tmrStatus.Interval = 2200
        '
        'ucThemeSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlSkins)
        Me.Controls.Add(Me.pnlRight)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.pnlHeader)
        Me.Name = "ucThemeSettings"
        Me.Size = New System.Drawing.Size(980, 640)
        CType(Me.pnlHeader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        CType(Me.rgFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtSearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pnlFooter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.pnlFooter.PerformLayout()
        CType(Me.pnlRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRight.ResumeLayout(False)
        Me.pnlRight.PerformLayout()
        CType(Me.grpOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpOptions.ResumeLayout(False)
        Me.grpOptions.PerformLayout()
        CType(Me.chkRounded.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackFont.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trackFont, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grpPalette, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpPalette.ResumeLayout(False)
        CType(Me.pnlSkins, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSkins.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As DevExpress.XtraEditors.PanelControl
    Friend WithEvents rgFilter As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents txtSearch As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblSubtitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblTitle As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlFooter As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblStatus As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnReset As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnUndo As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents pnlRight As DevExpress.XtraEditors.PanelControl
    Friend WithEvents grpOptions As DevExpress.XtraEditors.GroupControl
    Friend WithEvents chkRounded As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblFontValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents trackFont As DevExpress.XtraEditors.TrackBarControl
    Friend WithEvents lblFont As DevExpress.XtraEditors.LabelControl
    Friend WithEvents grpPalette As DevExpress.XtraEditors.GroupControl
    Friend WithEvents btnChoosePalette As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblCurrentPalette As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblCurrentSkin As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlSkins As DevExpress.XtraEditors.PanelControl
    Friend WithEvents flowSkins As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents tmrStatus As System.Windows.Forms.Timer

End Class
