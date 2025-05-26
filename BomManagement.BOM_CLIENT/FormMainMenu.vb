Imports System.Windows.Forms
Imports BomManagement.FW_APP
Imports BomManagement.FW_WIN

Public Class FormMainMenu
    Inherits Form

    Private menuPanel As New Panel() With {.Dock = DockStyle.Left, .Width = 180, .BackColor = Drawing.Color.Gainsboro}
    Private mainPanel As New Panel() With {.Dock = DockStyle.Fill}
    Private btnHinmoku As New Button() With {.Text = "品目", .Top = 30, .Left = 20, .Width = 140}
    Private btnSekkei As New Button() With {.Text = "設計部品表", .Top = 80, .Left = 20, .Width = 140}
    Private btnMitsumori As New Button() With {.Text = "見積部品表", .Top = 130, .Left = 20, .Width = 140}
    Private btnJuchu As New Button() With {.Text = "受注部品表", .Top = 180, .Left = 20, .Width = 140}
    Private lblUserInfo As New Label() With {.Text = "", .Top = 230, .Left = 20, .Width = 140, .AutoSize = True}
    Private btnLogout As New Button() With {.Text = "ログアウト", .Top = 280, .Left = 20, .Width = 140}

    Public Sub New()
        Me.Text = "メインメニュー"
        Me.IsMdiContainer = True
        Me.WindowState = FormWindowState.Maximized
        menuPanel.Controls.AddRange({btnHinmoku, btnSekkei, btnMitsumori, btnJuchu, lblUserInfo, btnLogout})
        Me.Controls.Add(menuPanel)
        Me.Controls.Add(mainPanel)
        AddHandler btnHinmoku.Click, Sub() OpenChildWindow(New FormHinmokuSearch())
        AddHandler btnSekkei.Click, Sub() OpenChildWindow(New FormSekkeiBuhinSearch())
        AddHandler btnMitsumori.Click, Sub() OpenChildWindow(New FormMitsumoriBuhinSearch())
        AddHandler btnJuchu.Click, Sub() OpenChildWindow(New FormJuchuBuhinSearch())
        AddHandler btnLogout.Click, AddressOf btnLogout_Click

        ' ログインユーザー情報の表示
        UpdateUserInfo()
    End Sub

    Private Sub UpdateUserInfo()
        If UserContext.IsAuthenticated() Then
            lblUserInfo.Text = $"ログインユーザー: {UserContext.GetUserName()}"
        End If
    End Sub

    Private Sub OpenChildWindow(child As Form)
        child.Show()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs)
        Try
            ' トークンをクリア
            HttpClientWrapper.SetAuthToken("")

            ' ログインフォームを表示
            Dim loginForm As New FormLogin()
            loginForm.Show()

            ' メインメニューを閉じる
            Me.Close()
        Catch ex As Exception
            MessageBox.Show($"ログアウト処理でエラーが発生しました: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub InitializeComponent()
        ' メニューの初期化処理
    End Sub
End Class