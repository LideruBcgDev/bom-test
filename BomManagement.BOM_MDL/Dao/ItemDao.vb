Imports BomManagement.BOM_PRM
Imports BomManagement.BOM_MDL.Common
Imports System.Data

Namespace Dao
    Public Class ItemDao
        ''' <summary>
        ''' 品目情報を取得する
        ''' </summary>
        Public Function GetItem(itemCode As String) As ItemInfo
            Dim sql As String = "SELECT ItemCode, ItemName, ItemType, Unit, StandardCost, " &
                                "Description, IsActive, CreatedDate, UpdatedDate " &
                                "FROM M_Item WHERE ItemCode = @ItemCode"

            Dim params As New Dictionary(Of String, Object) From {
                {"@ItemCode", itemCode}
            }

            Dim dt = DBUtility.ExecuteReader(sql, params)

            If dt.Rows.Count = 0 Then
                Return Nothing
            End If

            Dim row = dt.Rows(0)
            Return New ItemInfo() With {
                .ItemCode = row("ItemCode").ToString(),
                .ItemName = row("ItemName").ToString(),
                .ItemType = row("ItemType").ToString(),
                .Unit = row("Unit").ToString(),
                .StandardCost = CDec(row("StandardCost")),
                .Description = If(IsDBNull(row("Description")), "", row("Description").ToString()),
                .IsActive = CBool(row("IsActive")),
                .CreatedDate = CDate(row("CreatedDate")),
                .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
            }
        End Function

        ''' <summary>
        ''' 品目一覧を検索する
        ''' </summary>
        Public Function SearchItems(searchParam As ItemSearchParam) As List(Of ItemInfo)
            Dim sql As String = "SELECT ItemCode, ItemName, ItemType, Unit, StandardCost, " &
                                "Description, IsActive, CreatedDate, UpdatedDate " &
                                "FROM M_Item WHERE 1=1"

            Dim params As New Dictionary(Of String, Object)

            If Not String.IsNullOrEmpty(searchParam.ItemCode) Then
                sql &= " AND ItemCode LIKE @ItemCode"
                params.Add("@ItemCode", "%" & searchParam.ItemCode & "%")
            End If

            If Not String.IsNullOrEmpty(searchParam.ItemName) Then
                sql &= " AND ItemName LIKE @ItemName"
                params.Add("@ItemName", "%" & searchParam.ItemName & "%")
            End If

            If Not String.IsNullOrEmpty(searchParam.ItemType) Then
                sql &= " AND ItemType = @ItemType"
                params.Add("@ItemType", searchParam.ItemType)
            End If

            If searchParam.OnlyActive Then
                sql &= " AND IsActive = 1"
            End If

            sql &= " ORDER BY ItemCode"

            Dim dt = DBUtility.ExecuteReader(sql, params)
            Dim items As New List(Of ItemInfo)

            For Each row As DataRow In dt.Rows
                items.Add(New ItemInfo() With {
                    .ItemCode = row("ItemCode").ToString(),
                    .ItemName = row("ItemName").ToString(),
                    .ItemType = row("ItemType").ToString(),
                    .Unit = row("Unit").ToString(),
                    .StandardCost = CDec(row("StandardCost")),
                    .Description = If(IsDBNull(row("Description")), "", row("Description").ToString()),
                    .IsActive = CBool(row("IsActive")),
                    .CreatedDate = CDate(row("CreatedDate")),
                    .UpdatedDate = If(IsDBNull(row("UpdatedDate")), Nothing, CDate(row("UpdatedDate")))
                })
            Next

            Return items
        End Function

        ''' <summary>
        ''' 品目情報を登録する
        ''' </summary>
        Public Function InsertItem(item As ItemInfo) As Boolean
            Dim sql As String = "INSERT INTO M_Item (ItemCode, ItemName, ItemType, Unit, StandardCost, " &
                                "Description, IsActive, CreatedDate) " &
                                "VALUES (@ItemCode, @ItemName, @ItemType, @Unit, @StandardCost, " &
                                "@Description, @IsActive, GETDATE())"

            Dim params As New Dictionary(Of String, Object) From {
                {"@ItemCode", item.ItemCode},
                {"@ItemName", item.ItemName},
                {"@ItemType", item.ItemType},
                {"@Unit", item.Unit},
                {"@StandardCost", item.StandardCost},
                {"@Description", item.Description},
                {"@IsActive", item.IsActive}
            }

            Return DBUtility.ExecuteNonQuery(sql, params) > 0
        End Function

        ''' <summary>
        ''' 品目情報を更新する
        ''' </summary>
        Public Function UpdateItem(item As ItemInfo) As Boolean
            Dim sql As String = "UPDATE M_Item SET " &
                                "ItemName = @ItemName, " &
                                "ItemType = @ItemType, " &
                                "Unit = @Unit, " &
                                "StandardCost = @StandardCost, " &
                                "Description = @Description, " &
                                "IsActive = @IsActive, " &
                                "UpdatedDate = GETDATE() " &
                                "WHERE ItemCode = @ItemCode"

            Dim params As New Dictionary(Of String, Object) From {
                {"@ItemCode", item.ItemCode},
                {"@ItemName", item.ItemName},
                {"@ItemType", item.ItemType},
                {"@Unit", item.Unit},
                {"@StandardCost", item.StandardCost},
                {"@Description", item.Description},
                {"@IsActive", item.IsActive}
            }

            Return DBUtility.ExecuteNonQuery(sql, params) > 0
        End Function

        ''' <summary>
        ''' 品目タイプ一覧を取得する
        ''' </summary>
        Public Function GetItemTypes() As List(Of String)
            Dim sql As String = "SELECT DISTINCT ItemType FROM M_Item ORDER BY ItemType"
            Dim dt = DBUtility.ExecuteReader(sql, Nothing)

            Dim types As New List(Of String)
            For Each row As DataRow In dt.Rows
                types.Add(row("ItemType").ToString())
            Next

            Return types
        End Function
    End Class
End Namespace
