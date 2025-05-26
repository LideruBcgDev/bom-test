Imports System.Net.Http
Imports System.Net.Http.Json
Imports System.Text
Imports Newtonsoft.Json

'PostAsJsonAsyncはSystem.Text.Jsonのため使用禁止
'Newtonsoft.Jsonを使用（NuGetパッケージが必要）

Public Class HttpClientWrapper
    Private Shared _client As New HttpClient()
    Private Shared _baseUrl As String
    Private Shared _token As String

    Public Shared Sub SetBaseUrl(baseUrl As String)
        _baseUrl = baseUrl
        _client.BaseAddress = New Uri(baseUrl)
    End Sub

    Public Shared Sub SetAuthToken(token As String)
        _token = token
        If String.IsNullOrEmpty(token) Then
            _client.DefaultRequestHeaders.Remove("Authorization")
        Else
            _client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", token)
        End If
    End Sub

    Public Shared Async Function AuthAsync(Of TResult)(path As String, param As Object) As Task(Of TResult)
        Try
            Dim json = JsonConvert.SerializeObject(param)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")

            Dim response = Await _client.PostAsync(_baseUrl & path, content)

            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of TResult)(responseContent)
            Else
                Throw New HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}")
            End If

            '' デバッグ用のログ出力
            'Console.WriteLine($"AuthAsync called with path: {path}")
            'Console.WriteLine($"Base URL: {_baseUrl}")

            ''Dim response = Await _client.PostAsJsonAsync(path, param)

            '' レスポンスの詳細をログ出力
            'Console.WriteLine($"Response Status: {response.StatusCode}")
            'Dim content = Await response.Content.ReadAsStringAsync()
            'Console.WriteLine($"Response Content: {content}")

            'response.EnsureSuccessStatusCode()
            'Return Await response.Content.ReadFromJsonAsync(Of TResult)()
        Catch ex As Exception
            Console.WriteLine($"AuthAsync error: {ex.Message}")
            Console.WriteLine($"Stack trace: {ex.StackTrace}")
            Throw New Exception($"認証に失敗しました: {ex.Message}")
        End Try
    End Function

    Public Shared Async Function PostAsync(Of TResult)(path As String, param As Object) As Task(Of TResult)
        Try
            Dim json = JsonConvert.SerializeObject(param)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")

            Dim response = Await _client.PostAsync(_baseUrl & "client/" & path, content)

            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of TResult)(responseContent)
            Else
                Throw New HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}")
            End If

            'Dim fullPath = "client/" & path
            'Dim response = Await _client.PostAsJsonAsync(fullPath, param)
            'response.EnsureSuccessStatusCode()
            'Return Await response.Content.ReadFromJsonAsync(Of TResult)()
        Catch ex As Exception
            Throw New Exception($"リクエストに失敗しました: {ex.Message}")
        End Try
    End Function

    'Public Shared Async Function GetAsync(Of TResult)(path As String) As Task(Of TResult)
    '    Try
    '        Dim fullPath = "client/" & path
    '        Dim response = Await _client.GetAsync(fullPath)
    '        response.EnsureSuccessStatusCode()
    '        Return Await response.Content.ReadFromJsonAsync(Of TResult)()
    '    Catch ex As Exception
    '        Throw New Exception($"リクエストに失敗しました: {ex.Message}")
    '    End Try
    'End Function

    'Public Shared Async Function DeleteAsync(Of TResult)(path As String) As Task(Of TResult)
    '    Try
    '        Dim fullPath = "client/" & path
    '        Dim response = Await _client.DeleteAsync(fullPath)
    '        response.EnsureSuccessStatusCode()
    '        Return Await response.Content.ReadFromJsonAsync(Of TResult)()
    '    Catch ex As Exception
    '        Throw New Exception($"リクエストに失敗しました: {ex.Message}")
    '    End Try
    'End Function
End Class
