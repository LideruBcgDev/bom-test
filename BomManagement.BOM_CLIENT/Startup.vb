Imports System.IO
Imports System.Text.Json
Imports BomManagement.FW_APP

Public Class Startup
    Public Shared Sub Main()
        Try
            ' 設定ファイルの読み込み
            LoadAppSettings()

            ' ログイン画面の表示
            Dim loginForm = New FormLogin()
            If loginForm.ShowDialog() = DialogResult.OK Then
                ' ログイン成功時、メインメニューを表示
                Application.Run(New FormMainMenu())
            End If
        Catch ex As Exception
            MessageBox.Show($"アプリケーションの起動に失敗しました。{vbCrLf}{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Shared Sub LoadAppSettings()
        Try
            ' 設定ファイルのパス
            Dim configPath = Path.Combine(Application.StartupPath, "config", "appsettings.json")

            ' 設定ファイルの読み込み
            Dim jsonString = File.ReadAllText(configPath)
            Dim settings = JsonSerializer.Deserialize(Of AppSettings)(jsonString)

            ' ベースURLの設定
            If settings?.ApiSettings?.BaseUrl IsNot Nothing Then
                HttpClientWrapper.SetBaseUrl(settings.ApiSettings.BaseUrl)
            End If
        Catch ex As Exception
            MessageBox.Show($"設定ファイルの読み込みに失敗しました。{vbCrLf}{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Throw
        End Try
    End Sub
End Class 