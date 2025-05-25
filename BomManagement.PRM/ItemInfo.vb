Imports System.ComponentModel.DataAnnotations

''' <summary>
''' 品目情報
''' </summary>
Public Class ItemInfo
    <Required>
    <StringLength(30)>
    Public Property ItemCode As String
    
    <Required>
    <StringLength(100)>
    Public Property ItemName As String
    
    <Required>
    <StringLength(20)>
    Public Property ItemType As String
    
    <Required>
    <StringLength(10)>
    Public Property Unit As String
    
    <Range(0, Double.MaxValue)>
    Public Property StandardCost As Decimal
    
    <StringLength(500)>
    Public Property Description As String
    
    Public Property IsActive As Boolean
    
    Public Property CreatedDate As DateTime
    
    Public Property UpdatedDate As DateTime?
End Class

''' <summary>
''' 品目検索パラメータ
''' </summary>
Public Class ItemSearchParam
    Public Property ItemCode As String
    Public Property ItemName As String
    Public Property ItemType As String
    Public Property OnlyActive As Boolean = True
End Class

''' <summary>
''' 品目タイプ
''' </summary>
Public Class ItemType
    Public Const Product As String = "製品"
    Public Const Material As String = "原材料"
    Public Const Part As String = "部品"
    Public Const SemiFinished As String = "半製品"
End Class
