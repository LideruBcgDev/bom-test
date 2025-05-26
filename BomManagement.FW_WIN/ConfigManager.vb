Imports System.Configuration
Imports System.IO
Imports Newtonsoft.Json

Public Class ConfigManager
    Private Shared _config As AppConfig

    Shared Sub New()
        LoadConfig()
    End Sub

    ''' <summary>
    ''' 設定を読み込む
    ''' </summary>
    Private Shared Sub LoadConfig()
        Try
            Dim configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")
            
            If File.Exists(configPath) Then
                Dim json = File.ReadAllText(configPath)
                _config = JsonConvert.DeserializeObject(Of AppConfig)(json)
            Else
                ' デフォルト設定
                _config = New AppConfig() With {
                    .ApiBaseUrl = "https://localhost:5001/api/",
                    .ConnectionString = "Data Source=localhost;Initial Catalog=BomManagementDB;Integrated Security=True;TrustServerCertificate=True",
                    .EnableSSO = True,
                    .LogLevel = "Info"
                }
                
                ' 設定ファイルを作成
                SaveConfig()
            End If
        Catch ex As Exception
            _config = New AppConfig()
        End Try
    End Sub

    ''' <summary>
    ''' 設定を保存する
    ''' </summary>
    Private Shared Sub SaveConfig()
        Try
            Dim configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")
            Dim json = JsonConvert.SerializeObject(_config, Formatting.Indented)
            File.WriteAllText(configPath, json)
        Catch ex As Exception
            ' エラーログ出力
        End Try
    End Sub

    ''' <summary>
    ''' API基底URLを取得する
    ''' </summary>
    Public Shared ReadOnly Property ApiBaseUrl As String
        Get
            Return _config.ApiBaseUrl
        End Get
    End Property

    ''' <summary>
    ''' 接続文字列を取得する
    ''' </summary>
    Public Shared ReadOnly Property ConnectionString As String
        Get
            Return _config.ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' SSO有効フラグを取得する
    ''' </summary>
    Public Shared ReadOnly Property EnableSSO As Boolean
        Get
            Return _config.EnableSSO
        End Get
    End Property

    ''' <summary>
    ''' ログレベルを取得する
    ''' </summary>
    Public Shared ReadOnly Property LogLevel As String
        Get
            Return _config.LogLevel
        End Get
    End Property
End Class

''' <summary>
''' アプリケーション設定
''' </summary>
Public Class AppConfig
    Public Property ApiBaseUrl As String
    Public Property ConnectionString As String
    Public Property EnableSSO As Boolean
    Public Property LogLevel As String
End Class
