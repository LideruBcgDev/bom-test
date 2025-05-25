Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class HttpClientWrapper
    Private Shared ReadOnly _httpClient As HttpClient = New HttpClient()
    Private Shared _baseUrl As String = "https://localhost:5001/api/"
    Private Shared _token As String = ""

    Shared Sub New()
        _httpClient.DefaultRequestHeaders.Accept.Clear()
        _httpClient.DefaultRequestHeaders.Accept.Add(New Headers.MediaTypeWithQualityHeaderValue("application/json"))
    End Sub

    ''' <summary>
    ''' ベースURLを設定する
    ''' </summary>
    Public Shared Sub SetBaseUrl(baseUrl As String)
        _baseUrl = baseUrl
    End Sub

    ''' <summary>
    ''' 認証トークンを設定する
    ''' </summary>
    Public Shared Sub SetToken(token As String)
        _token = token
        If Not String.IsNullOrEmpty(token) Then
            _httpClient.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", token)
        End If
    End Sub

    ''' <summary>
    ''' GETリクエストを送信する
    ''' </summary>
    Public Shared Async Function GetAsync(Of T)(endpoint As String) As Task(Of T)
        Try
            Dim response = Await _httpClient.GetAsync(_baseUrl & endpoint)
            
            If response.IsSuccessStatusCode Then
                Dim content = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of T)(content)
            Else
                Throw New HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}")
            End If
        Catch ex As Exception
            Throw New Exception($"GET request failed: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' POSTリクエストを送信する
    ''' </summary>
    Public Shared Async Function PostAsync(Of T)(endpoint As String, data As Object) As Task(Of T)
        Try
            Dim json = JsonConvert.SerializeObject(data)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            
            Dim response = Await _httpClient.PostAsync(_baseUrl & endpoint, content)
            
            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of T)(responseContent)
            Else
                Throw New HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}")
            End If
        Catch ex As Exception
            Throw New Exception($"POST request failed: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' PUTリクエストを送信する
    ''' </summary>
    Public Shared Async Function PutAsync(Of T)(endpoint As String, data As Object) As Task(Of T)
        Try
            Dim json = JsonConvert.SerializeObject(data)
            Dim content = New StringContent(json, Encoding.UTF8, "application/json")
            
            Dim response = Await _httpClient.PutAsync(_baseUrl & endpoint, content)
            
            If response.IsSuccessStatusCode Then
                Dim responseContent = Await response.Content.ReadAsStringAsync()
                Return JsonConvert.DeserializeObject(Of T)(responseContent)
            Else
                Throw New HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}")
            End If
        Catch ex As Exception
            Throw New Exception($"PUT request failed: {ex.Message}", ex)
        End Try
    End Function

    ''' <summary>
    ''' DELETEリクエストを送信する
    ''' </summary>
    Public Shared Async Function DeleteAsync(endpoint As String) As Task(Of Boolean)
        Try
            Dim response = Await _httpClient.DeleteAsync(_baseUrl & endpoint)
            Return response.IsSuccessStatusCode
        Catch ex As Exception
            Throw New Exception($"DELETE request failed: {ex.Message}", ex)
        End Try
    End Function
End Class
