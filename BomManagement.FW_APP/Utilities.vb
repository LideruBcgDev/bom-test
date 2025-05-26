Imports System.Security.Cryptography
Imports System.Text

Public Class Utilities
    ''' <summary>
    ''' パスワードをハッシュ化する
    ''' </summary>
    Public Shared Function HashPassword(password As String) As String
        Using _sha256 = SHA256.Create()
            Dim bytes = _sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
            Return Convert.ToBase64String(bytes)
        End Using
    End Function

    ''' <summary>
    ''' 日付を文字列に変換する
    ''' </summary>
    Public Shared Function FormatDate(dateValue As DateTime?) As String
        If dateValue.HasValue Then
            Return dateValue.Value.ToString("yyyy/MM/dd")
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' 日時を文字列に変換する
    ''' </summary>
    Public Shared Function FormatDateTime(dateValue As DateTime?) As String
        If dateValue.HasValue Then
            Return dateValue.Value.ToString("yyyy/MM/dd HH:mm:ss")
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' 数値を文字列に変換する（カンマ区切り）
    ''' </summary>
    Public Shared Function FormatNumber(value As Decimal) As String
        Return value.ToString("#,##0.###")
    End Function

    ''' <summary>
    ''' 文字列をトリムする（Null対応）
    ''' </summary>
    Public Shared Function TrimString(value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return ""
        Else
            Return value.Trim()
        End If
    End Function

    ''' <summary>
    ''' オブジェクトをディープコピーする
    ''' </summary>
    Public Shared Function DeepCopy(Of T)(obj As T) As T
        Dim json = Newtonsoft.Json.JsonConvert.SerializeObject(obj)
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(json)
    End Function
End Class
