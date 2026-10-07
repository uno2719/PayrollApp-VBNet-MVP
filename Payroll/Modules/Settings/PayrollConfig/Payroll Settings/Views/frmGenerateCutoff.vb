Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Extensions
Imports Payroll.GlobalShared.Models
Imports Payroll.PayrollSettings.Presenters
Imports Payroll.PayrollSettings.Views

''' <summary>
''' Dialog na lumalabas pag pinindot ang [New] sa Cutoff tab. Pumili ng Pay Cycle at
''' Year, makikita agad ang preview ng mga cutoff na gagawin (ayon sa pattern sa Pay
''' Cycle Settings), at kung alin ang mayroon na (laktaw). Generate = ilalagay ang
''' lahat ng "New".
''' </summary>
Public Class frmGenerateCutoff
    Implements IGenerateCutoffView

    Private _presenter As GenerateCutoffPresenter
    Private _suppressRefresh As Boolean = False

    Public Sub New()
        InitializeComponent()

        colStart.ApplyDisplayDateFormat()
        colEnd.ApplyDisplayDateFormat()
        colPayDate.ApplyDisplayDateFormat()
    End Sub

    Public Sub SetPresenter(presenter As GenerateCutoffPresenter)
        _presenter = presenter
    End Sub

    Private Async Sub frmGenerateCutoff_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await _presenter.LoadAsync()
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Async Sub OnInputsChanged(sender As Object, e As EventArgs) _
        Handles cboCycleType.EditValueChanged, numYear.EditValueChanged

        If _suppressRefresh OrElse _presenter Is Nothing Then Return
        Await _presenter.RefreshPreviewAsync()
    End Sub

    Private Async Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Await _presenter.GenerateAsync()
    End Sub

#Region "IGenerateCutoffView"

    Public Property CycleType As String Implements IGenerateCutoffView.CycleType
        Get
            Return CStr(If(cboCycleType.EditValue, ""))
        End Get
        Set(value As String)
            _suppressRefresh = True
            cboCycleType.EditValue = value
            _suppressRefresh = False
        End Set
    End Property

    Public Property CutoffYear As Integer Implements IGenerateCutoffView.CutoffYear
        Get
            Return CInt(numYear.Value)
        End Get
        Set(value As Integer)
            _suppressRefresh = True
            numYear.Value = value
            _suppressRefresh = False
        End Set
    End Property

    Public Sub BindCycles(cycleTypes As List(Of String)) Implements IGenerateCutoffView.BindCycles
        _suppressRefresh = True
        cboCycleType.Properties.Items.Clear()
        For Each cycleType As String In cycleTypes
            cboCycleType.Properties.Items.Add(cycleType)
        Next
        _suppressRefresh = False
    End Sub

    Public Sub BindPreview(rows As List(Of CutoffPreviewRow)) Implements IGenerateCutoffView.BindPreview
        gridControlPreview.DataSource = rows
    End Sub

    Public Sub ShowPatternText(text As String) Implements IGenerateCutoffView.ShowPatternText
        lblPattern.Text = text
    End Sub

    Public Sub ShowSummary(text As String) Implements IGenerateCutoffView.ShowSummary
        lblSummary.Text = text
    End Sub

    Public Sub SetGenerateEnabled(enabled As Boolean) Implements IGenerateCutoffView.SetGenerateEnabled
        btnGenerate.Enabled = enabled
    End Sub

    Public Sub ShowMessage(message As String) Implements IGenerateCutoffView.ShowMessage
        XtraMessageBox.Show(Me, message, "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Sub ShowError(message As String) Implements IGenerateCutoffView.ShowError
        XtraMessageBox.Show(Me, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub CloseWithSuccess() Implements IGenerateCutoffView.CloseWithSuccess
        DialogResult = DialogResult.OK
        Close()
    End Sub

#End Region

End Class
