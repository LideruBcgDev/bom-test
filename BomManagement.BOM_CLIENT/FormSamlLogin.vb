Imports System.Windows.Forms
Imports System.Security.Claims
Imports System.Diagnostics
Imports System.Threading.Tasks
Imports BomManagement.FW_APP
Imports BomManagement.BOM_PRM

Public Class FormSamlLogin
    Inherits Form

    Private btnSamlLogin As New Button() With {.Text = "SAMLでログイン", .Location = New Drawing.Point(120, 30), .Width = 200}
    Private lblStatus As New Label() With {.Text = "SAML認証を開始します...", .Location = New Drawing.Point(120, 70), .Width = 200, .AutoSize = True}

    Public Sub New()
        Me.Text = "SAMLログイン"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Size = New Drawing.Size(400, 150)

        Me.Controls.AddRange({btnSamlLogin, lblStatus})
        AddHandler btnSamlLogin.Click, AddressOf BtnSamlLogin_Click
    End Sub

    Private Async Sub BtnSamlLogin_Click(sender As Object, e As EventArgs)
        Try
            ' SAML認証パラメータの作成
            Dim samlParam = New SamlLoginParam With {
                .Provider = "default", ' 使用するSAMLプロバイダー
                .ReturnUrl = "http://localhost:5001/client/auth/saml/callback" ' コールバックURL
            }

            ' サーバーサイドのSAML認証APIを呼び出す
            Dim result = Await HttpClientWrapper.PostAsync(Of SamlLoginResult)("auth/saml/login", samlParam)

            If result.Success Then
                ' 認証成功時、ユーザー情報をClaimsPrincipalとして設定
                Dim claims = New List(Of Claim) From {
                    New Claim(ClaimTypes.NameIdentifier, result.UserId),
                    New Claim(ClaimTypes.Name, result.UserName),
                    New Claim(ClaimTypes.Email, result.Email)
                }
                Dim identity = New ClaimsIdentity(claims, "SAML")
                Dim principal = New ClaimsPrincipal(identity)
                UserContext.SetCurrentUser(principal)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            ElseIf Not String.IsNullOrEmpty(result.RedirectUrl) Then
                ' SAMLプロバイダーへのリダイレクトが必要な場合
                lblStatus.Text = "SAMLプロバイダーにリダイレクトしています..."
                Process.Start(New ProcessStartInfo(result.RedirectUrl) With {.UseShellExecute = True})
            Else
                MessageBox.Show(result.ErrorMessage, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show($"SAML認証に失敗しました。{vbCrLf}{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class 