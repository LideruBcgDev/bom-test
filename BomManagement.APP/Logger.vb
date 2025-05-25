Imports System.IO

Public Class Logger
    Private Shared ReadOnly _lockObject As New Object()
    Private Shared _logPath As String

    Shared Sub New()
        Dim logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs")
        If Not Directory.Exists(logDir) Then
            Directory.CreateDirectory(logDir)
        End If
        
        _logPath = Path.Combine(logDir, $"{DateTime.Now:yyyyMMdd}.log")
    End Sub

    ''' <summary>
    ''' 情報ログを出力する
    ''' </summary>
    Public Shared Sub Info(message As String)
        WriteLog("INFO", message)
    End Sub

    ''' <summary>
    ''' 警告ログを出力する
    ''' </summary>
    Public Shared Sub Warning(message As String)
        WriteLog("WARN", message)
    End Sub

    ''' <summary>
    ''' エラーログを出力する
    ''' </summary>
    Public Shared Sub [Error](message As String)
        WriteLog("ERROR", message)
    End Sub

    ''' <summary>
    ''' エラーログを出力する（例外付き）
    ''' </summary>
    Public Shared Sub [Error](message As String, ex As Exception)
        WriteLog("ERROR", $"{message}{Environment.NewLine}{ex.ToString()}")
    End Sub

    ''' <summary>
    ''' デバッグログを出力する
    ''' </summary>
    Public Shared Sub Debug(message As String)
        If ConfigManager.LogLevel = "Debug" Then
            WriteLog("DEBUG", message)
        End If
    End Sub

    ''' <summary>
    ''' ログを書き込む
    ''' </summary>
    Private Shared Sub WriteLog(level As String, message As String)
        Try
            SyncLock _lockObject
                Dim logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}"
                File.AppendAllText(_logPath, logEntry & Environment.NewLine)
            End SyncLock
        Catch ex As Exception
            ' ログ出力エラーは無視
        End Try
    End Sub
End Class
