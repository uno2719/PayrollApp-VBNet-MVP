Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Constants
Imports Payroll.Login.Data
Imports Payroll.Login.Services

Namespace GlobalShared.Helpers

    ''' <summary>
    ''' Iisang lugar para sa Deactivate/Reactivate + Delete na mga button ng
    ''' lahat ng Settings maintenance views (New | Edit | Deactivate | Delete | Refresh),
    ''' para hindi na ulit-ulitin ang parehong code sa 10 views.
    '''
    ''' Ang Deactivate ay tinutukoy ng Tag ("Deactivate") - hindi ng caption,
    ''' kaya ligtas na magpalit ang caption ng Deactivate/Reactivate.
    ''' </summary>
    Public Module CommandBarHelper

        Private Const DeactivateCaption As String = " Deactivate"
        Private Const ReactivateCaption As String = " Reactivate"
        Private Const ViewOnlyTip As String = "You have View Only access to this module."

        ''' <summary>Itakda ang unang caption/icon/tooltip. Tawagin sa SetupCommandImages.</summary>
        Public Sub InitDeactivateDeleteButtons(panel As WindowsUIButtonPanel, deactivateIndex As Integer, deleteIndex As Integer)
            RefreshToggleCaption(panel, deactivateIndex, True)

            With panel.Buttons.Item(deleteIndex).Properties
                .Caption = " Delete"
                .ImageOptions.Image = My.Resources.icon_delete_24
                .ToolTip = DeleteTooltip()
            End With
        End Sub

        ''' <summary>
        ''' Palitan ang caption/icon ng toggle button depende sa Active state ng napiling row.
        ''' Tinatawag mula sa chkActive.CheckedChanged ng view.
        ''' </summary>
        Public Sub RefreshToggleCaption(panel As WindowsUIButtonPanel, deactivateIndex As Integer,
                                        isActive As Boolean, Optional hasEditAccess As Boolean = True)

            With panel.Buttons.Item(deactivateIndex).Properties
                If isActive Then
                    .Caption = DeactivateCaption
                    .ImageOptions.Image = My.Resources.icon_deactivate_24
                    .ToolTip = If(hasEditAccess, "Deactivate Selected", ViewOnlyTip)
                Else
                    .Caption = ReactivateCaption
                    .ImageOptions.Image = My.Resources.icon_reactivate_24
                    .ToolTip = If(hasEditAccess, "Reactivate Selected", ViewOnlyTip)
                End If
            End With
        End Sub

        ''' <summary>
        ''' Enable/disable ng Deactivate at Delete tuwing nagpapalit ng mode (SetFormMode).
        ''' PAALALA: dito mismo ini-enforce ang View Only, kasi ang ApplyReadOnlyMode
        ''' ay isang beses lang tinatawag at nao-overwrite ng SetFormMode pagkatapos.
        ''' </summary>
        Public Sub ApplyModeState(panel As WindowsUIButtonPanel, deactivateIndex As Integer, deleteIndex As Integer,
                                  isEditing As Boolean, isActive As Boolean, hasEditAccess As Boolean)

            panel.Buttons.Item(deactivateIndex).Properties.Enabled = (Not isEditing) AndAlso hasEditAccess
            RefreshToggleCaption(panel, deactivateIndex, isActive, hasEditAccess)

            panel.Buttons.Item(deleteIndex).Properties.Enabled = (Not isEditing) AndAlso AppSession.IsAdmin
            panel.Buttons.Item(deleteIndex).Properties.ToolTip = DeleteTooltip()
        End Sub

        ' Ipakita ang Status (Active/Inactive) bilang may kulay na label - hindi na checkbox.
        Public Sub ShowStatus(lbl As LabelControl, isActive As Boolean)
            lbl.Text = If(isActive, "Status: Active", "Status: Inactive")
            lbl.Appearance.ForeColor = If(isActive, Color.SeaGreen, Color.Firebrick)
            lbl.Appearance.Options.UseForeColor = True
        End Sub

        ' Lalabas kapag sine-save ang pag-edit ng INACTIVE na record.
        ' Yes = i-save at gawing Active, No = i-save pero manatiling Inactive,
        ' Cancel = bumalik sa pag-edit. Default button: No (walang magbabago sa status).
        Public Function AskInactiveSaveChoice(owner As IWin32Window) As DialogResult
            Return XtraMessageBox.Show(owner,
                "This entry is currently Inactive." & vbCrLf & vbCrLf &
                "Do you want to set it back to Active when you save?" & vbCrLf & vbCrLf &
                "   Yes  -  Save and set to Active" & vbCrLf &
                "   No   -  Save and keep it Inactive" & vbCrLf &
                "   Cancel  -  Go back to editing",
                "Save Inactive Entry",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        End Function

        Private Function DeleteTooltip() As String
            Return If(AppSession.IsAdmin,
                      "Delete Selected (permanent)",
                      "Only administrators can delete records.")
        End Function

        ''' <summary>
        ''' Tatlong hakbang bago ang totoong Delete:
        '''   1. Yes/No (default: No)
        '''   2. Final warning Yes/No (default: No)
        '''   3. Password ng naka-login na user
        ''' </summary>
        ''' <param name="itemDescription">Hal. "'BDO - BDO Unibank'" o "this bracket".</param>
        Public Function ConfirmDelete(owner As IWin32Window, itemDescription As String) As Boolean

            If Not AppSession.IsAdmin Then
                XtraMessageBox.Show(owner, "Only administrators can delete records.", "Delete",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            Dim first = XtraMessageBox.Show(owner,
                $"Delete {itemDescription}?" & vbCrLf & vbCrLf &
                "This removes the record permanently." & vbCrLf &
                "If you only want to stop using it, use Deactivate instead.",
                "Confirm Delete (1 of 2)",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If first <> DialogResult.Yes Then Return False

            Dim second = XtraMessageBox.Show(owner,
                "FINAL WARNING" & vbCrLf & vbCrLf &
                $"{itemDescription} will be permanently deleted and cannot be recovered." & vbCrLf & vbCrLf &
                "Are you absolutely sure?",
                "Confirm Delete (2 of 2)",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)

            If second <> DialogResult.Yes Then Return False

            ' Kapareho ng ginagawa ng frmLogin: gawa-gawa ng service sa mismong lugar.
            Dim auth As IAuthenticationService = New AuthenticationService(New UserRepository())
            Dim username = AppSession.CurrentUser

            Using dlg As New frmConfirmPassword(itemDescription)
                dlg.Verifier = Function(password As String) auth.VerifyCurrentUserPasswordAsync(username, password)
                Return dlg.ShowDialog(owner) = DialogResult.OK
            End Using
        End Function

    End Module

End Namespace
