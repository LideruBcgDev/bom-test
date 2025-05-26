Imports System.Windows.Forms
Imports System.Security.Claims
Imports System.Threading.Tasks
Imports BomManagement.FW_APP
Imports BomManagement.BOM_PRM

Public Class FormLogin
    Inherits Form

    Private txtUsername As New TextBox() With {.Location = New Drawing.Point(150, 30), .Width = 200, .Text = "admin"}
    Private txtPassword As New TextBox() With {.Location = New Drawing.Point(150, 70), .Width = 200, .PasswordChar = "*"c, .Text = "password"}
    Private btnLogin As New Button() With {.Text = "ログイン", .Location = New Drawing.Point(120, 110), .Width = 100}
    Private btnSamlLogin As New Button() With {.Text = "SAMLログイン", .Location = New Drawing.Point(230, 110), .Width = 100}
    Private lblUsername As New Label() With {.Text = "ユーザー名:", .Location = New Drawing.Point(50, 33)}
    Private lblPassword As New Label() With {.Text = "パスワード:", .Location = New Drawing.Point(50, 73)}

    Public Sub New()
        Me.Text = "ログイン"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Size = New Drawing.Size(400, 200)

        Me.Controls.AddRange({lblUsername, lblPassword, txtUsername, txtPassword, btnLogin, btnSamlLogin})

        AddHandler btnLogin.Click, AddressOf BtnLogin_Click
        AddHandler btnSamlLogin.Click, AddressOf BtnSamlLogin_Click
    End Sub

    Private Async Sub BtnLogin_Click(sender As Object, e As EventArgs)
        If String.IsNullOrEmpty(txtUsername.Text) OrElse String.IsNullOrEmpty(txtPassword.Text) Then
            MessageBox.Show("ユーザー名とパスワードを入力してください。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' サーバーサイドの認証処理を呼び出す
        If Await AuthenticateUser(txtUsername.Text, txtPassword.Text) Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub BtnSamlLogin_Click(sender As Object, e As EventArgs)
        ' SAMLログイン処理
        Dim samlLogin = New FormSamlLogin()
        If samlLogin.ShowDialog() = DialogResult.OK Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Async Function AuthenticateUser(username As String, password As String) As Task(Of Boolean)
        Try
            ' ログインパラメータの作成
            Dim loginParam = New LoginParam With {
                .UserId = username,
                .Password = password
            }

            ' サーバーサイドの認証APIを呼び出す
            Dim result = Await HttpClientWrapper.AuthAsync(Of LoginResult)("auth/login", loginParam)

            If result.Success Then
                ' 認証成功時、ユーザー情報をClaimsPrincipalとして設定
                Dim claims = New List(Of Claim) From {
                    New Claim(ClaimTypes.NameIdentifier, result.User.UserId),
                    New Claim(ClaimTypes.Name, result.User.UserName),
                    New Claim(ClaimTypes.Email, result.User.Email)
                }
                Dim identity = New ClaimsIdentity(claims, "Custom")
                Dim principal = New ClaimsPrincipal(identity)
                UserContext.SetCurrentUser(principal)
                HttpClientWrapper.SetAuthToken(result.Token)
                Return True
            Else
                MessageBox.Show(result.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show($"認証に失敗しました。{vbCrLf}{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
End Class 