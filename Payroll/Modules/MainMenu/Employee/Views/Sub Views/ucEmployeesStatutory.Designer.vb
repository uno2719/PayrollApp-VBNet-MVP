<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucEmployeesStatutory
    Inherits DevExpress.XtraEditors.XtraUserControl

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
        lcStatutory = New DevExpress.XtraLayout.LayoutControl()
        Root_Statutory = New DevExpress.XtraLayout.LayoutControlGroup()
        SeparatorHeader = New DevExpress.XtraEditors.SeparatorControl()
        txtTIN = New DevExpress.XtraEditors.TextEdit()
        txtSSSNo = New DevExpress.XtraEditors.TextEdit()
        txtPagIBIGNo = New DevExpress.XtraEditors.TextEdit()
        txtPagIBIGVoluntary = New DevExpress.XtraEditors.TextEdit()
        txtPhilHealthNo = New DevExpress.XtraEditors.TextEdit()
        txtFixTax = New DevExpress.XtraEditors.TextEdit()
        lciHeader = New DevExpress.XtraLayout.LayoutControlItem()
        lciTIN = New DevExpress.XtraLayout.LayoutControlItem()
        lciSSSNo = New DevExpress.XtraLayout.LayoutControlItem()
        lciPagIBIGNo = New DevExpress.XtraLayout.LayoutControlItem()
        lciPagIBIGVoluntary = New DevExpress.XtraLayout.LayoutControlItem()
        lciPhilHealthNo = New DevExpress.XtraLayout.LayoutControlItem()
        lciFixTax = New DevExpress.XtraLayout.LayoutControlItem()
        emptyBottom = New DevExpress.XtraLayout.EmptySpaceItem()

        CType(lcStatutory, ComponentModel.ISupportInitialize).BeginInit()
        lcStatutory.SuspendLayout()
        CType(txtTIN.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtSSSNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPagIBIGNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPagIBIGVoluntary.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtPhilHealthNo.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(txtFixTax.Properties, ComponentModel.ISupportInitialize).BeginInit()
        CType(SeparatorHeader, ComponentModel.ISupportInitialize).BeginInit()
        CType(Root_Statutory, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciHeader, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciTIN, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciSSSNo, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPagIBIGNo, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPagIBIGVoluntary, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciPhilHealthNo, ComponentModel.ISupportInitialize).BeginInit()
        CType(lciFixTax, ComponentModel.ISupportInitialize).BeginInit()
        CType(emptyBottom, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()

        ' =============================================
        ' lcStatutory
        ' =============================================
        lcStatutory.AllowCustomization = False
        lcStatutory.Appearance.Control.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lcStatutory.Appearance.Control.Options.UseFont = True
        lcStatutory.Controls.Add(txtTIN)
        lcStatutory.Controls.Add(txtSSSNo)
        lcStatutory.Controls.Add(txtPagIBIGNo)
        lcStatutory.Controls.Add(txtPagIBIGVoluntary)
        lcStatutory.Controls.Add(txtPhilHealthNo)
        lcStatutory.Controls.Add(txtFixTax)
        lcStatutory.Controls.Add(SeparatorHeader)
        lcStatutory.Dock = DockStyle.Fill
        lcStatutory.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lcStatutory.Location = New Point(0, 0)
        lcStatutory.Name = "lcStatutory"
        lcStatutory.Root = Root_Statutory
        lcStatutory.Size = New Size(1067, 713)
        lcStatutory.TabIndex = 0

        ' =============================================
        ' TextEdits
        ' =============================================
        SetupTextEdit(txtTIN, "txtTIN")
        txtTIN.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.SimpleMaskManager))
        txtTIN.Properties.MaskSettings.Set("mask", "000-000-000-000")
        txtTIN.Properties.UseMaskAsDisplayFormat = True

        SetupTextEdit(txtSSSNo, "txtSSSNo")
        txtSSSNo.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.SimpleMaskManager))
        txtSSSNo.Properties.MaskSettings.Set("mask", "00-0000000-0")
        txtSSSNo.Properties.UseMaskAsDisplayFormat = True

        SetupTextEdit(txtPagIBIGNo, "txtPagIBIGNo")
        txtPagIBIGNo.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.SimpleMaskManager))
        txtPagIBIGNo.Properties.MaskSettings.Set("mask", "0000-0000-0000")
        txtPagIBIGNo.Properties.UseMaskAsDisplayFormat = True

        SetupTextEdit(txtPagIBIGVoluntary, "txtPagIBIGVoluntary")
        txtPagIBIGVoluntary.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        txtPagIBIGVoluntary.Properties.MaskSettings.Set("mask", "f2")
        txtPagIBIGVoluntary.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        txtPagIBIGVoluntary.Properties.DisplayFormat.FormatString = "n2"

        SetupTextEdit(txtPhilHealthNo, "txtPhilHealthNo")
        txtPhilHealthNo.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.SimpleMaskManager))
        txtPhilHealthNo.Properties.MaskSettings.Set("mask", "00-000000000-0")
        txtPhilHealthNo.Properties.UseMaskAsDisplayFormat = True

        SetupTextEdit(txtFixTax, "txtFixTax")
        txtFixTax.Properties.MaskSettings.Set("MaskManagerType", GetType(DevExpress.Data.Mask.NumericMaskManager))
        txtFixTax.Properties.MaskSettings.Set("mask", "f2")
        txtFixTax.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        txtFixTax.Properties.DisplayFormat.FormatString = "n2"

        ' =============================================
        ' ROOT LAYOUT GROUP
        ' =============================================
        Root_Statutory.AppearanceItemCaption.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Root_Statutory.AppearanceItemCaption.Options.UseFont = True
        Root_Statutory.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
        Root_Statutory.GroupBordersVisible = False
        Root_Statutory.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {
            lciHeader,
            lciTIN, lciSSSNo, lciPagIBIGNo,
            lciPagIBIGVoluntary, lciPhilHealthNo,
            lciFixTax, emptyBottom})
        Root_Statutory.Name = "Root_Statutory"
        Root_Statutory.Size = New Size(1067, 713)
        Root_Statutory.TextVisible = False

        ' =============================================
        ' HEADER
        ' =============================================
        lciHeader.Control = SeparatorHeader
        lciHeader.Location = New Point(0, 0)
        lciHeader.Name = "lciHeader"
        lciHeader.Size = New Size(1047, 30)
        lciHeader.Text = "Statutory Details"
        lciHeader.AppearanceItemCaption.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        lciHeader.AppearanceItemCaption.Options.UseFont = True
        lciHeader.TextLocation = DevExpress.Utils.Locations.Top
        lciHeader.TextVisible = True

        ' =============================================
        ' LAYOUT ITEMS — single column, left side lang
        ' =============================================
        lciTIN.Control = txtTIN
        lciTIN.Location = New Point(0, 30)
        lciTIN.Name = "lciTIN"
        lciTIN.Size = New Size(521, 26)
        lciTIN.Text = "TIN"
        lciTIN.TextSize = New Size(126, 17)

        lciSSSNo.Control = txtSSSNo
        lciSSSNo.Location = New Point(0, 56)
        lciSSSNo.Name = "lciSSSNo"
        lciSSSNo.Size = New Size(521, 26)
        lciSSSNo.Text = "SSS No."
        lciSSSNo.TextSize = New Size(126, 17)

        lciPagIBIGNo.Control = txtPagIBIGNo
        lciPagIBIGNo.Location = New Point(0, 82)
        lciPagIBIGNo.Name = "lciPagIBIGNo"
        lciPagIBIGNo.Size = New Size(521, 26)
        lciPagIBIGNo.Text = "Pag-IBIG No."
        lciPagIBIGNo.TextSize = New Size(126, 17)

        lciPagIBIGVoluntary.Control = txtPagIBIGVoluntary
        lciPagIBIGVoluntary.Location = New Point(0, 108)
        lciPagIBIGVoluntary.Name = "lciPagIBIGVoluntary"
        lciPagIBIGVoluntary.Size = New Size(521, 26)
        lciPagIBIGVoluntary.Text = "Pag-IBIG Voluntary"
        lciPagIBIGVoluntary.TextSize = New Size(126, 17)

        lciPhilHealthNo.Control = txtPhilHealthNo
        lciPhilHealthNo.Location = New Point(0, 134)
        lciPhilHealthNo.Name = "lciPhilHealthNo"
        lciPhilHealthNo.Size = New Size(521, 26)
        lciPhilHealthNo.Text = "PhilHealth No."
        lciPhilHealthNo.TextSize = New Size(126, 17)

        lciFixTax.Control = txtFixTax
        lciFixTax.Location = New Point(0, 160)
        lciFixTax.Name = "lciFixTax"
        lciFixTax.Size = New Size(521, 26)
        lciFixTax.Text = "Fix Tax"
        lciFixTax.TextSize = New Size(126, 17)

        ' =============================================
        ' EMPTY BOTTOM
        ' =============================================
        emptyBottom.Location = New Point(0, 186)
        emptyBottom.Name = "emptyBottom"
        emptyBottom.Size = New Size(1047, 503)

        ' =============================================
        ' ucEmployeesStatutory
        ' =============================================
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        Controls.Add(lcStatutory)
        Name = "ucEmployeesStatutory"
        Size = New Size(1067, 713)

        CType(lcStatutory, ComponentModel.ISupportInitialize).EndInit()
        lcStatutory.ResumeLayout(False)
        CType(txtTIN.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtSSSNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPagIBIGNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPagIBIGVoluntary.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtPhilHealthNo.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(txtFixTax.Properties, ComponentModel.ISupportInitialize).EndInit()
        CType(SeparatorHeader, ComponentModel.ISupportInitialize).EndInit()
        CType(Root_Statutory, ComponentModel.ISupportInitialize).EndInit()
        CType(lciHeader, ComponentModel.ISupportInitialize).EndInit()
        CType(lciTIN, ComponentModel.ISupportInitialize).EndInit()
        CType(lciSSSNo, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPagIBIGNo, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPagIBIGVoluntary, ComponentModel.ISupportInitialize).EndInit()
        CType(lciPhilHealthNo, ComponentModel.ISupportInitialize).EndInit()
        CType(lciFixTax, ComponentModel.ISupportInitialize).EndInit()
        CType(emptyBottom, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub

    ' =============================================
    ' HELPER METHOD
    ' =============================================
    Private Sub SetupTextEdit(txt As DevExpress.XtraEditors.TextEdit, name As String)
        txt.EnterMoveNextControl = True
        txt.Name = name
        txt.Properties.Appearance.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
        txt.Properties.Appearance.Options.UseFont = True
        txt.Properties.TextPadding = New Padding(5, 0, 5, 0)
    End Sub

    ' =============================================
    ' FIELD DECLARATIONS
    ' =============================================
    Friend WithEvents lcStatutory As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents Root_Statutory As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents SeparatorHeader As DevExpress.XtraEditors.SeparatorControl
    Friend WithEvents txtTIN As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtSSSNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPagIBIGNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPagIBIGVoluntary As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPhilHealthNo As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtFixTax As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lciHeader As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciTIN As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciSSSNo As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPagIBIGNo As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPagIBIGVoluntary As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciPhilHealthNo As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents lciFixTax As DevExpress.XtraLayout.LayoutControlItem
    Friend WithEvents emptyBottom As DevExpress.XtraLayout.EmptySpaceItem

End Class