Imports System.ComponentModel.DataAnnotations

''' <summary>
''' 部品表ヘッダー情報
''' </summary>
Public Class BomHeader
    <Required>
    <StringLength(30)>
    Public Property ParentItemCode As String
    
    Public Property ParentItemName As String
    
    <Required>
    <Range(1, Integer.MaxValue)>
    Public Property Revision As Integer
    
    <Required>
    Public Property EffectiveDate As DateTime
    
    Public Property ExpirationDate As DateTime?
    
    <Required>
    <StringLength(20)>
    Public Property Status As String
    
    <StringLength(500)>
    Public Property Description As String
    
    Public Property CreatedDate As DateTime
    
    Public Property UpdatedDate As DateTime?
    
    Public Property Details As List(Of BomDetail)
End Class

''' <summary>
''' 部品表明細情報
''' </summary>
Public Class BomDetail
    <Required>
    Public Property ParentItemCode As String
    
    <Required>
    Public Property Revision As Integer
    
    <Required>
    Public Property LineNo As Integer
    
    <Required>
    <StringLength(30)>
    Public Property ChildItemCode As String
    
    Public Property ChildItemName As String
    
    <Required>
    <Range(0.001, Double.MaxValue)>
    Public Property Quantity As Decimal
    
    <Required>
    <StringLength(10)>
    Public Property Unit As String
    
    <Range(0, Integer.MaxValue)>
    Public Property LeadTime As Integer
    
    Public Property IsActive As Boolean = True
End Class

''' <summary>
''' 部品表検索パラメータ
''' </summary>
Public Class BomSearchParam
    Public Property ParentItemCode As String
    Public Property Status As String
    Public Property EffectiveDate As DateTime?
End Class

''' <summary>
''' 部品表ステータス
''' </summary>
Public Class BomStatus
    Public Const Draft As String = "作成中"
    Public Const Active As String = "有効"
    Public Const Inactive As String = "無効"
    Public Const Obsolete As String = "廃止"
End Class

''' <summary>
''' 部品表ツリーノード
''' </summary>
Public Class BomTreeNode
    Public Property Level As Integer
    Public Property ItemCode As String
    Public Property ItemName As String
    Public Property ItemType As String
    Public Property Quantity As Decimal
    Public Property Unit As String
    Public Property Children As List(Of BomTreeNode)
End Class

''' <summary>
''' 処理結果
''' </summary>
Public Class ProcessResult
    Public Property Success As Boolean
    Public Property Message As String
    Public Property Data As Object
End Class
