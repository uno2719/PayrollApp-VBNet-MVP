' My Project/Application.vb
Imports Microsoft.VisualBasic.ApplicationServices
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports Payroll.GlobalShared.Contracts
Imports Payroll.GlobalShared.Models
Imports Payroll.GlobalShared.Services

Namespace My
    Partial Friend Class MyApplication

        Private ReadOnly _themeSettingsService As IThemeSettingsService = New JsonThemeSettingsService()

        Protected Overrides Function OnInitialize(
            commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)
        ) As Boolean

            ' 🔥 GLOBAL DEFAULT FONT - dapat ito ang UNANG mangyari, bago pa
            ' ang skin registration at bago pa ang unang form. Isang lugar
            ' lang ito - awtomatikong susunod ang LAHAT ng DevExpress editors,
            ' grids, at labels sa buong app.
            WindowsFormsSettings.DefaultFont = New Font("Segoe UI", 9.0F)
            WindowsFormsSettings.DefaultMenuFont = New Font("Segoe UI", 9.0F)

            ' 🔥 REGISTER SKINS HERE
            DevExpress.UserSkins.BonusSkins.Register()
            DevExpress.Skins.SkinManager.EnableFormSkins()

            AppConfiguration.Initialize()

            ' 🔥 I-LOAD ang huling na-save na skin/palette, BAGO pa man
            ' lumabas ang frmLogin - dito pa lang, tama na agad ang itsura.
            Dim savedTheme = _themeSettingsService.Load()
            If String.IsNullOrEmpty(savedTheme.PaletteName) Then
                UserLookAndFeel.Default.SetSkinStyle(savedTheme.SkinName)
            Else
                UserLookAndFeel.Default.SetSkinStyle(savedTheme.SkinName, savedTheme.PaletteName)
            End If

            ' 🔥 I-LISTEN kada may magbago sa skin/palette (kahit galing sa
            ' built-in switcher mo sa MainForm) - awtomatikong ma-save
            AddHandler UserLookAndFeel.Default.StyleChanged, AddressOf OnThemeChanged

            ' 🔥 ONE-TIME SEED TOOL
            ' Para patakbuhin: Debug launch profile > Command line arguments,
            ' ilagay ang "--seed-admin", tapos i-run (F5). Pagkatapos, TANGGALIN
            ' ang argument na ito para hindi na tumakbo ulit sa susunod na F5.
            If commandLineArgs.Contains("--seed-admin") Then
                ' 👇 Palitan ng gusto mong username/password/EmployeeNo bago i-run
                Dim resultMessage = Login.Services.SeedTool.SeedAdminUserAsync(
                    "Uno", "pacsports", "620054").GetAwaiter().GetResult()

                MessageBox.Show(resultMessage, "Seed Admin User")
                Environment.Exit(0)
            End If

            ' 🔥 SET/RESET SETTINGS PIN
            ' Gamitin ito para i-set (o i-reset) ang PIN na kailangan para
            ' ma-access ang Application Settings mula sa Login screen.
            ' Walang DB dependency ito - gagana kahit sirang-sira ang
            ' connection settings mo.
            '
            '     Payroll.exe --set-settings-pin 2468
            If commandLineArgs.Contains("--set-settings-pin") Then
                Dim argIndex = commandLineArgs.IndexOf("--set-settings-pin")

                If argIndex >= 0 AndAlso argIndex + 1 < commandLineArgs.Count Then
                    Dim newPin = commandLineArgs(argIndex + 1)
                    Dim resultMessage = DBConnection.Services.SettingsPinTool.SetSettingsPin(newPin)
                    MessageBox.Show(resultMessage, "Set Settings PIN")
                Else
                    MessageBox.Show(
                        "Invalid format. Usage: --set-settings-pin <PIN>",
                        "Set Settings PIN - Error")
                End If

                Environment.Exit(0)
            End If

            ' 🔥 EMERGENCY RECOVERY TOOL - Reset Password
            ' Gamitin ito kung nawalan ng access ang isang account (nakalimutan
            ' ang password, o naka-lock) at walang ibang Admin na pwedeng
            ' mag-reset mula sa loob ng app mismo.
            '
            ' Paano patakbuhin (via actual command line, HINDI kailangang
            ' i-edit ang code - i-type lang ang tunay na username/password):
            '     Payroll.exe --reset-password admin BagongPassword123!
            '
            ' O kaya sa Debug launch profile > Command line arguments:
            '     --reset-password admin BagongPassword123!
            If commandLineArgs.Contains("--reset-password") Then
                Dim argIndex = commandLineArgs.IndexOf("--reset-password")

                If argIndex >= 0 AndAlso argIndex + 2 < commandLineArgs.Count Then
                    Dim targetUsername = commandLineArgs(argIndex + 1)
                    Dim newPassword = commandLineArgs(argIndex + 2)

                    Dim resultMessage = Login.Services.SeedTool.ResetAdminPasswordAsync(
                        targetUsername, newPassword).GetAwaiter().GetResult()

                    MessageBox.Show(resultMessage, "Reset Password")
                Else
                    MessageBox.Show(
                        "Invalid format. Usage: --reset-password <username> <new password>",
                        "Reset Password - Error")
                End If

                Environment.Exit(0)
            End If

            Return MyBase.OnInitialize(commandLineArgs)
        End Function

        Private Sub OnThemeChanged(sender As Object, e As EventArgs)
            Dim currentSettings As New ThemeSettings With {
                .SkinName = UserLookAndFeel.Default.SkinName,
                .PaletteName = UserLookAndFeel.Default.ActiveSvgPaletteName
            }
            _themeSettingsService.Save(currentSettings)
        End Sub

        ' 🔥 LOGIN GATE - dito na natin gagamitin ang AppComposition
        ' para maayos na naka-wire ang Repository -> Service -> Presenter -> View
        Protected Overrides Sub OnCreateMainForm()
            Dim loggedIn As Boolean = False

            Do
                Using loginForm As frmLogin = AppComposition.BuildLoginForm()
                    If loginForm.ShowDialog() = DialogResult.OK Then
                        ' ✅ Successful login
                        Dim main As New frmMain()
                        Me.MainForm = main
                        main._logout = False
                        main.ShowDialog()  ' ← i-show ang frmMain, hintayin mag-close

                        ' Pag nag-close ang frmMain (logout) — loop ulit para mag-login
                        loggedIn = Not main._logout 'False
                    Else
                        ' ✅ User nag-cancel ng login — exit na
                        Environment.Exit(0)
                    End If
                End Using
            Loop While Not loggedIn
            Environment.Exit(0)
        End Sub

    End Class
End Namespace