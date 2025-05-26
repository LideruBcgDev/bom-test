Imports System.ComponentModel.DataAnnotations

''' <summary>
''' ユーザー情報
''' </summary>
Public Class UserInfo
    <Required>
    <StringLength(20)>
    Public Property UserId As String
    
    <Required>
    <StringLength(50)>
    Public Property UserName As String
    
    <Required>
    <EmailAddress>
    <StringLength(100)>
    Public Property Email As String
    
    <StringLength(50)>
    Public Property Department As String
    
    Public Property IsActive As Boolean
    
    Public Property CreatedDate As DateTime
    
    Public Property UpdatedDate As DateTime?
End Class

''' <summary>
''' ユーザー検索パラメータ
''' </summary>
Public Class UserSearchParam
    Public Property UserId As String
    Public Property UserName As String
    Public Property Department As String
End Class

''' <summary>
''' ログインパラメータ
''' </summary>
Public Class LoginParam
    <Required>
    Public Property UserId As String
    
    <Required>
    Public Property Password As String
End Class

''' <summary>
''' ログイン結果
''' </summary>
Public Class LoginResult
    Public Property Success As Boolean
    Public Property User As UserInfo
    Public Property Token As String
    Public Property Message As String
End Class
